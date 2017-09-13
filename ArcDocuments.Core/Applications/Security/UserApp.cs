using ArcDocuments.Core.Applications.Common;
using ArcDocuments.Core.Applications.Security.Dtos;
using ArcDocuments.Core.Applications.Security.Mappers;
using ArcDocuments.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArcDocuments.Core.Applications.Security
{
    public class UserApp<TUser, TDocumentStrore> : BaseApp, IUserStore<TUser> where TUser : User where TDocumentStrore : class, IElasticClient
    {
        public IdentityErrorDescriber ErrorDescriber { get; }

        public UserApp(IdentityErrorDescriber errorDescriber = null) : base(Consts.SecurityIndexName)
        {
            ErrorDescriber = errorDescriber;
        }

        #region IDisposable

        private bool _disposed;
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
        #endregion

        private void CheckParam<T>(T p, string name, CancellationToken cancellationToken)
        {
            if (p == null)
            {
                throw new ArgumentNullException(name);
            }

            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            CheckParam(user, nameof(user), cancellationToken);
            return (await Storage.IndexAsync(user, null, cancellationToken)).ToIdentityResult();
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            CheckParam(user, nameof(user), cancellationToken);
            return (await Storage.DeleteAsync(new DocumentPath<TUser>(user.Id), null, cancellationToken)).ToIdentityResult();

        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            CheckParam(userId, nameof(userId), cancellationToken);
            return (await Storage.GetAsync(new DocumentPath<TUser>(userId), null, cancellationToken)).Source;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            CheckParam(user, nameof(user), cancellationToken);
            return (await Storage.UpdateAsync(new DocumentPath<TUser>(user.Id), u => u.Doc(user).RetryOnConflict(5), cancellationToken)).ToIdentityResult();
        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            CheckParam(normalizedUserName, nameof(normalizedUserName), cancellationToken);
            var results = await Storage.SearchAsync<TUser>(u => u.Size(1).Query(q => q.Term(t => t.NormalizedUserName, normalizedUserName)));

            return results.Hits.FirstOrDefault()?.Source;
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            CheckParam(user, nameof(user), cancellationToken);
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            CheckParam(user, nameof(user), cancellationToken);
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            CheckParam(user, nameof(user), cancellationToken);
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            CheckParam(user, nameof(user), cancellationToken);
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            CheckParam(user, nameof(user), cancellationToken);
            user.UserName = userName;
            return Task.CompletedTask;
        }


    }
}
