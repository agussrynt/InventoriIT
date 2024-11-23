$(document).ready(async function () {
    //Get IdHeader
    const urlParams = new URLSearchParams(window.location.search);
    const encodedParam = urlParams.get('idHeader');

    if (!encodedParam) {
        console.error("idHeader tidak ditemukan di URL");
        return;
    }

    const idHeader = window.atob(encodedParam); // Decode Base64
    console.log("Decoded idHeader:", idHeader);
    async function Load(idHeader) {
        var fd = new FormData();
        fd.append('idHeader', idHeader);
        await asyncAjax("/page/revenue/get-detail-revenue", "POST", fd)
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
            // Pisahkan data subtotal
            let dataTbody = [];
            let dataTfoot = [];

            listData.forEach(item => {
                if (item.project === null || item.project === "Subtotal") {
                    dataTfoot.push(item); // Tambahkan ke footer
                } else {
                    dataTbody.push(item); // Tambahkan ke body
                }
            });

            if ($.fn.DataTable.isDataTable('#fixedTable')) {
                $('#fixedTable').DataTable().destroy();
            }
            $('#fixedTable tbody').empty();
            $('#fixedTable').DataTable({
                //width: "100%",
                data: dataTbody,
                columns: [
                    { data: 'project', defaultContent: "" }, // Kolom project tidak diedit
                    {
                        data: 'januari',
                        defaultContent: "",
                        render: function (data, type, row) {
                            let formattedValue = formatRupiah(data || 0);
                            return '<input type="text" class="form-control table-input" value="' + formattedValue + '" data-column="januari">';
                        }
                    },
                    {
                        data: 'februari',
                        defaultContent: "",
                        render: function (data, type, row) {
                            let formattedValue = formatRupiah(data || 0);
                            return '<input type="text" class="form-control table-input" value="' + formattedValue + '" data-column="februari">';
                        }
                    },
                    {
                        data: 'maret',
                        defaultContent: "",
                        render: function (data, type, row) {
                            let formattedValue = formatRupiah(data || 0);
                            return '<input type="text" class="form-control table-input" value="' + formattedValue + '" data-column="maret">';
                        }
                    },
                    {
                        data: 'april',
                        defaultContent: "",
                        render: function (data, type, row) {
                            let formattedValue = formatRupiah(data || 0);
                            return '<input type="text" class="form-control table-input" value="' + formattedValue + '" data-column="april">';
                        }
                    },
                    {
                        data: 'mei',
                        defaultContent: "",
                        render: function (data, type, row) {
                            let formattedValue = formatRupiah(data || 0);
                            return '<input type="text" class="form-control table-input" value="' + formattedValue + '" data-column="mei">';
                        }
                    },
                    {
                        data: 'juni',
                        defaultContent: "",
                        render: function (data, type, row) {
                            let formattedValue = formatRupiah(data || 0);
                            return '<input type="text" class="form-control table-input" value="' + formattedValue + '" data-column="juni">';
                        }
                    },
                    {
                        data: 'juli',
                        defaultContent: "",
                        render: function (data, type, row) {
                            let formattedValue = formatRupiah(data || 0);
                            return '<input type="text" class="form-control table-input" value="' + formattedValue + '" data-column="juli">';
                        }
                    },
                    {
                        data: 'agustus',
                        defaultContent: "",
                        render: function (data, type, row) {
                            let formattedValue = formatRupiah(data || 0);
                            return '<input type="text" class="form-control table-input" value="' + formattedValue + '" data-column="agustus">';
                        }
                    },
                    {
                        data: 'september',
                        defaultContent: "",
                        render: function (data, type, row) {
                            let formattedValue = formatRupiah(data || 0);
                            return '<input type="text" class="form-control table-input" value="' + formattedValue + '" data-column="september">';
                        }
                    },
                    {
                        data: 'oktober',
                        defaultContent: "",
                        render: function (data, type, row) {
                            let formattedValue = formatRupiah(data || 0);
                            return '<input type="text" class="form-control table-input" value="' + formattedValue + '" data-column="oktober">';
                        }
                    },
                    {
                        data: 'november',
                        defaultContent: "",
                        render: function (data, type, row) {
                            let formattedValue = formatRupiah(data || 0);
                            return '<input type="text" class="form-control table-input" value="' + formattedValue + '" data-column="november">';
                        }
                    },
                    {
                        data: 'desember',
                        defaultContent: "",
                        render: function (data, type, row) {
                            let formattedValue = formatRupiah(data || 0);
                            return '<input type="text" class="form-control table-input" value="' + formattedValue + '" data-column="desember">';
                        }
                    },
                    {
                        data: 'total',
                        defaultContent: "",
                        render: function (data, type, row) {
                            let formattedValue = formatRupiah(data || 0);
                            return '<b>' + formattedValue + '</b>';
                        }
                    },

                    
                ],
                initComplete: function () {
                    // Cari data subtotal (contoh berdasarkan project === "Subtotal")
                    let subtotalData = listData.find(item => item.project === "Subtotal");


                    if (subtotalData) {
                        // Masukkan nilai subtotal ke dalam <tfoot> berdasarkan ID
                        $('#janAmount').text(formatRupiah(subtotalData.januari));
                        $('#febAmount').text(formatRupiah(subtotalData.februari));
                        $('#marAmount').text(formatRupiah(subtotalData.maret));
                        $('#aprAmount').text(formatRupiah(subtotalData.april));
                        $('#meiAmount').text(formatRupiah(subtotalData.mei));
                        $('#junAmount').text(formatRupiah(subtotalData.juni));
                        $('#julAmount').text(formatRupiah(subtotalData.juli));
                        $('#augAmount').text(formatRupiah(subtotalData.agustus));
                        $('#sepAmount').text(formatRupiah(subtotalData.september));
                        $('#oktAmount').text(formatRupiah(subtotalData.oktober));
                        $('#novAmount').text(formatRupiah(subtotalData.november));
                        $('#desAmount').text(formatRupiah(subtotalData.desember));
                        $('#totalAmount').text(formatRupiah(subtotalData.total));
                        // Lanjutkan untuk bulan lainnya sesuai kebutuhan
                    }
                },
                responsive: true,
                columnDefs: [
                    { targets: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12], width: "120px" }
                ],
                paging: false, 
                searching: false, 
                ordering: false, 
                info: false, 
                fixedColumns: {
                    start: 1,
                    end: 1
                },
                scrollCollapse: true,
                scrollX: true,
                scrollY: 300
                
            });
            

        }
    }
    $(document).on('input', '.table-input', function () {
        let rawValue = $(this).val().replace(/[^,\d]/g, ''); // Hanya ambil angka
        $(this).val(formatRupiah(rawValue)); // Format ulang ke rupiah
    });
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
    await Load(idHeader);
})