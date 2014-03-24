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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace MedicalConferenceSystem.UI.UserControls
{
	/// <summary>
	/// 作者：xxh 
	/// 时间：2014-03-24 10:43:06
	/// 版本：V1.0.0 	 
	/// </summary>
	public partial class UCType : UserControl
	{
		#region 变量

		#endregion

		#region 委托事件

		#endregion

		#region 属性

		#endregion

		#region 构造函数
		public UCType()
		{
			InitializeComponent();
		}
		#endregion

		#region 业务
		public void SetImage(string imagePath)
		{
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.UriSource = new Uri(imagePath);
			bitmapImage.EndInit();
			this.ImageMain.Source = bitmapImage;
		}

		public void SetInfoUp(string info)
		{
			tblUp.Text = info;
		}

		public void SetInfoDown(string info)
		{
			tblDown.Text = info;
		}

		public void BeginLoadAni()
		{
			Storyboard sbLoad = this.FindResource("StoryboardLoad") as Storyboard;
			if (sbLoad != null)
			{
				sbLoad.Begin();
			}
		}
		#endregion
	}
}
