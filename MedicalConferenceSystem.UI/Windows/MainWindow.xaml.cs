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
using MedicalConferenceSystem.UI.UserControls;
using System.IO;
using System.Timers;

namespace MedicalConferenceSystem.UI
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		#region 变量
		double aniTime = 0.3;
		bool isClosed = false;
		TouchPoint touchPointOld;
		List<string> listImagePath = new List<string>();
		Timer timer;
		int addIndex = 0;
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
			DoubleAnimation daMove = new DoubleAnimation(250, TimeSpan.FromMilliseconds(200));
			daMove.AccelerationRatio = 0.3;
			daMove.DecelerationRatio = 0.3;
			ScrollViewerList.BeginAnimation(ScrollViewer.HeightProperty, daMove);
			//scaleTS.BeginAnimation(ScaleTransform.ScaleYProperty, daMove);
		}

		private void HideListAni()
		{
			DoubleAnimation daMove = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
			daMove.AccelerationRatio = 0.3;
			daMove.DecelerationRatio = 0.3;
			ScrollViewerList.BeginAnimation(ScrollViewer.HeightProperty, daMove);
			//scaleTS.BeginAnimation(ScaleTransform.ScaleYProperty, daMove);
		}

		private void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			ShowListAni();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			HideListAni();
		}

		private void b4_Click(object sender, RoutedEventArgs e)
		{
			WindowImageList windowIm = new WindowImageList();
			windowIm.ShowDialog();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			string filePath = AppDomain.CurrentDomain.BaseDirectory + @"TypeImages";
			foreach (string path in Directory.GetFileSystemEntries(filePath))
			{
				listImagePath.Add(path);
			}

			BeginLoadWindowAnimation();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!isClosed)
			{
				e.Cancel = true;

				BeginClodeWindowAnimation();

				isClosed = true;
			}
		}

		#region 窗体渐大渐小动画
		/// <summary>
		/// 加载渐大动画
		/// </summary>
		private void BeginLoadWindowAnimation()
		{
			this.Opacity = 0;

			STMainWindow.CenterX = this.ActualWidth / 2;
			STMainWindow.CenterY = this.ActualHeight / 2;
			STMainWindow.ScaleX = 0;
			STMainWindow.ScaleY = 0;

			NameScope.SetNameScope(this, new NameScope());
			this.RegisterName("scale", STMainWindow);

			Storyboard sb = new Storyboard();
			sb.Completed += new EventHandler(sb_Completed);

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

		void sb_Completed(object sender, EventArgs e)
		{
			timer = new Timer();
			timer.Interval = 50;
			timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
			timer.Start();
		}

		void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (addIndex < 9)
			{
				this.Dispatcher.Invoke(new Action(() =>
				{
					UCType ucTypeLeft = new UCType();
					//ucTypeLeft.Width = 150;
					//ucType.Height = 160;
					ucTypeLeft.Width = (this.ActualWidth - 140) / 2;
					ucTypeLeft.Height = (this.ActualHeight - this.ActualHeight * 0.5) / 10;
					ucTypeLeft.Margin = new Thickness(0, 10, 0, 10);
					//ucType.SetImage(listImagePath[index++]);
					ucTypeLeft.SetInfoUp("类别 " + addIndex + " 糖尿病预防与流行病学");
					ucTypeLeft.SetInfoDown("摘要" + addIndex);
					StackPanelLeft.Children.Add(ucTypeLeft);

					UCType ucTypeRight = new UCType();
					//ucType.Width = 150;
					//ucType.Height = 160;
					ucTypeRight.Width = (this.ActualWidth - 140) / 2;
					ucTypeRight.Height = (this.ActualHeight - this.ActualHeight * 0.5) / 10;
					ucTypeRight.Margin = new Thickness(0, 10, 0, 10);
					//ucType.SetImage(listImagePath[index++]);
					ucTypeRight.SetInfoUp("类别 " + addIndex + " 糖尿病基础研究");
					ucTypeRight.SetInfoDown("摘要" + addIndex);
					StackPanelRight.Children.Add(ucTypeRight);

					addIndex++;
				}));
			}
			else
			{
				timer.Stop();
			}
		}

		/// <summary>
		/// 关闭窗体渐小动画
		/// </summary>
		private void BeginClodeWindowAnimation()
		{
			Storyboard sB = new Storyboard();
			sB.Completed += (s, e) =>
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
		}
		#endregion

		private void ScrollViewer_TouchDown(object sender, TouchEventArgs e)
		{
			touchPointOld = e.GetTouchPoint(ScrollViewerListCenter);
		}

		private void ScrollViewer_TouchUp(object sender, TouchEventArgs e)
		{
			UCType ucTypeCaptured = (e.TouchDevice).Captured as UCType;
			if (ucTypeCaptured == null)
			{
				return;
			}

			TouchPoint touchPointNew = e.GetTouchPoint(ScrollViewerListCenter);
			double offsetY = touchPointNew.Bounds.Top - touchPointOld.Bounds.Top;//判断X轴位移

			if (Math.Abs(offsetY) <= 6)//单点
			{
				WindowImageList windowIm = new WindowImageList();
				windowIm.ShowDialog();
			}
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			HideListAni();
		}
		#endregion
	}
}
