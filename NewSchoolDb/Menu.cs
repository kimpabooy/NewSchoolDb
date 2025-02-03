using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSchoolDb
{
    public class Menu
    {
        public void UserMenu()
        {
            DatabaseManager dbM = new DatabaseManager();
            var activeMenu = true;

            while (activeMenu)
            {
                Console.Clear();

                Console.WriteLine("Välkommen till Administrationsverktyget för SchoolDb!\n");
                Console.WriteLine("Välj altternativ nedan.\n");
                Console.WriteLine("[1] Hämta alla elever.");
                Console.WriteLine("[2] Hämta alla elever i en viss klass.");
                Console.WriteLine("[3] Lägga till ny personal");
                Console.WriteLine("[4] Avsluta program");
                Console.WriteLine();

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        Console.Clear();
                        break;
                    case "2":
                        Console.Clear();
                        break;
                    case "3":
                        Console.Clear();
                        break;
                    case "4":
                        activeMenu = false;
                        break;
                    default:
                        activeMenu = true;
                        break;
                }
            }
        }


    }
}
