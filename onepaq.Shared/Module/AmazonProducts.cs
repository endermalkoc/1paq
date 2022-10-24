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
    public class AmazonProducts : IModule
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
            xw.WriteStartElement("Products");

            Items[] products = AmazonGateway.SearchBooks(keyword, "MusicalInstruments");

            if (products != null)
            {
                foreach (Item product in products.ElementAt(0).Item)
                {
                    xw.WriteStartElement("Product");

                    if (product.DetailPageURL != null)
                        xw.WriteAttributeString("DetailPageUrl", product.DetailPageURL);

                    if (product.ItemAttributes != null && product.ItemAttributes.ListPrice != null)
                        xw.WriteAttributeString("Price", product.ItemAttributes.ListPrice.FormattedPrice);

                    if (product.ItemAttributes != null)
                        xw.WriteAttributeString("Title", product.ItemAttributes.Title);

                    if (product.ImageSets != null && product.ImageSets.Count() > 0 && product.ImageSets[0].ImageSet != null && product.ImageSets[0].ImageSet.Count() > 0)
                        xw.WriteAttributeString("ThumbnailUrl", product.ImageSets[0].ImageSet[0].ThumbnailImage.URL);

                    if (product.SmallImage != null && product.SmallImage.Width != null)
                        xw.WriteAttributeString("ThumbnailWidth", product.ImageSets[0].ImageSet[0].ThumbnailImage.Width.Value.ToString());

                    if (product.SmallImage != null && product.SmallImage.Height != null)
                        xw.WriteAttributeString("ThumbnailHeight", product.ImageSets[0].ImageSet[0].ThumbnailImage.Width.Value.ToString());

                    xw.WriteEndElement(); //Product
                }
            }

            xw.WriteEndElement(); //Products
            xw.WriteEndDocument(); //Document
            xw.Flush();

            XmlDocument xmlContent = new XmlDocument();
            xmlContent.LoadXml(sb.ToString());
            return xmlContent;
        }

        public string Title
        {
            get { return "Products at Amazon"; }
        }

        #endregion
    }

    public static class AmazonGateway
    {
        private const string privateKey = "V4h8m7k7OiEF7IcRM0s5w+JvsIU3rt+j3wFjwL8g";
        private const string publicKey = "AKIAIH5OIJCYZ2H56JHA";

        public static Items[] SearchBooks(string keyword, string category)
        {
            string filteredKeywords = string.Empty;

            foreach (char c in keyword)
            {
                if (char.IsLetterOrDigit(c))
                {
                    filteredKeywords += c;
                }
                else
                {
                    filteredKeywords += " ";
                }
            }

            filteredKeywords = filteredKeywords.Trim();

            ItemSearchRequest itemRequest = new ItemSearchRequest();
            itemRequest.Keywords = filteredKeywords;
            itemRequest.SearchIndex = category; // books only
            itemRequest.ResponseGroup = new string[] { "ItemAttributes", "Images" }; // images only

            ItemSearch request = new ItemSearch();
            request.AWSAccessKeyId = publicKey;
            request.Request = new ItemSearchRequest[] { itemRequest };

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = int.MaxValue;
            AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient(
                binding,
                new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));

            // add authentication to the ECS client 
            client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(publicKey, privateKey));

            ItemSearchResponse response = client.ItemSearch(request);

            // Determine if book was found
            bool itemFound = ((response.Items[0].Item != null)
                && (response.Items[0].Item.Length > 0));
            if (itemFound)
            {
                return response.Items;
            }
            else
            {
                return null;
            }
        }

        public static List<System.Drawing.Image> SearchAlbumArt(string movieKeywords)
        {
            string filteredKeywords = string.Empty;

            foreach (char c in movieKeywords)
            {
                if (char.IsLetterOrDigit(c))
                {
                    filteredKeywords += c;
                }
                else
                {
                    filteredKeywords += " ";
                }
            }

            filteredKeywords = filteredKeywords.Trim();
            
            ItemSearchRequest itemRequest = new ItemSearchRequest();
            itemRequest.Keywords = filteredKeywords;
            itemRequest.SearchIndex = "DVD"; // dvd’s only
            itemRequest.ResponseGroup = new string[] { "Images" }; // images only

            ItemSearch request = new ItemSearch();
            request.AWSAccessKeyId = publicKey;
            request.Request = new ItemSearchRequest[] { itemRequest };

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = int.MaxValue;
            AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient(
                binding,
                new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));

            // add authentication to the ECS client 
            client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(publicKey, privateKey));

            ItemSearchResponse response = client.ItemSearch(request);

            // Determine if book was found
            bool itemFound = ((response.Items[0].Item != null)
                && (response.Items[0].Item.Length > 0));
            if (itemFound)
            {
                List<System.Drawing.Image> images = new List<System.Drawing.Image>();

                foreach (Item currItem in response.Items[0].Item)
                {
                    try
                    {
                        images.Add(ConvertByteArrayToImage(
                            GetBytesFromUrl(currItem.LargeImage.URL)));
                    }
                    catch { }
                }

                return images;
            }
            else
            {
                return null;
            }
        }

        public static System.Drawing.Image ConvertByteArrayToImage(byte[] byteArray)
        {
            try
            {
                if (byteArray != null)
                {
                    MemoryStream ms = new MemoryStream(byteArray, 0,
                    byteArray.Length);
                    ms.Write(byteArray, 0, byteArray.Length);
                    return System.Drawing.Image.FromStream(ms, true);
                }
                return null;
            }
            catch { }
            return null;
        }

        static public byte[] GetBytesFromUrl(string url)
        {
            byte[] b;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            WebResponse myResp = myReq.GetResponse();

            Stream stream = myResp.GetResponseStream();
            //int i;
            using (BinaryReader br = new BinaryReader(stream))
            {
                //i = (int)(stream.Length);
                b = br.ReadBytes(500000);
                br.Close();
            }
            myResp.Close();
            return b;
        }

    }

    public class AmazonSigningEndpointBehavior : IEndpointBehavior
    {
        private string accessKeyId = "";
        private string secretKey = "";

        public AmazonSigningEndpointBehavior(string accessKeyId, string secretKey)
        {
            this.accessKeyId = accessKeyId;
            this.secretKey = secretKey;
        }

        public void ApplyClientBehavior(ServiceEndpoint serviceEndpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new AmazonSigningMessageInspector(accessKeyId, secretKey));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint serviceEndpoint, EndpointDispatcher endpointDispatcher) { return; }
        public void Validate(ServiceEndpoint serviceEndpoint) { return; }
        public void AddBindingParameters(ServiceEndpoint serviceEndpoint, BindingParameterCollection bindingParameters) { return; }

    }
    public class AmazonSigningMessageInspector : IClientMessageInspector
    {
        private string accessKeyId = "";
        private string secretKey = "";

        public AmazonSigningMessageInspector(string accessKeyId, string secretKey)
        {
            this.accessKeyId = accessKeyId;
            this.secretKey = secretKey;
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // prepare the data to sign
            string operation = Regex.Match(request.Headers.Action, "[^/]+$").ToString();
            DateTime now = DateTime.UtcNow;
            string timestamp = now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string signMe = operation + timestamp;
            byte[] bytesToSign = Encoding.UTF8.GetBytes(signMe);

            // sign the data
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            HMAC hmacSha256 = new HMACSHA256(secretKeyBytes);
            byte[] hashBytes = hmacSha256.ComputeHash(bytesToSign);
            string signature = Convert.ToBase64String(hashBytes);

            // add the signature information to the request headers
            request.Headers.Add(new AmazonHeader("AWSAccessKeyId", accessKeyId));
            request.Headers.Add(new AmazonHeader("Timestamp", timestamp));
            request.Headers.Add(new AmazonHeader("Signature", signature));

            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState) { }
    }
    public class AmazonHeader : MessageHeader
    {
        private string name;
        private string value;

        public AmazonHeader(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public override string Name { get { return name; } }
        public override string Namespace { get { return "http://security.amazonaws.com/doc/2007-01-01/"; } }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter xmlDictionaryWriter, MessageVersion messageVersion)
        {
            xmlDictionaryWriter.WriteString(value);
        }
    }
}
