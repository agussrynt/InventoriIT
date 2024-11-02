// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    //var $loading = $('.section-loading'),
    //    $content = $('.layout-wrapper'),
    //    $html = document.querySelector("html");

    $(document).on({
        ajaxstart: function () {
            //$html.style.overflow = "hidden";
            //$loading.show();
            //$content.hide();
            setLodingContent(true);
        },
        ajaxStop: function () {
            setLodingContent(false);
        },
    });

    $("a#logoutButton").click(function () {
        Swal.fire({
            title: 'Are you sure?',
            text: "Want to logout from this page!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, sure!'
        }).then((result) => {
            if (result.isConfirmed) {
                asyncAjax("/account/logout", "POST", null)
                    .then(function (response) {
                        console.log(response);
                        if (response.success) {
                            window.location.href = response.url;
                        } else {
                            swallAllert.Error("Oops", response.message);
                        }
                    })
                    .catch(function (err) {
                        console.log(err);
                        swallAllert.Error("Something wrong!", err);
                    })
            }
        })
    })
});


var
    swallAllert = {
        Success: function (title, message) {
            Swal.fire({
                icon: 'success',
                title: title,
                text: message,
                timer: 3500,
                //confirmButtonColor: '#d33',
            })
        },
        Error: function (title, message) {
            Swal.fire({
                icon: 'error',
                title: title,
                text: message,
                timer: 3500,
                //confirmButtonColor: '#d33',
            })
        },
        ConfirmDelete: function () {
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
                    Swal.fire(
                        'Deleted!',
                        'Your file has been deleted.',
                        'success'
                    )
                }
            })
        },
        ConfirmLogout: function () {
            Swal.fire({
                title: 'Are you sure?',
                text: "Want to logout from this page!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, sure!'
            }).then((result) => {
                if (result.isConfirmed) {
                    Swal.fire(
                        'Deleted!',
                        'Your file has been deleted.',
                        'success'
                    )
                }
            })
        },
        Confirm: {
            Success: function (title, message) {
                return new Promise(function (resolve) {
                    Swal.fire({
                        icon: 'success',
                        title: title,
                        text: message,
                    }).then((result) => {
                        resolve(result);
                    })
                })
            },
            Delete: function (title, message) {
                return new Promise(function (resolve) {
                    Swal.fire({
                        title: title,
                        text: message,
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#d33',
                        cancelButtonColor: '#3085d6',
                        confirmButtonText: 'Yes, delete it!'
                    }).then((result) => {
                        resolve(result);
                    })
                })
            },
            Save: function (title, message) {
                return new Promise(function (resolve) {
                    Swal.fire({
                        title: title,
                        text: message,
                        icon: 'success',
                        showCancelButton: true,
                        confirmButtonColor: '#d33',
                        cancelButtonColor: '#3085d6',
                        confirmButtonText: 'Yes, save it!'
                    }).then((result) => {
                        resolve(result);
                    })
                })
            }
        },
    },
    setLodingContent = function (flag) {
        if (flag) {
            $('#contentPage').addClass('d-none');
            $('#loadingContent').removeClass('d-none');
        } else {
            $('#loadingContent').addClass('d-none');
            $('#contentPage').removeClass('d-none');
        }
    },
    Base64toBlob = function (b64Data, contentType = "application/pdf", sliceSize = 512) {
        const byteCharacters = atob(b64Data);
        const byteArrays = [];

        for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            const slice = byteCharacters.slice(offset, offset + sliceSize);

            const byteNumbers = new Array(slice.length);
            for (let i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }

            const byteArray = new Uint8Array(byteNumbers);
            byteArrays.push(byteArray);
        }

        const blob = new Blob(byteArrays, { type: contentType });
        return blob;
    },
    Base64ToFile = function (fileName, base64String) {
        if (window.navigator && window.navigator.msSaveBlob) {
            const blob = this.Base64toBlob(base64String);
            window.navigator.msSaveBlob(blob, fileName);
        } else {
            const extension = fileName.split('.').pop().toLowerCase(); // Get uploaded file extension  
            const linkSource = "data:application/" + extension + ";base64," + base64String;
            const downloadLink = document.createElement("a");
            downloadLink.href = linkSource;
            downloadLink.download = fileName;
            downloadLink.click();
        }
    },
    abjad = ["a", "b", "c", "d", "e", "f",
        "g", "h", "i", "j", "k", "l", "m",
        "n", "o", "p", "q", "r", "s", "t",
        "u", "v", "w", "x", "y", "z"
    ],
    monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dece"
    ]
    ;

function convertDate(dateTime) {
    const d = new Date(dateTime);
    return d.getDate() + ' ' + monthNames[d.getMonth()] + ' ' + d.getFullYear();
}

function urlAction() {
    var
        url = "",
        splitUrl = currentUrl.split('/');
    splitUrl.pop()

    for (var i = 0; i < splitUrl.length; i++) {
        var el = splitUrl[i];
        if (i == 0) continue;

        url = url + el + "/";
    }
    return url;
}

function setAction() {
    return `<div class="d-flex">
        <button type="button" class="btn text-warning col btn-link btn-icon editButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="Edit">
            <i class="bx bx-edit-alt"></i>
        </button>
        <button type="button" class="btn text-danger col btn-link btn-icon deleteButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="Delete">
            <i class="bx bx-trash"></i>
        </button>
        <button type="button" class="btn text-info col btn-link btn-icon viewButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="View">
            <i class="bx bx-show"></i>
        </button>
    </div>`;
    //return `<div class="d-inline-block text-nowrap">
    //    <button type="button" class="btn btn-sm btn-icon dropdown-toggle hide-arrow" data-bs-toggle="dropdown" aria-expanded="false">
    //        <i class="bx bx-dots-vertical-rounded"></i>
    //    </button>
    //    <div class="dropdown-menu dropdown-menu-end">
    //        <a class="dropdown-item editButton" href="javascript:void(0);" title="Edit"><i class="bx bx-edit-alt me-1"></i> Edit</a>
    //        <a class="dropdown-item deleteButton" href="javascript:void(0);" title="Delete"><i class="bx bx-trash me-1"></i> Delete</a>
    //        <a class="dropdown-item viewButton" href="javascript:void(0);" title="View"><i class="bx bx-vision me-1"></i> View</a>
    //    </div>
    //</div>`
}

function asyncAjax(ajaxurl, method, params, json = false) {
    var
        _config = {
            method: method,
            url: baseUrl + ajaxurl
        };

    if (params) {
        // fd.append("VendorID", params);
        _config.data = params;
        //_config.contentType = !json ? json : "application/json; charset=utf-8";
        _config.contentType = !json ? json : "application/json";
        _config.processData = false;
        if (json)
            _config.dataType = "json";
    };

    return $.ajax(_config);
}

function loadingForm(flag, buttonID, buttonText) {
    let html = `
        <div class="d-flex align-items-center justify-content-center">
            <div class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></div>
            <strong>Loading...</strong>
        </div>
    `
    switch (flag) {
        // if loading
        case true:
            $(`button#${buttonID}`).html(html);
            $(`button#${buttonID}`).attr("disabled", true);
            break;
        // if not loading
        case false:
            $(`button#${buttonID}`).html(buttonText);
            $(`button#${buttonID}`).attr("disabled", false);
            break;
        default:
            $(`button#${buttonID}`).html(buttonText);
            $(`button#${buttonID}`).attr("disabled", true);
            break;
    }
};

function loadingFormAccount(flag, inputID, inputText) {
    let html = `
        <div class="d-flex align-items-center justify-content-center">
            <div class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></div>
            <strong>Loading...</strong>
        </div>
    `
    switch (flag) {
        // if loading
        case true:
            inputID.val(html);
            inputID.attr("disabled", true);
            break;
        // if not loading
        case false:
            inputID.val(buttonText);
            inputID.attr("disabled", false);
            break;
        default:
            inputID.val(inputText);
            inputID.attr("disabled", true);
            break;
    }
};

//function setLodingContent(flag) {
//    debugger
//    if (flag) {
//        //$('#contentPage').hide();
//        $('#contentPage').addClass('d-none');
//        $('#loadingContent').removeClass('d-none');
//        //$('#loadingContent').show();
//    } else {
//        //$('#loadingContent').hide();
//        //$('#contentPage').show();
//        $('#loadingContent').addClass('d-none');
//        $('#contentPage').removeClass('d-none');
//    }
//}

//comment on 031122
async function fillSelect(url, id, elem, idParent) {
    var groups_array = [];

    $.getJSON(url, {},
        function (response) {
            if (!response.success) {
                swallAllert.Error("Fetch Data List!", response.data);
            } else {
                $.each(response.data, function (index) {
                    if (elem) {
                        groups_array.push({
                            id: response.data[index][elem.key],
                            text: response.data[index][elem.value],
                        });
                    } else {
                        let opt = `<option value="${response.data[index].id}" ${index == 0 ? "selected" : ""}>${response.data[index].name}</option>`
                    }
                });

                $("select" + id).select2({
                    placeholder: "Please select one",
                    dropdownParent: idParent,
                    data: groups_array
                });
            }
        }
    );
}


function fillSelect2(elem, url = null) {
    if (url) {
        $.getJSON(
            url,
            {},
            function (response) {
                if (response.success < 1) {
                    swallAllert.Error("Fetch Data List!", response.data);
                } else {
                    $.each(response.data, function (index) {
                        let opt = `<option value="${response.data[index].name}">${response.data[index].name}</option>`
                        elem.append(opt);
                    });
                }
            }
        );
    } else {
        $.each(arrData, function (index) {
            let opt = `<option value="${arrData[index].id}" ${_value == arrData[index].id ? "selected" : ""}>${response.data[index].name}</option>`
            elem.append(opt);
        });
    }
}

var DateFunction = {
    convertDate: function (dateTime) {
        const d = new Date(dateTime);
        return d.getDate() + ' ' + monthNames[d.getMonth()] + ' ' + d.getFullYear();
    },
    dateStrYYYYMMDD: function (dateTime) {
        const d = new Date(dateTime);
        const dt = d.getDate() < 10 ? '0' + d.getDate() : d.getDate();
        const m = (d.getMonth() + 1) < 10 ? '0' + (d.getMonth() + 1) : (d.getMonth() + 1);
        return d.getFullYear() + '-' + m + '-' + dt;
    },
    dateStrMMDDYYYY: function (dateTime) {
        const d = new Date(dateTime);
        const dt = d.getDate() < 10 ? '0' + d.getDate() : d.getDate();
        const m = (d.getMonth() + 1) < 10 ? '0' + (d.getMonth() + 1) : (d.getMonth() + 1);
        return m + '-' + dt + '-' + d.getFullYear();
    },
    dateStrDDMMYYYY: function (dateTime) {
        const d = new Date(dateTime);
        const dt = d.getDate() < 10 ? '0' + d.getDate() : d.getDate();
        const m = (d.getMonth() + 1) < 10 ? '0' + (d.getMonth() + 1) : (d.getMonth() + 1);
        return dt + '/' + m+ '/' + d.getFullYear();
    }
} 