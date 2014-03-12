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
	/// UCFullImage.xaml 的交互逻辑
	/// </summary>
	public partial class UCFullImage : UserControl
	{
		#region 变量
		List<int> listDeviceID = new List<int>();
		bool isMuiltTouch = false;
		#endregion

		#region 委托事件

		#endregion

		#region 属性

		#endregion

		#region 构造函数
		public UCFullImage()
		{
			this.InitializeComponent();
		}
		#endregion

		#region 业务
		public void SetBackImage(string imgPath)
		{
			this.ImageMain.Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute));
		}

		protected override void OnManipulationStarting(ManipulationStartingEventArgs args)
		{
			if (isMuiltTouch)
			{
				args.ManipulationContainer = this;

				// Adjust Z-order
				FrameworkElement element = args.Source as FrameworkElement;
				Panel pnl = element.Parent as Panel;

				for (int i = 0; i < pnl.Children.Count; i++)
					Panel.SetZIndex(pnl.Children[i],
						pnl.Children[i] == element ? pnl.Children.Count : i);

				args.Handled = true;
			}

			base.OnManipulationStarting(args);
		}

		protected override void OnManipulationDelta(ManipulationDeltaEventArgs args)
		{
			if (isMuiltTouch)
			{
				UIElement element = args.Source as UIElement;
				MatrixTransform xform = element.RenderTransform as MatrixTransform;
				Matrix matrix = xform.Matrix;
				ManipulationDelta delta = args.DeltaManipulation;
				Point center = args.ManipulationOrigin;
				matrix.ScaleAt(delta.Scale.X, delta.Scale.Y, center.X, center.Y);
				//matrix.RotateAt(delta.Rotation, center.X, center.Y);
				//matrix.Translate(delta.Translation.X, delta.Translation.Y);
				xform.Matrix = matrix;

				//args.Handled = true;
			}

			base.OnManipulationDelta(args);
		}

		private void UserControl_TouchDown(object sender, TouchEventArgs e)
		{
			Console.WriteLine("Down");
			if (!listDeviceID.Contains(e.TouchDevice.Id))
			{
				listDeviceID.Add(e.TouchDevice.Id);
			}

			if (listDeviceID.Count >= 2)
			{
				isMuiltTouch = true;
			}
		}

		private void UserControl_TouchUp(object sender, TouchEventArgs e)
		{
			Console.WriteLine("Up");
			listDeviceID.Remove(e.TouchDevice.Id);

			if (listDeviceID.Count < 2)
			{
				isMuiltTouch = false;
			}
		}
		#endregion
	}
}