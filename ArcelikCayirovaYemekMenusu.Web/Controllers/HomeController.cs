using ArcelikCayirovaYemekMenusu.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace ArcelikCayirovaYemekMenusu.Web.Controllers
{
    public class HomeController : Controller
    {
        private const string Gunler = ",Pazartesi,Salı,Çarşamba,Perşembe,Cuma,Cumartesi,Pazar,";

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            if (username == "yeliz" && password == "engin")
            {
                Session["Yeliz"] = "Engin";

                return RedirectToAction(nameof(Menu));
            }

            return Index();
        }

        [HttpGet]
        public ActionResult Menu()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Menu(string yemekmenu)
        {
            var menuList = yemekmenu.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var satirSayisi = menuList.Length / 2;

            for (int iLoop = 0; iLoop < satirSayisi; iLoop++)
            {
                var menu = new GunlukMenu();

                menu.Date = DateTime.ParseExact(menuList[iLoop * 2], "d.M.yyyy", null);
                menu.Menu = new List<string>();

                var list = menuList[(iLoop * 2) + 1].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in list)
                {
                    if (Gunler.Contains(item))
                    {
                        menu.DayName = item.Trim();
                    }
                    else
                    {
                        menu.Menu.Add(item.Trim());
                    }
                }

                if (!Directory.Exists(Server.MapPath("/") + "menu"))
                {
                    Directory.CreateDirectory(Server.MapPath("/") + "menu");
                }

                System.IO.File.WriteAllText(Server.MapPath("/") + "menu/" + menu.Date.ToString("yyyy-MM-dd") + ".json", JsonConvert.SerializeObject(menu));
            }

            return View();
        }

        public ActionResult API(DateTime? date)
        {
            var tarih = date.HasValue ? date.Value : DateTime.Today;

            var menuList = new List<GunlukMenu>();

            for (int iLoop = -3; iLoop < 3; iLoop++)
            {
                var menu = System.IO.File.ReadAllText(Server.MapPath("/") + "menu/" + tarih.AddDays(iLoop).ToString("yyyy-MM-dd") + ".json");

                menuList.Add(JsonConvert.DeserializeObject<GunlukMenu>(menu));
            }

            return Json(menuList);
        }
    }
}