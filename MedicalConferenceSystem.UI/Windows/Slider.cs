using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Input;

namespace MedicalConferenceSystem.UI.Windows
{
	class Slider : Canvas
	{
		#region 变量
		private Size extent = new Size();
		private Size viewport = new Size();
		private Size thisSize = new Size();
		private TranslateTransform translate = new TranslateTransform();
		private double x;
		private TranslateTransform transform;
		private int page;
		private int index;
		#endregion

		#region 构造函数
		public Slider()
		{
			this.Background = Brushes.LightBlue;
			this.RenderTransform = translate;
			this.MouseDown += new MouseButtonEventHandler(Slider_MouseDown);
			this.MouseUp += new MouseButtonEventHandler(Slider_MouseUp);
			this.RenderTransform = (transform = new TranslateTransform());
		}
		#endregion

		#region 业务
		private void Slider_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.CaptureMouse();
			Point p = e.GetPosition(App.Current.MainWindow);
			x = p.X;
		}

		private void Slider_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (this.IsMouseCaptured)
			{
				Point p = e.GetPosition(App.Current.MainWindow);
				var offsetX = p.X - x;
				if (offsetX > 10)
				{
					if (index > 0) { --index; }
					Go();
				}
				if (offsetX < -10)
				{
					if (index < page - 1) { ++index; }
					Go();
				}
				this.ReleaseMouseCapture();
			}
		}
		private void Go()
		{
			DoubleAnimation a = new DoubleAnimation(-index * viewport.Width, TimeSpan.FromMilliseconds(500));
			a.AccelerationRatio = .3;
			a.DecelerationRatio = .3;
			transform.BeginAnimation(TranslateTransform.XProperty, a);
		}

		protected override Size MeasureOverride(Size constraint)
		{
			thisSize = constraint;
			double dWidth = Math.Floor(constraint.Width / constraint.Width);
			double dHeight = Math.Floor(constraint.Height / constraint.Height);
			Size s = new Size(Math.Ceiling(InternalChildren.Count / (dWidth * dHeight)) * constraint.Width, constraint.Height);

			Size extentTmp = new Size(s.Width * this.InternalChildren.Count, constraint.Height);
			foreach (UIElement each in InternalChildren)
			{
				each.Measure(constraint);
			}
			if (extentTmp != extent)
			{
				extent = s;
			}
			if (viewport != constraint)
			{
				viewport = constraint;
			}
			return s;
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			int count = (int)Math.Floor(viewport.Width / thisSize.Width);
			page = InternalChildren.Count;
			double xLocation = 0;

			try
			{
				for (int i = 0; i < InternalChildren.Count; i++)
				{
					this.InternalChildren[i].Arrange(new Rect(xLocation, 0, thisSize.Width, thisSize.Height));
					xLocation += thisSize.Width;
				}
			}
			catch
			{ }

			return arrangeSize;
		}
		#endregion
	}
}
