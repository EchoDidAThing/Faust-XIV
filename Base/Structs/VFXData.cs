using System.Runtime.InteropServices;

namespace Faust.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct VfxData
{
    
    [FieldOffset(0x0)]
    public uint String1Offset;

}
