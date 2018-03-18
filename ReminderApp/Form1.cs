using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ReminderApp
{
    public partial class Form1 : Form
    {
        int ID = 0;
        string connectionString = "SERVER=localhost;" + "DATABASE=arushi;" + "UID=root;" + "PASSWORD=;"; // Connection String to MySQL database
        string path = @"...Reminder.exe"; //Path to Executable of Background Application
        public Form1()
        {
            InitializeComponent();
            Process[] pname = Process.GetProcessesByName("Reminder");// Checks the name of the process and starts it if not already being executed
           if (pname.Length == 0)
           {
                Process.Start(path);
           }
            
            
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("ReminderApp", "\"" + path + "\""); // Sets the process in Start Up Directory of Windows 
            }
            groupBox2.Hide(); // Create New Reminder UI
            groupBox3.Hide(); // Update Delete UI
            dateTimePicker1.Format = DateTimePickerFormat.Time;
            dateTimePicker2.Format = DateTimePickerFormat.Time;
            dataGridView1.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView1_RowHeaderMouseClick);
            getGrid();
        }
        public void getGrid()
        {
            
            string query = "select * from reminders;"; // set query to fetch data "Select * from  tabelname"; 
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                {
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    
                    dataGridView1.DataSource = ds.Tables[0];
                    if(ds.Tables[0].Rows.Count==0)
                    {
                    //    foreach (var process in Process.GetProcessesByName("Reminder"))
                      //  {
                        //    process.Kill();
                        //}
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {  
           
            groupBox1.Show();

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox2.Hide();
        }

        private void button3_Click(object sender, EventArgs e) // Set Reminder
        {
            string time = dateTimePicker1.Value.ToString();
            string status = "active";
            string message = textBox1.Text.ToString();
            string snooze = "no";
            string repeat = "no";
            
            if(checkBox2.Checked)
            {
                repeat = "yes";
            }
            try
            {
            
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "INSERT INTO reminders(message,time,status,snooze,repeatDaily) VALUES('" + message + "','" + time + "','" + status + "','" + snooze + "','" + repeat + "')";
                comm.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Reminder Set Succesfully");
                groupBox2.Hide();
                getGrid();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Something went Wrong! Please try again. ");
                groupBox2.Hide();
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            dateTimePicker2.Value = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
            
            if (dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString() == "yes")
            {
                checkBox3.Checked = true;
            }
            else
            {
                checkBox3.Checked = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            getGrid();
            groupBox3.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            groupBox3.Hide();
        }
        
        private void button7_Click(object sender, EventArgs e)
        {
            string time = dateTimePicker2.Value.ToString();
            string status = "active";
            string message = textBox2.Text.ToString();
            string snooze = "no";
            string repeat = "no";
            
            if (checkBox3.Checked)
            {
                repeat = "yes";
            }
            try
            {
            
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "UPDATE reminders SET message='" + message + "',time='" + time + "',status='" + status + "',snooze='" + snooze + "',repeatDaily='" + repeat + "' where id="+ID+";";
                comm.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Reminder Updated Succesfully");
                groupBox3.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went Wrong! Please try again. ");
                
            }
        }
        private void ClearData()
        {

            ID = 0;
        }
        private void button6_Click(object sender, EventArgs e)
        {

            
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand comm = conn.CreateCommand();
            if (ID != 0)
            {
                comm.CommandText = "DELETE from reminders where id=" + ID + ";";
                comm.ExecuteNonQuery();
                MessageBox.Show("Deleted Succesfully");
            }
            else
            {
                MessageBox.Show("Please select a row to delete Reminder");
            }
            conn.Close();
            getGrid();

            ClearData();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand comm = conn.CreateCommand();
             
                comm.CommandText = "Truncate reminders;";
                comm.ExecuteNonQuery();
            
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue("ReminderApp");
            }
            MessageBox.Show("Deleted Succesfully");
            
            
            conn.Close();
            getGrid();

            ClearData();
        }
    }
}
