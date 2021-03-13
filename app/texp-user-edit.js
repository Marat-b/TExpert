/// <reference path="../angularjs/angular.d.ts" />
/// <reference path="../angular-ui-bootstrap/angular-ui-bootstrap.d.ts"/>
/// <reference path="./angular-toastr.d.ts"/>
/// <reference path="../flowjs/flowjs.d.ts"/>
/// <reference path="../ng-flow/ng-flow.d.ts" />
'use strict';
var ModalEditUserCtrl = (function () {
    function ModalEditUserCtrl($scope, $http, $uibModalInstance, toastr, userId) {
        var _this = this;
        var vm = this;
        vm.$scope = $scope;
        vm.$http = $http;
        vm.toastr = toastr;
        vm.$uibModalInstance = $uibModalInstance;
        vm.userId = userId;
        //vm.siteRoot=siteRoot;
        vm.$scope.files = [];
        vm.$scope.fileProgress = 0;
        vm.$scope.modelState = [];
        vm.$scope.status = 1;
        if (vm.userId != 0) {
            vm.Init();
        }
        vm.$scope.ok = function (formEditUser) {
            //var vm=this;
            vm.$http.post('api/EditUser/' + vm.userId, formEditUser).success(function () {
                vm.toastr.success('Данные сохранены!');
                vm.$uibModalInstance.close();
            }).error(function () {
                vm.toastr.error('Данные не сохранены');
            });
        };
        vm.$scope.cancel = function () {
            vm.$uibModalInstance.dismiss();
        };
        // flow
        vm.$scope.getURL=function () {
            return "api/FileUpload/Upload/" + vm.userId;
        }


        vm.$scope.fileUploadSuccess = function (message) {
            console.log("fileUploadSuccess + message=" + message);
            if (message.replace(/\s/g, "") != "") {
                _this.$scope.files.push(JSON.parse(message));
            }
            else {
                console.log("fileUploadSuccess ->   message is empty");
            }
        };
        vm.$scope.fileUploadProgress = function (progress) {
            console.log("progress=" + progress);
            _this.$scope.fileProgress = Math.round(progress * 100); //+ "%";
        };
        vm.$scope.uploadError = function (message) {
            var vm = _this;
            vm.$scope.modelState = [];
            //$scope.formUpload.$serverErrors = [];
            //console.log("uploadError file=" + file[0]);
            console.log("uploadError message=" + message);
            if (message.replace(/\s/g, "") != "") {
                var jsonResponse = JSON.parse(message);
                console.log("jsonResponse=" + jsonResponse);
                console.log("jsonResponse.Message=" + jsonResponse.Message);
                console.log("jsonResponse.ModelState=" + jsonResponse.ModelState);
                console.log("jsonResponse.ModelState.file=" + jsonResponse.ModelState.file);
                var modelState = jsonResponse.ModelState;
                vm.$scope.modelState = modelState;
                //console.log("modelState=" + modelState.file);
                //$scope.modelState = { file: ['size'] };
                vm.$scope.status = 2;
            }
            else {
                console.log("uploadError message is empty");
            }
        };
        vm.$scope.validateFile = function ($file) {
            var vm = _this;
            vm.$scope.modelState = [];
            vm.$scope.formUpload.$serverErrors = [];
            var allowedExtensions = ['jpeg', 'jpg', 'bmp', 'png'];
            var isValidType = allowedExtensions.indexOf($file.getExtension()) >= 0;
            if (!isValidType) {
                console.log("Not valid file type");
                vm.$scope.modelState = { file: ['type'] };
                vm.$scope.status = 0;
            }
            else {
                console.log("Valid file type");
            }
            return isValidType;
        };
    }
    ModalEditUserCtrl.prototype.Init = function () {
        var vm = this;
        vm.$http.get('api/EditUser/' + vm.userId).success(function (data) {
            vm.$scope.formEditUser = data;
            console.log('editUser get -> data is fetched');
            vm.$http.get('api/EditUser/GetImage/' + vm.userId).success(function (data) {
                vm.$scope.pic = data;
                console.log('Image is fetched');
            }).error(function () {
                console.log('Image is NOT fetched!');
            });
        }).error(function () {
            vm.toastr.error('Ошибка получения данных пользователя');
        });
    };
    return ModalEditUserCtrl;
}());
angular.module('TExpApp').controller('ModalEditUserCtrl', ModalEditUserCtrl);
ModalEditUserCtrl.$inject = ['$scope', '$http', '$uibModalInstance', 'toastr', 'userId'];
//# sourceMappingURL=texp-user-edit.js.map