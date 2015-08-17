(function () {
    angular.module('materialAdmin').controller('auditController', [
       'ngTableParams', 'abp.services.app.auditLog',
        function (ngTableParams,auditService) {          
            var vm = this;
            vm.auditLogs = [];
            vm.pageSize = 10;
            vm.pageIndex = 1;         
            vm.loadData = function (pageIndex) {             
                  abp.ui.setBusy(
                  null,
                  auditService.getAuditLogs({
                      skipCount: (pageIndex - 1) * vm.pageSize
                  }).success(function (data) {
                      vm.auditLogs = data.items;
                      vm.total = data.totalCount;                    
                  })
              );
                             
            }
            vm.getAuditDetail= function(id) {
                auditService.getDetail({ id: id }).success(function(data) {
                    vm.detailLog = data;
                });
            }
            vm.loadData(1);
            this.tableBasic = new ngTableParams({
                page: vm.pageIndex, // show first page
                count: vm.pageSize // count per page
            }, {
                total: vm.total, // length of data
                getData: function($defer, params) {
                    $defer.resolve(vm.loadData(params.page()));
                }
              
            });
        
          
        }
    ]);
})();