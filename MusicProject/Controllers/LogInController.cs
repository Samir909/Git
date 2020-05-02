using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicProject.Models;
using System.Data.SqlClient;

namespace MusicProject.Controllers
{
    public class LogInController : Controller
    {
        SqlConnection con=new SqlConnection();
        SqlCommand com=new SqlCommand();
        SqlCommand com2 = new SqlCommand();

        SqlDataReader dr;
        SqlDataReader dr2;
        // GET: LogIn
        public ActionResult LogIn()
        {
            return View();
        }

        void connectionString()
        {
            con.ConnectionString = @"Data Source=DESKTOP-N35FPFO\BIPONSQL; Initial Catalog= Music; Integrated Security=True";

        }

        [HttpPost]
        public ActionResult Verify(LogIn logIn)
        {




            Admin admin = new Admin();
            connectionString();
            con.Open();
            com.Connection = con;
            com2.Connection = con;


            com.CommandText = "select * from Admin where A_UserName='" + logIn.UserName + "' and A_Password='" + logIn.Password + "'";
            dr = com.ExecuteReader();


         
            if (dr.Read())
            {
                Session["A_UserName"] = logIn.UserName;

                con.Close();
                return RedirectToAction("Index","Home");
            }
            
            else
            {
                con.Close();
                
                return RedirectToAction("LogIn","LogIn");
            }

            string message = "";





            

        }

    }
}