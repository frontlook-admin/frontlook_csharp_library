using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace frontlook_csharp_library.FL_general
{
    public class FL_Combobox
    {
        public static String[] ComboBoxStrings(ComboBox comboBox)
        {
            string[] items = new string[comboBox.Items.Count];

            for(int i = 0; i<comboBox.Items.Count; i++)
            {
                items[i] = comboBox.Items[i].ToString();
            }
            return items;
        }

        public static IEnumerable<string> search_result(string tempStr, List<String> cmblist)
        {
            
            IEnumerable<string> data = (from m in cmblist
                where m.ToLower().Contains(tempStr.ToLower())
                select m);
            return data;
        }

    }
}
