using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Controllers.V2;
using WebApplication1.Models;

namespace WebApplication1.Controllers.V1
{
    [ApiController]
    [Route("/api/authors")]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly BookStoreDBContext _context;

        // Dependency Injection
        public AuthorController(ILogger<WeatherForecastController> logger, BookStoreDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            var response = new List<Author>();
            try
            {
                _logger.LogInformation("HELLO FROM AuthorController/GetAll");
                response = _context.Author.ToList();

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
                response = _context.Author.FirstOrDefault(author => author.AuthorId == id);
            }
            catch(Exception ex)
            {
                _logger.LogInformation("HELLO FROM api/authos");
                return BadRequest(ex.Message);
            }

            return Ok(response);
        }


        /// <summary>
        /// Add an author
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST author/add
        ///     {
        ///        "firstName": "foo",
        ///        "lastName": "bar"
        ///     }
        ///
        /// </remarks>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response> 
        [HttpPost]
        [Route("add")]
        public IActionResult Add(string firstName, string lastName)
        {
            try
            {
                Author author = new Author
                {
                    FirstName = firstName,
                    LastName = lastName
                };
                _context.Author.Add(author);
                _context.SaveChanges();
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
                var author = new Author()
                {
                    AuthorId = id,
                    Phone = phonenumber
                };
                _context.Author.Attach(author);
                _context.Entry(author).Property(a => a.Phone).IsModified = true;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"HELLO FROM api/authos/{id}/update/{phonenumber}");
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // NOTE: Attribute route with url param will make it required; you can also mix url param with query param.
    }
}
