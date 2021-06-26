using frontlook_csharp_library.FL_Dbf_Helper;
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
            if (!Directory.Exists(Path.GetDirectoryName(XmlPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(XmlPath));
            }
            var fileName = Path.GetFileNameWithoutExtension(XmlPath);
            var schemaPath = Path.Combine(Path.GetDirectoryName(XmlPath), $"{fileName}_Schema.xml");
            dt.WriteXml(XmlPath);
            dt.WriteXmlSchema(schemaPath);
        }

        public static void FL_ConvertDataTableToXML(this string sql, string DataTableName, string DbfFolderPath, string XmlFolderPath, bool UseDirectoryPath = true)
        {
            if (!Directory.Exists(XmlFolderPath))
            {
                Directory.CreateDirectory(XmlFolderPath);
            }
            var dt = sql.FL_DBF_ExecuteQuery(DataTableName, DbfFolderPath, UseDirectoryPath);
            var XmlPath = Path.Combine(XmlFolderPath, DataTableName, $"{DataTableName}.xml");
            dt.FL_ConvertDataTableToXML(XmlPath);
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
