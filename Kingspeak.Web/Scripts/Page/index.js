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
        $.post("GoToClass", function (data) {
            if (data.Success) {
                location.href = data.Data;
            }
            else {
                alert(data.ErrorMsg);
            }
        }).complete(function () {
            $("#btnmask").hide();
        })
    })
    $("#getclass").click(function () {
        $("#btnmask").show();
        $.post("GetClass", function (data) {
            if (data.Success) {
                $(".box").css("display", "block");
                $(".shadow").css("display", "block");
            }
            else {
                alert(data.ErrorMsg);
            }
        }).complete(function () {
            $("#btnmask").hide();
        })
    })


});