using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicProject.GetWay;
using MusicProject.Models;

namespace MusicProject.Controllers
{
    public class ClientController : Controller
    {

        // GET: Client
        MusicGetway gateway = new MusicGetway();
        [Authorize]
        public ActionResult Index()
        {
            //ViewBag.audio = gateway.GetAllAudio();
           
                List<AudioFile> audiolist = new List<AudioFile>();
                //ViewBag.Category = gateway.GetAllCategory();
               // ViewBag.Aritist = gateway.GetAllAritist();
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SongDetails", con);
                //  cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    AudioFile audio = new AudioFile();
                   
                    audio.Song_Name = rdr["Song_Name"].ToString();
                    audio.Aritist_name = rdr["Aritist_name"].ToString();
                    audio.C_Name = rdr["C_Name"].ToString();
                    audio.FileSize = Convert.ToInt32(rdr["FileSize"]);
                    audio.SongFile = rdr["SongFile"].ToString();

                    audiolist.Add(audio);
                }
            }
              
            
            return View(audiolist);

        }

    

        [HttpPost]
        [Authorize]
        public ActionResult ValuePass(string a)
        {
            List<AudioFile> audiolist = new List<AudioFile>();
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
               // string value = "select Song_Name,Aritist_name,C_Name,FileSize,SongFile from SongUp2 join Category_tbl on SongUp2.C_Id = Category_tbl.C_Id join Aritist on SongUp2.Aritist_Id = Aritist.Aritist_Id where Song_Name like a%";

                SqlCommand cmd = new SqlCommand("getValue", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlParameter = new SqlParameter("@a", a);
                cmd.Parameters.Add(sqlParameter);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    AudioFile audio = new AudioFile();

                    audio.Song_Name = rdr["Song_Name"].ToString();
                    audio.Aritist_name = rdr["Aritist_name"].ToString();
                    audio.C_Name = rdr["C_Name"].ToString();
                    audio.FileSize = Convert.ToInt32(rdr["FileSize"]);
                    audio.SongFile = rdr["SongFile"].ToString();

                    audiolist.Add(audio);
                }
            }


            return View(audiolist);
        }
    }
}