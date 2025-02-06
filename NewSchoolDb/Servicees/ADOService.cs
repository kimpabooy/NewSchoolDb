using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NewSchoolDb.Data;
using NewSchoolDb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSchoolDb.Servicees
{
    public class ADOService
    {
        private readonly string _connectionstring = "Data Source = localhost;Database = NewSchoolDb;Trusted_Connection=True;Trust server certificate = true;";
        
        public void GetStaff()
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();

                string query = @"SELECT 
                                CONCAT(FirstName, ' ', LastName) AS [Name],
                                r.RoleName, 
                                DATEDIFF(YEAR, s.YearsWorked, GETDATE()) AS YearsWorked
                                FROM Staff s
                                JOIN Role r ON s.Role_ID = r.RoleID";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Console.WriteLine("   Namn    Roll      År på skolan");
                        Console.WriteLine("--------------------------------");

                        while (reader.Read())
                        {
                            Console.WriteLine($"Namn: {reader["Name"]}\nRoll: {reader["RoleName"]}\nÅr Anställd: {reader["YearsWorked"]}\n");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
                bool keepGoing = true;

                while (keepGoing)
                {
                    Console.WriteLine("Vill du lägga till en ny anställd? (Ja/Nej)");
                    string userChoice = Console.ReadLine().ToLower();
                    
                    if (userChoice == "nej")
                    {
                        break;
                    }
                    else if (userChoice == "ja")
                    {
                        AddStaff();
                    }
                }
            }
        }

        public void AddStaff()
        {
            // Skapar en SQL-anslutning
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                string firstName;
                string lastName;

                while (true)
                {
                    Console.WriteLine("Vänligen ange informationen nedan\n");
                    Console.Write("Förnamn: ");
                    firstName = Console.ReadLine()?.Trim();
                    if (firstName.Length > 2 && firstName.Length < 56)
                    {
                        break;
                    }
                    else if (firstName.Length <= 2)
                    {
                        Console.WriteLine("Ditt namn måste ha minst 3 bokstäver");
                    }
                    else if (firstName.Length >= 55)
                    {
                        Console.WriteLine("Ditt namn är för långt, kan inte vara längre än 55 bokstäver");
                    }
                }

                while (true)
                {
                    Console.Write("Efternamn: ");
                    lastName = Console.ReadLine()?.Trim();
                    if (lastName.Length > 2 && lastName.Length < 56)
                    {
                        break;
                    }
                    else if (lastName.Length <= 2)
                    {
                        Console.WriteLine("Ditt efternamn måste ha minst 3 bokstäver");
                    }
                    else if (lastName.Length >= 55)
                    {
                        Console.WriteLine("Ditt efternamn är för långt, kan inte vara längre än 55 bokstäver");
                    }
                }

                Console.Write("Startdatum (YYYY-MM-DD): ");
                DateTime yearsWorked;

                //Checks if the inputformat is correct
                while (true)
                {
                    string input = Console.ReadLine();
                    if (DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out yearsWorked))
                    {
                        break;
                    }
                    Console.WriteLine("Fel format! Ange datum i formatet YYYY-MM-DD.");
                }

                string roleQuery = "SELECT RoleID, RoleName" +
                    "               FROM Role";

                // Open a connection for role search
                SqlCommand rolesCommand = new SqlCommand(roleQuery, connection);
                connection.Open();

                SqlDataReader reader = rolesCommand.ExecuteReader();

                // Shows available roles
                Console.WriteLine("\nTillgängliga roller:");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader[0]} - {reader[1]}");
                }

                int roleId;
                while (true)
                {
                    Console.Write("Välj Roll genom ID: ");
                    string roleInput = Console.ReadLine();

                    if (int.TryParse(roleInput, out roleId))
                    {
                        break;
                    }
                    Console.WriteLine("Ogiltigt ID. Ange ett existerande roll-ID.");
                }
                // Closing the role connection
                reader.Close();

                int departmentId = roleId;

                try
                {
                    string query = @"INSERT INTO Staff (FirstName, LastName, YearsWorked, Role_ID, Department_ID) 
                             VALUES (@FirstName, @LastName, @YearsWorked, @Role_ID, @Department_ID)";

                    SqlCommand command = new SqlCommand(query, connection);

                    //int rowsAffected = 0; // DEBUGG LÄGE

                    // Adding the parameters, SQL commands
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@YearsWorked", yearsWorked);
                    command.Parameters.AddWithValue("@Role_ID", roleId);
                    command.Parameters.AddWithValue("@Department_ID", departmentId);

                    //Executing the commands
                    int rowsAffected = command.ExecuteNonQuery();

                    // Checks if any rows are affected
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("\n**** Personal tillagd ****");
                    }
                    else
                    {
                        Console.WriteLine("\nDet gick inte att lägga till personalen.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel inträffade vid tilldelning av personal. ({ex.Message})");
                }
            }

            Console.WriteLine("\nTryck på valfri tangent...");
            Console.ReadKey();

        }

        
        public void GetStudentGrade()
        {
            Console.WriteLine("Ange elevens ID: (Förslagsvis 13, 30 eller 39");
            int studentId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                
                var query = @$"SELECT 
                            CONCAT(s.FirstName, ' ', s.LastName) AS Student,
                            CONCAT(st.FirstName, ' ', st.LastName) AS Lärare,
                            sub.SubjectName AS Ämne,
                            g.Grade AS Betyg,
                            g.GradeDate AS Betygsdatum
                            FROM Grade g
                            INNER JOIN Student s ON g.Student_ID = s.StudentID
                            INNER JOIN Subject sub ON g.Subject_ID = sub.SubjectID
                            INNER JOIN Staff st ON g.Staff_ID = st.StaffID
                            WHERE s.StudentID = @studentId
                            ORDER BY g.GradeDate DESC;";

                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@studentId", studentId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"Student: {reader["Student"]}, Ämne: {reader["Ämne"]}, Betyg: {reader["Betyg"]}, Lärare: {reader["Lärare"]}, Datum: {reader["Betygsdatum"]}");
                }
            }
            Console.ReadKey();
        }

        public void GetSalary()
        {
            string query = @"
                   SELECT 
                       d.DepartmentName AS Avdelning, 
                       SUM(d.Salary) AS Månadskostnad,
                       AVG(d.Salary) AS Medellön
                   FROM Department d
                   JOIN Staff s ON d.DepartmentID = s.Department_ID
                   GROUP BY d.DepartmentID, d.DepartmentName;";

            // Combined Sum and Average Salary
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                //Console.WriteLine("Avdelning | Månadskostnad | Medellön");
                Console.WriteLine("-----Avdelningskostnader-----");
                Console.WriteLine("---------------------------------------");

                while (reader.Read())
                {
                    Console.WriteLine($"Avdelning: {reader["Avdelning"]}\nMånadskostnad: {reader["Månadskostnad"]} SEK\nMedellönen: {Math.Round((decimal)reader["Medellön"], 2)} SEK"); // Limits the decimals to 2
                    Console.WriteLine("---------------------------------------");
                }
            }
            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
        }

        public void GetStudentById()
        {
            /*
             string query = @"CREATE PROCEDURE GetStudentInfo
                              @StudentID INT
                          AS
                          BEGIN
                              SELECT 
                                  s.StudentID,
                                  CONCAT(s.FirstName, ' ', s.LastName) AS Student,
                                  CONCAT(st.FirstName, ' ', st.LastName) AS Lärare,
                                  c.ClassName AS Klass,
                                  gr.Grade AS Betyg,
                                  sub.SubjectName AS Ämne
                              FROM 
                              Student s
                              LEFT JOIN Class c ON s.Class_ID = c.ClassID -- Klassnamn för student
                              LEFT JOIN Grade gr ON s.StudentID = gr.Student_ID -- Betyg för studenten
                              LEFT JOIN Subject sub ON gr.Subject_ID = sub.SubjectID -- Namn på ämnet för varje betyg.
                              LEFT JOIN Staff st ON gr.Staff_ID = st.StaffID -- Läraren som gav betyget
                              WHERE s.StudentID = @StudentID;
                          END;";
             */

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetStudentInfo", connection))
                {
                    Console.WriteLine("----Studentinformation----\n");
                    Console.WriteLine("Välj önskat Student ID: (Förslagsvis 13, 30 eller 39\"");
                    Console.Write("Val: ");
                    int studentId = Convert.ToInt32(Console.ReadLine());


                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentID", studentId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Student: {reader["Student"]}");
                            Console.WriteLine($"Klass: {reader["Klass"]}");
                            Console.WriteLine($"Lärare: {reader["Lärare"]}");
                            Console.WriteLine($"Ämne: {reader["Ämne"]}");
                            Console.WriteLine($"Betyg: {reader["Betyg"]}");
                            Console.WriteLine();
                        }
                    }
                }
            }
            Console.ReadKey();
        }

        public void GustavsMetod()
        {

            // Only run once 🙂 
            // Creates "StudentGrade" objects/rows to use in database
            // Uses DB Tables: 
            // "Students" (A Student)
            // "Course" t.ex. (A specific course with connection to class and subject - for example "7A ENGLISH".

            // Gets all courses, then (via connection to Class table) students, and creates a random grade (between 1 and 4) for each course, for each student, then saves it.
            // There are FK connections between teacher/course, studentgrade/gradescale, studentgrade/student etc. etc. - these need to be in place first. 


            //    var r = new Random();
            //    using (var context = new Labb2Context())
            //    {
            //        var coursesToGrade = context.Courses
            //            .Include(cl => cl.CurrentClass)
            //            .ThenInclude(st => st.Students)
            //            .ToList();


            //        foreach (Course c in coursesToGrade)
            //        {
            //            var grades = new List<StudentGrade>();
            //            foreach (Student s in c.CurrentClass.Students)
            //            {
            //                var newGrade = new StudentGrade()
            //                {
            //                    SetDate = DateOnly.FromDateTime(DateTime.Now),
            //                    StaffId = c.TeacherId,
            //                    StudentId = s.Id,
            //                    GradeScaleId = r.Next(1, 5), // change min/max number to your "grade id:s"
            //                    CourseId = c.Id
            //                };
            //                grades.Add(newGrade);
            //            }
            //            context.AddRange(grades);
            //        }

            //        context.SaveChanges();

            //    }
        }


        public void AddGradeToStudent()
        {
            Random random = new Random();
            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Open();

                
                string query = @"INSERT INTO Grade (GradeID, Grade, GradeDate, Student_ID, Subject_ID, Staff_ID) VALUES
                (@GradeID, @Grade, @GradeDate, @Student_ID, @Subject_ID, @Staff_ID)";

                var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@GradeID", );
                command.Parameters.AddWithValue("@Grade", random.Next(1, 5)); // A-F???
                command.Parameters.AddWithValue("@GradeDate", DateTime.Now.Date);
                command.Parameters.AddWithValue("@Student_ID", ); 
                command.Parameters.AddWithValue("@Subject_ID", ); 
                command.Parameters.AddWithValue("@Staff_ID", );

                //command.ExecuteNonQuery();
                
                
            }
        }




    }
}

