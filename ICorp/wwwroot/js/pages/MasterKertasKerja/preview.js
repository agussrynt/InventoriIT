$(async function () {
    
    //var datas = await fetchAllData();

    $('.btn-next-preview').on('click', preview);

    async function Load() {
        var mapping = new Array;

        for (var i = 0; i < storedUPLink.length; i++) {
            const up = storedUPLink[i];
            const object = {
                id: up.Id,
                unsurPemenuhan: up.Description
            };
            for (var j = 0; j < storedFUKDetail.length; j++) {
                const fuk = storedFUKDetail[j];
                if (fuk.Id === up.FUKId) {
                    var child = '';
                    if (fuk.Child) {
                        child = child + fuk.Child + '.';
                    }
                    object.faktorUjiKesesuaian = fuk.Sequence + child + ' ' + fuk.Description;
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

        function OnSuccess(data) {
            //console.log(data);
            //var collapsedGroups = {};
            var
                groupParent = new Array,
                collapsedGroups = new Array,
                tableHeader = $('#table-preview').DataTable();
            tableHeader.destroy();

            var table = $('#table-preview').DataTable({
                width: "100%",
                data: data,
                columns: [
                    { data: 'indikator', title: 'indikator' },
                    { data: 'parameter', title: 'parameter' },
                    { data: 'faktorUjiKesesuaian', title: 'fuk' },
                    {
                        data: 'unsurPemenuhan',
                        orderable: false,
                        title: 'Faktor Uji Kesesuaian / Unsur Pemenuhan',
                        class: 'order_id'
                    }
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
                        var groupAll = '';
                        for (var i = 0; i < level; i++) { groupAll += groupParent[i]; if (collapsedGroups[groupAll]) { return; } }
                        groupAll += group;
                        if ((typeof (collapsedGroups[groupAll]) == 'undefined') || (collapsedGroups[groupAll] === null)) { collapsedGroups[groupAll] = true; } //True = Start collapsed. False = Start expanded.

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

            $('#table-preview tbody').on('click', 'tr.dtrg-start', function () {
                var name = $(this).data('name');
                collapsedGroups[name] = !collapsedGroups[name];
                table.draw(false);
            });
        }

        OnSuccess(mapping);
    }

    function OnLoad(data) {

        //console.log(data);
        //var collapsedGroups = {};
        var
            groupParent = new Array,
            collapsedGroups = new Array,
            tableHeader = $('#table-preview').DataTable();
        tableHeader.destroy();

        var table = $('#table-preview').DataTable({
            width: "100%",
            data: data,
            columns: [
                { data: 'indikator', title: 'indikator' },
                { data: 'parameter', title: 'parameter' },
                { data: 'faktorUjiKesesuaian', title: 'fuk' },
                {
                    data: 'unsurPemenuhan',
                    orderable: false,
                    title: 'Faktor Uji Kesesuaian / Unsur Pemenuhan',
                    class: 'order_id'
                }
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
            searching: false,
            orderable: false,
            info: false,
            paging: false,
            bScrollCollapse: false,
            scrollY: false
        });

        $('#table-preview tbody').on('click', 'tr.dtrg-start', function () {
            var name = $(this).data('name');
            collapsedGroups[name] = !collapsedGroups[name];
            table.draw(false);
        });
    }

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

        return mapping;
    }

    async function preview() {
        var datas = await fetchAllData();
        await OnLoad(datas);
    }

    //await OnLoad(datas);
})