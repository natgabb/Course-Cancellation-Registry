using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data.Linq;
using System.Diagnostics;
using System.Net.Mail;

namespace CourseCancellationRegistry
{
    /// <summary>
    /// This class has utility methods to read from a feed and send emails.
    /// </summary>
    public class ReadFromFeed
    {
        /// <summary>
        /// 
        /// </summary>
        private const string FEED = "http://www2.dawsoncollege.qc.ca/class_cancellations/feed.xml";

        /// <summary>
        /// This loads cancelled classes from a feed and sends emails to registered students.
        /// </summary>
        public static void LoadStream()
        {
            XElement cancel = XElement.Load(FEED);
            var test = from t in cancel.Descendants("item")
                       select t.Element("title").Value;
            if(!test.FirstOrDefault().Equals("No classes cancelled."))
            {
                IEnumerable<CancelledClass> classes = from c in cancel.Descendants("item")
                                                  select new CancelledClass(c.Element("title").Value, 
                                                  c.Element("course").Value,
                                                  c.Element("datecancelled").Value,
                                                  c.Element("teacher").Value, c.Element("notes").Value);
                List<CancelledClass> newCancelled = new List<CancelledClass>();
                bool isNew = true;
                using (TablesDataContext db = new TablesDataContext())
                {
                    IEnumerable<CancelledClass> allCancelled = from c in db.CourseCancelleds
                                                               select new CancelledClass(c.CourseID, c.Title, c.DateCancelled, c.Teacher, c.Notes);
                    string previousCourseID = "";
                    string previousDateCancelled = "";

                    foreach (CancelledClass c in classes)
                    {
                        isNew = true;
                        if (c.CourseID.Equals(previousCourseID) && c.DateCancelled.Equals(previousDateCancelled))
                            isNew = false;
                        else
                            foreach (CancelledClass allC in allCancelled)
                            {
                                if(c.Equals(allC))
                                {
                                    isNew = false;
                                    break;
                                }
                            }
                        if (isNew)
                            newCancelled.Add(c);
                        previousCourseID = c.CourseID;
                        previousDateCancelled = c.DateCancelled;
                    }

                    //Add the new cancelled courses to the database
                    CourseCancelled course;
                    foreach (CancelledClass c in newCancelled)
                    {
                        course = new CourseCancelled();
                        course.CourseID = c.CourseID;
                        course.DateCancelled = c.DateCancelled;
                        course.Teacher = c.Teacher;
                        course.Notes = c.Notes;
                        course.Title = c.Title;
                        db.CourseCancelleds.InsertOnSubmit(course);

                        //Cycle through new cancellations and send emails.
                        var students = from s in db.StudentCourses
                                       where s.CourseID.Equals(c.CourseID)
                                       select s.StudentID;
                        foreach (int studentID in students)
                        {
                            //Select email, send email.
                            var listOfEmails = from s in db.Students
                                        where s.StudentID == studentID
                                        select s.Email;
                            string email = listOfEmails.FirstOrDefault();
                            sendEmail(email, c);
                        }
                    }
                    try
                    {
                        db.SubmitChanges();
                    }
                    catch (ChangeConflictException cce)
                    {
                        foreach (var conflict in db.ChangeConflicts)
                            conflict.Resolve(RefreshMode.KeepCurrentValues);
                        db.SubmitChanges();
                    }
                }//end of using
            }
        }//end of method

        /// <summary>
        /// This sends an email for a cancelled class.
        /// </summary>
        /// <param name="email">The email address</param>
        /// <param name="cancelled">The cancelled course</param>
        private static void sendEmail(string email, CancelledClass cancelled)
        {
            Debug.WriteLine("\nSending email to: " + email + " for class: \n" + cancelled);
            string body = "<b>You have a cancelled class:</b><br><br>" + cancelled.CourseID +
                " " + cancelled.Title + "<br>Date cancelled: " + cancelled.DateCancelled +
                "<br>Teacher: " + cancelled.Teacher + "<br>Notes: " + cancelled.Notes +
                "<br><br><br>To unsubscribe from this course, visit: " +
            "<a href='http://waldo.dawsoncollege.qc.ca/0932340/cancel'>http://waldo.dawsoncollege.qc.ca/0932340/cancel</a>";
            Debug.WriteLine("\nBody of email message:\n" + body);
            
            MailMessage message = new MailMessage("noreply@dawsoncollege.qc.ca", email,
                "Cancelled Class " + cancelled.Title , body );
            message.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(message);
            message.Dispose();
            smtp.Dispose();
        }
    }

    /// <summary>
    /// This class defines an object of type CancelledClass
    /// which contains information about a cancelled class.
    /// </summary>
    public class CancelledClass
    {
        public string CourseID { get; set; }
        public string Title { get; set; }
        public string DateCancelled { get; set; }
        public string Teacher { get; set; }
        public string Notes { get; set; }

        /// <summary>
        /// CancelledClass constructor.
        /// </summary>
        /// <param name="courseID">The courseID</param>
        /// <param name="dateCancelled">The date cancelled</param>
        /// <param name="teacher">The teacher</param>
        /// <param name="notes">The notes</param>
        public CancelledClass(string courseID, string title, string dateCancelled, string teacher, string notes)
        {
            CourseID = courseID;
            Title = title;
            DateCancelled = dateCancelled;
            Teacher = teacher;
            Notes = notes;
        }

        /// <summary>
        /// Overrides the behaviour of the Equals method.
        /// </summary>
        /// <param name="obj">The object to check for equality.</param>
        /// <returns>Whether they can be considered equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is CancelledClass)
            {
                CancelledClass c = obj as CancelledClass;
                if (this.CourseID.Equals(c.CourseID) &&
                   this.DateCancelled.Equals(c.DateCancelled))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Overrides the behaviour of the ToString method.
        /// </summary>
        /// <returns>A string representation of CancelledClass obj.</returns>
        public override string ToString()
        {
            return ("CourseID: " + CourseID + "\nDate cancelled: " + DateCancelled + "\nTeacher: " + Teacher + "\nNotes: " + Notes );
        }
    }
}