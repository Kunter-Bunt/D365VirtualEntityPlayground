using Microsoft.Xrm.Sdk;
using System;
using System.Linq;

namespace VirtualEntityTest1
{
    public class Retrieve : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            trace.Trace("Init");
            var reference = (EntityReference)context.InputParameters["Target"];
            trace.Trace($"Reference: {reference?.LogicalName}({reference?.Id})");

            var rec = GetFromId(reference.Id);
            trace.Trace("Record: " + rec.Id);
            var entity = MapRecord(rec);

            context.OutputParameters["BusinessEntity"] = entity;
        }

        public static Entity MapRecord(ExternalModel record)
        {
            Entity entity = new Entity("mwo_external");
            entity.Attributes.Add("mwo_externalid", record.Guid);
            entity.Attributes.Add("mwo_externalidname", record.Id);
            entity.Attributes.Add("mwo_name", record.Name);
            //entity.Attributes.Add("mwo_parent", record.Parent);
            return entity;
        }

        private ExternalModel GetFromId(Guid guid)
        {
            var repo = new Repository();
            var list = repo.Query;
            return list.First(_ => _.Guid == guid);
        }
    }
}
