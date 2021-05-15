
namespace frontlook_csharp_library.FL_Controls
{
    partial class FL_ProgressBar
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progress = new System.Windows.Forms.ProgressBar();
            this.progressText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progress
            // 
            this.progress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progress.Location = new System.Drawing.Point(0, 21);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(239, 23);
            this.progress.TabIndex = 0;
            // 
            // progressText
            // 
            this.progressText.AutoSize = true;
            this.progressText.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressText.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressText.Location = new System.Drawing.Point(0, 0);
            this.progressText.Name = "progressText";
            this.progressText.Size = new System.Drawing.Size(92, 19);
            this.progressText.TabIndex = 1;
            this.progressText.Text = "ProgressText";
            // 
            // FL_ProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Controls.Add(this.progressText);
            this.Controls.Add(this.progress);
            this.Name = "FL_ProgressBar";
            this.Size = new System.Drawing.Size(239, 44);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Label progressText;
    }
}
