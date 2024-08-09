using Discount.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();



/*
 
Application Architecture Style of Discount Microservices 

- Traditional N-Layer Architecture

- Data Access Layer 
    - Only database operations are performed on this layer, The task of this layer is to add, delete, update and extract data from the database.

- Business Layer
    - Implement business logics. Process the data taken by Data Access into the project. We do not use the Data Access layer directly in our applications.

- Presentation Layer
    - Use interacts layer. Show the data to the user and to transmit the data from the use to the Business Layer and Data Access.

- Libraries
    - Generic Packages
        - Mapster
        - FluentValidation
    - gRPC
        - AspnetCore.Grpc
    - Database
        - Microsoft.EntityFrameworkCore.Sqlite
        - Microsoft.EntityFrameworkCore.Tools

- Project Folder Structure of Discount Microservice
    - Models
        - SQLite entity
    - Data
        - EF Core data context and Migration Extentions
    - Service
        - gRPC Services
    - Protos
        - gRPC exposing Apis

- Map to N-Layer Architecture
    - Models -- Domains Layer
    - Data -- Data Access Layer
    - Services -- Business Layer
    - Protos -- Presentation Layer

 */