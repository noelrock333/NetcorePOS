using Helpers;
using Microsoft.AspNetCore.Mvc;
using Models;
public static class Authorization {
  // The parameters that RegisterAuthorizationEndpoints receives are dependecies that are injected by the framework
  public static void RegisterAuthorizationEndpoints(this IEndpointRouteBuilder routes) {
    
    var authorization = routes.MapGroup("/api/v1/authorization");

    authorization.MapPost("", async (AuthHelper auth, [FromBody] SystemUser sysUsr) => {
      var hashedPassword = auth.HashPassword(sysUsr.Password);
      var user = SystemCollections.SystemUsers.FirstOrDefault(item => item.Username == sysUsr.Username && item.Password == hashedPassword);
      if (user != null) {
        var token = auth.GenerateJWTToken(user);
        return Results.Ok(new { Token = token });
      } else {
        return Results.NotFound();
      }
    });
  }
}

public static class SystemCollections {
  public static List<SystemUser> SystemUsers = new List<SystemUser>() {
    new SystemUser() {
      Id = 1,
      Username = "MikeO",
      Name = "Miguelon",
      Email = "miguel@gmail.com",
      Role = "ADMIN",
      Password = "1af17e73721dbe0c40011b82ed4bb1a7dbe3ce29" // something
    },
    new SystemUser() {
      Id = 2,
      Username = "noelrock333",
      Name = "Noel",
      Email = "noel@gmail.com",
      Role = "EMPLOYEE",
      Password = "a761ce3a45d97e41840a788495e85a70d1bb3815" // supersecret
    }
  };
}