namespace NewSchoolDb
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseManager dbM = new DatabaseManager();
            Menu m = new Menu();
            //m.UserMenu();
            //dbM.GetActiveCourse();
            dbM.Test();
        }
    }
}
