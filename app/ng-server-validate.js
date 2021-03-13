'use strict';
var ServerValidateService = (function () {
    function ServerValidateService() {
        this.watchingFieldNames = [];
    }
    ServerValidateService.prototype.clearServerError = function (form, fieldName) {
        if (form[fieldName]) {
            for (var errorFieldName in form[fieldName].$error) {
                if (form[fieldName].$error.hasOwnProperty(errorFieldName)) {
                    form[fieldName].$setValidity(errorFieldName, true);
                }
            }
            form[fieldName].$validate();
        }
        var fieldNameIndex = this.watchingFieldNames.indexOf(fieldName);
        if (fieldNameIndex > -1)
            this.watchingFieldNames.splice(fieldNameIndex, 1);
    };
    ;
    ServerValidateService.prototype.clearServerErrors = function (form) {
        for (var i = 0, l = this.watchingFieldNames.length; i < l; i++) {
            this.clearServerError(form, this.watchingFieldNames[i]);
        }
    };
    ;
    ServerValidateService.prototype.watchOnce = function (form, fieldName) {
        var self = this;
        this.watchingFieldNames.push(fieldName);
        form[fieldName].$viewChangeListeners.push(addChangeListener);
        function addChangeListener() {
            self.clearServerError(form, fieldName);
            var listenerIndex = form[fieldName].$viewChangeListeners.indexOf(addChangeListener);
            if (listenerIndex > -1)
                form[fieldName].$viewChangeListeners.splice(listenerIndex, 1);
        }
    };
    ;
    ServerValidateService.prototype.addError = function (form, fieldName, validationProperty) {
        if (form[fieldName]) {
            form[fieldName].$setValidity(validationProperty, false);
            this.watchOnce(form, fieldName);
        }
        else {
            console.log('error on serverValidateService.setServerValidity(): there is no input with name="' + fieldName + '" ng-model="[something]" in the form');
        }
    };
    ;
    return ServerValidateService;
}());
function serverValidateDirective(serverValidateService) {
    return {
        restrict: 'A',
        require: 'form',
        link: function ($scope, $elem, $attrs, $form) {
            var errorFormat = $attrs.serverValidate;
            $form.$serverErrors = {};
            $scope.$watch('modelState', function () {
                serverValidateService.clearServerErrors($form);
                var foundErrors = false;
                if ($scope.modelState) {
                    if (errorFormat && errorFormat == 'Microsoft.Owin') {
                        foundErrors = true;
                        var inputName = $scope.modelState.error;
                        var errorType = $scope.modelState.error_description;
                        if ($form[inputName]) {
                            $form[inputName].$dirty = true;
                            $form[inputName].$pristine = false;
                            $form[inputName].$setValidity(errorType, false);
                            serverValidateService.watchOnce($form, inputName);
                        }
                        else {
                            if (!$form.$serverErrors[inputName])
                                $form.$serverErrors[inputName] = {};
                            $form.$serverErrors[inputName][errorType] = true;
                            angular.element($elem).on('submit', clearGeneralServerErrors);
                        }
                    }
                    else {
                        for (var fieldName in $scope.modelState) {
                            if ($scope.modelState.hasOwnProperty(fieldName)) {
                                foundErrors = true;
                                if ($form[fieldName]) {
                                    $form[fieldName].$dirty = true;
                                    $form[fieldName].$pristine = false;
                                    for (var i = 0, l = $scope.modelState[fieldName].length; i < l; i++) {
                                        $form[fieldName].$setValidity($scope.modelState[fieldName][i], false);
                                    }
                                    serverValidateService.watchOnce($form, fieldName);
                                }
                                else {
                                    if (!$form.$serverErrors[fieldName])
                                        $form.$serverErrors[fieldName] = {};
                                    for (var j = 0, jl = $scope.modelState[fieldName].length; j < jl; j++) {
                                        $form.$serverErrors[fieldName][$scope.modelState[fieldName][j]] = true;
                                    }
                                    angular.element($elem).on('submit', clearGeneralServerErrors);
                                }
                            }
                        }
                    }
                }
                else {
                    clearGeneralServerErrors();
                }
                if (foundErrors)
                    $form.$setDirty();
            });
            function clearGeneralServerErrors() {
                $form.$serverErrors = {};
                angular.element($elem).off('submit', clearGeneralServerErrors);
            }
        }
    };
}
serverValidateDirective.$inject = ['serverValidateService'];
angular.module('server-validate', [])
    .directive('serverValidate', serverValidateDirective)
    .service('serverValidateService', ServerValidateService);
//# sourceMappingURL=ng-server-validate.js.map