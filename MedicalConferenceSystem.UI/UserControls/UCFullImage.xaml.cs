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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Expression.Interactivity.Input;

namespace MedicalConferenceSystem.UI
{
	/// <summary>
	/// UCFullImage.xaml 的交互逻辑
	/// </summary>
	public partial class UCFullImage : UserControl
	{
		#region 变量
		bool isAttached = false;
		public int numUC;
		public string m_ImagePath;
		TranslateZoomRotateBehavior translateZoomRotateBehavior;
		TouchPoint touchPointOld;
		Matrix matrixInit;
		private double originX = 1;
		#endregion

		#region 委托事件
		public event Action<bool, int> ImageControlEvent;
		public void OnImageControlEvent(bool isImageControl, int num)
		{
			if (ImageControlEvent != null)
			{
				ImageControlEvent(isImageControl, num);
			}
		}

		public event Action<MoveType> BeginMoveEvent;
		public void OnBeginMoveEvent(MoveType moveType)
		{
			if (BeginMoveEvent != null)
			{
				BeginMoveEvent(moveType);
			}
		}
		#endregion

		#region 属性

		#endregion

		#region 构造函数
		public UCFullImage()
		{
			this.InitializeComponent();
			matrixInit = ((MatrixTransform)ImageMain.RenderTransform).Matrix;
		}
		#endregion

		#region 业务
		public void SetBackImage(string imgPath)
		{
			m_ImagePath = imgPath;
			Application.Current.Dispatcher.BeginInvoke(new Action(() =>
			{
				SetBackImage();
			}));
		}

		public void SetBackImage()
		{
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.UriSource = new Uri(m_ImagePath);
			bitmapImage.EndInit();
			this.ImageMain.Source = bitmapImage;
			//this.ImageMain.Source = new BitmapImage(new Uri(m_ImagePath, UriKind.Absolute));
		}

		public void ReleaseBackImage()
		{
			this.ImageMain.Source = null;
		}

		public void ResetImage()
		{
			ImageMain.RenderTransform = new MatrixTransform();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			ImageMain.IsManipulationEnabled = true;
			ImageMain.RenderTransform = new MatrixTransform();
		}

		private void ImageMain_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
		{
			e.Mode = ManipulationModes.All;
			e.ManipulationContainer = LayoutRoot;
			e.IsSingleTouchEnabled = false;
		}

		private void ImageMain_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			FrameworkElement element = (FrameworkElement)e.Source;
			Matrix matrix = ((MatrixTransform)element.RenderTransform).Matrix;
			ManipulationDelta deltaManipulation = e.DeltaManipulation;
			Point center = new Point(element.ActualWidth / 2, element.ActualHeight / 2);
			center = matrix.Transform(center);
			originX = originX * deltaManipulation.Scale.X;
			if (originX >= 1)
			{
				matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
			}
			//matrix.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);
			matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
			((MatrixTransform)element.RenderTransform).Matrix = matrix;
		}
		#endregion
	}

	public enum MoveType
	{
		Left,
		Right
	}
}