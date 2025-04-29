using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.eCommerce.Models;
using Library.eCommerce.Services;

namespace Maui.eCommerce.ViewModels;

public class ShoppingManagementViewModel : INotifyPropertyChanged, IDisposable
{
    private ShoppingCartService _cartService = ShoppingCartService.Current;
    private InventoryServiceProxy _invSvc = InventoryServiceProxy.Current;
    private ItemViewModel _selectedItem;
    private ObservableCollection<ItemViewModel> _inventory;
    private ItemViewModel? _selectedCartItem;
    private ObservableCollection<ItemViewModel> _shoppingCart;
    private string _quantityToAdd = "1";
    public string QuantityToAdd
    {
        get => _quantityToAdd;
        set
        {
            if (_quantityToAdd != value)
            {
                _quantityToAdd = value;
                OnPropertyChanged();
            }
        }
    }

    public ShoppingManagementViewModel()
    {
        LoadInventory();
        LoadShoppingCart();
        _invSvc.InventoryChanged += OnInventoryChanged;
    }

    private void LoadInventory()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Inventory = new ObservableCollection<ItemViewModel>(
                _invSvc.Inventory.Select(item => new ItemViewModel(item)));
            OnPropertyChanged(nameof(Inventory));
        });
    }

    private void LoadShoppingCart()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ShoppingCart = new ObservableCollection<ItemViewModel>(
                _cartService.CartItems.Select(item => new ItemViewModel(item)));
            OnPropertyChanged(nameof(ShoppingCart));
        });
    }

    private void OnInventoryChanged(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LoadInventory();
        });
    }

    public ItemViewModel? SelectedCartItem
    {
        get => _selectedCartItem;
        set
        {
            if (_selectedCartItem != value)
            {
                _selectedCartItem = value;
                System.Diagnostics.Debug.WriteLine($"SelectedCartItem changed to: {value?.Model?.Product?.Name ?? "null"}");
                OnPropertyChanged();
            }
        }
    }

    public ItemViewModel SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value)
            {
                _selectedItem = value;
                System.Diagnostics.Debug.WriteLine($"SelectedItem changed to: {value?.Model?.Product?.Name ?? "null"}");
                OnPropertyChanged();
            }  
        } 
    }

    public ObservableCollection<ItemViewModel> Inventory
    {
        get => _inventory ??= new ObservableCollection<ItemViewModel>(
            _invSvc.Inventory.Select(item => new ItemViewModel(item)));
        private set
        {
            if (_inventory != value)
            {
                _inventory = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<ItemViewModel> ShoppingCart
    {
        get => _shoppingCart ??= new ObservableCollection<ItemViewModel>(
            _cartService.CartItems.Select(item => new ItemViewModel(item)));
        private set
        {
            if (_shoppingCart != value)
            {
                _shoppingCart = value;
                OnPropertyChanged();
            }
        }
    }

    public void PurchaseItem()
    {
        try
        {
            if (SelectedItem != null && 
                int.TryParse(QuantityToAdd, out int quantity) && 
                quantity > 0)
            {
                for (int i = 0; i < quantity && SelectedItem.Model.Quantity > 0; i++)
                {
                    _cartService.AddOrUpdate(SelectedItem.Model);
                }
                
                LoadShoppingCart();
                LoadInventory();
                // Reset quantity to add
                QuantityToAdd = "1";
                
                // Force refresh of both collections
                OnPropertyChanged(nameof(Inventory));
                OnPropertyChanged(nameof(ShoppingCart));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in PurchaseItem: {ex.Message}");
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Application.Current?.MainPage?.DisplayAlert("Error", ex.Message, "OK");
            });
        }
    }
    public void RefreshDisplays()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LoadInventory();
            LoadShoppingCart();
            OnPropertyChanged(nameof(Inventory));
            OnPropertyChanged(nameof(ShoppingCart));
        });
    }

    public void ReturnItem()
    {
        try
        {
            if (SelectedCartItem != null)
            {
                _cartService.ReturnItem(SelectedCartItem.Model);
                LoadShoppingCart();
                LoadInventory();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in ReturnItem: {ex.Message}");
            // Handle or propagate the error as needed
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Dispose()
    {
        _invSvc.InventoryChanged -= OnInventoryChanged;
    }
}