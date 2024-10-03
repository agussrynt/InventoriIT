$(async function () {
    async function Load() {
        await asyncAjax("/page/audits/get-auction-review-ajax", "POST")
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
            //var tbl = $('#dataTable').DataTable();
            //tbl.destroy();

            $('#dataTable').DataTable({
                width: "100%",
                data: listData,
                columns: [
                    {
                        data: null,
                        defaultcontent: "",
                        render: function (data, type, row, meta) {
                            return ``;
                        }
                    },
                    { data: 'id', defaultcontent: "", visible: false },
                    { data: 'year', defaultcontent: "", title: 'Year' },
                    { data: 'auditBy', defaultcontent: "", title: 'Audit By' },
                    { data: 'scoreParameter', defaultcontent: "", title: 'Score Parameter' },
                    { data: 'scoreFuk', defaultcontent: "", title: 'Score FUK' },
                    { data: 'scoreUp', defaultcontent: "", title: 'Score UP' },
                    {
                        data: 'createdBy',
                        defaultContent: "-",
                        title: 'Created By'
                    },
                    {
                        data: 'createdDate',
                        defaultContent: "-",
                        render: function (data, type, full, meta) {
                            return convertDate(data);
                        },
                        title: 'Created Date'
                    },
                    {
                        data: 'status',
                        defaultContent: "-",
                        orderable: false,
                        render: function (data, type, row, meta) {
                            return `<label class="badge bg-label-info">${data}</label>`;
                        },
                        title: 'Status'
                    },
                    {
                        data: null,
                        defaultContent: "-",
                        title: 'Action',
                        orderable: false,
                        render: function (data, type, row, meta) {
                            return `
                                <button data-year="${row['year']}" class="btn btn-primary movePage">
                                    <i class='bx bxs-book-content me-1'></i>
                                    Detail
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
                order: [[2, 'asc']]
            });

            $('#dataTable').on('click', '.movePage', await movePage);
            async function movePage() {
                var
                    year = $(this).data('year'),
                    urldetail = decodeURIComponent(baseUrl + "/page/audits/review-detail?Year=Params"),
                    url = decodeURIComponent(urldetail);
                url = url.replace("Params", window.btoa(year));

                window.location.href = url;
            }
        }
    }

    await Load();
})