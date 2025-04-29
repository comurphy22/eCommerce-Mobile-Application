using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.eCommerce.Models;
using Library.eCommerce.Services;

namespace Maui.eCommerce.ViewModels;

public enum SortType
{
    Name,
    Price
}

public class ShoppingManagementViewModel : INotifyPropertyChanged, IDisposable
{
    private readonly ShoppingCartService _cartService = ShoppingCartService.Current;
    private readonly InventoryServiceProxy _invSvc = InventoryServiceProxy.Current;
    private ItemViewModel _selectedItem;
    private ObservableCollection<ItemViewModel> _inventory;
    private ItemViewModel _selectedCartItem;
    private ObservableCollection<ItemViewModel> _shoppingCart;
    private string _quantityToAdd = "1";
    private SortType _sortBy = SortType.Name;
    private bool _sortAscending = true;

    public ShoppingManagementViewModel()
    {
        LoadInventory();
        LoadShoppingCart();
        _invSvc.InventoryChanged += OnInventoryChanged;
    }

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

    public SortType SortBy
    {
        get => _sortBy;
        set
        {
            if (_sortBy != value)
            {
                _sortBy = value;
                OnPropertyChanged();
                SortShoppingCart();
            }
        }
    }

    public bool SortAscending
    {
        get => _sortAscending;
        set
        {
            if (_sortAscending != value)
            {
                _sortAscending = value;
                OnPropertyChanged();
                SortShoppingCart();
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
                OnPropertyChanged();
            }
        }
    }

    public ItemViewModel SelectedCartItem
    {
        get => _selectedCartItem;
        set
        {
            if (_selectedCartItem != value)
            {
                _selectedCartItem = value;
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

    public decimal Subtotal => ShoppingCart?.Sum(item => item.Model.Product.Price * item.Model.Quantity) ?? 0m;
    public decimal TaxAmount => TaxCalculator.CalculateTax(Subtotal);
    public decimal Total => Subtotal + TaxAmount;
    public string TaxRate => $"{TaxCalculator.GetTaxRate():F2}%";

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
        ShoppingCart = new ObservableCollection<ItemViewModel>(_cartService.CartItems
            .Select(item => new ItemViewModel(item)));
        SortShoppingCart();
        UpdateTotals();
    }

    private void UpdateTotals()
    {
        OnPropertyChanged(nameof(Subtotal));
        OnPropertyChanged(nameof(TaxAmount));
        OnPropertyChanged(nameof(Total));
        OnPropertyChanged(nameof(TaxRate));
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
                QuantityToAdd = "1";
                
                OnPropertyChanged(nameof(Inventory));
                OnPropertyChanged(nameof(ShoppingCart));
                UpdateTotals();
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

    public void ReturnItem()
    {
        try
        {
            if (SelectedCartItem != null)
            {
                _cartService.ReturnItem(SelectedCartItem.Model);
                LoadShoppingCart();
                LoadInventory();
                
                OnPropertyChanged(nameof(Inventory));
                OnPropertyChanged(nameof(ShoppingCart));
                UpdateTotals();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in ReturnItem: {ex.Message}");
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Application.Current?.MainPage?.DisplayAlert("Error", ex.Message, "OK");
            });
        }
    }

    private void SortShoppingCart()
    {
        var sortedItems = _sortBy switch
        {
            SortType.Name when _sortAscending => 
                ShoppingCart.OrderBy(x => x.Model.Name),
            SortType.Name => 
                ShoppingCart.OrderByDescending(x => x.Model.Name),
            SortType.Price when _sortAscending => 
                ShoppingCart.OrderBy(x => x.Model.Product.Price),
            SortType.Price => 
                ShoppingCart.OrderByDescending(x => x.Model.Product.Price),
            _ => ShoppingCart.AsEnumerable()
        };

        ShoppingCart = new ObservableCollection<ItemViewModel>(sortedItems);
    }

    public void RefreshDisplays()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LoadInventory();
            LoadShoppingCart();
            OnPropertyChanged(nameof(Inventory));
            OnPropertyChanged(nameof(ShoppingCart));
            UpdateTotals();
        });
    }

    private void OnInventoryChanged(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LoadInventory();
            UpdateTotals();
        });
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Dispose()
    {
        _invSvc.InventoryChanged -= OnInventoryChanged;
    }
}