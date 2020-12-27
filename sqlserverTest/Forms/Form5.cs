using SqlServerOperationFuncs;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace sqlserverTest
{
    public partial class Form5 : Form
    {
        public static Form5 form;
        public Form5()
        {
            InitializeComponent();
            form = this;
        }
        SqlConnection sqlCnt = new SqlConnection();
        CreateandDrop cd = new CreateandDrop();
        InsertandDelete Insert = new InsertandDelete();
        Read reader = new Read();

        string connectionString = "Data Source=(local);Integrated Security=true";
        static string fileName = @"D:\Data\sqlserver\copy\20201030.mdf";

        private void button1_Click(object sender, EventArgs e)
        {
            connectionString += ";AttachDBFileName=" + fileName;
            sqlCnt.ConnectionString = connectionString;
            sqlCnt.Open();
            int count = 0;
            string[] values = reader.getAllValues(fileName, "HD_STREETVIEW_FACADEINFO", sqlCnt);
            foreach (string value in values)
            {
                count++;
                richTextBox1.Text += count.ToString() + "\t" + value + "\r\n";
            }
            sqlCnt.Close();
            sqlCnt.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=(local);Integrated Security=true";
            connectionString += ";AttachDBFileName=" + fileName;
            string dataBaseName = fileName;
            sqlCnt.ConnectionString = connectionString;
            sqlCnt.Open();

            string[] tables = cd.getDataTablesName(dataBaseName, sqlCnt);
            foreach(string table in tables)
            {
                if (table == "HD_STREETVIEW_FACADEINFO")
                {
                    // 读取和写入
                    string[] values = reader.getAllValues(fileName, table, sqlCnt);
                    Insert.detAllValues(dataBaseName, table, sqlCnt);
                    for (int i = 1; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        string[] row = values[i].Split('\t');
                        string[] key = new string[row.Length];
                        string[] key_value = new string[row.Length];
                        for (int j = 0; j < row.Length; j++)
                        {
                            string[] keys = row[j].Split(':');
                            if (j > 3 && j < 7)
                            {
                                keys[1] = 0.ToString();
                            }
                            key[j] = keys[0];
                            key_value[j] = keys[1];
                        }
                        Insert.insertValues(dataBaseName, table, key, key_value, sqlCnt, true);
                    }
                }
            }
            sqlCnt.Close();

            sqlCnt.ConnectionString = connectionString;
            sqlCnt.Open();
            int count = 0;
            string[] Temp_values = reader.getAllValues(fileName, "HD_STREETVIEW_FACADEINFO", sqlCnt);
            foreach (string value in Temp_values)
            {
                count++;
                richTextBox1.Text += count.ToString() + " -- " + value + "\r\n";
            }
            sqlCnt.Close();
            sqlCnt.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string fileName_temp = @"D:\Data\image\image.mdf";
            string connectionString = "Data Source=(local);Integrated Security=true";
            try
            {
                string sqlStr = "insert into Image_test(Image) values(@i)";
                CreateandDrop cd = new CreateandDrop();
                SqlConnection sqlCnt = new SqlConnection(connectionString);
                cd.sqlCreate(fileName_temp, sqlCnt);
                cd.detachDataBase(Path.GetFileNameWithoutExtension(fileName_temp), sqlCnt);
                sqlCnt.Close();

                connectionString += ";AttachDBFileName=" + fileName_temp;
                sqlCnt.ConnectionString = connectionString;
                sqlCnt.Open();
                cd.createDataTable(fileName_temp, "Image_test", "Image", "Image", sqlCnt);

                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlCnt);
                FileStream stream = new FileStream("D:\\Data\\image\\哔哩哔哩-2233.jpg", FileMode.Open);
                int len = (int)stream.Length;
                byte[] bytes = new byte[len];
                stream.Read(bytes, 0, len);
                sqlCmd.Parameters.Add("@i", SqlDbType.Image, len);
                sqlCmd.Parameters["@i"].Value = bytes;
                sqlCmd.ExecuteNonQuery();

                stream.Close();
            }
            catch (Exception ex) { }
            finally
            {
                sqlCnt.Close();
                sqlCnt.Dispose();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string fileName_temp = @"D:\Data\image\image.mdf";
            string connectionString = "Data Source=(local);Integrated Security=true";
            string connectionString_test = connectionString + ";AttachDBFileName=";
            connectionString_test += fileName_temp;
            SqlConnection sqlCnt_test = new SqlConnection(connectionString_test);

            fileName_temp = @"D:\Data\sqlserver\copy\20201030.mdf";
            connectionString += ";AttachDBFileName=" + fileName_temp;
            try
            {
                sqlCnt_test.Open();
                SqlCommand sqlCmd_test = sqlCnt_test.CreateCommand();
                sqlCmd_test.CommandText = "insert into Image_test(Image) values(@i)";

                SqlConnection sqlCnt = new SqlConnection(connectionString);
                sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                //sqlCmd.CommandText = "SELECT * FROM Image_test";
                sqlCmd.CommandText = "SELECT * FROM HD_STREETVIEW_FACADEINFO";
                SqlDataReader sdr = sqlCmd.ExecuteReader();
                while (sdr.Read())
                {
                    //byte[] myData = (byte[])sdr["Image"];
                    byte[] myData = (byte[])sdr["Points"];
                    MemoryStream stream = new MemoryStream(myData);
                    string temp = stream.ToString();
                    //显示在pictureBox中
                    //System.Drawing.Image image = System.Drawing.Image.FromStream(stream, true);
                    //pictureBox1.Image = image;

                    int len = (int)stream.Length;
                    byte[] bytes = new byte[len];
                    stream.Read(bytes, 0, len);
                    sqlCmd_test.Parameters.Add("@i", SqlDbType.Image, len);
                    sqlCmd_test.Parameters["@i"].Value = bytes;
                    sqlCmd_test.ExecuteNonQuery();
                    stream.Close();
                }
            }
            catch (Exception ex) { }
            finally
            {
                sqlCnt_test.Close();
                sqlCnt_test.Dispose();
                sqlCnt.Close();
                sqlCnt.Dispose();
            }
        }
    }
}
