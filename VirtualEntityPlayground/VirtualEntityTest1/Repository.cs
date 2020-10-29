using System.Collections.Generic;

namespace VirtualEntityTest1
{
    class Repository
    {
        public IEnumerable<ExternalModel> Query { get => Entitites; }

        private List<ExternalModel> Entitites = new List<ExternalModel>
        {
            new ExternalModel
            {
                Id = "abc123",
                Name = "Hello"
            },
            new ExternalModel
            {
                Id = "abc124",
                Name = "World"
            }
        };
    }
}
