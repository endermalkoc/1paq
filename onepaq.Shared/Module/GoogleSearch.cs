using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Net;
using System.Xml;
using System.Text;
using System.IO;
using System.Resources;
using System.Reflection;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.YouTube;

namespace onepaq.Shared.Module
{
    //public class GoogleSearch : IModule
    //{
    //    #region IModule Members

    //    public System.Xml.XmlDocument GetData(string keyword, string connectionString, params object[] parameters)
    //    {
    //        GBaseQuery query = new GBaseQuery(GBaseUriFactory.Default.AttributesFeedUri);
    //        GBaseService service = new GBaseService("onepaq", "AI39si7ueCgjlowjXrRVWItTnGy2Eb2IG5dkFBC1XdfyIyEzhWm_CGwvyf1HngCPAEXT7PtNBpsKTepHgc1l18_41eG8hXIRYA");

    //        query.Query = keyword;
    //        query.NumberToRetrieve = 10;

    //        GBaseFeed feed = service.Query(query);

    //        StringBuilder sb = new StringBuilder();
    //        XmlWriterSettings xws = new XmlWriterSettings();
    //        xws.Indent = true;
    //        xws.NewLineHandling = NewLineHandling.Entitize;

    //        XmlWriter xw = XmlWriter.Create(sb);

    //        xw.WriteStartDocument();
    //        xw.WriteStartElement("SearchResults");

    //        foreach (GBaseEntry entry in feed.Entries)
    //        {
    //            xw.WriteStartElement("SearchResult");

    //            xw.WriteAttributeString("Title", entry.Title.Text);

    //            xw.WriteEndElement(); //SearchResult
    //        }

    //        xw.WriteEndElement(); //SearchResults
    //        xw.WriteEndDocument(); //Document
    //        xw.Flush();

    //        XmlDocument xmlContent = new XmlDocument();
    //        xmlContent.LoadXml(sb.ToString());
    //        return xmlContent;
    //    }

    //    public string Title
    //    {
    //        get { return "Google Search Results"; }
    //    }

    //    public bool IsDynamic
    //    {
    //        get { return true; }
    //    }

    //    #endregion
    //}
}
