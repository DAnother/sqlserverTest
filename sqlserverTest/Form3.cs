using SqlServerOperationFuncs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sqlserverTest
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateandDrop cd = new CreateandDrop();
            InsertandDelete insert = new InsertandDelete();
            Read reader = new Read();

            string fileName = @"D:\Data\sqlserver\20201030.mdf";

            string connectionString = "Data Source=(local);Integrated Security=true";
            connectionString += ";AttachDBFileName=" + fileName;
            SqlConnection sqlCnt = new SqlConnection(connectionString);
            sqlCnt.Open();

            List<string[]> columns = new List<string[]>();
            string[] tables = cd.getDataTablesName(sqlCnt);
            foreach (string table in tables)
            {
                richTextBox1.Text += table + "\r\n";
                string[] keys = reader.getColumns(table, sqlCnt);
                foreach (var key in keys)
                    richTextBox1.Text += key + ",";
                richTextBox1.Text += "\r\n";
                string[] key_type = reader.getTypeOfColumns(table, sqlCnt);
                foreach (var key in key_type)
                    richTextBox1.Text += key + ",";
                richTextBox1.Text += "\r\n\r\n";
                columns.Add(keys);
                columns.Add(key_type);
            }
            sqlCnt.Close();
            sqlCnt.Dispose();

            // 读取完之后，新建连接 写数据库
            connectionString = "Data Source=(local);Integrated Security=true";
            sqlCnt.ConnectionString = connectionString;
            sqlCnt.Open();
            fileName = @"D:\Data\sqlserver\20201030_test.mdf";
            cd.sqlCreate(fileName, sqlCnt, true);
            for (int count = 0; count < tables.Length; count++)
            {
                cd.createDataTable(tables[count], columns[count * 2][0], columns[count * 2 + 1][0], sqlCnt, true);
                for (int i = 1; i < columns[count * 2].Length; i++)
                {
                    insert.addColumns(tables[count], columns[count * 2][i], columns[count * 2 + 1][i], sqlCnt);
                }
            }

            richTextBox1.Text += "\r\n---------------------------------------\r\n";

            tables = cd.getDataTablesName(sqlCnt);
            foreach (string table in tables)
            {
                richTextBox1.Text += table + "\r\n";
                string[] keys = reader.getColumns(table, sqlCnt);
                foreach (var key in keys)
                    richTextBox1.Text += key + ",";
                richTextBox1.Text += "\r\n";
                string[] key_type = reader.getTypeOfColumns(table, sqlCnt);
                foreach (var key in key_type)
                    richTextBox1.Text += key + ",";
                richTextBox1.Text += "\r\n\r\n";
                columns.Add(keys);
                columns.Add(key_type);
            }
            sqlCnt.Close();
            sqlCnt.Dispose();
        }
    }
}
