$(async function () {
    async function Load() {
        await asyncAjax("/page/document-upload/get-dokumen-ajax", "POST")
            .then(async function successCallBack(response) {
                console.log(response);
                if (response.success) {
                    OnSuccess(response.data);
                } else {
                    swallAllert.Error("Fetch Data Failed!", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            });

        /** When success fetch data user and create dataTable */
        async function OnSuccess(listData) {
            var tbl = $('#dataTable').DataTable();
            tbl.destroy();

            $('#dataTable').DataTable({
                width: "100%",
                data: listData,
                columns: [
                    { data: 'id', defaultcontent: "", visible: false },
                    { data: 'year', defaultcontent: "" },
                    {
                        data: 'createdBy',
                        defaultContent: "-"
                    },
                    {
                        data: 'createdDate',
                        defaultContent: "-",
                        render: function (data, type, full, meta) {
                            return convertDate(data);
                        }
                    },
                    {
                        data: null,
                        defaultContent: "-",
                        orderable: false,
                        render: function (data, type, row, meta) {
                            return `
                                <button type="button" class="btn btn-sm btn-primary movePage" data-bs-toggle="tooltip" data-bs-placement="top" title="Upload Document">
                                    <i class='bx bx-upload me-1'></i>
                                    Upload Document
                                </button>`;
                        }
                    },

                ],
                columnDefs: [
                    {
                        sortable: false,
                        orderable: false,
                        searchable: false,
                        targets: 0,
                    }
                ],
                lengthMenu: [[5, 10, 50, 100, -1], [5, 10, 50, 100, "All"]],
                autoWidth: true,
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                scrollX: true,
                bScrollCollapse: true,
                fixedColumns: {
                    left: 1,
                },
                order: [[1, 'desc']]
            });

            $('#dataTable').on('click', '.movePage', await movePage);
            async function movePage() {
                var
                    tb = $('#dataTable').DataTable(),
                    dt = tb.row($(this).parents('tr')).data(),
                    urldetail = decodeURIComponent(baseUrl + "/page/document-upload/detail?Year=Params"),
                    url = decodeURIComponent(urldetail);
                url = url.replace("Params", window.btoa(dt.year));

                window.location.href = url;
            }
        }
    }

    await Load();
})