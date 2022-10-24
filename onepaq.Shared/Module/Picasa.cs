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
using Google.GData.Photos;
using Google.YouTube;

namespace onepaq.Shared.Module
{
    public class Picasa : IModule
    {
        public bool IsDynamic
        {
            get { return false; }
        }

        string IModule.Title
        {
            get
            {
                return "Related Images";
            }
        }

        XmlDocument IModule.GetData(string keyword, string connectionString, params object[] parameters)
        {
            PicasaQuery query = new PicasaQuery("http://picasaweb.google.com/data/feed/api/all");
            PicasaService service = new PicasaService("sumest");

            query.Query = keyword;
            query.NumberToRetrieve = 12;
            query.KindParameter = "photo";

            PicasaFeed feed = (PicasaFeed)service.Query(query);

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.Indent = true;
            xws.NewLineHandling = NewLineHandling.Entitize;

            XmlWriter xw = XmlWriter.Create(sb);

            xw.WriteStartDocument();
            xw.WriteStartElement("Images");

            foreach (PicasaEntry entry in feed.Entries)
            {
                xw.WriteStartElement("Image");

                xw.WriteAttributeString("Title", entry.Title.Text);
                xw.WriteAttributeString("Url", entry.Media.Content.Url);
                xw.WriteAttributeString("Width", entry.Media.Content.Width);
                xw.WriteAttributeString("Height", entry.Media.Content.Height);
                xw.WriteAttributeString("Description", entry.Media.Description.Value);
                xw.WriteAttributeString("ThumbnailUrl", ((Google.GData.Extensions.MediaRss.MediaThumbnail)entry.Media.Thumbnails[0]).Url);
                xw.WriteAttributeString("ThumbnailWidth", ((Google.GData.Extensions.MediaRss.MediaThumbnail)entry.Media.Thumbnails[0]).Width);
                xw.WriteAttributeString("ThumbnailHeight", ((Google.GData.Extensions.MediaRss.MediaThumbnail)entry.Media.Thumbnails[0]).Height);

                xw.WriteEndElement(); //Image
            }

            xw.WriteEndElement(); //Videos
            xw.WriteEndDocument(); //Document
            xw.Flush();

            XmlDocument xmlContent = new XmlDocument();
            xmlContent.LoadXml(sb.ToString());
            return xmlContent;
        }

    }
}
