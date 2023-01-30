using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace CustomAPI
{
    public class InsertOutputParams : IPlugin
    {
        public InsertOutputParams() { }
        public InsertOutputParams(string unsecure)
        {
            Unsecure = unsecure;
        }
        public InsertOutputParams(string unsecure, string secure)
        {
            Unsecure = unsecure;
            Secure = secure;
        }

        public string Unsecure { get; }
        public string Secure { get; }

        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            tracingService.Trace($"Setting Output from: {Secure}");
            var config = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(Secure);
            foreach (var pair in config)
            {
                context.OutputParameters[pair.Key] = pair.Value;
            }
            tracingService.Trace($"OutputParameters:");
            foreach (var key in context.OutputParameters.Keys)
            {
                tracingService.Trace($"Key: {key}, Value: {context.OutputParameters[key]}");
            }
        }
    }
}
