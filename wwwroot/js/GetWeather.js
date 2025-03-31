// Handles Ajax call to controller for gathering weather data

function getWeatherData(lat, lng) {
    getLocationDetails(lat, lng);
    workingForecast();
    $.ajax({
        url: "/Home/GetWeather",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ latitude: lat, longitude: lng }),
        success: function (data) {
            if (data.error) {
                $("#forecast").html(`
                    <p>${data.error}</p>
                `);
            } else {
                updateForecastInfo(data);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error:", error);
            alert("Failed getting weather data");
        },
    });
}