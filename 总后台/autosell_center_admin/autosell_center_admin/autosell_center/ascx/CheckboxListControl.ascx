<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckboxListControl.ascx.cs" Inherits="autosell_center.ascx.CheckboxListControl" %>
 <style type="text/css"> 
    .DivCheckBoxList
    {
        display: none;
        border: 1px solid Gray;
        background-color: White;
        width: 126px;
        position:absolute;
        height: 400px;
        overflow-y: auto;
        overflow-x: hidden;
        margin-top: -10px;
    }
    #divCheckBoxList input{
        width:16px;
        height:16px;
        float:left;
        margin:9px auto 9px 8px;
        position:relative;
        left:-16px;
    }
    .CheckBoxList
    {
        position: relative;
        width: 150px;
        height: 10px;
        overflow: scroll;
        font-size: small;
    }
     #divCustomCheckBoxList {
        top:0!important;
     }
    #divCustomCheckBoxList,#divCheckBoxList,#divCheckBoxList div,#divCheckBoxListClose{
        width:210px !important;
    }
    #divCheckBoxList{
        height:auto!important;
        min-height:260px;

        padding-left:24px;
        line-height:34px;
    }
    #txtcboName{
        position:relative !important;
    }
    #divCheckBoxList #allText,#divCheckBoxList #removeText{
        width:50% !important;
        text-align:left;
        color:#0094ff;
        float:left;
        cursor:pointer;
    }
    .screenseachMain ul li span #divCustomCheckBoxList{
        width:45% !important;
        float:right;
    }
</style>
<script type="text/javascript">
    var timoutID;
    function ShowMList() {
        var divRef = document.getElementById("divCheckBoxList");
        divRef.style.display = "block";
        var divRefC = document.getElementById("divCheckBoxListClose");
        divRefC.style.display = "block";
    }
    function HideMList() {
        if (document.getElementById("divCheckBoxList")!=null)
        document.getElementById("divCheckBoxList").style.display = "none";
        document.getElementById("divCheckBoxListClose").style.display = "none";
    }
    function changeinfo() {
        var ObjectText = "";
        var ObjectValue = "";
        var r = document.getElementsByName("subBox");
        for (var i = 0; i < r.length; i++) {
            
            if (r[i].checked) {
                ObjectValue += r[i].value + ",";
                ObjectText += r[i].nextSibling.nodeValue + ",";
            }
            document.getElementById("txtcboName").value = ObjectText.substring(0,ObjectText.length-1);
            $("#cbosDeparentment_hdscbo").val(ObjectValue.substring(0, ObjectValue.length-1));
        }
    }
    function chkAll()
    {
        $temp = $("#chk").attr("checked");
        if (typeof ($temp) == "undefined") {
           $("#chk").attr("checked", "checked");
        }
    }
</script>
<div id="divCustomCheckBoxList" onmouseover="clearTimeout(timoutID);" onmouseout="timoutID = setTimeout('HideMList()', 750);">
    <table>
        <tr>
            <td align="right">
                <asp:HiddenField runat="server" ID="hdscbo"></asp:HiddenField>
                <input id="txtcboName" type="text" value="<%=cboName %>" readonly="readonly" onclick="ShowMList()"  placeholder="请选择机器" />
            </td>
            <td align="left" valign="middle">
              <%--  <a href="#" onclick="ShowMList()" >选择</a>--%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
               
                <div id="divCheckBoxList" class="DivCheckBoxList">
                 <%--   <div id="allText" onclick="chkAll()">全选</div>--%>
                 <%--   <div id="removeText">清空</div>--%>
                    <%=cbostr%>
                </div>
                <div id="divCheckBoxListClose">
                    &nbsp;
                </div>
            </td>
        </tr>
    </table>
</div>
