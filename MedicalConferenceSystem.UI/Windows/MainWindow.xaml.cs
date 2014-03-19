using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace MedicalConferenceSystem.UI
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		#region 变量

		#endregion

		#region 委托事件

		#endregion

		#region 属性

		#endregion

		#region 构造函数
		public MainWindow()
		{
			InitializeComponent();
		}
		#endregion

		#region 业务
		private void Expander_Expanded(object sender, RoutedEventArgs e)
		{
			WindowImageList windowIm = new WindowImageList();
			windowIm.ShowDialog();
		}

		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void Expander_Collapsed(object sender, RoutedEventArgs e)
		{
			WindowImageList windowIm = new WindowImageList();
			windowIm.ShowDialog();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void tbSearch_GotFocus(object sender, RoutedEventArgs e)
		{
			if (tbSearch.Text == "请输入(作者/医院/壁报标题)中的任意关键字搜索")
			{
				tbSearch.Text = "";
			}
		}

		private void tbSearch_LostFocus(object sender, RoutedEventArgs e)
		{
			if (tbSearch.Text == "")
			{
				tbSearch.Text = "请输入(作者/医院/壁报标题)中的任意关键字搜索";
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void tbSearch_TouchDown(object sender, TouchEventArgs e)
		{
			if (tbSearch.Text == "请输入(作者/医院/壁报标题)中的任意关键字搜索")
			{
				tbSearch.Text = "";
			}

			try
			{
				System.Diagnostics.Process.Start(@"C:\Program Files\Common Files\Microsoft Shared\Ink\TabTip.exe");
			}
			catch (Exception ex)
			{
				System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "TabTip.exe");
				//MessageBox.Show(ex.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}
	
		private void ShowListAni()
		{
			DoubleAnimation daMove = new DoubleAnimation(1, TimeSpan.FromMilliseconds(200));
			daMove.AccelerationRatio = 0.3;
			daMove.DecelerationRatio = 0.3;
			scaleTS.BeginAnimation(ScaleTransform.ScaleYProperty, daMove);
		}

		private void HideListAni()
		{
			DoubleAnimation daMove = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
			daMove.AccelerationRatio = 0.3;
			daMove.DecelerationRatio = 0.3;
			scaleTS.BeginAnimation(ScaleTransform.ScaleYProperty, daMove);
		}

		private void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			ShowListAni();
		}
		#endregion

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			HideListAni();
		}

		private void b4_Click(object sender, RoutedEventArgs e)
		{
			WindowImageList windowIm = new WindowImageList();
			windowIm.ShowDialog();
		}
	}
}
