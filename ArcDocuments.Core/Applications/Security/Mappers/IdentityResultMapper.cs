using Microsoft.AspNetCore.Identity;
using Nest;

namespace ArcDocuments.Core.Applications.Security.Mappers
{
    public static class IdentityResultMapper
    {
        public static IdentityResult ToIdentityResult(this IResponse r)
        {
            return r.IsValid ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Code = r.ServerError.Status.ToString(), Description = r.ServerError.Error.Reason });
        }
    }
}
