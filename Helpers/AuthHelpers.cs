using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace Helpers {
  public class AuthHelper(IConfiguration configuration) : IAuthHelper {
    public string GenerateJWTToken(SystemUser user) {
      // We could add more params on the claims, that information will be added to the JWT token
      var claims = new List<Claim> {
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new Claim(ClaimTypes.Name, user.Name),
          new Claim(ClaimTypes.Role, user.Role),
      };

      var jwtSecret = configuration["ApplicationSettings:JWT_Secret"];
      if (string.IsNullOrEmpty(jwtSecret)) {
        throw new ArgumentNullException("JWT_Secret", "JWT_Secret is not configured in the application settings.");
      }

      var jwtToken = new JwtSecurityToken(
          claims: claims,
          notBefore: DateTime.UtcNow,
          expires: DateTime.UtcNow.AddDays(30),
          signingCredentials: new SigningCredentials(
              new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret)
                  ),
              SecurityAlgorithms.HmacSha256Signature)
          );
      return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public ClaimsPrincipal DecodeJWTToken(string token) {
      var jwtSecret = configuration["ApplicationSettings:JWT_Secret"];
      if (string.IsNullOrEmpty(jwtSecret)) {
        throw new ArgumentNullException("JWT_Secret", "JWT_Secret is not configured in the application settings.");
      }

      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(jwtSecret);

      var validationParameters = new TokenValidationParameters {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
      };

      try {
        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        return principal;
      } catch (Exception ex) {
        throw new SecurityTokenException("Invalid token", ex);
      }
    }

    public static string GetTokenFromHeader(HttpContext httpContext) {
      if (httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader)) {
        var token = authorizationHeader.ToString().Split(" ").Last();
        return token;
      }
      return null;
    }
  }
}