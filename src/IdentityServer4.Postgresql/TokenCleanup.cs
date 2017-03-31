using IdentityServer4.Postgresql.Entities;
using IdentityServer4.Postgresql.Options;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Postgresql
{
    internal class TokenCleanup
    {
        private readonly ILogger<TokenCleanup> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval;
        private CancellationTokenSource _source;
        public TokenCleanup(IServiceProvider serviceProvider, ILogger<TokenCleanup> logger, TokenCleanupOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.Interval < 1) throw new ArgumentException("interval must be more than 1 second");

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _interval = TimeSpan.FromSeconds(options.Interval);
        }

        public void Start()
        {
            if (_source != null) throw new InvalidOperationException("Already started. Call Stop first.");

            _logger.LogDebug("Starting token cleanup");

            _source = new CancellationTokenSource();
            Task.Factory.StartNew(() => Start(_source.Token));
        }
        public void Stop()
        {
            if (_source == null) throw new InvalidOperationException("Not started. Call Start first.");

            _logger.LogDebug("Stopping token cleanup");

            _source.Cancel();
            _source = null;
        }


        private async Task Start(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogDebug("CancellationRequested");
                    break;
                }

                try
                {
                    await Task.Delay(_interval, cancellationToken).ConfigureAwait(false);
                }
                catch
                {
                    _logger.LogDebug("Task.Delay exception. exiting.");
                    break;
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogDebug("CancellationRequested");
                    break;
                }

                ClearTokens();
            }
        }
        private void ClearTokens()
        {
            try
            {
                _logger.LogTrace("Querying for tokens to clear");

                using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    using (var session = serviceScope.ServiceProvider.GetService<IDocumentSession>())
                    {
                        session.DeleteWhere<PersistedGrant>(x => x.Expiration < DateTimeOffset.UtcNow);
                        session.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception cleaning tokens {exception}", ex.Message);
            }
        }
    }
}
