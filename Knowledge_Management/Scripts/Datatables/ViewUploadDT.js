$(document).ready(function () {

    var oTable = $('#UploadDT').dataTable({
        "language": {
            "url": "/Content/lang.txt"
        },
        "bServerSide": true,
        "sAjaxSource": "/ViewEntryInfo/UploadAjaxHandler",
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
                             "sName": "confirm_status",
                             "sWidth": '2%',
                             "bSearchable": false,
                             "bSortable": false,
                             "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                             "mRender": function (data, type, row) {

                                 if (data == "True") {
                                     return '<input disabled  type=\"checkbox\" checked value="' + data + '">';
                                 } else {
                                     return '<input disabled  type=\"checkbox\" value="' + data + '">';
                                 }
                             }
                         },{
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
                        ,
                          {
                              "sName": "Confirm",
                              "sWidth": '2%',
                              "bSearchable": false,
                              "bSortable": false,
                              "sDefaultContent": " "
                             , "sClass": "dt-body-center",
                              "mRender": function (data, type, row) {
                                  if (row[4].toString() == "True")
                                      return "<a class='glyphicon glyphicon-remove-circle a_clickable' onclick='confirm_upload(" + row[0] + ");'></a>"
                                  else
                                      return "<a class='glyphicon glyphicon-ok-circle a_clickable' onclick='confirm_upload(" + row[0] + ");'></a>"
                              }
                          }
                         , {
                             "sName": "DELETE",
                             "sWidth": '3%',
                             "bSearchable": false,
                             "bSortable": false,
                             "sDefaultContent": " "
                            , "sClass": "dt-body-center"
                            , "mRender": function (data, type, row) {
                                return "<a class='glyphicon glyphicon-trash a_clickable' onclick='delete_dialog(" + row[0] + ")'></a>"
                            }
                         }

        ]
    });


    $("#close_delete_modal").click(function () {
        $("#div_alert").css("visibility", "hidden");
        return false;
    });



});


var SuccessMessage = function (result) {
    if (result.msg) {
        $("#alert_success").html(result.msg);
        $("#div_alert").css("visibility", "visible");

    }

}


var download_file = function (upload_id) {

    $.ajax(
           {
               url: '/ViewEntryInfo/DownloadFile',
               contentType: 'application/json; charset=utf-8',
               datatype: 'json',
               data: {
                   uploadID: upload_id
               },
               type: "GET",
               success: function () {
                   window.location = '/ViewEntryInfo/DownloadFile?uploadID=' + upload_id;
               }
           });
}

var delete_dialog = function (upload_id) {

    var url = "/ViewEntryInfo/Delete_Upload"; // the url to the controller
    $.get(url + '/' + upload_id, function (data) {
        $('#confirm-container').html(data);
        $('#DeleteModal').modal('show');
    });
}

var confirm_upload= function (u_id) {

    var url = "/ViewEntryInfo/Confirm_Upload?u_id=" + u_id + "&s_id=" + $("#hd_id_solution").val();
    $.post(url , function (data) {
        var tbl = $("#UploadDT").dataTable({ bRetrieve: true });
        tbl.fnDraw();
    });

}



