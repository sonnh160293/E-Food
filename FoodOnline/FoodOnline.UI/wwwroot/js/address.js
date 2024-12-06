document.addEventListener("DOMContentLoaded", function () {
    var cities = document.getElementById("City");
    var districts = document.getElementById("District");
    var wards = document.getElementById("Ward");

    if (!cities || !districts || !wards) {
        console.error("One or more dropdown elements are missing.");
        return;
    }

    axios.get("https://raw.githubusercontent.com/kenzouno1/DiaGioiHanhChinhVN/master/data.json")
        .then(function (response) {
            const data = response.data;
            if (!Array.isArray(data)) {
                throw new Error("Data is not an array");
            }
            renderCity(data);
        })
        .catch(function (error) {
            console.error("Error fetching data:", error);
        });

    function renderCity(data) {
        cities.innerHTML = '<option value="" selected>Select your city</option>'; // Reset cities

        // Filter to find the city with the name "Thành phố Hà Nội"
        var hanoi = data.find(city => city.Name === "Thành phố Hà Nội");

        if (hanoi) {
            var opt = document.createElement("option");
            opt.value = hanoi.Name;
            opt.text = hanoi.Name;
            cities.appendChild(opt);
        }

        cities.addEventListener("change", function () {
            districts.innerHTML = '<option value="" selected>Select your District</option>'; // Reset districts
            wards.innerHTML = '<option value="" selected>Select your Ward</option>'; // Reset wards

            var selectedCityId = cities.value;
            var selectedCity = data.find(city => city.Name === selectedCityId);

            if (selectedCity) {
                selectedCity.Districts.forEach(function (district) {
                    var opt = document.createElement("option");
                    if (district.Name.startsWith("Qu")) {
                        opt.value = district.Name;
                        opt.text = district.Name;
                        districts.appendChild(opt);
                    }
                });
            }
        });

        districts.addEventListener("change", function () {
            wards.innerHTML = '<option value="" selected>Select your Ward</option>'; // Reset wards

            var selectedCityId = cities.value;
            var selectedDistrictId = districts.value;
            var selectedCity = data.find(city => city.Name === selectedCityId);

            if (selectedCity) {
                var selectedDistrict = selectedCity.Districts.find(district => district.Name === selectedDistrictId);
                if (selectedDistrict) {
                    selectedDistrict.Wards.forEach(function (ward) {
                        var opt = document.createElement("option");
                        opt.value = ward.Name;
                        opt.text = ward.Name;
                        wards.appendChild(opt);
                    });
                }
            }
        });
    }
});
