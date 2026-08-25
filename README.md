# Transaction Aggregation API

# A .NET 8 Web API that aggregates customer financial transactions from multiple mock data sources, normalizes them into a single consistent model, categorizes them, and exposes the results through a clean REST API.

# 

# The project is built using Clean Architecture, so the core business logic has no dependency on the web framework, the data sources, or any external detail.

# 

# Table of contents

# Overview

# Architecture

# Project structure

# API endpoints

# Authentication

# How to build and run

# Running with Docker

# Running the tests

# Sample data

# Overview

# The system solves a common real-world problem: a customer's transactions live across several systems (a bank account, a card processor, a mobile wallet), and each system stores them in a different format. This API:

# 

# Aggregates transactions from all sources.

# Normalizes each source's format into one consistent Transaction model.

# Categorizes each transaction (Groceries, Transport, Income, etc.).

# Exposes the results through endpoints for transactions, category summaries, and a spending overview.

# Architecture

# The project follows Clean Architecture. Dependencies point inward, so the inner layers never depend on the outer ones.

# 

# Domain – core business model (Transaction, Money, category and type enums). Depends on nothing.

# Application – use cases and orchestration (aggregation, categorization, query services) plus the interfaces the outer layers implement.

# Infrastructure – the concrete data sources that read the mock JSON files and normalize them.

# API – controllers, middleware, and configuration that expose the system over HTTP.

# The key benefit: if the mock data sources were replaced with real bank APIs, only the Infrastructure layer would change. The business logic stays untouched.

# 

# Project structure

# TransactionAggregation.Domain          Core entities, value objects, and enums

# TransactionAggregation.Application     Services, interfaces, and DTOs

# TransactionAggregation.Infrastructure  Mock data sources and dependency injection

# TransactionAggregation.API             Controllers, middleware, and Program.cs

# TransactionAggregation.UnitTests       Unit tests for the core logic

# TransactionAggregation.IntegrationTests End-to-end API tests

# API endpoints

# All endpoints are under api/customers/{customerId} and require an API key (see below).

# 

# Method	Route	Description

# GET	/api/customers/{customerId}/transactions	All aggregated, categorized transactions

# GET	/api/customers/{customerId}/categories	Spending totals grouped by category

# GET	/api/customers/{customerId}/overview	Income, spending, net, and top category

# GET	/health	Health check (no API key required)

# Interactive documentation is available through Swagger at /swagger.

# 

# Authentication

# The API is protected with a simple API key. Every request (except Swagger and the health check) must include the key in the X-Api-Key header, or it is rejected with 401 Unauthorized.

# 

# For local testing, the key is:

# 

# my-local-dev-key-12345

# In Swagger, click Authorize and paste the key to test the protected endpoints.

# 

# Note: For this exercise the key is stored in configuration for easy testing. In a production system it would be provided through environment variables or a secrets manager, and user-facing scenarios would use JWT bearer tokens with per-user authorization.

# 

# How to build and run

# Requirements: .NET 8 SDK

# 

# \# Restore and build

# dotnet build

# 

# \# Run the API

# dotnet run --project TransactionAggregation.API

# Then open the URL shown in the console (for example http://localhost:5298/swagger).

# 

# Running with Docker

# Requirements: Docker Desktop

# 

# \# Build the image (run from the solution root)

# docker build -t transaction-aggregation-api -f TransactionAggregation.API/Dockerfile .

# 

# \# Run the container

# docker run -p 8080:8080 transaction-aggregation-api

# Then open http://localhost:8080/swagger.

# 

# Running the tests

# dotnet test

# This runs both:

# 

# Unit tests – cover the categorization engine, the Money value object, and the aggregator (using Moq to fake data sources).

# Integration tests – start the API in memory and verify the endpoints end-to-end, including that requests without an API key are rejected.

# Sample data

# The mock sources are seeded with data for customer CUST-001, covering a salary, rent, groceries, electricity, and several card and wallet purchases. Use CUST-001 when testing the endpoints.

