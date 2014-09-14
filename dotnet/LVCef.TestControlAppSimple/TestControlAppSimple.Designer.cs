namespace TestControlAppSimple
{
    partial class TestControlAppSimpleForm
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
            this.lvCefControl1 = new LVCef.Control.LVCefControl();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvCefControl1
            // 
            this.lvCefControl1.Location = new System.Drawing.Point(12, 41);
            this.lvCefControl1.Name = "lvCefControl1";
            this.lvCefControl1.Size = new System.Drawing.Size(725, 341);
            this.lvCefControl1.TabIndex = 0;
            this.lvCefControl1.Text = "lvCefControl1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Create Browser";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(133, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(121, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Navigate to Dummy";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.TestControlAppSimpleForm_Click);
            // 
            // TestControlAppSimpleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 394);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lvCefControl1);
            this.Name = "TestControlAppSimpleForm";
            this.Text = "Form1";
            this.Click += new System.EventHandler(this.TestControlAppSimpleForm_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private LVCef.Control.LVCefControl lvCefControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;

    }
}

