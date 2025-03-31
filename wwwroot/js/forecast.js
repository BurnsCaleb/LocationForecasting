// This file is meant to handle changing, updating, and displaying
// the location and forecast information on the DOM


function updateLocationInfo(city, state) {
    $("#location").html(
        `
        <h2>${city}, ${state}</h3>
        `
    )
}

function workingForecast() {
    $("#forecast").html(
        `
        <div id="working">
            <h3>Working...</h3>
        </div>
        `
    );

    $("#weather").html(
        `
        `
    )
}

function updateForecastInfo(data) {
    if (data && data.length > 0) {
      
        // Filter to only show daytime
        const dayTimeData = data.filter(item => item.isDayTime === true);
        // Filter to only show nighttime
        const nightTimeData = data.filter(item => item.isDayTime === false);

        if (data[0].isDayTime === false) {
            const updatedDayData = [];
            // Add tonight to day list
            updatedDayData.push(data[0]);

            // Add day time data minus last day because 
            // I'm adding tonight to the day list
            for (let i = 1; i <= dayTimeData.length - 1; i++) {
                updatedDayData.push(data[i])
            }

            // Set Day list again
            dayTimeData = updatedDayData;

            // Override first night object to null
            nightTimeData[0] = null;
        }

        // Set empty forecast element then append to it
        $("#forecast").html("");

        dayTimeData.forEach((item, index) => {
            if (index === 0) {
                $("#forecast").append(`
                    <div class="badge" id="badge-${index}">
                        <h3>Today</h3>
                        <h5>${item.temperature} ${item.temperatureUnit}</h5>
                    </div>
                `)
                $(`#badge-${index}`).on("click", function () {
                    console.log(`Badge ${index} clicked`);
                    updateWeatherInfo(dayTimeData[index], nightTimeData[index]);
                });
            } else {
                $("#forecast").append(`
                <div class="badge" id="badge-${index}">
                    <h3>${item.forecast}</h3>
                    <h5>${item.temperature} ${item.temperatureUnit}</h5>
                </div>
            `);
                $(`#badge-${index}`).on("click", function () {
                    console.log(`Badge ${index} clicked`);
                    updateWeatherInfo(dayTimeData[index], nightTimeData[index]);
                });
            }
        });
    } else {
        $("#weather").html("<p>Unable to retrieve weather information.</p>");
    }
}

function updateWeatherInfo(day, night) {
    if (day && night) {
        $("#weather").html(
            `
            <div id="dayTime">
            <p id="dayName">${day.forecast}</p>
            <p id="temp">${day.temperature} ${day.temperatureUnit}</p>
            <p id="details">${day.detailedForecast}</p>
            </div>
            <div id="nightTime">
            <p id="dayName">${night.forecast}</p>
            <p id="temp">${night.temperature} ${night.temperatureUnit}</p>
            <p id="details">${night.detailedForecast}</p>
            </div>
            `
        );
    } else if (day && !night) {
        $("#weather").html(
            `
            <div id="dayTime">
            <p id="dayName">${day.forecast}</p>
            <p id="temp">${day.temperature} ${day.temperatureUnit}</p>
            <p id="details">${day.detailedForecast}</p>
            </div>
            `
        );
    }

}