$(async function () {
    $('.btn-next-up').on('click', loadUP);
    var
        checboxData,
        upLinks = $('#up-links'),
        btnSubmit = upLinks.find('button#btnSubmitMKK'),
        modalDialog = $('#addUPModal'),
        datas = await fetchAllData(),
        fukUpLink = modalDialog.find('form[name="form-up-link"]'),
        parameterInp = fukUpLink.find("select#ParemeterHeader"),
        contentInp = fukUpLink.find("select#ParemeterContent"),
        divContent = modalDialog.find('#fukDetail'),
        isNewData = true,
        itemId = 0;


    async function fetchAllData() {
        var
            mapping = new Array;
        for (var i = 0; i < storedUPLink.length; i++) {
            const up = storedUPLink[i];
            const object = {
                id: up.Id,
                unsurPemenuhan: up.Description,
                faktorUjiKesesuaian:''
            };
            for (var j = 0; j < storedFUKDetail.length; j++) {
                const fuk = storedFUKDetail[j];
                if (fuk.Id === up.FUKId) {
                    var child = '';
                    if (fuk.Child) {
                        child = child + '.' + fuk.Child;
                    }
                    object.faktorUjiKesesuaian = fuk.Sequence + child + '. ' + fuk.Description;
                    for (var k = 0; k < storedContentParameter.length; k++) {
                        const parameter = storedContentParameter[k];
                        if (parameter.Id === fuk.ContentId) {
                            object.parameter = parameter.Description;
                            object.indikator = parameter.Aspek;
                        }
                    }
                }
            }
            mapping.push(object);
        }

        console.log('fetchAllData');
        console.log(mapping);
        return mapping;
    }

    function onSetHeader(mapping) {
        //alert('mapping')
        //console.log('mapping')
        //console.log(mapping)
        var
            groupParent = new Array,
            collapsedGroups = new Array,
            tableHeader = upLinks.find("table#table-up-link").DataTable();
        tableHeader.destroy();
        console.log(mapping)
        var table = upLinks.find("table#table-up-link").DataTable({
            width: "100%",
            data: mapping,
            columns: [
                { data: "indikator", defaultContent: "", title: 'Indikator' },
                { data: "parameter", defaultContent: "", title: 'Parameter' },
                { data: 'faktorUjiKesesuaian', title: 'fuk' },
                { data: "id", defaultContent: "", visible: false },
                {
                    data: "unsurPemenuhan",
                    defaultContent: "",
                    width: "87%",
                    orderable: false,
                    title: 'Indikator / Parameter / Faktor Uji Kesesuaian / Unsur Pemenuhan',
                    class: 'order_id'
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
                            btnEdit = '<a  class="text-info"><i class="bx bx-edit"></i></a>',
                            btnDelt = '<a  class="text-danger"><i class="bx bx-trash"></i></a>';

                        return btnEdit + btnDelt;
                    },
                },
            ],
            order: [[0, 'asc'], [1, 'asc'], [2, 'asc']],
            columnDefs: [
                {
                    targets: [0, 1, 2],
                    visible: false
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
            //scrollY: '250px',
            searching: false,
            orderable: false,
            info: false,
            bPaginate: false,
            bLengthChange: false,
            bFilter: false,
            bInfo: false,
            bAutoWidth: false,
        });

        $('table#table-up-link tbody').on('click', 'tr.dtrg-start', function () {
            var name = $(this).data('name');
            collapsedGroups[name] = !collapsedGroups[name];
            table.draw(false);
        });

        $('table#table-up-link tbody').on('click', '.bx-edit', function () {
            var
                tb = $('table#table-up-link').DataTable(),
                dt = tb.row($(this).parents('tr')).data(),
                tr = tb.row($(this).parents('tr'));
            var selected_storedUPLink = storedUPLink.find(d => d.Id === dt.id);
            editUP(selected_storedUPLink);

           
        });

        $('table#table-up-link tbody').on('click', '.bx-trash', function () {
            swallAllert.Confirm.Delete('Are you sure?', "You won't be able to remove this!").then((result) => {
                if (result.isConfirmed) {
                    var
                        tb = $('table#table-up-link').DataTable(),
                        dt = tb.row($(this).parents('tr')).data(),
                    tr = tb.row($(this).parents('tr'));

                    var selected_storedUPLink = storedUPLink.find(d => d.Id === dt.id);
                    console.log(selected_storedUPLink);
                    var deleteIndexOf = storedUPLink.indexOf(selected_storedUPLink);
                    storedUPLink.splice(deleteIndexOf, 1);
                    localStorage.setItem("up-links", JSON.stringify(storedUPLink));
                    tb.row(tr).remove().draw();
                }
            })
        });
    };

    /** Button Show Modal UP */
    $('#btnModalUP').click(function () {
        $('#Description').val('');
        isNewData = true;
        itemId = 0;
        parameterInp.empty();
        parameterInp.append('<option value="" selected readonly>-- Select Indikator --</option>');
        parameterInp.val(0);
        contentInp.val('');

        divContent.addClass('d-none');

        for (var i = 0; i < storedHeaderParameter.length; i++) {
            const el = storedHeaderParameter[i];
            const val = '<option value="' + el.Id + '">' + el.Aspek + '</option>';
            parameterInp.append(val);
        }
        fukUpLink.find('textarea[name="Description"').val()
        modalDialog.modal('show');
    });

    /** Change Input Header Parameter */
    parameterInp.on('change', function () {
        var
            value = $(this).val() != "" ? parseInt($(this).val()) : 0,
            content = storedContentParameter.filter(x => x.HeaderId == value);
        if (value != 0) {
            contentInp.empty();
            contentInp.append('<option value="" selected readonly>-- Select Parameter --</option>');

            for (var i = 0; i < content.length; i++) {
                const el = content[i];
                const val = '<option value="' + el.Id + '">' + el.Description + '</option>';
                contentInp.append(val);
            }
            modalDialog.find('#paramDetailUp').removeClass('d-none');
        } else {
            modalDialog.find('#paramDetailUp').addClass('d-none');
        }
    });

    /** Change Input Content Parameter */
    contentInp.on('change', function () {
        
        var
            value = $(this).val() != "" ? parseInt($(this).val()) : 0,
            fuk = storedFUKDetail.filter(x => x.ContentId == value);
        if (value == 0) {
            divContent.addClass('d-none');
        } else {
            onSetFUK(fuk)
            divContent.removeClass('d-none');
        }
    });

    function editUP(obj) {
        isNewData = false;
        itemId = obj.Id;
        console.log(obj);

        var tempFUK = storedFUKDetail.find(d => d.Id === obj.FUKId),
            tempContent = storedContentParameter.find(d => d.Id = tempFUK.ContentId);


        parameterInp.empty();
        parameterInp.append('<option value="" selected readonly>-- Select Indikator --</option>');

        for (var i = 0; i < storedHeaderParameter.length; i++) {
            const el = storedHeaderParameter[i];
            const val = '<option value="' + el.Id + '">' + el.Aspek + '</option>';
            parameterInp.append(val);
        }
        parameterInp.val(tempContent.HeaderId);

        var content = storedContentParameter.filter(x => x.HeaderId == tempContent.HeaderId);
        contentInp.empty();
        contentInp.append('<option value="" selected readonly>-- Select Parameter --</option>');

        for (var i = 0; i < content.length; i++) {
            const el = content[i];
            const val = '<option value="' + el.Id + '">' + el.Description + '</option>';
            contentInp.append(val);
        }
        contentInp.val(tempContent.Id);

        var fuk = storedFUKDetail.filter(x => x.ContentId == tempContent.Id);
        console.log(fuk);
        onSetFUK(fuk);

        $('textarea#Description').val(obj.Description);

        divContent.removeClass('d-none');
        modalDialog.find('#paramDetailUp').removeClass('d-none');
        modalDialog.modal('show');
    }

    /**
     * On Set Content
     * @param {any} data
     */
    function onSetFUK(data) {
        var
            tableContent = divContent.find("table#input-fuk-detail").DataTable();
        tableContent.destroy();

        var table = divContent.find("table#input-fuk-detail").DataTable({
            width: "100%",
            data: data,
            columns: [
                {
                    data: null,
                    orderable: false,
                    defaultContent: "",
                    render: function (data, type, row, meta) {
                        return `<input id="cs" type="checkbox" value="${row['Id']}" />`;
                    }
                },
                { data: "Id", defaultContent: "", visible: false },
                { data: "Child", defaultContent: "", visible: false },
                {
                    data: "Sequence",
                    width: "5%",
                    defaultContent: "",
                    render: function (data, type, row) {
                        if (row['Child']) {
                            return data + '.' + row['Child'];
                        }

                        return data;

                    },
                },
                { data: "Description", defaultContent: "" },
            ],
            autoWidth: true,
            scrollX: true,
            scrollY: '100px',
            searching: false,
            paging: false,
            orderable: false,
            info: false,
            bOrder: false,
            bScrollCollapse: false,
            order: [[1, 'asc']]
        });

        $("table#input-fuk-detail tbody").on("change", '#cs', function (e) {
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

    /**
      * Submit Form UP Link
     */
    fukUpLink.submit(e => {
        e.preventDefault();
    }).validate({
        // Specify validation rules
        rules: {
            // on the right side
            Sequence: "required",
            Description: "required",
        },
        // Specify validation error messages
        messages: {
            Sequence: "Please provide a total parameter",
            Description: "Please fill description parameter",
        },
        // Make sure the form is submitted to the destination defined
        // in the "action" attribute of the form when valid
        submitHandler: async function (form) {
            var ln = await maxId();
            var
                tables = divContent.find("table#input-fuk-detail").DataTable(),
                //rows = tables.rows('.selected').data().toArray(),
                length = ln,
                upData = {
                    Id: length + 1,
                    FUKId: checboxData.Id,  
                    FUKDescription: checboxData.Description,
                    Description: $(form).find('textarea[name="Description"').val(),
                    //Sequence: $(form).find('input[name="Sequence"]').val(),
                }
            // Set Local Storage
            if (!isNewData) {
                var selected_storedUPLink = storedUPLink.find(d => d.Id === itemId);
                console.log(selected_storedUPLink);
                var deleteIndexOf = storedUPLink.indexOf(selected_storedUPLink);
                storedUPLink.splice(deleteIndexOf, 1);
            }
            storedUPLink.push(upData);
            localStorage.setItem("up-links", JSON.stringify(storedUPLink));

            // reset input ParameterHeader
            $(form).find("textarea[name='Description']").val("");
            $(form).find("input[type=text]").val("");

            // reset dataTables
            var dt = await fetchAllData()
            onSetHeader(dt);

            // close modal
            modalDialog.modal('hide');

            return false;
        }
    });

    async function maxId() {
        var idmax = 0;
        for (var i = 0; i < storedUPLink.length; i++) {
            var id = storedUPLink[i].Id;
            if (idmax <= id)
                idmax = id;
        }
        return idmax;
    }

    async function loadUP() {
        var datas = await fetchAllData();
        await onSetHeader(datas);
    }

    onSetHeader(datas);
});
