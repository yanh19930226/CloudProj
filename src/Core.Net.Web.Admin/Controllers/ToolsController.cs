using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Systems;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Shops;
using Core.Net.Service.Systems;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin.Controllers
{
    /// <summary>
    ///  后端常用方法
    /// </summary>
    [Description("后端常用方法")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToolsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISysMenuServices _sysMenuServices;
        private readonly ISysOrganizationServices _sysOrganizationServices;
        private readonly ISysRoleServices _sysRoleServices;
        private readonly ISysUserRoleServices _sysUserRoleServices;
        private readonly ISysRoleMenuServices _sysRoleMenuServices;
        private readonly ISysUserServices _sysUserServices;
        private readonly ICoreCmsSettingServices _coreCmsSettingServices;
        private readonly ICoreCmsAreaServices _areaServices;

        /// <summary>
        ///  构造函数
        /// </summary>
        public ToolsController(
           IWebHostEnvironment webHostEnvironment
            , ISysUserServices sysUserServices
             , ICoreCmsAreaServices areaServices
            , ISysRoleServices sysRoleServices
            , ISysMenuServices sysMenuServices
            , ISysUserRoleServices sysUserRoleServices
            , ISysOrganizationServices sysOrganizationServices,
           ICoreCmsSettingServices coreCmsSettingServices,
            ISysRoleMenuServices sysRoleMenuServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _sysUserServices = sysUserServices;
            _sysRoleServices = sysRoleServices;
            _sysMenuServices = sysMenuServices;
            _sysUserRoleServices = sysUserRoleServices;
            _sysOrganizationServices = sysOrganizationServices;
            _sysRoleMenuServices = sysRoleMenuServices;
            _coreCmsSettingServices = coreCmsSettingServices;
            _areaServices = areaServices;
        }

        #region 根据用户权限获取对应左侧菜单列表====================================================

        /// <summary>
        ///  根据用户权限获取对应左侧菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetNavs()
        {
            var jm = new AdminUiCallBack();

            //先获取用户关联角色
            var roles = await _sysUserRoleServices.QueryListByClauseAsync(p => p.userId == 1);

            //var roles = await _sysUserRoleServices.QueryListByClauseAsync(p => p.userId == _user.ID);
            if (roles.Any())
            {
                var roleIds = roles.Select(p => p.roleId).ToList();
                var sysRoleMenu = await _sysRoleMenuServices.QueryListByClauseAsync(p => roleIds.Contains(p.roleId));
                var menuIds = sysRoleMenu.Select(p => p.menuId).ToList();

                var where = PredicateBuilder.True<SysMenu>();
                //获取0代表菜单
                where = where.And(p => p.deleted == false && p.hide == false && p.menuType == 0);
                where = where.And(p => menuIds.Contains(p.id));

                var navs = await _sysMenuServices.QueryListByClauseAsync(where, p => p.sortNumber, OrderByType.Asc);
                var menus = GetMenus(navs, 0);

                jm.data = menus;
            }

            jm.msg = "数据获取正常";
            jm.code = 0;

            return new JsonResult(jm);
        }

        /// <summary>
        ///  迭代方法
        /// </summary>
        /// <param name="oldNavs"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private static List<AdminUiMenu> GetMenus(List<SysMenu> oldNavs, int parentId)
        {
            var childTree = new List<AdminUiMenu>();
            if (parentId == 0)
            {
                var topMenu = new AdminUiMenu { title = "主页", icon = "layui-icon-home", name = "HomePanel" };
                var list = new List<AdminUiMenu>
                {
                    new AdminUiMenu
                        {title = "控制台", jump = "/", name = "controllerPanel", list = new List<AdminUiMenu>()}
                };
                topMenu.list = list;
                childTree.Add(topMenu);
            }

            var model = oldNavs.Where(p => p.parentId == parentId).ToList();
            foreach (var item in model)
            {
                var menu = new AdminUiMenu
                {
                    name = item.identificationCode,
                    title = item.menuName,
                    icon = item.menuIcon,
                    jump = !string.IsNullOrEmpty(item.path) ? item.path : null
                };
                childTree.Add(menu);
                menu.list = GetMenus(oldNavs, item.id);
            }

            return childTree;
        }

        #endregion

        #region 裁剪Base64上传====================================================

        /// <summary>
        ///  裁剪Base64上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadFilesFByBase64([FromBody] FMBase64Post entity)
        {
            var jm = new AdminUiCallBack();

            //var _filesStorageOptions = await _coreCmsSettingServices.GetFilesStorageOptions();



            if (string.IsNullOrEmpty(entity.base64))
            {
                jm.msg = "请上传合法内容";
                return new JsonResult(jm);
            }
            //将base64头部信息替换
            entity.base64 = entity.base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");
            byte[] bytes = Convert.FromBase64String(entity.base64);
            MemoryStream memStream = new MemoryStream(bytes);

            Image mImage = Image.FromStream(memStream);
            Bitmap bp = new Bitmap(mImage);


            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".jpg";
            var today = DateTime.Now.ToString("yyyyMMdd");


            //if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.LocalStorage.ToString())
            //{
                var saveUrl = "/Upload/" + today + "/";
                var dirPath = _webHostEnvironment.WebRootPath + saveUrl;
                string bucketBindDomain = AppSettingsConstVars.AppConfigAppUrl;

                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                var filePath = dirPath + newFileName;
                var fileUrl = saveUrl + newFileName;

                bp.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);//注意保存路径

                jm.code = 0;
                jm.msg = "上传成功!";
                jm.data = new
                {
                    fileUrl,
                    src = bucketBindDomain + fileUrl
                };
                jm.otherData = GlobalEnumVars.FilesStorageOptionsType.LocalStorage.ToString();

            //}
            //else if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.AliYunOSS.ToString())
            //{
                ////上传到阿里云
                #region MyRegion
                //// 设置当前流的位置为流的开始
                //memStream.Seek(0, SeekOrigin.Begin);

                //await using (var fileStream = memStream) //转成Stream流
                //{
                //    var md5 = OssUtils.ComputeContentMd5(fileStream, memStream.Length);

                //    var filePath = "Upload/" + today + "/" + newFileName; //云文件保存路径
                //                                                          //初始化阿里云配置--外网Endpoint、访问ID、访问password
                //    var aliyun = new OssClient(_filesStorageOptions.Endpoint, _filesStorageOptions.AccessKeyId, _filesStorageOptions.AccessKeySecret);
                //    //将文件md5值赋值给meat头信息，服务器验证文件MD5
                //    var objectMeta = new ObjectMetadata
                //    {
                //        ContentMd5 = md5
                //    };
                //    //文件上传--空间名、文件保存路径、文件流、meta头信息(文件md5) //返回meta头信息(文件md5)
                //    aliyun.PutObject(_filesStorageOptions.BucketName, filePath, fileStream, objectMeta);
                //    //返回给UEditor的插入编辑器的图片的src
                //    jm.code = 0;
                //    jm.msg = "上传成功";
                //    jm.data = new
                //    {
                //        fileUrl = _filesStorageOptions.BucketBindUrl + filePath,
                //        src = _filesStorageOptions.BucketBindUrl + filePath
                //    };
                //}
                //jm.otherData = GlobalEnumVars.FilesStorageOptionsType.AliYunOSS.ToString(); 
                #endregion

            //}
            //else if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QCloudOSS.ToString())
            //{
                #region MyRegion
                //上传到腾讯云OSS
                //初始化 CosXmlConfig
                //string appid = _filesStorageOptions.AccountId;//设置腾讯云账户的账户标识 APPID
                //string region = _filesStorageOptions.CosRegion; //设置一个默认的存储桶地域
                //CosXmlConfig config = new CosXmlConfig.Builder()
                //    .SetAppid(appid)
                //    .IsHttps(true)  //设置默认 HTTPS 请求
                //    .SetRegion(region)  //设置一个默认的存储桶地域
                //    .SetDebugLog(true)  //显示日志
                //    .Build();  //创建 CosXmlConfig 对象

                //long durationSecond = 600;  //每次请求签名有效时长，单位为秒
                //QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(
                //    _filesStorageOptions.AccessKeyId, _filesStorageOptions.AccessKeySecret, durationSecond);


                //var cosXml = new CosXmlServer(config, qCloudCredentialProvider);

                //var filePath = "Upload/" + today + "/" + newFileName; //云文件保存路径
                //COSXML.Model.Object.PutObjectRequest putObjectRequest = new COSXML.Model.Object.PutObjectRequest(_filesStorageOptions.BucketName, filePath, bytes);

                //cosXml.PutObject(putObjectRequest);

                //jm.code = 0;
                //jm.msg = "上传成功";
                //jm.data = new
                //{
                //    fileUrl = _filesStorageOptions.BucketBindUrl + filePath,
                //    src = _filesStorageOptions.BucketBindUrl + filePath
                //}; 
                #endregion
            //}
            return new JsonResult(jm);
        }
        #endregion

        #region 后台Select三级下拉联动配合

        /// <summary>
        ///  获取大类列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAreaCheckedList([FromBody] FMIntId entity)
        {
            var res = new List<AreasDtoForAdminEdit>();

            if (entity.id != 0)
            {
                var model3 = new AreasDtoForAdminEdit();
                model3.info = await _areaServices.QueryByIdAsync(entity.id);
                if (model3.info != null && model3.info.parentId != 0)
                {
                    model3.list = await _areaServices.QueryListByClauseAsync(p => p.parentId == model3.info.parentId);

                    var model2 = new AreasDtoForAdminEdit();
                    model2.info = await _areaServices.QueryByIdAsync(model3.info.parentId);
                    if (model2.info != null && model2.info.parentId != 0)
                    {
                        model2.list =
                            await _areaServices.QueryListByClauseAsync(p => p.parentId == model2.info.parentId);

                        var model = new AreasDtoForAdminEdit();
                        model.info = await _areaServices.QueryByIdAsync(model2.info.parentId);
                        if (model.info != null)
                            model.list =
                                await _areaServices.QueryListByClauseAsync(p => p.parentId == model.info.parentId);
                        res.Add(model);
                    }

                    res.Add(model2);
                }

                res.Add(model3);
            }
            else
            {
                var model4 = new AreasDtoForAdminEdit();
                model4.list = await _areaServices.QueryListByClauseAsync(p => p.parentId == 0);
                model4.info = model4.list.FirstOrDefault();
                res.Add(model4);
            }

            return Json(res);
        }

        /// <summary>
        ///  取地区的下级列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAreaChildren([FromBody] FMIntId entity)
        {
            var list = await _areaServices.QueryListByClauseAsync(p => p.parentId == entity.id);
            return Json(list);
        }

        #endregion


    }

}
