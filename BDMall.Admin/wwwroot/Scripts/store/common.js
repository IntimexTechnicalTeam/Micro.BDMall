function getPMHost() {
    return $.cookie("PMServer");
}
function getCustUILanguage() {
    return $.cookie("uLanguage");
}

function WSAjaxStart() {
    $(document).ajaxStart(function () {
        showLoading("Loading...");
    });
}

function WSAjaxComplete() {
    $(document).ajaxComplete(function () {
        hideLoading();
    });
}

function WSGet(url, data, success, error) {
    var accessToken = $.cookie("access_token");
 
    // if (!accessToken) {
    //     if (!redirectJob) {
    //         setTimeout(function () { window.location.href = window.location.href; }, 5000);
    //     }
    //     redirectJob = true;
    //     return;
    // }
    $.ajax({
        type: "get",
        url: url,
        data: data,
        beforeSend: function (request) {
            request.setRequestHeader("Authorization", "Bearer " + accessToken);

            var userLanguage = $.cookie("uLanguage");
            request.setRequestHeader("UserLanguage", userLanguage);
        },
        statusCode: {
            404: function () {
                console.log("statusCode=404");
                //  alert("page not found");
            },
            403: function () {
                //console.log("statusCode=403");
                //window.location.href = "/en/member/login.aspx?returnUrl=" + window.location.href;
                $.cookie("access_token", null, { path: "/" });
               // setTimeout(function () { window.location.href = window.location.href; }, 5000);
            },
            500: function (xhr, status, text) {
                console.log("statusCode=500");
                alert(xhr.responseJSON.Message);
            }
        },
        success: function (data, status) {
            if (success) {
                if (status === "success") {
                    success(data, status);
                }
            }
        },
        error: function (e) {

            if (error) {
                error(e);
            }
        }
    });
}
var redirectJob = false;
function WSPost(url, data, success, error) {
    var accessToken = $.cookie("access_token");
    // if (!accessToken) {
    //     if (!redirectJob) {
    //         setTimeout(function () { window.location.href = window.location.href; }, 5000);
    //     }
    //     redirectJob = true;
    //     return;
    // }
    $.ajax({
        type: "post",
        url: url,
        data: data,
        beforeSend: function (request) {
            request.setRequestHeader("Authorization", "Bearer " + accessToken);
            var userLanguage = $.cookie("uLanguage");
            request.setRequestHeader("UserLanguage", userLanguage);
        },
        statusCode: {
            404: function () {
                alert("page not found");
            }, 403: function () {
                $.cookie("access_token", null, { path: "/" });
               // setTimeout(function () { window.location.href = window.location.href; }, 5000);
            }
        },
        success: function (data, status) {
            if (success) {
                success(data, status);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (error) {
                error(XMLHttpRequest, textStatus, errorThrown);
            }
        }
    });
}

//single parameter
function WSAjaxSP(p) {
    p.beforeSend = function (request) {
        var accessToken = $.cookie("access_token");
        // console.log(p.url + "||" + access_token);
        request.setRequestHeader("Authorization", "Bearer " + accessToken);

        var userLanguage = $.cookie("uLanguage");
        request.setRequestHeader("UserLanguage", userLanguage);

    };
    if (!p.statusCode) {
        p.statusCode = {
            404: function () {
                alert("page not found");
            },
            403: function () {
                $.cookie("access_token", null, { path: "/" });
               // setTimeout(function () { window.location.href = window.location.href; }, 5000);
            }
        };
    }
    $.ajax(p);
}

//type:post、get
//url:api路径
//data: var obj = new Object();
//      obj.id = id;
//successcallback:执行成功的回调函数
//errorcallback:执行失败的回调函数
function WSAjax(type, url, data, successcallback, errorcallback) {
    $.ajax({
        type: type,
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            successcallback(response);
        },
        statusCode: {
            404: function () {
                alert("page not found");
            }, 403: function () {
                $.cookie("access_token", null, { path: "/" });
                //setTimeout(function () { window.location.href = window.location.href; }, 5000);
            }
        },
        error: function () {
            errorcallback();
        },
        beforeSend: function (request) {
            var accessToken = $.cookie("access_token");
            request.setRequestHeader("Authorization", "Bearer " + accessToken);

            var userLanguage = $.cookie("uLanguage");
            request.setRequestHeader("UserLanguage", userLanguage);
        }
    });
}

var inited_loadingUI = false;

function initLoading() {
    $("body").append("<!-- loading -->" +
        "<div class='modal fade' id='loading' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' data-backdrop='static'>" +
        "<div class='modal-dialog' role='document'>" +
        "<div class='modal-content'>" +
        "<div class='modal-header'>" +
        "<h4 class='modal-title' id='myModalLabel'>Tips</h4>" +
        "</div>" +
        "<div id='loadingText' class='modal-body'>" +
        "<span class='glyphicon glyphicon-refresh' aria-hidden='true'>1</span>" +
        "处理中，请稍候。。。" +
        "</div>" +
        "</div>" +
        "</div>" +
        "</div>"
    );
    inited_loadingUI = true;
}

function showLoading(text, closeDelay) {

    if (!inited_loadingUI) {
        initLoading();
    }

    if (closeDelay) {
        hideLoading(closeDelay);
    }

    if (!text) {
        text = "Processing please wait!";
    }

    //$("#loadingText").html(text);
    //$("#loading").modal("show"); 
    $.blockUI({ message: "<img src='/images/busy.gif'/>" + text });


}

function hideLoading(delay) {
    if (!delay) {
        delay = 500;
    }
    //setTimeout(function () {
    //    $("#loading").modal("hide");
    //}, delay);
    setTimeout(function () {
        $.unblockUI();
    }, delay);

}


function showInfo(message) {
    $.blockUI({
        message: message,
        css: { top: '20%' }
    });

    setTimeout($.unblockUI, 1000);
}
function showWarn(message) {

    $.blockUI({
        message: message,
        css: { top: '20%' }
    });

    $('.blockOverlay').attr('title', 'Click to unblock').click($.unblockUI);
}
function showError(message) {
    $.blockUI({
        message: message,
        css: { top: '20%' }
    });

    $('.blockOverlay').attr('title', 'Click to unblock').click($.unblockUI);
}
function showConfirm(message, callback) {
    BootstrapDialog.show({
        title: 'System Message',
        type: BootstrapDialog.TYPE_WARNING,
        message: message,
        buttons: [{
            label: 'Yes',
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                callback();
                dialogItself.close();
            }
        },
        {
            label: 'No',
            cssClass: 'btn-default',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
}

function createMessage() {

    $("body").append("<div class='modal fade' id='alertMsgDiv' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' data-backdrop='static'>" +
        "<div class='modal-dialog' role='document'>" +
        "<div class='modal-content'>" +
        "<div class='modal-header'>" +
        "<h4 class='modal-title' id='myModalLabel'>Tips</h4>" +
        "</div>" +
        "<div id='alertMessageSpan' class='modal-body'>" +
        "<span class='glyphicon glyphicon-refresh' aria-hidden='true'>1</span>" +
        "处理中，请稍候。。。" +
        "</div>" +
        "</div>" +
        "</div>" +
        "</div>"
    );
}

function closeAlert() {
    $("#alertMsgDiv").modal("hide");
}


function setCookie(name, value, path, expiredays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + expiredays);
    document.cookie = name + "=" + escape(value) +
        ((expiredays === null) ? "" : ";expires=" + exdate.toGMTString()) + ";path=" + path;
}
function getCookie(name, path) {
    $.cookie("name");
}

String.format = function () {
    if (arguments.length === 0)
        return null;
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
};

