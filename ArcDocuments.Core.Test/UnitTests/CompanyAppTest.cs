using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArcDocuments.Core.Applications.Security;
using ArcDocuments.Core.Applications.Security.Dtos;
using ArcDocuments.Core.Configuration;
using ArcDocuments.Core.Applications.Security.Contracts;
using Autofac;

namespace ArcDocuments.Core.Test.UnitTests
{
    [TestClass]
    public class CompanyAppTest
    {
        [TestMethod]
        public void CompanyCrudTest()
        {

            var container = ContainerSetup.GetContainer();



            ICompanyApp app = container.Resolve<ICompanyApp>();

            const string companyName = "Arc Documents";
            const string companyNameUpdated = "Arc Documents [UPDATED]";
            var idCompany = app.CreateCompany(new Company
            {
                Name = companyName
            }, new User
            {
                Email = "aldoromo88@gmail.com",
                UserName = "Aldo Romo",
                Password = "123!@#"
            });

            var company = app.GetComnpany(idCompany);

            Assert.AreEqual(company.Name, companyName);
            app.UpdateCompany(new Company { Id = idCompany, Name = companyNameUpdated });
            company = app.GetComnpany(idCompany);
            Assert.AreEqual(company.Name, companyNameUpdated);

            app.DelateCompany(idCompany);

            company = app.GetComnpany(idCompany);
            Assert.IsNull(company);
        }


        [TestMethod]
        public void DeleteCompanyTest()
        {
            CompanyApp app = new CompanyApp();

            app.DelateCompany(Guid.Parse("e242415f-1d65-42b3-a086-17e318b91681"));


            //app.Client.Delete<User>(Guid.Parse("087386c7-85ae-49c3-a43f-37fd04bbfca7"));

        }
    }
}
