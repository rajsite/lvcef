namespace LVCef.ClientHandlers
{
    using System;
    using System.Diagnostics;
    using Xilium.CefGlue;
    using Xilium.CefGlue.Wrapper;
    using LVCef.Control;

    public sealed class LVCefMessageRouterHandler : CefMessageRouterBrowserSide.Handler
    {
        private const string DBGPREFIX = "[LVCef][LVCefMessageRouterHandler]: ";
        private LVCefControl _lvCefControl;
        private CefMessageRouterBrowserSide _messageRouter;

        internal LVCefMessageRouterHandler(LVCefControl lvCefControl, CefMessageRouterBrowserSide messageRouter)
        {
            Debug.WriteLine(DBGPREFIX + "Created");
            _lvCefControl = lvCefControl;
            _messageRouter = messageRouter;
        }

        public override bool OnQuery(CefBrowser browser, CefFrame frame, long queryId, string request, bool persistent, CefMessageRouterBrowserSide.Callback callback)
        {
            Debug.WriteLine(DBGPREFIX + "OnQuery called, [" + queryId + " " + (persistent ? "" : "not" + " persistent]: ") + request);
	        var handler = OnQueryEvent;
	        if (handler != null)
	        {
                Debug.WriteLine(DBGPREFIX + "OnQuery Delegate");
		        var e = new OnQueryEventArgs(browser, frame, queryId, request, persistent, callback);
		        handler(this, e);
                return e.Handled;
	        }
            return false;
        }
        #region OnQuery Scaffolding
        public event EventHandler<OnQueryEventArgs> OnQueryEvent;
        public sealed class OnQueryEventArgs : EventArgs
        {
            public readonly CefBrowser browser;
            public readonly CefFrame frame;
            public readonly long queryId;
            public readonly string request;
            public readonly bool persistent;
            public readonly CefMessageRouterBrowserSide.Callback callback;
            public bool Handled { get; set; }

            internal OnQueryEventArgs(CefBrowser _browser, CefFrame _frame, long _queryId, string _request, bool _persistent, CefMessageRouterBrowserSide.Callback _callback)
            {
                browser = _browser;
                frame = _frame;
                queryId = _queryId;
                request = _request;
                persistent = _persistent;
                callback = _callback;
                Handled = false; //default return value
            }
        }
        #endregion
    }
}