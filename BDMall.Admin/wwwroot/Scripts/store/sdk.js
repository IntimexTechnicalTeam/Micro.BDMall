/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 2);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var WSAPI = /** @class */ (function () {
    function WSAPI() {
        this.debug = true;
        this.apiHost = getPMHost(); //$.cookie("PMServer");
        this.apiPath = this.apiHost + "/API";
    }
    //controller: Array<object>;
    WSAPI.prototype.log = function (arg1, arg2) {
        if (this.debug) {
            console.log(arg1);
            if (arg2 != undefined) {
                console.log(arg2);
            }
        }
    };
    ;
    return WSAPI;
}());
exports.WSAPI = WSAPI;


/***/ }),
/* 1 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var MemberApi = /** @class */ (function (_super) {
    __extends(MemberApi, _super);
    function MemberApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     *  登入系統
     * @param account
     * @param password
     * @param rememberMe
     * @param callback
     */
    MemberApi.prototype.login = function (account, password, rememberMe, callback) {
        var _this = this;
        WSPost(
        //this.apiPath + "/Member/Login",
        "/account/Login", { Account: account, Password: password, RememberMe: rememberMe }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                    WSGet(addTimeStamp("/Account/GetMemberInfo"), {}, function () {
                        if (callback) {
                            callback(data);
                        }
                    }, function (e) {
                        alert(e);
                    });
                }
                else {
                    if (callback) {
                        callback(data);
                    }
                }
            }
        });
    };
    ;
    MemberApi.prototype.logout = function (callback) {
        // setCookie("access_token", "", "/", -1);
        // setCookie("uid", "", "/", -1); 
        var _this = this;
        WSGet(this.apiPath + "/Member/Logout", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
                window.location.href = "/account/Logoff";
            }
        });
    };
    ;
    /**
     * 登入系統
     * @param id
     * @param key
     * @param callback
     */
    MemberApi.prototype.fbLogin = function (id, key, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Member/FBLogin", { fbId: id, key: key }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                    var token = data.ReturnValue;
                    setCookie("access_token", token.access_token, "/", 1);
                }
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    MemberApi.prototype.updatePassword = function (oldpwd, newpwd, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/UpdatePassword", { oldpwd: oldpwd, newpwd: newpwd }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                }
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    MemberApi.prototype.getMallFun = function (callback) {
        WSGet(this.apiPath + "/Member/GetMemberMallFun", {}, function (data, status) {
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    /**
     * 注册会员
     * @param data
     * @param callback
     */
    MemberApi.prototype.register = function (data, callback) {
        WSAjaxSP({
            type: "Post",
            url: this.apiPath + "/Member/Register",
            data: data,
            success: callback,
            error: function (e) {
            },
            complete: function () {
            }
        });
    };
    ;
    MemberApi.prototype.joinAccount = function (data, callback) {
        WSAjaxSP({
            type: "Post",
            url: this.apiPath + "/Member/AssociatedThirdPartyAccount ",
            data: data,
            success: callback,
            error: function (e) {
            },
            complete: function () {
            }
        });
    };
    ;
    /**
    * 获取会员信息
    * @param callback
    */
    MemberApi.prototype.getProfile = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/getProfile", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                setCookie("uid", data.ReturnValue.Id, "/", null);
                if (data.Succeeded) {
                    setCookie("logined", "1", "/", null);
                    if (callback) {
                        callback(data.ReturnValue);
                    }
                    setInterval(function () {
                        var token = getCookie("access_token", "/");
                        if (!token) {
                            window.location.href = window.location.href;
                            setCookie("logined", "0", "/", -1);
                        }
                        else {
                            _this.log("token valid");
                        }
                    }, 5000);
                }
                else {
                    setCookie("logined", "0", "/", null);
                    if (data.ReturnValue) {
                        var l = getCookie("uLanguage", "/");
                        if (!l) {
                            setCookie("uLanguage", data.ReturnValue.LanguageCode, "/", null);
                            window.location.reload(true);
                        }
                    }
                    if (callback) {
                        callback();
                    }
                    setInterval(function () {
                        var token = getCookie("access_token", "/");
                        if (!token) {
                            window.location.href = window.location.href;
                        }
                        else {
                            _this.log("token valid");
                        }
                    }, 5000);
                }
            }
        });
    };
    ;
    /**
     * 获取会员信息
     * @param callback
     */
    MemberApi.prototype.getMemberInfo = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/getMemberInfo", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                    setCookie("uid", data.ReturnValue.Id, "/", null);
                    setCookie("logined", "1", "/", null);
                    if (callback) {
                        callback(data.ReturnValue);
                    }
                    setInterval(function () {
                        var token = getCookie("access_token", "/");
                        if (!token) {
                            // window.location.href = window.location.href;
                            setCookie("logined", "0", "/", -1);
                        }
                        else {
                            _this.log("token valid");
                        }
                    }, 5000);
                }
                else {
                    setCookie("logined", "0", "/", null);
                    if (data.ReturnValue) {
                        var l = getCookie("uLanguage", "/");
                        if (!l) {
                            setCookie("uLanguage", data.ReturnValue.LanguageCode, "/", null);
                            window.location.reload(true);
                        }
                    }
                    if (callback) {
                        callback(data.ReturnValue);
                    }
                    setInterval(function () {
                        var token = getCookie("access_token", "/");
                        if (!token) {
                            // window.location.href = window.location.href;
                        }
                        else {
                            _this.log("token valid");
                        }
                    }, 5000);
                }
            }
        });
    };
    ;
    /**
     * 更新會員資料
     * @param obj
     * @param callback
     */
    MemberApi.prototype.updateProfile = function (model, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Member/UpdateProfile", model, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        var lang = "C";
                        if (model.Language == 0) {
                            lang = "E";
                        }
                        else if (model.Language == 1) {
                            lang = "C";
                        }
                        else {
                            lang = "S";
                        }
                        setCookie("uLanguage", lang, "/", null);
                    }
                    callback(data);
                }
            }
        });
    };
    ;
    MemberApi.prototype.activate = function (mid, tid, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/Activate", { mid: mid, tid: tid }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    /**
     *
     * @param prodId
     * @param success
     * @param fail
     */
    MemberApi.prototype.addFavorite = function (prodId, success, fail) {
        var path = this.apiPath + "/member/AddFavorite";
        WSGet(path, { productId: prodId }, function (result, status) {
            if (result) {
                if (result.Succeeded) {
                    if (success) {
                        success(result);
                    }
                }
                else {
                    if (fail) {
                        fail(result.ReturnValue);
                    }
                    else {
                        showSystemError(result.ReturnValue);
                    }
                }
            }
            else {
                showMessage("Please try again later.");
            }
        });
    };
    ;
    /**
     *
     * @param prodId
     * @param success
     * @param fail
     */
    MemberApi.prototype.removeFavorite = function (prodId, success, fail) {
        var _this = this;
        WSGet(this.apiPath + "/member/RemoveFavorite", { productId: prodId }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                    if (success) {
                        success(data);
                    }
                }
                else {
                    if (fail) {
                        fail(data.ReturnValue);
                    }
                }
            }
        });
    };
    ;
    /**
  *
  * @param prodCode
  * @param success
  * @param fail
  */
    MemberApi.prototype.checkFavorite = function (prodCode, success, fail) {
        var _this = this;
        WSGet(this.apiPath + "/member/CheckFavorite", { code: prodCode }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (data) {
                    if (data.Succeeded) {
                        if (success) {
                            success(data);
                        }
                    }
                    else {
                        if (fail) {
                            fail(data.ReturnValue);
                        }
                    }
                }
            }
        });
    };
    ;
    /**
     *
     * @param callback
     */
    MemberApi.prototype.getFavorite = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/GetFavorite", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            }
        });
    };
    ;
    /**
     *
     * @param code
     * @param callback
     */
    MemberApi.prototype.setUILanguage = function (code, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/SaveUILang", { lang: code }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    MemberApi.prototype.setCurrency = function (cur, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/SetCurrency", { currency: cur }, function (data, status) {
            _this.log(data);
            if (data.Succeeded) {
                if (callback) {
                    callback(data);
                }
                else {
                    window.location.reload(true);
                }
            }
            else {
                alert(data.Message);
            }
        });
    };
    ;
    MemberApi.prototype.getCurrency = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/GetCurrency", {}, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    /**
     *
     * @param account 發送重置密碼郵件
     * @param callback
     */
    MemberApi.prototype.resetPassword = function (account, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/resetPassword", { account: account }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    MemberApi.prototype.forgetPassword = function (account, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/ForgetPassword", { account: account }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    MemberApi.prototype.resendActive = function (account, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/ResendActive", { account: account }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    /**
     * 從重置密碼郵件更新密碼
     * @param mid
     * @param code
     * @param pwd
     * @param callback
     */
    MemberApi.prototype.updatePwdFM = function (mid, code, pwd, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/UpdatePwdFM", { id: mid, code: code, password: pwd }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    /**
     * 检查邮件是否注册
     * @param email
     * @param callback
     */
    MemberApi.prototype.checkIsRegister = function (email, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/CheckMemberIsRegister", { email: email }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    /**
     * 订阅
     * @param email
     * @param callback
     */
    MemberApi.prototype.subscribe = function (email, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Member/Subscribe", { email: email }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    return MemberApi;
}(wsapi_1.WSAPI));
exports.MemberApi = MemberApi;


/***/ }),
/* 2 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var instoreSdk_1 = __webpack_require__(3);
window.InstoreSdk = new instoreSdk_1.InstoreSdk();


/***/ }),
/* 3 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var productApi_1 = __webpack_require__(4);
var promotionApi_1 = __webpack_require__(5);
var customerServiceApi_1 = __webpack_require__(6);
var deliveryApi_1 = __webpack_require__(7);
var memberApi_1 = __webpack_require__(1);
var ShoppingCartApi_1 = __webpack_require__(8);
var cmsApi_1 = __webpack_require__(9);
var orderApi_1 = __webpack_require__(10);
var paymentApi_1 = __webpack_require__(11);
var messageApi_1 = __webpack_require__(12);
var prodcommentApi_1 = __webpack_require__(13);
var uploadfileApi_1 = __webpack_require__(14);
var en_1 = __webpack_require__(15);
var zh_HK_1 = __webpack_require__(16);
var zh_CN_1 = __webpack_require__(17);
var merchantApi_1 = __webpack_require__(18);
var reportApi_1 = __webpack_require__(19);
var couponApi_1 = __webpack_require__(20);
var Api = /** @class */ (function () {
    function Api() {
        this.product = new productApi_1.ProductApi();
        this.promotion = new promotionApi_1.PromotionApi();
        this.customerService = new customerServiceApi_1.CustomerServiceApi();
        this.member = new memberApi_1.MemberApi();
        this.merchant = new merchantApi_1.MerchantApi();
        this.delivery = new deliveryApi_1.DeliveryApi();
        this.shoppingCart = new ShoppingCartApi_1.ShoppingCartApi();
        this.cms = new cmsApi_1.CMSApi();
        this.order = new orderApi_1.OrderApi();
        this.payment = new paymentApi_1.PaymentApi();
        this.message = new messageApi_1.MessageApi();
        this.prodcomment = new prodcommentApi_1.ProdCommentApi();
        this.uploadfile = new uploadfileApi_1.UploadFileApi();
        this.report = new reportApi_1.ReportApi();
        this.coupon = new couponApi_1.CouponApi();
    }
    return Api;
}());
exports.Api = Api;
var InstoreSdk = /** @class */ (function () {
    function InstoreSdk() {
        this.ver = "2.0";
        this.api = new Api();
        this.message = this.createMessage();
        console.log("InstoreSdk mall inited");
    }
    ;
    InstoreSdk.prototype.isLogin = function () {
        var logined = getCookie("logined", "/");
        return logined == "1";
    };
    ;
    InstoreSdk.prototype.createMessage = function () {
        var lang = getCookie("uLanguage", "/");
        switch (lang) {
            case "E":
                return new en_1.EnMessage();
            case "C":
                return new zh_HK_1.HKMessage();
            case "S":
                return new zh_CN_1.CNMessage();
            default:
                return new en_1.EnMessage();
        }
    };
    return InstoreSdk;
}());
exports.InstoreSdk = InstoreSdk;


/***/ }),
/* 4 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var memberApi_1 = __webpack_require__(1);
var ProductApi = /** @class */ (function (_super) {
    __extends(ProductApi, _super);
    function ProductApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ProductApi.prototype.getProduct = function (sku, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Product/GetById", { id: sku }, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    ;
    /**
     *  获取目录的产品分页列表
     * @param pager  data sample: {CatId:10,Page:1,PageSize=10}
     * @param callback
     */
    ProductApi.prototype.getCatProduct = function (pager, callback) {
        var _this = this;
        WSPost(this.apiPath + "/product/GetCatProdByPage", pager, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    ;
    /**
     * 搜寻产品
     * @param {object} cond ，例子：  {Key:"name or desc",PageInfo:{Page:1,PageSize:10}}
     * @param {Function} callback
     */
    ProductApi.prototype.search = function (cond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/product/Search", cond, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    ;
    /**
     *
     * @param callback
     */
    ProductApi.prototype.getCatalogs = function (callback) {
        var _this = this;
        var path = this.apiPath + "/product/GetCatalogs";
        WSAjaxSP({
            type: "get",
            data: {},
            url: path,
            success: function (result) {
                _this.log(path, result);
                if (callback) {
                    if (result.Succeeded) {
                        callback(result.ReturnValue);
                    }
                    else {
                        _this.log(result.Message);
                    }
                }
            }
        });
    };
    ;
    ProductApi.prototype.getCatalog = function (id, callback) {
        var _this = this;
        var path = this.apiPath + "/Product/GetCatalog";
        WSAjaxSP({
            type: "get",
            data: { cid: id },
            url: path,
            success: function (data) {
                _this.log(path, data);
                if (callback) {
                    if (data.Succeeded) {
                        callback(data);
                    }
                }
            }
        });
    };
    ;
    ProductApi.prototype.getAttrImage = function (sku, imageType, attr1, attr2, attr3, callback) {
        var _this = this;
        WSGet(this.apiPath + "/product/GetAttrImage", {
            sku: sku,
            attrType: imageType,
            attr1: attr1,
            attr2: attr2,
            attr3: attr3
        }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    /**
     * 获取相关产品
     */
    ProductApi.prototype.getRltProduct = function (sku, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Product/GetRelatedProduct", { id: sku }, function (result, status) {
            _this.log(result);
            if (callback) {
                if (result.Succeeded) {
                    callback(result.ReturnValue);
                }
                else {
                    _this.log(result.Message);
                }
            }
        });
    };
    ;
    ProductApi.prototype.checkSaleOut = function (code, attr1, attr2, attr3, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Product/CheckSaleOut", {
            code: code,
            attr1: attr1,
            attr2: attr2,
            attr3: attr3
        }, function (result, status) {
            _this.log(result);
            if (callback) {
                callback(result);
            }
        });
    };
    ;
    ProductApi.prototype.checkOnSale = function (code, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Product/checkOnSale", {
            code: code
        }, function (result, status) {
            _this.log(result);
            if (callback) {
                callback(result);
            }
        });
    };
    ;
    ProductApi.prototype.addFavorite = function (sku, success, fail) {
        var memberApi = new memberApi_1.MemberApi();
        memberApi.addFavorite(sku, success, fail);
    };
    ;
    ProductApi.prototype.removeFavorite = function (sku, success, fail) {
        var memberApi = new memberApi_1.MemberApi();
        memberApi.removeFavorite(sku, success, fail);
    };
    ;
    ProductApi.prototype.checkProductSaleQuota = function (count, code, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Product/CheckProductSaleQuota", {
            count: count,
            code: code
        }, function (result, status) {
            _this.log(result);
            if (callback) {
                callback(result);
            }
        });
    };
    ;
    return ProductApi;
}(wsapi_1.WSAPI));
exports.ProductApi = ProductApi;


/***/ }),
/* 5 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var PromotionApi = /** @class */ (function (_super) {
    __extends(PromotionApi, _super);
    function PromotionApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    PromotionApi.prototype.getLatestProduct = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Product/GetLatest", {}, function (result, status) {
            _this.log(result);
            if (callback) {
                if (result.Succeeded) {
                    callback(result.ReturnValue);
                }
                else {
                    _this.log(result.Message);
                }
            }
        });
    };
    ;
    PromotionApi.prototype.getHeaderBanner = function (data, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Banner/GetHeaderBanner", {
            from: data.from,
            page: data.page,
        }, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        });
    };
    ;
    PromotionApi.prototype.getPromotion = function (data, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Banner/GetPromotion", {
            from: data.from,
            page: data.page,
            position: data.position,
        }, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        });
    };
    ;
    PromotionApi.prototype.getPromotionList = function (data, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Banner/GetPromotionList", {
            from: data.from,
            page: data.page,
            position: data.position,
        }, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        });
    };
    ;
    PromotionApi.prototype.getHotSearchKeyList = function (qty, callback) {
        var _this = this;
        WSGet(this.apiPath + "/SearchKey/GetHotSearchKeys", {
            qty: qty
        }, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        });
    };
    ;
    //獲取最新的系統公告
    PromotionApi.prototype.getLatestSysAnnouncement = function (qty, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Message/GetLatestSysAnnouncement", { qty: qty }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    PromotionApi.prototype.getHeaderPrmtList = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Promotion/GetHeaderPrmtList", {}, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        });
    };
    ;
    return PromotionApi;
}(wsapi_1.WSAPI));
exports.PromotionApi = PromotionApi;


/***/ }),
/* 6 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var CustomerServiceApi = /** @class */ (function (_super) {
    __extends(CustomerServiceApi, _super);
    function CustomerServiceApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CustomerServiceApi.prototype.getInquiryType = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/CS/GetInquiryType", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    CustomerServiceApi.prototype.inquiry = function (model, callback) {
        var _this = this;
        WSPost(this.apiPath + "/CS/Inquiry", model, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                    if (callback) {
                        callback(data.Message);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showSystemError(data.ReturnValue);
                }
            }
        });
    };
    ;
    return CustomerServiceApi;
}(wsapi_1.WSAPI));
exports.CustomerServiceApi = CustomerServiceApi;


/***/ }),
/* 7 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var DeliveryApi = /** @class */ (function (_super) {
    __extends(DeliveryApi, _super);
    function DeliveryApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    DeliveryApi.prototype.getCountry = function (callback) {
        var _this = this;
        var path = this.apiPath + "/Address/Country";
        WSGet(path, {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                    else {
                        console.log(data.Message);
                    }
                }
            }
            else {
                showInfo(status);
            }
        });
    };
    ;
    DeliveryApi.prototype.getProvince = function (countryId, callback) {
        var _this = this;
        var path = this.apiPath + "/Address/Province";
        WSGet(path, { countryId: countryId }, function (data, status) {
            _this.log(path, data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
                else {
                    console.log(data.Message);
                }
            }
        });
    };
    ;
    DeliveryApi.prototype.getExpress = function (addressId, weight, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Express/GetExpress", { deliveryAddrId: addressId, totalWeight: weight }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        var key = data.Message;
                        callback(data.ReturnValue, key);
                    }
                }
            }
            else {
                showInfo(status);
            }
        });
    };
    ;
    DeliveryApi.prototype.getExpressWithDiscount = function (exCond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Express/GetExpressWithDiscount", exCond, function (data) {
            if (callback) {
                // if (data.Succeeded) {
                //     var key = data.Message;
                //     callback(data.ReturnValue, key);
                // }
                callback(data);
            }
        }, function () { });
    };
    ;
    DeliveryApi.prototype.getIPostStationExpress = function (exCond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Express/GetIPostStationExpress", exCond, function (data) {
            if (callback) {
                // if (data.Succeeded) {
                //     var key = data.Message;
                //     callback(data.ReturnValue, key);
                // }
                callback(data);
            }
        }, function () { });
    };
    ;
    DeliveryApi.prototype.getIPostStationList = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Express/GetIPostStationList", {}, function (data) {
            if (callback) {
                if (data.Succeeded) {
                    //var key = data.Message;
                    // callback(data.ReturnValue);
                    callback(data);
                }
            }
        }, function () { });
    };
    DeliveryApi.prototype.getCollectionList = function (exCond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Express/GetCollectionOfficeList", exCond, function (data) {
            if (callback) {
                // if (data.Succeeded) {
                //     var key = data.Message;
                //     callback(data.ReturnValue, key);
                // }
                callback(data);
            }
        }, function () { });
    };
    DeliveryApi.prototype.getIPostStationByMCNCode = function (code, merchId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Express/GetIPostStationByMCNCode", { mcnCode: code, merchId: merchId }, function (data) {
            if (callback) {
                //if (data.Succeeded) {
                callback(data);
                //}
            }
        }, function () { });
    };
    DeliveryApi.prototype.getDeliveryActiveInfo = function (mercId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Express/GetDeliveryActiveInfo", { merchantId: mercId }, function (data) {
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    DeliveryApi.prototype.checkCountryIsValid = function (info, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Express/CheckCountryIsVaild", info, function (data) {
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    DeliveryApi.prototype.getAddress = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Address/GetAddresses", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            }
        });
    };
    ;
    DeliveryApi.prototype.getDefaultAddr = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Address/GetDefaultAddr", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    DeliveryApi.prototype.getPickupAddress = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Address/GetPickupAddress", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    callback(data);
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    DeliveryApi.prototype.saveAddress = function (address, callback) {
        var _this = this;
        var action = "/Address/Add";
        console.log(address);
        if (address.Id && address.Id != "") {
            action = "/Address/Update";
        }
        WSAjaxSP({
            type: "post",
            url: this.apiPath + action,
            data: address,
            success: function (data, status) {
                _this.log(data);
                if (status === "success") {
                    if (callback && typeof (callback) === "function") {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
            }
        });
    };
    ;
    DeliveryApi.prototype.removeAddress = function (id, callback) {
        var _this = this;
        WSGet(this.apiPath + "/address/Remove", { id: id }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    callback(data);
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    DeliveryApi.prototype.gtPaymentType = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/GetPaymentTypes", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    callback(data);
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    return DeliveryApi;
}(wsapi_1.WSAPI));
exports.DeliveryApi = DeliveryApi;


/***/ }),
/* 8 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var ShoppingCartApi = /** @class */ (function (_super) {
    __extends(ShoppingCartApi, _super);
    function ShoppingCartApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ShoppingCartApi.prototype.loadData = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/ShoppingCart/GetShopCart", {}, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    ShoppingCartApi.prototype.loadCheckoutData = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/ShoppingCart/GetMallCart", {}, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
                else {
                    _this.log(data.Message);
                }
            }
        });
    };
    ;
    ShoppingCartApi.prototype.addItem = function (prodId, attr1, attr2, attr3, qty, callback, loginFun) {
        var _this = this;
        WSAjaxSP({
            type: "Post",
            url: this.apiPath + "/ShoppingCart/AddItem",
            data: { ProductId: prodId, Attr1: attr1, Attr2: attr2, Attr3: attr3, qty: qty },
            success: function (data, status) {
                _this.log(data);
                if (status === "success") {
                    if (callback) {
                        if (!data.Succeeded) {
                            if (data.ReturnValue == "3002") {
                                if (loginFun && typeof (loginFun) === "function") {
                                    loginFun();
                                }
                                else {
                                    window.location.href = "/account/login?returnUrl=" + window.location.href;
                                }
                                return;
                            }
                        }
                        callback(data);
                    }
                }
            }
        });
    };
    ;
    ShoppingCartApi.prototype.removeItem = function (id, callback) {
        var _this = this;
        WSAjaxSP({
            type: "get",
            url: this.apiPath + "/ShoppingCart/RemoveItem",
            data: { id: id },
            success: function (data, status) {
                _this.log(data);
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    ShoppingCartApi.prototype.updateItemQty = function (itemId, qty, callback) {
        var _this = this;
        WSGet(this.apiPath + "/ShoppingCart/UpdateItemQty", { id: itemId, qty: qty }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                callback(data);
            }
        });
    };
    ;
    ShoppingCartApi.prototype.saveDelivery = function (addressId, expressId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Pay/SaveDelivery", { deliveryAddressId: addressId, expressCompanyId: expressId }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                callback(data);
            }
        });
    };
    ;
    ShoppingCartApi.prototype.savePickupInfo = function (addressId, pickupDate, pickupTime, pickupAddress, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Pay/SavePickupInfo", { addressId: addressId, date: pickupDate, time: pickupTime, address: pickupAddress }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                callback(data);
            }
        });
    };
    ;
    ShoppingCartApi.prototype.savePayMethod = function (methodId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/SavePayMethod", { payMethodType: methodId }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    ShoppingCartApi.prototype.getFreeProduct = function (ruleId, skuId, qty, callback) {
        var _this = this;
        WSGet(this.apiPath + "/ShoppingCart/GetFreeProduct", { ruleId: ruleId, skuId: skuId, qty: qty }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    ShoppingCartApi.prototype.getSetPrice = function (ruleId, skuId, qty, callback) {
        var _this = this;
        WSGet(this.apiPath + "/ShoppingCart/GetSetPrice", { ruleId: ruleId, skuId: skuId, qty: qty }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    return ShoppingCartApi;
}(wsapi_1.WSAPI));
exports.ShoppingCartApi = ShoppingCartApi;


/***/ }),
/* 9 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var CMSApi = /** @class */ (function (_super) {
    __extends(CMSApi, _super);
    function CMSApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CMSApi.prototype.getContent = function (cId, success, error) {
        WSGet(this.apiPath + "/cms/GetContent", { cid: cId }, function (data) {
            if (success) {
                success(data);
            }
        }, function () { });
    };
    CMSApi.prototype.getContentsByCatId = function (catId, success, error) {
        WSGet(this.apiPath + "/cms/GetContentsByCatId", { catId: catId }, function (data) {
            if (success) {
                success(data);
            }
        }, function () { });
    };
    CMSApi.prototype.getCategoryTree = function (success, error) {
        WSGet(this.apiPath + "/cms/GetCategoryTree", {}, function (data) {
            if (success) {
                success(data);
            }
        }, function () { });
    };
    return CMSApi;
}(wsapi_1.WSAPI));
exports.CMSApi = CMSApi;


/***/ }),
/* 10 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var OrderApi = /** @class */ (function (_super) {
    __extends(OrderApi, _super);
    function OrderApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    OrderApi.prototype.getPaymentType = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/GetPaymentMethod", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                callback(data);
            }
        });
    };
    ;
    OrderApi.prototype.getPageOrder = function (pageSize, page, status, orderBy, callback) {
        var _this = this;
        if (status === void 0) { status = -1; }
        if (orderBy === void 0) { orderBy = "CreateDate"; }
        WSPost(this.apiPath + "/Order/SearchOrder", { Page: page, PageSize: pageSize, Status: status, OrderBy: orderBy }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                // if (callback && typeof (callback) === "function") {
                //     if (data.Succeeded) {
                //         callback(data.ReturnValue);
                //     }
                // } else {
                //     showInfo(data.Message);
                // }
                callback(data);
            }
        });
    };
    ;
    OrderApi.prototype.getPageOldOrder = function (pageSize, page, status, orderBy, callback) {
        var _this = this;
        if (status === void 0) { status = -1; }
        if (orderBy === void 0) { orderBy = "CreateDate"; }
        WSPost(this.apiPath + "/Order/SearchOldOrder", { Page: page, PageSize: pageSize, Status: status, OrderBy: orderBy }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                // if (callback && typeof (callback) === "function") {
                //     if (data.Succeeded) {
                //         callback(data.ReturnValue);
                //     }
                // } else {
                //     showInfo(data.Message);
                // }
                callback(data);
            }
        });
    };
    ;
    OrderApi.prototype.getOrder = function (id, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Order/GetOrder", { id: id }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    OrderApi.prototype.getOldOrder = function (id, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Order/GetOldOrder", { id: id }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    OrderApi.prototype.getOrderStatusList = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Order/GetOrderStatus", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    OrderApi.prototype.cancel = function (id, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Order/Cancel", { id: id }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    callback(data);
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    OrderApi.prototype.createOrder = function (order, callback) {
        WSPost(this.apiPath + "/order/Create", order, function (data, status) {
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    callback(data);
                }
                else {
                    showInfo(data.Message);
                }
            }
        }, function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
            console.log(textStatus);
            console.log(errorThrown);
        });
    };
    ;
    OrderApi.prototype.getReturnOrderTypeList = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/order/GetReturnType", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    OrderApi.prototype.createReturnOrder = function (returnorder, callback) {
        WSPost(this.apiPath + "/order/CreateReturnOrder", returnorder, function (data, status) {
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    callback(data);
                }
                else {
                    showInfo(data.Message);
                }
            }
        }, function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
            console.log(textStatus);
            console.log(errorThrown);
        });
    };
    ;
    OrderApi.prototype.getReturnOrders = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/order/GetReturnOrders", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    OrderApi.prototype.getOrderTrackingInfo = function (trackingNo, callback) {
        var _this = this;
        WSGet(this.apiPath + "/order/GetOrderMailTrackingInfo", { trackingNo: trackingNo }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    OrderApi.prototype.getReturnOrder = function (id, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Order/getReturnOrder", { id: id }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    return OrderApi;
}(wsapi_1.WSAPI));
exports.OrderApi = OrderApi;


/***/ }),
/* 11 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var PaymentApi = /** @class */ (function (_super) {
    __extends(PaymentApi, _super);
    function PaymentApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    PaymentApi.prototype.getPaymentMethod = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/GetPaymentMethod", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        });
    };
    ///获取PayPal支付方式的对象
    PaymentApi.prototype.getMPGSInfo = function (orderId, payType, callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/GetMPGSInfo", { orderId: orderId, payType: payType }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                    else {
                        _this.log(data.Message);
                    }
                }
            }
        });
    };
    ///获取PayPal支付方式的对象
    PaymentApi.prototype.getPayPalInfo = function (orderId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/GetPayPalInfo", { orderId: orderId }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ///获取PayPal支付方式的对象
    PaymentApi.prototype.getStripeInfo = function (orderId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/GetSripeInfo", { orderId: orderId }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    //支付完成后更新order的支付狀態
    PaymentApi.prototype.updateOrderPaymentStatus = function (orderId, type, paykey, callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/UpdateOrderPaymentStatus", { orderId: orderId, type: type, paymentKey: paykey }, function (data) {
            _this.log(data);
            //if (status === "success") {
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
                else {
                    _this.log(data.Message);
                }
            }
            //}
        });
    };
    return PaymentApi;
}(wsapi_1.WSAPI));
exports.PaymentApi = PaymentApi;


/***/ }),
/* 12 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var wsapi_1 = __webpack_require__(0);
var MessageApi = /** @class */ (function (_super) {
    __extends(MessageApi, _super);
    function MessageApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    //獲取未讀消息數量
    MessageApi.prototype.getUnreadMsgQty = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Message/GetMbrUnreadMsgQty", null, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    //獲取未讀消息列表
    MessageApi.prototype.getUnreadMsgList = function (pager, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Message/GetMemberUnreadMsg", pager, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //獲取全部消息列表
    MessageApi.prototype.getAllMsgList = function (pager, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Message/GetAllMemberMsg", pager, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //回復消息（需傳遞被回復記錄的ID）
    MessageApi.prototype.replyMsg = function (message, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Message/MbrReplyMessage", message, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //發送會員的售前咨詢信息
    MessageApi.prototype.sendPreSalesMsg = function (message, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Message/SendMemberPreSalesMsg", message, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //發送會員的售後咨詢信息
    MessageApi.prototype.sendAfterSalesMsg = function (message, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Message/SendMemberAfterSalesMsg", message, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //標記指定的消息為已讀狀態
    MessageApi.prototype.markedMessageAsRead = function (msgIdLst, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Message/MarkedMessageAsRead", { msgIdLst: msgIdLst }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //標記所有消息為已讀狀態
    MessageApi.prototype.markedMbrAllMsgAsRead = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Message/MarkedMbrAllMsgAsRead", null, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //刪除指定的信息
    MessageApi.prototype.deleteMsg = function (msgIdLst, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Message/DeleteMsg", { msgIdLst: msgIdLst }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //刪除所有信息
    MessageApi.prototype.deleteAllMsg = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Message/DeleteAllMsg", null, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //新增到貨通知
    MessageApi.prototype.addArrivalNotify = function (cond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Message/AddArrivalNotify", cond, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //取消到貨通知
    MessageApi.prototype.cancelArrivalNotify = function (cond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Message/CancelArrivalNotify", cond, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //是否已有到貨通知
    MessageApi.prototype.checkExsitArrivalNotify = function (cond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Message/CheckExsitArrivalNotify", cond, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    return MessageApi;
}(wsapi_1.WSAPI));
exports.MessageApi = MessageApi;


/***/ }),
/* 13 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var ProdCommentApi = /** @class */ (function (_super) {
    __extends(ProdCommentApi, _super);
    function ProdCommentApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ///获取买家的所有评论
    ProdCommentApi.prototype.getShopperComments = function (cond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/ProductComment/GetShopperComments", cond, function (data) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ///获取产品的所有评论
    ProdCommentApi.prototype.getProductComments = function (cond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/ProductComment/GetProductComments", cond, function (data) {
            if (callback) {
                if (data && data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    ///获取评论
    ProdCommentApi.prototype.getCommentById = function (commentId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/ProductComment/GetCommentById", { commentId: commentId }, function (data) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ///根據訂單獲取評論
    ProdCommentApi.prototype.getSubOrderComments = function (Id, callback) {
        var _this = this;
        WSGet(this.apiPath + "/ProductComment/GetSubOrderComments", { subOrderId: Id }, function (data) {
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    ///保存评论
    ProdCommentApi.prototype.saveComments = function (comments, callback) {
        var _this = this;
        console.log(comments);
        WSAjaxSP({
            url: this.apiPath + "/ProductComment/SaveComments",
            type: 'post',
            dataType: 'json',
            cache: false,
            processData: false,
            contentType: 'application/json',
            data: JSON.stringify(comments),
            success: function (data) {
                if (callback) {
                    callback(data);
                }
            },
            error: function () {
            }
        });
        //WSPost(this.apiPath + "/ProductComment/SaveComments", comments, function (data: any) {
        //    if (callback) {
        //        callback(data);
        //    }
        //}, function () { });
    };
    ///删除评论
    ProdCommentApi.prototype.deleteComment = function (commentId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/ProductComment/DeleteComment", { commentId: commentId }, function (data) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ///檢查當前用戶是否有該產品的評論權限
    ProdCommentApi.prototype.checkAuthorityForProduct = function (sku, callback) {
        WSGet(this.apiPath + "/ProductComment/CheckAuthorityOfProduct", { sku: sku }, function (data) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    return ProdCommentApi;
}(wsapi_1.WSAPI));
exports.ProdCommentApi = ProdCommentApi;


/***/ }),
/* 14 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var UploadFileApi = /** @class */ (function (_super) {
    __extends(UploadFileApi, _super);
    function UploadFileApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    UploadFileApi.prototype.uploadFiles = function (files, callback) {
        var _this = this;
        WSAjaxSP({
            url: this.apiPath + "/FileUpload/UploadFile",
            type: 'post',
            dataType: 'json',
            cache: false,
            data: files,
            processData: false,
            contentType: false,
            success: function (data) {
                if (callback) {
                    callback(data);
                }
            },
            error: function () {
            }
        });
        //console.log(files);
        //alert(123);
    };
    return UploadFileApi;
}(wsapi_1.WSAPI));
exports.UploadFileApi = UploadFileApi;


/***/ }),
/* 15 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var EnMessage = /** @class */ (function () {
    function EnMessage() {
        this.FileNotMoreThan = "File not more than ";
        this.FileSizeNotMoreThan = "File size not more than ";
        this.OnlyUploadImage = "Only upload Image";
        this.CommentSucceeded = "Comment succeeded";
        this.HaveComment = "You have a comment";
        this.ChangeAddress = "Change Address";
        this.AddAddress = "Add Address";
    }
    return EnMessage;
}());
exports.EnMessage = EnMessage;
;


/***/ }),
/* 16 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var HKMessage = /** @class */ (function () {
    function HKMessage() {
        this.FileNotMoreThan = "文件數量不能大於 ";
        this.FileSizeNotMoreThan = "文件大小不得大於 ";
        this.OnlyUploadImage = "只能上傳圖片";
        this.CommentSucceeded = "評論成功";
        this.HaveComment = "你已經評論過";
        this.ChangeAddress = "更改地址";
        this.AddAddress = "加入地址";
    }
    return HKMessage;
}());
exports.HKMessage = HKMessage;
;


/***/ }),
/* 17 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var CNMessage = /** @class */ (function () {
    function CNMessage() {
        this.FileNotMoreThan = "文件数量不能超过 ";
        this.FileSizeNotMoreThan = "文件大小不得大于 ";
        this.OnlyUploadImage = "只能上传图片";
        this.CommentSucceeded = "评论成功";
        this.HaveComment = "你已经评论过";
        this.ChangeAddress = "更改地址";
        this.AddAddress = "加入地址";
    }
    return CNMessage;
}());
exports.CNMessage = CNMessage;
;


/***/ }),
/* 18 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var MerchantApi = /** @class */ (function (_super) {
    __extends(MerchantApi, _super);
    function MerchantApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    MerchantApi.prototype.getMerchantInfo = function (merchantId, callback) {
        var path = this.apiPath + "/Merchant/GetMerchantInfo";
        WSGet(path, { merchID: merchantId }, function (result, status) {
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };
    ;
    MerchantApi.prototype.getMerchantList = function (callback) {
        var path = this.apiPath + "/Merchant/GetList";
        WSGet(path, "", function (result, status) {
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };
    ;
    MerchantApi.prototype.getProducts = function (cond, callback) {
        var path = this.apiPath + "/Merchant/GetProducts";
        WSPost(path, cond, function (result, status) {
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };
    ;
    return MerchantApi;
}(wsapi_1.WSAPI));
exports.MerchantApi = MerchantApi;


/***/ }),
/* 19 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var ReportApi = /** @class */ (function (_super) {
    __extends(ReportApi, _super);
    function ReportApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ReportApi.prototype.GetHotSale = function (pageSize, page, callback) {
        var _this = this;
        WSPost(this.apiPath + "/report/GetHotSale", { Page: page, PageSize: pageSize }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    ReportApi.prototype.GetHighScore = function (pageSize, page, callback) {
        var _this = this;
        WSPost(this.apiPath + "/report/GetHighScore", { Page: page, PageSize: pageSize }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    ReportApi.prototype.GetHotStore = function (pageSize, page, callback) {
        var _this = this;
        WSPost(this.apiPath + "/report/GetHotStore", { Page: page, PageSize: pageSize }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    return ReportApi;
}(wsapi_1.WSAPI));
exports.ReportApi = ReportApi;


/***/ }),
/* 20 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="./common.d.ts" />
var wsapi_1 = __webpack_require__(0);
var CouponApi = /** @class */ (function (_super) {
    __extends(CouponApi, _super);
    function CouponApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CouponApi.prototype.getCoupons = function (cond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Coupon/GetCouponGroup", cond, function (data) {
            if (callback) {
                callback(data);
                //if (data.Succeeded) {
                //    callback(data);
                //}
            }
        }, function () { });
    };
    ;
    CouponApi.prototype.getRules = function (callback) {
        var _this = this;
        var path = this.apiPath + "/Coupon/CheckHasGroupOrRuleDiscount";
        WSGet(path, {}, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
                //if (data.Succeeded) {
                //    callback(data);
                //} else {
                //    console.log(data.Message);
                //}
            }
        });
    };
    ;
    CouponApi.prototype.getPromotionCode = function (merchantId, code, price, callback) {
        var _this = this;
        var path = this.apiPath + "/Coupon/GetPromotionCodeCoupon";
        WSGet(path, { merchantId: merchantId, code: code, price: price }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    CouponApi.prototype.getMallFun = function (callback) {
        var _this = this;
        var path = this.apiPath + "/Coupon/GetMallFun";
        WSGet(path, {}, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    CouponApi.prototype.getMemberCoupon = function (cond, callback) {
        var path = this.apiPath + "/Coupon/GetMemberCoupon";
        WSPost(path, cond, function (data, status) {
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    return CouponApi;
}(wsapi_1.WSAPI));
exports.CouponApi = CouponApi;


/***/ })
/******/ ]);