using Microsoft.Data.SqlClient;

namespace Sample_Example.Helpers;

public static class DatabaseHelper
{
    private const string ConnectionString =
        "Data Source=.\\SQLEXPRESS;Initial Catalog=StudentCourseDB;Integrated Security=True;TrustServerCertificate=True;";

    public static SqlConnection GetConnection() => new(ConnectionString);
}
