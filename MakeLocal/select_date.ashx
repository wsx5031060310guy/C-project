<%@ WebHandler Language="C#" Class="select_date" %>

using System;
using System.Web;

public class select_date : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(context.Request["address"]);
        string utf8ReturnString = encoder.GetString(bytes);
        string address = utf8ReturnString;
        context.Response.Write(address);//輸出json字串
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}