namespace LVCef.ClientHandlers
{
    using System;
    using System.Diagnostics;
    using Xilium.CefGlue;
    using Xilium.CefGlue.Wrapper;
    using LVCef.Control;

    public sealed class LVCefRequestHandler : CefRequestHandler
    {
        private LVCefControl _lvCefControl;
        private CefMessageRouterBrowserSide _messageRouter;

        internal LVCefRequestHandler(LVCefControl lvCefControl, CefMessageRouterBrowserSide messageRouter)
        {
            Debug.WriteLine("Creating the LVCefRequestHandler");
            _lvCefControl = lvCefControl;
            _messageRouter = messageRouter;
        }

        protected override void OnRenderProcessTerminated(CefBrowser browser, CefTerminationStatus status)
        {
            Debug.WriteLine("Calling OnRenderProcessTerminated");
            _messageRouter.OnRenderProcessTerminated(browser);
        }

        protected override bool OnBeforeBrowse(CefBrowser browser, CefFrame frame, CefRequest request, bool isRedirect)
        {
            Debug.WriteLine("Calling OnBeforeBrowse");
            _messageRouter.OnBeforeBrowse(browser, frame);
            return false;
        }
    }
}