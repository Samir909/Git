using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MusicProject.Models;

namespace MusicProject.GetWay
{

    public class MusicGetway : BaseGateway
    {
        public List<Category_tbl> GetAllCategory()
        {



            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = "SELECT * FROM Category_tbl"
            };
            Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Category_tbl> categories = new List<Category_tbl>();
            while (reader.Read())
            {
                Category_tbl category = new Category_tbl()
                {
                    C_Id = Convert.ToInt32(reader["C_Id"]),


                    C_Name = reader["C_Name"].ToString()

                };
                categories.Add(category);
            }
            reader.Close();
            Connection.Close();
            return categories;

        }



        public List<Aritist> GetAllAritist()
        {



            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = "SELECT * FROM Aritist"
            };
            Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Aritist> aritists = new List<Aritist>();
            while (reader.Read())
            {
                Aritist aritist = new Aritist()
                {
                    Aritist_Id = Convert.ToInt32(reader["Aritist_Id"]),


                    Aritist_name = reader["Aritist_name"].ToString()

                };
                aritists.Add(aritist);
            }
            reader.Close();
            Connection.Close();
            return aritists;

        }



        public List<AudioFile> GetAllAudio()
        {



            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = "SELECT * FROM SongUp2"
            };
            Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<AudioFile> audioFiles = new List<AudioFile>();
            while (reader.Read())
            {
                AudioFile audioFile = new AudioFile()
                {
                    Song_Id = Convert.ToInt32(reader["Song_Id"]),


                    Song_Name = reader["Song_Name"].ToString()

                };
                audioFiles.Add(audioFile);
            }
            reader.Close();
            Connection.Close();
            return audioFiles;

        }
    }

}