(function () {
    angular.module('materialAdmin').controller('auditController', [
       'ngTableParams', 'abp.services.app.auditLog',
        function (ngTableParams, auditService) {
            var vm = this;
            vm.auditLogs = [];
            vm.pageSize = 10;
            vm.pageIndex = 1;
           
            vm.loadData = function (pageIndex) {
                auditService.getAuditLogs({
                    skipCount: (pageIndex - 1) * vm.pageSize
                }).success(function(data) {
                    vm.auditLogs = data.items;
                    vm.total = data.totalCount;
                    vm.tableBasic = new ngTableParams({
                        page: vm.pageIndex, // show first page
                        count: vm.pageSize // count per page
                    }, {
                        total: vm.total, // length of data
                        getData: function() {
                            return vm.auditLogs;
                        }                    
                    });
                });
            }
            vm.getAuditDetail= function(id) {
                auditService.getDetail({ id: id }).success(function(data) {
                    vm.detailLog = data;
                });
            }
            vm.loadData(1);         
        }
    ]);
})();