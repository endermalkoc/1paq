using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using onepaq.Shared.AmazonService;

namespace onepaq.Shared.Module
{
    public class AmazonBooks : IModule
    {
        #region IModule Members

        public bool IsDynamic
        {
            get { return false; }
        }

        public XmlDocument GetData(string keyword, string connectionString, params object[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.Indent = true;
            xws.NewLineHandling = NewLineHandling.Entitize;

            XmlWriter xw = XmlWriter.Create(sb);

            xw.WriteStartDocument();
            xw.WriteStartElement("Books");

            Items[] books = AmazonGateway.SearchBooks(keyword, "Books");

            if (books != null)
            {
                foreach (Item book in books.ElementAt(0).Item)
                {
                    xw.WriteStartElement("Book");

                    if (book.DetailPageURL != null)
                        xw.WriteAttributeString("DetailPageUrl", book.DetailPageURL);

                    if (book.ItemAttributes != null && book.ItemAttributes.Author != null && book.ItemAttributes.Author.Count() > 0)
                        xw.WriteAttributeString("Author", book.ItemAttributes.Author[0]);

                    if (book.ItemAttributes != null)
                        xw.WriteAttributeString("Binding", book.ItemAttributes.Binding);

                    if (book.ItemAttributes != null && book.ItemAttributes.ListPrice != null)
                        xw.WriteAttributeString("Price", book.ItemAttributes.ListPrice.FormattedPrice);

                    if (book.ItemAttributes != null)
                        xw.WriteAttributeString("Publisher", book.ItemAttributes.Publisher);

                    if (book.ItemAttributes != null)
                        xw.WriteAttributeString("Title", book.ItemAttributes.Title);

                    if (book.SmallImage != null)
                        xw.WriteAttributeString("ThumbnailUrl", book.SmallImage.URL);

                    if (book.SmallImage != null && book.SmallImage.Width != null)
                        xw.WriteAttributeString("ThumbnailWidth", book.SmallImage.Width.Value.ToString());

                    if (book.SmallImage != null && book.SmallImage.Height != null)
                        xw.WriteAttributeString("ThumbnailHeight", book.SmallImage.Height.Value.ToString());

                    xw.WriteEndElement(); //Book
                }
            }

            xw.WriteEndElement(); //Books
            xw.WriteEndDocument(); //Document
            xw.Flush();

            XmlDocument xmlContent = new XmlDocument();
            xmlContent.LoadXml(sb.ToString());
            return xmlContent;
        }

        public string Title
        {
            get { return "Books at Amazon"; }
        }

        #endregion
    }
}
