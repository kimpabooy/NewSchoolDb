using Microsoft.Data.SqlClient;
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
            using (var context = new NewSchoolDbContext())
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

                int staffRole;
                while (true)
                {
                    Console.Write("\nVälj det ID du vill lägga till: ");
                    if (int.TryParse(Console.ReadLine(), out staffRole) && roles.Any(roles => roles.RoleId == staffRole))
                    {
                        break;
                    }
                    Console.WriteLine("Ogiltigt roll ID. Försök igen.");
                }

                try
                {
                    var newStaff = new Staff
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        RoleId = staffRole
                    };

                    // adding the new staff to the databasse and saves
                    //context.Staff.Add(newStaff);
                    //context.SaveChanges();
                    Console.Clear();

                    Console.WriteLine("\n**** Personal tillagd ****");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel inträffade vid tilldelning av personal: {ex.Message}");
                }
            }
            Console.WriteLine("\nTryck på valfri tangent...");
            Console.ReadKey();
            Console.Clear();
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
