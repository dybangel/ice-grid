<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="setFullDiscountActivity.aspx.cs" Inherits="autosell_center.main.activity.setFullDiscountActivity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <link type="text/css" rel="Stylesheet" href="QXGL/zTreeStyle/zTreeStyle.css" />
    <script type="text/javascript" src="QXGL/Javascript/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="QXGL/Javascript/jquery.ztree.all-3.1.min.js"></script>
    <style>
        .btn{ 
 	line-height:30px;
	height:25px;
	width:100px;
	color:#ffffff;
	background-color:#4a8cf7;
	font-size:15px;
	font-weight:normal;
	font-family:Arial;
	border:0px solid #dcdcdc;
	-webkit-border-top-left-radius:6px;
	-moz-border-radius-topleft:6px;
	border-top-left-radius:6px;
	-webkit-border-top-right-radius:6px;
	-moz-border-radius-topright:6px;
	border-top-right-radius:6px;
	-webkit-border-bottom-left-radius:6px;
	-moz-border-radius-bottomleft:6px;
	border-bottom-left-radius:6px;
	-webkit-border-bottom-right-radius:6px;
	-moz-border-radius-bottomright:6px;
	border-bottom-right-radius:6px;
	-moz-box-shadow: inset 0px 0px 2px 0px #ffffff;
	-webkit-box-shadow: inset 0px 0px 2px 0px #ffffff;
	box-shadow: inset 0px 0px 2px 0px #ffffff;
	text-align:center;
	display:inline-block;
	text-decoration:none;
}
.btn:hover {
	background-color:#4a8cf7;
}
    </style>

     <script type="text/javascript">
    //初始化树的参数
        var zTreeObj,
        setting = {
            view: {
                expandSpeed: "slow",
                dblClickExpand: true,
                selectedMulti: false
            },
            data: {
                simpleData: {
                    enable: true,
                    idKey: "id",
                    pIdKey: "pId",
                    rootPId: 0
                }
            },
            check: {
                enable: true, //设置 zTree 的节点上是否显示 checkbox / radio
                chkStyle: "checkbox", //checkbox表示复选框，radio表示单选框
                chkboxType: { "Y": "ps", "N": "ps"}//Y 属性定义 checkbox 被勾选后的情况； N 属性定义 checkbox 取消勾选后的情况； "p" 表示操作会影响父级节点； "s" 表示操作会影响子级节点。
            },
            callback: {
                onCheck: zTreeOnCheck
            }
        };

        //每次点击 checkbox 或 radio 后，触发的事件
        function zTreeOnCheck(event, treeId, treeNode) {
            //alert(treeNode.tId + ", " + treeNode.name + "," + treeNode.checked);
        };

        //页面加载的事件
        $(document).ready(function () {
           
           
            GetDatas();
            CheckedNodes();
            //choose();
        });

        //获取选中的节点数据
        function GetCheckedDatas() {
            var nodes = zTreeObj.getSelectedNodes();
            //alert(nodes.length);
        }

        //从服务器获取数据
        function GetDatas() {
            $.ajax({
                url: "setFullDiscountActivity.aspx/getProdctList",
                type: "post",                //请求发送方式
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{companyID:'" + $("#companyID").val() + "'}",
              
                //请求成功执行
                success: function (data) {
                    //data 为返回值,初始化树形控件
                    zTreeObj = $.fn.zTree.init($("#tree"), setting, data.d);
                    CheckedNodes();
                }
              
            });
        }

        //获取被选中的所有节点
        function GetCheckedNodes() {
            return zTreeObj.getCheckedNodes(true);
        } 
        //获取指定类型的被选中的节点----后台调用方法，传值 
        function GetCheckedNodesByType() {
            var all = GetCheckedNodes();
           
            if (all.length > 0) {
                var str="";
                for (var i = 0; i < all.length; i++) {
                    if (all[i].type == "get") {//判断是否是属性type==”get“的节点
                        str += all[i].id + ",";
                    }
                    $("#m_id").val(str);
                   
                }
              
            }  
        }

        //默认选中
        function CheckedNodes() {
            //后台获取需要默认选中的集合
            if ($("#menu_id").val().length>0) {
               
            }
            var menu = $("#menu_id").val().split('-');
            if (menu != null) {
                for (var j = 0; j < menu.length; j++) {
                    zTreeObj.getNodeByParam("id", menu[j], null).checked = "true";
                }
            }
            zTreeObj.refresh();//刷新树，不然看不到被勾选效果
        }
        function save() {
            GetCheckedNodesByType();//调用上面的js方法
            
            alert("保存成功");
            $.ajax({
                url: "setFullDiscountActivity.aspx/qxChoose",
                type: "post",                //请求发送方式
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{id:'" + $("#m_id").val() + "',companyID:'" + $("#companyID").val() + "',activityID:'" + $("#_activityID").val() + "'}",
                //请求成功执行
                success: function (data, textStatus) {
                    
                   
                   
                  

                }
            });
            window.close();
        }

       

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="script" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div>
        <div class="form-group">  
             <input type="button" onclick="save()"   class="btn" value="保存"/>
        </div>
        <ul id="tree" class="ztree" style="width: 330px; overflow: auto; height: 500px; border: 0px solid #141300;overflow: auto;">
        </ul>
        <input type="hidden" id="m_id" name="m_id" runat="server" />
        <input type="hidden" id="menu_id" name="menu_id" runat="server" />
        <input type="hidden" id="companyID" runat="server"/>
        <input type="hidden" id="_activityID" runat="server"/>
    </div>
    </form>
</body>
</html>
