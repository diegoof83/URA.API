using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URA.API.Domain.Models;
using URA.API.Domain.Services;

namespace URA.API.Controllers
{
    [ApiController]
    [Route("api/ura/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _service;

        public UsersController(IUsersService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            var result = _service.GetAll();

            if (!result.Any())
            {
                return NoContent();
            }

            return Ok(result.OrderBy(entity => entity.FirstName));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetById(long id)
        {
            var entity = _service.GetById(id);

            if (entity == null)
                return NotFound(new { Message = "Object has been not found." });

            return Ok(entity);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> Post(User entity)
        {
            try
            {
                var result = _service.Create(entity);

                return new CreatedResult($"/users/{result.Id}", result);
            }
            catch (Exception e)
            {
                return ValidationProblem(e.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> Put(long id, User updatedEntity)
        {
            try
            {
                var entity = _service.GetById(id);

                if (entity == null)
                    return NotFound(new { Message = "Object has been not found." });

                entity.FirstName = updatedEntity.FirstName;
                entity.LastName = updatedEntity.LastName;
                entity.Email = updatedEntity.Email;
                entity.Password = updatedEntity.Password;

                entity = _service.Update(entity);

                return Ok(entity);
            }
            catch (Exception e)
            {
                return ValidationProblem(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(long id)
        {
            var entity = _service.GetById(id);

            if (entity == null)
                return NotFound(new { Message = "Object has been not found." });

            _service.Delete(entity);

            return Ok();
        }
    }
}
