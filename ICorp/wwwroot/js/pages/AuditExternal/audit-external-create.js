var arrScore = [];
var arrReco = [];
var form = $('form[name="frmAuditExternal"]');

function uploadDocScore() {
    var formData = new FormData();
    var fileInput = document.getElementById('file_ATTACHMENT_DATA_SCORE');
    formData.append(fileInput.files[0].name, fileInput.files[0]);

    $.ajax({
        url: baseUrl + '/page/auditexternal/upload-score',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.success) {
                arrScore = result.data;
                DrawScoreTable();
            } else {
                swallAllert.Error(result.data);
            }
        },
        error: function (err) {
            alert(err);
            swal.close();
        }
    });
}

function uploadMainFile() {
    var formData = new FormData();
    var fileInput = document.getElementById('ATTACHMENT_File');
    formData.append(fileInput.files[0].name, fileInput.files[0]);

    $.ajax({
        url: baseUrl + '/page/auditexternal/upload-file',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.success) {
                $('#ATTACHMENT_NAME').val(result.message);
                $('#ATTACHMENT').val(result.urlResponse);
            }
            else {
                alert(result.Message);
            }
        },
        error: function (err) {
            alert(err);
            swal.close();
        }
    });
}

function DrawScoreTable() {
    var tbl = $('#dataTableScore').DataTable();
    tbl.destroy();

    $('#dataTableScore').DataTable({
        width: "100%",
        data: arrScore,
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
            },
            //{
            //    data: '',
            //    defaultContent: "-",
            //    orderable: false,
            //    render: function (data, type, row, meta) {
            //        return `<div class="btn btn-danger remove-item btn-sm"><i class='bx bx-trash me-1'></i></div>`;
            //    }
            //},

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

    $('#dataTableScore').on('click', '.remove-item', removeItem);
    function removeItem() {
        swallAllert.Confirm.Delete('Are you sure?', "You won't be able to remove this!").then((result) => {
            if (result.isConfirmed) {
                var tb = $('#dataTableScore').DataTable(),
                    dt = tb.row($(this).parents('tr')).data();

                var objtemp = arrScore.find(s => s.iD_AUDIT_EXTERNAL_DATA_SCORE == dt.iD_AUDIT_EXTERNAL_DATA_SCORE);
                if (!jQuery.isEmptyObject(objtemp)) {
                    var idx = arrScore.indexOf(objtemp);
                    arrScore.splice(idx, 1);
                }
                if (arrScore.length === 0) {
                    document.getElementById('file_ATTACHMENT_DATA_SCORE').value = null;
                }
                DrawScoreTable();
            }
        })
    }
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



function uploadDocRecomendation() {
    var formData = new FormData();
    var fileInput = document.getElementById('file_ATTACHMENT_DATA_RECOMENDATION');
    formData.append(fileInput.files[0].name, fileInput.files[0]);

    $.ajax({
        url: baseUrl + '/page/auditexternal/upload-recomendation',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.success) {
                arrReco = result.data;
                DrawRecoTable();
            }
            else {
                swallAllert.Error(result.data);
            }
        },
        error: function (err) {
            alert(err);
            swal.close();
        }
    });
}

function DrawRecoTable() {
    var tbl = $('#dataTableRecomendation').DataTable();
    tbl.destroy();

    $('#dataTableRecomendation').DataTable({
        width: "100%",
        data: arrReco,
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
            },
            //{
            //    data: '',
            //    defaultContent: "-",
            //    orderable: false,
            //    render: function (data, type, row, meta) {
            //        return `<div class="btn btn-danger remove-item btn-sm"><i class='bx bx-trash me-1'></i></div>`;
            //    }
            //},

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
        //fixedColumns: {
        //    left: 1,
        //},
        //order: [[2, 'asc']]
    });

    $('#dataTableRecomendation').on('click', '.remove-item', removeItem);
    function removeItem() {
        swallAllert.Confirm.Delete('Are you sure?', "You won't be able to remove this!").then((result) => {
            if (result.isConfirmed) {
                var tb = $('#dataTableRecomendation').DataTable(),
                    dt = tb.row($(this).parents('tr')).data();

                var objtemp = arrReco.find(s => s.iD_AUDIT_EXTERNAL_DATA_SCORE == dt.iD_AUDIT_EXTERNAL_DATA_SCORE);
                if (!jQuery.isEmptyObject(objtemp)) {
                    var idx = arrReco.indexOf(objtemp);
                    arrReco.splice(idx, 1);
                }

                if (arrReco.length === 0) {
                    document.getElementById('file_ATTACHMENT_DATA_RECOMENDATION').value = null;
                }

                DrawScoreTable();
            }
        })
    }
}

function resetMainFile() {
    const file = document.querySelector('#ATTACHMENT');
    const file2 = document.querySelector('#ATTACHMENT_File');
    file.value = '';
    file2.value = '';
}

function resetScoreFile() {
    swallAllert.Confirm.Delete('Are you sure?', "You won't be able to remove this!").then((result) => {
        if (result.isConfirmed) {
            arrScore = [];
            DrawScoreTable();
            const file = document.querySelector('#file_ATTACHMENT_DATA_SCORE');
            file.value = '';
        }
    })
}

function resetRecoFile() {
    swallAllert.Confirm.Delete('Are you sure?', "You won't be able to remove this!").then((result) => {
        if (result.isConfirmed) {
            arrReco = [];
            DrawRecoTable();
            const file = document.querySelector('#file_ATTACHMENT_DATA_RECOMENDATION');
            file.value = '';
        }
    })
}

function submit() {
    var obj = {}, _objScore = {}, _objReco = {},
        _arrScore = [],
        _arrReco = [];

    if (form.valid()) {
        var _auditorName = $('#AUDITOR_NAME').val(),
            _date = $('#DATE').val(),
            _attachment = $('#ATTACHMENT').val(),
            _attachmentName = $('#ATTACHMENT_NAME').val();

        for (var i = 0; i < arrScore.length; i++) {
            var o = arrScore[i];
            _objScore = {
                BOBOT: o.bobot,
                CAPAIAN: o.capaian,
                INDIKATOR: o.indikator,
                JUMLAH_PARAMATER: o.jumlaH_PARAMATER,
                SCORE: o.score
            }
            _arrScore.push(_objScore);
        }

        for (var i = 0; i < arrReco.length; i++) {
            var o = arrReco[i];
            _objReco = {
                ASPEK: o.aspek,
                REKOMENDASI: o.rekomendasi
            }
            _arrReco.push(_objReco);
        }

        obj = {
            AUDITOR_NAME: _auditorName,
            DATE: _date,
            ATTACHMENT: _attachment,
            ATTACHMENT_NAME: _attachmentName,
            dataScores: _arrScore,
            dataRecomendations: _arrReco
        }
        console.log(obj);

        $.ajax({
            type: 'POST',
            url: baseUrl + '/page/auditexternal/post-create',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8', // when we use .serialize() this generates the data in query string format. this needs the default contentType (default content type is: contentType: 'application/x-www-form-urlencoded; charset=UTF-8') so it is optional, you can remove it
            data: obj,
            success: function (result) {
                if (result.success) {
                    swallAllert.Success("Success", "Succesfully submit");
                    window.location = baseUrl + '/page/auditexternal' ;
                }
                else {
                    swallAllert.Error("Oops", result.message);
                }
            },
            error: function () {
                alert('Failed to receive the Data');
                loadingForm(false, 'submitCreate', 'Submit!');
                console.log('Failed ');
            }
        })
    }
}