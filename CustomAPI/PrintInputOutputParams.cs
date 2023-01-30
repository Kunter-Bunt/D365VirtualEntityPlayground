using Microsoft.Xrm.Sdk;
using System;

namespace CustomAPI
{
    public class PrintInputOutputParams : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            tracingService.Trace($"InputParameters:");
            foreach (var key in context.InputParameters.Keys)
            {
                tracingService.Trace($"Key: {key}, Value: {context.InputParameters[key]}");
            }
            tracingService.Trace($"OutputParameters:");
            foreach (var key in context.OutputParameters.Keys)
            {
                tracingService.Trace($"Key: {key}, Value: {context.OutputParameters[key]}");
            }
        }
    }
}
