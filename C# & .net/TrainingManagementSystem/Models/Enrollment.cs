namespace EnrollmentModel;


// EnrollmentId
// LearnerId
// CourseCode
// EnrollmentDate
// BaseFee
// DiscountAmount
// FinalFee
// Status

// Create an EnrollmentStatus enum containing Active, Cancelled, and Completed.
// Store learner and course identifiers in each enrollment.
// Keep cancelled records for history instead of deleting them.
// Collection: List<Enrollment>, because enrollment records grow dynamically.
// Practise Add(), Remove() for suitable cases, Count, indexing, and loops. Use loops to find records; do not use LINQ.


public class Enrollment
{
    public enum EnrollmentStatus
    {
        Enrolled,
        Completed,
        Cancelled
    }
    public string EnrollmentId { get; set; }
    public string LearnerId { get; set; }
    public string CourseCode { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public decimal BaseFee { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalFee { get; set; }
    public EnrollmentStatus Status { get; set; }


    public Enrollment(string enrollmentId, string learnerId, string courseCode, DateTime enrollmentDate, decimal baseFee, decimal discountAmount, EnrollmentStatus status)
    {
        EnrollmentId = enrollmentId;
        LearnerId = learnerId;
        CourseCode = courseCode;
        EnrollmentDate = enrollmentDate;
        BaseFee = baseFee;
        DiscountAmount = discountAmount;
        FinalFee = baseFee - discountAmount; // Calculate final fee based on base fee and discount
        Status = status;
    }

}