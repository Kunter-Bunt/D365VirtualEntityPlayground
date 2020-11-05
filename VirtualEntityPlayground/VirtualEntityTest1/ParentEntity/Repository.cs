using System;
using System.Collections.Generic;

namespace VirtualEntityTest1.ParentEntity
{
    class Repository
    {
        public IEnumerable<ExternalModel> Query { get => Entitites; }

        private List<ExternalModel> Entitites = new List<ExternalModel>
        {
            new ExternalModel
            {
                Id = "abc123",
                Name = "Hello",
                Account = new Guid("a16b3f4b-1be7-e611-8101-e0071b6af231")
            },
            new ExternalModel
            {
                Id = "abc124",
                Name = "World"
            }
        };
    }
}
