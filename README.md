# Blazor320WasmHostedPwaOidc
A simple Blazor WebAssembly use AddOidcAuthentication() to call secured WebApis

## Third-party External resources
### 0.Oidc Server (eg. IdentityServer4)
https://localhost:6001

### 1.Client (ie. this repo)
https://localhost:5001
```
ClientId = "OurAuth.Demo.Oidc.Local",
ClientName = "OurAuth Demo Oidc (Local/Development)",
ClientUri = "https://localhost:5001",

AllowedGrantTypes = GrantTypes.Implicit,
AllowAccessTokensViaBrowser = true,
RequireClientSecret = false,

RedirectUris = { "https://localhost:5001/authentication/login-callback" },
PostLogoutRedirectUris = { "https://localhost:5001/authentication/logout-callback" },
AllowedCorsOrigins = { "https://localhost:5001" },

AllowedScopes = {
    IdentityServerConstants.StandardScopes.OpenId,
    IdentityServerConstants.StandardScopes.Profile,
    IdentityServerConstants.StandardScopes.Email,
    IdentityServerConstants.StandardScopes.Phone,
    "ourauth.mgrapi",
    "ourauth.demoapi"
}
```

### 2.1 DemoApi 
https://localhost:6005/api/Identity
https://localhost:6005/api/WeatherForecast
```
Name = "ourauth.demoapi";
DisplayName = "OurAuth DemoApi";

Scopes = new[]
{
    new Scope()
    {
        Name = "ourauth.demoapi",
        DisplayName = "Common access to OurAuth DemoAPI"
    }
};

UserClaims = new[]
{
    JwtClaimTypes.Subject,
    JwtClaimTypes.Name,
    JwtClaimTypes.PreferredUserName,
    JwtClaimTypes.NickName,
    JwtClaimTypes.Profile,
    JwtClaimTypes.IdentityProvider,
    JwtClaimTypes.Role
};
```

### 2.2 MgrApi
https://localhost:6003/api/ApiResources

```
Name = "ourauth.mgrapi";
DisplayName = "OurAuth ManagementApi";

Scopes = new[]
{
    new Scope()
    {
        Name = "ourauth.mgrapi",
        DisplayName = "Full access to OurAuth ManagementAPI"
    }
};

UserClaims = new[]
{
    JwtClaimTypes.Subject,
    JwtClaimTypes.Role
};
```

