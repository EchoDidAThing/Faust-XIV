using System.Runtime.InteropServices;

namespace Faust.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct ActionTransientData
{
    //NEW!! SETUP
    [FieldOffset(0x0)]
    public ushort DescriptionOffset;
}
