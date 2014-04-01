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

namespace MedicalConferenceSystem.UI
{
	/// <summary>
	/// UCImageList.xaml 的交互逻辑
	/// </summary>
	public partial class UCImageList : UserControl
	{
		public UCImageList()
		{
			this.InitializeComponent();
		}

		internal void SetBackImage(List<string> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
			//    this.Dispatcher.BeginInvoke(new Action(() =>
			//    {
			//        SetBackImageNew(i, list[i]);
			//    }));
				SetBackImageNew(i, list[i]);
			}
		}

		public void SetBackImageNew(int index, string imgPath)
		{
			Image ima = GridImage.Children[index] as Image;
			if (ima.Source != null)
			{
				return;
			}
			ima.Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute));

			//BitmapImage bitmapImage = new BitmapImage();
			//bitmapImage.BeginInit();
			//bitmapImage.UriSource = new Uri(m_ImagePath);
			//bitmapImage.EndInit();
			//this.ImageMain.Source = bitmapImage;

			//BitmapImage bitmapImage = new BitmapImage();
			//bitmapImage.BeginInit();
			//bitmapImage.UriSource = new Uri(imgPath);
			//bitmapImage.EndInit();
			//imagePath = imgPath;
			//this.ImageMain.Source = bitmapImage;
			////this.ImageMain.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
		}


		public void ReleaseBackImage()
		{
			foreach (Image item in GridImage.Children)
			{
				item.Source = null;
			}
		}
	}
}