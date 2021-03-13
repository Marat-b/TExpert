'use strict';
var DocumentCtrl = (function () {
    function DocumentCtrl($scope, $http) {
        var _this = this;
        var vm = this;
        vm.$scope = $scope;
        vm.$http = $http;
        vm.$scope.itemsPerPage = 15;
        vm.$scope.bigCurrentPage = 1;
        vm.$scope.totalItemsInList = 0;
        vm.$scope.maxSize = 10;
        vm.$scope.beginDate = new Date();
        vm.$scope.endDate = new Date();
        vm.$scope.openBDT = function () {
            _this.$scope.openedBDT = true;
        };
        vm.$scope.openEDT = function () {
            _this.$scope.openedEDT = true;
        };
        vm.$scope.getDocument = function () {
            vm.Activate();
        };
        vm.Activate();
        vm.$scope.$watch("bigCurrentPage", function () {
            console.log("watch is working!");
            vm.doItems();
        });
    }
    DocumentCtrl.prototype.Activate = function () {
        var vm = this;
        var query = "";
        query = "DateExp ge " + vm.cpDate(vm.$scope.beginDate).year + "-" + vm.cpDate(vm.$scope.beginDate).month + "-" + vm.cpDate(vm.$scope.beginDate).day + "T00:00:00Z";
        query += " and DateExp le " + vm.cpDate(vm.$scope.endDate).year + "-" + vm.cpDate(vm.$scope.endDate).month + "-" + vm.cpDate(vm.$scope.endDate).day + "T00:00:00Z";
        console.log(query);
        vm.$http.get('odata/Document?$filter=' + query).success(function (data) {
            console.log(data.value);
            vm.$scope.documentListAll = data.value;
            console.log('documentList get -> data is fetched');
            vm.$scope.bigTotalItems = vm.$scope.documentListAll.length;
            console.log("$scope.bigTotalItems=" + vm.$scope.bigTotalItems);
            var pages = Math.floor(vm.$scope.documentListAll.length / vm.$scope.itemsPerPage);
            vm.$scope.maxSize = pages < 11 ? pages : 10;
            vm.doItems();
        }).error(function () {
            console.log('Ошибка получения списка пользователей!');
        });
    };
    DocumentCtrl.prototype.doItems = function () {
        var vm = this;
        var begin = ((vm.$scope.bigCurrentPage - 1) * vm.$scope.itemsPerPage), end = begin + vm.$scope.itemsPerPage;
        if (vm.$scope.documentListAll !== undefined && vm.$scope.documentListAll !== null) {
            console.log('documentListAll not null!');
            vm.$scope.documentList = vm.$scope.documentListAll.slice(begin, end);
        }
    };
    DocumentCtrl.prototype.cpDate = function (value) {
        var date = new Date(value);
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var month2 = month.toString();
        if (month.toString().length == 1) {
            month2 = "0" + month.toString();
        }
        var day = date.getDate().toString();
        if (day.length == 1) {
            day = "0" + day;
        }
        return { day: day, month: month2, year: year };
    };
    return DocumentCtrl;
}());
angular.module('TExpApp').controller('DocumentCtrl', DocumentCtrl);
DocumentCtrl.$inject = ['$scope', '$http'];
//# sourceMappingURL=texp-document-list.js.map