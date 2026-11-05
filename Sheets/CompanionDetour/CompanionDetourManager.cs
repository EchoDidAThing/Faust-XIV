using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Faust.Base.Global;
using Lumina.Excel.Sheets;
using Serilog.Events;

#pragma warning disable CA1416 // Validate platform compatibility

namespace Faust.Sheets;

public static class CompanionDetourManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, CompanionDetourReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<Companion>()
            .Where(i => !string.IsNullOrEmpty(i.Singular.ToString()))
            .ToDictionary(i => i.RowId, i => i.Singular.ToString());

    public static IEnumerable<uint> AllCompanionDetourIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.CompanionDetourWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static CompanionDetourReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static CompanionDetourReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;
        
        List<KeyValuePair<int, CompanionDetourReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            if (item.CharacterName != Service.PlayerState.CharacterName) continue;
            if (item.HomeWorld != Service.PlayerState.HomeWorld.RowId) continue;
            if (!item.Jobs.CheckJob(Service.PlayerState.ClassJob.RowId)) continue;
            if (!item.Enabled) continue;
            foreach (var replacement in item.CompanionDetourWriter)
            {
                if (replacement.Key != idx) continue;
                if (!replacement.Value.Enabled) continue;
                replacements.Add(new KeyValuePair<int, CompanionDetourReplace>(item.Priority, replacement.Value.Replacement));
            }
        }
        if (replacements.Count == 0) { return null; }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static CompanionDetourReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<Companion>()?.GetRow(idx);
            var act2 = Service.DataManager.GetExcelSheet<ModelSkeleton>()?.GetRow(Service.DataManager.GetExcelSheet<ModelChara>()?.GetRow(act?.Model.RowId ?? 0).Model ?? 0);
            float motionwalk = act2?.Unknown0 ?? 0;
            float motionrun = act2?.Unknown1 ?? 0;
            replacement = new CompanionDetourReplace(
                motionwalk / 100,
                motionrun / 100,
                act?.Scale ?? 0
                );
        }
        return replacement!;
    }
}