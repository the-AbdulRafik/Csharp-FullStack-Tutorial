# HCL Training Program: Academic Repository

This repository contains coursework, projects, and practical exercises completed during the HCL Training program. It serves as a structured portfolio for academic evaluation and progress tracking.

---

## Technical Stack

| Layer | Technologies |
| --- | --- |
| Backend & Frameworks | C# (.NET Core / .NET 10), ASP.NET Core Web API |
| Architecture & Patterns | N-Tier Architecture, Repository Pattern, Service Layer, Dependency Separation |
| Database & Querying | Microsoft SQL Server, T-SQL, ADO.NET (`Microsoft.Data.SqlClient`) |
| Frontend & Web | HTML5, CSS3, JavaScript (ES6+), Responsive Layouts |
| Tooling & DevOps | Git, GitHub, Conventional Commits, Visual Studio, VS Code |

---

## Repository Structure

```text
HCL_Training/
├── .gitignore
├── README.md
├── C# & .net/
│   ├── BankManagement-API/             # ASP.NET Core REST API for bank account management
│   ├── Console-App/                    # Core C# syntax, control flow, and OOP basics
│   ├── CourierShipmentTrackingSystem/   # Domain-driven courier logistics and status tracking
│   ├── DB/                             # Relational database schemas and SQL exercises
│   ├── OrderManagementSystem/          # Order processing and inventory management console app
│   ├── Sample_Example/                 # Enterprise N-Tier student/course management with Repository Pattern
│   ├── Sample_Frontend/                # Web client for API interaction
│   └── TrainingManagementSystem/       # Training management with modular payment and session services
└── HTML & CSS/
    ├── Landing Page/                   # Multi-page responsive product landing experience
    ├── student management/             # Dynamic student record interface
    └── *.html, *.css, *.js             # Web foundations (forms, login, semantic layouts)
```

---

## Projects Overview

### 1. C# & .NET Core Modules

#### Bank Management API (`C# & .net/BankManagement-API`)
- ASP.NET Core Web API providing RESTful endpoints for account management.
- Features account creation, deposits, withdrawals, and balance inquiries.
- Includes Swagger OpenAPI support and HTTP request specifications.

#### Courier Shipment Tracking System (`C# & .net/CourierShipmentTrackingSystem`)
- Console-based tracking system managing customers, shipments, and lifecycle statuses.
- Implements custom domain exceptions, status history tracking, and JSON persistence.
- Uses validation utilities and search services.

#### Enterprise Student & Course Management (`C# & .net/Sample_Example`)
- Demonstrates enterprise N-Tier separation with SQL Server integration.
- Implements the Repository Pattern:
  - `StudentRepository`, `CourseRepository`, `DepartmentRepository`, `EnrollmentRepository`, `AuditLogRepository`
- Service orchestration layer for business logic and validation.
- SQL DDL and DML scripts included for schema setup.

#### Training Management System (`C# & .net/TrainingManagementSystem`)
- Modular enterprise training platform.
- Features polymorphic payment processing (`IPaymentProcessor`, `ClassroomPaymentProcessor`, `OnlinePaymentProcessor`).
- Supports session scheduling (`ClassroomSession`, `OnlineSession`), fee calculation, and enrollment reporting.

#### Order Management System (`C# & .net/OrderManagementSystem`)
- Interactive console application modeling order placement, product cataloging, and validation.

#### Console Fundamentals (`C# & .net/Console-App`)
- Foundational exercises covering C# data types, branching logic, collections, and methods.

#### Database Scripts (`C# & .net/DB`)
- SQL DDL, DML, relational integrity constraints, foreign keys, and analytical queries.

---

### 2. Web Development Modules (`HTML & CSS/`)

- **Landing Page**: Complete responsive web application with navigation, product listings, and contact forms.
- **Student Management UI**: Client-side JavaScript interface for student data manipulation.
- **Foundations**: Semantic HTML5 forms, modern CSS Flexbox styling, login views, and responsive design patterns.

---

## Version Control Standards

- **Single Monorepo**: All nested `.git` folders were eliminated to ensure direct tracking of all course projects under one unified repository.
- **Strict Build Artifact Exclusion**: A centralized `.gitignore` prevents compilation artifacts (`bin/`, `obj/`), IDE state files (`.vs/`), and local runtime datasets from entering source control.
- **Conventional Commits**: Every project and feature is isolated in descriptive, atomic commits formatted by scope.
