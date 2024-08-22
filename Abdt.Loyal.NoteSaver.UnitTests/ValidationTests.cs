using Abdt.Loyal.NoteSaver.BusinessLogic.Validation;
using Abdt.Loyal.NoteSaver.Domain;

namespace Abdt.Loyal.NoteSaver.UnitTests
{
    public class ValidationTests
    {
        [Fact]
        public void Valid_Note_Is_Valid()
        {
            // Arrange
            var validator = new Validator();
            var note = GetValidNote();

            // Act
            var result = validator.Validate(note);

            // Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Note_Title_Is_Not_Empty(string testTitle)
        {
            // Arrange
            var validator = new Validator();
            var note = GetValidNote();
            note.Title = testTitle;

            // Act
            var result = validator.Validate(note);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(1, result.Errors.Count);
        }

        [Theory]
        [InlineData('a', 5, true)]
        [InlineData('a', 49, true)]
        [InlineData('a', 50, true)]
        [InlineData('a', 51, false)]
        [InlineData('a', 60, false)]
        [InlineData('a', 120, false)]
        public void Note_Title_MaximumLength_50(char character, int length, bool expected)
        {
            // Arrange
            var validator = new Validator();
            var note = GetValidNote();
            var testTitle = new string(character, length);
            note.Title = testTitle;

            // Act
            var result = validator.Validate(note);

            // Assert
            Assert.Equal(result.IsValid, expected);
            Assert.Equal(expected ? 0 : 1, result.Errors.Count);
        }

        [Theory]
        [InlineData('a', 5, true)]
        [InlineData('a', 100, true)]
        [InlineData('a', 500, true)]
        [InlineData('a', 1999, true)]
        [InlineData('a', 2000, true)]
        [InlineData('a', 2001, false)]
        [InlineData('a', 2107, false)]
        public void Note_Content_MaximumLength_2000(char character, int length, bool expected)
        {
            // Arrange
            var validator = new Validator();
            var note = GetValidNote();
            var testContent = new string(character, length);
            note.Content = testContent;

            // Act
            var result = validator.Validate(note);

            // Assert
            Assert.Equal(result.IsValid, expected);
            Assert.Equal(expected ? 0 : 1, result.Errors.Count);
        }

        [Theory]
        [InlineData('a', 5, 100, true, 0)]
        [InlineData('a', 500, 1000, false, 1)]
        [InlineData('a', 1, 2005, false, 1)]
        [InlineData('a', 51, 2001, false, 2)]
        [InlineData('a', 0, 0, false, 1)]
        public void Note_Content_and_Title_MaximumLength(char character, int titleLength, int contentLength, bool expected, int errorsCount)
        {
            // Arrange
            var validator = new Validator();
            var note = GetValidNote();
            var testTitle = new string(character, titleLength);
            var testContent = new string(character, contentLength);
            note.Title = testTitle;
            note.Content = testContent;

            // Act
            var result = validator.Validate(note);

            // Assert
            Assert.Equal(result.IsValid, expected);
            Assert.Equal(errorsCount, result.Errors.Count);
        }

        [Fact]
        public void Validator_Throws_ArgumentNullEx_When_Note_Is_Null()
        {
            // Arrange
            var validator = new Validator();

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => validator.Validate((Note)null));
        }

        private Note GetValidNote() 
        {
            return new Note
            {
                Id = 1,
                Title = "first note",
                Content = "-",
            };
        }
    }
}