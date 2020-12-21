using System;
using System.IO;
using System.Windows.Forms;

namespace FileOperator
{
    class GetFileInfo
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
            catch (Exception)
            {

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

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static int isFileExists(string filePath, string fileName)
        {
            int flag = 0;
            try
            {
                flag = File.Exists(filePath + "\\" + fileName) ? 1 : 0;
            }
            catch (Exception)
            {
                flag = -1;
            }
            return flag;
        }
    }
}
