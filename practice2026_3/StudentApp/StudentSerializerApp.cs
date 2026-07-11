using System;
using System.Collections.Generic;
using task13;

namespace StudentSerializerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "student.json";

            Student student = new()
            {
                FirstName = "Иван",
                LastName = "Иванов",
                BirthDate = new DateTime(2007, 6, 12),
                Grades = new List<Subject>
                {
                    new() { Name = "Дискретная математика", Grade = 95 },
                    new() { Name = "Математическая логика", Grade = 100 }
                }
            };

            Console.WriteLine($"Студент: {student.FirstName} {student.LastName}");

            try
            {
                // сериализуем и сохраняем в файл
                StudentSerializer.SaveToFile(student, filePath);
                Console.WriteLine($"Данные успешно сохранены в файл: {filePath}");

                // выведем сгенерированный JSON в консоль для демонстрации форматирования
                string jsonContent = StudentSerializer.Serialize(student);
                Console.WriteLine("Содержимое JSON:");
                Console.WriteLine(jsonContent);

                // загружаем и десериализуем обратно
                Console.WriteLine("\nЗагрузка данных из файла и десериализация");
                Student loadedStudent = StudentSerializer.LoadFromFile(filePath);

                Console.WriteLine("Объект успешно восстановлен и прошел валидацию");
                Console.WriteLine($"Имя: {loadedStudent.FirstName}");
                Console.WriteLine($"Фамилия: {loadedStudent.LastName}");
                Console.WriteLine($"Дата рождения: {loadedStudent.BirthDate:yyyy-MM-dd}");

                if (loadedStudent.Grades != null)
                {
                    Console.WriteLine("Оценки:");
                    foreach (var subject in loadedStudent.Grades)
                    {
                        Console.WriteLine($" - {subject.Name}: {subject.Grade}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}