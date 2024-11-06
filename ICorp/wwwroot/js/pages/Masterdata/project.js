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
    var segmenDD = { key: 'id', value: 'segmen' };
    await fillSelect(baseUrl + "/master/project-revenue/get-segmenDD-ajax", "#Segmen", segmenDD, ddSegmenParent);

    var contractDD = { key: 'id', value: 'contract' };
    await fillSelect(baseUrl + "/master/project-revenue/get-contractTypeDD-ajax", "#Contract", contractDD, ddContractParent);

    var custumerDD = { key: 'id', value: 'customer' };
    await fillSelect(baseUrl + "/master/project-revenue/get-customerDD-ajax", "#Customer", custumerDD, ddCostumerParent);

    var SBTDD = { key: 'id', value: 'sbtIndex' };
    await fillSelect(baseUrl + "/master/project-revenue/get-SBTDD-ajax", "#SBT", SBTDD, ddSBTParent);

    var pekerjaanDD = { key: 'id', value: 'pekerjaan' };
    await fillSelect(baseUrl + "/master/project-revenue/get-pekerjaanDD-ajax", "#Pekerjaan", pekerjaanDD, ddPekerjaanParent);

    var assetDD = { key: 'id', value: 'asset' };
    await fillSelect(baseUrl + "/master/project-revenue/get-assetDD-ajax", "#Asset", assetDD, ddAssetParent);

    //$('#CostCenter').on('change', await onChangeCostCenter);

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
                    { data: 'segmen', defaultContent: "" },
                    { data: 'asset', defaultContent: "" },
                    { data: 'customer', defaultContent: "" },
                    { data: 'contract', defaultContent: "" },
                    { data: 'probability', defaultcontent: "" },
                    { data: 'sumur', defaultContent: "" },
                    { data: 'controlProject', defaultContent: "" },
                    { data: 'pekerjaan', defaultContent: "" },
                    { data: 'sbtIndex', defaultContent: "" },
                    {
                        data: null,
                        render: function (data, type, full, meta) {
                            return `<div class="d-flex">
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
                        targets: 12,
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

    async function onChangeCostCenter() {
        var costCenterDD = formProject.find('select[name="CostCenter"]').val();
        if (costCenterDD != '') {
            await asyncAjax("/master/project-revenue/get-CostCenter-ajax?FundsCenter=" + costCenterDD, "GET")
                .then(async function successCallBack(response) {
                    if (response.data.length > 0) {
                        formProject.find('input[name="project"]').val(response.data[0].name);
                    }
                })
                .catch(async function errorCallBack(err) {
                    console.log("err : ");
                    console.log(err);
                    swallAllert.Error("Fetch Data Failed!", err.data);
                })
        }
    }

    await Load();
});