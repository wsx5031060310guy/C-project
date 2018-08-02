using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class photo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["id"] == null)
        {
            Session["id"] = "";
        }
        Session["photo_count"] = 10;

        ///////
        Panel pdn_j = (Panel)this.FindControl("javaplace_forphoto");
        pdn_j.Controls.Clear();
        List<string> photo_list = new List<string>();

        //find photo
        SqlDataSource sql_place = new SqlDataSource();
        sql_place.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_place.SelectCommand = "select id";
        sql_place.SelectCommand += " from status_messages";
        sql_place.SelectCommand += " where uid='" + Session["id"].ToString() + "';";
        DataView ict_place = (DataView)sql_place.Select(DataSourceSelectArguments.Empty);
        Literal lip = new Literal();
        if (ict_place.Count > 0)
        {
            for (int i = 0; i < ict_place.Count;i++ )
            {
                SqlDataSource sql_photo = new SqlDataSource();
                sql_photo.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_photo.SelectCommand = "select filename";
                sql_photo.SelectCommand += " from status_messages_image";
                sql_photo.SelectCommand += " where smid='" + ict_place.Table.Rows[i]["id"].ToString() + "';";
                DataView ict_photo = (DataView)sql_photo.Select(DataSourceSelectArguments.Empty);
                {
                    for (int ii = 0; ii < ict_photo.Count; ii++)
                    {
                        string cutstr = ict_photo.Table.Rows[ii]["filename"].ToString();
                        int ind = cutstr.IndexOf(@"/");
                        string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                        photo_list.Add(cutstr1);
                    }
                }
            }
        }
        int counn = Convert.ToInt32(Session["photo_count"].ToString());
        if (Convert.ToInt32(Session["photo_count"].ToString()) > photo_list.Count)
        {
            counn = photo_list.Count;
        }

        lip.Text = @"<script type='text/javascript'>
photos = []
images = [
";
        for (int i = 0; i < counn; i++)
        {
            lip.Text += @"'" + photo_list[i] + @"',";
        }
        lip.Text = lip.Text.Substring(0, lip.Text.Length-1);
        lip.Text += @"]
	imageElements = [];
	for(var i=0; i<images.length; i++){
		imageElements[i] = new Image();
		imageElements[i].setAttribute('class', 'image'+i);
		imageElements[i].onload = function(){
			photos.push({src: this.src, ar: this.width / this.height})
			// console.log({src: this.src, ar: this.width / this.height});
		}
		imageElements[i].src = images[i];
	}
function floodDOM(){
		document.getElementById('workspace').innerHTML = "+'"'+'"'+@";
		viewport_width = window.innerWidth - 100;
		ideal_height = parseInt(window.innerHeight / 2);
		summed_width = photos.reduce((function(sum, p) {
		  return sum += p.ar * ideal_height;
		}), 0);
		rows = Math.round(summed_width / viewport_width);

	  weights = photos.map(function(p) {
	    return parseInt(p.ar * 100);
	  });
	  partition = part(weights, rows);

	  index = 0;
	  x = photos.slice(0);
	  row_buffer = [];
	  
	  for (var i = 0; i < partition.length; i++) {
	  	// console.log(partition[i])
	  	var summed_ratios;
	  	row_buffer = [];
	  	for (var j = 0; j<partition[i].length; j++) {
	  		row_buffer.push(photos[index++])
	  	};
	  	summed_ratios = row_buffer.reduce((function(sum, p) {
	      return sum += p.ar;
	    }), 0);
	    for (var k = 0; k<row_buffer.length; k++) {
	    	// console.log(row_buffer[k])
	    	photo = row_buffer[k];
	    	elem = document.createElement('div');
	    	elem.style.backgroundImage="+'"'+@"url('"+'"'+@"+x.shift().src+"+'"'+@"')"+'"'+ @";
	    	elem.style.width = parseInt(viewport_width / summed_ratios * photo.ar)+'px';
	    	elem.style.height = parseInt(viewport_width / summed_ratios)+'px';
	    	elem.setAttribute('class', 'photo');

	    	// console.log(elem, parseInt(viewport_width / summed_ratios * photo.ar) / parseInt(viewport_width / summed_ratios));
	    	
document.getElementById('workspace').appendChild(elem);
		  };
		}
		console.log({'viewport_width': viewport_width, 'ideal_height': ideal_height, 'summed_width': summed_width, 'rows': rows, 'weights': weights, 'partition': partition, 'row_buffer': row_buffer})
	}
	window.onresize = function(){floodDOM();}
$(window).load(function () {
floodDOM();
});
</script>
";

        pdn_j.Controls.Add(lip);
    }
    [WebMethod(EnableSession = true)]
    public static string search_new_photo(string param1)
    {
        string result = "";
        ///////
        //Panel pdn_j = (Panel)this.FindControl("javaplace_forphoto");
        //pdn_j.Controls.Clear();
        int counn = Convert.ToInt32(param1);
        //counn += 10;
        HttpContext.Current.Session["photo_count"] = counn;

        List<string> photo_list = new List<string>();

        //find photo
        SqlDataSource sql_place = new SqlDataSource();
        sql_place.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_place.SelectCommand = "select id";
        sql_place.SelectCommand += " from status_messages";
        sql_place.SelectCommand += " where uid='" + HttpContext.Current.Session["id"].ToString() + "';";
        DataView ict_place = (DataView)sql_place.Select(DataSourceSelectArguments.Empty);
        Literal lip = new Literal();
        if (ict_place.Count > 0)
        {
            for (int i = 0; i < ict_place.Count; i++)
            {
                SqlDataSource sql_photo = new SqlDataSource();
                sql_photo.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_photo.SelectCommand = "select filename";
                sql_photo.SelectCommand += " from status_messages_image";
                sql_photo.SelectCommand += " where smid='" + ict_place.Table.Rows[i]["id"].ToString() + "';";
                DataView ict_photo = (DataView)sql_photo.Select(DataSourceSelectArguments.Empty);
                {
                    for (int ii = 0; ii < ict_photo.Count; ii++)
                    {
                        string cutstr = ict_photo.Table.Rows[ii]["filename"].ToString();
                        int ind = cutstr.IndexOf(@"/");
                        string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                        photo_list.Add(cutstr1);
                    }
                }
            }
        }
        if (counn < photo_list.Count)
        {
            lip.Text = @"<script type='text/javascript'>
photos = []
images = [
";

            for (int i = (counn - 9); i < counn; i++)
            {
                lip.Text += @"'" + photo_list[i] + @"',";
            }
            lip.Text = lip.Text.Substring(0, lip.Text.Length - 1);
            lip.Text += @"]
	imageElements = [];
	for(var i=0; i<images.length; i++){
		imageElements[i] = new Image();
		imageElements[i].setAttribute('class', 'image'+i);
		imageElements[i].onload = function(){
			photos.push({src: this.src, ar: this.width / this.height})
			// console.log({src: this.src, ar: this.width / this.height});
		}
		imageElements[i].src = images[i];
	}
function floodDOM(){
		document.getElementById('workspace').innerHTML = " + '"' + '"' + @";
		viewport_width = window.innerWidth - 100;
		ideal_height = parseInt(window.innerHeight / 2);
		summed_width = photos.reduce((function(sum, p) {
		  return sum += p.ar * ideal_height;
		}), 0);
		rows = Math.round(summed_width / viewport_width);

	  weights = photos.map(function(p) {
	    return parseInt(p.ar * 100);
	  });
	  partition = part(weights, rows);

	  index = 0;
	  x = photos.slice(0);
	  row_buffer = [];
	  
	  for (var i = 0; i < partition.length; i++) {
	  	// console.log(partition[i])
	  	var summed_ratios;
	  	row_buffer = [];
	  	for (var j = 0; j<partition[i].length; j++) {
	  		row_buffer.push(photos[index++])
	  	};
	  	summed_ratios = row_buffer.reduce((function(sum, p) {
	      return sum += p.ar;
	    }), 0);
	    for (var k = 0; k<row_buffer.length; k++) {
	    	// console.log(row_buffer[k])
	    	photo = row_buffer[k];
	    	elem = document.createElement('div');
	    	elem.style.backgroundImage=" + '"' + @"url('" + '"' + @"+x.shift().src+" + '"' + @"')" + '"' + @";
	    	elem.style.width = parseInt(viewport_width / summed_ratios * photo.ar)+'px';
	    	elem.style.height = parseInt(viewport_width / summed_ratios)+'px';
	    	elem.setAttribute('class', 'photo');

	    	// console.log(elem, parseInt(viewport_width / summed_ratios * photo.ar) / parseInt(viewport_width / summed_ratios));
	    	
document.getElementById('workspace').appendChild(elem);
		  };
		}
		console.log({'viewport_width': viewport_width, 'ideal_height': ideal_height, 'summed_width': summed_width, 'rows': rows, 'weights': weights, 'partition': partition, 'row_buffer': row_buffer})
	}
	window.onresize = function(){floodDOM();}
$(window).load(function () {
floodDOM();
});
</script>
";
            result += lip.Text;
        }
        return result;
        //pdn_j.Controls.Add(lip);
    }
}