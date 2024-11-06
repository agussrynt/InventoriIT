$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/Revenue/GetHeaderRevenue",
            "type": "GET",
            "dataSrc": function (json) {
                if (json.success) {
                    return json.data;
                } else {
                    alert(json.message);
                    return [];
                }
            }
        },
        "columns": [
            { "data": "tahun" },
            { "data": "rjppNextSta" },
            { "data": "rkapYearSta" },
            { "data": "prognosa" },
            { "data": "realisasiBackYear" },
            { "data": "totalProject", "defaultContent": "0" },  // Sesuaikan jika ada field ini
            {
                "data": null,
                "render": function (data, type, row) {
                    return `<button class='btn btn-primary'>Edit</button>`;
                }
            }
        ]
    });
});
