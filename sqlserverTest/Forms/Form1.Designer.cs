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
            this.AlwaysDeteleDataBase = new System.Windows.Forms.CheckBox();
            this.AlwaysDeteleDataTable = new System.Windows.Forms.CheckBox();
            this.AlwaysUsing0InsteadNull = new System.Windows.Forms.CheckBox();
            this.Delete = new System.Windows.Forms.Button();
            this.OpenForm2 = new System.Windows.Forms.Button();
            this.OpenForm3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Cteate
            // 
            this.Cteate.Location = new System.Drawing.Point(638, 12);
            this.Cteate.Name = "Cteate";
            this.Cteate.Size = new System.Drawing.Size(150, 91);
            this.Cteate.TabIndex = 0;
            this.Cteate.Text = "Create";
            this.Cteate.UseVisualStyleBackColor = true;
            this.Cteate.Click += new System.EventHandler(this.Create_Click);
            // 
            // Write
            // 
            this.Write.Location = new System.Drawing.Point(638, 131);
            this.Write.Name = "Write";
            this.Write.Size = new System.Drawing.Size(150, 81);
            this.Write.TabIndex = 1;
            this.Write.Text = "Write";
            this.Write.UseVisualStyleBackColor = true;
            this.Write.Click += new System.EventHandler(this.Write_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 38);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(620, 400);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // Read
            // 
            this.Read.Location = new System.Drawing.Point(638, 238);
            this.Read.Name = "Read";
            this.Read.Size = new System.Drawing.Size(150, 86);
            this.Read.TabIndex = 3;
            this.Read.Text = "Read";
            this.Read.UseVisualStyleBackColor = true;
            this.Read.Click += new System.EventHandler(this.Read_Click);
            // 
            // AlwaysDeteleDataBase
            // 
            this.AlwaysDeteleDataBase.AutoSize = true;
            this.AlwaysDeteleDataBase.Checked = true;
            this.AlwaysDeteleDataBase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AlwaysDeteleDataBase.Location = new System.Drawing.Point(12, 12);
            this.AlwaysDeteleDataBase.Name = "AlwaysDeteleDataBase";
            this.AlwaysDeteleDataBase.Size = new System.Drawing.Size(189, 19);
            this.AlwaysDeteleDataBase.TabIndex = 7;
            this.AlwaysDeteleDataBase.Text = "AlwaysDeteleDataBase";
            this.AlwaysDeteleDataBase.UseVisualStyleBackColor = true;
            // 
            // AlwaysDeteleDataTable
            // 
            this.AlwaysDeteleDataTable.AutoSize = true;
            this.AlwaysDeteleDataTable.Checked = true;
            this.AlwaysDeteleDataTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AlwaysDeteleDataTable.Location = new System.Drawing.Point(207, 12);
            this.AlwaysDeteleDataTable.Name = "AlwaysDeteleDataTable";
            this.AlwaysDeteleDataTable.Size = new System.Drawing.Size(197, 19);
            this.AlwaysDeteleDataTable.TabIndex = 8;
            this.AlwaysDeteleDataTable.Text = "AlwaysDeteleDataTable";
            this.AlwaysDeteleDataTable.UseVisualStyleBackColor = true;
            // 
            // AlwaysUsing0InsteadNull
            // 
            this.AlwaysUsing0InsteadNull.AutoSize = true;
            this.AlwaysUsing0InsteadNull.Checked = true;
            this.AlwaysUsing0InsteadNull.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AlwaysUsing0InsteadNull.Location = new System.Drawing.Point(410, 12);
            this.AlwaysUsing0InsteadNull.Name = "AlwaysUsing0InsteadNull";
            this.AlwaysUsing0InsteadNull.Size = new System.Drawing.Size(213, 19);
            this.AlwaysUsing0InsteadNull.TabIndex = 9;
            this.AlwaysUsing0InsteadNull.Text = "AlwaysUsing0InsteadNull";
            this.AlwaysUsing0InsteadNull.UseVisualStyleBackColor = true;
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(638, 352);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(150, 86);
            this.Delete.TabIndex = 10;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // OpenForm2
            // 
            this.OpenForm2.Location = new System.Drawing.Point(537, 54);
            this.OpenForm2.Name = "OpenForm2";
            this.OpenForm2.Size = new System.Drawing.Size(86, 37);
            this.OpenForm2.TabIndex = 11;
            this.OpenForm2.Text = "Form2";
            this.OpenForm2.UseVisualStyleBackColor = true;
            this.OpenForm2.Click += new System.EventHandler(this.OpenForm2_Click);
            // 
            // OpenForm3
            // 
            this.OpenForm3.Location = new System.Drawing.Point(537, 121);
            this.OpenForm3.Name = "OpenForm3";
            this.OpenForm3.Size = new System.Drawing.Size(86, 35);
            this.OpenForm3.TabIndex = 12;
            this.OpenForm3.Text = "Form3";
            this.OpenForm3.UseVisualStyleBackColor = true;
            this.OpenForm3.Click += new System.EventHandler(this.OpenForm3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.OpenForm3);
            this.Controls.Add(this.OpenForm2);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.AlwaysUsing0InsteadNull);
            this.Controls.Add(this.AlwaysDeteleDataTable);
            this.Controls.Add(this.AlwaysDeteleDataBase);
            this.Controls.Add(this.Read);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Write);
            this.Controls.Add(this.Cteate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Cteate;
        private System.Windows.Forms.Button Write;
        private System.Windows.Forms.Button Read;
        public System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox AlwaysDeteleDataBase;
        private System.Windows.Forms.CheckBox AlwaysDeteleDataTable;
        private System.Windows.Forms.CheckBox AlwaysUsing0InsteadNull;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.Button OpenForm2;
        private System.Windows.Forms.Button OpenForm3;
    }
}

