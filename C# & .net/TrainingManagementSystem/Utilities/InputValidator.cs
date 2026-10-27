namespace InputValidatorPkg;

public static class InputValidator
{
    public static bool TryReadInt(string? input, out int value)
    {
        return int.TryParse(input, out value);
    }

    public static bool TryReadDecimal(string? input, out decimal value)
    {
        return decimal.TryParse(input, out value);
    }

    public static bool TryReadLearnerId(string? input, out string learnerId)
    {
        return FeeCalculatorPkg.FeeCalculator.TryParseLearnerId(input, out learnerId);
    }

    public static bool HasText(string? input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }
}
