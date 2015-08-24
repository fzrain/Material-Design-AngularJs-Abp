(function () {
    materialAdmin.controller('auditController', [
       'ngTableParams', 'abp.services.app.auditLog',
        function (ngTableParams, auditService) {
            var vm = this;         
            vm.tableBasic = new ngTableParams({
                page: 1,
                count: 10
            }, {
                total:0,
                getData: function ($defer, params) {
                    auditService.getAuditLogs({
                        skipCount: (params.page() - 1) * params.count(), maxResultCount:params.count()
                    }).success(function (data) {                       
                        params.total(data.totalCount);
                        $defer.resolve(data.items);
                    });
                   
                }
            });          
            vm.getAuditDetail= function(id) {
                auditService.getDetail({ id: id }).success(function(data) {
                    vm.detailLog = data;
                });
            }
            
        }
    ]);
})();