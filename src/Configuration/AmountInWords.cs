using System.Text;

namespace Students.Service.src.Configuration;

public sealed class AmountInWords
{
    private static readonly string[] Units =
    [
        "", "One", "Two", "Three", "Four",
        "Five", "Six", "Seven", "Eight", "Nine"
    ];

    private static readonly string[] TenToNineteen =
    [
        "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen",
        "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
    ];

    private static readonly string[] Tens =
    [
        "", "", "Twenty", "Thirty", "Forty",
        "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
    ];

    private static readonly string[] Hundreds =
    [
        "", "One Hundred", "Two Hundred", "Three Hundred",
        "Four Hundred", "Five Hundred", "Six Hundred",
        "Seven Hundred", "Eight Hundred", "Nine Hundred"
    ];

    public static string ConvertToWords(decimal number)
    {
        if (number == 0)
            return "Zero Metical";

        if (number > 999_999_999.99m)
            throw new InvalidOperationException(
                "Limit reached. Please contact the administrator!"
            );

        int integerPart = (int)Math.Floor(number);
        int decimalPart = (int)Math.Round((number - integerPart) * 100);

        var parts = new List<string>();

        int millions = integerPart / 1_000_000;
        int thousands = integerPart % 1_000_000 / 1_000;
        int hundreds = integerPart % 1_000;

        if (millions > 0)
        {
            parts.Add($"{ConvertPart(millions)} {(millions == 1 ? "Million" : "Millions")}");
        }

        if (thousands > 0)
        {
            if (thousands == 1)
                parts.Add("One Thousand");
            else
                parts.Add($"{ConvertPart(thousands)} Thousand");
        }

        if (hundreds > 0)
        {
            parts.Add(ConvertPart(hundreds));
        }

        var result = new StringBuilder();
        result.Append(string.Join(" and ", parts));

        if (integerPart > 0)
            result.Append(integerPart == 1 ? " Metical" : " Meticais");

        if (decimalPart > 0)
        {
            if (integerPart > 0)
                result.Append(" and ");

            result.Append(ConvertPart(decimalPart));
            result.Append(decimalPart == 1 ? " Penny" : " Pence");
        }

        return result.ToString();
    }

    private static string ConvertPart(int number)
    {
        var words = new StringBuilder();

        if (number >= 100)
        {
            int hundred = number / 100;
            words.Append(Hundreds[hundred]);

            number %= 100;

            if (number > 0)
                words.Append(" and ");
        }

        if (number >= 10 && number < 20)
        {
            words.Append(TenToNineteen[number - 10]);
        }
        else
        {
            if (number >= 20)
            {
                int ten = number / 10;
                words.Append(Tens[ten]);

                number %= 10;

                if (number > 0)
                    words.Append(" and ");
            }

            if (number > 0)
                words.Append(Units[number]);
        }

        return words.ToString();
    }
}
