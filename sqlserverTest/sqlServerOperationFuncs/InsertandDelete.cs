using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SqlServerOperationFuncs
{
    /*
     * InsertandDelete类主要实现功能
     * 1.添加字段
     * 2.按行插入数据
     * 3.删除字段
     * 以上 2020-12-14
     * 3.
     */
    class InsertandDelete
    {
        Read reader = new Read();
        /// <summary>
        /// 指定数据表添加列
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnNames">列名</param>
        /// <param name="typeofColumn">列类型 例如：INT、varchar(30)</param>
        /// <param name="sqlCnt"></param>
        public void addColumns(string dataBaseName, string tableName, string columnsName, string typeofColumn, SqlConnection sqlCnt)
        {
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();

                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("USE [{0}]", dataBaseName);
                sqlCmd.ExecuteNonQuery();
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
        public void insertValues(string dataBaseName, string tableName, string[] columnsName, string[] values, SqlConnection sqlCnt, bool isAlwaysUsing0InsteadNull)
        {
            int columnCount = reader.getColumns(dataBaseName, tableName, sqlCnt).Length;
            if (columnsName.Length > columnCount)
            {
                MessageBox.Show("输入字段数超出数据表包含字段数，请确认是否有误", "Error");
                return;
            }
            string columnNames = "";
            string valueStr = "";
            // 获取所有字段组合成一个字符串
            foreach (string str in columnsName)
            {
                columnNames += str + ",";
            }
            columnNames = columnNames.Substring(0, columnNames.Length - 1);
            if (columnsName.Length == values.Length)
            {
                // 获取所有字段值字符串
                foreach (string str in values)
                {
                    valueStr += str + ",";
                }
                valueStr = valueStr.Substring(0, valueStr.Length - 1);
            }
            else if (columnsName.Length > values.Length)
            {
                MessageBox.Show("输入字段数量与字段值数量不相符，请确认", "Error");
                if (isAlwaysUsing0InsteadNull || MessageBox.Show("是否采用NULL值补齐字段值数量", "Error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // 获取所有字段值字符串
                    for (int i = 0; i < columnsName.Length; i++)
                    {
                        if (i < values.Length)
                            valueStr += values[i] + ",";
                        else
                        {
                            valueStr += "NULL,";
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
                MessageBox.Show("输入的值数量大于表中的字段数，请确认");
                return;
            }
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("USE [{0}]", dataBaseName);
                sqlCmd.ExecuteNonQuery();
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
        /// 删除字段
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="tableName"></param>
        /// <param name="sqlCnt"></param>
        public void detColumn(string dataBaseName, string columnName, string tableName, SqlConnection sqlCnt)
        {      
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                int flag = isKeyExist(dataBaseName, columnName, tableName, sqlCnt) ? 1 : 0;
                if (flag == 0)
                {
                    MessageBox.Show("不存在该字段，无法删除", "Error");
                    return;
                }
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                SqlCommand sqlCmd = sqlCnt.CreateCommand();
                sqlCmd.CommandText =
                    String.Format("USE [{0}]", dataBaseName);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText =
                    String.Format("ALTER TABLE {0} DROP COLUMN {1}", tableName, columnName);
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                sqlCnt.Close();
            }
        }

        /// <summary>
        /// 查询键是否存在
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="tableName"></param>
        /// <param name="sqlCnt"></param>
        /// <returns></returns>
        public bool isKeyExist(string dataBaseName, string columnName, string tableName, SqlConnection sqlCnt)
        {
            bool flag = false;
            try
            {
                if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                string[] columnsName = reader.getColumns(dataBaseName, tableName, sqlCnt);
                foreach (string str in columnsName)
                {
                    if (str == columnName)
                        flag = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                sqlCnt.Close();
            }
            return flag;
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
