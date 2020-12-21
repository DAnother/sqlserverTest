using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System;
using System.IO;
using System.Collections;
using FileOperator;

namespace SqlServerOperationFuncs
{
    /*
     * CreateandDrop类主要实现功能
     * 1.创建数据库
     * 2.创建数据表
     * 3.判断数据库是否存在
     * 4.判断数据表是否存在
     * 
     * 6.删除数据库
     * 7.删除数据表
     * 8.获取数据表名列表
     * 以上 2020-12-14
     */

    class CreateandDrop
    {
        private static bool isalwaysdetdatabase;
        private static bool isalwaysdetdatatable;
        public bool isAlwaysDetDataBase
        {
            set
            {
                isalwaysdetdatabase = value;
            }
            get
            {
                return isalwaysdetdatabase;
            }
        }
        public bool isAlwaysDetDataTable
        {
            set
            {
                isalwaysdetdatatable = value;
            }
            get
            {
                return isalwaysdetdatatable;
            }
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sqlCnt"></param>
        /// <param name="isAlwaysDetDataBase">是否始终删除数据库并重建</param>
        /// <returns></returns>
        public bool sqlCreate(string fileName, SqlConnection sqlCnt)
        {
            // 建立数据库连接
            if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();

            // 实例化一个SqlCommand对象，绑定连接
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlCnt;
            /*
             * 或者直接利用SQLConnection创建SQLCommand对象
             * SqlCommand SqlCmd = sqlCnt.CreateCommand();
             */
            string dataBaseName = Path.GetFileNameWithoutExtension(fileName);
            // 检查数据库是否存在 若存在则考虑是否删除
            int flag = isDataBaseExists(dataBaseName, sqlCnt);
            if (flag == 1)
            {
                MessageBox.Show("该数据库已存在，无法再次创建", "Warning!");
                if (isalwaysdetdatabase || MessageBox.Show("是否删除该数据库以及数据库分离文件并重新创建\r\n请谨慎删除！！！", "Warning!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    detDataBase(dataBaseName, sqlCnt);
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
            // 删除.mdf、日志文件
            flag = GetFileInfo.isFileExists(Path.GetDirectoryName(fileName), Path.GetFileName(fileName));
            if (flag > 0)
            {
                string path = Path.GetDirectoryName(fileName);
                string mdfFileName = path + "\\" + Path.GetFileName(fileName);
                string ldfFileName = path + "\\" + Path.GetFileNameWithoutExtension(fileName) + "_log.ldf";
                if (GetFileInfo.IsFileInUsing(mdfFileName))
                {
                    try
                    {
                        ServerInfo server = new ServerInfo();
                        if (server.isMSSQLServerOpened())
                            server.StopMSSQLServer();
                        File.Delete(mdfFileName);
                        File.Delete(ldfFileName);
                        server.StartMSSQLServer();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("文件被占用", "Error");
                        MessageBox.Show(ex.ToString(), "Error");
                        return false;
                    }
                }
                else
                {
                    File.Delete(mdfFileName);
                    File.Delete(ldfFileName);
                }
            }
            else if (flag < 0)
            {
                return false;
            }

            if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
            // 创建数据库
            sqlCmd.CommandText =
                String.Format("CREATE DATABASE [{0}] ON PRIMARY (NAME='mydatabase',FILENAME='{1}')", dataBaseName, fileName);
            sqlCmd.ExecuteNonQuery();

            // 关闭连接
            sqlCnt.Close();
            // 释放连接对象 采用全局连接对象 在此处只关闭但不释放sqlCnt对象
            // sqlCnt.Dispose();

            return true;
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="dataTableName"></param>
        /// <param name="sqlCnt">连接选项</param>
        /// <param name="isAlwaysDetDataTable">是否始终删除</param>
        public void createDataTable(string dataBaseName, string dataTableName, string primaryKey, string keyType, SqlConnection sqlCnt)
        {
            int flag = isDataTableExists(dataTableName, sqlCnt);
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                // 首先判断数据表是否存在
                if (flag == 1)
                {
                    if (isalwaysdetdatatable || MessageBox.Show("该数据表已存在，是否删除并重新创建", "Error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        detDataTable(dataTableName, sqlCnt);
                        sqlCnt.Open();
                    }
                    else
                    {
                        MessageBox.Show("无法成功创建数据表，原因：该数据表已存在", "Error");
                        return;
                    }
                }
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("USE [{0}]", dataBaseName);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText =
                    String.Format("CREATE TABLE [{0}]({1} {2})", dataTableName, primaryKey, keyType);
                sqlCmd.ExecuteNonQuery();
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
            sqlCreateDBQuery = string.Format("SELECT * from sys.databases WHERE Name  = '{0}'", dataBaseName);

            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, sqlCnt);
                object resultObj = sqlCmd.ExecuteScalar();
                if (resultObj != null)
                    result = 1;
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
                    String.Format("DROP TABLE [{0}]", dataTableName), sqlCnt);
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
                    String.Format("DROP DataBase [{0}]", dataBaseName);
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
        public string[] getDataTablesName(SqlConnection sqlCnt)
        {
            ArrayList tables = new ArrayList();
            if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
            try
            {
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                // 可用于列举数据表 便于调试
                // 使用信息架构视图
                sqlCmd.CommandText =
                    String.Format("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'");
                SqlDataReader dr = sqlCmd.ExecuteReader();
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
        /// 分离数据库 便于拷贝数据文件
        /// </summary>
        /// <param name="dataBaseName"></param>
        /// <param name="sqlCnt"></param>
        /// <returns></returns>
        public bool detachDataBase(string dataBaseName,SqlConnection sqlCnt)
        {
            bool flag = false;
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("ALTER DATABASE [{0}] SET OFFLINE WITH ROLLBACK IMMEDIATE", dataBaseName);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText =
                    String.Format("EXEC sp_detach_db '{0}', 'true'", dataBaseName);
                sqlCmd.ExecuteNonQuery();
                flag = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
                return flag;
            }

            return flag;
        }
    }
}
