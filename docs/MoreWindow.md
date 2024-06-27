# WinUI3学习Blog（2）：自定义标题栏+窗口背景材质

## 开始之前：

本篇内容有点复杂，也有些难度，所以建议跳过这篇往后面看几篇再回来，尤其是自定义标题栏

*本篇文章继续沿用[上一篇文章的仓库](https://github.com/mstouk57g/ConiMite_WinUI/tree/master/FirstWinUI)*

*本篇目标：*

> *1、自定义标题栏*
> 
> *2、改变窗口材质（云母和亚克力）*

## 自定义标题栏

看到很多系统应用，应该都没有系统标题栏，而是将整个页面扩展到了标题栏中

所以，接下来将会说说怎么弄

### 仅仅是将系统标题栏隐藏掉

**注意：这种方法比较简单，但是不能添加按钮等可以用户交互的控件，因为你会发现交互不了**

*使用上一篇的MainWindow，仓库提交[bfa108](https://github.com/mstouk57g/ConiMite_WinUI/commit/bfa108d8038729eb447ce96787fddcab89b6f42f)*

首先，我们要在XAML中用布局的方法写一个标题栏的UI（`MainWindow.xaml`）

```XAML
<Grid
    x:Name="AppTitleBar"
    Height="48"
    Margin="48,0,0,0"
    VerticalAlignment="Top"
    Padding="0">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto" />
    </Grid.ColumnDefinitions>
    <TextBlock
        Margin="12,0,0,0"
        Grid.Column="1"
        VerticalAlignment="Center"
        Style="{StaticResource CaptionTextBlockStyle}"
        Text="FirstWinUI"/>
</Grid>
```

上面的代码中，用了一个Grid布局，名字叫`AppTitleBar`

里面放了一个`TextBlock`，这个东西是个文字标签（是这么叫？），相当于Label，标签上的文字是“FirstWinUI”

后面那一堆属性不管他，下篇研究

---

上一篇中的`MainWindow.xaml`用了一个StackPanel来在窗口中间。但是，**一个页面（不管是Window还是Page）最外层只能有一个布局**

所以，为了保留这个按钮的位置，我们要在这个标题栏的布局（上面的），和这个按钮的布局（StackPanel），外面套一层Grid，就像这样

```XAML
<Grid>
    <Grid
        x:Name="AppTitleBar"
        Height="48"
        Margin="48,0,0,0"
        VerticalAlignment="Top"
        Padding="0">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto" />
        </Grid.ColumnDefinitions>
        <TextBlock
            Margin="12,0,0,0"
            Grid.Column="1"
            VerticalAlignment="Center"
            Style="{StaticResource CaptionTextBlockStyle}"
            Text="FirstWinUI"/>
    </Grid>
    <StackPanel Orientation="Horizontal" HorizontalAlignment="Center" VerticalAlignment="Center">
        <Button x:Name="myButton" Click="myButton_Click">Click Me</Button>
    </StackPanel>
</Grid>
```

~~这就是传说中的嵌套布局吧！~~

然后，我们要在代码中应用上这个标题栏（`MainWindow.cs`）

在`public MainWindow()`底下

```C#
ExtendsContentIntoTitleBar = true; // 将页面扩展到标题栏中（隐藏了传统的标题栏）
SetTitleBar(AppTitleBar); // 设置标题栏是名字叫“AppTitleBar”的那个布局
```

注释中已经说明每行代码是干啥的了

最后长这样就对了：

```C#
public MainWindow()
{
    this.InitializeComponent();
    ExtendsContentIntoTitleBar = true; // 将页面扩展到标题栏中（隐藏了传统的标题栏）
    SetTitleBar(AppTitleBar); // 设置标题栏是名字叫“AppTitleBar”的那个布局
}
```

简单运行一下，可以发现是生效的

![成功力！](https://s21.ax1x.com/2024/06/27/pky4KEV.png)

---

只不过，你把那个按钮也扔到标题栏上试试：（`MainWindow.xaml`）*仓库提交：[30ac367](https://github.com/mstouk57g/ConiMite_WinUI/commit/30ac367d92361f23a4e9817ad9c2bb58f41f35ec)*

```XAML
<Grid>
    <Grid
        x:Name="AppTitleBar"
        Height="48"
        Margin="12,0,0,0"
        VerticalAlignment="Top"
        Padding="0">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto" />
            <ColumnDefinition Width="Auto" />
        </Grid.ColumnDet="FirstWinUI"/>
        <Button x:Name="myButton" Margin="12,0,0,0" Click="myButton_Click">finitions>
        <TextBlock
            Margin="12,0,0,0"
            Grid.Column="1"
            VerticalAlignment="Center"
            Style="{StaticResource CaptionTextBlockStyle}"
            TexClick Me</Button>
    </Grid>
</Grid>
```

运行后你会发现，根本点不开，双击直接最大化了。。。

![根本点不开](https://raw.githubusercontent.com/mstouk57g/ConiMite_WinUI/Page/Imgs/%E5%B1%8F%E5%B9%95%E5%BD%95%E5%88%B6%202024-06-27%20100523.gif)

所以，为了能让这个按钮正常工作，我们必须要完全自定义

### 完全自定义

*参考：[微软官方运行不了的文档](https://learn.microsoft.com/zh-cn/windows/apps/develop/title-bar)， 仓库提交：[7fe3ef](https://github.com/mstouk57g/ConiMite_WinUI/commit/7fe3ef5d0dbf3db337511f3033160ba76134a8cd)*

这个标题栏UI的样子保持不动，我们只需要改代码文件

标题栏，分为可以拖动的区域（图标，标题文字，空白区域等），和可以交互的区域（按钮~~，不包括三大金刚~~）

拖动的区域不进行控件的交互，交互的区域不能拖。所以最后代码被造成了这样（`MainWindow.cs`）：

```C#
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
```

`MainWindow.xaml`也要加一点料，把`<Grid.ColumnDefinitions>`这一块弄成这样

这样主要是为了计算最左侧的区域和最右侧三大金刚的区域

```XAML
<Grid.ColumnDefinitions>
    <ColumnDefinition x:Name="LeftPaddingColumn" Width="0"/>
    <ColumnDefinition Width="Auto" />
    <ColumnDefinition Width="Auto" />
    <ColumnDefinition x:Name="RightPaddingColumn" Width="0"/>
</Grid.ColumnDefinitions>
```

~~反正乱七八糟，有点C#和Windows开发基础的差不多应该看得懂，看不懂把变量换换复制粘贴也能跑~~

我在文档代码基础上把注释翻译了一下，然后又加了一些注释。~~对于复制粘贴改变量的，这些注释应该够理解得了（~~

不要忘记添加引用

```C#
using Windows.ApplicationModel;
using Rect = Windows.Foundation.Rect;
using Microsoft.UI.Windowing; //添加引用
```

这样就行了，运行可以看到按钮可以正常点，拖动也没有问题

![运行正常](https://raw.githubusercontent.com/mstouk57g/ConiMite_WinUI/Page/Imgs/%E5%B1%8F%E5%B9%95%E5%BD%95%E5%88%B6%202024-06-27%20105518.gif)

## 紧凑视图和全屏视图

***注意：这部分涉及一些按钮操作的知识，请先往后看，看完按钮部分后再回来看这个部分***

众所周知，WinUI是有紧凑视图和全屏视图的，可以使用`AppWindowPresenterKind`来进行切换

*所以，现在就要往窗口中添加三个按钮，分别为全屏，紧凑，和默认 [部分目标]*

*参考：[微软官方运行不了的文档](https://learn.microsoft.com/zh-cn/windows/apps/develop/title-bar)， 仓库提交：[a8ff216](https://github.com/mstouk57g/ConiMite_WinUI/commit/a8ff216600c8ce394e99b4d545df72206323dcc1)*

先往`MainWindow.xaml`上扔三个按钮，操作都指向`SwitchPresenter`（不知道为啥都指向一个操作的就跳过这个部分往后面看）

```XAML
<StackPanel Orientation="Horizontal" HorizontalAlignment="Center" VerticalAlignment="Center">
        <Button x:Name="CompactoverlaytBtn"
        Content="紧凑"
        Click="SwitchPresenter"/>
        <Button x:Name="FullscreenBtn" 
        Content="全屏"
        Click="SwitchPresenter"/>
        <Button x:Name="OverlappedBtn"
        Content="默认"
        Click="SwitchPresenter"/>
</StackPanel>
```

在初始化一堆乱七八糟那底下再加一个（`MainWindow.xaml`）

```C#
this.InitializeComponent();
// 初始化一堆乱七八糟的
m_AppWindow = this.AppWindow;
m_AppWindow.Changed += AppWindow_Changed; // 按上初始化 // 加的是这句，上面的原来都有
```

然后在类里按上几个操作，用来使用`AppWindowPresenterKind`

```C#
private void AppWindow_Changed(AppWindow sender, AppWindowChangedEventArgs args)
{
    if (args.DidPresenterChange)
    {
        switch (sender.Presenter.Kind)
        {
            case AppWindowPresenterKind.CompactOverlay:
                // 紧凑视图中，隐藏自定义的的标题栏，而是使用系统的标题栏
                // 因为这种时候，自定义的标题栏会被当成普通的控件来伺候
                // 当然，你乐意也行， 效果也差不了多少
                AppTitleBar.Visibility = Visibility.Collapsed; // 隐藏自定义标题栏
                sender.TitleBar.ResetToDefault(); // 使用系统默认标题栏
                break;

            case AppWindowPresenterKind.FullScreen:
                // 全屏的时候也隐藏自定义的标题栏，因为自定义的标题栏也会被当成普通的控件来伺候
                AppTitleBar.Visibility = Visibility.Collapsed; // 隐藏自定义标题栏
                sender.TitleBar.ExtendsContentIntoTitleBar = true; // 画布依旧扩展到标题栏上
                break;

            case AppWindowPresenterKind.Overlapped:
                // 重叠的时候（就是非常普通，启动后的样子），使用的是我们自己自定义的标题栏
                AppTitleBar.Visibility = Visibility.Visible;
                sender.TitleBar.ExtendsContentIntoTitleBar = true;
                break;

            default:
                // 使用系统默认标题栏
                sender.TitleBar.ResetToDefault();
                break;
        }
    }
}

private void SwitchPresenter(object sender, RoutedEventArgs e)
{
    if (AppWindow != null)
    {
        AppWindowPresenterKind newPresenterKind;
        switch ((sender as Button).Name)
        {
            case "CompactoverlaytBtn": // 紧凑
                newPresenterKind = AppWindowPresenterKind.CompactOverlay;
                break;

            case "FullscreenBtn": // 全屏
                newPresenterKind = AppWindowPresenterKind.FullScreen;
                break;

            case "OverlappedBtn": // 重叠（就是你不动它的时候）
                newPresenterKind = AppWindowPresenterKind.Overlapped;
                break;

            default: // 啥也不是（使用系统默认，这个就让系统决定）
                newPresenterKind = AppWindowPresenterKind.Default;
                break;
        }

        // 如果在这个模式中又按了这个模式的按钮，就滚回默认的模式（系统决定的）
        if (newPresenterKind == AppWindow.Presenter.Kind)
        {
            AppWindow.SetPresenter(AppWindowPresenterKind.Default);
        }
        else
        {
            // 如果不是，就切换
            AppWindow.SetPresenter(newPresenterKind);
        }
    }
}
```

注释里有对代码的解释，看不懂可以直接Copy。运行起来还可以

![运行效果](https://raw.githubusercontent.com/mstouk57g/ConiMite_WinUI/Page/Imgs/%E5%B1%8F%E5%B9%95%E5%BD%95%E5%88%B6%202024-06-27%20113944.gif)

## 窗口背景材质

Windows11中引用了云母（Mica）材质，对桌面进行了模糊处理，效果还可以，这里就拿这个当例子说

在WindowsAppSDK1.3中简化了使用这些材质的过程，使其几行代码就能搞定

### 方法一：在XAML中设置

*仓库提交：[b02a18a](https://github.com/mstouk57g/ConiMite_WinUI/commit/b02a18ab14ce67eb9eb28478a9a6bf859cf717a5)*

打开`MainWindow.xaml`，把下面这点东西丢到窗口的布局上面，Window标签那一堆IntelliSense（就是我从来不管的东西）底下

```XAML
<Window.SystemBackdrop>
    <MicaBackdrop Kind="Base"/>
</Window.SystemBackdrop>
```

然后直接跑就行

当然你也可以用亚克力，把`<MicaBackdrop Kind="Base"/>`换成`<DesktopAcrylicBackdrop/>`就行

MicaBackdrop中的`Kind`也可以换成`BaseAlt`，效果也有些不同

### 方法二：在代码中设置

*部分目标：在窗口中再按上仨按钮，分别可以切换三种背景材料。仓库提交：[372f032](https://github.com/mstouk57g/ConiMite_WinUI/commit/372f03268c5bcddbdc9ab8dc1f740febdc118d3c)*

首先先按上三个按钮，按下后触发`SetBackdrop`函数。（`MainWindow.xaml`）

```C#
<StackPanel Orientation="Horizontal">
    <Button x:Name="MicaBaseBtn"
        Content="MicaBase"
        Click="SetBackdrop"/>
    <Button x:Name="MicaBaseAltBtn" 
        Content="MicaBaseAlt"
        Click="SetBackdrop"/>
    <Button x:Name="AcrylicBtn"
        Content="Acrylic"
        Click="SetBackdrop"/>
</StackPanel>
```

为了放到原先三个按钮的底下，这里再次嵌套一个StackPanel，最后连着上面的三个按钮，代码长这样

```XAML
<StackPanel Orientation="Vertical" HorizontalAlignment="Center" VerticalAlignment="Center">
    <StackPanel Orientation="Horizontal">
        <Button x:Name="CompactoverlaytBtn"
        Content="紧凑"
        Click="SwitchPresenter"/>
        <Button x:Name="FullscreenBtn" 
        Content="全屏"
        Click="SwitchPresenter"/>
        <Button x:Name="OverlappedBtn"
        Content="默认"
        Click="SwitchPresenter"/>
    </StackPanel>
    <StackPanel Orientation="Horizontal">
        <Button x:Name="MicaBaseBtn"
            Content="MicaBase"
            Click="SetBackdrop"/>
        <Button x:Name="MicaBaseAltBtn" 
            Content="MicaBaseAlt"
            Click="SetBackdrop"/>
        <Button x:Name="AcrylicBtn"
            Content="Acrylic"
            Click="SetBackdrop"/>
    </StackPanel>
</StackPanel>
```

然后写逻辑代码，丢到`MainWindow`类里随便一个位置（`MainWindow.cs`）

```C#
private void SetBackdrop(object sender, RoutedEventArgs e)
{
    if (AppWindow != null)
    {
        switch ((sender as Button).Name)
        {
            case "MicaBaseBtn": // 按钮MicaBase
                SystemBackdrop = new MicaBackdrop()
                    { Kind = MicaKind.Base }; //进行更换
                break;
            case "MicaBaseAltBtn": // 按钮MicaBaseAlt
                SystemBackdrop = new MicaBackdrop()
                    { Kind = MicaKind.BaseAlt }; //进行更换
                break;
            case "AcrylicBtn": // 按钮Acrylic
                SystemBackdrop = new DesktopAcrylicBackdrop(); //进行更换
                break;
        }
    }
}
```

和上面的那仨按钮的逻辑一样，这里也是使用了switch-case。注释有解释，不再多说。

运行的时候窗口也是Mica是因为在Xaml中设置过了。如果没有设置过就没有效果。这个时候可以在窗口初始化那一堆底下加这个东西：

```C#
       SystemBackdrop = new MicaBackdrop()
                    { Kind = MicaKind.Base }
```

这样就可以直接运行了，效果如下

![效果](https://raw.githubusercontent.com/mstouk57g/ConiMite_WinUI/Page/Imgs/%E5%B1%8F%E5%B9%95%E5%BD%95%E5%88%B6%202024-06-27%20132703.gif)

### 三种材质区别

Acrylic：

![Acrylic效果](https://s21.ax1x.com/2024/06/27/pkyo4PK.png)

MicaBaseAlt：

![MicaBaseAlt效果](https://s21.ax1x.com/2024/06/27/pkyo58O.png)

MicaBase：

![MicaBase效果](https://s21.ax1x.com/2024/06/27/pkyoI2D.png)

## 总结

这篇讲了自定义标题栏和背景材质两个内容~~（我喜欢~）~~

不会可以先跳过，因为东西有点超前。~~但理论上来说，看着注释复制粘贴改改变量应该也可以跑~~

字数太多了，下篇讲往窗口上按Page
