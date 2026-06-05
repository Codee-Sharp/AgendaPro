using AgendaPro.Domain.Tags.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaPro.UnitTests.Domain.Tags
{
    public class TagModelTag
    {
        public class TagModelTests
        {
            [Fact]
            public void Constructor_ShouldSetNameAndCreatedBy()
            {
                // Arrange
                var name = "TestTag";
                var createdBy = Guid.NewGuid();

                // Act
                var tag = new TagModel(name, createdBy);

                // Assert
                Assert.Equal(name, tag.Name);
                Assert.Equal(createdBy, tag.CreatedBy);
                Assert.False(tag.IsDeleted);
                Assert.NotEqual(Guid.Empty, tag.Id);
                Assert.True(tag.CreatedAt <= DateTimeOffset.UtcNow);
            }
        }
    }
}
