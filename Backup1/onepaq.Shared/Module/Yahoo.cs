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
    public class Yahoo : IModule
    {
        #region IModule Members

        public bool IsDynamic
        {
            get { return false; }
        }

        XmlDocument IModule.GetData(string keyword, string connectionString, params object[] parameters)
        {
            string strURL = (string)parameters[0];
            HttpWebRequest wbrq = (HttpWebRequest)WebRequest.Create(strURL);
            wbrq.Method = "GET";
            HttpWebResponse wbrs = (HttpWebResponse)wbrq.GetResponse();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.Indent = true;
            xws.NewLineHandling = NewLineHandling.Entitize;

            XmlWriter xw = XmlWriter.Create(sb);

            xw.WriteStartDocument();
            xw.WriteStartElement("Links");

            using (StreamReader sr = new StreamReader(wbrs.GetResponseStream()))
            {
                doc.Load(sr);
                doc.OptionFixNestedTags = true;

                HtmlNode division = doc.DocumentNode.SelectSingleNode("//div[@class=\"st\"]");

                if (division != null)
                {
                    foreach (HtmlNode liNode in division.SelectSingleNode("ul").SelectNodes("li"))
                    {
                        xw.WriteStartElement("Link");

                        HtmlNode aNode = liNode.SelectSingleNode("a");

                        xw.WriteAttributeString("Href", aNode.GetAttributeValue("href", "").ToString());
                        xw.WriteAttributeString("Title", aNode.InnerText);

                        xw.WriteEndElement(); //Link
                    }
                }
            }

            xw.WriteEndElement(); //Links
            xw.WriteEndDocument(); //Document
            xw.Flush();

            XmlDocument xmlContent = new XmlDocument();
            xmlContent.LoadXml(sb.ToString());
            return xmlContent;
        }

        public string Title
        {
            get { return "Links"; }
        }

        #endregion
    }
}
