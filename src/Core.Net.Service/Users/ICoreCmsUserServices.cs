using Core.Net.Data;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Users;
using Core.Net.Entity.ViewModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Users
{
    public interface ICoreCmsUserServices : IBaseServices<CoreCmsUser>
    {
        ///// <summary>
        /////     更新积分
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //Task<AdminUiCallBack> UpdatePiont(FMUpdateUserPoint entity);

        ///// <summary>
        /////     更新余额
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="money"></param>
        ///// <returns></returns>
        //Task<AdminUiCallBack> UpdateBalance(int id, decimal money);
    }
}
