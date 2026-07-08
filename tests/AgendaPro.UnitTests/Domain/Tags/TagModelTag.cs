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
            public void Constructor_ShouldSetNameAndId()
            {
                // Arrange
                var name = "TestTag";
                // Act
                var tag = new TagModel(name);

                // Assert
                Assert.Equal(name, tag.Name);
                Assert.NotEqual(Guid.Empty, tag.Id);
            }
        }
    }
}
