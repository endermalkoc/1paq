using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.IO;
using HtmlAgilityPack;

namespace onepaq.Shared.Module
{
    public class MainImage : IModule
    {
        private const string baseWikiUrl = "http://en.wikipedia.org/wiki/";
        private const string wikiUrl = "http://en.wikipedia.org";

        public bool IsDynamic
        {
            get { return false; }
        }

        string IModule.Title
        {
            get
            {
                return "Main Image";
            }
        }

        XmlDocument IModule.GetData(string keyword, string connectionString, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0) keyword = parameters[0].ToString();

            string strURL = baseWikiUrl + keyword.Replace(' ', '_');
            HttpWebRequest wbrq = (HttpWebRequest)WebRequest.Create(strURL);
            wbrq.Method = "GET";
            HttpWebResponse wbrs = (HttpWebResponse)wbrq.GetResponse();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            using (StreamReader sr = new StreamReader(wbrs.GetResponseStream()))
            {
                doc.Load(sr);
                doc.OptionFixNestedTags = true;
                doc.OptionOutputAsXml = true;

                HtmlNodeCollection imageCellCollection = doc.DocumentNode.SelectNodes("//a[@class=\"image\"]");

                if (imageCellCollection.Count > 0)
                {
                    HtmlNode firstImageCellNode = imageCellCollection[0];
                    HtmlNodeNavigator nav = (HtmlNodeNavigator)firstImageCellNode.CreateNavigator();
                    StringBuilder content = new StringBuilder();
                    content.Append("<img src=\"");
                    nav.MoveToFirstChild();
                    content.Append(nav.CurrentNode.Attributes["src"].Value);
                    content.Append("\"></img>");

                    XmlDocument xmlContent = new XmlDocument();
                    xmlContent.LoadXml(content.ToString());
                    return xmlContent;
                }
            }

            return null;
        }
    }
}
