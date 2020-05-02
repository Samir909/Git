using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using MusicProject.Models;


namespace MusicProject.Controllers
{


    public class AdminController : Controller
    {

        private string connectionString = @"Data Source=DESKTOP-N35FPFO\BIPONSQL; Initial Catalog= Music; Integrated Security=True";
        // GET: Admin
        public ActionResult Index()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from Admin";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }

            return View(dt);
        }

        // GET: Admin/Create
        [HttpGet]
        public ActionResult Create()   // Create the form template for insert
        {
            return View(new Admin());
        }


        [HttpPost]
        public ActionResult Create(Admin admin)
        {
            using (SqlConnection Sqlcon = new SqlConnection(connectionString))
            {
                Sqlcon.Open();
                string query = "INSERT INTO Admin VALUES(@A_FullName,@A_UserName,@A_Password) ";
                SqlCommand cmd = new SqlCommand(query, Sqlcon);
                cmd.Parameters.AddWithValue("@A_FullName", admin.A_FullName);
                cmd.Parameters.AddWithValue("@A_UserName", admin.A_UserName);
                cmd.Parameters.AddWithValue("@A_Password", admin.A_Password);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {

            Admin admin= new Admin();
            DataTable dataTable=new DataTable();
            using (SqlConnection con=new SqlConnection(connectionString))
            {
                
                    con.Open();
                    string query = "SELECT * FROM Admin WHERE Admin_Id=@Admin_Id ";
                    SqlDataAdapter sqlDataAdapter =new SqlDataAdapter(query,con);
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Admin_Id", id);
                    sqlDataAdapter.Fill(dataTable);
                
            } 

            if (dataTable.Rows.Count==1)
            {
                admin.Admin_Id= Convert.ToInt32(dataTable.Rows[0][0].ToString());
                admin.A_FullName = dataTable.Rows[0][1].ToString();
                admin.A_UserName = dataTable.Rows[0][2].ToString();
                admin.A_Password = dataTable.Rows[0][3].ToString();
                return View(admin);
            }
            else
            {

                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public ActionResult Edit(Admin admin)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string query = "UPDATE Admin SET A_FullName= @A_FullName , A_UserName= @A_UserName , A_Password= @A_Password Where Admin_Id=@Admin_Id";
                SqlCommand sqlCommand=new SqlCommand(query,con);

                sqlCommand.Parameters.AddWithValue("@Admin_Id", admin.Admin_Id);
                sqlCommand.Parameters.AddWithValue("@A_FullName", admin.A_FullName);
                sqlCommand.Parameters.AddWithValue("@A_UserName", admin.A_UserName);
                sqlCommand.Parameters.AddWithValue("@A_Password", admin.A_Password);
                sqlCommand.ExecuteNonQuery();

            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            using (SqlConnection con=new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Delete from Admin where Admin_Id=@Admin_Id ";
                SqlCommand cmd=new  SqlCommand(query,con);
                cmd.Parameters.AddWithValue("@Admin_Id", id);
                cmd.ExecuteNonQuery();

            }

            return RedirectToAction("Index");
        }


    }



}