<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QRcounter.aspx.cs" Inherits="QRcounter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-1.12.4.js"></script>
    <title>mamaro</title>
</head>
<body>
    <div id="log1"></div>
    <div id="showplace"></div>
        <script>


            (function () {
                'use strict';

                var module = {
                    options: [],
                    header: [navigator.platform, navigator.userAgent, navigator.appVersion, navigator.vendor, window.opera],
                    dataos: [
                        { name: 'Windows Phone', value: 'Windows Phone', version: 'OS' },
                        { name: 'Windows', value: 'Win', version: 'NT' },
                        { name: 'iPhone', value: 'iPhone', version: 'OS' },
                        { name: 'iPad', value: 'iPad', version: 'OS' },
                        { name: 'iPod', value: 'iPod', version: 'OS' },
                        { name: 'Kindle', value: 'Silk', version: 'Silk' },
                        { name: 'Android', value: 'Android', version: 'Android' },
                        { name: 'PlayBook', value: 'PlayBook', version: 'OS' },
                        { name: 'BlackBerry', value: 'BlackBerry', version: '/' },
                        { name: 'Macintosh', value: 'Mac', version: 'OS X' },
                        { name: 'Linux', value: 'Linux', version: 'rv' },
                        { name: 'Palm', value: 'Palm', version: 'PalmOS' }
                    ],
                    databrowser: [
                        { name: 'Chrome', value: 'Chrome', version: 'Chrome' },
                        { name: 'Firefox', value: 'Firefox', version: 'Firefox' },
                        { name: 'Safari', value: 'Safari', version: 'Version' },
                        { name: 'Internet Explorer', value: 'MSIE', version: 'MSIE' },
                        { name: 'Opera', value: 'Opera', version: 'Opera' },
                        { name: 'BlackBerry', value: 'CLDC', version: 'CLDC' },
                        { name: 'Mozilla', value: 'Mozilla', version: 'Mozilla' }
                    ],
                    init: function () {
                        var agent = this.header.join(' '),
                            os = this.matchItem(agent, this.dataos),
                            browser = this.matchItem(agent, this.databrowser);

                        return { os: os, browser: browser };
                    },
                    matchItem: function (string, data) {
                        var i = 0,
                            j = 0,
                            html = '',
                            regex,
                            regexv,
                            match,
                            matches,
                            version;

                        for (i = 0; i < data.length; i += 1) {
                            regex = new RegExp(data[i].value, 'i');
                            match = regex.test(string);
                            if (match) {
                                regexv = new RegExp(data[i].version + '[- /:;]([\\d._]+)', 'i');
                                matches = string.match(regexv);
                                version = '';
                                if (matches) { if (matches[1]) { matches = matches[1]; } }
                                if (matches) {
                                    matches = matches.split(/[._]+/);
                                    for (j = 0; j < matches.length; j += 1) {
                                        if (j === 0) {
                                            version += matches[j] + '.';
                                        } else {
                                            version += matches[j];
                                        }
                                    }
                                } else {
                                    version = '0';
                                }
                                return {
                                    name: data[i].name,
                                    version: parseFloat(version)
                                };
                            }
                        }
                        return { name: 'unknown', version: 0 };
                    }
                };

                var e = module.init(),
                    debug = '';

                debug += 'os.name = ' + e.os.name + '<br/>';
                debug += 'os.version = ' + e.os.version + '<br/>';
                debug += 'browser.name = ' + e.browser.name + '<br/>';
                debug += 'browser.version = ' + e.browser.version + '<br/>';

                $(document).ready(function () {
                    function detectmobip() {
                        if (navigator.userAgent.match(/iPhone/i)
                        || navigator.userAgent.match(/iPad/i)
                        || navigator.userAgent.match(/iPod/i)
                        ) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                    function detectmoban() {
                        if (navigator.userAgent.match(/Android/i)) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                        $.ajax({
                            type: "POST",
                            url: "QRcounter.aspx/check_user",
                            data: "{param1: '" + e.os.name + "' , param2 :'" + e.os.version + "' , param3 :'" + e.browser.name + "' , param4 :'" + e.browser.version + "' }",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                                //console.log(result.d);
                                //alert(result.d);
                                //alert(result.d);
                                //$('#showplace').empty();
                                //$('#showplace').append(result.d);



                                    var ind = 0;
                                    if (detectmobip()) {
                                        ind = 1;
                                    }
                                    if (detectmoban()) {
                                        ind = 2;
                                    }
                                    if (ind == 1) {
                                        window.location = "";

                                    }
                                    else if (ind == 2) {
                                        window.location = "";

                                    } else if (ind == 0) {
                                        window.location = "";

                                    }

                            },
                            error: function (result) {
                                console.log(result.d);
                                window.location = "";
                            }
                        });
                });

                //document.getElementById('log1').innerHTML = debug;
            }());
    </script>
    <form id="form1" runat="server">
    <div>

    </div>
    </form>
</body>
</html>
