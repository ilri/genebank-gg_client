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
    public partial class RichTextEditor : Form
    {
        public RichTextEditor(string RichText, bool readOnly)
        {
            InitializeComponent();

            // Populate the RichTextBox...
            ux_richtextboxMessage.Text = RichText;
            // Set RichTextBox edit mode...
            ux_richtextboxMessage.ReadOnly = readOnly;
        }

        public string RichTextMessage
        {
            get
            {
                return ux_richtextboxMessage.Text;
            }
            set
            {
                ux_richtextboxMessage.Text = value;
            }
        }
    }
}
