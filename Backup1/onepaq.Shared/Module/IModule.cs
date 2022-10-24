using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace onepaq.Shared.Module
{
    public interface IModule
    {
        XmlDocument GetData(string keyword, string connectionString, params object[] parameters);
        string Title { get; }
        bool IsDynamic { get; }
    }
}
