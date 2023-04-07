using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;
using ConwayNativityDirectory.PluginApi;
using Conway_Nativity_Directory.Lua_Sandbox;
using Conway_Nativity_Directory.Lua_Helpers;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Conway_Nativity_Directory
{
    public static class LuaScripting
    {
        public static void Load()
        {
            engine = new Lua();
            LoadApi();
            LoadScripts();
        }

        private static Lua engine;
        public static Lua Engine
        {
            get { return engine; }
        }

        private static void LoadApi()
        {
            engine.State.Encoding = Encoding.UTF8;

            engine.LoadCLRPackage();
            engine.DoString(@"import('Conway Nativity Directory', 'Conway_Nativity_Directory.Lua_Sandbox')
                import('System.IO')
                cnd = {
                    _t = {},
                    keyboard = {},
                    http = {}
                }
            ");

            foreach (string path in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\bin\lua\builtin", "*.lua"))
                engine.DoFile(path);

            engine.RegisterFunction("print", typeof(CommandConsole).GetMethod("Log"));
            engine.RegisterFunction("clear", typeof(CommandConsole).GetMethod("Clear"));
            engine.RegisterFunction("message", typeof(GeneralEnvironment).GetMethod("Message"));
            engine.RegisterFunction("input", typeof(GeneralEnvironment).GetMethod("Input"));
            engine.RegisterFunction("cnd.is_project_open", typeof(Cnd).GetMethod("IsProjectOpen"));
            engine.RegisterFunction("cnd.get_nativities", typeof(Cnd).GetMethod("GetNativities"));
            engine.RegisterFunction("cnd.get_selected_nativities", typeof(Cnd).GetMethod("GetSelectedNativities"));
            engine.RegisterFunction("cnd.get_selected_nativity", typeof(Cnd).GetMethod("GetSelectedNativity"));
            engine.RegisterFunction("cnd.add_nativity", typeof(Cnd).GetMethod("AddNativity"));
            engine.RegisterFunction("cnd.remove_nativity", typeof(Cnd).GetMethod("RemoveNativity"));
            engine.RegisterFunction("cnd.keyboard.is_key_down", typeof(_Keyboard).GetMethod("IsKeyDown"));
            engine.RegisterFunction("cnd.keyboard.is_key_up", typeof(_Keyboard).GetMethod("IsKeyUp"));
            engine.RegisterFunction("cnd.keyboard.is_key_toggled", typeof(_Keyboard).GetMethod("IsKeyToggled"));

            //Get rid of unsafe stuff
            engine.DoString(@"import = function() end");
        }

        private static void LoadScripts()
        {
            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\bin\lua\scripts", "*.lua");
            if (files.Length > 0)
                App.MainWindow.scriptsMenuItem.Items.Add(new System.Windows.Controls.Separator());
            foreach (string fileName in files)
            {
                var scriptItem = new ScriptMenuItem();
                scriptItem.FileName = fileName;
                App.MainWindow.scriptsMenuItem.Items.Add(scriptItem);
            }
        }

        public static dynamic CreateTable() => Engine.DoString("return {}")[0];
    }
}

namespace Conway_Nativity_Directory.Lua_Helpers
{
    public static class Cnd
    {
        public static bool IsProjectOpen() => App.Project.IsOpen;

        public static object GetNativities()
        {
            LuaScripting.Engine.DoString(@"cnd._t.nativities = {}");
            if (App.Project.IsOpen)
            {
                foreach (var nativity in App.Project.Nativities.Cast<Nativity>())
                {
                    LuaScripting.Engine["cnd._t.nativity"] = new NativityObj(nativity);
                    LuaScripting.Engine.DoString("table.insert(cnd._t.nativities, cnd._t.nativity) \n cnd._t.nativity = nil");
                }
                return LuaScripting.Engine.DoString("local nativities = cnd._t.nativities \n cnd._t.nativities = nil \n return nativities")[0];
            }
            return null;
        }

        public static object GetSelectedNativities()
        {
            LuaScripting.Engine.DoString(@"cnd._t.nativities = {}");
            if (App.Project.IsOpen)
            {
                foreach (var nativity in App.Project.SelectedNativities.Cast<Nativity>())
                {
                    LuaScripting.Engine["cnd._t.nativity"] = new NativityObj(nativity);
                    LuaScripting.Engine.DoString("table.insert(cnd._t.nativities, cnd._t.nativity) \n cnd._t.nativity = nil");
                }
                return LuaScripting.Engine.DoString("local nativities = cnd._t.nativities \n cnd._t.nativities = nil \n return nativities")[0];
            }
            return null;
        }

        public static object GetSelectedNativity()
        {
            if (App.Project.IsOpen)
                return new NativityObj((Nativity)App.Project.SelectedNativity);
            return null;
        }

        public static void AddNativity(NativityObj nativityObj)
        {
            if (App.Project.IsOpen && nativityObj != null)
            {
                if (App.Project.Nativities.Contains(nativityObj.nativity))
                    throw new Exception("Cannot add a nativity that is already in the collection.");
                App.Project.Nativities.Add(nativityObj.nativity);
            }
        }

        public static bool RemoveNativity(NativityObj nativityObj)
        {
            if (App.Project.IsOpen && nativityObj != null)
                return App.Project.Nativities.Remove(nativityObj.nativity);
            return false;
        }

        //Web api

        //public static object Fetch(object def)
        //{
        //    if (def != null)
        //    {
        //        dynamic _def = def;
        //        H
        //    }
        //    return null;
        //}
    }

    public static class _Keyboard
    {
        public static bool IsKeyDown(string key)
        {
            Key _key = Key.None;
            bool result = Enum.TryParse<Key>(key, out _key);
            if (result)
                return Keyboard.IsKeyDown(_key);
            return false;
        }

        public static bool IsKeyUp(string key)
        {
            Key _key = Key.None;
            bool result = Enum.TryParse<Key>(key, out _key);
            if (result)
                return Keyboard.IsKeyUp(_key);
            return false;
        }

        public static bool IsKeyToggled(string key)
        {
            Key _key = Key.None;
            bool result = Enum.TryParse<Key>(key, out _key);
            if (result)
                return Keyboard.IsKeyToggled(_key);
            return false;
        }
    }

    public static class GeneralEnvironment
    {
        public static object Message(object message, object caption = null, string buttons = null, string image = null)
        {
            if (message == null)
                message = "";
            if (caption == null)
                caption = "";
            if (buttons == null)
                buttons = "OK";
            if (image == null)
                image = "None";

            return MessageBox.Show(message.ToString(), caption.ToString(),
                (MessageBoxButton)Enum.Parse(typeof(MessageBoxButton), buttons),
                (MessageBoxImage)Enum.Parse(typeof(MessageBoxImage), image)).ToString();
        }

        public static string Input(object defaultResponse = null, object title = null)
        {
            InputDialog input = new InputDialog();
            if (defaultResponse != null)
            {
                input.ResponseTextBox.Text = defaultResponse.ToString();
                input.ResponseTextBox.CaretIndex = input.ResponseTextBox.Text.Length;
            }
            if (title != null)
                input.Title = title.ToString();
            if (input.ShowDialog() == true)
                return input.Response;
            return null;
        }
    }
}
