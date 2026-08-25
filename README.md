# Transaction Aggregation API

A .NET 8 Web API that aggregates customer financial transactions from multiple mock data sources, normalizes them into a single consistent model, categorizes them, and exposes the results through a clean REST API.

The project is built using Clean Architecture, so the core business logic has no dependency on the web framework, the data sources, or any external detail.

Overview:

The system solves  a customer's transactions live across several systems (a bank account, a card processor, a mobile wallet), and each system stores them in a different format. This API:

- Aggregates transactions from all sources
- Normalizes each source's format into one consistent Transaction model
- Categorizes each transaction (Groceries, Transport, Income, and so on)
- Exposes the results through endpoints for transactions, category summaries, and a spending overview

# Architecture:

The project follows Clean Architecture. Dependencies point inward, so the inner layers never depend on the outer ones.

- Domain: the core business model (Transaction, Money, category and type enums). Depends on nothing.
- Application: the use cases and orchestration (aggregation, categorization, query service) plus the interfaces the outer layers implement.
- Infrastructure: the mock data sources that read the JSON files and normalize them.
- API: the controllers, middleware, and configuration that expose the system over HTTP.

If the mock data sources were replaced with real bank APIs, only the Infrastructure layer would change. The business logic stays untouched.

# Project structure:

- TransactionAggregation.Domain: core entities, value objects, and enums
- TransactionAggregation.Application: services, interfaces, and DTOs
- TransactionAggregation.Infrastructure: mock data sources and dependency injection
- TransactionAggregation.API: controllers, middleware, and Program.cs
- TransactionAggregation.UnitTests: unit tests for the core logic
- TransactionAggregation.IntegrationTests: end-to-end API tests

# API endpoints:

All endpoints are under api/customers/{customerId}  and require an API key.

- GET /api/customers/{customerId}/transactions: all aggregated, categorized transactions
- GET /api/customers/{customerId}/categories: spending totals grouped by category
-  GET /api/customers/{customerId}/overview: income, spending, net, and top category
-  GET /health: health check (no API key required)

the customer ID:  CUST-001

Interactive documentation is available through Swagger at /swagger.

# Authentication:

The API is protected with a simple API key. Every request (except Swagger and the health check) must include the key in the X-Api-Key header, or it is rejected with 401 Unauthorized.

# For local testing, the key is:

my-local-dev-key-12345

In Swagger, click Authorize and paste the key to test the protected endpoints.

Note: for this exercise the key is stored in configuration for easy testing. In a production system it would be provided through environment variables or a secrets manager, and user-facing scenarios would use JWT bearer tokens with per-user authorization.

# How to build and run:

Requires the .NET 8 SDK.

- dotnet build
- dotnet run --project TransactionAggregation.API

Then open the URL shown in the console, for example http://localhost:5298/swagger.

# Running with Docker:

Requires Docker Desktop.

- docker build -t transaction-aggregation-api -f TransactionAggregation.API/Dockerfile .

- docker run -p 8080:8080 transaction-aggregation-api

Then open http://localhost:8080/swagger. If port 8080 is busy, use -p 5000:8080 and open http://localhost:5000/swagger.

# Running the tests:

- dotnet test
  
This runs the unit tests (categorization engine, Money value object, and the aggregator using Moq) and the integration tests (the endpoints, tested end to end, including that requests without an API key are rejected).

Sample data:

The mock sources are seeded with data for customer CUST-001, covering a salary, rent, groceries, electricity, and several card and wallet purchases. Use CUST-001 when testing the endpoints.


# Landing page without swagger:
<img width="1907" height="1011" alt="image" src="https://github.com/user-attachments/assets/28048bda-8566-473b-bf42-11055e781e2b" />

# Landing page with swagger:

<img width="1885" height="1002" alt="image" src="https://github.com/user-attachments/assets/16eefb7f-d107-49ec-97ef-d75c3397cd54" />
