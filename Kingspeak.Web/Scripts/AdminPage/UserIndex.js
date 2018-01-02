var userIndex = new function () {
    var Current = this;
    this.Init = function () {
        Current.InitTable();
        Current.BtnClick();

        $("#selresource").change(function () {
            $('#table').bootstrapTable('selectPage', 1);
        })

        $('.datepicker').datepicker({
            format: 'yyyy-mm-dd',
            clearBtn: true,//清除按钮
            autoclose: true
        })

        Current.Validata();
        Current.InitUploadBtn();
    }

    this.InitTable = function () {
        $('#table').bootstrapTable({
            url: '/Admin/User/GetUserList',                           //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                   //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            pagination: true,                   //是否显示分页（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            clickToSelect: true,
            queryParams: Current.GetParams,//传递参数（*）
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 20, 30],            //可供选择的每页的行数（*）
            onPostBody: function () {
                $('#table input.switch').bootstrapSwitch({
                    size: "small",
                    onSwitchChange: function (event, state) {
                        if (state == true) {
                            $(this).val("1");
                        } else {
                            $(this).val("2");
                        }
                    }
                });
            },
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
                field: 'TelePhone',
                title: '联系方式'
            }, {
                field: 'RealName',
                title: '真实姓名'
            }, {
                field: 'Grade',
                title: '年级'
            }, {
                field: 'UserType',
                title: '用户类型'
                 , formatter: function (value) {
                     var html = '';
                     if (value == "1") {
                         html += '教师';
                     }
                     else {
                         html += '学生';
                     }
                     return html;
                 }
            }, {
                field: 'Sex',
                title: '性别'
            }, {
                field: 'Resource',
                title: '来源'
            }, {
                field: 'CreateTime',
                title: '创建时间'
                , sortable: true
                , formatter: function (value) { return Common.FormatTime(value); }
            }, {
                field: 'Status',
                title: '状态'
                , formatter: function (value,row) {
                    var html = '';
                    if (value == "1") {
                        var param = "'" + row.UserId + "','2'";
                        // html += '<input type="checkbox" class="switch switch-small" checked data-on-text="正常" data-off-text="禁用"/>  ';
                        html += '<a href="javascript:void(0)" title="领取课程" onclick="userIndex.ChangeState(' + param + ')"><span class="label label-success">启用</span></a>';
                    }
                    else {
                        var param = "'" + row.UserId + "','1'";
                        html += '<a href="javascript:void(0)" title="领取课程" onclick="userIndex.ChangeState(' + param + ')"><span class="label label-danger">禁用</span></a>';
                    }
                    return html;
                }
            }, {
                field: 'StuPhone',
                title: '是否已领取课程'
                , formatter: function (value, row) {
                    var html = '';
                    if (value) {
                        html += '<span class="label label-success">已领取</span>';
                    }
                    else {
                        var param = "'" + row.UserId + "'";
                        html += '<a href="javascript:void(0)" title="领取课程" onclick="userIndex.GetFreeClass(' + param + ')"><span class="label label-danger">未领取</span></a>';
                    }
                    return html;
                }
            }, {
                field: 'GetClassDate',
                title: '领取课程时间', sortable: true
                 , formatter: function (value) {
                     if (value) {
                         return Common.FormatTime(value);
                     }
                 }
            }, {
                field: 'ListenDate',
                title: '试听时间'
                 , formatter: function (value) {
                     if (value) {
                         return Common.FormatTime(value, 'yyyy-MM-dd');
                     }
                 }
            }, {
                field: 'SignupDate',
                title: '报名时间'
                 , formatter: function (value) {
                     if (value) {
                         return Common.FormatTime(value, 'yyyy-MM-dd');
                     }
                 }
            }
            , {
                field: 'SignupMoney',
                title: '报名费用'
            }
            , {
                field: 'AdviserName',
                title: '课程顾问'
            }
              , {
                  field: 'IDs',
                  title: '操作'
                 , events: operateEvents
                  // formatter: operateFormatter
                  , formatter: function (value, row) {
                      return ['<button type="button" class="EditClass btn btn-default  btn-sm" style="margin-right:15px;">修改</button>'
                      ].join('');
                      //var html = '';
                      //var param = "'" + row.UserId + "'";
                      //html += '<a href="javascripts:void(0)" title="编辑用户上课信息" onclick="userIndex.EditClassInfo(' + param + ')">编辑</a>';
                      //return html;
                  }
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
            var params = { limit: 1, offset: 2 };
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

        $("#btn_adduser").click(function () {
            Current.OpenDialog();
        })

        $("#btn_deluser").click(function () {
            Current.DeleteUser();
        })
    }

    this.ChangeState = function (userid,state) {
        var obj = { UserID: userid, State: state };
        $.post("/Admin/User/ChangeState", obj, function (data) {
            if (data.Success) {
                layer.msg("操作成功", { time: 1000 });
                $('#table').bootstrapTable("refresh");
            } else {
                layer.alert(data.ErrorMsg);

            }
        })
    }

    this.SaveUser = function () {
        $.post("/Admin/User/SaveUser", $("#addform").serialize(), function (data) {
            if (data.Success) {
                layer.alert("保存成功", { time: 1000 });
                layer.closeAll('page');
                $('#table').bootstrapTable("refresh");
            } else {
                layer.alert(data.ErrorMsg);

            }
        })
    }

    this.DeleteUser = function () {
        layer.confirm('确认要删除选中用户吗？', {
            title: '确认',
            btn: ['确定', '取消'] //按钮
        }, function () {
            var arr = $('#table').bootstrapTable('getSelections');
            if (arr.length < 1) {
                layer.msg("请选择一条记录", { time: 1000 });
                return;
            }
            var idarr = '';
            for (var i = 0; i < arr.length; i++) {
                if (idarr.length != 0) {
                    idarr += ',';
                }
                idarr += arr[i].UserId;
            }
            var obj = { UserIDs: idarr };
            $.post('/Admin/User/DeleteUser', obj, function (data) {
                if (data.Success) {
                    layer.alert("删除成功", { time: 1000 });
                    $('#table').bootstrapTable("refresh");
                }
                else {
                    layer.msg(data.ErrorMsg);
                }
            })

        }, function () {

        });
    }

    this.OpenDialog = function () {
        layer.open({
            title: '添加用户',
            type: 1,
            area: ['600px', '450px'],//大小设置
            fixed: false, //不固定
            btn: ['保存', '放弃'],
            content: $('#addUser'),
            btn1: function () {
                //按钮1的回掉（保存按钮)
                Current.SaveUser();
            }
        });
    }

    this.OpenEditClassDialog = function () {
        layer.open({
            title: '编辑试听课程信息',
            type: 1,
            area: ['600px', '400px'],//大小设置
            fixed: false, //不固定
            btn: ['保存', '放弃'],
            content: $('#editClass'),
            btn1: function () {
                //按钮1的回掉（保存按钮)
                Current.SaveClassInfo();
            }
        });
    }

    this.SaveClassInfo = function () {
        $.post("/Admin/User/SaveEditClassInfo", $("#editclassform").serialize(), function (data) {
            if (data.Success) {
                layer.alert("保存成功", { time: 1000 });
                layer.closeAll('page');
                $('#table').bootstrapTable("refresh");
            } else {
                layer.alert(data.ErrorMsg);

            }
        })
    }

    this.GetFreeClass = function (userid) {
        layer.confirm('获取课程操作不可逆，确认为改用户获取课程？', {
            title: '确认',
            btn: ['确定', '取消'] //按钮
        }, function () {
            var obj = { UserID: userid };
            $.post("/Admin/User/GetFreeClass", obj, function (data) {
                if (data.Success) {
                    layer.msg("获取成功", { time: 1000 });
                    $('#table').bootstrapTable("refresh");
                } else {
                    layer.alert(data.ErrorMsg);
                }
            })
        }, function () {

        });
    }

    this.InitUploadBtn = function () {
        var uploader = WebUploader.create({
            auto: true,
            // swf文件路径
            swf: '/Scripts/Plugins/webuploader/dist/Uploader.swf',
            // 文件接收服务端。
            server: '/Admin/User/UploadUser',
            // 选择文件的按钮。可选。
            // 内部根据当前运行是创建，可能是input元素，也可能是flash.
            pick: {
                id: '#btn_import',
                multiple: false
            },
            fileNumLimit: 1,
            // 只允许选择excel表格文件。
            accept: {
                title: 'Applications',
                extensions: 'xls,xlsx',
                mimeTypes: 'application/xls,application/xlsx'
            },
            uploadSuccess: function (file, response) {

            }
        });
    }

    this.Validata = function () {
        $("#addform").validate({
            rules: {
                txtuserName: "required",
                txttrueName: {
                    required: true
                },
                txttele: {
                    required: true
                } 
            },
            messages: {
                txtuserName: "请输入用户名",
                txttrueName: {
                    required: "请输入真实姓名"                    
                },
                txttele: {
                    required: "请输入手机号"                     
                }
               
            }
        });
    }
}
$(function () {
    userIndex.Init();
})


window.operateEvents = {
    'click .EditClass': function (e, value, row, index) {
        if (row.StuPhone) {
            $("#hidUserId").val(row.UserId);
            $("#hidfcid").val(row.FreeClassID);
            $("#txtun").val(row.UserName);
            $("#txtlistendate").val(Common.FormatTime(row.ListenDate, 'yyyy-MM-dd'));
            $("#txtsigupdate").val(Common.FormatTime(row.SignupDate, 'yyyy-MM-dd'));
            $("#txtsigupmoney").val(row.SignupMoney);
            if (row.ClassAdviser) {
                $("#selAdviser").val(row.ClassAdviser);
            }
            userIndex.OpenEditClassDialog();
        } else {
            layer.alert("请先领取课程", { time: 1000 });
        }

    }
}