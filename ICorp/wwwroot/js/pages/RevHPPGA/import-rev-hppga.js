
var gdata,
    importRevHPP = "btnSubmit",
    mapping = new Array;
/*var
    get_url = decodeURI(window.location.href),
    urlNew = new URL(get_url),
    paramUrl = urlNew.search.substring(urlNew.search.indexOf("?") + 1),
    splitParam = paramUrl.split("="),
    Param = window.atob(splitParam[1]);*/

var
    get_url = decodeURI(window.location.href);

// Validasi apakah get_url kosong
if (get_url.trim() === "") {
    console.warn("URL kosong. Proses dihentikan.");
} else {
    try {
        var urlNew = new URL(get_url),
            paramUrl = urlNew.search.substring(urlNew.search.indexOf("?") + 1),
            splitParam = paramUrl.split("=");
        // Validasi apakah ada parameter dalam URL
        if (splitParam.length > 1 && splitParam[1].trim() !== "") {
            var Param = window.atob(splitParam[1]);
        }
    } catch (error) {
        console.error("Terjadi kesalahan dalam pengolahan URL:", error);
    }
}

async function UploadSelectedExcelsheet() {
    var
        i = 0,
        fd = new FormData(),
        file = $("#formFile").get(0).files[0];
    if (file != undefined)
        fd.append("FileUpload", file);

    await asyncAjax("/page/revhppga/upload-ajax", "POST", fd)
        .then(function successCallBack(response) {
            //Load Data to View in Table
            loadRevenue(response.data.revenue);
            loadHPP(response.data.hpp);
            loadDetailRevHPP(response.data.detail);

            if (response.data.status == true && Param === undefined) {
                $("#btnSubmit").show();
                $("#btnEdit").hide();
            } else if (response.data.status == true && Param != undefined) {
                $("#btnEdit").show();
                $("#btnSubmit").hide();
            }
        })
        .catch(function errorCallBack(response) {
            console.log(response)
            swallAllert.Error("Error!", "Periksa Data Excell yang di Upload")
        })
}

function loadRevenue(rev) {
    $('#dataTableRevenue').DataTable({
        data: rev,
        columns: [
            {
                data: null,
                defaultContent: "-",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "tahun" },
            { "data": "revenue" }
        ]
    });
}

function loadHPP(hpp) {
    $('#dataTableHPP').DataTable({
        data: hpp,
        columns: [
            {
                data: null,
                defaultContent: "-",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "tahun" },
            { "data": "hpp" }
        ]
    });
}

function loadDetailRevHPP(revHpp) {
    $('#dataTableDetail').DataTable({
        data: revHpp,
        columns: [
            {
                data: null,
                defaultContent: "-",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "segmentRJPP" },
            { "data": "namaCostCenter" },
            { "data": "hp" },
            { "data": "uniqueCode" },
            { "data": "pic" },
            { "data": "costumer" },
            { "data": "project" },
            { "data": "hppSales" },
            { "data": "gaSales" },
            /*{ "data": "ownership" },
            { "data": "probability" },*/
            /*{ "data": "tahun" }*/
        ]
    });
}

$("#btnSubmit").click(function () {
    submitData()
});

$("#btnEdit").click(function () {
    submitData()
});

async function submitData() {
    var
        i = 0,
        fd = new FormData(),
        file = $("#formFile").get(0).files[0];
    if (file != undefined)
        fd.append("FileUpload", file);

    if (Param != undefined)
        fd.append("IDRev", Param)
    loadingForm(true, importRevHPP);

    await asyncAjax("/page/revhppga/new-upload-ajax", "POST", fd)
        .then(function successCallBack(response) {
            if (response.success) {
                window.history.back();
                swallAllert.Success("Success", response.message);
            } else {
                swallAllert.Error("Error!", response.message);
            }
            loadingForm(false, importRevHPP, "Import RevHPPGA!");
        })
        .catch(function errorCallBack() {
            swallAllert.Error("Error!", "Something wrong!")
            loadingForm(false, importRevHPP, "Import RevHPPGA!");
        })
}

$(document).ready(function () {
    $('#btnSubmit').hide();
    $("#btnEdit").hide();

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
});