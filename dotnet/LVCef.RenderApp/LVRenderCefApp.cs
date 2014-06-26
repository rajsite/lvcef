using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace LVCef.RenderApp
{
    class LVRenderCefApp : CefApp
    {
        private const string DBGPREFIX = "[LVCefRender]: ";
        private LVCefRenderProcessHandler renderProcessHandler = null;

        public LVRenderCefApp()
        {
            Debug.WriteLine(DBGPREFIX + "LVRenderCefApp created");
            renderProcessHandler = new LVCefRenderProcessHandler();
        }

        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            Debug.WriteLine(DBGPREFIX + "GetRenderProcessHandler called");
            return renderProcessHandler;
        }

    }
}
