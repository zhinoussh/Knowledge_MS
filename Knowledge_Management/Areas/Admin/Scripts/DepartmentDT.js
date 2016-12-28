$(document).ready(function () {

    var oTable = $('#departmentDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/Admin/Department/DepartmentAjaxHandler",
        "bProcessing": true,
        "pagingType": "numbers",
        "aoColumns": [
                        {
                            "sName": "ID",
                            "bSearchable": false,
                            "bSortable": false,
                            "bVisible": false
                        },
                        { "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center" },
                        { "sName": "dep_name", "sWidth": '85%' }
                         , {
                             "sName": "OBJECTIVES",
                             "sWidth": '3%',
                             "bSearchable": false,
                             "bSortable": false,
                             "sDefaultContent": " "
                            , "sClass": "dt-body-center"
                            , "mRender": function (data, type, row) {
                                return "<a class='glyphicon glyphicon-list-alt a_clickable' href='/Admin/ObjectiveDepartment/Index/" + row[0] + "'></a>"
                            }
                         }
                        , {
                            "sName": "EDIT",
                            "sWidth": '3%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                return '<a class="glyphicon glyphicon-edit a_clickable" onclick="edit_department(' + row[0] + ',\'' + row[2] + '\');"></a>'

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



    $("#reset_btn").click(function () {
        $("#frmDepartment").find('input:text,textarea,field-validation-error').val("");
        $("#alert_success").empty();
        $("#hd_id_department").val("0");
        $("#div_alert").slideUp(500);
    });


});

var delete_dialog = function (dep_id) {

    var url = "/Admin/Department/Delete_Department"; // the url to the controller
    $.get(url + '/' + dep_id, function (data) {
        $('#confirm-container').html(data);
        $('#DeleteModal').modal('show');
    });
}



var edit_department = function (dep_id, dep_name) {
    $("#hd_id_department").val(dep_id);
    $("#txt_dep_name").val(dep_name);
}


var SuccessMessage = function (result) {
    if (result.msg) {
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);

        $("#frmDepartment").find('input:text,textarea').val("");
        $("#hd_id_department").val("0");

        var $STTable = $("#departmentDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}

var SuccessDelete = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);

        var $STTable = $("#departmentDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}



