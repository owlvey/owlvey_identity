/*################## Clients ################## */
/*
	delete from ApiResources
	delete from ApiScopes
	delete from [ApiScopeClaims]
	delete from IdentityResources
	delete from Clients
	delete from aspnetroles
	delete from aspnetusers
	
*/

Declare @CreationDate Datetime = GetUtcDate();

/*BEGIN: APICORE */
INSERT INTO dbo.ApiResources ([Description], [DisplayName], [Enabled], [Name], [NonEditable], Created)
VALUES (NULL, 'api', 1, 'api', 0, @CreationDate)

Declare @ApiResourceId BigInt = SCOPE_IDENTITY();

INSERT INTO dbo.ApiScopes (ApiResourceId, [Description], DisplayName, Emphasize, [Name], [Required], ShowInDiscoveryDocument)
VALUES (@ApiResourceId, NULL, 'api', 0, 'api', 0, 1)

Declare @ApiScopeId BigInt = SCOPE_IDENTITY();

Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'role')
Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'name')
Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'givenname')
Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'fullname')
Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'email')

/*BEGIN: APIAUTH */
INSERT INTO dbo.ApiResources ([Description], [DisplayName], [Enabled], [Name], [NonEditable], Created)
VALUES (NULL, 'api.auth', 1, 'api.auth', 0, @CreationDate)

SET @ApiResourceId = SCOPE_IDENTITY();

INSERT INTO dbo.ApiScopes (ApiResourceId, [Description], DisplayName, Emphasize, [Name], [Required], ShowInDiscoveryDocument)
VALUES (@ApiResourceId, NULL, 'api.auth', 0, 'api.auth', 0, 1)

SET @ApiScopeId = SCOPE_IDENTITY();

Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'role')
Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'name')
Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'givenname')
Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'fullname')
Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'email')

-- Identity Resources

INSERT INTO dbo.IdentityResources (Description, DisplayName, Emphasize, Enabled, Name, Required, ShowInDiscoveryDocument, [NonEditable], Created)
VALUES (NULL, 'Your user identifier', 0, 1, 'openid', 1, 1, 0, @CreationDate)

Declare @IdentifierResourceId BigInt = SCOPE_IDENTITY();

INSERT INTO dbo.IdentityResources (Description, DisplayName, Emphasize, Enabled, Name, Required, ShowInDiscoveryDocument, [NonEditable], Created)
VALUES ('Your user profile information (first name, last name, etc.)', 'User profile', 1, 1, 'profile', 0, 1, 0, @CreationDate)

Declare @ProfileResourceId BigInt = SCOPE_IDENTITY();

-- Identity Claims

INSERT INTO dbo.IdentityClaims (IdentityResourceId, [Type])
VALUES (@IdentifierResourceId, 'sub')

INSERT INTO dbo.IdentityClaims (IdentityResourceId, [Type])
VALUES (@ProfileResourceId, 'name')

INSERT INTO dbo.IdentityClaims (IdentityResourceId, [Type])
VALUES (@ProfileResourceId, 'givenname')

INSERT INTO dbo.IdentityClaims (IdentityResourceId, [Type])
VALUES (@ProfileResourceId, 'fullname')

Insert Into IdentityClaims (IdentityResourceId, [Type])
Values (@ProfileResourceId, 'role')

INSERT INTO dbo.IdentityClaims (IdentityResourceId, [Type])
VALUES (@ProfileResourceId, 'email')

INSERT INTO dbo.IdentityClaims (IdentityResourceId, [Type])
VALUES (@ProfileResourceId, 'phone')

INSERT INTO dbo.IdentityClaims (IdentityResourceId, [Type])
VALUES (@ProfileResourceId, 'up_timezone')

INSERT INTO dbo.IdentityClaims (IdentityResourceId, [Type])
VALUES (@ProfileResourceId, 'up_dateformat')

INSERT INTO dbo.IdentityClaims (IdentityResourceId, [Type])
VALUES (@ProfileResourceId, 'up_timeformat')

INSERT INTO dbo.IdentityClaims (IdentityResourceId, [Type])
VALUES (@ProfileResourceId, 'up_theme')

/*################## Roles ################## */

declare @AdminRole nvarchar(50) = 'CA5723D4000444C78B8D060A4BE61F73'
declare @BasicRole nvarchar(50) = 'AFABBA88D0524867900597CF6301F7E0'

INSERT INTO dbo.AspNetRoles (Id, ConcurrencyStamp, Name, NormalizedName)
VALUES (@AdminRole, '1BCE2684-6D40-40E6-8677-F359E1D129AC', 'admin', 'ADMIN')

INSERT INTO dbo.AspNetRoles (Id, ConcurrencyStamp, Name, NormalizedName)
VALUES (@BasicRole, 'BDD8F187-1B5B-41AD-9D2D-D94DB9013D52', 'basicuser', 'BASICUSER')

/*################## Passwords Client ################## */

IF NOT EXISTS (SELECT 1 FROM dbo.Clients WHERE ClientId = 'B0D76E84BF394F1297CABBD7337D42B9')
	BEGIN

		INSERT INTO dbo.Clients (DeviceCodeLifetime, AbsoluteRefreshTokenLifetime, AccessTokenLifetime, AccessTokenType, AllowAccessTokensViaBrowser, AllowOfflineAccess, AllowPlainTextPkce, AllowRememberConsent, AlwaysIncludeUserClaimsInIdToken, AlwaysSendClientClaims, AuthorizationCodeLifetime, ClientId, ClientName, ClientUri, EnableLocalLogin, Enabled, IdentityTokenLifetime, IncludeJwtId, LogoUri, ProtocolType, RefreshTokenExpiration, RefreshTokenUsage, RequireClientSecret, RequireConsent, RequirePkce, SlidingRefreshTokenLifetime, UpdateAccessTokenClaimsOnRefresh, BackChannelLogoutSessionRequired, FrontChannelLogoutSessionRequired, NonEditable, Created)
		VALUES (300, 2592000, 3600, 0, 0, 1, 0, 1, 0, 0, 300, 'B0D76E84BF394F1297CABBD7337D42B9','Passwords Client', NULL, 1, 1, 300, 0, NULL, 'oidc', 1, 1, 1, 0, 0, 1296000, 0, 0, 0, 0, GETUTCDATE())

		DEclare @PasswordClientId BigInt = SCOPE_IDENTITY();

		-- Client Secrets
		-- (DEV) 0da45603-282a-4fa6-a20b-2d4c3f2a2127 = > QP27hk2IIMsbBE9ruK6Qul8Iu4WfwDnqh9LlY0mczr0=
		Declare @PasswordSharedSecret NVarChar(100) = 'QP27hk2IIMsbBE9ruK6Qul8Iu4WfwDnqh9LlY0mczr0='


		INSERT INTO dbo.ClientSecrets (ClientId, Description, Expiration, Type, Value, Created)
		VALUES (@PasswordClientId, NULL, NULL, 'SharedSecret', @PasswordSharedSecret, GETUTCDATE())

		-- Client GrantTypes

		INSERT INTO dbo.ClientGrantTypes (ClientId, GrantType)
		VALUES (@PasswordClientId, 'password')

		-- Client Claims

		INSERT INTO dbo.ClientClaims(ClientId, [Type], [Value])
		Values (@PasswordClientId, 'role', 'admin')

		INSERT INTO dbo.ClientClaims(ClientId, [Type], [Value])
		Values (@PasswordClientId, 'role', 'basicuser')

		-- Client Scopes

		INSERT INTO dbo.ClientScopes (ClientId, Scope)
		VALUES (@PasswordClientId, 'api')

		INSERT INTO dbo.ClientScopes (ClientId, Scope)
		VALUES (@PasswordClientId, 'api.auth')

		INSERT INTO dbo.ClientScopes (ClientId, Scope)
		VALUES (@PasswordClientId, 'openid')

		INSERT INTO dbo.ClientScopes (ClientId, Scope)
		VALUES (@PasswordClientId, 'profile')

	END

/*################## Default Client ################## */
IF NOT EXISTS (SELECT 1 FROM dbo.Clients WHERE ClientId = 'CF4A9ED44148438A99919FF285D8B48D')
	BEGIN

		INSERT INTO dbo.Clients (DeviceCodeLifetime, AbsoluteRefreshTokenLifetime, AccessTokenLifetime, AccessTokenType, AllowAccessTokensViaBrowser, AllowOfflineAccess, AllowPlainTextPkce, AllowRememberConsent, AlwaysIncludeUserClaimsInIdToken, AlwaysSendClientClaims, AuthorizationCodeLifetime, ClientId, ClientName, ClientUri, EnableLocalLogin, Enabled, IdentityTokenLifetime, IncludeJwtId, LogoUri, ProtocolType, RefreshTokenExpiration, RefreshTokenUsage, RequireClientSecret, RequireConsent, RequirePkce, SlidingRefreshTokenLifetime, UpdateAccessTokenClaimsOnRefresh, BackChannelLogoutSessionRequired, FrontChannelLogoutSessionRequired, NonEditable, Created)
		VALUES (300, 2592000, 3600, 0, 0, 1, 0, 1, 0, 0, 300, 'CF4A9ED44148438A99919FF285D8B48D','Default Client', NULL, 1, 1, 300, 0, NULL, 'oidc', 1, 1, 1, 0, 0, 1296000, 0, 0, 0, 0, GETUTCDATE())

		DEclare @ClientId BigInt = SCOPE_IDENTITY();

		-- Client Secrets
		-- (DEV) 0da45603-282a-4fa6-a20b-2d4c3f2a2127 = > QP27hk2IIMsbBE9ruK6Qul8Iu4WfwDnqh9LlY0mczr0=
		-- (PROD) 46a91ed0-7a5c-48c5-9bcc-32589cca541d = > xEJsFpGxlhTVAlZbx6hYhWbHIN56u1HQe21YHyNAwP8=
		Declare @SharedSecret NVarChar(100) = 'QP27hk2IIMsbBE9ruK6Qul8Iu4WfwDnqh9LlY0mczr0='


		INSERT INTO dbo.ClientSecrets (ClientId, Description, Expiration, Type, Value, Created)
		VALUES (@ClientId, NULL, NULL, 'SharedSecret', @SharedSecret, GETUTCDATE())

		-- Client GrantTypes

		INSERT INTO dbo.ClientGrantTypes (ClientId, GrantType)
		VALUES (@ClientId, 'client_credentials')

		-- Client Claims

		INSERT INTO dbo.ClientClaims(ClientId, [Type], [Value])
		Values (@ClientId, 'role', 'admin')

		INSERT INTO dbo.ClientClaims(ClientId, [Type], [Value])
		Values (@ClientId, 'role', 'basicuser')

		-- Client Scopes

		INSERT INTO dbo.ClientScopes (ClientId, Scope)
		VALUES (@ClientId, 'api')

		INSERT INTO dbo.ClientScopes (ClientId, Scope)
		VALUES (@ClientId, 'api.auth')

	END


DECLARE @DefaultUser UNIQUEIDENTIFIER = '5B914168-21E4-4078-AE85-77739D5375F5'
INSERT INTO [AspNetUsers] ([Id], [AccessFailedCount], [Avatar], [ConcurrencyStamp], [Email], [EmailConfirmed], [FirstName], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName])
VALUES (@DefaultUser, 0, NULL, '6b67ee9b-dfa0-4ac4-a683-91c122dd1c34', 'admin@owlvey.com', 
		0, 'Admin', 'Support', 1, NULL, 'ADMIN@OWLVEY.COM', 'ADMIN@OWLVEY.COM', 'AQAAAAEAACcQAAAAEBliu7tojAjRkI+LwJT1SUACDrxbtXiJyWHoY3ZWxiRKZNPRBYKmVUQDcU6ZczlgoQ==', 
		NULL, 0, 'GEYLVTSQ65FQB3XLQLGKQ4H4K56YC7MD', 0, 'admin@owlvey.com');

INSERT INTO [AspNetUserClaims] ([ClaimType], [ClaimValue], [UserId])
VALUES ('givenname', 'Admin', @DefaultUser);

INSERT INTO [AspNetUserClaims] ([ClaimType], [ClaimValue], [UserId])
VALUES ('fullname', 'Admin Support', @DefaultUser);


INSERT INTO [AspNetUserRoles] (UserId, RoleId)
VALUES (@DefaultUser, @AdminRole);

INSERT INTO [AspNetUserRoles] (UserId, RoleId)
VALUES (@DefaultUser, @BasicRole);