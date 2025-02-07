using Microsoft.EntityFrameworkCore;
using NewSchoolDb.Data;

namespace NewSchoolDb.Servicees
{
    public class EFService
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

        public void GetActiveCourse()
        {
            var today = DateTime.Now;

            using (var context = new NewSchoolDbContext())
            {
                var courses = context.Courses
                    .Where(c => c.EndDate < today) // Get active courses if EndDate is after todays date.
                    .Include(c => c.CourseName)
                    .Include(c => c.Subject)
                    .ToList();

                var count = 1;
                foreach (var course in courses)
                {
                    Console.WriteLine($"#{count} Kurs: {course.CourseName.CourseName1}{course.CourseId}, Ämne: {course.Subject.SubjectName}, Slutdatum: {course.EndDate}");
                    count++;
                }
            }
            Console.ReadKey();
        }

    }
}
