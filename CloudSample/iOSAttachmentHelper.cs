using System;
using System.Threading.Tasks;
using AVFoundation;
using CloudSample;
using Foundation;
using MediaPlayer;

using UIKit;

namespace SeeviApp.iOS.Helpers
{
	public class iOSAttachmentHelper : NSObject, IUIImagePickerControllerDelegate, IMPMediaPickerControllerDelegate
	{
		

		


       public iOSAttachmentHelper()
		{
		}

		//BaseViewModel VM;
		UIViewController parent;
        readonly TaskCompletionSource<iOSFileHelper> tcs;

		public iOSAttachmentHelper(UIViewController _parent, TaskCompletionSource<iOSFileHelper> _tcs)
        {
            parent = _parent;
			//VM = parent.ViewModel as BaseViewModel;
            tcs = _tcs;
        }


		public async Task<iOSFileHelper> PickImage()
        {
            await OpenCamera();
            return await tcs.Task;
        }


		public async Task<iOSFileHelper> PickVideo()
        {
            await OpenVideo();
            return await tcs.Task;
        }


		public async Task<iOSFileHelper> PickLibraryImage()
        {
            await OpenPhotoLibrary();
            return await tcs.Task;
        }

        async Task OpenPhotoLibrary()
        {
            var imagePicker = new UIImagePickerController();
            //if (Utilities.IsPhotoLibraryAuthorised())
                //{
                if (UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.PhotoLibrary))
                {
                    imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
                    imagePicker.AllowsEditing = true;
                    imagePicker.WeakDelegate = this;
				    parent.PresentViewController(imagePicker, true, null);

                }
                else
                {
					//await VM.ShowAlertMessage("Please grant photo library permission");
                    tcs.SetResult(null);
                }
            //}
            //else
            //{
            //    await VM.ShowAlertMessage("Please grant photo library permission");
            //    tcs.SetResult(null);
            //}
        }


        async Task OpenCamera()
        {
            
			var imagePicker = new UIImagePickerController();
           
			//if (Utilities.IsFileAuthorized(AVMediaType.Video))
            //{
				if (UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
                {
					imagePicker.SourceType = UIImagePickerControllerSourceType.Camera;
					//imagePicker.MediaTypes = new string[] { "public.movie" };
					//imagePicker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Video;
                    imagePicker.AllowsEditing = true;
                    imagePicker.ShowsCameraControls = true;
                     imagePicker.WeakDelegate = this;
				parent.PresentViewController(imagePicker, true, null);
                }
                else
                {
					var okAlertController = UIAlertController.Create("Warning", "Please check camera is not available", UIAlertControllerStyle.Alert);
                    okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    parent.PresentViewController(okAlertController, true, null);
                    tcs.SetResult(null);
                }
    //        }
    //        else
    //        {
				
				//var okAlertController = UIAlertController.Create("Warning", "Please enable camera use in the device settings", UIAlertControllerStyle.Alert);
            //    okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            //    parent.PresentViewController(okAlertController, true, null);
            //    tcs.SetResult(null);
            //}
        }
        

		async Task OpenVideo()
		{
			
            // Present Alert
			var imagePicker = new UIImagePickerController();
            //if (Utilities.IsFileAuthorized(AVMediaType.Video))
            //{
                if (UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
                {
                    imagePicker.SourceType = UIImagePickerControllerSourceType.Camera;
                    imagePicker.MediaTypes = new string[] { "public.movie" };
                    //imagePicker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Video;
                    imagePicker.AllowsEditing = true;
                    imagePicker.ShowsCameraControls = true;
                    imagePicker.Canceled += ImagePicker_Canceled;
                    //imagePicker.WeakDelegate = this;
                    imagePicker.FinishedPickingMedia += ImagePicker_FinishedPickingMedia;
				    parent.PresentViewController(imagePicker, true, null);
                }
                else
                {
					
                    // await VM.ShowAlertMessage(VM.Message_Camera_Availability);
                    tcs.SetResult(null);
                }
            //}
            //else
            //{
            //    //todo dhowalert message
            //    // await VM.ShowAlertMessage(VM.Message_Camera_Privacy);
            //    tcs.SetResult(null);
            //}
		}
			
		[Export("imagePickerController:didFinishPickingImage:editingInfo:")]
        public void FinishedPickingImage(UIImagePickerController picker, UIImage image, NSDictionary editingInfo)
        {
			//var path = picker.path
			var originalImage = image.AsJPEG().ToArray();
			var url1 = (NSUrl)editingInfo.ValueForKey(new NSString("UIImagePickerControllerImageURL"));
			ViewController.Path = url1.Path;

            //stringTaskCompletionSource.SetResult(url.Path);

			//ViewController.Path = (NSUrl)editingInfo.ValueForKey(new NSString("UIImagePickerControllerImageURL"));
			//image.pat
			//var compressedData = Utilities.GetImageQuality(image.AsJPEG());
			picker.DismissViewController(true, null);
			//  var byteImage = compressedData.ToArray();
			//var imageName = DateTime.Now.ToString();
			//var dateString = DateTimeHelper.GetDisplayDateTimeString(imageName, string.Format("{0} {1}", GlobalConsts.dateFormat, GlobalConsts.timeFormat));
			//var fileName = string.Format("{0}.{1}", dateString, "jpeg");
			var fileName = "a23.jpej";
            tcs.SetResult(new iOSFileHelper
            {
                Name = fileName,
				Bytes = originalImage
            });
        }

		void ImagePicker_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
			
			if (e.MediaType == "public.movie")
            {
				var i = e.MediaUrl.AbsoluteString;
				tcs.SetResult(null);
            }
			(sender as UIImagePickerController).DismissViewController(true, null);
        }

		void ImagePicker_Canceled(object sender, EventArgs e)
        {
		   (sender as UIImagePickerController).DismissViewController(true, null);
		    tcs.SetResult(null);
        }



        [Export("imagePickerControllerDidCancel:")]
        public void Canceled(UIImagePickerController picker)
        {
            picker.DismissViewController(true, null);
            tcs.SetResult(null);
        }
	}

	public class iOSFileHelper
	{
		public string Name
		{
			get;
			set;
		}
         
		//public MediaType MediaType
  //      {
		//	get;
  //          set;
		//}
            public byte[] Bytes
            {
                get;
                set;
            }

            public byte[] ThumbNailBytes
            {
                get;
                set;
            }
       }
    
}
