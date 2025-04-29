using Maui.eCommerce.ViewModels;
using Maui.eCommerce.Views;
using Microsoft.Maui.Controls;

namespace Maui.eCommerce;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel(); 
    }


    private void InventoryClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//InventoryManagement"); //routing, follows singleton
    }

    private void ShopClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//ShoppingCart"); //routing, follows singleton
    }

    private async void OnConfigurationClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new ConfigurationView());
    }
}