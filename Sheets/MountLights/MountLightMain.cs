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
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using static FFXIVClientStructs.FFXIV.Client.UI.Misc.AozNoteModule;
#pragma warning disable CA1416 // Validate platform compatibility

namespace Faust.Sheets;

#region Main
public class MountLightMain
{
    const string type = "Mount Light";
    const string typename = "Mount Light";
    const string typenameplural = "Mount Lights";
    private static string lastsearch = "";
    private static string localsearch = "";
    private static Dictionary<uint, string> SearchList = UiGlobals.CreateSearchList(type);
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search) {

        Dictionary<uint, MountLightConfig> Writer = _activeSet.MountLightWriter;
        var GetName = MountLightManager.GetName;
        var CreateEntry = MountLightConfig.CreateEntry;

        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), true);
        if (subList) {
            const string searchPopup = "Search " + typenameplural;
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in Writer.Keys) {
                if (ImGui.CollapsingHeader($"#{key:D5} - " + GetName(key))) {
                    var LocalWriter = Writer[key];
                    var old = MountLightManager.GetOriginal(key);
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
                    UiGlobals.DrawComboEnum<uint>("Light Shape", type, key, enablechanges, ref LocalWriter.Replacement.LightShape, old.LightShape, typeof(LightShape));
                    UiGlobals.DrawBool("Visible", type, key, enablechanges, ref LocalWriter.Replacement.Visible, old.Visible);
                    UiGlobals.DrawUInt("Bone Index", type, key, enablechanges, ref LocalWriter.Replacement.BoneIndex, old.BoneIndex);
                    UiGlobals.DrawVector3("Location Offsets[X, Y, Z]", type, key, enablechanges, ref LocalWriter.Replacement.Locoffsets, old.Locoffsets);
                    UiGlobals.DrawVector4("Rotation Offsets[X, Y, Z, W]", type, key, enablechanges, ref LocalWriter.Replacement.Rotoffsets, old.Rotoffsets);
                    UiGlobals.DrawVector3("Scale Offsets[X, Y, Z]", type, key, enablechanges, ref LocalWriter.Replacement.Scaleoffsets, old.Scaleoffsets);
                    UiGlobals.DrawVector3("Colour[R, G, B]", type, key, enablechanges, ref LocalWriter.Replacement.Colour, old.Colour);
                    UiGlobals.DrawFloat("Intensity", type, key, enablechanges, ref LocalWriter.Replacement.Intensity, old.Intensity);
                    UiGlobals.DrawFloat("Range", type, key, enablechanges, ref LocalWriter.Replacement.Range, old.Range);
                    UiGlobals.DrawFloat("Character Shadow Range", type, key, enablechanges, ref LocalWriter.Replacement.CharShadowRange, old.CharShadowRange);
                    UiGlobals.DrawVector2("Shadow Plane[Near, Far]", type, key, enablechanges, ref LocalWriter.Replacement.ShadowPlane, old.ShadowPlane);
                    UiGlobals.DrawComboEnum<uint>("Falloff Type", type, key, enablechanges, ref LocalWriter.Replacement.FalloffType, old.FalloffType, typeof(LightFalloffType));
                    UiGlobals.DrawFloat("Falloff Factor", type, key, enablechanges, ref LocalWriter.Replacement.FalloffFactor, old.FalloffFactor);
                    UiGlobals.DrawVector2("Flat Light Skew Angle", type, key, enablechanges, ref LocalWriter.Replacement.FlatLightSkewAngleDegrees, old.FlatLightSkewAngleDegrees);
                    UiGlobals.DrawFloat("Spotlight Angle", type, key, enablechanges, ref LocalWriter.Replacement.SpotLightAngleDegrees, old.SpotLightAngleDegrees);

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
