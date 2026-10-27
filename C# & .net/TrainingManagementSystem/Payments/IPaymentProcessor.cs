using PaymentModel;

namespace PaymentProcessorPkg;

public interface IPaymentProcessor
{
    Payment ProcessPayment(string enrollmentId, decimal amount);
}
