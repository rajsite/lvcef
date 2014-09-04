using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LVCef.ClientHandlers;

namespace TestControlAppSimple
{
    public partial class TestControlAppSimpleForm : Form
    {
        public TestControlAppSimpleForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lvCefControl1.StartUrl = "http://localhost:8000/jstest.html";
            LVCefClient lvcefclient = lvCefControl1.CefClient;
            lvcefclient.MessageRouterHandler.OnQueryEvent += (src, onquery) =>
            {
                onquery.Handled = true;
                onquery.callback.Success("huzzah");
            };
            lvCefControl1.createBrowser();
        }
    }
}
