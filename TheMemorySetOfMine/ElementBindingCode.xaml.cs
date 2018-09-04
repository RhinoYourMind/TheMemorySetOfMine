using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace TheMemorySetOfMine
{
    /// <summary>
    /// ElementBindingCode.xaml 的交互逻辑
    /// </summary>
    public partial class ElementBindingCode : Window
    {
        public ElementBindingCode()
        {
            InitializeComponent();
            BindingCode();
        }
        private void BindingCode()
        {
            Binding binding = new Binding();
            binding.Source = this.Slider2;//源对象
            binding.Path = new PropertyPath("Value");//Value为Slider2中的属性
            binding.Mode = BindingMode.TwoWay;//数据传递模式
            this.TextBlock2.SetBinding(TextBlock.TextProperty, binding);//SetBinding函数绑定,绑定的是以这个类TextBlock的属性TextProperty
        }
        private void Button1_OnClick(object sender, RoutedEventArgs e)
        {
            this.TextBlock1.Text = "0";
        }

        private void Button2_OnClick(object sender, RoutedEventArgs e)
        {
            this.TextBlock2.Text = "0";
        }


        private void Button3_OnClick(object sender, RoutedEventArgs e)
        {
            BindingOperations.ClearAllBindings(TextBlock1);
            BindingOperations.ClearAllBindings(TextBlock2);
        }
    }
}
