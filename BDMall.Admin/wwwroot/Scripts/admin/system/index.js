var tempStr = "";
tempStr += "<div id='menuArea' style='padding: 1px; overflow: auto;overflow-x: hidden;'>";
tempStr += "<ul id = 'main-nav' class='nav nav-tabs nav-stacked collapse navbar-collapse'>";
tempStr += "<li v-for='c in items'>";
tempStr += "<a v-if='c.Children&&c.Children.length>0' v-bind:href='\"#\"+c.Code' class='nav-header collapsed' data-toggle='collapse'>";
tempStr += "<i><img v-if='c.Img' v-bind:src='c.Img' width='25'/></i>";
tempStr += "{{c.Text}}";
tempStr += "<span class='pull-right glyphicon glyphicon-chevron-down'></span>";
tempStr += "</a>";
tempStr += "<a v-else v-bind:layuimini-href='c.Path' v-bind:id='c.Code' v-bind:data-title='c.Text'>";
tempStr += "<i><img v-if='c.Img' v-bind:src='c.Img'  width='25'/></i>";
tempStr += "{{c.Text}}";
tempStr += "</a>";
tempStr += "<ul v-if='c.Children&&c.Children.length>0' v-bind:id='c.Code' class='nav nav-list collapse secondmenu' style='height: 0px;'>";
tempStr += "<li v-for='cc in c.Children'><a v-bind:layuimini-href='cc.Path' v-bind:id='cc.Code' v-bind:data-title='cc.Text'><i><img v-if='cc.Img' v-bind:src='cc.Img' width='25'/></i>{{cc.Text}}</a></li>";
tempStr += "</ul>";
tempStr += "</li>";
tempStr += "</ul>";
tempStr += "</div>";

Vue.component('menu-item', {
    name: "menu-item",
    template: tempStr,
    props: ['items'],//注意不能有大寫
    data: function () {
        return {
            //  nodes: this.items
        };
    }
    , methods: {

    }
})

function showEdit() {
    console.log("call showEdit");
}
//var uri = "http://jm.intimex.hk/adminapi/SystemMenu";
var uri = "/adminapi/SystemMenu";
var app = new Vue({
    el: "#mainArea",
    data: {
        treeNodes: [
            {
                Id: "0", Text: "", Img: "", Collapse: true, Level: 1, Children: [
                    { Id: "0", Text: "", Collapse: true, Level: 2, Children: [] }]
            }]
    },
    methods: {
        loadMenus: function () {
            WS.AjaxP("get", uri + "/GetMenuTreeByUser", {}, function (response) {
                if (response) {
                    app.treeNodes = response;
                    app.$nextTick(function () {
                        InitbTabs();
                        navCollapse();
                    });
                } else {
                    window.location.href = "/";
                }
            }, function () { })
        },
        checkLogin: function () {
            var times = 0;
            setInterval(function () {
                WS.AjaxP("get", "/AdminApi/Token/Check?v=0.01", {}, function (result) {
                    if (result.Succeeded === false) {
                        //console.log("失效。。。");
                        times = times + 1;
                        if (times > 5) {
                            window.location.href = "/default/index";
                        }
                    } else {
                        times = 0;
                    }
                }, function () {
                    times = times + 1;
                    if (times > 5) {
                        window.location.href = "/default/index";
                    } else {
                        times = 0;
                    }
                });
            }, 5000);

        }
    },
    mounted: function () {
        this.loadMenus();
        this.checkLogin();
    }
});





