using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace task13
{
    public class Student
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime BirthDate { get; set; }
        public List<Subject>? Grades { get; set; }
    }
}
