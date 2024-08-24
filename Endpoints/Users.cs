using System.Collections.ObjectModel;
using System.Security.Principal;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Models;

public static class Collections {
  public static List<User> Users = new List<User>() {
    new User() {
      Id = 1,
      FirstName = "Jorge",
      LastName = "Serrano",
      BirthDate = new DateOnly(1990,10,10)
    },
    new User() {
      Id = 2,
      FirstName = "Pedro",
      LastName = "Paramo",
      BirthDate = new DateOnly(2020, 02, 03)
    },
    new User() {
      Id = 3,
      FirstName = "Noel",
      LastName = "Escobedo",
      BirthDate = new DateOnly(1990, 03, 11) 
    }
  };
}

public static class Users {
  public static void RegisterUserEndpoints(this IEndpointRouteBuilder routes) {
    var users = routes.MapGroup("/api/v1/users");

    users.MapGet("", () => Collections.Users);

    users.MapGet("{id}", (HttpContext httpContext, int id, AuthHelper auth) => {
      var token = auth.GetTokenFromHeader(httpContext);
      var authenticatedUser = auth.DecodeJWTToken(token);
      Console.WriteLine(token);
      Console.WriteLine("authenticatedUser", authenticatedUser);
      var user = Collections.Users.FirstOrDefault(item => item.Id == id);
      if (user == null) {
        return Results.NotFound();
      }
      return Results.Ok(new { user });
      // return Results.Ok(new { authenticatedUser });
    }).RequireAuthorization();

    users.MapPost("", (User user) => {
      Collections.Users.Add(user);
      return Results.Created($"/api/v1/users/{user.Id}", user);
    });

    users.MapPut("{id}", (User user, int id) => {
      var existingUser = Collections.Users.FirstOrDefault(item => item.Id == id);
      if (existingUser == null) {
        return Results.NotFound();
      }
      existingUser.FirstName = user.FirstName;
      existingUser.LastName = user.LastName;
      existingUser.BirthDate = user.BirthDate;

      return Results.Ok(existingUser);

    });

    users.MapDelete("{id}", (int id) => {
      var existingUser = Collections.Users.FirstOrDefault(item => item.Id == id);
      if (existingUser != null) {
        Collections.Users.Remove(existingUser);
      } 
      Results.Ok();
    });
  }
}