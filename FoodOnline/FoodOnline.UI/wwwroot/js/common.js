function showToaster(type, text, timeout = 5000) {
    toastr.options = {
        closeButton: true,
        progressBar: true,
        positionClass: 'toast-bottom-right', // Position at bottom-right
        timeOut: timeout,
        extendedTimeOut: timeout,
        hideEasing: 'linear',
        hideMethod: 'fadeOut',
        showEasing: 'linear',
        showMethod: 'fadeIn'
    };

    toastr[type.toLowerCase()](text, type);
}

function mapObjectToControlView(modelView) {
    if (typeof (modelView) !== 'object') {
        return;
    }

    for (const property in modelView) {
        if (modelView.hasOwnProperty(property)) {
            // Convert the first character of the property to uppercase
            const [firstCharacter, ...restChar] = property;
            const capitalText = `${firstCharacter.toLocaleUpperCase()}${restChar.join('')}`;

            const inputField = $(`#${capitalText}`);

            if (inputField.length) {
                if (inputField.attr('type') === 'checkbox') {
                    // Set the checked property for checkboxes
                    inputField.prop('checked', modelView[property]);
                } else {
                    // Set the value for other input types
                    inputField.val(modelView[property]);
                }
            }
        }
    }
}
