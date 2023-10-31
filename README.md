# 🌟 Cosmic Compass Restful API 🚀

Introducing the Cosmic Compass Restful API! 🌟 This API provides comprehensive and up-to-date information about the exoplanets in our universe. With a user-friendly interface, you can easily access a wealth of knowledge about exoplanets, including their name, host, number of hosts, number of planets on the same system, and much more. 🤩

Whether you're a seasoned astronomer 🔭 or simply interested in learning more about the stars and their planets 🤓, our API has everything you need. With a focus on accuracy and reliability 💯, you can trust that the information you receive is both accurate and up-to-date.

---

# Features 🎉

- RESTful architecture, allowing for easy integration with your own projects and applications. 💻
- Retrieve information about specific exoplanets with simple HTTP requests. 🔍
- Retrieve a list of exoplanets based on specific criteria. 🔎
- Focus on accuracy and reliability, providing you with trustworthy information. 💯

So why wait? 🤔 If you're looking for an authoritative and comprehensive source of information about exoplanets, look no further than the Star Information Restful API! 🌟

---

# Usage & API Documentation

## Relevant Configuration
The project is configured to run on localhost port 7183 by default. You can change this by modifying the applicationURL parameter on the `launchSettings.json` file.

In order to connect to your own Redis Database, you will need to update the environment variables. You should add:
- `ASPNETCORE_REDISPASS`: Your Redis connection password.
- `ASPNETCORE_REDISURL`: Your Redis connection url.
- `ASPNETCORE_REDISPORT`: The port you have setup for Redis.

## Endpoints

### Star Systems
The base url for the starsystems endpoint is `/api/systems`

#### Get All Star Systems

Endpoint: `GET /systems`

Description: This endpoint retrieves all star systems that are present in the database.

Example response: 
```json
[
  {
    "StarSystemId": "01HBYKFCQHWNH6T8A9H4V5N8KA",
    "Name": "Beta Centauri"
  },
  {
    "StarSystemId": "01HCBJT57REFVX9PT1WGGZ54YP",
    "Name": "Solis"
  }
]
```

#### Get Specific Star System

Endpoint: `GET /systems/{system_id}`

Description: This endpoint retrieves a specific star system based on the given id. The resopnse includes the star system information, as well as the system's stars and planets.

Example response: 
```json
{
  "starSystemId": "01HCBJT57REFVX9PT1WGGZ54YP",
  "name": "Solis",
  "stars": [
    {
      "starId": "01HD539TD9744369SK8940PF5J",
      "starSystemId": "01HCBJT57REFVX9PT1WGGZ54YP",
      "name": "Sol",
      "type": 2,
      "mass": 1
    }
  ],
  "planets": [
    {
      "planetId": "01HCBJWGZ2EJHBGZ0TR1BKCDKP",
      "starSystemId": "01HCBJT57REFVX9PT1WGGZ54YP",
      "name": "Mercury",
      "type": 0,
      "radius": 2450,
      "distanceFromStar": 0.39,
      "habitable": false
    },
    {
      "planetId": "01HE1GMDFKY37YPPYE4SEYS8NW",
      "starSystemId": "01HCBJT57REFVX9PT1WGGZ54YP",
      "name": "Venus",
      "type": 1,
      "radius": 6051,
      "distanceFromStar": 0.72,
      "habitable": false
    },
    {
      "planetId": "01HE1GNJR6W5PJZXCZRD0YM2B0",
      "starSystemId": "01HCBJT57REFVX9PT1WGGZ54YP",
      "name": "Earth",
      "type": 1,
      "radius": 6371,
      "distanceFromStar": 1,
      "habitable": true
    },
    {
      "planetId": "01HE1GP78EK2HTYCH440XDZ99F",
      "starSystemId": "01HCBJT57REFVX9PT1WGGZ54YP",
      "name": "Mars",
      "type": 1,
      "radius": 3389,
      "distanceFromStar": 1.52,
      "habitable": false
    },
    {
      "planetId": "01HE1GQGAHNATHND7CK2V2M9B0",
      "starSystemId": "01HCBJT57REFVX9PT1WGGZ54YP",
      "name": "Jupiter",
      "type": 2,
      "radius": 69911,
      "distanceFromStar": 5.2,
      "habitable": false
    },
    {
      "planetId": "01HE1GVPSXCZTM8SBZFV39G252",
      "starSystemId": "01HCBJT57REFVX9PT1WGGZ54YP",
      "name": "Saturn",
      "type": 2,
      "radius": 58232,
      "distanceFromStar": 9.58,
      "habitable": false
    },
    {
      "planetId": "01HE1GWPA41NYQ1TXNKF46M3Y8",
      "starSystemId": "01HCBJT57REFVX9PT1WGGZ54YP",
      "name": "Uranus",
      "type": 3,
      "radius": 25362,
      "distanceFromStar": 19.22,
      "habitable": false
    },
    {
      "planetId": "01HE1GXA8RXWEDNX9NWBKPNDYA",
      "starSystemId": "01HCBJT57REFVX9PT1WGGZ54YP",
      "name": "Neptune",
      "type": 3,
      "radius": 24622,
      "distanceFromStar": 30.05,
      "habitable": false
    }
  ]
}
```

#### Create a Star System

Endpoint: `POST /systems`

Description: This endpoint adds a new star system to the database.

Example request: 
```json
{
  "name": "Polaris"
}
```

Example response: 
`Cosmic_Compass.Documents.StarSystem:01HE1HCQ9NB9AEC0BHZ7Z9R08Z` (The ID of the newly created star system)

#### Update a Star System

Endpoint: `PUT /systems/{system_id}`

Description: This endpoint updates the attributes of an existing star system. This method is only used for the star system attributes, in order to update a star or a planet, please refer to their respective methods.

Example request: 
```json
{
  "name": "Sirius"
}
```

Example response: 

The star system `01HE1HCQ9NB9AEC0BHZ7Z9R08Z` has been updated successfully.


#### Delete a Star System

Endpoint: `DELETE /systems/{system_id}`

Description: This endpoint deletes an existing star system from the database.

The response for a successful scenario should be 204.
  
