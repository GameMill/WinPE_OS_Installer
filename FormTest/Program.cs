using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new Loading());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            reportError(((Exception)e.ExceptionObject));
            Application.Exit();
        }

        private static void reportError(Exception exceptionObject)
        {
            ((Form)Activator.CreateInstance(Loading.Error, new object[] { exceptionObject} )).ShowDialog();

            // type.InvokeMember("Output", BindingFlags.InvokeMethod, null, Activator.CreateInstance(Loading.Error), new object[] { @"Hello" });

            // (new Error(exceptionObject)).ShowDialog();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            reportError(e.Exception);
            Application.Exit();

        }
    }
}
