namespace LVCef.ClientHandlers
{
    using System;
    using System.Diagnostics;
    using Xilium.CefGlue;
    using Xilium.CefGlue.Wrapper;
    using LVCef.Control;

    public sealed class LVCefRequestHandler : CefRequestHandler
    {
        private const string DBGPREFIX = "[LVCef][LVCefRequestHandler]: ";
        private LVCefControl _lvCefControl;
        private CefMessageRouterBrowserSide _messageRouter;

        internal LVCefRequestHandler(LVCefControl lvCefControl, CefMessageRouterBrowserSide messageRouter)
        {
            Debug.WriteLine(DBGPREFIX + "Constructor");
            _lvCefControl = lvCefControl;
            _messageRouter = messageRouter;
        }

        protected override void OnRenderProcessTerminated(CefBrowser browser, CefTerminationStatus status)
        {
            Debug.WriteLine(DBGPREFIX + "OnRenderProcessTerminated");
            _messageRouter.OnRenderProcessTerminated(browser);
        }

        protected override bool OnBeforeBrowse(CefBrowser browser, CefFrame frame, CefRequest request, bool isRedirect)
        {
            Debug.WriteLine(DBGPREFIX + "OnBeforeBrowse");
            _messageRouter.OnBeforeBrowse(browser, frame);
            return false;
        }

        protected override CefResourceHandler GetResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request)
        {
            Debug.WriteLine(DBGPREFIX + "OnGetResourceHandler called for URL: " + request.Url);
	        var handler = OnGetResourceHandlerEvent;
	        if (handler != null)
	        {
                LVCefRequest lvCefRequest = new LVCefRequest(OnProcessRequestEvent, OnCancelEvent);
                Debug.WriteLine(DBGPREFIX + "OnGetResourceHandler for URL: " + request.Url + " assigned id " + lvCefRequest.id);
		        var e = new OnGetResourceHandlerEventArgs(lvCefRequest.id, browser, frame, request);
		        handler(this, e);

                if (e.DelegateRequest)
                {
                    Debug.WriteLine(DBGPREFIX + "OnGetResourceHandler for id " + lvCefRequest.id + " to be delegated");
                    return lvCefRequest;
                }
	        }
            Debug.WriteLine(DBGPREFIX + "OnGetResourceHandler for URL: " + request.Url + " to be handled normally by CEF");
            return null;
        }
        #region OnGetResourceHandler Scaffolding
        public event EventHandler<OnGetResourceHandlerEventArgs> OnGetResourceHandlerEvent;
        public sealed class OnGetResourceHandlerEventArgs : EventArgs
        {
            public readonly string id;
            public readonly CefBrowser browser;
            public readonly CefFrame frame;
            public readonly CefRequest request;
            public bool DelegateRequest { get; set; }

            public OnGetResourceHandlerEventArgs(string _id, CefBrowser _browser, CefFrame _frame, CefRequest _request)
            {
                id = _id;
                browser = _browser;
                frame = _frame;
                request = _request;
                DelegateRequest = false; //Default do not delegate, allow CEF to handle using network
            }
        }
        #endregion

        #region OnProcessRequest Scaffolding
        public event EventHandler<OnProcessRequestEventArgs> OnProcessRequestEvent;
        public sealed class OnProcessRequestEventArgs : EventArgs
        {
            public readonly string id;
            public readonly CefRequest request;
            public readonly CefCallback callback;
            public bool continueRequest { get; set; }
            public OnProcessRequestEventArgs(string _id, CefRequest _request, CefCallback _callback)
            {
                id = _id;
                request = _request;
                callback = _callback;
                continueRequest = false; //do not continue by default 
            }
        }
        #endregion

        #region OnCancel Scaffolding
        public event EventHandler<OnCancelEventArgs> OnCancelEvent;
        public sealed class OnCancelEventArgs : EventArgs
        {
            public readonly string id;
            public OnCancelEventArgs(string _id)
            {
                id = _id;
            }
        }
        #endregion


        /**
         * An instance of LVCefRequest is created by the library with each request. The instance creates a UUID for the request
         * and copies the delegates from LVCefRequestHandler. Each delegate should pass the UUID so the host application can track
         * the request during its lifetime (multiple requests possible or request functions interleaved, etc.)
         */
        private sealed class LVCefRequest : CefResourceHandler
        {
            private const string DBGPREFIX = "[LVCef][LVCefRequestHandler][LVCefRequest]: ";
            public readonly string id = null;
            private event EventHandler<OnProcessRequestEventArgs> OnProcessRequestEvent;
            private event EventHandler<OnCancelEventArgs> OnCancelEvent;

            public LVCefRequest(EventHandler<OnProcessRequestEventArgs> _OnProcessRequestEvent,
                                EventHandler<OnCancelEventArgs> _OnCancelEvent)
            {
                Debug.WriteLine(DBGPREFIX + "Constructor");
                id = System.Guid.NewGuid().ToString();
                OnProcessRequestEvent = _OnProcessRequestEvent;
                OnCancelEvent = _OnCancelEvent;
            }

            protected override bool ProcessRequest(CefRequest request, CefCallback callback)
            {
                Debug.WriteLine(DBGPREFIX + "ProcessRequest for id: " + id);
                var handler = OnProcessRequestEvent;
	            if (handler != null)
	            {
		            var e = new OnProcessRequestEventArgs(id, request, callback);
		            handler(null, e);
                    Debug.WriteLine(DBGPREFIX + "ProcessRequest for id: " + id + (e.continueRequest ? " will continue processing" : " will be cancelled") );
                    return e.continueRequest;
	            }
                return false;
            }

            protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
            {
                throw new NotImplementedException();
            }

            protected override bool ReadResponse(System.IO.Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
            {
                throw new NotImplementedException();
            }

            protected override bool CanGetCookie(CefCookie cookie)
            {
                throw new NotImplementedException();
            }

            protected override bool CanSetCookie(CefCookie cookie)
            {
                throw new NotImplementedException();
            }

            protected override void Cancel()
            {
                Debug.WriteLine(DBGPREFIX + "Request cancelled for id: " + id);
               	var handler = OnCancelEvent;
	            if (handler != null)
	            {
		            var e = new OnCancelEventArgs(id);
		            handler(null, e);
	            }
            }
        }
        
    }
}