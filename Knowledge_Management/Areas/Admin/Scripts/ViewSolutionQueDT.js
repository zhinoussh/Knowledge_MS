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
                            "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                                , "bSearchable": false, "bSortable": false
                        },
                        { "sName": "solution", "sWidth": '80%', "sClass": "dt-body-left" }
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
                               
                                 return (row[3].toString()== "True") ?
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
    
    var url = "/Admin/ViewEntrybyEmployee/Confirm_Solution?s_id=" + s_id + "&q_id=" + $("#hd_id_question").val();
    $.post(url, function (data) {
        var tbl = $("#SoutionListDT").dataTable({ bRetrieve: true });
        tbl.fnDraw();
    });

}





