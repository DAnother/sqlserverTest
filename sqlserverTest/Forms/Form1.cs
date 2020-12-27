using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlServerOperationFuncs;
using System.Data;
using System.IO;

namespace sqlserverTest
{
    public partial class Form1 : Form
    {
        public static Form1 form1;
        public Form1()
        {
            InitializeComponent();
            form1 = this;
        }

        string connectionString = "Data Source=(local);Integrated Security = true;User Instance=False";
        string fileName = @"D:\Data\sqlserver\test.mdf";
        CreateandDrop sqlCD = new CreateandDrop();
        InsertandDelete sqlDataOperater = new InsertandDelete();
        Read r = new Read();
        /// <summary>
        /// 实现创建数据库、数据表、字段、字段数据填充功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Create_Click(object sender, EventArgs e)
        {
            connectionString = "Data Source=(local);Integrated Security = true;User Instance=False";
            sqlCD.isAlwaysDetDataBase = true;
            sqlCD.isAlwaysDetDataTable = true;
            string dataBaseName = Path.GetFileNameWithoutExtension(fileName);
            string dataTableName = "coor";

            SqlConnection sqlCnt = new SqlConnection(connectionString);
            sqlCnt.Open();
            sqlCD.sqlCreate(fileName, sqlCnt);
            //sqlCnt.Dispose();


            // 创建数据表
            sqlCD.isAlwaysDetDataBase = AlwaysDeteleDataTable.Checked;
            sqlCD.isAlwaysDetDataTable = AlwaysDeteleDataTable.Checked;
            sqlCD.createDataTable(dataBaseName, dataTableName, "id", "INT", sqlCnt);
            sqlCD.createDataTable(dataBaseName, "table1", "id", "INT", sqlCnt);

            sqlCnt.Close();
            connectionString += ";Database=" + dataBaseName;
            sqlCnt.ConnectionString = connectionString;
            sqlCnt.Open();
            // 显示所有数据表
            Form1.form1.richTextBox1.Text += "数据表名：" + "\r\n";
            string[] tablesName = sqlCD.getDataTablesName(dataBaseName, sqlCnt);
            foreach (var em in tablesName)
            {
                Form1.form1.richTextBox1.Text += em + "\r\n";
            }
            // 插入一列
            sqlDataOperater.addColumns(dataBaseName, dataTableName, "X", "DECIMAL(12,4)", sqlCnt);
            sqlDataOperater.addColumns(dataBaseName, dataTableName, "Y", "DECIMAL(12,4)", sqlCnt);
            sqlDataOperater.addColumns(dataBaseName, dataTableName, "Z", "DECIMAL(12,4)", sqlCnt);
            // 查询某个数据表的列名
            Form1.form1.richTextBox1.Text += String.Format("数据表{0}的列名：", dataTableName) + "\r\n";
            string[] columns = r.getColumns(dataBaseName, dataTableName, sqlCnt);
            foreach (var em in columns)
            {
                Form1.form1.richTextBox1.Text += em + "\r\n";
            }

            sqlCD.detachDataBase(dataBaseName, sqlCnt);
            // 关闭连接 释放连接对象
            sqlCnt.Dispose();
        }

        private void Write_Click(object sender, EventArgs e)
        {
            connectionString =
                String.Format("Data Source=(local);Integrated Security = true;AttachDBFileName={0}", fileName);
            connectionString += ";DataBase=" + Path.GetFileNameWithoutExtension(fileName);
            string dataBaseName = Path.GetFileNameWithoutExtension(fileName);
            string tableName = "coor";
            string[] columnsName = { "id","X", "Y", "Z" };
            InsertandDelete sqlDataOperater = new InsertandDelete();
            string[][] dataRows = new string[][]
            {
                new string[]{"1","1000","2000","3000" },
                new string[]{"2","4000","5000","6000" },
                new string[]{"3","7000" },
            };

            SqlConnection sqlCnt = new SqlConnection(connectionString);
            sqlCnt.Open();
            for (int i = 0; i < dataRows.Length; i++)
            {
                sqlDataOperater.insertValues(dataBaseName, tableName, columnsName, dataRows[i], sqlCnt, AlwaysUsing0InsteadNull.Checked);
            }
            sqlCD.detachDataBase(dataBaseName, sqlCnt);
            sqlCnt.Close();
            sqlCnt.Dispose();
        }

        private void Read_Click(object sender, EventArgs e)
        {
            connectionString = "Data Source=(local);Integrated Security = true;User Instance=False;AttachDBFileName=";
            connectionString += fileName;
            string dataBaseName = fileName;
            SqlConnection sqlCnt = new SqlConnection(connectionString);
            sqlCnt.Open();
            Read r = new Read();
            string tableName = "coor";
            string columnName = "X";
            string[] table = r.getAllValues(dataBaseName, "coor", sqlCnt);
            foreach (string str in table)
            {
                richTextBox1.Text += str + "\r\n";
            }
            richTextBox1.Text += "\r\ncoor表中 字段 X 的值如下：\n";
            string[] values = r.getColumnValues(dataBaseName, tableName, columnName,"INT", sqlCnt);
            foreach (string str in values)
            {
                richTextBox1.Text += str + "\r\n";
            }
            richTextBox1.Text += "字段类型如下：\r\n";
            string[] columns = r.getTypeOfColumns(dataBaseName, "coor", sqlCnt);
            foreach (string str in columns)
            {
                richTextBox1.Text += str + "\r\n";
            }
            sqlCD.detachDataBase(dataBaseName, sqlCnt);
            sqlCnt.Close();
            sqlCnt.Dispose();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "删除 Y 字段\r\n";
            SqlConnection sqlCnt = new SqlConnection(connectionString);
            sqlCnt.Open();
            string dataBaseName = fileName;
            sqlDataOperater.detColumn(dataBaseName, "Y", "coor", sqlCnt);
            sqlCD.detachDataBase(dataBaseName, sqlCnt);
            sqlCnt.Close();
            sqlCnt.Dispose();
        }

        private void OpenForm2_Click(object sender, EventArgs e)
        {
            Form form2 = new Form2();
            form2.ShowDialog();
        }

        private void OpenForm3_Click(object sender, EventArgs e)
        {
            Form form3 = new Form3();
            form3.ShowDialog();
        }
    }
}
