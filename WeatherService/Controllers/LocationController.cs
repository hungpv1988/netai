using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Model;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : Controller
    {
        [HttpGet()]
        public List<Location> Get()
        {
            return new List<Location>
            {
                new Location { Name = "Hanoi", Description = "Capital of Vietnam, provide free wifi and low tax on alcohol" },
                new Location { Name = "Helsinki", Description = "Capital of Finland, provide cruise services to travel across baltic sea, free wifi, and provide free sauna" },
                new Location { Name = "London", Description = "Capital of UK, 10% tax on alcohol, famous for long-standing castles, especially film studio for Harry Porter series" }
            };
        }

        // GET: LocationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LocationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LocationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
