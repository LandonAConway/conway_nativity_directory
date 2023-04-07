using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Conway_Nativity_Directory
{
    public static class AutoSaving
    {
        static AutoSavePreference preference;
        static readonly DispatcherTimer timer = new DispatcherTimer();
        public static void Load()
        {
            preference = (AutoSavePreference)App.Preferences.GetPreference(@"Optimization\Auto-Save");
            timer.Interval = TimeSpan.FromSeconds(0.25);
            timer.Tick += Timer_Tick;
            end = DateTime.Now + TimeSpan.FromMinutes(preference.AutoSaveIncrement);
        }

        public static void StartTimer()
        {
            timer.Stop();
            if (preference.AutoSaveEnabled)
                timer.Start();
        }

        public static void StopTimer()
        {
            timer.Stop();
        }


        static DateTime end;
        private static void Timer_Tick(object sender, EventArgs e)
        {
            if (end <= DateTime.Now)
            {
                timer.Stop();
                App.Project.AutoSave(true);
                end = DateTime.Now + TimeSpan.FromMinutes(preference.AutoSaveIncrement);
                timer.Start();
            }
        }
    }
}
