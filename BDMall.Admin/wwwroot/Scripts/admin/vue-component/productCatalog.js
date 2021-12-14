var totalCombobox = 9
var catalogs = [];
//var path = "";
var tempStr = "";
tempStr += "<div class='row'>";
tempStr += "<div class='col-xs-12 col-sm-4 col-lg-2'><select id='cbo-1' class='form-control '  v-on:change='initNextCombobox($event)'></select></div>";
tempStr += "<div class='col-xs-12 col-sm-4 col-lg-2'><select id='cbo-2' class ='form-control'   v-on:change='initNextCombobox($event)' ></select></div>";
tempStr += "<div class='col-xs-12 col-sm-4 col-lg-2'><select id='cbo-3' class ='form-control' v-on:change='initNextCombobox($event)' ></select></div>";
tempStr += "<div class='col-xs-12 col-sm-4 col-lg-2'><select id='cbo-4' class ='form-control'  v-on:change='initNextCombobox($event)'></select></div>";
tempStr += "<div class='col-xs-12 col-sm-4 col-lg-2'><select id='cbo-5' class ='form-control' v-on:change='initNextCombobox($event)' ></select></div>";
tempStr += "<div class='col-xs-12 col-sm-4 col-lg-2'><select id='cbo-6' class ='form-control'  v-on:change='initNextCombobox($event)' ></select></div>";
tempStr += "<div class='col-xs-12 col-sm-4 col-lg-2'><select id='cbo-7' class ='form-control'  v-on:change='initNextCombobox($event)' ></select></div>";
tempStr += "<div class='col-xs-12 col-sm-4 col-lg-2'><select id='cbo-8' class ='form-control'  v-on:change='initNextCombobox($event)' ></select></div>";
tempStr += "<div class='col-xs-12 col-sm-4 col-lg-2'><select id='cbo-9' class ='form-control'  v-on:change='initNextCombobox($event)' ></select></div>";
tempStr += "</div>";


Vue.component('combobox-item', {
    name: "combobox-item",
    template: tempStr,
    props: ['getcombobox'],
    data: function () {
        return {
            result: {
                nextLength: 0,
                currentValue: 0
            }

        }
    }
    , methods: {
        initNextCombobox: function (me) {
            if (me.currentTarget.value != "" && this.result.currentValue != me.currentTarget.value) {
                var id = parseInt(me.currentTarget.id.split('-')[1]);
                var nextID = id + 1;
                this.result.currentValue = me.currentTarget.value;

                hideRestCombobox(nextID, this.result);
                var data = new Object();
                data.catId = me.currentTarget.value;

                BindCombobox("cbo-" + nextID, me.currentTarget.value)
                //InitNormalSelect("cbo-" + nextID, "/adminapi/Product/GetCatalog", true, data);

                //this.$emit('getselected', value);
                this.result.nextLength = $("#cbo-" + nextID + " option").length;

                if (this.result.nextLength) {
                    $("#cbo-" + nextID).parent().show();
                }
                
                this.getcombobox(this.result);
                //this.$emit
            }

        }
    }

});


function hideRestCombobox(id,data) {
    if (id <= totalCombobox) {
        for (var i = id; i <= totalCombobox; i++) {
            //$("#cbo-" + i).css('visibility', '');
            
            $("#cbo-" + i).empty();
            $("#cbo-" + i).parent().hide();
            //.css('visibility', 'hide')
        }
    }
}

function BindCombobox(comboboxId, parentId) {
    var data = [];
    if (catalogs.length > 0) {

        catalogs.forEach(function (val, index, me) {
            if (val.ParentId == parentId) {
                var option = new Object();
                option.Id = val.Id;
                option.Text = val.Desc;
                data.push(option);
            }
        });
        InitNormalSelectByData(comboboxId, data);

    }


}

$(document).ready(function () {
    
    WS.AjaxP("get", "/adminapi/ProdCatalog/GetAllCatalog", null, function (response) {
        if (response.Succeeded == true) {
            catalogs = response.ReturnValue;
            BindCombobox("cbo-1", WS.GuidEmpty);
        }
        else {
            alert(response.Message);
        }
    }, function () { });
    hideRestCombobox(2);
});

