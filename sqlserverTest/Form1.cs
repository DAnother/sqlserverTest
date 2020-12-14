using System;
using System.Windows.Forms;
using System.Data.SqlClient;

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

        /// <summary>
        /// 实现创建数据库、数据表、字段、字段数据填充功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Create_Click(object sender, EventArgs e)
        {
            CreateandDrop sqlCD = new CreateandDrop();
            string fileName = @"D:\Data\sqlserver\test.mdf";
            string dataBaseName = "testTable";
            string DataTableName = "coor";

            SqlConnection sqlCnt = new SqlConnection(connectionString);
            sqlCD.sqlCreate(fileName, sqlCnt);
            // 显示所有数据表
            string[] tablesName = sqlCD.getDataTablesName(dataBaseName, sqlCnt);
            foreach (var em in tablesName)
            {
                Form1.form1.richTextBox1.Text += em + "\r\n";
            }
            // 创建数据表
            sqlCD.createDataTable(DataTableName, sqlCnt);
            // 插入一列
            sqlCD.addColumns(DataTableName, new string[] { "X" }, new string[] { "INT" }, sqlCnt);
            // 查询某个数据表的列名
            string[] columns = sqlCD.getColumns(DataTableName, sqlCnt);
            foreach (var em in columns)
            {
                Form1.form1.richTextBox1.Text += em + "\r\n";
            }
        }

        private void Write_Click(object sender, EventArgs e)
        {
            SqlConnection sqlCnt = new SqlConnection(connectionString);
            rw.sqlWrite(@"D:\Data\sqlserver\test.mdf",sqlCnt);
        }

        private void Read_Click(object sender, EventArgs e)
        {
            rw.sqlRead(@"D:\Data\sqlserver\test.mdf");
        }
    }
}
