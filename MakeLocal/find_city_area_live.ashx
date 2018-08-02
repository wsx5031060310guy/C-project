<%@ WebHandler Language="C#" Class="find_city_area_live" %>

using System;
using System.Web;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;

public class find_city_area_live : IHttpHandler {

    DBUtil db = new DBUtil();
    DBUtil db_sel = new DBUtil();
    DBUtil db_in = new DBUtil();
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(context.Request["address"]);
        string utf8ReturnString = encoder.GetString(bytes);
        string address = utf8ReturnString;
        context.Response.Write(this.convertAddressToJSONString(address));//輸出json字串
    }

    private string convertAddressToJSONString(string address)
    {
        address = address.Replace("'", "");
        address = address.Replace(@"""", "");
        //address = CheckSpecialString(address);
        string result = String.Empty;
        if (address == "" || address == null)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("\"address\":\"" + address + "\",");
            sb.AppendLine("\"lat\":\"0\",");
            sb.AppendLine("\"lng\":\"0\",");
            sb.AppendLine("\"Message\":\"No value\"");
            sb.AppendLine("}");

            return sb.ToString();
        }
        else
        {
            var url = "http://maps.google.com/maps/api/geocode/json?sensor=true&address=" + HttpContext.Current.Server.UrlEncode(address);

            System.Net.HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            using (var response = request.GetResponse())
            using (StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }



            JObject jArray = JObject.Parse(result);
            string lat = (string)jArray["results"][0]["geometry"]["location"]["lat"];
            string lng = (string)jArray["results"][0]["geometry"]["location"]["lng"];
            bool chc_country = false, chc_country_1 = false;
            bool chc_place = false;
            string postal_code = "";
            foreach (var item in jArray["results"][0]["address_components"])
            {
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
                if (chc_country)
                {
                    string country = (string)item["long_name"];
                    if (country == "Japan")
                    {
                        chc_country_1 = true;
                    }

                }
                if (chc_place)
                {
                    postal_code = (string)item["long_name"];
                }
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("\"address\":\"" + address + "\",");
            sb.AppendLine("\"lat\":\"" + lat + "\",");
            sb.AppendLine("\"lng\":\"" + lng + "\",");


            sb.AppendLine("\"Message\":\"" + postal_code + "\"");
            sb.AppendLine("}");

            return sb.ToString();
            

//            if (chc_country_1)
//            {
//                DataTable dt;
//                dt = db.queryDataTable(@"SELECT address,lat,lng,postal_code
//                                            from address
//                                            where lat=" + lat + " and lng=" + lng + @"
//                                            Order by ID ASC");
//                int cou = dt.Rows.Count;
//                if (cou == 0)
//                {
//                    //if cou=0 no one registered

//                    db_in.queryDataTable(@"insert into address(address,lat,lng,postal_code) 
//                                            values(N'" + address + "'," + lat + "," + lng + ",N'" + postal_code + "')");



//                    sb.AppendLine("\"Message\":\"Success\"");
//                    sb.AppendLine("}");

//                    return sb.ToString();
//                }
//                else
//                {
//                    //if cou>0 some one registered
//                    sb.AppendLine("\"Message\":\"Someone have same address\"");
//                    sb.AppendLine("}");

//                    return sb.ToString();
//                }

//            }
//            else
//            {
//                //not in Japan
//                sb.AppendLine("\"Message\":\"Not in Japan\"");
//                sb.AppendLine("}");

//                return sb.ToString();
//            }
        }

    }
    private string CheckSpecialString(string Inputstr)
    {
        string s = Inputstr;
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"[\W_][^/_]+");
        string r = regex.Replace(s, "");

        return r;

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}