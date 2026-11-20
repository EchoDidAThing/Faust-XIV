using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using Lumina.Excel.Sheets;
using Quaternion = FFXIVClientStructs.FFXIV.Common.Math.Quaternion;

#pragma warning disable CA1416 // Validate platform compatibility

namespace Faust.Sheets;

public static class MountLightManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, LightReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<Mount>()
            .Where(i => !string.IsNullOrEmpty(i.Singular.ToString()))
            .ToDictionary(i => i.RowId, i => i.Singular.ToString());

    public static IEnumerable<uint> AllMountLightIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.MountLightWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static LightReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static LightReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, LightReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            if (item.CharacterName != Service.PlayerState.CharacterName) continue;
            if (item.HomeWorld != Service.PlayerState.HomeWorld.RowId) continue;
            if (!item.Jobs.CheckJob(Service.PlayerState.ClassJob.RowId)) continue;
            if (!item.Enabled) continue;
            foreach (var replacement in item.MountLightWriter)
            {
                if (replacement.Key != idx) continue;
                if (!replacement.Value.Enabled) continue;
                replacements.Add(new KeyValuePair<int, LightReplace>(item.Priority, replacement.Value.Replacement));

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

    public static LightReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            replacement = new LightReplace(
                2u, 
                false,
                0,
                new Vector3(0f,0f,0f),
                new Vector4(1f,1f,1f, 1f),
                new Vector3(0f,0f,0f),
                new Vector3(1.0f,1.0f,1.0f),
                1f,
                20f,
                20f,
                new Vector2(0.01f,20f),
                0u,
                1f,
                new Vector2(0f,0f),
                45f
                );
        }
        return replacement!;
    }
}