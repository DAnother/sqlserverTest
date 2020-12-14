using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace sqlserverTest.sqlServerOperationFuncs
{
    class InsertandDelete
    {
        /// <summary>
        /// 指定字段插入数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnsName">字段名</param>
        /// <param name="values">值-字符串数组</param>
        /// <param name="sqlCnt"></param>
        public void insertValues(string tableName, string columnsName, string[] values, SqlConnection sqlCnt)
        {
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                for (int i = 0; i < values.Length; i++)
                {
                    sqlCmd.CommandText =
                        String.Format("INSERT INTO {0}({1}) VALUES({2})", tableName, columnsName, values[i]);
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
