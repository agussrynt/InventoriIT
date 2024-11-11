$(document).ready(async function () {
    async function Load() {
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
                                                <i class="bx bx-pen"></i>
                                            </button> | <button type="button" class="btn text-danger col btn-link btn-icon deleteButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="Delete">
                                                <i class="bx bx-trash"></i>
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


            //Delete Row
            $('#dataTable').on('click', '.deleteButton', removeRow);
            async function removeRow() {
                var
                    table = $('#dataTable').DataTable(),
                    data = table.row($(this).parents('tr')).data();
                swallAllert.Confirm.Delete('Are you sure?', "Are You Sure Want to Delete This Data").then(async (result) => {
                    console.log(result)
                    if (result.isConfirmed) {
                        var formData = new FormData();
                        formData.append('idProject', data.id);

                        await asyncAjax("/page/revenue/delete-header-ajax", "POST", formData)
                            .then(async function successCallBack(response) {
                                Load();
                            })
                            .catch(async function errorCallBack(err) {
                                console.log("err : ");
                                console.log(err);
                                swallAllert.Error("Fetch Data Failed!", err.data);
                            })
                    }
                })
            }


            //Edit Row
            $('#dataTable').on('click', '.editButton', await movePage);
            async function movePage() {
                var
                    tb = $('#dataTable').DataTable(),
                    dt = tb.row($(this).parents('tr')).data(),
                    urldetail = decodeURIComponent(baseUrl + "/page/revenue/edit"),
                    url = decodeURIComponent(urldetail);
                url = url.replace("Params", window.btoa(dt.year));

                window.location.href = url;
            }
        }
        
    }

    async function submitRevenueData(e) {
        e.preventDefault();

        const headerData = {
            Tahun: $("select[name='tahunBerjalan']").val(),
            RJPPNextSta: $("input[name='rjppNextSta']").val(),
            RKAPYearSta: $("input[name='rkapYearSta']").val(),
            Prognosa: $("input[name='prognosa']").val(),
            RealisasiBackYear: $("input[name='realisasiBackYear']").val(),
        };
        console.log(headerData);

        try {
            const response = await fetch("/page/revenue/create-header-ajax", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(headerData),
            });

            const result = await response.json();
            if (result.success) {
                alert("Data berhasil disimpan!");
                $('#dataTable').DataTable().ajax.reload(); // Reload tabel
                $('#addRevenue').modal('hide'); // Tutup modal
            } else {
                alert("Gagal menyimpan data: " + result.message);
            }
        } catch (error) {
            console.error("Error:", error);
            alert("Terjadi kesalahan saat menyimpan data." , error);
        }
    }

    // Event listener untuk submit modal form
    $('.addRevenue-modal').on('submit', submitRevenueData);


    await Load();
})