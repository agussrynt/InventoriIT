$(document).ready(function () {
    $("#btnModalUploadFile").click(function () {
        $("#uploadFileModal").modal("show");
    });

        $.ajax({
            url: baseUrl + '/page/inputkonsolidasi/get_list_inputkonsolidasi',
            method: "POST",
            dataType: 'json',
            beforeSend: function () {
                $(".loader").show();
            },
            success: function (response) {
                $('th.sorting_disabled').removeClass('sorting_disabled');
                document.querySelector('.dataTables_length')?.remove();
                $(".loader").hide();

                $("#pendapatanTable tbody").empty();
                console.log(response.pendapatan);
                console.log(response.beban);
                console.log(response.administrasi);
                console.log(response.subBeban);
                console.log(response.subPendapatan);

                let get_pendapatan = response.pendapatan;
                let get_beban = response.beban;
                let get_administrasi = response.administrasi;

                let get_totalPendapatanPDSI = response.subPendapatan;
                let get_totalBebanPDSI = response.subBeban;

                let arrTahunP = get_pendapatan.map(item => item.tahun || console.error('Tahun tidak ditemukan pada pendapatan:', item));
                let arrTahunB = get_beban.map(item => item.tahun || console.error('Tahun tidak ditemukan pada beban:', item));
                let arrTahunA = get_administrasi.map(item => item.tahun || console.error('Tahun tidak ditemukan pada administrasi:', item));

                let uniqueTahunP = arrTahunP.filter(onlyUnique);
                let uniqueTahunB = arrTahunB.filter(onlyUnique);
                let uniqueTahunA = arrTahunA.filter(onlyUnique);

                let uniqueTahun = uniqueTahunP.concat(uniqueTahunB, uniqueTahunA).filter(onlyUnique);

                console.log('Tahun unik:', uniqueTahun);

                // Tambahkan kolom tahun ke <thead>
                let currentYear = new Date().getFullYear(); // Tahun sekarang (2024)

                // Tambahkan kolom tahun ke <thead>
                let theadRow = `<tr><th>Field</th>`;
                uniqueTahun.forEach(tahun => {
                    if (tahun == currentYear) {
                        theadRow += `<th style="text-align: center; vertical-align: middle; padding: 10px; background-color: #ebeceb">RKAP ${tahun}</th>`;
                    } else if (tahun > currentYear) {
                        theadRow += `<th style="text-align: center; vertical-align: middle; padding: 10px;">RJPP ${tahun}</th>`;
                    } else {
                        theadRow += `<th style="text-align: center; vertical-align: middle; padding: 10px;">${tahun}</th>`;
                    }
                });
                theadRow += `</tr>`;
                $('#dataTable thead').html(theadRow);

                // Buat data untuk isi tabel
                let tableData = [
                    //region Pendapatan
                    {
                        name: `<span style="color: black;">Pendapatan Usaha Aktifitas Operasi Lainnya</span>`,
                        values: {},
                        isTitle: true
                    },
                    {
                        name: "PDSI : Pendapatan Usaha Aktifitas Operasi Lainnya",
                        values: get_totalPendapatanPDSI.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.pdsI_PendapatanUsaha
                                ? new Intl.NumberFormat('id-ID').format(curr.pdsI_PendapatanUsaha)
                                : '-';
                            return acc;
                        }, {})
                    },
                    {
                        name: "PDC : Pendapatan Usaha Aktifitas Operasi Lainnya",
                        values: get_pendapatan.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.pdC_PendapatanUsaha
                                ? new Intl.NumberFormat('id-ID').format(curr.pdC_PendapatanUsaha)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "PDC : Eliminasi",
                        values: get_pendapatan.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.pdC_Eliminasi
                                ? new Intl.NumberFormat('id-ID').format(curr.pdC_Eliminasi)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "M&A : Jumlah Penjualan dan Pendapatan Usaha Lainnya",
                        values: get_pendapatan.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.mA_PendapatanUsaha
                                ? new Intl.NumberFormat('id-ID').format(curr.mA_PendapatanUsaha)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "M&A : Eliminasi",
                        values: get_pendapatan.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.mA_Eliminasi
                                ? new Intl.NumberFormat('id-ID').format(curr.mA_Eliminasi)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "<strong>Total</strong>",
                        values: get_pendapatan.reduce((acc, curr) => {
                            const tahun = curr.tahun;

                            // Ambil nilai dari PDSI
                            const pdsiPend = get_totalPendapatanPDSI.find(item => item.tahun === tahun)?.pdsI_PendapatanUsaha || 0;

                            // Ambil nilai dari PDC
                            const pdcPend = curr.pdC_PendapatanUsaha || 0;
                            const pdcElim = curr.pdC_Eliminasi || 0;
                            const maPend = curr.mA_PendapatanUsaha || 0;
                            const maElim = curr.mA_Eliminasi || 0;

                            // Hitung total untuk tahun ini
                            const total = pdsiPend + pdcPend + pdcElim + maPend + maElim;

                            // Format hasil dan masukkan ke dalam objek akumulasi
                            acc[tahun] = total > 0 ? new Intl.NumberFormat('id-ID').format(total) : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    //Region Beban
                    {
                        name: `<span style="color: black;">Beban Usaha dari Aktifitas Operasi Lainnya</span>`,
                        values: {},
                        isTitle: true
                    },
                    {
                        name: "PDSI : Beban Usaha dari Aktifitas Operasi Lainnya",
                        values: get_totalBebanPDSI.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.pdsI_BebanUsaha
                                ? new Intl.NumberFormat('id-ID').format(curr.pdsI_BebanUsaha)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "PDC : Beban Usaha dari Aktifitas Operasi Lainnya",
                        values: get_beban.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.pdC_BebanUsaha
                                ? new Intl.NumberFormat('id-ID').format(curr.pdC_BebanUsaha)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "PDC : Eliminasi",
                        values: get_beban.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.pdC_Eliminasi
                                ? new Intl.NumberFormat('id-ID').format(curr.pdC_Eliminasi)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "M&A : Beban Usaha dari Aktifitas Operasi Lainnya",
                        values: get_beban.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.mA_BebanUsaha
                                ? new Intl.NumberFormat('id-ID').format(curr.mA_BebanUsaha)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "M&A : Eliminasi",
                        values: get_beban.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.mA_Eliminasi
                                ? new Intl.NumberFormat('id-ID').format(curr.mA_Eliminasi)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "<strong>Total</strong>",
                        values: get_beban.reduce((acc, curr) => {
                            const tahun = curr.tahun;

                            // Ambil nilai dari PDSI
                            const pdsiBeban = get_totalBebanPDSI.find(item => item.tahun === tahun)?.pdsI_BebanUsaha || 0;

                            // Ambil nilai dari PDC
                            const pdcBeban = curr.pdC_BebanUsaha || 0;
                            const pdcElimBeban = curr.pdC_Eliminasi || 0;
                            const maBeban = curr.mA_BebanUsaha || 0;
                            const maElimBeban = curr.mA_Eliminasi || 0;

                            // Hitung total untuk tahun ini
                            const total = pdsiBeban + pdcBeban + pdcElimBeban + maBeban + maElimBeban;

                            // Format hasil dan masukkan ke dalam objek akumulasi
                            acc[tahun] = total > 0 ? new Intl.NumberFormat('id-ID').format(total) : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    //region Administrasi
                    {
                        name: `<span style="color: black;">Beban Umum dan Administrasi</span>`,
                        values: {},
                        isTitle: true
                    },
                    {
                        name: "PDSI : Beban Umum dan Administrasi",
                        values: get_administrasi.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.pdsI_BebanUmum
                                ? new Intl.NumberFormat('id-ID').format(curr.pdsI_BebanUmum)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "PDC : Beban Umum dan Administrasi",
                        values: get_administrasi.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.pdC_BebanUmum
                                ? new Intl.NumberFormat('id-ID').format(curr.pdC_BebanUmum)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "M&A : Beban Umum dan Administrasi",
                        values: get_administrasi.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.mA_BebanUmum
                                ? new Intl.NumberFormat('id-ID').format(curr.mA_BebanUmum)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "<strong>Total</strong>",
                        values: get_administrasi.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.total_BebanUmum
                                ? new Intl.NumberFormat('id-ID').format(curr.total_BebanUmum)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    }
                ];

                // Tambahkan data ke <tbody>
                let tbody = '';
                tableData.forEach((item) => {
                    const row = document.createElement("tr");
                    const cellName = document.createElement("td");

                    // Jika item adalah judul, buat tebal dan rata kiri
                    if (item.isTitle) {
                        cellName.innerHTML = `<strong>${item.name}</strong>`;
                        cellName.colSpan = 2; // Atur colspan jika hanya ada satu kolom data
                    } else {
                        // Jika bukan judul, tambahkan indentasi
                        cellName.innerHTML = `<span style="margin-left: 20px;">${item.name}</span>`;
                    }

                    row.appendChild(cellName);
                    document.querySelector("#dataTable").appendChild(row);

                    // Render kolom "values" jika bukan judul
                    if (!item.isTitle) {
                        Object.keys(item.values).forEach((year) => {
                            const cellValue = document.createElement("td");

                            if (item.name === "<strong>Total</strong>") {
                                cellValue.style.color = "black";
                            }

                            if (parseInt(year) === currentYear) {
                                cellValue.style.backgroundColor = "#ebeceb"; // Warna abu-abu
                            }

                            cellValue.textContent = item.values[year];
                            cellValue.style.textAlign = "center"; // Horizontal tengah
                            cellValue.style.verticalAlign = "middle"; // Vertikal tengah
                            cellValue.style.padding = "10px"; 
                            row.appendChild(cellValue);
                        });
                    }
                });

            },
            error: function () {
                alert("Gagal mengambil data dari server.");
            }
});

    document.getElementById("uploadFileForm").addEventListener("submit", async function (e) {
        e.preventDefault();

        var uploadFile = $("#uploadFile").prop("files")[0];

        if (uploadFile === undefined) {
            swal.fire(
                "Warning!",
                "Please input the Excel file to be uploaded!",
                "warning"
            );
            return false;
        }

        var fileSize = uploadFile.size;

        var ext = uploadFile.name
            .substring(uploadFile.name.indexOf(".") + 1)
            .toLowerCase();

        if (fileSize > 5000000) { // 5MB = 5 * 1024 * 1024 bytes
            swal.fire(
                "Warning!",
                "File size is too large! Maximum Limit 5MB.",
                "warning"
            );
            return false;
        }

        if (ext != "xlsx") {
            swal.fire("Warning!", "Wrong file format! Please upload files with the extension .xlsx", "warning");
            return false;
        }

        var loader = document.querySelector('.loader');
        loader.style.display = 'block'; // Menampilkan loader

        var formData = new FormData(this); // Ambil data form langsung
        var submitButton = this.querySelector("button[type='submit']"); // Ambil tombol submit
        submitButton.disabled = true; // Nonaktifkan tombol submit

        try {
            const response = await fetch("/page/inputkonsolidasi/upload-excel-ajax", {
                method: "POST",
                body: formData,
            });

            const result = await response.json();

            if (result.success) {
                swallAllert.Success("Success", result.message, "success");
                setTimeout(() => location.reload(), 2000); // Tunggu 2 detik sebelum reload
            } else {
                swallAllert.Error("Error!", result.message, "warning");
            }
        } catch (error) {
            swallAllert.Error("Error!", "Something went wrong!");
        } finally {
            loader.style.display = 'none'; // Menyembunyikan loader
            submitButton.disabled = false; // Aktifkan kembali tombol submit
        }
    });

});

function onlyUnique(value, index, array) {
    return array.indexOf(value) === index;
}
