using Faust.Base.Global;
using Faust.Sheets;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine.Node;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;

#pragma warning disable CA1416 // Validate platform compatibility

namespace Faust.Interface;

public sealed partial class ConfigWindow : Window
{
    private Dictionary<string, List<string>> _allHeaders = [];

    //TOSETUP: Add new search here
    private string _searchActionTransients = string.Empty;
    private string _searchCompanionTransients = string.Empty;
    private string _searchJobTransients = string.Empty;
    private string _searchMountDetour = string.Empty;
    private string _searchMountTransients = string.Empty;
    private string _searchOrnamentTransients = string.Empty;
    private string _searchStatusTransients = string.Empty;
    private string _searchTraitTransients = string.Empty;
    private string activesheet = "NONE";

    private void DrawSheets()
    {
        if (!_allHeaders.ContainsKey("ActionTransients"))
        {
            //TOSETUP: Add new headers here
            _allHeaders.Add("ActionTransients", []);
            _allHeaders.Add("CompanionTransients", []);
            _allHeaders.Add("JobTransients", []);
            _allHeaders.Add("MountDetour", []);
            _allHeaders.Add("MountTransients", []);
            _allHeaders.Add("OrnamentTransients", []);
            _allHeaders.Add("StatusTransients", []);
            _allHeaders.Add("TraitTransients", []);
            //_AllHeaders.Add("PointMenuChoice", []);

        }

        using var child = ImRaii.Child("Sheets", new Vector2(-1f, -1f), false);
        if (!child) return;
        if (_activeSet is null) return;
        

        switch (activesheet)
        {
            //TOSETUP: Add new case here to call the subsheet
            case "ActionTransients":
                ActionTransientsMain.Draw(activesheet, ref _activeSet, ref _searchActionTransients);
                break;
            case "CompanionTransients":
                CompanionTransientsMain.Draw(activesheet, ref _activeSet, ref _searchCompanionTransients);
                break;
            case "JobTransients":
               JobTransientsMain.Draw(activesheet, ref _activeSet, ref _searchJobTransients);
                break;
           case "MountDetour":
                MountDetourMain.Draw(activesheet, ref _activeSet, ref _searchMountDetour);
                break;
            case "MountTransients":
                MountTransientsMain.Draw(activesheet, ref _activeSet, ref _searchMountTransients);
                break;
            case "OrnamentTransients":
                OrnamentTransientsMain.Draw(activesheet, ref _activeSet, ref _searchOrnamentTransients);
                break;
            case "StatusTransients":
                StatusTransientsMain.Draw(activesheet, ref _activeSet, ref _searchStatusTransients);
                break;
            case "TraitTransients":
                TraitTransientsMain.Draw(activesheet, ref _activeSet, ref _searchTraitTransients);
                break;
            case "NONE":
                break;
            default:
                Service.Log.Error(activesheet + " is not defined in DrawSheets");
                break;
                //case "OrnamentCustomizeGroup":
                //    OrnamentCustomizeGroupMain.Draw(activesheet, ref _activeSet, ref _searchOrnamentCustomizeGroup);
                //    break;
                //case "PointMenuChoice":
                //    PointMenuChoiceMain.Draw(activesheet, ref _activeSet, ref _searchPointMenuChoice);
                //    break;
        }
    }
}