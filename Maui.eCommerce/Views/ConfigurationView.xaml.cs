using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class ConfigurationView : ContentPage
{
    public ConfigurationView()
    {
        InitializeComponent();
        BindingContext = new ConfigurationViewModel();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}