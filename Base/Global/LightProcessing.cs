using System;
using System.Collections.Generic;
using Faust.Sheets;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using FFXIVClientStructs.FFXIV.Common.Math;
using FFXIVClientStructs.Havok.Animation.Rig;
using Serilog.Events;
using Light = FFXIVClientStructs.FFXIV.Client.Graphics.Scene.Light;

namespace Faust.Base.Global;

public unsafe class LightProcess
{
    public class LightEntry(Light* light, Character* charaObject, LightReplace properties)
    {
        public Light* Light = light;
        public Character* CharaObject = charaObject;
        public LightReplace Properties = properties;
    }
    
    public Dictionary<string, LightEntry> LightEntries = new Dictionary<string, LightEntry>();
    
    public static Light* AddLight(LightReplace properties, string poolname)
    {
        Light* templight = Light.Create((LightShape)properties.LightShape, poolname);
        templight->IsVisible = properties.Visible;
        templight->RenderLight->Color = properties.Colour;
        templight->RenderLight->Intensity = properties.Intensity;
        templight->RenderLight->LightFlags = LightFlags.SpecularHighlights | LightFlags.CharacterShadows | LightFlags.DynamicShadows | LightFlags.ObjectShadows;
        templight->RenderLight->Range = properties.Range;
        templight->RenderLight->CharacterShadowRange = properties.CharShadowRange;
        templight->RenderLight->ShadowPlaneNear = properties.ShadowPlane.X;
        templight->RenderLight->ShadowPlaneFar = properties.ShadowPlane.Y;
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
        if (CharaObject->DrawObject == null) return;
        if (!CharaObject->DrawObject->IsVisible) return;
        //var bonedata = ((CharacterBase*)CharaObject->GetParentCharacter()->DrawObject)->Skeleton->PartialSkeletons[0].GetHavokPose(0)->AccessBoneModelSpace((int)properties.BoneIndex, hkaPose.PropagateOrNot.DontPropagate);
        if (((CharacterBase*)(CharaObject->GetParentCharacter()->DrawObject))->Skeleton->PartialSkeletonCount == 0) return;
        var ParentSkeleton = ((CharacterBase*)(CharaObject->GetParentCharacter()->DrawObject))->Skeleton;
        var bonedata = ParentSkeleton->PartialSkeletons[2].GetHavokPose(0)->AccessBoneLocalSpace((int)properties.BoneIndex);
        //FIXME:When turning, y is offset, wh?
        Logging.pluginDevLog(LogEventLevel.Error,"Updating light location! X" + bonedata->Translation.Y);
        light->RenderLight->Transform->Position = new Vector3(ParentSkeleton->Transform.Position.X + properties.Locoffsets.X + bonedata->Translation.X,
            ParentSkeleton->Transform.Position.Y + properties.Locoffsets.Y + bonedata->Translation.Y,
            ParentSkeleton->Transform.Position.Z + properties.Locoffsets.Z + bonedata->Translation.Z);
        //light->RenderLight->Transform->Rotation = CharaObject->GetParentCharacter();->
        light->RenderLight->Transform->Rotation =  new Quaternion(CharaObject->GetParentCharacter()->DrawObject->Rotation.X + properties.Rotoffsets.X * bonedata->Rotation.X,
            ParentSkeleton->Transform.Rotation.Y + properties.Rotoffsets.Y * bonedata->Rotation.Y,
            ParentSkeleton->Transform.Rotation.Z + properties.Rotoffsets.Z * bonedata->Rotation.Z,
            ParentSkeleton->Transform.Rotation.W + properties.Rotoffsets.W * bonedata->Rotation.W);
        light->RenderLight->Transform->Scale = new Vector3(
            ParentSkeleton->Transform.Scale.X + properties.Scaleoffsets.X + bonedata->Scale.X,
            ParentSkeleton->Transform.Scale.Y + properties.Scaleoffsets.Y + bonedata->Scale.Y,
            ParentSkeleton->Transform.Scale.Z + properties.Scaleoffsets.Z + bonedata->Scale.Z);
        
        light->UpdateCulling();
        
    }
    
    public static void RemoveLight(ref Light* light)
    {
        light->VirtualTable->CleanupRender(light);
        light->VirtualTable->Dtor(light, 0);
        light = null;
    }
}