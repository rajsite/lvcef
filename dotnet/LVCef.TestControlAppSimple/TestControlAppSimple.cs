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
        LVCefClient lvcefclient = null;

        public TestControlAppSimpleForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lvCefControl1.StartUrl = "http://localhost:8000/jstest.html";
            lvcefclient = new LVCefClient(lvCefControl1);
            lvcefclient.MessageRouterHandler.OnQueryEvent += (src, onquery) =>
            {
                onquery.Handled = true;
                onquery.callback.Success("huzzah");
            };
            lvcefclient.createBrowser();
        }
    }
}
