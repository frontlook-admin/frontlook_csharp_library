using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using frontlook_csharp_library.FL_General;

namespace frontlook_csharp_library.FL_Controls
{
    public partial class FL_ProgressBar : UserControl
    {
        public string MasterString;
        public FL_ProgressBar()
        {
            InitializeComponent();
            MasterString = "";
            FL_Progress("", 100, 0, 0);
        }

        /*public FL_ProgressBar(string _progressText, double maxValue, double minValue, double currentValue)
        {
            InitializeComponent();
            FL_Progress(_progressText, maxValue, minValue, currentValue);
        }*/

        public FL_ProgressBar(string _progressText)
        {
            InitializeComponent();
            FL_Progress(_progressText, 100, 0, 0);
        }

        public FL_ProgressBar(string _progressText, int maxValue, int minValue, int currentValue)
        {
            InitializeComponent();
            FL_Progress(_progressText, maxValue, minValue, currentValue);
        }

        public void FL_Progress(string _progressText, double maxValue, double minValue, double currentValue)
        {
            progressText.Text = _progressText;
            progress.Maximum = int.Parse(maxValue.FL_EvaluateRounded(0).ToString(CultureInfo.InvariantCulture));
            progress.Minimum = int.Parse(minValue.FL_EvaluateRounded(0).ToString(CultureInfo.InvariantCulture));
            progress.Value = int.Parse(currentValue.FL_EvaluateRounded(0).ToString(CultureInfo.InvariantCulture));
            if (currentValue != progress.Maximum && currentValue != progress.Minimum)
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
            this.Anchor = AnchorStyles.Bottom;
        }

        public void FL_Progress(string _progressText, int maxValue, int minValue, int currentValue)
        {
            progressText.Text = _progressText;
            progress.Maximum = maxValue;
            progress.Minimum = minValue;
            progress.Value = currentValue;
            if (currentValue != progress.Maximum && currentValue != progress.Minimum)
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
            this.Anchor = AnchorStyles.Bottom;
        }

        public void FL_Progress(double currentValue)
        {
            progress.Value = int.Parse(currentValue.FL_EvaluateRounded(0).ToString(CultureInfo.InvariantCulture));
            if (currentValue != progress.Maximum && currentValue != progress.Minimum)
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
        }

        public void FL_Progress(int currentValue)
        {
            progress.Value = currentValue;
            if (currentValue != progress.Maximum && currentValue != progress.Minimum)
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
        }

        public void FL_Progress(string _progressString,string _masterString, bool clear = false)
		{
            if(string.IsNullOrEmpty(_masterString))
			{
                MasterString = _masterString;
            }
            progressText.Text = clear ? _progressString : $"{MasterString}  {_progressString}";
        }
        

        public string FL_GetProgressText()
        {
            return progressText.Text.Trim('1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '%');
        }

        public void Dismiss()
        {
            progress.Value = 0;
            this.Hide();
        }
    }


    /*public partial class FL_ProgressBarObj : ProgressBar
    {
        
    }*/
    
}