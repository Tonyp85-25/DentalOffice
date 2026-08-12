using System.Diagnostics;
using Microsoft.Extensions.Primitives;

namespace CleanTeeth.IntegrationTests.Services;

using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string AuthenticationScheme = "Test";
    public const string TestUserIdHeader = "X-Test-User-Id";
    public const string TestUserRoleHeader = "X-Test-User-Role";

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check for test headers
        if (!Request.Headers.TryGetValue(TestUserIdHeader, out var userId))
        {
            return Task.FromResult(AuthenticateResult.Fail("No test user specified"));
        }


        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
        };


        if (!Request.Headers.TryGetValue(TestUserRoleHeader, out var role))
        {
            return Task.FromResult(AuthenticateResult.Fail("No role specified"));
        }
        claims.Add(new Claim(ClaimTypes.Role, role));
        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
