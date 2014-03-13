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
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace MedicalConferenceSystem.UI.Windows
{
	/// <summary>
	/// Author：xxh 
	/// CreateTime：2014-03-13 20:40:33 
	/// </summary>
	public partial class WindowSignleImage : Window
	{
		#region 变量
		double aniTime = 0.3;
		bool isClosed = false;
		TouchPoint touchPointOld;
		#endregion

		#region 属性

		#endregion

		#region 委托事件

		#endregion

		#region 构造函数
		public WindowSignleImage()
		{
			InitializeComponent();
		}
		#endregion

		#region 业务
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
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

		public void SetBackImage(ImageSource imageSource)
		{
			this.ImageMain.Source = imageSource;
		}

		private void Window_TouchDown(object sender, TouchEventArgs e)
		{
			touchPointOld = e.GetTouchPoint(this);
		}

		private void Window_TouchUp(object sender, TouchEventArgs e)
		{
			TouchPoint touchPointNew = e.GetTouchPoint(this);
			if (touchPointOld.Bounds.Left == touchPointNew.Bounds.Left && touchPointOld.Bounds.Top == touchPointNew.Bounds.Top)
			{
				this.Close();
			}
		}
		#endregion


	}
}
