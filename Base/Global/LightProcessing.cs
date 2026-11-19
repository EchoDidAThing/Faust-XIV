using System;
using Faust.Sheets;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using FFXIVClientStructs.FFXIV.Common.Math;
using Serilog.Events;
using Light = FFXIVClientStructs.FFXIV.Client.Graphics.Scene.Light;

namespace Faust.Base.Global;

public unsafe class LightProcess
{
    public class LightEntry(Light* light, Character* charaObject, ObjectKind objectkind)
    {
        public Light* Light = light;
        public Character* CharaObject = charaObject;
        public ObjectKind ObjectKind = objectkind;
    }
    
    public static Light* AddLight(LightReplace properties)
    {
        Light* templight = Light.Create((LightShape)properties.LightShape, "Mount_Light");
        templight->IsVisible = true;
        templight->RenderLight->Color = properties.Colour;
        templight->RenderLight->Intensity = properties.Intensity;
        templight->RenderLight->LightFlags = LightFlags.SpecularHighlights | LightFlags.CharacterShadows | LightFlags.DynamicShadows | LightFlags.ObjectShadows;
        templight->RenderLight->Range = properties.Range;
        templight->RenderLight->CharacterShadowRange = properties.CharShadowRange;
        templight->RenderLight->ShadowPlaneNear = properties.ShadowPlaneNear;
        templight->RenderLight->ShadowPlaneFar = properties.ShadowPlaneFar;
        templight->RenderLight->FalloffType = (LightFalloffType)properties.FalloffType;
        templight->RenderLight->FalloffFactor = properties.FalloffFactor;
        templight->RenderLight->SpotLightAngleDegrees = properties.SpotLightAngleDegrees;
        templight->RenderLight->FlatLightSkewAngleDegrees = properties.FlatLightSkewAngleDegrees;
        return templight;
    }

    public static void UpdatePos(Light* light, Character* CharaObject, LightReplace properties)
    {
        if (light == null) return;
        if (CharaObject == null) return;

        //Logging.pluginDevLog(LogEventLevel.Error,"Updating light location! ROT" + CharaObject->GetParentCharacter()->DrawObject->Rotation);
        light->RenderLight->Transform->Position = new Vector3(CharaObject->GetParentCharacter()->DrawObject->Position.X + properties.Locoffsets.X,
            CharaObject->GetParentCharacter()->DrawObject->Position.Y + properties.Locoffsets.Y,
            CharaObject->GetParentCharacter()->DrawObject->Position.Z + properties.Locoffsets.Z);
        //light->RenderLight->Transform->Rotation = CharaObject->GetParentCharacter();
        light->RenderLight->Transform->Rotation =  new Quaternion(CharaObject->GetParentCharacter()->DrawObject->Rotation.X + properties.Rotoffsets.X,
            CharaObject->GetParentCharacter()->DrawObject->Rotation.Y + properties.Rotoffsets.Y,
            CharaObject->GetParentCharacter()->DrawObject->Rotation.Z + properties.Rotoffsets.Z,
            CharaObject->GetParentCharacter()->DrawObject->Rotation.W + properties.Rotoffsets.W);

        light->UpdateCulling();
        
    }
    
    public static void RemoveLight(ref Light* light)
    {
        
        light->VirtualTable->CleanupRender(light);
        light->VirtualTable->Dtor(light, 0);
        light = null;
    }
}