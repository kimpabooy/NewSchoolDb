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
                //Console.ReadKey();

                
                
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
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                Console.WriteLine("Vänligen ange information nedan\n");
                
                Console.Write("Förnamn: ");
                string firstName = Console.ReadLine();
                
                Console.Write("Efternamn: ");
                string lastName = Console.ReadLine();
                
                Console.Write("Startdatum (YYYY-MM-DD): ");
                DateTime yearsWorked = DateTime.Parse(Console.ReadLine());
                
                using (NewSchoolDbContext context = new NewSchoolDbContext())
                {
                    // Hämtar alla roller
                    var roles = context.Roles.ToList();

                    // Checks if there is any roles
                    if (!roles.Any())
                    {
                        Console.WriteLine("Det finns inga roller tillgängliga. Lägg till en roll först.");
                        return;
                    }

                    // List the available roles
                    Console.WriteLine("\nTillgängliga roller:");
                    foreach (var role in roles)
                    {
                        Console.WriteLine($"{role.RoleId} - {role.RoleName}");
                    }
                }

                Console.Write("Välj Roll genom ID: ");
                int roleId = int.Parse(Console.ReadLine());

                int departmentId = roleId;

                try
                {
                    string query = @"INSERT INTO Staff (FirstName, LastName, YearsWorked, Role_ID, Department_ID) 
                             VALUES (@FirstName, @LastName, @YearsWorked, @Role_ID, @Department_ID)";

                    SqlCommand command = new SqlCommand(query, connection);
                    //command.Parameters.AddWithValue("@FirstName", firstName);
                    //command.Parameters.AddWithValue("@LastName", lastName);
                    //command.Parameters.AddWithValue("@YearsWorked", yearsWorked);
                    //command.Parameters.AddWithValue("@Role_ID", roleId);
                    //command.Parameters.AddWithValue("@Department_ID", departmentId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("\n**** Personal tillagd ****");
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel inträffade vid tilldelning av personal. ( {ex.Message} )");
                }
            }
            Console.WriteLine("\nTryck på valfri tangent...");
            Console.ReadKey();
            
        }

        public void GetStudentGrade()
        {

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
