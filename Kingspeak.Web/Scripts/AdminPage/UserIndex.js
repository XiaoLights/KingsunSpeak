var userIndex = new function () {
    var Current = this;
    this.Init = function () {
        Current.InitTable();
        Current.BtnClick();

        $("#selresource").change(function () {
            $('#table').bootstrapTable('selectPage', 1);
        })

    }

    this.InitTable = function () {
        $('#table').bootstrapTable({
            url: '/Admin/User/GetUserList',                           //请求后台的URL（*）
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
                field: 'UserId',
                title: '用户编号'
                , sortable: true
            }, {
                field: 'UserName',
                title: '用户名称'
                 , sortable: true
            }, {
                field: 'Resource',
                title: '来源'
            }, {
                field: 'CreateTime',
                title: '创建时间'
                , sortable: true
                , formatter: function (value) { return Common.FormatTime(value); }
            }, {
                field: 'UserIdMod',
                title: 'MOD用户编号'
            }, {
                field: 'UserType',
                title: '用户类型'
            }, {
                field: 'RealName',
                title: '真实姓名'
            }, {
                field: 'Sex',
                title: '性别'
            }, {
                field: 'AddTime',
                title: '添加时间'
                 , sortable: true
                  , formatter: function (value) { return Common.FormatTime(value); }
            }, {
                field: 'Identity',
                title: 'MOD用户编号'
            }, {
                field: 'Grade',
                title: '年级'
            }, {
                field: 'Status',
                title: '状态'
            }, {
                field: 'YUid',
                title: '来源系统用户编号'
            }
            //, {
            //    field: 'UserId',
            //    title: '操作'
            //     , formatter: Current.GetFunHtml
            //}

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
            , Source: $("#selresource").val()
        };
        return temp;
    }

    this.GetFunHtml = function (value, row) {
        var html = '<a>修改</a>';
        return html;
    }

    this.BtnClick = function () {
        $("#btn_export").click(function () {
            var param = { limit: 1, offset: 2 };
            var obj = Current.GetParams(params);
            var $form = $('<form target="down-file-iframe" method="post" />');
            $form.attr('action', "/Admin/User/ExportUser");
            for (var key in obj) {
                $form.append('<input type="hidden" name="' + key + '" value="' + obj[key] + '" />');
            }
            $(document.body).append($form);
            $form.submit();
            $form.remove();
        })
        $("#btn_search").click(function () {
            $('#table').bootstrapTable('selectPage', 1);
        })
    }
}
$(function () {
    userIndex.Init();
})