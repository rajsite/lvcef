namespace LVCef.AppHandlers
{
    using Xilium.CefGlue;
    using System;
    using LVCef.Control;
    using System.Diagnostics;

    public sealed class LVCefBrowserProcessHandler : CefBrowserProcessHandler
    {
        private const string DBGPREFIX = "[LVCef][LVCefBrowserProcessHandler]: ";

        internal LVCefBrowserProcessHandler()
        {
            Debug.WriteLine(DBGPREFIX + "Creating");
        }

        protected override void OnBeforeChildProcessLaunch(CefCommandLine commandLine)
        {
            Debug.WriteLine(DBGPREFIX + "OnBeforeChildProcessLaunch");
            var handler = OnBeforeChildProcessLaunchEvent;
            if (handler != null)
            {
                Debug.WriteLine(DBGPREFIX + "OnBeforeChildProcessLaunch Delegate");
                var e = new OnBeforeChildProcessLaunchEventArgs(commandLine);
                handler(this, e);
            }
        }
        #region OnBeforeChildProcessLaunch Scaffolding
        public event EventHandler<OnBeforeChildProcessLaunchEventArgs> OnBeforeChildProcessLaunchEvent;
        public sealed class OnBeforeChildProcessLaunchEventArgs : EventArgs
        {
            public readonly CefCommandLine commandLine;
            internal OnBeforeChildProcessLaunchEventArgs(CefCommandLine _commandLine)
            {
                commandLine = _commandLine;
            }
        }
        #endregion

    }
}