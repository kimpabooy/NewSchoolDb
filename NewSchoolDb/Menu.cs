﻿using System;
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

                Console.WriteLine("Välkommen till Administrationsverktyget för NewSchoolDb!\n");
                Console.WriteLine("Välj altternativ nedan.\n");
                Console.WriteLine("[1] Hämta antalet anställda per avdelning .");
                Console.WriteLine("[2] Hämta relevant information om Studenterna");
                Console.WriteLine("[3] Hämta Aktiva kurser");
                Console.WriteLine("[5] Hämta Personalöversikt");
                Console.WriteLine("[6] ");
                Console.WriteLine("[7] Avsluta");
                Console.WriteLine("[8] ");
                Console.WriteLine("[9] ");
                Console.WriteLine();
                // GetStaff()
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        Console.Clear();
                        dbM.GetTeacherAmount();
                        break;
                    case "2":
                        Console.Clear();
                        dbM.GetStudentInfo();
                        break;
                    case "3":
                        Console.Clear();
                        dbM.GetActiveCourse();
                        break;
                    case "4":
                        Console.Clear();
                        dbM.GetTeacherAmount();
                        break;
                    case "5":
                        Console.Clear();
                        dbM.GetStaff();
                        break;
                    case "6":
                        Console.Clear();
                        break;
                    case "7":
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
