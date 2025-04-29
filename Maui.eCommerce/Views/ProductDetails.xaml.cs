using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.Views;

public partial class ProductDetails : ContentPage
{
    public ProductDetails()
    {
        InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object? sender, NavigatedToEventArgs e)
    {
        //throw new NotImplementedException();
    }

    private void OkClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//InventoryManagement");
    }

    private void GoBackClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//InventoryManagement");
    }
}