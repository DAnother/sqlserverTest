using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace SqlServerOperationFuncs
{
    /*
     * Read
     * 1.获取字段名
     * 2.查询一个数据表的所有数据
     * 3.读取某列的所有数据
     * 以上 2020-12-14
     */

    class Read
    {
        //TODO: Another-当字段类型为Image时，不能直接读取
        /// <summary>
        /// 查询数据表的列名
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sqlCnt"></param>
        /// <returns></returns>
        public string[] getColumns(string dataBaseName, string tableName, SqlConnection sqlCnt)
        {
            ArrayList array = new ArrayList();
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("USE [{0}]", dataBaseName);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText =
                    String.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}'", tableName);
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
        /// 获取字段的类型
        /// </summary>
        /// <param name="dataBaseName"></param>
        /// <param name="tableName"></param>
        /// <param name="sqlCnt"></param>
        /// <returns></returns>
        public string[] getTypeOfColumns(string dataBaseName, string tableName, SqlConnection sqlCnt)
        {
            ArrayList array = new ArrayList();
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("USE [{0}]", dataBaseName);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText =
                    String.Format("SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='{0}'", tableName);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    array.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                sqlCnt.Close();
            }

            return (string[])array.ToArray(typeof(string));
        }

        /// <summary>
        /// 返回一个表的所有值
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sqlCnt"></param>
        /// <returns></returns>
        public string[] getAllValues(string dataBaseName, string tableName, SqlConnection sqlCnt)
        {
            ArrayList array = new ArrayList();
            array.Add(tableName);
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                string[] columnsName = getColumns(dataBaseName, tableName, sqlCnt);
                string[] tempStrArray = new string[columnsName.Length];
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("USE [{0}]", dataBaseName);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText =
                    String.Format("SELECT * FROM {0}", tableName);
                SqlDataReader sdr = sqlCmd.ExecuteReader();
                while (sdr.Read())
                {
                    for (int i = 0; i < columnsName.Length; i++)
                    {
                        string tempStr = (sdr.GetValue(sdr.GetOrdinal(columnsName[i]))).ToString();
                        tempStrArray[i] = (columnsName[i] + ":" + tempStr + "\t");
                    }
                    string dataRow = "";
                    foreach (string str in tempStrArray)
                    {
                        dataRow += str;
                    }
                    array.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                sqlCnt.Close();
            }
            return (string[])array.ToArray(typeof(string));
        }

        /// <summary>
        /// 读取某列的所有数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="sqlCnt"></param>
        /// <returns></returns>
        public string[] getColumnValues(string dataBaseName, string tableName, string columnName,string columnType, SqlConnection sqlCnt, out MemoryStream[] mStream)
        {
            mStream = null;
            ArrayList array = new ArrayList();
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                if (columnType.Contains("Image"))
                {
                    readImageData(dataBaseName, tableName, columnName, sqlCnt, out mStream);
                }
                else
                {
                    SqlCommand sqlCmd = sqlCnt.CreateCommand();
                    sqlCmd.CommandText =
                        String.Format("USE [{0}]", dataBaseName);
                    sqlCmd.ExecuteNonQuery();
                    sqlCmd.CommandText =
                        String.Format("SELECT {0} FROM {1}", columnName, tableName);
                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        array.Add(reader[0].ToString());
                    }
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
        /// 读取Image格式的字段
        /// </summary>
        /// <param name="dataBaseName">数据库名</param>
        /// <param name="tableName">表名</param>
        /// <param name="keyName">Image格式的键</param>
        /// <param name="sqlCnt">数据库链接</param>
        /// <returns></returns>
        public bool readImageData(string dataBaseName, string tableName, string keyName, SqlConnection sqlCnt, out MemoryStream[] mStream)
        {
            bool flag = false;
            ArrayList array = new ArrayList();
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("USE [{0}]", dataBaseName);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText =
                    String.Format("SELECT * FROM {0}", tableName);
                SqlDataReader sdr = sqlCmd.ExecuteReader();
                while (sdr.Read())
                {
                    byte[] myData = (byte[])sdr[keyName];//读取第一个图片的位流
                    MemoryStream stream = new MemoryStream(myData);
                    array.Add(stream);
                    stream.Close();
                    stream.Dispose();
                }
                flag = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                sqlCnt.Close();
            }
            mStream = (MemoryStream[])array.ToArray(typeof(MemoryStream));
            return flag;
        }

    }
}
