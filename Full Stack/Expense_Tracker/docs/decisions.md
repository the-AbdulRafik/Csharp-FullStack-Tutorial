# Architecture Decisions

## One backend project

The API remains one production project because the application is small and educational. Splitting it into multiple projects would add indirection without a current benefit.

## Controllers and services

Controllers are used for HTTP entry points and services for application logic. This keeps responsibilities clear without introducing generic abstractions.

## EF Core without migrations

The DbContext and SQL Server provider are configured now so future work has a stable persistence boundary. Business entities, tables, and migrations are intentionally deferred.

## Angular standalone components

Standalone components and lazy routes are the current Angular approach and keep each feature easy to locate.

## No authentication in Phase 0

Authentication and authorization are excluded until the core expense workflows and data model exist.
