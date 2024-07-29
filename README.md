# EShopMicroservices

# Architecture

- Solution
  - Api Gateways
    - YarpApiGateway

  - Buildin Blocks

  - Services
    - Basket
      - Basket.API
    - Catalog
      - Catalog.API
    - Discount
      - Discount.Grpc
    - Ordering
      - Ordering.API
      - Orderging.Application
      - Ordering.Domain
      - Ordering.Infrastructure
    - WebApps
      - Shopping.Web
  - docker-compose

  # Port Numbers 

  - Microservices
    - Catalog: 
      - Local Env:
        - 5000 - 5050
      - Docker Env:
        - 6000 - 6060
      - Docker Inside
        - 8080 - 8081
    - Basket: 
      - Local Env:
        - 5001 - 5051
      - Docker Env:
        - 6001 - 6061
      - Docker Inside
        - 8080 - 8081
    - Discount: 
      - Local Env:
        - 5002 - 5052
      - Docker Env:
        - 6002 - 6062
      - Docker Inside
        - 8080 - 8081
    - Ordering: 
      - Local Env:
        - 5003 - 5053
      - Docker Env:
        - 6003 - 6063
      - Docker Inside
        - 8080 - 8081
  