<%@ WebHandler Language="C#" Class="getSpot" %>

using System;
using System.Web;
/*要引用以下的命名空間*/
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
/*Json.NET相關的命名空間*/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class getSpot : IHttpHandler {

    int zip_no = 100;//中正區的郵遞區號
    DBUtil db = new DBUtil();
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        if (!string.IsNullOrEmpty(context.Request["zip_no"]))
        {
            int.TryParse(context.Request["zip_no"], out this.zip_no);//防SQL Injection，轉型別失敗就用預設值
        }

         
        //取得DataTable原始資料
        DataTable dt = db.queryDataTable(@"SELECT zip_no,company_title,address,company_desc,iconName,lat,lng
                                            from tb_company
                                            Where zip_no="+this.zip_no+@"
                                            Order by ID ASC");
        //因為本範例的資料都沒有緯度和經度，所以把原始資料DataTable傳入取得一個新的DataTable(有緯度、經度的)
        DataTable dt_new  = this.fillLatLng(dt);
        //利用Json.NET將DataTable轉成JSON字串，請參考另一篇文章：http://www.dotblogs.com.tw/shadow/archive/2011/11/30/60083.aspx
        string str_json = JsonConvert.SerializeObject(dt_new, Formatting.Indented);
        context.Response.Write(str_json);
    }
 
    /// <summary>
    /// 回傳有緯度和經度的DataTable
    /// </summary>
    /// <returns></returns>
     private DataTable fillLatLng(DataTable dt)
    {

        DataTable dt_new = dt.Copy();
        for (int i=0;i<dt.Rows.Count;i++)//走訪原始資料
        {
          string json_address=  this.convertAddressToJSONString(dt.Rows[i]["address"].ToString());
          //取得緯度和經度
          double[] latLng = this.getLatLng(json_address);
          dt_new.Rows[i]["lat"] = latLng[0];
          dt_new.Rows[i]["lng"] = latLng[1];
            
        }


        return dt_new;
    }


    /// <summary>
    /// 傳入JSON字串，取得經緯度
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
     private double[] getLatLng(string json)
     {
         JObject jo = JsonConvert.DeserializeObject<JObject>(json);

         //從results開始往下找
         JArray ja = (JArray)jo.Property("results").Value;
         jo = ja.Value<JObject>(0);//JArray第0筆
         //得到location的JObject
         jo = (JObject)((JObject)jo.Property("geometry").Value).Property("location").Value;
         
         //緯度
         double lat = Convert.ToDouble(((JValue)jo.Property("lat").Value).Value);
         //經度
         double lng = Convert.ToDouble(((JValue)jo.Property("lng").Value).Value);
         
         double[] latLng= {lat,lng};

         return latLng;
     
     } 
      
    /// <summary>
     /// 把地址轉成JSON格式，這樣資訊裡才有經緯度
     /// 因為使用到地理編碼技術，請注意使用限制：http://code.google.com/intl/zh-TW/apis/maps/documentation/geocoding/#Limits
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
     private string convertAddressToJSONString(string address)
     {
         //var url = "http://maps.google.com/maps/api/geocode/json?sensor=true&address=" + HttpContext.Current.Server.UrlEncode(address);
         //2012.4.12 以上是舊寫法，新寫法請用以下
        var url =    "http://maps.googleapis.com/maps/api/geocode/json?sensor=true&address=" + HttpContext.Current.Server.UrlEncode(address);


         string result = String.Empty;
         HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
         using (var response = request.GetResponse())
         using (StreamReader sr = new StreamReader(response.GetResponseStream()))
         {

             result = sr.ReadToEnd();
         }
         return result;

     }
 
    
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}