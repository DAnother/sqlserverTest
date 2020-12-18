using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Collections;

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

        // 获取字段的类型
        public string[] getTypeOfColumns(string tableName, SqlConnection sqlCnt)
        {
            ArrayList array = new ArrayList();
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
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
        public string[] getAllValues(string tableName, SqlConnection sqlCnt)
        {
            ArrayList array = new ArrayList();
            array.Add(tableName);
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                string[] columnsName = getColumns(tableName, sqlCnt);
                string[] tempStrArray = new string[columnsName.Length];
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("SELECT * FROM {0}", tableName);
                SqlDataReader sdr = sqlCmd.ExecuteReader();
                while (sdr.Read())
                {
                    for(int i = 0; i < columnsName.Length; i++)
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
        public string[] getColumnValues(string tableName, string columnName, SqlConnection sqlCnt)
        {
            ArrayList array = new ArrayList();
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();

                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText = 
                    String.Format("SELECT {1} FROM {0}", tableName,columnName);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    array.Add(reader[0].ToString());
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
    }
}
