using Core.Net.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Net.Util.Helper
{
    //public static class GoodsHelper
    //{

    //    #region 获取商品分类下来Dtree============================================================

    //    /// <summary>
    //    /// 获取导航下拉上级树
    //    /// </summary>
    //    /// <returns></returns>
    //    [Description("获取导航下拉上级树")]
    //    public static DTree GetTree(List<CoreCmsGoodsCategory> categories, bool isHaveTop = true)
    //    {

    //        var model = new DTree();
    //        model.status = new dtreeStatus() { code = 200, message = "操作成功" };

    //        var list = GetMenus(categories, 0);

    //        if (isHaveTop)
    //        {
    //            list.Insert(0, new dtreeChild()
    //            {
    //                id = "0",
    //                last = true,
    //                parentId = "0",
    //                title = "无父级",
    //                children = new List<dtreeChild>()
    //            });
    //        }
    //        model.data = list;
    //        return model;
    //    }


    //    /// <summary>
    //    /// 迭代方法
    //    /// </summary>
    //    /// <param name="oldNavs"></param>
    //    /// <param name="parentId"></param>
    //    /// <returns></returns>
    //    private static List<dtreeChild> GetMenus(List<CoreCmsGoodsCategory> oldNavs, int parentId)
    //    {
    //        List<dtreeChild> childTree = new List<dtreeChild>();
    //        var model = oldNavs.Where(p => p.parentId == parentId).ToList();
    //        foreach (var item in model)
    //        {
    //            var parentTree = new dtreeChild();
    //            parentTree.id = item.id.ToString();
    //            parentTree.title = item.name;
    //            parentTree.parentId = item.parentId.ToString();
    //            parentTree.last = !oldNavs.Exists(p => p.parentId == item.id);

    //            childTree.Add(parentTree);
    //            parentTree.children = GetMenus(oldNavs, item.id);
    //        }
    //        return childTree;
    //    }

    //    #endregion

    //    #region 后端判断提交的商品属性值是否符合规则（判断内容，只允许中文，字母，数字，和-，/）

    //    /// <summary>
    //    /// 判断内容，只允许中文，字母，数字，和-，/
    //    /// </summary>
    //    /// <param name="inputValue">输入字符串</param>
    //    /// <remarks>判断内容，只允许中文，字母，数字，和-，/</remarks>
    //    /// <returns></returns>
    //    public static bool FilterChar(string inputValue)
    //    {
    //        return Regex.IsMatch(inputValue, "[`~!@#$^&*()=|\"{}':;',\\[\\]<>?~！@#￥……&*&;|{}。*-+]+");
    //        //if (Regex.IsMatch(inputValue, "[A-Za-z0-9\u4e00-\u9fa5-]+"))
    //        //{
    //        //    return Regex.Match(inputValue, "[A-Za-z0-9\u4e00-\u9fa5-]+").Value;
    //        //}
    //        //return "";
    //        //return Regex.IsMatch(inputValue, "[~!@#$%^&*()_+|<>,.?:;'\\[\\]{}\"]+");

    //        //return Regex.IsMatch(inputValue, "[A-Za-z0-9\u4e00-\u9fa5-/]+");
    //    }

    //    #endregion
    //}
}
