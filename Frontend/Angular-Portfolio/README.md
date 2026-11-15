# Abdul Rafik S — Professional Angular Portfolio

A responsive, multi-page personal developer portfolio built with Angular 22, TypeScript, and modern component architecture.

The project demonstrates production-grade Angular fundamentals, RxJS streams, routing, dependency injection, and responsive UI engineering. All portfolio content is derived directly from Abdul Rafik S's verified resume.

---

## 1. Project Overview

- **Developer:** Abdul Rafik S
- **Role:** AI Backend Engineer
- **Education:** B.Tech in Artificial Intelligence & Data Science, PSNA College of Engineering and Technology, Anna University (CGPA: 8.70/10)
- **Key Domain Focus:** Multi-Turn Agentic Workflows (LangGraph, LangChain), Distributed Pipelines (Kafka, Temporal), and High-Throughput Microservices (FastAPI, Python).

The portfolio provides an interactive experience showcasing:
- **Home (`/`):** Professional introduction, live system stats queried via HTTP Interceptor, core competencies, and featured project cards.
- **About (`/about`):** Detailed career summary, internship experience at Giggso Pvt Ltd, categorized technical skills, education, and nested sub-routes for competitive hackathon awards (`/about/achievements`).
- **Projects (`/projects`):** Project catalog with category filtering, real-time search, reusable card components, `@ViewChild` programmatic filter reset, and route parameter detail views (`/projects/:id`).
- **Contact (`/contact`):** Verified direct communication channels, template-driven form with Angular validation (`required`, `email`, `minlength`), two-way data binding `[(ngModel)]`, and local submission feedback.
- **404 Fallback (`**`):** Custom route not found screen with navigation recovery.

---

## 2. Technology Stack

- **Framework:** Angular 22 (`@angular/core`, `@angular/router`, `@angular/forms`, `@angular/common`)
- **CLI & Builder:** `@angular/cli` 22.2.6, `@angular/build:application` (Vite / esbuild)
- **Language:** TypeScript 6.0
- **Reactive Programming:** RxJS 7.8
- **Testing:** Vitest 5.1 with `@angular/build:unit-test`
- **Styling:** Modular CSS with CSS Custom Properties (CSS variables) for light and dark theme switching
- **Package Manager:** npm 11.17.0
- **Node.js:** v24.19.0

---

## 3. Installation Instructions

1. Ensure Node.js (v20+ or v24+) and npm are installed on your machine.
2. Navigate to the project directory:
   ```bash
   cd Frontend/Angular-Portfolio
   ```
3. Install dependencies:
   ```bash
   npm install
   ```

---

## 4. Commands to Run and Build

### Development Server
Run the local development server at `http://localhost:4200/`:
```bash
npm start
# or
npx ng serve
```

### Production Build
Compile optimized application bundles with esbuild:
```bash
npm run build
# or
npx ng build
```
Production output files are generated in `dist/AngularPortfolio/browser/`.

### Automated Testing
Execute the unit and smoke test suite:
```bash
npm run test -- --no-watch
# or
npx ng test --no-watch
```

---

## 5. Folder Structure

```text
Frontend/Angular-Portfolio/
├── public/
│   └── assets/
│       └── portfolio-stats.json              # Local mock asset for HTTP Interceptor demonstration
├── src/
│   ├── app/
│   │   ├── core/
│   │   │   ├── guards/
│   │   │   │   └── project-exists.guard.ts   # Functional CanActivateFn route guard
│   │   │   ├── interceptors/
│   │   │   │   └── demo-header.interceptor.ts# HttpInterceptorFn adding custom demonstration headers
│   │   │   └── services/
│   │   │       ├── portfolio-data.service.ts # Centralized resume data store & HttpClient calls
│   │   │       ├── project.service.ts        # Filtering and project query logic
│   │   │       └── theme.service.ts          # Light/dark mode state and localStorage persistence
│   │   ├── features/
│   │   │   ├── about/
│   │   │   │   ├── about-achievements.component.ts # Nested route: Hackathons & honors
│   │   │   │   ├── about-overview.component.ts     # Nested route: Experience, education, skills
│   │   │   │   └── about.component.ts              # Parent route shell with sub-tabs & router-outlet
│   │   │   ├── contact/
│   │   │   │   └── contact.component.ts      # Template form, [(ngModel)], validation, submit feedback
│   │   │   ├── home/
│   │   │   │   └── home.component.ts         # Introduction, terminal card, skills highlight
│   │   │   ├── not-found/
│   │   │   │   └── not-found.component.ts    # 404 error page fallback
│   │   │   └── projects/
│   │   │       ├── project-detail.component.ts # Route param (:id) view with ActivatedRoute
│   │   │       └── projects.component.ts     # Catalog, filter controls, @ViewChild integration
│   │   ├── models/
│   │   │   └── portfolio.models.ts           # Strict TypeScript interfaces
│   │   ├── shared/
│   │   │   ├── components/
│   │   │   │   ├── card-wrapper/             # Content projection (<ng-content>) multi-slot component
│   │   │   │   ├── project-card/             # @Input, @Output, EventEmitter, lifecycle hooks
│   │   │   │   ├── quick-filter/             # [(ngModel)] two-way binding & filter methods
│   │   │   │   └── section-header/           # Header with projected action slots
│   │   │   ├── directives/
│   │   │   │   └── card-hover.directive.ts   # Custom attribute directive (@HostListener, Renderer2)
│   │   │   ├── pipes/
│   │   │   │   ├── filter-projects.pipe.ts   # Custom pipe for project filtering
│   │   │   │   └── truncate.pipe.ts          # Custom pipe for string truncation
│   │   │   └── shared.module.ts              # NgModule demonstrating declarations, imports, and exports
│   │   ├── app.component.spec.ts             # Smoke and unit tests
│   │   ├── app.component.ts                  # Root layout shell, navigation, mobile drawer, theme toggle
│   │   ├── app.config.ts                     # ApplicationConfig, provideRouter, provideHttpClient
│   │   └── app.routes.ts                     # Route table, lazy loading, guards, nested routes
│   ├── index.html                            # Root HTML template
│   ├── main.ts                               # Angular application bootstrap
│   └── styles.css                            # CSS custom properties, responsive design, dark/light theme
├── angular.json                              # Angular CLI project configuration
├── package.json                              # Dependencies and build scripts
├── tsconfig.app.json                         # Application TypeScript configuration
├── tsconfig.json                             # Base TypeScript configuration
└── tsconfig.spec.json                        # Testing TypeScript configuration
```

---

## 6. Implemented Features

1. **Light & Dark Theme Engine:**
   - Managed by `ThemeService` using a reactive `signal`.
   - Persists state across page refreshes via `localStorage`.
   - Modifies CSS custom properties on `document.documentElement[data-theme]`.
   - Zero layout shift with smooth color transitions.
2. **Responsive Mobile Navigation:**
   - Functional hamburger drawer menu with automatic close on route transition.
   - Fluid typography and responsive CSS grid (`grid-cols-2`, `grid-cols-3`).
3. **Multi-Page Routing & Lazy Loading:**
   - Complete router setup with code splitting (`loadComponent`).
   - Every route loads as an isolated JavaScript chunk.
   - Nested route hierarchy under `/about`.
   - Dynamic route parameter extraction under `/projects/:id`.
4. **Route Protection Guard:**
   - `projectExistsGuard` checks the project ID in route parameters against the data store.
   - Redirects invalid IDs back to `/projects`.
5. **Interactive Filtering & Child Communication:**
   - `QuickFilterComponent` emits filter changes to `ProjectsComponent`.
   - Parent accesses child via `@ViewChild` to clear or reset filters programmatically.
6. **Form Validation with Feedback:**
   - Validation triggers on blur and dirty states (`required`, `email`, `minlength`).
   - Disables submit button until the form is completely valid.
   - Displays a preview banner of submitted payload without server spoofing.

---

## 7. Angular Syllabus Coverage Table

| Angular Concept | Implementation Location | Description |
|---|---|---|
| **CLI Project & Architecture** | `angular.json`, `package.json`, `tsconfig.app.json` | Configured modern Angular 22 workspace using `@angular/build:application`. |
| **Components & Templates** | `src/app/features/*` | Structured components with templates and scoped styling. |
| **Interpolation (`{{ }}`)** | `AppComponent`, `HomeComponent`, `ProjectCardComponent` | Data rendering for titles, counts, dates, and descriptions. |
| **Property Binding (`[prop]`)** | `ProjectCardComponent`, `CardWrapperComponent` | Binding project properties, CSS classes, disabled states, and attributes. |
| **Event Binding (`(event)`)** | `ProjectCardComponent`, `ContactComponent` | Handling click events, form submissions, and modal triggers. |
| **Two-Way Binding (`[(ngModel)]`)** | `QuickFilterComponent`, `ContactComponent` | Binding form fields (name, email, subject, message, search input). |
| **Angular Modules (`NgModule`)** | `src/app/shared/shared.module.ts` | Declares and exports shared components, directives, and pipes. |
| **`@Input()` Decorator** | `ProjectCardComponent`, `CardWrapperComponent`, `SectionHeaderComponent` | Parent-to-child data communication. |
| **`@Output()` & `EventEmitter`** | `ProjectCardComponent`, `QuickFilterComponent` | Child-to-parent event emission (`selectProject`, `viewDetails`, `filterChange`). |
| **Lifecycle Hooks (`ngOnInit`, `ngOnChanges`, `ngOnDestroy`)** | `ProjectCardComponent` | Tracking lifecycle stages, input updates, and cleanup. |
| **`@ViewChild` Access** | `src/app/features/projects/projects.component.ts` | Parent component programmatic access to `QuickFilterComponent.resetFilters()`. |
| **Content Projection (`<ng-content>`)** | `CardWrapperComponent`, `SectionHeaderComponent` | Multi-slot content projection using selectors (`[card-header]`, `[actions]`). |
| **Structural Directives (`*ngIf`, `*ngFor`)** | `ProjectCardComponent`, `ContactComponent`, `CardWrapperComponent` | Conditional template rendering and iteration. |
| **Control Flow (`@if`, `@for`)** | `ProjectsComponent` | Modern control flow syntax for projects grid and empty state handling. |
| **Attribute Directives (`ngClass`, `ngStyle`)** | `CardWrapperComponent`, `ProjectCardComponent`, `AppComponent` | Dynamic styling based on variants, active tabs, and theme state. |
| **Custom Directive** | `src/app/shared/directives/card-hover.directive.ts` | `CardHoverDirective` (`appCardHover`) using `@HostListener` and `Renderer2`. |
| **Built-in Pipes (`date`, `uppercase`, `slice`)** | `ProjectCardComponent`, `SectionHeaderComponent`, `HomeComponent` | Formats project dates, technology slices, and uppercase badges. |
| **Custom Pipes** | `src/app/shared/pipes/truncate.pipe.ts`, `filter-projects.pipe.ts` | `TruncatePipe` and `FilterProjectsPipe` for text formatting and filtering. |
| **Dependency Injection & Services** | `src/app/core/services/*` | `@Injectable({ providedIn: 'root' })` services with `inject()` and constructor DI. |
| **HTTP Client & Interceptor** | `demo-header.interceptor.ts`, `portfolio-data.service.ts` | Request interception adding `X-Portfolio-Client` demonstration header. |
| **Router & Route Parameters** | `app.routes.ts`, `project-detail.component.ts` | Routing configuration, `RouterLinkActive`, and parameter extraction (`:id`). |
| **Nested Routes** | `app.routes.ts`, `src/app/features/about/` | Child routes under `/about` (`/about` overview and `/about/achievements`). |
| **Route Guard** | `src/app/core/guards/project-exists.guard.ts` | `CanActivateFn` validating project ID existence before activating route. |
| **Lazy Loading** | `src/app/app.routes.ts` | Code-split feature components using dynamic `loadComponent()` imports. |
| **Unit & Smoke Testing** | `src/app/app.component.spec.ts` | Automated tests verifying creation, theme toggling, pipes, and services. |
