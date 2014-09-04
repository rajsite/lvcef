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
        //private IntPtr _browserWindowHandle;
        
        private LVCefClient _lvCefClient; //keeping references to anything CEF makes it crashy, release reference after browser made

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


        [DefaultValue("about:blank")]
        public string StartUrl { get; set; }

        [Browsable(false)]
        public CefBrowserSettings BrowserSettings { get; set; }

        /*
		internal void InvokeIfRequired(Action a)
		{
			if (InvokeRequired)
				Invoke(a);
			else
				a();
		}
        */

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
                //_browserWindowHandle = IntPtr.Zero;
                //_lvCefClient = null;
                //Paint += PaintInDesignMode;
            }
            
            base.Dispose(disposing);
            Debug.WriteLine(DBGPREFIX + "Finish Dispose");
        }
        /*
        public event EventHandler BrowserCreated;

        internal protected virtual void OnBrowserAfterCreated(CefBrowser browser)
        {
            _browser = browser;
            _browserWindowHandle = _browser.GetHost().GetWindowHandle();
            ResizeWindow(_browserWindowHandle, Width, Height);

            if (BrowserCreated != null)
                BrowserCreated(this, EventArgs.Empty);
        }

        internal protected virtual void OnTitleChanged(TitleChangedEventArgs e)
        {
            Title = e.Title;

            var handler = TitleChanged;
            if (handler != null) handler(this, e);
        }

        public string Title { get; private set; }

        public event EventHandler<TitleChangedEventArgs> TitleChanged;

        internal protected virtual void OnAddressChanged(AddressChangedEventArgs e)
        {
            Address = e.Address;

            var handler = AddressChanged;
            if (handler != null) handler(this, e);
        }

        public string Address { get; private set; }

        public event EventHandler<AddressChangedEventArgs> AddressChanged;

        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;

        internal protected virtual void OnConsoleMessage(ConsoleMessageEventArgs e)
        {
            if (ConsoleMessage != null)
                ConsoleMessage(this, e);
            else
                e.Handled = false;
        } 
        
         
        */


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

		public void InvalidateSize()
		{
            Debug.WriteLine(DBGPREFIX + "InvalidateSize");
			ResizeWindow(Handle, Width, Height);
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



    	public event EventHandler BeforeClose;
        internal protected virtual void OnBeforeClose()
        {
            Debug.WriteLine(DBGPREFIX + "OnBeforeClose");
            //_browserWindowHandle = IntPtr.Zero;
            if (BeforeClose != null)
                BeforeClose(this, EventArgs.Empty);
        }

        /*
        public LVCefLifeSpanHandler LifeSpanHandler
        {
            get { return _lvCefClient.LifeSpanHandler; }
        }
        */
        //expose the OnProcessMessageReceived event directly as it is inside the CefClient and not a handler
        /*
        public EventHandler<LVCefClient.OnProcessMessageReceivedEventArgs> OnProcessMessageReceivedEvent
        {
            set { _lvCefClient.OnProcessMessageReceivedEvent += value;}
        }
        */
        // Functions used internally
        /*
         * Called from LVCefClient.createBrowser
         */
        internal CefWindowInfo getCefWindowInfo()
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

        /*
        * Should be set from the onBrowserCreated callback 
        */
        [Browsable(false)]
        public CefBrowser Browser
        {
            get { return _browser; }
            internal set
            {
                Trace.Assert( value.GetType() == typeof(CefBrowser));
                Debug.WriteLine(DBGPREFIX + "has initialized the CefBrowser");
                _browser = value; //TODO  why does saving this break everything, apparantly not saving breaks less?
                //Paint -= PaintInDesignMode;
                //if(_browser != null)
                //    _browserWindowHandle = _browser.GetHost().GetWindowHandle();
                
                //Accessed from a thread it was not created on?
                ResizeWindow(Handle, Width, Height);

                CefClient = null; //holding onto references makes it crashy, release after browser made
            }
        }


        //Race condition? user calls createBrowser twice very quickly
        //Does CEF check for multiple uses of same handle?
        public void createBrowser()
        {
            if (Browser == null)
            {
                Debug.WriteLine(DBGPREFIX + "createBrowser starting creation of CefBrowser");
                var settings = new CefBrowserSettings { };
                CefBrowserHost.CreateBrowser(getCefWindowInfo(), CefClient, settings, StartUrl);
            }
            else
                Debug.WriteLine(DBGPREFIX + "createBrowser has already created CefBrowser instance for this control, request ignored.");
        }
    }
}
