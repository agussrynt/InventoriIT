$(document).ready(function () {
    $("#btnModalUploadFile").click(function () {
        $("#uploadFileModal").modal("show");
    });

    $("#uploadFileForm").submit(function (event) {
        event.preventDefault(); // Mencegah pengiriman form secara otomatis
    }).validate({
        submitHandler: function (form) {
            var submitFile = $("#submitFile").prop("files")[0];

            if (submitFile === undefined) {
                swal.fire("Warning!", "Please input the Excel file to be uploaded!", "warning");
                return false;
            }

            var fileSize = submitFile.size;
            var ext = submitFile.name
                .substring(submitFile.name.lastIndexOf(".") + 1)
                .toLowerCase();

            if (fileSize > 5000000) { // 5MB = 5 * 1024 * 1024 bytes
                swal.fire("Warning!", "File size is too large! Maximum Limit 5MB.", "warning");
                return false;
            }

            if (ext != "xlsx" && ext != "xls") {
                swal.fire("Warning!", "Wrong file format! Please upload files with the extension .xlsx or .xls", "warning");
                return false;
            }

            var formData = new FormData($("#uploadFileForm")[0]);

            $(".loader").show();
            $.ajax({
                url: baseUrl + "/page/revhppga/upload-file-revhppga",
                method: "POST",
                data: formData,
                dataType: "json",
                contentType: false,
                processData: false,
                success: function (data) {
                    $(".loader").hide();
                    if (data.status == "success") {
                        swal.fire("Success!", data.message, data.alert).then(
                            function () {
                                location.reload();
                            }
                        );
                    } else {
                        swal.fire("Warning!", data.message, data.alert);
                    }
                },
                error: function (data) {
                    $(".loader").hide();
                    console.error("Error response:", data);
                    swal.fire("Error!", "Something went wrong! Status: " + data.status + ", Response: " + data.responseText, "error");
                },
            });
            return false;
        }
    });



});
