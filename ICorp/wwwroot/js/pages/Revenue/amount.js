$(document).ready(async function () {
    async function Load() {
        document.getElementById("loadingContent").classList.remove("d-none");
        try {
            await asyncAjax("/page/revenue/get-header-revenue", "POST")
                .then(async function successCallBack(response) {
                    console.log(response);
                    if (response.success) {
                        console.log(response.data);
                        await OnSuccess(response.data);
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
                            title: "Aksi",
                            orderable: false,
                            render: function (data, type, row, meta) {
                                return `<button type="button" class="btn text-info col btn-link btn-icon editButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="Edit">
                                                <i class="bx bx-pen"></i> Add Ammount
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
                    fixedHeader: true,
                    order: [[1, 'desc']],
                });

                //Edit Row
                $('#dataTable').on('click', '.editButton', async function () {
                    const tb = $('#dataTable').DataTable();
                    const dt = tb.row($(this).parents('tr')).data();

                    // Debugging: Pastikan data baris ada
                    console.log("Data row:", dt);

                    if (!dt || !dt.id) {
                        swallAllert.Error("ID tidak ditemukan. Silakan coba lagi.");
                        return;
                    }

                    const encodedId = window.btoa(dt.id); // Encode ID menggunakan Base64
                    const urldetail = `${baseUrl}/page/revenue/amount-input?idHeader=${encodedId}`;

                    console.log("Redirecting to:", urldetail); // Debugging URL
                    window.location.href = urldetail;
                });
            }

        } finally {
            document.getElementById("loadingContent").classList.add("d-none");
        }

    }


    await Load();
})