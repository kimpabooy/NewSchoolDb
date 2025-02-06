using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NewSchoolDb.Data;
using NewSchoolDb.Models;
using System;
using System.Collections.Generic;
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
            //baka in vad meddellönen är för de olika avdelningarna
        }

        public void GetStudentById()
        {

        }

        public void Test()
        {


        }
    }
}
