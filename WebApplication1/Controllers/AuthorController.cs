using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("/api/authors")]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public AuthorController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            var response = new List<Author>();
            try
            {
                _logger.LogInformation("HELLO FROM AuthorController/GetAll");
                using var context = new BookStoreDBContext();
                response = context.Author.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogInformation("HELLO FROM api/authos");
                return BadRequest(ex.Message);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var response = new Author();
            try
            {
                _logger.LogInformation("HELLO FROM AuthorController/Get");
                using var context = new BookStoreDBContext();
                response = context.Author.FirstOrDefault(author => author.AuthorId == id);
            }
            catch(Exception ex)
            {
                _logger.LogInformation("HELLO FROM api/authos");
                return BadRequest(ex.Message);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(string firstName, string lastName)
        {
            try
            {
                using var context = new BookStoreDBContext();
                Author author = new Author
                {
                    FirstName = firstName,
                    LastName = lastName
                };
                context.Author.Add(author);
                context.SaveChanges();
            } 
            catch(Exception ex)
            {
                _logger.LogInformation("HELLO FROM api/authos/Add");
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPut]
        [Route("{id}/update/{phonenumber}")]
        public IActionResult Update(int id, string phonenumber)
        {
            try
            {
                using var context = new BookStoreDBContext();
                var author = new Author()
                {
                    AuthorId = id,
                    Phone = phonenumber
                };
                context.Author.Attach(author);
                context.Entry(author).Property(a => a.Phone).IsModified = true;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"HELLO FROM api/authos/{id}/update/{phonenumber}");
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
