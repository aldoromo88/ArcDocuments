using ArcDocuments.Core.Applications.Security.Contracts;
using ArcDocuments.Core.Applications.Security.Dtos;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArcDocuments.Core.Applications.Security
{
    public class CompanyApp : ICompanyApp
    {
        private ElasticClient _client;

        public ElasticClient Client => _client;

        public CompanyApp()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex("arc-documents");
            settings.DisableDirectStreaming(true);
            _client = new ElasticClient(settings);
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

            user.Id = Guid.NewGuid();
            user.IdCompany = company.Id;

            var response = _client.Bulk(b => b
                .Index<Company>(i => i.Document(company))
                .Index<User>(i => i.Document(user))
            );

            return company.Id;
        }

        public void UpdateCompany(Company company)
        {
            _client.Update<Company, object>(
                new DocumentPath<Company>(company.Id),
                u => u.Doc(new { company.Name }).RetryOnConflict(3)
                );
        }

        public IReadOnlyCollection<IHit<Company>> SearchCompanies(string name)
        {
            var companies = _client.Search<Company>(c => c.From(0).Size(10).Query(q => q.Term(t => t.Name, name)));
            return companies.Hits;
        }
        

        public void DelateCompany(Guid id)
        {
            var resultDeleteUsers = // _client.DeleteByQuery<User>(f => f.Query(q => q.Term(t => t.IdCompany, id)));

            _client.DeleteByQuery<User>(f=>f.AllIndices().QueryOnQueryString($"idCompany:{id}").Refresh());

            var resultDeteleCompany = _client.Delete<Company>(id);
        }

        public Company GetComnpany(Guid id)
        {
            var company = _client.Get<Company>(id);
            return company.Source;
        }
    }
}
