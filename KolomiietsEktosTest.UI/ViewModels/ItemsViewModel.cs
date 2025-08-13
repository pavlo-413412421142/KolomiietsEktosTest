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
    private bool isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasItems))]
    [NotifyPropertyChangedFor(nameof(CanLoadItems))]
    private bool isEmpty;

    public bool HasItems => !IsEmpty;

    public bool CanLoadItems => !IsBusy;

    public ObservableCollection<HeaderParsed> Items { get; } = new();

    public IAsyncRelayCommand LoadItemsCommand { get; }

    public ItemsViewModel(DataService dataService)
    {
        _dataService = dataService;
        LoadItemsCommand = new AsyncRelayCommand(LoadItemsAsync);
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
                MainThread.BeginInvokeOnMainThread(() => Items.Add(parsed));
            }

            IsEmpty = Items.Count == 0;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Failed to load items: " + ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
