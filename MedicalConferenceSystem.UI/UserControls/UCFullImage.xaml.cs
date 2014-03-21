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
		List<int> listDeviceID = new List<int>();
		bool isMuiltTouch = false;
		bool isAttached = false;
		public int numUC;
		public string m_ImagePath;
		TranslateZoomRotateBehavior translateZoomRotateBehavior;
		TouchPoint touchPointOld;
		bool isMultipeTouch = false;
		Matrix matrixInit;
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

		public event Action<bool> NotifyScaleStateEvent;
		public void OnNotifyScaleState(bool isScaling)
		{
			if (NotifyScaleStateEvent != null)
			{
				NotifyScaleStateEvent(isScaling);
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
			this.Dispatcher.BeginInvoke(new Action(() =>
			{
				SetBackImage();
			}));
			//BitmapImage bitmapImage = new BitmapImage();
			//bitmapImage.BeginInit();
			//bitmapImage.UriSource = new Uri(imgPath);
			//bitmapImage.EndInit();
			//imagePath = imgPath;
			//this.ImageMain.Source = bitmapImage;
			////this.ImageMain.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
		}

		public void SetBackImage()
		{
			//m_ImagePath = imgPath;
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.UriSource = new Uri(m_ImagePath);
			bitmapImage.EndInit();
			this.ImageMain.Source = bitmapImage;
			//this.ImageMain.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
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
		}

		private void ImageMain_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			if (isMultipeTouch)
			{
				FrameworkElement element = (FrameworkElement)e.Source;
				Matrix matrix = ((MatrixTransform)element.RenderTransform).Matrix;
				ManipulationDelta deltaManipulation = e.DeltaManipulation;
				Point center = new Point(element.ActualWidth / 2, element.ActualHeight / 2);
				center = matrix.Transform(center);
				matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
				//matrix.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);
				matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
				((MatrixTransform)element.RenderTransform).Matrix = matrix;
			}
		}

		private void ImageMain_TouchDown(object sender, TouchEventArgs e)
		{
			if (!listDeviceID.Contains(e.TouchDevice.Id))
			{
				listDeviceID.Add(e.TouchDevice.Id);
			}

			if (listDeviceID.Count >= 2)//多点缩放
			{
				isMultipeTouch = true;
			}
			else
			{
				isMultipeTouch = false;
			}
		}

		private void ImageMain_TouchUp(object sender, TouchEventArgs e)
		{
			listDeviceID.Remove(e.TouchDevice.Id);
		}
		#endregion
	}

	public enum MoveType
	{
		Left,
		Right
	}
}