$(document).ready(async function () {
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
        await asyncAjax("/page/revenue/get-detail-revenue", "POST", fd)
            .then(async function successCallBack(response) {

                console.log(response);
                if (response.success) {
                    console.log(response.data);
                    await OnSuccess(response.data);
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
            if ($.fn.DataTable.isDataTable('#fixedTable')) {
                $('#fixedTable').DataTable().destroy();
            }
            $('#fixedTable tbody').empty();
            $('#fixedTable').DataTable({
                //width: "100%",
                data: listData,
                columns: [
                    { data: 'project', defaultContent: "" },
                    { data: 'januari', defaultContent: "" },
                    { data: 'februari', defaultContent: "" },
                    { data: 'maret', defaultContent: "" },
                    { data: 'april', defaultContent: "" },
                    { data: 'mei', defaultContent: "" },
                    { data: 'juni', defaultContent: "" },
                    { data: 'juli', defaultContent: "" },
                    { data: 'agustus', defaultContent: "" },
                    { data: 'september', defaultContent: "" },
                    { data: 'oktober', defaultContent: "" },
                    { data: 'november', defaultContent: "" },
                    { data: 'desember', defaultContent: "" },
                    { data: 'total', defaultContent: "" },
                    
                ],
                paging: false, 
                searching: false, 
                ordering: false, 
                info: false, 
                fixedColumns: {
                    start: 1,
                    end: 1
                },
                scrollCollapse: true,
                scrollX: true,
                scrollY: 300
                
            });

        }
    }
    await Load(idHeader);
})