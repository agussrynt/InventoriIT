$(async function () {
    $('.btn-next-fuk').on('click', loadFUK);
    var
        checboxData,
        parenting = false,
        fukDetail = $('#fuk-detail'),
        modalDialog = $('#addFUKModal'),
        fukParameterForm = modalDialog.find('form[name="form-fuk-detail"]'),
        parameterInp = fukParameterForm.find("select#ParemeterHeader"),
        datas = await GrouppingData(),
        divContent = modalDialog.find('#parameterContent'),
        itemId = 0,
        isNewData = true;

    // On Set Header
    function onSetHeader(mapping) {
        console.log('onSetHeader')
        console.log(mapping)
        var
            //groupColumn = 2,
            //mapping = new Array,
            groupParent = new Array,
            collapsedGroups = new Array;
        var table = fukDetail.find("table#table-fuk-detail").DataTable()
        table.destroy();
        // Table FUK Detail
        //console.log(mapping)
        var table = fukDetail.find("table#table-fuk-detail").DataTable({
            width: "100%",
            data: mapping,
            columns: [
                { data: "indikator", defaultContent: "", title: 'Indikator' },
                { data: "parameter", defaultContent: "", title: 'Parameter' },
                { data: "id", defaultContent: "", visible: false },
                {
                    data: "parent",
                    defaultContent: "",
                    visible: false
                },
                { data: "child", defaultContent: "-", visible: false },
                {
                    data: "sequence",
                    defaultContent: "",
                    orderable: false,
                    title: 'Seq',
                    width: "3%",
                    class: 'order_id',
                    render: function (data, type, row) {
                        if (row['child']) {
                            return data + '.' + row['child'];
                        }

                        return data;

                    },
                },
                {
                    data: "faktorUjiKesesuaian",
                    defaultContent: "",
                    width: "87%",
                    orderable: false,
                    title: 'Faktor Uji Kesesuaian',
                },
                {
                    data: null,
                    defaultContent: "",
                    orderable: false,
                    title: 'Action',
                    width: "10%",
                    className: "text-right",
                    render: function (data, type, row) {
                        var
                            btnEdit = '<a type="button" id="btnModalFUKedit"  class="text-info"><i class="bx bx-edit"></i></a>',
                            btnList = '<a type="button" class="text-success addListPointer" data-bs-toggle="tooltip" data-bs-placement="top" title="Add List Pointer"><i class="bx bx-list-plus"></i></a>',
                            btnDelt = '<a type="button" id="btnFUKdelete" class="text-danger"><i class="bx bx-trash"></i></a>';
                        if (row['parent'] === "On") {
                            return btnEdit + btnList + btnDelt;
                        }

                        return btnEdit + btnDelt;

                    },
                },
            ],
            order: [[0, 'asc'], [1, 'asc'], [5, 'asc']],
            columnDefs: [
                {
                    targets: [0, 1],
                    visible: false
                }
            ],
            rowGroup: {
                dataSrc: ['indikator', 'parameter'],
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

                    var icon = '<i class="bx bx-plus-circle text-success me-2"></i>';
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
            scrollY: '250px',
            searching: false,
            orderable: false,
            info: false,
        }); 

        $('table#table-fuk-detail tbody').on('click', 'tr.dtrg-start', function () {
            var name = $(this).data('name');
            collapsedGroups[name] = !collapsedGroups[name];
            table.draw(false);
        });

        $('table#table-fuk-detail tbody').on('click', '#btnModalFUKedit', function () {
            var
                tb = $('table#table-fuk-detail').DataTable(),
                dt = tb.row($(this).parents('tr')).data();
            isNewData = false;
            itemId = dt.id;
            fillModalFUK();
            appendData_edit(dt.id);
        });

        $('table#table-fuk-detail tbody').on('click', '.addListPointer', function () {
            var
                tb = $('table#table-fuk-detail').DataTable(),
                dt = tb.row($(this).parents('tr')).data();
            $('form[name="form-fuk-child-detail"]').find('#Description').val('');
            $('form[name="form-fuk-child-detail"]').find("input#Id").val(dt.id);
            $('#addFUKChildModal').modal("show");
        });

        $('table#table-fuk-detail tbody').on('click', '#btnFUKdelete', function () {
            swallAllert.Confirm.Delete('Are you sure?', "You won't be able to remove this!").then(async (result) => {
                if (result.isConfirmed) {
                    var
                        tb = $('table#table-fuk-detail').DataTable(),
                        dt = tb.row($(this).parents('tr')).data();
                    tr = tb.row($(this).parents('tr'));

                    var temp = storedUPLink.find(d => d.FUKId === dt.id);
                    if (temp) {
                        swallAllert.Error("Oops, something wrong!", 'This Item Has Child value, please remove Unsur Pemenuhan value');
                    }
                    else {
                        var selected_storedFUKDetail = storedFUKDetail.find(d => d.Id === dt.id);
                        if (selected_storedFUKDetail.Parent === 'Off' || (selected_storedFUKDetail.Parent !== 'Off' && selected_storedFUKDetail.Parent !== 'On')) {
                            var deleteIndexOf = storedFUKDetail.indexOf(selected_storedFUKDetail);
                            storedFUKDetail.splice(deleteIndexOf, 1);
                            await reorderNumberAfterRemove(selected_storedFUKDetail);
                            localStorage.setItem("fuk-details", JSON.stringify(storedFUKDetail));
                            tb.row(tr).remove().draw();
                            var datas = await GrouppingData();
                            await onSetHeader(datas);
                        }
                        else if (selected_storedFUKDetail.Parent === 'On') {
                            var hasChild = false;
                            var arrChild = storedFUKDetail.filter(s => s.Parent == selected_storedFUKDetail.Id);

                            for (var i = 0; i < arrChild.length; i++) {
                                var temp = storedUPLink.find(d => d.FUKId === arrChild[i].Id);
                                if (temp) {
                                    hasChild = true;
                                } 
                            }

                            if (hasChild) {
                                swallAllert.Error("Oops, something wrong!", 'This Item Has Child value, please remove Unsur Pemenuhan value');
                            }
                            else {
                                var deleteIndexOf = storedFUKDetail.indexOf(selected_storedFUKDetail);
                                storedFUKDetail.splice(deleteIndexOf, 1);

                                for (var i = 0; i < arrChild.length; i++) {
                                    var deleteIndexOf = storedFUKDetail.indexOf(arrChild[i]);
                                    storedFUKDetail.splice(deleteIndexOf, 1);
                                }

                                await reorderNumberAfterRemove(selected_storedFUKDetail);
                                localStorage.setItem("fuk-details", JSON.stringify(storedFUKDetail));
                                tb.row(tr).remove().draw();
                                var datas = await GrouppingData();
                                await onSetHeader(datas);
                            }
                        }
                    }
                }

            })
        });
    };

    async function reorderNumberAfterRemove(objRemove) {
        if (objRemove.Parent === 'Off' || objRemove.Parent === 'On') {
            for (var i = 0; i < storedFUKDetail.length; i++) {
                if (storedFUKDetail[i].ContentId == objRemove.ContentId) {
                    if (storedFUKDetail[i].Sequence > objRemove.Sequence) {
                        storedFUKDetail[i].Sequence = storedFUKDetail[i].Sequence - 1;
                    }
                }
            }
        }

        if (objRemove.Parent !== 'Off' && objRemove.Parent !== 'On') {
            for (var i = 0; i < storedFUKDetail.length; i++) {
                if (storedFUKDetail[i].Parent == objRemove.Parent) {
                    if (storedFUKDetail[i].Child !== null) {
                        var childRemoveIdx = abjad.indexOf(objRemove.Child);
                        var childCurrentIdx = abjad.indexOf(storedFUKDetail[i].Child);

                        if (childCurrentIdx > childRemoveIdx) {
                            var childNewNumber = abjad[childCurrentIdx - 1];
                            storedFUKDetail[i].Child = childNewNumber;
                        }
                    }
                }
            }
        }
    }

    $('form[name="form-fuk-child-detail"]').submit(function (e) {
        e.preventDefault();
    }).validate({
        rules: {
            // on the right side
            Description: "required",
        },
        // Specify validation error messages
        messages: {
            Description: "Please fill description child",
        },
        // Make sure the form is submitted to the destination defined
        // in the "action" attribute of the form when valid
        submitHandler: async function (form) {
            var
                length = storedFUKDetail.length,
                fukF = storedFUKDetail.find(x => x.Id == $(form).find('input#Id').val()),
                childF = storedFUKDetail.filter(x => x.Parent == $(form).find('input#Id').val()),
                contentF = storedContentParameter.find(x => x.Id == fukF.ContentId),
                fukData = {
                    Id: length + 1,
                    Parent: fukF.Id,
                    Child: abjad[childF.length],
                    ContentId: contentF.Id,
                    ContentDescription: contentF.Description,
                    Description: $(form).find('textarea[name="Description"').val(),
                    Sequence: fukF.Sequence
                };

            // Set Local Storage
            storedFUKDetail.push(fukData);
            localStorage.setItem("fuk-details", JSON.stringify(storedFUKDetail));

            // reset input ParameterHeader
            $(form).find("textarea[name='Description']").val("");
            $(form).find("input[type=hidden]").val("");

            //mapping.push(object);
            var loadData = await GrouppingData();
            onSetHeader(loadData);

            // close modal
            $('#addFUKChildModal').modal('hide');
            return false;
        }
    });

    async function GrouppingData() {
        var mapping = new Array();
        for (var j = 0; j < storedFUKDetail.length; j++) {
            const fuk = storedFUKDetail[j];
            const object = {
                id: fuk.Id,
                parent: fuk.Parent,
                child: !fuk.Child ? '' : fuk.Child,
                sequence: fuk.Sequence,
                faktorUjiKesesuaian: fuk.Description
            };
            for (var k = 0; k < storedContentParameter.length; k++) {
                const parameter = storedContentParameter[k];
                if (parameter.Id === fuk.ContentId) {
                    object.parameter = parameter.Description
                    object.indikator = parameter.Aspek
                }
            }
            mapping.push(object);
        }
        return mapping;
    }

    /** Button Show Modal Fuk */
    $('#btnModalFUK').click(function () {
        isNewData = true;
        $('form[name="form-fuk-detail"]').find("select#ParemeterHeader").removeAttr("disabled");
        $('form[name="form-fuk-detail"]').find("input#Parent").removeAttr("disabled");
        fillModalFUK();
    });

    function fillModalFUK() {
        divContent.addClass('d-none');
        $('form[name="form-fuk-detail"]').find("input#Parent").prop("checked", false);
        $('form[name="form-fuk-detail"]').find("textarea#Description").val('');
        parameterInp.empty();
        parameterInp.append('<option value="" selected readonly>-- Select Parameter --</option>');

        for (var i = 0; i < storedHeaderParameter.length; i++) {
            const el = storedHeaderParameter[i];
            const val = '<option value="' + el.Id + '">' + el.Aspek + '</option>';
            parameterInp.append(val);
        }

        //set initial state.
        //fukParameterForm.find("input[type='checkbox']").val($(this).is(':checked')).removeAttr("checked");

        fukParameterForm.find("input[type='checkbox']").change(function () {
            if ($(this).is(":checked")) {
                $(this).attr("checked", true);
                parenting = true;
            } else {
                $(this).attr("checked", false);
                parenting = false;
            }

            fukParameterForm.find("input[type='checkbox']").val(!!$(this).is(':checked'));
        });

        modalDialog.modal('show');
    }

    function appendData_edit(fukID) {
        var selected_storedFUKDetail = storedFUKDetail.find(d => d.Id === fukID);
        var selected_storedContentParameter = storedContentParameter.find(d => d.Id === selected_storedFUKDetail.ContentId);
        var selected_storedHeaderParameter = storedHeaderParameter.find(d => d.Id === selected_storedContentParameter.HeaderId);

        $('form[name="form-fuk-detail"]').find("input#fukid").val(fukID).prop("readonly", "readonly");
        $('form[name="form-fuk-detail"]').find("select#ParemeterHeader").val(selected_storedHeaderParameter.Id).prop("readonly", "readonly");
        $('form[name="form-fuk-detail"]').find("textarea#Description").val(selected_storedFUKDetail.Description);

        if (parseInt(selected_storedHeaderParameter.Id) == 0) {
            divContent.addClass('d-none');
        } else {
            divContent.removeClass('d-none');
            onSetContentEdit(new Array(selected_storedContentParameter));
        }
        
        if (selected_storedFUKDetail.Parent == 'On') {
            $('form[name="form-fuk-detail"]').find("input#Parent").prop("checked", true).prop("readonly","readonly");
        } else {
            $('form[name="form-fuk-detail"]').find("input#Parent").prop("checked", false).prop("readonly", "readonly");
        }

        //disabled field
        $('form[name="form-fuk-detail"]').find("select#ParemeterHeader").prop("disabled", "disabled");
        $('form[name="form-fuk-detail"]').find("input#Parent").prop("disabled", "disabled");
    }

    /** Change Input Parameter */
    parameterInp.on('change', function () {
        var
            value = $(this).val() != "" ? parseInt($(this).val()) : 0,
            content = storedContentParameter.filter(x => x.HeaderId == value);
        if (value == 0) {
            divContent.addClass('d-none');
        } else {
            divContent.removeClass('d-none');
            onSetContent(content)
        }
    });

    function onSetContent(data) {
        var
            tableContent = divContent.find("table#input-content-parameter").DataTable();
        // Destroy
        tableContent.destroy();

        var table = divContent.find("table#input-content-parameter").DataTable({
            width: "100%",
            data: data,
            columns: [
                {
                    data: null,
                    orderable: false,
                    defaultContent: "",
                    render: function (data, type, row, meta) {
                        return `<input id="cs" type="checkbox"  value="${row['Id']}" />`;
                    }

                },
                { data: "Id", defaultContent: "0", visible: false },
                { data: "Description", defaultContent: "" }
            ],
            columnDefs: [],
            scrollX: true,
            scrollY: '100px',
            searching: true,
            paging: false,
            orderable: false,
            info: false,
            bScrollCollapse: false,
            order: [[1, 'asc']]
        });

        $("table#input-content-parameter tbody").on("change", '#cs', function (e) {
            e.preventDefault();
            checboxData = table.row(this.closest('tr')).data()
            if ($(this).is(":checked")) {
                $("input[type=checkbox]").removeClass('selected');
                $("input[type=checkbox]").prop("checked", false);
                $(this).addClass('selected');
                $(this).prop("checked", true);
            }
        });
        table.columns.adjust();
    };

    function onSetContentEdit(data) {
        var
            tableContent = divContent.find("table#input-content-parameter").DataTable();
        // Destroy
        tableContent.destroy();

        var table = divContent.find("table#input-content-parameter").DataTable({
            width: "100%",
            data: data,
            columns: [
                {
                    data: null,
                    orderable: false,
                    defaultContent: "",
                    render: function (data, type, row, meta) {
                        //return `<input id="cs" type="checkbox" checked="true" readonly value="${row['Id']}" />`;
                        return ``;
                    }

                },
                { data: "Id", defaultContent: "0", visible: false },
                { data: "Description", defaultContent: "" }
            ],
            columnDefs: [],
            scrollX: true,
            scrollY: '100px',
            searching: true,
            paging: false,
            orderable: false,
            info: false,
            bScrollCollapse: false,
            order: [[1, 'asc']]
        });

        $("table#input-content-parameter tbody").on("change", '#cs', function (e) {
            e.preventDefault();
            checboxData = table.row(this.closest('tr')).data()
            if ($(this).is(":checked")) {
                $("input[type=checkbox]").removeClass('selected');
                $("input[type=checkbox]").prop("checked", false);
                $(this).addClass('selected');
                $(this).prop("checked", true);
            }
        });
        table.columns.adjust();
    };

    //console.log(storedFUKDetail);
    fukParameterForm.submit(function (e) {
        e.preventDefault();
    }).validate({
        // Specify validation rules
        rules: {
            // on the right side
            ParameterHeader: "required",
            Description: "required",
        },
        // Specify validation error messages
        messages: {
            ParameterHeader: "Please select header",
            //Sequence: "Please provide a total parameter",
            Description: "Please fill description parameter",
        },
        // Make sure the form is submitted to the destination defined
        // in the "action" attribute of the form when valid
        submitHandler: async function (form) {
            var
                tables = divContent.find("table#input-content-parameter").DataTable(),
                rows = $("table#input-content-parameter").find('input.selected').val(),
                length = maxId(),
                //lengthP = storedFUKDetail.filter(x => x.ContentId == rows[0].Id && x.Parent).length,
                //lengthP = storedFUKDetail.filter(x => x.ContentId == parseInt(rows[0]) && x.Parent == 'Off').length,
                //lengthP = storedFUKDetail.filter(x => x.Child === null).length,
                //lengthP = storedFUKDetail.filter(x => x.Child === null && x.Parent === 'Off' && x.ContentId === checboxData.Id).length,
                lengthP = 0,
                fukData = {};
            if (isNewData) {
                lengthP = storedFUKDetail.filter(x => x.Child === null && x.ContentId === checboxData.Id).length;
                 fukData = {
                    Id: length + 1,
                    Parent: parenting ? "On" : "Off",
                    Child: null,
                    ContentId: checboxData.Id,
                    ContentDescription: checboxData.Description,
                    Description: $(form).find('textarea[name="Description"').val(),
                    Sequence: lengthP + 1,
                };
            }

            // Set Local Storage
            if (!isNewData) {
                var selected_storedFUKDetail = storedFUKDetail.find(d => d.Id === itemId);
                fukData = {
                    Id: selected_storedFUKDetail.Id,
                    Parent: selected_storedFUKDetail.Parent,
                    Child: selected_storedFUKDetail.Child,
                    ContentId: selected_storedFUKDetail.ContentId,
                    ContentDescription: selected_storedFUKDetail.ContentId,
                    Description: $(form).find('textarea[name="Description"').val(),
                    Sequence: selected_storedFUKDetail.Sequence
                }
                var deleteIndexOf = storedFUKDetail.indexOf(selected_storedFUKDetail);
                storedFUKDetail.splice(deleteIndexOf, 1);
                localStorage.setItem("fuk-details", JSON.stringify(storedFUKDetail));
            }
            storedFUKDetail.push(fukData);
            localStorage.setItem("fuk-details", JSON.stringify(storedFUKDetail));

            // reset input ParameterHeader
            $(form).find("textarea[name='Description']").val("");
            $(form).find("input[type=text]").val("");
            $(form).find("select").val("0");

            // reset dataTables
            tables.destroy();
            divContent.addClass('d-none');
            // perbarui data
            var loadData = await GrouppingData();
            onSetHeader(loadData);

            // close modal
            modalDialog.modal('hide');
            parenting = false;
            return false;
        }
    });

    function maxId() {
        var idmax = 0;
        for (var i = 0; i < storedFUKDetail.length; i++) {
            var id = storedFUKDetail[i].Id;
            if (idmax <= id)
                idmax = id;
        }
        return idmax;
    }

    async function loadFUK() {
        var datas = await GrouppingData();
        await onSetHeader(datas);
        
    }

    
    onSetHeader(datas);
});
