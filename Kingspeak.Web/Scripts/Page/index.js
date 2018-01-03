// JavaScript Document
$(function () {

    /*领取弹框*/
    //$(".header .head_nr .order").click(function () {
    //    $(".box").css("display", "block");
    //    $(".shadow").css("display", "block");
    //});
    //$(".videos .waijiao a").click(function () {
    //    $(".box").css("display", "block");
    //    $(".shadow").css("display", "block");
    //});

    /*点击弹窗消失*/
    $(".box .close").click(function () {
        $(".box").css("display", "none");
        $(".shadow").css("display", "none");
    });
    $(".box .sour").click(function () {
        $(".box").css("display", "none");
        $(".shadow").css("display", "none");
    });

    /*切换电视*/
    $(".videos .videos_nr ul li").click(function () {
        var num = $(this).index();
        $(".videos .videos_nr ul li").removeClass("on");
        $(this).addClass("on");
        var mp4url = $(".videos .videos_nr video").attr("src");
        var posterurl = $(".videos .videos_nr video").attr("poster");
        var tmp = mp4url.substring(0, mp4url.indexOf('t/') + 1);
        $(".videos .videos_nr video").attr("src", tmp + "/video/video" + (num + 1) + ".mp4");
        $(".videos .videos_nr video").attr("poster", tmp + "/images/video" + (num + 1) + ".png");
        $(".videos .waijiao p").css("display", "none");
        $(".videos .waijiao .p" + (num + 1)).css("display", "block");
    });

    /*点击播放视频*/
    $(".videos .videos_nr  img").click(function () {
        var video1 = document.getElementById("video1");
        video1.play();
        $(this).css("display", "none");
    });

    $("#gotoclass").click(function () {
        $("#btnmask").show();
        $.post("XZJTYGoToClass", function (data) {
            if (data.Success) {
                location.href = data.Data;
            }
            else {
                if (data.ErrorCode == "001") {
                    var html = '<img class="img2" src="../Content/images/tanchu1.png" alt=""><p class="p2">你还没有领取课程呢</p><p>请先领取课程哦！</p><a href="javascript:layer.closeAll();" class="sour">确认</a>';
                    Show(html);
                }
                else if (data.ErrorCode == "002") {
                    var html = '<img class="img2" width=150px; height=150px; src="../Content/images/kcgw.jpg" alt=""><p class="p2">请等待课程顾问为你服务</p><p>等不及的话，扫码联系课程顾问</p><img class="img1" src="../Content/images/star.png" alt=""><a href="javascript:layer.closeAll();" class="sour">确认</a>';
                    Show(html);
                }
                else {
                    layer.alert(data.ErrorMsg, { closeBtn: 0, time: 2000 });
                }
            }
        }).complete(function () {
            $("#btnmask").hide();
        })
    })

    $("#getclass").click(function () {
        $("#btnmask").show();
        $.post("GetClass", function (data) {
            if (data.Success) {
                var html = '<p class="p1">恭喜您领取成功!</p><p>我们的课程顾问将会</p><p>第一时间联系您!</p><img class="img1" src="../Content/images/star.png" alt=""><a href="javascript:layer.closeAll();" class="sour">确认</a>';
                Show(html);
            }
            else {
                if (data.ErrorCode == "001") {
                    var html = '<img class="img2" width=150px; height=150px; src="../Content/images/kcgw.jpg" alt=""><p class="p2">你已经领取了当前课程</p><p>如有疑问，请扫描二维码联系课程顾问</p><img class="img1" src="../Content/images/star.png" alt=""><a href="javascript:layer.closeAll();" class="sour">确认</a>';
                    Show(html);
                }
                else {
                    layer.alert(data.ErrorMsg, { closeBtn: 0, time: 2000 });
                }
            }
        }).complete(function () {
            $("#btnmask").hide();
        })
    })

    function Show(content) {
        layer.open({
            type: 1 //Page层类型
              , area: ['420px', '400px']
              , title: false
              , shade: 0.6 //遮罩透明度
              , maxmin: false//允许全屏最小化
              , anim: 3 //0-6的动画形式，-1不开启
              , content: content
        });
    }

    var loginresult = $("#loginResult").val();
    if (loginresult && loginresult != "true") {
        layer.msg("登录失败：" + loginresult, { offset: 't', time: 800 });
    }
});