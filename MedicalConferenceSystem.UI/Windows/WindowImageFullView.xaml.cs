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
using System.IO;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using MedicalConferenceSystem.UI.Windows;

namespace MedicalConferenceSystem.UI
{
	/// <summary>
	/// WindowImageFullView.xaml 的交互逻辑
	/// </summary>
	public partial class WindowImageFullView : Window
	{
		#region 变量
		double ucWidth;
		double ucHeight;
		double aniTime = 0.3;
		int currentIndex;
		int pageCount;
		bool isClosed = false;
		List<Storyboard> listStoryHide;
		List<Storyboard> listStoryShow;
		ObservableCollection<UCFullImage> collectionUCImage = new ObservableCollection<UCFullImage>();
		TouchPoint touchPointOld;
		bool isMultipeTouch = false;
		List<int> listDeviceID = new List<int>();
		List<string> listImagePath = new List<string>();
		bool isEditing = false;
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
		public WindowImageFullView()
		{
			this.InitializeComponent();

			// 在此点之下插入创建对象所需的代码。
		}
		#endregion

		#region 业务
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			CanvasMainTR = new TranslateTransform();

			ucWidth = this.BorderCenter.ActualWidth;
			ucHeight = this.BorderCenter.ActualHeight;

			double xLocation = 0;

			string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Images";
			foreach (string path in Directory.GetFileSystemEntries(filePath))
			{
				listImagePath.Add(path);
				UCFullImage ucFull = new UCFullImage();
				ucFull.BeginEditEvent += ucFull_BeginEditEvent;
				ucFull.BeginMoveEvent += new Action<MoveType>(ucFull_BeginMoveEvent);
				ucFull.Width = ucWidth;
				ucFull.Height = ucHeight;
				//ucFull.SetBackImage(path);
				CanvasMain.Children.Add(ucFull);
				Canvas.SetLeft(ucFull, xLocation);
				Canvas.SetTop(ucFull, 0);
				xLocation += ucWidth;
			}

			pageCount = CanvasMain.Children.Count;

			CanvasMain.Width = ucWidth * pageCount;

			LoadImage(currentIndex);
			LoadImage(currentIndex + 1);
			//InitAnimation();

			BeginLoadWindowAnimation();
		}

		void ucFull_BeginMoveEvent(MoveType obj)
		{
			//if (obj == MoveType.Left)
			//{
			//    BeginMove(MoveType.Left);//左移动画
			//}
			//else
			//{
			//    BeginMove(MoveType.Right);//右移动画
			//}
		}

		void ucFull_BeginEditEvent(bool obj)
		{
			isEditing = obj;
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

		private void LoadImage(int index)
		{
			if (index > -1)
			{
				((UCFullImage)CanvasMain.Children[index]).SetBackImage(listImagePath[index]);
			}
		}

		private void RemoveImage(int index)
		{
			if (index > -1)
			{
				((UCFullImage)CanvasMain.Children[index]).ReleaseBackImage();
			}
		}

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

		private void ScrollViewrCenter_TouchDown(object sender, TouchEventArgs e)
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

		private void ScrollViewrCenter_TouchUp(object sender, TouchEventArgs e)
		{
			//if (e.TouchDevice.Captured == null)//CanvasMain事件
			//{
				TouchPoint touchPointNew = e.GetTouchPoint(BorderCenter);
				double offsetX = touchPointNew.Bounds.Left - touchPointOld.Bounds.Left;//判断X轴位移

				//((UCFullImage)CanvasMain.Children[currentIndex]).ResetImage();

				if (offsetX < -10)//左移
				{
					BeginMove(MoveType.Left);//左移动画
				}
				else if (offsetX > 10)//右移
				{
					BeginMove(MoveType.Right);//右移动画
				}
			//}
			//else
			//{
			//    if (!isMultipeTouch && !isEditing)//单点时进行平移并且不处于编辑状态
			//    {
			//        TouchPoint touchPointNew = e.GetTouchPoint(BorderCenter);
			//        double offsetX = touchPointNew.Bounds.Left - touchPointOld.Bounds.Left;//判断X轴位移

			//        //((UCFullImage)CanvasMain.Children[currentIndex]).ResetImage();

			//        if (offsetX < -10)//左移
			//        {
			//            BeginMove(MoveType.Left);//左移动画
			//        }
			//        else if (offsetX > 10)//右移
			//        {
			//            BeginMove(MoveType.Right);//右移动画
			//        }
			//    }
			//}

			listDeviceID.Remove(e.TouchDevice.Id);

			//if (touchPointOld.Bounds.Left == touchPointNew.Bounds.Left)
			//{
			//    WindowSignleImage winImage = new WindowSignleImage();
			//    winImage.SetBackImage(((System.Windows.Controls.Image)((e.TouchDevice).DirectlyOver)).Source);
			//    winImage.ShowDialog();
			//}
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

			Storyboard sbMove = new Storyboard();
			sbMove.Completed += (o, s) =>
			{
				if (currentIndex > 0 && currentIndex < pageCount - 1)
				{
					Console.WriteLine("X");
					LoadImage(currentIndex + 1);
					//LoadImage(currentIndex - 1);
				}

				if (moveType == MoveType.Left)
				{
					((UCFullImage)CanvasMain.Children[currentIndex - 1]).ResetImage();
					//RemoveImage(currentIndex - 1);
				}
				else
				{
					((UCFullImage)CanvasMain.Children[currentIndex + 1]).ResetImage();
					//RemoveImage(currentIndex + 1);
				}
			};

			sbMove.Children.Clear();
			sbMove.Children.Add(daMove);
			sbMove.Begin(this);
		}

		#region OldAniamtion
		private void InitAnimation()
		{
			//foreach (UCFullImage item in StackPanelCenter.Children)
			//{
			//    CreateHideAnimation(item);
			//    CreateShowAnimation(item);
			//}
		}

		private void CreateHideAnimation(UCFullImage ucFullIma)
		{
			DoubleAnimation da = new DoubleAnimation();
			da.To = 0;
			da.Duration = TimeSpan.FromSeconds(0.3);

			Storyboard.SetTarget(da, ucFullIma);
			Storyboard.SetTargetProperty(da, new PropertyPath(WidthProperty));

			Storyboard sbTurnPage = new Storyboard();
			sbTurnPage.Children.Add(da);

			listStoryHide.Add(sbTurnPage);
		}

		private void CreateShowAnimation(UCFullImage ucFullIma)
		{
			DoubleAnimation da = new DoubleAnimation();
			da.To = ucWidth;
			da.Duration = TimeSpan.FromSeconds(0.3);

			Storyboard.SetTarget(da, ucFullIma);
			Storyboard.SetTargetProperty(da, new PropertyPath(WidthProperty));

			Storyboard sbTurnPage = new Storyboard();
			sbTurnPage.Children.Add(da);

			listStoryShow.Add(sbTurnPage);
		}

		private void BeginHideAnimation(int index)
		{
			listStoryHide[index].Begin();
		}

		private void BeginShowAnimation(int index)
		{
			listStoryShow[index].Begin();
		}

		private void btnPreview_Click(object sender, RoutedEventArgs e)
		{
			BeginMove(MoveType.Left);//左移动画
		}

		private void btnNext_Click(object sender, RoutedEventArgs e)
		{
			BeginMove(MoveType.Right);//右移动画
		}
		#endregion

		private void Button_TouchUp(object sender, TouchEventArgs e)
		{
			this.Close();
		}
		#endregion

		private void Button_TouchUp_1(object sender, TouchEventArgs e)
		{
			this.Close();
		}

		private void Button_TouchUp_2(object sender, TouchEventArgs e)
		{
			BeginMove(MoveType.Left);//左移动画
		}

		private void Button_TouchUp_3(object sender, TouchEventArgs e)
		{
			BeginMove(MoveType.Right);//右移动画
		}
	}
}