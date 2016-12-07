<%@ WebHandler Language="C#" Class="DownloadCountHandler" %>

using System;
using System.Web;

public class DownloadCountHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        
        var method = context.Request["method"];
        if (method == "addCount")
        {
            new Service.DownloadCountService().addCount();
        }
        //Service.MongoDBService.GetHostoryPrices(15063, Convert.ToDateTime("2016-11-01"), Convert.ToDateTime("2016-12-10"));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}