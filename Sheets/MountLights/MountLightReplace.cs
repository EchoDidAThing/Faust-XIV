using Faust.Base.Global;
using Faust.Base.Structs;
using Faust.Sheets;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using System.Collections;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Faust.Sheets;


public class MountLightConfig(LightReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public LightReplace Replacement = replace;
    public static MountLightConfig CreateEntry(uint key)
    {
        LightReplace originalvalues = MountLightManager.GetOriginal(key);
        return new MountLightConfig(originalvalues, false);
    }
}
