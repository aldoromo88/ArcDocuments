using System;

namespace ArcDocuments.Core.Applications.Security.Dtos
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid IdCompany { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
