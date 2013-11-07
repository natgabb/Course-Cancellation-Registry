using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Linq;

namespace CourseCancellationRegistry
{
    /// <summary>
    /// Courses page. Displays all courses, and selected courses.
    /// </summary>
    public partial class courses : System.Web.UI.Page
    {
        private TablesDataContext db;

        /// <summary>
        /// Loads the course lists and various labels.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            db = new TablesDataContext();
            if(!IsPostBack)
            {
                try
                {
                    if (Session["Email"] == null)
                    {
                        Response.Redirect("default.aspx");
                    }
                    
                    var student = from s in db.Students
                                    where s.Email.Equals((String)Session["Email"])
                                    select s.StudentID;
                    if (student.Count() == 0)
                    {
                        //Create new entry.
                        Student newStudent = new Student();
                        newStudent.Email = (String)Session["Email"];
                        db.Students.InsertOnSubmit(newStudent);
                        SubmitChanges();
                        student = from s in db.Students
                                    where s.Email.Equals((String)Session["Email"])
                                    select s.StudentID;
                    }

                    Session["StudentID"] = student.FirstOrDefault();

                    emailLabel.Text += (String)Session["Email"];
                }
                catch (HttpException exc)
                {
                    Response.Write("<script type='text/javascript'>alert('An error has occured: " + exc.Message + "');</script>");
                }
                loadCourses();
            }
        }

        /// <summary>
        /// Loads the two course lists.
        /// </summary>
        private void loadCourses()
        {
            var studentCoursesTableResult = from sc in db.StudentCourses
                                where sc.StudentID == (int)Session["StudentID"]
                                select sc;
            List<String> courseIDs = new List<String>();
            foreach (var sc in studentCoursesTableResult)
                courseIDs.Add(sc.CourseID);


            var studentCourses = from sc in db.Courses
                                 where courseIDs.Contains(sc.CourseID)
                                 select sc;

            var courses = from s in db.Courses
                          where !courseIDs.Contains(s.CourseID)
                          select s;

            CoursesDropDownList.Items.Clear();
            foreach (var c in courses)
                CoursesDropDownList.Items.Add(c.CourseID + " " + c.Title);

            StudentCoursesDropDownList.Items.Clear();
            foreach (var sc in studentCourses)
                StudentCoursesDropDownList.Items.Add(sc.CourseID + " " + sc.Title);
        }

        /// <summary>
        /// Disposes of the DataContext before Unloading.
        /// </summary>
        protected void Page_Unload()
        {
            if(db != null)
                db.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Add button click event handler. Redirects user to
        /// confirmation page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddButton_Click(object sender, EventArgs e)
        {
            if (CoursesDropDownList.Items.Count > 0)
            {
                String selected = CoursesDropDownList.SelectedValue;
                Session["Action"] = "add";
                Session["CourseID"] = selected.Substring(0, 16);
                Response.Redirect("confirm.aspx");
            }
        }

        /// <summary>
        /// Remove button click event handler. Redirects user to
        /// confirmation page.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        protected void RemoveButton_Click(object sender, EventArgs e)
        {
            if (StudentCoursesDropDownList.Items.Count > 0)
            {
                String selected = StudentCoursesDropDownList.SelectedValue;
                Session["Action"] = "remove";
                Session["CourseID"] = selected.Substring(0, 16);
                Response.Redirect("confirm.aspx");
            }
        }

        /// <summary>
        /// Submits the changes to the datacontext.
        /// </summary>
        private void SubmitChanges()
        {
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
        }

        /// <summary>
        /// This exits this page and sends the user back to the default page.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        protected void ExitButton_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("default.aspx");
        }
    }
}