using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maui.eCommerce.ViewModels;

public class ConfigurationViewModel
{
    private string _taxRate;
    private const string TAX_RATE_KEY = "TaxRate";  
    public ConfigurationViewModel()
    {
        LoadConfiguration();
        SaveCommand = new Command(ExecuteSave);
    }
    public string TaxRate
    {
        get => _taxRate;
        set
        {
            if (_taxRate != value)
            {
                _taxRate = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SaveCommand { get; }

    private void LoadConfiguration()
    {
        TaxRate = Preferences.Get(TAX_RATE_KEY, "0.0");
    }

    private async void ExecuteSave()
    {
        if (ValidateTaxRate())
        {
            if (decimal.TryParse(TaxRate, out decimal rate))
            {
                Preferences.Set(TAX_RATE_KEY, TaxRate);
                TaxCalculator.SetTaxRate(rate); // Add this line to update TaxCalculator
                await Application.Current.MainPage.DisplayAlert("Success", "Tax rate has been saved.", "OK");
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid tax rate (0-100).", "OK");
        }
    }

    private bool ValidateTaxRate()
    {
        if (decimal.TryParse(TaxRate, out decimal rate))
        {
            return rate >= 0 && rate <= 100;
        }
        return false;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
