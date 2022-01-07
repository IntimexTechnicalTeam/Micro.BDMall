var tempStr = "";
tempStr += "<ul class ='list-group' style='margin:0px;'>";
tempStr += "       <template v-for='c in items'>";
tempStr += "          <li class ='list-group-item' >";
tempStr += "              <span v-if='c.Children&&c.Children.length>0&&c.Collapse' style='cursor:pointer;' class ='glyphicon glyphicon-collapse-down pull-left' v-on:click='toggleNode(c)'></span>";
tempStr += "              <span v-if='c.Children&&c.Children.length>0&&!c.Collapse' style='cursor:pointer;' class ='glyphicon glyphicon-collapse-up pull-left' v-on:click='toggleNode(c)'></span>";
tempStr += "              <div style='height:20px;'>";
tempStr += "                  <div class ='pull-left' style='width:80%;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;'>";
tempStr += "                    <img v-if='c.SmallIcon' v-bind:src='c.SmallIcon' v-on:click='dialog($event)' width='20' /> {{c.Desc}}";

tempStr += "                  </div>";
tempStr += "                    <div v-if='canedit==1' style='float:right;'>";
tempStr += "                      <div v-if='issorted==true' class='glyphicon'><input type='text' v-model='c.Seq'  style='width:40px;' v-on:change='changeFlag(c)' maxLength=4/></div>";
tempStr += "                            <div v-bind:class ='BtnEdit' style='cursor:pointer;' v-on:click='editClick(c,$event)'></div>";
tempStr += "                            <div v-if='c.Children.length==0'v-bind:class ='BtnDelete' style='cursor:pointer;'  v-on:click='removeClick(c,$event)'></div>";
tempStr += "                            <div v-if='c.Level<limitlevel' style='cursor:pointer;' v-bind:class ='BtnAdd' v-on:click='addClick(c,$event)'></div>";
tempStr += "                            <div v-if='c.IsActive==false' style='cursor:pointer;' v-bind:class ='BtnActive' v-on:click='activeClick(c)'></div>";
tempStr += "                            <div v-else style='cursor:pointer;' v-bind:class ='BtnDisActive' v-on:click='disActiveClick(c)'></div>";
tempStr += "                        </div>";
tempStr += "                        <div v-if='canedit==2&&c.Children.length==0' style='float:right;' >";
tempStr += "                            <a v-on:click='selectClick(c,$event)' style='cursor:pointer;'>select</a>";
tempStr += "                        </div>";
tempStr += "                    </div>";
tempStr += "              </div>";
tempStr += "         </li>";
tempStr += "         <template v-if='c.Children&&c.Children.length>0'>";
tempStr += "            <li class='list-group-item' style='padding-top:0px;' :class='{hidden:c.Collapse}'>";
tempStr += "              <menu-item v-bind:items='c.Children' v-bind:limitlevel='limitlevel' v-bind:canedit='canedit'  v-bind:clickadd='clickadd' v-bind:clickedit='clickedit' v-bind:clickremove='clickremove' v-bind:issorted='issorted'  v-bind:activeclick='activeclick'  v-bind:disactiveclick='disactiveclick'></menu-item>";
tempStr += "            </li>";
tempStr += "         </template>";
tempStr += "      </template>";
tempStr += "</ul>";


Vue.component('menu-item', {
    name: "menu-item",
    template: tempStr,
    props: ['items', 'canedit', 'clickadd', 'clickedit', 'clickremove', 'limitlevel', 'issorted', 'activeclick', 'disactiveclick'],//注意不能有大寫
    data: function () {
        return {
            //  nodes: this.items
            Edit: "編輯",
            BtnEdit: "glyphicon glyphicon-edit",
            BtnDelete: "glyphicon glyphicon-trash",
            BtnAdd: "glyphicon glyphicon-plus",
            BtnActive: "glyphicon glyphicon-ok",
            BtnDisActive: "glyphicon glyphicon-remove"
        };
    }
    , methods: {
        toggleNode: function (obj) {
            obj.Collapse = !obj.Collapse;
        },
        editClick: function (obj, e) {
            //console.log("component click edit", obj);
            //this.$emit('click-edit');
            //eval(this.clickedit + "()"); //綁定字符串的調用方法
            this.clickedit(obj);
        },
        removeClick: function (obj, e) {
            //console.log("component click remove", obj);
            this.clickremove(obj);
        },
        addClick: function (obj) {
            //console.log("component click add", obj);
            //if (this.clickadd) {
            this.clickadd(obj);
            //}
        },
        selectClick: function (obj, e) {
            bDialog.closeCurrent(obj);
        },
        activeClick: function (obj) {
            this.activeclick(obj);
        },
        disActiveClick: function (obj) {
            this.disactiveclick(obj);
        },
        changeFlag: function (obj) {
            obj.IsChange = true;
            //alert(obj.IsChange);
        },
        dialog: function (e) {
            //var _this = $(this);//将当前的pimg元素作为_this传入函数  
            var el = e.target;
            imgShow("#outerdiv", "#innerdiv", "#bigimg", $(el));
        }


    }
})

layui.use(['miniTab'], function () {
    var layer = layui.layer,
        miniTab = layui.miniTab;
});

function showEdit() {
    console.log("call showEdit");
}
//var uri = "http://jm.intimex.hk/adminapi/SystemMenu";
var uri = "/adminapi/ProdCatalog";
var app = new Vue({
    el: "#MainContent",
    data: {
        //node: { Id: 0, ParentId: 0, Path: "", Code: "", Text: "", Url: "", Level: 0, Img: "", ImgPath: "", MutiLanguage: [{ Lang: { Text: "", Code: "" }, Desc: "" }] },
        treeNodes: [
            {
                Id: "1", ParentId: "", Desc: "", Icon: "", IconPath: "", Collapse: true, Level: 1, Seq: 0
                , Children: [{ Id: "", Desc: "", Collapse: true, Level: 2, Seq: 0, Children: [] }
                    , { Id: "12", Desc: "settting12", Collapse: true, Level: 2, Children: [] }]
            }
        ],
        isSorted: false,
        isNeedActive: false

    },
    methods: {
        loadMenus: function (type) {
            var url = '';
            if (type == '1') {
                url = uri + "/GetCatalogTree";
                app.isNeedActive = false;
            } else {
                url = uri + "/GetActiveCatalogTree";
                app.isNeedActive = true;
            }
            WS.AjaxP("get", url, "", function (response) {
                if (response.Succeeded == true) {
                    app.treeNodes = response.ReturnValue;
                }
                else {
                    showInfo(response.Message);
                }

            },
                function () {
                });

        },
        editItem: function (obj) {

            var tabId = self.frameElement.parentElement.id;
            miniTab.openNewTabByIframe({
                href: "/Product/EditCatalog/" + obj.Id,
                title: 'Modify-' + obj.Text,
                callback: app.loadMenus
            });
            //OpenDialog("Modify Catalog", 800, 800, "/Catalog/CatalogEdit", params, function () {
            //    app.loadMenus();
            //});
        },
        removeItem: function (obj) {
            SystemConfirm(Resources.ConfirmDelete, function () {
                var data = new Object();
                data.id = obj.Id;
                WS.AjaxP("get", uri + "/Delete", data, function (response) {
                    if (response.Succeeded == true) {
                        app.loadMenus(1);
                    }
                    else {
                        showInfo(response.Message);
                    }
                },
                    function () { })
            });

        },
        addItem: function (obj) {

            var tabId = self.frameElement.parentElement.id;
            var parentInfo = obj.Id + "|" + obj.Level;

            console.log(parentInfo);

            miniTab.openNewTabByIframe({
                href: "/Product/EditCatalog/" + WS.GuidEmpty + "/" + parentInfo,
                title: 'Add Catalog',
                callback: app.loadMenus
            });

        },
        activeItem: function (obj) {
            var data = new Object();
            data.catId = obj.Id;
            WS.AjaxP("get", uri + "/ActiveCatalog", data, function (response) {
                if (response.Succeeded == true) {
                    app.loadMenus(1);
                }
                else {
                    showInfo(response.Message);
                }
            }, function () { });
        },
        disActiveItem: function (obj) {
            SystemConfirm(Resources.ConfirmDisActiveCatalog, function () {
                var data = new Object();
                data.catId = obj.Id;
                WS.AjaxP("get", uri + "/DisActiveCatalog", data, function (response) {
                    if (response.Succeeded == true) {
                        app.loadMenus(1);
                    }
                    else {
                        showInfo(response.Message);
                    }
                }, function () { });
            });

        },
        sorted: function () {
            app.isSorted = true;
        },
        saveSorted: function () {
            var data = new Object();
            data.tree = app.treeNodes;

            console.log(data.tree);
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
        cancelSorted: function () {
            app.isSorted = false;
        },
        addRoot: function () {
            var parentInfo = ""
            var tabId = self.frameElement.parentElement.id;
            miniTab.openNewTabByIframe({
                href: "/Product/EditCatalog/" + WS.GuidEmpty,
                title: 'Add Catalog',
                callback: app.loadMenus
            });
        }

    },
    mounted: function () {
        //this.loadMenus();
        //this.editItem(null);
    }
});



function loadMenus() {
    app.loadMenus(1)
}