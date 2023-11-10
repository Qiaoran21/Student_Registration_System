using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BITCollege_QX.Models;
using BITCollege_QX.Data;


namespace BITCollege_QX.TuitionRateAdjustmentTest
{
    public class Program
    {
        private static BITCollege_QXContext db = new BITCollege_QXContext();

        static void Main(string[] args)
        {
            Suspended_Newly_Registered_44();
            Suspended_Newly_Registered_60();
            Suspended_Newly_Registered_80();

            Probation_Newly_Registered_3_Courses();
            Probation_Newly_Registered_7_Courses();

            Regular_Newly_Registered_250();

            Honours_Newly_Registered_3_Courses();
            Honours_Newly_Registered_4_Courses();
            Honours_Newly_Registered_7_Courses();
            Honours_Newly_Registered_7_Courses_2();
        }

        /// <summary>
        /// Test for when GPA is 0.44. 
        /// </summary>
        static void Suspended_Newly_Registered_44()
        {
            Student student = db.Students.Find(3);
            student.GradePointAverage = 0.44;
            student.GradePointStateId = 1;
            db.SaveChanges();

            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Suspended State:");
            Console.WriteLine("Expected tuition: 1150");
            Console.WriteLine("Actual tuition: " + tuitionRate);
            Console.WriteLine();
        }

        /// <summary>
        /// Test for when GPA is 0.60.
        /// </summary>
        static void Suspended_Newly_Registered_60()
        {
            Student student = db.Students.Find(3);
            student.GradePointAverage = 0.60;
            student.GradePointStateId = 1;
            db.SaveChanges();

            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected tuition: 1120");
            Console.WriteLine("Actual tuition: " + tuitionRate);
            Console.WriteLine();
        }

        /// <summary>
        /// Test for when GPA is 0.80.
        /// </summary>
        static void Suspended_Newly_Registered_80()
        {
            Student student = db.Students.Find(3);
            student.GradePointAverage = 0.80;
            student.GradePointStateId = 1;
            db.SaveChanges();

            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected tuition: 1100");
            Console.WriteLine("Actual tuition: " + tuitionRate);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Test for when GPA is 1.15 and has complete 3 courses.
        /// </summary>
        static void Probation_Newly_Registered_3_Courses()
        {
            Student student = db.Students.Find(3);
            student.GradePointAverage = 1.15;
            student.GradePointStateId = 5;
            db.SaveChanges();

            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Probation State:");
            Console.WriteLine("Expected tuition: 1075");
            Console.WriteLine("Actual tuition: " + tuitionRate);
            Console.WriteLine();
        }

        /// <summary>
        /// Test for when GPA is 1.15 and has complete 7 courses.
        /// </summary>
        static void Probation_Newly_Registered_7_Courses()
        {
            Student student = db.Students.Find(2);
            student.GradePointAverage = 1.15;
            student.GradePointStateId = 5;
            db.SaveChanges();

            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected tuition: 1035");
            Console.WriteLine("Actual tuition: " + tuitionRate);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Test when GPA is 2.5.
        /// </summary>
        static void Regular_Newly_Registered_250()
        {
            Student student = db.Students.Find(2);
            student.GradePointAverage = 2.50;
            student.GradePointStateId = 6;
            db.SaveChanges();

            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Regular State:");
            Console.WriteLine("Expected tuition: 1000");
            Console.WriteLine("Actual tuition: " + tuitionRate);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Test for when GPA is 3.9 and has complete 3 courses.
        /// </summary>
        static void Honours_Newly_Registered_3_Courses()
        {
            Student student = db.Students.Find(3);
            student.GradePointAverage = 3.90;
            student.GradePointStateId = 7;
            db.SaveChanges();

            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Honours State:");
            Console.WriteLine("Expected tuition: 900");
            Console.WriteLine("Actual tuition: " + tuitionRate);
            Console.WriteLine();
        }

        /// <summary>
        /// Test for when GPA is 4.27 and has complete 4 courses.
        /// </summary>
        static void Honours_Newly_Registered_4_Courses()
        {
            Student student = db.Students.Find(3);
            student.GradePointAverage = 4.27;
            student.GradePointStateId = 7;
            db.SaveChanges();

            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected tuition: 880");
            Console.WriteLine("Actual tuition: " + tuitionRate);
            Console.WriteLine();
        }

        /// <summary>
        /// Test for when GPA is 4.40 and has complete 7 courses.
        /// </summary>
        static void Honours_Newly_Registered_7_Courses()
        {
            Student student = db.Students.Find(2);
            student.GradePointAverage = 4.40;
            student.GradePointStateId = 7;
            db.SaveChanges();

            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected tuition: 830");
            Console.WriteLine("Actual tuition: " + tuitionRate);
            Console.WriteLine();
        }

        /// <summary>
        /// Test for when GPA is 4.10 and has complete 7 courses.
        /// </summary>
        static void Honours_Newly_Registered_7_Courses_2()
        {
            Student student = db.Students.Find(2);
            student.GradePointAverage = 4.10;
            student.GradePointStateId = 7;
            db.SaveChanges();

            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected tuition: 850");
            Console.WriteLine("Actual tuition: " + tuitionRate);
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
