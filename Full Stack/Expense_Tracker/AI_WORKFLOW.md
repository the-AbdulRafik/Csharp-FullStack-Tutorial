# AI Workflow

## Architecture rules

- Keep one ASP.NET Core API production project and one Angular application.
- Use controllers for HTTP endpoints, services for business logic, and `ExpenseTrackerDbContext` for persistence.
- Keep Angular organized by feature and use standalone components with lazy routes.
- Prefer plain CSS and understandable code.
- Do not add microservices, CQRS/MediatR, generic repositories, unit-of-work wrappers, authentication, or UI frameworks without an explicit decision.

## Coding conventions

- Use nullable reference types and async APIs when I/O is introduced.
- Keep API contracts in DTOs.
- Keep configuration in appsettings/environment configuration rather than components.
- Use clear names and small classes.

## Agent responsibilities

Agents should inspect existing work before editing, make focused changes, preserve user changes, and explain any limitation. Agents must not add business features outside the active phase.

## Testing requirements

Run the smallest relevant build and test commands after changes. Backend endpoint changes require a runtime probe when practical. New behavior should have a focused test.

## Documentation requirements

Update the directly related documentation when architecture, commands, URLs, dependencies, or behavior changes. Documentation must distinguish verified behavior from unavailable environment checks.

## Git conventions

Inspect changes, run validation, and ask the user for approval before committing. Use meaningful commit messages. Never commit secrets, local databases, build output, or credentials.

## Complexity constraint

Do not introduce unnecessary architectural complexity. The simplest maintainable design is preferred for this learning project.
