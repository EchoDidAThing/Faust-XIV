using Faust.Base.Global;
using Faust.Sheets;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using Lumina.Excel.Sheets;
using Lumina.Excel.Sheets.Experimental;
using System;
using System.Numerics;
using System.Threading.Channels;
using World = Lumina.Excel.Sheets.World;

#pragma warning disable CA1416 // Validate platform compatibility

namespace Faust.Interface;

public sealed partial class ConfigWindow : Window
{
    private void DrawBodyMain()
    {
        using var child = ImRaii.Child("Main body", new Vector2(-1f, 345f), false);
        if (!child) return;
        if (_activeSet is null) return;

        using (ImRaii.PushFont(GetFont(20)))
        {
            var width = ImGui.CalcTextSize(_activeSet.Name).X + ImGui.GetStyle().FramePadding.X * 2;
            var x = ImGui.GetWindowWidth() / 2 - width / 2;

            ImGui.SetCursorPosX(x);
            ImGui.SetNextItemWidth(width);

            if (ImGui.InputText("##Name", ref _activeSet.Name, 256))
            {
                Service.Config.Save();
            }
        }

        if (ImGui.Checkbox("Set Enable", ref _activeSet.Enabled))
        {
            Service.Config.Save();
        }
        ImGui.SameLine();
        ImGui.SetNextItemWidth(50 * CalcGlobals.GlobalScale());

        if (ImGui.DragInt("Priority", ref _activeSet.Priority))
        {
            Service.Config.Save();
        }

        if (ImGui.InputText("Character Name", ref _activeSet.CharacterName, 512, ImGuiInputTextFlags.EnterReturnsTrue))
        {
            Service.Config.Save();
        }
        ImGui.SameLine();
        if (ImGui.Button("Current##name"))
        {
            string playername = Service.PlayerState.CharacterName;
            if (playername.IsNullOrEmpty()) { return; }
            _activeSet.CharacterName = playername;
        }
        
        UiGlobals.DrawComboEXD("Character HomeWorld", "Homeworld",0,true, ref _activeSet.HomeWorld, 0);

        ImGui.SameLine();
        if (ImGui.Button("Current##world"))
        {
            uint worldname = Service.PlayerState.HomeWorld.RowId;
            _activeSet.HomeWorld = worldname;
        }
        ImGui.NewLine();

        DrawJobSelectors(ref _activeSet);
    }

    private void DrawJobSelectors(ref Configuration.ReplacementSet activeset)
    {
        ImGui.Columns(Math.Max(3, (int)(ImGui.GetWindowWidth() / (80 * ImGui.GetIO().FontGlobalScale))), "###equipAsClassList", false);
        if (ImGui.Button("Select All"))
        {
            activeset.Jobs.SetAll(true);
        }
        ImGui.NextColumn();
        if (ImGui.Button("Select None"))
        {
            activeset.Jobs.SetAll(false);
        }
        ImGui.NextColumn();
        ImGui.NextColumn();
        ImGui.NextColumn();
        ImGui.NextColumn();
        ImGui.NextColumn();
        ImGui.NextColumn();
        ImGui.NextColumn();
        if (ImGui.Button("Add Current"))
        {
            switch (Service.PlayerState.ClassJob.Value.Abbreviation.ToString())
            {
                case "GLA":
                    activeset.Jobs.GLA = true;
                    break;
                case "MRD":
                    activeset.Jobs.MRD = true;
                    break;
                case "CNJ":
                    activeset.Jobs.CNJ = true;
                    break;
                case "PGL":
                    activeset.Jobs.PGL = true;
                    break;
                case "LNC":
                    activeset.Jobs.LNC = true;
                    break;
                case "ROG":
                    activeset.Jobs.ROG = true;
                    break;
                case "ARC":
                    activeset.Jobs.ARC = true;
                    break;
                case "THM":
                    activeset.Jobs.THM = true;
                    break;
                case "ACN":
                    activeset.Jobs.ACN = true;
                    break;
                case "PLD":
                    activeset.Jobs.PLD = true;
                    break;
                case "WAR":
                    activeset.Jobs.WAR = true;
                    break;
                case "DRK":
                    activeset.Jobs.DRK = true;
                    break;
                case "GNB":
                    activeset.Jobs.GNB = true;
                    break;
                //case "BAS":
                // activeset.Jobs.BAS = true;
                // break;
                case "WHM":
                    activeset.Jobs.WHM = true;
                    break;
                case "SCH":
                    activeset.Jobs.SCH = true;
                    break;
                case "AST":
                    activeset.Jobs.AST = true;
                    break;
                case "SGE":
                    activeset.Jobs.SGE = true;
                    break;
                case "MNK":
                    activeset.Jobs.MNK = true;
                    break;
                case "DRG":
                    activeset.Jobs.DRG = true;
                    break;
                case "NIN":
                    activeset.Jobs.NIN = true;
                    break;
                case "SAM":
                    activeset.Jobs.SAM = true;
                    break;
                case "RPR":
                    activeset.Jobs.RPR = true;
                    break;
                case "VPR":
                    activeset.Jobs.VPR = true;
                    break;
                case "BST":
                    activeset.Jobs.BST = true;
                    break;
                case "BRD":
                    activeset.Jobs.BRD = true;
                    break;
                case "MCH":
                    activeset.Jobs.MCH = true;
                    break;
                case "DNC":
                    activeset.Jobs.DNC = true;
                    break;
                //case "ZPH":
                // activeset.Jobs.ZPH = true;
                // break;
                case "BLM":
                    activeset.Jobs.BLM = true;
                    break;
                case "SMN":
                    activeset.Jobs.SMN = true;
                    break;
                case "RDM":
                    activeset.Jobs.RDM = true;
                    break;
                case "PCT":
                    activeset.Jobs.PCT = true;
                    break;
                case "BLU":
                    activeset.Jobs.BLU = true;
                    break;
                case "CRP":
                    activeset.Jobs.CRP = true;
                    break;
                case "BSM":
                    activeset.Jobs.BSM = true;
                    break;
                case "ARM":
                    activeset.Jobs.ARM = true;
                    break;
                case "GSM":
                    activeset.Jobs.GSM = true;
                    break;
                case "LTW":
                    activeset.Jobs.LTW = true;
                    break;
                case "WVR":
                    activeset.Jobs.WVR = true;
                    break;
                case "ALC":
                    activeset.Jobs.ALC = true;
                    break;
                case "CUL":
                    activeset.Jobs.CUL = true;
                    break;
                case "MIN":
                    activeset.Jobs.MIN = true;
                    break;
                case "BTN":
                    activeset.Jobs.BTN = true;
                    break;
                case "FSH":
                    activeset.Jobs.FSH = true;
                    break;
                default:
                    {
                        Service.Log.Info(Service.PlayerState.ClassJob.Value.Abbreviation.ToString() + " does not exist in AddCurrent Switch");
                        break;
                    }
            }
        }
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.GLA, "GLA", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.MRD, "MRD", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.CNJ, "CNJ", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.PGL, "PGL", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.LNC, "LNC", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.ROG, "ROG", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.ARC, "ARC", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.THM, "THM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.ACN, "ACN", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.PLD, "PLD", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.WAR, "WAR", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.DRK, "DRK", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.GNB, "GNB", activeset.Name);
        ImGui.NextColumn();
        //UiGlobals.DrawClassJobBool(ref activeset.Jobs.BAS, "BAS", activeset.Name);
        //ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.WHM, "WHM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.SCH, "SCH", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.AST, "AST", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.SGE, "SGE", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.MNK, "MNK", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.DRG, "DRG", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.NIN, "NIN", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.SAM, "SAM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.RPR, "RPR", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.VPR, "VPR", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.BST, "BST", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.BRD, "BRD", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.MCH, "MCH", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.DNC, "DNC", activeset.Name);
        ImGui.NextColumn();
        //UiGlobals.DrawClassJobBool(ref activeset.Jobs.ZPH, "ZPH", activeset.Name);
        //ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.BLM, "BLM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.SMN, "SMN", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.RDM, "RDM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.PCT, "PCT", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.BLU, "BLU", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.CRP, "CRP", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.BSM, "BSM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.ARM, "ARM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.GSM, "GSM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.LTW, "LTW", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.WVR, "WVR", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.ALC, "ALC", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.CUL, "CUL", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.MIN, "MIN", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.BTN, "BTN", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.FSH, "FSH", activeset.Name);
    }
        
}