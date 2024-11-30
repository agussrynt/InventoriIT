$(async function () {
    var
        get_url = decodeURI(window.location.href),
        urlNew = new URL(get_url),
        paramUrl = urlNew.search.substring(urlNew.search.indexOf("?") + 1),
        splitParam = paramUrl.split("="),
        Param = window.atob(splitParam[1]);

    async function Load() {
        var fd = new FormData();
        fd.append('idProject', Param);

        await asyncAjax("/master/project-revenue/get-project-detail", "POST", fd)
            .then(function successCallBack(response) {
                console.log(response);
                if (response.success) {
                    bindingProjectDetail(response.data);
                }
            })
            .catch(function errorCallBack(err) {
                console.log(err);
                swallAllert.Error('Something wrong!', 'Please check backed!');
            });

        function DrawScoreTable(data) {
            var tbl = $('#dataTableScore').DataTable();
            tbl.destroy();

            $('#dataTableScore').DataTable({
                width: "100%",
                data: data,
                columns: [
                    { data: 'indikator', defaultcontent: "" },
                    {
                        data: 'bobot',
                        defaultContent: "-"
                    },
                    {
                        data: 'jumlaH_PARAMATER',
                        defaultContent: "-"
                    },
                    {
                        data: 'score',
                        defaultContent: "-"
                    },
                    {
                        data: 'capaian',
                        defaultContent: "-"
                    }
                ],
                columnDefs: [
                    {
                        sortable: false,
                        orderable: false,
                        searchable: false,
                        targets: 0,
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
        }

        function DrawRecoTable(data) {
            var tbl = $('#dataTableRecomendation').DataTable();
            tbl.destroy();

            $('#dataTableRecomendation').DataTable({
                width: "100%",
                data: data,
                columns: [
                    { data: 'rekomendasi', defaultcontent: "" },
                    {
                        data: '',
                        defaultContent: "-",
                        orderable: false,
                        render: function (data, type, row, meta) {
                            if (row.aspek === 'Pemegang Saham')
                                return `<span class="bx bx-user-check"></span>`;
                        }
                    },
                    {
                        data: '',
                        defaultContent: "-",
                        orderable: false,
                        render: function (data, type, row, meta) {
                            if (row.aspek === 'Dewan Komisaris')
                                return `<span class="bx bx-user-check"></span>`;
                        }
                    },
                    {
                        data: '',
                        defaultContent: "-",
                        orderable: false,
                        render: function (data, type, row, meta) {
                            if (row.aspek === 'Direksi')
                                return `<span class="bx bx-user-check"></span>`;
                        }
                    }
                ],
                columnDefs: [
                    {
                        sortable: false,
                        orderable: false,
                        searchable: false,
                        targets: 0,
                    },
                    {
                        className: "text-center",
                        targets: [1, 2, 3],
                    },
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
            });
        }
    }
    await Load();
})

function bindingProjectDetail(dt) {
    console.log(dt[0])
    $('#NamaProject').val(dt[0].namaProject);
    $('#Segmen').val(dt[0].segmen);
    $('#Asset').val(dt[0].asset);
    $('#CostCenter').val(dt[0].costCenter);
    $('#Customer').val(dt[0].customer);
    $('#Regional').val(dt[0].regional);
    $('#Contract').val(dt[0].contract);
    $('#Probability').val(dt[0].probability);
    $('#Sumur').val(dt[0].sumur);
    $('#ControlProject').val(dt[0].controlProject);
    $('#Pekerjaan').val(dt[0].pekerjaan);
    $('#SBT').val(dt[0].sbtIndex);
}

function clickDtScore() {
    $('#stp_reco').removeClass('active');
    $('#stp_score').addClass('active');
    $('#parameter-details').addClass('active');
    $('#fuk-detail').removeClass('active');
}

function clickRecomendation() {
    $('#stp_reco').addClass('active');
    $('#stp_score').removeClass('active');
    $('#parameter-details').removeClass('active');
    $('#fuk-detail').addClass('active');
}