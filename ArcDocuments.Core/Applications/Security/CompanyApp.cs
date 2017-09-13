using ArcDocuments.Core.Applications.Common;
using ArcDocuments.Core.Applications.Security.Contracts;
using ArcDocuments.Core.Applications.Security.Dtos;
using ArcDocuments.Core.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArcDocuments.Core.Applications.Security
{
    public class CompanyApp :BaseApp, ICompanyApp
    {

        public CompanyApp():base(Consts.SecurityIndexName)
        {
        }

        public Guid CreateCompany(Company company, User user)
        {
            var companies = SearchCompanies(company.Name);
            var companyeAlreadyExists = companies.Where(c => c.Score == 100).Any();

            if (companyeAlreadyExists)
            {
                throw new Exception($"Company with name {company.Name} already exists, please use a different name.");
            }

            company.Id = Guid.NewGuid();

            user.Id = Guid.NewGuid().ToString();
            user.IdCompany = company.Id;

            var response = Storage.Bulk(b => b
                .Index<Company>(i => i.Document(company))
                .Index<User>(i => i.Document(user))
            );

            return company.Id;
        }

        public void UpdateCompany(Company company)
        {
            Storage.Update<Company, object>(
                new DocumentPath<Company>(company.Id),
                u => u.Doc(new { company.Name }).RetryOnConflict(3)
                );
        }

        public IReadOnlyCollection<IHit<Company>> SearchCompanies(string name)
        {
            var companies = Storage.Search<Company>(c => c.From(0).Size(10).Query(q => q.Term(t => t.Name, name)));
            return companies.Hits;
        }
        

        public void DelateCompany(Guid id)
        {
            var resultDeleteUsers = // _client.DeleteByQuery<User>(f => f.Query(q => q.Term(t => t.IdCompany, id)));

            Storage.DeleteByQuery<User>(f=>f.AllIndices().QueryOnQueryString($"idCompany:{id}").Refresh());

            var resultDeteleCompany = Storage.Delete<Company>(id);
        }

        public Company GetComnpany(Guid id)
        {
            var company = Storage.Get<Company>(id);
            return company.Source;
        }
    }
}
