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
		public int numUC;
		public string m_ImagePath;
		Matrix matrixInit;
		private double originX = 1;
		private double originY = 1;
		TouchPoint touchPointOld;
		bool isEditing = false;
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

		public event Action<bool> BeginEditEvent;
		public void OnBeginEditEvent(bool isEditing)
		{
			if (BeginEditEvent != null)
			{
				BeginEditEvent(isEditing);
			}
		}
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
			if (this.ImageMain.Source != null)
			{
				return;
			}
			//m_ImagePath = imgPath;
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
			originX = 1;
			isEditing = false;
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
			//FrameworkElement element = (FrameworkElement)e.Source;
			//Matrix matrix = ((MatrixTransform)element.RenderTransform).Matrix;
			//ManipulationDelta deltaManipulation = e.DeltaManipulation;
			//Point center = e.ManipulationOrigin;
			//originX = originX * deltaManipulation.Scale.X;
			//originY = originY * deltaManipulation.Scale.Y;
			////Console.WriteLine("matrix:{0}", matrix.OffsetX);

			//if (originX <= 1 && !isEditing)//初始状态
			//{
			//    OnBeginEditEvent(false);
			//}
			//else//编辑状态
			//{
			//    isEditing = true;
			//    OnBeginEditEvent(true);
			//    if (originX > 1)
			//    {
			//        matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);//缩放
			//        ((MatrixTransform)element.RenderTransform).Matrix = matrix;
			//    }

			//    //平移，判断有没有超出边界
			//    double offX = ImageMain.ActualWidth * (originX - 1);//可以左右移动的最大值
			//    double offY = ImageMain.ActualHeight * (originY - 1);//可以上下移动的最大值

			//    matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
			//    ////X轴平移
			//    //matrix.Translate(e.DeltaManipulation.Translation.X, 0);
			//    //if (matrix.OffsetX >= -offX && matrix.OffsetX <= 0)
			//    //{
			//    //    ((MatrixTransform)element.RenderTransform).Matrix = matrix;
			//    //}
			//    //Console.WriteLine("matrix.OffsetX:{0},-offX{1}", matrix.OffsetX, -offX);
			//    ////Y轴平移
			//    //matrix.Translate(0, e.DeltaManipulation.Translation.Y);
			//    //if (matrix.OffsetY >= -offY && matrix.OffsetY <= 0)
			//    //{
			//    //    ((MatrixTransform)element.RenderTransform).Matrix = matrix;
			//    //}
			//    ((MatrixTransform)element.RenderTransform).Matrix = matrix;

			//    //翻页
			//    //if (matrix.OffsetX < -ImageMain.ActualWidth / 2)
			//    //{
			//    //    OnBeginMoveEvent(MoveType.Left);
			//    //}
			//    //else if (matrix.OffsetX > ImageMain.ActualWidth / 2)
			//    //{
			//    //    OnBeginMoveEvent(MoveType.Right);
			//    //}
			//    //matrix.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);
			//}
		}

		private void UserControl_TouchDown(object sender, TouchEventArgs e)
		{
			touchPointOld = e.GetTouchPoint(this);
		}

		private void UserControl_TouchUp(object sender, TouchEventArgs e)
		{
			//TouchPoint touchPointNew = e.GetTouchPoint(this);

			//if (touchPointNew.Position == touchPointOld.Position)
			//{
			//    ResetImage();
			//    OnBeginEditEvent(false);
			//}
		}
		#endregion
	}

	public enum MoveType
	{
		Left,
		Right
	}
}