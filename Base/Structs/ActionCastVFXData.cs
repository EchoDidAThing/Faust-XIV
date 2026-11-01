using System.Runtime.InteropServices;

namespace Faust.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct ActionCastVFXData
{
    [FieldOffset(0x0)]
    public ushort Vfx;
}
