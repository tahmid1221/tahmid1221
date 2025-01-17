using FastMemory;
using fastxMem;
using Guna.UI2.WinForms;
using KeyAuth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TestAcrylicBlur.EffectBlur;

namespace ThreeX_Main
{
    public partial class Form1 : Form
    {
        private readonly string pastebinUrl = "https://pastebin.com/raw/i7VQ6umU"; // Replace with your Pastebin raw URL
        private readonly string appCode = "1111"; // Replace with your app's code

        private DateTime startTime;
        private Timer timer;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        //memory call
        Memoryfast memoryfast1 = new Memoryfast();
        fast memoryfast = new fast();
        /////////////////////////



        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
        private uint _blurOpacity;

        public double BlurOpacity
        {
            get { return _blurOpacity; }
            set { _blurOpacity = (uint)value; EnableBlur(); }
        }

        private uint _blurBackgroundColor = 0x990000;

        internal void EnableBlur()
        {

            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);
            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;
            SetWindowCompositionAttribute(this.Handle, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }



        public Form1()
        {
            InitializeComponent();
            server.Tick += CheckCode;


            this.Size = new Size(452, 522);
            AIM.Location = new Point(96, 25);
            this.Size = new Size(452, 522);
            AIM.Location = new Point(96, 25);
            mainpanel.Location = new Point(19, 90);
            error.Visible = false;
            pending.Visible = false;
            Time.Visible = false;
            done.Visible = true;
            done.Location = new Point(94, 429);
            donemsg.Text = "WELCOME TO 3X CORPORATION PREMIUME PANEL";
            ShowIPAddress();
            guna2Panel21.Visible = false;

         



            // Initialize the timer
            timer = new Timer();
            timer.Interval = 1000; // Timer will tick every 1 second
            timer.Tick += STOPTIMER_Tick;

            // Initialize the start time
            startTime = DateTime.Now;

            // Start the timer when the form is loaded
            timer.Start();




            // Subscribe to the same events for multiple panels
            foreach (Control control in this.Controls)
            {
                if (control is Panel)
                {


                    mainpanel.MouseDown += Panel_MouseDown;
                    mainpanel.MouseMove += Panel_MouseMove;
                    mainpanel.MouseUp += Panel_MouseUp;
                    // Repeat for other panels...

                    AIM.MouseDown += Panel_MouseDown;
                    AIM.MouseMove += Panel_MouseMove;
                    AIM.MouseUp += Panel_MouseUp;
                    // Repeat for other panels...

                    SNIPER.MouseDown += Panel_MouseDown;
                    SNIPER.MouseMove += Panel_MouseMove;
                    SNIPER.MouseUp += Panel_MouseUp;
                    // Repeat for other panels...

                    CHAMS.MouseDown += Panel_MouseDown;
                    CHAMS.MouseMove += Panel_MouseMove;
                    CHAMS.MouseUp += Panel_MouseUp;
                    // Repeat for other panels...

                    UPCOMING.MouseDown += Panel_MouseDown;
                    UPCOMING.MouseMove += Panel_MouseMove;
                    UPCOMING.MouseUp += Panel_MouseUp;
                    // Repeat for other panels...

                    INFO.MouseDown += Panel_MouseDown;
                    INFO.MouseMove += Panel_MouseMove;
                    INFO.MouseUp += Panel_MouseUp;
                    // Repeat for other panels...

                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////
        private async void CheckCode(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string pastebinCode = await client.GetStringAsync(pastebinUrl);

                    if (!appCode.Equals(pastebinCode.Trim()))
                    {
                        TerminateApplication();
                    }
                }
                catch
                {
                    TerminateApplication();
                }
            }
        }

        private void TerminateApplication()
        {
            KillProcess("HD-Player");
            Application.Exit();
        }

        private void KillProcess(string processName)
        {
            try
            {
                foreach (var process in Process.GetProcessesByName(processName))
                {
                    process.Kill();
                }
            }
            catch
            {
                // Ignore errors during process termination
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Stop();
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////




        private void ShowIPAddress()
        {
            try
            {
                // Get host entry for local machine
                string hostName = Dns.GetHostName();
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

                // Get the first available IPv4 address
                string ipAddress = hostEntry.AddressList
                                    .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?
                                    .ToString();

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    IP.Text = ipAddress;
                }
                else
                {
                    IP.Text = "No IPv4 Address Found.";
                }
            }
            catch (Exception ex)
            {
                IP.Text = "Error: " + ex.Message;
            }
        }



        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            // Start dragging when the left mouse button is pressed
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            }
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            // If dragging, update the form's location
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            // Stop dragging when the left mouse button is released
            if (e.Button == MouseButtons.Left)
            {
                dragging = false;
            }
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            server.Start();


            label27.Text = Form2.KeyAuthApp.user_data.username;

            label26.Text = $"{expirydaysleft()}";
        }

        public string expirydaysleft()
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            dtDateTime = dtDateTime.AddSeconds(long.Parse(Form2.KeyAuthApp.user_data.subscriptions[0].expiry)).ToLocalTime();
            TimeSpan difference = dtDateTime - DateTime.Now;
            return Convert.ToString(difference.Days + " Days " + difference.Hours + " Hours Left");
        }

        public DateTime UnixTimeToDateTime(long unixtime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            try
            {
                dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
            }
            catch
            {
                dtDateTime = DateTime.MaxValue;
            }
            return dtDateTime;
        }


        private void login_Click(object sender, EventArgs e)
        {



        }

        private void chkRemember_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void key_Click(object sender, EventArgs e)
        {

        }

        private void hidebutton_Tick(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void guna2CustomCheckBox1_Click(object sender, EventArgs e)
        {



            //if (guna2CustomCheckBox1.Checked)
            //{

            //    try
            //    {
            //        int proc = Process.GetProcessesByName("HD-Player")[0].Id;
            //        memoryfast1.OpenProcess(proc);
            //        error.Visible = false;
            //        done.Visible = false;
            //        pending.Visible = true;
            //        pending.Location = new Point(94, 429);
            //        pendingmsg.ForeColor = Color.Yellow;
            //        pendingmsg.Text = "AIMBOT NEAK APPLYING";

            //        IEnumerable<long> result = await memoryfast1.AoBScan(0x0000000000010000, 0x00007ffffffeffff, "FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43 00 00", true, true, string.Empty);

            //        if (result.Any())
            //        {
            //            foreach (var currentAddress in result)
            //            {
            //                string str = currentAddress.ToString("X");
            //                Byte[] valueBytes = memoryfast1.ReadMemoryx((currentAddress + 0x70).ToString("X"), 4);
            //                memoryfast1.WriteMemory((currentAddress + 0x6c).ToString("X"), "int", BitConverter.ToInt32(valueBytes, 0).ToString());
            //            }

            //            pending.Visible = false;
            //            done.Visible = true;
            //            done.Location = new Point(94, 429);
            //            donemsg.Text = done.Text = "AIMBOT NEAK SUCCESS";
            //            donemsg.ForeColor = Color.Lime;
            //            Time.Visible = true;
            //            Time.Text = DateTime.Now.ToString("hh:mm: tt");
            //            Time.ForeColor = Color.White;

            //        }
            //        else
            //        {
            //            pending.Visible = false;
            //            done.Visible = false;
            //            error.Visible = true;
            //            error.Location = new Point(94, 429);
            //            errormsg.ForeColor = Color.Red;
            //            errormsg.Text = "AIMBOT NEAK FAILED";
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        errormsg.Text = "Error: " + ex.Message;
            //    }
            //}
            //else
            //{
            //    done.Visible = true;
            //    error.Visible = false;
            //    pending.Visible = false;
            //    done.Location = new Point(94, 429);
            //    donemsg.Text = "BUTTON DISABLE";
            //}
        }

        private async void guna2CustomCheckBox2_Click(object sender, EventArgs e)
        {
            string[] pocessname = { "HD-Player" };
            bool sucess = memoryfast.SetProcess(pocessname);

            if (!sucess)
            {
                return;
            }
            else
            {
                Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
                error.Visible = false;
                done.Visible = false;
                pending.Visible = true;
                pending.Location = new Point(94, 429);
                pendingmsg.ForeColor = Color.Yellow;
                pendingmsg.Text = "Scope Activating.....";
                IEnumerable<long> result = await memoryfast.AoBScan("00 00 00 00 FF FF FF FF 1A 88 03 00 09 8A 03 00 FF FF FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 00 00 00 00 00 00 00 00 F0 41 00 00 48 42 00 00 00 3F 33 33 13 40 00 00 B0 3F 00 00 80 3F 01");
                if (result.Count() != 0 && result.Count() < 2)
                {
                    foreach (var CurrentAddress in result)
                    {
                        //JOTO NUMBER VALUE CHANGE HOBE TAR AGER NUMBER 46L
                        Int64 address10thByte = CurrentAddress + 46L; // 10th byte (index 9) //47

                        //JOTO NUMBER VALUE CHANGE HOBE TAR AGER NUMBER 47L
                        Int64 address11thByte = CurrentAddress + 47L; // 11th byte (index 10) //48


                        // Write the new value (FF) to both addresses
                        memoryfast.AobReplace(address10thByte, "F0"); // Replace 10th byte with 0xF0
                        memoryfast.AobReplace(address11thByte, "FF"); // Replace 11th byte with 0xFF

                        pending.Visible = false;
                        done.Visible = true;
                        done.Location = new Point(94, 429);
                        donemsg.Text = "SNIPER NEW SCOPE SUCCESS";
                        donemsg.ForeColor = Color.Lime;
                        guna2CustomCheckBox2.Enabled = false;



                    }
                }
                else if (result.Count() <= 2)
                {
                    pending.Visible = false;
                    done.Visible = false;
                    error.Visible = true;
                    error.Location = new Point(94, 429);
                    errormsg.ForeColor = Color.Red;
                    errormsg.Text = "The Code Is Pathced";
                }
                else
                {
                    pending.Visible = false;
                    done.Visible = false;
                    error.Visible = true;
                    error.Location = new Point(94, 429);
                    errormsg.ForeColor = Color.Red;
                    errormsg.Text = "Error.";
                }
            }

        }

        IEnumerable<long> resule7 = null;
        private async void guna2CustomCheckBox3_Click(object sender, EventArgs e)
        {




            string[] pocessname = { "HD-Player" };
            bool sucess = memoryfast.SetProcess(pocessname);
            error.Visible = false;
            done.Visible = false;
            pending.Visible = true;
            pending.Location = new Point(94, 429);
            pendingmsg.ForeColor = Color.Yellow;
            pendingmsg.Text = "AWM-Y Switch Activating.....";

            if (!sucess)
            {
                pending.Visible = false;
                done.Visible = false;
                error.Visible = true;
                error.Location = new Point(94, 429);
                errormsg.ForeColor = Color.Red;
                errormsg.Text = "Error.";
                return;
            }

            // Perform the scan and store the result
            resule7 = await memoryfast.AoBScan("C8 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 00 00 00 01 00 00 00 81 95 E3 3F 01 00 00 00 81 95 E3 3F 00 00 00 00 81 95 E3 3F 00 00 80 3F 00 00 80 3F 0A D7 A3 3D 00 00 00 00 00 00 5C 43 00 00 90 42 00 00 B4 42 96 00 00 00 00 00 00 00 00 00 00 3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 3D 0A 57 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F");

            if (resule7.Any())
            {
                pending.Visible = false;
                done.Visible = true;
                done.Location = new Point(94, 429);
                donemsg.Text = "AWM-Y ULTRA SWITCH SUCCESS";
                donemsg.ForeColor = Color.Lime;
                guna2CustomCheckBox3.Enabled = false;
                guna2Panel3.Visible = false;
                guna2Panel21.Visible = true;
                guna2Panel10.Location = new Point(8, 195);
                guna2Panel4.Location = new Point(8, 258);
                guna2Panel21.Location = new Point(8, 111);
            }
            else
            {
                pending.Visible = false;
                done.Visible = false;
                error.Visible = true;
                error.Location = new Point(94, 429);
                errormsg.ForeColor = Color.Red;
                errormsg.Text = "The Code Is Pathced";
            }


            //string[] pocessname = { "HD-Player" };
            //bool sucess = memoryfast.SetProcess(pocessname);

            //if (!sucess)
            //{
            //    return;
            //}
            //else
            //{
            //    Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
            //    error.Visible = false;
            //    done.Visible = false;
            //    pending.Visible = true;
            //    pending.Location = new Point(94, 429);
            //    pendingmsg.ForeColor = Color.Yellow;
            //    pendingmsg.Text = "AWM-Y Switch Activating.....";
            //    IEnumerable<long> result = await memoryfast.AoBScan("C8 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 00 00 00 01 00 00 00 81 95 E3 3F 01 00 00 00 81 95 E3 3F 00 00 00 00 81 95 E3 3F 00 00 80 3F 00 00 80 3F 0A D7 A3 3D 00 00 00 00 00 00 5C 43 00 00 90 42 00 00 B4 42 96 00 00 00 00 00 00 00 00 00 00 3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 3D 0A 57 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F");
            //    if (result.Count() != 0 && result.Count() < 2)
            //    {
            //        foreach (var currentAddress in result)
            //        {
            //            // Calculate the address of the 10th and 11th bytes in the pattern
            //            Int64 address10thByte = currentAddress + 87L; // 10th byte
            //            Int64 address11thByte = currentAddress + 91L; // 11th byte

            //            // Use AobReplace to modify the byte values at those addresses
            //            memoryfast.AobReplace(address10thByte, "3C"); // Replace 10th byte with 0x3D
            //            memoryfast.AobReplace(address11thByte, "3C"); // Replace 11th byte with 0x3D

            //            pending.Visible = false;
            //            done.Visible = true;
            //            done.Location = new Point(94, 429);
            //            donemsg.Text = "AWM-Y ULTRA SWITCH SUCCESS";
            //            donemsg.ForeColor = Color.Lime;
            //            guna2CustomCheckBox3.Enabled = false;
            //            //guna2Panel3.Visible = false;
            //            //guna2Panel21.Visible = true;  
            //            //guna2Panel10.Location = new Point(8, 195);
            //            //guna2Panel4.Location = new Point(8, 258);
            //            //guna2Panel21.Location = new Point(8, 111);



            //        }
            //    }
            //    else if (result.Count() <= 2)
            //    {
            //        pending.Visible = false;
            //        done.Visible = false;
            //        error.Visible = true;
            //        error.Location = new Point(94, 429);
            //        errormsg.ForeColor = Color.Red;
            //        errormsg.Text = "The Code Is Pathced";
            //    }
            //    else
            //    {
            //        pending.Visible = false;
            //        done.Visible = false;
            //        error.Visible = true;
            //        error.Location = new Point(94, 429);
            //        errormsg.ForeColor = Color.Red;
            //        errormsg.Text = "Error.";
            //    }
            //}
        }


        private async void guna2CustomCheckBox10_Click(object sender, EventArgs e)
        {
            string[] pocessname = { "HD-Player" };
            bool sucess = memoryfast.SetProcess(pocessname);

            if (!sucess)
            {
                return;
            }
            else
            {
                Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
                error.Visible = false;
                done.Visible = false;
                pending.Visible = true;
                pending.Location = new Point(94, 429);
                pendingmsg.ForeColor = Color.Yellow;
                pendingmsg.Text = "AWM Switch Activating.....";
                IEnumerable<long> result = await memoryfast.AoBScan("C8 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 00 00 00 01 00 00 00 8F C2 F5 3F 01 00 00 00 8F C2 F5 3F 00 00 00 00 8F C2 F5 3F 00 00 80 3F 00 00 80 3F 0A D7 A3 3D 00 00 00 00 00 00 5C 43 00 00 90 42 00 00 B4 42 96 00 00 00 00 00 00 00 00 00 00 3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 3D 0A 57 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F");
                if (result.Count() != 0 && result.Count() < 2)
                {
                    foreach (var currentAddress in result)
                    {
                        // Calculate the address of the 10th and 11th bytes in the pattern
                        Int64 address10thByte = currentAddress + 87L; // 10th byte
                        Int64 address11thByte = currentAddress + 91L; // 11th byte

                        // Use AobReplace to modify the byte values at those addresses
                        memoryfast.AobReplace(address10thByte, "3D"); // Replace 10th byte with 0x3D
                        memoryfast.AobReplace(address11thByte, "3D"); // Replace 11th byte with 0x3D

                        pending.Visible = false;
                        done.Visible = true;
                        done.Location = new Point(94, 429);
                        donemsg.Text = "AWM ULTRA SWITCH SUCCESS";
                        donemsg.ForeColor = Color.Lime;
                        guna2CustomCheckBox10.Enabled = false;

                    }
                }
                else if (result.Count() <= 2)
                {
                    pending.Visible = false;
                    done.Visible = false;
                    error.Visible = true;
                    error.Location = new Point(94, 429);
                    errormsg.ForeColor = Color.Red;
                    errormsg.Text = "The Code Is Pathced";
                }
                else
                {
                    pending.Visible = false;
                    done.Visible = false;
                    error.Visible = true;
                    error.Location = new Point(94, 429);
                    errormsg.ForeColor = Color.Red;
                    errormsg.Text = "Error.";
                }
            }
        }


        private async void guna2CustomCheckBox4_Click(object sender, EventArgs e)
        {
            string[] pocessname = { "HD-Player" };
            bool sucess = memoryfast.SetProcess(pocessname);

            if (!sucess)
            {
                return;
            }
            else
            {
                Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
                error.Visible = false;
                done.Visible = false;
                pending.Visible = true;
                pending.Location = new Point(94, 429);
                pendingmsg.ForeColor = Color.Yellow;
                pendingmsg.Text = "M82B Switch Activating.....";
                IEnumerable<long> result = await memoryfast.AoBScan("00 00 00 40 00 00 00 00 00 00 00 40 00 00 80 3F 00 00 80 3F 9A 99 99 3E 00 00 00 00 00 00 5C 43 00 00 28 42 00 00 B4 42 78 00 00 00 00 00 00 00 9A 99 19 3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F CD CC 4C 3F CD CC 8C 3F 00 00 80 3F 00 00 00 00 66");
                if (result.Count() != 0 && result.Count() < 2)
                {
                    foreach (var CurrentAddress in result)
                    {
                        // Calculate the address of the 10th byte in the pattern
                        Int64 address10thByte = CurrentAddress + 51L; // 10th byte (index 9) //69

                        // Calculate the address of the 11th byte in the pattern
                        Int64 address11thByte = CurrentAddress + 55L; // 11th byte (index 10) //73


                        // Write the new value (FF) to both addresses
                        memoryfast.AobReplace(address10thByte, "3C"); // Replace 10th byte with 0x1B
                        memoryfast.AobReplace(address11thByte, "3C"); // Replace 11th byte with 0x10
                        pending.Visible = false;
                        done.Visible = true;
                        done.Location = new Point(94, 429);
                        donemsg.Text = "M82B ULTRA SWITCH SUCCESS";
                        donemsg.ForeColor = Color.Lime;
                        guna2CustomCheckBox4.Enabled = false;
                    }
                }
                else if (result.Count() <= 2)
                {
                    pending.Visible = false;
                    done.Visible = false;
                    error.Visible = true;
                    error.Location = new Point(94, 429);
                    errormsg.ForeColor = Color.Red;
                    errormsg.Text = "The Code Is Pathced";
                }
                else
                {
                    pending.Visible = false;
                    done.Visible = false;
                    error.Visible = true;
                    error.Location = new Point(94, 429);
                    errormsg.ForeColor = Color.Red;
                    errormsg.Text = "Error.";
                }
            }
        }

        /// ////////////////////////////////////////////////////////////

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        const uint PROCESS_CREATE_THREAD = 0x2;
        const uint PROCESS_QUERY_INFORMATION = 0x400;
        const uint PROCESS_VM_OPERATION = 0x8;
        const uint PROCESS_VM_WRITE = 0x20;
        const uint PROCESS_VM_READ = 0x10;
        const uint MEM_COMMIT = 0x1000;
        const uint PAGE_READWRITE = 4;
        private WebClient webclient = new WebClient();


        private static string MN;
        private static string FC;
        private static void threeXBDLove(string resourceName, string outputPath)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            using (Stream resourceStream = executingAssembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new ArgumentException($"Resource '{resourceName}' not found.");
                }
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create))
                {
                    byte[] buffer = new byte[resourceStream.Length];
                    resourceStream.Read(buffer, 0, buffer.Length);
                    fileStream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        private async void guna2CustomCheckBox5_Click(object sender, EventArgs e)
        {
            string processName = "HD-Player";
            string dllResourceName = "ThreeX_Main.Properties.zxocheat.dll";
            string tempDllPath = Path.Combine(Path.GetTempPath(), "zxocheat.dll");
            threeXBDLove(dllResourceName, tempDllPath); ;
            Process[] targetProcesses = Process.GetProcessesByName(processName);
            if (targetProcesses.Length == 0)
            {
                Console.WriteLine($"Waiting for {processName}.exe...");
            }
            if (targetProcesses.Length == 0)
            {
                MessageBox.Show("Open first Emulator....!");
            }
            else
            {
                Process targetProcess = targetProcesses[0];
                IntPtr hProcess = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, targetProcess.Id);
                IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)tempDllPath.Length, MEM_COMMIT, PAGE_READWRITE);
                IntPtr bytesWritten;
                WriteProcessMemory(hProcess, allocMemAddress, System.Text.Encoding.ASCII.GetBytes(tempDllPath), (uint)tempDllPath.Length, out bytesWritten);
                CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
                guna2CustomCheckBox5.Enabled = false;
            }

        }

        private void guna2CustomCheckBox11_Click(object sender, EventArgs e)
        {
            string processName = "HD-Player";
            string dllResourceName = "ThreeX_Main.Properties.CHAMSV2.dll";
            string tempDllPath = Path.Combine(Path.GetTempPath(), "CHAMSV2.dll");
            threeXBDLove(dllResourceName, tempDllPath); ;
            Process[] targetProcesses = Process.GetProcessesByName(processName);
            if (targetProcesses.Length == 0)
            {
                Console.WriteLine($"Waiting for {processName}.exe...");
            }
            if (targetProcesses.Length == 0)
            {
                MessageBox.Show("Open first Emulator....!");
            }
            else
            {
                Process targetProcess = targetProcesses[0];
                IntPtr hProcess = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, targetProcess.Id);
                IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)tempDllPath.Length, MEM_COMMIT, PAGE_READWRITE);
                IntPtr bytesWritten;
                WriteProcessMemory(hProcess, allocMemAddress, System.Text.Encoding.ASCII.GetBytes(tempDllPath), (uint)tempDllPath.Length, out bytesWritten);
                CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
                guna2CustomCheckBox11.Enabled = false;
            }
        }

        bool smOpen = false;
        public static bool Streaming;
        [DllImport("user32.dll")]
        public static extern uint SetWindowDisplayAffinity(IntPtr hwnd, uint dwAffinity);
        private async void guna2CustomCheckBox6_Click(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox6.Checked)
            {
                base.ShowInTaskbar = false;
                Form1.Streaming = true;
                Form1.SetWindowDisplayAffinity(base.Handle, 17U);
         
            }
            else
            {
                base.ShowInTaskbar = true;
                Form1.Streaming = false;
                Form1.SetWindowDisplayAffinity(base.Handle, 0U);
             
            }
        }

        private void guna2CustomCheckBox7_Click(object sender, EventArgs e)
        {

        }
        private Color GetOppositeColor(Color color)
        {
            // Calculate the opposite color by inverting RGB values
            return Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
        }

        private void welcome_Click(object sender, EventArgs e)
        {

        }

        private void loginpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
           
        }


       
        private void guna2CustomCheckBox7_Click_1(object sender, EventArgs e)
        {
           
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
         
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
          
        }

        private void mainpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void guna2ControlBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AIM.Visible = true;
            AIM.Location = new Point(96, 25);
            SNIPER.Visible = false;
            UPCOMING.Visible = false;
            CHAMS.Visible = false;
            INFO.Visible = false;
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            SNIPER.Visible = true;
            SNIPER.Location = new Point(96, 25);
            AIM.Visible = false;
            UPCOMING.Visible = false;
            CHAMS.Visible = false;
            INFO.Visible = false;
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            CHAMS.Visible = true;
            CHAMS.Location = new Point(96, 25);
            AIM.Visible = false;
            SNIPER.Visible = false;
            UPCOMING.Visible = false;
            INFO.Visible = false;
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            UPCOMING.Visible = true;
            UPCOMING.Location = new Point(96, 25);
            AIM.Visible = false;
            SNIPER.Visible = false;
            CHAMS.Visible = false;
            INFO.Visible = false;
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            INFO.Visible = true;
            UPCOMING.Visible = false;
            INFO.Location = new Point(96, 25);
            AIM.Visible = false;
            SNIPER.Visible = false;
            CHAMS.Visible = false;

        }

        private void AIM_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ControlBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void guna2CustomCheckBox12_Click(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox12.Checked)
            {
              EnableBlur();
               
            }
            else
            {
                base.ShowInTaskbar = false;
                Form1.Streaming = true;
                Form1.SetWindowDisplayAffinity(base.Handle, 0U);
                await Task.Delay(1);
                base.ShowInTaskbar = true;
                Form1.Streaming = false;
                Form1.SetWindowDisplayAffinity(base.Handle, 0U);
            }
        }

        private void guna2CustomCheckBox15_Click(object sender, EventArgs e)
        {
            pending.Visible = false;
            done.Visible = false;
            error.Visible = true;
            error.Location = new Point(94, 429);
            errormsg.Text = "SPEED HACK IS COMING SOON";
        }

        private void guna2CustomCheckBox14_Click(object sender, EventArgs e)
        {
            pending.Visible = false;
            done.Visible = false;
            error.Visible = true;
            error.Location = new Point(94, 429);
            errormsg.Text = "WALL HACK IS COMING SOON";
        }

        private void guna2CustomCheckBox13_Click(object sender, EventArgs e)
        {
            pending.Visible = false;
            done.Visible = false;
            error.Visible = true;
            error.Location = new Point(94, 429);
            errormsg.Text = "CAMERA HACK IS COMING SOON";
        }

        private void guna2CustomCheckBox16_Click(object sender, EventArgs e)
        {
            pending.Visible = false;
            done.Visible = false;
            error.Visible = true;
            error.Location = new Point(94, 429);
            errormsg.Text = "GLITCH FIRE IS COMING SOON";
        }

        private void STOPTIMER_Tick(object sender, EventArgs e)
        {
            // Calculate elapsed time
            TimeSpan elapsedTime = DateTime.Now - startTime;

            // Display the elapsed time in the label (in the format hh:mm:ss)
            label25.Text = elapsedTime.ToString(@"hh\:mm\:ss");
        }

        private void DATE_Tick(object sender, EventArgs e)
        {

        }


        


        private async void guna2CustomCheckBox9_Click(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox9.Checked)
            {

                try
                {
                    int proc = Process.GetProcessesByName("HD-Player")[0].Id;
                    memoryfast1.OpenProcess(proc);
                    error.Visible = false;
                    done.Visible = false;
                    pending.Visible = true;
                    pending.Location = new Point(94, 429);
                    pendingmsg.ForeColor = Color.Yellow;
                    pendingmsg.Text = "AIMBOT DRAG APPLYING";

                    IEnumerable<long> result = await memoryfast1.AoBScan(0x0000000000010000, 0x00007ffffffeffff, "00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? 01 00 00 00 00 00 00 00 00 00 00 00 ?? 00 00 00 ?? 00 00 00 00 00 00 00 ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? 00 00 00 01 00 00 00 ?? 00 00 00 ?? ?? ?? ?? ?? 00 00 00 ?? 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF", true, true, string.Empty);

                    if (result.Any())
                    {
                        foreach (var currentAddress in result)
                        {
                            string str = currentAddress.ToString("X");
                            Byte[] valueBytes = memoryfast1.ReadMemoryx((currentAddress + 0x10).ToString("X"), 4);
                            memoryfast1.WriteMemory((currentAddress + 0x5c).ToString("X"), "int", BitConverter.ToInt32(valueBytes, 0).ToString());
                        }

                        pending.Visible = false;
                        done.Visible = true;
                        done.Location = new Point(94, 429);
                        donemsg.Text = done.Text = "AIMBOT DRAG SUCCESS";
                        donemsg.ForeColor = Color.Lime;
                        Time.Visible = true;
                        Time.Text = DateTime.Now.ToString("hh:mm: tt");
                        Time.ForeColor = Color.White;

                    }
                    else
                    {
                        pending.Visible = false;
                        done.Visible = false;
                        error.Visible = true;
                        error.Location = new Point(94, 429);
                        errormsg.ForeColor = Color.Red;
                        errormsg.Text = "AIMBOT DRAG FAILED";
                    }
                }
                catch (Exception ex)
                {
                    errormsg.Text = "Error: " + ex.Message;
                }
            }
            else
            {
                done.Visible = true;
                error.Visible = false;
                pending.Visible = false;
                done.Location = new Point(94, 429);
                donemsg.Text = "BUTTON DISABLE";
            }



        }

        private async void guna2CustomCheckBox7_Click_2(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox7.Checked)
            {

                try
                {
                    int proc = Process.GetProcessesByName("HD-Player")[0].Id;
                    memoryfast1.OpenProcess(proc);
                    error.Visible = false;
                    done.Visible = false;
                    pending.Visible = true;
                    pending.Location = new Point(94, 429);
                    pendingmsg.ForeColor = Color.Yellow;
                    pendingmsg.Text = "AIMBOT DRAG APPLYING";

                    IEnumerable<long> result = await memoryfast1.AoBScan(0x0000000000010000, 0x00007ffffffeffff, "FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43 00 00", true, true, string.Empty);

                    if (result.Any())
                    {
                        foreach (var currentAddress in result)
                        {
                            string str = currentAddress.ToString("X");
                            Byte[] valueBytes = memoryfast1.ReadMemoryx((currentAddress + 0x9c).ToString("X"), 4);
                            memoryfast1.WriteMemory((currentAddress + 0x6c).ToString("X"), "int", BitConverter.ToInt32(valueBytes, 0).ToString());
                        }

                        pending.Visible = false;
                        done.Visible = true;
                        done.Location = new Point(94, 429);
                        donemsg.Text = done.Text = "AIMBOT DRAG SUCCESS";
                        donemsg.ForeColor = Color.Lime;
                        Time.Visible = true;
                        Time.Text = DateTime.Now.ToString("hh:mm: tt");
                        Time.ForeColor = Color.White;

                    }
                    else
                    {
                        pending.Visible = false;
                        done.Visible = false;
                        error.Visible = true;
                        error.Location = new Point(94, 429);
                        errormsg.ForeColor = Color.Red;
                        errormsg.Text = "AIMBOT DRAG FAILED";
                    }
                }
                catch (Exception ex)
                {
                    errormsg.Text = "Error: " + ex.Message;
                }
            }
            else
            {
                done.Visible = true;
                error.Visible = false;
                pending.Visible = false;
                done.Location = new Point(94, 429);
                donemsg.Text = "BUTTON DISABLE";
            }
        }

        private async void guna2CustomCheckBox8_Click(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox8.Checked)
            {
                var stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                int proc = Process.GetProcessesByName("HD-Player")[0].Id;
                memoryfast1.OpenProcess(proc);
                error.Visible = false;
                done.Visible = false;
                pending.Visible = true;
                pending.Location = new Point(94, 429);
                pendingmsg.ForeColor = Color.Yellow;
                pendingmsg.Text = "AIMBOT HEAD APPLYING";

                IEnumerable<long> result = await memoryfast1.AoBScan(0x0000000000010000, 0x00007ffffffeffff, "00 00 00 A5 43 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00", true, true, string.Empty);

                if (result.Any())
                {
                    foreach (var currentAddress in result)
                    {
                        string str = currentAddress.ToString("X"); // for what ?
                        Byte[] PointerVal = memoryfast1.ReadMemoryx((currentAddress + 0x5C).ToString("X"), 4);
                        Byte[] valueBytes = memoryfast1.ReadMemoryx((currentAddress + 0x58).ToString("X"), 4);
                        memoryfast1.WriteMemory((currentAddress + 0x5C).ToString("X"), "int", BitConverter.ToInt32(valueBytes, 0).ToString());
                        memoryfast1.WriteMemory((currentAddress + 0x58).ToString("X"), "int", BitConverter.ToInt32(PointerVal, 0).ToString());
                    }

                    stopwatch.Stop();
                    double elapsedTime = stopwatch.Elapsed.TotalSeconds;

                    pending.Visible = false;
                    done.Visible = true;
                    done.Location = new Point(94, 429);
                    donemsg.Text = done.Text = $"AIMBOT Head SUCCESS ({elapsedTime:F2} seconds)";
                    donemsg.ForeColor = Color.Lime;
                    Time.Visible = true;
                    Time.Text = DateTime.Now.ToString("hh:mm: tt");
                    Time.ForeColor = Color.White;


                    Console.Beep(400, 200);

                }
                else
                {
                    pending.Visible = false;
                    done.Visible = false;
                    error.Visible = true;
                    error.Location = new Point(94, 429);
                    errormsg.ForeColor = Color.Red;
                    errormsg.Text = "Aimbot Head Failed";
                }
            }
            else
            {
                error.Visible = false;
                done.Visible = false;
                pending.Visible = true;
                pending.Location = new Point(94, 429);
                pendingmsg.ForeColor = Color.Yellow;
                pendingmsg.Text = "AIMBOT BUTTON DISABLE";
            }
        }

        private async void switchtrack_Scroll(object sender, ScrollEventArgs e)
        {
            switch (switchtrack.Value)
            {
                case 1:

                    if (resule7.Count() != 0 && resule7.Count() < 2)
                    {
                        foreach (var currentAddress in resule7)
                        {
                            // Calculate the address of the 10th and 11th bytes in the pattern
                            Int64 address10thByte = currentAddress + 87L; // 10th byte
                            Int64 address11thByte = currentAddress + 91L; // 11th byte

                            // Use AobReplace to modify the byte values at those addresses
                            memoryfast.AobReplace(address10thByte, "3F"); // Replace 10th byte with 0x3D
                            memoryfast.AobReplace(address11thByte, "3E"); // Replace 11th byte with 0x3D
                       

                        }
                    }
                 
                    break;
                case 2:
                

                    if (resule7.Count() != 0 && resule7.Count() < 2)
                    {
                        foreach (var currentAddress in resule7)
                        {
                            // Calculate the address of the 10th and 11th bytes in the pattern
                            Int64 address10thByte = currentAddress + 87L; // 10th byte
                            Int64 address11thByte = currentAddress + 91L; // 11th byte

                            // Use AobReplace to modify the byte values at those addresses
                            memoryfast.AobReplace(address10thByte, "3D"); // Replace 10th byte with 0x3D
                            memoryfast.AobReplace(address11thByte, "3D"); // Replace 11th byte with 0x3D

                       
                        }
                    }

                    break;
                case 3:
                    if (resule7.Count() != 0 && resule7.Count() < 2)
                    {
                        foreach (var currentAddress in resule7)
                        {
                            // Calculate the address of the 10th and 11th bytes in the pattern
                            Int64 address10thByte = currentAddress + 87L; // 10th byte
                            Int64 address11thByte = currentAddress + 91L; // 11th byte

                            // Use AobReplace to modify the byte values at those addresses
                            memoryfast.AobReplace(address10thByte, "3C"); // Replace 10th byte with 0x3D
                            memoryfast.AobReplace(address11thByte, "3C"); // Replace 11th byte with 0x3D

                       
                        }
                    }
                    break;
                case 4:
                    if (resule7.Count() != 0 && resule7.Count() < 2)
                    {
                        foreach (var currentAddress in resule7)
                        {
                            // Calculate the address of the 10th and 11th bytes in the pattern
                            Int64 address10thByte = currentAddress + 87L; // 10th byte
                            Int64 address11thByte = currentAddress + 91L; // 11th byte

                            // Use AobReplace to modify the byte values at those addresses
                            memoryfast.AobReplace(address10thByte, "3B"); // Replace 10th byte with 0x3D
                            memoryfast.AobReplace(address11thByte, "3B"); // Replace 11th byte with 0x3D

                      
                        }
                    }
                    break;
                case 5:

                    if (resule7.Count() != 0 && resule7.Count() < 2)
                    {
                        foreach (var currentAddress in resule7)
                        {
                            // Calculate the address of the 10th and 11th bytes in the pattern
                            Int64 address10thByte = currentAddress + 87L; // 10th byte
                            Int64 address11thByte = currentAddress + 91L; // 11th byte

                            // Use AobReplace to modify the byte values at those addresses
                            memoryfast.AobReplace(address10thByte, "3A"); // Replace 10th byte with 0x3D
                            memoryfast.AobReplace(address11thByte, "3A"); // Replace 11th byte with 0x3D

                   
                        }
                    }
                    break;

            }
        }
       
        private void Crack_Tick(object sender, EventArgs e)
        {
           
        }

        private void server_Tick(object sender, EventArgs e)
        {

        }
    }
}



