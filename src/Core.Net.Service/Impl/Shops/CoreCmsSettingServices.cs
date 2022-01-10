using Core.Net.Configuration;
using Core.Net.Data;
using Core.Net.Entity.Model.Shops;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Shops;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Impl.Shops
{
    public class CoreCmsSettingServices : BaseServices<CoreCmsSetting>, ICoreCmsSettingServices
    {
        private readonly IBaseRepository<CoreCmsSetting> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsSettingServices(IBaseRepository<CoreCmsSetting> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 获取数据库整合后配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, DictionaryKeyValues>> GetConfigDictionaries()
        {
            var configs = SystemSettingDictionary.GetConfig();
            //var settings = await GetDatas();
            //foreach (KeyValuePair<string, DictionaryKeyValues> kvp in configs)
            //{
            //    var model = settings.Find(p => p.sKey == kvp.Key);
            //    if (model != null)
            //    {
            //        kvp.Value.sValue = model.sValue;
            //    }
            //}
            return configs;
        }

        public Task<FilesStorageOptions> GetFilesStorageOptions()
        {
            throw new NotImplementedException();
        }
    }
}
