using Microsoft.AspNetCore.Mvc;
using Cosmic_Compass.Documents;
using Cosmic_Compass.Repository;
using System.Numerics;

namespace Cosmic_Compass.Controllers
{
    [ApiController]
    public class StarController : Controller
    {
        private StarRepository _starRepository;
        private StarSystemRepository _starSystemRepository;

        public StarController()
        {
            _starRepository = new StarRepository();
            _starSystemRepository = new StarSystemRepository();
        }

        /// <summary>
        /// Endpoint for creating a new Star and adding it to a star system.
        /// </summary>
        /// <remarks>
        /// Request should be equivalent to:
        /// 
        ///     POST /systems/{starSystemId}/stars
        ///     {
        ///        "Name": "Sol",
        ///        "StarType": "RedDwarf",
        ///        "Mass": 1
        ///     }
        ///     
        /// </remarks>
        /// <param name="starSystemId"></param>
        /// <param name="star"></param>
        /// <returns></returns>
        [HttpPost(template: "systems/{starSystemId}/stars")]
        public IActionResult CreateStar(string starSystemId, Star star)
        {
            IActionResult response = Ok();
            StarSystem starSystem = _starSystemRepository.Get(starSystemId);

            star.StarSystemId = starSystemId;
            string id = _starRepository.Create(star: star);
            starSystem.Stars.Add(star);
            string updatedStarSystemId = _starSystemRepository.Update(id: starSystemId, updatedStarSystem: starSystem);
            response = Ok("The star [" + star.StarId + ": " + star.Name + "] has been added successfully to the star system [" + updatedStarSystemId + ": " + starSystem.Name + "].");

            return response;
        }

        
    }
}
