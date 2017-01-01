/***USED IN Solution/YourSolution*************/


$(document).ready(function () {

    var oTable = $('#SolutionDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/User/Solution/YourSolutionAjaxHandler",
        "bProcessing": true,
        "pagingType": "numbers"
        , "aoColumns": [
                         {
                             "sName": "Sol_ID",
                             "bSearchable": false,
                             "bSortable": false,
                             "bVisible": false
                         },
                        {
                            "sName": "QID",
                            "bSearchable": false,
                            "bSortable": false,
                            "bVisible": false
                        },
                            {
                                "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                                , "bSearchable": false, "bSortable": false
                            },
                        { "sName": "question", "sWidth": '40%' },
                        { "sName": "solution", "sWidth": '40%', "bSearchable": false, "bSortable": false }
                        , {
                            "sName": "upload_num", "sWidth": '3%', "bSearchable": false, "bSortable": false,
                            "sClass": "dt-body-center"
                        }
                       , {
                           "sName": "confirm_status",
                           "sWidth": '2%',
                           "bSearchable": false,
                           "bSortable": false,
                           "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                           "mRender": function (data, type, row) {

                               if (data == "True") {
                                   return '<div class=\"checkbox checkbox-info\"><input  type=\"checkbox\" id=\"check_confirm\" disabled checked value="' + data + '"><label for=\"check_confirm\"></label></div>';
                               } else {
                                   return '<div class=\"checkbox checkbox-info\"><input  type=\"checkbox\" id=\"check_confirm\" disabled value="' + data + '"><label for=\"check_confirm\"></label></div>';
                               }
                           }
                       }, {
                           "sName": "EDIT",
                           "sWidth": '2%',
                           "bSearchable": false,
                           "bSortable": false,
                           "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                var url = 'NewSolution/' + row[1] + '?solution_id=' + row[0];
                                return "<a class='glyphicon glyphicon-edit a_clickable' href='" + url + "'></a>"

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
                                return "<a class='glyphicon glyphicon-trash a_clickable' onclick='delete_dialog(" + row[0] + ")'></a>"
                            }
                        }


        ]
    });


});



var delete_dialog = function (job_id) {

    var url = "/User/Solution/Delete_Solution"; // the url to the controller
    $.get(url + '/' + job_id, function (data) {
        $('#ModalContainer').html(data);
        $('#ModalContainer').find("#DeleteModal").modal('show');
    });
}

var SuccessDelete = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        var $STTable = $("#SolutionDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}


