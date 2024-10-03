$(async function () {
    async function Load() {
        await asyncAjax("/master/year/get-list-year-ajax", "POST")
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
                    { data: 'year', defaultcontent: "" },
                    { data: 'remarks', defaultContent: "" },
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
                        data: 'lastUpdatedBy',
                        defaultContent: "-"
                    },
                    {
                        data: 'lastUpdatedDate',
                        defaultContent: "-",
                        render: function (data, type, full, meta) {
                            return convertDate(data);
                        }
                    },
                    {
                        data: null,
                        defaultContent: "-",
                        render: function (data, type, row, meta) {
                            //var btn = `
                            //    <a type="button" class="text-primary setAssignment" data-bs-toggle="tooltip" data-bs-placement="top" title="Set Assignment">
                            //        <i class='bx bx-street-view'></i>
                            //    </a>
                            //    <a href="#" class="text-info">
                            //        <i class='bx bx-edit'></i>
                            //    </a>
                            //    <a href="#" class="text-danger">
                            //        <i class='bx bx-trash'></i>
                            //    </a>
                            //`;
                            var btn = `
                                <a type="button" class="text-primary setAssignment" data-bs-toggle="tooltip" data-bs-placement="top" title="Set Assignment">
                                    <i class='bx bx-street-view'></i>
                                </a>
                            `;

                            return btn;
                        }
                    },

                ],
                columnDefs: [
                    //{
                    //    sortable: false,
                    //    orderable: false,
                    //    //className: "bg-white text-center",
                    //    searchable: false,
                    //    targets: 0,
                    //}
                    {
                        className: "text-center",
                        targets: [6],
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
                order: [[0, 'desc']]
            });

            $('#dataTable').on('click', '.setAssignment', await movePage);
            async function movePage() {
                var
                    tb = $('#dataTable').DataTable(),
                    dt = tb.row($(this).parents('tr')).data(),
                    urldetail = decodeURIComponent(baseUrl + "/page/fuk-assignment?Year=Params"),
                    url = decodeURIComponent(urldetail);
                url = url.replace("Params", window.btoa(dt.year));

                window.location.href = url;
            }
        }
    }

    await Load();
})