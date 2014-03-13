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
		TranslateZoomRotateBehavior translateZoomRotateBehavior;
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

		#region overrid
		//protected override void OnManipulationStarting(ManipulationStartingEventArgs args)
		//{
		//    if (isMuiltTouch)
		//    {
		//        args.ManipulationContainer = this;
		//        args.Mode = ManipulationModes.Scale;

		//        // Adjust Z-order
		//        FrameworkElement element = args.Source as FrameworkElement;
		//        Panel pnl = element.Parent as Panel;

		//        for (int i = 0; i < pnl.Children.Count; i++)
		//            Panel.SetZIndex(pnl.Children[i],
		//                pnl.Children[i] == element ? pnl.Children.Count : i);

		//        args.Handled = true;
		//    }

		//    base.OnManipulationStarting(args);

		//}

		//protected override void OnManipulationDelta(ManipulationDeltaEventArgs args)
		//{
		//    if (isMuiltTouch)
		//    {
		//        UIElement element = args.Source as UIElement;
		//        MatrixTransform xform = element.RenderTransform as MatrixTransform;
		//        Matrix matrix = xform.Matrix;
		//        ManipulationDelta delta = args.DeltaManipulation;
		//        Point center = args.ManipulationOrigin;
		//        matrix.Translate(-center.X, -center.Y);
		//        matrix.Scale(delta.Scale.X, delta.Scale.Y);
		//        matrix.Rotate(delta.Rotation);
		//        matrix.Translate(center.X, center.Y);
		//        matrix.Translate(delta.Translation.X, delta.Translation.Y);
		//        xform.Matrix = matrix;

		//        args.Handled = true;
		//        base.OnManipulationDelta(args);
		//        //        OnImageControlEvent(true, numUC);
		//    }
		//    //    else
		//    //    {
		//    //        OnImageControlEvent(false, numUC);
		//    //    }

		//    //    base.OnManipulationDelta(args);

		//} 
		#endregion

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			////translateZoomRotateBehavior = this.tzb;
			//TranslateZoomRotateBehavior tc = new TranslateZoomRotateBehavior();
			//tc.SupportedGestures = ManipulationModes.Scale;
			//tc.MaximumScale = 10;
			//tc.MinimumScale = 0.5;
			//tc.Attach(this);

			ImageMain.IsManipulationEnabled = true;
			ImageMain.RenderTransform = new MatrixTransform();
		}

		private void ImageMain_TouchDown(object sender, TouchEventArgs e)
		{
			////Console.WriteLine("Down");
			//if (!listDeviceID.Contains(e.TouchDevice.Id))
			//{
			//    listDeviceID.Add(e.TouchDevice.Id);
			//}

			//if (listDeviceID.Count >= 2)//多点缩放
			//{
			//    isMuiltTouch = true;
			//    OnImageControlEvent(true, numUC);

			//    if (!isAttached)
			//    {
			//        //TranslateZoomRotateBehavior tc = new TranslateZoomRotateBehavior();
			//        //tc.SupportedGestures = ManipulationModes.Scale;
			//        //tc.MaximumScale = 10;
			//        //tc.MinimumScale = 0.5;
			//        //tc.Attach(this);
			//        ////translateZoomRotateBehavior = tc;
			//        ////tc.ConstrainToParentBounds = true;
			//        //isAttached = true;
			//    }
			//}
			//else//单点平移
			//{
			//    OnImageControlEvent(false, numUC);

			//    //if (isAttached)
			//    //{
			//    //    translateZoomRotateBehavior.Detach();
			//    //    isAttached = false;
			//    //}

			//}
		}

		private void ImageMain_TouchUp(object sender, TouchEventArgs e)
		{
			//Console.WriteLine("Up");
			//listDeviceID.Remove(e.TouchDevice.Id);

			//if (listDeviceID.Count < 2)
			//{
			//    isMuiltTouch = false;
			//}
		}

		private void ImageMain_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
		{
			//e.Mode = ManipulationModes.Scale;
			//e.ManipulationContainer = LayoutRoot;
		}

		private void ImageMain_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			//if (isMuiltTouch)
			//{
			//    FrameworkElement element = (FrameworkElement)e.Source;
			//    Matrix matrix = ((MatrixTransform)element.RenderTransform).Matrix;
			//    ManipulationDelta deltaManipulation = e.DeltaManipulation;
			//    Point center = new Point(element.ActualWidth / 2, element.ActualHeight / 2);
			//    center = matrix.Transform(center);
			//    matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
			//    matrix.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);
			//    matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
			//    ((MatrixTransform)element.RenderTransform).Matrix = matrix;
			//}
		}
		#endregion
	}
}