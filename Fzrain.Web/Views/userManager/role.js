(function () {
    materialAdmin.controller('roleController', [
        'abp.services.app.role', 'ngTableParams', 'notifyService',
    function (roleService, ngTableParams, notifyService) {
        var vm = this;
        vm.activeTab = "role";
        vm.roleTab = function () {
            vm.activeTab = "role";
        }
        vm.permissionTab = function () {
            vm.activeTab = "permission";
        }
        vm.tableBasic = new ngTableParams({
            page: 1,
            count: 10
        }, {
            total: 0,
            getData: function ($defer, params) {
                roleService.getRoles({
                    skipCount: (params.page() - 1) * params.count(), maxResultCount: params.count()
                }).success(function (data) {
                    params.total(data.totalCount);
                    $defer.resolve(data.items);
                });

            }
        });
        vm.role = {};
        vm.add = function () {
            vm.role = null;
        }
        vm.getCheck = function () {

            var nodes = $("#permissionTree").jstree("get_checked"); //使用get_checked方法 

            alert(nodes);
        }
        vm.save = function () {
            vm.role.permissions = $("#permissionTree").jstree("get_checked"); //使用get_checked方法 
            roleService.addOrUpdate(vm.role).success(function () {
                vm.tableBasic.reload();
                notifyService.notify('保存成功！', 'success');
                $("#modalRoleEdit").modal("hide");
            });
        }
        vm.delete = function (id) {
            swal(
                {
                    title: "确定要删除吗?",
                    text: "删除此记录将不可恢复!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "删除!",
                    closeOnConfirm: false,
                    cancelButtonText: "取消"
                },
                function () {
                    roleService.delete({ id: id }).success(function () {
                        vm.tableBasic.reload();
                        swal("删除成功!", "此角色已删除！", "success");
                    });

                });
        }
        vm.getRoleDetail = function (id) {
         
            roleService.getById({ id: id }).success(function (data) {
                vm.role = data;
                var permission = [];
                for (var i = 0; i < data.permissions.length; i++) {
                    var node = {
                        "id": data.permissions[i].name,
                        "parent": data.permissions[i].parentName == "无" ? "#" : data.permissions[i].parentName,
                        "text": data.permissions[i].displayName,
                        "state": {
                            "opened": true,
                            "selected": data.permissions[i].isGrantedByDefault
                        }
                    }
                    permission.push(node);
                }
                $('#permissionTree').data('jstree', false).empty().jstree({
                    'plugins': ["wholerow", "checkbox", "types"],
                    "checkbox": {
                        "three_state": false,
                        "cascade": "down"
                    },
                    'core': {
                        'data': permission,
                        'themes': {
                            'name': 'proton',
                            'responsive': true
                        }
                    },
                    "types": {
                        "default": {
                            "icon": "fa fa-folder icon-state-warning icon-lg"
                        },
                        "file": {
                            "icon": "fa fa-file icon-state-warning icon-lg"
                        }
                    }
                });               
            });
        }
    }
    ]);
})();