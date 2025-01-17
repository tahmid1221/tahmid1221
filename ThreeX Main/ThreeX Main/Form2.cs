using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeyAuth;
using static Guna.UI2.Native.WinApi;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Diagnostics;

namespace ThreeX_Main
{
    public partial class Form2 : Form
    {
        public static api KeyAuthApp = new api(
    name: "Zxo", // Application Name
    ownerid: "IbwptaAtFV", // Owner ID
    secret: "ab0d1a46c8e39cefebf4874c332494305633b5eb7ba0186ac83dfffbbd80077f", // Application Secret
    version: "9.0" // Application Version /*
                   //path: @"Your_Path_Here" // (OPTIONAL) see tutorial here https://www.youtube.com/watch?v=I9rxt821gMk&t=1s
);


        public Form2()
        {
            InitializeComponent();
            this.Size = new Size(424, 522);
            error.Visible = false;

            ///////////////////////////////////////////////////
            // Username Password Save code Down
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Username))
            {
                user.Text = Properties.Settings.Default.Username;
                pass.Text = Properties.Settings.Default.Password;
                chkRemember.Checked = true;
            }
            ///////////////////////////////////////////////////

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            KeyAuthApp.init();
            KeyAuthApp.check();



        }

        public static bool SubExist(string name)
        {
            if (KeyAuthApp.user_data.subscriptions.Exists(x => x.subscription == name))
                return true;
            return false;
        }

        static string random_string()
        {
            string str = null;

            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                str += Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))).ToString();
            }
            return str;

        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/@3XCorporationBD");
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/WHhZ7WXetK");
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            KeyAuthApp.login(user.Text, pass.Text);


            if (KeyAuthApp.response.success)
            {

                if (SubExist("default"))
                {

                    Form1 Main = new Form1();
                    this.Hide();
                    Main.Show();





                    //////////////////////////////////
                    //Username password Save Code down

                    string username = user.Text;
                    string password = pass.Text;

                    // Check if "Remember Me" is checked
                    if (chkRemember.Checked)
                    {
                        // Save the username and password in application settings
                        Properties.Settings.Default.Username = username;
                        Properties.Settings.Default.Password = password;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        // Clear the saved username and password if "Remember Me" is unchecked
                        Properties.Settings.Default.Username = string.Empty;
                        Properties.Settings.Default.Password = string.Empty;
                        Properties.Settings.Default.Save();
                    }
                    ///////////////////////////////////////////////////
                }
            }
            else
            {


                error.Visible = true;
                error.Location = new Point(20, 437);
                errormsg.Text = KeyAuthApp.response.message;

            }
        }
         private void timer1_Tick(object sender, EventArgs e)
        {


        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }
    }
}


