

using Models;

public static class Products {
  public static void RegisterProductsEndpoints(this IEndpointRouteBuilder routes) {
    var products = routes.MapGroup("/api/v1/products");

    products.MapGet("", () => {
      var productsList = new List<Product>() {
        new Product() {
          Id = 1,
          Name = "Papitas",
          Description = "Papas de 150 gramos enchiladas",
          Quantity = 10,
        },
        new Product() {
          Id = 2,
          Name = "Galletas animalito",
          Description = "Galletas animalito gamesa de 240 gramos",
          Quantity = 3,
        }
      };
      return productsList;
    });

    products.MapGet("{id}", (int id) => {
      return new Product() {
        Id = id,
        Name = "Galletas animalito",
        Description = "Galletas animalito gamesa de 240 gramos",
        Quantity = 3
      };
    });

    products.MapPost("", (Product product) => {
      return product;
    });

    products.MapDelete("{id}", (int id) => {
      return Results.Json(new OkResponse() {
        Success = true, 
        Message = "Usuario eliminado"
      });
    });

    products.MapPut("{id}", (int id, Product product) => {
      return Results.Ok(new OkResponse() {
        Success = true,
        Message = "Usuario actualizado"
      });
    });

  }
}