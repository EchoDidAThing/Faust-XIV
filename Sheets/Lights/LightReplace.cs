using Faust.Base.Global;
using Faust.Base.Structs;
using Faust.Sheets;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using System.Collections;
using System.Numerics;
using System.Runtime.InteropServices;
using Quaternion = FFXIVClientStructs.FFXIV.Common.Math.Quaternion;

namespace Faust.Sheets;

public class LightReplace(
    uint shape,
    bool visible,
    uint boneindex,
    Vector3 locoffsets,
    Vector4 rotoffsets,
    Vector3 scaleoffsets,
    Vector3 colour,
    float intensity,
    float range,
    float charShadowRange,
    float shadowPlaneNear,
    float shadowPlaneFar,
    uint falloffType,
    float falloffFactor,
    Vector2 flatLightSkewAngleDegrees,
    float spotLightAngleDegrees)
{
    public uint LightShape = shape;
    public bool Visible = visible;
    public uint BoneIndex = boneindex;
    public Vector3 Locoffsets = locoffsets;
    public Vector4 Rotoffsets = rotoffsets;
    public Vector3 Scaleoffsets = scaleoffsets;
    public Vector3 Colour = colour;
    public float Intensity = intensity;
    public float Range = range;
    public float CharShadowRange = charShadowRange;
    public float ShadowPlaneNear = shadowPlaneNear;
    public float ShadowPlaneFar = shadowPlaneFar;
    public uint FalloffType = falloffType;
    public float FalloffFactor= falloffFactor;
    public Vector2 FlatLightSkewAngleDegrees = flatLightSkewAngleDegrees;
    public float SpotLightAngleDegrees = spotLightAngleDegrees;
}