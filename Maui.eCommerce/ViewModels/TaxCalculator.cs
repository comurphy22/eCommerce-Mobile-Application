namespace Maui.eCommerce.ViewModels;

public static class TaxCalculator
{
    private const string TAX_RATE_KEY = "TaxRate";

    public static decimal CalculateTax(decimal amount)
    {
        string taxRateStr = Preferences.Get(TAX_RATE_KEY, "0.0");
        if (decimal.TryParse(taxRateStr, out decimal taxRate))
        {
            return amount * (taxRate / 100m);
        }
        return 0m;
    }

    public static decimal GetTaxRate()
    {
        string taxRateStr = Preferences.Get(TAX_RATE_KEY, "0.0");
        if (decimal.TryParse(taxRateStr, out decimal taxRate))
        {
            return taxRate;
        }
        return 0m;
    }
}