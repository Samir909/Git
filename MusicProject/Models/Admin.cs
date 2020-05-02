using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicProject.Models
{
    public class Admin
    {
        public int Admin_Id { get; set; }

        public string A_FullName { get; set; }
        public string A_UserName { get; set; }
       
        public string A_Password { get; set; }

    }
}