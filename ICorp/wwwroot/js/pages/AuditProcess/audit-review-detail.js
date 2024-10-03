﻿$(async function () {
    var
        btnId, btnText,
        countNoResponded,
        dataFollowUp,
        modalDiv = $('#responseAuditee'),
        form = $('form[name="form-response-auditee"]'),
        get_url = decodeURI(window.location.href),
        urlNew = new URL(get_url),
        paramUrl = urlNew.search.substring(urlNew.search.indexOf("?") + 1),
        splitParam = paramUrl.split("="),
        Param = window.atob(splitParam[1]);
    console.log(Param)
    async function Load() {
        var fd = new FormData();
        fd.append('year', Param)
        await asyncAjax("/page/follow-up/get-follow-up-detail-ajax", "POST", fd)
            .then(function successCallBack(response) {
                console.log(response);
                if (response.success) {
                    dataFollowUp = response.data;
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
                        class: 'order_id',
                        width: "70%"
                    },
                    {
                        data: 'review',
                        orderable: false,
                        defaultContent: "-",
                        title: 'Response Auditor',
                        class: 'text-center',
                        width: "10%"
                    },
                    {
                        data: 'status',
                        orderable: false,
                        title: 'Status Audit',
                        render: function (data, type, row) {
                            if (!data) {
                                return '<label class="badge bg-label-info">No Response</label>'
                            } else if (data == '0') {
                                return '<label class="badge bg-label-danger">Rejected</label>'
                            } else {
                                return '<label class="badge bg-label-success">Completed</label>'
                            }
                        },
                        class: 'text-center',
                        width: "10%"
                    },
                    {
                        data: 'score',
                        orderable: false,
                        defaultContent: "0.0",
                        title: 'Score',
                        render: function (data, type, row) {
                            if (!row['status'] || (data == 0 && row['status'] == '0')) {
                                return '0.0';
                            }

                            return data;
                        },
                        class: 'text-center',
                        width: "10%"
                    },
                    {
                        data: 'followUp',
                        orderable: false,
                        title: 'Response Auditee',
                        class: 'text-center',
                        render: function (data, type, row) {
                            if (!data) {
                                return '<span class="badge bg-danger">No Response</span>';
                            }

                            return data
                        },
                    },
                    {
                        data: 'upload',
                        orderable: false,
                        title: 'File Follow Up',
                        render: function (data, type, row) {
                            if (!data) {
                                return '<span class="badge bg-danger">No File</span>';
                            }
                            var link = '<a href="" class="text-success cursor-pointer"><i class="bx bxs-file me-1"></i><span>' + data + '</span></a>';

                            return link
                        },
                    },
                    {
                        data: null,
                        //width: "15%",
                        title: 'Action',
                        orderable: false,
                        render: function (data, type, row) {
                            return `<button data-id="${row['id']}" class="btn btn-sm btn-info cursor-pointer responseAuditee">
                                        <i class='bx bx-edit me-1'></i>Update Score
                                    </button>`;
                        },
                    },
                ],
                order: [[0, 'asc'], [1, 'asc'], [2, 'asc']],
                columnDefs: [
                    {
                        targets: [0, 1, 2],
                        visible: false
                    },
                    {
                        targets: 11,
                        width: '50%'
                    },
                    {
                        targets: [12, 13, 14],
                        width: '10%'
                    }
                ],
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
                autoWidth: true,
                searching: true,
                orderable: false,
                info: true,
                paging: false,
                bScrollCollapse: false,
                scrollY: false
            });

            $('#dataTable tbody').on('click', 'tr.dtrg-start', function () {
                var name = $(this).data('name');
                collapsedGroups[name] = !collapsedGroups[name];
                table.draw(false);
            });

            $('#dataTable tbody').on('click', '.responseAuditee', function () {
                form.find('#showNote').hide();
                var
                    id = $(this).data('id'),
                    datas = dataFollowUp.find(el => el.id === id);
                //table = $('#dataTable').DataTable(),
                //datas = table.row($(this).parents('tr')).data();
                console.log(datas.followUp);
                form.find('input#Id').val(datas.id);
                form.find('textarea#Parameter').val(datas.parameter);
                form.find('textarea#FUK').val(datas.faktorUjiKesesuaian);
                form.find('textarea#UP').val(datas.unsurPemenuhan);
                form.find('input#Year').val(datas.year);

                form.find('textarea#Remarks').val('');
                form.find('textarea#Remarks').html();
                form.find('input#Score').val('0.0')

                if (datas.score) {
                    form.find('input#Score').val(datas.score.toString())
                }

                if (datas.upload) {
                    var link = '<a href="" class="text-success cursor-pointer"><i class="bx bxs-file me-1"></i>' + datas.upload + '</a>';
                    form.find('#showFile').append(link);
                }

                if (datas.review) {
                    form.find('textarea#Note').val(datas.review)
                    form.find('#showNote').show();
                }

                if (datas.followUp) {
                    form.find('textarea#Remarks').val(datas.followUp)
                    //form.find('#showNote').show();
                }

                modalDiv.modal("show");
            });
        }
    }

    form.find('input#formFile').change(function () {
        // Get uploaded file extension  
        var extension = $(this).val().split('.').pop().toLowerCase();
        // Create array with the files extensions that we wish to upload  
        var validFileExtensions = ['doc', 'docx', 'pdf'];
        //Check file extension in the array.if -1 that means the file extension is not in the list.  
        if ($.inArray(extension, validFileExtensions) == -1) {
            swallAllert.Error('Opps .. !', "Upload only 'doc', 'docx', 'pdf' file")
            // Clear fileuload control selected file  
            $(this).replaceWith($(this).val('').clone(true));
        } else {
            // Check and restrict the file size to 1.28MB.  
            if ($(this).get(0).files[0].size > (1310720)) {
                alert("Sorry!! Max allowed file size is 1.28 MB");
                // Clear fileuload control selected file  
                $(this).replaceWith($(this).val('').clone(true));
            }
        }
    })

    $('input#Score').on("input", function (evt) {
        var self = $(this);
        self.val(self.val().replace(/[^0-9\.]/g, ''));
        if ((evt.which != 46 || self.val().indexOf('.') != -1) && (evt.which < 48 || evt.which > 57)) {
            evt.preventDefault();
        }
    });

    form.submit(async function (e) {
        e.preventDefault();
        if (form.valid()) {
            loadingForm(true, 'btnUploadDocument');
            var
                fd = new FormData(),
                score = form.find('input#Score').val();
            score = score.replace('.', ',');
            fd.append("UpId", form.find('input#Id').val());
            fd.append("Score", score);

            await asyncAjax("/page/audits/post-update-score-ajax", "POST", fd)
                .then(async function successCallBack(response) {
                    console.log(response);
                    if (response.success) {
                        modalDiv.modal("hide");
                        swallAllert.Success("Success!", response.message);
                        loadingForm(false, 'btnUploadDocument');
                        setTimeout(reloadPage, 3500)
                    } else {
                        swallAllert.Error("Fetch Data Failed!", response.message);
                    }
                })
                .catch(async function errorCallBack(err) {
                    swallAllert.Error("Fetch Data Failed!", err.data);
                });

            function reloadPage() {
                window.location.reload();
                clearTimeout();
            }
        }
    });

    await Load();
});