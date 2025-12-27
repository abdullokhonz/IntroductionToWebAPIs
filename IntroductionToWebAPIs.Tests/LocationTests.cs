using IntroductionToWebAPIs.Entity;

namespace IntroductionToWebAPIs.Tests
{
    public class LocationTests
    {
        [Fact]
        public void Location_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Test Location";
            var description = "A location for testing purposes.";
            var parentId = Guid.NewGuid();
            var parent = new Location();
            var children = new List<Location>();

            // Act
            var location = new Location
            {
                Id = id,
                Name = name,
                Description = description,
                ParentId = parentId,
                Parent = parent,
                Children = children
            };

            // Assert
            Assert.Equal(id, location.Id);
            Assert.Equal(name, location.Name);
            Assert.Equal(description, location.Description);
            Assert.Equal(parentId, location.ParentId);
            Assert.Equal(parent, location.Parent);
            Assert.Equal(children, location.Children);
        }

        [Fact]
        public void Location_Children_InitializesAsEmptyList()
        {
            // Act
            var location = new Location();
            // Assert
            Assert.NotNull(location.Children);
            Assert.Empty(location.Children);
        }

        [Fact]
        public void Location_ParentId_CanBeNull()
        {
            // Act
            var location = new Location();
            // Assert
            Assert.Null(location.ParentId);
        }

        [Fact]
        public void Location_Description_CanBeNull()
        {
            // Act
            var location = new Location();
            // Assert
            Assert.Null(location.Description);
        }

        [Fact]
        public void Location_Name_DefaultsToEmptyString()
        {
            // Act
            var location = new Location();
            // Assert
            Assert.Equal(string.Empty, location.Name);
        }

        [Fact]
        public void Location_Id_IsGuid()
        {
            // Act
            var location = new Location();
            // Assert
            Assert.IsType<Guid>(location.Id);
        }

        [Fact]
        public void Location_DefaultConstructor_SetsDefaultValues()
        {
            // Act
            var location = new Location();

            // Assert
            Assert.NotEqual(Guid.Empty, location.Id);
            Assert.Equal(string.Empty, location.Name);
            Assert.Null(location.Description);
            Assert.Null(location.ParentId);
            Assert.Null(location.Parent);
            Assert.Empty(location.Children);
        }
    }
}
