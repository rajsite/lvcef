using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using LVCef.ClientHandlers;

namespace TestControlAppSimple
{
    public partial class TestControlAppSimpleForm : Form
    {
        private const string DBGPREFIX = "[TestControlAppSimple]: ";

        public TestControlAppSimpleForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lvCefControl1.StartUrl = "http://localhost:8000/jstest.html";
            LVCefClient lvcefclient = lvCefControl1.CefClient;
            if (lvCefControl1.CefClient != null)
            {
                lvcefclient.MessageRouterHandler.OnQueryEvent += (src, onquery) =>
                {
                    onquery.Handled = true;
                    onquery.callback.Success("huzzah");
                };
                lvcefclient.LifeSpanHandler.OnAfterCreatedEvent += (src, onafter) =>
                {
                    if(button2.InvokeRequired)
                        button2.Invoke((Action)delegate { button2.Enabled = true; });
                    else
                        button2.Enabled = true;
                };

                lvCefControl1.createBrowser();
            }
        }

        private void TestControlAppSimpleForm_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(DBGPREFIX + "Navigating to new URL");
            lvCefControl1.Browser.GetMainFrame().LoadUrl("http://localhost:8000/dummy.html");
        }
    }
}
