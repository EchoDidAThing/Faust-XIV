using System;
using System.Collections.Generic;
using System.Text;
using Faust;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Faust.Base.Detours;

namespace Faust;

internal class Events
{
    public Events()
    {
        Service.ClientState.ClassJobChanged += ClientState_ClassJobChanged;
    }

    public void ClientState_ClassJobChanged(uint classJobId)
    {
        //Service.Log.Error("job changed to " + classJobId);
        //JobChangeDetour.UpdateIconList(ref JobChangeDetour.CurrentJobIcons);
    }

    public void Dispose()
    {
        Service.ClientState.ClassJobChanged -= ClientState_ClassJobChanged;
    }
   
}
