function myMap() {
    var mapProp = {
        center: new google.maps.LatLng(51.508742, -0.120850),
        zoom: 5,
    };
    var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
    var inputAdress = document.getElementById('address');
    var autocomplete = new google.maps.places.Autocomplete(inputAdress);
}