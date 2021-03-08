using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.OData.UriParser;
using ODataService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;

namespace ODataService.Controllers
{
    public class ItemsController : ODataController
    {
        private List<Item> items = new List<Item>()
        {
            new Item()
            {
                ID = new Guid("1852B129-26BA-4880-A2D5-32DBF95515E5"),
                Name = "Bread",
                Description = "A slice of Bread",
                EAN = 12345
            },
            new Item()
            {
                ID = new Guid("CC20B7A7-2BA6-4C3C-85D5-C3A82F986324"),
                Name = "Butter",
                Description = "Golden Smooth",
                EAN = 67890
            }
        };

        /// <summary>
        /// This sample only reacts to the filter "Name"!
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [EnableQuery]
        public IQueryable<Item> Get(ODataQueryOptions options)
        {
            if (options.Filter != null && options.Filter.FilterClause != null)
            {
                var binaryOperator = options.Filter.FilterClause.Expression as BinaryOperatorNode;
                if (binaryOperator != null)
                {
                    var property = binaryOperator.Left as SingleValuePropertyAccessNode ?? binaryOperator.Right as SingleValuePropertyAccessNode;
                    var constant = binaryOperator.Left as ConstantNode ?? binaryOperator.Right as ConstantNode;

                    if (property != null && property.Property != null && constant != null && constant.Value != null)
                    {
                        Debug.WriteLine("Property: " + property.Property.Name);
                        Debug.WriteLine("Operator: " + binaryOperator.OperatorKind);
                        Debug.WriteLine("Value: " + constant.LiteralText);

                        if (property.Property.Name == nameof(Item.Name))
                            return items.Where(_ => _.Name == constant.LiteralText.Trim('\'')).AsQueryable();
                    }
                }
            }

            return new List<Item>().AsQueryable();
        }

        public SingleResult<Item> Get([FromODataUri] Guid key)
        {
            IQueryable<Item> result = items.Where(p => p.ID == key).AsQueryable();
            return SingleResult.Create(result);
        }
    }
}