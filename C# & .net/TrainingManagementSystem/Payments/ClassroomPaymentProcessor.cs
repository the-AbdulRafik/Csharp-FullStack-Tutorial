using PaymentModel;

namespace PaymentProcessorPkg;

public class ClassroomPaymentProcessor : IPaymentProcessor
{
    Payment IPaymentProcessor.ProcessPayment(string enrollmentId, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(enrollmentId) || amount <= 0m)
        {
            throw new ArgumentException("A valid enrollment ID and positive amount are required.");
        }

        return new Payment($"P-{Guid.NewGuid():N}", enrollmentId, amount, DateTime.Now, PaymentStatus.Successful);
    }
}
