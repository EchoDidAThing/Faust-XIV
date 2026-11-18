using Faust.Base.Global;
using Faust.Interface;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.Config;
using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using FFXIVClientStructs.FFXIV.Common.Lua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using static FFXIVClientStructs.FFXIV.Client.UI.Misc.AozNoteModule;
#pragma warning disable CA1416 // Validate platform compatibility

namespace Faust.Sheets;

#region Main
public class CompanionDetourMain
{
    const string type = "Minion Properties";
    const string typename = "CompanionDetour";
    const string typenameplural = "CompanionDetours";
    private static string lastsearch = "";
    private static string localsearch = "";
    private static Dictionary<uint, string> SearchList = UiGlobals.CreateSearchList(type);
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search) {

        Dictionary<uint, CompanionDetourConfig> Writer = _activeSet.CompanionDetourWriter;
        var GetName = CompanionDetourManager.GetName;
        var CreateEntry = CompanionDetourConfig.CreateEntry;

        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), true);
        if (subList) {
            const string searchPopup = "Search " + typenameplural;
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in Writer.Keys) {
                if (ImGui.CollapsingHeader($"#{key:D5} - " + GetName(key))) {
                    var LocalWriter = Writer[key];
                    var old = CompanionDetourManager.GetOriginal(key);
                    bool enablechanges = UiGlobals.CheckIsEnabled(Service.Config.EnableReplacement, _activeSet, LocalWriter.Enabled);

                    using (ImRaii.PushFont(UiBuilder.IconFont)) {
                        if (UiGlobals.DrawDeleteEntryButton($"{FontAwesomeIcon.Trash.ToIconString()}##{key}")) {
                            Writer.Remove(key);
                            Service.Config.Save();
                        }
                    }
                    if (ImGui.Checkbox("##" + key, ref LocalWriter.Enabled)) {
                        enablechanges = UiGlobals.CheckIsEnabled(Service.Config.EnableReplacement, _activeSet, LocalWriter.Enabled);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();
                    ImGui.TextUnformatted("Entry Enabled");
                    UiGlobals.DrawItemSeparator();



                    
                    #region Datainputs
                    //UiGlobals.DrawFloat("Walk Animation Speed", type, key, enablechanges, ref LocalWriter.Replacement.WalkAnimationSpeed, old.WalkAnimationSpeed);
                    //UiGlobals.DrawFloat("Run Animation Speed", type, key, enablechanges, ref LocalWriter.Replacement.RunAnimationSpeed, old.RunAnimationSpeed);
                    //UiGlobals.DrawFloat("Swim Animation Speed", type, key, enablechanges, ref LocalWriter.Replacement.SwimAnimationSpeed, old.SwimAnimationSpeed);
                    //UiGlobals.DrawItemSeparator();
                    UiGlobals.DrawFloat("Scale", type, key, enablechanges, ref LocalWriter.Replacement.ModelScale, old.ModelScale);
                    UiGlobals.DrawByte("inactiveIdle0", type, key, enablechanges, ref LocalWriter.Replacement.InactiveIdle0, old.InactiveIdle0);
                    UiGlobals.DrawByte("inactiveIdle1", type, key, enablechanges, ref LocalWriter.Replacement.InactiveIdle1, old.InactiveIdle1);
                    UiGlobals.DrawByte("inactiveBattle0", type, key, enablechanges, ref LocalWriter.Replacement.InactiveBattle, old.InactiveBattle);
                    UiGlobals.DrawByte("inactiveWandering", type, key, enablechanges, ref LocalWriter.Replacement.InactiveWandering, old.InactiveWandering);
                    UiGlobals.DrawComboEnum<byte>("Behaviour", type, key, enablechanges, ref LocalWriter.Replacement.Behaviour, old.Behaviour, typeof(CompanionMove));
                    UiGlobals.DrawByte("Special", type, key, enablechanges, ref LocalWriter.Replacement.Special, old.Special);
                    //UiGlobals.DrawByte("Unknown10", type, key, enablechanges, ref LocalWriter.Replacement.Unknown10, old.Unknown10);
                    //UiGlobals.DrawByte("Unknown11", type, key, enablechanges, ref LocalWriter.Replacement.Unknown11, old.Unknown11);
                    UiGlobals.DrawByte("WanderingWait", type, key, enablechanges, ref LocalWriter.Replacement.WanderingWait, old.WanderingWait);
                    //UiGlobals.DrawUShort("priority", type, key, enablechanges, ref LocalWriter.Replacement.Priority, old.Priority);
                    //UiGlobals.DrawBool("unknown5", type, key, enablechanges, ref LocalWriter.Replacement.Unknown5, old.Unknown5);
                    //UiGlobals.DrawBool("unknown6", type, key, enablechanges, ref LocalWriter.Replacement.Unknown6, old.Unknown6);
                    //UiGlobals.DrawBool("unknown7", type, key, enablechanges, ref LocalWriter.Replacement.Unknown7, old.Unknown7);
                    //UiGlobals.DrawBool("unknown8", type, key, enablechanges, ref LocalWriter.Replacement.Unknown8, old.Unknown8);
                    //UiGlobals.DrawBool("unknown9", type, key, enablechanges, ref LocalWriter.Replacement.Unknown9, old.Unknown9);
                    UiGlobals.DrawUShort("Enemy", type, key, enablechanges, ref LocalWriter.Replacement.Enemy, old.Enemy);
                    UiGlobals.DrawBool("IsBattle", type, key, enablechanges, ref LocalWriter.Replacement.IsBattle, old.IsBattle);
                   

                    UiGlobals.DrawItemSeparator();
                    continue;

                    #endregion

                }
            }


#endregion
            #region Search/Set

            using var searchMenu = ImRaii.Popup(searchPopup);
            if (searchMenu)
            {
                ImGui.SetNextItemWidth(200);
                if (ImGui.InputText("##Search " + typenameplural.ToLower(), ref search, 256)) { localsearch = search; }
                ImGui.SameLine();
                using (ImRaii.PushFont(UiBuilder.IconFont))
                {
                    if (ImGui.Button($"{FontAwesomeIcon.Undo.ToIconString()}##{typenameplural.ToLower()}searchreset"))
                    {
                        search = "";
                        localsearch = search;
                    }
                }

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                if (localsearch == "" && localsearch != lastsearch) { SearchList = UiGlobals.CreateSearchList(type); }
                if (localsearch != lastsearch)
                {
                    var tempSearchList = UiGlobals.CreateSearchList(type).OrderBy(i => {
                        if (string.IsNullOrEmpty(localsearch)) return 0;
                        return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                            ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                    });
                    SearchList = tempSearchList.ToDictionary<uint, string>();
                    lastsearch = localsearch;
                }
                foreach (var pair in SearchList)
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        Writer[pair.Key] = CreateEntry(pair.Key);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
