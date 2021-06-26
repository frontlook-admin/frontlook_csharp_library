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

namespace frontlook_csharp_library.FL_Forms
{
    public partial class FL_ProgressBarForm : Form
    {
        public Action action;

        public Action<string[]> actionwithargs;
        public FL_ProgressBarForm()
        {
            InitializeComponent();
            FL_Progress("", 100, 0, 0);
        }

        /*public FL_ProgressBar(string _progressText, double maxValue, double minValue, double currentValue)
        {
            InitializeComponent();
            FL_Progress(_progressText, maxValue, minValue, currentValue);
        }*/

        public FL_ProgressBarForm(string _progressText)
        {
            InitializeComponent();
            FL_Progress(_progressText, 100, 0, 0);
        }

        public FL_ProgressBarForm(string _progressText, int value)
        {
            InitializeComponent();
            FL_Progress(_progressText, 100, 0, 0);
            FL_Progress(value);
        }

        public FL_ProgressBarForm(string _progressText, int maxValue, int minValue, int currentValue)
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

        public void FL_Progress(string _progressString, int currentValue, bool clear = false)
        {
            progressText.Text = clear ? _progressString : $"{progressText.Text}  {_progressString}";
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

        public string FL_GetProgressText()
        {
            return progressText.Text.Trim('1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '%');
        }

        public void Dismiss()
        {
            progress.Value = 0;
            this.Hide();
        }

        public void DoAction(object sender, DoWorkEventArgs e)
        {
            action.Invoke();
        }


        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.Value = e.ProgressPercentage;
        }
    }
}