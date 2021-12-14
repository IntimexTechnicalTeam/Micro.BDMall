var tempStr = "";
tempStr += "<ul class='list-group' style='margin:0px;'>";
tempStr += "<template v-for= 'c in items' >";
tempStr += "<li class='list-group-item' >";
tempStr += "<span v-if='c.Children&&c.Children.length>0&&c.Collapse' class='glyphicon glyphicon-collapse-down pull-left' v-on:click='toggleNode(c)'>";
tempStr += "</span>";
tempStr += "<span v-if='c.Children&&c.Children.length>0&&!c.Collapse' class='glyphicon glyphicon-collapse-up pull-left' v-on:click='toggleNode(c)'>";
tempStr += "</span>";
tempStr += "<div>";
tempStr += "<div class='pull-left' style='width:500px'>";
tempStr += "<img v-show='c.Img' v-bind:src='c.Img' width='25'/> {{ c.Text }}";
tempStr += "</div>";
tempStr += "<div v-show='canedit==1'>";
tempStr += "<div v-show='issorted==true' class='glyphicon'><input type='text' v-model='c.Seq'  style='width:40px;' v-on:change='changeFlag(c)' maxLength=4/></div>";
tempStr += "<div v-bind:class='BtnEdit' v-on:click='editClick(c,$event)'></div>";
tempStr += "<div v-bind:class='BtnDelete' v-on:click='removeClick(c,$event)'></div>";
tempStr += "<div v-show='c.Level<limitlevel' v-bind:class='BtnAdd' v-on:click='addClick(c,$event)'></div>";
tempStr += "</div>";
tempStr += "</div>";
tempStr += "</li>";
tempStr += "<template v-if='c.Children&&c.Children.length>0'>";
tempStr += "<li class='list-group-item' style='padding-top:0px;' :class='{hidden: c.Collapse}'>";
tempStr += "<menu-item v-bind:items='c.Children' v-bind:limitlevel='limitlevel' v-bind:canedit='canedit'  v-bind:clickadd='clickadd' v-bind:clickedit='clickedit' v-bind:clickremove='clickremove' v-bind:issorted='issorted'  ></menu-item>";
tempStr += "</li >";
tempStr += "</template >";
tempStr += "</template >";
tempStr += "</ul>";

Vue.component('menu-item', {
    name: "menu-item",
    template: tempStr,
    props: ['items', 'canedit', 'clickadd', 'clickedit', 'clickremove', 'limitlevel', 'issorted'],//注意不能有大寫
    data: function () {
        return {
            //  nodes: this.items

            Edit: "編輯",
            BtnEdit: "glyphicon glyphicon-edit",
            BtnDelete: "glyphicon glyphicon-trash",
            BtnAdd: "glyphicon glyphicon-plus"
        };
    }
    , methods: {
        toggleNode: function (obj) {
            obj.Collapse = !obj.Collapse;
        },
        editClick: function (obj, e) {
            console.log("component click edit", obj);
            //this.$emit('click-edit');
            //eval(this.clickedit + "()"); //綁定字符串的調用方法
            this.clickedit(obj.Id);
        },
        removeClick: function (obj, e) {
            console.log("component click remove", obj);
            this.clickremove(obj.Id);
        },
        addClick: function (obj) {
            console.log("component click add", obj);
            if (this.clickadd) {
                this.clickadd(obj);
            }
        },
        changeFlag: function (obj) {
            obj.IsChange = true;
            //alert(obj.IsChange);
        }
    }
})

function showEdit() {
    console.log("call showEdit");
}
//var uri = "http://jm.intimex.hk/adminapi/SystemMenu";
var uri = "/adminapi/SystemMenu";
var app = new Vue({
    el: "#MainContent",
    data: {
        model: {
            Id: "", Code: "", NameTranslation: [
                { Lang: { Text: "English", Code: "e" }, Desc: "" },
                { Lang: { Text: "繁體中文", Code: "c" }, Desc: "" }]
            , PermissionId: 0,
            Module: {},
            Function: {}
        },
        itemList: [
            { Id: "", Code: "", Names: [{ Lang: { Name: "English", Code: "e" }, Name: "menu1" }, { Lang: { Name: "繁體中文", Code: "c" }, Name: "菜單1" }] },
            { Id: "", Code: "", Names: [{ Lang: { Name: "English", Code: "e" }, Name: "menu2" }, { Lang: { Name: "繁體中文", Code: "c" }, Name: "菜單2" }] }
        ],
        treeNodes: [
            { Id: "1", Text: "system-settting1", Img: "", Collapse: true, Level: 1, Children: [{ Id: "11", Text: "settting11", Collapse: true, Level: 2, Children: [] }, { Id: "12", Text: "settting12", Collapse: true, Level: 2, Children: [] }] },
            { Id: "2", Text: "system-settting2", Img: "", Collapse: true, Level: 1, Children: [{ Id: "21", Text: "settting21", Collapse: true, Level: 2, Children: [] }, { Id: "22", Text: "settting22", Collapse: true, Level: 2, Children: [] }] }],
        isSorted: false,
        functions: [],
        modules: []
    },
    methods: {
        loadMenus: function () {
            WS.AjaxP("get", uri + "/GetList", null, function (response) {
                app.itemList = response;
            },
                function () {

                });

            WS.AjaxP("get", uri + "/GetMenuTree", {}, function (response) {
                app.treeNodes = response;
            }, function () { })
            
        },
        loadFunc: function (event) {
            if (event) {
                console.log(this.model.ModuleId);
                WS.AjaxP("get", "/adminApi/Permission/GetPermissionFunction", { moduleId: this.model.ModuleId }, function (response) {
                    app.functions = response;
                }, function () { })
            }
        },
        editItem: function (id) {
            //console.log("app call editItem function");

            var data = new Object();
            data.id = id;
            WS.AjaxP("get", uri + "/GetMenuInfo", data, function (response) {
                app.model = response; 
                WS.AjaxP("get", "/adminApi/Permission/GetPermissionFunction", { moduleId: app.model.ModuleId }, function (result) {
                    app.functions = result;
                    Vue.nextTick(function () {
                        app.FunctionId = app.model.FunctionId;
                    });
                }, function () { })
            }, function () { })
        },
        removeItem: function (id) {
            var _this = this;
            if (!confirm("Confirm Delete?")) {
                return;
            }
            var data = new Object();
            data.id = id;
            WS.AjaxP("get", uri + "/Delete", data, function (response) {
                if (response.Succeeded == true) {
                    _this.loadMenus();
                }
                else {
                    showWarn(response.Message);
                }
            }, function () { })
        },
        addItem: function (obj) {
            //console.log("app call addItem function");
            $.ajax({
                type: "get",
                url: uri + "/GetMenuInfo",
                data: { id: 0 },
                dataType: "json",
                success: function (response) {
                    console.log(response);
                    response.ParentId = obj.Id;
                    app.model = response;
                }
            });
        },
        createNew: function () {
            console.log("app call createNew function");
            $.ajax({
                type: "get",
                url: uri + "/GetMenuInfo",
                data: { id: 0 },
                dataType: "json",
                success: function (response) {
                    console.log(response);
                    app.model = response;
                }
            });
        },
        sorted: function () {
            app.isSorted = true;
        },
        cancelSorted: function () {
            app.isSorted = false;
        },
        saveSorted: function () {
            WS.Ajax({
                type: "post",
                url: uri + "/UpdateSeq",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(app.treeNodes),
                success: function (response) {
                    if (response.Succeeded == true) {
                        app.loadMenus();
                        app.isSorted = false;
                    }
                    else {
                        showInfo(response.Message);
                    }
                },
                error: function () { }
            });
        },
        saveModel: function () {
            var _this = this;
            if (!this.model.ModuleId) {
                showWarn("请选择模组权限");
                return;
            }

            WS.AjaxP("post", uri + "/Save", this.model, function (response) {
                if (response.Succeeded == true) {
                    _this.loadMenus();
                }
                else {
                    showWarn(response.Message);
                }
            }, function () { })

        },
        checkCodeUniqe: function () {
            if (this.model.Code != "" && this.model.Code != null) {
                WS.AjaxP("get", uri + "/CheckMenuCodeIsExists", { code: this.model.Code }, function (response) {
                    if (response.Succeeded == true) {
                        if (response.ReturnValue == true) {
                            if (response.Message != "") {
                                showWarn(response.Message)
                            }
                            Vue.set(app.model, 'Code', "");
                        }
                    }
                    else {
                        showWarn(response.Message);
                    }
                }, function () { })
            }
        }

    },
    mounted: function () {
        this.loadMenus();
        Vue.nextTick(function () {
            WS.AjaxP("get", "/adminApi/Permission/GetPermissionModule", {}, function (response) {
                app.modules = response;
            }, function () { })
        });

    }
});





