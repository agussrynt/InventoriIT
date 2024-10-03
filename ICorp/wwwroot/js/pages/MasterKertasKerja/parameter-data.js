$(function () {
    var
        parameterDetail = $('#parameter-details'),
        headerParameterForm = $('form[name="form-header-parameter"]'),
        contentParameterForm = $('form[name="form-content-parameter"]'),
        headerParameterEditForm = $('form[name="form-header-parameter-edit"]'),
        isNewDataHeader = true,
        itemHeaderId = 0,
        isNewDataContent = true,
        itemContentId;

    // Header Parameter Or Indikator Submit Modal
    headerParameterForm.validate({
        // Specify validation rules
        rules: {
            // on the right side
            Aspek: "required",
            Bobot: "required",
            Description: "required",
        },
        // Specify validation error messages
        messages: {
            Aspek: "Please enter aspek parameter",
            Bobot: "Please provide a total parameter",
            Description: "Please fill description parameter",
        },
        // Make sure the form is submitted to the destination defined
        // in the "action" attribute of the form when valid
        submitHandler: function (form) {
            var IsExistAspek = checkExistAspek($(form).find('input[name="Aspek"]').val());
            if (!IsExistAspek) {
                var
                    dataHeader = {
                        Id: storedHeaderParameter.length + 1,
                        Sequence: storedHeaderParameter.length + 1,
                        Aspek: $(form).find('input[name="Aspek"]').val(),
                    };

                // Set Data Header
                storedHeaderParameter.push(dataHeader);
                localStorage.setItem("header-parameters", JSON.stringify(storedHeaderParameter));

                // Set Data Content
                for (var i = 0; i < storedHeaderParameter.length; i++) {
                    const el = storedHeaderParameter[i];
                    if (el.Aspek !== $(form).find('input[name="Aspek"]').val()) {
                        continue;
                    }

                    const ct = {
                        Id: storedContentParameter.length + 1,
                        HeaderId: el.Id,
                        Aspek: el.Aspek,
                        Description: $(form).find('textarea[name="Description"').val(),
                        Bobot: $(form).find('input[name="Bobot"]').val(),
                    }
                    storedContentParameter.push(ct);
                    localStorage.setItem("content-parameters", JSON.stringify(storedContentParameter));
                }

                // reset input
                $(form).find("textarea[name='Description']").val("");
                $(form).find("input[type=text]").val("");
                onSetHeader();

                // close modal
                $('#addHeaderParamsModal').modal('hide');
                isNewDataHeader = true;
                itemHeaderId = 0;

                return false;
            }
            else {
                swallAllert.Error("Oops, something wrong!", 'Data Aspek already Exist');
            }
        }
    });

    // Header Parameter Or Indikator Submit Edit Modal
    headerParameterEditForm.validate({
        // Specify validation rules
        rules: {
            // on the right side
            aspek: "required",
        },
        // Specify validation error messages
        messages: {
            aspek: "Please enter aspek parameter",
        },
        // Make sure the form is submitted to the destination defined
        // in the "action" attribute of the form when valid
        submitHandler: function (form) {
            var
                dataHeader = {
                    Id: parseInt($(form).find('input[name="header-id"]').val()),
                    Sequence: parseInt($(form).find('input[name="header-seq"]').val()),
                    Aspek: $(form).find('input[name="aspek"]').val(),
                };

            // Set Data Header

            var objtemp = storedHeaderParameter.find(s => s.Id == dataHeader.Id);
            if (!jQuery.isEmptyObject(objtemp)) {
                var idx = storedHeaderParameter.indexOf(objtemp);
                storedHeaderParameter.splice(idx, 1);
            }

            storedHeaderParameter.push(dataHeader);
            localStorage.setItem("header-parameters", JSON.stringify(storedHeaderParameter));

            // Set Data Content
            for (var i = 0; i < storedContentParameter.length; i++) {
                const el = storedContentParameter[i];
                if (el.HeaderId === dataHeader.Id) {
                    el.Aspek = dataHeader.Aspek;
                }
            }

            localStorage.setItem("content-parameters", JSON.stringify(storedContentParameter));

            // reset input
            $(form).find("textarea[name='Description']").val("");
            $(form).find("input[type=text]").val("");
            onSetHeader();

            // close modal
            $('#editHeaderParamsModal').modal('hide');
            isNewDataHeader = true;
            itemHeaderId = 0;

            return false;
        }
    });

    function checkExistAspek(aspek) {
        var result = false;
        var objtemp = storedHeaderParameter.find(s => s.Aspek === aspek);

        if (jQuery.isEmptyObject(objtemp)) 
            result = false;
        else
            result = true;

        return result;
    }

    function onSetHeader() {
        //console.log(storedHeaderParameter, counter);
        var tableHeader = parameterDetail.find("table#table-header-parameter").DataTable();
        tableHeader.destroy();

        parameterDetail.find("table#table-header-parameter").DataTable({
            width: "100%",
            data: storedHeaderParameter,
            columns: [
                { data: "Id", defaultContent: "", visible: false },
                { data: "Sequence", defaultContent: "" },
                { data: "Aspek", defaultContent: "" },
                {
                    data: null,
                    defaultContent: "",
                    orderable: false,
                    render: function (data, type, row) {
                        var button = `
                            <a class="text-info">
                                <i class='bx bx-edit'></i>
                            </a>
                            <a class="text-danger">
                                <i class='bx bx-trash'></i>
                            </a>
                        `;

                        return button
                    },
                },
            ],
            scrollX: true,
            searching: false,
            orderable: false,
            info: false,
            bPaginate: false,
            bLengthChange: false,
            bFilter: false,
            bInfo: false,
            bAutoWidth: false
        });
        if (storedHeaderParameter.length > 0) {
            $('#contentParams').show();
            onSetContent(storedContentParameter);
        } else {
            $('#contentParams').hide();
        }

        $('table#table-header-parameter tbody').on('click', '.bx-trash', function () {
            swallAllert.Confirm.Delete('Are you sure?', "You won't be able to remove this!").then((result) => {
                if (result.isConfirmed) {
                    var
                        tb = $('table#table-header-parameter').DataTable(),
                        dt = tb.row($(this).parents('tr')).data(),
                        tr = tb.row($(this).parents('tr'));
                    var temp = storedContentParameter.find(d => d.HeaderId === parseInt(dt.Id));
                    if (!jQuery.isEmptyObject(temp)) {
                        swallAllert.Error("Oops, something wrong!", 'This Item Has Child value, please remove Parameter value');
                    }
                    else {
                        var selected_storedFUKDetail = storedHeaderParameter.find(d => d.Id === dt.Id);
                        var deleteIndexOf = storedHeaderParameter.indexOf(selected_storedFUKDetail);
                        storedHeaderParameter.splice(deleteIndexOf, 1);
                        localStorage.setItem("header-parameters", JSON.stringify(storedHeaderParameter));
                        tb.row(tr).remove().draw();


                    }
                }
            })
        });

        $('table#table-header-parameter tbody').on('click', '.bx-edit', function () {
            var tb = $('table#table-header-parameter').DataTable(),
                dt = tb.row($(this).parents('tr')).data();

            isNewDataHeader = false;
            itemHeaderId = dt.Id;

            headerParameterEditForm.find('input[name="header-id"]').val(itemHeaderId);
            headerParameterEditForm.find('input[name="aspek"]').val(dt.Aspek);
            headerParameterEditForm.find('input[name="header-seq"]').val(dt.Sequence);

            $('#editHeaderParamsModal').modal('show');
        });

        function storedContentParameterCounterByHeader(headerId) {
            var counter = 0;
            for (var i = 0; i < storedContentParameter.length; i++) {
                if (storedContentParameter[i].HeaderId == headerId)
                    counter++;
            }
            return counter;
        }

        function removeHeader(headerId) {
            var selected_storedFUKDetail = storedHeaderParameter.find(d => d.Id === headerId);
            var seq = selected_storedFUKDetail.Sequence;
            var deleteIndexOf = storedHeaderParameter.indexOf(selected_storedFUKDetail);
            storedHeaderParameter.splice(deleteIndexOf, 1);

            for (var i = 0; i < storedHeaderParameter.length; i++) {
                if (storedHeaderParameter[i].Sequence > seq)
                    storedHeaderParameter[i].Sequence = storedHeaderParameter[i].Sequence - 1;
            }

            localStorage.setItem("header-parameters", JSON.stringify(storedHeaderParameter));
            onSetHeader();
        }

        function onSetContent(data) {
            var
                tableContent = parameterDetail.find("table#table-content-parameter").DataTable(),
                groupColumn = 2,
                collapsedGroups = {};
            // Destroy
            tableContent.destroy();

            var table = parameterDetail.find("table#table-content-parameter").DataTable({
                width: "100%",
                data: data,
                columns: [
                    { data: "Id", defaultContent: "", orderable: false, visible: false },
                    { data: "HeaderId", defaultContent: "", orderable: false },
                    { data: "Aspek", defaultContent: "", orderable: false },
                    { data: "Description", defaultContent: "", orderable: false },
                    { data: "Bobot", defaultContent: "", orderable: false },
                    {
                        data: null,
                        defaultContent: "",
                        orderable: false,
                        className: 'text-right',
                        render: function (data, type, row) {
                            var button = `
                                <a  class="text-info">
                                    <i class='bx bx-edit'></i>
                                </a>
                                <a  class="text-danger">
                                    <i class='bx bx-trash'></i>
                                </a>
                            `;

                            return button
                        },
                    },
                ],
                columnDefs: [
                    { visible: false, targets: groupColumn },
                    { visible: false, targets: 1 },
                ],
                scrollX: true,
                searching: false,
                paging: false,
                info: false,
                bLengthChange: false,
                bOrder: false,
                bScrollCollapse: false,
                order: [[1, 'asc']],
                rowGroup: {
                    // Uses the 'row group' plugin
                    dataSrc: 'Aspek',
                    startRender: function (rows, group) {
                        var collapsed = !!collapsedGroups[group];

                        rows.nodes().each(function (r) {
                            r.style.display = collapsed ? 'none' : '';
                        });

                        var icon = "<i id='btnCollapseGr' data-name='" + group + "' class='bx bx-plus-circle cursor-pointer text-success me-1'></i>";
                        if (!collapsed)
                            icon = "<i id='btnCollapseGr' data-name='" + group + "' class='bx bx-minus-circle cursor-pointer text-danger me-1'></i>";

                        // Add category name to the <tr>. NOTE: Hardcoded colspan 
                        var dataCollapse = '<div class="d-flex justify-content-between"><div class="col-auto">' + icon + group + ' (' + rows.count() + ')</div><div class="col-auto"><button type="button" id="btnAddContent" data-name="' + group + '" class="btn btn-sm btn-primary"><i class="bx bx-plus"></i></button></div>'
                        var childData = '<td colspan="4">' + dataCollapse + '</td>';
                        return $('<tr/>')
                            .append(childData)
                            .toggleClass('collapsed', collapsed);
                    }
                },
            });

            $('table#table-content-parameter tbody').on('click', '.bx-trash', function () {
                swallAllert.Confirm.Delete('Are you sure?', "You won't be able to remove this!").then((result) => {
                    if (result.isConfirmed) {
                        var
                            tb = $('table#table-content-parameter').DataTable(),
                            dt = tb.row($(this).parents('tr')).data(),
                            tr = tb.row($(this).parents('tr'));

                        var temp = storedFUKDetail.find(d => d.ContentId === dt.Id);
                        if (temp) {
                            swallAllert.Error("Oops, something wrong!", 'This Item Has Child value, please remove Faktor Uji Kesesuaian value');
                        }
                        else {
                            var selected_storedFUKDetail = storedContentParameter.find(d => d.Id === dt.Id);
                            var deleteIndexOf = storedContentParameter.indexOf(selected_storedFUKDetail);
                            storedContentParameter.splice(deleteIndexOf, 1);
                            localStorage.setItem("content-parameters", JSON.stringify(storedContentParameter));
                            tb.row(tr).remove().draw();

                            var counter = storedContentParameterCounterByHeader(dt.Id);
                            if (counter === 0) {
                                removeHeader(dt.Id);
                            }
                        }
                    }
                })
            });

            $('table#table-content-parameter tbody').on('click', '#btnCollapseGr', function () {
                var name = $(this).data('name');
                collapsedGroups[name] = !collapsedGroups[name];
                table.draw(false);
            });

            $('table#table-content-parameter tbody').on('click', '#btnAddContent', function () {
                var
                    modalDialog = $('#addContentParamsModal'),
                    modalForm = modalDialog.find('form[name="form-content-parameter"]'),
                    headerName = $(this).data('name');

                isNewDataContent = true;
                itemContentId = 0;
                // input form input
                modalForm.find('input[name="Aspek"]').val(headerName).attr('disabled', true);

                modalDialog.modal('show');
            });

            $('table#table-content-parameter tbody').on('click', '.bx-edit', function () {
                var
                    tb = $('table#table-content-parameter').DataTable(),
                    dt = tb.row($(this).parents('tr')).data(),
                    tr = tb.row($(this).parents('tr'));

                isNewDataContent = false;
                itemContentId = dt.Id;

                var
                    modalDialog = $('#addContentParamsModal'),
                    modalForm = modalDialog.find('form[name="form-content-parameter"]'),
                    headerName = dt.Aspek;
                // input form input
                modalForm.find('input[name="Aspek"]').val(headerName).attr('disabled', true);
                modalForm.find('input[name="Bobot"]').val(dt.Bobot);
                modalForm.find('textarea[name="Description"]').val(dt.Description);

                modalDialog.modal('show');
            });
        }
    }

    // Content Parameter Submit Modal
    contentParameterForm.validate({
        // Specify validation rules
        rules: {
            // on the right side
            Aspek: "required",
            Bobot: "required",
            Description: "required",
        },
        // Specify validation error messages
        messages: {
            Aspek: "Please enter aspek parameter",
            Bobot: "Please provide a total parameter",
            Description: "Please fill description parameter",
        },
        // Make sure the form is submitted to the destination defined
        // in the "action" attribute of the form when valid
        submitHandler: function (form) {
            var modalDialog = $('#addContentParamsModal');
            if (isNewDataContent) {
                var
                    content = storedHeaderParameter.find(x => x.Aspek == $(form).find('input[name="Aspek"]').val()),
                    length = storedContentParameter.filter(x => x.Aspek == $(form).find('input[name="Aspek"]').val()).length;
                //length = storedHeaderParameter.filter(x => x.Aspek == $(form).find('input[name="Aspek"]').val()).length;
                const ct = {
                    Id: storedContentParameter.length + 1,
                    HeaderId: content.Id,
                    Aspek: content.Aspek,
                    Description: $(form).find('textarea[name="Description"').val(),
                    Bobot: $(form).find('input[name="Bobot"]').val(),
                }
                storedContentParameter.push(ct);
                localStorage.setItem("content-parameters", JSON.stringify(storedContentParameter));
            }
            else {

                var objtemp = storedContentParameter.find(x => x.Id == itemContentId);
                if (!jQuery.isEmptyObject(objtemp)) {
                    var ct = {
                        Id: itemContentId,
                        HeaderId: objtemp.HeaderId,
                        Aspek: objtemp.Aspek,
                        Description: $(form).find('textarea[name="Description"').val(),
                        Bobot: $(form).find('input[name="Bobot"]').val(),
                    }
                    var idx = storedContentParameter.indexOf(objtemp);
                    storedContentParameter.splice(idx, 1);
                    storedContentParameter.push(ct);
                    localStorage.setItem("content-parameters", JSON.stringify(storedContentParameter));
                }
            }


            // reset input
            $(form).find("textarea[name='Description']").val("");
            $(form).find("input[type=text]").val("");
            onSetHeader();

            // close modal
            modalDialog.modal('hide');

            return false;
        }
    });
    onSetHeader();
})
