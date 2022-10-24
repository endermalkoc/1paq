using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Xml;

namespace onepaq.Shared.Module
{
    public class RelatedSites : IModule
    {
        #region IModule Members

        public System.Xml.XmlDocument GetData(string keyword, string connectionString, params object[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.Indent = true;
            xws.NewLineHandling = NewLineHandling.Entitize;

            XmlWriter xw = XmlWriter.Create(sb);

            xw.WriteStartDocument();
            xw.WriteStartElement("Sites");

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string sSql = "SELECT SiteId, Name FROM SUM_SITE " +
                        "WHERE SiteTypeId = (SELECT SiteTypeId FROM SUM_SITE WHERE Name = @Name)" +
                        "AND SiteId <> (SELECT SiteId FROM SUM_SITE WHERE Name = @Name) ORDER BY RAND() LIMIT 10;";


                con.Open();
                using (MySqlCommand command = new MySqlCommand(sSql, con))
                {
                    command.Parameters.AddWithValue("Name", keyword);
                    using (MySqlDataReader rs = command.ExecuteReader())
                    {
                        while (rs.Read())
                        {
                            xw.WriteStartElement("Site");

                            xw.WriteAttributeString("Name", rs["Name"].ToString());

                            xw.WriteEndElement(); //Site
                        }
                    }
                }
            }

            xw.WriteEndElement(); //Sites
            xw.WriteEndDocument(); //Document
            xw.Flush();

            XmlDocument xmlContent = new XmlDocument();
            xmlContent.LoadXml(sb.ToString());
            return xmlContent;

        }

        public string Title
        {
            get { return "Related Sites"; }
        }

        public bool IsDynamic
        {
            get { return true; }
        }

        #endregion
    }
}
