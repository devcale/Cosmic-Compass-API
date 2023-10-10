using Cosmic_Compass.Documents;
using Cosmic_Compass.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Redis.OM.Searching;

namespace Cosmic_Compass.Controllers
{
    public class PlanetController : Controller
    {
        private PlanetRepository _planetRepository;
        private StarSystemRepository _starSystemRepository;

        public PlanetController()
        {
            _planetRepository = new PlanetRepository();
            _starSystemRepository = new StarSystemRepository();
        }

        /// <summary>
        /// Endpoint for listing existing planets on a given star system.
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "systems/{starSystemId}/planets")]
        public IActionResult ListPlanets(string starSystemId)
        {
            JArray planetsInJson = new JArray();
            ICollection<Planet> planets = _planetRepository.FindAll(starSystemId);
            foreach (Planet planet in planets)
            {
                JObject jPlanet = new JObject(new JProperty("PlanetId", planet.PlanetId.ToString()), new JProperty("StarSystemId", planet.StarSystemId.ToString()), new JProperty("Name", planet.Name.ToString()), new JProperty("Type", planet.Type.ToString()), new JProperty("Radius", planet.Radius), new JProperty("DistanceFromStar", planet.DistanceFromStar), new JProperty("Habitable", planet.Habitable));
                planetsInJson.Add(jPlanet);
            }

            return Ok(planetsInJson.ToString());
        }

        /// <summary>
        /// Endpoint for creating a new Planet and adding it to a star system.
        /// </summary>
        /// <remarks>
        /// Request should be equivalent to:
        /// 
        ///     POST /systems/{starSystemId}/stars
        ///     {
        ///        "Name": "Terra",
        ///        "PlanetType": "Rocky",
        ///        "Radius": 6371,
        ///        "DistanceFromStar": 1,
        ///        "Habitable": true
        ///     }
        ///     
        /// </remarks>
        /// <param name="starSystemId"></param>
        /// <param name="planet"></param>
        /// <returns></returns>
        [HttpPost(template: "systems/{starSystemId}/planets")]
        public IActionResult CreatePlanet(string starSystemId, Planet planet)
        {
            IActionResult response = Ok();
            StarSystem starSystem = _starSystemRepository.Get(starSystemId);

            string id = _planetRepository.Create(planet: planet);
            starSystem.Planets.Add(planet);
            string updatedStarSystemId = _starSystemRepository.Update(id: starSystemId, updatedStarSystem: starSystem);
            response = Ok("The planet [" + planet.PlanetId + ": " + planet.Name + "]  has been added successfully to the star system [" + starSystem.StarSystemId + ": " + starSystem.Name + "].");

            return response;
        }
    }
}
