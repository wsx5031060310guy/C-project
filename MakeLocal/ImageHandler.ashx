﻿<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;

public class ImageHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        if ((context.Session["ImageBytes"]) != null)
        {
            byte[] image = (byte[])(context.Session["ImageBytes"]);
            context.Response.ContentType = "image/JPEG";
            context.Response.BinaryWrite(image);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}