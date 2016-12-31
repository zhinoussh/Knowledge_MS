/***USED IN ViewEntrybyJob/Index*************/

$(document).ready(function () {

    var oTable = $('#SearchQuestionDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/Admin/ViewEntrybyJob/SearchQuestionAjaxHandler",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "job_id", "value": $('#dropdown_job').val() });
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
                        },
                         {
                             "sName": "keywords",
                             "bSearchable": false,
                             "bSortable": false,
                             "bVisible": false
                         },
                          {
                              "sName": "job_Desc",
                              "bSearchable": false,
                              "bSortable": false,
                              "bVisible": false
                          },
                           {
                               "sName": "dep_Obj",
                               "bSearchable": false,
                               "bSortable": false,
                               "bVisible": false
                           },
                            {
                                "sName": "Strategy",
                                "bSearchable": false,
                                "bSortable": false,
                                "bVisible": false
                            },
                            {
                                "sName": "fullQuestion",
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
                                    keywords: row[1], jobdesc: row[2], dep_objective: row[3]
                                    , strategy: row[4], fullQuestion: row[5]
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
                                return "<a class='glyphicon glyphicon-list-alt a_clickable' href='/Admin/ViewEntrybyEmployee/QuestionSolutions/" + row[0] + "'></a>"
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

    $("#close_delete_modal").click(function () {
        $("#div_alert").slideDown(500);
        return false;
    });

    $("#btn_view_Question").click(function () {
        var $STTable = $("#SearchQuestionDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    });

    $("#dropdown_department").change(function () {

        var dep_id = $('#dropdown_department').val();
        $.ajax({
            url: '/Admin/ViewEntrybyJob/FillJobs',
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
        url: '/Admin/ViewEntrybyJob/QuestionDetails',
        success: function (result) {
            $('#ModalContainer').html(result);
            $('#ModalContainer').find("#DetailModal").modal('show');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

var delete_dialog = function (q_id) {

    var url = "/Admin/ViewEntrybyJob/Delete_Question"; // the url to the controller
    $.get(url + '/' + q_id, function (data) {
        $('#confirm-container').html(data);
        $('#DeleteModal').modal('show');
    });
}

var SuccessDelete = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);
        var $STTable = $("#SearchQuestionDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}

