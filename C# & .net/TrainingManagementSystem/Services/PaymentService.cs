using PaymentModel;
using PaymentProcessorPkg;

namespace PaymentServicePkg;

public class PaymentService
{
    private List<Payment> payments = new List<Payment>();

    public List<Payment> Payments => payments;

    public Payment Process(string enrollmentId, decimal amount, bool online)
    {
        IPaymentProcessor processor = online
            ? new OnlinePaymentProcessor()
            : new ClassroomPaymentProcessor();
        Payment payment = online
            ? processor.ProcessPayment(enrollmentId, amount)
            : ProcessClassroomPayment(processor, enrollmentId, amount);
        payments.Add(payment);
        return payment;
    }

    private static Payment ProcessClassroomPayment(IPaymentProcessor processor, string enrollmentId, decimal amount)
    {
        return processor.ProcessPayment(enrollmentId, amount);
    }
}
