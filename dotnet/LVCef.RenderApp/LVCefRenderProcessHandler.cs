using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace LVCef.RenderApp
{
    internal class LVCefRenderProcessHandler : CefRenderProcessHandler
    {
        private const string DBGPREFIX = "[LVCefRender]: ";
        private CefMessageRouterRendererSide _messageRouter;

        internal LVCefRenderProcessHandler()
        {
            Debug.WriteLine(DBGPREFIX + "LVCefRenderProcessHandler created");
            _messageRouter = new CefMessageRouterRendererSide(new CefMessageRouterConfig());
        }

        protected override void OnContextCreated(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            Debug.WriteLine(DBGPREFIX + "OnContextCreated Called");
            _messageRouter.OnContextCreated(browser, frame, context);
        }

        protected override void OnContextReleased(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            Debug.WriteLine(DBGPREFIX + "OnContextReleased called");
            _messageRouter.OnContextReleased(browser, frame, context);
        }

        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            Debug.WriteLine(DBGPREFIX + "OnProcessMessageReceived called");
            return _messageRouter.OnProcessMessageReceived(browser, sourceProcess, message);
        }

    }
}
