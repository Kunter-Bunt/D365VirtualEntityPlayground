using System.Collections.Generic;

namespace VirtualEntityTest1.ChildEntity
{
    class ChildRepository
    {
        public IEnumerable<ChildModel> Query { get => Entitites; }

        private List<ChildModel> Entitites = new List<ChildModel>
        {
            new ChildModel
            {
                Id = "1",
                Name = "Child 1",
                ParentId = "abc123"
            },
            new ChildModel
            {
                Id = "2",
                Name = "Child 2",
                ParentId = "abc123"
            },
            new ChildModel
            {
                Id = "3",
                Name = "Child 3",
                ParentId = "abc124"
            },
            new ChildModel
            {
                Id = "4",
                Name = "Child 4",
                ParentId = "abc124"
            }
        };

    }
}
