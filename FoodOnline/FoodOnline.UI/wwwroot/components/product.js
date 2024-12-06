//display table
const elementName = "#product-list";
const columns = [

    { data: 'id', name: 'id', autoWidth: true },
    { data: 'name', name: 'name', autoWidth: true },
    { data: 'weight', name: 'weight', autoWidth: true },
    { data: 'volume', name: 'volume', autoWidth: true },
    { data: 'unitPrice', name: 'unitPrice', autoWidth: true },
    { data: 'categoryName', name: 'categoryName', autoWidth: true },
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
                                           <a href="/Admin/Product/SaveData/${data}" class="btn btn-info btn-sm" type="button">Update</a>
                                            <a class="btn btn-danger btn-sm btn-delete"  >Delete</a>
                                       <span/>
                                    `;
        }
    }
];
const urlApi = "/Admin/Product/GetProductsForDatatable";
registerDatatable(elementName, columns, urlApi);