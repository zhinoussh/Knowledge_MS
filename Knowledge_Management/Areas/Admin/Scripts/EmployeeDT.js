$(document).ready(function () {

    var oTable = $('#EmployeeDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/Admin/Employee/EmployeeAjaxHandler",
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
                             "sName": "Dep_Id",
                             "bSearchable": false,
                             "bSortable": false,
                             "bVisible": false
                         },
                          {
                              "sName": "Job_Id",
                              "bSearchable": false,
                              "bSortable": false,
                              "bVisible": false
                          },
                            { "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                            , "bSearchable": false, "bSortable": false },
                        { "sName": "fname", "sWidth": '15%' },
                         { "sName": "lname", "sWidth": '20%' },
                         { "sName": "pcode", "sWidth": '15%' }
                            ,{ "sName": "department", "sWidth": '20%' },
                            { "sName": "job", "sWidth": '20%' }
                            , {
                                "sName": "dt_entry",
                                "sWidth": '2%',
                                "bSearchable": false,
                                "bSortable": false,
                                "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                                "mRender": function (data, type, row) {
                                   
                                    if (data == "True") {
                                        return '<div class=\"checkbox checkbox-primary\"><input  type=\"checkbox\" id=\"check_dataEntry\" disabled checked value="' + data + '"><label for=\"check_dataEntry\"></label></div>';
                                    } else {
                                        return '<div class=\"checkbox checkbox-primary\"><input  type=\"checkbox\" id=\"check_dataEntry\" disabled value="' + data + '"><label for=\"check_dataEntry\"></label></div>';

                                    }
                                }
                            }
                            , {
                                "sName": "dt_view",
                                "sWidth": '2%',
                                "bSearchable": false,
                                "bSortable": false,
                                "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                if (data == "True") {
                                    return '<div class=\"checkbox checkbox-primary\"><input  type=\"checkbox\" id=\"check_dataView\" disabled checked value="' + data + '"><label for=\"check_dataView\"></label></div>';
                                }
                                else {
                                    return '<div class=\"checkbox checkbox-primary\"><input  type=\"checkbox\" id=\"check_dataView\" disabled value="' + data + '"><label for=\"check_dataView\"></label></div>';
                                }
                            }
                            }
                        , {
                            "sName": "EDIT",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                var param_array = { id: row[0], dep_id: row[1], job_id: row[2], fname: row[4], lname: row[5], pcode: row[6], dt_e: row[9].toString(), dt_v: row[10].toString() };
                                var param_obj = [];
                                param_obj.push(param_array);
                               // var o = JSON.parse(param_obj);
                                return "<a class='glyphicon glyphicon-edit a_clickable' onclick='edit_employee(" + JSON.stringify(param_obj) + ");'></a>"

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
        $("#frmEmployee").find('input:text,textarea,field-validation-error').val("");
        $("#alert_success").empty();
        $("#div_alert").slideUp(500);

        $("#hd_id_emp").val("0");
        $('input[type="checkbox"]').prop('checked', false);
       
        $('#dropdown_department option:selected').removeAttr('selected');
        $('#dropdown_department').trigger('change');
    });

    //called when key is pressed in textbox
    $("#txt_personel_code").keypress(function (e) {
        //if the letter is not digit then display error and don't type anything
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //display error message
          //  $("#errmsg").html("Digits Only").show().fadeOut("slow");
            return false;
        }
    });

    $("#dropdown_department").change(function () {
      
        var dep_id = $('#dropdown_department').val();
        $.ajax({
            url: '/Admin/Employee/FillJobs',
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

var delete_dialog = function (emp_id) {

    var url = "/Admin/Employee/Delete_Employee"; // the url to the controller
    $.get(url + '/' + emp_id, function (data) {
        $('#confirm-container').html(data);
        $('#DeleteModal').modal('show');
    });
}

var edit_employee = function (s) {
    $("#hd_id_emp").val(s[0].id);
    $("#dropdown_department").val(s[0].dep_id);
    $('#dropdown_department').trigger('change');
    $("#dropdown_job").val(s[0].job_id);
    $("#txt_first_name").val(s[0].fname);
    $("#txt_last_name").val(s[0].lname);
    $("#txt_personel_code").val(s[0].pcode);

    var dt_entry = s[0].dt_e == "True" ? true : false
    var dt_view = s[0].dt_v == "True" ? true : false

    $("#chk_dt_entry").prop('checked', dt_entry);
    $("#chk_dt_view").prop('checked', dt_view);
}


var SuccessMessage = function (result) {
    if (result.msg) {
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);

        //if success
        if (result.result == 1) {
            $("#frmEmployee").find('input:text,textarea').val("");
            $("#hd_id_emp").val("0");
            $('input[type="checkbox"]').prop('checked', false);

            $('#dropdown_department option:selected').removeAttr('selected');
            $('#dropdown_department').trigger('change');

            var $STTable = $("#EmployeeDT").dataTable({ bRetrieve: true });
            $STTable.fnDraw();
        }
    }
}

var SuccessDelete = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        var $STTable = $("#EmployeeDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}



