using System;
using System.Collections.Generic;
using ArcDocuments.Core.Applications.Security.Dtos;
using Nest;

namespace ArcDocuments.Core.Applications.Security.Contracts
{
    public interface ICompanyApp
    {
        Guid CreateCompany(Company company, User user);
        void DelateCompany(Guid id);
        Company GetComnpany(Guid id);
        IReadOnlyCollection<IHit<Company>> SearchCompanies(string name);
        void UpdateCompany(Company company);
    }
}