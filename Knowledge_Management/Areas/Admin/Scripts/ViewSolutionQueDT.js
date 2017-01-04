/***USED IN ViewEntrybyEmployee/QuestionSolutions*************/

$(document).ready(function () {

    var oTable = $('#SoutionListDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/Admin/ViewEntrybyEmployee/SolutionQuestionAjaxHandler",
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
                            "sName": "radif", "sWidth": '1%', "sClass": "dt-body-center"
                                , "bSearchable": false, "bSortable": false
                        },
                        { "sName": "solution", "sWidth": '50%', "sClass": "dt-body-left" }
                        , {
                            "sName": "confirm_status",
                            "sWidth": '1%',
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
                        },
                        { "sName": "upload_count", "sWidth": '2%', "sClass": "dt-body-center", "bSearchable": false, "bSortable": false },
                        {
                            "sName": "Actions",
                            "sWidth": '10%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                             , "sClass": "dt-body-center",
                             "mRender": function (data, type, row) {

                                 var confirm_btn=(
                                     (row[3].toString()== "True") ?
                                     " <a class='glyphicon glyphicon-remove-circle a_clickable' onclick='confirm_solution(" + row[0] + ");' data-toggle='tooltip' title='Reject'></a>"
                                     :
                                    " <a class='glyphicon glyphicon-ok-circle a_clickable' onclick='confirm_solution(" + row[0] + ");' data-toggle='tooltip' title='Confirm'></a>"
                                     );

                                 return '<a class="glyphicon glyphicon-list a_clickable" href="/Admin/ViewEntrybyEmployee/ViewFullSolution/' + row[0] + '" data-toggle="tooltip" title="Details"></a>'
                                         + " <a class='glyphicon glyphicon-trash a_clickable' onclick='delete_dialog(" + row[0] + ")' data-toggle='tooltip' title='Delete'></a>"
                                + confirm_btn;
                            }
                        }
                         
        ]
    });

});



var delete_dialog = function (s_id) {

    var url = "/Admin/ViewEntrybyEmployee/Delete_Solution"; // the url to the controller
    $.get(url + '/' + s_id, function (data) {
        $('#ModalContainer').html(data);
        $('#ModalContainer').find("#DeleteModal").modal('show');
    });
}
 
var confirm_solution = function (s_id) {
    
    var url = "/Admin/ViewEntrybyEmployee/Confirm_Solution_Question?s_id=" + s_id + "&q_id=" + $("#hd_id_question").val();
    $.post(url, function (data) {
        var tbl = $("#SoutionListDT").dataTable({ bRetrieve: true });
        tbl.fnDraw();
    });

}

var SuccessDeleteSolution = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        var $STTable = $("#SoutionListDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}




