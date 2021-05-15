namespace frontlook_csharp_library.FL_Controls
{
    partial class FL_SqlQueryManager
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.dbf_to_excel_series_worker = new System.ComponentModel.BackgroundWorker();
            this.button2 = new System.Windows.Forms.Button();
            this.test = new System.Windows.Forms.Button();
            this.nonquery = new System.Windows.Forms.Button();
            this.clear = new System.Windows.Forms.Button();
            this.dbf_to_excel_single = new System.Windows.Forms.Button();
            this.dbf_to_excel_series = new System.Windows.Forms.Button();
            this.view_db = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.db_viewer = new System.ComponentModel.BackgroundWorker();
            this.db_to_excel_single_worker = new System.ComponentModel.BackgroundWorker();
            this.label4 = new System.Windows.Forms.Label();
            this.query = new System.Windows.Forms.RichTextBox();
            this.progress = new frontlook_csharp_library.FL_Controls.FL_ProgressBar();
            this.stop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 42);
            this.label1.TabIndex = 7;
            this.label1.Text = "DBF Manager";
            // 
            // dbf_to_excel_series_worker
            // 
            this.dbf_to_excel_series_worker.WorkerSupportsCancellation = true;
            this.dbf_to_excel_series_worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Dbf_to_excel_series_worker_DoWork);
            this.dbf_to_excel_series_worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Dbf_to_excel_series_worker_ProgressChanged);
            this.dbf_to_excel_series_worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Dbf_to_excel_series_worker_RunWorkerCompleted);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.PowderBlue;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(931, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(51, 31);
            this.button2.TabIndex = 0;
            this.button2.Text = "TEST";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // test
            // 
            this.test.BackColor = System.Drawing.Color.Aquamarine;
            this.test.FlatAppearance.BorderSize = 0;
            this.test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.test.Location = new System.Drawing.Point(521, 44);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(247, 31);
            this.test.TabIndex = 0;
            this.test.Text = "EXECUTE QUERY";
            this.test.UseVisualStyleBackColor = false;
            this.test.Click += new System.EventHandler(this.Test_Click);
            // 
            // nonquery
            // 
            this.nonquery.BackColor = System.Drawing.Color.LightCoral;
            this.nonquery.FlatAppearance.BorderSize = 0;
            this.nonquery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nonquery.Location = new System.Drawing.Point(268, 45);
            this.nonquery.Name = "nonquery";
            this.nonquery.Size = new System.Drawing.Size(247, 31);
            this.nonquery.TabIndex = 0;
            this.nonquery.Text = "EXECUTE NON QUERY";
            this.nonquery.UseVisualStyleBackColor = false;
            this.nonquery.Click += new System.EventHandler(this.nonquery_Click);
            // 
            // clear
            // 
            this.clear.BackColor = System.Drawing.Color.PowderBlue;
            this.clear.FlatAppearance.BorderSize = 0;
            this.clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clear.Location = new System.Drawing.Point(756, 7);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(56, 31);
            this.clear.TabIndex = 0;
            this.clear.Text = "CLEAR";
            this.clear.UseVisualStyleBackColor = false;
            this.clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // dbf_to_excel_single
            // 
            this.dbf_to_excel_single.BackColor = System.Drawing.Color.PowderBlue;
            this.dbf_to_excel_single.FlatAppearance.BorderSize = 0;
            this.dbf_to_excel_single.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dbf_to_excel_single.Location = new System.Drawing.Point(565, 8);
            this.dbf_to_excel_single.Name = "dbf_to_excel_single";
            this.dbf_to_excel_single.Size = new System.Drawing.Size(188, 31);
            this.dbf_to_excel_single.TabIndex = 0;
            this.dbf_to_excel_single.Text = "SAVE SELECTED DBF TO EXCEL";
            this.dbf_to_excel_single.UseVisualStyleBackColor = false;
            this.dbf_to_excel_single.Click += new System.EventHandler(this.Dbf_to_excel_single_Click);
            // 
            // dbf_to_excel_series
            // 
            this.dbf_to_excel_series.BackColor = System.Drawing.Color.PowderBlue;
            this.dbf_to_excel_series.FlatAppearance.BorderSize = 0;
            this.dbf_to_excel_series.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dbf_to_excel_series.Location = new System.Drawing.Point(392, 7);
            this.dbf_to_excel_series.Name = "dbf_to_excel_series";
            this.dbf_to_excel_series.Size = new System.Drawing.Size(167, 34);
            this.dbf_to_excel_series.TabIndex = 0;
            this.dbf_to_excel_series.Text = "SAVE ALL DBF TO EXCLE";
            this.dbf_to_excel_series.UseVisualStyleBackColor = false;
            this.dbf_to_excel_series.Click += new System.EventHandler(this.Dbf_to_excel_series_Click);
            // 
            // view_db
            // 
            this.view_db.BackColor = System.Drawing.Color.PowderBlue;
            this.view_db.FlatAppearance.BorderSize = 0;
            this.view_db.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.view_db.Location = new System.Drawing.Point(818, 9);
            this.view_db.Name = "view_db";
            this.view_db.Size = new System.Drawing.Size(107, 30);
            this.view_db.TabIndex = 0;
            this.view_db.Text = "VIEW DATABASE";
            this.view_db.UseVisualStyleBackColor = false;
            this.view_db.Click += new System.EventHandler(this.View_db_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.PowderBlue;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(268, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "SELECT DATABASE";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridView1.Location = new System.Drawing.Point(3, 236);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridView1.Size = new System.Drawing.Size(981, 318);
            this.dataGridView1.TabIndex = 2;
            // 
            // db_viewer
            // 
            this.db_viewer.WorkerSupportsCancellation = true;
            this.db_viewer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Db_viewer_DoWork);
            this.db_viewer.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Db_viewer_ProgressChanged);
            this.db_viewer.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Db_viewer_RunWorkerCompleted);
            // 
            // db_to_excel_single_worker
            // 
            this.db_to_excel_single_worker.WorkerSupportsCancellation = true;
            this.db_to_excel_single_worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Db_to_excel_single_worker_DoWork);
            this.db_to_excel_single_worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Db_to_excel_single_worker_ProgressChanged);
            this.db_to_excel_single_worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Db_to_excel_single_worker_RunWorkerCompleted);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 19);
            this.label4.TabIndex = 11;
            this.label4.Text = "Query";
            // 
            // query
            // 
            this.query.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.query.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.query.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.query.ForeColor = System.Drawing.Color.Black;
            this.query.Location = new System.Drawing.Point(3, 78);
            this.query.Name = "query";
            this.query.Size = new System.Drawing.Size(979, 152);
            this.query.TabIndex = 13;
            this.query.Text = "";
            // 
            // progress
            // 
            this.progress.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.progress.BackColor = System.Drawing.Color.LightSkyBlue;
            this.progress.Location = new System.Drawing.Point(614, 78);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(368, 45);
            this.progress.TabIndex = 14;
            this.progress.Visible = false;
            // 
            // stop
            // 
            this.stop.BackColor = System.Drawing.Color.Crimson;
            this.stop.FlatAppearance.BorderSize = 0;
            this.stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stop.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.stop.Location = new System.Drawing.Point(774, 44);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(113, 31);
            this.stop.TabIndex = 0;
            this.stop.Text = "STOP";
            this.stop.UseVisualStyleBackColor = false;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // FL_SqlQueryManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.Controls.Add(this.progress);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.test);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.nonquery);
            this.Controls.Add(this.query);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dbf_to_excel_single);
            this.Controls.Add(this.dbf_to_excel_series);
            this.Controls.Add(this.view_db);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.MinimumSize = new System.Drawing.Size(986, 557);
            this.Name = "FL_SqlQueryManager";
            this.Size = new System.Drawing.Size(986, 557);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker dbf_to_excel_series_worker;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button dbf_to_excel_single;
        private System.Windows.Forms.Button dbf_to_excel_series;
        private System.Windows.Forms.Button view_db;
        private System.Windows.Forms.Button clear;
        private System.ComponentModel.BackgroundWorker db_viewer;
        private System.ComponentModel.BackgroundWorker db_to_excel_single_worker;
        private System.Windows.Forms.Button nonquery;
        private System.Windows.Forms.Button test;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox query;
        private System.Windows.Forms.DataGridView dataGridView1;
        //private Microsoft.Reporting.WinForms.ReportViewer rv;
        private System.Windows.Forms.Button button2;
        private FL_ProgressBar progress;
        private System.Windows.Forms.Button stop;
    }
}

