using EtkinlikYonetim.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EtkinlikYonetim.Controllers
{
    public class HomeController : Controller
    {
        EtkinlikYonetimContext db = new EtkinlikYonetimContext();
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var etkinlikler = db.EyEtkinlik.OrderBy(c => c.BaslangicTarihi).ToList();
            return View(etkinlikler);
        }
        public virtual JsonResult Basic_Usage_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<SchedulerTask> tasks = new List<SchedulerTask>();
            SchedulerTask task1 = new SchedulerTask()
            {
                TaskID = 1,
                Title = "deneme1",
                Description = "aciklama1",
                IsAllDay = false,
                Start = DateTime.Now,
                End = DateTime.Now
            };
            SchedulerTask task2 = new SchedulerTask()
            {
                TaskID = 2,
                Title = "deneme2",
                Description = "aciklama2",
                IsAllDay = true,
                Start = DateTime.Now,
                End = DateTime.Now
            };
            SchedulerTask task3 = new SchedulerTask()
            {
                TaskID = 3,
                Title = "deneme3",
                Description = "aciklama3",
                IsAllDay = false,
                Start = DateTime.Now,
                End = DateTime.Now
            };
            tasks.Add(task1);
            tasks.Add(task2);
            tasks.Add(task3);
            return Json(tasks.ToDataSourceResult(request));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
