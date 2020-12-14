using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System;
using System.IO;
using System.Collections;

namespace sqlserverTest
{
    class CreateandDrop
    {
        /// <summary>
        /// 创建数据库、数据表
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool sqlCreate(string fileName, SqlConnection sqlCnt)
        {
            // 建立数据库连接
            sqlCnt.Open();

            // 实例化一个SqlCommand对象，绑定连接
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlCnt;
            /*
             * 或者直接利用SQLConnection创建SQLCommand对象
             * SqlCommand SqlCmd = sqlCnt.CreateCommand();
             */
            string databaseName = Path.GetFileNameWithoutExtension(fileName);
            // 检查数据库是否存在 若存在则考虑是否删除
            int flag = (isDataBaseExists(databaseName, sqlCnt) + isFileExists(Path.GetDirectoryName(fileName), Path.GetFileName(fileName))) > 0 ? 1 : 0;
            if (flag == 1)
            {
                MessageBox.Show("该数据库已存在，无法再次创建", "Warning!");
                if (MessageBox.Show("是否删除该数据库以及数据库分离文件并重新创建\r\n请谨慎删除！！！", "Warning!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (isDataBaseExists(databaseName, sqlCnt) > 0)
                        detDataBase(databaseName, sqlCnt);
                    // 删除.mdf、日志文件
                    if (isFileExists(Path.GetDirectoryName(fileName), Path.GetFileName(fileName)) > 0)
                    {
                        string path = Path.GetDirectoryName(fileName);
                        string mdfFileName = path + "\\" + Path.GetFileName(fileName);
                        string ldfFileName = path + "\\" + Path.GetFileNameWithoutExtension(fileName) + "_log.ldf";
                        File.Delete(mdfFileName);
                        File.Delete(ldfFileName);
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (flag == -1)
            {
                return false;
            }
            if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
            // 创建数据库
            sqlCmd.CommandText =
                String.Format("CREATE DATABASE {0} ON PRIMARY(NAME={0}, FILENAME='{1}')", databaseName, fileName);
            sqlCmd.ExecuteNonQuery();
            // 分离数据库 便于拷贝数据文件
            sqlCmd.CommandText =
                String.Format("EXEC sp_detach_db '{0}', 'true'", databaseName);
            sqlCmd.ExecuteNonQuery();

            // 关闭连接
            sqlCnt.Close();
            // 释放连接对象 采用全局连接对象 在此处只关闭但不释放sqlCnt对象
            // sqlCnt.Dispose();

            return true;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="fileName"></param>
        public void sqlWrite(string fileName, SqlConnection sqlCnt)
        {
            sqlCnt.Open();
            SqlCommand command = sqlCnt.CreateCommand();
            command.CommandText = "test";

            sqlCnt.Close();
            sqlCnt.Dispose();
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="fileName"></param>
        public void sqlRead(string fileName)
        {
            string connectionDataFile =
                String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={0};Integrated Security=True", fileName);

            SqlConnection connection = new SqlConnection(connectionDataFile);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM HD_STREETVIEW_IMAGEINFO";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Form1.form1.richTextBox1.Text = reader.GetString(1);
            }
            connection.Close();
            connection.Dispose();
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="sqlCnt">连接选项</param>
        public void createDataTable(string dataTableName, SqlConnection sqlCnt)
        {
            int flag = 0;
            flag = isDataTableExists(dataTableName, sqlCnt);
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                // 首先判断数据表是否存在
                if (flag == 1)
                {
                    if (MessageBox.Show("该数据表已存在，是否删除并重新创建", "Error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        detDataTable(dataTableName, sqlCnt);
                    else
                    {
                        MessageBox.Show("无法成功创建数据表，原因：该数据表已存在", "Error");
                        return;
                    }
                }
                SqlCommand command2 =
                    new SqlCommand("CREATE TABLE " + dataTableName + "(id int IDENTITY(1, 1) PRIMARY KEY not null)", sqlCnt);
                command2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                sqlCnt.Close();
                // 连接资源 保持开启 以便后续操作 直至完成 采用全局连接对象
                // sqlCnt.Dispose();
            }
        }

        /// <summary>
        /// 判断数据库是否存在
        /// </summary>
        /// <param name="databaseName">参与判断的数据库</param>
        /// <returns></returns>
        public int isDataBaseExists(string dataBaseName, SqlConnection sqlCnt)
        {
            int result = 0;
            string sqlCreateDBQuery;
            sqlCreateDBQuery = string.Format("SELECT database_id from sys.databases WHERE Name  = '{0}'", dataBaseName);

            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, sqlCnt);
                object resultObj = sqlCmd.ExecuteScalar();
                int databaseID = 0;
                if (resultObj != null)
                {
                    int.TryParse(resultObj.ToString(), out databaseID);
                }
                result = (databaseID > 0) ? 1 : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "error");
                result = -1;
            }
            finally
            {
                sqlCnt.Close();
                // sqlCnt.Dispose();
            }
            return result;
        }

        /// <summary>
        /// 判断数据表是否存在
        /// </summary>
        /// <param name="dataTableName"></param>
        /// <returns></returns>
        public int isDataTableExists(string dataTableName, SqlConnection sqlCnt)
        {
            int flag = 0;
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                //sqlCmd.CommandText = String.Format("SELECT object_id FROM {0} WHERE NAME = '{1}'", dataBaseName, dataTableName);
                DataTable dt = sqlCnt.GetSchema("Tables");
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[2].ToString() == dataTableName)
                        flag = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                sqlCnt.Close();
                // sqlCnt.Dispose();
            }
            return flag;
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public int isFileExists(string filePath, string fileName)
        {
            int flag = 0;
            try
            {
                flag = File.Exists(filePath + "\\" + fileName) ? 1 : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            return flag;
        }

        /// <summary>
        /// 删除数据表
        /// </summary>
        /// <param name="dataBaseName">数据表名</param>
        /// <param name="sqlCnt">连接</param>
        public void detDataTable(string dataTableName, SqlConnection sqlCnt)
        {
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = new SqlCommand(
                    String.Format("DROP TABLE {0}", dataTableName), sqlCnt);
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                sqlCnt.Close();
                // sqlCnt.Dispose();
            }
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="dataBaseName"></param>
        /// <param name="sqlCnt"></param>
        public void detDataBase(string dataBaseName, SqlConnection sqlCnt)
        {
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("DROP DataBase {0}", dataBaseName);
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                sqlCnt.Close();
                // sqlCnt.Dispose();
            }
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="dataBaseName"></param>
        /// <param name="sqlCnt"></param>
        /// <returns></returns>
        public string[] getDataTablesName(string dataBaseName, SqlConnection sqlCnt)
        {
            ArrayList tables = new ArrayList();
            if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
            try
            {
                // 可用于列举数据表 便于调试
                // 使用信息架构视图
                SqlCommand sqlcmd = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'", sqlCnt);
                SqlDataReader dr = sqlcmd.ExecuteReader();
                while (dr.Read())
                {
                    tables.Add(dr.GetString(0));
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                sqlCnt.Close();
                // sqlCnt.Dispose();
            }
            return (string[])tables.ToArray(typeof(string));
        }

        /// <summary>
        /// 查询数据表的列名
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sqlCnt"></param>
        /// <returns></returns>
        public string[] getColumns(string tableName, SqlConnection sqlCnt)
        {
            ArrayList array = new ArrayList();
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_NAME = '{0}'", tableName);
                SqlDataReader dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    array.Add(dr.GetString(0));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                sqlCnt.Close();
                // sqlCnt.Dispose();
            }
            return (string[])array.ToArray(typeof(string));
        }

        /// <summary>
        /// 指定数据表添加列
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnNames">列名</param>
        /// <param name="typeofColumn">列类型 例如：INT、varchar(30)</param>
        /// <param name="sqlCnt"></param>
        public void addColumns(string tableName, string[] columnsName, string[] typeofColumn, SqlConnection sqlCnt)
        {
            if (columnsName.Length != typeofColumn.Length)
            {
                MessageBox.Show("请输入相匹配数量的列名与类型");
                return;
            }
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                for (int i = 0; i < columnsName.Length; i++)
                {
                    SqlCommand sqlCmd = sqlCnt.CreateCommand();
                    sqlCmd.CommandText =
                        String.Format("ALTER TABLE {0} ADD {1} {2}", tableName, columnsName[i], typeofColumn[i]);
                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                sqlCnt.Close();
                // sqlCnt.Dispose();
            }
        }
    }
}
