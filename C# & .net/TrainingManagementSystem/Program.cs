using CourseModel;
using EnrollmentModel;
using EnrollmentServicePkg;
using FeeCalculatorPkg;
using InputValidatorPkg;
using LearnerModel;
using LearnerServicePkg;
using PaymentServicePkg;
using ReportServicePkg;
using TrainingServicePkg;

Course[] courses =
{
    new Course("CS101", "Introduction to Computer Science", 500m, 30, 50),
    new Course("CS102", "Data Structures and Algorithms", 600m, 45, 40),
    new Course("CS103", "Database Management Systems", 550m, 35, 45),
    new Course("CS104", "Web Development", 700m, 40, 30),
    new Course("CS105", "Machine Learning", 800m, 50, 25)
};

Learner[] initialLearners =
{
    new Learner("L001", "Alice Smith", "123-456-7890", "alice.smith@example.com"),
    new Learner("L002", "Bob Johnson", "987-654-3210", "bob.johnson@example.com")
};

TrainingService trainingService = new TrainingService(courses);
LearnerService learnerService = new LearnerService(initialLearners);
EnrollmentService enrollmentService = new EnrollmentService();
PaymentService paymentService = new PaymentService();
ReportService reportService = new ReportService();
FeeCalculator feeCalculator = new FeeCalculator();

bool isRunning = true;
while (isRunning)
{
    DisplayMenu();
    Console.Write("Choose an option: ");
    string option = Console.ReadLine() ?? string.Empty;

    switch (option)
    {
        case "1":
            trainingService.DisplayCourses();
            break;
        case "2":
            SearchCourse();
            break;
        case "3":
            SortCourses();
            break;
        case "4":
            learnerService.DisplayLearners();
            break;
        case "5":
            RegisterLearner();
            break;
        case "6":
            AddEnrollment();
            break;
        case "7":
            MakePayment();
            break;
        case "8":
            CalculateFee();
            break;
        case "9":
            DisplayReports();
            break;
        case "0":
            isRunning = false;
            Console.WriteLine("Application closed.");
            break;
        default:
            Console.WriteLine("Invalid option. Please choose a number from the menu.");
            break;
    }
}

void DisplayMenu()
{
    Console.WriteLine();
    Console.WriteLine("===== Training Management System =====");
    Console.WriteLine("1. Display all courses");
    Console.WriteLine("2. Search for a course");
    Console.WriteLine("3. Sort courses by fee");
    Console.WriteLine("4. Display learners");
    Console.WriteLine("5. Register a learner");
    Console.WriteLine("6. Enroll a learner");
    Console.WriteLine("7. Make a payment");
    Console.WriteLine("8. Calculate a fee");
    Console.WriteLine("9. Display reports");
    Console.WriteLine("0. Exit");
}

void SearchCourse()
{
    Console.Write("Enter a course code or part of a title: ");
    string searchText = Console.ReadLine() ?? string.Empty;

    Course? course = trainingService.FindByCode(searchText);
    if (course != null)
    {
        Console.WriteLine($"{course.CourseCode} - {course.Title} - Fee: {course.Fee:C}");
        return;
    }

    List<Course> matches = trainingService.SearchByTitle(searchText);
    if (matches.Count == 0)
    {
        Console.WriteLine("No matching course was found.");
        return;
    }

    foreach (Course matchingCourse in matches)
    {
        Console.WriteLine($"{matchingCourse.CourseCode} - {matchingCourse.Title} - Fee: {matchingCourse.Fee:C}");
    }
}

void SortCourses()
{
    Console.Write("Sort ascending or descending? (A/D): ");
    string answer = Console.ReadLine() ?? string.Empty;
    bool descending = answer.Equals("D", StringComparison.OrdinalIgnoreCase);

    List<Course> sortedCourses = trainingService.SortByFee(descending);
    foreach (Course course in sortedCourses)
    {
        Console.WriteLine($"{course.CourseCode} - {course.Title} - {course.Fee:C}");
    }
}

void RegisterLearner()
{
    Console.Write("Learner ID, for example L003: ");
    string learnerIdInput = Console.ReadLine() ?? string.Empty;

    if (!InputValidator.TryReadLearnerId(learnerIdInput, out string learnerId))
    {
        Console.WriteLine("Invalid learner ID. Use a format such as L003.");
        return;
    }

    Console.Write("Name: ");
    string name = Console.ReadLine() ?? string.Empty;
    Console.Write("Contact number: ");
    string contactNumber = Console.ReadLine() ?? string.Empty;
    Console.Write("Email: ");
    string email = Console.ReadLine() ?? string.Empty;

    try
    {
        learnerService.Register(new Learner(learnerId, name, contactNumber, email));
        Console.WriteLine("Learner registered successfully.");
    }
    catch (Exception exception) when (exception is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine($"Learner could not be registered: {exception.Message}");
    }
}

void AddEnrollment()
{
    Console.Write("Learner ID: ");
    string learnerId = Console.ReadLine() ?? string.Empty;
    Console.Write("Course code: ");
    string courseCode = Console.ReadLine() ?? string.Empty;

    enrollmentService.AddEnrollment(learnerId, courseCode, courses, learnerService.Learners);
}

void MakePayment()
{
    Console.Write("Enrollment ID: ");
    string enrollmentId = Console.ReadLine() ?? string.Empty;
    Enrollment? selectedEnrollment = null;

    foreach (Enrollment enrollment in enrollmentService.GetEnrollments())
    {
        if (enrollment.EnrollmentId.Equals(enrollmentId, StringComparison.OrdinalIgnoreCase))
        {
            selectedEnrollment = enrollment;
            break;
        }
    }

    if (selectedEnrollment == null)
    {
        Console.WriteLine("Enrollment was not found.");
        return;
    }

    Console.Write("Is this an online payment? (Y/N): ");
    string paymentType = Console.ReadLine() ?? string.Empty;
    bool isOnline = paymentType.Equals("Y", StringComparison.OrdinalIgnoreCase);

    try
    {
        PaymentModel.Payment payment = paymentService.Process(
            selectedEnrollment.EnrollmentId,
            selectedEnrollment.FinalFee,
            isOnline);
        Console.WriteLine($"Payment completed. Payment ID: {payment.PaymentId}");
        Console.WriteLine($"Status: {payment.Status}, Amount: {payment.Amount:C}");
    }
    catch (ArgumentException exception)
    {
        Console.WriteLine($"Payment failed: {exception.Message}");
    }
}

void CalculateFee()
{
    Console.Write("Enter base fee: ");
    if (!InputValidator.TryReadDecimal(Console.ReadLine(), out decimal baseFee))
    {
        Console.WriteLine("Please enter a valid decimal number.");
        return;
    }

    Console.Write("Session type (O for online, C for classroom): ");
    string sessionInput = Console.ReadLine() ?? string.Empty;
    SessionType sessionType = sessionInput.Equals("C", StringComparison.OrdinalIgnoreCase)
        ? SessionType.Classroom
        : SessionType.Online;

    Console.Write("Apply early-bird discount? (Y/N): ");
    bool earlyBird = (Console.ReadLine() ?? string.Empty).Equals("Y", StringComparison.OrdinalIgnoreCase);
    Console.Write("Apply group discount? (Y/N): ");
    bool group = (Console.ReadLine() ?? string.Empty).Equals("Y", StringComparison.OrdinalIgnoreCase);

    try
    {
        (decimal BaseFee, decimal TotalDiscount, decimal FinalFee) result =
            feeCalculator.CalculateFee(baseFee, sessionType, earlyBird, group);
        Console.WriteLine($"Base fee: {result.BaseFee:C}");
        Console.WriteLine($"Total discount: {result.TotalDiscount:C}");
        Console.WriteLine($"Final fee: {result.FinalFee:C}");
    }
    catch (ArgumentOutOfRangeException exception)
    {
        Console.WriteLine($"Fee could not be calculated: {exception.Message}");
    }
}

void DisplayReports()
{
    reportService.DisplayCourseOccupancyReport(courses);
    reportService.DisplayExpectedFeesReport(courses, enrollmentService.GetEnrollments());
    reportService.DisplayPaymentSummaryReport(paymentService.Payments);
    reportService.DisplayCourseEnrollmentSummaryReport(courses, enrollmentService.GetEnrollments());
}
