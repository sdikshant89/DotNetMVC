$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    var datatable = $('#myTable').DataTable({
        "ajax": { url:'/Admin/Product/getAll'},
        "columns": [
            { data: 'name' },
            { data: 'brand'},
            { data: 'price'},
            { data: 'category.name' },
            { data: 'rating'},
            {
                data: 'productId',
                "render": function (data, type, row) {
                    const imageUrl = row.imageUrl ? encodeURIComponent(row.imageUrl) : "";
                    return `
                    <div class="btn-group" role="group">
                            <a class="btn btn-success btn-md mx-2 rounded-pill"
                               href="/Admin/Product/Edit?productId=${data}">
                                <i class="bi bi-pencil-fill"></i> Edit
                            </a>
                            <a class="btn btn-danger btn-md mx-2 rounded-pill"
                               onclick="prepareDeleteModal(${data}, '${row.name}')"
                               data-bs-toggle="modal"
                               data-bs-target="#deleteProductModal">
                                <i class="bi bi-trash3-fill"></i> Delete
                            </a>
                            <a class="btn btn-info btn-md mx-2 rounded-pill"
                               onclick="prepareInfo(${data}, '${row.name}', ${row.price}, '${row.description || ""}', decodeURIComponent('${imageUrl}'), '${row.category ? row.category.name : ""}')"
                               data-bs-toggle="modal"
                               data-bs-target="#infoProductModal">
                                <i class="bi bi-arrows-angle-expand"></i> More info
                            </a>
                        </div>
                    `;
                },
                width: '40%'
            }
        ]
    });
}