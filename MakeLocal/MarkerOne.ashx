<%@ WebHandler Language="C#" Class="MarkerOne" %>

using System;
using System.Web;
using System.IO;
using System.Net;
public class MarkerOne : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string address = context.Request.QueryString["address"];//中文地址
        double lat = Convert.ToDouble(context.Request.QueryString["lat"]), lng = Convert.ToDouble(context.Request.QueryString["lng"]);
        context.Response.Write(this.convertAddressToJSONString(address,lat,lng));//輸出json字串
    }
    //把地址轉成JSON格式，這樣資訊裡才有緯經度
    //因為使用到地理編碼技術，請注意使用限制：http://code.google.com/intl/zh-TW/apis/maps/documentation/geocoding/#Limits
    private string convertAddressToJSONString(string address, double lat, double lng)
    {
        string result = String.Empty;
        if (lat == 0 && lng == 0)
        {
            var url = "http://maps.google.com/maps/api/geocode/json?sensor=true&address=" + HttpContext.Current.Server.UrlEncode(address);

            System.Net.HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            using (var response = request.GetResponse())
            using (StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }
        else
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("\"address\":\"" + address + "\",");
            sb.AppendLine("\"lat\":\"0\",");
            sb.AppendLine("\"lng\":\"0\",");
            sb.AppendLine("\"Message\":\"test\"");
            sb.AppendLine("}");

            return sb.ToString();
        }
        

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}