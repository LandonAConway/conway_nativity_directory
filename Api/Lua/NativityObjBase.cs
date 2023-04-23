using ConwayNativityDirectory.PluginApi.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConwayNativityDirectory.PluginApi.Lua_Sandbox
{
    /// <summary>
    /// Base class for the NativityObj which is exposed to the lua scripting api.
    /// </summary>
    public abstract class NativityObjBase
    {
        internal protected abstract NativityBase NativityBase { get; }
    }

    public static class NativityObjBaseHelpers
    {
        public static NativityBase GetNativityBase(this Lua_Sandbox.NativityObjBase nativityObjBase) => nativityObjBase.NativityBase;
        public static NativityObjBase CreateNativityObj() => PluginDatabase.ConwayNativityDirectory.CreateNativityObjBase();
        public static NativityObjBase CreateNativityObj(NativityBase nativity) => PluginDatabase.ConwayNativityDirectory.GetNativityObjBase(nativity);
    }
}