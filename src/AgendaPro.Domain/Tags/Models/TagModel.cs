using AgendaPro.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaPro.Domain.Tags.Models
{
    public class TagModel : AuditableEntity
    {
        public string Name { get; protected set; }

        public TagModel(string name, Guid createdBy) : base(createdBy)
        {
            Name = name;
        }
    }
}
