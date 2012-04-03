using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BaseInfrastructure.NHibernate.TestCore;
using Moq;

namespace BaseInfrastructure.NHibernate.Mvc.TestCore
{
    /// <summary>
    /// Provides a base for testing ASP.NET MVC Controllers.
    /// </summary>
    public abstract class ControllerTestBase : TestBase
    {
        protected Mock<HttpContextBase> HttpContextMock;
        protected Mock<HttpRequestBase> HttpRequestMock;
        protected Mock<HttpResponseBase> HttpResponseMock;
        protected Mock<HttpSessionStateBase> HttpSessionStateMock;
        protected Mock<HttpServerUtilityBase> HttpServerUtilityMock;

        protected override void Given()
        {
            base.Given();

            HttpContextMock = new Mock<HttpContextBase>();
            HttpRequestMock = new Mock<HttpRequestBase>();
            HttpResponseMock = new Mock<HttpResponseBase>();
            HttpSessionStateMock = new Mock<HttpSessionStateBase>();
            HttpServerUtilityMock = new Mock<HttpServerUtilityBase>();

            HttpContextMock.Setup(ctx => ctx.Request).Returns(HttpRequestMock.Object);
            HttpContextMock.Setup(ctx => ctx.Response).Returns(HttpResponseMock.Object);
            HttpContextMock.Setup(ctx => ctx.Session).Returns(HttpSessionStateMock.Object);
            HttpContextMock.Setup(ctx => ctx.Server).Returns(HttpServerUtilityMock.Object);
        }

        protected RequestContext CreateRequestContext()
        {
            return new RequestContext(HttpContextMock.Object, new RouteData());
        }

        protected ControllerContext GetControllerContext(Controller controller)
        {
            return new ControllerContext(CreateRequestContext(), controller);
        }

        protected void SetFakeControllerContext(Controller controller)
        {
            controller.ControllerContext = GetControllerContext(controller);
        }
    }
}
