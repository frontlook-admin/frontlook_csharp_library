using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace frontlook_csharp_library.FL_General
{
    public static class FL_ThreadHealper
    {
        delegate void SetTextF(Control ctrl, Form f, string text);

        delegate void SetTextUC(Control ctrl, UserControl f, string text);


        delegate void SetTextUCTS(ToolStrip ctrl, UserControl f, string text);


        delegate void SetTextUCTB(ToolStrip ctrl, UserControl f, string text);


        delegate string GetTextF(Control ctrl, Form f);

        delegate string GetTextUC(Control ctrl, UserControl f);
        /// <summary>
        /// Set text property of various controls
        /// </summary>
        /// <param name="form">The calling form</param>
        /// <param name="ctrl"></param>
        /// <param name="text"></param>
        public static void FL_SetText(this Control ctrl, Form form, string text)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (ctrl.InvokeRequired)
            {
                SetTextF d = new SetTextF(FL_SetText);
                form.Invoke(d, new object[] { ctrl, form, text });
            }
            else
            {
                ctrl.Text = text;
            }
        }

        /// <summary>
        /// Set text property of various controls
        /// </summary>
        /// <param name="form">The calling form</param>
        /// <param name="ctrl"></param>
        /// <param name="text"></param>
        public static void FL_SetText(this Control ctrl, UserControl form, string text)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (ctrl.InvokeRequired)
            {
                SetTextUC d = new SetTextUC(FL_SetText);
                form.Invoke(d, new object[] { ctrl, form, text });
            }
            else
            {
                ctrl.Text = text;
            }
        }

        /// <summary>
        /// Set text property of various controls
        /// </summary>
        /// <param name="form">The calling form</param>
        /// <param name="ctrl"></param>
        /// <param name="text"></param>
        public static void FL_SetText(this ToolStrip ctrl, UserControl form, string text)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (ctrl.InvokeRequired)
            {
                SetTextUC d = new SetTextUC(FL_SetText);
                form.Invoke(d, new object[] { ctrl, form, text });
            }
            else
            {
                ctrl.Text = text;
            }
        }

        public static string FL_GetString(this Control ctrl)
        {
            var rt = "";
            if(ctrl.GetType() == typeof(ComboBox))
            {
                ComboBox c = (ComboBox)ctrl;
                if (c.InvokeRequired)
                {
                    c.Invoke((MethodInvoker)delegate { rt = c.SelectedItem.ToString(); });
                }
                else
                {
                    rt = c.SelectedItem.ToString();
                }
                
            }
            if(ctrl.GetType() == typeof(MaskedTextBox))
            {
                MaskedTextBox c = (MaskedTextBox)ctrl;
                if (c.InvokeRequired)
                {
                    c.Invoke((MethodInvoker)delegate { rt = c.Text.ToString(); });
                }
                else
                {
                    rt = c.Text.ToString();
                }
            }
            if (ctrl.GetType() == typeof(TextBox))
            {
                TextBox c = (TextBox)ctrl;
                if (c.InvokeRequired)
                {
                    c.Invoke((MethodInvoker)delegate { rt = c.Text.ToString(); });
                }
                else
                {
                    rt = c.Text.ToString();
                }
            }

            return rt;
        }
    }
    /*
    public static class ThreadHelperClass
    {
        delegate void SetTextCallback(Form f, Control ctrl, string text);
        /// <summary>
        /// Set text property of various controls
        /// </summary>
        /// <param name="form">The calling form</param>
        /// <param name="ctrl"></param>
        /// <param name="text"></param>
        public static void SetText(this Form form, Control ctrl, string text)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (ctrl.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                form.Invoke(d, new object[] { form, ctrl, text });
            }
            else
            {
                ctrl.Text = text;
            }
        }
    }*/
}
