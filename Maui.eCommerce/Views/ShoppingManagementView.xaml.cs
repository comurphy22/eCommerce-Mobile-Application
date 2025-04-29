using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class ShoppingManagementView : ContentPage
{
    
    private readonly ShoppingManagementViewModel _viewModel;
    public ShoppingManagementView()
    {
        InitializeComponent();
        _viewModel = new ShoppingManagementViewModel();
        BindingContext = _viewModel;
    }

    private void RemoveFromCartClicked(object sender, EventArgs e)
    {   
        //check if quantity becomes 0, if so delete
        (BindingContext as ShoppingManagementViewModel).ReturnItem();
    }

    private void AddToCartClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel).PurchaseItem();   
    }

    private void InlineAddClicked(object sender, EventArgs e)
    {
        //(BindingContext as ShoppingManagementViewModel).RefreshUX();
    }
    
    private void OnBackButtonClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }
}