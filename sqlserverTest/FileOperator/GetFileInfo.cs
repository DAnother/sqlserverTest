using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
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

        /// <summary>
        /// 新建文件夹 返回文件夹路径
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static string newFolder(string folderPath, string folderName)
        {
            string subFolder = folderPath + "\\" + folderName;
            try
            {
                if (Directory.Exists(folderPath))
                {
                    if (Directory.Exists(subFolder))
                    {
                        if (Directory.GetFileSystemEntries(subFolder).Length > 0)
                            MessageBox.Show("文件夹已存在且非空", "Error");
                    }
                    else
                    {
                        Directory.CreateDirectory(subFolder);
                    }
                }
            }
            catch (Exception)
            {
                return "Error";
            }
            return subFolder;
        }

        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static int copyFile(string filePath, string targetPath)
        {
            int result = -1;
            bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
            try
            {
                if (File.Exists(targetPath))
                {
                    MessageBox.Show("文件已存在", "Error");
                    result = 0;
                }
                else
                {
                    File.Copy(filePath, targetPath, isrewrite);
                    result = 1;
                }
            }
            catch(Exception)
            {
                return result;
            }

            return result;
        }

        /// <summary>
        /// 获取目录权限列表
        /// </summary>
        /// <param name="path">目录的路径。</param>
        /// <returns>指示目录的权限列表</returns>
        public static List<FileSystemRights> GetDirectoryPermission(string path)
        {
            try
            {
                List<FileSystemRights> result = new List<FileSystemRights>();
                var dSecurity = Directory.GetAccessControl(new DirectoryInfo(path).FullName);
                foreach (FileSystemAccessRule rule in dSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                    result.Add(rule.FileSystemRights);

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
