using System;
using Faust.Base.Global;
using Dalamud.Hooking;
using Faust.Sheets;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.System.Resource.Handle;
using Serilog.Events;

namespace Faust.Base.Detours;

public unsafe class Minionstuffs : IDisposable
{
    private readonly Hook<CompanionContainer.Delegates.SetupCompanion>? _SetupCompanionHook;
    public Minionstuffs()
    {
        _SetupCompanionHook = Service.GameInteropProvider.HookFromAddress<CompanionContainer.Delegates.SetupCompanion>(CompanionContainer.Addresses.SetupCompanion.Value, SetupCompanionDetour);
        
        this._SetupCompanionHook.Enable();
    }
    
    
    private void SetupCompanionDetour(CompanionContainer* thisptr, short companionId, uint param)
    {
        _SetupCompanionHook!.Original(thisptr, companionId, param);
        var companionObject = thisptr->CompanionObject;
        if (companionObject == null) return;
        var companionOwnerObject = companionObject->GetParentCharacter();
        if (companionOwnerObject == null) return;
        //Logging.pluginDevLog(LogEventLevel.Error,"companion triggered! " + companionId);
        if ((companionOwnerObject->GetName().ToString() == Service.PlayerState.CharacterName)
            && (companionOwnerObject->HomeWorld == Service.PlayerState.HomeWorld.RowId)
            && (companionId != 0))
        {
            var companionvalue = CompanionDetourManager.GetReplacement((uint)companionId);
            //Logging.pluginDevLog(LogEventLevel.Error,"companion triggered! " + companionvalue.ModelScale);
            companionObject->Scale = companionvalue.ModelScale / 100;
        }
    }
    public void Dispose()
    {
        this._SetupCompanionHook.Dispose();
        //FFXIVClientStructs.FFXIV.Client.Game.ActionManager.Delegates.
        //Service.AddonLifecycle.UnregisterListener(OnActionBarRequestedUpdate);
        
    }
}
