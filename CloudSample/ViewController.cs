using System;
using System.Threading.Tasks;
using SampleCloudinary;
using SeeviApp.iOS.Helpers;
using UIKit;

namespace CloudSample
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

		public static string Path = string.Empty;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			var button = new UIButton(UIButtonType.Custom);
			button.Frame = new CoreGraphics.CGRect(100, 100, 200, 200);
			button.BackgroundColor = UIColor.Blue;


			View.AddSubview(button);
            
			button.TouchUpInside +=async (sender, e) => 
			{
				var tcs = new TaskCompletionSource<iOSFileHelper>();
				var h = new iOSAttachmentHelper(this, tcs);
				var result =await h.PickLibraryImage();
				if(result != null)
				{
					if(!string.IsNullOrEmpty(Path))
					{
						Class1 obj = new Class1();
						obj.Init();
						obj.Upload(Path);
					}
					//result
				}
			};
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
