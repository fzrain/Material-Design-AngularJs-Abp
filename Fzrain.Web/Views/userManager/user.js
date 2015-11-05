(function () {
    materialAdmin.controller('userController', [
        'abp.services.app.user','ngTableParams','notifyService',
        function (userService, ngTableParams, notifyService) {
            var vm = this;
            vm.activeTab = "user";
            vm.roleTab = function () {
                vm.activeTab = "role";
            }
            vm.userTab = function () {
                vm.activeTab = "user";
            }
            vm.tableBasic = new ngTableParams({
                page: 1,
                count: 10
            }, {
                total: 0,
                getData: function ($defer, params) {
                    userService.getUsers({
                        skipCount: (params.page() - 1) * params.count(), maxResultCount: params.count()
                    }).success(function (data) {
                        params.total(data.totalCount);
                        $defer.resolve(data.items);
                    });

                }
            });
          
            vm.add = function () {
                vm.user = {};
                userService.getUserForEdit({id:null}).success(function (data) {
                    vm.user = data;
                    vm.user.id = null;                                  
                });
            }
            vm.getUserDetail = function (id) {
                userService.getUserForEdit({ id: id }).success(function (data) {
                    vm.user = data;                   
                });
            }
            vm.resetPermissions = function () {
                userService.resetUserSpecificPermissions({ id: vm.userPermissionId }).success(function () {
                    vm.getUserPermission(vm.userPermissionId);
                });
                
            }
            vm.getUserPermission = function (id) {
                vm.userPermissionId = id;
                userService.getUserPermissions({ id: id }).success(function (data) {              
                    var permission = [];
                    for (var i = 0; i < data.length; i++) {
                        var node = {
                            "id": data[i].name,
                            "parent": data[i].parentName == "无" ? "#" : data[i].parentName,
                            "text": data[i].displayName,
                            "state": {
                                "opened": (data[i].name.split(".")).length <= 1,
                                "selected": data[i].isGrantedByDefault
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
            vm.save = function () {
                vm.user.roles = [];
                $.each(vm.user.roleInfos, function (i,n) {
                    if (n.isAssigned) {
                        vm.user.roles.push(n.name);
                    }
                });
                userService.addOrUpdate(vm.user).success(function () {
                    vm.tableBasic.reload();
                    notifyService.notify('保存成功！', 'success');
                    $("#modalUserEdit").modal("hide");
                });
            }
            vm.delete = function (id) {
                notifyService.comform(function () {
                    userService.delete({ id: id }).success(function () {
                        vm.tableBasic.reload();
                        window.swal("删除成功!", "此用户已删除！", "success");
                    });
                });
            }
            vm.savePermission = function () {
                userService.updateUserPermission({
                    id: vm.userPermissionId,
                    permissions: $("#permissionTree").jstree("get_checked")
                }).success(function () {
                    vm.tableBasic.reload();
                    notifyService.notify('保存权限成功！', 'success');
                    $("#modalUserPermissionEdit").modal("hide");
                });
            }
        }
    ]);
})();