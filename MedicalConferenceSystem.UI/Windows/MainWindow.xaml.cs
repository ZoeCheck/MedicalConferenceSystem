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

namespace MedicalConferenceSystem.UI
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

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
	}
}
