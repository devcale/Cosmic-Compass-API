using Cosmic_Compass.Documents;
using Cosmic_Compass.Repository;
using Microsoft.AspNetCore.Mvc;

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
