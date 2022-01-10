using Autofac;
using AutoMapper;
using Core.Net.Configuration;
using Core.Net.Data;
using Core.Net.Data.Impl;
using Core.Net.Service;
using Core.Net.Service.Impl;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.Net.AutoFac
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            string connectionString = AppSettingsConstVars.DbSqlConnection;
            string dbTypeString = AppSettingsConstVars.DbDbType;
        
            var dbType = dbTypeString == DbType.MySql.ToString() ? DbType.MySql : DbType.SqlServer;
            var connectionConfig = new ConnectionConfig()
            {
                ConnectionString = connectionString, //必填
                DbType = dbType, //必填
                IsAutoCloseConnection = false,
                InitKeyType = InitKeyType.Attribute,
            };

            var basePath = AppContext.BaseDirectory;
            var servicesDllFile = Path.Combine(basePath, "Core.Net.Service.dll");
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            //builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces().InstancePerDependency()
            //    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.Register(c => new SqlSugarClient(connectionConfig)).AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblysServices).Where(t => t.Name.EndsWith("Services")).AsImplementedInterfaces().InstancePerDependency()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            //注册AutoMapper
            List<Profile> autoMapperProfiles = (Assembly.GetEntryAssembly()!.GetTypes()).Where(p => p.BaseType == typeof(Profile)).Select(p => (Profile)Activator.CreateInstance(p)).ToList();
            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                foreach (Profile item in autoMapperProfiles)
                {
                    cfg.AddProfile(item);
                }
            }));

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().InstancePerLifetimeScope();
        }
    }
}
