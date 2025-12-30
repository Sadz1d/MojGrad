using Market.Application.Abstractions;
using Market.Domain.Entities.Identity;
using Market.Shared.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Market.Infrastructure.Common;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _jwt;
    private readonly TimeProvider _time;

    public JwtTokenService(IOptions<JwtOptions> options, TimeProvider time)
    {
        _jwt = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _time = time ?? throw new ArgumentNullException(nameof(time));
    }

    public JwtTokenPair IssueTokens(MarketUserEntity user)
    {
        var now = _time.GetUtcNow();
        var nowUtc = now.UtcDateTime;

        var accessExpires = now.AddMinutes(_jwt.AccessTokenMinutes).UtcDateTime;
        var refreshExpires = now.AddDays(_jwt.RefreshTokenDays).UtcDateTime;

        // 🔐 CLAIMS – prilagođeni MOJGRAD useru
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),

            // 👇 ROLE FLAGS (kao kod profesora)
            new("is_admin",    user.IsAdmin.ToString().ToLowerInvariant()),
            new("is_manager",  user.IsManager.ToString().ToLowerInvariant()),
            new("is_employee", user.IsEmployee.ToString().ToLowerInvariant()),

            // 🔄 verzija tokena (za revoke)
            new("ver", user.TokenVersion.ToString()),

            // ⏱ standardni JWT claimovi
            new(JwtRegisteredClaimNames.Iat,
                ToUnixTimeSeconds(now).ToString(),
                ClaimValueTypes.Integer64),

            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new(JwtRegisteredClaimNames.Aud, _jwt.Audience)
        };

        // 🔑 SIGNING
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 🎟 ACCESS TOKEN
        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            notBefore: nowUtc,
            expires: accessExpires,
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        // 🔄 REFRESH TOKEN
        var refreshRaw = GenerateRefreshTokenRaw(64);
        var refreshHash = HashRefreshToken(refreshRaw);

        return new JwtTokenPair
        {
            AccessToken = accessToken,
            AccessTokenExpiresAtUtc = accessExpires,

            RefreshTokenRaw = refreshRaw,
            RefreshTokenHash = refreshHash,
            RefreshTokenExpiresAtUtc = refreshExpires
        };
    }

    public string HashRefreshToken(string rawToken)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(rawToken));
        return Base64UrlEncoder.Encode(bytes);
    }

    private static string GenerateRefreshTokenRaw(int bytes)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(bytes);
        return Base64UrlEncoder.Encode(randomBytes);
    }

    private static long ToUnixTimeSeconds(DateTimeOffset dto)
        => dto.ToUnixTimeSeconds();
}
