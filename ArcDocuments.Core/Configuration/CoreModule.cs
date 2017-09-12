using ArcDocuments.Core.Applications.Security;
using ArcDocuments.Core.Applications.Security.Contracts;
using Autofac;

namespace ArcDocuments.Core.Configuration
{
    public class CoreModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CompanyApp>().As<ICompanyApp>();
        }
    }
}
