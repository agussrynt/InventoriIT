$(async function () {
    var
        modalDiv = $('#uploadDokumen'),
        form = $('form[name="form-upload-dokumen"]'),
        get_url = decodeURI(window.location.href),
        urlNew = new URL(get_url),
        paramUrl = urlNew.search.substring(urlNew.search.indexOf("?") + 1),
        splitParam = paramUrl.split("="),
        Param = window.atob(splitParam[1]);
    
    $('#year').text(Param);
    //console.log(Param);
    async function Load() {
       
        var fd = new FormData();
        fd.append('year', Param)
        await asyncAjax("/page/document-upload/get-dokumen-detail-ajax", "POST", fd)
            .then(function successCallBack(response) {
                console.log(response);
                if (response.success) {
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
                    { data: 'year', visible: false, orderable: false },
                    { data: 'child', visible: false, orderable: false },
                    {
                        data: 'sequence',
                        visible: false,
                        orderable: false,
                        render: function (data, type, row) {
                            if (row['child']) {
                                return data + '.' + row['child'];
                            }

                            return data + '.';
                        }
                    },
                    {
                        data: 'unsurPemenuhan',
                        orderable: false,
                        title: 'Unsur Pemenuhan',
                        class: 'order_id'
                    },
                    {
                        data: 'upload',
                        orderable: false,
                        title: 'File Upload',
                        render: function (data, type, row) {
                            if (row['parent'] == 1) {
                                return ''
                            }
                            if (!data) {
                                return '<label class="badge bg-danger">Not Uploaded</label>'
                            }

                            return data
                        },
                    },
                    {
                        data: 'remarks',
                        orderable: false,
                        title: 'Remarks',
                        render: function (data, type, row) {
                            if (row['parent'] == 1) {
                                return ''
                            }
                            if (!data) {
                                return '<label class="badge bg-danger">No Remarks</label>'
                            }

                            return data
                        },
                    },
                    {
                        data: null,
                        width: "12%",
                        orderable: false,
                        title: 'Action',
                        render: function (data, type, row) {
                            if (row['parent'] == 1) {
                                return ''
                            }

                            var button = `
                                <button class="btn btn-sm btn-info cursor-pointer uploadDokumen">
                                    <i class='bx bx-edit me-1'></i>
                                    Edit
                                </button>
                            `;

                            if (data.upStatus == '[Assignment] Reject') {
                                return button;
                            }

                            if (row['upload']) {
                                return '<button data-id="' + row['upload'] + '" class="btn btn-sm btn-success cursor-pointer uploadDokumen"><i class="bx bx-show me-1"></i>View</a>';
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
                searching: false,
                orderable: false,
                info: false,
                paging: false,
                bScrollCollapse: false,
                scrollY: false
            });

            $('#dataTable tbody').on('click', 'tr.dtrg-start', function () {
                var name = $(this).data('name');
                collapsedGroups[name] = !collapsedGroups[name];
                table.draw(false);
            });

            $('#dataTable tbody').on('click', '.uploadDokumen', function () {
                var
                    table = $('#dataTable').DataTable(),
                    datas = table.row($(this).parents('tr')).data();
                var documentData = $(this).data('id');
                console.log(documentData);
                form.find('input#Id').val(datas.id);
                alert(datas.filePath);
                //form.find('input#FukId').val(datas.fukId);
                form.find('textarea#Parameter').val(datas.parameter);
                form.find('textarea#FUK').val(datas.faktorUjiKesesuaian);
                form.find('textarea#UP').val(datas.unsurPemenuhan);
                form.find('input#Year').val(datas.year);
                form.find('textarea#DocumentReview').val(datas.documentReview);
                
                form.find('textarea#Remarks').val(datas.remarks).attr('readonly', false);
                form.find('button#btnUploadDocument').show();
                form.find('input#formFile').val(''); 
                form.find('#showFile').empty();
                form.find('#showFile').append('<label for="formFile" class="form-label">Import File</label><input class="form-control" type = "file" id = "formFile" name="formFile" required="">');
                
                
                if (documentData) {
                    form.find('#showFile').empty();
                    //var link = '<a href="" class="text-success cursor-pointer"><i class="bx bxs-file me-1"></i>' + documentData + '</a>';
                    //form.find('#showFile').append(link);
                    var link = '<a class="text-success cursor-pointer" href="' + baseUrl + datas.filePath + '" download><i class="bx bxs-file me-1"></i>' + datas.upload + '</a>';
                    form.find('#showFile').append(link);
                    //form.find('#showFile').html(documentData);
                    form.find('textarea#Remarks').val(datas.remarks).attr('readonly', true);
                    form.find('button#btnUploadDocument').hide();
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
            alert("Sorry!! Upload only 'doc', 'docx', 'pdf' file")
            // Clear fileuload control selected file  
            $(this).replaceWith($(this).val('').clone(true));
        } else {
            // Check and restrict the file size to 128 KB.  
            if ($(this).get(0).files[0].size > (131072)) {
                alert("Sorry!! Max allowed file size is 128 kb");
                // Clear fileuload control selected file  
                $(this).replaceWith($(this).val('').clone(true));
            }
        }  
    })

    form.submit(async function (e) {
        e.preventDefault();
        if (form.valid()) {
            loadingForm(true, 'btnUploadDocument');
            var
                fd = new FormData(),
                file = form.find('input#formFile').get(0).files[0];
            fd.append("Id", form.find('input#Id').val());
            fd.append("FileUpload", file);
            fd.append("Remarks", form.find('textarea#Remarks').val());

            await asyncAjax("/page/document-upload/upload-ajax", "POST", fd)
                .then(async function successCallBack(response) {
                    console.log(response);
                    if (response.success) {
                        swallAllert.Success("Success!", response.message);
                        loadingForm(false, 'btnUploadDocument');
                        $('#setAssignment').modal("hide");
                        window.location.reload();
                        //await Load();
                    } else {
                        swallAllert.Error("Fetch Data Failed!", response.message);
                    }
                })
                .catch(async function errorCallBack(err) {
                    swallAllert.Error("Fetch Data Failed!", err.data);
                });
        }
    });

    await Load();
});