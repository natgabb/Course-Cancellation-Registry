using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Timers;
using System.Net;

namespace CourseCancellationRegistry
{
    /// <summary>
    /// This loads the RSS feed every 15 minutes and sends a request to the webpage.
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        private Timer timer;

        /// <summary>
        /// Sets up the timer at the beginning of the application.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            ReadFromFeed.LoadStream();
            timer = new Timer(900000);
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(myTimerElapsed);
        }

        /// <summary>
        /// Reads the RSS feed every time it is called.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args.</param>
        private void myTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ReadFromFeed.LoadStream();
            WebRequest request = WebRequest.Create("http://waldo.dawsoncollege.qc.ca/0932340/cancel/default.aspx");
            request.GetResponse();
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Disposes the timer at the application's end.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        protected void Application_End(object sender, EventArgs e)
        {
           timer.Dispose();
        }
    }
}