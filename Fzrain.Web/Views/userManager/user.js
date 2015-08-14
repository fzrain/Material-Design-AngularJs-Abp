(function () {
    angular.module('materialAdmin').controller('userController', [
        'abp.services.app.user',
        function (userService) {
            var vm = this;
            vm.users = [];
            abp.ui.setBusy(
                null,
                userService.getUsers().success(function (data) {
                    vm.users = data.items;
                })
            );
        }
    ]);
})();