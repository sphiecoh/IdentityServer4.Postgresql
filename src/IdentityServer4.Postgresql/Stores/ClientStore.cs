using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Marten;
using IdentityServer4.Postgresql.Mappers;

namespace IdentityServer4.Postgresql.Stores
{
	public class ClientStore : IClientStore
	{
		private readonly IDocumentSession _documentSession;
		public ClientStore(IDocumentSession documentSession)
		{
			_documentSession = documentSession;
		}
		public async Task<Client> FindClientByIdAsync(string clientId)
		{
			var client = await _documentSession.Query<Entities.Client>().FirstOrDefaultAsync(x => x.ClientId == clientId);
			return client?.ToModel();
		}
	}
}