using Helpers;
using Models;
public static class Authorization {
  // The parameters that RegisterAuthorizationEndpoints receives are dependecies that are injected by the framework
  public static void RegisterAuthorizationEndpoints(this IEndpointRouteBuilder routes, IConfiguration configuration) {
    
    var authorization = routes.MapGroup("/api/v1/authorization");

    authorization.MapPost("", (User user) => {
      var authHelper = new AuthHelper(configuration);
      var token = authHelper.GenerateJWTToken(new SystemUser() {
        Id = 1,
        Name = "Miguelon",
        Email = "miguel@gmail.com",
        Role = "ADMIN"
      });

      return Results.Ok(new { Token = token });
    });
  }
}