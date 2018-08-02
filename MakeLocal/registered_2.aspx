<%@ Page Language="C#" AutoEventWireup="true" CodeFile="registered_2.aspx.cs" Inherits="registered_2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <link rel="stylesheet" href="css/file-upload.css" type="text/css" />
    <link rel="stylesheet" href="css/jquery-ui.css">

    <link rel="stylesheet" href="css/style.css">
    <link rel="stylesheet" href="css/registered_2.css">
    <meta name="viewport" content="width=device-width, initial-scale=1">



    <script src="Scripts/jquery-1.12.4.js"></script>
    <script src="Scripts/jquery-ui.js"></script>
    <script src="Scripts/datepicker-ja.js"></script>
    <script type="text/javascript">

        // 顯示讀取遮罩
        function ShowProgressBar() {
            displayProgress();
            displayMaskFrame();
        }

        // 隱藏讀取遮罩
        function HideProgressBar() {
            var progress = $('#divProgress');
            var maskFrame = $("#divMaskFrame");
            progress.hide();
            maskFrame.hide();
        }
        // 顯示讀取畫面
        function displayProgress() {
            var w = $(document).width();
            var h = $(window).height();
            var progress = $('#divProgress');
            progress.css({ "z-index": 999999, "top": (h / 2) - (progress.height() / 2), "left": (w / 2) - (progress.width() / 2) });
            progress.show();
        }
        // 顯示遮罩畫面
        function displayMaskFrame() {
            var w = $(window).width();
            var h = $(document).height();
            var maskFrame = $("#divMaskFrame");
            maskFrame.css({ "z-index": 999998, "opacity": 0.7, "width": w, "height": h });
            maskFrame.show();
        }

        $(document).ready(function () {

            var counter = 2;

            $("#addButton").click(function () {

                if (counter > 10) {
                    alert("Only 10 textboxes allow");
                    return false;
                }

                $("#KidGroup").append('<div id="KidDiv' + counter + '"><table style="width:100%;"> <tr> <td width="40%" valign="top"> <table style="width:100%;"> <tr> <td width="50%"> <span>お名前</span> </td> <td width="50%" valign="bottom"> <span style ="color: #FF5050; font-size: XX-Small;">※必須</span> </td> </tr> </table> </td> <td width="60%"> <table style="width:100%;"> <tr> <td width="50%"> <input id="real_first_name_text' + counter + '" type="text" name="real_first_name_text' + counter + '" class="textbox" placeholder="姓" style="height:20px;width:90%;" /> <br /><br /><span id="real_first_name_la' + counter + '" style="color: #FF0000"></span> </td> <td width="50%"> <input id="real_second_name_text' + counter + '" type="text" name="real_second_name_text' + counter + '" class="textbox" placeholder="名" style="height:20px;width:90%;" /> <br /><br /><span id="real_second_name_la' + counter + '" style="color: #FF0000"></span> </td> </tr> </table> </td> </tr> <tr> <td width="40%" valign="top"> <table style="width:100%;"> <tr> <td width="50%"> <span>フリガナ</span> </td> <td width="50%" valign="bottom"> <span style ="color: #FF5050; font-size: XX-Small;">※必須</span> </td> </tr> </table> </td> <td width="60%"> <table style="width:100%;"> <tr> <td width="50%"> <input id="real_spell_first_name_text' + counter + '" type="text" name="real_spell_first_name_text' + counter + '" class="textbox" placeholder="セイ" style="height:20px;width:90%;" /> <br /><br /><span id="real_spell_first_name_la' + counter + '" style="color: #FF0000"></span> </td> <td width="50%"> <input id="real_spell_second_name_text' + counter + '" type="text" name="real_spell_second_name_text' + counter + '" class="textbox" placeholder="メイ" style="height:20px;width:90%;" /> <br /><br /><span id="real_spell_second_name_la' + counter + '" style="color: #FF0000"></span> </td> </tr> </table> </td> </tr> <tr> <td width="40%" valign="top"> <table style="width:100%;"> <tr> <td width="50%"> <span>性别</span> </td> <td width="50%" valign="bottom"> <span style ="color: #FF5050; font-size: XX-Small;">※必須</span> </td> </tr> </table> </td> <td width="60%"> <input type="radio" name="sex' + counter + '" value="Girl">女性 <input type="radio" name="sex' + counter + '" value="Boy">男性 <br /> <br /><span id="sex_la' + counter + '" style="color: #FF0000"></span> </td> </tr> <tr> <td width="40%" valign="center"> <table style="width:100%;"> <tr> <td width="50%"> <span>生年月日</span> </td> <td width="50%" valign="bottom"> <span style ="color: #FF5050; font-size: XX-Small;">※必須</span> </td> </tr> </table> <br /> </td> <td width="60%" valign="center"> <p><input type="text" name="datepicker' + counter + '" id="datepicker' + counter + '" class="textbox" placeholder="2016/01/01" readonly></p> <script> $(function () { $("#datepicker' + counter + '").datepicker({ format: "yyyy/mm/dd", language: "ja", changeMonth: true, changeYear: true, autoclose: true, clearBtn: true, clear: "閉じる" }); });</' + 'script><br /><span id="date_la' + counter + '" style="color: #FF0000"></span> </td> </tr> <tr> <td> <span>保育園/学校名</span> <br /> <br /> </td> <td> <input name="school_name_text' + counter + '" type="text" id="school_name_text' + counter + '" class="textbox" placeholder="通っている保育園/病院を入力" style="height:20px;width:100%;"> </td> </tr> <tr> <td> <span>かかりつけ医院名</span> <br /> <br /> </td> <td> <input name="hospital_name_text' + counter + '" type="text" id="hospital_name_text' + counter + '" class="textbox" placeholder="病院名/診療所名を入力" style="height:20px;width:100%;"> </td> </tr> <tr> <td> <span>アレルギー/持病</span> <br /> <br /> </td> <td> <textarea name="sick_name_text' + counter + '" rows="2" cols="20" wrap="off" id="sick_name_text' + counter + '" class="textbox" placeholder="例小麦アレルキーなど" style="height:50px;width:100%;"></textarea> </td> </tr> </table> <hr/></div>');



                counter++;
            });

            $("#removeButton").click(function () {
                if (counter == 2) {
                    alert("No more Kid to remove");
                    return false;
                }

                counter--;

                $("#KidDiv" + counter).remove();

            });

            $("#Button1").click(function () {
                var checkfin = true;
                var param1 = "<%= Session["id"]%>".toString();
                for (i = 1; i < counter; i++) {
                    $('#real_first_name_la' + i).text("");
                    $('#real_second_name_la' + i).text("");
                    $('#real_spell_first_name_la' + i).text("");
                    $('#real_spell_second_name_la' + i).text("");
                    $('#date_la' + i).text("");
                    $('#sex_la' + i).text("");


                    if ($('#real_first_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() != "" && $('#real_second_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() != "" && $('#real_spell_first_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() != "" && $('#real_spell_second_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() != "" && $('#datepicker' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() != "" && document.querySelector('input[name="sex' + i + '"]:checked') != null) {

                    } else {
                        checkfin = false;
                        if ($('#real_first_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() == "") {
                            $("#real_first_name_la" + i).text("未入力");
                        }
                        if ($('#real_second_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() == "") {
                            $('#real_second_name_la' + i).text("未入力");
                        }
                        if ($('#real_spell_first_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() == "") {
                            $('#real_spell_first_name_la' + i).text("未入力");
                        }
                        if ($('#real_spell_second_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() == "") {
                            $('#real_spell_second_name_la' + i).text("未入力");
                        }
                        if ($('#datepicker' + i).val() == "") {
                            $('#date_la' + i).text("未入力");
                        }
                        if (document.querySelector('input[name="sex' + i + '"]:checked') == null) {
                            $('#sex_la' + i).text("未入力");
                        }
                    }
                }
                HideProgressBar();

                if (checkfin) {
                    ShowProgressBar();
                    for (i = 1; i < counter; i++) {
                        $('#real_first_name_la' + i).text("");
                        $('#real_second_name_la' + i).text("");
                        $('#real_spell_first_name_la' + i).text("");
                        $('#real_spell_second_name_la' + i).text("");
                        $('#date_la' + i).text("");
                        $('#sex_la' + i).text("");
                        $.ajax({
                            type: "POST",
                            url: "registered_2.aspx/Save",
                            data: "{param1: '" + param1 + "' , param2 :'" + $('#real_first_name_text' + i).val() + "',param3 :'" + $('#real_second_name_text' + i).val() + "',param4 :'" + $('#real_spell_first_name_text' + i).val() + "',param5 :'" + $('#real_spell_second_name_text' + i).val() + "',param6 :'" + $('#datepicker' + i).val() + "',param7 :'" + $('#school_name_text' + i).val() + "',param8 :'" + $('#hospital_name_text' + i).val() + "',param9 :'" + $('#sick_name_text' + i).val().replace(/\r?\n/g, '<br />') + "',param10 :'" + document.querySelector('input[name="sex' + i + '"]:checked').value + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                                //console.log(result.d);
                                //alert(result.d);

                            },
                            error: function (result) {
                                //console.log(result.Message);
                                //alert(result.d);
                            }
                        });
                    }
                    window.location.href = "registered_2_1.aspx";
                }

            });


        });

        function escapeRegExp(str) {
            return str.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="header">
            <table style="width: 100%; height: 100%">
                <tr>
                    <td align="left" width="15%">

                        <table style="width: 100%;">
                            <tr>
                                <td width="5%">&nbsp;</td>

                                <td class="rin">
                                    <asp:Image ID="Label_logo" Style="width: 60px; height: auto; cursor: pointer; margin-left: 3px" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>
                                <!--hehe-->
                            </tr>
                        </table>
                    </td>
                    <td width="55%">&nbsp;</td>
                    <td width="30%">
                        <table style="width: 100%;">
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div>

            <table style="width: 100%;">
                <tr>
                    <td class="space">&nbsp;</td>
                    <td width="50%" align="center">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/step/2/1.PNG" />
                    </td>
                    <td class="space">&nbsp;</td>
                </tr>
                <tr>
                    <td class="space">&nbsp;</td>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="お子様の情報"></asp:Label>
                                    &nbsp;<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="KidDiv1">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td width="40%" valign="top">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td width="50%">
                                                                <span>お名前</span>
                                                            </td>
                                                            <td width="50%" valign="bottom">
                                                                <span style='color: #FF5050; font-size: XX-Small;'>※必須</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td width="60%">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td width="50%">
                                                                <input id="real_first_name_text1" type="text" name='real_first_name_text1' class='textbox' placeholder='姓' style='height: 20px; width: 90%;' />
                                                                <br />
                                                                <br />
                                                                <span id="real_first_name_la1" style="color: #FF0000"></span>
                                                            </td>
                                                            <td width="50%">
                                                                <input id="real_second_name_text1" type="text" name='real_second_name_text1' class='textbox' placeholder='名' style='height: 20px; width: 90%;' />
                                                                <br />
                                                                <br />
                                                                <span id="real_second_name_la1" style="color: #FF0000"></span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="40%" valign="top">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td width="50%">
                                                                <span>フリガナ</span>
                                                            </td>
                                                            <td width="50%" valign="bottom">
                                                                <span style='color: #FF5050; font-size: XX-Small;'>※必須</span>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </td>
                                                <td width="60%">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td width="50%">
                                                                <input id="real_spell_first_name_text1" type="text" name='real_spell_first_name_text1' class='textbox' placeholder='セイ' style='height: 20px; width: 90%;' />
                                                                <br />
                                                                <br />
                                                                <span id="real_spell_first_name_la1" style="color: #FF0000"></span>
                                                            </td>
                                                            <td width="50%">
                                                                <input id="real_spell_second_name_text1" type="text" name='real_spell_second_name_text1' class='textbox' placeholder='メイ' style='height: 20px; width: 90%;' />
                                                                <br />
                                                                <br />
                                                                <span id="real_spell_second_name_la1" style="color: #FF0000"></span>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="40%" valign="top">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td width="50%">
                                                                <span>性别</span>
                                                            </td>
                                                            <td width="50%" valign="bottom">
                                                                <span style='color: #FF5050; font-size: XX-Small;'>※必須</span>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </td>
                                                <td width="60%">
                                                    <input type="radio" name="sex1" value="Girl">女性
                                                <input type="radio" name="sex1" value="Boy">男性
                                                <br />
                                                    <br />
                                                    <span id="sex_la1" style="color: #FF0000"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="40%" valign="center">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td width="50%">
                                                                <span>生年月日</span>
                                                            </td>
                                                            <td width="50%" valign="bottom">
                                                                <span style='color: #FF5050; font-size: XX-Small;'>※必須</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                </td>
                                                <td width="60%" valign="center">
                                                    <p>
                                                        <input type='text' name='datepicker1' id='datepicker1' class='textbox' placeholder='2016/01/01' readonly></p>
                                                    <script>
                                                        $(function () {
                                                            $("#datepicker1").datepicker({
                                                                format: 'yyyy/mm/dd',
                                                                language: 'ja',
                                                                changeMonth: true,
                                                                changeYear: true,
                                                                autoclose: true, // これ
                                                                clearBtn: true,
                                                                clear: '閉じる'
                                                            });
                                                        });
                                                    </script>
                                                    <br />
                                                    <span id="date_la1" style="color: #FF0000"></span>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>保育園/学校名</span>
                                                    <br />
                                                    <br />
                                                </td>
                                                <td>
                                                    <input name="school_name_text1" type="text" id="school_name_text1" class="textbox" placeholder="通っている保育園/病院を入力" style="height: 20px; width: 100%;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>かかりつけ医院名</span>
                                                    <br />
                                                    <br />
                                                </td>
                                                <td>
                                                    <input name="hospital_name_text1" type="text" id="hospital_name_text1" class="textbox" placeholder="病院名/診療所名を入力" style="height: 20px; width: 100%;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>アレルギー/持病</span>
                                                    <br />
                                                    <br />
                                                </td>
                                                <td>
                                                    <textarea name="sick_name_text1" rows="2" cols="20" wrap="off" id="sick_name_text1" class="textbox" placeholder="例小麦アレルキーなど" style="height: 50px; width: 100%;"></textarea>
                                                </td>
                                            </tr>
                                        </table>
                                        <hr />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="KidGroup">
                                    </div>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:HyperLink ID='addButton' runat="server" NavigateUrl="javascript:void(0);"
                                        Target="_blank" Font-Size="Small" Font-Underline="False">+別のお子様を追加</asp:HyperLink>
                                    <br />
                                    <asp:HyperLink ID='removeButton' runat="server" NavigateUrl="javascript:void(0);"
                                        Target="_blank" Font-Size="Small" Font-Underline="False">-別のお子様を削除</asp:HyperLink>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <input id="Button1" type="button" value="保存して次へ" class="file-upload" style="width: 50%;" onclick="ShowProgressBar();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="space">&nbsp;</td>
                </tr>
                <tr>
                    <td class="space">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="space">&nbsp;</td>
                </tr>
            </table>

        </div>
        <div id="divProgress" style="text-align: center; display: none; position: fixed; top: 50%; left: 50%;">
            <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" />
            <br />
            <font color="#95989A" size="2px">読み込み中</font>
        </div>
        <div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px; position: absolute; top: 0px;">
        </div>
    </form>
</body>
</html>
