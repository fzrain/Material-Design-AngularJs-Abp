(function () {
    materialAdmin.controller('permissionController', [
        'abp.services.app.permissionInfo', 'ngTableParams', 'notifyService',
        function (permissionService, ngTableParams, notifyService) {
            var vm = this;
           
            vm.tableBasic = new ngTableParams({
                page: 1,
                count: 10
            }, {
                total: 0,
                getData: function ($defer, params) {    
                    permissionService.getPermissions({
                        skipCount: (params.page() - 1) * params.count(), maxResultCount: params.count()
                    }).success(function (data) {
                        params.total(data.totalCount);
                        $defer.resolve(data.items);
                    });

                }
            });
            vm.permission = {};
            vm.add = function () {
                vm.permission = null;
            }
            vm.save = function () {
                permissionService.add(vm.permission).success(function () {
                    notifyService.notify('保存成功！', 'success');
                    $("#modalPermissionEdit").modal("hide");
                });
            }
        }
    ]);
})();