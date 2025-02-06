//using NewSchoolDb.EFService;
using NewSchoolDb.Servicees;
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
            EFService efS = new EFService();
            ADOService adoS = new ADOService();

            var activeMenu = true;

            while (activeMenu)
            {
                Console.Clear();

                Console.WriteLine("Välkommen till Administrationsverktyget för NewSchoolDb!\n");
                Console.WriteLine("Välj altternativ nedan.\n");

                Console.WriteLine("[1] Hämta antalet anställda per avdelning.");
                Console.WriteLine("[2] Hämta relevant information om Studenterna.");
                Console.WriteLine("[3] Hämta Aktiva kurser.");
                Console.WriteLine("[4] Hämta Personalöversikt.");
                Console.WriteLine("[5] Hämta Elever och betyg");
                Console.WriteLine("[6] Hämta kostnad/mån per avdelning");
                Console.WriteLine("[7] Hämta Student genom ID");
                Console.WriteLine("[8] ");
                Console.WriteLine("[9] ");
                Console.WriteLine("[0] Avsluta");
                Console.WriteLine();
                Console.Write("Val: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        Console.Clear();
                        efS.GetTeacherAmount();
                        break;
                    case "2":
                        Console.Clear();
                        efS.GetStudentInfo();
                        break;
                    case "3":
                        Console.Clear();
                        efS.GetActiveCourse();
                        break;
                    case "4":
                        Console.Clear();
                        adoS.GetStaff();
                        break;
                    case "5":
                        Console.Clear();
                        adoS.GetStudentGrade();
                        break;
                    case "6":
                        Console.Clear();
                        adoS.GetSalary();
                        break;
                    case "7":
                        Console.Clear();
                        adoS.GetStudentById();
                        break;
                    case "8":
                        Console.Clear();
                        break;
                    case "9":
                        Console.Clear();
                        break;
                    case "0":
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
