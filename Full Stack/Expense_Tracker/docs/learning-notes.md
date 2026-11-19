# Learning Notes

- Standalone Angular components declare their own imports and do not need Angular NgModules.
- Angular providers such as Router and HttpClient are configured in `app.config.ts`.
- Lazy routes load a feature component only when its route is visited.
- ASP.NET Core dependency injection creates services and DbContexts for controllers.
- A controller translates HTTP requests into application calls; it should not contain database design or complex business rules.
- EF Core's DbContext is the place where future entity mappings and database queries will live.
- OpenAPI describes the HTTP contract, while Scalar provides a human-friendly way to explore that contract.
- Problem Details gives clients a consistent error response shape.
- CORS is a browser security policy; the development policy explicitly allows the Angular origin.
- ASP.NET Core Identity hashes passwords and stores users in `AspNetUsers`; `ApplicationUser` extends `IdentityUser` when application-specific user fields are needed later.
- `IdentityDbContext<ApplicationUser>` combines ASP.NET Core Identity models with the existing Category and Transaction DbSets.
- A JWT contains claims and is signed; issuer, audience, signing key, and expiration are validated when the API receives it.
- Authentication proves identity. Authorization decides whether an authenticated identity may access a resource.
- Angular `AuthService` owns token storage and login/logout state.
- `authInterceptor` attaches the bearer token and handles 401 responses.
- `authGuard` prevents unauthenticated route activation and preserves the intended return URL.
- Reactive Forms validate login and registration fields before API calls.
