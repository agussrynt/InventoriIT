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
                            title: "Tahun",

                        },
                        {
                            data: 'rjppNextSta',
                            defaultContent: "-",
                            title: "RJPPNextSta",
                            render: function (data, type, row) {
                                let formattedValue = formatRupiah(data || 0);
                                return '<div>Rp. ' + formattedValue + '</div>';
                            }
                        },
                        {
                            data: 'rkapYearSta',
                            defaultContent: "-",
                            title: "RKAPYearSta",
                            render: function (data, type, row) {
                                let formattedValue = formatRupiah(data || 0);
                                return '<div>Rp. ' + formattedValue + '</div>';
                            }
                        },
                        {
                            data: 'prognosa',
                            defaultContent: "-",
                            title: "Prognosa",
                            render: function (data, type, row) {
                                let formattedValue = formatRupiah(data || 0);
                                return '<div>Rp. ' + formattedValue + '</div>';
                            }
                        },
                        {
                            data: 'realisasiBackYear',
                            defaultContent: "-",
                            title: "Realisasi Back Year",
                            render: function (data, type, row) {
                                let formattedValue = formatRupiah(data || 0);
                                return '<div>Rp. ' + formattedValue + '</div>';
                            }
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
                            formData.append('IDHeader', data.id);

                            await asyncAjax("/page/revenue/delete-header-revenue", "POST", formData)
                                .then(async function successCallBack(response) {
                                    swallAllert.Success("Header Revenue Berhasil Dihapus");
                                })
                                .catch(async function errorCallBack(err) {
                                    console.log("err : ");
                                    console.log(err);
                                    swallAllert.Error("Header Revenue Gagal Dihapus!", err.data);
                                })
                        }
                    })
                }


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
                    const urldetail = `${baseUrl}/page/revenue/edit?idHeader=${encodedId}`;

                    console.log("Redirecting to:", urldetail); // Debugging URL
                    window.location.href = urldetail;
                });
            }

        } finally {
            document.getElementById("loadingContent").classList.add("d-none");
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
            const response = await fetch(baseUrl+"/page/revenue/create-header-ajax", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(headerData),
            });

            const result = await response.json();
            if (result.success) {
                swallAllert.Success("Data berhasil disimpan!"); // Reload tabel
                $('#addRevenue').modal('hide'); 
            } else {
                swallAllert.Error("Gagal menyimpan data: " + result.message);
            }
        } catch (error) {
            console.error("Error:", error);
            swallAllert.Error("Terjadi kesalahan saat menyimpan data.", error);
        }
    }

    // Event listener untuk submit modal form
    $('.addRevenue-modal').on('submit', submitRevenueData);
    function formatRupiah(value) {
        let numberString = value.toString().replace(/[^,\d]/g, ''), // Hilangkan karakter non-angka
            split = numberString.split(','),
            sisa = split[0].length % 3,
            rupiah = split[0].substr(0, sisa),
            ribuan = split[0].substr(sisa).match(/\d{3}/gi);

        if (ribuan) {
            let separator = sisa ? '.' : '';
            rupiah += separator + ribuan.join('.');
        }

        return split[1] !== undefined ? rupiah + ',' + split[1] : rupiah;
    }

    await Load();
})