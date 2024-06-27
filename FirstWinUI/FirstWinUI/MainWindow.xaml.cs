using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.ApplicationModel;
using Rect = Windows.Foundation.Rect;
using Microsoft.UI.Windowing; //添加引用
using Windows.Foundation.Collections;
using Microsoft.UI.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FirstWinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private AppWindow m_AppWindow;
        private Window m_window;
        public MainWindow()
        {
            this.InitializeComponent();
            // 初始化一堆乱七八糟的
            m_AppWindow = this.AppWindow;
            Activated += MainWindow_Activated;
            AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;
            AppTitleBar.Loaded += AppTitleBar_Loaded;

            ExtendsContentIntoTitleBar = true;
            if (ExtendsContentIntoTitleBar == true)
            {
                m_AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            }
            TitleBarTextBlock.Text = AppInfo.Current.DisplayInfo.DisplayName;
            ExtendsContentIntoTitleBar = true; // 将页面扩展到标题栏中（隐藏了传统的标题栏）
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            m_window = new AnotherWindow();
            m_window.Activate();
        }
        private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
        {
            if (ExtendsContentIntoTitleBar == true)
            {
                // 当页面画布扩展到标题栏的时候，初始化交互的区域
                SetRegionsForCustomTitleBar();
            }
        }

        private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ExtendsContentIntoTitleBar == true)
            {
                // 在页面画布扩展到标题栏的前提下，当窗口大小变化时更新交互区域
                SetRegionsForCustomTitleBar();
            }
        }

        private void SetRegionsForCustomTitleBar()
        {
            // 计算标题栏的区域到底有多大
            // 交互区域， 就是那个按钮的区域，那个需要和用户交互

            double scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;

            RightPaddingColumn.Width = new GridLength(m_AppWindow.TitleBar.RightInset / scaleAdjustment);
            LeftPaddingColumn.Width = new GridLength(m_AppWindow.TitleBar.LeftInset / scaleAdjustment);

            // 获取按钮矩形的区域
            GeneralTransform transform = this.myButton.TransformToVisual(null);
            Rect bounds = transform.TransformBounds(new Rect(0, 0,
                                                             this.myButton.ActualWidth,
                                                             this.myButton.ActualHeight));
            Windows.Graphics.RectInt32 myButton = GetRect(bounds, scaleAdjustment);

            // 标题栏中有几个交互控件顶上这5行就复制几次，然后把变量都改成控件的Name，最后弄出来的区域按到底下那个var里边就行，如果看不懂的话

            var rectArray = new Windows.Graphics.RectInt32[] { myButton };

            InputNonClientPointerSource nonClientInputSrc =
                InputNonClientPointerSource.GetForWindowId(this.AppWindow.Id);
            nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, rectArray);
        }

        private Windows.Graphics.RectInt32 GetRect(Rect bounds, double scale)
        {
            // Rect事矩形区域
            return new Windows.Graphics.RectInt32(
                _X: (int)Math.Round(bounds.X * scale),
                _Y: (int)Math.Round(bounds.Y * scale),
                _Width: (int)Math.Round(bounds.Width * scale),
                _Height: (int)Math.Round(bounds.Height * scale)
            );
        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            // 当窗口不活动时弄成灰色的，活动时就不弄成灰色的
            if (args.WindowActivationState == WindowActivationState.Deactivated)
            {
                TitleBarTextBlock.Foreground =
                    (SolidColorBrush)App.Current.Resources["WindowCaptionForegroundDisabled"];
            }
            else
            {
                TitleBarTextBlock.Foreground =
                    (SolidColorBrush)App.Current.Resources["WindowCaptionForeground"];
            }
        }
    }
}
