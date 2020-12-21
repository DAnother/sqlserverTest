using SqlServerOperationFuncs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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
            cd.isAlwaysDetDataBase = true;
            cd.isAlwaysDetDataTable = true;
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
            string dataBaseName = Path.GetFileNameWithoutExtension(fileName);
            cd.sqlCreate(fileName, sqlCnt);
            for (int count = 0; count < tables.Length; count++)
            {
                cd.createDataTable(dataBaseName, tables[count], columns[count * 2][0], columns[count * 2 + 1][0], sqlCnt);
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

        private void button2_Click(object sender, EventArgs e)
        {
            CreateandDrop cd = new CreateandDrop();
            string connectionString = @"Data Source=(local);DataBase=D:\DATA\SQLSERVER\20201030.MDF;Integrated Security=True;";
            SqlConnection sqlCnt = new SqlConnection(connectionString);
            sqlCnt.Open();
            int flag = cd.isDataTableExists("HD_STREETVIEW_CARINFO", sqlCnt);
            richTextBox1.Text = flag.ToString(); 
        }
    }
}
