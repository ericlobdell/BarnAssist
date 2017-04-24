﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using BarnAssist.Services;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace BarnAssist
{
  public sealed class StartupTask : IBackgroundTask
  {
    public async void Run(IBackgroundTaskInstance taskInstance)
    {
      // 
      // TODO: Insert code to perform background work
      //
      // If you start any asynchronous methods here, prevent the task
      // from closing prematurely by using BackgroundTaskDeferral as
      // described in http://aka.ms/backgroundtaskdeferral
      //
      var deferral = taskInstance.GetDeferral();

      var mediaCaptureService = new MediaCaptureService();
      await mediaCaptureService.StartVideoCapture();

      deferral.Complete();
    }
  }
}
