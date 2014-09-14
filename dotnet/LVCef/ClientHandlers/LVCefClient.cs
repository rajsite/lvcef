namespace LVCef.ClientHandlers
{
    using System;
    using System.Collections.Generic;
    using Xilium.CefGlue;
    using Xilium.CefGlue.Wrapper;
    using LVCef.Control;
    using System.Diagnostics;

    public class LVCefClient : CefClient
    {
        private const string DBGPREFIX = "[LVCef][LVCefClient]: ";
        //private LVCefControl _myLVCefControl;
        //private CefWebLifeSpanHandler _lifeSpanHandler; //need
        //private CefWebDisplayHandler _displayHandler;
        //private CefWebLoadHandler _loadHandler;
        //private CefWebRequestHandler _requestHandler; //need
        //renderprocesshandler
        //browserprocesshandler

        private CefMessageRouterBrowserSide _messageRouter = null;

        private readonly LVCefControl _lvCefControl;

        internal LVCefClient(LVCefControl lvCefControl)
        {
            Debug.WriteLine(DBGPREFIX + "Created");
            if (lvCefControl == null)
                throw new CefRuntimeException(DBGPREFIX + "Instance of a LVCefControl required to create an LVCefClient");
            _lvCefControl = lvCefControl;
            _messageRouter = new CefMessageRouterBrowserSide(new CefMessageRouterConfig());

            LifeSpanHandler = new LVCefLifeSpanHandler(_lvCefControl, _messageRouter);
            RequestHandler = new LVCefRequestHandler(_lvCefControl, _messageRouter);
            MessageRouterHandler = new LVCefMessageRouterHandler(_lvCefControl, _messageRouter);
            RegisterMessageRouter();
        }

        /*
         * I think it is possible to RegisterMessageRouter prior to an event being added but after the other handler objects are made
         * The MessageRouterHandler will do a lookup of registered events every time
         **/
        private void RegisterMessageRouter()
        {
            Debug.WriteLine(DBGPREFIX + "Message Router, attempting registration");
            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                Debug.WriteLine(DBGPREFIX + "Message Router, creating task on UI Thread");
                CefRuntime.PostTask(CefThreadId.UI, new ActionTask(RegisterMessageRouter));
                return;
            }
            // window.cefQuery({ request: 'my_request', onSuccess: function(response) { console.log(response); }, onFailure: function(err,msg) { console.log(err, msg); } });
            _messageRouter.AddHandler(MessageRouterHandler);
            Debug.WriteLine(DBGPREFIX + "Message Router, registered");
        }
        private sealed class ActionTask : CefTask
        {
            public Action _action;
            public ActionTask(Action action) { _action = action; }
            protected override void Execute() { _action(); _action = null; }
        }

        public LVCefLifeSpanHandler LifeSpanHandler {get; private set;}
        protected override CefLifeSpanHandler GetLifeSpanHandler()
        {
            return LifeSpanHandler;
        }

        public LVCefRequestHandler RequestHandler { get; private set; }
        protected override CefRequestHandler GetRequestHandler()
        {
            return RequestHandler;
        }

        public LVCefMessageRouterHandler MessageRouterHandler { get; private set; }
        
        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            Debug.WriteLine(DBGPREFIX + "OnProcessMessageReceived: " + message.ToString());
            var isHandled = _messageRouter.OnProcessMessageReceived(browser, sourceProcess, message);
            if (isHandled)
                return true;

	        var handler = OnProcessMessageReceivedEvent;
            if (handler != null)
            {
                Debug.WriteLine(DBGPREFIX + "OnProcessMessageReceived Delegate");
                var e = new OnProcessMessageReceivedEventArgs(browser, sourceProcess, message);
                handler(this, e);
                return e.Handled;
            }
            return false;
        }
        #region OnProcessMessageReceived Scaffolding
        public event EventHandler<OnProcessMessageReceivedEventArgs> OnProcessMessageReceivedEvent;
        public sealed class OnProcessMessageReceivedEventArgs : EventArgs
        {
            public readonly CefBrowser browser;
            public readonly CefProcessId sourceProcess;
            public readonly CefProcessMessage message;
            public bool Handled { get; set; }
            internal OnProcessMessageReceivedEventArgs(CefBrowser _browser, CefProcessId _sourceProcess, CefProcessMessage _message)
	        {
                browser = _browser;
                sourceProcess = _sourceProcess;
                message = _message;
                Handled = false; //default return value
	        }
        }
        #endregion
        
    }
}
