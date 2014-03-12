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
		}
		#endregion

		#region 业务
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//listStoryHide = new List<Storyboard>();
			//listStoryShow = new List<Storyboard>();
			int num = 0;
			ucWidth = this.StackPanelCenter.ActualWidth;
			ucHeight = this.StackPanelCenter.ActualHeight;

			string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Images";
			foreach (string path in Directory.GetFileSystemEntries(filePath))
			{
				UCFullImage ucFull = new UCFullImage();
				ucFull.Width = ucWidth;
				ucFull.Height = ucHeight;
				ucFull.SetBackImage(path);
				StackPanelCenter.Children.Add(ucFull);
				pageCount++;

				ucFull.IsManipulationEnabled = true;
				ucFull.ImageControlEvent += ucFull_ImageControlEvent;
				ucFull.numUC = num++;
			}

			//InitAnimation();

			//BeginLoadWindowAnimation();
		}

		void ucFull_ImageControlEvent(bool isImageControl,int numUC)
		{
			if (isImageControl)//图片缩放
			{
				try
				{
					this.ScrollViewrCenter.PanningMode = PanningMode.None;
					Console.WriteLine(numUC + "缩放");
				}
				catch 
				{
				}
			}
			else//列表水平滚动
			{
				this.ScrollViewrCenter.PanningMode = PanningMode.HorizontalOnly;
				Console.WriteLine(numUC + "平移");
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//if (!isClosed)
			//{
			//    e.Cancel = true;
			//    BeginClodeWindowAnimation();
			//    isClosed = true;
			//}
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

		private void InitAnimation()
		{
			foreach (UCFullImage item in StackPanelCenter.Children)
			{
				CreateHideAnimation(item);
				CreateShowAnimation(item);
			}
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
			if (currentIndex > 0)
			{
				BeginShowAnimation(--currentIndex);
			}
		}

		private void btnNext_Click(object sender, RoutedEventArgs e)
		{
			if (currentIndex < pageCount - 1)
			{
				BeginHideAnimation(currentIndex++);
			}
		}
		#endregion


	}
}