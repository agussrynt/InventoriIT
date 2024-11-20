$(document).ready(async function () {

    //Parent Dropdown
    ddSegmenParent = $("#ddSegmen"),
    ddContractParent = $("#ddContract"),
    ddCostumerParent = $("#ddCustomer"),
    ddSBTParent = $("#ddSBT"),
    ddPekerjaanParent = $("#ddPekerjaan"),
    ddAssetParent = $("#ddAsset")
    ddProjectExist = $("#ddProject")

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

    var projectDD = { key: 'id', value: 'namaProject' };
    await fillSelect(baseUrl + "/PAGE/revenue/get-projectDD-ajax", "#Project", projectDD, ddProjectExist);
    //Get IdHeader
    const urlParams = new URLSearchParams(window.location.search);
    const encodedParam = urlParams.get('idHeader');

    if (!encodedParam) {
        console.error("idHeader tidak ditemukan di URL");
        return;
    }

    const idHeader = window.atob(encodedParam); // Decode Base64
    console.log("Decoded idHeader:", idHeader);
    async function Load(idHeader) {
        var fd = new FormData();
        fd.append('idHeader', idHeader);
        await asyncAjax("/page/revenue/get-project-revenue", "POST", fd)
            .then(async function successCallBack(response) {
     
                console.log(response);
                if (response.success) {
                    console.log(response.data);
                    // Ekstraksi data
                    const listData = response.data.list;
                    const headerEditData = response.data.headerEdit[0];

                    console.log(listData);
                    console.log(headerEditData);
                    await OnSuccess(listData);
                    await formEdit(headerEditData);
                } else {
                    swallAllert.Error("Data ngga ada woy, ", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            });

        /** When success fetch data user and create dataTable */
        async function OnSuccess(listData) {
            console.log(listData)
            $('#dataTable').DataTable({
                width: "100%",
                data: listData,
                columns: [
                    { data: 'idMapping', defaultcontent: "", visible: false },
                    {
                        data: 'segmen',
                        defaultcontent: "",
                        title: "Segmen"
                    },
                    {
                        data: 'asset',
                        defaultContent: "-",
                        title: "Asset"
                    },
                    {
                        data: 'customer',
                        defaultContent: "-",
                        title: "Customer"
                    },
                    {
                        data: 'contract',
                        defaultContent: "-",
                        title: "Contract"
                    },
                    {
                        data: 'pekerjaan',
                        defaultContent: "-",
                        title: "Pekerjaan"
                    },
                    {
                        data: null,
                        defaultContent: "-",
                        title: "Aksi",
                        orderable: false,
                        render: function (data, type, row, meta) {
                            return `<button type="button" class="btn text-info col btn-link btn-icon editButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="Edit">
                                                <i class="bx bx-pen"></i>
                                            </button> | <button type="button" class="btn text-danger col btn-link btn-icon deleteButton" data-bs-toggle="tooltip" data-bs-html="true" data-bs-original-title="Delete">
                                                <i class="bx bx-trash"></i>
                                            </button>`;
                        }
                    }
                ],
                columnDefs: [],
                lengthMenu: [[5, 10, 50, 100, -1], [5, 10, 50, 100, "All"]],
                autoWidth: true,
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                scrollX: true,
                bScrollCollapse: true,
                fixedHeader: true,
                order: [[1, 'desc']],
            });


            //Delete Row
            $('#dataTable').on('click', '.deleteButton', removeRow);
            async function removeRow() {
                var
                    table = $('#dataTable').DataTable(),
                    data = table.row($(this).parents('tr')).data();
                swallAllert.Confirm.Delete('Are you sure?', "Are You Sure Want to Delete This Data").then(async (result) => {
                    console.log(result)
                    if (result.isConfirmed) {
                        var formData = new FormData();
                        formData.append('idProject', data.id);

                        await asyncAjax("/page/revenue/delete-header-ajax", "POST", formData)
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
        }

        async function formEdit(headerDetail) {
            console.log(headerDetail);
            document.getElementById('idHeader').value = headerDetail.id;
            document.getElementById('tahunBerjalan').value = headerDetail.tahun;
            document.getElementById('rjppNextStaRev').value = headerDetail.rjppNextSta;
            document.getElementById('rkapYearStaRev').value = headerDetail.rkapYearSta;
            document.getElementById('prognosaRev').value = headerDetail.prognosa;
            document.getElementById('realisasiBackYearRev').value = headerDetail.realisasiBackYear;
            
        }
    }


    async function submitRevenueData(e) {
        e.preventDefault();

        const headerData = {
            ID: $("input[name='IDHeader']").val(),
            Tahun: $("select[name='tahunBerjalan']").val(),
            RJPPNextSta: $("input[name='rjppNextSta']").val(),
            RKAPYearSta: $("input[name='rkapYearSta']").val(),
            Prognosa: $("input[name='prognosa']").val(),
            RealisasiBackYear: $("input[name='realisasiBackYear']").val(),
        };
        console.log(headerData);

        try {
            const response = await fetch("/page/revenue/create-header-ajax", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(headerData),
            });

            const result = await response.json();
            if (result.success) {
                alert("Data berhasil diubah!");
                location.reload(true);
            } else {
                alert("Gagal mengubah data: " + result.message);
                location.reload(true);
            }
        } catch (error) {
            console.error("Error:", error);
            alert("Terjadi kesalahan saat mengubah data.", error);
        }
    }

    // Event listener untuk submit modal form
    $('.editHeaderRevenue').on('submit', submitRevenueData);

    $('#Project').on('change', onProjectChange);

    async function onProjectChange(e) {
        e.preventDefault();

        const headerData = {
            IDProject: $("select[name='Project']").val(),
        };
        console.log(headerData);

        try {
            const response = await fetch("/page/revenue/create-header-ajax", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(headerData),
            });

            const result = await response.json();
            if (result.success) {
                alert("Data berhasil diubah!");
                location.reload(true);
            } else {
                alert("Gagal mengubah data: " + result.message);
                location.reload(true);
            }
        } catch (error) {
            console.error("Error:", error);
            alert("Terjadi kesalahan saat mengubah data.", error);
        }
    }

    //show modal add new project
    $('#addNewProject').on('click', function () {
        const addProjectExistModal = bootstrap.Modal.getInstance(document.getElementById('addProjectExist'));
        const addNewProjectModal = new bootstrap.Modal(document.getElementById('addNewProject'));

        addProjectExistModal.hide();

        document.getElementById('addProjectExist').addEventListener('hidden.bs.modal', function () {
            addNewProjectModal.show();
        }, { once: true });
    });
    

    await Load(idHeader);
})