using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Common.Lua;
#pragma warning disable CA1416 // Validate platform compatibility

namespace Faust.Interface;

public sealed partial class ConfigWindow : Window
{
    private static void DrawHeader()
    {
        if (ImGui.Checkbox("Enable all", ref Service.Config.EnableReplacement))
        {
            Service.Config.Save();
        }
        ImGui.SameLine();
        if (ImGui.Button(Service.Config.devLogging? "Logging On" : "Logging Off"))
        {
            Service.Config.devLogging = !Service.Config.devLogging;
            Service.Config.Save();
        }
    }
}
