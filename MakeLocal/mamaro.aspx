<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mamaro.aspx.cs" Inherits="mamaro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

    <title></title>
    <script src="Scripts/jquery-1.12.4.js"></script>
    <!-- Latest compiled and minified CSS -->
<link rel="stylesheet" type="text/css" href="css/bootstrap.min.css">

<!-- Optional theme -->
<link rel="stylesheet" type="text/css" href="css/bootstrap-theme.min.css">

<!-- Latest compiled and minified JavaScript -->
<script src="js/bootstrap.min.js" type="text/javascript"></script>
    <link href='//fonts.googleapis.com/css?family=Khula' rel="stylesheet">

    <style>
        html {
    height: 100%;
    width:100%;
    /*-webkit-background-size: cover;
  -moz-background-size: cover;
  -o-background-size: cover;
  background-size: cover;*/
}

body {
    /*position: fixed;
  top: 0;
  left: 0;*/
	font-family: "Noto Sans", "Noto Sans CJK JP", sans-serif;
  /* Preserve aspet ratio */
  min-width: 100%;
  min-height: 100%;
}

.fullheight {
    /*position: fixed;
  top: 0;
  left: 0; */

  /* Preserve aspet ratio */
  min-width: 100%;
  min-height: 100%;
}
.lefttop {
    margin-left: 20px;
}
        .temp_la
        {
            font-family: 'Khula', sans-serif;
            font-size: 20px;
            color: #323837;
        }
        .temp_la1
        {
            font-family: 'Khula', sans-serif;
            font-size: 14px;
            color: #323837;
        }
        .middle_pan
        {
            background-image: url("img/img/ver1/mamaro_console_2.jpg");
            background-repeat: no-repeat;
            background-size: 100% 100%;
            height:100%;
        }
        .middle_center
        {
            margin:50px 50px 50px 50px;
            background-image: url("img/img/ver1/back4.png");
            background-repeat: no-repeat;
            background-size: 100% 100%;
            height:80%;
            opacity: 0.9;
            filter: alpha(opacity=90); /* For IE8 and earlier */
        }
        .bottom_logo
        {
            font-family: 'HelveticaNeue-CondensedBold' , Baskerville;
            font-size: 13px;
            color: #9FA0A0;
        }
         .middle_icon_a
        {
            height:250px;
            width:250px;
	        border-radius: 10px;
            cursor: pointer;
        }
        .middle_icon
        {
            margin-right:40px;
            height:250px;
            width:250px;
	        border-radius: 10px;
            cursor: pointer;
            opacity: 1;
	        filter: alpha(opacity=100);

        }
        .middle_icon:hover
{
	opacity: 0.7;
	filter: alpha(opacity=70);
}
        .middle_title
        {
            font-family: "Noto Sans", "Noto Sans CJK JP", sans-serif;
            font-size: 38px;
            color: #ffffff;
            cursor: pointer;
            word-break: break-all;
            opacity: 1;
	        filter: alpha(opacity=100);
        }
        .middle_title:hover
        {
            opacity: 0.7;
	        filter: alpha(opacity=70);
        }
         .middle_des
        {
            font-family: "Noto Sans", "Noto Sans CJK JP", sans-serif;
            font-size: 18px;
            color: #ffffff;
            cursor: pointer;
            word-break: break-all;
             opacity: 1;
	        filter: alpha(opacity=100);
        }
         .middle_des:hover
        {
            opacity: 0.7;
	        filter: alpha(opacity=70);
        }
        .middle_text
        {
            vertical-align:middle;
        }
        .middle_text1
        {
            vertical-align:top;
            height:150px;
        }
        a {
text-decoration: none;

}
        a:hover {
text-decoration: none;
}
        #white-background{
                display: none;
                width: 100%;
                height: 100%;
                position: fixed;
                top: 0px;
                left: 0px;
                background-color: #323837;
                opacity: 0.7;
                z-index: 999998;
            }

        .dlg{
                /*initially dialog box is hidden*/
                display: none;
                position: fixed;
                width: 90%;
                margin-left: 5%;
                z-index: 999999;
                border-radius: 10px;
            }
        .dlg_fin
        {
             /*initially dialog box is hidden*/
                display: none;
                position: fixed;
                width: 90%;
                margin-left: 5%;
                z-index: 999999;
                border-radius: 10px;
                cursor: pointer;
        }

            .dlgh{
                background-color: #6AD5AC;
                color: white;
                font-size: 20px;
                padding: 10px;
                margin: 10px 10px 0px 10px;
            }

            .dlgh1{
                background-color: #ffffff;
                color: white;
                font-size: 20px;
                padding: 10px;
                margin: 10px 10px 0px 10px;
            }

            .dlgb{
                background-color: white;
                color: black;
                font-size: 14px;
                padding: 10px;
                margin: 0px 10px 0px 10px;

            }
            .dlgb_fin{
                background-color: #6AD5AC;
                color: white;
                font-size: 14px;
                padding: 10px;
                margin: 0px 10px 0px 10px;
            }
        .dlgf_fin
        {
             background-color: #6AD5AC;
                text-align: center;
                padding: 10px;
                margin: 0px 10px 10px 10px;
                cursor: pointer;
        }
            .dlgf{
                background-color: #ffffff;
                text-align: center;
                padding: 10px;
                margin: 0px 10px 10px 10px;
            }
             .dlgf button{
                background-color: #f48686;
                color: white;
                padding: 5px;
                border: 0px;
            }
        .dlg_text
        {
            color:#323837;

        }
        .dlg_text_title
        {
            font-size:30px;
        }
        .dlg_text_err
        {
            color:#FF3C3C;

        }
         .dlg_text_des1
        {
            font-size:18px;
        }
        .dlg_text_des
        {
            font-size:14px;
        }
        select {
            font-family: 'MS UI Gothic', sans-serif;
        font-size: 30px;
        font-weight: bold;
    }
        .QAnext
        {
            cursor: pointer;
            opacity: 1;
	        filter: alpha(opacity=100);
        }
        .QAnext:hover {
            opacity: 0.8;
	        filter: alpha(opacity=80);
        }
        .choice_div {
            width:100%;
            height:100%;
            cursor: pointer;
        }
    </style>
    <script>

        function dlg_show() {

            var whitebg = document.getElementById("white-background");
            var dlg = document.getElementById("dlgbox_QA");
            whitebg.style.display = "block";
            dlg.style.display = "block";

            var winWidth = window.innerWidth;
            var winHeight = window.innerHeight;

            dlg.style.left = 0 + "px";
            dlg.style.top = winHeight / 15 + "px";
        }

        //language
        function outputSelectedValueAndText(obj) {
            /*
             * obj は select タグであり、selectedIndex プロパティには
             * 変更後の選択項目のインデックスが格納されています
             */
            var idx = obj.selectedIndex;
            var value = obj.options[idx].value; // 値
            var text = obj.options[idx].text;  // 表示テキスト

            // 値とテキストをコンソールに出力
            console.log('value = ' + value + ', ' + 'text = ' + text);

            window.sessionStorage.setItem(['lan'], [text]);

            var vaaa = window.sessionStorage.getItem(['lan']);
            if (vaaa == '日本語') {
                document.getElementById('A0_1').innerHTML = "お子さまのご年齢は";
                document.getElementById('A0_2').innerHTML = "歳";
                document.getElementById('A0_3').innerHTML = "ヶ月";
                document.getElementById('A0_4').innerHTML = "あなたは";
                document.getElementById('parent_select').options[0].innerHTML = "ママ";
                document.getElementById('parent_select').options[1].innerHTML = "パパ";

                document.getElementById('A1_0').innerHTML = "mamaroがどこにあったら便利? ( 複数選択可 )";
                document.getElementById('A1_1').innerHTML = "テーマパーク";
                document.getElementById('A1_2').innerHTML = "ショッピングモール";
                document.getElementById('A1_3').innerHTML = "病院";
                document.getElementById('A1_4').innerHTML = "市役所";
                document.getElementById('A1_5').innerHTML = "競技場";
                document.getElementById('A1_6').innerHTML = "駅";

                document.getElementById('A2_0').innerHTML = "mamaroをどこで知った? ( 複数選択可 )";
                document.getElementById('A2_1').innerHTML = "新聞";
                document.getElementById('A2_2').innerHTML = "ネットニュース";
                document.getElementById('A2_3').innerHTML = "TrimのHP";
                document.getElementById('A2_4').innerHTML = "知人から";
                document.getElementById('A2_5').innerHTML = "babymapで";
                document.getElementById('A2_6').innerHTML = "通りすがりに見つけた";

                document.getElementById('A3_0').innerHTML = "mamaroに 「もっとこんな機能が欲しい。こんなのがあったら嬉しい。」";
                document.getElementById('A3_0_1').innerHTML = "というものはありますか？";
                document.getElementById('A3_1').innerHTML = "子様の身長‧体重を測れる";
                document.getElementById('A3_2').innerHTML = "医療相談";
                document.getElementById('A3_3').innerHTML = "DVや家庭内事情について相談";
                document.getElementById('A3_4').innerHTML = "ゲーム機能";
                document.getElementById('A3_5').innerHTML = "買い物の決済";
                document.getElementById('A3_6').innerHTML = "ママ友を増やせる";

                document.getElementById('A4_0').innerHTML = "授乳室は個室派？わいわい派？";
                document.getElementById('A4_1').innerHTML = "個室派";
                document.getElementById('A4_2').innerHTML = "わいわい派";

                document.getElementById('final_1').innerHTML = "ア ン ケ ー ト に ご 協 力 い た だ き";
                document.getElementById('final_2').innerHTML = "あ り が と う ご ざ い ま し た 。";
            } else if (vaaa == 'English') {
                document.getElementById('A0_1').innerHTML = "How old is your child?";
                document.getElementById('A0_2').innerHTML = "year";
                document.getElementById('A0_3').innerHTML = "months old";
                document.getElementById('A0_4').innerHTML = "You are";
                document.getElementById('parent_select').options[0].innerHTML = "Mam";
                document.getElementById('parent_select').options[1].innerHTML = "Dad";

                document.getElementById('A1_0').innerHTML = "Where would be the best location for mamaro? (possible multiple choice answers)";
                document.getElementById('A1_1').innerHTML = "amusement park";
                document.getElementById('A1_2').innerHTML = "shopping malls";
                document.getElementById('A1_3').innerHTML = "hospitals";
                document.getElementById('A1_4').innerHTML = "civil halls";
                document.getElementById('A1_5').innerHTML = "sports stadium";
                document.getElementById('A1_6').innerHTML = "stations";

                document.getElementById('A2_0').innerHTML = "How did you know mamaro? (possible multiple choice answers)";
                document.getElementById('A2_1').innerHTML = "news paper";
                document.getElementById('A2_2').innerHTML = "news web site";
                document.getElementById('A2_3').innerHTML = "Trim's HP";
                document.getElementById('A2_4').innerHTML = "Hear it";
                document.getElementById('A2_5').innerHTML = "babymap";
                document.getElementById('A2_6').innerHTML = "by seeing";

                document.getElementById('A3_0').innerHTML = "Do you have your opinion for mamaro like that <I want";
                document.getElementById('A3_0_1').innerHTML = "more features like this, I am glad if there is such a thing.>";
                document.getElementById('A3_1').innerHTML = "Measure the child's height and weight";
                document.getElementById('A3_2').innerHTML = "Medical";
                document.getElementById('A3_3').innerHTML = "Consultation about DV";
                document.getElementById('A3_4').innerHTML = "Game";
                document.getElementById('A3_5').innerHTML = "Shopping";
                document.getElementById('A3_6').innerHTML = "Make friend with mother";

                document.getElementById('A4_0').innerHTML = "Which would you prefer?";
                document.getElementById('A4_1').innerHTML = "Private room";
                document.getElementById('A4_2').innerHTML = "Public";

                document.getElementById('final_1').innerHTML = "Thank you very much for your corporation!";
                document.getElementById('final_2').innerHTML = "";
            } else if (vaaa == '繁體中文') {
                document.getElementById('A0_1').innerHTML = "你的小孩幾歲?";
                document.getElementById('A0_2').innerHTML = "歲";
                document.getElementById('A0_3').innerHTML = "個月";
                document.getElementById('A0_4').innerHTML = "你是";
                document.getElementById('parent_select').options[0].innerHTML = "媽媽";
                document.getElementById('parent_select').options[1].innerHTML = "爸爸";

                document.getElementById('A1_0').innerHTML = "哪裡是放mamaro最好的地方? (複選)";
                document.getElementById('A1_1').innerHTML = "主題樂園";
                document.getElementById('A1_2').innerHTML = "購物中心";
                document.getElementById('A1_3').innerHTML = "醫院";
                document.getElementById('A1_4').innerHTML = "政府單位";
                document.getElementById('A1_5').innerHTML = "運動中心";
                document.getElementById('A1_6').innerHTML = "車站";

                document.getElementById('A2_0').innerHTML = "你怎麼知道mamaro? (複選)";
                document.getElementById('A2_1').innerHTML = "新聞";
                document.getElementById('A2_2').innerHTML = "網站";
                document.getElementById('A2_3').innerHTML = "Trim宣傳";
                document.getElementById('A2_4').innerHTML = "聽到的";
                document.getElementById('A2_5').innerHTML = "babymap";
                document.getElementById('A2_6').innerHTML = "看到的";

                document.getElementById('A3_0').innerHTML = "有什麼mamaro未來的建議";
                document.getElementById('A3_0_1').innerHTML = "希望如何會更好?";
                document.getElementById('A3_1').innerHTML = "測量小孩身高體重";
                document.getElementById('A3_2').innerHTML = "醫療";
                document.getElementById('A3_3').innerHTML = "家暴諮詢";
                document.getElementById('A3_4').innerHTML = "遊戲";
                document.getElementById('A3_5').innerHTML = "購物";
                document.getElementById('A3_6').innerHTML = "和媽媽做朋友";

                document.getElementById('A4_0').innerHTML = "你喜歡哪種方式?";
                document.getElementById('A4_1').innerHTML = "私人空間";
                document.getElementById('A4_2').innerHTML = "公共空間";

                document.getElementById('final_1').innerHTML = "謝謝協助填問卷!";
                document.getElementById('final_2').innerHTML = "";
            } else if (vaaa == '简体中文') {
                document.getElementById('A0_1').innerHTML = "你的小孩几岁?";
                document.getElementById('A0_2').innerHTML = "岁";
                document.getElementById('A0_3').innerHTML = "个月";
                document.getElementById('A0_4').innerHTML = "你是";
                document.getElementById('parent_select').options[0].innerHTML = "妈妈";
                document.getElementById('parent_select').options[1].innerHTML = "爸爸";

                document.getElementById('A1_0').innerHTML = "哪里是放mamaro最好的地方? (复选)";
                document.getElementById('A1_1').innerHTML = "主题乐园";
                document.getElementById('A1_2').innerHTML = "购物中心";
                document.getElementById('A1_3').innerHTML = "医院";
                document.getElementById('A1_4').innerHTML = "政府单位";
                document.getElementById('A1_5').innerHTML = "运动中心";
                document.getElementById('A1_6').innerHTML = "车站";

                document.getElementById('A2_0').innerHTML = "你怎么知道mamaro? (复选)";
                document.getElementById('A2_1').innerHTML = "新闻";
                document.getElementById('A2_2').innerHTML = "网站";
                document.getElementById('A2_3').innerHTML = "Trim宣传";
                document.getElementById('A2_4').innerHTML = "听到的";
                document.getElementById('A2_5').innerHTML = "babymap";
                document.getElementById('A2_6').innerHTML = "看到的";

                document.getElementById('A3_0').innerHTML = "有什么mamaro未来的建议";
                document.getElementById('A3_0_1').innerHTML = "希望如何会更好?";
                document.getElementById('A3_1').innerHTML = "测量小孩身高体重";
                document.getElementById('A3_2').innerHTML = "医疗";
                document.getElementById('A3_3').innerHTML = "家暴咨询";
                document.getElementById('A3_4').innerHTML = "游戏";
                document.getElementById('A3_5').innerHTML = "购物";
                document.getElementById('A3_6').innerHTML = "和妈妈做朋友";

                document.getElementById('A4_0').innerHTML = "你喜欢哪种方式?";
                document.getElementById('A4_1').innerHTML = "私人空间";
                document.getElementById('A4_2').innerHTML = "公共空间";

                document.getElementById('final_1').innerHTML = "谢谢协助填问卷!";
                document.getElementById('final_2').innerHTML = "";
            }else if (vaaa == 'Vietnamese') {
                document.getElementById('A0_1').innerHTML = "Tuổi của bé?";
                document.getElementById('A0_2').innerHTML = "năm ";
                document.getElementById('A0_3').innerHTML = "tháng tuổi";
                document.getElementById('A0_4').innerHTML = "Bạn là";
                document.getElementById('parent_select').options[0].innerHTML = "Mẹ";
                document.getElementById('parent_select').options[1].innerHTML = "Cha";

                document.getElementById('A1_0').innerHTML = "Bạn muốn mamaro ở đâu? (có thể chọn nhiều hơn 1 địa điểm)";
                document.getElementById('A1_1').innerHTML = "Viện bảo tàng";
                document.getElementById('A1_2').innerHTML = "Trung tâm mua sắm";
                document.getElementById('A1_3').innerHTML = "Bệnh viện";
                document.getElementById('A1_4').innerHTML = "Toà thị chính";
                document.getElementById('A1_5').innerHTML = "Sân vận động";
                document.getElementById('A1_6').innerHTML = "Ga tàu điện";

                document.getElementById('A2_0').innerHTML = "Ban biết đến mamaro như thế nào? (có thể chọn nhiều hơn 1 đáp án)";
                document.getElementById('A2_1').innerHTML = "Báo chí";
                document.getElementById('A2_2').innerHTML = "Internet";
                document.getElementById('A2_3').innerHTML = "Các chiến dịch quảng cáo của Trim";
                document.getElementById('A2_4').innerHTML = "Từ bạn bè";
                document.getElementById('A2_5').innerHTML = "Từ ứng dụng babymap";
                document.getElementById('A2_6').innerHTML = "Bắt gặp mamaro ở đâu đó";

                document.getElementById('A3_0').innerHTML = "Hay đóng góp ý kiến giúp mamaro trở nên tốt hơn <Tôi muốn …";
                document.getElementById('A3_0_1').innerHTML = "mamaro có nhiều tính năng hơn như>";
                document.getElementById('A3_1').innerHTML = "đo chiều cao và cân nặng của bé";
                document.getElementById('A3_2').innerHTML = "các giải pháp y tế cho bé";
                document.getElementById('A3_3').innerHTML = "Chăm sóc bởi các chuyên gia tâm lý dành cho các bà mẹ mới sinh";

                document.getElementById('A3_4').innerHTML = "Game";
                document.getElementById('A3_5').innerHTML = "Shopping";
                document.getElementById('A3_6').innerHTML = "Kết bạn với các bà mẹ khác";

                document.getElementById('A4_0').innerHTML = "Bạn thích mamaro như thế nào?";
                document.getElementById('A4_1').innerHTML = "Private room";
                document.getElementById('A4_2').innerHTML = "Public";

                document.getElementById('final_1').innerHTML = "Cảm ơn vì sự đóng góp của bạn!";
                document.getElementById('final_2').innerHTML = "";
            }

        }
        function outputSelectedValueAndText_1(obj) {
            var idx = obj.selectedIndex;
            var value = obj.options[idx].value; // 値
            var text = obj.options[idx].text;  // 表示テキスト

            // 値とテキストをコンソールに出力
            console.log('value = ' + value + ', ' + 'text = ' + text);

            window.sessionStorage.setItem(['byear'], [value]);

        }
        function outputSelectedValueAndText_2(obj) {
            var idx = obj.selectedIndex;
            var value = obj.options[idx].value; // 値
            var text = obj.options[idx].text;  // 表示テキスト

            // 値とテキストをコンソールに出力
            console.log('value = ' + value + ', ' + 'text = ' + text);

            window.sessionStorage.setItem(['bmonth'], [value]);
        }
        function outputSelectedValueAndText_3(obj) {
            var idx = obj.selectedIndex;
            var value = obj.options[idx].value; // 値
            var text = obj.options[idx].text;  // 表示テキスト

            // 値とテキストをコンソールに出力
            console.log('value = ' + value + ', ' + 'text = ' + text);

            window.sessionStorage.setItem(['parent'], [value]);
        }

        //change next view
        function nextview(obj) {
            var idx = obj.id;

            // 値とテキストをコンソールに出力
            //console.log('value = ' + idx);

            if (idx == 'QAN0')
            {
                var dlg = document.getElementById("dlgbox_QA1");
                dlg.style.display = "block";

                var winWidth = window.innerWidth;
                var winHeight = window.innerHeight;

                dlg.style.left = 0 + "px";
                dlg.style.top = winHeight / 15 + "px";

                var dlg1 = document.getElementById("dlgbox_QA");
                dlg1.style.display = "none";
            }
            else if (idx == 'QAN1')
            {
                var dlg = document.getElementById("dlgbox_QA2");
                dlg.style.display = "block";

                var winWidth = window.innerWidth;
                var winHeight = window.innerHeight;

                dlg.style.left = 0 + "px";
                dlg.style.top = winHeight / 15 + "px";

                var dlg1 = document.getElementById("dlgbox_QA1");
                dlg1.style.display = "none";
            } else if (idx == 'QAN2') {

                var va1 = idx.substr(idx.length - 1, 1);
                var se = Number(va1);
                se = se - 1;
                var vaaa = window.sessionStorage.getItem(['QA' + se]);
                if (vaaa == null||vaaa =='')
                {
                    document.getElementById('err_txt' + va1).innerHTML = "Please select answer.";
                } else
                {
                    var dlg = document.getElementById("dlgbox_QA3");
                    dlg.style.display = "block";

                    var winWidth = window.innerWidth;
                    var winHeight = window.innerHeight;

                    dlg.style.left = 0 + "px";
                    dlg.style.top = winHeight / 15 + "px";

                    var dlg1 = document.getElementById("dlgbox_QA2");
                    dlg1.style.display = "none";
                }

            }
            else if (idx == 'QAN3') {
                var va1 = idx.substr(idx.length - 1, 1);
                var se = Number(va1);
                se = se - 1;
                var vaaa = window.sessionStorage.getItem(['QA' + se]);
                if (vaaa == null || vaaa == '') {
                    document.getElementById('err_txt' + va1).innerHTML = "Please select answer.";
                } else {
                    var dlg = document.getElementById("dlgbox_QA4");
                    dlg.style.display = "block";

                    var winWidth = window.innerWidth;
                    var winHeight = window.innerHeight;

                    dlg.style.left = 0 + "px";
                    dlg.style.top = winHeight / 15 + "px";

                    var dlg1 = document.getElementById("dlgbox_QA3");
                    dlg1.style.display = "none";
                }
            }
            else if (idx == 'QAN4') {
                var va1 = idx.substr(idx.length - 1, 1);
                var se = Number(va1);
                se = se - 1;
                var vaaa = window.sessionStorage.getItem(['QA' + se]);
                if (vaaa == null || vaaa == '') {
                    document.getElementById('err_txt' + va1).innerHTML = 'Please select answer.';
                } else {
                    var dlg = document.getElementById("dlgbox_QA5");
                    dlg.style.display = "block";

                    var winWidth = window.innerWidth;
                    var winHeight = window.innerHeight;

                    dlg.style.left = 0 + "px";
                    dlg.style.top = winHeight / 15 + "px";

                    var dlg1 = document.getElementById("dlgbox_QA4");
                    dlg1.style.display = "none";
                }
            }
            else if (idx == 'QAN5') {
                var va1 = idx.substr(idx.length - 1, 1);
                var se = Number(va1);
                se = se - 1;
                var vaaa = window.sessionStorage.getItem(['QA' + se]);
                if (vaaa == null || vaaa == '') {
                    document.getElementById('err_txt' + va1).innerHTML = 'Please select answer.';
                } else {
                    var dlg = document.getElementById("dlgbox_QA6");
                    dlg.style.display = "block";

                    var winWidth = window.innerWidth;
                    var winHeight = window.innerHeight;

                    dlg.style.left = 0 + "px";
                    dlg.style.top = winHeight / 15 + "px";

                    var dlg1 = document.getElementById("dlgbox_QA5");
                    dlg1.style.display = "none";
                }
            }

        }
        //change back view
        function backview(obj) {
            var idx = obj.id;

            // 値とテキストをコンソールに出力
            //console.log('value = ' + idx);

            if (idx == 'QAB1') {
                var dlg = document.getElementById("dlgbox_QA");
                dlg.style.display = "block";

                var winWidth = window.innerWidth;
                var winHeight = window.innerHeight;

                dlg.style.left = 0 + "px";
                dlg.style.top = winHeight / 15 + "px";

                var dlg1 = document.getElementById("dlgbox_QA1");
                dlg1.style.display = "none";
            }
            else if (idx == 'QAB2') {
                var va1 = idx.substr(idx.length - 1, 1);

                document.getElementById('err_txt' + va1).innerHTML = '';

                var dlg = document.getElementById("dlgbox_QA1");
                dlg.style.display = "block";

                var winWidth = window.innerWidth;
                var winHeight = window.innerHeight;

                dlg.style.left = 0 + "px";
                dlg.style.top = winHeight / 15 + "px";

                var dlg1 = document.getElementById("dlgbox_QA2");
                dlg1.style.display = "none";
            }
            else if (idx == 'QAB3') {
                var va1 = idx.substr(idx.length - 1, 1);

                document.getElementById('err_txt' + va1).innerHTML = '';
                var se = Number(va1);
                se = se - 1;
                document.getElementById('err_txt' + se).innerHTML = '';

                var dlg = document.getElementById("dlgbox_QA2");
                dlg.style.display = "block";

                var winWidth = window.innerWidth;
                var winHeight = window.innerHeight;

                dlg.style.left = 0 + "px";
                dlg.style.top = winHeight / 15 + "px";

                var dlg1 = document.getElementById("dlgbox_QA3");
                dlg1.style.display = "none";
            }
            else if (idx == 'QAB4') {
                var va1 = idx.substr(idx.length - 1, 1);

                document.getElementById('err_txt' + va1).innerHTML = '';
                var se = Number(va1);
                se = se - 1;
                document.getElementById('err_txt' + se).innerHTML = '';
                var dlg = document.getElementById("dlgbox_QA3");
                dlg.style.display = "block";

                var winWidth = window.innerWidth;
                var winHeight = window.innerHeight;

                dlg.style.left = 0 + "px";
                dlg.style.top = winHeight / 15 + "px";

                var dlg1 = document.getElementById("dlgbox_QA4");
                dlg1.style.display = "none";
            }
            else if (idx == 'QAB5') {
                var va1 = idx.substr(idx.length - 1, 1);

                document.getElementById('err_txt' + va1).innerHTML = '';
                var se = Number(va1);
                se = se - 1;
                document.getElementById('err_txt' + se).innerHTML = '';
                var dlg = document.getElementById("dlgbox_QA4");
                dlg.style.display = "block";

                var winWidth = window.innerWidth;
                var winHeight = window.innerHeight;

                dlg.style.left = 0 + "px";
                dlg.style.top = winHeight / 15 + "px";

                var dlg1 = document.getElementById("dlgbox_QA5");
                dlg1.style.display = "none";
            }
        }
        //image choice
        function choice_change(obj) {

            var idx = obj.id;

            var value = idx.substr(1, idx.length - 1);
            //console.log('value = ' + value);
            var elem = document.getElementById(value);
            var elemvar = elem.src;
            if (elemvar.indexOf('170817_32_off.png') != -1)
            {
                elem.src = "img/img/QA/170817_32_on.png";
                var va1 = idx.substr(3, 1);
                var vaa = document.getElementById(idx).getAttribute('value');
                var vaaa = window.sessionStorage.getItem(['QA' + va1]);
                if (vaaa == null) {
                    window.sessionStorage.setItem(['QA' + va1], [vaa + ',']);
                } else {
                    window.sessionStorage.setItem(['QA' + va1], [vaaa + vaa + ',']);
                }
                console.log('session QA' + va1 + ' value = ' + window.sessionStorage.getItem(['QA' + va1]));
            }
            else
            {
                elem.src = "img/img/QA/170817_32_off.png";
                var va1 = idx.substr(3, 1);
                var vaa = document.getElementById(idx).getAttribute('value');
                var vaaa = window.sessionStorage.getItem(['QA' + va1]);
                if (vaaa != null) {
                    var vaaa1 = vaaa.replace(vaa+',', "");
                    window.sessionStorage.setItem(['QA' + va1], [vaaa1]);
                }
                console.log('session QA' + va1 + ' value = ' + window.sessionStorage.getItem(['QA' + va1]));
            }

            //var va = idx.substr(2, idx.length - 2);
            //console.log('image name = ' + va);

            //var va1 = idx.substr(3, 1);
            //var vaa = document.getElementById(idx).getAttribute('value');
            //var vaaa = window.sessionStorage.getItem(['QA' + va1]);
            //if (vaaa == 'null') {
            //    window.sessionStorage.setItem(['QA' + va1], [vaa+',']);
            //} else {
            //    window.sessionStorage.setItem(['QA' + va1], [vaaa +vaa+ ',']);
            //}
            //console.log('session QA' + va1+' value = ' + window.sessionStorage.getItem(['QA' + va1]));


            //// 値とテキストをコンソールに出力
            //console.log('value = ' + value + ', ' + 'text = ' + text);
        }
        //final view
        function finalview() {

            // 値とテキストをコンソールに出力
            console.log('value = save');

            //save value
            var whitebg = document.getElementById("white-background");
            var dlg = document.getElementById("dlgbox_QA6");
            whitebg.style.display = "none";
            dlg.style.display = "none";

            var param1 = "<%= Session["mid"]%>".toString();
            var param2 = window.sessionStorage.getItem(['QA'+'1']);
            var param3 = window.sessionStorage.getItem(['QA' + '2']);
            var param4 = window.sessionStorage.getItem(['QA' + '3']);
            var param5 = window.sessionStorage.getItem(['QA' + '4']);


            var param6 = window.sessionStorage.getItem(['lan']);
            var param7 = window.sessionStorage.getItem(['byear']);
            var param8 = window.sessionStorage.getItem(['bmonth']);
            var param9 = window.sessionStorage.getItem(['parent']);

            $.ajax({
                type: "POST",
                url: "mamaro.aspx/save_list",
                data: "{param1: '" + param1 + "',param2: '" + param2 + "',param3: '" + param3 + "',param4: '" + param4 + "',param5: '" + param5 + "',param6: '" + param6 + "',param7: '" + param7 + "',param8: '" + param8 + "',param9: '" + param9 + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    console.log(result.d);
                    //window.location.href = "Date_Calendar_success.aspx";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });

        }
    </script>
</head>
<body>
    <div id="white-background">
        </div>
    <div id="dlgbox_QA" class="dlg">
            <div class="dlgh1"></div>
            <div style="height: 464px; overflow: auto;vertical-align:middle;" class="dlgb" align="center">
                <div style="margin-top:145px;width:83%;display:inline-block;">
                <img src='img/img/QA/170815_3.png' width='200px' height='200px'>
                <span class="dlg_text dlg_text_title">言語 (language)</span>&nbsp;
                <select name="select" onchange="outputSelectedValueAndText(this);">
                     <option value="0">日本語</option>
                     <option value="1">English</option>
                     <option value="2">繁體中文</option>
                     <option value="3">简体中文</option>
                    <option value="4">Vietnamese</option>
                </select>
                    </div>
                <div style="width:10%;display:inline-block;" align="right">
                    <img id="QAN0" class='QAnext' src='img/img/next.png' width='30px' height='50px' onclick="nextview(this);">
                </div>
            </div>
            <div class="dlgf">

            </div>
        </div>
    <div id="dlgbox_QA1" class="dlg">
            <div class="dlgh" align="center">
                <span class="dlg_text_title">Q1</span>
            </div>
            <div style="height: 422px; overflow: auto;vertical-align:middle;" class="dlgb" align="center">

                <div style="margin-top:5px;width:100%;">
                    <table width="100%" height="100%">
                        <tr>
                            <td rowspan="3" width="10%" align="center">
                                <img id="QAB1" class='QAnext' src='img/img/back.png' width='30px' height='50px' onclick="backview(this);">
                            </td>
                            <td width="20%" align="center">
                                <img src='img/img/QA/170815_4.png' width='167px' height='167px' style="left:0px;">
                            </td>
                            <td align="center" width="60%">
                                <span id="A0_1" class="dlg_text dlg_text_des1">お子さまのご年齢は</span>&nbsp;
                <select name="select" onchange="outputSelectedValueAndText_1(this);">
                     <option value="0">0</option>
                     <option value="1">1</option>
                     <option value="2">2</option>
                     <option value="3">3</option>
                    <option value="4">4</option>
                     <option value="5">5</option>
                     <option value="6">6</option>
                     <option value="7">7</option>
                    <option value="8">8</option>
                     <option value="9">9</option>
                     <option value="10">10</option>
                </select>
                    &nbsp;<span id="A0_2" class="dlg_text dlg_text_des1">歳</span>&nbsp;
                    <select name="select" onchange="outputSelectedValueAndText_2(this);">
                     <option value="0">0</option>
                     <option value="1">1</option>
                     <option value="2">2</option>
                     <option value="3">3</option>
                    <option value="4">4</option>
                     <option value="5">5</option>
                     <option value="6">6</option>
                     <option value="7">7</option>
                    <option value="8">8</option>
                     <option value="9">9</option>
                     <option value="10">11</option>
                </select>
                    &nbsp;<span id="A0_3" class="dlg_text dlg_text_des1">ヶ月</span>
                            </td>
                            <td rowspan="3" width="10%" align="center">
                                <img id="QAN1" class='QAnext' src='img/img/next.png' width='30px' height='50px' onclick="nextview(this);">
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <img src='img/img/QA/170815_5.png' width='167px' height='167px' style="left:0px;">
                            </td>
                            <td align="center">
                                   <span id="A0_4" class="dlg_text dlg_text_des1">あなたは</span>&nbsp;
                <select id="parent_select" name="select" onchange="outputSelectedValueAndText_3(this);">
                     <option value="ママ">ママ</option>
                     <option value="パパ">パパ</option>
                </select>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <br><br>
                                <img src='img/img/QA/170817_31.png' width='230px' height='20px'>
                            </td>
                        </tr>
                    </table>

                    </div>


            </div>
            <div class="dlgf">

            </div>
        </div>
        <div id="dlgbox_QA2" class="dlg">
            <div class="dlgh" align="center">
                <span class="dlg_text_title">Q2</span>
            </div>
            <div style="height: 422px; overflow: auto;vertical-align:middle;" class="dlgb" align="center">

                <div style="margin-top:0px;width:100%;height:100%;">
                    <table width="100%" height="100%">
                        <tr>
                            <td rowspan="4" width="10%" align="center">
                                <img id="QAB2" class='QAnext' src='img/img/back.png' width='30px' height='50px' onclick="backview(this);">
                            </td>
                            <td colspan="3" width="80%" height="100px" align="center">
                                <span id="A1_0" class="dlg_text dlg_text_title">mamaroがどこにあったら便利? ( 複数選択可 )</span>
                            </td>
                              <td rowspan="4" width="10%" align="center">
                                <img id="QAN2" class='QAnext' src='img/img/next.png' width='30px' height='50px' onclick="nextview(this);">
                            </td>
                        </tr>
                        <tr>
                            <td width="27%" align="center" height="120px">
                                <div id="DQA1_1" class="choice_div" value="テーマパーク" onclick="choice_change(this);">
                                <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA1_1" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_6.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A1_1" class="dlg_text dlg_text_des">テーマパーク</span>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                            </td>
                            <td align="center" width="27%" height="120px">
                                 <div id="DQA1_2" class="choice_div" value="ショッピングモール" onclick="choice_change(this);">
                                <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA1_2" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_8.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A1_2" class="dlg_text dlg_text_des">ショッピングモール</span>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                            </td>
                          <td width="26%" align="center" height="120px">
                              <div id="DQA1_3" class="choice_div" value="病院" onclick="choice_change(this);">
                              <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA1_3" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_10.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A1_3" class="dlg_text dlg_text_des">病院</span>
                                        </td>
                                    </tr>
                                </table>
                                  </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" width="27%" height="120px">
                                 <div id="DQA1_4" class="choice_div" value="市役所" onclick="choice_change(this);">
                                <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA1_4" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_7.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A1_4" class="dlg_text dlg_text_des">市役所</span>
                                        </td>
                                    </tr>
                                </table>
                                     </div>
                            </td>
                            <td align="center" width="27%" height="120px">
                                <div id="DQA1_5" class="choice_div" value="競技場" onclick="choice_change(this);">
                                  <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA1_5" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_9.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A1_5" class="dlg_text dlg_text_des">競技場</span>
                                        </td>
                                    </tr>
                                </table>
                                    </div>
                            </td>
                            <td align="center" width="26%" height="120px">
                                <div id="DQA1_6" class="choice_div" value="駅" onclick="choice_change(this);">
                                 <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA1_6" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_11.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A1_6" class="dlg_text dlg_text_des">駅</span>
                                        </td>
                                    </tr>
                                </table>
                                    </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <br><br>
                                <img src='img/img/QA/170817_30.png' width='230px' height='20px'>
                            </td>
                        </tr>
                    </table>

                    </div>


            </div>
            <div class="dlgf">
                <span id="err_txt2" class="dlg_text_err dlg_text_des"></span>
            </div>
        </div>

            <div id="dlgbox_QA3" class="dlg">
            <div class="dlgh" align="center">
                <span class="dlg_text_title">Q3</span>
            </div>
            <div style="height: 422px; overflow: auto;vertical-align:middle;" class="dlgb" align="center">

                <div style="margin-top:0px;width:100%;height:100%;">
                    <table width="100%" height="100%">
                        <tr>
                            <td rowspan="4" width="10%" align="center">
                                <img id="QAB3" class='QAnext' src='img/img/back.png' width='30px' height='50px' onclick="backview(this);">
                            </td>
                            <td colspan="3" width="80%" height="100px" align="center">
                                <span id="A2_0" class="dlg_text dlg_text_title">mamaroをどこで知った? ( 複数選択可 )</span>
                            </td>
                              <td rowspan="4" width="10%" align="center">
                                <img id="QAN3" class='QAnext' src='img/img/next.png' width='30px' height='50px' onclick="nextview(this);">
                            </td>
                        </tr>
                        <tr>
                            <td width="27%" align="center" height="120px">
                                <div id="DQA2_1" class="choice_div" value="新聞" onclick="choice_change(this);">
                                <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA2_1" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_12.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A2_1" class="dlg_text dlg_text_des">新聞</span>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                            </td>
                            <td align="center" width="27%" height="120px">
                                 <div id="DQA2_2" class="choice_div" value="ネットニュース" onclick="choice_change(this);">
                                <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA2_2" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_13.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A2_2" class="dlg_text dlg_text_des">ネットニュース</span>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                            </td>
                          <td width="26%" align="center" height="120px">
                              <div id="DQA2_3" class="choice_div" value="TrimのHP" onclick="choice_change(this);">
                              <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA2_3" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_14.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A2_3" class="dlg_text dlg_text_des">TrimのHP</span>
                                        </td>
                                    </tr>
                                </table>
                                  </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" width="27%" height="120px">
                                 <div id="DQA2_4" class="choice_div" value="知人から" onclick="choice_change(this);">
                                <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA2_4" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_15.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A2_4" class="dlg_text dlg_text_des">知人から</span>
                                        </td>
                                    </tr>
                                </table>
                                     </div>
                            </td>
                            <td align="center" width="27%" height="120px">
                                <div id="DQA2_5" class="choice_div" value="babymapで" onclick="choice_change(this);">
                                  <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA2_5" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_16.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A2_5" class="dlg_text dlg_text_des">babymapで</span>
                                        </td>
                                    </tr>
                                </table>
                                    </div>
                            </td>
                            <td align="center" width="26%" height="120px">
                                <div id="DQA2_6" class="choice_div" value="通りすがりに見つけた" onclick="choice_change(this);">
                                 <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA2_6" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_17.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A2_6" class="dlg_text dlg_text_des">通りすがりに見つけた</span>
                                        </td>
                                    </tr>
                                </table>
                                    </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <br><br>
                                <img src='img/img/QA/170817_29.png' width='230px' height='20px'>
                            </td>
                        </tr>
                    </table>

                    </div>


            </div>
            <div class="dlgf">
                <span id="err_txt3" class="dlg_text_err dlg_text_des"></span>
            </div>
        </div>


        <div id="dlgbox_QA4" class="dlg">
            <div class="dlgh" align="center">
                <span class="dlg_text_title">Q4</span>
            </div>
            <div style="height: 422px; overflow: auto;vertical-align:middle;" class="dlgb" align="center">

                <div style="margin-top:0px;width:100%;height:100%;">
                    <table width="100%" height="100%">
                        <tr>
                            <td rowspan="4" width="10%" align="center">
                                <img id="QAB4" class='QAnext' src='img/img/back.png' width='30px' height='50px' onclick="backview(this);">
                            </td>
                            <td colspan="3" width="80%" height="100px" align="center">
                                <span id="A3_0" class="dlg_text dlg_text_title">mamaroに 「もっとこんな機能が欲しい。こんなのがあったら嬉しい。」</span>
                                <span id="A3_0_1" class="dlg_text dlg_text_title">というものはありますか？</span>
                            </td>
                              <td rowspan="4" width="10%" align="center">
                                <img id="QAN4" class='QAnext' src='img/img/next.png' width='30px' height='50px' onclick="nextview(this);">
                            </td>
                        </tr>
                        <tr>
                            <td width="27%" align="center" height="120px">
                                <div id="DQA3_1" class="choice_div" value="子様の身長‧体重を測れる" onclick="choice_change(this);">
                                <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA3_1" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_18.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A3_1" class="dlg_text dlg_text_des">子様の身長‧体重を測れる</span>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                            </td>
                            <td align="center" width="27%" height="120px">
                                 <div id="DQA3_2" class="choice_div" value="医療相談" onclick="choice_change(this);">
                                <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA3_2" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_20.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A3_2" class="dlg_text dlg_text_des">医療相談</span>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                            </td>
                          <td width="26%" align="center" height="120px">
                              <div id="DQA3_3" class="choice_div" value="DVや家庭内事情について相談" onclick="choice_change(this);">
                              <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA3_3" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_22.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A3_3" class="dlg_text dlg_text_des">DVや家庭内事情について相談</span>
                                        </td>
                                    </tr>
                                </table>
                                  </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" width="27%" height="120px">
                                 <div id="DQA3_4" class="choice_div" value="ゲーム機能" onclick="choice_change(this);">
                                <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA3_4" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_19.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A3_4" class="dlg_text dlg_text_des">ゲーム機能</span>
                                        </td>
                                    </tr>
                                </table>
                                     </div>
                            </td>
                            <td align="center" width="27%" height="120px">
                                <div id="DQA3_5" class="choice_div" value="買い物の決済" onclick="choice_change(this);">
                                  <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA3_5" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_21.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A3_5" class="dlg_text dlg_text_des">買い物の決済</span>
                                        </td>
                                    </tr>
                                </table>
                                    </div>
                            </td>
                            <td align="center" width="26%" height="120px">
                                <div id="DQA3_6" class="choice_div" value="ママ友を増やせる" onclick="choice_change(this);">
                                 <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA3_6" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_23.png' width='80px' height='80px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A3_6" class="dlg_text dlg_text_des">ママ友を増やせる</span>
                                        </td>
                                    </tr>
                                </table>
                                    </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <br><br>
                                <img src='img/img/QA/170817_28.png' width='230px' height='20px'>
                            </td>
                        </tr>
                    </table>

                    </div>


            </div>
            <div class="dlgf">
                <span id="err_txt4" class="dlg_text_err dlg_text_des"></span>
            </div>
        </div>

            <div id="dlgbox_QA5" class="dlg">
            <div class="dlgh" align="center">
                <span class="dlg_text_title">Q5</span>
            </div>
            <div style="height: 422px; overflow: auto;vertical-align:middle;" class="dlgb" align="center">

                <div style="margin-top:0px;width:100%;height:100%;">
                    <table width="100%" height="100%">
                        <tr>
                            <td rowspan="3" width="10%" align="center">
                                <img id="QAB5" class='QAnext' src='img/img/back.png' width='30px' height='50px' onclick="backview(this);">
                            </td>
                            <td colspan="2" width="80%" height="100px" align="center">
                                <span id="A4_0" class="dlg_text dlg_text_title">授乳室は個室派？わいわい派？</span>
                            </td>
                              <td rowspan="3" width="10%" align="center">
                                <img id="QAN5" class='QAnext' src='img/img/next.png' width='30px' height='50px' onclick="nextview(this);">
                            </td>
                        </tr>
                        <tr>
                            <td width="40%" align="center" height="240px">
                                <div id="DQA4_1" class="choice_div" value="個室派" onclick="choice_change(this);">
                                <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA4_1" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_24.png' width='120px' height='120px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A4_1" class="dlg_text dlg_text_des">個室派</span>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                            </td>
                            <td align="center" width="40%" height="240px">
                                 <div id="DQA4_2" class="choice_div" value="わいわい派" onclick="choice_change(this);">
                                <table width="100%" height="100%">
                                    <tr>
                                        <td align="center" width="60%">
                                             <img id="QA4_2" src='img/img/QA/170817_32_off.png' width='40px' height='40px'>
                                &nbsp;<img src='img/img/QA/170815_25.png' width='120px' height='120px'>&nbsp;
                                        </td>
                                        <td width="40%">
                                            <span id="A4_2" class="dlg_text dlg_text_des">わいわい派</span>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <br><br>
                                <img src='img/img/QA/170817_27.png' width='230px' height='20px'>
                            </td>
                        </tr>
                    </table>

                    </div>


            </div>
            <div class="dlgf">
                <span id="err_txt5" class="dlg_text_err dlg_text_des"></span>
            </div>
        </div>

                <div id="dlgbox_QA6" class="dlg_fin" onclick="finalview();">
            <div class="dlgh" align="center">
                <span class="dlg_text_title"></span>
            </div>
            <div style="height: 464px; overflow: auto;vertical-align:middle;" class="dlgb_fin" align="center">

                <div style="margin-top:70px;width:100%;">
                    <span id="final_1" class="dlg_text_title">ア ン ケ ー ト に ご 協 力 い た だ き</span>
                    <br>
                    <span id="final_2" class="dlg_text_title">あ り が と う ご ざ い ま し た 。</span>
                    <br><br>
                    <img src='img/img/QA/170815_26.png' width='150px' height='150px'>



                    </div>


            </div>
            <div class="dlgf_fin">

            </div>
        </div>


    <form id="form1" runat="server">

<div class="fullheight">
    <div class="row">
<div align="center" class="col-md-4" style="margin-top:10px;margin-bottom:10px;height:120px;">
    <img border="0" src="img/img/ver1/mamaro_console_logo_1.png" width="140" height="100" style="margin-top:13px;margin-right:23px;" alt="mamaro">

</div>
  <div align="center" vertical-align="middle" class="col-md-4" style="margin-top:40px;">
       <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
        </asp:ScriptManager>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                </asp:Timer>
                <asp:Label ID="Label1" runat="server" Text="Label" style="font-size:16px;"></asp:Label>
                <br />
                <asp:Label ID="Label2" runat="server"  style="font-size:44px;" Text="Label"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
  </div>
  <div class="col-md-4" style="margin-top:25px;">
      <asp:Panel ID="Panel1" runat="server" style="margin-left:40px;width:345px;">
      </asp:Panel>
        </div>
</div>
        <div class="row" style="height:520px;">
  <div class="col-md-12 middle_pan">
      <div class="middle_center">
      <asp:Panel ID="Panel2" runat="server"></asp:Panel>
          </div>
  </div>
</div>
    <div class="row">
<div class="col-md-4"></div>
  <div class="col-md-4">

  </div>
  <div align="center" vertical-align="middle" class="col-md-4">
      <asp:Panel ID="Panel3" runat="server" style="margin-left:180px;margin-top:70px;"></asp:Panel>
  </div>
</div>
    </div>
    </form>
</body>
</html>
