using System.Runtime.InteropServices;

namespace Faust.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct MountTransientData
{
    //ALL NEW!! SETUP
    [FieldOffset(0x0)]
    public ushort DescriptionOffset;

    [FieldOffset(0x4)]
    public ushort FullDescriptionOffset;

    [FieldOffset(0x8)]
    public ushort TooltipOffset;
}