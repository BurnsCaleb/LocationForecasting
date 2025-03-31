// Handles Ajax call to controller for gathering location

function getLocationDetails(lat, lng) {

    $.ajax({
        url: "/Home/GetLocation",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ latitude: lat, longitude: lng }),
        success: function (data) {
            if (data.error) {
                $("#location").html(`
                    <p>${data.error}</p>
                `);
            } else if (data.city && data.state) {
                updateLocationInfo(data.city, data.state);
            } else {
                $("#location").html(`
                    <p>No valid location data. Ensure to select a location in the United States</p>
                `);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error:", error);
            alert("Failed to retrieve location data.");
        },
    });
}