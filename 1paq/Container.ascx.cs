using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace onepaq
{
    public partial class Container : System.Web.UI.UserControl
    {
        public string Title
        {
            get
            {
                return lblTitle.Text;
            }
            set
            {
                lblTitle.Text = value;
            }
        }

        public string Content
        {
            get
            {
                return ltContent.Text;
            }
            set
            {
                ltContent.Text = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}