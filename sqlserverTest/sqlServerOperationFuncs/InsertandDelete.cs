using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace sqlServerOperationFuncs
{
    /*
     * InsertandDelete类主要实现功能
     * 1.添加字段
     * 2.按行插入数据
     * 以上 2020-12-14
     * 3.
     */
    class InsertandDelete
    {
        /// <summary>
        /// 指定数据表添加列
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnNames">列名</param>
        /// <param name="typeofColumn">列类型 例如：INT、varchar(30)</param>
        /// <param name="sqlCnt"></param>
        public void addColumns(string tableName, string columnsName, string typeofColumn, SqlConnection sqlCnt)
        {
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();

                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("ALTER TABLE {0} ADD {1} {2}", tableName, columnsName, typeofColumn);
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
        /// 按行插入数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnsName">字段名</param>
        /// <param name="values">值-字符串数组</param>
        /// <param name="sqlCnt"></param>
        /// <param name="isAlwaysUsing0InsteadNull">True表示默认使用0值代替空值（缺少值）</param>
        public void insertValues(string tableName, string[] columnsName, string[] values, SqlConnection sqlCnt, bool isAlwaysUsing0InsteadNull)
        {
            string columnNames = "";
            string valueStr = "";
            // 获取所有字段组合成一个字符串
            foreach (string str in columnsName)
            {
                columnNames += str + ",";
            }
            columnNames = columnNames.Substring(0, columnNames.Length - 1);
            if (columnsName.Length != values.Length)
            {
                MessageBox.Show("输入字段数量与字段值数量不相符，请确认", "Error");
                if (isAlwaysUsing0InsteadNull || MessageBox.Show("是否采用0值补齐字段值数量", "Error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // 获取所有字段值字符串
                    for (int i = 0; i < columnsName.Length; i++)
                    {
                        if (i < values.Length)
                            valueStr += values[i] + ",";
                        else
                        {
                            valueStr += "0,";
                        }
                    }
                    valueStr = valueStr.Substring(0, valueStr.Length - 1);
                }
                else
                {
                    MessageBox.Show("请确认输入字段数和字段值数量是否匹配后再次尝试", "Error");
                    return;
                }
            }
            else
            {
                // 获取所有字段值字符串
                foreach (string str in values)
                {
                    valueStr += str + ",";
                }
                valueStr = valueStr.Substring(0, valueStr.Length - 1);
            }
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, columnNames, valueStr);
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
       /// 删除指定字段的值为value的所有值(待补充)
       /// </summary>
       /// <param name="tableName"></param>
       /// <param name="value"></param>
       /// <param name="sqlCnt"></param>
        public void detValues(string tableName,string value, SqlConnection sqlCnt)
        {
            // 待补充
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                sqlCnt.Close();
            }
        }
    }
}
