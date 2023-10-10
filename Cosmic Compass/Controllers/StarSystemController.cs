using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Cosmic_Compass.Repository;
using Cosmic_Compass.Documents;
using System;
using Redis.OM.Searching;

namespace Cosmic_Compass.Controllers
{
    [ApiController]
    public class StarSystemController : Controller
    {
        private StarSystemRepository _repository;

        public StarSystemController()
        {
            _repository = new StarSystemRepository();
        }

        /// <summary>
        /// Endpoint for listing existing star systems.
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "systems")]
        public IActionResult ListStarSystems()
        {
            JArray starSystemsInJson = new JArray();
            IRedisCollection<StarSystem> starSystems = _repository.FindAll();
            foreach (StarSystem starSystem in starSystems)
            {
                JObject jStarSystem = new JObject(new JProperty("StarSystemId", starSystem.StarSystemId.ToString()), new JProperty("Name", starSystem.Name.ToString()));
                starSystemsInJson.Add(jStarSystem);
            }

            return Ok(starSystemsInJson.ToString());
        }

        /// <summary>
        /// Endpoint for system creation. 
        /// </summary>
        /// <remarks>
        /// Request should be equivalent to:
        /// 
        ///     POST /systems
        ///     {
        ///        "Stars": [],
        ///        "Planets": []
        ///     }
        ///     
        /// All information is set by default.
        /// </remarks>
        /// <param name="starSystem"></param>
        /// <returns>The ID of the created system.</returns>
        [HttpPost(template: "systems")]
        public IActionResult CreateSystem(StarSystem starSystem)
        {
            string id = _repository.Create(starSystem);

            return Ok(id);
        }

        /// <summary>
        /// Endpoint for updating the info on a star system.
        /// </summary>
        /// <remarks>
        /// This request only receives the id for the star system by url path.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut(template: "systems/{id}")]
        public IActionResult UpdateSystem(string id, StarSystem updatedStarSystem)
        {
            IActionResult response = Ok();
            string updateId = "";
            StarSystem existingStarSystem = _repository.Get(id: id);
            if (existingStarSystem == null)
            {
                response = NotFound("The requested star system does not exist");
            }
            else
            {
                if(updatedStarSystem.Stars.Count() > 0)
                {
                    response = BadRequest("To update the star collection of the system, please refer to each star individually.");
                }
                else
                {
                    existingStarSystem.Name = updatedStarSystem.Name;
                    updateId = _repository.Update(id: id, updatedStarSystem: existingStarSystem);
                    response = Ok("The star system " + id + " has been updated successfully.");
                }
                
            }

            return response;
        }

        /// <summary>
        /// Endpoint for deleting a star system.
        /// </summary>
        /// <remarks>
        /// This request only receives the id for the star system by url path.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("systems/{id}")]
        public IActionResult DeleteStarSystem(string id)
        {
            var starSystem = _repository.Get(id: id);
            if (starSystem == null)
            {
                return NotFound();
            }

            _repository.Remove(id);

            return NoContent(); 
        }
    }
}
