var tempStr = "";
tempStr += "<ul class='nav nav-tabs' id='ulLang'>";
tempStr += "<li v-for='(item,index) in data' v-bind:id='item.Lang.Code' v-bind:class='{active:currentLang==item.Lang.Code}' v-on:click='selectLanguage(item)'>";
tempStr += "<a style='cursor:pointer;'>{{item.Lang.Text}}</a>";
tempStr += "</li>";
tempStr += "</ul>";


Vue.component('multilang-bar', {
    name: "multilang-bar",
    template: tempStr,
    props: ['data', 'selectlanguage'],//注意不能有大寫
    data: function () {
        var langCode;
        if (this.data != undefined && this.data.length > 0) {
            langCode = this.data[0].Lang.Code;
        }
        return {
            //  nodes: this.items
            currentLang: langCode
        };
    },
    methods: {
        selectLanguage: function (obj) {
            this.currentLang = obj.Lang.Code;
            this.selectlanguage(obj);
        },
        setCurrentLanguage: function (currentLang) {
            this.currentLang = currentLang;
        }
        //tabClick: function ()
        //{
        //    this.tabclick();
        //} 
    }
})