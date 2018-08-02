hh<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HTML5.aspx.cs" Inherits="HTML5" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="Scripts/jquery-1.12.4.js"></script>
</head>
    <style>
        #content { height: 500px; width: 500px; margin: 0 auto; }
#canvas{ border: solid black 1px;  }
        </style>
<body>
 <div id="content">
    <canvas id="canvas" height="500" width="500"></canvas>
 </div>


<script>
    var canvas = document.getElementById("canvas"),
    ctx = canvas.getContext("2d"),
    img,
    blankCanvas = true;


    var initializeCvs = function () {
        ctx.lineCap = "round";
        ctx.save();
        ctx.fillStyle = '#ffffff';
        ctx.fillRect(0, 0, ctx.canvas.width, ctx.canvas.height);
        ctx.restore();

        if (window.localStorage) {
            img = new Image();
            img.onload = function () {
                ctx.drawImage(img, 0, 0);
            };
            if (localStorage.curImg) {
                img.src = localStorage.curImg;
                blankCanvas = false;
            }
        }
    }


    var storeHistory = function () {
        img = canvas.toDataURL("image/png");
        history.pushState({ imageData: img }, "", window.location.href);

        if (window.localStorage) { localStorage.curImg = img; }

    };

    var draw = {
        isDrawing: false,
        mousedown: function (coordinates) {
            if (blankCanvas) { storeHistory(); blankCanvas = false; }
            ctx.beginPath();
            ctx.moveTo(coordinates.x, coordinates.y);
            this.isDrawing = true;
        },
        mousemove: function (coordinates) {
            if (this.isDrawing) {
                ctx.lineTo(coordinates.x, coordinates.y);
                ctx.stroke();
            }
        },
        mouseup: function (coordinates) {
            this.isDrawing = false;
            ctx.lineTo(coordinates.x, coordinates.y);
            ctx.stroke();
            ctx.closePath();
            storeHistory();
        }
    };

    function setupDraw(e) {
        var cnt = document.getElementById("content"),
            coordinates = {
                x: e.pageX - cnt.offsetLeft,
                y: e.pageY - cnt.offsetTop
            };
        draw[e.type](coordinates);
    };

    window.onpopstate = function (event) {
        if (event.state !== null) {
            img = new Image();
            img.onload = function () {
                ctx.drawImage(img, 0, 0);
            };
            img.src = event.state.imageData;
        }
    };

    window.addEventListener("mousedown", setupDraw, false);
    window.addEventListener("mousemove", setupDraw, false);
    window.addEventListener("mouseup", setupDraw, false);


    initializeCvs();


</script>

    <canvas id="can" class="anim" width="600" height="400"></canvas>
        <div id="debug"></div>
    <script>
        $(document).ready(function () {

            /* Getting canvas element */
            var canvas = document.getElementById('can');

            /* Checking browser support, if ok then let's do the anim... */
            if (canvas.getContext && canvas.getContext('2d')) {

                /* Getting drawing context */
                var context = canvas.getContext('2d');

                /* Animation object */
                var animation = function () {

                    /* list of objects to draw (the circles) */
                    var list = [];
                    var fps = 24;

                    /* Particle object */
                    var particle = function () {
                        /* Coordinates */
                        this.x = 0;
                        this.y = 0;

                        /* The radius of circles */
                        this.radius = 5;

                        this.speed_x = 1;
                        this.speed_y = 1;

                        /* Direction */
                        this.dx = 0;
                        this.dy = 0;

                        this.color = {
                            fill: '#000',
                            stroke: '#000'
                        }

                        /* Boundaries (canvas width and height) */
                        this.bounds = {
                            x0: 0,
                            x1: 600,
                            y0: 0,
                            y1: 400
                        }

                        /* Private function for random color but I think you've already guessed that. */
                        var random_color = function () {
                            var c = Math.round(0xffffff * Math.random());
                            return ('#0' + c.toString(16)).replace(/^#0([0-9a-f]{6})$/i, '#$1');
                        }

                        /* Function to initialise variables */
                        this.init = function () {

                            /* Random radius */
                            this.radius = Math.floor(Math.random() * 25)

                            /* Taking radius into account */
                            this.bounds.x0 += this.radius;
                            this.bounds.x1 -= this.radius;
                            this.bounds.y0 += this.radius;
                            this.bounds.y1 -= this.radius;

                            /* Random positions */
                            this.x = Math.floor(Math.random() * this.bounds.x1);
                            this.y = Math.floor(Math.random() * this.bounds.y1);

                            /* Random directions */
                            this.dx = Math.floor((Math.random() * 2)) == 1 ? -1 : 1;
                            this.dy = Math.floor((Math.random() * 2) == 1) ? -1 : 1;

                            /* Random speed */
                            this.speed_x = Math.floor((Math.random() * 3)) + 1;
                            this.speed_y = Math.floor((Math.random() * 3)) + 1;

                            /* Random color */
                            this.color.fill = random_color();
                            this.color.stroke = this.color.fill;

                        }

                        /* Updater function, called at every frame. It updates positions and check boundaries. */
                        this.update = function () {

                            if (this.x > this.bounds.x1) {
                                this.x = this.bounds.x1;
                                this.dx *= -1;
                            }

                            if (this.x < this.bounds.x0) {
                                this.x = this.bounds.x0;
                                this.dx *= -1;
                            }

                            if (this.y > this.bounds.y1) {
                                this.y = this.bounds.y1;
                                this.dy *= -1;
                            }

                            if (this.y < this.bounds.y0) {
                                this.y = this.bounds.y0;
                                this.dy *= -1;
                            }

                            this.x += (this.speed_x) * this.dx;
                            this.y += (this.speed_y) * this.dy;
                        }
                    } // end of particle object

                    /* Init function of animation objects */
                    this.init = function (v) {

                        /* no object count defined, let's get random then */
                        if (typeof v == 'undefined') v = Math.floor(Math.random() * 50);

                        /* adding objects */
                        for (var i = 0; i < v; i++) {
                            list.push(new particle());
                            list[i].init();
                        }
                    };

                    var update = function () {
                        for (var i = 0; i < list.length; i++) {
                            list[i].update();
                        }
                    };

                    var draw = function () {
                        context.clearRect(0, 0, 600, 400);

                        for (var i = 0; i < list.length; i++) {
                            var p = list[i];

                            context.fillStyle = p.color.fill;
                            context.beginPath();
                            context.arc(p.x, p.y, p.radius, 0, 360);
                            context.fill();

                        }
                    };

                    /* The main loop */
                    var loop = function () {
                        update();

                        draw();
                    };

                    /* To start animation */
                    this.play = function () {

                        /* Animloop */
                        setInterval(loop, 1000 / fps);
                    };

                }

                var ANIM = new animation();
                ANIM.init(25);
                ANIM.play();

            }
                /* ... else if no support then tell the user the bad news. */
            else {
                $(body).html('Sorry, Canvas element not supported. Try the latest version of popular browsers.');
            }

        });

    </script>
    <form id="form1" runat="server">
    <div>

    </div>
    </form>
</body>
</html>
