$(document).ready(function () {

    var oTable = $('#JobDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/Admin/Job/JobAjaxHandler",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "dep_id", "value": $('#dropdown_department').val() });
        },
        "bProcessing": true,
        "pagingType": "numbers"
        , "aoColumns": [
                        {
                            "sName": "ID",
                            "bSearchable": false,
                            "bSortable": false,
                            "bVisible": false
                        }
                        , { "sName": "radif", "sWidth": '3%', "sClass": "dt-body-center" },
                        { "sName": "job_name", "sWidth": '90%' }
                        , {
                            "sName": "EDIT",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                return '<a class="glyphicon glyphicon-edit a_clickable" onclick="edit_job(' + row[0] + ',\'' + row[2] + '\');"></a>'

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
        $("#frmJob").find('input:text,textarea,field-validation-error').val("");
        $("#alert_success").empty();
        $("#div_alert").slideDown(500);
        $("#hd_id_job").val("0");

    });

    $("#dropdown_department").change(function () {
        var $STTable = $("#JobDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    });


});

var delete_dialog = function (job_id) {

    var url = "/Admin/Job/Delete_Job"; // the url to the controller
    $.get(url + '/' + job_id, function (data) {
        $('#confirm-container').html(data);
        $('#DeleteModal').modal('show');
    });
}



var edit_job = function (job_id, job_name) {
    $("#hd_id_job").val(job_id);
    $("#txt_job_name").val(job_name);
}


var SuccessMessage = function (result) {
    if (result.msg) {
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        $("#frmJob").find('input:text,textarea').val("");
        $("#hd_id_job").val("0");

        var $STTable = $("#JobDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}



var SuccessDelete = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        var $STTable = $("#JobDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}