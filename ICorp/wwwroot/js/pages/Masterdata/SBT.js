$(document).ready(async function () {
    async function Load() {
        /** Fetch data user*/
        await asyncAjax("/master/SBT/get-SBT-list", "POST")
            .then(async function successCallBack(response) {
                if (response.success) {
                    await OnSuccess(response.data);
                } else {
                    swallAllert.Error("Fetch Data Failed!", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            })

        /** When success fetch data user and create dataTable */
        async function OnSuccess(listData) {
            var tbl = $('#dataTable').DataTable();
            tbl.destroy();

            var t = $('#dataTable').DataTable({
                width: "100%",
                data: listData,
                columns: [
                    {
                        data: null,
                        defaultContent: "-",
                        render: function (data, type, row, meta) {
                            return meta.row + meta.settings._iDisplayStart + 1;
                        }
                    },
                    { data: 'id', defaultcontent: "", visible: false },
                    { data: 'sbtIndex', defaultcontent: "" },
                    { data: 'level1', defaultcontent: "" },
                    { data: 'level2', defaultcontent: "" },
                    { data: 'level3', defaultcontent: "" },
                    { data: 'penjelasan', defaultcontent: "" },
                    /*{
                        data: null,
                        render: function (data, type, full, meta) {
                            return `<div class="d-flex">
                                            <button type="button" class="btn text-warning col btn-link btn-icon editButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="Edit">
                                                <i class="bx bx-edit-alt"></i>
                                            </button>
                                            <button type="button" class="btn text-danger col btn-link btn-icon deleteButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="Delete">
                                                <i class="bx bx-trash"></i>
                                            </button>
                                        </div>`;
                        },
                    }*/

                ],
                columnDefs: [
                    {
                        searchable: false,
                        targets: 0,
                    },
                    {
                        searchable: false,
                        orderable: false,
                        visible: false,
                        targets: 1,
                    }
                ],
                lengthMenu: [[5, 10, 50, 100, -1], [5, 10, 50, 100, "All"]],
                autoWidth: true,
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                bScrollCollapse: true
            });

            t.on('order.dt search.dt', function () {
                let i = 1;

                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        }
    }

    await Load();
});