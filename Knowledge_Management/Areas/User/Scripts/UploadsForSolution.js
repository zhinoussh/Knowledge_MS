/***USED IN Solution/FullSolution*************/

$(document).ready(function () {

    var oTable = $('#UploadDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/User/Solution/UploadAjaxHandler",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "solution_id", "value": $('#hd_id_solution').val() });
        },
        "bProcessing": true,
        "bFilter": false,
        "bSort": false,
        "pagingType": "numbers"
        , "aoColumns": [
                        {
                            "sName": "ID",
                            "bSearchable": false,
                            "bSortable": false,
                            "bVisible": false
                        },
                         {
                             "sName": "filePath",
                             "bSearchable": false,
                             "bSortable": false,
                             "bVisible": false
                         },
                        {
                            "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                                , "bSearchable": false, "bSortable": false
                        },
                        { "sName": "upload", "sWidth": '80%' }
                         , {
                             "sName": "download",
                             "sWidth": '2%',
                             "bSearchable": false,
                             "bSortable": false,
                             "sDefaultContent": " "
                             , "sClass": "dt-body-center",
                             "mRender": function (data, type, row) {
                                 return "<a class='glyphicon glyphicon-download a_clickable' onclick='download_file(" + row[0] + ")'></a>"

                             }
                         }
                      

        ]
    });



});

var download_file = function (upload_id) {

    $.ajax(
           {
               url: '/User/Solution/DownloadFile',
               contentType: 'application/json; charset=utf-8',
               datatype: 'json',
               data: {
                   uploadID: upload_id
               },
               type: "GET",
               success: function () {
                   window.location = '/User/Solution/DownloadFile?uploadID=' + upload_id;
               }
           });
}