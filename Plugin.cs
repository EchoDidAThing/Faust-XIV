using Faust.Base;
using Faust.Base.Detours;
using Faust.Interface;
using Faust.Sheets;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI;
using System;
using System.Collections.Immutable;
using System.Collections.Specialized;
using Faust.Base.Global;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using Serilog.Events;

namespace Faust;

public sealed class Plugin : IDalamudPlugin
{

    private readonly HookHandler HookHandler;
    private readonly ImmutableArray<IDisposable> _disposables;
    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Service>();
        pluginInterface.Create<Events>();
        HookHandler = new HookHandler();


        //用于读取Config
        //Service.Log.Error("cur value is " + Service.PluginInterface.ConfigFile.FullName);
        //Service.Config = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Service.Config.Load();
        Service.Config.Save();

        _disposables = [new WindowManager()];

    }

    public void Dispose()
    {
        HookHandler?.Dispose();
        Service.Config.Save();
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}