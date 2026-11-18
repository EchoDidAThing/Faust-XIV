using Faust;
using Faust.Base;
using Faust.Base.Detours;
using Faust.Base.Global;
using Faust.Base.Structs;
using Faust.Sheets;
using Dalamud.Game;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Game.Agent;
using Dalamud.Game.Gui;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Enums;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Sound;
using FFXIVClientStructs.FFXIV.Client.System.Resource.Handle;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Common.Lua;
using FFXIVClientStructs.FFXIV.Component.Exd;
using FFXIVClientStructs.FFXIV.Component.GUI;
using InteropGenerator.Runtime;
using Lumina.Data.Parsing.Scd;
using Lumina.Text;
using Lumina.Text.Payloads;
using Lumina.Text.ReadOnly;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using Lumina.Excel.Sheets;
using Serilog.Events;
using static FFXIVClientStructs.FFXIV.Client.UI.AddonRelicNoteBook;
using static FFXIVClientStructs.FFXIV.Client.UI.Agent.AgentActionDetail.Delegates;
using static FFXIVClientStructs.FFXIV.Client.UI.ListPanel.Delegates;
using static Lumina.Data.Parsing.Uld.NodeData;
using static System.Net.Mime.MediaTypeNames;
using Companion = FFXIVClientStructs.FFXIV.Client.Game.Character.Companion;
using CompanionMove = FFXIVClientStructs.FFXIV.Client.Game.Character.CompanionMove;

namespace ActionTimelineReplace.Base.Detours;


public unsafe class Characterstuffs : IDisposable
{
    private delegate char WalkAnimSpeedDelegate(TimelineContainer* thisptr, float* a1);
    private delegate char RunAnimSpeedDelegate(TimelineContainer* thisptr, float* a1);

    private readonly Hook<WalkAnimSpeedDelegate>? _WalkAnimSpeedHook;
    private readonly Hook<RunAnimSpeedDelegate>? _RunAnimSpeedHook;
    
    
    private delegate Companion* companionPropsDelegate(GameObject* thisptr, int companiontype, uint companionID);
    private readonly Hook<companionPropsDelegate>? _companionPropsHook;

    public Characterstuffs()
    {
        _WalkAnimSpeedHook = Service.GameInteropProvider.HookFromSignature<WalkAnimSpeedDelegate>(Hooks.walkanimspeedhook, WalkAnimSpeedDetour);
        _RunAnimSpeedHook = Service.GameInteropProvider.HookFromSignature<RunAnimSpeedDelegate>(Hooks.runanimspeedhook, RunAnimSpeedDetour);
        
        this._WalkAnimSpeedHook.Enable();
        this._RunAnimSpeedHook.Enable();
        
        _companionPropsHook = Service.GameInteropProvider.HookFromSignature<companionPropsDelegate>(Hooks.companionminionprops, companionPropsDetour);
        
        
        this._companionPropsHook.Enable();

    }
    private char WalkAnimSpeedDetour(TimelineContainer* thisptr, float* a1)
    {
        //Logging.pluginDevLog(LogEventLevel.Warning,"ownerkind is " + thisptr->OwnerObject->ObjectKind);
        char tempout = _WalkAnimSpeedHook!.Original(thisptr, a1);
        var ownerobject = thisptr->OwnerObject;
        if (ownerobject == null) return tempout;
        if ((ownerobject->ObjectKind == ObjectKind.Mount) /*|| (ownerobject->ObjectKind == ObjectKind.Companion)*/)
        {
            var ownerownerobject = ownerobject->GetParentCharacter();
            if (ownerownerobject == null) return tempout;
            if (ownerobject->ObjectKind == ObjectKind.Mount)
            {
                if ((ownerownerobject->GetName().ToString() == Service.PlayerState.CharacterName)
                    && (ownerownerobject->HomeWorld == Service.PlayerState.HomeWorld.RowId)
                    && (ownerownerobject->IsMounted() == true) && (ownerownerobject->Mount.MountId != 0))
                {
                    var mountvalue = MountDetourManager.GetReplacement(ownerownerobject->Mount.MountId);
                    //Logging.pluginDevLog(LogEventLevel.Warning,"ownerkind is " + mountvalue.WalkAnimationSpeed);
                    *a1 = mountvalue.WalkAnimationSpeed;
                }
            }
            //Logging.pluginDevLog(LogEventLevel.Warning,"baseid is " + ownerobject->BaseId);
            /*if (ownerobject->ObjectKind == ObjectKind.Companion)
            {
                if ((ownerownerobject->GetName().ToString() == Service.PlayerState.CharacterName)
                    && (ownerownerobject->HomeWorld == Service.PlayerState.HomeWorld.RowId)
                    && (ownerobject->BaseId != 0))
                {
                    var companionvalue = CompanionDetourManager.GetReplacement(ownerobject->BaseId);
                    *a1 = companionvalue.WalkAnimationSpeed;
                    //Logging.pluginDevLog(LogEventLevel.Warning,"walkspeed is " + *a1);

                }
            }*/
        }
        //What are these, and how could one access them?
        //a1[1] = 0;
        //a1[2] = 0;
        //Logging.pluginDevLog(LogEventLevel.Warning,"overallspeed is " + thisptr->OverallSpeed);
        return tempout;
    }

    private char RunAnimSpeedDetour(TimelineContainer* thisptr, float* a1)
    {
        //Logging.pluginDevLog(LogEventLevel.Warning,"ownerkind is " + thisptr->OwnerObject->ObjectKind);
        char tempout = _RunAnimSpeedHook!.Original(thisptr, a1);
        var ownerobject = thisptr->OwnerObject;
        if (ownerobject == null) return tempout;
        if ((ownerobject->ObjectKind == ObjectKind.Mount) /*|| (ownerobject->ObjectKind == ObjectKind.Companion)*/)
        {
            var ownerownerobject = ownerobject->GetParentCharacter();
            if (ownerownerobject == null) return tempout;
            if (ownerobject->ObjectKind == ObjectKind.Mount)
            {
                if ((ownerownerobject->GetName().ToString() == Service.PlayerState.CharacterName)
                    && (ownerownerobject->HomeWorld == Service.PlayerState.HomeWorld.RowId)
                    && (ownerownerobject->IsMounted() == true) && (ownerownerobject->Mount.MountId != 0))
                {
                    var mountvalue = MountDetourManager.GetReplacement(ownerownerobject->Mount.MountId);
                    //Logging.pluginDevLog(LogEventLevel.Warning,"ownerkind is " + mountvalue.WalkAnimationSpeed);
                    *a1 = mountvalue.RunAnimationSpeed;
                }
            }
            /*if (ownerobject->ObjectKind == ObjectKind.Companion)
            {
                //Logging.pluginDevLog(LogEventLevel.Warning,"runspeed is " + ownerownerobject->HomeWorld);
                if ((ownerownerobject->GetName().ToString() == Service.PlayerState.CharacterName)
                    && (ownerownerobject->HomeWorld == Service.PlayerState.HomeWorld.RowId)
                    && (ownerobject->BaseId != 0))
                {
                    var companionvalue = CompanionDetourManager.GetReplacement(ownerobject->BaseId);
                    *a1 = companionvalue.RunAnimationSpeed;
                    Logging.pluginDevLog(LogEventLevel.Warning,"runspeed is " + *a1);
                }
            }*/
        }
        //What are these, and how could one access them?
        //a1[1] = 0;
        //a1[2] = 0;
        //Logging.pluginDevLog(LogEventLevel.Warning,"overallspeed is " + thisptr->OverallSpeed);
        return tempout;
    }
    
    
    private Companion* companionPropsDetour(GameObject* thisptr, int companiontype, uint companionID)
    {
        //Logging.pluginDevLog(LogEventLevel.Warning,"ownerkind is " + thisptr->OwnerObject->ObjectKind);
        Companion* tempout = _companionPropsHook!.Original(thisptr, companiontype, companionID);
        Companion* companionobject = (Companion*)thisptr;
        if (companionobject==null) return tempout;
        var ownerobject = companionobject->GetParentCharacter();
        if (ownerobject == null) return tempout;
        //type 1 = companion 2 = followmount
        if ((ownerobject->GetName().ToString() == Service.PlayerState.CharacterName)
            && (ownerobject->HomeWorld == Service.PlayerState.HomeWorld.RowId)
            && (companionID != 0))
        {
            if ((companiontype == 1))
            {
                var thiscompanion = CompanionDetourManager.GetReplacement(companionID);
                companionobject->InactiveIdle[0] = thiscompanion.InactiveIdle0;
                companionobject->InactiveIdle[1] = thiscompanion.InactiveIdle1;
                companionobject->InactiveBattle = thiscompanion.InactiveBattle;
                companionobject->InactiveWandering = thiscompanion.InactiveWandering;
                companionobject->Behavior = (CompanionMove)thiscompanion.Behaviour;
                companionobject->Special = thiscompanion.Special;
                //unknown10
                //unknown11
                companionobject->WanderingWait = thiscompanion.WanderingWait;
                //companionobject->Priority = thiscompanion.Priority;
                //unknown5
                //unknown6
                //unknown7
                //unknown8
                //unknown9
                companionobject->Enemy = thiscompanion.Enemy;
                companionobject->IsBattle = thiscompanion.IsBattle;
            }
        }

        return tempout;
    }
    
    public void Dispose()
    {
        // When we're done, disable the hook again and clean up. Dispose does this in one go!
        this._WalkAnimSpeedHook.Dispose();
        this._RunAnimSpeedHook.Dispose();
        this._companionPropsHook.Dispose();
    }
}
