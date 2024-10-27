using productMonitor.OpCommand;
using productMonitor.UserControls;
using productMonitor.ViewModels;
using productMonitor.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace productMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 视图模型
        /// </summary>
        MainWindoVM mainWindowVM = new MainWindoVM();//给数据上下文，实例化一个DataContext
        private object get;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = mainWindowVM;
        }

        ///<summary>
        ///显示车间详情页
        ///</summary>
        private void ShowDetailUC()
        {
            WorkShopDetailUC workShopDetailUC = new WorkShopDetailUC();
            mainWindowVM.MonitorUC = workShopDetailUC;//替换用户控件

            //动画效果(由下而上)
            //位移 移动时间
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(0, 50, 0, -50), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 400));

            //透明度
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 0, 400));

            Storyboard.SetTarget(thicknessAnimation, workShopDetailUC);
            Storyboard.SetTarget(doubleAnimation, workShopDetailUC);

            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath("Margin"));
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity"));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(thicknessAnimation);
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();

        }
        /// <summary>
        /// 返回到监控
        /// </summary>
        private void GoBackMonitor()
        {
            MonitorUC monitorUC = new MonitorUC();
            mainWindowVM.MonitorUC = monitorUC;
        }

        /// <summary>
        /// 展示详情命令
        /// </summary>
        public Command ShowDetaiCmm
        {
            get
            {
                return new Command(ShowDetailUC);
            }
        }

        ///<summary>
        ///返回到监控命令
        ///</summary>
        public Command GoBackMonitorCmm
        {
            get
            {
                return new Command(GoBackMonitor);
            }
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMin(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// 弹出配置窗口
        /// </summary>
        private void ShowSettingWin()
        {
            //父子关系
            SettingsWin settingsWin = new SettingsWin() { Owner = this };
            settingsWin.ShowDialog();
        }

        /// <summary>
        /// 创建 弹出配置窗口 命令
        /// </summary>
        public Command ShowSettingaCmm
        {
            get
            {
                return new Command(ShowSettingWin);
            }
        }
    }
}