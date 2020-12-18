using System;
using System.IO;
using System.Windows.Forms;

namespace fileOperator
{
    class fileInfo
    {
        /// <summary>
        /// 判断文件是否被占用
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsFileInUsing(string fileName)
        {
            bool isUsed = true;

            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                isUsed = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            return isUsed;//true表示正在使用,false没有使用
        }
    }
}
