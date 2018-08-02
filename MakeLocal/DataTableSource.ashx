
<%@ WebHandler Language="C#" Class="DataTableSource" %>

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

public class DataTableSource : IHttpHandler
{
    int ranid = 1;
    int zip_no = 100;
    DBUtil db = new DBUtil();
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        //取得DataTable
        DataTable dt = this.queryDataTable(context);
        //將DataTable轉成JSON字串
        string str_json = JsonConvert.SerializeObject(dt, Formatting.Indented);
        context.Response.Write(str_json);
    }
 
    /// <summary>
    /// 從DB撈出DataTable
    /// </summary>
    /// <returns></returns>
    private DataTable queryDataTable(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        if (!string.IsNullOrEmpty(context.Request["ranid"]))
        {
            int.TryParse(context.Request["ranid"], out this.ranid);//防SQL Injection，轉型別失敗就用預設值
        }
        DataTable dt;
        if (this.ranid == 1)
        {
            dt = db.queryDataTable(@"SELECT address as '地址',title as '名稱',company_desc,iconName,lat,lng
                                            from tb_company
                                            Order by ID ASC");
        }
        else
        {
            dt = db.queryDataTable(@"SELECT address as '地址',title as '名稱',company_desc,iconName,lat,lng 
                                            from tb_company
                                            Where id=" + this.ranid + @" or id=1
                                            Order by ID ASC");
        }

        return dt;
    }


    
  
    
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}