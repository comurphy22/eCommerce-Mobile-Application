using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.eCommerce.ViewModels;

public class InventoryManagementViewModel : INotifyPropertyChanged
{
    private InventoryServiceProxy _svc = InventoryServiceProxy.Current;
    private SortType _sortBy = SortType.Name;
    private bool _sortAscending = true;
    private string? _query;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string? Query
    {
        get => _query;
        set
        {
            if (_query != value)
            {
                _query = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Inventory));
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
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Inventory));
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
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Inventory));
            }
        }
    }

    public ObservableCollection<Item?> Inventory
    {
        get
        {
            var items = string.IsNullOrWhiteSpace(Query)
                ? _svc.Inventory
                : _svc.Inventory.Where(p => p?.Name?.ToLower().Contains(Query.ToLower()) ?? false);

            var sortedItems = _sortBy switch
            {
                SortType.Name when _sortAscending =>
                    items.OrderBy(x => x?.Name),
                SortType.Name =>
                    items.OrderByDescending(x => x?.Name),
                SortType.Price when _sortAscending =>
                    items.OrderBy(x => x?.Product.Price),
                SortType.Price =>
                    items.OrderByDescending(x => x?.Product.Price),
                _ => items
            };

            return new ObservableCollection<Item?>(sortedItems);
        }
    }

    public Item? SelectedItem { get; set; }

    public Item? Add()
    {
        return null;
    }

    public Item? Delete()
    {
        if (SelectedItem is null)
        {
            return null;
        }
        
        var item = _svc.Delete(SelectedItem?.Id ?? 0);
        NotifyPropertyChanged(nameof(Inventory));
        return item;
    }

    public void RefreshProductList()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            NotifyPropertyChanged(nameof(Inventory));
        });
    }

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        if (propertyName is null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }
        
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}