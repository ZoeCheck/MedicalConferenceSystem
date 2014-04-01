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
using System.IO;

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
		TouchPoint touchPointOld;
		int pageCount;
		List<int> listDeviceID = new List<int>();
		bool isMultipeTouch = false;
		int currentIndex;
		private Storyboard sbMove = new Storyboard();
		double aniTime = 0.3;
		List<string> listImagePath;
		List<List<string>> ListImageMain = new List<List<string>>();
		#endregion

		#region 委托事件

		#endregion

		#region 属性
		private TranslateTransform CanvasMainTR
		{
			get
			{
				return this.CanvasMain.RenderTransform as TranslateTransform;
			}
			set
			{
				this.CanvasMain.RenderTransform = value;
			}
		}
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
			CanvasMainTR = new TranslateTransform();

			ucWidth = this.BorderCenter.ActualWidth; ;
			ucHeight = this.BorderCenter.ActualHeight;

			double xLocation = 0;
			int count = 0;
			string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Images";
			foreach (string path in Directory.GetFileSystemEntries(filePath))
			{
				if (count == 0)
				{
					listImagePath = new List<string>();
				}
				listImagePath.Add(path);
				count++;
				if (count >= 4)
				{
					count = 0;
					ListImageMain.Add(listImagePath);
				}
			}

			pageCount = ListImageMain.Count;

			CanvasMain.Width = ucWidth * pageCount;

			for (int i = 0; i < pageCount; i++)
			{
				UCImageList ucIma = new UCImageList();
				ucIma.Width = ucWidth;
				ucIma.Height = ucHeight;
				CanvasMain.Children.Add(ucIma);
				Canvas.SetLeft(ucIma, xLocation);
				Canvas.SetTop(ucIma, 0);
				xLocation += ucWidth;
				//ucIma.SetBackImage(ListImageMain[i]);
			}

			LoadUCImage(0);
			LoadUCImage(1);

			BeginLoadWindowAnimation();
		}

		private void LoadUCImage(int pageIndex)
		{
			if (pageIndex > -1 && pageIndex < pageCount - 1)
			{
				((UCImageList)CanvasMain.Children[pageIndex]).SetBackImage(ListImageMain[pageIndex]);
			}
		}

		private void RemoveUCImage(int pageIndex)
		{
			if (pageIndex > -1 && pageIndex < pageCount - 1)
			{
				((UCImageList)CanvasMain.Children[pageIndex]).ReleaseBackImage();
			}
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
		///// <summary>
		///// 加载渐大动画
		///// </summary>
		//private void BeginLoadWindowAnimation()
		//{
		//    this.Opacity = 0;


		//    STMainWindow.ScaleX = 0;
		//    STMainWindow.ScaleY = 0;

		//    NameScope.SetNameScope(this, new NameScope());
		//    this.RegisterName("scale", STMainWindow);

		//    Storyboard sb = new Storyboard();

		//    DoubleAnimation daX = new DoubleAnimation();
		//    daX.To = 1;
		//    daX.Duration = TimeSpan.FromSeconds(aniTime);

		//    DoubleAnimation daY = new DoubleAnimation();
		//    daY.To = 1;
		//    daY.Duration = TimeSpan.FromSeconds(aniTime);

		//    DoubleAnimation daOp = new DoubleAnimation();
		//    daOp.To = 1;
		//    daOp.Duration = TimeSpan.FromSeconds(aniTime);

		//    Storyboard.SetTargetName(daX, "scale");
		//    Storyboard.SetTargetProperty(daX, new PropertyPath(ScaleTransform.ScaleXProperty));
		//    Storyboard.SetTargetName(daY, "scale");
		//    Storyboard.SetTargetProperty(daY, new PropertyPath(ScaleTransform.ScaleYProperty));
		//    Storyboard.SetTarget(daOp, this);
		//    Storyboard.SetTargetProperty(daOp, new PropertyPath(UIElement.OpacityProperty));

		//    sb.Children.Add(daX);
		//    sb.Children.Add(daY);
		//    sb.Children.Add(daOp);

		//    sb.Begin(this);
		//}

		///// <summary>
		///// 关闭窗体渐小动画
		///// </summary>
		//private void BeginClodeWindowAnimation()
		//{
		//    Storyboard sB = new Storyboard();
		//    sB.Completed += (s, ee) =>
		//    {
		//        this.Close();
		//    };

		//    DoubleAnimation daX = new DoubleAnimation();
		//    daX.To = 0;
		//    daX.Duration = TimeSpan.FromSeconds(aniTime);

		//    DoubleAnimation daY = new DoubleAnimation();
		//    daY.To = 0;
		//    daY.Duration = TimeSpan.FromSeconds(aniTime);

		//    DoubleAnimation daOp = new DoubleAnimation();
		//    daOp.To = 0;
		//    daOp.Duration = TimeSpan.FromSeconds(aniTime);

		//    Storyboard.SetTargetName(daX, "scale");
		//    Storyboard.SetTargetProperty(daX, new PropertyPath(ScaleTransform.ScaleXProperty));
		//    Storyboard.SetTargetName(daY, "scale");
		//    Storyboard.SetTargetProperty(daY, new PropertyPath(ScaleTransform.ScaleYProperty));
		//    Storyboard.SetTarget(daOp, this);
		//    Storyboard.SetTargetProperty(daOp, new PropertyPath(UIElement.OpacityProperty));

		//    sB.Children.Add(daX);
		//    sB.Children.Add(daY);
		//    sB.Children.Add(daOp);

		//    sB.Begin(this);
		//} 
		#endregion

		/// <summary>
		/// 从右往左渐入动画
		/// </summary>
		private void BeginLoadWindowAnimation()
		{
			//设置初始位置
			STMainWindow.X = ucWidth;
			this.Opacity = 0;

			NameScope.SetNameScope(this, new NameScope());
			this.RegisterName("scale", STMainWindow);

			Storyboard sb = new Storyboard();

			DoubleAnimation daX = new DoubleAnimation();
			daX.To = 0;
			daX.Duration = TimeSpan.FromSeconds(aniTime);

			DoubleAnimation daOp = new DoubleAnimation();
			daOp.To = 1;
			daOp.Duration = TimeSpan.FromSeconds(aniTime);

			Storyboard.SetTargetName(daX, "scale");
			Storyboard.SetTargetProperty(daX, new PropertyPath(TranslateTransform.XProperty));

			Storyboard.SetTargetProperty(daOp, new PropertyPath(UIElement.OpacityProperty));

			sb.Children.Add(daX);
			sb.Children.Add(daOp);

			sb.Begin(this);
		}

		/// <summary>
		/// 从左往右渐出动画
		/// </summary>
		private void BeginClodeWindowAnimation()
		{
			Storyboard sB = new Storyboard();
			sB.Completed += (s, ee) =>
			{
				this.Close();
			};
			DoubleAnimation daX = new DoubleAnimation();
			daX.To = ucWidth;
			daX.Duration = TimeSpan.FromSeconds(aniTime);

			Storyboard.SetTargetName(daX, "scale");
			Storyboard.SetTargetProperty(daX, new PropertyPath(TranslateTransform.XProperty));

			sB.Children.Add(daX);
			sB.Begin(this);
		}

		private void ScrollViewerCenter_TouchDown(object sender, TouchEventArgs e)
		{
			touchPointOld = e.GetTouchPoint(BorderCenter);

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

		private void ScrollViewerCenter_TouchUp(object sender, TouchEventArgs e)
		{
			TouchPoint touchPointNew = e.GetTouchPoint(BorderCenter);
			double offsetX = touchPointNew.Bounds.Left - touchPointOld.Bounds.Left;//判断X轴位移

			if (currentIndex > 0 && currentIndex < pageCount - 1)
			{
				LoadUCImage(currentIndex + 1);
				LoadUCImage(currentIndex - 1);
				RemoveUCImage(currentIndex + 2);
				RemoveUCImage(currentIndex - 2);
			}

			if (offsetX < -10)//左移
			{
				BeginMove(MoveType.Left);//左移动画
			}
			else if (offsetX > 10)//右移
			{
				BeginMove(MoveType.Right);//右移动画
			}
			else if (offsetX == 0)//单点弹窗
			{
				string path = ((System.Windows.Controls.Image)((e.TouchDevice).Captured)).Source.ToString();
				path = path.Substring(8);
				path = path.Replace('/','\\');
				WindowImageFullView windowFull = new WindowImageFullView(path);
				windowFull.ShowDialog();
			}

			listDeviceID.Remove(e.TouchDevice.Id);
		}

		private void BeginMove(MoveType moveType)
		{
			if (moveType == MoveType.Left && currentIndex < pageCount - 1)
			{
				++currentIndex;
			}
			else if (moveType == MoveType.Right && currentIndex > 0)
			{
				--currentIndex;
			}

			DoMoveAnimation(moveType);
		}

		private void DoMoveAnimation(MoveType moveType)
		{
			//开始平移动画
			DoubleAnimation daMove = new DoubleAnimation(-currentIndex * ucWidth, TimeSpan.FromMilliseconds(500));
			daMove.AccelerationRatio = 0.3;
			daMove.DecelerationRatio = 0.3;
			//CanvasMainTR.BeginAnimation(TranslateTransform.XProperty, daMove);

			this.RegisterName("tr", CanvasMainTR);

			Storyboard.SetTargetName(daMove, "tr");
			Storyboard.SetTargetProperty(daMove, new PropertyPath(TranslateTransform.XProperty));

			sbMove.Completed += (o, s) =>
			{
				if (currentIndex > 0 && currentIndex < pageCount - 1)
				{
				}

				if (moveType == MoveType.Left)
				{
				}
				else
				{
				}
			};

			sbMove.Children.Clear();
			sbMove.Children.Add(daMove);
			sbMove.Begin(this);
		}

		private void Button_TouchUp(object sender, TouchEventArgs e)
		{
			this.Close();

		}

		private void Button_TouchUp_1(object sender, TouchEventArgs e)
		{
			this.Close();
		}

		private void Button_TouchUp_2(object sender, TouchEventArgs e)
		{
			WindowImageFullView windowFull = new WindowImageFullView();
			windowFull.ShowDialog();
		}
		#endregion
	}
}