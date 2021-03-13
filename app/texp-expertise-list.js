'use strict';
var ExpertiseListModalCtrl = (function () {
    function ExpertiseListModalCtrl($scope, $http, $timeout, $uibModalInstance, $uibModal, toastr, equipmentId, isReader) {
        var vm = this;
        vm.$scope = $scope;
        vm.$http = $http;
        vm.$timeout = $timeout;
        vm.toastr = toastr;
        vm.$uibModalInstance = $uibModalInstance;
        vm.$uibModal = $uibModal;
        vm.equipmentId = equipmentId;
        vm.$scope.isReader = isReader
        console.log("vm.$scope.isReader=" + vm.$scope.isReader);
        
        vm.$scope.startWizard = function (expertiseId) {
            var options = {
                templateUrl: 'Pages/WizardModal.html',
                controller: WizardModalContr,
                size: 'lg',
                resolve: {
                    itemId: function () { return vm.equipmentId; },
                    expertiseId: function () { return expertiseId; }
                }
            };
            var  wizardModal=   vm.$uibModal.open(options);

            wizardModal.result.then(function () {
                //vm.$timeout(function () { return vm.Init(); }, 5000);
                vm.Init();
            }, function () { });
        };
        vm.Init();
        vm.$scope.ok = function () {
            vm.$uibModalInstance.close();
        };
    }
    ExpertiseListModalCtrl.prototype.Init = function () {
        var vm = this;
        vm.$http.get("api/ExpertiseList/" + vm.equipmentId).success(function (data) {
            vm.$scope.itemExpertises = data;
            vm.toastr.success("Данные считаны");
        }).error(function () {
            vm.toastr.error("Не удалось считать данные", "Ошибка!");
        });
    };
    return ExpertiseListModalCtrl;
}());
angular.module('TExpApp').controller('ExpertiseListModalCtrl', ExpertiseListModalCtrl);
ExpertiseListModalCtrl.$inject = ['$scope', '$http', '$timeout', '$uibModalInstance', '$uibModal', 'toastr', 'equipmentId','isReader'];
//# sourceMappingURL=texp-expertise-list.js.map