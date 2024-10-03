$(document).ready(async function () {
    /** Initiate variable awal */
    var
        ifEdit = false,
        submitUser = "submitUser",
        formUser = $("form.user-modal"),
        /* userType = formUser.find('select[name="UserType"]'),*/
        userType = formUser.find('input[name="UserType"]'),
        modalDialog = $("#userModal"),
        modalTitle = modalDialog.find("h5.modal-title"),
        ldapChoice = $('#ldapChoice'),
        isLDAP = false,
        idUser = '';

    $('#FungsiId').on('change', await onChangeFungsi);

    ///** Fill dropdown select */
    //await fillSelect2(formUser.find('select[name="Roles"]'), "/master/role-management/get-role-list");
    //$('#Roles').select2();
    var ob = { key: 'id', value: 'name' };
    await fillSelect(baseUrl + "/master/role-management/get-role-list", "#Roles", ob);

    var p = { key: 'iD_FUNGSI', value: 'fungsiname' };
    await fillSelect(baseUrl + "/master/fungsi/get-list-ajax", "#FungsiId", p);

    /** Open Modal Add User */
    $('#btnNew').click(await openModal);

    $("#validateLDAP").on('click', async function () {
        var fd = new FormData();
        var userN = $('#UserName').val()
        fd.append('username', userN);
        loadingForm(true, 'validateLDAP');
        await asyncAjax("/master/user-management/create-validateldap-ajax", "POST", fd)
            .then(function successCallBack(response) {
                console.log(response);
                if (response.success) {
                    var dt = response.data,
                        fName = dt.namaLengkap.split(' ', 1),
                        lName = dt.namaLengkap.substring(fName.toString().length);

                    swallAllert.Success('LDAP Validation', response.message);
                    $('#Email').val(dt.email);
                    $('#Fullname').val(dt.namaLengkap);
                    $('#FirstName').val(fName);
                    $('#LastName').val(lName.trim());
                    $('#EmpNumber').val(dt.empNumber);
                }
                else {
                    swallAllert.Error('Something wrong!', 'User Name tidak sesuai');
                }
                loadingForm(false, 'validateLDAP', 'Validate LDAP');
            })
            .catch(function errorCallBack(err) {
                console.log(err);
                swallAllert.Error('Something wrong!', 'Please check backed!');
                loadingForm(false, 'validateLDAP', 'Validate LDAP');
            });
    });

    /** Create New User Modal with validation jquery */
    formUser.submit(function (e) {
        e.preventDefault();
    }).validate({
        rules: {
            FirstName: "required",
            Email: {
                required: true,
                email: true
            },
            Roles: "required",
            UserType: "required",
            UserName: {
                required: function (el) {
                    return userType.val() == 1;
                }
            },
            Password: {
                required: function (el) {
                    return userType.val() == 1 && !ifEdit;
                },
                minlength: 6
            },
        },
        messages: {
            FirstName: "First name field is required!",
            Email: {
                required: "Email field is required!",
                email: "Please type a valid Email!"
            },
            Role: "Role field is required!",
            UserType: "User type field is required!",
            UserName: {
                required: "Username field is required!"
            },
            Password: {
                required: "Password field is required!",
                minlength: "Password must be at least 6 characters long!"
            }

        },
        submitHandler: async function (form) {
            var
                url = !ifEdit ? "/master/user-management/create-administrator-ajax" : "/master/user-management/edit-administrator-ajax";
            loadingForm(true, submitUser);
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
                    loadingForm(false, submitUser, "Save user!");
                })
                .catch(async function errorCallBack(err) {
                    console.log(err);
                    swallAllert.error("oops...!", "something wrong when login!");
                    loadingform(false, submituser, "save user!");
                });

            return false;
        }
    })

    /** Load dataTable User */
    async function Load() {
        /** Fetch data user*/
        await asyncAjax("/master/user-management/fetch-data-user", "POST")
            .then(async function successCallBack(response) {
                console.log(response)
                if (response.success) {
                    await OnSuccess(response.data);
                } else {
                    swallAllert.Error("Fetch Data Failed!", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                console.log("err : ");
                console.log(err);
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
                        //render: function (data, type, row, meta) {
                        //    return meta.row + meta.settings._iDisplayStart + 1;
                        //}
                    },
                    { data: 'userId', defaultcontent: "", visible: false },
                    { data: 'username', defaultContent: "" },//nama beda
                    { data: 'email', defaultContent: "" },
                    { data: 'fullName', defaultContent: "-" },
                    { data: 'role', defaultContent: "" },
                    { data: 'fungsi', defaultContent: "" },
                    {
                        data: 'isActive',
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
                        data: 'createdAt', visible: false,
                        defaultContent: "-",
                        render: function (data, type, full, meta) {
                            return convertDate(data);
                        }
                    },
                    {
                        data: null,
                        render: function (data, type, full, meta) {
                            if (full.isActive) {
                                return `<div class="d-flex">
                                            <button type="button" class="btn text-warning col btn-link btn-icon editButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="Edit">
                                                <i class="bx bx-edit-alt"></i>
                                            </button>
                                            <button type="button" class="btn text-info col btn-link btn-icon viewButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="View">
                                                <i class="bx bx-show"></i>
                                            </button>
                                            <button type="button" class="btn text-danger col btn-link btn-icon deleteButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="Delete">
                                                <i class="bx bx-trash"></i>
                                            </button>
                                        </div>`;
                            }
                            else {
                                return `<div class="d-flex">
                                        <button type="button" class="btn text-success col btn-link btn-icon activateButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="Edit">
                                            <i class="bx bx-edit"></i>
                                        </button>
                                    </div>`;
                            }
                        },
                    }

                ],
                columnDefs: [
                    {
                        sortable: false,
                        orderable: false,
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
                        targets: 5,
                    },
                    {
                        searchable: false,
                        orderable: false,
                        width: "10%",
                        targets: 8,
                    }
                ],
                lengthMenu: [[5, 10, 50, 100, -1], [5, 10, 50, 100, "All"]],
                autoWidth: true,
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                /*scrollX: true,*/
                bScrollCollapse: true,
                order: [[2, 'asc']]
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

                swallAllert.Confirm.Delete('Are you sure?', "Are You Sure Want to Inactive This User").then(async (result) => {
                    if (result.isConfirmed) {
                        var fd = new FormData();
                        fd.append('UserName', data.username);
                        fd.append('IsActivate', false);

                        await asyncAjax("/master/user-management/activate-ajax", "POST", fd)
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

            $('#dataTable').on('click', '.activateButton', activeteRow);
            async function activeteRow() {
                var
                    table = $('#dataTable').DataTable(),
                    data = table.row($(this).parents('tr')).data();

                swallAllert.Confirm.Save('Are you sure want to activate this user?', "").then(async (result) => {
                    if (result.isConfirmed) {
                        var fd = new FormData();
                        fd.append('UserName', data.username);
                        fd.append('IsActivate', true);

                        await asyncAjax("/master/user-management/activate-ajax", "POST", fd)
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

                idUser = data.idUser;
                await openModal(data, true);
            }

            /** Open Modal View User */
            $('#dataTable').on('click', '.viewButton', viewRow);
            async function viewRow() {
                //swallAllert.Success("Success", "Edit Button");
                var
                    table = $('#dataTable').DataTable(),
                    data = table.row($(this).parents('tr')).data();

                idUser = data.idUser;
                await openModalView(data, true);
            }
        }
    }

    /**
     * Function open model bergantung pada trigger pada button
     * 
     * @param {Object} data => data edit modal user
     * @param {Boolean} status => kondisi modal saat dibuka jika true berarti modal edit
     */
    async function openModal(data, status = false) {
        // Kondisi awal saat open modal dialog
        formUser[0].reset(); // Reset fill input
        formUser.find('select[name="Roles"]').val('').trigger('change');

        ifEdit = status; // Set status modal berdasarkan parameter yang diterima, secara default itu false
        !ifEdit ? $("#passwordInp").show() : $("#passwordInp").hide(); // Menampilkan input password
        var title = !ifEdit ? "Add" : "Edit";

        // Jika modal edit
        if (ifEdit) {
            var
                nameSplit = data.fullName.split(" "),
                FirstName = nameSplit[0],
                LastName = "";

            for (var i = 0; i < nameSplit.length; i++) {
                if (i + 1 < nameSplit.length) {
                    LastName += nameSplit[i + 1] + " ";
                };
            }

            var arrRole = data.roleId.split(',');
            var selectedValues = new Array();
            for (var i = 0; i < arrRole.length; i++) {
                selectedValues[i] = arrRole[i];
            }
            console.log(selectedValues)
            // Fill modal input form edit
            formUser.find('input[name="FirstName"]').val(FirstName);
            formUser.find('input[name="LastName"]').val(LastName);
            formUser.find('input[name="Email"]').val(data.email);
            formUser.find('input[name="IdUser"]').val(data.userId);
            formUser.find('input[name="UserName"]').val(data.username);
            formUser.find('input[name="IdProfile"]').val(data.id);
            //formUser.find('select[name="Roles"]').val(data.role).trigger("change");
            formUser.find('select[name="Roles"]').val(selectedValues).trigger("change");
            formUser.find('select[name="UserType"]').val(data.userType).trigger("change");
            formUser.find('select[name="FungsiId"]').val(data.fungsiId).trigger("change");



            formUser.find('input[name="UserName"]').attr('readonly', 'readonly');
            $('.rowValidateLDAP').attr('hidden', 'hidden');
        }
        else {
            formUser.find('select[name="FungsiId"]').val('').trigger('change');
            formUser.find('input[name="UserName"]').removeAttr('readonly');
            $('.rowValidateLDAP').removeAttr('hidden');
        }

        $('#submitUser').removeAttr('hidden');

        modalTitle.html(`${title} User`);
        modalDialog.modal("show");
    }

    async function openModalView(data, status = false) {
        // Kondisi awal saat open modal dialog
        formUser[0].reset(); // Reset fill input
        formUser.find('select[name="Roles"]').val('').trigger('change');

        ifEdit = status; // Set status modal berdasarkan parameter yang diterima, secara default itu false
        !ifEdit ? $("#passwordInp").show() : $("#passwordInp").hide(); // Menampilkan input password
        var title = !ifEdit ? "Add" : "Edit";

        // Jika modal edit
        if (ifEdit) {
            var
                nameSplit = data.fullName.split(" "),
                FirstName = nameSplit[0],
                LastName = "";

            for (var i = 0; i < nameSplit.length; i++) {
                if (i + 1 < nameSplit.length) {
                    LastName += nameSplit[i + 1] + " ";
                };
            }

            var arrRole = data.roleId.split(',');
            var selectedValues = new Array();
            for (var i = 0; i < arrRole.length; i++) {
                selectedValues[i] = arrRole[i];
            }
            console.log(selectedValues)
            // Fill modal input form edit
            formUser.find('input[name="FirstName"]').val(FirstName);
            formUser.find('input[name="LastName"]').val(LastName);
            formUser.find('input[name="Email"]').val(data.email);
            formUser.find('input[name="IdUser"]').val(data.userId);
            formUser.find('input[name="UserName"]').val(data.username);
            formUser.find('input[name="IdProfile"]').val(data.id);
            //formUser.find('select[name="Roles"]').val(data.role).trigger("change");
            formUser.find('select[name="Roles"]').val(selectedValues).trigger("change");
            formUser.find('select[name="UserType"]').val(data.userType).trigger("change");
            formUser.find('select[name="FungsiId"]').val(data.fungsiId).trigger("change");



            formUser.find('input[name="UserName"]').attr('readonly', 'readonly');
            $('.rowValidateLDAP').attr('hidden', 'hidden');
        }

        $('.rowValidateLDAP').attr('hidden', 'hidden');
        $('#submitUser').attr('hidden', 'hidden');

        modalTitle.html(`${title} User`);
        modalDialog.modal("show");

    }

    async function onChangeFungsi() {
        var idFungsi = formUser.find('select[name="FungsiId"]').val();
        if (idFungsi != '') {
            await asyncAjax("/master/fungsi/get-item-ajax?id=" + idFungsi, "GET")
                .then(async function successCallBack(response) {
                    console.log(response)
                    if (response.success) {
                        formUser.find('input[name="Directorate"]').val(response.data.directoratename);

                    } else {
                        swallAllert.Error("Fetch Data Failed!", response.data);
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

    userType.prop('checked')
});