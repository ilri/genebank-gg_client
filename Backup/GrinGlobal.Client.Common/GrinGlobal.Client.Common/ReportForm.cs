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
    public partial class ReportForm : Form
    {
        private DataTable _reportSourceTable;
        private string _crystalReportFilePath;

        public ReportForm(DataTable reportData, string crystalReportFilePath)
        {
            InitializeComponent();

            // Make a copy of the data to be provided to the Crystal Report...
            _reportSourceTable = reportData.Copy();
            _crystalReportFilePath = crystalReportFilePath;
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument reportDocument = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            reportDocument.Load(_crystalReportFilePath);
            reportDocument.SetDataSource(_reportSourceTable);  // C:\VisualStudio2008_SVN\MyPlayground\MyPlayground\FieldLabel1.rpt
            crystalReportViewer1.ReportSource = reportDocument;
        }
    }
}
