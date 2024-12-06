
//get data
function registerDatatable(elementName, columns, urlApi) {
    new DataTable(elementName, {

        processing: true,
        serverSide: true,
        columns: columns,
        ajax: {
            url: urlApi,
            type: 'POST',
            dataType: 'json'
        },
        lengthChange: true,
        searching: true,
        paging: true,
        info: true
    });
}