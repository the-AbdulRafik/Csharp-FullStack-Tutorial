# Console Feature Record

This file is the feature checklist for the console menu. Update the status when a menu feature changes.

| Menu | Feature | Status |
|---|---|---|
| Branch Management | Add branch | Complete |
| Branch Management | List and search branches | Complete |
| Branch Management | Update branch | Complete |
| Branch Management | Deactivate branch | Complete |
| Member Management | Register member | Complete |
| Member Management | List/search by ID or name | Complete |
| Member Management | Filter members by branch | Complete |
| Member Management | Update member | Complete |
| Member Management | Deactivate member | Complete |
| Book Management | Add book | Complete |
| Book Management | List/search by title, author, or ISBN | Complete |
| Book Management | Filter books by branch | Complete |
| Book Management | Update book | Complete |
| Book Management | Deactivate book | Complete |
| Loan Management | Issue book | Complete |
| Loan Management | List all loans | Complete |
| Loan Management | View member loan history | Complete |
| Loan Management | View book loan history | Complete |
| Loan Management | Return book | Complete |
| Loan Management | Mark book as lost | Complete |
| Reports | Member count by branch | Complete |
| Reports | Loan count by book | Complete |
| Reports | Members without loans | Complete |
| Reports | Books without loans | Complete |
| Audit Logs | View recent audit entries | Complete |

## Main menu

```text
1. Branch Management
2. Member Management
3. Book Management
4. Loan Management
5. Reports
6. View Audit Logs
0. Exit
```

## Feature notes

- A branch, member, or book is deactivated instead of physically deleted.
- The database prevents a member from holding the same book twice at the same time.
- Issuing a loan and writing its audit entry happen in one transaction.
- Invalid input and SQL errors are shown in the console.
