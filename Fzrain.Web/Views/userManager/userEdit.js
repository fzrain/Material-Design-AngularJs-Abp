(function () {
    angular.module('materialAdmin').controller('userEditController', [
        'abp.services.app.user',
        function (userService) {
            var vm = this;
            vm.activeTab = "user";
            vm.roleTab= function() {
                vm.activeTab = "role";
            }
            vm.userTab = function () {
                vm.activeTab = "user";
            }
        }
    ]);
})();