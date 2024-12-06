(function () {

    document.getElementById('image-upload').onchange = function () {
        const input = document.getElementById('image-upload').files[0];
        if (input) {
            document.getElementById('img-preview').src = URL.createObjectURL(input);
        }
    };

})();