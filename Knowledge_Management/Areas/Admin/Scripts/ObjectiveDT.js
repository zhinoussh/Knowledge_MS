$(document).ready(function () {

    var oTable = $('#ObjectiveDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/Admin/ObjectiveDepartment/ObjectiveAjaxHandler",
        "fnServerParams": function ( aoData ) {
            aoData.push({ "name": "dep_id", "value": $("#hd_id_dep").val() });
        },
        "bProcessing": true,
        "pagingType": "numbers"
        ,"aoColumns": [
                        {
                            "sName": "ID",
                            "bSearchable": false,
                            "bSortable": false,
                            "bVisible": false
                        }
                        ,{ "sName": "radif", "sWidth": '3%', "sClass": "dt-body-center" },
                        { "sName": "obj_name", "sWidth": '90%' }
                        , {
                            "sName": "EDIT",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                return '<a class="glyphicon glyphicon-edit a_clickable" onclick="edit_objective(' + row[0] + ',\'' + row[2] + '\');"></a>'

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



    $("#reset_btn").click(function () {
        $("#frmObjective").find('input:text,textarea,field-validation-error').val("");
        $("#alert_success").empty();
        $("#div_alert").slideUp(500);
        $("#hd_id_obj").val("0");

    });

});

var delete_dialog = function (obj_id) {

    var url = "/Admin/ObjectiveDepartment/Delete_Objective"; // the url to the controller
    $.get(url + '/' + obj_id, function (data) {
        $('#confirm-container').html(data);
        $('#DeleteModal').modal('show');
    });
}



var edit_objective = function (obj_id, obj_name) {
    $("#hd_id_obj").val(obj_id);
    $("#txt_obj_name").val(obj_name);
}


var SuccessMessage = function (result) {
    if (result.msg) {
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        $("#frmObjective").find('input:text,textarea').val("");
        $("#hd_id_obj").val("0");

        var $STTable = $("#ObjectiveDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}

var SuccessDelete = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        var $STTable = $("#ObjectiveDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}

