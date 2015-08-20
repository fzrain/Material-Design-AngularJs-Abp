(function () {
    angular.module('materialAdmin').controller('userController', [
        'abp.services.app.user','ngTableParams',
        function (userService, ngTableParams) {
            var vm = this;
            vm.users = [];
            vm.activeTab = "user";
            vm.roleTab = function () {
                vm.activeTab = "role";
            }
            vm.userTab = function () {
                vm.activeTab = "user";
            }
            vm.loadData = function (pageIndex) {
                userService.getUsers({
                    skipCount: (pageIndex - 1) * vm.pageSize
                }).success(function (data) {
                    vm.users = data.items;
                    vm.tableBasic = new ngTableParams({
                        page: vm.pageIndex, // show first page
                        count: vm.pageSize // count per page
                    }, {
                        total: data.totalCount, // length of data
                        getData: function () {
                            return vm.users;
                        }
                    });
                });
            }
            vm.getUserDetail = function (id) {
                userService.getDetail({ id: id }).success(function (data) {
                    vm.user = data;
                });
            }
            vm.loadData(1);
        }
    ]);
})();