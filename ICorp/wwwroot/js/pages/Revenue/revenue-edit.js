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

    async function submitMappingProjectExist(e) {
        e.preventDefault();

        const headerData = {
            IDProject: $("select[name='projectExist']").val(),
            IDHeader: idHeader
        };
        console.log(headerData);

        try {
            const response = await fetch("/page/revenue/create-mapping-project", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(headerData),
            });

            const result = await response.json();
            if (result.success) {
                alert("Data berhasil disimpan!");
            } else {
                alert("Gagal menyimpan data: " + result.message);
            }
        } catch (error) {
            console.error("Error:", error);
            alert("Terjadi kesalahan saat menyimpan data.", error);
        }
    }

    async function submitMappingProject(e) {
        e.preventDefault();

        const headerData = {
            IDHeader: idHeader,
            Segmen: $("select[name='Segmen']").val(),
            NamaProject: $("input[name='NamaProject']").val(),
            Asset: $("select[name='Asset']").val(),
            Customer: $("select[name='Customer']").val(),
            Contract: $("select[name='Contract']").val(),
            Probability: $("input[name='Probability']").val(),
            Sumur: $("input[name='Sumur']").val(),
            ControlProject: $("input[name='ControlProject']").val(),
            Pekerjaan: $("select[name='Pekerjaan']").val(),
            SBT: $("select[name='SBT']").val(),
        };
        console.log(headerData);

        try {
            const response = await fetch("/page/revenue/create-mapping-project", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(headerData),
            });

            const result = await response.json();
            if (result.success) {
                alert("Data berhasil disimpan!");
            } else {
                alert("Gagal menyimpan data: " + result.message);
            }
        } catch (error) {
            console.error("Error:", error);
            alert("Terjadi kesalahan saat menyimpan data.", error);
        }
    }

    // Event listener untuk submit modal form
    $('.addProjectExist-modal').on('submit', submitMappingProjectExist);
    $('.addProject-modal').on('submit', submitMappingProject);

    $('#Project').on('change', onProjectChange);
    async function onProjectChange() {
        ProjectID = $("select[name='projectExist']").val()
        var fd = new FormData();
        fd.append('ProjecTID', ProjectID);
            await asyncAjax("/page/revenue/get-project-exist", "POST", fd)
            .then(async function successCallBack(response) {

                console.log(response);
                if (response.success) {
                    console.log(response.data);
                    await fillFormProjectExist(response.data);
                } else {
                    swallAllert.Error("Data ngga ada woy, ", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            });

        async function fillFormProjectExist(listProjectExist) {
            const segmenDropdown = document.getElementById('SegmenExist');
            const assetDropdown = document.getElementById('AssetExist');
            const customerDropdown = document.getElementById('CustomerExist');
            const contractDropdown = document.getElementById('ContractExist');
            const pekerjaanDropdown = document.getElementById('PekerjaanExist');
            const sbtDropdown = document.getElementById('SBTExist');
            const costCenterInput = document.getElementById('CostCenterExist');
            const regionalInput = document.getElementById('RegionalExist');
            const probabilityInput = document.getElementById('ProbabilityExist');
            const sumurInput = document.getElementById('SumurExist');
            const controlProjectInput = document.getElementById('ControlProjectExist');

            // Kosongkan opsi sebelumnya
            segmenDropdown.innerHTML = '';
            customerDropdown.innerHTML = '';
            contractDropdown.innerHTML = '';
            pekerjaanDropdown.innerHTML = '';
            sbtDropdown.innerHTML = '';
            assetDropdown.innerHTML = '';


            // Tambahkan opsi baru ke dropdown
            if (listProjectExist && listProjectExist.length > 0) {
                const project = listProjectExist[0]; // Ambil proyek pertama (sesuai struktur respons)

                // Tambahkan opsi baru ke dropdown segmen
                let option = document.createElement('option');
                option.value = project.idSegmen; 
                option.textContent = project.segmen;
                segmenDropdown.appendChild(option);
                segmenDropdown.value = project.idSegmen;

                //Isi Dropdown asset
                let assetOption = document.createElement('option');
                assetOption.value = project.idAsset;
                assetOption.textContent = project.asset;
                assetDropdown.appendChild(assetOption);
                assetDropdown.value = project.idAsset;

                // Isi dropdown Customer
                let customerOption = document.createElement('option');
                customerOption.value = project.idCustomer; // Gunakan idCustomer sebagai value
                customerOption.textContent = project.customer; // Gunakan customer sebagai teks
                customerDropdown.appendChild(customerOption);
                customerDropdown.value = project.idCustomer;

                // Isi dropdown Contract
                let contractOption = document.createElement('option');
                contractOption.value = project.idContract; // Gunakan idCustomer sebagai value
                contractOption.textContent = project.contract; // Gunakan customer sebagai teks
                contractDropdown.appendChild(contractOption);
                contractDropdown.value = project.idContract;

                //Isi dropdown Pekerjaan
                let pekerjaanOption = document.createElement('option');
                pekerjaanOption.value = project.idPekerjaan;
                pekerjaanOption.textContent = project.pekerjaan;
                pekerjaanDropdown.appendChild(pekerjaanOption);
                pekerjaanDropdown.value = project.idPekerjaan;

                //iSI Dropdown SBT
                let sbtOption = document.createElement('option');
                sbtOption.value = project.idsbt;
                sbtOption.textContent = project.sbtIndex;
                sbtDropdown.appendChild(sbtOption);
                sbtDropdown.value = project.idsbt;


                // Isi input Cost Center
                costCenterInput.value = project.costCenter || '';
                regionalInput.value = project.regional || '';
                probabilityInput.value = project.probability || '';
                sumurInput.value = project.sumur || '';
                controlProjectInput.value = project.controlProject || '';
            }
            else {
                // Jika tidak ada data proyek
                let option = document.createElement('option');
                option.value = '';
                option.textContent = '-- Tidak ada data segmen --';
                segmenDropdown.appendChild(option);

                let assetOption = document.createElement('option');
                assetOption.value = '';
                assetOption.textContent = '-- Tidak ada data Asset --';
                assetDropdown.appendChild(assetOption);

                let customerDefault = document.createElement('option');
                customerDefault.value = '';
                customerDefault.textContent = '-- Tidak ada data customer --';
                customerDropdown.appendChild(customerDefault);

                // Isi dropdown Contract
                let contractDefualt = document.createElement('option');
                contractDefualt.value = ''; 
                contractDefualt.textContent = ' -- Tidak ada data Contract --'; 
                contractDropdown.appendChild(contractDefualt);

                //Isi dropdown Pekerjaan
                let pekerjaanDefault = document.createElement('option');
                pekerjaanDefault.value = '';
                pekerjaanDefault.textContent = ' -- Tidak ada data Pekerjaan -- ';
                pekerjaanDropdown.appendChild(pekerjaanDefault);
                

                //iSI Dropdown SBT
                let sbtDefault = document.createElement('option');
                sbtDefault.velue = '';
                sbtDefault.textContent = ' -- Tidak ada data SBT -- ';
                sbtDropdown.appendChild(sbtOption);
                
                // Kosongkan input Cost Center
                costCenterInput.value = '';
                regionalInput.value = '';
                probabilityInput.value = '';
                sumurInput.value = '';
                controlProjectInput.value = '';
            }
        }
    }

    $('#Asset').on('change', onAssetChange);
    async function onAssetChange() {
        AssetID = $("select[name='Asset']").val()
        var fd = new FormData();
        fd.append('AssetID', AssetID);
        await asyncAjax("/page/revenue/get-costcenter-fill", "POST", fd)
            .then(async function successCallBack(response) {

                console.log(response);
                if (response.success) {
                    console.log(response.data);
                    if (!response.data) {
                        var balikan = "Cost Center Tidak ditemukan";
                        await fillCostCenter(balikan);
                    } else {
                        await fillCostCenter(response.data);
                    }
                   
                } else {
                    swallAllert.Error("Data ngga ada woy, ", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            });

        async function fillCostCenter(cost) {
            const costCenterFill = document.getElementById('CostCenter');
            costCenterFill.value = '';
            if (!cost) {
                costCenterFill.value = cost;
            }
            else {
                const cc = cost[0];
                costCenterFill.value = cc.fundsCenter;
            }

            
        }
    }

    $('#Customer').on('change', onCustomerChange);
    async function onCustomerChange() {
        CustomerID = $("select[name='Customer']").val()
        var fd = new FormData();
        fd.append('CustomerID', CustomerID);
        await asyncAjax("/page/revenue/get-regional-fill", "POST", fd)
            .then(async function successCallBack(response) {

                console.log(response);
                if (response.success) {
                    console.log(response.data);
                    if (!response.data) {
                        var balikan = "Regional Tidak ditemukan";
                        await fillRegional(balikan);
                    } else {
                        await fillRegional(response.data);
                    }

                } else {
                    swallAllert.Error("Data ngga ada woy, ", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            });

        async function fillRegional(regional) {
            const regionalFill = document.getElementById('Regional');
            regionalFill.value = '';
            if (!regional) {
                regionalFill.value = regional;
            }
            else {
                const r = regional[0];
                regionalFill.value = r.regional;
            }


        }
    }

    await Load(idHeader);
})