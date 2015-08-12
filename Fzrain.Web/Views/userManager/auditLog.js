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
                vm.startPage = Math.floor((vm.pageIndex - 1) / 5) * 5 + 1;
              
                  abp.ui.setBusy(
                  null,
                  auditService.getAuditLogs({
                      skipCount: (pageIndex - 1) * vm.pageSize,
                  }).success(function (data) {
                      vm.auditLogs = data.items;
                      vm.total = data.totalCount;
                      vm.totalPage = Math.ceil(vm.total / 10);
                  })
              );
               
                  if (vm.pageSize * (vm.startPage+4)>= vm.total) {
                      var index = vm.startPage + 4 - Math.ceil(vm.total/10);
                      vm.pages = [];
                      for (var i = 0; i <= 4-index; i++) {
                          vm.pages.push(vm.startPage+i);
                      }
                      vm.showNextPager = false;
                  } else {
                      vm.pages = [vm.startPage, vm.startPage + 1, vm.startPage + 2, vm.startPage + 3, vm.startPage + 4];
                      vm.showNextPager = true;
                  }
                  
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