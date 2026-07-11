using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using task13;

namespace task13tests
{
    public class StudentSerializationTests
    {
        [Fact]
        public void Serialize_ValidStudent_AppliesCustomDateFormatAndIgnoresNull()
        {
            var student = new Student
            {
                FirstName = "Олег",
                LastName = "Петров",
                BirthDate = new DateTime(2002, 11, 25),
                Grades = null
            };

            string json = StudentSerializer.Serialize(student);


            Assert.Contains("\"BirthDate\": \"2002-11-25\"", json);
            Assert.DoesNotContain("Grades", json);
        }

        [Fact]
        public void Deserialize_ValidJson_ReturnsCorrectObject()
        {
            string json = @"
            {
                ""FirstName"": ""Анна"",
                ""LastName"": ""Сидорова"",
                ""BirthDate"": ""2004-03-15"",
                ""Grades"": [
                    { ""Name"": ""Физика"", ""Grade"": 90 }
                ]
            }";

            Student student = StudentSerializer.DeserializeAndValidate(json);

            Assert.Equal("Анна", student.FirstName);
            Assert.Equal("Сидорова", student.LastName);
            Assert.Equal(new DateTime(2004, 3, 15), student.BirthDate);
            Assert.Single(student.Grades);
            Assert.Equal("Физика", student.Grades[0].Name);
            Assert.Equal(90, student.Grades[0].Grade);
        }

        [Theory]
        [InlineData("", "Иванов")]
        [InlineData("Иван", "  ")]
        public void Deserialize_EmptyNames_ThrowsArgumentException(string firstName, string lastName)
        {
            string json = $@"
            {{
                ""FirstName"": ""{firstName}"",
                ""LastName"": ""{lastName}"",
                ""BirthDate"": ""2000-01-01""
            }}";

            var exception = Assert.Throws<ArgumentException>(() =>
                StudentSerializer.DeserializeAndValidate(json)
            );

            Assert.Contains("имя (FirstName) и Фамилия (LastName) не могут быть пустыми", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void Deserialize_InvalidGradeRange_ThrowsArgumentException(int invalidGrade)
        {
            string json = $@"
            {{
                ""FirstName"": ""Петр"",
                ""LastName"": ""Николаев"",
                ""BirthDate"": ""2001-05-10"",
                ""Grades"": [
                    {{ ""Name"": ""Химия"", ""Grade"": {invalidGrade} }}
                ]
            }}";

            var exception = Assert.Throws<ArgumentException>(() =>
                StudentSerializer.DeserializeAndValidate(json)
            );

            Assert.Contains("некорректная оценка", exception.Message);
        }

        [Fact]
        public void Deserialize_EmptySubjectName_ThrowsArgumentException()
        {
            string json = @"
            {
                ""FirstName"": ""Игорь"",
                ""LastName"": ""Волков"",
                ""BirthDate"": ""1999-08-20"",
                ""Grades"": [
                    { ""Name"": ""   "", ""Grade"": 85 }
                ]
            }";

            var exception = Assert.Throws<ArgumentException>(() =>
                StudentSerializer.DeserializeAndValidate(json)
            );

            Assert.Contains("название предмета не может быть пустым", exception.Message);
        }

        [Fact]
        public void Deserialize_FutureBirthDate_ThrowsArgumentException()
        {
            string futureDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string json = $@"
            {{
                ""FirstName"": ""Максим"",
                ""LastName"": ""Зайцев"",
                ""BirthDate"": ""{futureDate}""
            }}";

            var exception = Assert.Throws<ArgumentException>(() =>
                StudentSerializer.DeserializeAndValidate(json)
            );

            Assert.Contains("указана некорректная дата рождения", exception.Message);
        }

        [Fact]
        public void Deserialize_BadDateFormat_ThrowsFormatException()
        {
            string json = @"
            {
                ""FirstName"": ""Мария"",
                ""LastName"": ""Крылова"",
                ""BirthDate"": ""31/12/2000""
            }";

            Assert.Throws<FormatException>(() =>
                StudentSerializer.DeserializeAndValidate(json)
            );
        }
    }

}
