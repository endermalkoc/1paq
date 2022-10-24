using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Text;
using onepaq.DataSet;
using onepaq.DataSet.siteTableAdapters;
using onepaq.Shared.Module;

namespace onepaq
{
    public enum ModulePosition
    {
        Top = 1,
        Left = 2,
        Center = 3,
        Right = 4,
        Bottom = 5,
        Logo = 6
    }

    public partial class _Default : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            string host = Request.Url.Host;
            string keyword = Request.QueryString["q"];

            if (host.IndexOf('.') > -1)
            {
                string subdomain = host.Substring(0, host.IndexOf('.'));

                if (subdomain != "www" && subdomain != "1paq" && subdomain != "onepaq")
                {
                    keyword = subdomain[0].ToString().ToUpper() + subdomain.Substring(1);
                }
            }

            if (keyword == null)
            {
                Response.Redirect("sites.aspx");
            }

            Page.Title = keyword;
            lblTitle.Text = keyword;

            SiteModuleTableAdapter adapter = new SiteModuleTableAdapter();
            SumModuleInstanceTableAdapter modAdapter = new SumModuleInstanceTableAdapter();

            onepaq.DataSet.site.SiteModuleDataTable dt = adapter.GetDataBySiteName(keyword);

            if (dt.Rows.Count == 0)
            {
                // TODO : Show 404
            }
            else
            {
                foreach (onepaq.DataSet.site.SiteModuleRow dr in dt.Rows)
                {
                    XmlDocument content = null;

                    IModule module = (IModule)Activator.CreateInstance(Assembly.GetAssembly(typeof(onepaq.Shared.Module.IModule)).GetType(dr.ClassName));

                    if (dr.IsModuleContentNull())
                    {
                        if (dr.IsParametersNull())
                        {
                            content = module.GetData(keyword, System.Configuration.ConfigurationManager.ConnectionStrings["onepaqConnectionString"].ConnectionString);
                        }
                        else
                        {
                            content = module.GetData(keyword, System.Configuration.ConfigurationManager.ConnectionStrings["onepaqConnectionString"].ConnectionString, dr.Parameters);
                        }

                        if (!module.IsDynamic)
                        {
                            onepaq.DataSet.site.SumModuleInstanceDataTable smidt =
                                modAdapter.GetDataBySiteIdAndSiteTypeModuleMapId(dr.SiteId, dr.SiteTypeModuleMapId);

                            if (smidt.Rows.Count == 0)
                            {
                                onepaq.DataSet.site.SumModuleInstanceRow newRow = smidt.NewSumModuleInstanceRow();
                                newRow.SiteId = dr.SiteId;
                                newRow.SiteTypeModuleMapId = dr.SiteTypeModuleMapId;
                                newRow.ModuleContent = content.OuterXml;

                                smidt.Rows.Add(newRow);
                            }
                            else
                            {
                                ((onepaq.DataSet.site.SumModuleInstanceRow)smidt.Rows[0]).ModuleContent = content.OuterXml;
                            }

                            modAdapter.Update(smidt);
                        }
                    }
                    else
                    {
                        content = new XmlDocument();
                        content.LoadXml(dr.ModuleContent);
                    }

                    string moduleName = module.GetType().Name;
                    XslCompiledTransform transform = null;

                    Control cntr = null;

                    if ((ModulePosition)dr.ModulePositionId == ModulePosition.Right)
                    {
                        cntr = new LiteralControl();
                    }
                    else
                    {
                        cntr = (Container)LoadControl("~/Container.ascx");
                        ((Container)cntr).Title = module.Title;
                    }

                    if (Global.Transformations.ContainsKey(moduleName))
                    {
                        transform = Global.Transformations[moduleName];
                    }
                    else
                    {
                        Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("onepaq.Xslt." + moduleName + ".xslt");

                        if (stream != null)
                        {
                            XmlDocument xslt = new XmlDocument();
                            transform = new XslCompiledTransform();
                            XmlTextReader xtr = new XmlTextReader(stream);
                            xslt.Load(xtr);
                            transform.Load(xslt);
                            Global.Transformations.Add(moduleName, transform);
                        }
                    }

                    if (transform != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        XmlWriter xw = XmlWriter.Create(sb);

                        transform.Transform(content, xw);

                        if (cntr is Container)
                        {
                            (cntr as Container).Content = sb.ToString();
                        }
                        else if (cntr is LiteralControl)
                        {
                            (cntr as LiteralControl).Text = sb.ToString();
                        }
                    }
                    else
                    {
                        if (cntr is Container)
                        {
                            (cntr as Container).Content = content.OuterXml;
                        }
                        else if (cntr is LiteralControl)
                        {
                            (cntr as LiteralControl).Text = content.OuterXml;
                        }
                    }

                    switch ((ModulePosition)dr.ModulePositionId)
                    {
                        case ModulePosition.Top:
                            pnlTopPane.Controls.Add(cntr);
                            break;
                        case ModulePosition.Left:
                            pnlLeftPane.Controls.Add(cntr);
                            break;
                        case ModulePosition.Center:
                            pnlCenterPane.Controls.Add(cntr);
                            break;
                        case ModulePosition.Right:
                            pnlRightPane.Controls.Add(cntr);
                            break;
                        case ModulePosition.Bottom:
                            pnlBottomPane.Controls.Add(cntr);
                            break;
                        case ModulePosition.Logo:
                            //pnlLogoPane.Controls.Add(cntr);
                            break;
                    }
                }
            }

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
