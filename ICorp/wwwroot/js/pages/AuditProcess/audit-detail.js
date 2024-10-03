$(async function () {
    var
        btnId, btnText,
        countNoResponded,
        modalDiv = $('#uploadDokumen'),
        form = $('form[name="form-upload-dokumen"]'),
        formResponse = $('form[name="form-response-author"]'),
        get_url = decodeURI(window.location.href),
        urlNew = new URL(get_url),
        paramUrl = urlNew.search.substring(urlNew.search.indexOf("?") + 1),
        splitParam = paramUrl.split("="),
        Param = window.atob(splitParam[1]),
        formE = $('form[name="form-edit-dokumen"]'),
        fileBase64 = '',
        fileName = '';

    //SetFinishAudit
    $('#btn-finish').on('click', SetFinishAudit);

    $('#yearData').text(Param);

    $('#showFile').on("click", () => {
        return Base64ToFile(fileName, fileBase64)
    })

    async function Load() {
        var fd = new FormData();
        fd.append('year', Param)
        await asyncAjax("/page/audits/get-auction-detail-ajax", "POST", fd)
            .then(function successCallBack(response) {
                console.log(response);
                if (response.success) {
                    countNoResponded = response.data.filter(el => !el.status).length;
                    //if (checkNoResponded < 1) {

                    //}
                    OnSuccess(response.data);
                }
            })
            .catch(function errorCallBack(err) {
                console.log(err);
                swallAllert.Error('Something wrong!', 'Please check backed!');
            });

        function OnSuccess(data) {
            var collapsedGroups = [];
            var groupParent = [];

            var table = $('#dataTable').DataTable({
                width: "100%",
                data: data,
                columns: [
                    { data: 'indikator', title: 'indikator' },
                    { data: 'parameter', title: 'parameter' },
                    { data: 'faktorUjiKesesuaian', title: 'fuk' },
                    { data: 'sequence', visible: false, orderable: false },
                    { data: 'child', visible: false, orderable: false },
                    { data: 'id', visible: false, orderable: false },
                    { data: 'fukId', visible: false, orderable: false },
                    { data: 'parameterId', visible: false, orderable: false },
                    { data: 'note', visible: false, orderable: false },
                    { data: 'year', visible: false, orderable: false },
                    { data: 'child', visible: false, orderable: false },
                    {
                        data: 'unsurPemenuhan',
                        orderable: false,
                        title: 'Unsur Pemenuhan',
                        class: 'order_id'
                    },
                    {
                        data: 'review',
                        orderable: false, 
                        defaultContent: "-",
                        title: 'Review',
                        render: function (data, type, row) {
                            if (row['parent'] == 1) {
                                return '';
                            }

                            return data;
                        },
                        class: 'text-center',
                    },
                    {
                        data: 'score',
                        orderable: false,
                        defaultContent: "-",
                        title: 'Score',
                        render: function (data, type, row) {
                            if (row['parent'] == 1) {
                                return '';
                            }
                            if (!row['status'] || (data == 0 && row['status'] == '0')) {
                                return '-';
                            }

                            return data;
                        },
                        class: 'text-center'
                    },
                    {
                        data: 'upload',
                        orderable: false,
                        title: 'Document',
                        class: 'text-center',
                        render: function (data, type, row) {
                            //if (row['parent'] == 1) {
                            //    return '';
                            //}
                            if (!data) {
                                //return '<label class="badge bg-info">Not Uploaded</label>'
                                return '<span class="badge bg-danger">Not Uploaded</span>';
                            }

                            return data
                        },
                    },
                    {
                        data: 'status',
                        orderable: false,
                        //title: 'Unsur Pemenuhan / Review / Document / Action',
                        title: 'Status Audit',
                        render: function (data, type, row) {
                            //if (row['parent'] == 1) {
                            //    return '';
                            //}

                            if (row['upStatus'] == "[Assignment] Reject")
                                return '<label class="badge bg-label-danger">Rejected</label>'

                            if (!data) {
                                return '<label class="badge bg-label-info">No Response</label>'
                            } else if (data == '0') {
                                return '<label class="badge bg-label-danger">Upload</label>'
                            } else {
                                return '<label class="badge bg-label-success">Completed</label>'
                            }

                            //upStatus:"[Assignment] Reject
                            //return data
                        },
                        class: 'text-center',
                    },
                    {
                        data: null,
                        width: "15%",
                        orderable: false,
                        //visible: countNoResponded < 1 ? false : true,
                        render: function (data, type, row) {
                            if (row['parent'] == 1) {
                                return '';
                            }
                            var button = `
                                <div class="d-flex" style="flex-direction: column; float: right">
                                    <button class="btn btn-sm btn-info cursor-pointer reviewDocument mb-2">
                                        <i class='bx bx-edit me-1'></i>
                                        <span>Review</span>
                                    </button>
                                    <button class="btn btn-sm btn-danger cursor-pointer responAuthor">
                                        <i class='bx bx-block me-1'></i>
                                        <span>Reject</span>
                                    </button>
                                </div>
                            `;

                            if (data.upStatus == '[Assignment] Upload') {
                                return button;
                            }

                            if (data.audit > 1) {
                                return `<button class="btn btn-sm btn-success cursor-pointer viewDocument" style="float: right">
                                        <i class='bx bx-show me-1'></i>
                                        <span>View</span>
                                    </button>`;
                            }

                            //if (row['review']) {
                            //    return `<button class="btn btn-sm btn-warning cursor-pointer editDocument" style="float: right">
                            //            <i class='bx bx-edit me-1'></i>
                            //            <span>Edit</span>
                            //        </button>`;
                            //}

                            if (row['review']) {
                                return `<button class="btn btn-sm btn-warning cursor-pointer editDocument" style="float: right">
                                        <i class='bx bx-show me-1'></i>
                                        <span>Edit</span>
                                    </button>`;
                            }

                            if (data.upStatus === null || data.upStatus === '') {
                                return '';
                            } 

                            

                            return button
                        },
                    },
                ],
                order: [[0, 'asc'], [1, 'asc'], [2, 'asc']],
                columnDefs: [{
                    targets: [0, 1, 2],
                    visible: false
                }],
                rowGroup: {
                    dataSrc: ['indikator', 'parameter', 'faktorUjiKesesuaian'],
                    startRender: function (rows, group, level) {
                        groupParent[level] = group;
                        var groupAll = '';
                        for (var i = 0; i < level; i++) { groupAll += groupParent[i]; if (collapsedGroups[groupAll]) { return; } }
                        groupAll += group;
                        if ((typeof (collapsedGroups[groupAll]) == 'undefined') || (collapsedGroups[groupAll] === null)) { collapsedGroups[groupAll] = false; } //True = Start collapsed. False = Start expanded.

                        var collapsed = collapsedGroups[groupAll];

                        rows.nodes().each(function (r) {
                            r.style.display = collapsed ? 'none' : '';
                        });

                        var icon = '<i class="bx bx-plus-circle text-success me-1"></i>';
                        if (!collapsed)
                            icon = '<i class="bx bx-minus-circle text-danger me-1"></i>';

                        // Add category name to the <tr>. NOTE: Hardcoded colspan
                        return $('<tr/>')
                            .append('<td colspan="12">' + icon + group + ' (' + rows.count() + ')</td>')
                            //.attr('data-name', all)
                            .attr('data-name', groupAll)
                            .toggleClass('collapsed', collapsed);
                    }
                },
                scrollX: true,
                searching: true,
                orderable: false,
                info: true,
                paging: false,
                bScrollCollapse: false,
                scrollY: false,
                createdRow: function (row, data, dataIndex) {
                    //console.log(row)
                    //console.log(data)
                    // If name is "Ashton Cox"
                    if (data.parent === 1) {
                        // Add COLSPAN attribute
                        $('td:eq(11)', row).attr('colspan', 6);

                        // Hide required number of columns
                        // next to the cell with COLSPAN attribute
                        $('td:eq(12)', row).css('display', 'none');
                        $('td:eq(13)', row).css('display', 'none');
                        $('td:eq(14)', row).css('display', 'none');
                        $('td:eq(15)', row).css('display', 'none');
                        $('td:eq(16)', row).css('display', 'none');

                        // Update cell data
                        this.api().cell($('td:eq(1)', row)).data(data.unsurPemenuhan);
                    }
                }
            });

            $('#dataTable tbody').on('click', 'tr.dtrg-start', function () {
                var name = $(this).data('name');
                collapsedGroups[name] = !collapsedGroups[name];
                table.draw(false);
            });

            $('#dataTable tbody').on('click', '.reviewDocument', function () {
                $('#formRecommendation').hide();
                form.find('#showNote').hide();
                var
                    table = $('#dataTable').DataTable(),
                    datas = table.row($(this).parents('tr')).data();

                form.find('input#Id').val(datas.id);
                form.find('textarea#Parameter').val(datas.parameter);
                form.find('textarea#FUK').val(datas.faktorUjiKesesuaian);
                form.find('textarea#UP').val(datas.unsurPemenuhan);
                form.find('input#Year').val(datas.year);

                form.find("input#setRecommendation").prop('checked', false);
                form.find('textarea#Recommendation').val('')
                form.find('textarea#Recommendation').html('')
                form.find('textarea#Remarks').val('')
                form.find('textarea#Remarks').html()
                form.find('input#Score').val('0.0')


                if (datas.upload) {
                    $('#showFile').empty();
                    form.find('#showFile a').empty();
                    //var link = '<a class="text-success cursor-pointer"><i class="bx bxs-file me-1"></i>' + datas.upload + '</a>';
                    var link = '<a class="text-success cursor-pointer" href="' + baseUrl + datas.file + '" download><i class="bx bxs-file me-1"></i>' + datas.upload + '</a>';
                    //href="/Documents/ExternalAudit/Template/TemplateScoreGCG.xlsx" download
                    form.find('#showFile').append(link);
                    //form.find('#showFile').html(documentData); 
                }

                if (datas.note) {
                    form.find('textarea#Note').val(datas.note)
                    form.find('#showNote').show();
                }

                modalDiv.modal("show");
            });

            $('#dataTable tbody').on('click', '.editDocument', function () {
                $('form[name=form-edit-dokumen]').find('#showNote').hide();
                $('form[name=form-edit-dokumen]').find('input#Score').removeAttr('readonly');
                $('#btnEditDocument').show();
                var
                    table = $('#dataTable').DataTable(),
                    datas = table.row($(this).parents('tr')).data();
                console.log(datas);

                $('form[name=form-edit-dokumen]').find('input#Id').val(datas.id);
                $('form[name=form-edit-dokumen]').find('textarea#Parameter').val(datas.parameter);
                $('form[name=form-edit-dokumen]').find('textarea#FUK').val(datas.faktorUjiKesesuaian);
                $('form[name=form-edit-dokumen]').find('textarea#UP').val(datas.unsurPemenuhan);
                $('form[name=form-edit-dokumen]').find('input#Year').val(datas.year);
                $('form[name=form-edit-dokumen]').find('textarea#Remarks').val(datas.review);

                $('form[name=form-edit-dokumen]').find('input#Score').val(datas.score.toString())

                if (datas.upload) {
                    $('#showFile').empty();
                    form.find('#showFile a').empty();
                    //var link = '<a class="text-success cursor-pointer"><i class="bx bxs-file me-1"></i>' + datas.upload + '</a>';
                    var link = '<a class="text-success cursor-pointer" href="' + baseUrl + datas.file + '" download><i class="bx bxs-file me-1"></i>' + datas.upload + '</a>';
                    $('form[name=form-edit-dokumen]').find('#showFile').append(link);
                    //form.find('#showFile').html(documentData); 
                }

                if (datas.note) {
                    $('form[name=form-edit-dokumen]').find('textarea#Note').val(datas.note)
                    $('form[name=form-edit-dokumen]').find('#showNote').show();
                }

                $('#editDokumen').modal("show");
            });

            $('#dataTable tbody').on('click', '.viewDocument', function () {
                $('form[name=form-edit-dokumen]').find('#showNote').hide();
                $('form[name=form-edit-dokumen]').find('input#Score').attr('readonly', 'readonly');
                $('#btnEditDocument').hide();
                var
                    table = $('#dataTable').DataTable(),
                    datas = table.row($(this).parents('tr')).data();
                console.log(datas);

                $('form[name=form-edit-dokumen]').find('input#Id').val(datas.id);
                $('form[name=form-edit-dokumen]').find('textarea#Parameter').val(datas.parameter);
                $('form[name=form-edit-dokumen]').find('textarea#FUK').val(datas.faktorUjiKesesuaian);
                $('form[name=form-edit-dokumen]').find('textarea#UP').val(datas.unsurPemenuhan);
                $('form[name=form-edit-dokumen]').find('input#Year').val(datas.year);
                $('form[name=form-edit-dokumen]').find('textarea#Remarks').val(datas.review);

                $('form[name=form-edit-dokumen]').find('input#Score').val(datas.score.toString())

                if (datas.upload) {
                    $('#showFile a').empty();
                    form.find('#showFile a').empty();
                    //var link = '<a class="text-success cursor-pointer"><i class="bx bxs-file me-1"></i>' + datas.upload + '</a>';
                    var link = '<a class="text-success cursor-pointer" href="' + baseUrl + datas.file + '" download><i class="bx bxs-file me-1"></i>' + datas.upload + '</a>';
                    $('form[name=form-edit-dokumen]').find('#showFile').append(link);
                    //form.find('#showFile').html(documentData); 
                }

                if (datas.note) {
                    $('form[name=form-edit-dokumen]').find('textarea#Note').val(datas.note)
                    $('form[name=form-edit-dokumen]').find('#showNote').show();
                }

                $('#editDokumen').modal("show");
            });

            $('#dataTable tbody').on('click', '.responAuthor', function () {
                var
                    table = $('#dataTable').DataTable(),
                    datas = table.row($(this).parents('tr')).data();

                console.log(datas);
                formResponse.find('input#Id').val(datas.id);

                $('#statusAuditModal').modal("show");
            });
        }
    }

    $('input#setRecommendation').on("change", function (evt) {
        var self  = $(this);
        var check = self.is(':checked');
        if (check) {
            $('#formRecommendation').show();
        } else {
            $('#formRecommendation').hide();
        }
    });

    $('input#Score').on("input", function (evt) {
        var self = $(this);
        self.val(self.val().replace(/[^0-9\.]/g, ''));
        if ((evt.which != 46 || self.val().indexOf('.') != -1) && (evt.which < 48 || evt.which > 57)) {
            evt.preventDefault();
        }
    });

    $('button#btnUploadDocument').click(async function (e) {
        //Review
        e.preventDefault();
        var score = form.find('input#Score').val();
        //score = score.replace('.', ',');

        if (form.valid()) {
            btnid = 'btnUploadDocument';
            btntext = 'submit';
            loadingForm(true, btnid);
            isRecommendation = $('input#setRecommendation').is(':checked');
            //console.log($('input#setRecommendation').is(':checked'))
            //console.log(form.find('input#Id').val(), 1, form.find('textarea#Remarks').val(), score, form.find('textarea#Recommendation').val())
            await FormAction(form.find('input#Id').val(), 1, form.find('textarea#Remarks').val(), score, form.find('textarea#Recommendation').val(), isRecommendation)
        }
    });

    $('button#btnEditDocument').click(async function (e) {
        //Review
        e.preventDefault();
        var score = formE.find('input#Score').val();
        //score = score.replace('.', ',');

        if (formE.valid()) {
            btnid = 'btnEditDocument';
            btntext = 'submit';
            loadingForm(true, btnid);
            isRecommendation = $('input#setRecommendation').is(':checked');
            //console.log($('input#setRecommendation').is(':checked'))
            //console.log(form.find('input#Id').val(), 1, form.find('textarea#Remarks').val(), score, form.find('textarea#Recommendation').val())
            await FormAction(formE.find('input#Id').val(), 1, formE.find('textarea#Remarks').val(), score, formE.find('textarea#Recommendation').val(), isRecommendation)
        }
    });

    $('button#btnResponseAuthor').click(async function (e) {
        // Reject
        e.preventDefault();

        if (form.valid()) {
            btnId = 'btnResponseAuthor';
            btnText = 'Yes, Reject';
            loadingForm(true, btnId);
            await FormAction(formResponse.find('input#Id').val(), 0, formResponse.find('textarea#Remarks').val())
        }
    });

    async function FormAction(id, responseType, remarks, score, recommendation, isrecommendation) {
        var
            fd = new FormData();
        fd.append("UpId", id);
        fd.append("responseType", responseType);
        fd.append("Remarks", remarks);
        fd.append("Recommendation", recommendation);
        fd.append("IsRecommendation", !isrecommendation ? 0 : 1);
        fd.append("Score", score);

        await asyncAjax("/page/audits/response-author-ajax", "POST", fd)
            .then(async function successCallBack(response) {
                console.log(response);
                if (response.success) {
                    swallAllert.Success("Success!", response.message);
                    loadingForm(false, btnId, btnText);
                    $('#statusAuditModal').modal("hide");
                    setTimeout(reloadPage, 3500)
                    
                } else {
                    swallAllert.Error("Fetch Data Failed!", response.message);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
                loadingForm(false, btnId, btnText);
            });

        function reloadPage() {
            window.location.reload();
            clearTimeout(myTimeout);
        }
    }

    async function CheckReadyFinish() {
        var fd = new FormData();
        fd.append('year', Param)
        await asyncAjax("/page/audits/response-isreadycomplete-ajax", "POST", fd)
            .then(function successCallBack(response) {
                console.log(response);
                if (response.message === 'NO') {
                    $('#btn-finish').hide();
                }
                else {
                    $('#btn-finish').show();
                }
            })
            .catch(function errorCallBack(err) {
                console.log(err);
                swallAllert.Error('Something wrong!', 'Please check backed!');
            });
    }

    async function SetFinishAudit() {
        var fd = new FormData();
        fd.append('year', Param)
        await asyncAjax("/page/audits/response-setfinishaudit-ajax", "POST", fd)
            .then(function successCallBack(response) {
                console.log(response);
                if (response.message === 'OK') {
                    swallAllert.Confirm.Success("Success", "Finish Audit").then((result) => { window.location.href = baseUrl + '/page/audits/process'; });
                }
                else {
                    swallAllert.Error('Something wrong!', 'Failed Set Finish');
                }
            })
            .catch(function errorCallBack(err) {
                console.log(err);
                swallAllert.Error('Something wrong!', 'Please check backed!');
            });
    }

    await Load();
    await CheckReadyFinish();
});