using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace LVCef.RenderApp
{
    static class Program
    {
        private const string DBGPREFIX = "[LVCefRender]: ";
        [STAThread]
        private static int Main(string[] args)
        {
            try
            {
                Debug.WriteLine(DBGPREFIX + "Loading CefRuntime in Render Process");
                CefRuntime.Load();
            }
            catch (DllNotFoundException ex)
            {
                Debug.WriteLine(DBGPREFIX + ex.ToString() );
                throw;
            }
            catch (CefRuntimeException ex)
            {
                Debug.WriteLine(DBGPREFIX + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DBGPREFIX + ex.ToString());
                throw;
            }

            var mainArgs = new CefMainArgs(args);
            var cefApp = new LVRenderCefApp();
            Debug.WriteLine(DBGPREFIX + "Starting ExecuteProcess");
            var exitCode = CefRuntime.ExecuteProcess(mainArgs, cefApp, IntPtr.Zero);
            if (exitCode == -1)
            {
                Debug.WriteLine(DBGPREFIX + "This process at the following path should not be run directly, specify as target in browser process: " + System.Windows.Forms.Application.ExecutablePath);
                throw new CefRuntimeException("Process should not be run directly, instead specify as target in browser process");
            }
            Debug.WriteLine(DBGPREFIX + "Finishing ExecuteProcess and exiting application");
            return exitCode;
        }
    }
}
