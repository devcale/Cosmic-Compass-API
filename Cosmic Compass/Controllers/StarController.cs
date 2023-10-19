using Microsoft.AspNetCore.Mvc;
using Cosmic_Compass.Documents;
using Cosmic_Compass.Repository;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

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
            _starSystemRepository.Update(id: starSystemId, updatedStarSystem: starSystem);
            response = Ok("The star [" + star.StarId + ": " + star.Name + "] has been added successfully to the star system [" + starSystemId + ": " + starSystem.Name + "].");

            return response;
        }


        /// <summary>
        /// Endpoint for listing existing stars on a given star system.
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "systems/{starSystemId}/stars")]
        public IActionResult ListStars(string starSystemId)
        {
            JArray starsInJson = new JArray();
            IActionResult response = Ok();
            try
            {
                ICollection<Star> stars = _starRepository.FindAll(starSystemId);
                foreach (Star star in stars)
                {
                    JObject jstar = new JObject(new JProperty("starId", star.StarId.ToString()), new JProperty("StarSystemId", star.StarSystemId.ToString()), new JProperty("Name", star.Name.ToString()), new JProperty("Type", star.Type.ToString()), new JProperty("Mass", star.Mass));
                    starsInJson.Add(jstar);
                }

                return Ok(starsInJson.ToString());
            }
            catch (Exception ex)
            {
                response = BadRequest(ex.Message);
            }
            return response;

        }

        /// <summary>
        /// Endpoint for getting a specific star from a star system.
        /// </summary>
        /// <remarks>
        /// This request receives the id for the star system as well as the id for the star by url path.
        /// </remarks>
        /// <param name="systemId"></param>
        /// <param name="starId"></param>
        /// <returns></returns>
        [HttpGet(template: "systems/{systemId}/stars/{starId}")]
        public IActionResult GetStar(string systemId, string starId)
        {
            IActionResult response = Ok();
            try
            {
                Star? star = _starRepository.Get(systemId, starId);
                if (star == null)
                {
                    response = NotFound("The requested star does not exist on the requested star system");
                }
                else
                {
                    response = Ok(star);
                }
            }
            catch (Exception ex)
            {
                response = BadRequest(ex.Message);

            }


            return response;
        }

        /// <summary>
        /// Endpoint for updating the info on a star from a star system.
        /// </summary>
        /// <remarks>
        /// This request receives the id for the star system and the id for the star by url path.
        /// </remarks>
        /// <param name="starSystemId"></param>
        /// <param name="starId"></param>
        /// <returns></returns>
        [HttpPut(template: "systems/{starSystemId}/stars/{starId}")]
        public IActionResult UpdateStar([FromRoute] string starSystemId, [FromRoute] string starId, [FromBody] Star updatedStar)
        {
            IActionResult response = Ok();
            StarSystem starSystem = _starSystemRepository.Get(starSystemId);
            Star? star = _starRepository.Get(starSystemId, starId);
            if (star == null)
            {
                response = NotFound("The requested star does not exist");
            }
            else if (star.StarSystemId != starSystemId)
            {
                response = BadRequest("The requested star is not part of the requested star system.");
            }
            else
            {
                foreach (Star p in starSystem.Stars)
                {
                    if (p.StarId.ToString() == starId)
                    {
                        p.Name = updatedStar.Name != "" ? updatedStar.Name : p.Name;
                        p.Type = updatedStar.Type != Enums.StarType.Unknown ? updatedStar.Type : p.Type;
                        p.Mass = updatedStar.Mass > 0 ? updatedStar.Mass : p.Mass;
                    }
                }
                try
                {
                    _starSystemRepository.Update(id: starSystemId, updatedStarSystem: starSystem);
                    response = Ok("The star " + starId + " has been updated successfully.");
                }
                catch (Exception ex)
                {
                    response = BadRequest(ex.Message);
                }

            }

            return response;
        }

    }
}
