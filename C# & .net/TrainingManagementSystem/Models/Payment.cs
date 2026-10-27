namespace PaymentModel;

public class Payment
{
    public Payment(string paymentId, string enrollmentId, decimal amount, DateTime paymentDate, PaymentStatus status)
    {
        PaymentId = paymentId;
        EnrollmentId = enrollmentId;
        Amount = amount;
        PaymentDate = paymentDate;
        Status = status;
    }

    public string PaymentId { get; }
    public string EnrollmentId { get; }
    public decimal Amount { get; }
    public DateTime PaymentDate { get; }
    public PaymentStatus Status { get; set; }
}
