# Movie Ticket Booking Database ER Diagram

The source code for the diagram is in [database-er-diagram.eraser](./database-er-diagram.eraser). Paste that file into [app.eraser.io](https://app.eraser.io) and select the Entity Relationship Diagram format.

```text
Movies.movieId < Shows.movieId
Theatres.theatreId < Screens.theatreId
Screens.screenId < Seats.screenId
Screens.screenId < Shows.screenId
Shows.showId < ShowSeats.showId
Seats.seatId < ShowSeats.seatId
Shows.showId < Bookings.showId
Bookings.bookingId < BookingSeats.bookingId
Seats.seatId < BookingSeats.seatId
```

`Users` stores registered customers and seeded administrative accounts. Its email column is unique and its password is stored as an ASP.NET Core Identity hash.

`Seat` is the physical seat. `ShowSeat` is the availability of that physical seat for one show. `BookingSeat` is the historical seat and price captured in a booking.
