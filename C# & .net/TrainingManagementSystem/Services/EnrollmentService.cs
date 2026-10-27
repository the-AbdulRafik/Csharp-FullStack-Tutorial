// Step 4 — Implement Course Enrollment
// Create an EnrollmentService class to coordinate enrollment operations.
// Ask for a learner ID and verify that the learner exists.
// Ask for a course code and verify that the course exists.
// Check whether the learner is already actively enrolled in the same course.
// Check whether the course has available seats.
// Calculate the applicable fee.
// Create and store the enrollment record.
// Increment EnrolledCount only after all checks succeed.
// Display an enrollment confirmation.
// Use loops to search the enrollment list. Cancelled enrollments must not count as active duplicates. Reject invalid requests without changing seat counts.


namespace EnrollmentServicePkg;
using EnrollmentModel;
using LearnerModel;
using CourseModel;
using System;

public class EnrollmentService
{
    private List<Enrollment> enrollments;

    public EnrollmentService()
    {
        enrollments = new List<Enrollment>();
    }

    public bool IsLearnerEnrolled(string learnerId, string courseCode)
    {
        foreach (var enrollment in enrollments)
        {
            if (enrollment.LearnerId == learnerId && enrollment.CourseCode == courseCode && enrollment.Status == Enrollment.EnrollmentStatus.Enrolled)
            {
                return true;
            }
        }
        return false;
    }

    public bool LearnerExists(string learnerId, Dictionary<string, Learner> learners)
    {
        return learners.ContainsKey(learnerId);
    }

    public bool CourseExists(string courseCode, Course[] courses)
    {
        foreach (var course in courses)
        {
            if (course.CourseCode == courseCode)
            {
                return true;
            }
        }
        return false;
    }

    public void AddEnrollment(string learnerId, string courseCode, Course[] courses, Dictionary<string, Learner> learners)
    {
        if (!LearnerExists(learnerId, learners))
        {
            Console.WriteLine($"Learner {learnerId} does not exist. Enrollment not added.");
            return;
        }

        if (!CourseExists(courseCode, courses))
        {
            Console.WriteLine($"Course {courseCode} does not exist. Enrollment not added.");
            return;
        }

        if (IsLearnerEnrolled(learnerId, courseCode))
        {
            Console.WriteLine($"Learner {learnerId} is already enrolled in course {courseCode}. Enrollment not added.");
            return;
        }

        if (!HasAvailableSeats(courseCode, courses))
        {
            Console.WriteLine($"Course {courseCode} has no available seats. Enrollment not added.");
            return;
        }

        // Find the course to get its base fee
        decimal baseFee = 0;
        foreach (var course in courses)
        {
            if (course.CourseCode == courseCode)
            {
                baseFee = course.Fee;
                break;
            }
        }

        // Calculate discount based on some criteria (for example, 10% discount for demonstration)
        decimal discountAmount = baseFee * 0.10m; // 10% discount

        // Create a new enrollment record
        string enrollmentId = $"E{enrollments.Count + 1:D3}";        
        Enrollment enrollment = new Enrollment(enrollmentId, learnerId, courseCode, DateTime.Now, baseFee, discountAmount, Enrollment.EnrollmentStatus.Enrolled);
        enrollments.Add(enrollment);

        // Increment the enrolled count for the course
        foreach (var course in courses)
        {
            if (course.CourseCode == courseCode)
            {
                course.EnrolledCount++;
                break;
            }
        }

        Console.WriteLine($"Enrollment successful for Learner {learnerId} in Course {courseCode}. Final Fee: {enrollment.FinalFee}");
    }
    

    public int GetEnrolledCount(string courseCode)
    {
        int count = 0;
        foreach (var enrollment in enrollments)
        {
            if (enrollment.CourseCode == courseCode && enrollment.Status == Enrollment.EnrollmentStatus.Enrolled)
            {
                count++;
            }
        }
        return count;
    }


    

    public bool HasAvailableSeats(string courseCode, Course[] courses)
    {
        foreach (var course in courses)
        {
            if (course.CourseCode == courseCode)
            {
                return course.EnrolledCount < course.TotalSeats;
            }
        }
        return false;
    }

    
    public List<Enrollment> GetEnrollments()
    {
        return enrollments;
    }

    public void CancelEnrollment(string enrollmentId, Course[] courses)
    {
        
        foreach (var enrollment in enrollments)
        {
            if (enrollment.EnrollmentId == enrollmentId && enrollment.Status == Enrollment.EnrollmentStatus.Enrolled)
            {
                enrollment.Status = Enrollment.EnrollmentStatus.Cancelled;

                // Decrement the enrolled count for the course
                foreach (var course in courses)
                {
                    if (course.CourseCode == enrollment.CourseCode)
                    {
                        course.EnrolledCount--;
                        break;
                    }
                }

                Console.WriteLine($"Enrollment {enrollmentId} has been cancelled.");
                return;
            }

            if (enrollment.EnrollmentId == enrollmentId && enrollment.Status == Enrollment.EnrollmentStatus.Cancelled)
            {
                Console.WriteLine($"Enrollment {enrollmentId} is already cancelled.");
                return;
            }
        }
        Console.WriteLine($"Enrollment {enrollmentId} not found ");
    }



}