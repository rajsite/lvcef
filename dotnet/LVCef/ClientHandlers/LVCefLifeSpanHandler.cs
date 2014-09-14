namespace LVCef.ClientHandlers
{
    using Xilium.CefGlue;
    using Xilium.CefGlue.Wrapper;
    using System;
    using LVCef.Control;
    using System.Diagnostics;

    public sealed class LVCefLifeSpanHandler : CefLifeSpanHandler
    {
        private const string DBGPREFIX = "[LVCef][LVCefLifeSpanHandler]: ";
        private LVCefControl _lvCefControl;
        private CefMessageRouterBrowserSide _messageRouter;

        internal LVCefLifeSpanHandler(LVCefControl lvCefControl, CefMessageRouterBrowserSide messageRouter)
        {
            Debug.WriteLine(DBGPREFIX + "Created");
            _lvCefControl = lvCefControl;
            _messageRouter = messageRouter;
        }

        
        protected override bool DoClose(CefBrowser browser)
        {
            Debug.WriteLine(DBGPREFIX + "DoClose");
            // Returning false will send a top level browser close event? guess that is why the labview window closed?
            // Sure does, need to return true and figure out more about the workflow
            return true;
        }

        protected override void OnBeforeClose(CefBrowser browser)
        {
            Debug.WriteLine(DBGPREFIX + "OnBeforeClose - Safe to exit after OnBeforeClose called for each Browser instance");
            _messageRouter.OnBeforeClose(browser);
        }
        
        protected override void OnAfterCreated(CefBrowser browser)
        {
            Debug.WriteLine(DBGPREFIX + "OnAfterCreated");
            //Need to be called on ui thread?
            //yes
            _lvCefControl.InvokeIfRequired(() =>
            {
                _lvCefControl.Browser = browser;
            });

	        var handler = OnAfterCreatedEvent;
	        if (handler != null)
	        {
                Debug.WriteLine(DBGPREFIX + "OnAfterCreated Delegate");
		        var e = new OnAfterCreatedEventArgs(browser);
		        handler(this, e);
	        }
        }
        #region OnAfterCreated Scaffolding
        public event EventHandler<OnAfterCreatedEventArgs> OnAfterCreatedEvent;
        public sealed class OnAfterCreatedEventArgs : EventArgs
        {
            public readonly CefBrowser browser;
            internal OnAfterCreatedEventArgs(CefBrowser _browser)
            {
                browser = _browser;
            }
        }
        #endregion
        

    }
}