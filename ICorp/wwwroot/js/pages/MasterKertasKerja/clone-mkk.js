$(async function () {
    $('#btnPreview').on('click', OnLoadDataClone);

    async function Load() {
        await asyncAjax("/master/year/get-list-year-ajax", "POST")
            .then(async function successCallBack(response) {
                console.log(response);
                if (response.success) {
                    var groups_array = [];
                    $.each(response.data, function (index) {
                        groups_array.push({
                            id: response.data[index].year,
                            text: response.data[index].year,
                        });
                    });
                    $("select#Year").select2({
                        placeholder: "Please select one",
                        minimumResultsForSearch: -1,
                        allowClear: true,
                        data: groups_array,
                    });
                    $('select#Year').val("").trigger('change');
                } else {
                    swallAllert.Error("Fetch Data Failed!", response.data);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            });
    }

    $('#btnClone').click(function () {
        var value = $('select#Year').val();
        if (value && value != "") {

        }
    })

    async function OnLoadPartialJS() {
        await asyncAjax("/page/master-kertas-kerja/_partialJS", "GET")
            .then(async function successCallBack(response) {
                console.log(response);
                $('#partialScript').html(response);
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            });
    }

    async function OnLoadDataClone() {
        var year = $('select#Year').val();
        await asyncAjax("/page/master-kertas-kerja/get-datamasterkertaskerja-ajax/" + year, "GET")
            .then(async function successCallBack(response) {
                console.log(response);
                if (response.success) {
                    var data = response.data;
                    parameterHeader = [];
                    parameterContent = [];
                    parameterFUK = [];
                    parameterUP = [];
                    localDataHeader = [];
                    localDataContent = [];
                    localDataFUK = [];
                    localDataUP = [];


                    for (var i = 0; i < data.parameterHeader.length; i++) {
                        var obj = data.parameterHeader[i];
                        var objHeader = {
                            Aspek: obj.aspek,
                            Id: obj.id,
                            Sequence: obj.sequence
                        }
                        localDataHeader.push(objHeader);
                    }

                    for (var i = 0; i < data.parameterContent.length; i++) {
                        var obj = data.parameterContent[i];
                        var objContent = {
                            Aspek: obj.aspek,
                            Bobot: obj.bobot,
                            Description: obj.description,
                            HeaderId: obj.headerId,
                            Id: obj.id
                        }
                        localDataContent.push(objContent);
                    }

                    for (var i = 0; i < data.parameterFUK.length; i++) {
                        var obj = data.parameterFUK[i];
                        var objFuk = {
                            Child: (obj.child !== null ? obj.child.trim(): null),
                            ContentDescription: obj.contentDescription,
                            ContentId: obj.contentId,
                            Description: obj.description,
                            Id: obj.id,
                            Parent: (obj.child !== null ? obj.childParent : (obj.parent === 1 ? 'On' : 'Off')),
                            Sequence: obj.sequence
                        }
                        localDataFUK.push(objFuk);
                    }

                    for (var i = 0; i < data.parameterUP.length; i++) {
                        var obj = data.parameterUP[i];
                        var objUP = {
                            Description: obj.description,
                            FUKDescription: obj.fukDescription,
                            FUKId: obj.fukId,
                            Id: obj.id,
                        }
                        localDataUP.push(objUP);
                    }

                    storedHeaderParameter = !localDataHeader ? new Array : localDataHeader,
                        storedContentParameter = !localDataContent ? new Array : localDataContent,
                        storedFUKDetail = !localDataFUK ? new Array : localDataFUK,
                        storedUPLink = !localDataUP ? new Array : localDataUP;


                    setTimeout(await OnLoadPartialJS, 500);
                }
            })
            .catch(async function errorCallBack(err) {
                swallAllert.Error("Fetch Data Failed!", err.data);
            });
    }

    await Load();
})