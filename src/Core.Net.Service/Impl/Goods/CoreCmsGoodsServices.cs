using Core.Net.Data;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Common;
using Core.Net.Entity.Model.Goods;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Common;
using Core.Net.Service.Goods;
using Core.Net.Util.Helper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Impl.Goods
{
    public class CoreCmsGoodsServices : BaseServices<CoreCmsGoods>, ICoreCmsGoodsServices
    {
        private readonly IBaseRepository<CoreCmsGoods> _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarClient _sqlSugarClient;
        private readonly ICoreCmsLabelServices _labelServices;

        public CoreCmsGoodsServices(IBaseRepository<CoreCmsGoods> dal, IUnitOfWork unitOfWork, SqlSugarClient sqlSugarClient, ICoreCmsLabelServices labelServices)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _sqlSugarClient = sqlSugarClient;
            _labelServices = labelServices;
        }

        public async Task<AdminUiCallBack> DoBatchMarketableDown(int[] ids)
        {
            var jm = new AdminUiCallBack();

            var bl = await _dal.UpdateAsync(p => new CoreCmsGoods() { isMarketable = false }, p => ids.Contains(p.id));
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "下架成功" : "下架失败";

            return jm;
        }

        public async Task<AdminUiCallBack> DoBatchMarketableUp(int[] ids)
        {
            var jm = new AdminUiCallBack();

            var bl = await _dal.UpdateAsync(p => new CoreCmsGoods() { isMarketable = true }, p => ids.Contains(p.id));
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "上架成功" : "上架失败";

            return jm;
        }

        public async Task<AdminUiCallBack> DoDeleteLabel(FmSetLabel entity)
        {
            var jm = new AdminUiCallBack();

            var names = entity.labels.Select(p => p.text).ToList();
            //获取已经存在数据库数据
            var labels = await _labelServices.QueryListByClauseAsync(p => names.Contains(p.name));
            var labelIds = labels.Select(p => p.id).ToList();

            var goods = await base.QueryListByClauseAsync(p => entity.ids.Contains(p.id));
            goods.ForEach(p =>
            {
                if (!string.IsNullOrEmpty(p.labelIds))
                {
                    var ids = CommonHelper.StringToIntArray(p.labelIds);
                    var newIds = ids.Except(labelIds).ToList();
                    if (newIds.Any())
                    {
                        p.labelIds = String.Join(",", newIds);
                    }
                    else
                    {
                        p.labelIds = "";
                    }
                }
            });

            var bl = await base.UpdateAsync(goods);

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "设置成功" : "设置失败";


            return jm;
        }

        public async Task<AdminUiCallBack> DoSetLabel(FmSetLabel entity)
        {
            var jm = new AdminUiCallBack();


            var names = entity.labels.Select(p => p.text).ToList();
            //获取已经存在数据库数据
            var olds = await _labelServices.QueryListByClauseAsync(p => names.Contains(p.name));
            if (olds.Any())
            {
                var oldNames = olds.Select(p => p.name).ToList();
                //获取未插入数据库数据
                var newNames = entity.labels.Where(p => !oldNames.Contains(p.text)).ToList();
                if (newNames.Any())
                {
                    var labels = new List<CoreCmsLabel>();
                    newNames.ForEach(p =>
                    {
                        labels.Add(new CoreCmsLabel()
                        {
                            name = p.text,
                            style = p.style
                        });
                    });
                    await _labelServices.InsertAsync(labels);
                }
            }
            else
            {
                var labels = new List<CoreCmsLabel>();
                entity.labels.ForEach(p =>
                {
                    labels.Add(new CoreCmsLabel()
                    {
                        name = p.text,
                        style = p.style
                    });
                });
                await _labelServices.InsertAsync(labels);
            }

            var items = await _labelServices.QueryListByClauseAsync(p => names.Contains(p.name));
            var idsInts = items.Select(p => p.id).ToArray();
            var ids = String.Join(",", idsInts);

            var bl = await base.BaseDal.UpdateAsync(p => new CoreCmsGoods() { labelIds = ids }, p => entity.ids.Contains(p.id));

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "设置成功" : "设置失败";

            return jm;
        }
    }
}
