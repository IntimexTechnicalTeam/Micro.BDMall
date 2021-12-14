var tempStr = "";
tempStr += "<div style='width: 100%;text-align: center;clear: both;'>";
tempStr += "<ul class ='pagination' style='cursor:pointer'>";
tempStr += "<li><a v-on:click='firstPage()'>" + Resources.First + "</a></li>";
tempStr += "<li><a v-on:click='previousPage()'>&laquo;</a></li>";
tempStr += "<li class ='active'><a>{{currentPage}}</a></li>";
tempStr += "<li><a v-on:click='nextPage()'>&raquo;</a></li>";
tempStr += "<li><a v-on:click='lastPage()'>" + Resources.Last + "</a></li>";
tempStr += "<li><a>" + Resources.TotalPages + ": {{totalPage}}</a></li>";
tempStr += "</ul> ";
tempStr += "</div>";
// 注册
Vue.component('data-pager',
    {
        template: tempStr,
        props: ["url", "condition", "pagesize"],
        data: function () {
            return {
                currentPage: 1,
                totalPage: 0,
                size: this.pagesize,
                pData: [],
                dataUrl: this.url,
                queryCondition: this.condition
            };
        },
        methods: {
            firstPage: function () {
                this.currentPage = 1;
                this.query();
            },
            previousPage: function () {
                this.currentPage -= 1;
                if (this.currentPage < 1) {
                    this.currentPage = 1;
                } else {
                    this.query();
                }
            },
            nextPage: function () {
                var page = this.currentPage + 1;
                if (page > this.totalPage) {
                    return;
                } else {
                    this.currentPage = page;
                    this.query();
                }

            },
            lastPage: function () {
                this.currentPage = this.totalPage;
                this.query();
            },
            refresh: function (page, pageSize) {
                if (page) {
                    this.currentPage = page;
                }
                if (pageSize) {
                    this.size = pageSize;
                }
                this.query();
            },
            query: function () {
                var _this = this;
                var data = this.queryCondition;
                data.PageInfo = { Page: this.currentPage, PageSize: this.size };
                data.Page = this.currentPage;
                data.PageSize = this.size;
                WS.AjaxP("post",
                    this.dataUrl,
                    data,
                    function (result) {
                        //if (result.Succeeded) {

                        _this.totalPage = result.ReturnValue.TotalPage;
                        //_this.currentPage = result.Page;
                        _this.pData = result.ReturnValue.Data;
                        //} else {
                        //    showError(result.Message);
                        //}
                        _this.$emit('load', result);
                    });

            }
        }
    });
