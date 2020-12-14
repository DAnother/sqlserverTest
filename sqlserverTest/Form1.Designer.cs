namespace sqlserverTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Cteate = new System.Windows.Forms.Button();
            this.Write = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Read = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Cteate
            // 
            this.Cteate.Location = new System.Drawing.Point(638, 12);
            this.Cteate.Name = "Cteate";
            this.Cteate.Size = new System.Drawing.Size(150, 144);
            this.Cteate.TabIndex = 0;
            this.Cteate.Text = "Create";
            this.Cteate.UseVisualStyleBackColor = true;
            this.Cteate.Click += new System.EventHandler(this.Create_Click);
            // 
            // Write
            // 
            this.Write.Location = new System.Drawing.Point(638, 162);
            this.Write.Name = "Write";
            this.Write.Size = new System.Drawing.Size(150, 135);
            this.Write.TabIndex = 1;
            this.Write.Text = "Write";
            this.Write.UseVisualStyleBackColor = true;
            this.Write.Click += new System.EventHandler(this.Write_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(620, 426);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // Read
            // 
            this.Read.Location = new System.Drawing.Point(638, 303);
            this.Read.Name = "Read";
            this.Read.Size = new System.Drawing.Size(150, 135);
            this.Read.TabIndex = 3;
            this.Read.Text = "Read";
            this.Read.UseVisualStyleBackColor = true;
            this.Read.Click += new System.EventHandler(this.Read_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Read);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Write);
            this.Controls.Add(this.Cteate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Cteate;
        private System.Windows.Forms.Button Write;
        private System.Windows.Forms.Button Read;
        public System.Windows.Forms.RichTextBox richTextBox1;
    }
}

