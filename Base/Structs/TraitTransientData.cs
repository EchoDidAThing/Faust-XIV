using System;
using System.Runtime.InteropServices;

namespace Faust.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct TraitTransientData
{
    //NEW!! SETUP
    [FieldOffset(0x0)]
    public ushort DescriptionOffset;

}