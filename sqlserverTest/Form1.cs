using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using sqlServerOperationFuncs;
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

        string connectionString = "Data Source = (local);Initial Catalog = tempdb;Integrated Security = true";
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
            sqlCD.sqlCreate(fileName, sqlCnt, true);
            // 创建数据表
            sqlCD.createDataTable(dataTableName, sqlCnt, true);
            sqlCD.createDataTable("test", sqlCnt, true);
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
            string[] columnsName = { "X", "Y" };
            InsertandDelete sqlDataOperate = new InsertandDelete();
            string[][] dataRows = new string[][]
            {
                new string[]{"1000","2000" },
                new string[]{"4000","5000" },
                new string[]{"6000" },
            };

            SqlConnection sqlCnt = new SqlConnection(connectionString);
            sqlCnt.Open();
            for (int i = 0; i < dataRows.Length; i++)
            {
                sqlDataOperate.insertValues(tableName, columnsName, dataRows[i], sqlCnt, true);
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
            richTextBox1.Text += "\r\n字段X的值如下：\n";
            string[] values = r.getColumnValues(tableName, columnName, sqlCnt);
            foreach (string str in values)
            {
                richTextBox1.Text += str + "\r\n";
            }
            sqlCnt.Close();
            sqlCnt.Dispose();
        }
    }
}
