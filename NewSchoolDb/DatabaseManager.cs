using Microsoft.EntityFrameworkCore;
using NewSchoolDb.Data;
using NewSchoolDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSchoolDb
{
    public class DatabaseManager
    {
        public void GetTeacherAmount()
        {
            using (var context = new NewSchoolDbContext())
            {
                var department = context.Staff
                    .Include(s => s.Department)
                    .GroupBy(s => s.Department.DepartmentName)
                    .Select(g => new
                    {
                        Name = g.Key,
                        Amount = g.Count()
                    })
                    .ToList();

                foreach (var dep in department)
                {
                    Console.WriteLine($"{dep.Name} - {dep.Amount}");
                }
                Console.ReadKey();
            }
        }

        public void GetStudentInfo()
        {
            using (NewSchoolDbContext context = new NewSchoolDbContext())
            {
                var students = context.Students
                    .Include(s => s.Class)
                    .Include(s => s.Grades)
                    //.Where(s => new[] { "7A", "7B", "7C", "8A", "8B", "8C", "9A", "9B", "9C", "9D" }.Contains(s.Class.ClassName))
                    .OrderBy(s => s.Class.ClassName)
                    .ToList();

                // Group students by class
                var groupedStudents = students.GroupBy(s => s.Class.ClassName);

                foreach (var classGroup in groupedStudents)
                {
                    Console.WriteLine($"\n---- Klass {classGroup.Key} ----");  // Displys classname

                    var count = 0;
                    foreach (var student in classGroup)
                    {
                        count++;
                        Console.WriteLine($"#{count} | {student.FirstName} {student.LastName.PadRight(15)} | {student.SecurityNum}");
                    }
                }
                Console.ReadKey();
            }
        }

        
    }
}
