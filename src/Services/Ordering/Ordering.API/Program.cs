using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container 

/*
 Infrastructure - EF Core 
 Application - MediatR
 API - Carter, HealthCheckes, ...

 */


builder.Services
   .AddApplicationServices()
   .AddInfrastructureServices(builder.Configuration)
   .AddApiServices();


var app = builder.Build();

// Configure the HTTP requqest Pipeline
app.UseApiServices();

if(app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
}

app.Run();



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

Primitive Obsession 
    - Primitive Obsession is a code smell where primitives (like string, int, Guid) are used for domain concepts, leading to ambiguity and errors.
    - Using a Guid or string for an orderId, customerId, or productId makes it easy to mix these identifiers up, as they're all of the same type.
    - How Can we solve << Primitive Obssesion >>
        - Strongly Typed IDs
          
Strongly Typed IDs Pattern 
    - Creating distinct types for each kind of ID in your domain
    - This makes your code more expressive and less error-prone
    - It clarifies which type of ID is expected and prevents accidentally using one type of ID (like a productId) where another (like an orderId) is intended.

Anemic-Domain Model Entity 
    - Entity have little or no business logic (domain logic)
    - Essentially data structures with getters and setters
    - But the business rules and behaviors are typically implemented outside the entity, often in service layers.
    - public class Order { public List<OrderItem> OrderItems {get; set;}  }
    - Order Class is anemic because it only contains data and lacks any domain logic or behaviors.

Rich-Domain Model Entity 
    - Entities encapsulate both data and behavior
    - This model enriches entities with methods that embody business rules and domain logic.
    - public classe Order : Aggregate<Guid> {
        private readonly List<OrderItem> _ordemItems = new();
        public IReadOnlyLIst<OrderItem> OrderItems => _orderItem.AsReadOnly();
        public voic AddOrderItem(OrderItem item) { //logic here }
        public void RemoveOrderItem(OrderItem item) { //logic here }
    }
    - Order is a rich domain model as it includes methods AddOrderItem and RemoveOrderItem which encapsulate the business logic for manipulating the order items.
    - Rich-Domain model for Entities and ValuesObjects
        - Entities -> Create Static Method
        - ValueObjects -> Of Static Method

Domain Event in DDD 
    - Domain Events represent something that happened in the past and the other parts of the same service boundary same domain need to react to these changes.
    - Domain Event is a business event that ocurrs within the domain model. It often represents a side effect of a domain operation.
    - Achieve consistency between aggregates in the same domain.
    - When an order is laced, an OrderPlaced event might be triggered.
    - Trigger side effects or notify other parts of the system about changes witin the domain.
    - How to use Domain Events in DDD?
        - Encapsulate the event details and dispatch them to interested parties.
        - Communicate changes within the domain to external handlers which may perform actions based on these events.

Domain vs Integration Events 
    - Domain Events 
        - Published and consumed withing a single domain. Strictly within the boundary of the microservice/domain context.
        - Indicate something that has happened within the aggregate.
        - In-process and synchronously, sent using an in-memory message bus. 
        - Example: OrderPlacedEvent
    - Integration Events 
        - Used to communicate state changes or events between contexts or microservices.
        - Overall system's reaction to certain domain events
        - Asynchronously, sent with a message broker over a queue.
        - Example: After handling OrderPlacedEvent, and OrderPlacedIntegrationEvent might be published to a message broker like RabbitMQ, then consumes by other microservices.

Entity Framework Core
    - Entity Framework (EF) Core is a lightweight, extensible, open source and cross-platform data access technology.
    - EF Core can serve as an object-relational mapper (ORM)
    - Enables .NET developers to work with a database using .NET objects. 
    - Eliminates the need for most of the data-access code that typically needs to be written. 
    - EF Core supports many database engines through the use of "database providers".
    - Each system has its own database provider, which is shipped as NuGet package.
    - Relations with Has/With Pattern
        - RelationsShip with other entities one-to-one, one-to-many, or many-to-many
        - EF Core handles these relationsships using the Has/With Pattern 
        - Relationsship can be confiured using HasMany/WithOne methods for one-to-many and HasOne/WithMane for many-to-one relationship.
        - One-To-Many RelationShip
            - Order and OrderItem have a one-to-many relationship (an order can have multiple items) configured using EF Core Fluent API as follows:
                - builder.Entity<Order>().HasMany(o => o.OrderItems).WithOne().HasForeignKey(oi => oi.OrderId);
            - Each OrderItem references an Order through a foreign key OrderId.
    - ValueObject Mapping with Complex Type and ComplexProperty 
        - EF Core 8, "Complex Types" are introduced to better support value objects in DDD.
        - Complex Types is an object that does not have a primary key and is used to represent a set of properties in an entity. 
        - Examples of Complex Types (Value Objects)
            - Address can be a complex type representing the shipping and billing addresses for an order. And configuring Complex Types in OnModelCreating:
            - builder.Entity<Order>(b => {  b.ComplexProperty(e => e.BillingAddress); b.ComplexProperty(e => e.ShippingAddress);  });
            - [ComplexType  ] public class Address { //properties here }
    - Interceptors
        - In EF Core enable the interception, modification, or suppression of EF Core Operations
        - This includes low-level database operations such as executing a command, as well as higher-level operations, such as calls to SaveChanges.
        - SaveChanges Interception
            - The SaveChanges and SaveChangesAsync interception points are used to execute custom logic when saving changes to the database.
            - These interception points are defined by the ISaveChangesInterceptor interface.
            - EF Core provides a SaveChangesInterceptor base class with no-op (no operation) method as a convenience.
            - USe Case: SaveChanges interceptions for Auditing
                - Interception of SaveChanges can be used to create an independent audit record of chagnes.
                - This is useful for maintaining a history of who changed an entity ans when.
                - Before saving changes to the database, you can iterate through the changed entities in the DbContext and log or store the audit information, like timestamp or user identifiers.
        - Registering Interceptors
            - How To Register Interceptors?
                - Interceptors are registered using AddInterceptors when configuring a DbContext instance.
                - Common approach is usually done in the OnConfiguring method of the DbContext.
                - Example Code: Registering a SaveChangesInterceptor
                - public class ExampleContext: BlogsContext {
                    protected overrid void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
                        => optionsBuilder.AddInterceptors(new TaggedQueryCommandInterceptor());
                  }
        - Implementing a Custom SaveChangesInterceptor 
            - Implement a custom interceptor by extending SaveChangesInterceptor or implemeting ISaveChangesInterceptor.
            - Override methods like SavingChanges and SavedChanges to execute custom logic.
            - Example Code: Implementing Auditing in an Interceptor 
            - public class MySaveChangeInterceptor : SaveChangeInterceptor {
                public override InterceptionResult<int> SavingChanges(DbContextEventData eventData)  {
                    var context = eventData.Context;
                    foreach(var entry in context.ChangeTraker.Entries().Where(e => e.State == ... ) 
                    {
                        if(entry.Entity is AuditableEntity entity) 
                        {
                            if(entry.State == EntityState.Added) 
                            {
                                entity.CreatedAt = Datetime.UtcNow;
                                entity.CreatedBy = "anonymous";  //get current user here
                            }
                        }
                    }
                }
              }

Dispatch Domain Eventswith EF Core SaveChangesInterceptor
    - Steps dispatch Events:
        - Creawte a SaveChangesInterceptor
        - Identify and Dispatch Domain Events 
            - public class DomainEventDispatcherInterceptor : SaveChangesInterceptor
        - Registering the Interceptor into DependencyInjection class
    - Best Practice to Dispatch Events:
        - EF Core SaveChangesInterceptor
        - [Entity (Domain Events)] <- [Event Dispatcher] -> [Event Consumer]

CQRS - Command Query Responsability Segregation
    - CQRS design pattern in order to avoid complex queries to get rid of inefficient joins.
    - Separates read and write operation with separating database.
    - Commands: changing the state of data into application.
    - Queries: handling complex join operations and returning a result and don't change the state of data into application.
    - Large-scaled microservies architectures needs to manage high-volume data requirements.
    - Single database for services can cause bottlenecks.
    - Uses both CQRS and Event Sourcing patterns to improve application performance.
    - CQRS offers to separates read and write data that provide to maximize query performance and scalability
    - Monolhitic has single database is both working for complex join queries, and also perform CRUD operations.
    - When application goes more complex, this query and CRUD operations will become un-manageble situation.
    - Application required some query that needs to join more than 10 table, will lock the database due to latency of query computation.
    - Performing CRUD operations need to make complex validations and process long business logics, will cause to lock databse operations.
    - Reading and Writing database has different approaches define different strategy.
    - <<Separation of concerns>> principles: separate reading database and the writing database with 2 database.
        - Read database uses No-Sql database with denormalized data.
        - Write database uses Relational databases with fully normalized and supports strong data consistency.

CQRS - Logical and Physical Implementation
    - Logical Implementation: 
        - Splitting Operations, Not Database. Separate read (query) operations from the write (command) operations at the code, but not necessarily at the database level. 
        - Even though the same database is used, the paths for reading and writing data are distinct.
    - Physical Implementation 
        - Separate Database. Splitting the read and write operations not just at the code level but also physically using separate databases.
        - Introduces data consistency and synchronization problems.

Event Sourcing Pattern
    - Most applications saves data into database with the current state of the entity. I.e. user change the email address table, email field updated with the latest updated one. Always kow the latst status of the data.
    - In large-scaled architectures, frequent update database operations can negatively impact databse perform responsiveness, and limits of scalability.
    - Event Sourcing pattern offers to persit each action that affects to data into Event Store database. And call all these actions as a event.
    - Instead of saving latest status of data into database, Event Sourcing pattern offers to save all events into database with sequential ordered of data events.
    - This events database called Event Store.
    - Instead of overriding the data into table, It create a new record for each change to data, and it becomes sequential list of past events.
    - Event Store database become the source-of-truth of data.
    - Sequential event list using generating Materialize Views that represents final state of data to perform queries.
    - Event Store convert to read database with following the Materialized Views Pattern.
    - Convert operation can handle by publish/subcriber pattern with publish event with message broker systems.
    - Event list gives ability to replay events at given certain timestamp.
    - CQRS pattern is mostly using with the Event Sourcing pattern
    - Store events into the write database; source-of-truth events database.
    - Read database of CQRS pattern provides materialized views of the data with denormalized tables.
    - Materialized views read database consumes events from write database convert them into denormalized views. 
    - The writing database is never save status of data only events actions are stored.
    - Store history of data and able to reply any point of time in order to re-generate status of data.
    - System can increased query performance and scale databases independently;

Eventual Consistency Principle
    - CQRS with Event Sourcing Pattern leads Eventual consistency.
    - Eventual Consistency is especially used for systemas prefer high availability to strong consistency.
    - The systema will become consistent after a certain time.
    - We called this latency is a Eventual Consistency Principle and offers to be consistent after a certain time.
    - There are 2 type of "Consistency Level"
    - Strict Consistency: When we save data, the data should affect and seen immediately for every client.
    - Eventual Consistency: When we write any data, it will take some time for clients reading the data.
    - CQRS Design Pattern and Event Sourcing patterns:
        - When user perform any action into application, this will save actions as a event into event store.
        - Data will convert to Reading database with following the publish/subscribe patter with using message brokers.
        - Data will be denormalized into materialized view database for querying from the application.
    - We call this process is a Eventual Consistency Principle

Mediator Pattern and Pipeline Behaviours
    - Mediator Pattern: useful in complex or enterprise-level applications where request processing often involves more than just business logic.
    - Handling a request: might require additional steps like logging, validation, auditing, and applying security checks. These are known as cross-cutting concerns.
    - MediatR: provides a mediator pipeline where these cross-cutting concerns can be inserted transparently.
    - Pipeline coordinates the request handling, ensuring that all necessary steps are executed in the rigth order.
    - In MediatR, pipeline behaviours are used to implement cross-cutting concerns.
    - Wrap around the request handling, allowing you to execute logic before and after the actual handler is called.

EShop Microservices Pipeline Behaviours
    - LogBehavior: A behavior that logs details about the handling of a request.
    - ValidatiorBehavior: A behavior that validates incoming requests before they reach the handler.
    - Every request processed by MediatR in the application is logged and validate consistently
    - Using MediatR with the mediator pattern in ASP.NET Core applications provides a structured and clean way to handle complex request processing.

*/


