(function () {
    materialAdmin.controller('auditController', [
       'NgTableParams', 'abp.services.app.auditLog',
        function (NgTableParams, auditService) {
            var vm = this;
            vm.permission = {
                detail: abp.auth.hasPermission('Administration.AuditLog.Detail')
            };
            vm.tableBasic = new NgTableParams({
                page: 1,
                count: 10
            }, {
                total:0,
                getData: function ($defer, params) {
                    auditService.getAuditLogs({
                        skipCount: (params.page() - 1) * params.count(), maxResultCount:params.count(),filter:params.filter()
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