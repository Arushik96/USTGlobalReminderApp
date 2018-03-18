using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
namespace Reminder
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string connectionString = "SERVER=  localhost;" + "DATABASE=arushi;" + "UID=root;" + "PASSWORD=;"; //Set your MySQL connection string here.
            string query = "select * from reminders;"; // set query to fetch data "Select * from  tabelname"; 

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DateTime time = new DateTime();
            string message = "";
            List<string> reminderTime = new List<string>();
            string rTime = "";
            int flag = 0;
            Program p = new Program();
            
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                {
                    while (true)
                    {
                        try
                        {
                            ds = new DataSet();
                            adapter.Fill(ds);

                            dt = ds.Tables[0];

                            foreach (DataRow dr in dt.Rows)
                            {
                                time = Convert.ToDateTime(dr[2].ToString());
                                if (time.Date.Equals(DateTime.Now.Date) || dr[5].ToString().Equals("yes"))
                                {
                                    if (!reminderTime.Contains(dr[2].ToString()))
                                    {
                                        reminderTime.Add(dr[2].ToString() + "*" + dr[1].ToString());
                                    }
                                }

                            }
                        }catch(Exception ex)
                        {

                        }
                        foreach(string s in reminderTime)
                        {
                            try
                            {

                                message = s.Split('*')[1];
                                rTime = s.Split('*')[0];
                                if (Convert.ToDateTime(rTime).Hour.Equals(DateTime.Now.Hour) && Convert.ToDateTime(rTime).Minute.Equals(DateTime.Now.Minute))
                                {
                                    if (flag == 0)
                                    {
                                        p.showBalloon("ReminderApp", message);
                                 
                                        flag = 1;
                                    }

                                }
                                else
                                {
                                    flag = 0;
                                }
                            }
                            catch(Exception ex)
                            {

                            }
                        }
                    }
                }
            }
        }
        private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        public void showBalloon(string title, string body)
        {
            notifyIcon.Visible = true;
            notifyIcon.Icon = System.Drawing.SystemIcons.Information;
            if (title != null)
            {
                notifyIcon.BalloonTipTitle = title;
            }

            if (body != null)
            {
                notifyIcon.BalloonTipText = body;
            }

            notifyIcon.ShowBalloonTip(30000);//30 seconds
            Thread.Sleep(600000);
            notifyIcon.Dispose();
        }
           
        
           
        
        
    }
}
