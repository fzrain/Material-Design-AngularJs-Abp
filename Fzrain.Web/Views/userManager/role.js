(function () {
    materialAdmin.controller('roleController', [
        'abp.services.app.role', 'ngTableParams', 'notifyService','$modal',
        function (roleService, ngTableParams, notifyService, $modal) {
            var vm = this;
            vm.activeTab = "role";
            vm.roleTab = function () {
                vm.activeTab = "role";
            }
            vm.permissionTab = function () {
                vm.activeTab = "permission";
            }
        
            vm.alert = function () {
               
                //$(".modal-backdrop fade in").css("position", "static");
                // notifyService.notify('更新成功！', 'danger');
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
            vm.save = function () {
                roleService.addRole(vm.role).success(function () {
                    notifyService.notify('保存成功！', 'success');
                    $("#modalRoleEdit").modal("hide");
                });
            }
        }
    ]);
})();