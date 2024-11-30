$(document).ready(function () {

    $(document).ready(function () {
        $.ajax({
            url: '/page/inputkonsolidasi/get_list_inputkonsolidasi',
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
                        name: `<span style="color: black;">Penjualan dan pendapatan usaha lainnya</span>`,
                        values: {},
                        isTitle: true
                    },
                    {
                        name: "Penjualan dalam Negri",
                        values: {},
                        isTitle: false
                    },
                    {
                        name: "Penggantian biaya subsidi jenis BBM tertentu & LPG dari Pemerintah",
                        values: {},
                        isTitle: false
                    },
                    {
                        name: "Penjualan ekspor",
                        values: {},
                        isTitle: false
                    },
                    {
                        name: "Imbalan jasa Pemasaran",
                        values:{},
                        isTitle: false
                    },
                    {
                        name: "Pendapatan usaha aktifitas operasi lainnya",
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
                    {
                        name: "<strong>Jumlah penjualan dan pendapatan usaha lainnya</strong>",
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
                        name: `<span style="color: black;">Beban Pokok Penjualan dan Beban Langsung Lainnya</span>`,
                        values: {},
                        isTitle: true
                    },
                    {
                        name: "Harga Pokok Penjualan",
                        values: {},
                        isTitle: false
                    },
                    {
                        name: "Beban produksi hulu dan liftings",
                        values: {},
                        isTitle: false
                    },
                    {
                        name: "Beban eksplorasi",
                        values: {},
                        isTitle: false
                    },
                    {
                        name: "Imbalan jasa pemasaran",
                        values: {},
                        isTitle: false
                    },
                    {
                        name: "Beban usaha dari aktifitas operasi lainnya",
                        values: get_beban.reduce((acc, curr) => {

                            const tahun = curr.tahun;

                            const pdsiBeban = get_totalBebanPDSI.find(item => item.tahun === tahun)?.pdsI_BebanUsaha || 0;

                            const pdcBeban = curr.pdC_BebanUsaha || 0;
                            const pdcElimBeban = curr.pdC_Eliminasi || 0;
                            const maBeban = curr.mA_BebanUsaha || 0;
                            const maElimBeban = curr.mA_Eliminasi || 0;

                            const total = pdsiBeban + pdcBeban + pdcElimBeban + maBeban + maElimBeban;

                            acc[tahun] = total > 0 ? new Intl.NumberFormat('id-ID').format(total) : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "<strong>Jumlah beban pokok penjualan dan beban langsung lainnya</strong>",
                        values: get_beban.reduce((acc, curr) => {
                            const tahun = curr.tahun;

                            // Ambil nilai dari PDSI
                            const pdsiBeban = get_totalBebanPDSI.find(item => item.tahun === tahun)?.pdsI_BebanUsaha || 0;

                            // Ambil nilai dari PDC
                            const pdcBeban = curr.pdC_BebanUsaha || 0;
                            const pdcElimBeban = curr.pdC_Eliminasi || 0;
                            const maBeban = curr.mA_BebanUsaha || 0;
                            const maElimBeban = curr.mA_Eliminasi || 0;

                            //console.log(pdcBeban);

                            // Hitung total untuk tahun ini
                            const total = pdsiBeban + pdcBeban + pdcElimBeban + maBeban + maElimBeban;

                            // Format hasil dan masukkan ke dalam objek akumulasi
                            acc[tahun] = total > 0 ? new Intl.NumberFormat('id-ID').format(total) : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },

                    //Laba Kotor
                    {
                        name: `<span style="color: black; margin-left: -20px;"><strong>Laba Kotor<strong></span>`,
                        values: get_beban.reduce((acc, curr) => {
                            const tahun = curr.tahun;

                            const pdsiBeban = get_totalBebanPDSI.find(item => item.tahun === tahun)?.pdsI_BebanUsaha || 0;

                            const pdcBeban = curr.pdC_BebanUsaha || 0;
                            const pdcElimBeban = curr.pdC_Eliminasi || 0;
                            const maBeban = curr.mA_BebanUsaha || 0;
                            const maElimBeban = curr.mA_Eliminasi || 0;

                            const pdsiPend = get_totalPendapatanPDSI.find(item => item.tahun === tahun)?.pdsI_PendapatanUsaha || 0;
                            const pdcPend = get_pendapatan.find(item => item.tahun === tahun)?.pdC_PendapatanUsaha || 0;
                            const pdcElim = get_pendapatan.find(item => item.tahun === tahun)?.pdC_Eliminasi || 0;
                            const maPend = get_pendapatan.find(item => item.tahun === tahun)?.mA_PendapatanUsaha || 0;
                            const maElim = get_pendapatan.find(item => item.tahun === tahun)?.mA_Eliminasi || 0;

                            const total = (pdsiPend + pdcPend + pdcElim + maPend + maElim) - (pdsiBeban + pdcBeban + pdcElimBeban + maBeban + maElimBeban);

                            //console.log(pdsiPend);
                            //console.log(pdcPend);
                            //console.log(pdcElim);
                            //console.log(maPend);
                            //console.log(maElim);
                            //console.log(pdsiPend + pdcPend + pdcElim + maPend + maElim);

                            acc[curr.tahun] = curr.mA_Eliminasi
                                ? new Intl.NumberFormat('id-ID').format(total)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },

                    //Beban Usaha
                    {
                        name: `<span style="color: black;">Beban Usaha</span>`,
                        values: {},
                        isTitle: true
                    },
                    {
                        name: "Beban penjualan dan pemasaran",
                        values: {},
                        isTitle: false
                    },
                    {
                        name: "Beban Umum dan Administrasi",
                        values: get_administrasi.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.total_BebanUmum
                                ? new Intl.NumberFormat('id-ID').format(curr.total_BebanUmum)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: "<strong>Jumlah Beban Usaha</strong>",
                        values: get_administrasi.reduce((acc, curr) => {
                            acc[curr.tahun] = curr.total_BebanUmum
                                ? new Intl.NumberFormat('id-ID').format(curr.total_BebanUmum)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                    {
                        name: `<span style="color: black; margin-left: -20px;"><strong>Laba (Rugi) Usaha<strong></span>`,
                        values: get_beban.reduce((acc, curr) => {
                            const tahun = curr.tahun;

                            const pdsiBeban = get_totalBebanPDSI.find(item => item.tahun === tahun)?.pdsI_BebanUsaha || 0;
                            const pdsiPend = get_totalPendapatanPDSI.find(item => item.tahun === tahun)?.pdsI_PendapatanUsaha || 0;

                            const pdcBeban = curr.pdC_BebanUsaha || 0;
                            const pdcElimBeban = curr.pdC_Eliminasi || 0;
                            const maBeban = curr.mA_BebanUsaha || 0;
                            const maElimBeban = curr.mA_Eliminasi || 0;

                            const pdcPend = get_pendapatan.find(item => item.tahun === tahun)?.pdC_PendapatanUsaha || 0;
                            const pdcElim = get_pendapatan.find(item => item.tahun === tahun)?.pdC_Eliminasi || 0;
                            const maPend = get_pendapatan.find(item => item.tahun === tahun)?.mA_PendapatanUsaha || 0;
                            const maElim = get_pendapatan.find(item => item.tahun === tahun)?.mA_Eliminasi || 0;

                            const labaKotor = (pdsiPend + pdcPend + pdcElim + maPend + maElim) - (pdsiBeban + pdcBeban + pdcElimBeban + maBeban + maElimBeban);

                            const totalBebanUmum = get_administrasi.find(item => item.tahun === tahun)?.total_BebanUmum || 0;

                            const labaBersih = labaKotor - totalBebanUmum;

                            //console.log(totalBebanUmum);

                            acc[curr.tahun] = curr.mA_Eliminasi
                                ? new Intl.NumberFormat('id-ID').format(labaBersih)
                                : '-';
                            return acc;
                        }, {}),
                        isTitle: false
                    },
                ];

                // Tambahkan data ke <tbody>
                let tbody = '';
                tableData.forEach((item) => {
                    const row = document.createElement("tr");
                    const cellName = document.createElement("td");

                    // Jika item adalah judul, buat tebal dan rata kiri
                    if (item.isTitle) {
                        cellName.innerHTML = `<strong>${item.name}</strong>`;
                        cellName.colSpan = 1; // Atur colspan jika hanya ada satu kolom data
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

                            if (item.name === "<strong>Jumlah penjualan dan pendapatan usaha lainnya</strong>") {
                                cellValue.style.color = "black";
                            }

                            if (item.name === "<strong>Jumlah beban pokok penjualan dan beban langsung lainnya</strong>") {
                                cellValue.style.color = "black";
                            }

                            if (item.name === "<strong>Jumlah Beban Usaha</strong>") {
                                cellValue.style.color = "black";
                            }

                            if (item.name === `<span style="color: black; margin-left: -20px;"><strong>Laba Kotor<strong></span>`) {
                                cellValue.style.color = "black";
                                cellValue.style.fontWeight = "bold";
                            }

                            if (item.name === `<span style="color: black; margin-left: -20px;"><strong>Laba (Rugi) Usaha<strong></span>`) {
                                cellValue.style.color = "black";
                                cellValue.style.fontWeight = "bold";
                            }

                            if (parseInt(year) === currentYear) {
                                cellValue.style.backgroundColor = "#ebeceb"; // Warna abu-abu
                            }


                            cellValue.textContent = item.values[year];
                            cellValue.style.textAlign = "center"; 
                            cellValue.style.verticalAlign = "middle"; 
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
    });
});

function onlyUnique(value, index, array) {
    return array.indexOf(value) === index;
}