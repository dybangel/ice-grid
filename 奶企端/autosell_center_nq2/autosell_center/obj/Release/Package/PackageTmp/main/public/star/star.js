$(function () {
    starChange();
});
function starChange() {
    for (var i = 1; i <= 5; i++) {
        //为每个星星 绑定鼠标移入 移出 事件
        $(".star" + i).hover(
			//鼠标移入
			function () {
			    var index = $(this).attr("class").substr(4, 1);
			    for (var j = index; j > 0; j--) {
			        $(".star" + j).addClass("c3");
			    }
			    //改变评价样式 及 内容
			    switch (index) {
			        case "1":
			            $(".remark").addClass("c3");
			            break;
			        case "2":
			            $(".remark").addClass("c3");
			            break;
			        case "3":
			            $(".remark").addClass("c3");
			            break;
			        case "4":
			            $(".remark").addClass("c3");
			            break;
			        case "5":
			            $(".remark").addClass("c3");
			            break;
			    }
			},
			//鼠标移出
			function () {
			    var index = $(this).attr("class").substr(4, 1);
			    for (var i = index; i > 0; i--) {
			        //恢复本身及之前的星星的颜色
			        $(".star" + i).removeClass("c3");
			    }
			}
		);

        //为每个星星 绑定点击事件
        $(".star" + i).bind("click", function () {
            var index = $(this).attr("class").substr(4, 1);
            //点击之后 解绑本身及之前星星的 所有事件
            for (var k = 1; k <= index; k++) {
                $(".star" +k ).unbind();
            }
            $("#HF_starNum").val(index);
            ////将后面的隐藏
            //while (++index <= 5) {
            //    $(".star" + index).css("display", "none");
            //}
        });
    }
}
