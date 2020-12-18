using SqlServerOperationFuncs;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using FileOperator;

namespace sqlserverTest
{
    public partial class Form2 : Form
    {
        public static Form2 form2;
        public Form2()
        {
            InitializeComponent();
            form2 = this;
        }


        //public string connectionString = "Data Source=(local);Integrated Security = true;User Instance=False" +
        //   ";Initial Catalog=";
        public string connectionString = "Data Source=(local);Integrated Security = true;User Instance=False" +
            ";AttachDBFileName=";

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = Form2.form2.textBox1.Text;
            //string fileName = Path.GetFileNameWithoutExtension(jiafang.form2.textBox1.Text);
            connectionString += fileName;

            SqlConnection sqlCnt = new SqlConnection(connectionString);
            sqlCnt.Open();

            CreateandDrop cd = new CreateandDrop();
            string[] tables = cd.getDataTablesName(sqlCnt);
            SqlCommand sqlCmd = sqlCnt.CreateCommand();


            Read r = new Read();
            string[] datas = r.getColumns("HD_STREETVIEW_FACADEINFO", sqlCnt);

            //if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
            //sqlCmd.CommandText =
            //    String.Format("EXEC sp_detach_db '{0}', 'true'", Path.GetFileNameWithoutExtension(fileName));
            //sqlCmd.ExecuteNonQuery();

            sqlCnt.Close();
            sqlCnt.Dispose();

            foreach (string table in tables)
                richTextBox1.Text += table + "\r\n";
            foreach (string data in datas)
                richTextBox1.Text += data + "\r\n";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            form2.textBox1.Text = 1.22.ToString("F8");

            //string fileName = jiafang.form2.textBox1.Text;
            string fileName = @"D:\Data\dwg\032028.dwg";
            Form2.form2.richTextBox1.Text += "\r\n文件" + fileName + "是否被占用：\r\n" +
                FileInfo.IsFileInUsing(fileName).ToString();
        }
    }
}
