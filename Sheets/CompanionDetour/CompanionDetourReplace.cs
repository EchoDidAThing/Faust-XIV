using Faust.Base.Global;
using Faust.Base.Structs;
using Faust.Sheets;
using System.Collections;
using System.Runtime.InteropServices;

namespace Faust.Sheets;


public class CompanionDetourConfig(CompanionDetourReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public CompanionDetourReplace Replacement = replace;
    public static CompanionDetourConfig CreateEntry(uint key)
    {
        CompanionDetourReplace originalvalues = CompanionDetourManager.GetOriginal(key);
        return new CompanionDetourConfig(originalvalues, false);
    }
}

public class CompanionDetourReplace(
    float groundAnimationSpeed,
    float flightAnimationSpeed,
    float modelscale)
{
    public float WalkAnimationSpeed = groundAnimationSpeed;
    public float RunAnimationSpeed = flightAnimationSpeed;
    public float ModelScale = modelscale;
}