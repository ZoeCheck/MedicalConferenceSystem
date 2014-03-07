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

namespace MedicalConferenceSystem.UI.Windows
{
	/// <summary>
	/// Author：xxh 
	/// CreateTime：2014-03-07 21:30:33 
	/// </summary>
	public partial class WindowMuiltTouchTest : Window
	{
		public WindowMuiltTouchTest()
		{
			InitializeComponent();
		}

		private Dictionary<int, Ellipse> movingEllipses = new Dictionary<int, Ellipse>();
		Random rd = new Random();

		/// <summary>
		/// TouchDown 事件主要是完成当触碰产生时在<Canvas> 控件中生成彩色圆圈的任务（C#代码如下）。
		/// 使用Ellipse 创建随机颜色的圆圈，通过GetTouchPoint 方法获取触碰位置点，并调整圆圈在<Canvas> 中的位置。
		/// 为了跟踪手指移动轨迹，需要将触屏设备ID 及UI 控件存储在集合movingEllipses 中。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void touchPad_TouchDown(object sender, TouchEventArgs e)
		{
			Ellipse ellipse = new Ellipse();
			ellipse.Width = 30;
			ellipse.Height = 30;
			ellipse.Stroke = Brushes.White;
			ellipse.Fill = new SolidColorBrush(Color.FromRgb((byte)rd.Next(0, 255), (byte)rd.Next(0, 255), (byte)rd.Next(0, 255)));
			TouchPoint touchPoint = e.GetTouchPoint(touchPad);
			Canvas.SetTop(ellipse, touchPoint.Bounds.Top);
			Canvas.SetLeft(ellipse, touchPoint.Bounds.Left);
			movingEllipses[e.TouchDevice.Id] = ellipse;
			touchPad.Children.Add(ellipse);
		}

		/// <summary>
		/// 当手指离开触屏时TouchUp 事件将被触发，
		/// 首先将触碰设备从movingEllipses 集合中删除不再跟踪手指相关操作，
		/// 并从<Canvas> 中将彩色圆圈移除。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void touchPad_TouchUp(object sender, TouchEventArgs e)
		{
			movingEllipses.Remove(e.TouchDevice.Id);
			Ellipse ellipse = movingEllipses[e.TouchDevice.Id];
			touchPad.Children.Remove(ellipse);
		}

		/// <summary>
		/// 当手指在触屏上持续移动时TouchMove 事件触发，它来跟踪手指移动轨迹，并重新调整圆圈在<Canvas> 中的位置。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void touchPad_TouchMove(object sender, TouchEventArgs e)
		{
			Ellipse ellipse = movingEllipses[e.TouchDevice.Id];
			TouchPoint touchPoint = e.GetTouchPoint(touchPad);
			Canvas.SetTop(ellipse, touchPoint.Bounds.Top);
			Canvas.SetLeft(ellipse, touchPoint.Bounds.Left);
		}
	}
}
