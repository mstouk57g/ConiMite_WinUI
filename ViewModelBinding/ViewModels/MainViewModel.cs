using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace ViewModelBinding.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string displayText = "初始文本";

    [RelayCommand]
    private void ChangeText()
    {
        Trace.WriteLine("nm");
        DisplayText = "按钮被点击了";
    }
}
