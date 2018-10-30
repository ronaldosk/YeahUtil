namespace CodeGenerate
{
    partial class DataSourceConfig
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dataSourceOptionsPage1 = new CodeBuilder.WinForm.UI.OptionsPages.DataSourceOptionsPage();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dataSourceOptionsPage1);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(517, 371);
            this.groupBox5.TabIndex = 18;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "DataBase Setting";
            // 
            // dataSourceOptionsPage1
            // 
            this.dataSourceOptionsPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataSourceOptionsPage1.Location = new System.Drawing.Point(3, 17);
            this.dataSourceOptionsPage1.Name = "dataSourceOptionsPage1";
            this.dataSourceOptionsPage1.Size = new System.Drawing.Size(511, 351);
            this.dataSourceOptionsPage1.TabIndex = 0;
            // 
            // DataSourceConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 371);
            this.Controls.Add(this.groupBox5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DataSourceConfig";
            this.Text = "连接黑木崖";
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private CodeBuilder.WinForm.UI.OptionsPages.DataSourceOptionsPage dataSourceOptionsPage1;
    }
}