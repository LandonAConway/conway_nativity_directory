using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Collections.Specialized;

namespace Conway_Nativity_Directory
{
    public partial class App : Application
    {
        public static void LoadLogging()
        {
            startCurrentLog();
            Log("Launch", "Session started. v" + App.Version.ToString());
        }

        public static void Log(string level, string description)
        {
            writeToLog(level + ": " + description);
        }

        public static void Log(string level, string name, string description)
        {
            writeToLog(level + ": " + name + ": " + description);
        }

        public static void Log(Exception exception)
        {
            writeToLog("Exception: [" + exception.GetType().ToString() + "] " +
                exception.Message + ": " + exception.InnerException);
        }

        private static void writeToLog(string data)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\bin\debug.txt";
            if (!File.Exists(path))
                File.WriteAllText(path, String.Empty);

            File.AppendAllText(path, DateTime.Now.ToString() + ": " + data + Environment.NewLine);
        }

        private static void startCurrentLog()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\bin\debug.txt";
            if (!File.Exists(path))
                File.WriteAllText(path, String.Empty);

            string text = Environment.NewLine +
                new string('=', 11) + Environment.NewLine +
                " Separator " + Environment.NewLine +
                new string('=', 11) + Environment.NewLine;

            File.AppendAllText(path, text);
        }

        //startup, exit

        DateTime started;
        public DateTime Started { get { return started; } }

        private static string associatedFileOnStartup;
        public static string AssociatedFileOnStartup { get { return associatedFileOnStartup; } }
        protected override void OnStartup(StartupEventArgs e)
        {
            started = DateTime.Now;

            if (e.Args != null)
                if (e.Args.ElementAtOrDefault(0) != null)
                {
                    if (Path.GetExtension(e.Args[0]) == ".cnb" || Path.GetExtension(e.Args[0]) == ".cnp")
                        associatedFileOnStartup = e.Args[0];
                    else if (Path.GetExtension(e.Args[0]) == ".cndp")
                    {
                        startCurrentLog();
                        Log("Launch", "Session started to configure a plugin. path=\"" + e.Args[0] + "\"");
                        var installInfo = InstallPlugin(e.Args[0]);
                        Log("PluginConfig", "Plugin was successfully " + installInfo["action"] + ".");
                        MessageBox.Show("Plugin was successfully " + installInfo["action"] + ".", "Conway Nativity Directory");
                        Application.Current.Shutdown();
                    }
                }

            LoadTypes();
            LoadLogging();
            this.DispatcherUnhandledException += Application_DispatcherUnhandledException;
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            TimeSpan session = Started.Subtract(DateTime.Now);
            string[] durations = session.ToString(@"h\:m\:s").Split(':');
            string _session = durations[0] + " hours, " +
                durations[1] + " minutes, " +
                durations[2] + " seconds.";

            Log("Exit", "Session ended. Session lasted " + _session);
            base.OnExit(e);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log(e.Exception);

            MessageBox.Show("An exception was thrown and Conway Nativity Directory must exit. See 'debug.txt' for more details." +
                Environment.NewLine + "Exception: [" + e.Exception.GetType().ToString() + "] " +
                e.Exception.Message + ": " + e.Exception.InnerException);

            if (Internet.HasConnection("https://www.cnd-app.conwaynativities.com"))
            {
                MessageBoxResult mbr = MessageBox.Show("Would you like to report this issue before Conway Nativity Directory exits?", "Report Issue",
                    MessageBoxButton.YesNo);

                if (mbr == MessageBoxResult.Yes)
                {
                    IssueReport issueReport = new IssueReport();
                    issueReport.Title = "Software Crash";
                    issueReport.Description = "";
                    issueReport.ExceptionType = e.Exception.GetType().ToString();
                    issueReport.ExceptionMessage = e.Exception.Message;
                    issueReport.Post();
                }
            }

            e.Handled = true;
            Application.Current.Shutdown();
        }
    }
}
