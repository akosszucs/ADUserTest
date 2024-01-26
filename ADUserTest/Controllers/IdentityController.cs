using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ADUserTest.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ADUserTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IUserProvider _provider;

        public IdentityController(IUserProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        [HttpGet("[action]")]
        public async Task<List<AdUser>> GetDomainUsers() => await _provider.GetDomainUsers();

        [HttpGet("[action]/{search}")]
        public async Task<List<AdUser>> FindDomainUser([FromRoute] string search) => await _provider.FindDomainUser(search);

        [HttpGet("[action]")]
        public AdUser GetCurrentUser() => _provider.CurrentUser;
    }
}
