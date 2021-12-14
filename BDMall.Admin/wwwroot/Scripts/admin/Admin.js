var miniTab;

layui.use(['miniTab'], function () {
    miniTab = layui.miniTab;
});

//显示提示内容，并附有关闭窗口按钮
function showCloseInfo(message) {
    showInfo(message, null, function () {
        if (self.frameElement && self.frameElement.parentElement) {
            // var tabId = self.frameElement.parentElement.id;
            // parent.closeTab(tabId);
            miniTab.deleteCurrentByIframe();
        }
    });
}


function clowWin() {
    if (self.frameElement && self.frameElement.parentElement) {
        // var tabId = self.frameElement.parentElement.id;
        // parent.closeTab(tabId);
        miniTab.deleteCurrentByIframe();
    }
}

//進行JQuery validate驗證時如果存在多tab，校驗不通過時將焦點指向校驗失敗的tab
function focusWrongPlace(errorMap, callback) {
    var i = 0;
    // 遍历错误列表
    for (var obj in errorMap) {
        // 自定义错误提示效果
        $('input[name=' + obj + ']').parent().addClass('has-error');
        if (i <= 0) {
            if ($('input[name=' + obj + ']').parents(".tab-pane").length > 0) {
                var tabId = $('input[name=' + obj + ']').parents(".tab-pane")[0].id;
                if ($("a[name='" + tabId + "']").length > 0) {
                    $("a[name='" + tabId + "']")[0].click();
                }

            }
            //有"_"存在，代表有多語言。這個會指向為空的語言
            if (obj.indexOf('_') > 0) {
                var lang = obj.split('_')[1];
                callback(lang);

            }
        }
        i++;
    }
}

function focusWrongMultiLanguageTab(errorMap, callback)
{
    var i = 0;
    // 遍历错误列表
    for (var obj in errorMap) {
        if (i <= 0) {
            //有"_"存在，代表有多語言。這個會指向為空的語言
            if (obj.indexOf('_') > 0) {
                var lang = obj.split('_')[1];
                callback(lang);

            }
        }
        i++;
    }
}
//全屏显示大图
function imgShow(outerdiv, innerdiv, bigimg, _this) {
    var src = _this.attr("src");//获取当前点击的pimg元素中的src属性
    src = src.replace("_s.", "_b.");
    $(bigimg).attr("src", src);//设置#bigimg元素的src属性  

    /*获取当前点击图片的真实大小，并显示弹出层及大图*/
    $("<img/>").attr("src", src).load(function () {
        var windowW = $(window).width();//获取当前窗口宽度  
        var windowH = $(window).height();//获取当前窗口高度  
        var realWidth = this.width;//获取图片真实宽度  
        var realHeight = this.height;//获取图片真实高度  
        var imgWidth, imgHeight;
        var scale = 0.8;//缩放尺寸，当图片真实宽度和高度大于窗口宽度和高度时进行缩放  

        if (realHeight > windowH * scale) {//判断图片高度  
            imgHeight = windowH * scale;//如大于窗口高度，图片高度进行缩放  
            imgWidth = imgHeight / realHeight * realWidth;//等比例缩放宽度  
            if (imgWidth > windowW * scale) {//如宽度扔大于窗口宽度  
                imgWidth = windowW * scale;//再对宽度进行缩放  
            }
        } else if (realWidth > windowW * scale) {//如图片高度合适，判断图片宽度  
            imgWidth = windowW * scale;//如大于窗口宽度，图片宽度进行缩放  
            imgHeight = imgWidth / realWidth * realHeight;//等比例缩放高度  
        } else {//如果图片真实高度和宽度都符合要求，高宽不变  
            imgWidth = realWidth;
            imgHeight = realHeight;
        }
        $(bigimg).css("width", imgWidth);//以最终的宽度对图片缩放  

        var w = (windowW - imgWidth) / 2;//计算图片与窗口左边距  
        var h = (windowH - imgHeight) / 2;//计算图片与窗口上边距  
        $(innerdiv).css({ "top": h, "left": w });//设置#innerdiv的top和left属性  
        $(outerdiv).fadeIn("fast");//淡入显示#outerdiv及.pimg  
    });

    $(outerdiv).click(function () {//再次点击淡出消失弹出层  
        $(this).fadeOut("fast");
    });
}

//全屏显示大图
function imgShowBySrc(outerdiv, innerdiv, bigimg,src) {
    $(bigimg).attr("src", src);//设置#bigimg元素的src属性  

    /*获取当前点击图片的真实大小，并显示弹出层及大图*/
    $("<img/>").attr("src", src).load(function () {
        var windowW = $(window).width();//获取当前窗口宽度  
        var windowH = $(window).height();//获取当前窗口高度  
        var realWidth = this.width;//获取图片真实宽度  
        var realHeight = this.height;//获取图片真实高度  
        var imgWidth, imgHeight;
        var scale = 0.8;//缩放尺寸，当图片真实宽度和高度大于窗口宽度和高度时进行缩放  

        if (realHeight > windowH * scale) {//判断图片高度  
            imgHeight = windowH * scale;//如大于窗口高度，图片高度进行缩放  
            imgWidth = imgHeight / realHeight * realWidth;//等比例缩放宽度  
            if (imgWidth > windowW * scale) {//如宽度扔大于窗口宽度  
                imgWidth = windowW * scale;//再对宽度进行缩放  
            }
        } else if (realWidth > windowW * scale) {//如图片高度合适，判断图片宽度  
            imgWidth = windowW * scale;//如大于窗口宽度，图片宽度进行缩放  
            imgHeight = imgWidth / realWidth * realHeight;//等比例缩放高度  
        } else {//如果图片真实高度和宽度都符合要求，高宽不变  
            imgWidth = realWidth;
            imgHeight = realHeight;
        }
        $(bigimg).css("width", imgWidth);//以最终的宽度对图片缩放  

        var w = (windowW - imgWidth) / 2;//计算图片与窗口左边距  
        var h = (windowH - imgHeight) / 2;//计算图片与窗口上边距  
        $(innerdiv).css({ "top": h, "left": w });//设置#innerdiv的top和left属性  
        $(outerdiv).fadeIn("fast");//淡入显示#outerdiv及.pimg  
    });

    $(outerdiv).click(function () {//再次点击淡出消失弹出层  
        $(this).fadeOut("fast");
    });
}