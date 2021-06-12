using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontlook_csharp_library.FL_XmlDataInterop
{
    public static class FL_XmlToData
    {
        /*public static DataSet FL_ConvertXMLtoDataSet(this string XmlPath)
        {
            DataSet ds;
            try
            {
                ds = new DataSet();
                ds.ReadXml(XmlPath);
                return ds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }*/
        public static DataTable FL_ConvertXMLtoDataTable(this string XmlPath)
        {
            DataTable dt = new DataTable();
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(XmlPath);
                var schemaPath = Path.Combine(Path.GetDirectoryName(XmlPath), $"{fileName}_Schema.xml");
                dt.ReadXmlSchema(schemaPath);
                dt.ReadXml(XmlPath);
                return dt;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public static string FL_StringifyXML(this string XmlPath)
        {
            var xml = File.ReadAllText(XmlPath);
            return xml.ToString();
        }
    }
}
