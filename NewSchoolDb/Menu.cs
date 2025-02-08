using NewSchoolDb.Servicees;

namespace NewSchoolDb
{
    /*
        while (!int.TryParse(Console.ReadLine(), out studentId))
                {
                    Console.WriteLine("Felaktig input! Ange en giltig siffra");
                    Console.Write("Val: ");
                }
     */

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
                Console.WriteLine("[5] Hämta Student och betyg");
                Console.WriteLine("[6] Hämta kostnad/mån per avdelning");
                Console.WriteLine("[7] Hämta Student genom ID");
                Console.WriteLine("[8] Sätt betyg på en elev");
                Console.WriteLine("[0] Avsluta");
                Console.WriteLine();

                Console.Write("Val: ");
                string userInput = Console.ReadLine();
                
                Console.Clear();
                
                switch (userInput)
                {
                    case "1":
                        efS.GetTeacherAmount();
                        break;
                    case "2":
                        efS.GetStudentInfo();
                        break;
                    case "3":
                        efS.GetActiveCourse();
                        break;
                    case "4":
                        adoS.GetStaff();
                        break;
                    case "5":
                        adoS.GetStudentGrade();
                        break;
                    case "6":
                        adoS.GetSalary();
                        break;
                    case "7":
                        adoS.GetStudentById();
                        break;
                    case "8":
                        adoS.AddGradeToStudent();
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
