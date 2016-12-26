/***USED IN ViewEntrybyDetail/Index*************/

$(document).ready(function () {

    var oTable = $('#SearchQuestionDT').dataTable({
        "language": {
            "url": "/Content/lang.txt"
        },
        "bServerSide": true,
        "sAjaxSource": "/ViewEntrybyDetail/SearchQuestionAjaxHandler",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "jobDesc_id", "value": $('#dropdown_jobDesc').val() });
            aoData.push({ "name": "depObj_id", "value": $('#dropdown_dep_Objective').val() });
            aoData.push({ "name": "strategy_id", "value": $('#dropdown_strategy').val() });
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
                                return "<a class='glyphicon glyphicon-list-alt a_clickable' href='/ViewEntrybyEmployee/QuestionSolutions/" + row[0] + "'></a>"
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
            url: '/ViewEntrybyDetail/FillObjectives',
            type: "GET",
            dataType: 'JSON',
            data: { DepId: dep_id },
            success: function (jobs) {
                $("#dropdown_dep_Objective").html(""); // clear before appending new list 

                $.each(jobs, function (i, job) {
                    $("#dropdown_dep_Objective").append($('<option></option>').val(job.Value).html(job.Text));
                });

            },
            error: function (e) {
                alert('err:' + e.toString());
            }
        });


    });

    $("#dropdown_job").change(function () {

        var job_id = $('#dropdown_job').val();
        $.ajax({
            url: '/ViewEntrybyDetail/FillJobDescriptions',
            type: "GET",
            dataType: 'JSON',
            data: { JobId: job_id },
            success: function (jobs) {
                $("#dropdown_jobDesc").html(""); // clear before appending new list 

                $.each(jobs, function (i, job) {
                    $("#dropdown_jobDesc").append($('<option></option>').val(job.Value).html(job.Text));
                });

            },
            error: function (e) {
                alert('err:' + e.toString());
            }
        });


    });
 });

var details = function (s) {
    $("#question").html("شرح مسئله: " + s[0].fullQuestion);
    $("#keywords").html("کلید واژه ها: " + s[0].keywords);
    $("#strategy_name").html("شرح استراتژی: " + (s[0].strategy == null ? '' : s[0].strategy));
    $("#dep_objective").html("هدف واحد سازمانی: " + (s[0].dep_objective == null ? '' : s[0].dep_objective));
    $("#job_desc").html("شرح شغل: " + (s[0].jobdesc == null ? '' : s[0].jobdesc));
    $('#DetailModal').modal('show');
}

var delete_dialog = function (q_id) {

    var url = "/ViewEntrybyDetail/Delete_Question"; // the url to the controller
    $.get(url + '/' + q_id, function (data) {
        $('#confirm-container').html(data);
        $('#DeleteModal').modal('show');
    });
}



