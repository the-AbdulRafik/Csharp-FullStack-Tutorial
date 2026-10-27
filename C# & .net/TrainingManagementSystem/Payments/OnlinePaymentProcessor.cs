using PaymentModel;

namespace PaymentProcessorPkg;

public class OnlinePaymentProcessor : IPaymentProcessor
{
    public Payment ProcessPayment(string enrollmentId, decimal amount)
    {
        ValidatePayment(enrollmentId, amount);
        return new Payment($"P-{Guid.NewGuid():N}", enrollmentId, amount, DateTime.Now, PaymentStatus.Successful);
    }

    private static void ValidatePayment(string enrollmentId, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(enrollmentId) || amount <= 0m)
        {
            throw new ArgumentException("A valid enrollment ID and positive amount are required.");
        }
    }
}
