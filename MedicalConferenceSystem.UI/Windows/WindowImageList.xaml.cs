using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace MedicalConferenceSystem.UI
{
	/// <summary>
	/// WindowImageList.xaml 的交互逻辑
	/// </summary>
	public partial class WindowImageList : Window
	{
		public WindowImageList()
		{
			this.InitializeComponent();
			
			// 在此点之下插入创建对象所需的代码。
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			WindowImageFullView windowFull = new WindowImageFullView();
			windowFull.ShowDialog();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			DoubleAnimation da = new DoubleAnimation();
			da.To = 0;
			da.Duration = TimeSpan.FromSeconds(0.5);

			Storyboard.SetTarget(da, UCIm1);
			Storyboard.SetTargetProperty(da,new PropertyPath(WidthProperty));

			Storyboard sbTurnPage = new Storyboard();
			sbTurnPage.Children.Add(da);

			sbTurnPage.Begin();
		}
	}
}