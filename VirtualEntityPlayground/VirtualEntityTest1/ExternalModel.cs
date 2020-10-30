using System;
using System.Text;

namespace VirtualEntityTest1
{
    public class ExternalModel
    {
        public string Id { get; set; }

        public Guid? Guid
        {
            get => new Guid(Encoding.UTF8.GetBytes(Id?.PadRight(16) ?? "NoId"));
            set => Id = Encoding.UTF8.GetString(value.GetValueOrDefault().ToByteArray());
        }

        public string Name { get; set; }

        public Guid? Parent { get; set; }
    }
}
