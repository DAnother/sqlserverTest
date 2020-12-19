using System;
using System.ServiceProcess;
using System.Windows.Forms;

namespace SqlServerOperationFuncs
{
    class ServerInfo
    {
        /// <summary>
        /// 判断MSSQL服务是否打开
        /// </summary>
        /// <returns></returns>
        public bool isMSSQLServerOpened()
        {
            ServiceController sc = new ServiceController("MSSQLSERVER");

            //判断服务是否已经关闭
            if (sc.Status == ServiceControllerStatus.Stopped)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 打开MSSQL服务
        /// </summary>
        /// <returns></returns>
        public bool StartMSSQLServer()
        {
            bool flag = false;
            ServiceController sc = new ServiceController("MSSQLSERVER");
            if (isMSSQLServerOpened())
                flag = true;
            else
                try
                {
                    sc.Start();
                    flag = true;
                }
                catch (Exception ex)
                {
                    flag = false;
                    MessageBox.Show(ex.ToString(), "Error");
                }

            return flag;
        }
        
        /// <summary>
        /// 关闭MSSQL服务
        /// </summary>
        /// <returns></returns>
        public bool StopMSSQLServer()
        {
            bool flag = false;
            ServiceController sc = new ServiceController("MSSQLSERVER");
            if (isMSSQLServerOpened())
            {
                try
                {
                    sc.Stop();
                    flag = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error");
                }
            }
            else
                flag = true;

            return flag;
        }
    }
}

