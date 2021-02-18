using Autofac;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockDemo.Controllers;
using Moq;
using Ptc.SETOP.Test;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Hosting;
using System.Web.Routing;
using static System.Web.Http.Hosting.HttpPropertyKeys;
namespace MockDemo.Tests.TestAutoFac
{
    [TestClass]
    public class DITest : IoCSupportedTest<BusinessLogicModule>
    {
        [TestMethod]
        public void Repo()
        {
            var repo = Resolve<IService>();

            repo.OutputMessage("test");
        }

        [TestMethod]
        public void FakeSome()
        {
            void act(ContainerBuilder builder)
            {
                var mock = new Mock<IUserRepository>();

                mock.Setup(x => x.OutputMessage(It.IsAny<string>())).Callback(() => Debug.WriteLine("fack"));

                mock.Setup(x => x.Get(It.IsAny<string>())).Callback<string>(x => {
                    Debug.WriteLine(x);
                }).Returns("gg").Callback<string>(x => {
                    Debug.WriteLine(x);
                });

                builder.RegisterInstance(mock.Object).As<IUserRepository>();
            }

       
            RemoveByType(act);
            var repo = Resolve<IService>();
            repo.OutputMessage("test");
        }

        /// <summary>
        /// Fake Controller Users
        /// </summary>
        [TestMethod]
        public void Controller()
        {
            var repo = Resolve<HomeController>();
            var httpContextMock = new Mock<HttpContextBase>();

            httpContextMock.SetupGet(x => x.User.Identity).Returns(new APPUser { Name = "很棒喔"});

            var routeData = new RouteData();

            var requestContext = new RequestContext(httpContextMock.Object, routeData);

            repo.ControllerContext = new System.Web.Mvc.ControllerContext(requestContext, repo);

            repo.Index();

        }

        /// <summary>
        /// Fake ApiController User
        /// </summary>
        [TestMethod]
        public async Task APIController()
        {

            try
            {
                var repo = Resolve<TestAPIController>();

                repo.Request = new HttpRequestMessage();

                repo.Configuration = new System.Web.Http.HttpConfiguration();

                repo.User = new GenericPrincipal(new APPUser { Name = "玩的不錯" }, null);

                repo.Get(5);
            }
            catch (Exception ex)
            {

                var notImplExceptionFilterAttribute = new NotImplExceptionFilterAttribute();
                var context = CreateExecutedContext(ex);

                await notImplExceptionFilterAttribute.OnExceptionAsync(context, CancellationToken.None);

                var b =  await context.Response.Content.ReadAsStringAsync();

            }

        }

        /// <summary>
        /// Fake ApiController User
        /// </summary>
        [TestMethod]
        public async Task FiterError()
        {

            try
            {
                var repo = Resolve<TestAPIController>();

                repo.Request = new HttpRequestMessage();

                repo.Configuration = new System.Web.Http.HttpConfiguration();

                repo.User = new GenericPrincipal(new APPUser { Name = "玩的不錯" }, null);

                repo.Post("sad");
            }
            catch (Exception ex)
            {

                var notImplExceptionFilterAttribute = new NotImplExceptionFilterAttribute();
                var context = CreateExecutedContext(ex);

                await notImplExceptionFilterAttribute.OnExceptionAsync(context, CancellationToken.None);

                var b = await context.Response.Content.ReadAsStringAsync();

            }

        }

        private HttpActionExecutedContext CreateExecutedContext(Exception exception)
        {
            return new HttpActionExecutedContext
            {
                ActionContext = new HttpActionContext
                {
                    ControllerContext = new HttpControllerContext
                    {
                        Request = GetHttpRequestMessage()
                    }
                },
                Exception = exception,
                Response = new HttpResponseMessage()
            };
        }

        private HttpRequestMessage GetHttpRequestMessage()
        {
            var request = new HttpRequestMessage();
            request.Properties[HttpConfigurationKey] = new System.Web.Http.HttpConfiguration();

            return request;
        }
    }


}
