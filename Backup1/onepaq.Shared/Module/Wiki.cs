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
    public class Wiki : IModule
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
                return "Definition";
            }
        }

        XmlDocument IModule.GetData(string keyword, string connectionString, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0) keyword = parameters[0].ToString();

            string strURL = baseWikiUrl + keyword.Replace(' ', '_');
            HttpWebRequest wbrq = (HttpWebRequest)WebRequest.Create(strURL);
            wbrq.Method = "GET";
            HttpWebResponse wbrs = null;

            try
            {
                wbrs = (HttpWebResponse)wbrq.GetResponse();
            }
            catch
            {
                return null;
            }

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            using (StreamReader sr = new StreamReader(wbrs.GetResponseStream()))
            {
                doc.Load(sr);
                doc.OptionFixNestedTags = true;
                doc.OptionOutputAsXml = true;

                HtmlNodeCollection infoboxCollection = doc.DocumentNode.SelectNodes("//table[@class=\"infobox\"]");
                HtmlNodeNavigator nav = null;

                if (infoboxCollection != null && infoboxCollection.Count > 0)
                {
                    HtmlNode infoboxNode = infoboxCollection[0];
                    nav = (HtmlNodeNavigator)infoboxNode.CreateNavigator();
                }
                else
                {
                    HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//body");
                    HtmlNode firstParagraph = bodyNode.SelectSingleNode("descendant::p");
                    nav = (HtmlNodeNavigator)firstParagraph.CreateNavigator();
                    nav.MoveToPrevious();
                }

                StringBuilder content = new StringBuilder();
                content.Append("<div>");

                if (nav.MoveToNext("p", ""))
                {
                    //Fix href links
                    foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                    {
                        HtmlAttribute att = link.Attributes["href"];

                        if (att.Value.Contains("#"))
                        {
                            att.Value = strURL + att.Value;
                        }
                        else
                        {
                            att.Value = wikiUrl + att.Value;
                        }
                    }

                    //Remove class attributes
                    foreach (HtmlNode cls in doc.DocumentNode.SelectNodes("//@class"))
                    {
                        HtmlAttribute att = cls.Attributes["class"];
                        att.Remove();
                    }

                    do
                    {
                        content.Append(nav.CurrentNode.OuterHtml);
                        nav.MoveToNext();

                        if (nav.CurrentNode.Name == "table" || nav.CurrentNode.Name == "h2") break;

                    } while (true);


                    content.Append("</div>");

                    XmlDocument xmlContent = new XmlDocument();
                    xmlContent.LoadXml(content.ToString());
                    return xmlContent;
                }
            }

            return null;
        }
    }
}
