using Core.Net.Configuration;
using Core.Net.Data;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Goods;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Goods;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Impl.Goods
{
    public class CoreCmsGoodsTypeServices : BaseServices<CoreCmsGoodsType>, ICoreCmsGoodsTypeServices
    {
        private readonly IBaseRepository<CoreCmsGoodsType> _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarClient _sqlSugarClient;

        public CoreCmsGoodsTypeServices(IBaseRepository<CoreCmsGoodsType> dal, IUnitOfWork unitOfWork, SqlSugarClient sqlSugarClient)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _sqlSugarClient = sqlSugarClient;
        }

        public async Task<AdminUiCallBack> DeleteByIdAsync(int id)
        {
            var jm = new AdminUiCallBack();

            var bl = await _sqlSugarClient.Deleteable<CoreCmsGoodsType>(id).ExecuteCommandHasChangeAsync();
            if (bl)
            {
                //删除参数关联
                await _sqlSugarClient.Deleteable<CoreCmsGoodsTypeParams>().Where(p => p.typeId == id).ExecuteCommandHasChangeAsync();
                //删除属性关联
                await _sqlSugarClient.Deleteable<CoreCmsGoodsTypeSpecRel>().Where(p => p.typeId == id).ExecuteCommandHasChangeAsync();
            }
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> InsertAsync(FmGoodsTypeInsert entity)
        {
            var jm = new AdminUiCallBack();
            var goodsType = new CoreCmsGoodsType();
            goodsType.name = entity.name;
            goodsType.parameters = "";

            var typesId = await _sqlSugarClient.Insertable(goodsType).ExecuteReturnIdentityAsync();
            var bl = typesId > 0;
            if (bl)
            {
                //参数集合处理
                if (entity.parameters != null && entity.parameters.Any())
                {
                    //已经存在数据处理
                    var oldPar = entity.parameters.Where(p => p.paramsId > 0).Select(p => p.paramsId).ToList();
                    if (oldPar.Any())
                    {
                        //存在的数据直接进行数据关联
                        var goodsTypeParams = new List<CoreCmsGoodsTypeParams>();
                        oldPar.ForEach(p =>
                        {
                            goodsTypeParams.Add(new CoreCmsGoodsTypeParams()
                            {
                                paramsId = p,
                                typeId = typesId
                            });
                        });
                        bl = _sqlSugarClient.Insertable(goodsTypeParams).ExecuteCommand() > 0;
                    }
                    var newPar = entity.parameters.Where(p => p.paramsId == 0).ToList();
                    if (newPar.Any())
                    {
                        //不存在的参数先插入数据库，并且返回ids数组。
                        ArrayList ids = new ArrayList();
                        newPar.ForEach(p =>
                        {
                            var paramsModel = new CoreCmsGoodsParams()
                            {
                                name = p.paramsName,
                                value = p.paramsValue,
                                type = p.paramsType,
                                createTime = DateTime.Now
                            };
                            var id = _sqlSugarClient.Insertable(paramsModel).ExecuteReturnIdentity();
                            ids.Add(id);
                        });
                        //新参数集合插入成功并且返回数组后进行关联操作。
                        if (ids.Count > 0)
                        {
                            var goodsTypeParams = new List<CoreCmsGoodsTypeParams>();
                            foreach (var id in ids)
                            {
                                goodsTypeParams.Add(new CoreCmsGoodsTypeParams()
                                {
                                    paramsId = (int)id,
                                    typeId = typesId
                                });
                            }
                            bl = _sqlSugarClient.Insertable(goodsTypeParams).ExecuteCommand() > 0;
                        }
                    }

                }
                //属性集合处理
                if (entity.types != null && entity.types.Any())
                {
                    //针对已经存在数据库的属性
                    var oldTypeSpec = entity.types.Where(p => p.typeId > 0).Select(p => p.typeId).ToList();
                    if (oldTypeSpec.Any())
                    {
                        //存在的数据直接进行数据关联
                        var goodsTypeSpecRel = new List<CoreCmsGoodsTypeSpecRel>();
                        oldTypeSpec.ForEach(p =>
                        {
                            goodsTypeSpecRel.Add(new CoreCmsGoodsTypeSpecRel()
                            {
                                specId = p,
                                typeId = typesId
                            });
                        });
                        bl = await _sqlSugarClient.Insertable(goodsTypeSpecRel).ExecuteCommandAsync() > 0;
                    }
                    //针对数据库不存在的数据
                    var newTypeSpec = entity.types.Where(p => p.typeId == 0).ToList();
                    if (newTypeSpec.Any())
                    {
                        //执行新增操作
                        ArrayList ids = new ArrayList();
                        var goodsTypeSpecValues = new List<CoreCmsGoodsTypeSpecValue>();
                        newTypeSpec.ForEach(async p =>
                        {
                            var goodsTypeSpec = new CoreCmsGoodsTypeSpec();
                            goodsTypeSpec.name = p.typeName;
                            goodsTypeSpec.sort = 100;
                            var id = await _sqlSugarClient.Insertable(goodsTypeSpec).ExecuteReturnIdentityAsync();

                            if (!string.IsNullOrEmpty(p.typeValue))
                            {
                                if (p.typeValue.Contains(","))
                                {
                                    var values = p.typeValue.Split(",");
                                    foreach (var value in values)
                                    {
                                        goodsTypeSpecValues.Add(new CoreCmsGoodsTypeSpecValue()
                                        {
                                            specId = id,
                                            value = value,
                                            sort = 100
                                        });
                                    }
                                }
                                else
                                {
                                    goodsTypeSpecValues.Add(new CoreCmsGoodsTypeSpecValue()
                                    {
                                        specId = id,
                                        value = p.typeValue,
                                        sort = 100
                                    });
                                }
                            }
                            if (goodsTypeSpecValues.Any())
                            {
                                bl = await _sqlSugarClient.Insertable(goodsTypeSpecValues).ExecuteCommandAsync() > 0;
                            }
                            ids.Add(id);
                        });
                        //新参数集合插入成功并且返回数组后进行关联操作。
                        if (ids.Count > 0)
                        {
                            var goodsTypeSpecRel = new List<CoreCmsGoodsTypeSpecRel>();
                            foreach (var id in ids)
                            {
                                goodsTypeSpecRel.Add(new CoreCmsGoodsTypeSpecRel()
                                {
                                    specId = (int)id,
                                    typeId = typesId
                                });
                            }
                            bl = await _sqlSugarClient.Insertable(goodsTypeSpecRel).ExecuteCommandAsync() > 0;
                        }
                    }
                }
            }
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            if (bl)
            {
                var types = await _sqlSugarClient.Queryable<CoreCmsGoodsType>().ToListAsync();
                jm.data = new
                {
                    types
                };
            }

            return jm;
        }

        public async Task<AdminUiCallBack> UpdateParametersAsync(FMUpdateArrayIntDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _sqlSugarClient.Queryable<CoreCmsGoodsType>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            var goodsTypeParams = new List<CoreCmsGoodsTypeParams>();
            var count = 0;
            var oldParams = await _sqlSugarClient.Queryable<CoreCmsGoodsTypeParams>().Where(p => p.typeId == entity.id).ToListAsync();
            if (oldParams.Any())
            {
                var ids = oldParams.Select(p => p.paramsId).ToList();
                var exceptOld = ids.Except(entity.data).ToList();
                if (exceptOld.Any())
                {
                    count += await _sqlSugarClient.Deleteable<CoreCmsGoodsTypeParams>().Where(p => exceptOld.Contains(p.paramsId) && p.typeId == entity.id).ExecuteCommandAsync();
                }
                var exceptNew = entity.data.Except(ids).ToList();
                if (exceptNew.Any())
                {
                    exceptNew.ForEach(p =>
                    {
                        goodsTypeParams.Add(new CoreCmsGoodsTypeParams() { paramsId = p, typeId = entity.id });
                    });
                }
            }
            else if (entity.data.Any())
            {
                entity.data.ForEach(p =>
                {
                    goodsTypeParams.Add(new CoreCmsGoodsTypeParams() { paramsId = p, typeId = entity.id });
                });
            }
            if (goodsTypeParams.Any())
            {
                count += await _sqlSugarClient.Insertable(goodsTypeParams).ExecuteCommandAsync();
            }
            //事物处理过程结束
            var bl = count >= 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        public async Task<AdminUiCallBack> UpdateTypesAsync(FMUpdateArrayIntDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _sqlSugarClient.Queryable<CoreCmsGoodsType>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            var goodsTypeSpecRel = new List<CoreCmsGoodsTypeSpecRel>();
            var count = 0;
            var oldParams = await _sqlSugarClient.Queryable<CoreCmsGoodsTypeSpecRel>().Where(p => p.typeId == entity.id).ToListAsync();
            if (oldParams.Any())
            {
                var ids = oldParams.Select(p => p.specId).ToList();
                var exceptOld = ids.Except(entity.data).ToList();
                if (exceptOld.Any())
                {
                    count += await _sqlSugarClient.Deleteable<CoreCmsGoodsTypeSpecRel>().Where(p => exceptOld.Contains(p.specId) && p.typeId == entity.id).ExecuteCommandAsync();
                }
                var exceptNew = entity.data.Except(ids).ToList();
                if (exceptNew.Any())
                {
                    exceptNew.ForEach(p =>
                    {
                        goodsTypeSpecRel.Add(new CoreCmsGoodsTypeSpecRel() { specId = p, typeId = entity.id });
                    });
                }
            }
            else if (entity.data.Any())
            {
                entity.data.ForEach(p =>
                {
                    goodsTypeSpecRel.Add(new CoreCmsGoodsTypeSpecRel() { specId = p, typeId = entity.id });
                });
            }
            if (goodsTypeSpecRel.Any())
            {
                count += await _sqlSugarClient.Insertable(goodsTypeSpecRel).ExecuteCommandAsync();
            }
            //事物处理过程结束
            var bl = count >= 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
    }
}
