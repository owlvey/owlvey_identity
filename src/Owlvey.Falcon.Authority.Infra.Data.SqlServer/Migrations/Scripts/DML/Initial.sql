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
declare @GuestRole nvarchar(50) = 'AFABBA88D0524867900597CF6301F7E0'
declare @IntegrationRole nvarchar(50) = '76EF07CB-0948-4B0D-B7F3-323A21B415F0'

INSERT INTO dbo.AspNetRoles (Id, ConcurrencyStamp, Name, NormalizedName)
VALUES (@AdminRole, '1BCE2684-6D40-40E6-8677-F359E1D129AC', 'admin', 'ADMIN')

INSERT INTO dbo.AspNetRoles (Id, ConcurrencyStamp, Name, NormalizedName)
VALUES (@GuestRole, 'BDD8F187-1B5B-41AD-9D2D-D94DB9013D52', 'guest', 'GUEST')

INSERT INTO dbo.AspNetRoles (Id, ConcurrencyStamp, Name, NormalizedName)
VALUES (@IntegrationRole, '7E4E2422-0804-4614-9EB6-D52ED1758283', 'integration', 'INTEGRATION')

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
		Values (@PasswordClientId, 'role', 'guest')

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

INSERT INTO dbo.AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FirstName, LastName, Avatar)
VALUES ('5B914168-21E4-4078-AE85-77739D5375F5', 'admin@owlvey.com', 'ADMIN@OWLVEY.COM', 'admin@owlvey.com', 'ADMIN@OWLVEY.COM', 0, 'AQAAAAEAACcQAAAAEBliu7tojAjRkI+LwJT1SUACDrxbtXiJyWHoY3ZWxiRKZNPRBYKmVUQDcU6ZczlgoQ==', 'GEYLVTSQ65FQB3XLQLGKQ4H4K56YC7MD', '6b67ee9b-dfa0-4ac4-a683-91c122dd1c34', NULL, 0, 0, NULL, 1, 0, 'Admin', 'Admin', NULL)

INSERT INTO [AspNetUserClaims] ([ClaimType], [ClaimValue], [UserId])
VALUES ('givenname', 'Admin', '5B914168-21E4-4078-AE85-77739D5375F5');

INSERT INTO [AspNetUserClaims] ([ClaimType], [ClaimValue], [UserId])
VALUES ('fullname', 'Admin', '5B914168-21E4-4078-AE85-77739D5375F5');

INSERT INTO [AspNetUserRoles] (UserId, RoleId)
VALUES ('5B914168-21E4-4078-AE85-77739D5375F5', @AdminRole);

INSERT INTO [AspNetUserRoles] (UserId, RoleId)
VALUES ('5B914168-21E4-4078-AE85-77739D5375F5', @GuestRole);

INSERT INTO [AspNetUserRoles] (UserId, RoleId)
VALUES ('5B914168-21E4-4078-AE85-77739D5375F5', @IntegrationRole);

/***/

INSERT INTO dbo.AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FirstName, LastName, Avatar)
VALUES ('550de393-86d0-423e-9b20-6fcf59b9e0eb', 'guest@owlvey.com', 'GUEST@OWLVEY.COM', 'guest@owlvey.com', 'GUEST@OWLVEY.COM', 0, 'AQAAAAEAACcQAAAAEAij5QIoCBTCOnYKiuZG6S4nes2iLzfsi8hKnONKHoXoxlSozVTAV8s4c6D0yz2BXg==', '4WVZXGLTC4ZGLQ5IVTHSAT34S33KGFAA', 'c326706e-d985-4fbb-b113-0ffcde6feb2b', NULL, 0, 0, NULL, 1, 0, 'Guest', 'Guest', NULL)

INSERT INTO [AspNetUserClaims] ([ClaimType], [ClaimValue], [UserId])
VALUES ('givenname', 'Guest', '550de393-86d0-423e-9b20-6fcf59b9e0eb');

INSERT INTO [AspNetUserClaims] ([ClaimType], [ClaimValue], [UserId])
VALUES ('fullname', 'Guest', '550de393-86d0-423e-9b20-6fcf59b9e0eb');

INSERT INTO [AspNetUserRoles] (UserId, RoleId)
VALUES ('550de393-86d0-423e-9b20-6fcf59b9e0eb', @GuestRole);

/***/

INSERT INTO dbo.AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FirstName, LastName, Avatar)
VALUES ('514107f7-2c47-4ac8-84df-221d8da691b9', 'integration@owlvey.com', 'INTEGRATION@OWLVEY.COM', 'integration@owlvey.com', 'INTEGRATION@OWLVEY.COM', 0, 'AQAAAAEAACcQAAAAEDlYIQaCf0tos8ZeHkmnauwrVwvxMaYnbeQc0Rsq8cH5bDbWn9MDOPIdOfqsLTgCcw==', '2FSRH27RBN3V2UELGDLJQHPQGOUKYJFE', '8a7c9ac7-992a-4cfd-b88e-6eea4b73bcc7', NULL, 0, 0, NULL, 1, 0, 'Integration', 'Integration', NULL)

INSERT INTO [AspNetUserClaims] ([ClaimType], [ClaimValue], [UserId])
VALUES ('givenname', 'Integration', '514107f7-2c47-4ac8-84df-221d8da691b9');

INSERT INTO [AspNetUserClaims] ([ClaimType], [ClaimValue], [UserId])
VALUES ('fullname', 'Integration', '514107f7-2c47-4ac8-84df-221d8da691b9');

INSERT INTO [AspNetUserRoles] (UserId, RoleId)
VALUES ('514107f7-2c47-4ac8-84df-221d8da691b9', @IntegrationRole);