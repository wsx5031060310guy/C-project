using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mamaro_switch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
     GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Init(object sender, EventArgs e)
    {
        gc = new GCP_MYSQL();
        Panel mypan = (Panel)this.FindControl("main_Panel");
        Literal lip = new Literal();
        Query = "select * from nursing_room;";
        DataView ict_ff = gc.select_cmd(Query);
        bool chee = true;
        lip.Text += "<script src=" + '"' + @"http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js" + '"' + @"></script>";
        lip.Text += "<script src=" + '"' + @"http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.2/jquery-ui.min.js" + '"' + @"></script>";
        lip.Text += "<link rel='stylesheet' href='css/jquery.switchButton.css'>";
        lip.Text += "<script src=" + '"' + @"js/jquery.switchButton.js" + '"' + @"></script>";
        if (ict_ff.Count > 0)
        {
            for (int i = 0; i < ict_ff.Count; i++)
            {
                lip.Text += "<br><div class='center'>" + ict_ff.Table.Rows[i]["name"].ToString() + @"<br><br>";
                //lip.Text += "<div id=" + '"' + "mamaroname" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + ">" + ict_ff.Table.Rows[i]["name"].ToString() + @"</div>";
                lip.Text += @"
<div class='slider demo' id='slider-"+i+@"'>
        <input type='checkbox' value='"+ ict_ff.Table.Rows[i]["id"].ToString()+@"' checked>
      </div>

";
                string state = "false";
                if (ict_ff.Table.Rows[i]["close"].ToString() == "0")
                {
                    state = "true";
                }

                  lip.Text += @"<script type=" + '"' + "text/javascript" + '"' + @">
$(function() {
$('#slider-" + i + @".demo input').switchButton({
checked:" + state + @",
          width: 100,
          height: 40,
          button_width: 50,
on_callback(){
console.log('on');
 $.ajax({
                     type: " + '"' + @"POST" + '"' + @",
                     url: " + '"' + @"mamaro_switch.aspx/send_on" + '"' + @",
                     data: " + '"' + @"{param1: '" + '"' + @" + $('#slider-" + i + @".demo input').val() + " + '"' + @"'   }" + '"' + @",
                     contentType: " + '"' + @"application/json; charset=utf-8" + '"' + @",
                     dataType: " + '"' + @"json" + '"' + @",
                     async: true,
                     cache: false,
                     success: function (result) {
                        console.log(result.d);

                     },
                     error: function (result) {
                         console.log(result.d);
                     }
                 });
},
off_callback(){
console.log('off');
 $.ajax({
                     type: " + '"' + @"POST" + '"' + @",
                     url: " + '"' + @"mamaro_switch.aspx/send_off" + '"' + @",
                     data: " + '"' + @"{param1: '" + '"' + @" + $('#slider-" + i + @".demo input').val() + " + '"' + @"'   }" + '"' + @",
                     contentType: " + '"' + @"application/json; charset=utf-8" + '"' + @",
                     dataType: " + '"' + @"json" + '"' + @",
                     async: true,
                     cache: false,
                     success: function (result) {
                         console.log(result.d);

                     },
                     error: function (result) {
                         console.log(result.d);
                     }
                 });
},
        });
    })
</script>
";
                lip.Text += "</div>";

                //if (ict_ff.Table.Rows[i]["close"].ToString() == "0")
                //{
                //    lip.Text += "<div id=" + '"' + "mamaroname" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + ">" + ict_ff.Table.Rows[i]["name"].ToString() + @"</div>";
                //    lip.Text += "<div id=" + '"' + "mamarotime" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + "></div>";

                //}

            }
        }

//        lip.Text += @"
//<div class='slider demo' id='slider-1'>
//        <input type='checkbox' value='abc' checked>
//      </div>
//<div class='slider demo' id='slider-2'>
//        <input type='checkbox' value='efg' checked>
//      </div>
//";

       
//        lip.Text += @"<script type=" + '"' + "text/javascript" + '"' + @">
//$(function() {
//
//
//$('#slider-1.demo input').switchButton({
//checked:true,
//          width: 100,
//          height: 40,
//          button_width: 50,
//on_callback(){
//console.log('on');
//console.log($('#slider-1.demo input').val());
//},
//off_callback(){
//console.log('off');
//},
//        });
//
//
//$('#slider-2.demo input').switchButton({
//checked:true,
//          width: 100,
//          height: 40,
//          button_width: 50,
//on_callback(){
//console.log('on');
//console.log($('#slider-2.demo input').val());
//},
//off_callback(){
//console.log('off');
//},
//        });
//
//
//
//      })
//</script>
//";


        //if (ict_ff.Count > 0)
        //{
        //    for (int i = 0; i < ict_ff.Count; i++)
        //    {
        //        lip.Text += "<div id=" + '"' + "mamaroname" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + ">" + ict_ff.Table.Rows[i]["name"].ToString() + @"</div>";
        //        lip.Text += "<div id=" + '"' + "mamarotime" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + "></div>";

        //        //if (ict_ff.Table.Rows[i]["close"].ToString() == "0")
        //        //{
        //        //    lip.Text += "<div id=" + '"' + "mamaroname" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + ">" + ict_ff.Table.Rows[i]["name"].ToString() + @"</div>";
        //        //    lip.Text += "<div id=" + '"' + "mamarotime" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + "></div>";

        //        //}

        //    }
        //}

        main_Panel.Controls.Add(lip);

    }
    public static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string send_on(string param1)
    {
        //string result = param1 + "," + param2;
        string result = "";
        GCP_MYSQL gc = new GCP_MYSQL();
        string id = RemoveSpecialCharacters(param1);
        try
        {
            int pid = Convert.ToInt32(id);
            GCP_MYSQL gc1 = new GCP_MYSQL();
            string Query1 = "";
            Query1 = "update nursing_room set close='0',close_update_time=NOW() where id='" + pid + "';";
            result = gc1.update_cmd(Query1);

        }
        catch (Exception ex)
        {
            result = "fail";

            //return result;
            throw ex;
        }
        return result;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string send_off(string param1)
    {
        //string result = param1 + "," + param2;
        string result = "";
        GCP_MYSQL gc = new GCP_MYSQL();
        string id = RemoveSpecialCharacters(param1);
        try
        {
            int pid = Convert.ToInt32(id);
            GCP_MYSQL gc1 = new GCP_MYSQL();
            string Query1 = "";
            Query1 = "update nursing_room set close='1',close_update_time=NOW() where id='" + pid + "';";
            result = gc1.update_cmd(Query1);

        }
        catch (Exception ex)
        {
            result = "fail";

            //return result;
            throw ex;
        }
        return result;
    }
}