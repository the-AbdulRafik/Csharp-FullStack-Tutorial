namespace ExpenseTracker.Api.Extensions;

public sealed class ApiNotFoundException(string message) : Exception(message);

public sealed class ApiConflictException(string message) : Exception(message);

public sealed class ApiBadRequestException(string message) : Exception(message);
