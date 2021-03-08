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
        /// This sample only returns something if the filter "Name" is present!
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [EnableQuery]
        public IQueryable<Item> Get(ODataQueryOptions options)
        {
            var filter = GetFilter(options?.Filter?.FilterClause?.Expression);

            if (filter.ContainsKey(nameof(Item.Name)))
                return items.Where(_ => _.Name == filter[nameof(Item.Name)].ToString()).AsQueryable();

            return new List<Item>().AsQueryable();
        }

        private Dictionary<string, object> GetFilter(SingleValueNode filter)
        {
            var dict = new Dictionary<string, object>();
            if (filter is BinaryOperatorNode bin)
            {
                var left = GetFilter(bin.Left);
                foreach (var filt in left)
                    dict.Add(filt.Key, filt.Value);

                var right = GetFilter(bin.Right);
                foreach (var filt in right)
                    dict.Add(filt.Key, filt.Value);

                if (bin.Left is SingleValuePropertyAccessNode key && bin.Right is ConstantNode value)
                    dict.Add(key.Property.Name, value.Value);
            }
            return dict;
        }

        public SingleResult<Item> Get([FromODataUri] Guid key)
        {
            IQueryable<Item> result = items.Where(p => p.ID == key).AsQueryable();
            return SingleResult.Create(result);
        }
    }
}