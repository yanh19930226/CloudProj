using Core.Net.Entity.Model.Shops;
using Core.Net.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Shops
{
    public interface ICoreCmsSettingServices:IBaseServices<CoreCmsSetting>
    {
        /// <summary>
        ///     获取数据库整合后配置信息
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, DictionaryKeyValues>> GetConfigDictionaries();
        /// <summary>
        ///     获取附件存储的配置信息
        /// </summary>
        /// <returns></returns>
        Task<FilesStorageOptions> GetFilesStorageOptions();
    }
}
