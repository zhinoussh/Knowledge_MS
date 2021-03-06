﻿/***USED IN Solution/NewSolution*************/

$(document).ready(function () {

    var oTable = $('#UploadDT').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/User/Solution/UploadAjaxHandler_NewSOlution",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "solution_id", "value": $('#hd_id_new_solution').val() });

        },
        "bProcessing": true,
        "bFilter":false,
        "bSort":false,
        "pagingType": "numbers"
        , "aoColumns": [
                        {
                            "sName": "ID",
                            "bSearchable": false,
                            "bSortable": false,
                            "bVisible": false
                        },
                         {
                             "sName": "filePath",
                             "bSearchable": false,
                             "bSortable": false,
                             "bVisible": false
                         },
                        {
                            "sName": "radif", "sWidth": '2%', "sClass": "dt-body-center"
                                , "bSearchable": false, "bSortable": false
                        },
                        { "sName": "upload", "sWidth": '80%' }
                        , {
                            "sName": "confirm_status",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                            , "sClass": "dt-body-center",
                            "mRender": function (data, type, row) {

                                if (data == "True") {
                                    return '<div class=\"checkbox checkbox-info\"><input  type=\"checkbox\" id=\"check_confirm\" disabled checked value="' + data + '"><label for=\"check_confirm\"></label></div>';
                                } else {
                                    return '<div class=\"checkbox checkbox-info\"><input  type=\"checkbox\" id=\"check_confirm\" disabled value="' + data + '"><label for=\"check_confirm\"></label></div>';
                                }
                            }
                        },
                        {
                            "sName": "download",
                            "sWidth": '2%',
                            "bSearchable": false,
                            "bSortable": false,
                            "sDefaultContent": " "
                             , "sClass": "dt-body-center",
                             "mRender": function (data, type, row) {
                                 return "<a class='glyphicon glyphicon-download a_clickable' onclick='download_file(" + row[0] + ")'></a>"

                            }
                        }
                           , {
                               "sName": "Edit",
                               "sWidth": '3%',
                               "bSearchable": false,
                               "bSortable": false,
                               "sDefaultContent": " "
                            , "sClass": "dt-body-center"
                            , "mRender": function (data, type, row) {
                                var param_array = { upload_id: row[0], upload_desc: row[3] };
                                var obj = [];
                                obj.push(param_array);
                                return "<a class='glyphicon glyphicon-edit a_clickable' onclick='edit_upload("+JSON.stringify(obj)+")'></a>"
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

    //Uploadify

    $('#file_upload').uploadify({
        'swf': "../../../Content/img/uploadify.swf",
        'uploader': "/User/Solution/Upload",
        'formData': { '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val() },
        'onUploadStart': function (file) {
            $('#file_upload').uploadify('settings', 'formData', {
                'solution_id': $('#hd_id_new_solution').val(),
                'question_id': $('#hd_id_question').val(),
                'upload_desc': $('#txt_upload_desc').val()
            });
        },
        'onSWFReady': function () {
            $('#file_upload').uploadify('disable', true);
        },
        'onUploadSuccess': function (file, data, response) {
            $("#alert_success_upload").html('File uploaded successfully.');
            $("#div_alert_upload").slideDown(500);
            $("#hd_id_new_solution").val(data);
            var $STTable = $("#UploadDT").dataTable({ bRetrieve: true });
            $STTable.fnDraw();

            $('#txt_upload_desc').val('');
            $('#file_upload').uploadify('disable', true);
        //data is whatever you return from the server
        //we're sending the URL from the server so we append this as an image to the #uploaded div
        //  $("#uploaded").append("<img src='" + data + "' alt='Uploaded Image' />");
    },
        'onError': function (a, b, c, d) {
            if (d.status == 404)
                alert('Could not find upload script.');
            else if (d.type === "HTTP")
                alert('error ' + d.type + ": " + d.status);
            else if (d.type === "File Size")
                alert(c.name + ' ' + d.type + ' Limit: ' + Math.round(d.sizeLimit / 1024) + 'KB');
            else
                alert('error ' + d.type + ": " + d.text);
        }
    });
    
});

$(document).on("keyup", '#txt_upload_desc', function () {

        if( $('#txt_upload_desc').val()!='')
            $('#file_upload').uploadify('disable', false);
        else
            $('#file_upload').uploadify('disable', true);

});

$(document).on('click', '#close_alert_upload', function () {
    $("#div_alert_upload").slideUp(500);
    return false;
});



var download_file = function (upload_id) {
   
    $.ajax(
           {
               url: '/User/Solution/DownloadFile',
               contentType: 'application/json; charset=utf-8',
               datatype: 'json',
               data: {
                   uploadID: upload_id
               },
               type: "GET",
               success: function () {
                   window.location = '/User/Solution/DownloadFile?uploadID=' + upload_id;
               }
           });
}



var delete_dialog = function (upload_id) {

    var url = "/User/Solution/Delete_Upload"; // the url to the controller
    $.get(url + '/' + upload_id, function (data) {
        $('#ModalContainer').html(data);
        $('#ModalContainer').find("#DeleteModal").modal('show');
    });
}

var edit_upload = function (upload) {
    var NewSolutionViewModel = {
        upload_id: upload[0].upload_id,
        file_description: upload[0].upload_desc
    };

    $.ajax({
        type: 'Get',
        data: NewSolutionViewModel,
        url:'/User/Solution/EditUpload',
        success: function (result) {
            $('#ModalContainer').html(result);
            $('#ModalContainer').find("#EditModal").modal('show');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });

}

var SuccessDelete = function (result) {
    if (result.msg) {
        $('#DeleteModal').modal('hide');
        $("#alert_success_upload").html(result.msg);
        $("#div_alert_upload").slideDown(500);
        var $STTable = $("#UploadDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();
    }
}

var SuccessMessage_Editupload = function (result)
{
    if (result.msg) {
        $('#EditModal').modal('hide');
        $("#alert_success_upload").html(result.msg);
        $("#div_alert_upload").slideDown(500);
        var $STTable = $("#UploadDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();


    }
}

  
var SuccessMessage_AddSolution = function (result) {
    if (result.msg) {
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);

        $('#hd_id_new_solution').val(result.new_solution_id);

    }

}