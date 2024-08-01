using Catalog.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCarter();
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure HTTP request pipeline
app.MapCarter();

app.UseExceptionHandler(options => { });


app.Run();




/*
Patterns 

- CQRS Pattern: Command Query Responsability Segregation divides operations into commands (write) and queries (read)
- Mediator Pattern: Facilitates object interacting through a 'mediator' reducing direct dependencies and simplifying communications.
- DI in Asp.Net Core: Dependency Injection is a core feature, allowing us to inject dependencies.
- Minimal APIs and Routing in Asp.Net 8: Asp.Net 8's Minimal APIs simplify endoint definitions, providing lightweight syntax for routing and handling HTTP requests.
- ORM Pattern: Object-Relational Mapping abstracts database interactions, work with database objects using high-level codes.

Libraries

- MediatR for CQRS: MediatR library simplifies the implementation of the CQRS pattern.
- Carter for API Endpoints: Routing and handling HTTP requests, easier to define API endpoints with clean and concise code.
- Marten for PostgreSQL Interaction: Use PostgreSQL as a Document DB. It leverages PostgresSQL's JSON capabilities for storing, querying, and managing documents.
- Mapster for Object Mapping: Mapster is a fast, configurable object mapper that simplifies tha task of mapping objects.
- FluentValidation for Input Validation: For building strongly-typed validation rule, ensure inputs are correct before processed.

Folder Structure
    - The project is organized into Model, Features, Data, and Abstractions.
    - Feature like CreateProduct and GetProduct have dedicated handlers and endpoints definitions.
    - Feature folder will be Products.
    - Data Folder and Context objects manages database intercations.

    
    - Data
        - CatalogInitialData.cs
    - Excptions
        - ProductNotFoundException.cs
    - Models
        - Product.cs
    - Features
        - Products
            - CreateProduct
                - CreateProductEndpoint.cs
                - CreateProductHandler.cs
            - DeleteProduct
            - GetProductByCategory
            - GetProductById
            - GetProducts
            - UpdateProduct

Vertical Slice Architecture
    - Introduce by Jimmy Bogard offers this architecture against to traditional layered/onion/clear architecture approaches.
    - Aims to organize code around specific features or use cases, rather than technical concerns.
    - Feature is implemented accross all layers of the software, from the user interface to the database.
    - Often used in the development of complex, feature-rich apps.
    - Divide application into distinct features or functionalities, each of wich cuts through all the layers of the application.
    - Contrast to traditional n-tier or layered architectures, where the application is divide horizontaly (e.g. presentation, business logic, data access layers)
    - Characteristics of Vertical Slice Architecture
        - The application is dividd into feature-based slices.
        - Each slicee is self-contained and independent.
        - There are reduced dependencies between different parts of the application.
        - It promotes the use of cross-functional teams.
        - The architecture supports scalability and maintanability
        - It allows for improved testing and deployment proceesses.
        - Every microservice handles a specific piece of functionality and communicates with other services through weell-defined interfaces.
    - Benefits
        - Focused development, teams can concentrate on one feature at a time.
        - Simplifies refactoring and upgrades since changes in on slice usually don't affect others.
        - Aligns well with Agile and DevOps practices, supporting incremental developmement and continuous delivery.
    - Challenges and Considerations 
        - Duplication of code across slices, particulary for common functionalities.
        - Learning curve involved, especially for teams accustomed to traditional architectures.
        - Design of each slice requires careful consideration to ensure independence and maintanability.
    - Vertical Slice Architecture vs. Clean Architecture
        - VSA emphasize organizing software development around features, cutting through all layers.
        - CA focuses on separation of concerns and dependency rules. It organizes code into layers.
        - VSA, development teams concentrate on delivering complete features, each potentially touching all layers of the stack.
        - CA more structured approach, ensuring that business logic is decoupled from external concerns.
        - VSA well-suited for agile teams working on complex applications with numerous features.
        - CA ideal for large-scale applications where long-term maintanance, scalability and the ability to adapt to changing business requirements.
    - When to choose Vertical Slice over Clean Architecture
        - Rapid Development & Deployment: when the priority is to develop an deploy features rapidly and indepently.
        - Agile & Scrum Teams: For teams practcing Agile or Scrum that need to deliver complete features in short cycles.
        - Microservices: In a microservices architecture, where each service is often responsible for a distinct feature or business capability.

    - Carter Library for Minimal Api Endpoint Definition 
        - With ASP.NET Core 8, Minimal APIs have become a popular way to build lightweigth and performant microservices and web APIs.
        - Carter is a library that extends the capabilitie of ASP.NET Core's Minimal APIs.
        - Provide a more structured way to organize our endpoints and simplifies the creation of HTTP request handlers.
        - Carter is framework that is a thin layer of extension methods and functionality over ASP.NET Core.
        - Carter, especially beneficial in minimal APIs, simplify the development of minimal API endpoints.
 */