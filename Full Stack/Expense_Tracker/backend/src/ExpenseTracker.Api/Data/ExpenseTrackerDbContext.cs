using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Data;

public class ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options)
    : DbContext(options)
{
}
