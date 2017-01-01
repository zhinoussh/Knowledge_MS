$(document).ready(function () {

    var oTable = $('#JobDescDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/Admin/JobDescription/JobDescAjaxHandler",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "job_id", "value": $('#dropdown_job').val() });
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
                        { "sName": "radif", "sWidth": '3%', "sClass": "dt-body-center" },
                        { "sName": "job_desc", "sWidth": '90%' }
                        , {
                            "sName": "EDIT",
                            "sWidth": '3%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                               
                                return '<a class="glyphicon glyphicon-edit a_clickable" onclick="edit_JobDesc(' + row[0] + ',\'' + row[2] + '\');"></a>'

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
        $("#frmJobDesc").find('input:text,textarea,field-validation-error').val("");
        $("#alert_success").empty();
        $("#div_alert").slideUp(500);
        $("#hd_id_jobDsc").val("0");

        $('#dropdown_department option:selected').removeAttr('selected');
        $('#dropdown_department').trigger('change');
    });


    $("#btn_view_jobDesc").click(function () {
        var $STTable = $("#JobDescDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    });
    
    $("#dropdown_department").change(function () {

        var dep_id = $('#dropdown_department').val();
        $.ajax({
            url: '/Admin/JobDescription/FillJobs',
            type: "GET",
            dataType: 'JSON',
            data: { DepId: dep_id },
            success: function (jobs) {
                $("#dropdown_job").html(""); // clear before appending new list 

                $.each(jobs, function (i, job) {
                    $("#dropdown_job").append($('<option></option>').val(job.Value).html(job.Text));
                });

            },
            error: function (e) {
                alert('err:' + e.toString());
            }
        });


    });


});

var delete_dialog = function (jd_id) {

    var url = "/Admin/JobDescription/Delete_JobDesc"; // the url to the controller
    $.get(url + '/' + jd_id, function (data) {
        $('#ModalContainer').html(data);
        $('#ModalContainer').find("#DeleteModal").modal('show');
    });
}

var edit_JobDesc = function (jd_id,job_desc) {
    $("#hd_id_jobDsc").val(jd_id);
    $("#txt_job_desc").val(job_desc);
}


var SuccessMessage = function (result) {
    if (result.msg) {
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        $("#frmJobDesc").find('input:text,textarea').val("");
        $("#hd_id_jobDsc").val("0");

        $('#dropdown_department option:selected').removeAttr('selected');
        $('#dropdown_department').trigger('change');

        var $STTable = $("#JobDescDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}

var SuccessDelete = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        var $STTable = $("#JobDescDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}

