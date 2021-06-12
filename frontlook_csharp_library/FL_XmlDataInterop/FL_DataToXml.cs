using System.Data;
using System.IO;

namespace frontlook_csharp_library.FL_XmlDataInterop
{
    public static class FL_DataToXml
    {
        public static void FL_ConvertDataSetToXML(this DataSet ds, string XmlPath)
{
            var fileName = Path.GetFileNameWithoutExtension(XmlPath);
            var schemaPath = Path.Combine(Path.GetDirectoryName(XmlPath), $"{fileName}_Schema.xml");
            ds.WriteXml(XmlPath);
        }

        public static void FL_ConvertDataTableToXML(this DataTable dt, string XmlPath)
        {
            var fileName = Path.GetFileNameWithoutExtension(XmlPath);
            var schemaPath = Path.Combine(Path.GetDirectoryName(XmlPath), $"{fileName}_Schema.xml");
            dt.WriteXml(XmlPath);
            dt.WriteXmlSchema(schemaPath);
        }

        public static string FL_StringifyDataSetToXML(this DataSet ds)
        {
           var path = $"{Path.GetTempFileName()}.xml";
           ds.FL_ConvertDataSetToXML(path);
           var xml = File.ReadAllText(path);
           return xml.ToString();
        }

        public static string FL_StringifyDataTableToXML(this DataTable dt)
        {
            var path = $"{Path.GetTempFileName()}.xml";
            dt.FL_ConvertDataTableToXML(path);
            var xml = File.ReadAllText(path);
            return xml.ToString();
        }
    }
}
