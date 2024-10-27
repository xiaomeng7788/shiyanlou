using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace productMonitor.UserControls
{
    /// <summary>
    /// RingUC.xaml 的交互逻辑
    /// </summary>
    public partial class RingUC : UserControl
    {
        public RingUC()
        {
            InitializeComponent();
            SizeChanged += OnSizeChanged;//界面大小改变跟着改
        }
        
        //使用画图方法
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Dubg();
        }

        //依赖属性
        //百分比

        public double PercentValue
        {
            get { return (double)GetValue(PercentValueProperty); }
            set { SetValue(PercentValueProperty, value); }
        }

        //使用DependencyProperty作为PercentValue的后备存储。这启用了动画、样式、绑定等功能……
        public static readonly DependencyProperty PercentValueProperty =
            DependencyProperty.Register("PercentValue", typeof(double), typeof(RingUC));

        /// <summary>
        /// 画圆环方法
        /// </summary>
        private void Dubg()
        {
            LayOutGrid.Width = Math.Min(RenderSize.Width, RenderSize.Height);
            double raduis = LayOutGrid.Width / 2;

            double x = raduis + (raduis - 3) * Math.Cos((PercentValue % 100 * 3.6 - 90) * Math.PI / 180);
            double y = raduis + (raduis - 3) * Math.Sin((PercentValue % 100 * 3.6 - 90) * Math.PI / 180);

            int Is50 = PercentValue < 50 ? 0 : 1;

            //M:移动  A:画弧
            string pathStr = $"M{raduis + 0.01} 3A{raduis - 3} {raduis - 3} 0 {Is50} 1 {x} {y}";//移动路径

            //画图形对象
            var converter = TypeDescriptor.GetConverter(typeof(Geometry));//获取 Geometry 类型的转换器，用于将字符串转换为 Geometry 几何图形对象。
            path.Data = (Geometry)converter.ConvertFrom(pathStr);//传递给 path 的路径数据。
        }
    }
}
