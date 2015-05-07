using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GRINGlobal.Client.Common
{
    public partial class GGMessageBox : Form
    {
        public GGMessageBox(string messageText, string captionText, MessageBoxButtons messageBoxButtons, MessageBoxDefaultButton messageBoxDefaultButton)
        {
            InitializeComponent();

            FormatForm(messageText, captionText, messageBoxButtons, messageBoxDefaultButton);
        }

        public string Caption
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        public string MessageText
        {
            get
            {
                return ux_textboxMessage.Text;
            }
            set
            {
                ux_textboxMessage.Text = value;
            }
        }

        private void GGMessageBox_Load(object sender, EventArgs e)
        {
            this.ux_textboxMessage.Text = ux_textboxMessage.Text.Replace("\\r", "\r").Replace("\\n", "\n").Replace(Environment.NewLine, "__newline__").Replace("\r", Environment.NewLine).Replace("\n", Environment.NewLine).Replace("__newline__", Environment.NewLine);
        }

        private void FormatForm(string messageText, string captionText, MessageBoxButtons messageBoxButtons, MessageBoxDefaultButton messageBoxDefaultButton)
        {
            // NOTE:  the location of the visible buttons is relative to the left most button (ux_button3)
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = captionText;
this.ux_textboxMessage.Text = messageText.Replace("\\r", "\r").Replace("\\n", "\n").Replace(Environment.NewLine, "__newline__").Replace("\r", Environment.NewLine).Replace("\n", Environment.NewLine).Replace("__newline__", Environment.NewLine);
            switch (messageBoxButtons)
            {
                case MessageBoxButtons.AbortRetryIgnore:
                    this.ux_button1.DialogResult = DialogResult.Abort;
                    this.ux_button1.Text = DialogResult.Abort.ToString();
                    this.ux_button2.DialogResult = DialogResult.Retry;
                    this.ux_button2.Text = DialogResult.Retry.ToString();
                    this.ux_button3.DialogResult = DialogResult.Ignore;
                    this.ux_button3.Text = DialogResult.Ignore.ToString();
                    break;
                case MessageBoxButtons.OK:
                    this.ux_button1.DialogResult = DialogResult.OK;
                    this.ux_button1.Text = DialogResult.OK.ToString();
                    // Move button1 to button3 location...
                    this.ux_button1.Location = this.ux_button3.Location;
                    this.ux_button2.Visible = false;
                    this.ux_button3.Visible = false;
                    break;
                case MessageBoxButtons.OKCancel:
                    this.ux_button1.DialogResult = DialogResult.OK;
                    this.ux_button1.Text = DialogResult.OK.ToString();
                    this.ux_button2.DialogResult = DialogResult.Cancel;
                    this.ux_button2.Text = DialogResult.Cancel.ToString();
                    // Move button1 to button2 location...
                    this.ux_button1.Location = this.ux_button2.Location;
                    // Move button2 to button3 location...
                    this.ux_button2.Location = this.ux_button3.Location;
                    // Hide button3
                    this.ux_button3.Visible = false;
                    break;
                case MessageBoxButtons.RetryCancel:
                    this.ux_button1.DialogResult = DialogResult.Retry;
                    this.ux_button1.Text = DialogResult.Retry.ToString();
                    this.ux_button2.DialogResult = DialogResult.Cancel;
                    this.ux_button2.Text = DialogResult.Cancel.ToString();
                    // Move button1 to button2 location...
                    this.ux_button1.Location = this.ux_button2.Location;
                    // Move button2 to button3 location...
                    this.ux_button2.Location = this.ux_button3.Location;
                    // Hide button3
                    this.ux_button3.Visible = false;
                    break;
                case MessageBoxButtons.YesNo:
                    this.ux_button1.DialogResult = DialogResult.Yes;
                    this.ux_button1.Text = DialogResult.Yes.ToString();
                    this.ux_button2.DialogResult = DialogResult.No;
                    this.ux_button2.Text = DialogResult.No.ToString();
                    // Move button1 to button2 location...
                    this.ux_button1.Location = this.ux_button2.Location;
                    // Move button2 to button3 location...
                    this.ux_button2.Location = this.ux_button3.Location;
                    // Hide button3
                    this.ux_button3.Visible = false;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    this.ux_button1.DialogResult = DialogResult.Yes;
                    this.ux_button1.Text = DialogResult.Yes.ToString();
                    this.ux_button2.DialogResult = DialogResult.No;
                    this.ux_button2.Text = DialogResult.No.ToString();
                    this.ux_button3.DialogResult = DialogResult.Cancel;
                    this.ux_button3.Text = DialogResult.Cancel.ToString();
                    break;
                default:
                    this.ux_button1.DialogResult = DialogResult.OK;
                    this.ux_button1.Text = DialogResult.OK.ToString();
                    // Move button1 to button3 location...
                    this.ux_button1.Location = this.ux_button3.Location;
                    this.ux_button2.Visible = false;
                    this.ux_button3.Visible = false;
                    break;
            }

            switch (messageBoxDefaultButton)
            {
                case MessageBoxDefaultButton.Button1:
                    this.ux_button1.Visible = true;
                    this.ux_button1.Focus();
                    this.ActiveControl = this.ux_button1;
                    break;
                case MessageBoxDefaultButton.Button2:
                    if (messageBoxButtons != MessageBoxButtons.OK)
                    {
                        this.ux_button2.Visible = true;
                        this.ux_button2.Focus();
                        this.ActiveControl = this.ux_button2;
                    }
                    break;
                case MessageBoxDefaultButton.Button3:
                    if (messageBoxButtons == MessageBoxButtons.AbortRetryIgnore ||
                        messageBoxButtons == MessageBoxButtons.YesNoCancel)
                    {
                        this.ux_button3.Visible = true;
                        this.ux_button3.Focus();
                        this.ActiveControl = this.ux_button3;
                    }
                    break;
                default:
                    this.ux_button1.Visible = true;
                    this.ux_button1.Focus();
                    this.ActiveControl = this.ux_button1;
                    break;
            }
        }
    }
}
