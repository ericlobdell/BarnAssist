using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarnAssist.StandardLib.Interfaces
{
  public interface IMediaCaptureService
  {
    Task StartVideoCapture();
    Task StopVideoCapture();
  }
}
