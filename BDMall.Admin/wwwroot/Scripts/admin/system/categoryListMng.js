var tempStr = "";
tempStr += "<ul class ='list-group' style='margin:0px;'>";
tempStr += "       <template v-for='c in items'>";
tempStr += "          <li class ='list-group-item' >";
tempStr += "              <span v-if='c.Children&&c.Children.length>0&&c.Collapse' style='cursor:pointer;' class ='glyphicon glyphicon-collapse-down pull-left' v-on:click='toggleNode(c)'></span>";
tempStr += "              <span v-if='c.Children&&c.Children.length>0&&!c.Collapse' style='cursor:pointer;' class ='glyphicon glyphicon-collapse-up pull-left' v-on:click='toggleNode(c)'></span>";
tempStr += "              <div  style='height:15px;'>";
tempStr += "                  <div class ='pull-left'>";
tempStr += "                          <img v-if='c.ImgPath' v-bind:src='c.ImgPath' width='20' /> {{c.Text}}";
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
tempStr += "</ul>";


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
})

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
        isSorted: false

    },
    methods: {
        loadMenus: function () {
            WS.AjaxP("get", uri + "/GetCatalogTree", null, function (response) {
                if (response.Succeeded == true) {
                    app.treeNodes = response.ReturnValue;
                }
                else {
                    showInfo(response.Message);
                }

            },
            function () {
            })

        },
        showContentList: function (obj) {
            var params = new Object();
            params.id = obj.Id;
            params.obj = obj;

            parent.setSrcTobTab("EditCategory_" + obj.Id, obj.Text + '_' + 'ContentList', "/CMS/CMSContentList/" + obj.Id, true);
            app.loadMenus();
        },
        editItem: function (obj) {
            var params = new Object();
            params.id = obj.Id;
            params.obj = obj;
            id = 0;
            parent.setSrcTobTab("EditCategory_" + obj.Id, obj.Text + '_' + 'CategoryEdit', "/CMS/EditCMSCategory/" + obj.Id +"/"+ id, true);
            app.loadMenus();
        },
        addContent: function (obj) {
            var params = new Object();
            params.id = obj.Id;
            params.obj = obj;
            id = obj.Id;
            parent.setSrcTobTab("AddContent_" + obj.Id, 'Add_' + obj.Text + 'Content', "/CMS/EditCMSContent/0/" + id, true);
            app.loadMenus();

        },
        removeItem: function (obj) {
            console.log("app call removeItem function");
            SystemConfirm("Confirm Delete?", function () {
                var data = new Object();
                data.id = obj.Id;
                WS.AjaxP("get", uri + "/DeleteCatalog", data, function (response) {
                    if (response.Succeeded == true) {
                        app.loadMenus();
                    }
                    else {
                        showInfo(response.Message);
                    }
                },
                function () { })
            });

        },
        addItem: function (obj) {

            var params = new Object();
            params.id = obj.Id;
            params.obj = obj;
            id = obj.Id;
            parent.setSrcTobTab("AddSubCategory_", 'Add' + ' ' + obj.Text + ' ' + 'SubCategoty', "/CMS/EditCMSCategory/0/" + id, true);
            app.loadMenus();

        },
        sorted: function () {
            app.isSorted = true;
        },

        addMainCategory: function () {
            id = 0;
            parent.setSrcTobTab("AddCategory", 'MainCategoryAdd', "/CMS/EditCMSCategory/0" + id, true);
            app.loadMenus();
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
        }
    },

    mounted: function () {
        this.loadMenus();
        //this.editItem(null);
    }



});