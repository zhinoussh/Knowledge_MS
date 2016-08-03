$(document).ready(function () {

    var oTable = $('#strategyDT').dataTable({
        "language": {
            "url": "/Content/lang.txt"
        },
        "bServerSide": true,
        "sAjaxSource": "StrategyAjaxHandler",
        "bProcessing": true,
        "pagingType": "numbers",
        "aoColumns": [
                        {
                            "sName": "ID",
                            "bSearchable": false,
                            "bSortable": false,
                            "bVisible": false
                        },
                        { "sName": "radif", "sWidth": '3%', "sClass": "dt-body-center" },
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
                                return "<a class='glyphicon glyphicon-remove a_clickable' onclick='delete_dialog(" + row[0] + ")'></a>"
                            }
                        }
        ]
    });

     

    $("#reset_btn").click(function () {
        $("#frmStrategy").find('input:text,textarea,field-validation-error').val("");
        $("#alert_success").empty();
        $("#div_alert").css("visibility", "hidden");
        $("#hd_id_strategy").val("0");

    });

    $(".close").click(function () {
        $("#div_alert").css("visibility", "hidden");
        return false;
    });
    
    
});

var delete_dialog = function (st_id) {
    
    var url = "/Strategy/Delete_Strategy"; // the url to the controller
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
        $("#div_alert").css("visibility", "visible");
        $("#frmStrategy").find('input:text,textarea').val("");
        $("#hd_id_strategy").val("0");

        var $STTable = $("#strategyDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}



