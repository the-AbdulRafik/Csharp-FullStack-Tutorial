# API endpoint reference

All routes are rooted at `/api` and return camelCase JSON.

| Module | Routes |
| --- | --- |
| Movies | `GET/POST /movies`, `GET/PUT/DELETE /movies/{id}`, `GET /movies/search?title=` |
| Theatres | `GET/POST /theatres`, `GET/PUT/DELETE /theatres/{id}` |
| Screens | `GET/POST /screens`, `GET/PUT/DELETE /screens/{id}`, `GET /screens/theatre/{theatreId}` |
| Seats | `POST /seats`, `POST /seats/bulk`, `GET /seats/{id}`, `GET /seats/screen/{screenId}`, `PUT/DELETE /seats/{id}` |
| Shows | `GET/POST /shows`, `GET/PUT/DELETE /shows/{id}`, movie/theatre/date filters, and `GET /shows/{showId}/seats` |
| Show seats | `GET /shows/{showId}/seats/available`, `GET /shows/{showId}/seats/booked` |
| Bookings | `GET/POST /bookings`, `GET /bookings/{id}`, `GET /bookings/show/{showId}`, `PUT /bookings/{id}/cancel` |
| Health | `GET /health` |
| Authentication | `POST /auth/register`, `POST /auth/login` |

`POST /shows` creates the availability rows for the screen's physical seats. `POST /bookings` accepts only identifiers and customer details; the service determines the amount from the show and releases those same seats on cancellation.

Use the JWT returned by login in the `Authorization: Bearer <token>` header. Catalogue reads are public. Catalogue writes require `Admin` or `TheatreManager`; booking creation requires `Customer`; booking administration and cancellation require `Admin` or `TheatreManager`.
