using NewSchoolDb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSchoolDb
{
    public class DatabaseManager
    {
        public void GetTeacherAmount()
        {
            using (var context = new NewSchoolDbContext())
            {
                var teachers = context.Staff;

                
            }

        }

    }
}
