using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;

namespace BarnAssist.Services
{
  public sealed class MediaCaptureService
  {
    public IAsyncAction StartVideoCapture()
    {
      return StartRecordAndSaveLoop().AsAsyncAction();
    }

    private async Task StartRecordAndSaveLoop()
    {
      while (true)
      {
        var durationInMinutes = 15;
        var fileName = GetFileName(durationInMinutes); 

        var capture = await StartRecording(fileName);

        await Task.Delay(TimeSpan.FromMinutes(durationInMinutes));

        await capture.StopRecordAsync();
      }
    }

    private string GetFileName(int durationInMinutes)
    {
      var now = DateTime.Now;
      return $"{now.ToString("yyyyMMddHHmm")}-{now.AddMinutes(durationInMinutes).ToString("yyyyMMddHHmm")}.wmv";
    }

    private async Task<MediaCapture> StartRecording(string fileName)
    {
      var devices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(Windows.Devices.Enumeration.DeviceClass.VideoCapture);
      var camerDeviceId = devices.FirstOrDefault()?.Id;

      var captureSettings = new MediaCaptureInitializationSettings
      {
        VideoDeviceId = camerDeviceId,
        AudioDeviceId = string.Empty,
        PhotoCaptureSource = PhotoCaptureSource.VideoPreview,
        StreamingCaptureMode = StreamingCaptureMode.Video
      };

      var mediaCapture = new MediaCapture();
      await mediaCapture.InitializeAsync(captureSettings);

      var def = new Windows.Media.Effects.VideoEffectDefinition(Windows.Media.VideoEffects.VideoStabilization);
      await mediaCapture.AddVideoEffectAsync(def, MediaStreamType.VideoRecord);

      var profile = MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Qvga);

      var storageFolder = await KnownFolders.RemovableDevices
        .GetFolderAsync("E:\\");

      var storageFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);

      await mediaCapture.StartRecordToStorageFileAsync(profile, storageFile);

      return mediaCapture; 
    }
  }
}
