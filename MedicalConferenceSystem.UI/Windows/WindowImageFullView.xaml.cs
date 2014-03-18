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
		private TranslateTransform transform = new TranslateTransform();
		bool isMultipeTouch = false;
		List<int> listDeviceID = new List<int>();
		List<string> listImagePath = new List<string>();
		#endregion

		#region 委托事件

		#endregion

		#region 属性

		#endregion

		#region 构造函数
		public WindowImageFullView()
		{
			this.InitializeComponent();

			// 在此点之下插入创建对象所需的代码。
			this.CanvasMain.RenderTransform = transform;
		}
		#endregion

		#region 业务
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			ucWidth = this.BorderCenter.ActualWidth;
			ucHeight = this.BorderCenter.ActualHeight;

			double xLocation = 0;

			string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Images";
			foreach (string path in Directory.GetFileSystemEntries(filePath))
			{
				listImagePath.Add(path);
				UCFullImage ucFull = new UCFullImage();
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
			//InitAnimation();

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

		private void LoadImage(int index)
		{
			((UCFullImage)CanvasMain.Children[index]).SetBackImage(listImagePath[index]);
		}

		private void RemoveImage(int index)
		{
			((UCFullImage)CanvasMain.Children[index]).ReleaseBackImage();
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
			if (!isMultipeTouch)//非多点触控时
			{
				TouchPoint touchPointNew = e.GetTouchPoint(BorderCenter);
				double offsetX = touchPointNew.Bounds.Left - touchPointOld.Bounds.Left;//判断X轴位移

				((UCFullImage)CanvasMain.Children[currentIndex]).ResetImage();//重置缩放

				if (offsetX < -10)//左移
				{
					LoadImage(currentIndex + 1);
					BeginMove(MoveType.Left);//左移动画
					RemoveImage(currentIndex - 1);
				}
				else if (offsetX > 10)//右移
				{
					LoadImage(currentIndex - 1);
					BeginMove(MoveType.Right);//右移动画
					RemoveImage(currentIndex + 1);
				}
			}

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
			//开始平移动画
			DoubleAnimation a = new DoubleAnimation(-currentIndex * ucWidth, TimeSpan.FromMilliseconds(500));
			a.AccelerationRatio = 0.3;
			a.DecelerationRatio = 0.3;
			transform.BeginAnimation(TranslateTransform.XProperty, a);
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
			//if (currentIndex > 0)
			//{
			//    BeginShowAnimation(--currentIndex);
			//}
		}

		private void btnNext_Click(object sender, RoutedEventArgs e)
		{
			//if (currentIndex < pageCount - 1)
			//{
			//    BeginHideAnimation(currentIndex++);
			//}
		}
		#endregion
		#endregion
	}
}