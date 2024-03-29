﻿using ADUserTest.Identity.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;

namespace ADUserTest.Identity
{
    public class AdUserProvider : IUserProvider
    {
        public AdUser CurrentUser { get; set; }
        public bool Initialized { get; set; }

        public async Task Create(HttpContext context, IConfiguration config)
        {
            CurrentUser = await GetAdUser(context.User.Identity);
            Initialized = true;
        }

        public Task<AdUser> GetAdUser(IIdentity identity)
        {
            return Task.Run(() =>
            {
                try
                {
                    PrincipalContext context = new PrincipalContext(ContextType.Domain);
                    UserPrincipal principal = new UserPrincipal(context);

                    if (context != null)
                    {
                        principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, identity.Name);
                    }

                    return AdUser.CastToAdUser(principal);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving AD User", ex);
                }
            });
        }

        public Task<AdUser> GetAdUser(string samAccountName)
        {
            return Task.Run(() =>
            {
                try
                {
                    PrincipalContext context = new PrincipalContext(ContextType.Domain);
                    UserPrincipal principal = new UserPrincipal(context);

                    if (context != null)
                    {
                        principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, samAccountName);
                    }

                    return AdUser.CastToAdUser(principal);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving AD User", ex);
                }
            });
        }

        public Task<AdUser> GetAdUser(Guid guid)
        {
            return Task.Run(() =>
            {
                try
                {
                    PrincipalContext context = new PrincipalContext(ContextType.Domain);
                    UserPrincipal principal = new UserPrincipal(context);

                    if (context != null)
                    {
                        principal = UserPrincipal.FindByIdentity(context, IdentityType.Guid, guid.ToString());
                    }

                    return AdUser.CastToAdUser(principal);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving AD User", ex);
                }
            });
        }

        public Task<List<AdUser>> GetDomainUsers()
        {
            return Task.Run(() =>
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain);
                UserPrincipal principal = new UserPrincipal(context);
                principal.UserPrincipalName = "*@*";
                principal.Enabled = true;
                PrincipalSearcher searcher = new PrincipalSearcher(principal);

                var users = searcher
                    .FindAll()
                    .AsQueryable()
                    .Cast<UserPrincipal>()
                    .FilterUsers()
                    .SelectAdUsers()
                    .OrderBy(x => x.Surname)
                    .ToList();

                return users;
            });
        }

        public Task<List<AdUser>> FindDomainUser(string search)
        {
            return Task.Run(() =>
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain);
                UserPrincipal principal = new UserPrincipal(context);
                principal.SamAccountName = $"*{search}*";
                principal.Enabled = true;
                PrincipalSearcher searcher = new PrincipalSearcher(principal);

                var users = searcher
                    .FindAll()
                    .AsQueryable()
                    .Cast<UserPrincipal>()
                    .FilterUsers()
                    .SelectAdUsers()
                    .OrderBy(x => x.Surname)
                    .ToList();

                return users;
            });
        }
    }
}
