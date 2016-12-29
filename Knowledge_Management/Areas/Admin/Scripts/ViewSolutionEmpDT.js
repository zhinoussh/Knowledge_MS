/***USED IN ViewEntrybyEmployee/EmployeeSolutions*************/

$(document).ready(function () {

    var oTable = $('#SolutionEmployeeDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/Admin/ViewEntrybyEmployee/SolutionEmployeeAjaxHandler",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "emp_id", "value": $('#hd_id_emp').val() });
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
                            "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                                , "bSearchable": false, "bSortable": false
                        },
                        { "sName": "question", "sWidth": '40%' },
                        { "sName": "solution", "sWidth": '40%' }
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
                        },
                        { "sName": "upload_count", "sWidth": '5%', "sClass": "dt-body-center", "bSearchable": false, "bSortable": false },
                        {
                            "sName": "Show_FullSolution",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                             , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                return '<a class="glyphicon glyphicon-list a_clickable" href="/Admin/ViewEntrybyEmployee/ViewFullSolution/' + row[0] + '"></a>'

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

                                  return (row[4].toString() == "True") ?
                                      "<a class='glyphicon glyphicon-remove-circle a_clickable' onclick='confirm_solution(" + row[0] + ");'></a>"
                                      :
                                     "<a class='glyphicon glyphicon-ok-circle a_clickable' onclick='confirm_solution(" + row[0] + ");'></a>"

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



var delete_dialog = function (s_id) {

    var url = "/Admin/ViewEntrybyEmployee/Delete_Solution"; // the url to the controller
    $.get(url + '/' + s_id, function (data) {
        $('#confirm-container').html(data);
        $('#DeleteModal').modal('show');
    });
}

var confirm_solution = function (s_id) {

    var url = "/Admin/ViewEntrybyEmployee/Confirm_Solution?s_id=" + s_id + "&emp_id=" + $("#hd_id_emp").val();
    $.post(url, function (data) {
        var tbl = $("#SolutionEmployeeDT").dataTable({ bRetrieve: true });
        tbl.fnDraw();
    });

}


var SuccessDeleteSolution = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        var $STTable = $("#SolutionEmployeeDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}


