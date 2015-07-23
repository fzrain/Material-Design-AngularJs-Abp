(function () {
    angular.module('materialAdmin').controller('roleController', [
        'abp.services.app.role',
        function (roleService) {
            var vm = this;
            vm.roles = [];
            abp.ui.setBusy(
                null,
                roleService.getRoles().success(function (data) {
                    vm.roles = data.items;
                })
            );
        }
    ]);
})();