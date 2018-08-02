<%@ WebHandler Language="C#" Class="some_generic_handler" %>

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

public class some_generic_handler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string myVar = "";
        if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form["myVar"])) { myVar = System.Web.HttpContext.Current.Request.Form["myVar"].Trim(); }

        var pointgray = JsonConvert.DeserializeObject<string[,]>(myVar);
        int max,pos;
        int[] numbers = new int[9];
        int[,] pointrunx = new int[pointgray.GetLength(0), pointgray.GetLength(1)];
        int[,] pointruny = new int[pointgray.GetLength(0), pointgray.GetLength(1)];
        for (int i = 1; i < pointgray.GetLength(0)-1; i++)
        {
            for (int j = 1; j < pointgray.GetLength(1)-1; j++)
            {
                max = 9999999;
                pos = -1;
                numbers[0] =Convert.ToInt32( pointgray[i - 1,j - 1]);
                numbers[1] =Convert.ToInt32( pointgray[i,j - 1]);
                numbers[2] =Convert.ToInt32( pointgray[i + 1,j - 1]);
                numbers[3] =Convert.ToInt32( pointgray[i - 1,j]);
                numbers[4] =Convert.ToInt32( pointgray[i,j]);
                numbers[5] =Convert.ToInt32( pointgray[i + 1,j]);
                numbers[6] =Convert.ToInt32( pointgray[i - 1,j + 1]);
                numbers[7] =Convert.ToInt32( pointgray[i,j + 1]);
                numbers[8] =Convert.ToInt32( pointgray[i + 1,j + 1]);
                for (var ii = 0; ii < numbers.GetLength(0); i++)
                {
                    if (numbers[ii] < max)
                    {
                        max = numbers[ii];
                        pos = ii;
                    }
                }
                if (pos == 0) { pointrunx[i,j] = i - 1; pointruny[i,j] = j - 1; }
                if (pos == 1) { pointrunx[i,j] = i; pointruny[i,j] = j - 1; }
                if (pos == 2) { pointrunx[i,j] = i + 1; pointruny[i,j] = j - 1; }
                if (pos == 3) { pointrunx[i,j] = i - 1; pointruny[i,j] = j; }
                if (pos == 4) { pointrunx[i,j] = i; pointruny[i,j] = j; }
                if (pos == 5) { pointrunx[i,j] = i + 1; pointruny[i,j] = j; }
                if (pos == 6) { pointrunx[i,j] = i - 1; pointruny[i,j] = j + 1; }
                if (pos == 7) { pointrunx[i,j] = i; pointruny[i,j] = j + 1; }
                if (pos == 8) { pointrunx[i,j] = i + 1; pointruny[i,j] = j + 1; }
            }
        }
        string xx = "0",yy="0";
        for (int i = 1; i < pointgray.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < pointgray.GetLength(1) - 1; j++)
            {
                if (i == 1 && j == 1)
                {
                    xx = ""+pointrunx[i, j]; yy = ""+pointruny[i, j];
                }
                else
                {
                    xx += "," + pointrunx[i,j];
                    yy += "," + pointruny[i, j];
                }
                
            }
        }

        string response = String.Format(@"{{ ""success"" : true, ""message"" : ""Cool! We're done."", ""xElement"" : ""{0}"", ""yElement"" : ""{1}"" }}", xx,yy);

        context.Response.ContentType = "application/json";
        context.Response.Write(response);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}