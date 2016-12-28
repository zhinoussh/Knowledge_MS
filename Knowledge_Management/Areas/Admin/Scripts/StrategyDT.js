$(document).ready(function () {

    var oTable = $('#strategyDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/Admin/Strategy/StrategyAjaxHandler",
        "bProcessing": true,
        "pagingType": "numbers",
        "aoColumns": [
                        {
                            "sName": "ID",
                            "bSearchable": false,
                            "bSortable": false,
                            "bVisible": false
                        },
                        {
                            "sName": "radif", "sWidth": '3%', "sClass": "dt-body-center", "bSearchable": false,
                            "bSortable": false
                        },
                        { "sName": "strategy_name", "sWidth": '90%' }
                        , {
                            "sName": "EDIT",
                            "sWidth": '3%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                return '<a class="glyphicon glyphicon-edit a_clickable" onclick="edit_strategy(' + row[0] + ',\'' + row[2] + '\');"></a>'

                            }
                        }
                        , {
                            "sName": "DELETE",
                            "sWidth": '3%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center"
                            ,"mRender": function (data, type, row) {
                                return "<a class='glyphicon glyphicon-trash a_clickable' onclick='delete_dialog(" + row[0] + ")'></a>"
                            }
                        }
        ]
    });

    $("#reset_btn").click(function () {
        $("#frmStrategy").find('input:text,textarea,field-validation-error').val("");
        $("#alert_success").empty();
        $("#div_alert").slideUp(500);
        $("#hd_id_strategy").val("0");

    });
    
});

var delete_dialog = function (st_id) {
    
    var url = "/Admin/Strategy/Delete_Strategy"; // the url to the controller
     $.get(url + '/' + st_id, function (data) {
        $('#confirm-container').html(data);
        $('#DeleteModal').modal('show');
    });
}

var edit_strategy = function (st_id,st_name) {
    $("#hd_id_strategy").val(st_id);
    $("#txt_st_name").val(st_name);
}


var SuccessMessage = function (result) {
    if (result.msg) {
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        $("#frmStrategy").find('input:text,textarea').val("");
        $("#hd_id_strategy").val("0");

        var $STTable = $("#strategyDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}

var SuccessDelete = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        var $STTable = $("#strategyDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}



