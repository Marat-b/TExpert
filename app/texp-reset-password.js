'use strict';
var ResetPasswordModalCtrl = (function () {
    function ResetPasswordModalCtrl($scope, $http, $uibModalInstance, toastr, userId, userName, fio) {
        var _this = this;
        var vm = this;
        vm.$scope = $scope;
        vm.$http = $http;
        vm.toastr = toastr;
        vm.$uibModalInstance = $uibModalInstance;
        vm.userId = userId;
        vm.$scope.userName = userName;
        vm.$scope.fio = fio;
        vm.$scope.ok = function (password) {
            var vm = _this;
            var passwordForm = { Password: password };
            vm.$http.post('api/ResetPassword/' + vm.userId, passwordForm).success(function () {
                vm.toastr.success('Данные сохранены!');
                vm.$uibModalInstance.close();
            }).error(function () {
                vm.toastr.error('Ошибка', 'Данные не сохранены');
            });
        };
        vm.$scope.cancel = function () {
            vm.$uibModalInstance.dismiss();
        };
    }
    return ResetPasswordModalCtrl;
}());
angular.module('TExpApp').controller('ResetPasswordModalCtrl', ResetPasswordModalCtrl);
ResetPasswordModalCtrl.$inject = ['$scope', '$http', '$uibModalInstance', 'toastr', 'userId', 'userName', 'fio'];
//# sourceMappingURL=texp-reset-password.js.map