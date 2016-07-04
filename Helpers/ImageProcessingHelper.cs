using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace NumberTracker
{
	public static class ImageProcessingHelper
	{
		public static async Task<string> GetTextFromImage(MediaFile mediaFile)
		{
			try
			{
				byte[] rawBytes;
				using (MemoryStream ms = new MemoryStream())
				{
					mediaFile.GetStream().CopyTo(ms);
					rawBytes = ms.ToArray();
				}
				var resizedImageBytes = DependencyService.Get<IImageResizer>().Resize(rawBytes, 400, 250);

				StreamContent scontent = new StreamContent(new MemoryStream(resizedImageBytes));
				scontent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
				{
					FileName = Path.GetFileName(mediaFile.Path),
					Name = "image"
				};
				scontent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

				var client = new HttpClient();
				var multi = new MultipartFormDataContent();
				multi.Add(scontent);
				client.BaseAddress = new Uri("http://45.55.137.37:5000/");
				var response = await client.PostAsync("api/photo", multi);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return content;
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
			}
			return string.Empty;
		}
	}
}

