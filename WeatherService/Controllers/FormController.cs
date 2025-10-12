using Microsoft.AspNetCore.Mvc;
using WeatherService.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        // GET: api/<FormController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FormController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FormController>
        [HttpPost]
        public ActionResult Post([FromBody] Form form)
        {
            var context = form.Context;
            var numberOfRows = form.NumberOfRows;
            var formStructure = form.FormStructure;
            // Process the form data as needed
            form.Id = Guid.NewGuid(); // Assign a new ID to the form
            return new JsonResult(form);
        }

        // PUT api/<FormController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FormController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
