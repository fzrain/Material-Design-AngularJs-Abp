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
        vm.permission = {
            add: abp.auth.hasPermission('Administration.Role.Create'),
            edit: abp.auth.hasPermission('Administration.Role.Edit'),
            delete: abp.auth.hasPermission('Administration.Role.Delete')
        };

        vm.boolValue= {"是":true,"否":false}
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
        vm.save = function () {
            vm.role.permissions = $("#permissionTree").jstree("get_checked"); //使用get_checked方法 
            roleService.addOrUpdate(vm.role).success(function () {
                vm.tableBasic.reload();
                notifyService.notify('保存成功！', 'success');
                $("#modalRoleEdit").modal("hide");
            });
        }
        vm.delete = function (id) {
            notifyService.comform(function () {
                roleService.delete({ id: id }).success(function () {
                    vm.tableBasic.reload();
                    window.swal("删除成功!", "此角色已删除！", "success");
                });
            });         
        }
        vm.getRoleDetail = function (id) {

            roleService.getById({ id: id }).success(function (data) {
               
                vm.role = data;
                if (id == null) {
                    vm.role.id = null;
                }
                var permission = [];
                for (var i = 0; i < data.permissions.length; i++) {
                    var node = {
                        "id": data.permissions[i].name,
                        "parent": data.permissions[i].parentName == "无" ? "#" : data.permissions[i].parentName,
                        "text": data.permissions[i].displayName,
                        "state": {
                            "opened": (data.permissions[i].name.split(".")).length <= 1,
                            "selected": data.permissions[i].isGrantedByDefault
                        }
                    }
                    permission.push(node);
                }
                $('#permissionTree').data('jstree', false).empty().jstree({
                    'plugins': ["wholerow", "checkbox", "types"],
                    "checkbox": {
                        "three_state": false,
                        //"cascade": "down"
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