using Microsoft.UI.Xaml.Controls;

using ViewModelBinding.ViewModels;

namespace ViewModelBinding.Views;

public sealed partial class EmptyPage : Page
{
    public EmptyViewModel ViewModel
    {
        get;
    }

    public EmptyPage()
    {
        ViewModel = App.GetService<EmptyViewModel>();
        InitializeComponent();
    }
}
