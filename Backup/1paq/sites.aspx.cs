using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace onepaq
{
    public partial class sites : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string SiteLinks()
        {
            StringBuilder sb = new StringBuilder();

            onepaq.DataSet.siteTableAdapters.SumSiteTableAdapter adapter = new onepaq.DataSet.siteTableAdapters.SumSiteTableAdapter();

            onepaq.DataSet.site.SumSiteDataTable dt = null;

            if (Request.QueryString["id"] == null)
            {
                dt = adapter.GetData();
            }
            else
            {
                dt = adapter.GetDataBySiteTypeId(int.Parse(Request.QueryString["id"]));
            }

            foreach(onepaq.DataSet.site.SumSiteRow dr in dt.Rows)
            {
                sb.Append("<a href=\"/Default.aspx?q=" + dr.Name + "\">" + dr.Name + "</a><br />");
            }

            return sb.ToString();
        }
    }
}
