$(function () {
    var
        upLinks = $('#up-links'),
        btnSubmit = upLinks.find('button#btnSubmitMKK'),
        modalDialog = $('#addUPModal'),
        fukUpLink = modalDialog.find('form[name="form-up-link"]'),
        parameterInp = fukUpLink.find("select#ParemeterHeader"),
        contentInp = fukUpLink.find("select#ParemeterContent"),
        divContent = modalDialog.find('#fukDetail');

    async function fetchAllData() {
        var
            mapping = new Array;

        for (var i = 0; i < storedUPLink.length; i++) {
            const up = storedUPLink[i];
            const object = {
                id: up.Id,
                unsurPemenuhan: up.Description
            };
            for (var j = 0; j < storedFUKDetail.length; j++) {
                const fuk = storedFUKDetail[j];
                if (fuk.Id === up.FUKId) {
                    object.faktorUjiKesesuaian = fuk.Description
                    for (var k = 0; k < storedContentParameter.length; k++) {
                        const parameter = storedContentParameter[k];
                        if (parameter.Id === fuk.ContentId) {
                            object.parameter = parameter.description
                            for (var l = 0; l < storedHeaderParameter.length; l++) {
                                const indikator = storedHeaderParameter[l];
                                if (indikator.id === parameter.HeaderId) {
                                    object.indikator = indikator.aspek
                                }
                            }
                        }
                    }
                }
            }
            mapping.push(object);
        }
    }

    function onSetHeader() {
        var
            groupColumn = 2,
            collapsedGroups = {},
            tableHeader = upLinks.find("table#table-up-link").DataTable();
        tableHeader.destroy();

        var table = upLinks.find("table#table-up-link").DataTable({
            width: "100%",
            data: storedUPLink,
            columns: [
                { data: "Id", defaultContent: "", orderable: false, visible: false },
                { data: "FUKId", defaultContent: "", orderable: false },
                { data: "FUKDescription", defaultContent: "", orderable: false },
                //{ data: "Sequence", defaultContent: "", orderable: false },
                { data: "Description", defaultContent: "", orderable: false },
                {
                    data: null,
                    defaultContent: "",
                    orderable: false,
                    render: function (data, type, row) {
                        var button = `
                            <a href="#" class="text-info">
                                <i class='bx bx-edit'></i>
                            </a>
                            <a href="#" class="text-danger">
                                <i class='bx bx-trash'></i>
                            </a>
                        `;

                        return button
                    },
                },
            ],
            columnDefs: [
                { orderable: false, targets: 0 },
                { visible: false, targets: 1 },
                { visible: false, targets: groupColumn },
            ],
            scrollX: true,
            scrollY: '250px',
            searching: false,
            orderable: false,
            info: false,
            bPaginate: false,
            bLengthChange: false,
            bFilter: false,
            bInfo: false,
            bAutoWidth: false,
            rowGroup: {
                // Uses the 'row group' plugin
                dataSrc: 'FUKDescription',
                startRender: function (rows, group) {
                    var collapsed = !!collapsedGroups[group];

                    rows.nodes().each(function (r) {
                        r.style.display = collapsed ? 'none' : '';
                    });

                    var icon = '<i class="bx bx-plus-circle text-success me-1"></i>';
                    if (!collapsed)
                        icon = '<i class="bx bx-minus-circle text-danger me-1"></i>';

                    // Add category name to the <tr>. NOTE: Hardcoded colspan
                    return $('<tr/>')
                        .append('<td colspan="6">' + icon + group + ' (' + rows.count() + ')</td>')
                        .attr('data-name', group)
                        .toggleClass('collapsed', collapsed);
                }
            },
        });

        $('table#table-up-link tbody').on('click', 'tr.group-start', function () {
            var name = $(this).data('name');
            collapsedGroups[name] = !collapsedGroups[name];
            table.draw(false);
        });
    };

    /** Button Show Modal UP */
    $('#btnModalUP').click(function () {
        parameterInp.empty();
        parameterInp.append('<option value="" selected readonly>-- Select Indikator --</option>');
        parameterInp.val(0)

        for (var i = 0; i < storedHeaderParameter.length; i++) {
            const el = storedHeaderParameter[i];
            const val = '<option value="' + el.Id + '">' + el.Aspek + '</option>';
            parameterInp.append(val);
        }
        //onSetContent(storedFUKDetail)

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
        console.log(fuk)
        if (value == 0) {
            divContent.addClass('d-none');
        } else {
            onSetFUK(fuk)
            divContent.removeClass('d-none');
        }
    });

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
                    width: "5%"
                },
                { data: "Id", defaultContent: "", visible: false },
                { data: "Child", defaultContent: "", visible: false },
                {
                    data: "Sequence",
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
            columnDefs: [
                {
                    'targets': 0,
                    'checkboxes': {
                        'selectRow': true
                    }
                }
            ],
            select: {
                'style': 'single'
            },
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

        table.columns.adjust();
    };

    /**
      * Submit Form UP Link
     */
    fukUpLink.validate({
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
        submitHandler: function (form) {
            var
                tables = divContent.find("table#input-fuk-detail").DataTable(),
                rows = tables.rows('.selected').data().toArray(),
                length = storedUPLink.length,
                upData = {
                    Id: length + 1,
                    FUKId: rows[0].Id,
                    FUKDescription: rows[0].Description,
                    Description: $(form).find('textarea[name="Description"').val(),
                    //Sequence: $(form).find('input[name="Sequence"]').val(),
                }
            // Set Local Storage
            storedUPLink.push(upData);
            localStorage.setItem("up-links", JSON.stringify(storedUPLink));

            // reset input ParameterHeader
            $(form).find("textarea[name='Description']").val("");
            $(form).find("input[type=text]").val("");

            // reset dataTables
            onSetHeader();

            // close modal
            modalDialog.modal('hide');

            return false;
        }
    });

    onSetHeader();
});
