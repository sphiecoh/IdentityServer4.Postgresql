using IdentityServer4.Services;
using Marten;
using Microsoft.Extensions.Logging;
using IdentityServer4.Postgresql.Entities;
using System.Linq;
using System.Threading.Tasks;
using System;

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
            var origins = await _documentSession.Query<Client>().SelectMany(x => x.AllowedCorsOrigins).Select(y => y.Origin).ToListAsync().ConfigureAwait(false);
            var distinctOrigins = origins.Where(x => x != null).Distinct();
            var isAllowed = distinctOrigins.Any(x => x.ToLower() == origin.ToLower());

            _logger.LogDebug("Origin {origin} is allowed: {originAllowed}", origin, isAllowed);

            return isAllowed;

        }
    }
}
