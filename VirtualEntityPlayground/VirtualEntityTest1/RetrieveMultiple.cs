using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualEntityTest1
{
    public class RetrieveMultiple : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            trace.Trace("Init");
            var query = (QueryExpression)context.InputParameters["Query"];
            trace.Trace("Query: " + query);
            if (query != null)
            {
                var externalRecords = GetFromCriteria(query.Criteria);
                EntityCollection collection = MapRecords(externalRecords);
                trace.Trace("Records: " + collection.Entities.Count);
                context.OutputParameters["BusinessEntityCollection"] = collection;
            }
        }

        private EntityCollection MapRecords(IEnumerable<ExternalModel> externalRecords)
        {
            var collection = new EntityCollection();
            foreach(var record in externalRecords)
            {
                Entity entity = new Entity("mwo_external");
                entity.Attributes.Add("mwo_externalid", record.Guid);
                entity.Attributes.Add("mwo_externalidname", record.Id);
                entity.Attributes.Add("mwo_name", record.Name);
                //entity.Attributes.Add("mwo_parent", record.Parent);

                collection.Entities.Add(entity);
            }
            return collection;
        }

        private IEnumerable<ExternalModel> GetFromCriteria(FilterExpression ex)
        {
            var repo = new Repository();
            var list = repo.Query;
            if (ex.Conditions.Any() && ex.Conditions.First().AttributeName == "mwo_name")
            {
                list = list.Where(_ => _.Name == (string)ex.Conditions.First().Values.FirstOrDefault());
            }
            return list;
        }
    }
}
