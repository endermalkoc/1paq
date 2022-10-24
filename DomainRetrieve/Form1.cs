using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace DomainRetrieve
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime firstDate = new DateTime(2009, 4, 29);
            this.Text = firstDate.ToShortDateString();
            Application.DoEvents();
            DomainTableAdapters.DomDomainTableAdapter adapter = new DomainRetrieve.DomainTableAdapters.DomDomainTableAdapter();

            while (firstDate.Year == 2009)
            {
                string month = firstDate.Month.ToString();
                string day = firstDate.Day.ToString();

                if (month.Length == 1) month = "0" + month;
                if (day.Length == 1) day = "0" + day;

                string url = string.Format("http://www.expire.biz/2009/{0}/{1}", month, day);

                HttpWebRequest wbrq = (HttpWebRequest)WebRequest.Create(url);
                wbrq.Method = "GET";
                HttpWebResponse wbrs = (HttpWebResponse)wbrq.GetResponse();

                using (StreamReader sr = new StreamReader(wbrs.GetResponseStream()))
                {
                    do
                    {
                        string line = sr.ReadLine();

                        if (line == null) break;

                        string domain = line.Substring(0, line.IndexOf("."));
                        string suffix = line.Substring(line.IndexOf(".") + 1);

                        if (suffix.ToUpper() == "COM" && domain.Length < 10)
                        {
                            adapter.Insert(domain, suffix, null, 0);
                        }
                    } while (true);
                }

                firstDate = firstDate.AddDays(1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sUrl = "http://instantdomainsearch.com/services/quick/?name={0}";

            DomainTableAdapters.DomDomainTableAdapter adapter = new DomainRetrieve.DomainTableAdapters.DomDomainTableAdapter();

            Domain.DomDomainDataTable dt = adapter.GetDataByScheduled();

            foreach (Domain.DomDomainRow dr in dt.Rows)
            {
                HttpWebRequest wbrq = (HttpWebRequest)WebRequest.Create(String.Format(sUrl, dr.DomainName));
                wbrq.Method = "GET";
                HttpWebResponse wbrs = (HttpWebResponse)wbrq.GetResponse();

                using (StreamReader sr = new StreamReader(wbrs.GetResponseStream()))
                {
                    string line = sr.ReadLine();

                    if (line == null) continue;

                    string status = line.Substring(line.IndexOf("'com'") + 7, 1);

                    if (status == "a")
                    {
                        dr.IsAvailable = 1;
                        dr.IsScheduledToCheck = 0;
                    }
                    else if (status == "u")
                    {
                        dr.IsAvailable = 0;
                        dr.IsScheduledToCheck = 0;
                    }

                    adapter.Update(dr);
                }
            }
        }
    }
}
