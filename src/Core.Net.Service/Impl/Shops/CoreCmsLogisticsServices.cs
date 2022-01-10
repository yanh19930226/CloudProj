using Core.Net.Configuration;
using Core.Net.Data;
using Core.Net.Entity.Model.Shops;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Shops;
using Core.Net.Util.Helper;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Impl.Shops
{
    public class CoreCmsLogisticsServices : BaseServices<CoreCmsLogistics>, ICoreCmsLogisticsServices
    {
        private readonly IBaseRepository<CoreCmsLogistics> _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoreCmsSettingServices _settingServices;
        public CoreCmsLogisticsServices(IBaseRepository<CoreCmsLogistics> dal, IUnitOfWork unitOfWork, ICoreCmsSettingServices settingServices)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _settingServices = settingServices;
        }

        /// <summary>
        /// 通过接口更新所有快递公司信息
        /// </summary>
        public async Task<AdminUiCallBack> DoUpdateCompany()
        {
            var jm = new AdminUiCallBack();

            //获取所有字典配置
            var allConfigs = await _settingServices.GetConfigDictionaries();
            var showApiAppid = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShowApiAppid);
            var showApiSecret = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShowApiSecret);
            var shopMobile = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopMobile);

            var showApiTimesTamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            var maxSize = 1500;
            var signStr = "maxSize" + maxSize + "showapi_appid" + showApiAppid + "showapi_timestamp" + showApiTimesTamp + showApiSecret;
            var md5Sign = CommonHelper.Md5For32(signStr).ToLower();

            var url = "https://route.showapi.com/64-20?expName=&maxSize=1500&page=&showapi_appid=" + showApiAppid +
                      "&showapi_timestamp=" + showApiTimesTamp + "&showapi_sign=" + md5Sign;
            var person = await url.GetJsonAsync<ShowApiGetExpressCompanyListResult>();

            if (person.showapi_res_code == 0)
            {
                if (person.showapi_res_body != null && person.showapi_res_body.ret_code == 0 && person.showapi_res_body.expressList != null && person.showapi_res_body.expressList.Count > 0)
                {
                    var list = new List<CoreCmsLogistics>();


                    var systemLogistics = SystemSettingDictionary.GetSystemLogistics();
                    systemLogistics.ForEach(p =>
                    {
                        var logistics = new CoreCmsLogistics();
                        logistics.logiCode = p.sKey;
                        logistics.logiName = p.sDescription;
                        logistics.imgUrl = "";
                        logistics.phone = shopMobile;
                        logistics.url = "";
                        logistics.sort = -1;
                        logistics.isDelete = false;

                        list.Add(logistics);
                    });


                    var count = 0;
                    person.showapi_res_body.expressList.ForEach(p =>
                    {
                        var logistics = new CoreCmsLogistics();
                        logistics.logiCode = p.com;
                        logistics.logiName = p.exp_name;
                        logistics.imgUrl = p.img_url;
                        logistics.phone = p.phone;
                        logistics.url = p.url;
                        logistics.sort = count * 5;
                        logistics.isDelete = false;

                        list.Add(logistics);
                        count++;
                    });
                    await _dal.DeleteAsync(p => p.id > 0);
                    var bl = await _dal.InsertAsync(list) > 0;
                    jm.code = bl ? 0 : 1;
                    jm.msg = bl ? "数据刷新成功" : "数据刷新失败";
                }
                else
                {
                    jm.msg = "接口获取数据失败";
                }
            }
            else
            {
                jm.msg = person.showapi_res_error;
            }

            return jm;
        }
    }
}
