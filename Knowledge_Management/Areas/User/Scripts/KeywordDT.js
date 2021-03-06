﻿/***USED IN SearchInfo/SearchbyKeyword*************/
$(document).ready(function () {

    var oTable = $('#KeywordDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/User/SearchInfo/KeywordAjaxHandler",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "jobdesc_id", "value": $('#dropdown_jobDesc').val() });
            aoData.push({ "name": "depObj_id", "value": $('#dropdown_dep_Objective').val() });
            aoData.push({ "name": "st_id", "value": $('#dropdown_strategy').val() });
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
                        { "sName": "keyword", "sWidth": '80%' }
                       , {
                              "sName": "details",
                              "sWidth": '2%',
                              "bSearchable": false,
                              "bSortable": false,
                              "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                              "mRender": function (data, type, row) {
                                  var param_array = {depobj_id: row[1], jobdesc_id: row[2], st_id: row[3],keyword:row[5]};
                                  var param_obj = [];
                                  param_obj.push(param_array);
                                  // var o = JSON.parse(param_obj);
                                  return "<a class='glyphicon glyphicon-list a_clickable' onclick='details(" + JSON.stringify(param_obj) + ");'></a>"

                              }
                          }
                        , {
                            "sName": "Questions",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center"
                            , "mRender": function (data, type, row) {
                                return "<a class='glyphicon glyphicon-list-alt a_clickable' href=/User/SearchInfo/SearchAll/" + row[0] + "></a>"
                            }
                        }

        ]
    });

    $("#search_btn").click(function () {
        var $STTable = $("#KeywordDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    });

});


var details = function (s) {
    var KeywordDetailViewModel = {
        keyword: s[0].keyword,
        strategyId: s[0].st_id,
        depObjId: s[0].depobj_id,
        jobDescId: s[0].jobdesc_id,
    };

    $.ajax({
        type: 'Get',
        url: '/User/SearchInfo/KeywordDetails',
        data: KeywordDetailViewModel,
        success: function (result) {
            $("#ModalContainer").html(result);
            $("#ModalContainer").find("#DetailModal").modal('show');
        }
        , error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });

}







