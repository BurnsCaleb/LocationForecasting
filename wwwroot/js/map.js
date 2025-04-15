let map;
let marker;
const dsmLat = 41.59012;
const dsmLng = -93.6036
const zoomLvl = 8;
const mapId = '91353addb5b690b9';

async function initMap() {

    // Default Position - Des Moines Capital
    const position = { lat: dsmLat, lng: dsmLng };

    // Request needed libraries.
    //@ts-ignore
    const { Map } = await google.maps.importLibrary("maps");

    // Create Map
    map = new Map(document.getElementById("map"), {
        zoom: zoomLvl,
        center: position,
        mapId: mapId,
    });

    // Click Listener for placing marker on map
    map.addListener("click", (event) => {
        const latitude = event.latLng.lat();
        const longitude = event.latLng.lng();

        placeMarker(event.latLng);

        getWeatherData(latitude, longitude);
    });
}

function placeMarker(location) {
    if (marker) {
        marker.setMap(null);
    }

    // Place marker
    marker = new google.maps.marker.AdvancedMarkerElement({
        position: location,
        map: map,
    });
}

window.onload = () => {
    initMap();
    getWeatherData(dsmLat, dsmLng);
}