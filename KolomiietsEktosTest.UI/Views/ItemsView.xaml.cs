using KolomiietsEktosTest.UI.ViewModels;

namespace KolomiietsEktosTest.UI.Views;

public partial class ItemsView : ContentPage
{
    private readonly ItemsViewModel _viewModel;
    public ItemsView(ItemsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.LoadItemsCommand.ExecuteAsync(null);
    }
}