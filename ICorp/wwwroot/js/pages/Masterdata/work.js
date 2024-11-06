$(document).ready(async function () {
    /** Initiate variable awal */
    var
        ifEdit = false,
        submitWork = "submitWork",
        formWork = $("form.work-modal"),
        modalDialog = $("#workModal"),
        modalTitle = modalDialog.find("h4.modal-title"),
        idWork = ''

    /** Open Modal Add User */
    $('#btnNewWork').click(await openModalwork);

    /** Create New User Modal with validation jquery */
    formWork.submit(function (e) {
        e.preventDefault();
    }).validate({
        rules: {
            Pekerjaan: "required",
        },
        messages: {
            Pekerjaan: "Work is required!"
        },
        submitHandler: async function (form) {
            var
                url = !ifEdit ? "/master/works/create-work-ajax" : "/master/works/update-work-ajax";
            loadingForm(true, submitWork);
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
                    loadingForm(false, submitWork, "Save work!");
                })
                .catch(async function errorCallBack(err) {
                    swallAllert.error("oops...!", "something wrong when login!");
                    loadingform(false, submitWork, "Save work!");
                });

            return false;
        }
    })

    async function Load() {
        /** Fetch data user*/
        await asyncAjax("/master/works/get-work-list", "POST")
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
                    { data: 'pekerjaan', defaultcontent: "" },
                    {
                        data: 'isAktif',
                        defaultContent: "-",
                        render: function (data, type, full, meta) {
                            var color = 'bg-label-success',
                                label = 'Active';

                            if (data == 0) {
                                color = 'bg-label-danger';
                                label = 'InActive';
                            };

                            return `<span class='badge ${color} me-1'>${label}</span>`;
                        }
                    },
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
                        targets: 4,
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
                swallAllert.Confirm.Delete('Are you sure?', "Are You Sure Want to Delete This Data").then(async (result) => {
                    console.log(result)
                    if (result.isConfirmed) {
                        var formData = new FormData();
                        formData.append('IdWork', data.id);

                        await asyncAjax("/master/works/delete-work-ajax", "POST", formData)
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
                idWork = data.id;
                await openModalwork(data, true);
            }
        }
    }

    /**
     * Function open model bergantung pada trigger pada button
     *
     * @param {Object} data => data edit modal user
     * @param {Boolean} status => kondisi modal saat dibuka jika true berarti modal edit
     */
    async function openModalwork(data, status = false) {
        // Kondisi awal saat open modal dialog
        formWork[0].reset(); // Reset fill input

        ifEdit = status; // Set status modal berdasarkan parameter yang diterima, secara default itu false
        var title = !ifEdit ? "Add" : "Edit";

        // Jika modal edit
        if (ifEdit) {
            // Fill modal input form edit
            formWork.find('input[name="ID"]').val(data.id);
            formWork.find('input[name="Pekerjaan"]').val(data.pekerjaan);
        }

        $('#submitWork').removeAttr('hidden');

        modalTitle.html(`Modal ${title} Data Pekerjaan`);
        modalDialog.modal("show");
    }

    await Load();
});