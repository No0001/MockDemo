using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Web.Mvc;
using System.IO;
using Autofac.Integration.Mvc;

namespace Ptc.SETOP.Test
{

    public class IoCSupportedTest<TModule> where TModule : Autofac.Core.IModule, new()
    {
        private IContainer container;
        //private HttpSimulator _httpSimulator;

        public IoCSupportedTest()
        {
            //建立Builder
            var builder = new ContainerBuilder();


            //選擇載入的dll
            Assembly[] assColl = new System.Reflection.Assembly[]
            {
                Assembly.Load("Domain"),
                Assembly.Load("Repository"),
                Assembly.Load("MockDemo"),
            };

             RegiestConcreteType(builder, assColl);


            //註冊assembly
            builder.RegisterAssemblyTypes(assColl).AsImplementedInterfaces().SingleInstance();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // builder.RegisterModule(new MyMndule());
            // builder.RegisterModule(new MyMndule());

            container = builder.Build();

        }

        public TEntity Resolve<TEntity>()
        {
            return container.Resolve<TEntity>();
        }

        protected void ShutdownIoC()
        {
            container.Dispose();
        }

        public void RemoveByType(Action<ContainerBuilder> act, params Type[] types)
        {
            var builder = new ContainerBuilder();

            var components = container.ComponentRegistry.Registrations
                    .Where(c => !types.Contains(c.Activator.LimitType));

            foreach (var c in components)
            {
                builder.RegisterComponent(c);
            }

            foreach (var source in container.ComponentRegistry.Sources)
            {
                builder.RegisterSource(source);
            };

            act?.Invoke(builder);

            container = builder.Build();
        }

        public static void RegiestConcreteType(ContainerBuilder builder, Assembly[] assColl)
        {
            foreach (var ass in assColl)
            {
                foreach (var type in ass?.DefinedTypes)
                {

                    builder.RegisterTypes(type);
                }
            }

        }
    }

    public class BusinessLogicModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //register other modules/dependencies here
        }
    }
}
