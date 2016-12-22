﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BulksqltoExcel.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <title>登录</title>
    <link rel="stylesheet" type="text/css" href="~/resource/Css/mail.css" />
    <link rel="stylesheet" type="text/css" href="~/resource/css/common.css" />
    <script type="text/javascript" src="./Resource/Scripts/jquery-1.10.2.min.js"></script>
    <script>
        function isEmpty(str){
            return (str.replace(/(\s*)/g,'').length<1);
        }
        function doLogin() {
            var name = $("#userName").val();
            var pwd = $("#pwd").val();
            if (isEmpty(name)) {
                alert("请输入登录名.");
                return;
            }
            if (isEmpty(pwd)) {
                alert("请输入密码.");
                return;
            }
            //document.forms[0].submit();
            Login(name, pwd);

        }
        
        function Login(name,pwd) {
            var parameters = {
                name: name,
                pwd: pwd
            };

            $.ajax({
                type: "post",
                url: "~/Login/Submit",
                dataType: "json",
            global: false,
            data: parameters,
            async: false,
            beforeSend: function(xml) {},
            success: function (data) {

                if (data == "验证通过")
                    window.location.href = "~/Home/Index";

            },
            error: function (event, xmlHttpRequest, ajaxOptions, thrownError) {
               
                alert(xmlHttpRequest);
            },
            complete: function(xml, ts) { }
        });
        }


    </script>

</head>
<body>
        <div class="mian">
            <div class="banner">导出报表</div>
            <div class="center">
                <div class="denglu">
                    <ul>
                        <li class="text_zhanghao">
                            <P><input type="text" name="userName" id="userName" style="width: 126px; height: 27px;"/></P>
                            <P><input type="password" name="pwd" id="pwd" style="width: 126px; height: 27px;"/></P>
                        </li>
                        <li class="denglu_text">&nbsp;&nbsp;&nbsp;<a href="javascript:doLogin();">登陆</a></li>
                    </ul>
                </div>
            </div>
        </div>
</body>

</html>