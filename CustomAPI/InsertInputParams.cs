using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace CustomAPI
{
    public class InsertInputParams : IPlugin
    {
        public InsertInputParams() { }
        public InsertInputParams(string unsecure)
        {
            Unsecure = unsecure;
        }
        public InsertInputParams(string unsecure, string secure)
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

            tracingService.Trace($"Setting Input from: {Secure}");
            var config = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(Secure);
            foreach (var pair in config)
            {
                context.InputParameters[pair.Key] = pair.Value;
            }
            tracingService.Trace($"InputParameters:");
            foreach (var key in context.InputParameters.Keys)
            {
                tracingService.Trace($"Key: {key}, Value: {context.InputParameters[key]}");
            }
        }
    }
}
