using Models;
public static class Customers {
  public static void RegisterCustomersEndpoint(this IEndpointRouteBuilder routes) {
    var customers = routes.MapGroup("/api/v1/customers");

    customers.MapGet("", () => {
      return Results.Ok(new OkResponse() {
        Success = true,
        Message = "Customers list"
      });
    });
  }
}