using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using VirtualEntityTest1.ChildEntity;
using VirtualEntityTest1.ParentEntity;

namespace VirtualEntityTest1
{
    public class RetrieveMultiple : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);
            var source = (IEntityDataSourceRetrieverService)serviceProvider.GetService(typeof(IEntityDataSourceRetrieverService));

            trace.Trace("Init");
            PrintContext(trace, context);
            PrintSource(trace, source);

            EntityCollection collection = new EntityCollection { EntityName = "mwo_external" };

            var query = (QueryExpression)context.InputParameters["Query"];
            if (query != null)
            {
                PrintQuery(trace, service, query);

                if (query.EntityName == "mwo_external")
                {
                    var externalRecords = GetFromCriteria(query.Criteria);
                    collection = MapRecords(externalRecords);
                }
                else if (query.EntityName == "mwo_externalchild")
                {
                    var externalRecords = GetChildFromCriteria(query.Criteria);
                    collection = MapRecords(externalRecords);
                }

                trace.Trace("Records: " + collection.Entities.Count);
            }

            context.OutputParameters["BusinessEntityCollection"] = collection;

            trace.Trace("Done");
        }

        public static void PrintSource(ITracingService trace, IEntityDataSourceRetrieverService source)
        {
            var ent = source.RetrieveEntityDataSource();
            trace.Trace($"Source: {ent?.LogicalName}({ent?.Id})");
            foreach (var input in ent.Attributes)
            {
                trace.Trace($"{input.Key}: {input.Value}");
            }
        }

        public static void PrintContext(ITracingService trace, IPluginExecutionContext context)
        {
            foreach (var input in context.InputParameters)
            {
                trace.Trace($"{input.Key}: {input.Value}");
            }
        }

        private static void PrintQuery(ITracingService trace, IOrganizationService service, QueryExpression query)
        {
            var conversionRequest = new QueryExpressionToFetchXmlRequest
            {
                Query = query
            };
            var conversionResponse = (QueryExpressionToFetchXmlResponse)service.Execute(conversionRequest);
            var fetchXml = FormatXml(conversionResponse.FetchXml);
            trace.Trace("Query: \n" + fetchXml);
        }

        private static string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception)
            {
                return xml;
            }
        }

        private EntityCollection MapRecords(IEnumerable<ExternalModel> externalRecords)
        {
            var collection = new EntityCollection { EntityName = "mwo_external" };
            foreach (var record in externalRecords)
            {
                var entity = Retrieve.MapRecord(record);
                collection.Entities.Add(entity);
            }
            return collection;
        }

        private IEnumerable<ExternalModel> GetFromCriteria(FilterExpression ex)
        {
            if (ex.Filters.Any()) return GetFromCriteria(ex.Filters.First());

            var repo = new Repository();
            var list = repo.Query;
            if (ex.Conditions.Any(_ => _.AttributeName == "mwo_name"))
            {
                list = list.Where(_ => _.Name == (string)ex.Conditions.First(s => s.AttributeName == "mwo_name").Values.FirstOrDefault());
            }
            if (ex.Conditions.Any(_ => _.AttributeName == "mwo_externalid"))
            {
                list = list.Where(_ => _.Guid == (Guid)ex.Conditions.First(s => s.AttributeName == "mwo_externalid").Values.FirstOrDefault());
            }
            if (ex.Conditions.Any(_ => _.AttributeName == "mwo_account"))
            {
                list = list.Where(_ => _.Account == (Guid)ex.Conditions.First(s => s.AttributeName == "mwo_account").Values.FirstOrDefault());
            }

            return list;
        }

        private EntityCollection MapRecords(IEnumerable<ChildModel> externalRecords)
        {
            var collection = new EntityCollection { EntityName = "mwo_externalchild" };
            foreach (var record in externalRecords)
            {
                var entity = Retrieve.MapRecord(record);
                collection.Entities.Add(entity);
            }
            return collection;
        }

        private IEnumerable<ChildModel> GetChildFromCriteria(FilterExpression ex)
        {
            var repo = new ChildRepository();
            var list = repo.Query;
            if (ex.Conditions.Any(_ => _.AttributeName == "mwo_name"))
            {
                list = list.Where(_ => _.Name == (string)ex.Conditions.First(s => s.AttributeName == "mwo_name").Values.FirstOrDefault());
            }
            if (ex.Conditions.Any(_ => _.AttributeName == "mwo_externalchildid"))
            {
                list = list.Where(_ => _.Guid == (Guid)ex.Conditions.First(s => s.AttributeName == "mwo_externalchildid").Values.FirstOrDefault());
            }
            if (ex.Conditions.Any(_ => _.AttributeName == "mwo_parent"))
            {
                list = list.Where(_ => _.ParentGuid == (Guid)ex.Conditions.First(s => s.AttributeName == "mwo_parent").Values.FirstOrDefault());
            }
            return list;
        }
    }
}
