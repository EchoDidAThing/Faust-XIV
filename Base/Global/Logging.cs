using System;
using Faust;
using Serilog.Events;

namespace Faust.Base.Global;

public class Logging
{
    public static void pluginDevLog(LogEventLevel msgType, string message)
    {
        if (!Service.Config.devLogging) return;
        Service.Log.Write(msgType,null, message);
    }
    public static void pluginDevLog(LogEventLevel msgType, Exception e, string message)
    {
        if (!Service.Config.devLogging) return;
        Service.Log.Write(msgType,null, message);
    }
    
    public static void pluginLog(LogEventLevel msgType, string message)
    {
        Service.Log.Write(msgType,null, message);
    }
}