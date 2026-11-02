# Architecture Diagram

This diagram documents the implemented backend layers and their relationships.

## Application Architecture

<!-- mermaid-checked: no \n, no em-dash/en-dash, no {} in labels, subgraphs are id["label"], arrows are -->|"label"|, all subgraphs closed by end, ids unique -->
~~~mermaid
flowchart TD
    subgraph Client["Client Layer"]
        ApiClient["Angular or Postman Client"]
    end
    subgraph App["ASP.NET Core Application"]
        Controllers["REST Controllers"]
        Services["Business Services"]
        Security["JWT Authentication"]
        Middleware["Exception Middleware"]
    end
    subgraph Data["Data Layer"]
        Repositories["Repository Implementations"]
        Context["EF Core DbContext"]
        Database[("SQL Server MovieTicketBookingDB")]
    end

    ApiClient -->|"HTTP JSON requests"| Middleware
    Middleware -->|"routes requests"| Controllers
    Controllers -->|"calls services"| Services
    Middleware -->|"authenticates bearer tokens"| Security
    Security -->|"authorizes roles"| Controllers
    Services -->|"uses repositories"| Repositories
    Repositories -->|"LINQ and EF Core"| Context
    Context -->|"SQL queries"| Database
~~~

### Technology Stack Summary

| Layer | Technology | Version | Purpose |
| --- | --- | --- | --- |
| Language | C# | Current .NET 10 | Domain and application logic |
| Runtime | .NET | 10 | Hosting and dependency injection |
| API | ASP.NET Core Web API | 10 | REST endpoints |
| ORM | Entity Framework Core | 10 | Database access and migrations |
| Database | Microsoft SQL Server Express | Local | Persistent storage |
| API contract | OpenAPI | 3 | API discovery |

### Data Storage and External Services

The application uses SQL Server through EF Core and stores users alongside booking data. JWTs are generated and validated by the application; there is no cache, message broker, payment provider, or external identity provider.

### Key Architectural Decisions

- Controllers are thin and business rules are kept in services.
- DTOs prevent EF entities from becoming public API contracts.
- Repository and service interfaces support dependency inversion and future unit testing.

## Component Relationships

<!-- mermaid-checked: no \n, no em-dash/en-dash, no {} in labels, subgraphs are id["label"], arrows are -->|"label"|, all subgraphs closed by end, ids unique -->
~~~mermaid
flowchart LR
    subgraph PresentationLayer["Presentation"]
        cMovies["MoviesController"]
        cCatalogue["CatalogueControllers"]
        cShows["ShowsController"]
        cBookings["BookingsController"]
        cAuth["AuthController"]
    end
    subgraph BusinessLayer["Business Logic"]
        cMovieService["MovieService"]
        cCatalogueService["CatalogueService"]
        cShowService["ShowService"]
        cBookingService["BookingService"]
        cUserService["UserService"]
        cJwtService["JwtService"]
    end
    subgraph DataAccessLayer["Data Access"]
        cMovieRepository["MovieRepository"]
        cTheatreRepository["TheatreRepository"]
        cScreenRepository["ScreenRepository"]
        cSeatRepository["SeatRepository"]
        cShowRepository["ShowRepository"]
        cBookingRepository["BookingRepository"]
        cUserRepository["UserRepository"]
        cDbContext["ApplicationDbContext"]
    end
    subgraph CrossCutting["Cross Cutting"]
        cErrors["GlobalExceptionHandler"]
        cAuthMiddleware["JWT Authentication"]
    end

    cMovies -->|"delegates"| cMovieService
    cCatalogue -->|"delegates"| cCatalogueService
    cShows -->|"delegates"| cShowService
    cBookings -->|"delegates"| cBookingService
    cAuth -->|"delegates"| cUserService
    cUserService -->|"issues tokens"| cJwtService
    cUserService -->|"user access"| cUserRepository
    cMovieService -->|"queries"| cMovieRepository
    cCatalogueService -->|"catalogue access"| cTheatreRepository
    cCatalogueService -->|"catalogue access"| cScreenRepository
    cCatalogueService -->|"catalogue access"| cSeatRepository
    cShowService -->|"show access"| cShowRepository
    cBookingService -->|"booking access"| cBookingRepository
    cMovieRepository -->|"uses"| cDbContext
    cTheatreRepository -->|"uses"| cDbContext
    cScreenRepository -->|"uses"| cDbContext
    cSeatRepository -->|"uses"| cDbContext
    cShowRepository -->|"uses"| cDbContext
    cBookingRepository -->|"uses"| cDbContext
    cUserRepository -->|"uses"| cDbContext
    cAuthMiddleware -.->|"protects routes"| cBookings
    cAuthMiddleware -.->|"protects routes"| cAuth
    cErrors -.->|"wraps requests"| cMovies
    cErrors -.->|"wraps requests"| cBookings
~~~

### Component Inventory

| Component | Layer | Type | Responsibility |
| --- | --- | --- | --- |
| Controllers | Presentation | ASP.NET Core controllers | HTTP routes and status codes |
| MovieService | Business | Service | Movie catalogue rules |
| CatalogueService | Business | Service | Theatre, screen, and seat rules |
| ShowService | Business | Service | Scheduling and ShowSeat generation |
| BookingService | Business | Service | Booking, pricing, transactions, cancellation |
| Repositories | Data Access | Repositories | Encapsulated persistence operations |
| ApplicationDbContext | Data Access | EF Core DbContext | Unit of work and mappings |
| GlobalExceptionHandler | Cross Cutting | Middleware | Consistent error responses and logging |
| AuthController | Presentation | Controller | Registration and login endpoints |
| UserService | Business | Service | Password verification and token workflow |
| JwtService | Business | Token service | Creates signed JWT bearer tokens |
| UserRepository | Data Access | Repository | User lookup and persistence |
| JWT Authentication | Cross Cutting | Middleware | Validates tokens and role claims |
