$(async function () {
    async function Load() {
        await asyncAjax("/page/revenue/get-header-revenue", "POST")
            .then(async function successCallBack(response) {
                console.log(response);
                if (response.success) {
                    console.log(response.data);
                    OnSuccess(response.data);
                } else {
                    swallAllert.Error("Data ngga ada woy, ", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            });

        
            

        /** When success fetch data user and create dataTable */
        async function OnSuccess(listData) {
            console.log(listData)
            $('#dataTable').DataTable({
                width: "100%",
                data: listData,
                columns: [
                    { data: 'id', defaultcontent: "", visible: false },
                    {
                        data: 'tahun',
                        defaultcontent: "",
                        title: "Tahun"
                    },
                    {
                        data: 'rjppNextSta',
                        defaultContent: "-",
                        title: "RJPPNextSta"
                    },
                    {
                        data: 'rkapYearSta',
                        defaultContent: "-",
                        title: "RKAPYearSta"
                    },
                    {
                        data: 'prognosa',
                        defaultContent: "-",
                        title: "Prognosa"
                    },
                    {
                        data: 'realisasiBackYear',
                        defaultContent: "-",
                        title: "Realisasi Back Year"
                    },
                    {
                        data: 'totalProject',
                        defaultContent: "-",
                        title: "Total Project"
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
                    urldetail = decodeURIComponent(baseUrl + "/"),
                    url = decodeURIComponent(urldetail);
                url = url.replace("Params", window.btoa(dt.year));

                window.location.href = url;
            }
        }
        
    }

    await Load();
})