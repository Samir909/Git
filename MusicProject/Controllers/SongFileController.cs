using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicProject.GetWay;
using MusicProject.Models;

namespace MusicProject.Controllers
{
    public class SongFileController : Controller
    {
        // GET: SongFile


        /*   public ActionResult Index()
           {
               return View();
           }
           */
        MusicGetway gateway = new MusicGetway();
        [HttpGet]
        public ActionResult UploadAudio()
        {
            List<AudioFile> audiolist = new List<AudioFile>();
            ViewBag.Category = gateway.GetAllCategory();
            ViewBag.Aritist = gateway.GetAllAritist();
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("ViewALL", con);
              //  cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    AudioFile audio = new AudioFile();
                    audio.Song_Id= Convert.ToInt32(rdr["Song_Id"]);
                    audio.Song_Name = rdr["Song_Name"].ToString();
                    audio.C_Id = Convert.ToInt32(rdr["C_Id"]);
                    audio.Aritist_Id = Convert.ToInt32(rdr["Aritist_Id"]);
                    audio.FileSize = Convert.ToInt32(rdr["FileSize"]);
                    audio.SongFile = rdr["SongFile"].ToString();

                    audiolist.Add(audio);
                }
            }
            return View(audiolist);
        }

        [HttpPost]
        public ActionResult UploadAudio( AudioFile audioFile, HttpPostedFileBase fileupload)
        {
            //AudioFile audioFile=new AudioFile();
            if (fileupload != null)
            {
                string fileName = Path.GetFileName(fileupload.FileName);
                int fileSize = fileupload.ContentLength;
                int Size = fileSize / 1000000;
                fileupload.SaveAs(Server.MapPath("~/AudioFileUpload/" + fileName));

                string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(CS))
                {
                   AudioFile audio = new AudioFile();


                    SqlCommand cmd = new SqlCommand("CreatNew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Song_Name", fileName);
                    cmd.Parameters.AddWithValue("@FileSize", Size);
                    cmd.Parameters.AddWithValue("SongFile", "~/AudioFileUpload/" + fileName);

                    cmd.Parameters.AddWithValue("@C_Id",audioFile.C_Id);
                    cmd.Parameters.AddWithValue("@Aritist_Id",audioFile.Aritist_Id);
                    cmd.ExecuteNonQuery();

                }
            }
            return RedirectToAction("UploadAudio");
        }
    }
}