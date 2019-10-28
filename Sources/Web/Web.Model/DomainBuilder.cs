using System;
using System.Configuration;
using System.Reflection;
using System.Text;
using Xtensive.Orm;
using Xtensive.Orm.Configuration;

//[assembly: System.Security.SecurityRules(System.Security.SecurityRuleSet.Level1)]
namespace TXSoftware.DataObjectsNetEntityModel.Web.Model
{
    public static class DomainBuilder
    {
        private static Domain domain;
        private static DomainConfiguration domainConfiguration;
        private static string activeDBDomain;

        public static Domain Domain
        {
            get { return domain; }
        }

        public static string ActiveDbDomain
        {
            get
            {
                if (string.IsNullOrEmpty(activeDBDomain))
                {
                    activeDBDomain = ConfigurationManager.AppSettings["db.active.domain"];
                }
                return activeDBDomain;
            }
        }

        public static DomainConfiguration DomainConfiguration
        {
            get { return domainConfiguration; }
        }

        public static DomainConfiguration GetDomainConfiguration()
        {
            if (domainConfiguration == null)
            {
                domainConfiguration = DomainConfiguration.Load(ActiveDbDomain);
            }

            return domainConfiguration;
        }

        public static Domain Build()
        {
            return Build(false);
        }

        public static Domain Build(bool rebuild)
        {
            return Build(GetDomainConfiguration(), rebuild);
        }

        public static Domain Build(DomainConfiguration configuration, bool rebuild)
        {
            if (domain == null || rebuild)
            {
                if (rebuild)
                {
                    configuration = configuration.Clone();
                    configuration.UpgradeMode = DomainUpgradeMode.Recreate;
                }
                try
                {
                    domain = Domain.Build(configuration);
                }
                catch (Exception e)
                {
                    DumpAndThrowAllExcs(e);
                }
            }

            if (domain != null) 
            {
                //domain.SessionOpen += OnSessionOpen;
            }
            
            return domain;
        }

        private static void DumpAndThrowAllExcs(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            Exception inner = e;
            while (inner != null)
            {
                sb.AppendFormat("{0}: {1} st: {2}", inner.GetType().Name, inner.Message, inner.StackTrace);
                if (inner is ReflectionTypeLoadException)
                {
                    var excs = IterateExcs((inner as ReflectionTypeLoadException).LoaderExceptions);
                    sb.AppendLine(excs);
                }
                sb.AppendLine();
                inner = inner.InnerException;
            }

            throw new ApplicationException(sb.ToString());
        }

        private static string IterateExcs(Exception[] exceptions)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var exception in exceptions)
            {
                sb.AppendFormat("{0}: {1} st: {2}", exception.GetType().Name, exception.Message, exception.StackTrace);
            }

            return sb.ToString();
        }

        public static Session OpenSession()
        {
            return OpenSession(domain);
        }

        public static Session OpenSession(Domain _domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("Domain is not builded and thus cannot open new session!");
            }

            Session session = Session.Open(domain);
            //session.Events.EntityCreated += OnEntityCreated;

            return session;
        }

       /* private static void OnSessionOpen(object sender, SessionEventArgs e)
        {
            e.Session.Events.EntityCreated += OnEntityCreated;
        }


        private static void OnEntityCreated(object sender, EntityEventArgs e)
        {
            if (e.Entity is ICreatedBySupport)
            {
                ICreatedBySupport createdBySupport = (ICreatedBySupport) e.Entity;
                User loggedUser = ContextBuilder.LoggedUserBuilder != null ? ContextBuilder.LoggedUserBuilder() : null;
//                if (loggedUser == null)
//                {
//                    throw new InvalidOperationException(string.Format("Error creating entity '{0}' because context does not have any logged user!", e.Entity));
//                }
                createdBySupport.CreatedBy = loggedUser;
            }
        }*/
    }
}