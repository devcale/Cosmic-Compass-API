using Cosmic_Compass.Documents;
using Cosmic_Compass.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Redis.OM.Searching;
using System.Numerics;

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
            _starSystemRepository.Update(id: starSystemId, updatedStarSystem: starSystem);
            response = Ok("The planet [" + planet.PlanetId + ": " + planet.Name + "]  has been added successfully to the star system [" + starSystem.StarSystemId + ": " + starSystem.Name + "].");

            return response;
        }

        /// <summary>
        /// Endpoint for listing existing planets on a given star system.
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "systems/{starSystemId}/planets")]
        public IActionResult ListPlanets(string starSystemId)
        {
            JArray planetsInJson = new JArray();
            IActionResult response = Ok();
            try
            {
                ICollection<Planet> planets = _planetRepository.FindAll(starSystemId);
                foreach (Planet planet in planets)
                {
                    JObject jPlanet = new JObject(new JProperty("PlanetId", planet.PlanetId.ToString()), new JProperty("StarSystemId", planet.StarSystemId.ToString()), new JProperty("Name", planet.Name.ToString()), new JProperty("Type", planet.Type.ToString()), new JProperty("Radius", planet.Radius), new JProperty("DistanceFromStar", planet.DistanceFromStar), new JProperty("Habitable", planet.Habitable));
                    planetsInJson.Add(jPlanet);
                }

                return Ok(planetsInJson.ToString());
            } catch (Exception ex)
            {
                response = BadRequest(ex.Message);
            }
            return response;

        }

        /// <summary>
        /// Endpoint for getting a specific planet from a star system.
        /// </summary>
        /// <remarks>
        /// This request receives the id for the star system as well as the id for the planet by url path.
        /// </remarks>
        /// <param name="systemId"></param>
        /// <param name="planetId"></param>
        /// <returns></returns>
        [HttpGet(template: "systems/{systemId}/planets/{planetId}")]
        public IActionResult GetPlanet(string systemId, string planetId)
        {
            IActionResult response = Ok();
            try
            {
                Planet? planet = _planetRepository.Get(systemId, planetId);
                if (planet == null)
                {
                    response = NotFound("The requested planet does not exist on the requested star system");
                }
                else
                {
                    response = Ok(planet);
                }
            }
            catch (Exception ex) {
                response = BadRequest(ex.Message);

            }


            return response;
        }


        /// <summary>
        /// Endpoint for updating the info on a planet from a star system.
        /// </summary>
        /// <remarks>
        /// This request receives the id for the star system and the id for the planet by url path.
        /// </remarks>
        /// <param name="starSystemId"></param>
        /// <param name="planetId"></param>
        /// <returns></returns>
        [HttpPut(template: "systems/{starSystemId}/planets/{planetId}")]
        public IActionResult UpdatePlanet([FromRoute] string starSystemId, [FromRoute] string planetId, [FromBody] Planet updatedPlanet)
        {
            IActionResult response = Ok();
            StarSystem starSystem = _starSystemRepository.Get(starSystemId);
            Planet? planet = _planetRepository.Get(starSystemId, planetId);
            if (planet == null)
            {
                response = NotFound("The requested planet does not exist");
            }
            else if (planet.StarSystemId != starSystemId)
            {
                response = BadRequest("The requested planet is not part of the requested star system.");
            }
            else
            {                
                foreach( Planet p in starSystem.Planets )
                {
                    if(p.PlanetId.ToString() == planetId)
                    {
                        p.Name = updatedPlanet.Name != "" ? updatedPlanet.Name : p.Name;
                        p.Type = updatedPlanet.Type != Enums.PlanetType.Unknown ? updatedPlanet.Type : p.Type;
                        p.Radius = updatedPlanet.Radius > 0 ? updatedPlanet.Radius : p.Radius;
                        p.DistanceFromStar= updatedPlanet.DistanceFromStar > 0 ? updatedPlanet.DistanceFromStar : p.DistanceFromStar;
                        p.Habitable= updatedPlanet.Habitable;
                    }    
                }
                try
                {
                    _starSystemRepository.Update(id: starSystemId, updatedStarSystem: starSystem);
                    response = Ok("The planet " + planetId + " has been updated successfully.");
                }
                catch (Exception ex)
                {
                    response = BadRequest(ex.Message);
                }

            }

            return response;
        }

        /// <summary>
        /// Endpoint for deleting a planet from a star system.
        /// </summary>
        /// <remarks>
        /// This request receives both the id for the star system and the planet by url path.
        /// </remarks>
        /// <param name="system_id"></param>
        /// <param name="planet_id"></param>
        /// <returns></returns>
        [HttpDelete("systems/{system_id}/planets/{planet_id}")]
        public IActionResult DeletePlanet(string system_id, string planet_id)
        {
            IActionResult response = Ok();
            StarSystem starSystem = _starSystemRepository.Get(system_id);
            Planet? planet = _planetRepository.Get(system_id, planet_id);
            if (planet == null)
            {
                response = NotFound("The requested planet does not exist");
            }
            else if (planet.StarSystemId != system_id)
            {
                response = BadRequest("The requested planet is not part of the requested star system.");
            }
            else
            {
                Planet requested = null;
                foreach( Planet p in starSystem.Planets )
                {
                    if(p.PlanetId.ToString() == planet_id)
                    {
                        requested = p; break;
                    }    
                }
                starSystem.Planets.Remove(requested);
                try
                {
                    _starSystemRepository.Update(id: system_id, updatedStarSystem: starSystem);
                    response = Ok("The planet " + planet_id + " has been removed successfully.");
                }
                catch (Exception ex)
                {
                    response = BadRequest(ex.Message);
                }
            }

            return NoContent();
        }
    }
}
