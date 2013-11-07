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
    /// Confirmation page. Displays confirmation information.
    /// </summary>
    public partial class confirm : System.Web.UI.Page
    {
        private String action;
        private String courseID;

        /// <summary>
        /// Sets the confirmation information labels.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            action = (String)Session["Action"];
            courseID = (String)Session["CourseID"];

            if (!IsPostBack)
            {
                if (action == null || courseID == null)
                {
                    Response.Redirect("default.aspx");
                }
                else
                {
                    ActionLabel.Text = action + " ";
                    if (action.Equals("add"))
                    {
                        ToFromLabel.Text = "to ";
                    }
                    else if (action.Equals("remove"))
                    {
                        ToFromLabel.Text = "from ";
                    }
                    else
                    {
                        Response.Redirect("default.aspx");
                    }
                    using (TablesDataContext db = new TablesDataContext())
                    {
                        var course = from c in db.Courses
                                     where c.CourseID.Equals(courseID)
                                     select c;
                        CourseTitle.InnerHtml = course.FirstOrDefault().CourseID + " "
                                            + course.FirstOrDefault().Title;
                        var courseTimes = from ct in db.CourseTimes
                                          where ct.CourseID.Equals(courseID)
                                          select ct;
                        String[] days = { "","Monday", "Tuesday","Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                        String start, end;
                        CourseTimesContent.InnerHtml = "";
                        foreach(var ct in courseTimes)
                        {
                            start = "" + ct.Start;
                            if (start.Length == 3)
                                start = start.Substring(0, 1) + ":" + start.Substring(1);
                            else
                                start = start.Substring(0, 2) + ":" + start.Substring(2);

                            end = "" + ct.End;
                            if (end.Length == 3)
                                end = end.Substring(0, 1) + ":" + end.Substring(1);
                            else
                                end = end.Substring(0, 2) + ":" + end.Substring(2);

                            CourseTimesContent.InnerHtml += days[(int)ct.DayOfWeek] + " from " + 
                                                            start + " to " + end + "<br />";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Confirmation button click event handler. Removes or adds the course 
        /// from/to the student's courses. Redirects user to previous page afterwards.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        protected void ConfirmButton_Click(object sender, EventArgs e)
        {
            using (TablesDataContext db = new TablesDataContext())
            {
                if (action.Equals("add"))
                {
                    StudentCourse sc = new StudentCourse();
                    sc.CourseID = courseID;
                    sc.StudentID = (int)Session["StudentID"];
                    db.StudentCourses.InsertOnSubmit(sc);
                    SubmitChanges(db);
                }
                else if (action.Equals("remove"))
                {
                    var course = from sc in db.StudentCourses
                                 where (sc.CourseID.Equals(courseID)
                                 && sc.StudentID == (int)Session["StudentID"])
                                 select sc;
                    db.StudentCourses.DeleteOnSubmit(course.FirstOrDefault());
                    SubmitChanges(db);
                }
                else
                {
                    Response.Redirect("default.aspx");
                }

                Session["Action"] = null;
                Session["CourseID"] = null;
                Response.Redirect("courses.aspx");
            }
        }

        /// <summary>
        /// Cancel button click event handler. Redirects user to previous page.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Session["Action"] = null;
            Session["CourseID"] = null;
            Response.Redirect("courses.aspx");
        }

        /// <summary>
        /// Submits the changes to the datacontext.
        /// </summary>
        private void SubmitChanges(TablesDataContext db)
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
    }
}