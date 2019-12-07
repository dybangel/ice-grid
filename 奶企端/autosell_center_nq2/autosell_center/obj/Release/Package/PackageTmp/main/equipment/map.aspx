<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="map.aspx.cs" Inherits="autosell_center.main.equipment.map" %>
<%--NUhf3DfhMuF97uz15QjH3ykOEB1YURoi--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
    <script src="/main/public/script/jquery-3.2.1.min.js" type="text/javascript"></script>
	<script charset="utf-8" src="http://map.qq.com/api/js?v=2.exp" type="text/javascript"></script>
	<script src="http://open.map.qq.com/apifiles/2/4/79/main.js" type="text/javascript"></script>
	<style>
      #address{height: 31px;padding: 0 10px;}
      .map-seach{background: #50a4ec;padding: 5px 20px;color: #fff;display: inline-block;}
      .map-seach:active{background: rgba(80, 164, 236, 0.4);}
    </style>
</head>
<body>
    <div class="form-group">
			<label class="col-sm-1 control-label">经度</label>
			<div class="col-sm-11">
				<input type="text" class="form-control" id="longitude" name="longitude" value=""/>
			</div>
		</div>
		<div class="form-group">
            <label class="col-sm-1 control-label">纬度</label>
            <div class="col-sm-11">
                <input type="text" class="form-control" id="latitude" name="latitude" value=""/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-1 control-label">地图</label>
            <div class="col-sm-11">
                <div class="input-group">
                    <input type="text" id="address" class="form-control" value=""/>
                    <span class="input-group-btn">
                            <button type="button"  id="mapseacrh" class="btn btn-primary">搜索</button></span>
                </div>
                <span class="help-block m-b-none"> 地图上选自己公司的位置会自动获取到你的经纬度</span>
                <div id="container" style="width:100%;height:300px;"></div>
            </div>
        </div>

</body>
</html>
<script type="text/javascript">
    $(function () {
        var geocoder, citylocation, map, marker = null;
        var markersArray = [];
        var x = $("#longitude").val();
        var y = $("#latitude").val();
        var center = new qq.maps.LatLng(y, x);
        map = new qq.maps.Map(document.getElementById('container'), {
            center: center,
            zoom: 13
        });
        geocoder = new qq.maps.Geocoder({
            complete: function (result) {
                map.setCenter(result.detail.location);
                var marker = new qq.maps.Marker({
                    map: map,
                    position: result.detail.location
                });
            }
        });
        marker = new qq.maps.Marker({
            position: new qq.maps.LatLng(y, x),
            map: map
        });
        //获取城市列表接口设置中心点
        if (y == '' || x == '') {
            citylocation = new qq.maps.CityService({
                complete: function (result) {
                    map.setCenter(result.detail.latLng);
                }
            });
            //调用searchLocalCity();方法    根据用户IP查询城市信息。
            citylocation.searchLocalCity();
        }


        //绑定单击事件添加参数
        qq.maps.event.addListener(map, 'click', function (event) {
            //            alert('您点击的位置为: [' + event.latLng.getLat() + ', ' +
            //                    event.latLng.getLng() + ']');
            qq.maps.event.addListener(map, 'click', function (event) {
                marker.setMap(null);
                $("#longitude").attr("value", "");
                $("#longitude").attr("value", event.latLng.getLng());
                $("#latitude").attr("value", "");
                $("#latitude").attr("value", event.latLng.getLat());
                marker = new qq.maps.Marker({
                    position: new qq.maps.LatLng(event.latLng.getLat(), event.latLng.getLng()),
                    map: map
                });
            });
        });
        geocoder = new qq.maps.Geocoder({
            complete: function (result) {
                marker.setMap(null);
                map.setCenter(result.detail.location);
                marker = new qq.maps.Marker({
                    map: map,
                    position: result.detail.location
                });
                $("#latitude").attr("value", marker.position.lat);
                $("#longitude").attr("value", marker.position.lng);
            }
        });
        /*            var map = new qq.maps.Map(document.getElementById("container"),{
         center: new qq.maps.LatLng(y,x),
         zoom: 13
         });*/

        $("#mapseacrh").click(function () {
            var address = $("#address").val();
            //通过getLocation();方法获取位置信息值
            geocoder.getLocation(address);
        });

    });
</script>
