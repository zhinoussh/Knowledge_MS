$(document).ready(function () {

    var oTable = $('#QuestionDT').dataTable({
        "language": {
            "url": "/Content/lang.txt"
        },
        "bServerSide": true,
        "sAjaxSource": "QuestionAjaxHandler",
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
                             "sName": "DepObj_Id",
                             "bSearchable": false,
                             "bSortable": false,
                             "bVisible": false
                         },
                          {
                              "sName": "JobDesc_Id",
                              "bSearchable": false,
                              "bSortable": false,
                              "bVisible": false
                          },
                          {
                              "sName": "st_Id",
                              "bSearchable": false,
                              "bSortable": false,
                              "bVisible": false
                          },
                            {
                                "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                            , "bSearchable": false, "bSortable": false
                            },
                        { "sName": "question", "sWidth": '80%' }
                       ,{
                              "sName": "solution",
                              "bSearchable": false,
                              "bSortable": false,
                              "bVisible": false
                          },
                          {
                              "sName": "keys",
                              "bSearchable": false,
                              "bSortable": false,
                              "bVisible": false
                          }, {
                            "sName": "EDIT",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                var param_array = { id: row[0], depobj_id: row[1], jobdesc_id: row[2], st_id: row[3], question: row[5], solution: row[6], keys: row[7]};
                                var param_obj = [];
                                param_obj.push(param_array);
                                // var o = JSON.parse(param_obj);
                                return "<a class='glyphicon glyphicon-edit a_clickable' onclick='edit_question(" + JSON.stringify(param_obj) + ");'></a>"

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
        $("#frmQuestion").find('input:text,field-validation-error').val("");
        $("#alert_success").empty();
        $("#div_alert").css("visibility", "hidden");
        $("#hd_id_question").val("0");
        $("#txt_solution").val("");

        $('#dropdown_dep_Objective option:selected').removeAttr('selected');
        $('#dropdown_jobDesc option:selected').removeAttr('selected');
        $('#dropdown_strategy option:selected').removeAttr('selected');

        //cleare keyword part
        var field1_input = '<input autocomplete="off" class="txt_keyword" id="field1" name="field1"  type="text" placeholder="کلید واژه" data-items="8">';
        field1_input += '<button id="b1" class="btn add-more" style="padding:0" type="button">+</button>';
        $("#field").html(field1_input);
        var next = $("#count").val("1");
    });

   
   

});

var delete_dialog = function (q_id) {

    var url = "/InsertInfo/Delete_Question"; // the url to the controller
    $.get(url + '/' + q_id, function (data) {
        $('#confirm-container').html(data);
        $('#DeleteModal').modal('show');
    });
}

var edit_question = function (s) {
    $("#hd_id_question").val(s[0].id);
    $("#dropdown_dep_Objective").val(s[0].depobj_id);
    $("#dropdown_jobDesc").val(s[0].jobdesc_id);
    $("#dropdown_strategy").val(s[0].st_id);
    $("#txt_question").val(s[0].question);
    $("#txt_solution").val(s[0].solution);

    //cleare keyword part
    var field1_input = '<input autocomplete="off" class="txt_keyword" id="field1" name="field1"  type="text" placeholder="کلید واژه" data-items="8">';
    field1_input += '<button id="b1" class="btn add-more" style="padding:0" type="button">+</button>';
    $("#field").html(field1_input);
    var next = $("#count").val("1");

    var array = s[0].keys.split(",");
    var count_key = array.length;
    if (count_key > 0)
    {
        $("#field1").val(array[0]);
        for (j = 1; j < count_key ; j++) {
            $(".add-more").click();
            $("#field"+(j+1)).val(array[j]);
        }
    }
    
}



var SuccessMessage = function (result) {
    if (result.msg) {
        $("#alert_success").html(result.msg);
        $("#div_alert").css("visibility", "visible");

        $("#frmQuestion").find('input:text').val("");
        $("#hd_id_question").val("0");
        $("#txt_solution").val("");

        $('#dropdown_dep_Objective option:selected').removeAttr('selected');
        $('#dropdown_jobDesc option:selected').removeAttr('selected');
        $('#dropdown_strategy option:selected').removeAttr('selected');

        //cleare keyword part
        var field1_input = '<input autocomplete="off" class="txt_keyword" id="field1" name="field1"  type="text" placeholder="کلید واژه" data-items="8">';
        field1_input += '<button id="b1" class="btn add-more" style="padding:0" type="button">+</button>';
        $("#field").html(field1_input);
        var next = $("#count").val("1");

        var $STTable = $("#QuestionDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();

    }
}



