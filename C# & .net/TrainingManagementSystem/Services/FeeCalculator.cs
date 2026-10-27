namespace FeeCalculatorPkg;

public enum SessionType
{
    Online,
    Classroom
}

public struct FeeSummary
{
    public FeeSummary(decimal baseFee, decimal totalDiscount, decimal finalFee)
    {
        BaseFee = baseFee;
        TotalDiscount = totalDiscount;
        FinalFee = finalFee;
    }

    public decimal BaseFee { get; }
    public decimal TotalDiscount { get; }
    public decimal FinalFee { get; }
}

public class FeeCalculator
{
    private const decimal OnlineSurcharge = 200m;
    private const decimal ClassroomSurcharge = 800m;

    // Calculates the fee when no discounts apply.
    public (decimal BaseFee, decimal TotalDiscount, decimal FinalFee) CalculateFee(
        decimal baseFee,
        SessionType sessionType)
        
    {
        ValidateBaseFee(baseFee);

        decimal finalFee = baseFee + GetSessionSurcharge(sessionType);
        return (baseFee, 0m, finalFee);
    }

    // Calculates both discounts from the original base fee, so discounts are not compounded.
    public (decimal BaseFee, decimal TotalDiscount, decimal FinalFee) CalculateFee(
        decimal baseFee,
        SessionType sessionType,
        decimal firstDiscountPercent,
        decimal secondDiscountPercent)
    {
        ValidateBaseFee(baseFee);
        ValidateDiscount(firstDiscountPercent, nameof(firstDiscountPercent));
        ValidateDiscount(secondDiscountPercent, nameof(secondDiscountPercent));

        decimal firstDiscount = baseFee * firstDiscountPercent / 100m;
        decimal secondDiscount = baseFee * secondDiscountPercent / 100m;
        decimal totalDiscount = firstDiscount + secondDiscount;
        decimal discountedFee = baseFee - totalDiscount;
        decimal finalFee = discountedFee + GetSessionSurcharge(sessionType);

        return (baseFee, totalDiscount, finalFee);
    }

    public (decimal BaseFee, decimal TotalDiscount, decimal FinalFee) CalculateFee(
        decimal baseFee,
        SessionType sessionType,
        bool earlyBirdDiscount,
        bool groupDiscount)
    {
        decimal earlyBirdPercent = earlyBirdDiscount ? 10m : 0m;
        decimal groupPercent = groupDiscount ? 5m : 0m;
        return CalculateFee(baseFee, sessionType, earlyBirdPercent, groupPercent);
    }

    public void DisplayFeeBreakdown(FeeSummary summary, SessionType sessionType)
    {
        Console.WriteLine("Fee breakdown");
        Console.WriteLine($"Base fee:       {summary.BaseFee:C}");
        Console.WriteLine($"Total discount: {summary.TotalDiscount:C}");
        Console.WriteLine($"Session fee:    {GetSessionSurcharge(sessionType):C}");
        Console.WriteLine($"Final fee:      {summary.FinalFee:C}");
    }

    public static void IncrementEnrolledCount(ref int enrolledCount, int totalSeats)
    {
        if (enrolledCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(enrolledCount), "Enrolled count cannot be negative.");
        }

        if (totalSeats < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalSeats), "Total seats cannot be negative.");
        }

        if (enrolledCount >= totalSeats)
        {
            throw new InvalidOperationException("The course has no available seats.");
        }

        enrolledCount++;
    }

    public static bool TryParseLearnerId(string? input, out string learnerId)
    {
        if (!string.IsNullOrWhiteSpace(input))
        {
            string trimmedId = input.Trim();
            if (trimmedId.StartsWith("L", StringComparison.OrdinalIgnoreCase)
                && trimmedId.Length > 1
                && int.TryParse(trimmedId[1..], out int numericPart)
                && numericPart > 0)
            {
                learnerId = $"L{numericPart:D3}";
                return true;
            }
        }

        learnerId = string.Empty;
        return false;
    }

    private static decimal GetSessionSurcharge(SessionType sessionType)
    {
        return sessionType switch
        {
            SessionType.Online => OnlineSurcharge,
            SessionType.Classroom => ClassroomSurcharge,
            _ => throw new ArgumentOutOfRangeException(nameof(sessionType), "Unknown session type.")
        };
    }

    private static void ValidateBaseFee(decimal baseFee)
    {
        if (baseFee <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(baseFee), "Base fee must be greater than zero.");
        }
    }

    private static void ValidateDiscount(decimal discountPercent, string parameterName)
    {
        if (discountPercent < 0m || discountPercent > 100m)
        {
            throw new ArgumentOutOfRangeException(
                parameterName,
                "Discount percentage must be between 0 and 100.");
        }
    }
}