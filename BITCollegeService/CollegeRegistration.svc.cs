using BITCollege_QX.Data;
using BITCollege_QX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls;
using Utility;

namespace BITCollegeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CollegeRegistration" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CollegeRegistration.svc or CollegeRegistration.svc.cs at the Solution Explorer and start debugging.
    public class CollegeRegistration : ICollegeRegistration
    {
        BITCollege_QXContext db = new BITCollege_QXContext();

        int returnValue = 0;

        //public void DoWork()
        //{
        //}

        /// <summary>
        /// DropCourse method in CollegeRegistration.
        /// </summary>
        /// <param name="registrationId">registrationId</param>
        /// <returns>True if the course registration was successfully dropped. 
        /// False if it failed or the specified registrationId was not found.</returns>
        public bool DropCourse(int registrationId)
        {
            Registration registrations = db.Registrations.Where(x=>x.RegistrationId == registrationId).FirstOrDefault();

            try
            {
                if (registrations != null)
                {
                    db.Registrations.Remove(registrations);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// RegisterCourse method in CollegeRegistration.
        /// </summary>
        /// <param name="studentId">studentId</param>
        /// <param name="courseId">courseId</param>
        /// <param name="notes">notes</param>
        /// <returns>localValue</returns>
        public int RegisterCourse(int studentId, int courseId, string notes)
        {
            //Retrieve all records from Registrations table involving Student and Course.
            IQueryable<Registration> registrations = db.Registrations.Where(x=>x.StudentId == studentId 
                                                       && x.CourseId == courseId && x.Notes == notes);

            //Retrieve Course record.
            Course course = db.Courses.Where(x => x.CourseId == courseId && x.Notes == notes).FirstOrDefault();
            //Retrieve Student record.
            Student student = db.Students.Where(x => x.StudentId == studentId && x.Notes == notes).FirstOrDefault();

            //Query the allRegistrations query - null records.
            IEnumerable<Registration> nullRecords = db.Registrations.Where(x => x.Grade == null);
            if (nullRecords.Count() > 0)
            {
                returnValue = -100;
            }

            //Check to see if course is Mastery course, and get max attempts.
            CourseType type = BusinessRules.CourseTypeLookup(course.CourseType);

            if (type == CourseType.MASTERY)
            {
                MasteryCourse masteryCourse = (MasteryCourse)course;
                int maximumAttempts = masteryCourse.MaximumAttempts;

                //Query the allRegistrations query - not null records.
                IEnumerable<Registration> notNullRecords = db.Registrations.Where(x => x.Grade != null);
                if (notNullRecords.Count() > maximumAttempts)
                {
                    returnValue = -200;
                }
            }
            
            try
            {
                Registration registration = new Registration();

                registration.StudentId =studentId;
                registration.CourseId = courseId;
                registration.Notes = notes;
                registration.RegistrationDate = DateTime.Today;
                registration.SetNextRegistrationNumber();

                db.Registrations.Add(registration);
                db.SaveChanges();

                double tuitionAmount = course.TuitionAmount;

                double adjustedTuitionAmount = student.GradePointState.TuitionRateAdjustment(student);

                student.OutstandingFees += adjustedTuitionAmount;

                db.SaveChanges();
            }
            catch (Exception)
            {
                returnValue = -300;
            }

            return returnValue;
        }

        /// <summary>
        /// UpdateGrade method in CollegeRegistration. 
        /// </summary>
        /// <param name="grade">grade</param>
        /// <param name="registrationId">registrationId</param>
        /// <param name="notes">notes</param>
        /// <returns>newGradePointAverage</returns>
        public double? UpdateGrade(double grade, int registrationId, string notes)
        {
            Registration registration = db.Registrations.Where(x => x.RegistrationId == registrationId && x.Notes == notes).FirstOrDefault();

            registration.Grade = grade;

            registration.Notes = notes;

            db.SaveChanges();

            double? newGradePointAverage = CalculateGradePointAverage(registration.StudentId);

            return newGradePointAverage;
        }

        /// <summary>
        /// CalculateGradePointAverage method in CollegeRegistration.
        /// </summary>
        /// <param name="studentId">studentId</param>
        /// <returns>calcuatedGreadePointAverage</returns>
        private double? CalculateGradePointAverage(int studentId)
        {
            double grade;
            CourseType courseType;
            double gradePoint;
            double gradePointValue;
            double totalCreditHours = 0.0;
            double totalGradePointValue = 0.0;
            double? calcuatedGreadePointAverage;

            IQueryable<Registration> registrations = db.Registrations.Where(x => x.Grade != null 
                                                            && x.StudentId == studentId);

            foreach (Registration registration in registrations.ToList())
            {
                grade = registration.Grade.Value;

                courseType = BusinessRules.CourseTypeLookup(registration.Course.CourseType);

                if (courseType != CourseType.AUDIT)
                {
                    gradePoint = BusinessRules.GradeLookup(grade, courseType);

                    gradePointValue = gradePoint * registration.Course.CreditHours;

                    totalGradePointValue += gradePointValue;

                    totalCreditHours += registration.Course.CreditHours;
                }
            }

            if (totalCreditHours == 0)
            {
                calcuatedGreadePointAverage = null;
            } else
            {
                calcuatedGreadePointAverage = totalGradePointValue / totalCreditHours;
            }

            Student student = db.Students.Where(x => x.StudentId == studentId && x.GradePointAverage == calcuatedGreadePointAverage).FirstOrDefault();

            db.SaveChanges();

            return calcuatedGreadePointAverage;
        }
    }
}