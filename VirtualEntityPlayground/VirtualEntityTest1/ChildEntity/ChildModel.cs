using System;
using System.Text;

namespace VirtualEntityTest1.ChildEntity
{
    public class ChildModel
    {
        public string Id { get; set; }

        public Guid? Guid
        {
            get => new Guid(Encoding.UTF8.GetBytes((Id ?? "NoId").PadRight(16)));
            set => Id = Encoding.UTF8.GetString(value.GetValueOrDefault().ToByteArray());
        }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public Guid? ParentGuid
        {
            get => new Guid(Encoding.UTF8.GetBytes((ParentId ?? "NoId").PadRight(16)));
            set => ParentId = Encoding.UTF8.GetString(value.GetValueOrDefault().ToByteArray());
        }

    }
}
