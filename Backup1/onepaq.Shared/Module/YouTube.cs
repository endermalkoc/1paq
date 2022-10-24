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
using Google.YouTube;

namespace onepaq.Shared.Module
{
    public class YouTube : IModule
    {
        public bool IsDynamic
        {
            get { return false; }
        }

        string IModule.Title
        {
            get
            {
                return "Related Videos";
            }
        }

        XmlDocument IModule.GetData(string keyword, string connectionString, params object[] parameters)
        {
            YouTubeQuery query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);

            query.Query = keyword;
            query.OrderBy = "relevance";
            query.Time = YouTubeQuery.UploadTime.AllTime;
            query.NumberToRetrieve = 10;

            IEnumerable<Video> videos = GetVideos(query);

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.Indent = true;
            xws.NewLineHandling = NewLineHandling.Entitize;

            XmlWriter xw = XmlWriter.Create(sb);

            xw.WriteStartDocument();
            xw.WriteStartElement("Videos");

            for (int i = 0; i < 10; i++)
            {
                Video video = videos.ElementAt(i);

                xw.WriteStartElement("Video");

                xw.WriteAttributeString("Title", video.Title);
                xw.WriteAttributeString("Description", video.Description);
                xw.WriteAttributeString("Keywords", video.Keywords);
                xw.WriteAttributeString("WatchPage", video.WatchPage.ToString());
                xw.WriteAttributeString("Duration", video.Media.Duration.Seconds);
                xw.WriteAttributeString("ThumbnailUrl", ((Google.GData.Extensions.MediaRss.MediaThumbnail)video.Thumbnails[0]).Url);
                xw.WriteAttributeString("ThumbnailWidth", ((Google.GData.Extensions.MediaRss.MediaThumbnail)video.Thumbnails[0]).Width);
                xw.WriteAttributeString("ThumbnailHeight", ((Google.GData.Extensions.MediaRss.MediaThumbnail)video.Thumbnails[0]).Height);

                xw.WriteEndElement(); //Video
            }

            xw.WriteEndElement(); //Videos
            xw.WriteEndDocument(); //Document
            xw.Flush();

            XmlDocument xmlContent = new XmlDocument();
            xmlContent.LoadXml(sb.ToString());
            return xmlContent;
        }

        private IEnumerable<Video> GetVideos(YouTubeQuery q)
        {
            YouTubeRequest request = GetRequest();
            Feed<Video> feed = null; 

            try
            {
                feed = request.Get<Video>(q);
            }
            catch (GDataRequestException gdre)
            {
                HttpWebResponse response = (HttpWebResponse)gdre.Response;
            }

            return feed != null ? feed.Entries : null;
        }

        private YouTubeRequest GetRequest()
        {
            YouTubeRequestSettings settings = new YouTubeRequestSettings("sumest",
                                            null,
                                            "AI39si7ueCgjlowjXrRVWItTnGy2Eb2IG5dkFBC1XdfyIyEzhWm_CGwvyf1HngCPAEXT7PtNBpsKTepHgc1l18_41eG8hXIRYA",
                                            null);
            settings.AutoPaging = true;


            return new YouTubeRequest(settings);
        }
    }
}
