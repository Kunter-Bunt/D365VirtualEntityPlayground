using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Script.Serialization;

namespace CustomAPI
{
    public class GenerateToken : IPlugin
    {
        public GenerateToken() { }
        public GenerateToken(string unsecure)
        {
            Unsecure = unsecure;
        }
        public GenerateToken(string unsecure, string secure) 
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

            tracingService.Trace($"Generating Token from: {Secure}");
            var config = new JavaScriptSerializer().Deserialize<Config>(Secure);

            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            var target = context.InputParameters["Target"] as EntityReference;
            var audience = service.Retrieve(target.LogicalName, target.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
            var audienceUrl = audience.GetAttributeValue<string>("mwo_applicationiduri");
            tracingService.Trace($"Audience : {audienceUrl}");

            var token = GetClientCredentialsToken(config.ClientId, config.ClientSecret, audienceUrl, config.TenantId);
            tracingService.Trace($"Token : {token}");

            context.OutputParameters["Token"] = token;
        }

        public virtual string GetClientCredentialsToken(string clientId, string clientSecret, string resource, string tenantId)
        {
            var url = $"https://login.microsoftonline.com/{tenantId}/oauth2/token";
            var dict = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", clientId },
                { "client_secret", Uri.EscapeDataString(clientSecret) },
                { "resource", resource },
            };
            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(dict) };
            var res = client.SendAsync(req).Result;

            return new JavaScriptSerializer().Deserialize<Token>(res.Content.ReadAsStringAsync().Result)?.access_token;
        }
    }

    public class Config
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
    }

    public class Token
    {
        public string access_token { get; set; }
    }
}
