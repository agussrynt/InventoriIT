var gdata,
    mapping = new Array;

async function UploadSelectedExcelsheet() {
    var
        i = 0,
        fd = new FormData(),
        file = $("#formFile").get(0).files[0];
    if (file != undefined)
        fd.append("FileUpload", file);



    //await asyncAjax("upload-ajax", "POST", fd)
    await asyncAjax("/page/master-kertas-kerja/upload-ajax", "POST", fd)
        .then(function successCallBack(response) {
            console.log(response);
            loadIndicator(response.data.indicator);
            loadFukup(response.data.fuk);
            if (response.data.status == true) {
                $("#btnSubmit").show();
            }                
        })
        .catch(function errorCallBack() {
            swallAllert.Error("Error!", "Something wrong!")
        })
}

function loadIndicator(indicator) {
    $('#dataTableIndikatorParameter').DataTable({
        data: indicator,
        columns: [
            {
                data: null,
                defaultContent: "-",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "indikatorCategory" },
            { "data": "noParameter" },
            { "data": "parameter" },
            { "data": "bobot" }
        ]
    });
}

function loadFukup(fukup) {
    $('#dataTableFukUp').DataTable({
        data: fukup,
        columns: [
            {
                data: null,
                defaultContent: "-",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "noParameter" },
            { "data": "seq" },
            { "data": "fuk" },
            { "data": "up" }
        ]
    });
}

$("#btnSubmit").click(function () {
    submitData()
});

async function submitData() {
    var
        i = 0,
        fd = new FormData(),
        file = $("#formFile").get(0).files[0];
    if (file != undefined)
        fd.append("FileUpload", file);
    //await asyncAjax("upload-ajax", "POST", fd)
    await asyncAjax("/page/master-kertas-kerja/new-upload-ajax", "POST", fd)
        .then(function successCallBack(response) {
            console.log(response);
            if (response.success) {
                swallAllert.Success("Success", response.message);
                location.reload();
            } else {
                swallAllert.Error("Error!", response.message);
            }
        })
        .catch(function errorCallBack() {
            swallAllert.Error("Error!", "Something wrong!")
        })
}
/*
  $("#validateLDAP").click(function () {
        $.ajax({
            url: "/master/user-management/on-list-api-pdsi-ajax",
            type: 'GET',
            cache: false,
            success: function (response) {
                //console.log(response);
   
            }
        });
    });
 */

$(document).ready(function () {
    $('#btnSubmit').hide();
    $("#btnUploadFile").click(function () {
        if ($("#formFile").val() != "") {
            var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.xlsx|.xls)$/;
            if (!regex.test($("#formFile").val().toLowerCase())) {
                alert("Please upload a valid Excel file!");
                return false;
            }
            else {
                UploadSelectedExcelsheet();
            }
        }
        else {
            alert("Please upload a Excel file!");
            return false;
        }
    });
    /*
            url: "/master/role-management/update-role-ajax",
            method: "POST",
            data: JSON.stringify({ Id: $('input[name="roleIdUp"]').val(), Name: $('input[name="roleNameUp"]').val() }),
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            processData: false,
           
        $("#validateLDAP").click(function () {
        $.ajax({
            url: "/master/user-management/on-list-api-pdsi-ajax",
            type: 'GET',
            cache: false,
            success: function (response) {
                //console.log(response);
            }
        });
    });
     */
    //$("#btnPreview").click(function () {
    //    var _year = $("#Year").val()
    //    if (_year == null) {
    //        alert("Please select year!")
    //    } else {
    //        $.ajax({
    //            url: "/page/master-kertas-kerja/cloning-data",
    //            type: 'GET',
    //            //url: '@Url.Action("master-kertas-kerja", "cloning-data")' + _year,
    //            //method: 'POST',
    //            cache: false,
    //            //data: { year: _year },
    //            //data: JSON.stringify({ Year: _year }),
    //            //dataType: "json",
    //            success: function (response) {
    //                console.log(response);
    //            }
    //        });
    //    }
        
    //});
});