$(document).ready(function () {
    async function LoadRevHPPGA() {
        /** Fetch data user*/
        await asyncAjax("/page/revhppga/get-all-revhppga", "POST")
            .then(async function successCallBack(response) {
                if (response.success) {
                    await OnSuccess(response.data);
                } else {
                    swallAllert.Error("Fetch Data Failed!", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Warning("Data Kosong!");
            })

        /** When success fetch data user and create dataTable */
        async function OnSuccess(listData) {
            // Ambil data dari response
            var tbl = $('#dataTableRevHPP').DataTable();
            tbl.destroy();
            
            // Generate dynamic columns based on revenue keys
            var revenueKeys = Object.keys(listData[0].revenues || {});
            var hppKeys = Object.keys(listData[0].hpPs || {});
            var dynamicColumns = [];

            
            // Add dynamic headers
            revenueKeys.forEach(key => {
                $('#headerRevHPPGA').append(`<th>${key.replace('_', ' ').toUpperCase()}</th>`);
                dynamicColumns.push({
                    data: `revenues.${key}`,
                    title: key.replace('_', ' ').toUpperCase(),
                    render: function (data, type, row) {
                        return formatNumberToThousands(data); // Memformat nilai sebagai Rupiah
                    }
                });
            });

            hppKeys.forEach(key => {
                $('#headerRevHPPGA').append(`<th>${key.replace('_', ' ').toUpperCase()}</th>`);
                dynamicColumns.push({
                    data: `hpPs.${key}`,
                    title: key.replace('_', ' ').toUpperCase(),
                    render: function (data, type, row) {
                        return formatNumberToThousands(data); // Memformat nilai sebagai Rupiah
                    }
                });
            });

            var t = $('#dataTableRevHPP').DataTable({
                width: "100%",
                data: listData,
                columns: [
                    {
                        data: null,
                        defaultContent: "-",
                        render: function (data, type, row, meta) {
                            return meta.row + meta.settings._iDisplayStart + 1;
                        }
                    },
                    { data: 'id', defaultcontent: "", visible: false },
                    { data: 'segmentRJPP', defaultcontent: "" },
                    { data: 'namaCostCenter', defaultContent: "" },
                    { data: 'hp', defaultContent: "" },
                    { data: 'uniqueCode', defaultContent: "" },
                    { data: 'kategoriRIG', defaultcontent: "" },
                    { data: 'pic', defaultContent: "" },
                    { data: 'costumer', defaultContent: "" },
                    { data: 'project', defaultContent: "" },
                    { data: 'hppSales', defaultcontent: "" },
                    { data: 'gaSales', defaultContent: "" },
                    ...dynamicColumns

                ],
                columnDefs: [
                    {
                        searchable: false,
                        targets: 0,
                    },
                    {
                        searchable: false,
                        orderable: false,
                        visible: false,
                        targets: 1,
                    },
                    {
                        className: "text-center",
                        targets: 3,
                    },
                    {
                        className: "text-center",
                        searchable: false,
                        orderable: false,
                        targets: 5,
                    }
                ],
                lengthMenu: [[5, 10, 50, 100, -1], [5, 10, 50, 100, "All"]],
                autoWidth: true,
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                bScrollCollapse: true
            });

            t.on('order.dt search.dt', function () {
                let i = 1;

                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();

            var IDRev = t
                .column(1) // Pilih kolom pertama (ID)
                .data()    // Ambil data dari kolom
                .toArray(); // Ubah menjadi array


            /** Open Modal View User */
            $('#btnEdit').on('click', moveRow);
            async function moveRow() {
                var
                    urldetail = decodeURIComponent(baseUrl + "/page/revhppga/importRevHPPGA?id=Params"),
                    url = decodeURIComponent(urldetail);
                    
                url = url.replace("Params", window.btoa(IDRev));

                window.location.href = url;
            }
        }
    }

    async function LoadSUM() {
        /** Fetch data user*/
        await asyncAjax("/page/revhppga/get-sum-revhppga", "POST")
            .then(async function successCallBack(response) {
                if (response.success) {
                    await OnSuccess(response.data);
                } else {
                    swallAllert.Error("Fetch Data Failed!", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Warning("Data Kosong!", response.data);
            })

        /** When success fetch data user and create dataTable */
        async function OnSuccess(listData) {
            // Ambil data dari response

            var tbl = $('#dataTableSUM').DataTable();
            tbl.destroy();

            var dynamicSUM = [];
            var rjppKeys = Object.keys(listData[0].rjpp || {});

            rjppKeys.forEach(key => {
                $('#headerSUM').append(`<th>${key.replace('_', ' ').toUpperCase()}</th>`);
                dynamicSUM.push({
                    data: `rjpp.${key}`,
                    title: key.replace('_', ' ').toUpperCase(),
                    render: function (data, type, row) {
                        return formatNumberToThousands(data); // Memformat nilai sebagai Rupiah
                    }
                });
            });

            // Inisialisasi DataTable
            var t = $('#dataTableSUM').DataTable({
                width: "100%",
                data: listData,
                columns: [
                    {
                        data: 'category',
                        defaultContent: "",
                        render: function (data, type, row) {
                            return '<b>' + data + '</b>';
                        }
                    },
                    ...dynamicSUM
                ],
                columnDefs: [
                    {
                        className: "text-center",
                        targets: '_all', // Semua kolom mendapat class center
                    }
                ],
                lengthMenu: [[5, 10, 50, 100, -1], [5, 10, 50, 100, "All"]],
                autoWidth: true,
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                bScrollCollapse: true
            });

            // Menambahkan logika untuk mereset nomor urut setelah filter dan sorting
        }
    }

    LoadSUM()
    LoadRevHPPGA()
});