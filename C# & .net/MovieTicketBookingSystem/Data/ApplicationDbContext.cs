using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Theatre> Theatres => Set<Theatre>();
    public DbSet<Screen> Screens => Set<Screen>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Show> Shows => Set<Show>();
    public DbSet<ShowSeat> ShowSeats => Set<ShowSeat>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingSeat> BookingSeats => Set<BookingSeat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(x => x.MovieId);
            entity.Property(x => x.Title).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.Property(x => x.Language).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Genre).HasMaxLength(50).IsRequired();
            entity.ToTable("Movies", table => table.HasCheckConstraint("CK_Movies_DurationMinutes", "[DurationMinutes] > 0"));
        });

        modelBuilder.Entity<Theatre>(entity =>
        {
            entity.HasKey(x => x.TheatreId);
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Location).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<Screen>(entity =>
        {
            entity.HasKey(x => x.ScreenId);
            entity.Property(x => x.ScreenName).HasMaxLength(50).IsRequired();
            entity.HasIndex(x => new { x.TheatreId, x.ScreenName }).IsUnique();
            entity.HasOne(x => x.Theatre).WithMany(x => x.Screens).HasForeignKey(x => x.TheatreId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(x => x.SeatId);
            entity.Property(x => x.RowName).HasMaxLength(5).IsRequired();
            entity.Property(x => x.SeatType).HasMaxLength(30).IsRequired();
            entity.HasIndex(x => new { x.ScreenId, x.RowName, x.SeatNumber }).IsUnique();
            entity.ToTable("Seats", table => table.HasCheckConstraint("CK_Seats_SeatNumber", "[SeatNumber] > 0"));
            entity.HasOne(x => x.Screen).WithMany(x => x.Seats).HasForeignKey(x => x.ScreenId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Show>(entity =>
        {
            entity.HasKey(x => x.ShowId);
            entity.Property(x => x.TicketPrice).HasPrecision(10, 2);
            entity.ToTable("Shows", table =>
            {
                table.HasCheckConstraint("CK_Shows_TimeRange", "[EndTime] > [StartTime]");
                table.HasCheckConstraint("CK_Shows_TicketPrice", "[TicketPrice] >= 0");
            });
            entity.HasOne(x => x.Movie).WithMany(x => x.Shows).HasForeignKey(x => x.MovieId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Screen).WithMany(x => x.Shows).HasForeignKey(x => x.ScreenId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ShowSeat>(entity =>
        {
            entity.HasKey(x => x.ShowSeatId);
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.HasIndex(x => new { x.ShowId, x.SeatId }).IsUnique();
            entity.HasOne(x => x.Show).WithMany(x => x.ShowSeats).HasForeignKey(x => x.ShowId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.Seat).WithMany(x => x.ShowSeats).HasForeignKey(x => x.SeatId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(x => x.BookingId);
            entity.Property(x => x.BookingReference).HasMaxLength(30).IsRequired();
            entity.HasIndex(x => x.BookingReference).IsUnique();
            entity.Property(x => x.CustomerName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.CustomerEmail).HasMaxLength(150).IsRequired();
            entity.Property(x => x.CustomerPhone).HasMaxLength(15).IsRequired();
            entity.Property(x => x.TotalAmount).HasPrecision(10, 2);
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.ToTable("Bookings", table => table.HasCheckConstraint("CK_Bookings_TotalAmount", "[TotalAmount] >= 0"));
            entity.HasOne(x => x.Show).WithMany(x => x.Bookings).HasForeignKey(x => x.ShowId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BookingSeat>(entity =>
        {
            entity.HasKey(x => x.BookingSeatId);
            entity.Property(x => x.Price).HasPrecision(10, 2);
            entity.ToTable("BookingSeats", table => table.HasCheckConstraint("CK_BookingSeats_Price", "[Price] >= 0"));
            entity.HasOne(x => x.Booking).WithMany(x => x.BookingSeats).HasForeignKey(x => x.BookingId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.Seat).WithMany(x => x.BookingSeats).HasForeignKey(x => x.SeatId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
