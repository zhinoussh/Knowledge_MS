$(document).ready(function () {

    var oTable = $('#SoutionListDT').dataTable({
        "language": {
            "url": "/Content/lang.txt"
        },
        "bServerSide": true,
        "sAjaxSource": "/ViewEntryInfo/SoutionListAjaxHandler",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "q_id", "value": $('#hd_id_question').val() });
        },
        "bProcessing": true,
        "pagingType": "numbers"
        , "aoColumns": [
                        {
                            "sName": "ID",
                            "bSearchable": false,
                            "bSortable": false,
                            "bVisible": false
                        },
                            {
                                "sName": "fullSolution",
                                "bSearchable": false,
                                "bSortable": false,
                                "bVisible": false
                            },
                            {
                                "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                                , "bSearchable": false, "bSortable": false
                            },
                        { "sName": "solution", "sWidth": '80%' },
                        { "sName": "upload_count", "sWidth": '5%', "sClass": "dt-body-center", "bSearchable": false, "bSortable": false },
                        {
                            "sName": "Show_FullSolution",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                             , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                
                                return '<a class="glyphicon glyphicon-list a_clickable" onclick="details(\'' + row[1] + '\');"></a>'

                            }
                        }
                         ,
                          {
                              "sName": "Show_Uploads",
                              "sWidth": '2%',
                              "bSearchable": false,
                              "bSortable": false,
                              "sDefaultContent": " "
                             , "sClass": "dt-body-center",
                              "mRender": function (data, type, row) {

                                  return "<a class='glyphicon glyphicon-folder-open a_clickable' onclick='show_uploads(" + row[0] + ");'></a>"

                              }
                          }
                         , {
                             "sName": "DELETE",
                             "sWidth": '2%',
                             "bSearchable": false,
                             "bSortable": false,
                             "sDefaultContent": " "
                            , "sClass": "dt-body-center"
                            , "mRender": function (data, type, row) {
                                return "<a class='glyphicon glyphicon-remove a_clickable' onclick='delete_dialog(" + row[0] + ")'></a>"
                            }
                         }


        ]
    });

    $("#close_delete_modal").click(function () {
        $("#div_alert").css("visibility", "hidden");
        return false;
    });

  
});

var details = function (s) {
    $("#FullSolution").html(s);
    $('#DetailModal').modal('show');
}

var delete_dialog = function (s_id) {

    var url = "/ViewEntryInfo/Delete_Solution"; // the url to the controller
    $.get(url + '/' + s_id, function (data) {
        $('#confirm-container').html(data);
        $('#DeleteModal').modal('show');
    });
}

var show_uploads=function(s_id)
{
    var url = "/ViewEntryInfo/Get_Uploads";
    $.get(url + '/' + s_id, function (data) {
        $("#upload-container").html(data);
        $("#UploadModal").modal('show');
    });
}




