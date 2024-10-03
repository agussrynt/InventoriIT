$(async function () {
    var
        parameterId,
        fr = $('form[name="form-set-assignment"]')
        get_url = decodeURI(window.location.href),
        urlNew = new URL(get_url),
        paramUrl = urlNew.search.substring(urlNew.search.indexOf("?") + 1),
        splitParam = paramUrl.split("="),
        Param = window.atob(splitParam[1]);

    async function Load() {
        var fd = new FormData();
        fd.append("ParamsId", Param);
        await asyncAjax("/page/master-kertas-kerja/get-dropdown-ajax", "POST", fd)
            .then(async function successCallBack(response) {
                if (response.success) {
                    var groups_array = [];
                    $.each(response.data, function (index) {
                        groups_array.push({
                            id: response.data[index].id,
                            text: response.data[index].name,
                        });
                    });
                    $("select#Indikator").select2({
                        placeholder: "Please select one",
                        minimumResultsForSearch: -1,
                        allowClear: true,
                        data: groups_array,
                    });
                    $('select#Indikator').val("").trigger('change');
                    $('#yearData').html(Param);
                } else {
                    swallAllert.Error("Fetch Data Failed!", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            });

        // dropdown Indikator
        $('select#Indikator').change(async function () {
            var value = $(this).val();
            $("select#Parameter").empty().trigger("change");
            $("select#Parameter").select2({
                placeholder: "No data available",
                allowClear: true,
            });

            if (value && value !== "") {
                var fd = new FormData();
                fd.append("ParamsId", value)
                fd.append("Option", 1)

                await asyncAjax("/page/master-kertas-kerja/get-dropdown-ajax", "POST", fd)
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
                                $("select#Parameter").select2({
                                    placeholder: "Please select one",
                                    minimumResultsForSearch: -1,
                                    allowClear: true,
                                    data: groups_array,
                                });
                                $('select#Parameter').val("").trigger('change');
                                $('#paramsSelect').removeClass('d-none');
                            }
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

    $('#btnShow').click(async function () {
        var params = $("select#Parameter").val();
        if (params && params != "") {
            SetDataTable(params);
        }
    });

    console.log('test')

    async function SetDataTable(parameterId) {
        var fd = new FormData();
        fd.append('ParameterId', parameterId);

        await asyncAjax("/page/fuk-assignment/get-list", "POST", fd)
            .then(async function successCallBack(response) {
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
            var
                tbl = $('#dataTable').DataTable();
            tbl.destroy();

            $('#dataTable').DataTable({
                width: "100%",
                data: listData,
                columns: [
                    { data: 'id', defaultcontent: "", visible: false },
                    {
                        data: 'sequence',
                        defaultcontent: "",
                        render: function (data, type, full, meta) {
                            if (full.child) {
                                return data + '.' + full.child;
                            }

                            return data;
                        },
                        width: "3%"
                    },
                    { data: 'child', defaultcontent: "", visible: false },
                    {
                        data: 'description',
                        defaultContent: "-",
                        width: "50%"
                    },
                    {
                        data: 'assignment',
                        defaultContent: "-",
                        className: "text-center",
                        render: function (data, row, full, meta) {
                            if (!data) {
                                return '<span class="badge bg-info">Not Assignment</span>';
                            }

                            return data;
                        } 
                    },
                    {
                        data: 'dueDate',
                        defaultContent: "-",
                        className: "text-center",
                        render: function (data, type, full, meta) {
                            if (data) {
                                var date = new Date(data);
                                if (date.getFullYear() > 2018) {
                                    return convertDate(data);
                                }
                            }

                            return '<label class="badge bg-info">No Due Date</label>'
                        }
                    },
                    {
                        data: null,
                        orderable: false,
                        defaultContent: "-",
                        width: "5%",
                        render: function (data, type, row, meta) {
                            var btn = `<a id="btnModal" class="text-info">
                                <i class='bx bx-edit'></i>
                            </a>`;

                            return btn;
                        }
                    }
                ],
                columnDefs: [],
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
                    //right: 1
                },
                order: [[0, 'asc']]
            });
            $('#tableData').removeClass('d-none');
            $('#dataTable').on('click', '#btnModal', await openModal);
            async function openModal() {
                var
                    tb = $('#dataTable').DataTable(),
                    dt = tb.row($(this).parents('tr')).data(),
                    id = fr.find('input[type="hidden"]'),
                    dd = fr.find('input[type="date"]'),
                    tx = fr.find('textarea#Description'),
                    ob = { key: 'iD_FUNGSI', value: 'fungsiname' };

                id.val(dt.id);
                tx.val(dt.description);

                dd.val("");
                await fillSelect(baseUrl + "/master/fungsi/get-list-ajax", "#Assignment", ob);
                $('select#Assignment').val("").trigger('change');
                $('#setAssignment').modal("show");
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
                Id: fr.find('input[type="hidden"]').val(),
                Description: fr.find('textarea').val(),
                DueDate: fr.find('input[type="date"]').val(),
                Assignment: fr.find('select#Assignment').val(),
                PIC: fr.find('select#PIC').val()
            };

            await asyncAjax("/page/fuk-assignment/set-ajax", "POST", JSON.stringify(data), true)
                .then(async function successCallBack(response) {
                    console.log(response);
                    if (response.success) {
                        swallAllert.Success("Success!", response.message);
                        loadingForm(false, 'btnSetAssignment', "Submit");
                        $('#setAssignment').modal("hide");

                        await SetDataTable($("select#Parameter").val());
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
});