$(document).ready(async function () {
    /** Initiate variable awal */
    var
        ifEdit = false,
        submitAsset = "submitAsset",
        formAsset = $("form.asset-modal"),
        modalDialog = $("#assetModal"),
        modalTitle = modalDialog.find("h4.modal-title"),
        idAsset = ''

    ///** Fill dropdown select */
    var costCenterDD = { key: 'fundsCenter', value: 'fundsCenter' };
    await fillSelect(baseUrl + "/master/asset-revenue/get-CostCenterDD-ajax", "#CostCenter", costCenterDD, modalDialog);

    var assetTypeDD = { key: 'id', value: 'tipeAsset' };
    await fillSelect(baseUrl + "/master/asset-revenue/get-assetType-ajax", "#TipeAsset", assetTypeDD, modalDialog);

    /** Open Modal Add User */
    $('#btnNewAsset').click(await openModalAsset);

    /** Create New User Modal with validation jquery */
    formAsset.submit(function (e) {
        e.preventDefault();
    }).validate({
        rules: {
            Asset: "required",
            Keterangan: "required",
        },
        messages: {
            Asset: "Asset / Alat is required!",
            Keterangan: "Keterangan Asset is required!",
        },
        submitHandler: async function (form) {
            var
                url = !ifEdit ? "/master/asset-revenue/create-asset-ajax" : "/master/asset-revenue/update-asset-ajax";
            loadingForm(true, submitAsset);
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
                    loadingForm(false, submitAsset, "Save Asset!");
                })
                .catch(async function errorCallBack(err) {
                    swallAllert.error("oops...!", "something wrong when login!");
                    loadingform(false, submitAsset, "Save Asset!");
                });

            return false;
        }
    })

    async function Load() {
        /** Fetch data user*/
        await asyncAjax("/master/asset-revenue/get-asset-list", "POST")
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
                    { data: 'asset', defaultcontent: "" },
                    { data: 'costCenter', defaultContent: "" },
                    { data: 'keterangan', defaultContent: "" },
                    { data: 'tipeAsset', defaultContent: "" },
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
                        targets: 3,
                    },
                    {
                        className: "text-center",
                        searchable: false,
                        orderable: false,
                        targets: 5,
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
                swallAllert.Confirm.Delete('Are you sure?', "Are You Sure Want to Delete This Asset").then(async (result) => {
                    console.log(result)
                    if (result.isConfirmed) {
                        var formData = new FormData();
                        formData.append('IdAsset', data.id);

                        await asyncAjax("/master/asset-revenue/delete-asset-ajax", "POST", formData)
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
                idAsset = data.id;
                await openModalAsset(data, true);
            }
        }
    }

    /**
     * Function open model bergantung pada trigger pada button
     *
     * @param {Object} data => data edit modal user
     * @param {Boolean} status => kondisi modal saat dibuka jika true berarti modal edit
     */
    async function openModalAsset(data, status = false) {
        // Kondisi awal saat open modal dialog
        formAsset[0].reset(); // Reset fill input
        console.log(data)

        ifEdit = status; // Set status modal berdasarkan parameter yang diterima, secara default itu false
        var title = !ifEdit ? "Add" : "Edit";

        // Jika modal edit
        if (ifEdit) {
            // Fill modal input form edit
            formAsset.find('input[name="ID"]').val(data.id);
            formAsset.find('input[name="Asset"]').val(data.asset);
            formAsset.find('textarea[name="Keterangan"]').val(data.keterangan);
            formAsset.find('select[name="CostCenter"]').val(data.costCenter).trigger("change");
            formAsset.find('select[name="TipeAsset"]').val(data.idAssetType).trigger("change");
        }
        else {
            formAsset.find('select[name="CostCenter"]').val('').trigger('change');
            formAsset.find('select[name="TipeAsset"]').val('').trigger('change');
        }

        $('#submitAsset').removeAttr('hidden');

        modalTitle.html(`Modal ${title} Data Asset`);
        modalDialog.modal("show");
    }

    await Load();
});