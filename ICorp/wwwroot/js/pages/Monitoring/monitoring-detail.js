$(async function () {
    var
        btnId, btnText,
        dataMonitoring,
        modalDiv = $('#responseAdmin'),
        form = $('form[name="form-response-admin"]'),
        get_url = decodeURI(window.location.href),
        urlNew = new URL(get_url),
        paramUrl = urlNew.search.substring(urlNew.search.indexOf("?") + 1),
        splitParam = paramUrl.split("="),
        Param = window.atob(splitParam[1]);
    console.log(Param)
    $('#year-span').text(Param);
    async function Load() {
        var fd = new FormData();
        fd.append('year', Param)
        await asyncAjax("/page/monitoring/get-monitoring-detail-ajax", "POST", fd)
            .then(function successCallBack(response) {
                console.log(response);
                if (response.success) {
                    dataMonitoring = response.data;
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
                    },
                    {
                        data: 'review',
                        orderable: false,
                        defaultContent: "-",
                        title: 'Review Document',
                        class: 'text-center',
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
                        data: 'recommendation',
                        orderable: false,
                        title: 'Recommendation Auditor',
                        class: 'text-center',
                        render: function (data, type, row) {
                            if (!data) {
                                return '<span class="badge bg-danger">No Recommendation</span>';
                            }

                            return data
                        },
                    },
                    {
                        data: 'assignment',
                        orderable: false,
                        defaultContent: "-",
                        title: 'Assignment to',
                        class: 'text-center',
                    },
                    {
                        data: 'dueDate',
                        width: "15%",
                        orderable: false,
                        defaultContent: "-",
                        render: function (data, type, full, meta) {
                            //return `<div class="d-flex"><button data-id="${full.id}" class="btn btn-link btn-sm updateDueDate"><i class="bx bxs-pencil me-1"></i></button>${convertDate(data)}</div>`;
                            return convertDate(data);
                        },
                        class: 'text-center',
                        title: "Due Date"
                    },
                    {
                        data: 'followUp',
                        orderable: false,
                        title: 'Follow Up Auditee',
                        //class: 'text-center',
                        render: function (data, type, row) {
                            if (!data) {
                                return '<span class="badge bg-label-danger">No Response</span>';
                            }

                            return data
                        },
                    },
                    {
                        data: 'upload',
                        orderable: false,
                        title: 'File Upload',
                        render: function (data, type, full, meta) {
                            if (!data) {
                                return '<span class="badge bg-label-danger">No File</span>';
                            }
                            var link = '<a target="_blank" href="' + baseUrl + full.filePath +'" class="text-success cursor-pointer"><i class="bx bxs-file me-1"></i><span>' + data + '</span></a>';

                            return link
                        },
                    },
                    {
                        data: null,
                        title: 'Action',
                        orderable: false,
                        render: function (data, type, full, row) {
                            
                            if (full.upload && full.followUp) {
                                //return `
                                //    <button data-id="${row['id']}" class="btn btn-sm btn-success cursor-pointer approveBtn">
                                //        Approve
                                //    </button>
                                //    <button data-id="${row['id']}" class="btn btn-sm btn-danger cursor-pointer rejectBtn">
                                //        Reject
                                //    </button>`;
                                return '';
                            } else {
                                return `<button data-id="${row['id']}" class="btn btn-sm btn-info cursor-pointer updateDueDate">
                                    <i class="bx bxs-pencil me-1" style="font-size: 12px"></i>Set Due Date
                                </button>`;
                            }
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

            $('#dataTable tbody').on('click', '.updateDueDate', function () {
                form.find('#showNote').hide();
                var
                    table = $('#dataTable').DataTable(),
                    datas = table.row($(this).parents('tr')).data();
                //var
                //    id = $(this).data('id'),
                //    datas = dataMonitoring.find(el => el.id === id);

                console.log(DateFunction.dateStrYYYYMMDD(datas.dueDate));
                form.find('input#Id').val(datas.id);
                form.find('textarea#Parameter').val(datas.parameter);
                form.find('textarea#FUK').val(datas.faktorUjiKesesuaian);
                form.find('textarea#UP').val(datas.unsurPemenuhan);
                form.find('input#Year').val(datas.year);
                //form.find('input#DueDate').val(new Date(datas.dueDate) );
                form.find('input#DueDate').val(DateFunction.dateStrYYYYMMDD(datas.dueDate));
                form.find('textarea#Review').val(datas.review);
                form.find('input#Assignment').val(datas.assignment);
                form.find('input#Score').val(datas.score.toString());

                if (datas.upload) {
                    var link = '<a href="" class="text-success cursor-pointer"><i class="bx bxs-file me-1"></i>' + datas.upload + '</a>';
                    form.find('#showFile').append(link);
                } else {
                    var response = '<label class="badge bg-lb-danger p-2">Not Uploaded</label>'
                    form.find('#showFile').append(link);
                }

                    
                if (datas.recommendation) {
                    form.find('textarea#Recommendation').val(datas.recommendation);
                    form.find().show();
                }

                modalDiv.modal("show");
            });
        }

        //btnUpdateDueDate
        $('button#btnUpdateDueDate').click(async function (e) {
            e.preventDefault();
            if (form.valid()) {
                btnId = 'btnUpdateDueDate';
                loadingForm(true, btnId);
                var id = form.find('input#Id').val();//DueDate
                var newDate = form.find('input#DueDate').val();//DueDate
                var fd = new FormData();
                fd.append("idUP", id);
                fd.append("newDate", newDate);

                await asyncAjax("/page/monitoring/post-newduedate-ajax", "POST", fd)
                    .then(async function successCallBack(response) {
                        console.log(response);
                        if (response.success) {
                            swallAllert.Success("Success!", response.message);
                            loadingForm(false, btnId);
                            $('#responseAdmin').modal("hide");
                            setTimeout(reloadPage, 1500)

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
                }
            }
        })
    }

    await Load();
});