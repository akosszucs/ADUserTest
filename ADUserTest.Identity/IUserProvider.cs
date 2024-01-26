using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ADUserTest.Identity;
using System.Security.Principal;

namespace ADUserTest.Identity
{
    public interface IUserProvider
    {
        AdUser CurrentUser { get; set; }
        bool Initialized { get; set; }
        Task Create(HttpContext context, IConfiguration config);
        Task<AdUser> GetAdUser(IIdentity identity);
        Task<AdUser> GetAdUser(string samAccountName);
        Task<AdUser> GetAdUser(Guid guid);
        Task<List<AdUser>> GetDomainUsers();
        Task<List<AdUser>> FindDomainUser(string search);
    }
}