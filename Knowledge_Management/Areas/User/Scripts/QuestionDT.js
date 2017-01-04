$(document).ready(function () {

    var oTable = $('#QuestionDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/User/InsertInfo/YourQuestionAjaxHandler",
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
                          }, {
                              "sName": "keys",
                              "bSearchable": false,
                              "bSortable": false,
                              "bVisible": false
                          },
                            {
                                "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                            , "bSearchable": false, "bSortable": false
                            },
                        { "sName": "question", "sWidth": '80%' },
                         
                          {
                              "sName": "details",
                              "sWidth": '2%',
                              "bSearchable": false,
                              "bSortable": false,
                              "sDefaultContent": " "
                             , "sClass": "dt-body-center",
                              "mRender": function (data, type, row) {
                                  var param_array = {
                                      dep_objective: row[1], jobdesc: row[2], strategy: row[3],keywords: row[4]
                                      , fullQuestion: row[6]
                                  };
                                  var param_obj = [];
                                  param_obj.push(param_array);
                                  return "<a class='glyphicon glyphicon-list a_clickable' onclick='details(" + JSON.stringify(param_obj) + ");'></a>"

                              }
                          },
                        {
                            "sName": "Solutions",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                return "<a class='glyphicon glyphicon-list-alt a_clickable' href='/User/Solution/Index/" + row[0] + "'></a>"
                            }

                        }
                        ,
                        {
                            "sName": "New_Solution",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                return "<a class='glyphicon glyphicon-file a_clickable' href='/User/Solution/NewSolution/" + row[0] + "'></a>"
                            }

                        },
                          {
                            "sName": "EDIT",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {
                                return "<a class='glyphicon glyphicon-edit a_clickable' href='/User/InsertInfo/NewQuestion/" + row[0] + "'></a>"

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
        $("#frmQuestion").find('input:text,field-validation-error').val("");
        $("#alert_success").empty();
        $("#div_alert").slideDown(500);
        $("#hd_id_question").val("0");

        $('#dropdown_dep_Objective option:selected').removeAttr('selected');
        $('#dropdown_jobDesc option:selected').removeAttr('selected');
        $('#dropdown_strategy option:selected').removeAttr('selected');

        //cleare keyword part
        var field1_input = '<input autocomplete="off" class="txt_keyword" id="field1" name="field1"  type="text" placeholder="Enter keyword" data-items="8">';
        field1_input += '<button id="b1" class="btn add-more" type="button">+</button>';
        $("#field").html(field1_input);
        var next = $("#count").val("1");
    });

});

var delete_dialog = function (q_id) {

    var url = "/User/InsertInfo/Delete_Question"; // the url to the controller
    $.get(url + '/' + q_id, function (data) {
        $('#ModalContainer').html(data);
        $('#ModalContainer').find("#DeleteModal").modal('show');
    });
}

var details = function (s) {
    var QuestionViewModel = {
        question: s[0].fullQuestion,
        lst_keywords: s[0].keywords,
        dep_objective: (s[0].dep_objective == null ? '' : s[0].dep_objective),
        job_desc: (s[0].jobdesc == null ? '' : s[0].jobdesc),
        strategy_name: (s[0].strategy == null ? '' : s[0].strategy)
    };

    $.ajax({
        type: 'GET',
        data: QuestionViewModel,
        url: '/User/SearchInfo/QuestionDetails',
        success: function (result) {
            $('#ModalContainer').html(result);
            $('#ModalContainer').find("#DetailModal").modal('show');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}



var SuccessDelete = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        var $STTable = $("#QuestionDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}



