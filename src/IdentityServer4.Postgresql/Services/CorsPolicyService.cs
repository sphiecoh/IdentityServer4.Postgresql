using IdentityServer4.Services;
using Marten;
using Microsoft.Extensions.Logging;
using IdentityServer4.Postgresql.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Postgresql.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly ILogger<CorsPolicyService> _logger;
        private readonly IDocumentSession _documentSession;
        public CorsPolicyService(ILogger<CorsPolicyService> logger, IDocumentSession documentSession)
        {
            _logger = logger;
            _documentSession = documentSession;
        }
        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            var origins = await _documentSession.Query<Client>().Select(x => x.AllowedCorsOrigins).ToListAsync();
            _logger.LogInformation(1, "Found {length} origin(s)",origin.Length);
            return origin.Contains(origin);

        }
    }
}
