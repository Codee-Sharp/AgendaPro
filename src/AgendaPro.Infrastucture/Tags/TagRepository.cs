using AgendaPro.Domain.Tags.Models;
using AgendaPro.Domain.Tags.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaPro.Infrastucture.Tags
{
    public class TagRepository : ITagRepository
    {
        public Task SaveAsync(TagModel model)
        {
            return Task.CompletedTask;
        }
    }
}
