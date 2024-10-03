$(async function () {
    async function Load() {
        await asyncAjax("/page/auditexternal/get-list-ajax", "POST")
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
                    //{ data: 'iD_AUDIT_EXTERNAL', defaultcontent: "", visible: false },
                    { data: 'auditoR_NAME', defaultcontent: "" },
                    { data: 'totaL_SCORE', defaultcontent: "" },
                    {
                        data: 'date',
                        defaultContent: "-",
                        render: function (data, type, full, meta) {
                            return convertDate(data);
                        }
                    },
                    {
                        data: 'recomendatioN_UPLOAD',
                        defaultContent: "-",
                        render: function (data, type, row, meta) {
                            if (row.recomendatioN_UPLOAD == 1) {
                                return `<label class="badge bg-label-success">Yes</label>`;
                            }
                        }
                    },
                    {
                        data: 'attachmenT_NAME',
                        defaultContent: "-"
                    },
                    {
                        data: null,
                        defaultContent: "-",
                        orderable: false,
                        render: function (data, type, row, meta) {
                            //return `<button class="btn btn-success movePage">
                            //            <i class='bx bxs-book-alt'></i>
                            //            View
                            //        </button>`;

                            return `<button type="button" class="btn text-info col btn-link btn-icon viewButton movePage" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="View">
                                        <i class="bx bx-show"></i>
                                      </button>`
                        }
                    },

                ],
                columnDefs: [
                    {
                        sortable: false,
                        orderable: false,
                        searchable: false,
                        targets: 0,
                    },{
                        className: "text-center",
                        targets: [3,5],
                    }, {
                        className: "text-right",
                        targets: [1],
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
                    tb = $('#dataTable').DataTable(),
                    dt = tb.row($(this).parents('tr')).data(),
                    urldetail = decodeURIComponent(baseUrl + "/page/auditexternal/detail?id=Params"),
                    url = decodeURIComponent(urldetail);
                url = url.replace("Params", window.btoa(dt.iD_AUDIT_EXTERNAL));

                window.location.href = url;
            }
        }
    }

    await Load();
})