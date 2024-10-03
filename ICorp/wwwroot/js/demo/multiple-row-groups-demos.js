$(function () {
    var data = [
        {
            indikator: "I. Bab 1 Pendahuluan",
            parameter: "Parameter 1",
            fuk: "fuk 1",
            up: "up 1",
            document: "document 1",
            ip_id: 1
        },
        {
            indikator: "I. Bab 1 Pendahuluan",
            parameter: "Parameter 1",
            fuk: "fuk 1",
            up: "up 2",
            document: "document 2",
            ip_id: 2
        },
        {
            indikator: "I. Bab 1 Pendahuluan",
            parameter: "Parameter 1",
            fuk: "fuk 1",
            up: "up 3",
            document: "document 3",
            ip_id: 3
        },
        {
            indikator: "I. Bab 1 Pendahuluan",
            parameter: "Parameter 1",
            fuk: "fuk 2",
            up: "up 1",
            document: "document 1",
            ip_id: 4
        },
        {
            indikator: "I. Bab 1 Pendahuluan",
            parameter: "Parameter 2",
            fuk: "fuk 1",
            up: "up 1",
            document: "document 1",
            ip_id: 5
        },
        {
            indikator: "II. Bab 2 Latar Belakang",
            parameter: "Parameter 1",
            fuk: "fuk 1",
            up: "up 1",
            document: "document 1",
            ip_id: 5
        }
    ];

    var collapsedGroups = {};
    var top = '';
    var parent = '';

    var table = $('#dataTable').DataTable({
        width: "100%",
        data: data,
        columns: [
            { data: 'indikator', title: 'indikator' },
            { data: 'parameter', title: 'parameter' },
            { data: 'fuk', title: 'fuk' },
            {
                data: 'up',
                title: 'FUK / UP / Action',
                class: 'order_id'
            },
            { data: 'document' },
            {
                data: null,
                width: "3%",
                render: function (data, type, row) {
                    var button = `
                        <a href="#" class="text-info">
                            <i class='bx bx-edit'></i>
                        </a>
                    `;

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
            dataSrc: ['indikator', 'parameter', 'fuk'],
            startRender: function (rows, group, level) {
                var all;
                if (level === 0) {
                    top = group;
                    all = group;
                } else {
                    // if parent collapsed, nothing to do
                    if (!!collapsedGroups[top]) {
                        return;
                    }
                    all = top + group;
                }

                var collapsed = !!collapsedGroups[all];

                rows.nodes().each(function (r) {
                    r.style.display = collapsed ? 'none' : '';
                });

                // Add category name to the <tr>. NOTE: Hardcoded colspan
                return $('<tr/>')
                    .append('<td colspan="12">' + group + ' (' + rows.count() + ')</td>')
                    .attr('data-name', all)
                    .toggleClass('collapsed', collapsed);
                //if (group === 'I. Pendahuluan') {
                //    var collapsed = !!collapsedIndikator[group];

                //    rows.nodes().each(function (r) {
                //        r.style.display = collapsed ? 'none' : '';
                //    });

                //    var icon = '<i class="bx bx-plus-circle text-success me-1"></i>';
                //    if (!collapsed)
                //        icon = '<i class="bx bx-minus-circle text-danger me-1"></i>';

                //    // Add category name to the <tr>. NOTE: Hardcoded colspan
                //    return $('<tr/>')
                //        .append('<td colspan="6">' + icon + group + ' (' + rows.count() + ')</td>')
                //        .attr('data-name', group)
                //        .toggleClass('collapsed', collapsed);
                //} else
                //    if (group === 'Lorem ipsum parameter') {
                //    var collapsed = !!collapsedParameter[group];

                //    rows.nodes().each(function (r) {
                //        r.style.display = collapsed ? 'none' : '';
                //    });

                //    var icon = '<i class="bx bx-plus-circle text-success me-1"></i>';
                //    if (!collapsed)
                //        icon = '<i class="bx bx-minus-circle text-danger me-1"></i>';

                //    // Add category name to the <tr>. NOTE: Hardcoded colspan
                //    return $('<tr/>')
                //        .append('<td colspan="5">' + icon + group + ' (' + rows.count() + ')</td>')
                //        .attr('data-name', group)
                //        .toggleClass('collapsed', collapsed);
                //}

                //// Add category name to the <tr>. NOTE: Hardcoded colspan
                //return $('<tr/>')
                //    .append('<td colspan="6">' + icon + group + ' (' + rows.count() + ')</td>')
                //    .attr('data-name', group)
                //    .toggleClass('collapsed', collapsed);
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
})