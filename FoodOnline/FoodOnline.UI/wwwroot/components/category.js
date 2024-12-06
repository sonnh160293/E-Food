(function () {
    //display table
    const elementName = "#category-list";
    const columns = [

        { data: 'id', name: 'id', autoWidth: true },
        { data: 'name', name: 'name', autoWidth: true },
        { data: 'isActive', name: 'isActive', autoWidth: true },
        {
            data: 'lastModifiedDate',
            name: 'lastModifiedDate',
            autoWidth: true,
            render: function (data, type, row) {
                if (!data) {
                    return ''; // Return an empty string if data is null or undefined
                }
                // Format date to dd/mm/yyyy hh:mm:ss using Moment.js
                return moment(data).format('DD/MM/YYYY HH:mm:ss');
            }
        },
        { data: 'lastModifiedBy', name: 'lastModifiedBy', autoWidth: true },
      
        {
            data: 'id',
            name: 'id',
            width: '100',
            render: function (data, type, row) {
                // Thay thế cột ID bằng các nút với giá trị là ID
                return `
                                       <span data-key="${data}">
                                            <a  class="btn btn-info btn-sm btn-update" type="button">Update</a>
                                            <a class="btn btn-danger btn-sm btn-delete"  >Delete</a>
                                       <span/>
                                    `;
            }
        }
    ];
    const urlApi = "/Admin/Category/GetCategoryPagination";
    registerDatatable(elementName, columns, urlApi);

    //display category info in modal

    $(document).on('click', '.btn-update', function () {
        const key = $(this).closest('span').data('key');
        $.ajax({
            url: `/Admin/Category/GetCategoryById?id=${key}`,
            method: "GET",
            success: function (response) {
                console.log(response);
                mapObjectToControlView(response);
                $('#category-modal').modal('show');
            },
            error: function (error) {
                console.error("Error:", error);
            }
        });
    });

    
    //save data
    $('#form-category').submit(function (e) {
        e.preventDefault();

        const formData = $(this).serialize();

        $.ajax({
            url: $(this).attr('action'),
            method: $(this).attr('method'),
            data: formData,
            success: function (response) {
                if (response.success) {
                    showToaster("Success", " success");
                    $('#category-modal').modal('hide');
                    $(elementName).DataTable().ajax.reload();
                }
            },
            error: function (error) {
                showToaster("Error", " fail");
            }
        });
    });

    //delete
    $(document).on("click", '.btn-delete', function () {

        const key = $(this).closest('span').data('key');

        $.ajax({
            url: '/Admin/Category/Delete/' + key,
            dataType: 'json',
            method: 'POST',
            success: function (response) {
                $(elementName).DataTable().ajax.reload();

                showToaster("Success", "Delete success");

                console.log(response)
            }
        })

    });

})()

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
