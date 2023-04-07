using CustRes.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for CommandConsoleWindow.xaml
    /// </summary>
    public partial class CommandConsoleWindow : Window
    {
        public CommandConsoleWindow()
        {
            InitializeComponent();
            SetCurrentConsoleWindow(this);
            ConsoleLines = new ObservableCollection<string>(_ConsoleLines);
            ConsoleLines.CollectionChanged += new NotifyCollectionChangedEventHandler((
                object sender, NotifyCollectionChangedEventArgs e) => {
                    _ConsoleLines = ConsoleLines.ToArray();
                });

            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate () {
                    inputTextBox.Focus();
                    inputTextBox.CaretIndex = inputTextBox.Text.Count();
                    Keyboard.Focus(inputTextBox);
                }));

            this.inputTextBox.PreviewKeyDown += InputTextBox_PreviewKeyDown;
        }

        private void InputTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                UpCommand(null, null);
            else if (e.Key == Key.Down)
                DownCommand(null, null);
        }

        public static readonly DependencyProperty ConsoleLinesProperty = DependencyProperty.Register(
            nameof(ConsoleLines), typeof(ObservableCollection<string>), typeof(CommandConsoleWindow),
            new PropertyMetadata());

        public ObservableCollection<string> ConsoleLines
        {
            get { return (ObservableCollection<string>)GetValue(ConsoleLinesProperty); }
            set { SetValue(ConsoleLinesProperty, value); }
        }

        public void EnterCommand(object sender, ExecutedRoutedEventArgs e)
        {
            string input = inputTextBox.Text;
            enteredCommands.Add(input);
            List<string> parts = input.Split(' ').ToList();
            if (parts.Count > 0)
            {
                string command = parts[0];
                parts.RemoveAt(0);
                string text = String.Join(" ", parts);

                ConwayNativityDirectory.PluginApi.ICommand cmd = Commands.RegisteredCommands[command];

                if (cmd == null)
                {
                    CommandConsoleWindow.Log("The command '" + command + "' does not exist.");
                    return;
                }

                if (cmd.RequiresLogin && !App.MainWindow.CanExecuteCommandWithCurrentLogin)
                {
                    CommandConsoleWindow.Log("The command entered requires the user to be logged in.");
                    return;
                }
                if (cmd.RequiresProject && !App.MainWindow.Project.IsOpen)
                {
                    CommandConsoleWindow.Log("The command entered requires a project to be open.");
                    return;
                }

                cmd.Execute(text);
            }

            currentCommand = enteredCommands.Count - 1;
        }

        public static List<string> enteredCommands = new List<string>();
        public static int currentCommand = 0;
        public void UpCommand(object sender, ExecutedRoutedEventArgs e)
        {
            currentCommand = (currentCommand-1).Clamp(0, enteredCommands.Count - 1);
            if (enteredCommands.Count > 0)
                inputTextBox.Text = enteredCommands[currentCommand];
            inputTextBox.CaretIndex = inputTextBox.Text.Length;
        }

        public void DownCommand(object sender, ExecutedRoutedEventArgs e)
        {
            currentCommand = (currentCommand+1).Clamp(0, enteredCommands.Count - 1);
            if (enteredCommands.Count > 0)
                inputTextBox.Text = enteredCommands[currentCommand];
            inputTextBox.CaretIndex = inputTextBox.Text.Length;
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e) => this.Close();

        //Static Stuff
        private static string[] _ConsoleLines = new string[] { };

        private static CommandConsoleWindow CurrentWindow;
        internal static void SetCurrentConsoleWindow(CommandConsoleWindow window) => CurrentWindow = window;

        public static void Log(object value)
        {
            if (value != null && CurrentWindow != null)
            {
                CurrentWindow.ConsoleLines.Add(value.ToString());
                ScrollViewer scrollViewer = CurrentWindow.consoleListView.GetVisualChild<ScrollViewer>();
                if (scrollViewer != null)
                    scrollViewer.ScrollToBottom();
            }
        }

        public static void Clear()
        {
            CurrentWindow.ConsoleLines.Clear();
        }
    }
}
