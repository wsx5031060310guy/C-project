<%@ WebHandler Language="C#" Class="get_GPS" %>

using System;
using System.Web;

public class get_GPS : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string address = context.Request.QueryString["address"];//中文地址
        double lat = Convert.ToDouble(context.Request.QueryString["lat"]), lng = Convert.ToDouble(context.Request.QueryString["lng"]);
        context.Response.Write(this.convertAddressToJSONString(address, lat, lng));//輸出json字串
    }
    //把地址轉成JSON格式，這樣資訊裡才有緯經度
    //因為使用到地理編碼技術，請注意使用限制：http://code.google.com/intl/zh-TW/apis/maps/documentation/geocoding/#Limits
    private string convertAddressToJSONString(string address, double lat, double lng)
    {
        string result = String.Empty;

        if (lat != 0 && lng != 0)
        {
            var url = "http://maps.google.com/maps/api/geocode/json?sensor=true&language=ja&address=" + HttpContext.Current.Server.UrlEncode(lat.ToString() + "," + lng.ToString());

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
            using (var response = request.GetResponse())
            using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }

            if (result != "")
            {
                Newtonsoft.Json.Linq.JObject jArray = Newtonsoft.Json.Linq.JObject.Parse(result);
                bool chc_country = false;
                string postal_code = "";
                string address_for = jArray["results"][0]["formatted_address"].ToString();
                foreach (var item in jArray["results"][0]["address_components"])
                {
                    bool chc_place = false;
                    foreach (var item_c in item["types"])
                    {
                        string type = (string)item_c;
                        if (type == "country")
                        {
                            chc_country = true;
                        }
                        if (type == "postal_code")
                        {
                            chc_place = true;
                        }
                    }
                    if (chc_place)
                    {
                        postal_code = (string)item["long_name"];
                    }
                }
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine("\"postal_code\":\"" + postal_code + "\",");
                sb.AppendLine("\"address\":\"" + address_for + "\",");
                sb.AppendLine("\"lat\":\"" + lat.ToString() + "\",");
                sb.AppendLine("\"lng\":\"" + lng.ToString() + "\",");
                sb.AppendLine("\"Message\":\"test\"");
                sb.AppendLine("}");

                return sb.ToString();


            }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine("\"postal_code\":\"0\",");
                sb.AppendLine("\"address\":\"0\",");
                sb.AppendLine("\"lat\":\"0\",");
                sb.AppendLine("\"lng\":\"0\",");
                sb.AppendLine("\"Message\":\"test\"");
                sb.AppendLine("}");

                return sb.ToString();
            }
        }
        else
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("\"postal_code\":\"0\",");
            sb.AppendLine("\"address\":\"0\",");
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