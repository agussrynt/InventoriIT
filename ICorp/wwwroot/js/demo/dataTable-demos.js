// Call the dataTables jQuery plugin
$(document).ready(function () {
    $('#dataTable').DataTable({
        width: "100%",
        scrollX: true,
        searching: false,
        columnDefs: [
            {
                sortable: false,
                orderable: false,
                searchable: false,
                targets: 0,
            }
        ],
        info: false,
        fixedColumns: {
            left: 1,
            //right: 1
        },
        bScrollCollapse: false,
        scrollY: false
    });
});
