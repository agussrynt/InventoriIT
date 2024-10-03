$(document).ready(function () {
    var
        totalUser = $('#totalUser'),
        userLDAP = $('#userLDAP'),
        userSITE = $('#userSITE'),
        activeUser = $('#activeUser'),

        ifEdit = false,
        submitRole = "submitRole",
        formRole = $("form.role-modal")
        modalDialog = $("#roleModal"),
        modalUpDialog = $("#roleModalUpdate"),
        modalViDialog = $("#roleModalView"),
        modalTitle = modalDialog.find("h5.modal-title");

    filterSearch();
    //$('#btnNew').click(await openModal);
    //async function openModal(data, status = false) {

    $("#btnNew").click(function () {
        $('input[name="roleName"]').val('');
        $('input[name="roleId"]').val('');
        openModal();
    });

});

function openModal() {
    modalDialog.modal("show");
}

function filterSearch() {
    $("#dataTable").DataTable({
        lengthMenu: [[5, 10, 50, 100, -1], [5, 10, 50, 100, "All"]],
        autoWidth: true,
        width: "100%",
        paging: true,
        lengthChange: true,
        searching: true,
        ordering: true,
        info: true,
        bScrollCollapse: true,
        order: [[2, 'asc']],
        bDestroy: true,
        ajax: {
            url: baseUrl + "/master/role-management/get-role-list",
            type: "GET",
            contentType: "application/x-www-form-urlencoded",
            dataType: "json	",
            data: {
                //__keywords: $("#_accountKeywords").val(),
            }
        },
        columns: [
            {
                data: null,
                defaultContent: "-",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { data: 'name', defaultContent: "" },
            {
                data: null,
                render: function (data, type, full, meta) {
                    return setAction();
                },
            }
        ], columnDefs: [
            {
                width: "15%",
                targets: 2
            }
        ]
    });

    $('table#dataTable tbody').on('click', '.deleteButton', function () {
        var
            tb = $('table#dataTable').DataTable(),
            dt = tb.row($(this).parents('tr')).data();

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                asyncAjax("/master/role-management/delete-role-ajax", "POST", JSON.stringify({ Id: dt.id }), true)
                    .then(function (response) {
                        console.log(response);
                        if (response.success) {
                            filterSearch();
                            //window.location.href = response.url;
                        } else {
                            swallAllert.Error("Oops", response.message);
                        }
                    })
                    .catch(function (err) {
                        console.log(err);
                        swallAllert.Error("Something wrong!", err);
                    })
            }
        });
    });

    $('table#dataTable tbody').on('click', '.editButton', function () {
        var
            tb = $('table#dataTable').DataTable(),
            dt = tb.row($(this).parents('tr')).data();
        $('input[name="roleNameUp"]').val(dt.name);
        $('input[name="roleIdUp"]').val(dt.id);
        modalUpDialog.modal("show");
    });
    $('table#dataTable tbody').on('click', '.viewButton', function () {

        var
            tb = $('table#dataTable').DataTable(),
            dt = tb.row($(this).parents('tr')).data();
        $('input[name="roleNameVi"]').val(dt.name);
        modalViDialog.modal("show");
    });
}

$('form[name="role-modal"]').submit(function (e) {
    e.preventDefault();
}).validate({
    submitHandler: async function (form) {
        $.ajax({
            url: baseUrl + "/master/role-management/create-role-ajax",
            method: "POST",
            data: JSON.stringify({ Name: $('input[name="roleName"]').val() }),
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            processData: false,
            success: function (result) {
                console.log(result);
                if (result.success) {
                    swallAllert.Success("Success", result.message);
                    $('input[name="roleName"]').val('');
                    modalDialog.modal("hide");
                    filterSearch();
                } else {
                    swallAllert.Error("Failed!", result.message)

                }
            },
            error: function (err) {
                console.log(err);
                swallAllert.Error(500, "Something wrong!");
            }
        });
        return false;
    }
});

$('form[name="role-modal-update"]').submit(function (e) {
    e.preventDefault();
}).validate({
    submitHandler: async function (form) {
        $.ajax({
            url: baseUrl + "/master/role-management/update-role-ajax",
            method: "POST",
            data: JSON.stringify({ Id: $('input[name="roleIdUp"]').val(), Name: $('input[name="roleNameUp"]').val() }),
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            processData: false,
            success: function (result) {
                console.log(result);
                if (result.success) {
                    swallAllert.Success("Success", result.message);
                    $('input[name="roleIdUp"]').val('');
                    $('input[name="roleNameUp"]').val('');
                    modalUpDialog.modal("hide");
                    filterSearch();
                } else {
                    swallAllert.Error("Failed!", result.message)

                }
            },
            error: function (err) {
                console.log(err);
                swallAllert.Error(500, "Something wrong!");
            }
        });
        return false;
    }
});


//document.addEventListener("DOMContentLoaded", function (event) {
//    filterSearch();
//});
