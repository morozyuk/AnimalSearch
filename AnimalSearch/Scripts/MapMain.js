﻿var markersArray = [];

function myMap() {// map start
    var mapProp = {//setting default map
        center: new google.maps.LatLng(51.508742, -0.120850),
        zoom: 3,
    };

    //search button call
    $('.buttonSearch').on('click', function (e) {
        e.preventDefault();
        getAnimals($('#searchString').val());
    })

    // ajax method for normal search
    getAnimals = function (query) {
        $.ajax({
            url: "/Home/GetAnimals",
            type: "post",
            data: { query: query },
            success: function (result) {
                $('#tbl-animals').html('');//clear table before next search
                clearOverlays();//clear markers before next search 
                for (var i = 0; i < result.length; i++) {

                    render(result[i]);

                }
            },
            error: function () {
                console.log("request error");
            }
        });
    };

    //delete button action
    $(document).on('click', '.btn-delete', function () {
        var id = $(this).data('animalid'); //setting id of animal to delete
        var $tr = $('#track-' + id);//row and id 
        $.ajax({
            url: "/Home/Delete",
            type: "get",
            data: { id: id },
            success: function () {
                $tr.remove();  //remove by row and id
            },
            error: function () {

            }
        });
    })
    // rendering data 
    render = function (animal) {

        // variable to fill table with data
        var tr = '<tr id="track-' + animal.id + '">'
            + '<td>' + animal.name + '</td>'
            + '<td>' + animal.kind + '</td>'
            + '<td>' + animal.breed + '</td>'
            + '<td>' + animal.description + '</td>'
            + '<td>' + animal.address + '</td>'
            + '<td>' + '<img height="60px" src="' + animal.imageSrc + '"/>' + '</td>'
            + '<td>'
            + '<button type="button" class="btn-delete btn" data=action="delete-animal" data-animalid="' + animal.id + '">Delete</button>' 
            + '<a href="/Home/Edit?id=' + animal.id + '" class="btn" >Edit</a>'
            + '</td>'
            + '</tr>';
        //adding data into a table
        var tbody = $('#tbl-animals').append(tr);

        var geocoder = new google.maps.Geocoder();//  geocoder to change adress from string to lat lng
        geocoder.geocode({
            'address': animal.address
        }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                var myLatlng = results[0].geometry.location;//setting lat lng
                //info window content creating
                var contentString = '<h2 class="infoWindowTitle">' + "Name : " + animal.name + '</h2>' +
                    '<h3 class="infoWindowContent">' + '<br />' + 'kind : ' + animal.kind +
                    '<br />' + 'breed : ' + animal.breed +
                    '<br />' + 'description : ' + animal.description +
                    '<br />' + 'address : ' + animal.address + '</h3>'
                    + '<img height="60px" src="' + animal.imageSrc + '"/>';

                var marker = new google.maps.Marker({//marker add
                    map: map,
                    position: myLatlng,
                    title: animal.name,
                    info: contentString
                });
                markersArray.push(marker);

                var circle = new google.maps.Circle({//adding circle around the marker
                    map: map,
                    radius: 100,
                    fillColor: '#AA0000'
                });
                circle.bindTo('center', marker, 'position');

                infoWindow = new google.maps.InfoWindow({ content: contentString });//adding info window
                google.maps.event.addListener(marker, 'click', function () {
                    infoWindow.setContent(this.info);
                    infoWindow.open(map, this);
                });
            }

        });


    };
    var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
    var inputAdress = document.getElementById('address');
    var autocomplete = new google.maps.places.Autocomplete(inputAdress);//adding autocomplete for input
    //getAnimals();
    var checkArr = [];
    var image = 'http://maps.google.com/mapfiles/ms/icons/blue.png';//poly marker image
    var isClosed = false;//is used to check when polygon is closed
    var poly = new google.maps.Polyline({ map: map, path: [], strokeColor: "#FF0000", strokeOpacity: 1.0, strokeWeight: 2 });//poly options
    var ajaxBut = document.getElementById('ajaxcheck'); //try to save polygon markers location
    ajaxBut.addEventListener('click', function () {
        var temp = [];
        for (var i = 0; i < checkArr.length; i++) {  //loop to check animals markers
            temp.push([checkArr[i].lat, checkArr[i].lng]);
        }
        $.ajax({
            url: "/Home/SearchPoly",//polygon search ajax
            type: "post",
            data: { cornes: temp },
            success: function (data) {//polygon search results
                if (data.length === 0)
                    alert('empty array');
                for (var i = 0; i < data.length; i++) {
                    // alert(data[i].Animal.Name + ' ' + data[i].Animal.Latitude + ' ' + data[i].Animal.Longtitude);
                    //console.log(data);
                    render(data[i]);
                }

            },
            eror: function () {
                alert("we run into some problems");
            }
        });
    });
    google.maps.event.addListener(map, 'click', function (clickEvent) {//adding polygon markers
        if (isClosed)
            return;
        var markerIndex = poly.getPath().length;
        var isFirstMarker = markerIndex === 0;
        var marker = new google.maps.Marker({ map: map, position: clickEvent.latLng, draggable: true, icon: image });//polygon markers options
        checkArr.push(marker.position);

        if (isFirstMarker) {//check , if click on first marker to close polygon
            google.maps.event.addListener(marker, 'click', function () {
                if (isClosed)
                    return;
                var path = poly.getPath();
                poly.setMap(null);
                poly = new google.maps.Polygon({ map: map, path: path, strokeColor: "#FF0000", strokeOpacity: 0.8, strokeWeight: 2, fillColor: "#FF0000", fillOpacity: 0.35 });//marks area inside polygon
                isClosed = true;
                console.log(poly);
            });

        }
        google.maps.event.addListener(marker, 'drag', function (dragEvent) {//add draging ability for markers
            poly.getPath().setAt(markerIndex, dragEvent.latLng);
        });
        poly.getPath().push(clickEvent.latLng);
       
    });
    //function to clear markers before next serch (if it is needed
    function clearOverlays() {
        for (var i = 0; i < markersArray.length; i++) {
            markersArray[i].setMap(null);
        }
        markersArray.length = 0;
    }
}
