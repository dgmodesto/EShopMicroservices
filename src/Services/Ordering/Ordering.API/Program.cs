using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container 

/*
 Infrastructure - EF Core 
 Application - MediatR
 API - Carter, HealthCheckes, ...

 builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddWebServices();
 */


builder.Services
   .AddApplicationServices()
   .AddInfrastructureServices(builder.Configuration)
   .AddApiServices();


var app = builder.Build();

// Configure the HTTP requqest Pipeline


app.Run();

app.UseApiServices();


/*
 
CRUD Ordering Operations

- Get Order with Items (can filter by Name and Customer)
- Create a new Order
- Update an existing Order
- Delete Order
- Add and Remove Item from Order

Asycrnous Ordering Operations 

- Basket Checkout: Consume Basket Checkout event from RabbitMQ using MassTransit
- Order Fulfilment: Perform order fulfilment operations (billing, shipment, notification, ...)
- Raise OrderCreated Domain Event that leads to integration event.
 

Key Prinicples of Clean Architecture 

- Indenpendece of Frameworks
    - The system is not tighly coupled with a specific framework, which makes it adaptable to changes in frameworks and tools.

- Testable
    - The business rules can be tested without the UI, database, web server or any external element.

- UI Agnostic
    - The UI can be change easily, without changing the rest of the system.

- Database Agnostic
    - Business rule are not bound to the database, decoupled from the underlying data store.

- External System Agnostic 
    - Business rules don't know anything about the outside world, isolated.

Clean Architecture Layers

- Entities Layer (Domain Layer)
    - Contains enterprise-wide business rules.
    - Entities encapsulate the most general and high-level rules.
    - Example: In an ordering microservice, entities might be Order, OrderItem, etc. 
- Uses Cases Layer (Application Layer)
    - Contains application-specific business rules.
    - Encapsulates and implements all of the use cases of the system. 
    - Example: CreateOrder, CancelOrder, and UpdateOrderStatus use cases. 
- Interface Adapters Layer (Infrastructure Layer)
    - Converts data from the format most convenient for the use cases and entities, to the format most convenient for external systems.
    - Examples: Mapping data from database models to domain entities. 
- Frameworks and Drivers Layer (Infrastructure/External Concerns)
    - The outermost layer consisting of frameworks and tools such as the Database, the Web Framework, etc.
    - Generally includes the UI, database, external interfaces, etc.
    - Example: REST controllers, database repositories.
- Clean Architecture is Modular, Adaptable, Testable and Maintainable.


Project Folder Structure of Ordering Microservices

- Clean Architecture: 3 Class library and 1 Web Api: Ordering.Domain, Ordering.Application, Ordering.Infrastructure, Ordering.API

- Ordering.Domain
    - Domain Models like Order, OrderItem, Customer, Product.
    - Value Objects, events, exceptions, and base entity classes 

- Ordering.Application
    - CQRS commands and handlers organized by features
    - Dependency injection configurations

- Ordering.Infrastructure
    - Database context, configurations, migrations, and initial data seeding

- Ordering.API
    - API contracts and endpoint definitions.

DDD Types - Strategic and Tactical DDD

- Strategic DDD
    - Understanding and modeling the business domain.
    - It involves identifying he different domains, their subdomains, ar how they interact with each other.

- Tactical DDD
    - Implementation details and provides design patterns.
    - Including Patterns like Entities, Value Objects, Aggregates, etc.
    - We'll focus on Strategic DDD and its key concepts.
    - Tactical DDDD is a set of patterns and principles that help in modeling complex software systems. 
    - Focuses on the domain and domain logic, importance of a model that reflects the business and its rules. 
    - Entities, Value Objects, Aggregates, Aggregate Roots.

    - Entities 
        - Entity is an object that is identified by its identity (ID), rather than its attributes.
        - Identity makes each entity unique, even if other attributes are the same
        - Entities are used to represent objects in the system that have a distinct identity and lifecycle.
        - Examples: Ordering Micorservices, an Order can be an Entity. 
        - It is uniquely identified by an OrderId, even if the other attributes (like data, total amount) are identical to another order. 

    - Value Objects 
        - Value Object is an object that describes some characteristic or attribute but carries no concept of identity.
        - Value Objects are used to describe aspects of the domain with no conceptual identity.
        - They are immutable and are often used to encapsulate complex attributes.
        - Example: An Address used in an Order might be a Value Object, as it is important for the order but does not define the order's identity.

    - Aggregates 
        - Aggregate is a cluster of domain objects (Entities and Value Objects) that can be treated as a single unit.
        - Aggregate will have one of its components be the Aggregate Root
        - Define boundaries around related objects. Operations within the boundary should maintain consistency of changes to data in an Aggregate.
        - Example: Order can be an Aggregate, containing OrderItems, PaymentDetails, etc., within it. The consistency of an order (like total price calculation, stock validation) is maintained within the Aggregate boundaries.

    - Aggregate Roots
        - Aggregate Root is the main Entity in an Aggragate, through which external objects interact with the Aggregate.
        - Provides a gateway to the Aggregate, ensuring that the Aggregate's invariants are enforced and consistency is maintained.
        - Example: In the Order Aggregate, Order itself can be the Aggregate Root. External objects would interact with Order to affect changes within the Aggregate.


 */