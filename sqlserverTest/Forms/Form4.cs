using FileOperator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace sqlserverTest
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "当前文件夹的当前账户权限：\r\n";
            string dirPath = textBox1.Text;
            string fileName = textBox2.Text;
            List<FileSystemRights> rights = GetFileInfo.GetDirectoryPermission(fileName);
            //获取文件信息
            FileInfo fileInfo = new FileInfo(fileName);
            //获得该文件的访问权限
            FileSecurity fileSecurity = fileInfo.GetAccessControl();
            foreach (FileSystemRights right in rights)
            {
                richTextBox1.Text += right+"\r\n";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<string> files = new List<string>();
            string fileName = @"D:\Data\sqlserver\20201030.mdf";
            files.Add(fileName);
            fileName = @"D:\Data\sqlserver\20201030_log.ldf";
            files.Add(fileName);
            foreach (string file in files)
            {
                string currentFolder = Path.GetDirectoryName(file);
                string copyFileName = GetFileInfo.newFolder(currentFolder, "copy") + "\\" + Path.GetFileName(file);
                int isFileExist = GetFileInfo.copyFile(file, copyFileName);
                if (isFileExist == 1)
                {
                    richTextBox1.Text += "\r\n拷贝文件成功\r\n文件路径为：";
                    richTextBox1.Text += copyFileName;
                }
                else if (isFileExist == 0)
                {
                    richTextBox1.Text += "文件已存在，拷贝失败！";
                }
                else
                {
                    richTextBox1.Text += "拷贝文件失败！";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
