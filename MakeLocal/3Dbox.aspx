﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="3Dbox.aspx.cs" Inherits="_3Dbox" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <title>3Dbox</title>
    <style>
        body,
        html {
            height: 100%;
            padding: 0;
            margin: 0;
            box-sizing: border-box;
            overflow: hidden;
            background-color: rgb(0, 0, 0);
            position: relative;
        }

        .wraper {
            position: absolute;
            width: 260px;
            height: 260px;
            top:0;
    right:0;
    bottom:0;
    left:0;
    margin:auto;
            perspective: 1000px;
        }

        .cube {
            height: 100%;
            width: 100%;
            position: relative;
            transform-style: preserve-3d;
            /*transform: rotateX(-30deg) rotateY(-45deg);*/
            animation: spin 5s ease-in-out;
        }

        @keyframes spin {
            from {
                transform: rotateX(0deg) rotateY(0deg) rotateZ(0deg);
            }

            to {
                transform: rotateX(360deg) rotateY(360deg) rotateZ(360deg);
            }
        }

        .cube > div {
            width: 100%;
            height: 100%;
            position: absolute;
            top: 0;
            left: 0;
            background-color: rgba(114, 210, 214, .8);
            text-align: center;
            line-height: 260px;
            color: #fff;
            font-size: 48px;
            border: 2px solid #72CCC0;
            user-select: none;
        }

        .front {
            transform: translateZ(130px);
        }

        .end {
            transform: rotateY(180deg) translateZ(130px);
        }

        .top {
            transform: rotateX(90deg) translateZ(130px);
        }

        .bottom {
            transform: rotateX(-90deg) translateZ(130px);
        }

        .left {
            transform: rotateY(-90deg) translateZ(130px);
        }

        .right {
            transform: rotateY(90deg) translateZ(130px);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>

    </div>
    </form>
     <div class="wraper">
        <div class="cube">
            <div class="front">1</div>
            <div class="end">2</div>
            <div class="left">3</div>
            <div class="right">4</div>
            <div class="top">5</div>
            <div class="bottom">6</div>
        </div>
    </div>
    <script>

		setTimeout(function () {
			var cube = document.querySelector(".cube"),
				downX, downY, moveX, moveY, tempX, tempY, degX = 0, degY = 0;
			window.onmousedown = function (e) {
				e = e || event;
				downX = e.clientX;			//获取鼠标点下去时的坐标
				downY = e.clientY;
				console.log('can');
				window.onmousemove = function (e) {
					e = e || event;
					moveX = e.clientX - downX;			//算出鼠标移动的距离
					moveY = e.clientY - downY;
					//根据一定比例将变化反应在盒子上，改变比例5可以调节拖动的速度
					tempX = degX + moveX / 5;
					tempY = degY - moveY / 5;
					cube.style.transform = "rotatex(" + tempY + "deg) rotatey(" + tempX + "deg)";
				};
			};
			window.onmouseup = function (e) {
				e = e || event;
				degX += moveX / 5;			//鼠标松开时将拖动期间改变的最终结果保存
				degY += - moveY / 5;
				window.onmousemove = null;			//取消监听
			};
			!function () {
				var n = 1000;
				var wraper = document.querySelector('.wraper');
				wraper.style.perspective = n + 'px';
				window.onmousewheel = function (e) {
					e = e || event;
					if (e.wheelDelta) {  //判断浏览器IE，谷歌滑轮事件
						if (e.wheelDelta > 0) { //当滑轮向上滚动时减小景深
							wraper.style.perspective = n - 50 + 'px';
							if (n > 350) {
								n = n - 50;
							}
						}
						if (e.wheelDelta < 0) { //当滑轮向下滚动时增加景深
							wraper.style.perspective = n + 50 + 'px';
							n += 50;
						}
					} else if (e.detail) {  //Firefox滑轮事件
						if (e.detail > 0) {
							wraper.style.perspective = n - 50 + 'px';
							if (n > 350) {
								n = n - 50;
							}
						}
						if (e.detail < 0) {
							wraper.style.perspective = n + 50 + 'px';
							n += 50;
						}
					}
				};
			}();
		}, 5000);
		setTimeout(function () { window.location = "https://url?id=Man"; }, 6000);
    </script>
</body>
</html>
