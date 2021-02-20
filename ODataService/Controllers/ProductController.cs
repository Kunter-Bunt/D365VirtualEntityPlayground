using Microsoft.AspNet.OData;
using ODataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ODataService.Controllers
{
    public class ProductsController : ODataController
    {
        private List<Product> products = new List<Product>()
        {
            new Product()
            {
                ID = new Guid("1852B129-26BA-4880-A2D5-32DBF95515E5"),
                Name = "Bread",
                Description = "A slice of Bread",
                EAN = 12345
            },
            new Product()
            {
                ID = new Guid("CC20B7A7-2BA6-4C3C-85D5-C3A82F986324"),
                Name = "Butter",
                Description = "Golden Smooth",
                EAN = 67890
            }
        };

        [EnableQuery]
        public List<Product> Get()
        {
            return products;
        }

        [EnableQuery]
        public SingleResult<Product> Get([FromODataUri] Guid key)
        {
            IQueryable<Product> result = products.Where(p => p.ID == key).AsQueryable();
            return SingleResult.Create(result);
        }
    }
}