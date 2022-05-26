using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;
using System.Text;

namespace F_Tool
{


    public class Form1 : Form
    {
        private Dictionary<UserControl1, int> timers = new Dictionary<UserControl1, int>();
        private int windowHandle;
        private Form1.States state = Form1.States.STOPPED;
        private IContainer components = (IContainer)null;
        internal Label Label5;
        internal ComboBox ComboBoxWindows;
        internal Label Label3;
        internal TextBox TextBoxHwnd;
        private Button button1;
        private Panel panel1;
        internal Button ButtonGetActive;
        private Button button2;
        private Timer timer1;
        private static Dictionary<IntPtr, string> Windows = new Dictionary<IntPtr, string>();
        private Form1.CallBackPtr callBackPtr;
        private ListBox cmbWindowNames;

        [DllImport("user32.dll")]
        private static extern int EnumWindows(Form1.CallBackPtr callPtr, int lPar);


        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hWnd, int Msg, uint wParam, int lParam);

        public Form1() => this.InitializeComponent();

        private void getWindowsList(object sender, EventArgs e)
        {
            this.ComboBoxWindows.Items.Clear();
            this.windowHandle = 0;

            TextBoxHwnd_KeyUp(null, null);

            foreach (Object item in cmbWindowNames.Items)
            {
                //Console.WriteLine("getWindowsList: " + item.ToString());
                ComboBoxWindows.Items.Add(item);
            }
        }


        private void RefreshWindowsList()
        {
            Form1.Windows.Clear();
            this.callBackPtr = new Form1.CallBackPtr(Form1.Report);
            Form1.EnumWindows(this.callBackPtr, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.state == Form1.States.STOPPED)
            {
                UserControl1 key = new UserControl1();
                this.panel1.Height += key.Height;
                this.Height += key.Height;
                key.Dock = DockStyle.Top;
                this.panel1.Controls.Add((Control)key);
                this.timers.Add(key, 0);
            }

        }

        private void panel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            this.panel1.Height -= e.Control.Height;
            this.Height -= e.Control.Height;
            this.timers.Remove((UserControl1)e.Control);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
                return;
            int num = (int)MessageBox.Show("Please Run with Admin privilege!");
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (KeyValuePair<UserControl1, int> pair in this.timers.ToList<KeyValuePair<UserControl1, int>>())
            {
                if (pair.Value <= 0)
                {
                    this.pressKEy(pair);
                    this.timers[pair.Key] = pair.Key.getInterval();
                }
                else
                {
                    Dictionary<UserControl1, int> timers;
                    UserControl1 key;
                    (timers = this.timers)[key = pair.Key] = timers[key] - 1;
                }
            }
        }

        private void pressKEy(KeyValuePair<UserControl1, int> pair)
        {
            Form1.PostMessage((IntPtr)this.windowHandle, 0x100, (uint)pair.Key.getKey(), 0);
            Form1.PostMessage((IntPtr)this.windowHandle, 0x101, (uint)pair.Key.getKey(), 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.state == Form1.States.STOPPED)
            {
                if (this.windowHandle == 0 || this.ComboBoxWindows.Text == "")
                {
                    int num = (int)MessageBox.Show("Please make sure to choose a window before!");
                }
                else
                {
                    foreach (UserControl1 key in this.timers.Keys.ToList<UserControl1>())
                    {
                        if (!key.isready())
                            return;
                        this.timers[key] = 0;
                    }
                    this.timer1.Start();
                    this.button2.Text = "Stop";
                    this.state = Form1.States.STARTED;
                }
            }
            else
            {
                this.timer1.Stop();
                this.button2.Text = "Start";
                this.state = Form1.States.STOPPED;
            }
        }


        private void ComboBoxWindows_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ComboBoxWindows.SelectedItem == null || this.ComboBoxWindows.Text == "")
            {
                int num = (int)MessageBox.Show("Please choose a window!");
            }

            WindowListItem item = (WindowListItem)this.ComboBoxWindows.SelectedItem;
            //Console.WriteLine("SelectedItem: " + item.GetHwnd());

            this.windowHandle = (int)item.GetHwnd();
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void ButtonGetActive_Click_1(object sender, EventArgs e)
        {
            try
            {
                Form1.SetForegroundWindow(new IntPtr(this.windowHandle));
            }
            catch
            {
                int num = (int)MessageBox.Show("Window does not exist!");
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cmbWindowNames = new System.Windows.Forms.ListBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.ComboBoxWindows = new System.Windows.Forms.ComboBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.TextBoxHwnd = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ButtonGetActive = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // cmbWindowNames
            // 
            this.cmbWindowNames.Location = new System.Drawing.Point(0, 0);
            this.cmbWindowNames.Name = "cmbWindowNames";
            this.cmbWindowNames.Size = new System.Drawing.Size(120, 96);
            this.cmbWindowNames.TabIndex = 0;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(5, 49);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(37, 13);
            this.Label5.TabIndex = 33;
            this.Label5.Text = "Select";
            // 
            // ComboBoxWindows
            // 
            this.ComboBoxWindows.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxWindows.FormattingEnabled = true;
            this.ComboBoxWindows.Location = new System.Drawing.Point(46, 44);
            this.ComboBoxWindows.Name = "ComboBoxWindows";
            this.ComboBoxWindows.Size = new System.Drawing.Size(178, 21);
            this.ComboBoxWindows.TabIndex = 31;
            this.ComboBoxWindows.SelectedIndexChanged += new System.EventHandler(this.ComboBoxWindows_SelectedIndexChanged);
            this.ComboBoxWindows.Click += new System.EventHandler(this.getWindowsList);
            // 
            // Label3
            // 
            this.Label3.AllowDrop = true;
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(5, 15);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(29, 13);
            this.Label3.TabIndex = 32;
            this.Label3.Text = "Filter";
            // 
            // TextBoxHwnd
            // 
            this.TextBoxHwnd.Location = new System.Drawing.Point(50, 12);
            this.TextBoxHwnd.Name = "TextBoxHwnd";
            this.TextBoxHwnd.Size = new System.Drawing.Size(245, 20);
            this.TextBoxHwnd.TabIndex = 34;
            this.TextBoxHwnd.Text = "Flyff Universe";
            this.TextBoxHwnd.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBoxHwnd_KeyUp);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 90);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(307, 23);
            this.button1.TabIndex = 35;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(38, 74);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(223, 10);
            this.panel1.TabIndex = 36;
            this.panel1.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.panel1_ControlRemoved);
            // 
            // ButtonGetActive
            // 
            this.ButtonGetActive.Location = new System.Drawing.Point(229, 44);
            this.ButtonGetActive.Name = "ButtonGetActive";
            this.ButtonGetActive.Size = new System.Drawing.Size(67, 21);
            this.ButtonGetActive.TabIndex = 37;
            this.ButtonGetActive.Text = "Switch to";
            this.ButtonGetActive.UseVisualStyleBackColor = true;
            this.ButtonGetActive.Click += new System.EventHandler(this.ButtonGetActive_Click_1);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button2.Location = new System.Drawing.Point(0, 113);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(307, 23);
            this.button2.TabIndex = 38;
            this.button2.Text = "Start";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 136);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ButtonGetActive);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.ComboBoxWindows);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.TextBoxHwnd);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Flyff-Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private enum States
        {
            STARTED,
            STOPPED,
        }

        public delegate bool CallBackPtr(int hwnd, int lParam);

        public static bool Report(int hWnd, int lParam)
        {
            StringBuilder lpString = new StringBuilder(Form1.GetWindowTextLength((IntPtr)hWnd) + 1);
            Form1.GetWindowText((IntPtr)hWnd, lpString, lpString.Capacity);
            string str = lpString.ToString();
            Form1.Windows.Add((IntPtr)hWnd, str);
            return true;
        }

        private void TextBoxHwnd_KeyUp(object sender, KeyEventArgs e)
        {
            this.RefreshWindowsList();
            this.cmbWindowNames.Items.Clear();
            if (this.TextBoxHwnd.Text.Length <= 0)
                return;
            foreach (KeyValuePair<IntPtr, string> keyValuePair in Form1.Windows.Where<KeyValuePair<IntPtr, string>>((Func<KeyValuePair<IntPtr, string>, bool>)(kp => kp.Value.StartsWith(this.TextBoxHwnd.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))))
                this.cmbWindowNames.Items.Add((object)new WindowListItem()
                {
                    HWnd = keyValuePair.Key,
                    WindowName = keyValuePair.Value
                });
        }
    }
}
