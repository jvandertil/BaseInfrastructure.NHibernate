using System;
using System.Web.Mvc;
using BaseInfrastructure.NHibernate.Core;
using NHibernate;
using NHibernate.Cfg;

namespace BaseInfrastructure.NHibernate.Mvc
{
    internal class ExampleNHibernateActionFilter : ActionFilterAttribute
    {
        private static readonly System.Lazy<ISessionFactory> SessionFactory = new System.Lazy<ISessionFactory>(BuildSessionFactory, true);

        protected static ISessionFactory BuildSessionFactory()
        {
            return new Configuration()
                .Configure()
                .BuildSessionFactory();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sessionController = filterContext.Controller as AbstractController;

            if (sessionController == null)
                return;

            sessionController.LazySession = new Lazy<ISession>(() =>
            {
                var session = SessionFactory.Value.OpenSession();
                session.BeginTransaction();
                return session;
            }, false);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var sessionController = filterContext.Controller as AbstractController;

            if (sessionController == null)
                return;

            // Database session was not used.
            if (!sessionController.LazySession.IsValueCreated)
                return;

            using (var session = sessionController.LazySession.Value)
            {
                if (session == null)
                    return;

                if (!session.Transaction.IsActive)
                    return;

                if (filterContext.Exception != null)
                    session.Transaction.Rollback();
                else
                    session.Transaction.Commit();
            }
        }
    }
}