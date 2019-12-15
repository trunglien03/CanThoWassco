
        function initMap() {
            var canTho = direction();
            var mapOption = {
                zoom: 16,
                center: canTho
            }
            map = new google.maps.Map(document.getElementById('map'), mapOption);
        }


        function direction() { // lấy vị trí hiện tại và chỉ đường

            var pos;
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    pos = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude
                    };// lấy latlng hiện tại. (điểm đầu)

                    var geocoder = new google.maps.Geocoder(); // khởi tạo một thể hiện của lớp
                    var address = document.getElementById('address').value; // lấy giá trị từ thẻ input
                    console.log(" trong file script" + address)
                    // console.log(address);
                    geocoder.geocode({'address': address}, function (results, status) {
                        var destination = '';// khởi tạo biến lưu latlng điểm cuối
                        if (status === 'OK') {
                            destination = results[0].geometry.location; // chuyển chuổi tìm kiếm thàng latlng
                        } else {
                            alert('Geocode was not successful for the following reason: ' + status); // báo lỗi không tìm thấy vị trí.
                        }
                        var directionService = new google.maps.DirectionsService(); // dịch vụ chỉ đường
                        var directionRenderer = new google.maps.DirectionsRenderer();// để vẽ đường
                        directionRenderer.setMap(map);// vẽ lên map
                        var request = {
                            origin: pos,
                            destination: destination,
                            travelMode: 'DRIVING'
                        };
                        directionService.route(request, function (response, status) { //response và status tham số mặc định.
                            if (status === 'OK') {
                                directionRenderer.setDirections(response);
                            } else {
                                window.alert('Lỗi không tìm được đường đi !!! Vui lòng liện hệ quản trị ' + status);
                            }
                        });

                    });


                })

            } else {
                alert("Geolocation is not supported by this browser.");
            }
        }
