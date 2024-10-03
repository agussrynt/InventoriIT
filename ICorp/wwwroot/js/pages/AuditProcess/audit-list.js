$(async function () {
    async function Load() {
        await asyncAjax("/page/audits/get-auction-process-ajax", "POST")
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
                    { data: 'scoreParameter', defaultcontent: "", title: 'Total Score Parameter' },
                    { data: 'scoreFuk', defaultcontent: "", title: 'Total Score FUK' },
                    { data: 'scoreUp', defaultcontent: "", title: 'Total Score UP' },
                    {
                        data: 'status',
                        defaultContent: "-",
                        orderable: false,
                        render: function (data, type, row, meta) {
                            console.log(data);
                            if (!data) {
                                return `
                                    <button class="btn btn-primary movePage">
                                        <i class='bx bxs-book-content me-1'></i>
                                        Start Audit
                                    </button>`;
                            } else if (data == 1) {
                                return `
                                    <button class="btn btn-success movePage">
                                        <i class='bx bx-loader-circle'></i>
                                        On Process
                                    </button>`;
                            } else {
                                return `
                                    <button class="btn btn-success movePage">
                                        <i class='bx bxs-book-alt'></i>
                                        Complete
                                    </button>`;
                            }
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
                    urldetail = decodeURIComponent(baseUrl + "/page/audits/detail?Year=Params"),
                    url = decodeURIComponent(urldetail);
                url = url.replace("Params", window.btoa(dt.year));

                window.location.href = url;
            }
        }
    }

    await Load();
})