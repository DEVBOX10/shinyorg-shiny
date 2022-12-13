﻿using System.IO;
using System.Text;
using Foundation;

namespace Shiny.Net.Http;


static class PlatformExtensions
{
    //const int statusCodeOkMin = 200;
    //const int statusCodeOkMax = 299;
    //const int cancelErrorCode = -999;


    public static void WriteString(this Stream stream, string value, bool includeNewLine = true)
    {
        stream.Write(Encoding.Default.GetBytes(value));
        if (includeNewLine)
            stream.WriteLine();
    }


    public static void WriteLine(this Stream stream)
        => stream.Write(Encoding.Default.GetBytes("\r\n"));


    public static HttpTransferState GetStatus(this NSUrlSessionTask task) => task.State switch
    {
        NSUrlSessionTaskState.Canceling => HttpTransferState.Canceled,
        NSUrlSessionTaskState.Completed => HttpTransferState.Completed,
        NSUrlSessionTaskState.Running => HttpTransferState.InProgress,

        // TODO: paused vs pending
        NSUrlSessionTaskState.Suspended => HttpTransferState.Paused
    };
}

//public static string GetUploadTempFilePath(this IPlatform platform, HttpTransferRequest request)
//    => GetUploadTempFilePath(platform, request.LocalFile.Name);


//public static string GetUploadTempFilePath(this IPlatform platform, HttpTransfer transfer)
//    => GetUploadTempFilePath(platform, transfer.LocalFilePath);


//static string GetUploadTempFilePath(IPlatform platform, string fileName)
//{
//    var tempPath = Path.Combine(platform.Cache.FullName, fileName + ".tmp");
//    return tempPath;
//}


//public static NSMutableUrlRequest ToNative(this HttpTransferRequest request)
//{
//    var url = NSUrl.FromString(request.Uri);
//    var native = new NSMutableUrlRequest(url)
//    {
//        HttpMethod = request.HttpMethod.Method,
//        AllowsCellularAccess = request.UseMeteredConnection,
//    };

//    if (request.Headers.Any())
//    {
//        native.Headers = NSDictionary.FromObjectsAndKeys(
//            request.Headers.Values.ToArray(),
//            request.Headers.Keys.ToArray()
//        );
//    }
//    return native;
//}


//public static HttpTransfer FromNative(this NSUrlSessionTask task)
//{
//    var status = HttpTransferState.Unknown;
//    var upload = task is NSUrlSessionUploadTask;
//    Exception? exception = null;

//    switch (task.State)
//    {
//        case NSUrlSessionTaskState.Canceling:
//            status = HttpTransferState.Canceled;
//            break;

//        case NSUrlSessionTaskState.Completed:
//            if (task.Error != null)
//            {
//                if (task.Error?.Code == cancelErrorCode)
//                {
//                    status = HttpTransferState.Canceled;
//                }
//                else
//                {
//                    status = HttpTransferState.Error;
//                    var e = task.Error;
//                    var msg = $"{e?.LocalizedDescription} - {e?.LocalizedFailureReason}";
//                    exception = new Exception(msg);
//                }
//            }
//            else if ((task.Response as NSHttpUrlResponse)?.StatusCode < statusCodeOkMin ||
//                     (task.Response as NSHttpUrlResponse)?.StatusCode > statusCodeOkMax)
//            {
//                // Status code is not in between 200-299 (Http Ok States)
//                status = HttpTransferState.Error;
//                exception = new Exception("Transfer exception with error code: " + (task.Response as NSHttpUrlResponse)?.StatusCode);
//            }
//            else
//            {
//                status = HttpTransferState.Completed;
//            }
//            break;

//        case NSUrlSessionTaskState.Suspended:
//            status = HttpTransferState.Paused;
//            break;

//        case NSUrlSessionTaskState.Running:
//            var bytes = task.BytesSent + task.BytesReceived;
//            status = bytes == 0 ? HttpTransferState.Pending : HttpTransferState.InProgress;
//            break;

//        default:
//            status = HttpTransferState.Error;
//            if (task.Error == null)
//            {
//                exception = new Exception("Unknown Error");
//            }
//            else
//            {
//                status = HttpTransferState.Error;
//                var e = task.Error;
//                var msg = $"{e.LocalizedDescription} - {e.LocalizedFailureReason}";
//                exception = new Exception(msg);
//            }
//            break;
//    }

//    var taskId = TaskIdentifier.FromString(task.TaskDescription);
//    return new HttpTransfer(
//        taskId.Value,
//        task.OriginalRequest.Url.ToString(),
//        taskId.File.FullName,
//        upload,
//        task.OriginalRequest.AllowsCellularAccess,
//        exception,
//        upload
//            ? task.BytesExpectedToSend
//            : task.BytesExpectedToReceive,
//        upload
//            ? task.BytesSent
//            : task.BytesReceived,

//        status
//    );
//}