using System.Runtime.InteropServices;

namespace Faust.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct ItemActionData
{
    //ALL NEW!! SETUP
    [FieldOffset(0x0)]
    public ushort Action;
}
