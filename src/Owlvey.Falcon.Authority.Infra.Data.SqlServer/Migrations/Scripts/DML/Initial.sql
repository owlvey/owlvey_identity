/*################## Clients ################## */
Declare @CreationDate Datetime = GetUtcDate();

/*BEGIN: APICORE */
INSERT INTO dbo.ApiResources ([Description], [DisplayName], [Enabled], [Name], [NonEditable], Created)
VALUES (NULL, 'api_falcon', 1, 'api_falcon', 0, @CreationDate)

Declare @ApiResourceId BigInt = SCOPE_IDENTITY();

INSERT INTO dbo.ApiScopes (ApiResourceId, [Description], DisplayName, Emphasize, [Name], [Required], ShowInDiscoveryDocument)
VALUES (@ApiResourceId, NULL, 'api_falcon', 0, 'api_falcon', 0, 1)

Declare @ApiScopeId BigInt = SCOPE_IDENTITY();

Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'role')

Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'name')

Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'givenname')

Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'fullname')

Insert Into [dbo].[ApiScopeClaims] (ApiScopeId, [Type]) Values (@ApiScopeId, 'email')

/*BEGIN: APIAUTH */
INSERT INTO dbo.ApiResources ([Description], [DisplayName], [Enabled], [Name], [NonEditable], Created)
VALUES (NULL, 'api_auth', 1, 'api_auth', 0, @CreationDate)

SET @ApiResourceId = SCOPE_IDENTITY();

INSERT INTO dbo.ApiScopes (ApiResourceId, [Description], DisplayName, Emphasize, [Name], [Required], ShowInDiscoveryDocument)
VALUES (@ApiResourceId, NULL, 'api_auth', 0, 'api_auth', 0, 1)

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

INSERT INTO dbo.AspNetRoles (Id, ConcurrencyStamp, Name, NormalizedName)
VALUES ('CA5723D4000444C78B8D060A4BE61F73', '1BCE2684-6D40-40E6-8677-F359E1D129AC', 'admin', 'ADMIN')

INSERT INTO dbo.AspNetRoles (Id, ConcurrencyStamp, Name, NormalizedName)
VALUES ('AFABBA88D0524867900597CF6301F7E0', 'BDD8F187-1B5B-41AD-9D2D-D94DB9013D52', 'basicuser', 'BASICUSER')

/*################## Users ################## */
