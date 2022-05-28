using BL.Counters;
using BL.Helper;
using DB.Model;
using Moq;
using Ninject;
using NUnit.Framework;
using RKC.App_Start;
using RKC.Controllers;
using AppCache;
using System.Web;

namespace RKC_TEST
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var mocks = new MockRepository();
            var Kernel = NinjectWebCommon.CreateKernel();
            var controller = new CounterController(Kernel.Get<ICounter>(),Kernel.Get<Ilogger>(),Kernel.Get<IGeneratorDescriptons>(),Kernel.Get<ICacheApp>());
            var httpContext = FakeHttpContext(mocks, true);
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                RequestContext = new RequestContext(httpContext, new RouteData())
            };

            httpContext.User.Expect(u => u.IsInRole("User")).Return(false);
            mocks.ReplayAll();

            // Act
            var result =
                controller.ActionInvoker.InvokeAction(controller.ControllerContext, "Index");
            var statusCode = httpContext.Response.StatusCode;

            // Assert
            Assert.IsTrue(result, "Invoker Result");
            Assert.AreEqual(401, statusCode, "Status Code");
            mocks.VerifyAll();
        }
       
       
    }
    public static HttpContextBase FakeHttpContext(MockRepository mocks, bool isAuthenticated)
    {
        var context = mocks.StrictMock<HttpContextBase>();
        var request = mocks.StrictMock<HttpRequestBase>();
        var response = mocks.StrictMock<HttpResponseBase>();
        var session = mocks.StrictMock<HttpSessionStateBase>();
        var server = mocks.StrictMock<HttpServerUtilityBase>();
        var cachePolicy = mocks.Stub<HttpCachePolicyBase>();
        var user = mocks.StrictMock<IPrincipal>();
        var identity = mocks.StrictMock<IIdentity>();
        var itemDictionary = new Dictionary<object, object>();

        identity.Expect(id => id.IsAuthenticated).Return(isAuthenticated);
        user.Expect(u => u.Identity).Return(identity).Repeat.Any();

        context.Expect(c => c.User).PropertyBehavior();
        context.User = user;
        context.Expect(ctx => ctx.Items).Return(itemDictionary).Repeat.Any();
        context.Expect(ctx => ctx.Request).Return(request).Repeat.Any();
        context.Expect(ctx => ctx.Response).Return(response).Repeat.Any();
        context.Expect(ctx => ctx.Session).Return(session).Repeat.Any();
        context.Expect(ctx => ctx.Server).Return(server).Repeat.Any();

        response.Expect(r => r.Cache).Return(cachePolicy).Repeat.Any();
        response.Expect(r => r.StatusCode).PropertyBehavior();

        return context;
    }
}