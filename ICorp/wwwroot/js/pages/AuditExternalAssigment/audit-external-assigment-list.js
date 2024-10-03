$(async function () {
    var fr = $('form[name="form-set-assignment"]');
    $('#assignYear').on('change', await Load);

    async function Load() {
        var fd = new FormData()
            , yr = $('#assignYear').val();
        fd.append('year', parseInt(yr));

        await asyncAjax("/page/auditexternalassigment/get-list-ajax", "POST", fd)
            .then(async function successCallBack(response) {
                console.log(response);
                if (response.success) {
                    OnSuccess(response.data);
                } else {
                    swallAllert.Error("Fetch Data Failed!", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            });

        /** When success fetch data user and create dataTable */
        async function OnSuccess(listData) {
            var groupParent = [];
            var collapsedGroups = [];
            var tbl = $('#dataTable').DataTable();
            tbl.destroy();

            $('#dataTable').DataTable({
                width: "100%",
                data: listData,
                columns: [
                    //{ data: 'iD_AUDIT_EXTERNAL_DATA_RECOMENDATION', defaultcontent: "", visible: false },
                    { data: 'iD_AUDIT_EXTERNAL', defaultcontent: "", visible: false },
                    //{ data: 'auditoR_NAME', defaultcontent: "", visible: false }, 
                    { data: 'rekomendasi', defaultcontent: "" },
                    { data: 'functioN_STR', defaultcontent: "" },
                    { data: 'piC_STR', defaultcontent: "" },
                    {
                        data: 'duE_DATE',
                        defaultContent: "-",
                        render: function (data, type, full, meta) {
                            if (data != null)
                                return convertDate(data);
                            else
                                return '';
                        }
                    },
                    {
                        data: null,
                        defaultContent: "-",
                        orderable: false,
                        render: function (data, type, row, meta) {
                            var result = ''
                            if (data.status === '')
                                result = `<a type="button" class="text-primary assign" data-bs-toggle="tooltip" data-bs-placement="top" title="Set Assignment"><i class='bx bx-street-view'></i></a>`;
                            if (data.status === '[External] Assignment')
                                result = `<a type="button" class="text-info editAssign"><i class='bx bx-edit'></i></a>`;
                            return result;
                        }
                    }
                ],
                columnDefs: [
                    {
                        sortable: false,
                        orderable: false,
                        searchable: false,
                        targets: 0,
                    }, {
                        className: "text-center",
                        targets: [5],
                    }
                ],
                    rowGroup: {
                        dataSrc: ['auditoR_NAME'],
                        //dataSrc: ['iD_AUDIT_EXTERNAL'],
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
                                .append('<td colspan="12">' + icon + group + '</td>')
                                //.attr('data-name', all)
                                .attr('data-name', groupAll)
                                .toggleClass('collapsed', collapsed);
                        }
                    },
                
                lengthMenu: [[5, 10, 50, 100, -1], [5, 10, 50, 100, "All"]],
                autoWidth: true,
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                scrollX: true,
                bScrollCollapse: true,
                fixedColumns: {
                    left: 1,
                },
                order: [[0, 'desc']]
            });

            $('#dataTable').on('click', '.assign', await openModalAdd);
            $('#dataTable').on('click', '.editAssign', await openModalEdit);

            async function openModalAdd() {
                var
                    tb = $('#dataTable').DataTable(),
                    dt = tb.row($(this).parents('tr')).data(),
                    id = fr.find('input[type="hidden"]'),
                    dd = fr.find('input[type="date"]'),
                    tx = fr.find('textarea#Recomendation'),
                    ob = { key: 'iD_FUNGSI', value: 'fungsiname' };

                id.val(dt.iD_AUDIT_EXTERNAL_DATA_RECOMENDATION);
                tx.val(dt.rekomendasi);

                dd.val("");
                await fillSelect(baseUrl + "/master/fungsi/get-list-ajax", "#Assignment", ob);
                $('select#Assignment').val("").trigger('change');
                $('#setAssignment').modal("show");
            }

            async function openModalEdit() {
                var
                    tb = $('#dataTable').DataTable(),
                    dt = tb.row($(this).parents('tr')).data(),
                    id = fr.find('input[type="hidden"]'),
                    dd = fr.find('input[type="date"]'),
                    tx = fr.find('textarea#Recomendation'),
                    ob = { key: 'iD_FUNGSI', value: 'fungsiname' };

                id.val(dt.iD_AUDIT_EXTERNAL_DATA_RECOMENDATION);
                tx.val(dt.rekomendasi);
                dd.val(DateFunction.dateStrYYYYMMDD(dt.duE_DATE));
               // await fillSelect("/master/fungsi/get-list-ajax", "#Assignment", ob);

                await asyncAjax("/master/fungsi/get-list-ajax", "GET", ob, true)
                    .then(async function successCallBack(response) {
                        if (response.success) {
                            if (response.data.length > 0) {
                                var groups_array = [];
                                $.each(response.data, function (index) {
                                    groups_array.push({
                                        id: response.data[index].iD_FUNGSI,
                                        text: response.data[index].fungsiname,
                                    });
                                });
                                $("select#Assignment").select2({
                                    placeholder: "Please select one",
                                    minimumResultsForSearch: -1,
                                    allowClear: true,
                                    data: groups_array,
                                });
                                $("select#Assignment").select2("val", dt.iD_FUNGSI.toLowerCase());
                                $('select#Assignment').val(dt.iD_FUNGSI).trigger('change');
                                console.log(dt.useriD_STR);

                                $('select#Assignment').val(dt.iD_FUNGSI).trigger('change');
                                $("select#Assignment").select2("val", dt.iD_FUNGSI);
                                var data = dt.iD_FUNGSI;
                                if (data && data !== "") {
                                    var params = {
                                        ParamsId: parseInt(data)
                                    };

                                    await asyncAjax("/master/user-management/get-pic-ajax", "POST", JSON.stringify(params), true)
                                        .then(async function successCallBack(response) {
                                            if (response.success) {
                                                if (response.data.length > 0) {
                                                    var groups_array = [];
                                                    $.each(response.data, function (index) {
                                                        groups_array.push({
                                                            id: response.data[index].id.toString(),
                                                            text: response.data[index].name,
                                                        });
                                                    });
                                                    $("select#PIC").select2({
                                                        placeholder: "Please select one",
                                                        minimumResultsForSearch: -1,
                                                        allowClear: true,
                                                        data: groups_array,
                                                    });
                                                    $("select#PIC").select2("val", dt.useriD_STR.toLowerCase());
                                                    console.log(dt.useriD_STR);
                                                }
                                                $('#userPic').removeClass('d-none');
                                            } else {
                                                swallAllert.Error("Fetch Data Failed!", response.data);
                                            }
                                        })
                                        .catch(async function errorCallBack(err) {
                                            swallAllert.Error("Fetch Data Failed!", err.data);
                                        });
                                }

                            }
                            

                            $('#setAssignment').modal("show");
                        } else {
                            swallAllert.Error("Fetch Data Failed!", response.data);
                        }
                    })
                    .catch(async function errorCallBack(err) {
                        swallAllert.Error("Fetch Data Failed!", err.data);
                    });

                
            }
        }
        // dropdown field survey exist
        $('select#Assignment').on("change", async function () {
            $("select#PIC").empty().trigger("change");
            $("select#PIC").select2({
                placeholder: "No data available",
                allowClear: true,
            });
            var data = this.value;
            if (data && data !== "") {
                var params = {
                    ParamsId: parseInt(data)
                };

                await asyncAjax("/master/user-management/get-pic-ajax", "POST", JSON.stringify(params), true)
                    .then(async function successCallBack(response) {
                        if (response.success) {
                            if (response.data.length > 0) {
                                var groups_array = [];
                                $.each(response.data, function (index) {
                                    groups_array.push({
                                        id: response.data[index].id,
                                        text: response.data[index].name,
                                    });
                                });
                                $("select#PIC").select2({
                                    placeholder: "Please select one",
                                    minimumResultsForSearch: -1,
                                    allowClear: true,
                                    data: groups_array,
                                });
                                $('select#PIC').val("").trigger('change');
                            }
                            $('#userPic').removeClass('d-none');
                        } else {
                            swallAllert.Error("Fetch Data Failed!", response.data);
                        }
                    })
                    .catch(async function errorCallBack(err) {
                        swallAllert.Error("Fetch Data Failed!", err.data);
                    });
            }

            return;
        });
    }

    fr.submit(async function (e) {
        e.preventDefault();
        if (fr.valid()) {
            loadingForm(true, 'btnSetAssignment');
            var data = {
                ID_AUDIT_EXTERNAL_DATA_RECOMENDATION: fr.find('input[type="hidden"]').val(),
                DUE_DATE: fr.find('input[type="date"]').val(),
                Assignment: fr.find('select#Assignment').val(),
                USERID: fr.find('select#PIC').val()
            };

            await asyncAjax("/page/auditexternalassigment/post-ajax", "POST", JSON.stringify(data), true)
                .then(async function successCallBack(response) {
                    console.log(response);
                    if (response.success) {
                        swallAllert.Success("Success!", response.message);
                        loadingForm(false, 'btnSetAssignment', "Submit");
                        $('#setAssignment').modal("hide");
                        Load();
                    } else {
                        swallAllert.Error("Fetch Data Failed!", response.message);
                        loadingForm(false, 'btnSetAssignment', "Submit");
                    }
                })
                .catch(async function errorCallBack(err) {
                    swallAllert.Error("Fetch Data Failed!", err.data);
                    loadingForm(false, 'btnSetAssignment', "Submit");
                });
        }
    });

    await Load();
})