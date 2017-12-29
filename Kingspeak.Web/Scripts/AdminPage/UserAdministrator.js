var userAdministrator = new function () {
    var Current = this;
    this.Init = function () {
        Current.InitTable();
        Current.BtnClick();
    }

    this.InitTable = function () {
        $('#table').bootstrapTable({
            url: '/Admin/User/GetAdminList',                           //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                   //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            pagination: true,                   //是否显示分页（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            queryParams: Current.GetParams,//传递参数（*）
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 20, 30],            //可供选择的每页的行数（*）
            columns: [{
                checkbox: true,
                align: 'center'
            }, {
                field: 'UserID',
                title: '用户编号'
            }, {
                field: 'UserName',
                title: '用户名称'
                 , sortable: true
            }, {
                field: 'TrueName',
                title: '来源'
            }, {
                field: 'AcatarImg',
                title: '头像'
            }, {
                field: 'CreateDate',
                title: '创建时间'
                 , sortable: true
                 , formatter: function (value) { return Common.FormatTime(value); }
            }, {
                field: 'LastOnlineDate',
                title: '最后登录时间'
                , sortable: true
                 , formatter: function (value) { return Common.FormatTime(value); }
            }, {
                field: 'UserID',
                title: '操作'
                 , formatter: Current.GetFunHtml
            }

            ]
        });
    }

    this.GetParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset//页码
            , SearchType: $("#searchType").val()
            , SearchKey: $("#searchkey").val()
            , sortOrder: params.order//排序
            , sortName: params.sort//排序字段
        };
        return temp;
    }

    this.GetFunHtml = function (value, row) {
        var html = '<a>修改</a>';
        return html;
    }

    this.BtnClick = function () {
        

        $("#btn_search").click(function () {
            $('#table').bootstrapTable('selectPage', 1);
        })
    }
}
$(function () {
    userAdministrator.Init();
})