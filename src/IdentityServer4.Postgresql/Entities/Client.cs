using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer4.Postgresql.Entities
{
	public class Client : EntityKey
	{
		public Client()
		{
			Id = Guid.NewGuid().ToString();
		}
		public bool Enabled { get; set; } = true;
		public string ClientId { get; set; }
		public string ProtocolType { get; set; } = ProtocolTypes.OpenIdConnect;
		public List<ClientSecret> ClientSecrets { get; set; }
		public bool RequireClientSecret { get; set; } = true;
		public string ClientName { get; set; }
		public string ClientUri { get; set; }
		public string LogoUri { get; set; }
		public bool RequireConsent { get; set; } = true;
		public bool AllowRememberConsent { get; set; } = true;
		public List<ClientGrantType> AllowedGrantTypes { get; set; }
		public bool RequirePkce { get; set; }
		public bool AllowPlainTextPkce { get; set; }
		public bool AllowAccessTokensViaBrowser { get; set; }
		public List<ClientRedirectUri> RedirectUris { get; set; }
		public List<ClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; }
		public string LogoutUri { get; set; }
		public bool LogoutSessionRequired { get; set; } = true;
		public bool AllowOfflineAccess { get; set; }
		public List<ClientScope> AllowedScopes { get; set; }
		public int IdentityTokenLifetime { get; set; } = 300;
		public int AccessTokenLifetime { get; set; } = 3600;
		public int AuthorizationCodeLifetime { get; set; } = 300;
		public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;
		public int SlidingRefreshTokenLifetime { get; set; } = 1296000;
		public int RefreshTokenUsage { get; set; } = (int)TokenUsage.OneTimeOnly;
		public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
		public int RefreshTokenExpiration { get; set; } = (int)TokenExpiration.Absolute;
		public int AccessTokenType { get; set; } = 0; // AccessTokenType.Jwt;
		public bool EnableLocalLogin { get; set; } = true;
		public List<ClientIdPRestriction> IdentityProviderRestrictions { get; set; }
		public bool IncludeJwtId { get; set; }
		public List<ClientClaim> Claims { get; set; }
		public bool AlwaysSendClientClaims { get; set; }
		public bool PrefixClientClaims { get; set; } = true;
		public List<ClientCorsOrigin> AllowedCorsOrigins { get; set; }
		public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
		/// <summary>
		/// Specifies logout URI at client for HTTP front-channel based logout.
		/// </summary>
		public string FrontChannelLogoutUri { get; set; }

		/// <summary>
		/// Specifies is the user's session id should be sent to the FrontChannelLogoutUri. Defaults to <c>true</c>.
		/// </summary>
		public bool FrontChannelLogoutSessionRequired { get; set; } = true;

		/// <summary>
		/// Specifies logout URI at client for HTTP back-channel based logout.
		/// </summary>
		public string BackChannelLogoutUri { get; set; }

		/// <summary>
		/// Specifies is the user's session id should be sent to the BackChannelLogoutUri. Defaults to <c>true</c>.
		/// </summary>
		public bool BackChannelLogoutSessionRequired { get; set; } = true;
		/// <summary>
		/// Lifetime of a user consent in seconds. Defaults to null (no expiration)
		/// </summary>
		public int? ConsentLifetime { get; set; } = null;
		/// <summary>
		/// Gets or sets a value to prefix it on client claim types. Defaults to <c>client_</c>.
		/// </summary>
		/// <value>
		/// Any non empty string if claims should be prefixed with the value; otherwise, <c>null</c>.
		/// </value>
		public string ClientClaimsPrefix { get; set; } = "client_";
		/// <summary>
		/// Gets or sets a salt value used in pair-wise subjectId generation for users of this client.
		/// </summary>
		public string PairWiseSubjectSalt { get; set; }
		/// <summary>
		/// Gets or sets the custom properties for the client.
		/// </summary>
		/// <value>
		/// The properties.
		/// </value>
		public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
	}
}
