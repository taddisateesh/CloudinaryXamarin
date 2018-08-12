using System;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace SampleCloudinary
{
	public class Class1
	{

		public Cloudinary cloudinary { get; set; }
		public void Init()
		{
			Account account = new Account(
				"xxxxxx",
				"xxxxxxx",
				"xxxxx");

			cloudinary = new Cloudinary(account);
		}

		/// <summary>
        /// Upload a file by its path.
        /// </summary>
        /// <returns>The upload end path.</returns>
        /// <param name="path">Path.</param>
        public async Task<string> Upload(string path)
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
				//User.GetUserInstance().ID
                File = new FileDescription(path),
                PublicId = String.Format("{0}.{1}.jpg","sateeshxamarintest" , DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"))
            };
            ImageUploadResult uploadResult = await cloudinary.UploadAsync(uploadParams);

            return uploadResult.Uri.ToString();
        }

        /// <summary>
        /// Delete the specified url.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="url">URL.</param>
        public void Delete(string url)
        {
            DeletionParams deletionParams = new DeletionParams(url);
            cloudinary.DestroyAsync(deletionParams);
        }
	}

}
