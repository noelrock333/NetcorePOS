using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Models;
public interface IAuthHelper
{
    string GenerateJWTToken(SystemUser user);
    ClaimsPrincipal DecodeJWTToken(string token);
}