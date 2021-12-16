var WS = {

}

WS.GuidEmpty = "00000000-0000-0000-0000-000000000000";

// 獲取所處平台（PC/移動） （type: 0 => 從父級頁面獲取，1 => 從Tab子頁面獲取）
function disDevices (type,callback) {
    var oWidth = type ? window.parent.document.documentElement.clientWidth : document.documentElement.clientWidth;
    if (oWidth < 768) {
        if (type) {
            WS.inDevice = 'mobile';
        } else {
            WS.outDevice = 'mobile';
        }
    } else {
        if (type) {
            WS.inDevice = 'pc';
        } else {
            WS.outDevice = 'pc';
        }
    }

    if(typeof callback == 'function') {
        callback();
    }

    $(window).resize(function() {
        var onChange = false;
        oWidth = type ? window.parent.document.documentElement.clientWidth : document.documentElement.clientWidth;
        if (oWidth < 768) {
            if (type) {
                if (WS.inDevice !== 'mobile') {
                    onChange = true;
                    WS.inDevice = 'mobile';
                } else {
                    onChange = false;
                }
            } else {
                if (WS.outDevice !== 'mobile') {
                    onChange = true;
                    WS.outDevice = 'mobile';
                } else {
                    onChange = false;
                }
            }
        } else {
            if (type) {
                if (WS.inDevice !== 'pc') {
                    onChange = true;
                    WS.inDevice = 'pc';
                } else {
                    onChange = false;
                }
            } else {
                if (WS.outDevice !== 'pc') {
                    onChange = true;
                    WS.outDevice = 'pc';
                } else {
                    onChange = false;
                }
            }
        }

        if(typeof callback == 'function' && onChange) {
            callback();
        }
    });
}

WS.Get = function (url, data, success, error) {
    checkToken();
    var pmServer = getCookie("PMServer", "/") || "";

    $.ajax({
        type: "get",
        url: pmServer + url,
        data: data,
        beforeSend: function (request) {
            var access_token = $.cookie("access_token");
            request.setRequestHeader("Authorization", "Bearer " + access_token);
            //var userLanguage = $.cookie("Language");
            //request.setRequestHeader("UserLanguage", userLanguage);
        },
        success: function (data, status) {
            if (success) {
                success(data, status);
            }
        },
        error: function (e) {
            if (error) {
                error(e);
            }
        },
        statusCode: {
            404: function () {
                console.log("statusCode=404");
                alert("page not found");
            },
            403: function () {

                console.log("no authorization ");
                //$.cookie("access_token", null, { path: "/" });
                // window.location.href = "/Account/Login?returnUrl=" + window.location.pathname;
                //window.location.href = "/Personal/nopermission";
            },
            500: function (xhr, status, text) {
                console.log("statusCode=500");
                alert(xhr.responseJSON.Message);
            }
        }
    });
}

/**

*/

WS.Post = function (url, data, success, error) {
    var pmServer = getCookie("PMServer", "/") || "";

    checkToken();
    $.ajax({
        type: "post",
        url: pmServer + url,
        data: data,
        beforeSend: function (request) {
            var access_token = $.cookie("access_token");
            request.setRequestHeader("Authorization", "Bearer " + access_token);
            //var userLanguage = $.cookie("Language");
            //request.setRequestHeader("UserLanguage", userLanguage);
        },
        success: function (data, status) {
            if (success) {
                success(data, status);
            }
        },
        error: function (e) {
            if (error) {
                error(e);
            }
        },
        statusCode: {
            404: function () {
                console.log("statusCode=404");
                alert("page not found");
            },
            403: function () {
                //console.log("no authorization "); 
                //window.location.href = "/Account/Login?returnUrl=" + window.location.pathname;
                //window.location.href = "/Personal/nopermission";
            },
            500: function (xhr, status, text) {
                console.log("statusCode=500");
                alert(xhr.responseJSON.Message);
            }
        }
    });
}
function checkToken() {
    //var access_token = $.cookie("access_token");
    //if (!access_token)  window.location.href = "/Account/Login";

    //access_token = window.localStorage.getItem("access_token");
    //if (!access_token) window.location.href = "/Account/Login"; 
}

//single parameter
WS.AjaxSP = function (p) {
    checkToken();
    var pmServer = getCookie("PMServer", "/") || "";
    $.ajax({
        type: p.type,
        url: pmServer + p.url,
        data: p.data,
        beforeSend: function (request) {
            var access_token = $.cookie("access_token");    //从cookie或localstorage取
            request.setRequestHeader("Authorization", "Bearer " + access_token);
            //var userLanguage = $.cookie("Language");
            //request.setRequestHeader("UserLanguage", userLanguage);
        },
        success: function (data, status) {
            if (p.success) {
                p.success(data, status);
            }
        },
        error: function (e) {
            if (p.error) {
                p.error(e);
            }
        },
        statusCode: {
            404: function () {
                console.log("statusCode=404");
                alert("page not found");
            },
            403: function () {
                console.log("no authorization ");
                //$.cookie("access_token", null, { path: "/" });
                //  window.location.href = "/Account/Login?returnUrl=" + window.location.pathname;
                //window.location.href = "/Personal/nopermission";
            },
            500: function (xhr, status, text) {
                console.log("statusCode=500");
                alert(xhr.responseJSON.Message);
            }
        }
    });



}

//傳入Ajax對象
WS.Ajax = function (p) {
    checkToken();
    var pmServer = getCookie("PMServer", "/") || "";
    p.beforeSend = function (request) {
        var access_token = $.cookie("access_token");    // 从cookie或localstorage取
        request.setRequestHeader("Authorization", "Bearer " + access_token);
        //var userLanguage = $.cookie("Language");
        //request.setRequestHeader("UserLanguage", userLanguage);
    };
    if (!p.statusCode) {
        p.statusCode = {
            404: function () {
                alert("page not found");
            },
            403: function () {

                console.log("no authorization ");
                //$.cookie("access_token", null, { path: "/" });
                // window.location.href = "/Account/Login?returnUrl=" + window.location.pathname;
                //window.location.href = "/Personal/nopermission";
            },
            500: function (xhr, status, text) {
                console.log("statusCode=500");
                alert(xhr.responseJSON.Message);
            }
        };
    };
    p.url = pmServer + p.url;
    $.ajax(p);
}


/**
 * 通過傳入參數調用Ajax
 * @param {string} type - Ajax類型.
 * @param {string} url - Ajax路徑.
 * @param {Object} data - Ajax參數.
 * @param {function} successcallback - Ajax成功回調方法.
 * @param {function} errorcallback - Ajax錯誤回調方法.
 */
WS.AjaxP = function (type, url, data, successcallback, errorcallback) {
    checkToken();
    var pmServer = getCookie("PMServer", "/") || "";
    $.ajax({
        type: type,
        url: pmServer + url,
        data: data,
        //async:false,
        dataType: "json",
        success: function (response) {
            successcallback(response);
        },
        error: function (e) {
            if (errorcallback) {
                errorcallback();
            } else {
                showInfo(e);
            }
        },
        beforeSend: function (request) {
            var access_token = $.cookie("access_token");    //从cookie或localstorage取
            request.setRequestHeader("Authorization", "Bearer " + access_token);
            //var userLanguage = $.cookie("Language");
            //request.setRequestHeader("UserLanguage", userLanguage);
        },
        statusCode: {
            404: function () {
                console.log("statusCode=404");
                alert("page not found");
            },
            403: function () {
                console.log("no authorization ");
                //$.cookie("access_token", null, { path: "/" });
                // window.location.href = "/Account/Login?returnUrl=" + window.location.pathname;
                //window.location.href = "/Personal/nopermission";
            },
            500: function (xhr, status, text) {
                console.log("statusCode=500");
                alert(xhr.responseJSON.Message);
            }
        }
    });
}


/**
 * 綁定下拉框
 * @param {string} eid - 下拉框Id.
 * @param {string} url - 獲取下拉框內容路徑.
 * @param {bool} isNeedAll - 是否需要添加-ALL-開頭.
 * @param {object} para - Ajax參數.
 * @param {bool} async - 是否開啟異步.
 */
function InitNormalSelect(eid, url, isNeedAll, para, async) {
    checkToken();
    var tempAjax = "";
    var pmServer = getCookie("PMServer", "/") || "";
    $.ajax({
        type: "get",
        url: pmServer + url,
        async: async,
        data: para,
        success: function (data) {
            if (data.length > 0) {
                //$("#" + eid).selectpicker('show');
                $("#" + eid).show();
                if (isNeedAll === true) {
                    tempAjax += "<option value='-1'>" + Resources.PleaseSelect + "</option>";
                }
                $.each(data, function (i, n) {
                    tempAjax += "<option value='" + n.Id + "'>" + n.Text + "</option>";
                });
                $("#" + eid).empty();
                $("#" + eid).append(tempAjax);
                //更新内容刷新到相应的位置
                //$("#" + eid).selectpicker('render');
                //$("#" + eid).selectpicker('refresh');
            }
            else {
                tempAjax += "<option value='-1'>" + Resources.PleaseSelect + "</option>";
                $("#" + eid).empty();
                $("#" + eid).append(tempAjax);
                //$("#" + eid).empty();
                //$("#" + eid).hide();
            }
        },
        beforeSend: function (request) {
            var access_token = $.cookie("access_token");
            request.setRequestHeader("Authorization", "Bearer " + access_token);
            //var userLanguage = $.cookie("Language");
            //request.setRequestHeader("UserLanguage", userLanguage);
        }
    });
}

/**
 * 綁定下拉框(樹形結構)
 * @param {string} eid - 下拉框Id.
 * @param {string} url - 獲取下拉框內容路徑.
 * @param {bool} isNeedAll - 是否需要添加-ALL-開頭.
 * @param {object} para - Ajax參數.
 * @param {bool} async - 是否開啟異步.
 */
function InitNormalTreeSelect(eid, url, isNeedAll, para, async) {
    var tempAjax = "";
    checkToken();
    $.ajax({
        type: "get",
        url: url,
        async: async,
        data: para,
        success: function (data) {
            if (data.length > 0) {
                $("#" + eid).show();
                if (isNeedAll === true) {
                    tempAjax += "<option value='-1'>" + Resources.PleaseSelect + "</option>";
                }
                for (var i = 0; i < data.length; i++) {
                    tempAjax += "<option value='" + data[i].Id + "'>" + data[i].Text + "</option>";
                    for (var p = 0; p < data[i].Children.length; p++)
                        tempAjax += "<option value='" + data[i].Children[p].Id + "'>" + "--" + data[i].Children[p].Text + "</option>";
                }
                $("#" + eid).empty();
                $("#" + eid).append(tempAjax);
                //更新内容刷新到相应的位置
                //$("#" + eid).selectpicker('render');
                //$("#" + eid).selectpicker('refresh');
            }
            else {
                tempAjax += "<option value='-1'>" + Resources.PleaseSelect + "</option>";
                $("#" + eid).empty();
                $("#" + eid).append(tempAjax);
                //$("#" + eid).empty();
                //$("#" + eid).hide();
            }
        },
        beforeSend: function (request) {
            var access_token = $.cookie("access_token");
            request.setRequestHeader("Authorization", "Bearer " + access_token);
            //var userLanguage = $.cookie("Language");
            //request.setRequestHeader("UserLanguage", userLanguage);
        }
    });
}

/**
 * 傳入數據源綁定下拉框
 * @param {string} eid - 下拉框Id.
 * @param {Enumerable} data - 下拉框內容.
 */
function InitNormalSelectByData(eid, data) {

    var tempAjax = "";
    if (data !== null) {
        if (data.length > 0) {
            $("#" + eid).show();

            tempAjax += "<option value='-1'>" + Resources.PleaseSelect + "</option>";

            $.each(data, function (i, n) {
                tempAjax += "<option value='" + n.Id + "'>" + n.Text + "</option>";
            });
            $("#" + eid).empty();
            $("#" + eid).append(tempAjax);

        }
    }
}

/**
 * 傳入數據源綁定下拉框，不包含額外選項
 * @param {string} eid - 下拉框Id.
 * @param {Enumerable} data - 下拉框內容.
 */
function InitNormalSelectByDataOnly(eid, data) {
    var tempAjax = "";
    if (data !== null) {
        if (data.length > 0) {
            $("#" + eid).show();

            $.each(data, function (i, n) {
                tempAjax += "<option value='" + n.Id + "'>" + n.Text + "</option>";
            });
            $("#" + eid).empty();
            $("#" + eid).append(tempAjax);

        }
    }
}


/**
 * 綁定Bootstrap Select下拉框
 * @param {string} eid - 下拉框Id.
 * @param {string} url - 獲取下拉框內容路徑.
 * @param {int} maxOption - 可以多選時，最多選擇個數.
 * @param {bool} liveSearch - 是否開啟動態查詢.
 * @param {bool} isNeedAll - 是否在開頭添加-ALL-.
 * @param {object} para - Ajax參數.
 * @param {function} callback -回調函數.
 * @param {bool} async - 是否開啟異步.
  */
function InitBootstrapSelect(eid, url, maxOption, liveSearch, isNeedAll, para, callback, async) {
    var tempAjax = "";
    checkToken();
    $("#" + eid).selectpicker({
        liveSearch: liveSearch,
        size: 10,
        //width: width + 'px',
        maxOptions: maxOption
    });
    var pmServer = getCookie("PMServer", "/") || "";


    $.ajax({
        type: "get",
        url: pmServer + url,
        data: para,
        async: async,
        success: function (data) {
            if (isNeedAll) {
                tempAjax += "<option value='-1' selected='selected'>" + Resources.PleaseSelect + "</option>";
            }

            $.each(data, function (i, n) {
                if (i == 0 && isNeedAll == false) {
                    tempAjax += "<option value='" + n.Id + "' selected='selected'>" + n.Text + "</option>";
                }
                else {
                    tempAjax += "<option value='" + n.Id + "'>" + n.Text + "</option>";
                }
            });
            $("#" + eid).empty();
            $("#" + eid).append(tempAjax);
            //更新内容刷新到相应的位置
            $("#" + eid).selectpicker('render');
            $("#" + eid).selectpicker('refresh');
            callback();
        },
        beforeSend: function (request) {
            var access_token = $.cookie("access_token");
            request.setRequestHeader("Authorization", "Bearer " + access_token);
            //var userLanguage = $.cookie("Language");
            //request.setRequestHeader("UserLanguage", userLanguage);
        },
        statusCode: {
            404: function () {
                console.log("statusCode=404");
                alert("page not found");
            },
            403: function () {
                console.log("no authorization ");
                // window.location.href = "/Account/Login?returnUrl=" + window.location.pathname; 
                window.location.href = "/Personal/nopermission";
            },
            500: function (xhr, status, text) {
                console.log("statusCode=500");
                showInfo(xhr.responseJSON.Message);
            }
        }
    });
}


/**
 * 通過傳入數據源綁定Bootstrap Select下拉框
 * @param {string} eid - 下拉框Id.
 * @param {Enumerable} data - 下拉框數據源.
 * @param {int} maxOption - 可以多選時，最多選擇個數.
 * @param {bool} liveSearch - 是否開啟動態查詢.
 * @param {bool} isNeedAll - 是否在開頭添加-ALL-.
 * @param {function} callback -回調函數.
  */
function InitBootstrapSelectByData(eid, data, maxOption, liveSearch, isNeedAll, callback) {
    var tempAjax = "";

    $("#" + eid).selectpicker({
        liveSearch: liveSearch,
        size: 10,
        //width: '300px',
        maxOptions: maxOption
    });


    if (isNeedAll) {
        tempAjax += "<option value='-1'>" + Resources.PleaseSelect + "</option>";
    }

    $.each(data, function (i, n) {
        tempAjax += "<option value='" + n.Id + "'>" + n.Text + "</option>";
    });
    $("#" + eid).empty();
    $("#" + eid).append(tempAjax);
    //更新内容刷新到相应的位置
    $("#" + eid).selectpicker('render');
    $("#" + eid).selectpicker('refresh');
    callback();

}


function InitBootstrapSelectWithGroup(eid, url, para, callback, async) {
    var tempAjax = "";

    $("#" + eid).selectpicker({
        liveSearch: false,
        size: 20,
        //width: width + 'px',
        //maxOptions: maxOption
    });
    var pmServer = getCookie("PMServer", "/") || "";
    $.ajax({
        type: "get",
        url: pmServer + url,
        data: para,
        async: async,
        success: function (data) {
            $.each(data, function (i, n) {
                if (n.SubGroup.length > 0) {
                    tempAjax += "<optgroup label='" + n.Name + "'>";
                    $.each(n.SubGroup, function (j, d) {
                        tempAjax += "<option value='" + d.Id + "'>" + d.Name + "</option>";
                    })
                    tempAjax += "</optgroup>";
                }
                else {
                    tempAjax += "<option value='" + n.Id + "'>" + n.Name + "</option>";
                }
            });
            $("#" + eid).empty();
            $("#" + eid).append(tempAjax);
            //更新内容刷新到相应的位置
            $("#" + eid).selectpicker('render');
            $("#" + eid).selectpicker('refresh');
            callback();
        },
        beforeSend: function (request) {
            //var access_token = $.cookie("access_token");
            //request.setRequestHeader("Authorization", "Bearer " + access_token);
            //var userLanguage = $.cookie("Language");
            //request.setRequestHeader("UserLanguage", userLanguage);
        }
    });
}

/**
*單個文件上傳類型
* @param {string} CtrlName-控件id
* @param {string} UploadUrl-上傳方法路徑
* @param {function} SuccessCallback - 上傳成功后回調函數
* @param {function} ErrorCallback  - 上傳失敗后回調函數
*/
//var SingleFileInputOption = function () {

//    this.CtrlName = "";
//    this.UploadUrl = "";
//    this.SuccessCallback = "";
//    this.ErrorCallback = "";
//    this.FileExtensions = ['jpg', 'png'];
//}

///**
//* 初始化上傳單個圖片控件
//*/
//function FileInputInit(singleFileInputOption) {

//    //初始化fileinput控件（第一次初始化）
//    //this.Init = function (ctrlName, uploadUrl) {
//    var control = $('#' + singleFileInputOption.CtrlName);

//    //初始化上传控件的样式
//    control.fileinput({
//        language: 'LANG', //设置语言
//        uploadUrl: singleFileInputOption.UploadUrl, //上传的地址
//        allowedFileExtensions: singleFileInputOption.FileExtensions,//接收的文件后缀
//        showUpload: false, //是否显示上传按钮
//        showCaption: true,//是否显示标题
//        browseClass: "btn btn-primary", //按钮样式
//        dropZoneEnabled: false,//是否显示拖拽区域
//        showPreview: false,

//        //minImageWidth: 50, //图片的最小宽度
//        //minImageHeight: 50,//图片的最小高度
//        //maxImageWidth: 1000,//图片的最大宽度
//        //maxImageHeight: 1000,//图片的最大高度
//        //maxFileSize: 0,//单位为kb，如果为0表示不限制文件大小
//        //minFileCount: 0,
//        maxFileCount: 1,
//        enctype: 'multipart/form-data'
//        //uploadExtraData: { isOverCover: false }
//        //validateInitialCount: true,
//        //previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",

//        //msgFilesTooMany: "选择上传的文件数量({n}) 超过允许的最大数值{m}！",
//    });

//    //导入文件上传完成之后的事件
//    control.on("filebatchselected", function (event, files) {
//        $(this).fileinput("upload");
//    });

//    control.on("fileerror", function (event, files) {
//        singleFileInputOption.ErrorCallback();
//    });

//    control.on("fileuploaded", function (event, data, previewId, index) {
//        singleFileInputOption.SuccessCallback(data);
//    });

//};

/**
*文件上傳參數
* @param {string} CtrlName-控件id
* @param {string} UploadUrl-上傳方法路徑
* @param {int} MaxFile-一次最多上傳個數
* @param {function} SuccessCallback - 上傳成功后回調函數
* @param {function} SelectedCallback - 選擇圖片后回調函數
* @param {function} ErrorCallback  - 上傳失敗后回調函數
*/
var FileInputOption = function () {

    this.CtrlName = "";
    this.UploadUrl = "";
    this.MaxFile = 1;
    this.SuccessCallback = "";
    this.SelectedCallback = "";
    this.ErrorCallback = "";
    this.FileExtensions = ['jpg', 'png', 'jpeg'];
    this.FileSize = 2048;
    this.ShowRemove = true;
    this.ShowUpload = false;
    this.ShowPerview = true;
    this.MinHeight = null;
    this.MinWidth = null;
    this.MaxHeight = null;
    this.MaxWidth = null;
    this.ShowCaption = false;

}

/**
*初始化多圖片上傳控件
*/
function FileInputInit(FileInputOption) {

    //初始化fileinput控件（第一次初始化）
    //this.Init = function (ctrlName, uploadUrl) {
    var control = $('#' + FileInputOption.CtrlName);
    //var isShowUploadButton = false;
    var uploadAsync = true;
    if (FileInputOption.MaxFile > 1) {
        FileInputOption.ShowUpload = true;
        uploadAsync = false;
    }
    //初始化上传控件的样式
    control.fileinput({
        language: 'LANG', //设置语言
        uploadAsync: uploadAsync,
        uploadUrl: FileInputOption.UploadUrl, //上传的地址
        allowedFileExtensions: FileInputOption.FileExtensions,//接收的文件后缀
        showUpload: FileInputOption.ShowUpload, //是否显示上传按钮
        showCaption: FileInputOption.ShowCaption,//是否显示标题
        showRemove: FileInputOption.ShowRemove,
        browseClass: "btn btn-primary", //按钮样式
        dropZoneEnabled: false,//是否显示拖拽区域
        showPreview: FileInputOption.ShowPerview,
        maxFileCount: FileInputOption.MaxFile,
        maxFileSize: FileInputOption.FileSize,
        enctype: 'multipart/form-data',
        minImageWidth: FileInputOption.MinHeight,
        minImageHeight: FileInputOption.MinHeight,
        maxImageHeight: FileInputOption.MaxHeight,
        maxImageWidth: FileInputOption.MaxWidth
    });

    if (FileInputOption.MaxFile == 1) {//单文件上传
        if (!FileInputOption.ShowUpload) {
            control.on("filebatchselected", function (event, files) {

                if (files.length <= 0) {
                    if (FileInputOption.ErrorCallback) {
                        FileInputOption.ErrorCallback();
                    }
                } else {
                    $(this).fileinput("upload");
                }
            });
        }

        if (FileInputOption.ErrorCallback) {
            control.on("fileerror", function (event, files, msg) {
                FileInputOption.ErrorCallback();
            });
        }
        if (FileInputOption.SuccessCallback) {
            control.on("fileuploaded", function (event, data, previewId, index) {
                FileInputOption.SuccessCallback(data);
            });
        }

    } else {//多文件上传
        if (FileInputOption.SelectedCallback) {
            control.on("filebatchselected", function (event, files) {
                FileInputOption.SelectedCallback(files);
            });
        }
        if (FileInputOption.ErrorCallback) {
            control.on("filebatchuploaderror", function (event, data, msg) {
                FileInputOption.ErrorCallback();
            });
        }
        if (FileInputOption.SuccessCallback) {
            control.on("filebatchuploadsuccess", function (event, data, previewId, index) {
                FileInputOption.SuccessCallback(data);
            });
        }
    }
};

/**
  * 重置文件上傳控件的perview區域
  */
function clearPerview(id) {
    $("#" + id).fileinput("refreshContainer");
    $(".file-preview").css('visibility', 'hidden');
}

/**
  * @param {number} closeDelay  延迟关闭时间
  */
function showLoading(text, closeDelay) {
    //if (!inited_loadingUI) {
    //    initLoading();
    //}
    if (closeDelay) {
        hideLoading(closeDelay);
    }

    if (!text) {
        text = Resources.Processing;
    }

    $.blockUI({ message: "<img src='/images/busy.gif'/>" + text });
}
function hideLoading(delay) {
    if (!delay) {
        delay = 500;
    }

    setTimeout(function () {
        $.unblockUI();
    }, delay);

}

/**
初始化日曆控件
*/
function InitDateTimePicker() {
    $("input[name^='DateTimePicker']").datetimepicker({
        format: 'yyyy-mm-dd',
        autoclose: true,
        todayBtn: true,
        minView: 'month',
        pickerPosition: 'bottom-left'
    });
}

function InitDateTimePickerWithTime() {
    var newDate = new Date();
    //newDate.setDate(newDate.getDate() + 1);
    var t = newDate.toJSON();
    $("input[name^='DateTimePicker']").datetimepicker({
        format: 'yyyy-mm-dd hh:ii:00',
        autoclose: true,
        todayBtn: true,
        minuteStep: 15,
        //minView: 'month',
        //pickerPosition: 'bottom-left',
        pickerPosition: 'top-right',
        weekStart: 1,
        startDate: new Date(t),
    });
}

function InitDateTimePickerWithStartDate(minDateVal) {
    var myDTPicker = $("input[name^='DateTimePickerWithStart']");

    var minDate = new Date(minDateVal);
    var startDate = new Date(minDate.toJSON());
    myDTPicker.datetimepicker({
        format: 'yyyy-mm-dd',
        autoclose: true,
        todayBtn: false,
        minView: 'month',
        pickerPosition: 'top-left',
        weekStart: 1,
        todayHighlight: 0,
        startView: 2,
        forceParse: 0,
        startDate: startDate,
    });

    var tt = startDate.getFullYear() + "-" + (startDate.getMonth() + 1) + "-" + startDate.getDate();
    myDTPicker.datetimepicker('update', tt);
}

function InitDateTimePickerForMonth() {
    $("input[name^='MonthPicker']").datetimepicker({
        format: 'yyyy-mm',
        autoclose: true,
        todayBtn: false,
        maxView: 4,
        minView: 3,
        startView: 3,
        pickerPosition: 'bottom-left'
    });
}


function showInfo(message, okCallback, closeCallback) {
    var title = 'System Message';
    var ok = "OK";
    if (Resources && Resources.InfoTitle) {
        title = Resources.InfoTitle;
        ok = Resources.Ok;
    }
    var buttons = [
        {
            type: BootstrapDialog.TYPE_INFO,
            label: ok,
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                dialogItself.close();
                if (okCallback) {
                    okCallback();
                }
            }
        }
    ];
    if (closeCallback) {
        buttons.push({
            type: BootstrapDialog.TYPE_INFO,
            label: Resources.CloseTab,
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                dialogItself.close();
                closeCallback();
            }
        });
    }

    BootstrapDialog.show({
        title: title,
        message: message,
        buttons: buttons
    });
}


function showWarn(message) {
    var title = 'System Message';
    var close = "Close";
    if (Resources && Resources.InfoTitle) {
        title = Resources.InfoTitle;
        close = Resources.Close;
    }
    BootstrapDialog.show({
        title: title,
        type: BootstrapDialog.TYPE_PRIMARY,
        message: message,
        buttons: [{
            label: close,
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
}
function showError(message) {
    var title = 'System Message';
    var close = "Close";
    if (Resources && Resources.InfoTitle) {
        title = Resources.InfoTitle;
        close = Resources.Close;
    }
    BootstrapDialog.show({
        title: title,
        type: BootstrapDialog.TYPE_PRIMARY,
        message: message,
        buttons: [{
            label: close,
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
}
function SystemConfirm(message, callback) {
    var title = 'System Message';
    var yes = "Yes";
    var no = "No";
    if (Resources && Resources.InfoTitle) {
        title = Resources.InfoTitle;
        yes = Resources.Yes;
        no = Resources.No;
    }
    BootstrapDialog.show({
        title: title,
        type: BootstrapDialog.TYPE_WARNING,
        message: message,
        buttons: [{
            label: yes,
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                callback();
                dialogItself.close();
            }
        },
        {
            label: no,
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
}

//打開Dialog，內容為頁面
function OpenDialog(title, width, height, url, params, callback) {
    bDialog.open({
        title: title,
        width: width,
        height: height,
        url: url,
        params: params,
        callback: function (data) {
            callback(data);
        }
    });
}

//打開dialog，內容為html
function OpenDialogByHtml(title, width, height, domContent, params, callback) {
    bDialog.open({
        title: title,
        width: width,
        height: height,
        dom: domContent,
        params: params,
        callback: function (data) {
            callback(data);
        }
    });
}

function setCookie(name, value, path, expiredays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + expiredays);
    document.cookie = name + "=" + escape(value) +
        ((expiredays === null) ? "" : ";expires=" + exdate.toGMTString()) + ";path=" + path;
}
function getCookie(name, path) {
    return $.cookie(name);
}

String.prototype.ReplaceAll = function (oldStr, newStr) {//將字符串oldStr替換成newStr
    var reg = new RegExp(oldStr, "g"); //創建正則RegExp對象   
    return this.replace(reg, newStr);
}

function validatorHTML() {
    jQuery.validator.addMethod("validatorHTML", function (value, element, param) {
        return charFilter(value);
        
    }, Resources.ExistHTMLLabel);
}

function charFilter(val) {
    var reg = new RegExp("<\\s*(img|br|p|b|/p|a|div|iframe|button|script|i|html|form|input|frameset|body|table|br|label|link|li|style).*?>", "i"); //創建正則RegExp對象   
    //var reg = new RegExp("/<(\S*?)[^>]*>.*?|<.*? />/", "i"); //創建正則RegExp對象   
    var a = val.search(reg);
    if (a >= 0) {
        return false;
    }
    else {
        return true;
    }
}

