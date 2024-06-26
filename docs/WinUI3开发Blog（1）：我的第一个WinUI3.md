[返回目录](https://mstouk57g.github.io/ConiMite_WinUI/)

## 开始之前：

### 自我介绍

先介绍一下我自己吧：我，ConiMite，简称Coni，玩MC的时候叫ntcho，，高中牲一头，是一个渣渣中的渣渣（

我个人比较擅长的语言是Python，C#呢也就是为了学习UWP以及WASDK才学的，所以自然而然不如Python顺手

因此，你可能会在我的任何一个C#的仓库中看到以下现象：

> 1、C#中嵌套着Python代码
> 
> 2、一块代码复制n遍，靠复制解决问题
> 
> 3、没有注释，同样的功能写两边（
> 
> 4、借鉴其他的开源代码来实现部分功能（（
> 
> 5、屎山（（（

至于为什么入门呢？也就是觉得好看罢了~

为了更好交流，已经建立讨论群：750935016，欢迎加群讨论（当然你也会看到群主在那里问问题）

这一个系列，都事使用Xanl+C#开发

同时，这一系列，是个人记录用，写的肯定不好，不详细，别喷啊！

还有一点，这个系列的仓库我都扔到GitHub力，文章在哪仓库就在哪~

---

好了，不废话了，进入正题

这个项目，作为第一个应用，就会先说说创建窗口，以及窗口绑定页面（算作绑定？）

### WinUI3是个啥？

~~WinUI3事WinUI3（即答）~~

看看微软的解释：

[![Microsoft解释](https://s21.ax1x.com/2024/06/26/pkyF9gJ.png)](https://learn.microsoft.com/zh-cn/windows/apps/winui/)

说白了，就是个UI库，可以给UWP使，也可以给Win32使（个人理解）

## 创建一个WinUI3应用

首先，你得有个Visual Studio 2022

### 直接创建WinUI3

然后，安装的时候选择`.Net桌面开发`和`通用Windows平台开发`这俩个工作负荷

在`.Net桌面开发`中有个`Windows应用SDK C#模板`，把它选上

最后大致长这样，其他的自己可以看着选选



![大致长这样](https://s21.ax1x.com/2024/06/26/pkyFBbq.png)



---

安装后，就可以创建项目了

![创建项目](https://s21.ax1x.com/2024/06/26/pkyFcPU.png)

在项目类型中选WinUI就行了，你会看到3个项目（就是上图后面那仨蓝色图标的，第一个文后会说）

关于已打包项目和使用Windows应用程序打包项目打包项目这俩的区别，就是一个创建了项目里面没有打包项目（共1个项目），另一个有（共2个项目）

我个人的理解是，已打包项目就是将打包项目扔到了WinUI3这个项目里边了（因为AppxManifest.xml在）

但这俩选了后，开发的效果应该是一样的（自我感觉）

### Temlate Studio

~~这东西应该会有人熟悉吧，反正我是用不习惯~~

这个东西就是给你个模板，这样创建应用会快一些

可以选的模板有菜单栏和导航栏，以及啥也没有这三种

这玩意后面可以通过向导创建页面，如果你创建设置的话，里面自带切换深浅色的一个设置项

这个东西还支持创建各种的页面，看看图就知道有哪几种

![各种页面](https://s21.ax1x.com/2024/06/26/pkyFHPO.png "各种页面")



***<u>但是，有一点要注意：这个东西用的设计模式只有MVVM模式，项目结构有些复杂（个人感觉），如果是个纯小白，没有接触过WPF/UWP/WinUI3项目开发的，或者不知道MVVM是啥的，别用，等着熟悉了后再试试吧。。。</u>***

[最后，这个东西是个Visual Studio扩展，需要单独安装]([Template Studio for WinUI (C#) - Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=TemplateStudio.TemplateStudioForWinUICs))

### 创建完了，瞅瞅

创建项目完了后，你就可以开始开发了。创建项目的时候已经给你了模板，自带一个按钮

选择VisualStudio上方编辑栏的`本地计算机`就可以运行看看了

![大致长这样](https://s21.ax1x.com/2024/06/26/pkykFzQ.png)

## 试试开发

*本部分目标：创建一个窗口，并在单击窗口的时候唤起这个窗口，以及最基本的按钮操作和最基本布局的介绍*

### 创建一个窗口

*仓库提交：`c83b176`*

首先肯定是要创建一个窗口。这点很简单：

右击项目->添加->新建项->显示所有模板（如果已经有模板了就不用这一步）->WinUI->空白窗口->命名创建

**![](https://raw.githubusercontent.com/mstouk57g/ConiMite_WinUI/Page/Imgs/demo.gif)**

这样，你就会在项目里面得到一个空白的窗口（我创建的文件是`AnotherWindow.xaml`）

### 往新建的窗口中加一个按钮

打开`AnotherWindow.xaml`，这个xaml就是存放UI代码的地方了

xaml和继承了许多xml的特点，同时，开发的方式也和html差不多

里面已经写好了很多东西，上面的那一堆可以先不管，先看那个`<Grid></Grid>`

这个就是网格布局，就是整个窗口的布局

我们要往布局里面放一个按钮：很简单，就在`<Grid></Grid>`之间加点代码就行

```XAML
<Button Content="AnotherWindow"/>
```

这行代码的意思比较明确，就是一个按钮，按钮上的文字是`AnotherWindow`

### 让窗口上的按钮唤起另一个窗口

*仓库提交：c380b2b*

刚才创建了个窗口，但是运行后还是没有用，因为程序启动后还是唤醒刚才最开始模板自带的窗口（MainWindow）

MainWindow中有一个按钮，那我们可以尝试用那个按钮唤起

打开`MainWindow.xaml`，你会发现一些代码。同样上面的不管，看中间的这段：

```XAML
    <StackPanel Orientation="Horizontal" HorizontalAlignment="Center" VerticalAlignment="Center">
        <Button x:Name="myButton" Click="myButton_Click">Click Me</Button>
    </StackPanel>
```

稍微的解释一下：

>   `<StackPanel></StackPanel>`也是布局，可以理解为水平布局或垂直布局
> 
> `Orientation="Horizontal"`代表着这时的StackPenel是水平的，垂直的话用`Orientation=Vertical"`
> 
> `VerticalAlignment="Center"`指里面的控件（按钮啥的），是从中间开始摆
> 
>  `<Button></Button>`就是在布局里面放一个非常普通的按钮，中间的文字相当于Content，是显示在按钮上的
> 
> ` x:Name="myButton`的意思是这个按钮的“名字”叫做`myButton`，后面写C#的时候操控这个按钮就要用这个名字
> 
> ` Click="myButton_Click`是指按下按钮后的操作是调用`myButton_Click`这个函数，一会会在代码文件中写

代码非常简单明确，也比较易懂（

然后打开`MainWindow.cs`（找不到的把`MainWindow.xaml`展开），前面那堆引用先不管

看到这点代码：

```C#
public MainWindow()
{
    this.InitializeComponent();
}
```

这个就是初始化这个窗口，要不然xaml中的控件在代码中全都没法用

然后看底下的一段代码：

```C#
private void myButton_Click(object sender, RoutedEventArgs e)
{
    myButton.Content = "Clicked";
}
```

这个就是传说中按下按钮触发的操作`myButton_Click`

后面的参数可以先不管

而`  myButton.Content = "Clicked`的作用就是把`myButton`的Content，也就是显示的东西，变成“Clicked”

`myButton`就是前面用`x:Name`来定义的名字

---

所以，如果我们要让这个按钮的的操作是唤起窗口，就要对这个函数下手~

改成这样，直接粘贴~

```C#
private void myButton_Click(object sender, RoutedEventArgs e)
{
    m_window = new AnotherWindow();
    m_window.Activate();
}
private Window m_window;
```

简单解释一下，还是这个函数

> 在函数外面，有个`private Window m_window;`，定义了m_window的类型是Window
> 
> 然后，在函数里面`m_window = new AnotherWindow();`向m_window赋值，是AnotherWindow
> 
> 最后，`m_window.Activate();`是将m_window中的Window给唤醒起来

最后大概的效果：

![最后效果](https://raw.githubusercontent.com/mstouk57g/ConiMite_WinUI/Page/Imgs/demo2.gif)

## 继续开发

文章就到这里了，后面记几句

一些工具可以帮助我们继续开发

1、WinUI3 Gallery（官方控件，里面的控件库挺好的）

2、Windows Community Toolkit Gallery（社区控件，里面的控件库挺好的）

3、微软官方的文档（毕竟是官方的。。。文档）

## 总结

这篇文章中，我们创建了一个WinUI3应用，并在应用中创建了一个新的窗口，使主窗口中的按钮唤起这个窗口

同时，我们还认识了按钮控件以及`Grid`和`StackPanel`两个布局

下一篇再对窗口做点补充，bye~


