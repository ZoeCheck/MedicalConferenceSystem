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
		#region 变量
		double ucWidth;
		double ucHeight;
		bool isClosed = false;
		double aniTime = 0.3;
		TouchPoint touchPointOld;
		#endregion

		#region 委托事件

		#endregion

		#region 属性

		#endregion

		#region 构造函数
		public WindowImageList()
		{
			this.InitializeComponent();

			// 在此点之下插入创建对象所需的代码。
		}
		#endregion

		#region 业务
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			WindowImageFullView windowFull = new WindowImageFullView();
			windowFull.ShowDialog();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			//DoubleAnimation da = new DoubleAnimation();
			//da.To = 0;
			//da.Duration = TimeSpan.FromSeconds(0.3);

			//Storyboard.SetTarget(da, UCIm1);
			//Storyboard.SetTargetProperty(da,new PropertyPath(WidthProperty));

			//Storyboard sbTurnPage = new Storyboard();
			//sbTurnPage.Children.Add(da);

			//sbTurnPage.Begin();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			//DoubleAnimation da = new DoubleAnimation();
			//da.To = ucWidth;
			//da.Duration = TimeSpan.FromSeconds(0.3);

			//Storyboard.SetTarget(da, UCIm1);
			//Storyboard.SetTargetProperty(da, new PropertyPath(WidthProperty));

			//Storyboard sbTurnPage = new Storyboard();
			//sbTurnPage.Children.Add(da);

			//sbTurnPage.Begin();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			ucWidth = this.ScrollViewerCenter.ActualWidth;
			ucHeight = this.ScrollViewerCenter.ActualHeight;

			foreach (UCImageList item in StackPanelUC.Children)
			{
				item.Width = ucWidth;
				item.Height = ucHeight;
			}

			this.Opacity = 0;

			STMainWindow.CenterX = this.ActualWidth / 2;
			STMainWindow.CenterY = this.ActualHeight / 2;
			STMainWindow.ScaleX = 0;
			STMainWindow.ScaleY = 0;

			NameScope.SetNameScope(this, new NameScope());
			this.RegisterName("scale", STMainWindow);

			Storyboard sb = new Storyboard();

			DoubleAnimation daX = new DoubleAnimation();
			daX.To = 1;
			daX.Duration = TimeSpan.FromSeconds(aniTime);

			DoubleAnimation daY = new DoubleAnimation();
			daY.To = 1;
			daY.Duration = TimeSpan.FromSeconds(aniTime);

			DoubleAnimation daOp = new DoubleAnimation();
			daOp.To = 1;
			daOp.Duration = TimeSpan.FromSeconds(aniTime);

			Storyboard.SetTargetName(daX, "scale");
			Storyboard.SetTargetProperty(daX, new PropertyPath(ScaleTransform.ScaleXProperty));
			Storyboard.SetTargetName(daY, "scale");
			Storyboard.SetTargetProperty(daY, new PropertyPath(ScaleTransform.ScaleYProperty));
			Storyboard.SetTarget(daOp, this);
			Storyboard.SetTargetProperty(daOp, new PropertyPath(UIElement.OpacityProperty));

			sb.Children.Add(daX);
			sb.Children.Add(daY);
			sb.Children.Add(daOp);

			sb.Begin(this);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!isClosed)
			{
				e.Cancel = true;

				Storyboard sB = new Storyboard();
				sB.Completed += (s, ee) =>
				{
					this.Close();
				};
				DoubleAnimation daX = new DoubleAnimation();
				daX.To = 0;
				daX.Duration = TimeSpan.FromSeconds(aniTime);

				DoubleAnimation daY = new DoubleAnimation();
				daY.To = 0;
				daY.Duration = TimeSpan.FromSeconds(aniTime);

				DoubleAnimation daOp = new DoubleAnimation();
				daOp.To = 0;
				daOp.Duration = TimeSpan.FromSeconds(aniTime);

				Storyboard.SetTargetName(daX, "scale");
				Storyboard.SetTargetProperty(daX, new PropertyPath(ScaleTransform.ScaleXProperty));
				Storyboard.SetTargetName(daY, "scale");
				Storyboard.SetTargetProperty(daY, new PropertyPath(ScaleTransform.ScaleYProperty));
				Storyboard.SetTarget(daOp, this);
				Storyboard.SetTargetProperty(daOp, new PropertyPath(UIElement.OpacityProperty));

				sB.Children.Add(daX);
				sB.Children.Add(daY);
				sB.Children.Add(daOp);

				sB.Begin(this);

				isClosed = true;
			}
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void ScrollViewerCenter_TouchDown(object sender, TouchEventArgs e)
		{
			touchPointOld = e.GetTouchPoint(BorderCenter);
		}

		private void ScrollViewerCenter_TouchUp(object sender, TouchEventArgs e)
		{
			TouchPoint touchPointNew = e.GetTouchPoint(BorderCenter);
			double offsetX = touchPointNew.Bounds.Left - touchPointOld.Bounds.Left;//判断X轴位移
			if (offsetX == 0)
			{
				WindowImageFullView windowFull = new WindowImageFullView();
				windowFull.ShowDialog();
			}
		}
		#endregion
	}
}