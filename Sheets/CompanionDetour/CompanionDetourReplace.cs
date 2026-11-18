using Faust.Base.Global;
using Faust.Base.Structs;
using Faust.Sheets;
using System.Collections;
using System.Runtime.InteropServices;
using FFXIVClientStructs.FFXIV.Client.Game.UI;

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
    float modelscale,
    byte inactiveidle0,
    byte inactiveidle1,
    byte inactivebattle,
    byte inactivewandering,
    byte behaviour,
    byte special,
    byte unknown10,
    byte unknown11,
    byte wanderingwait,
    ushort priority,
    bool unknown5,
    bool unknown6,
    bool unknown7,
    bool unknown8,
    bool unknown9,
    ushort enemy,
    bool isbattle
    )
{
    public float WalkAnimationSpeed = groundAnimationSpeed;
    public float RunAnimationSpeed = flightAnimationSpeed;
    public float ModelScale = modelscale;
    public byte InactiveIdle0 = inactiveidle0;
    public byte InactiveIdle1 = inactiveidle1;
    public byte InactiveBattle = inactivebattle;
    public byte InactiveWandering = inactivewandering;
    public byte Behaviour = behaviour;
    public byte Special = special;
    public byte Unknown10 = unknown10;
    public byte Unknown11 = unknown11;
    public byte WanderingWait = wanderingwait;
    public ushort Priority = priority;
    public bool Unknown5 = unknown5;
    public bool Unknown6 = unknown6;
    public bool Unknown7 = unknown7;
    public bool Unknown8 = unknown8;
    public bool Unknown9 = unknown9;
    public ushort Enemy = enemy;
    public bool IsBattle = isbattle;

}