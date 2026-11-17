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
