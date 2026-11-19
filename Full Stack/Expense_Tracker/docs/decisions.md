# Architecture Decisions

## One backend project

The API remains one production project because the application is small and educational. Splitting it into multiple projects would add indirection without a current benefit.

## Controllers and services

Controllers are used for HTTP entry points and services for application logic. This keeps responsibilities clear without introducing generic abstractions.

## EF Core with Identity migration

The existing DbContext was changed from `DbContext` to `IdentityDbContext<ApplicationUser>`. A new additive migration adds only Identity tables; `InitialCreate` was not changed.

## Angular standalone components

Standalone components and lazy routes are the current Angular approach and keep each feature easy to locate.

## JWT authentication

JWT bearer authentication is used for the MVP. Access tokens expire after 30 minutes. Refresh tokens and server-side revocation are intentionally deferred.

## Client token storage

The Angular application stores the token and expiration in `localStorage` for understandable training behavior. This is not immune to XSS. Logout clears client state and does not revoke an already-issued JWT.
