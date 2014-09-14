namespace LVCef.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using System.Diagnostics;
    using Xilium.CefGlue;
    using LVCef.ClientHandlers;

    [ToolboxBitmap(typeof(LVCefControl))]
    public class LVCefControl : Control
    {
        private const string DBGPREFIX = "[LVCef][LVCefControl]: ";
        private bool _controlHandleCreated;
        private CefBrowser _browser;
        private LVCefClient _lvCefClient; //keeping references to anything CEF makes it crashy, release reference after browser made

        [DefaultValue("about:blank")]
        public string StartUrl { get; set; }

        [Browsable(false)]
        public LVCefClient CefClient
        {
            get {
                if(_lvCefClient == null && Browser == null) 
                    _lvCefClient = new LVCefClient(this); // only make when needed
                return _lvCefClient; 
            }
            internal set { _lvCefClient = value; }
        }

        // Should only be set from the onBrowserCreated callback 
        [Browsable(false)]
        public CefBrowser Browser
        {
            get { return _browser; }
            internal set
            {
                Trace.Assert(value.GetType() == typeof(CefBrowser));
                Debug.WriteLine(DBGPREFIX + "has initialized the CefBrowser");
                _browser = value; //TODO  why does saving this break everything, apparantly not saving breaks less?

                //Accessed from a thread it was not created on?
                ResizeWindow(Handle, Width, Height);

                CefClient = null; //holding onto references makes it crashy, release after browser made
            }
        }

        public LVCefControl()
        {
            Debug.WriteLine(DBGPREFIX + "Constructor");
            SetStyle(
                ControlStyles.ContainerControl
                | ControlStyles.ResizeRedraw
                | ControlStyles.FixedWidth
                | ControlStyles.FixedHeight
                | ControlStyles.StandardClick
                | ControlStyles.UserMouse
                | ControlStyles.SupportsTransparentBackColor
                | ControlStyles.StandardDoubleClick
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.CacheText
                | ControlStyles.EnableNotifyMessage
                | ControlStyles.DoubleBuffer
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UseTextForAccessibility
                | ControlStyles.Opaque,
                false);

            SetStyle(
                ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.Selectable,
                true);

            StartUrl = "about:blank";

        }

        public void InvalidateSize()
        {
            Debug.WriteLine(DBGPREFIX + "InvalidateSize");
            ResizeWindow(Handle, Width, Height);
        }

        //Race condition? user calls createBrowser twice very quickly
        //Does CEF check for multiple uses of same handle?
        public void createBrowser()
        {
            Debug.WriteLine(DBGPREFIX + "createBrowser starting creation of CefBrowser");
            if (Browser == null)
            {
                var settings = new CefBrowserSettings { };
                CefBrowserHost.CreateBrowser(getCefWindowInfo(), CefClient, settings, StartUrl);
            }
            else
                Debug.WriteLine(DBGPREFIX + "createBrowser has already created CefBrowser instance for this control, request ignored.");
        }

        //Needed to modify control attributes from CEF callbacks, etc
		internal void InvokeIfRequired(Action a)
		{
			if (InvokeRequired)
				Invoke(a);
			else
				a();
		}

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            Debug.WriteLine(DBGPREFIX + "OnHandleCreated");
            // Assuming OnHandleCreated only ever called once
            //if (DesignMode) // LabVIEW never uses design mode, draw design mode until createBrowser called
            if (!_controlHandleCreated)
                Paint += PaintInDesignMode;

            _controlHandleCreated = true;
        }

        //Should I be making a Dispose method as well? http://msdn.microsoft.com/en-us/library/fs2xkftw(v=vs.110).aspx
        //No, see implementing for derived class in article
        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine(DBGPREFIX + "Start Dispose with browser" + ((_browser == null) ? " " : " not ") + "null");
            if (_browser != null && disposing) // TODO: ugly hack to avoid crashes when CefWebBrowser are Finalized and underlying objects already finalized
            {
                if (_browser.IsLoading)
                {
                    Debug.WriteLine(DBGPREFIX + "Browser still trying to load a page");
                    System.Threading.Thread.Sleep(50);
                }
                var host = _browser.GetHost();
                if (host != null)
                {
                    host.CloseBrowser(false);
                    host.Dispose();
                }
                _browser.Dispose();
                _browser = null;
                //Paint += PaintInDesignMode; //Not needed because LabVIEW creates a new control instance
            }
            
            base.Dispose(disposing);
            Debug.WriteLine(DBGPREFIX + "Finish Dispose");
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Debug.WriteLine(DBGPREFIX + "Resize");
            if (Handle != IntPtr.Zero)
            {
                // Ignore size changes when form are minimized.
                var form = TopLevelControl as Form;
                if (form != null && form.WindowState == FormWindowState.Minimized)
                {
                    return;
                }

                ResizeWindow(Handle, Width, Height);
            }
            else
                Debug.WriteLine(DBGPREFIX + "Resize ignored, Handle zero");
        }

        private void PaintInDesignMode(object sender, PaintEventArgs e)
        {
            var width = this.Width;
            var height = this.Height;
            if (width > 1 && height > 1)
            {
                var brush = new SolidBrush(this.ForeColor);
                var pen = new Pen(this.ForeColor);
                pen.DashStyle = DashStyle.Dash;

                e.Graphics.DrawRectangle(pen, 0, 0, width - 1, height - 1);

                var fontHeight = (int)(this.Font.GetHeight(e.Graphics) * 1.25);

                var x = 3;
                var y = 3;

                e.Graphics.DrawString("LVCefControl", Font, brush, x, y + (0 * fontHeight));
                e.Graphics.DrawString(string.Format("StartUrl: {0}", StartUrl), Font, brush, x, y + (1 * fontHeight));

                brush.Dispose();
                pen.Dispose();
            }
        }

        private static void ResizeWindow(IntPtr handle, int width, int height)
        {
            Debug.WriteLine(DBGPREFIX + "ResizeWindow");
            if (handle != IntPtr.Zero)
            {
                
                NativeMethods.SetWindowPos(handle, IntPtr.Zero,
                    0, 0, width, height,
                    SetWindowPosFlags.NoMove | SetWindowPosFlags.NoZOrder
                    );
                 
            }
        }

        private CefWindowInfo getCefWindowInfo()
        {
            Debug.WriteLine(DBGPREFIX + "getCefWindowInfo");
            if (_controlHandleCreated)
            {
                var windowInfo = CefWindowInfo.Create();
                windowInfo.SetAsChild(Handle, new CefRectangle { X = 0, Y = 0, Width = Width, Height = Height });
                return windowInfo;
            }
            else
            {
                throw new CefRuntimeException("LVCefControl handle must be created before it can be used (added to a form, etc)");
            }
        }

    }
}
