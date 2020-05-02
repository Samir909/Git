using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using MusicProject.Models;

namespace MusicProject.Controllers
{
    public class AritistTestController : Controller
    {
        // GET: AritistTest
        public ActionResult Index()
        
        {
            return View();
        }

        public ActionResult ViewAll()
        {


            return View(GetAllAritist());
        }

        IEnumerable<Aritist> GetAllAritist()
        {

            using (DbModels db=new DbModels())
            {
                return db.Aritists.ToList<Aritist>();
            }

        }

        public ActionResult AddorEdit(int id=0)
        {
            Aritist aritist=new Aritist();
            if (id!=0)
            {
                using (DbModels db=new DbModels())
                {
                    aritist = db.Aritists.Where(x => x.Aritist_Id == id).FirstOrDefault<Aritist>();
                }
            }
            return View(aritist);
        }
        [HttpPost]
        public ActionResult AddorEdit(Aritist aritist)
        {
            try
            {
                if (aritist.ImageFile !=null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(aritist.ImageFile.FileName);
                    string extension = Path.GetExtension(aritist.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    aritist.Aritist_Image = "~/APPFile/Images/" + fileName;
                    aritist.ImageFile.SaveAs(Path.Combine(Server.MapPath("~/APPFile/Images/"), fileName));
                }

                using (DbModels db=new DbModels())
                {
                    if (aritist.Aritist_Id==0)
                    {
                        db.Aritists.Add(aritist);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(aritist).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    
                }


                return Json(new {success = true, html = GlobalClass.RenderRazorViewToString(this,"ViewAll", GetAllAritist()), message = "Submitted Successfully"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (DbModels db =new DbModels())
                {
                    Aritist aritist = db.Aritists.Where(x => x.Aritist_Id == id).FirstOrDefault<Aritist>();
                    db.Aritists.Remove(aritist);
                    db.SaveChanges();
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllAritist()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}