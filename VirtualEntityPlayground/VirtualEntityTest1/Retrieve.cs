using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using VirtualEntityTest1.ChildEntity;
using VirtualEntityTest1.ParentEntity;

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

            Entity entity = null;
            if (reference.LogicalName == "mwo_external")
            {
                var rec = GetFromId(reference.Id);
                trace.Trace("Record: " + rec.Id);
                entity = MapRecord(rec);
            }
            else if (reference.LogicalName == "mwo_externalchild")
            {
                var rec = GetChildFromId(reference.Id);
                trace.Trace("Record: " + rec.Id);
                entity = MapRecord(rec);
            }

            context.OutputParameters["BusinessEntity"] = entity;
        }

        public static Entity MapRecord(ExternalModel record)
        {
            Entity entity = new Entity("mwo_external");
            entity.Attributes.Add("mwo_externalid", record.Guid);
            entity.Attributes.Add("mwo_externalidname", record.Id);
            entity.Attributes.Add("mwo_name", record.Name);
            entity.Attributes.Add("mwo_account", new EntityReference("account", record.Account ?? Guid.Empty));
            return entity;
        }

        private ExternalModel GetFromId(Guid guid)
        {
            var repo = new Repository();
            var list = repo.Query;
            return list.First(_ => _.Guid == guid);
        }

        public static Entity MapRecord(ChildModel record)
        {
            Entity entity = new Entity("mwo_externalchild");
            entity.Attributes.Add("mwo_externalchildid", record.Guid);
            entity.Attributes.Add("mwo_externalidname", record.Id);
            entity.Attributes.Add("mwo_name", record.Name);
            entity.Attributes.Add("mwo_parent", new EntityReference("mwo_external", record.ParentGuid ?? Guid.Empty));
            return entity;
        }

        private ChildModel GetChildFromId(Guid guid)
        {
            var repo = new ChildRepository();
            var list = repo.Query;
            return list.First(_ => _.Guid == guid);
        }

    }
}
