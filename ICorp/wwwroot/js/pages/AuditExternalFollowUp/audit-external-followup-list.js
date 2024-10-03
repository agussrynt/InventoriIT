$(async function () {
    var fr = $('form[name="form-set-followup"]');
    async function Load() {
        await asyncAjax("/page/auditexternalfollowup/get-list-ajax", "POST")
            .then(async function successCallBack(response) {
                console.log(response);
                if (response.success) {
                    OnSuccess(response.data);
                } else {
                    swallAllert.Error("Fetch Data Failed!", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            });

        /** When success fetch data user and create dataTable */
        async function OnSuccess(listData) {
            var groupParent = [];
            var collapsedGroups = [];
            var tbl = $('#dataTable').DataTable();
            tbl.destroy();

            $('#dataTable').DataTable({
                width: "100%",
                data: listData,
                columns: [
                    { data: 'auditoR_NAME', defaultcontent: "", visible: false }, 
                    { data: 'totaL_SCORE', defaultcontent: "" },
                    {
                        data: 'date',
                        defaultContent: "-",
                        render: function (data, type, full, meta) {
                            if (data != null)
                                return convertDate(data);
                            else
                                return '';
                        }
                    },
                    { data: 'rekomendasi', defaultcontent: "" },
                    { data: 'functioN_STR', defaultcontent: "" },
                    {
                        data: 'duE_DATE',
                        defaultContent: "-",
                        render: function (data, type, full, meta) {
                            if (data != null)
                                return convertDate(data);
                            else
                                return '';
                        }
                    },
                    {
                        data: null,
                        defaultContent: "-",
                        orderable: false,
                        render: function (data, type, row, meta) {
                            var result = ''
                            if (data.statuS_NAME === '[External] Assignment')
                                result = `<button class="btn btn-success followup"><i class='bx bxs-book-alt'></i>FollowUp</button>`;

                            return result;
                        }
                    },

                ],
                columnDefs: [
                    {
                        sortable: false,
                        orderable: false,
                        searchable: false,
                        targets: 0,
                    }],
                rowGroup: {
                    dataSrc: ['auditoR_NAME'],
                    startRender: function (rows, group, level) {
                        groupParent[level] = group;
                        var groupAll = '';
                        for (var i = 0; i < level; i++) { groupAll += groupParent[i]; if (collapsedGroups[groupAll]) { return; } }
                        groupAll += group;
                        if ((typeof (collapsedGroups[groupAll]) == 'undefined') || (collapsedGroups[groupAll] === null)) { collapsedGroups[groupAll] = false; } //True = Start collapsed. False = Start expanded.

                        var collapsed = collapsedGroups[groupAll];

                        rows.nodes().each(function (r) {
                            r.style.display = collapsed ? 'none' : '';
                        });

                        var icon = '<i class="bx bx-plus-circle text-success me-1"></i>';
                        if (!collapsed)
                            icon = '<i class="bx bx-minus-circle text-danger me-1"></i>';

                        // Add category name to the <tr>. NOTE: Hardcoded colspan
                        return $('<tr/>')
                            .append('<td colspan="12">' + icon + group + '</td>')
                            //.attr('data-name', all)
                            .attr('data-name', groupAll)
                            .toggleClass('collapsed', collapsed);
                    }
                },

                lengthMenu: [[5, 10, 50, 100, -1], [5, 10, 50, 100, "All"]],
                autoWidth: true,
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                scrollX: true,
                bScrollCollapse: true,
                fixedColumns: {
                    left: 1,
                },
                order: [[0, 'desc']]
            });

            $('#dataTable').on('click', '.followup', await openModal);
            async function openModal() {
                $(".remark_follow").hide();
                var
                    tb = $('#dataTable').DataTable(),
                    dt = tb.row($(this).parents('tr')).data(),
                    id = fr.find('input[type="hidden"]'),
                    dd = fr.find('input[type="date"]'),
                    tx = fr.find('textarea#Recomendation'),
                    rm = fr.find('textarea#Remark'),
                    date = padToDigits(dt.duE_DATE);

                id.val(dt.iD_ASSIGMENT_RECOMENDATION);
                tx.val(dt.rekomendasi);
                dd.val(date);
                if (dt.remark != '') {
                    rm.val(dt.remark);
                    $(".remark_follow").show();
                }
                
                $('#setFollowUp').modal("show");
                console.log(padToDigits(dt.duE_DATE));
            }
        }
        
    }

    fr.submit(async function (e) {
        e.preventDefault();
        if (fr.valid()) {
            loadingForm(true, 'btnSetAssignment');
            var data = {
                ID_ASSIGMENT_RECOMENDATION: fr.find('input[type="hidden"]').val(),
                AUDITEE_FOLLOWUP: $('#FollowUp').val(),
                ATTACHMENT: fr.find('input[name="ATTACHMENT"]').val(),
                FILENAME: fr.find('input[name="ATTACHMENT_NAME"]').val()
            };

            await asyncAjax("/page/auditexternalfollowup/post-ajax", "POST", JSON.stringify(data), true)
                .then(async function successCallBack(response) {
                    console.log(response);
                    if (response.success) {
                        swallAllert.Success("Success!", response.message);
                        loadingForm(false, 'btnSetAssignment', "Submit");
                        $('#setFollowUp').modal("hide");
                        Load();
                    } else {
                        swallAllert.Error("Fetch Data Failed!", response.message);
                        loadingForm(false, 'btnSetAssignment', "Submit");
                    }
                })
                .catch(async function errorCallBack(err) {
                    swallAllert.Error("Fetch Data Failed!", err.data);
                    loadingForm(false, 'btnSetAssignment', "Submit");
                });
        }
    });

    await Load();
})

function padToDigits(num) {
    return num.toString().substring(0, 10);
}

function uploadMainFile() {
    var formData = new FormData();
    var fileInput = document.getElementById('ATTACHMENT_File');
    formData.append(fileInput.files[0].name, fileInput.files[0]);

    $.ajax({
        url: baseUrl + '/page/auditexternalfollowup/upload-file',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.success) {
                $('#ATTACHMENT_NAME').val(result.message);
                $('#ATTACHMENT').val(result.urlResponse);
            }
            else {
                alert(result.Message);
            }
        },
        error: function (err) {
            alert(err);
            swal.close();
        }
    });
}