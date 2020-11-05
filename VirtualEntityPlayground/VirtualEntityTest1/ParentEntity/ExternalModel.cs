using System;
using System.Text;

namespace VirtualEntityTest1.ParentEntity
{
    public class ExternalModel
    {
        public string Id { get; set; }

        public Guid? Guid
        {
            get => new Guid(Encoding.UTF8.GetBytes((Id ?? "NoId").PadRight(16)));
            set => Id = Encoding.UTF8.GetString(value.GetValueOrDefault().ToByteArray());
        }

        public string Name { get; set; }

        public Guid? Account { get; set; }
    }
}
