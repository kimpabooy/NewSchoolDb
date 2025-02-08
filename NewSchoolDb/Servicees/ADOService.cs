using Microsoft.Data.SqlClient;
using NewSchoolDb.Models;
using System.Data;
using System.Reflection.PortableExecutable;


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
                                CONCAT(s.FirstName, ' ', s.LastName) AS [Name],
                                r.RoleName, 
                                DATEDIFF(YEAR, s.YearsWorked, GETDATE()) AS YearsWorked
                                FROM Staff s
                                JOIN Role r ON s.Role_ID = r.RoleID";

                SqlCommand command = new SqlCommand(query, connection);

                
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("--------------Personalöversikt--------------");

                    while (reader.Read())
                    {
                        Console.WriteLine($"Namn: {reader["Name"]}\nRoll: {reader["RoleName"]}\nÅr Anställd på skolan: {reader["YearsWorked"]}\n");
                    }
                }
                
                bool keepGoing = true;
                while (keepGoing)
                {
                    Console.WriteLine("\nVill du lägga till en ny anställd? (Ja/Nej)");
                    Console.Write("Val: ");
                    string userChoice = Console.ReadLine().ToLower();
                    
                    if (userChoice == "nej")
                    {
                        break;
                    }
                    else if (userChoice == "ja")
                    {
                        AddStaff();
                    }
                    else
                    {
                        Console.WriteLine("Fel inmatning, prova Ja / Nej\n");
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


                    // Adding the parameters, SQL commands
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@YearsWorked", yearsWorked);
                    command.Parameters.AddWithValue("@Role_ID", roleId);
                    command.Parameters.AddWithValue("@Department_ID", departmentId);

                    //Executing the commands
                    //int rowsAffected = 0; // DEBUGG MODE
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

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();

                string queryGetStudent = @"SELECT
                                            s.StudentID AS ID,
                                            CONCAT(s.FirstName, ' ', s.LastName) AS Student
                                           FROM Student s";

                Console.WriteLine("--------------Studentöversikt--------------");
                SqlCommand commandGetStudent = new SqlCommand(queryGetStudent, connection);

                SqlDataReader readerGetStudent = commandGetStudent.ExecuteReader();

                List<int> validStudentID = new List<int>();
                while (readerGetStudent.Read())
                {
                    int id = (int)readerGetStudent["ID"];
                    validStudentID.Add(id);
                    Console.WriteLine($"ID: {readerGetStudent["ID"]} {readerGetStudent["Student"]}");
                }
                readerGetStudent.Close();

                int studentId;
                do
                {
                    Console.WriteLine("\nAnge Student ID");
                    Console.Write("Val: ");

                    while (!int.TryParse(Console.ReadLine(), out studentId))
                    {
                        Console.WriteLine("Felaktig input! Ange en giltig siffra: ");
                        Console.Write("Val: ");
                    }

                    if (!validStudentID.Contains(studentId))
                    {
                        Console.WriteLine("Fel! ID finns inte i listan. Försök igen.");
                    }

                } while (!validStudentID.Contains(studentId));
                Console.Clear();

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

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@studentId", studentId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"Student: {reader["Student"]}, Ämne: {reader["Ämne"]}, Betyg: {reader["Betyg"]}, Lärare: {reader["Lärare"]}, Betygsdatum: {reader["Betygsdatum"]}");
                }
            }
            Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn...");
            Console.ReadKey();
        }

        public void GetSalary()
        {
            // Combined Sum and Average Salary
            string query = @"
                   SELECT 
                       d.DepartmentName AS Avdelning, 
                       SUM(d.Salary) AS Månadskostnad,
                       AVG(d.Salary) AS Medellön
                   FROM Department d
                   JOIN Staff s ON d.DepartmentID = s.Department_ID
                   GROUP BY d.DepartmentID, d.DepartmentName;";

            
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

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
             * 
                                            *** This is how i made my Store Procedure ***

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
                              LEFT JOIN Class c ON s.Class_ID = c.ClassID -- Class Name for student.
                              LEFT JOIN Grade gr ON s.StudentID = gr.Student_ID -- Grade for student.
                              LEFT JOIN Subject sub ON gr.Subject_ID = sub.SubjectID -- Name for each subject in grades.
                              LEFT JOIN Staff st ON gr.Staff_ID = st.StaffID -- The teacher who set the grade.
                              WHERE s.StudentID = @StudentID;
                          END;";
             */

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();

                Console.WriteLine("----Studentinformation----\n");

                string queryGetStudent = @"SELECT
                                            s.StudentID AS ID,
                                            CONCAT(s.FirstName, ' ', s.LastName) AS Student
                                           FROM Student s";

                SqlCommand commandGetStudent = new SqlCommand(queryGetStudent, connection);
                SqlDataReader readerGetStudent = commandGetStudent.ExecuteReader();
                
                List<int> validStudentID = new List<int>();
                while (readerGetStudent.Read())
                {
                    int id = (int)readerGetStudent["ID"];
                    validStudentID.Add(id);
                    Console.WriteLine($"ID: {readerGetStudent["ID"]} {readerGetStudent["Student"]}");
                }

                // Checks if student ID excists
                int studentId;
                do
                {
                    Console.WriteLine("\nAnge Student ID");
                    Console.Write("Val: ");

                    while (!int.TryParse(Console.ReadLine(), out studentId))
                    {
                        Console.WriteLine("Felaktig input! Ange en giltig siffra: ");
                        Console.Write("Val: ");
                    }

                    if (!validStudentID.Contains(studentId))
                    {
                        Console.WriteLine("Fel! ID finns inte i listan. Försök igen.");
                    }

                } while (!validStudentID.Contains(studentId));
                Console.Clear();

                readerGetStudent.Close();

                using (SqlCommand command = new SqlCommand("GetStudentInfo", connection))
                {
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

        public void AddGradeToStudent()
        {
            string[] validGrades = { "A", "B", "C", "D", "E", "F" };

            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Open();

                // Get Student information
                string queryGetStudentInfo = @"SELECT s.StudentID AS ID,
                                                CONCAT(s.FirstName, ' ', s.LastName) AS Student
                                              FROM Student s";

                SqlCommand command = new SqlCommand(queryGetStudentInfo, connection);
                SqlDataReader reader = command.ExecuteReader();


                List<int> validStudentID = new List<int>();
                while (reader.Read())
                {
                    int id = (int)reader["ID"];
                    validStudentID.Add(id);
                    Console.WriteLine($"ID: {reader["ID"]} {reader["Student"]}");
                }
                reader.Close();

                int studentId;
                do
                {
                    Console.WriteLine("\nAnge Student ID");
                    Console.Write("Val: ");

                    while (!int.TryParse(Console.ReadLine(), out studentId))
                    {
                        Console.WriteLine("Felaktig input! Ange en giltig siffra: ");
                        Console.Write("Val: ");
                    }

                    if (!validStudentID.Contains(studentId))
                    {
                        Console.WriteLine("Fel! ID finns inte i listan. Försök igen.");
                    }

                } while (!validStudentID.Contains(studentId));
                Console.Clear();


                // Get Subjects
                string queryGetSubject = @"SELECT s.SubjectID AS ID,
                                             s.SubjectName AS Ämne
                                           FROM Subject s";
                
                command.CommandText = queryGetSubject;
                reader = command.ExecuteReader();

                List<int> validSubjectIds = new List<int>(); // Saves subjectID to a list.

                // Checks if subject excists with the input from user.
                int subjectId;
                while (reader.Read())
                {
                    int id = (int)reader["ID"];
                    validSubjectIds.Add(id);

                    Console.WriteLine($"ID: {reader["ID"]} Ämne: {reader["Ämne"]}");
                }
                reader.Close();

                do
                {
                    Console.Write("\nVälj ett Ämne (ID): ");
                    while (!int.TryParse(Console.ReadLine(), out subjectId))
                    {
                        Console.Write("Felaktig input! Ange en giltig siffra: ");
                    }

                    if (!validSubjectIds.Contains(subjectId))
                    {
                        Console.WriteLine("Fel! ID finns inte i listan. Försök igen.");
                    }
                } while (!validSubjectIds.Contains(subjectId));
                Console.Clear();


                // Get Teatchers
                string queryGetStaff = @"SELECT s.StaffID AS ID,
                                            CONCAT(s.FirstName, ' ', s.LastName) AS Lärare
                                         FROM Staff s
                                         WHERE s.Department_ID = 1;";
                command.CommandText = queryGetStaff;
                reader = command.ExecuteReader();

                List<int> validTeacherID = new List<int>();
                while (reader.Read())
                {
                    int id = (int)reader["ID"];
                    validTeacherID.Add(id);
                    Console.WriteLine($"ID: {reader["ID"]} {reader["Lärare"]}");
                }
                reader.Close();

                int staffId;
                do
                {
                    Console.WriteLine("\nAnge Student ID");
                    Console.Write("Val: ");

                    while (!int.TryParse(Console.ReadLine(), out staffId))
                    {
                        Console.WriteLine("Felaktig input! Ange en giltig siffra: ");
                        Console.Write("Val: ");
                    }

                    if (!validTeacherID.Contains(staffId))
                    {
                        Console.WriteLine("Fel! ID finns inte i listan. Försök igen.");
                    }

                } while (!validTeacherID.Contains(staffId));
                Console.Clear();

                
                // Get Grades
                Console.WriteLine("Tillgängliga betyg:\n A\n B\n C\n D\n E\n F\n");
                Console.Write("\nVälj ett betyg: ");
                string grade = Console.ReadLine().ToUpper();

                while (!validGrades.Contains(grade))
                {
                    Console.Write("Felaktigt betyg! Välj A-F: ");
                    grade = Console.ReadLine().ToUpper();
                }
                Console.Clear();

                // Checks if user wants to continue with the provided inputs
                bool keepGoing = true;
                while (keepGoing)
                {
                    Console.WriteLine("**** Sumering ****\n");

                    Console.WriteLine($"StudentID: {studentId}");
                    Console.WriteLine($"SubjectID: {subjectId}");
                    Console.WriteLine($"StaffID: {staffId}");
                    Console.WriteLine($"Grade: {grade}");
                    Console.WriteLine($"Datum: {DateTime.Now}");

                    Console.WriteLine("Är du säker på att du vill lägga till följande information (Ja / Nej)");
                    string userChoice = Console.ReadLine().ToUpper();

                    if (userChoice == "JA")
                    {
                        // Transaktion query
                        string queryTransaction = @"
                                                  BEGIN TRANSACTION;
                                                  BEGIN TRY
                                                      INSERT INTO Grade (Grade, GradeDate, Student_ID, Subject_ID, Staff_ID)
                                                      VALUES (@Grade, @GradeDate, @Student_ID, @Subject_ID, @Staff_ID);
                                                      COMMIT TRANSACTION;
                                                      PRINT 'Data har satts in framgångsrikt!';
                                                  END TRY
                                                  BEGIN CATCH
                                                      ROLLBACK TRANSACTION;
                                                      PRINT 'Fel inträffade: ' + ERROR_MESSAGE();
                                                  END CATCH;";

                        // Adds the grade to the database
                        using (var commandTransaction = new SqlCommand(queryTransaction, connection))
                        {
                            commandTransaction.Parameters.AddWithValue("@Grade", grade);
                            commandTransaction.Parameters.AddWithValue("@GradeDate", DateTime.Now);
                            commandTransaction.Parameters.AddWithValue("@Student_ID", studentId);
                            commandTransaction.Parameters.AddWithValue("@Subject_ID", subjectId);
                            commandTransaction.Parameters.AddWithValue("@Staff_ID", staffId);

                            commandTransaction.ExecuteNonQuery();
                        }

                        Console.WriteLine("\nBetyg har lagts till.");
                        keepGoing = false;
                    }
                    else if (userChoice == "NEJ")
                    {
                        Console.WriteLine("Inget betyg har lags till");
                        Console.WriteLine("\n Tryck på valfri tangent för att återgå till menyn");
                        keepGoing = false;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Fel inmatning, testa (Ja / Nej )\n");
                    }
                }
            }
            Console.ReadKey();
        }
    }
}