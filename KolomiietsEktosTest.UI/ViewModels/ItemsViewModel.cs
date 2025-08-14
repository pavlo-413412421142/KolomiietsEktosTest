using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KolomiietsEktosTest.UI.Models;
using KolomiietsEktosTest.UI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KolomiietsEktosTest.UI.ViewModels;

public partial class ItemsViewModel : ObservableObject
{
    private readonly DataService _dataService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanLoadItems))]
    private bool isBusy = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasItems))]
    [NotifyPropertyChangedFor(nameof(CanLoadItems))]
    private bool isEmpty = true;

    [ObservableProperty]
    private bool isMetric = true;

    public string UnitSystemButtonText => IsMetric ? "Switch to Imperial" : "Switch to Metric";
    public bool HasItems => !IsEmpty;
    public bool CanLoadItems => !IsBusy;

    public ObservableCollection<HeaderParsed> Items { get; } = new();

    public IAsyncRelayCommand LoadItemsCommand { get; }
    public IRelayCommand ToggleUnitSystemCommand { get; }

    public ItemsViewModel(DataService dataService)
    {
        _dataService = dataService;

        LoadItemsCommand = new AsyncRelayCommand(LoadItemsAsync);
        ToggleUnitSystemCommand = new RelayCommand(ToggleUnitSystem);
    }

    private async Task LoadItemsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            Items.Clear();

            var itemsFromDb = await _dataService.GetAllItemsAsync();

            foreach (var dbItem in itemsFromDb)
            {
                var parsed = HeaderParsed.ParseFromBinary(dbItem.Header);
                parsed.IsMetric = this.IsMetric;
                Items.Add(parsed);
            }

            IsEmpty = Items.Count == 0;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Failed to load items: " + ex.Message);
            IsEmpty = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ToggleUnitSystem()
    {
        if (Items.Count == 0)
            return;

        IsMetric = !IsMetric;

        foreach (var item in Items)
        {
            item.IsMetric = IsMetric;
            item.NotifyDisplayPropertiesChanged();
        }

        OnPropertyChanged(nameof(UnitSystemButtonText));
    }
}
