$(document).ready(function () {

    var oTable = $('#JobDT').dataTable({
        "language": {
            "url": "/Content/lang.txt"
        },
        "bServerSide": true,
        "sAjaxSource": "/Job/JobAjaxHandler",
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
                                return "<a class='glyphicon glyphicon-remove a_clickable' onclick='delete_dialog(" + row[0] + ")'></a>"
                            }
                        }
        ]
    });



    $("#reset_btn").click(function () {
        $("#frmJob").find('input:text,textarea,field-validation-error').val("");
        $("#alert_success").empty();
        $("#div_alert").css("visibility", "hidden");
        $("#hd_id_job").val("0");

    });

    $("#dropdown_department").change(function () {
        var $STTable = $("#JobDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    });



});

var delete_dialog = function (job_id) {

    var url = "/Job/Delete_Job"; // the url to the controller
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
        $("#div_alert").css("visibility", "visible");
        $("#frmJob").find('input:text,textarea').val("");
        $("#hd_id_job").val("0");

        var $STTable = $("#JobDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}



