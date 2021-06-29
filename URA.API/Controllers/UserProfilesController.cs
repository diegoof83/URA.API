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
    public class UserProfilesController : ControllerBase
    {
        private readonly IUserProfilesService _service;

        public UserProfilesController(IUserProfilesService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<UserProfile>> GetAll()
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
        public ActionResult<UserProfile> GetById(string id)
        {
            var entity = _service.GetById(id);

            if (entity is null)
                return NotFound(new { Message = "Object has not been found." });

            return Ok(entity);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserProfile>> Post(UserProfile entity)
        {
            try
            {
                var result = await _service.CreateAsync(entity);

                return new CreatedResult($"/users/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return ValidationProblem(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserProfile> Put(string id, UserProfile userProfile)
        {
            try
            {
                if (id != userProfile.Id)
                    return BadRequest();

                var existingProfile = _service.GetById(id);

                if (existingProfile is null)
                    return NotFound(new { Message = "Object has not been found." });

                existingProfile.FirstName = userProfile.FirstName;
                existingProfile.LastName = userProfile.LastName;                

                existingProfile = _service.Update(existingProfile);

                return Ok(existingProfile);
            }
            catch (Exception ex)
            {
                return ValidationProblem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(string id)
        {
            var entity = _service.GetById(id);

            if (entity is null)
                return NotFound(new { Message = "Object has not been found." });

            _service.Delete(entity);

            return Ok();
        }
    }
}
