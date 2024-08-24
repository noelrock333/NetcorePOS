using Helpers;
using Models;
public static class Authorization {
  // The parameters that RegisterAuthorizationEndpoints receives are dependecies that are injected by the framework
  public static void RegisterAuthorizationEndpoints(this IEndpointRouteBuilder routes) {
    
    var authorization = routes.MapGroup("/api/v1/authorization");

    authorization.MapPost("", (AuthHelper auth, User user) => {
      var token = auth.GenerateJWTToken(new SystemUser() {
        Id = 1,
        Name = "Miguelon",
        Email = "miguel@gmail.com",
        Role = "ADMIN"
      });

      return Results.Ok(new { Token = token });
    });
  }
}