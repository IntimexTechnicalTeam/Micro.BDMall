var tempStr = "";
tempStr += "<div>";
tempStr += "    <ul class ='list-group' style='margin:0px;'>";
tempStr += "       <template v-for='c in items'>";
tempStr += "          <li class ='list-group-item' >";
tempStr += "              <span v-if='c.Children&&c.Children.length>0&&c.Collapse' style='cursor:pointer;' class ='glyphicon glyphicon-collapse-down pull-left' v-on:click='toggleNode(c)'></span>";
tempStr += "              <span v-if='c.Children&&c.Children.length>0&&!c.Collapse' style='cursor:pointer;' class ='glyphicon glyphicon-collapse-up pull-left' v-on:click='toggleNode(c)'></span>";
tempStr += "              <div style='height:15px;'>";
tempStr += "                  <div class ='pull-left'>";
tempStr += "                          <img v-if='c.ImgPath' v-bind:src='c.ImgPath' width='20' /> {{c.Text}}";
tempStr += "                  </div>";
tempStr += "                  <div v-if='canedit==1' style='float:right;'>";
tempStr += "                      <div v-if='issorted==true' class='glyphicon'><input type='text' v-model='c.Seq'  style='width:40px;' v-on:change='changeFlag(c)' maxLength=4/></div>";
tempStr += "                      <div v-bind:class ='BtnEdit'  data-toggle='tooltip' data-placement='left' title='Edit Category'  style='cursor:pointer;' v-on:click='editClick(c,$event)'></div>";
tempStr += "                      <div v-bind:class ='BtnContent' data-toggle='tooltip' data-placement='left' title='Add Content' style='cursor:pointer;' v-on:click='addContentClick(c,$event)'></div>";
tempStr += "                      <div v-if='c.ContentCount == 0 && c.Children ==null ' v-bind:class ='BtnDelete'  data-toggle='tooltip' data-placement='left' title='Delete Category' style='cursor:pointer;'  v-on:click='removeClick(c,$event)'></div>";
tempStr += "                      <div v-if='c.Level<limitlevel'  v-bind:class ='BtnAdd'     data-toggle='tooltip' data-placement='left' title='Add Sub-Category'style='cursor:pointer;'  v-on:click='addClick(c,$event)'></div>";
tempStr += "                      <div class='glyphicon' ><span class='badge' style='margin:0 0 5px 0;cursor:pointer;' data-toggle='tooltip' data-placement='left' title='Category Content' v-on:click='showContentList(c,$event)'>{{c.ContentCount}}</span></a></div>";
tempStr += "                  </div>";
tempStr += "                  <div v-if='canedit==2&&!c.Children' style='float:right;' >";
tempStr += "                      <a v-on:click='selectClick(c,$event)' style='cursor:pointer;'>select</a>";
tempStr += "                  </div>";
tempStr += "            </div>";
tempStr += "         </li>";
tempStr += "         <template v-if='c.Children&&c.Children.length>0'>";
tempStr += "            <li class='list-group-item' style='padding-top:0px;' :class='{hidden:c.Collapse}'>";
tempStr += "              <menu-item v-bind:items='c.Children' v-bind:limitlevel='limitlevel' v-bind:showcontentlist='showcontentlist' v-bind:canedit='canedit'  v-bind:clickadd='clickadd' v-bind:clickaddcontent= 'clickaddcontent' v-bind:clickedit='clickedit' v-bind:clickremove='clickremove' v-bind:issorted='issorted'  ></menu-item>";
tempStr += "            </li>";
tempStr += "         </template>";
tempStr += "      </template>";
tempStr += "    </ul>";
tempStr += "</div>";


Vue.component('menu-item', {
    name: "menu-item",
    template: tempStr,
    props: ['items', 'canedit', 'clickadd', 'clickedit', 'clickremove', 'limitlevel', 'issorted', 'showcontentlist', 'clickaddcontent'],//注意不能有大寫
    data: function () {
        return {
            //  nodes: this.items
            Edit: "編輯",
            BtnEdit: "glyphicon glyphicon-edit",
            BtnDelete: "glyphicon glyphicon-trash",
            BtnAdd: "glyphicon glyphicon-plus",
            BtnContent: "glyphicon glyphicon-pencil",

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
            this.clickedit(obj);
        },
        addContentClick: function (obj, e) {
            console.log("component click addcontent", obj);
            //this.$emit('click-edit');
            //eval(this.clickedit + "()"); //綁定字符串的調用方法
            this.clickaddcontent(obj);
        },
        removeClick: function (obj, e) {
            console.log("component click remove", obj);
            this.clickremove(obj);
        },
        addClick: function (obj) {
            //console.log("component click add", obj);
            //if (this.clickadd) {
            this.clickadd(obj);
            //}
        },
        showContentList: function (obj, e) {
            console.log("component click showcontentlist", obj);
            this.showcontentlist(obj);
        },
        selectClick: function (obj, e) {
            bDialog.closeCurrent(obj);
        },
        changeFlag: function (obj) {
            obj.IsChange = true;
            //alert(obj.IsChange);
        }


    }
});

layui.use(['miniTab'], function () {
    var layer = layui.layer,
        miniTab = layui.miniTab;
});

function showEdit() {
    console.log("call showEdit");
}
//var uri = "http://jm.intimex.hk/adminapi/SystemMenu";
var uri = "/adminapi/CMS";
var app = new Vue({
    el: "#MainContent",
    data: {
        //node: { Id: 0, ParentId: 0, Path: "", Code: "", Text: "", Url: "", Level: 0, Img: "", ImgPath: "", MutiLanguage: [{ Lang: { Text: "", Code: "" }, Desc: "" }] },
        treeNodes: [
            {
                Id: "1",
                ParentId: "",
                Path: "",
                Text: "system-settting1",
                Img: "",
                Collapse: true,
                Level: 1,
                Seq: 0,
                ContentCount: 0,
                Children: [{
                    Id: "11",
                    Text: "settting11",
                    Collapse: true,
                    Level: 2,
                    Seq: 0,
                    ContentCount: 0,
                    Children: []
                },
                {
                    Id: "12",
                    Text: "settting12",
                    Collapse: true,
                    Level: 2,
                    ContentCount: 0,
                    Children: []
                }]
            }
        ],
        isSorted: false,
        Condition: {
            CatId: "",
            Key: "",
            Name: "",
            Pager: {
                Page: 1,
                PageSize: 10,
                SortName: "",
                SortOrder: "",
            },
        },
        IsModify: true,
        IsDelete: true
    },
    methods: {
        loadMenus: function () {
            WS.AjaxP("get",
                uri + "/GetCatalogTree",
                null,
                function (response) {
                    if (response.Succeeded === true) {
                        app.treeNodes = response.ReturnValue;
                    } else {
                        showInfo(response.Message);
                    }

                },
                function () {
                });

        },
        showContentList: function (obj) {
            var params = new Object();
            params.id = obj.Id;
            params.obj = obj;
            console.log(obj);
            var tabId = self.frameElement.parentElement.id;
            //parent.setSrcTobTab("showContentList_" + obj.Id, obj.Text + '_' + 'ContentList', "/CMSCategory/CMSContentList/" + obj.Id, true);
            //app.loadMenus();
            miniTab.openNewTabByIframe({
                href: "/CMS/CMSContentList/" + obj.Id,
                title: obj.Text + "_" + "ContentList",
                callback: app.loadMenus
            });

        },
        editItem: function (obj) {
            var params = new Object();
            params.id = obj.Id;
            params.obj = obj;
            console.log(obj);
            id = 0;
            var tabId = self.frameElement.parentElement.id;
            //parent.setSrcTobTab("EditCategory_" + obj.Id, obj.Text + '_' + 'CategoryEdit', "/CMSCategory/CMSCategoryEdit/" + obj.Id +"/"+ id, true);
            //app.loadMenus();

            miniTab.openNewTabByIframe({
                href: "/CMS/EditCMSCategory/" + obj.Id + "/" + obj.ParentId,
                title: obj.Text + "_" + "CategoryEdit",
                callback: app.loadMenus
            });
        },
        addContent: function (obj) {
            var params = new Object();
            params.id = obj.Id;
            params.obj = obj;
            id = obj.Id;
            var tabId = self.frameElement.parentElement.id;
            miniTab.openNewTabByIframe({
                href: "/CMS/EditCMSContent/0/" + id,
                title: "Add_" + obj.Text + "_" + "Content",
                callback: app.loadMenus
            });
            //parent.setSrcTobTab("AddContent_" + obj.Id, 'Add_' + obj.Text + 'Content', "/CMSCategory/CMSContentEdit/0/" + id, true);
            //    app.loadMenus();

        },
        removeItem: function (obj) {
            SystemConfirm(Resources.ConfirmDeleteCategory, function () {
                var data = new Object();
                data.id = obj.Id;
                WS.AjaxP("get",
                    uri + "/DeleteCatalog",
                    data,
                    function (response) {
                        if (response.Succeeded === true) {
                            app.loadMenus();
                        } else {
                            showInfo(response.Message);
                        }
                    },
                    function () { });
            });

        },
        addItem: function (obj) {

            var params = new Object();
            params.id = obj.Id;
            params.obj = obj;
            id = obj.Id;
            var tabId = self.frameElement.parentElement.id;
            //parent.setSrcTobTab("AddSubCategory_", 'Add' + ' ' + obj.Text + ' ' + 'SubCategoty', "/CMSCategory/CMSCategoryEdit/0/"+id, true);
            //app.loadMenus();

            miniTab.openNewTabByIframe({
                href: "/CMS/EditCMSCategory/0/" + id + "/" + obj.Level,
                title: "Add" + "_" + obj.Text + "_" + "SubCategoty",
                callback: app.loadMenus
            });

        },
        sorted: function () {
            app.isSorted = true;
        },

        addMainCategory: function () {
            id = 0;
            var tabId = self.frameElement.parentElement.id;
            //parent.setSrcTobTab("AddCategory", 'MainCategoryAdd', "/CMSCategory/CMSCategoryEdit/0" + id, true);
            //app.loadMenus();
            miniTab.openNewTabByIframe({
                href: "/CMS/EditCMSCategory/0" + id + "/0",
                title: "Add_MainCategory",
                callback: app.loadMenus
            });
        },
        saveSorted: function () {
            var data = new Object();
            data.tree = app.treeNodes;
            WS.Ajax({
                type: "post",
                url: uri + "/UpdateCatalogSeq",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(app.treeNodes),
                success: function (response) {
                    if (response.Succeeded === true) {
                        app.loadMenus();
                        app.isSorted = false;
                    }
                    else {
                        showInfo(response.Message);
                    }
                },
                error: function () { }
            });
            //WS.AjaxP("post", uri + "/UpdateCatalogSeq", , function (response) {
            //    if (response.Succeeded == true) {
            //        app.treeNodes = response.ReturnValue;
            //    }
            //    else {
            //        showInfo(response.Message);
            //    }

            //},
            //function () {
            //});
        },
        cancelSorted: function () {
            app.isSorted = false;
        },
        search: function () {
            $("#tblContentList").bootstrapTable("refresh", { url: "/adminapi/CMS/GetContentListByCond" });
        },
        contentAdd: function () {
            id = 0;
            var tabId = self.frameElement.parentElement.id;

            miniTab.openNewTabByIframe({
                href: "/CMS/EditCMSContent/" + WS.GuidEmpty + "/" + id + "/" + "Add",
                title: "Add Content",
                callback: app.search
            });
        },
        contentModify: function () {
            var row = $("#tblContentList").bootstrapTable("getSelections");
            var tabId = self.frameElement.parentElement.id;

            miniTab.openNewTabByIframe({
                href: "/CMS/EditCMSContent/" + row[0].Id + "/" + row[0].CategoryId + "/" + "Modify",
                title: row[0].Name + "_ContentEdit",
                callback: app.search
            });
        },
        contentDelete: function () {
            var a = $("#tblContentList").bootstrapTable("getSelections");

            var ids = "";
            SystemConfirm(Resources.ConfirmDelete, function () {

                for (var i = 0; i < a.length; i++) {
                    if (ids === "") {
                        ids = a[i].Id;
                    }
                    else {
                        ids += "," + a[i].Id;
                    }
                }

                var obj = new Object();
                obj.tids = ids;
                console.log(obj.tids);
                WS.AjaxP("get", "/adminapi/CMS/DeleteContent", obj, function (response) {
                    if (response.Succeeded === true) {
                        app.search();
                    }
                    else {
                        showWarn('@BDMall.Resources.Message.DeleteFailed');
                    }
                }, function () { });
            });
        },
        setButtonState: function () {
            var selected = $("#tblContentList").bootstrapTable("getSelections");
            if (selected.length === 1) {
                app.IsModify = false;
            }
            else {
                app.IsModify = true;
            }

            if (selected.length >= 1) {
                app.IsDelete = false;
            }
            else {
                app.IsDelete = true;
            }
        }
    },

    mounted: function () {
        this.loadMenus();
        //this.editItem(null);
    }
});

function EditCategory(id, name) {
    var tabId = self.frameElement.parentElement.id;

    miniTab.openNewTabByIframe({
        href: "/CMS/EditCMSCategory/" + id + "/" + 0,
        title: name + "_" + "CategoryEdit",
        callback: app.search
    });
}

function EditContent(id, key, name) {
    var tabId = self.frameElement.parentElement.id;

    miniTab.openNewTabByIframe({
        href: "/CMS/EditCMSContent/" + key + "/" + id + "/" + "Modify",
        title: name + "_ContentEdit",
        callback: app.search
    });
}
