using System;

namespace ArcDocuments.Core.Applications.Security.Dtos
{
    public class User
    {
        public string Id { get; set; }
        public Guid IdCompany { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

    }
}
