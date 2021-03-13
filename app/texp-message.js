
$(document).ready(function () {
    $(".popovers").popover();
});

appTexp.controller("Gontroller",['$scope', '$http', '$filter', '$interval','toastr', function ($scope, $http, $filter, $interval,toastr) {

        $scope.message = {
            MessageId: [],
            Effective:[],
            Limit:[],
            Deadline:[],
            OverCostRepair:[],
            CostRepair: [],
            LastNumber:[]
        };

        //////////////////// Spinner routines - Down
        $scope.message.CostRepair = 0;
        $scope.ActionDown = null;
        $scope.ActionUp = null;

        $scope.spinnerDown = function (mouseAction) {
            $scope.isSpinnerDown = false;
                
            if (mouseAction == "down") {
                $scope.ActionDown = $interval(function () {

                    if ($scope.message.CostRepair == undefined || $scope.message.CostRepair > 0) {
                        $scope.message.CostRepair = parseInt($scope.message.CostRepair) - 1;

                    }
                    else {
                        $scope.isSpinnerDown = true;
                        $interval.cancel($scope.ActionDown);

                    }

                }, 50
                );
            }

            if (mouseAction == "up") {
                    
                $interval.cancel($scope.ActionDown);
                if ($scope.message.CostRepair < 100) {
                    $scope.isSpinnerUp = false;
                }

            }

        };
        //////////////////// Spinner routines - Up

        $scope.spinnerUp = function (mouseAction) {
            $scope.isSpinnerUp = false;
                
            if (mouseAction == "down") {
                $scope.ActionUp = $interval(function () {
                    if ($scope.message.CostRepair == undefined || $scope.message.CostRepair == '') {
                        $scope.message.CostRepair = 0;
                    }
                    if ($scope.message.CostRepair < 100) {
                        $scope.message.CostRepair = parseInt($scope.message.CostRepair) + 1;
                    }
                    else {
                        $scope.isSpinnerUp = true;
                        $interval.cancel($scope.ActionUp);
                    }
                }, 50);
            }
            if (mouseAction == "up") {
                    
                $interval.cancel($scope.ActionUp);
                if ($scope.message.CostRepair > 0) {
                    $scope.isSpinnerDown = false;
                }

            }
        };

        /////////////////// OnLoad Page \\\\\\\\\\\\\\\\\\

        $http.get( "api/Message").
            success(function (data) {
                $scope.message = angular.copy(data);
            }).
            error(function () {
                toastr.error("Ошибка при загрузке данных!","");
            });

        /////////////////// btnMessage \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        $scope.btnMessage = function () {
                
            if ($scope.message.MessageId==undefined || $scope.message.MessageId==0)
            {
                $scope.message.MessageId = 0;
                $http.post( "api/Message", $scope.message).
                success(function () {
                    toastr.success("Данные успешно сохранены!","");
                }).
                error(function () {
                    toastr.error("Ошибка при сохранении данных!","");
                });
            }
            else
            {
                $http.put( "api/Message/" + $scope.message.MessageId, $scope.message).
                success(function () {
                    toastr.success("Данные успешно сохранены!","");
                }).
                error(function () {
                    toastr.error("Ошибка при сохранении данных!","");
                });
                        
            }
        }
    }]);