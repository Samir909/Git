using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicProject.Models
{
    public class AudioFile
    {

        public int Song_Id { get; set; }
        public string Song_Name { get; set; }
        public Nullable<int> FileSize { get; set; }
        public string SongFile { get; set; }
      
        public int Aritist_Id { get; set; }
        public string Aritist_name { get; set; }

        public int C_Id { get; set; }
        public string C_Name { get; set; }

        [NotMapped] 
        public HttpPostedFileBase fileupload { get; set; }

        public virtual Aritist Aritist { get; set; }
        public virtual Category_tbl Category_tbl { get; set; }


    }
}