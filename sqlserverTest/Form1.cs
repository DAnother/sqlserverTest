using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlServerOperationFuncs;
using System.Data;

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

        string connectionString = "Data Source = (local); Integrated Security = true";
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
            string fileName = @"D:\Data\sqlserver\test.mdf";
            string dataTableName = "coor";

            SqlConnection sqlCnt = new SqlConnection(connectionString);
            sqlCD.sqlCreate(fileName, sqlCnt, AlwaysDeteleDataBase.Checked);
            // 创建数据表
            sqlCD.createDataTable(dataTableName, "id", "INT", sqlCnt, AlwaysDeteleDataTable.Checked);
            sqlCD.createDataTable("test", "id", "INT", sqlCnt, AlwaysDeteleDataTable.Checked);
            // 显示所有数据表
            Form1.form1.richTextBox1.Text += "数据表名：" + "\r\n";
            string[] tablesName = sqlCD.getDataTablesName(sqlCnt);
            foreach (var em in tablesName)
            {
                Form1.form1.richTextBox1.Text += em + "\r\n";
            }
            // 插入一列
            sqlDataOperater.addColumns(dataTableName, "X", "DECIMAL(12,4)", sqlCnt);
            sqlDataOperater.addColumns(dataTableName, "Y", "DECIMAL(12,4)", sqlCnt);
            sqlDataOperater.addColumns(dataTableName, "Z", "DECIMAL(12,4)", sqlCnt);
            // 查询某个数据表的列名
            Form1.form1.richTextBox1.Text += String.Format("数据表{0}的列名：", dataTableName) + "\r\n";
            string[] columns = r.getColumns(dataTableName, sqlCnt);
            foreach (var em in columns)
            {
                Form1.form1.richTextBox1.Text += em + "\r\n";
            }
            // 关闭连接 释放连接对象
            sqlCnt.Close();
            sqlCnt.Dispose();
        }

        private void Write_Click(object sender, EventArgs e)
        {
            string tableName = "coor";
            string[] columnsName = { "X", "Y", "Z" };
            InsertandDelete sqlDataOperater = new InsertandDelete();
            string[][] dataRows = new string[][]
            {
                new string[]{"1000","2000","3000" },
                new string[]{"4000","5000","6000" },
                new string[]{"7000" },
            };

            SqlConnection sqlCnt = new SqlConnection(connectionString);
            sqlCnt.Open();
            for (int i = 0; i < dataRows.Length; i++)
            {
                sqlDataOperater.insertValues(tableName, columnsName, dataRows[i], sqlCnt, AlwaysUsing0InsteadNull.Checked);
            }
            sqlCnt.Close();
            sqlCnt.Dispose();
        }

        private void Read_Click(object sender, EventArgs e)
        {
            SqlConnection sqlCnt = new SqlConnection(connectionString);
            sqlCnt.Open();
            Read r = new Read();
            string tableName = "coor";
            string columnName = "X";
            string[] table = r.getAllValues("coor", sqlCnt);
            foreach (string str in table)
            {
                richTextBox1.Text += str + "\r\n";
            }
            richTextBox1.Text += "\r\ncoor表中 字段 X 的值如下：\n";
            string[] values = r.getColumnValues(tableName, columnName, sqlCnt);
            foreach (string str in values)
            {
                richTextBox1.Text += str + "\r\n";
            }
            string[] columns = r.getTypeOfColumns("coor", sqlCnt);
            foreach (string str in columns)
            {
                richTextBox1.Text += str + "\r\n";
            }
            sqlCnt.Close();
            sqlCnt.Dispose();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "删除 Y 字段\r\n";
            SqlConnection sqlCnt = new SqlConnection(connectionString);
            sqlCnt.Open();
            sqlDataOperater.detColumn("Y", "coor", sqlCnt);

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
