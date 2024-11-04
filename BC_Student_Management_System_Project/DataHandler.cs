using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Student_Management_System_Project
{
    internal class DataHandler
    {
        public static void Summary()
        {
            List<Student> students = FileHandler.Read();

            int count = 0, totAge = 0;
            double avgAge;
                
            foreach (Student student in students) 
            {
                ++count;
                totAge += student.Age;
            }

            avgAge = totAge/count;


        }
    }
}
