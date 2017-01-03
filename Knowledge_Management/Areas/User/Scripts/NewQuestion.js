$(document).ready(function () {

    if ($("#hd_keywords").val() != '')
        set_keywords($("#hd_keywords").val());
});

var set_keywords = function (keys) {
 
    //cleare keyword part
    var field1_input = '<input autocomplete="off" class="txt_keyword" id="field1" name="field1"  type="text" placeholder="Enter Keyword" data-items="8">';
    field1_input += '<button id="b1" class="btn add-more" type="button">+</button>';
    $("#field").html(field1_input);
    var next = $("#count").val("1");

    var array = keys.split(",");
    var count_key = array.length;
    if (count_key > 0) {
        $("#field1").val(array[0]);
        for (j = 1; j < count_key ; j++) {
            $(".add-more").click();
            $("#field" + (j + 1)).val(array[j]);
        }
    }
}

var SuccessMessage = function (result) {
    if (result.msg) {
        $("#alert_success").html(result.msg);
        $("#div_alert").slideDown(500);

        $("#frmQuestion").find('input:text').val("");
        $("#hd_id_question").val("0");

        $('#dropdown_dep_Objective option:selected').removeAttr('selected');
        $('#dropdown_jobDesc option:selected').removeAttr('selected');
        $('#dropdown_strategy option:selected').removeAttr('selected');

        //cleare keyword part
        var field1_input = '<input autocomplete="off" class="txt_keyword" id="field1" name="field1"  type="text" placeholder="Enter keyword" data-items="8">';
        field1_input += '<button id="b1" class="btn add-more" type="button">+</button>';
        $("#field").html(field1_input);
        var next = $("#count").val("1");

        var $STTable = $("#QuestionDT").dataTable({ bRetrieve: true });
        $STTable.fnDraw();

    }
}