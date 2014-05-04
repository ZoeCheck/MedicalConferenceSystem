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
using System.Linq;
using System.Windows.Media.Animation;

namespace MedicalConferenceSystem.UI
{
	/// <summary>
	/// UCFullImage.xaml 的交互逻辑
	/// </summary>
	public partial class UCFullImage : UserControl
	{
		#region 变量
		public int numUC;
		public string m_ImagePath;
		Matrix matrixInit;
		private double originX = 1;
		private double originY = 1;
		TouchPoint touchPointOld;
		bool isEditing = false;
		TimeSpan tsResetTime = TimeSpan.FromSeconds(0.2);
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
			//BitmapImage bitmapImage = new BitmapImage();
			//bitmapImage.BeginInit();
			//bitmapImage.UriSource = new Uri(m_ImagePath);
			//bitmapImage.EndInit();
			//this.ImageMain.Source = bitmapImage;
			this.ImageMain.Source = new BitmapImage(new Uri(m_ImagePath, UriKind.Absolute));
		}

		public void ReleaseBackImage()
		{
			this.ImageMain.Source = null;
		}

		public void ResetImage()
		{
			originX = 1;
			originY = 1;
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
			FrameworkElement element = (FrameworkElement)e.Source;
			Matrix matrix = ((MatrixTransform)element.RenderTransform).Matrix;
			ManipulationDelta deltaManipulation = e.DeltaManipulation;
			Point center = e.ManipulationOrigin;
			originX = originX * deltaManipulation.Scale.X;
			originY = originY * deltaManipulation.Scale.Y;

			if (originX <= 1 && !isEditing)//初始状态
			{
				OnBeginEditEvent(false);
			}
			else//编辑状态
			{
				isEditing = true;
				OnBeginEditEvent(true);
				if (originX > 1 && e.Manipulators.Count() > 1)
				{
					matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);//缩放
					((MatrixTransform)element.RenderTransform).Matrix = matrix;
				}

				matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
				((MatrixTransform)element.RenderTransform).Matrix = matrix;

				//Matrix matrixX = matrix;//复制X轴矩阵副本
				//Matrix matrixY;//定义Y轴矩阵副本
				////X轴平移
				//matrixX.Translate(e.DeltaManipulation.Translation.X, 0);//X轴平移
				//if (matrixX.OffsetX >= -offX && matrixX.OffsetX <= 0)//在范围内
				//{
				//    ((MatrixTransform)element.RenderTransform).Matrix = matrixX;//替换矩阵
				//    matrixY = matrixX;
				//}
				//else//在范围外
				//{
				//    matrixY = matrix;
				//}

				////Y轴平移
				//matrixY.Translate(0, e.DeltaManipulation.Translation.Y);//Y轴平移
				//if (matrixY.OffsetY >= -offY && matrixY.OffsetY <= 0)//在范围内
				//{
				//    ((MatrixTransform)element.RenderTransform).Matrix = matrixY;//替换矩阵
				//}
			}
		}

		private void UserControl_TouchDown(object sender, TouchEventArgs e)
		{
			touchPointOld = e.GetTouchPoint(this);
		}

		private void UserControl_TouchUp(object sender, TouchEventArgs e)
		{
			TouchPoint touchPointNew = e.GetTouchPoint(this);

			if (touchPointNew.Position == touchPointOld.Position || originX <= 1)
			{
				ResetImage();
				OnBeginEditEvent(false);
			}

			//判断有没有超出边界
			double offX = this.ActualWidth * (originX - 1);//可以左右移动的最大值
			double offY = this.ActualHeight * (originY - 1);//可以上下移动的最大值
			Matrix matrixOld = ((MatrixTransform)ImageMain.RenderTransform).Matrix;
			Matrix toMatrix = new Matrix();
			toMatrix.M11 = matrixOld.M11;
			toMatrix.M12 = matrixOld.M12;
			toMatrix.M21 = matrixOld.M21;
			toMatrix.M22 = matrixOld.M22;
			if (matrixOld.OffsetX < -offX && matrixOld.OffsetY >= -offY && matrixOld.OffsetY < 0)//只有右边界溢出，即 < -offX
			{
				//matrixNew.OffsetX = -offX;
				toMatrix.OffsetX = -offX;
				toMatrix.OffsetY = matrixOld.OffsetY;

				PlayMatrixTransformAnimation((MatrixTransform)ImageMain.RenderTransform, toMatrix, tsResetTime);
			}
			if (matrixOld.OffsetX > 0 && matrixOld.OffsetY >= -offY && matrixOld.OffsetY < 0)//只有左边界溢出，即>0
			{
				//matrixOld.OffsetX = 0;
				toMatrix.OffsetX = 0;
				toMatrix.OffsetY = matrixOld.OffsetY;

				PlayMatrixTransformAnimation((MatrixTransform)ImageMain.RenderTransform, toMatrix, tsResetTime);
			}
			if (matrixOld.OffsetY < -offY && matrixOld.OffsetX >= -offX && matrixOld.OffsetX < 0)//只有下边界溢出，即 < -offY
			{
				//matrixOld.OffsetY = -offY;
				toMatrix.OffsetX = matrixOld.OffsetX;
				toMatrix.OffsetY = -offY;

				PlayMatrixTransformAnimation((MatrixTransform)ImageMain.RenderTransform, toMatrix, tsResetTime);
			}
			if (matrixOld.OffsetY > 0 && matrixOld.OffsetX >= -offX && matrixOld.OffsetX < 0)//只有上边界溢出，即>0
			{
				//matrixOld.OffsetY = 0;
				toMatrix.OffsetX = matrixOld.OffsetX;
				toMatrix.OffsetY = 0;

				PlayMatrixTransformAnimation((MatrixTransform)ImageMain.RenderTransform, toMatrix, tsResetTime);
			}
			if (matrixOld.OffsetX < -offX && matrixOld.OffsetY > 0)//右上溢出
			{
				toMatrix.OffsetX = -offX;
				toMatrix.OffsetY = 0;
				PlayMatrixTransformAnimation((MatrixTransform)ImageMain.RenderTransform, toMatrix, tsResetTime);
			}
			if (matrixOld.OffsetX < -offX && matrixOld.OffsetY < -offY)//右下溢出
			{
				toMatrix.OffsetX = -offX;
				toMatrix.OffsetY = -offY;
				PlayMatrixTransformAnimation((MatrixTransform)ImageMain.RenderTransform, toMatrix, tsResetTime);
			}
			if (matrixOld.OffsetX > 0 && matrixOld.OffsetY > 0)//左上溢出
			{
				toMatrix.OffsetX = 0;
				toMatrix.OffsetY = 0;
				PlayMatrixTransformAnimation((MatrixTransform)ImageMain.RenderTransform, toMatrix, tsResetTime);
			}
			if (matrixOld.OffsetX > 0 && matrixOld.OffsetY < -offY)//左下溢出
			{
				toMatrix.OffsetX = 0;
				toMatrix.OffsetY = -offY;
				PlayMatrixTransformAnimation((MatrixTransform)ImageMain.RenderTransform, toMatrix, tsResetTime);
			}

			//((MatrixTransform)this.ImageMain.RenderTransform).Matrix = matrixOld;
		}

		public static void PlayMatrixTransformAnimation(MatrixTransform matrixTransform, Matrix newMatrix, TimeSpan timeSpan)
		{
			var animation = new LinearMatrixAnimation(matrixTransform.Matrix, newMatrix, new Duration(timeSpan));
			animation.AccelerationRatio = 0.3;
			animation.DecelerationRatio = 0.3;
			animation.FillBehavior = FillBehavior.HoldEnd;
			animation.Completed += (sender, e) =>
			{
				//去除属性的动画绑定  
				matrixTransform.BeginAnimation(MatrixTransform.MatrixProperty, null);
				//将期望结果值保留  
				matrixTransform.Matrix = newMatrix;
			};

			//启动动画  
			matrixTransform.BeginAnimation(MatrixTransform.MatrixProperty, animation, HandoffBehavior.SnapshotAndReplace);
		}
		#endregion
	}

	public enum MoveType
	{
		Left,
		Right
	}
}