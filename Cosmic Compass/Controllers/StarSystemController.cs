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
    }
}
