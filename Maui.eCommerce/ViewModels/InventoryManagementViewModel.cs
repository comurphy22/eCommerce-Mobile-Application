using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.eCommerce.ViewModels;

public class InventoryManagementViewModel : INotifyPropertyChanged
{
    private InventoryServiceProxy _svc = InventoryServiceProxy.Current;
    public string? Query { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<Item?> Inventory
    {
        get
        {
            // If Query is empty or null, show all items
            if (string.IsNullOrWhiteSpace(Query))
            {
                return new ObservableCollection<Item?>(_svc.Inventory);
            }

            // Otherwise, filter by name but don't exclude based on quantity
            var filteredList = _svc.Inventory.Where(p => 
                p?.Name?.ToLower().Contains(Query.ToLower()) ?? false);
            return new ObservableCollection<Item?>(filteredList);
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
