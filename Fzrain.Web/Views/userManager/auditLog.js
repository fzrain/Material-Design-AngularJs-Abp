(function () {
    angular.module('materialAdmin').controller('auditController', [
        'abp.services.app.auditLog',
        function (auditService) {
            var vm = this;
            vm.auditLogs = [];
            vm.pageSize = 10;
            vm.pageIndex = 1;
            vm.loadData = function (pageIndex) {
                    vm.pageIndex = pageIndex;
                  abp.ui.setBusy(
                  null,
                  auditService.getAuditLogs({
                      skipCount: (pageIndex - 1) * vm.pageSize,
                  }).success(function (data) {
                      vm.auditLogs = data.items;
                      vm.total = data.totalCount;
                  })
              );
            }
            vm.loadData(1);

        }
    ]);
})();