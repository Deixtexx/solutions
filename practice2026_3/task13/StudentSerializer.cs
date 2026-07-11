using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13
{
    public static class StudentSerializer
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
        };

        public static string Serialize(Student student)
        {
            return JsonSerializer.Serialize(student, Options);
        }

        public static void SaveToFile(Student student, string filePath)
        {
            string json = Serialize(student);
            File.WriteAllText(filePath, json);
        }

        public static Student LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            string json = File.ReadAllText(filePath);
            return DeserializeAndValidate(json);
        }
        public static Student DeserializeAndValidate(string json)
        {
            var student = JsonSerializer.Deserialize<Student>(json, Options);
            if (student == null) throw new JsonException("данные пусты");
            ValidateStudent(student);
            return student;
        }

        private static void ValidateStudent(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FirstName) || string.IsNullOrWhiteSpace(student.LastName))
            {
                throw new ArgumentException("имя (FirstName) и Фамилия (LastName) не могут быть пустыми");
            }

            if (student.BirthDate > DateTime.Now || student.BirthDate < DateTime.Now.AddYears(-120))
            {
                throw new ArgumentException("указана некорректная дата рождения");
            }

            if (student.Grades != null)
            {
                foreach (var subject in student.Grades)
                {
                    if (string.IsNullOrWhiteSpace(subject.Name))
                        throw new ArgumentException("название предмета не может быть пустым");

                    if (subject.Grade < 1 || subject.Grade > 100) // по 100-балльной системе
                        throw new ArgumentException($"некорректная оценка '{subject.Grade}' по предмету {subject.Name}, она должна быть от 1 до 100");
                }
            }
        }
    }
}
