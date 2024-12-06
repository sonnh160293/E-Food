(function () {
    //display table
    const elementName = "#account-list";
    const columns = [

        { data: 'username', name: 'userName', autoWidth: true },
        { data: 'fullname', name: 'fullName', autoWidth: true },
        { data: 'email', name: 'email', autoWidth: true },
        { data: 'roleName', name: 'roleName', autoWidth: true },
        { data: 'branchName', name: 'branchName', autoWidth: true },
        { data: 'isActive', name: 'isActive', autoWidth: true },
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
    const urlApi = "/Admin/Account/GetUserPagination";
    registerDatatable(elementName, columns, urlApi);



    
})()
   