$(async function () {
    async function Load() {
        await asyncAjax("/page/monitoring/get-monitoring-list-ajax", "POST")
            .then(async function successCallBack(response) {
                console.log(response);
                if (response.success) {
                    console.log(response.data);
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
            //var tbl = $('#dataTable').DataTable();
            //tbl.destroy();
            $('#dataTable').DataTable({
                width: "100%",
                data: listData,
                columns: [
                    { data: 'id', defaultcontent: "", visible: false },
                    {
                        data: 'year',
                        defaultcontent: "",
                        title: "Year"
                    },
                    {
                        data: 'status',
                        defaultContent: "-",
                        render: function (data, type, full, meta) {
                            return '<label class="badge bg-label-info">' + data + '</label>';
                        },
                        title: "Status"
                    },
                    {
                        data: 'createdBy',
                        defaultContent: "-",
                        title: "Created By"
                    },
                    {
                        data: 'createdDate',
                        defaultContent: "-",
                        render: function (data, type, full, meta) {
                            return convertDate(data);
                        },
                        title: "Created Date"
                    },
                    {
                        data: 'auditBy',
                        defaultContent: "-",
                        title: "Audit By"
                    },
                    {
                        data: 'auditDate',
                        defaultContent: "-",
                        render: function (data, type, full, meta) {
                            return convertDate(data);
                        },
                        title: "Audit Date"
                    },
                    {
                        data: null,
                        defaultContent: "-",
                        orderable: false,
                        render: function (data, type, row, meta) {
                            return `<button class="btn btn-primary movePage">
                                <i class='bx bxs-book-content me-1'></i>
                                Detail
                            </button>`;
                        }
                    }
                ],
                columnDefs: [],
                lengthMenu: [[5, 10, 50, 100, -1], [5, 10, 50, 100, "All"]],
                autoWidth: true,
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                scrollX: true,
                bScrollCollapse: true,
                order: [[1, 'desc']],
            });

            $('#dataTable').on('click', '.movePage', await movePage);
            async function movePage() {
                var
                    tb = $('#dataTable').DataTable(),
                    dt = tb.row($(this).parents('tr')).data(),
                    urldetail = decodeURIComponent(baseUrl+"/page/monitoring/detail?Year=Params"),
                    url = decodeURIComponent(urldetail);
                url = url.replace("Params", window.btoa(dt.year));

                window.location.href = url;
            }
        }
    }

    await Load();
})