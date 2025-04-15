<h1>Location Forecasting</h1>
<h3>Overview</h3>
<p>
  Select any location in the United States using google maps and receive the city, state, and the week's forecast.
</p>

<div>
  <h3>How it Works</h3>

  <h4>Getting the Location</h4>
  <p>
    The user clicks a location on the map, in the background, the map grabs the latitude and longitude of the placed mark. 
    I then call the Google Maps API and pass in the latitude and longitude. It returns with lots of information regarding that position so I only grab the City and State name.
  </p>

  <h4>Getting the Weather Forecast</h4>
  <p>
    Once again, I use the latitude and longitude to send to National Weather Service API which then returns the grid that contains the latitude longitude position. 
    With that grid, a forecast url is also returned which contains the weather forecast of that region.
  </p>

  <h4>Database Functionality</h4>
  <p>
    I use a database to store weather request logs, exception logs, and user info.
    I give the user a cookie the first time they enter the website that is used with the logs.
    The request logs table shows the latitude, longitude, forecast url, the forecast data for the first day, and the request time along with the user that requested the forecast.
    The exception logs shows the exception message, stack trace, exception time, and the user that experienced the exception.
  </p>
</div>

<div>
  <img src="Images/landscape.png" width=800>
  <img src="Images/portrait.png" width=400>
</div>

