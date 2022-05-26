using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace F_Tool
{
  public class UserControl1 : UserControl
  {
    private IContainer components = (IContainer) null;
    private ComboBox comboBox1;
    private TextBox textBox1;
    private Button button1;

    public UserControl1()
    {
      this.InitializeComponent();
      this.comboBox1.Items.Add((object) Keys.F1);
      this.comboBox1.Items.Add((object) Keys.F2);
      this.comboBox1.Items.Add((object) Keys.F3);
      this.comboBox1.Items.Add((object) Keys.F4);
      this.comboBox1.Items.Add((object) Keys.F5);
      this.comboBox1.Items.Add((object) Keys.F6);
      this.comboBox1.Items.Add((object) Keys.F7);
      this.comboBox1.Items.Add((object) Keys.F8);
      this.comboBox1.Items.Add((object) Keys.F9);
      this.comboBox1.Items.Add((object) Keys.F10);
      this.comboBox1.Items.Add((object) UserControl1.Keys_Numbers.key_0);
      this.comboBox1.Items.Add((object) UserControl1.Keys_Numbers.key_1);
      this.comboBox1.Items.Add((object) UserControl1.Keys_Numbers.key_2);
      this.comboBox1.Items.Add((object) UserControl1.Keys_Numbers.key_3);
      this.comboBox1.Items.Add((object) UserControl1.Keys_Numbers.key_4);
      this.comboBox1.Items.Add((object) UserControl1.Keys_Numbers.key_5);
      this.comboBox1.Items.Add((object) UserControl1.Keys_Numbers.key_6);
      this.comboBox1.Items.Add((object) UserControl1.Keys_Numbers.key_7);
      this.comboBox1.Items.Add((object) UserControl1.Keys_Numbers.key_8);
      this.comboBox1.Items.Add((object) UserControl1.Keys_Numbers.key_9);
    }

    private void button1_Click(object sender, EventArgs e) => this.Parent.Controls.Remove((Control) this);

    public Keys getKey() => (Keys) this.comboBox1.SelectedItem;

    public int getInterval() => Convert.ToInt32(float.Parse(this.textBox1.Text) * 10f);

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (!Regex.Match(textBox.Text, "[^,.0-9]").Success)
        return;
      textBox.Text = "";
      int num = (int) MessageBox.Show("Only numbers allowed", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    public bool isready()
    {
      if (this.comboBox1.SelectedItem == null || this.comboBox1.Text == "")
      {
        int num = (int) MessageBox.Show("Please select an F-key");
        return false;
      }
      if (!(this.textBox1.Text == ""))
        return true;
      int num1 = (int) MessageBox.Show("Please select an interval (in seconds)");
      return false;
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(7, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(82, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textBox1.Location = new System.Drawing.Point(99, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(77, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(188, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(224, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    [Flags]
    [ComVisible(true)]
    [Editor("System.Windows.Forms.Design.ShortcutKeysEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
    [TypeConverter(typeof (KeysConverter))]
    public enum Keys_Numbers
    {
      key_0 = 48, // 0x00000030
      key_1 = 49, // 0x00000031
      key_2 = 50, // 0x00000032
      key_3 = 51, // 0x00000033
      key_4 = 52, // 0x00000034
      key_5 = 53, // 0x00000035
      key_6 = 54, // 0x00000036
      key_7 = 55, // 0x00000037
      key_8 = 56, // 0x00000038
      key_9 = 57, // 0x00000039
    }
  }
}
