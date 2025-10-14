using AgendaPro.Domain.Tags.Models;

namespace AgendaPro.Application.Tags.Dtos
{
    public class TagDto
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public TagDto()
        {

        }

        public TagDto(TagModel model)
        {
            Id = model.Id;
            Name = model.Name;
        }
    }
}
