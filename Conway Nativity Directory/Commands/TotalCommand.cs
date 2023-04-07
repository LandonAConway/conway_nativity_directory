using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ConwayNativityDirectory.PluginApi;

namespace Conway_Nativity_Directory
{
    public class TotalCommand : ICommand
    {
        public bool RequiresProject => true;
        public bool RequiresLogin => true;
        public string Command => @"total";
        public string Title => "Total";
        public string Description => "Shows the estimated total dollar value of each nativity in the view.";
        public string Params => "";

        public void Execute(string text)
        {
            MainWindow mainWindow = App.MainWindow;

            double amount = 0;
            foreach (Nativity nativity in mainWindow.nativityListView.Items)
                amount += nativity.Cost;

            CommandConsoleWindow.Log("The total cost of the nativities shown on screen is estimated to be " + amount.ToString("C2"));
        }
    }
}
