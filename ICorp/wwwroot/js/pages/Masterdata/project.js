$(document).ready(async function () {
    /** Initiate variable awal */
    var
        ifEdit = false,
        submitProject = "submitProject",
        formProject = $("form.project-modal"),
        modalDialog = $("#projectModal"),
        modalTitle = modalDialog.find("h4.modal-title"),
        idproject = '',

        //Parent Dropdown
        ddSegmenParent = $("#ddSegmen"),
        ddContractParent = $("#ddContract"),
        ddCostumerParent = $("#ddCustomer"),
        ddSBTParent = $("#ddSBT"),
        ddPekerjaanParent = $("#ddPekerjaan"),
        ddAssetParent = $("#ddAsset")

    ///** Fill dropdown select */

    //Dropdown Segmen
    await selectSegmen()

    //Dropdown Contract
    await selectContract();

    //Dropdown Customer
    await selectCustomer();

    //Dropdown SBT
    await selectSBT();

    //Dropdown Pekerjaan
    await selectPekerjaan();

    //Dropdown Asset
    await selectAsset();

    $('#Asset').on('change', await onChangeCostCenter);
    $('#Customer').on('change', await onChangeRegional);

    /** Open Modal Add User */
    $('#btnNewProject').click(await openModalProject);

    /** Create New User Modal with validation jquery */
    formProject.submit(function (e) {
        e.preventDefault();
    }).validate({
        rules: {
            NamaProject: "required",
            Probability: "required",
            Sumur: "required",
            ControlProject: "required"
        },
        messages: {
            NamaProject: "is required",
            Probability: "is required",
            Sumur: "is required",
            ControlProject: "is required"
        },
        submitHandler: async function (form) {
            var
                url = !ifEdit ? "/master/project-revenue/create-project-ajax" : "/master/project-revenue/update-project-ajax";
            loadingForm(true, submitProject);
            await asyncAjax(url, "POST", new FormData(form))
                .then(async function successCallBack(response) {
                    console.log(response);
                    if (response.success) {
                        // Reset fill input
                        //formUser[0].reset();
                        swallAllert.Success("Success", response.message);
                        // Rollback data user in DataTable after create new user
                        await Load();
                        modalDialog.modal("hide");
                    } else {
                        console.log(response.message);
                        swallAllert.Error("Oops...!", response.message);
                    }
                    loadingForm(false, submitproject, "Save project!");
                })
                .catch(async function errorCallBack(err) {
                    swallAllert.error("oops...!", "something wrong when login!");
                    loadingform(false, submitproject, "Save project!");
                });

            return false;
        }
    })

    async function Load() {
        /** Fetch data user*/
        await asyncAjax("/master/project-revenue/get-project-list", "POST")
            .then(async function successCallBack(response) {
                if (response.success) {
                    console.log(response.data);
                    await OnSuccess(response.data);
                } else {
                    swallAllert.Error("Fetch Data Failed!", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            })

        /** When success fetch data user and create dataTable */
        async function OnSuccess(listData) {
            var tbl = $('#dataTable').DataTable();
            tbl.destroy();

            var t = $('#dataTable').DataTable({
                width: "100%",
                data: listData,
                columns: [
                    {
                        data: null,
                        defaultContent: "-",
                        render: function (data, type, row, meta) {
                            return meta.row + meta.settings._iDisplayStart + 1;
                        }
                    },
                    { data: 'id', defaultcontent: "", visible: false },
                    { data: 'namaProject', defaultcontent: "" },
                    { data: 'customer', defaultContent: "" },
                    { data: 'asset', defaultContent: "" },
                    /*{ data: 'segmen', defaultContent: "" },*/
                    { data: 'contract', defaultContent: "" },
                    /* { data: 'probability', defaultcontent: "" },
                     { data: 'sumur', defaultContent: "" },
                     { data: 'controlProject', defaultContent: "" },
                     { data: 'pekerjaan', defaultContent: "" },
                     { data: 'sbtIndex', defaultContent: "" },*/
                    {
                        data: null,
                        render: function (data, type, full, meta) {
                            return `<div class="d-flex">
                                            <button type="button" class="btn text-info col btn-link btn-icon moveButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="View">
                                                <i class="bx bx-show"></i>
                                            </button>
                                            <button type="button" class="btn text-warning col btn-link btn-icon editButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="Edit">
                                                <i class="bx bx-edit-alt"></i>
                                            </button>
                                            <button type="button" class="btn text-danger col btn-link btn-icon deleteButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="Delete">
                                                <i class="bx bx-trash"></i>
                                            </button>
                                        </div>`;
                        },
                    }

                ],
                columnDefs: [
                    {
                        searchable: false,
                        targets: 0,
                    },
                    {
                        searchable: false,
                        orderable: false,
                        visible: false,
                        targets: 1,
                    },
                    {
                        className: "text-center",
                        searchable: false,
                        orderable: false,
                        targets: 6,
                    }
                ],
                lengthMenu: [[5, 10, 50, 100, -1], [5, 10, 50, 100, "All"]],
                autoWidth: true,
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                bScrollCollapse: true
            });

            t.on('order.dt search.dt', function () {
                let i = 1;

                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();

            /* Function delete from button delete DataTable */
            $('#dataTable').on('click', '.deleteButton', removeRow);
            async function removeRow() {
                var
                    table = $('#dataTable').DataTable(),
                    data = table.row($(this).parents('tr')).data();
                swallAllert.Confirm.Delete('Are you sure?', "Are You Sure Want to Delete This Project").then(async (result) => {
                    console.log(result)
                    if (result.isConfirmed) {
                        var formData = new FormData();
                        formData.append('idProject', data.id);

                        await asyncAjax("/master/project-revenue/delete-project-ajax", "POST", formData)
                            .then(async function successCallBack(response) {
                                Load();
                            })
                            .catch(async function errorCallBack(err) {
                                console.log("err : ");
                                console.log(err);
                                swallAllert.Error("Fetch Data Failed!", err.data);
                            })
                    }
                })
            }

            /** Open Modal Edit User */
            $('#dataTable').on('click', '.editButton', editRow);
            async function editRow() {
                //swallAllert.Success("Success", "Edit Button");
                var
                    table = $('#dataTable').DataTable(),
                    data = table.row($(this).parents('tr')).data();
                idproject = data.id;
                await openModalProject(data, true);
            }

            /** Open Modal View User */
            $('#dataTable').on('click', '.moveButton', moveRow);
            async function moveRow() {
                var
                    tb = $('#dataTable').DataTable(),
                    dt = tb.row($(this).parents('tr')).data(),
                    urldetail = decodeURIComponent(baseUrl + "/master/project-revenue/getDetail?id=Params"),
                    url = decodeURIComponent(urldetail);
                url = url.replace("Params", window.btoa(dt.id));

                window.location.href = url;
            }

        }
    }

    /**
     * Function open model bergantung pada trigger pada button
     *
     * @param {Object} data => data edit modal user
     * @param {Boolean} status => kondisi modal saat dibuka jika true berarti modal edit
     */
    async function openModalProject(data, status = false) {
        // Kondisi awal saat open modal dialog
        formProject[0].reset(); // Reset fill input
        console.log(data)
        ifEdit = status; // Set status modal berdasarkan parameter yang diterima, secara default itu false
        var title = !ifEdit ? "Add" : "Edit";

        // Jika modal edit
        if (ifEdit) {
            // Fill modal input form edit
            formProject.find('input[name="ID"]').val(data.id);
            formProject.find('input[name="NamaProject"]').val(data.namaProject);
            formProject.find('input[name="Probability"]').val(data.probability);
            formProject.find('input[name="Sumur"]').val(data.sumur);
            formProject.find('input[name="ControlProject"]').val(data.controlProject);
            formProject.find('select[name="Segmen"]').val(data.idSegmen).trigger("change");
            formProject.find('select[name="Asset"]').val(data.idAsset).trigger("change");
            formProject.find('select[name="Customer"]').val(data.idCustomer).trigger("change");
            formProject.find('select[name="Contract"]').val(data.idContract).trigger("change");
            formProject.find('select[name="Pekerjaan"]').val(data.idPekerjaan).trigger("change");
            formProject.find('select[name="SBT"]').val(data.idsbt).trigger("change");
        }
        else {
            formProject.find('select[name="CostCenter"]').val('').trigger('change');
            formProject.find('select[name="Tipeproject"]').val('').trigger('change');
            formProject.find('select[name="CostCenter"]').val('').trigger('change');
            formProject.find('select[name="Tipeproject"]').val('').trigger('change');
            formProject.find('select[name="CostCenter"]').val('').trigger('change');
            formProject.find('select[name="Tipeproject"]').val('').trigger('change');
        }

        $('#submitProject').removeAttr('hidden');

        modalTitle.html(`Modal ${title} Data Project`);
        modalDialog.modal("show");
    }

    async function onChangeRegional() {
        var custRegionalDD = formProject.find('select[name="Customer"]').val();
        if (custRegionalDD != '') {
            await asyncAjax("/master/project-revenue/get-Regional-ajax?IDCust=" + custRegionalDD, "GET")
                .then(async function successCallBack(response) {
                    console.log(response)

                    if (response.data.length > 0) {
                        formProject.find('input[name="Regional"]').val(response.data[0].regional);
                    }
                })
                .catch(async function errorCallBack(err) {
                    console.log("err : ");
                    console.log(err);
                    swallAllert.Error("Fetch Data Failed!", err.data);
                })
        }
    }

    async function onChangeCostCenter() {
        var costCenterDD = formProject.find('select[name="Asset"]').val();
        if (costCenterDD != '') {
            await asyncAjax("/master/project-revenue/get-CostCenterFD-ajax?IDCC=" + costCenterDD, "GET")
                .then(async function successCallBack(response) {
                    console.log(response)
                    if (response.data.length > 0) {
                        formProject.find('input[name="CostCenter"]').val(response.data[0].costCenter);
                    }
                })
                .catch(async function errorCallBack(err) {
                    console.log("err : ");
                    console.log(err);
                    swallAllert.Error("Fetch Data Failed!", err.data);
                })
        }
    }

    async function selectSegmen() {
        await asyncAjax("/master/project-revenue/get-segmenDD-ajax", "GET")
            .then(async function successCallBack(response) {
                console.log(response)
                var data = response.data;
                var opt = '<option></option>';

                for (var i = 0; i < data.length; i++) {
                    opt += '<option value="' + data[i].id + '"> ' + data[i].segmen + '</option>';
                }

                $("#Segmen").html(opt);
                $("#Segmen").select2({
                    placeholder: '- Please select a Segmen -',
                    dropdownParent: ddSegmenParent
                })
            })
            .catch(async function errorCallBack(err) {
                console.log("err : ");
                console.log(err);
                swallAllert.Error("Fetch Data Failed!", err.data);
            })
    }

    async function selectContract() {
        await asyncAjax("/master/project-revenue/get-contractTypeDD-ajax", "GET")
            .then(async function successCallBack(response) {
                var data = response.data;
                var opt = '<option></option>';

                for (var i = 0; i < data.length; i++) {
                    opt += '<option value="' + data[i].id + '"> ' + data[i].contract + '</option>';
                }

                $("#Contract").html(opt);
                $("#Contract").select2({
                    placeholder: '- Please select a Contract -',
                    dropdownParent: ddContractParent
                })
            })
            .catch(async function errorCallBack(err) {
                console.log("err : ");
                console.log(err);
                swallAllert.Error("Fetch Data Failed!", err.data);
            })
    }

    async function selectCustomer() {
        await asyncAjax("/master/project-revenue/get-customerDD-ajax", "GET")
            .then(async function successCallBack(response) {
                console.log(response)
                var data = response.data;
                var opt = '<option></option>';

                for (var i = 0; i < data.length; i++) {
                    opt += '<option value="' + data[i].id + '"> ' + data[i].customer + '</option>';
                }

                $("#Customer").html(opt);
                $("#Customer").select2({
                    placeholder: '- Please select a Customer -',
                    dropdownParent: ddCostumerParent
                })
            })
            .catch(async function errorCallBack(err) {
                console.log("err : ");
                console.log(err);
                swallAllert.Error("Fetch Data Failed!", err.data);
            })
    }

    async function selectSBT() {
        await asyncAjax("/master/project-revenue/get-SBTDD-ajax", "GET")
            .then(async function successCallBack(response) {
                console.log(response)
                var data = response.data;
                var opt = '<option></option>';

                for (var i = 0; i < data.length; i++) {
                    opt += '<option value="' + data[i].id + '"> ' + data[i].sbtIndex + '</option>';
                }

                $("#SBT").html(opt);
                $("#SBT").select2({
                    placeholder: '- Please select a SBT Index -',
                    dropdownParent: ddSBTParent
                })
            })
            .catch(async function errorCallBack(err) {
                console.log("err : ");
                console.log(err);
                swallAllert.Error("Fetch Data Failed!", err.data);
            })
    }

    async function selectPekerjaan() {
        await asyncAjax("/master/project-revenue/get-pekerjaanDD-ajax", "GET")
            .then(async function successCallBack(response) {
                console.log(response)
                var data = response.data;
                var opt = '<option></option>';

                for (var i = 0; i < data.length; i++) {
                    opt += '<option value="' + data[i].id + '"> ' + data[i].pekerjaan + '</option>';
                }

                $("#Pekerjaan").html(opt);
                $("#Pekerjaan").select2({
                    placeholder: '- Please select a Work -',
                    dropdownParent: ddPekerjaanParent
                })
            })
            .catch(async function errorCallBack(err) {
                console.log("err : ");
                console.log(err);
                swallAllert.Error("Fetch Data Failed!", err.data);
            })
    }

    async function selectAsset() {
        await asyncAjax("/master/project-revenue/get-assetDD-ajax", "GET")
            .then(async function successCallBack(response) {
                console.log(response)
                var data = response.data;
                var opt = '<option></option>';

                for (var i = 0; i < data.length; i++) {
                    opt += '<option value="' + data[i].id + '"> ' + data[i].asset + '</option>';
                }

                $("#Asset").html(opt);
                $("#Asset").select2({
                    placeholder: '- Please select a Asset -',
                    dropdownParent: ddAssetParent
                })
            })
            .catch(async function errorCallBack(err) {
                console.log("err : ");
                console.log(err);
                swallAllert.Error("Fetch Data Failed!", err.data);
            })
    }

    await Load();
});