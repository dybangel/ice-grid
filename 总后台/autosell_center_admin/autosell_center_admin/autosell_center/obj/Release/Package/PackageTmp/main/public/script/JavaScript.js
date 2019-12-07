//自定义弹窗样式

(function () {
    window.alert = function (text) {
        text = text.toString().replace(/\\/g, '\\').replace(/\n/g, '<br />').replace(/\r/g, '<br />'); //解析alert内容中的换行符
        var alertdiv = '<div id="alertdiv" style="position:absolute; display:none ; overflow:hidden; padding:10px 10px 8px; top: 50%; left: 50%; text-align:center; line-height:22px; background-color:#DDE4EE; border:1px solid #ccc">' + text + '<br /><input type="submit" name="button" id="button" value="确定" style="margin-top:8px;" onclick="$(this).parent().remove(); "/></div>'; //自定义div弹窗
        $(document.body).append(alertdiv); //动态加载div
        $("#alertdiv").css({ "margin-left": $("#alertdiv").width() / 2 * (-1) - 20, "margin-top": $("#alertdiv").height() / 2 * (-1) - 20 }); //设置偏移数值，实现div居中
        $("#alertdiv").show(); //显示
    };
})();