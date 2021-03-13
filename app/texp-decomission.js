var DecommissionModalContr = [
    '$scope', '$http', '$uibModalInstance', '$timeout', 'toastr', 'itemId', function($scope, $http, $uibModalInstance, $timeout, toastr, itemId) {


        $scope.dateDecommission = new Date();
        $scope.opened = false;
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        var today = new Date();

        $scope.formDecommission = {
            StateOfDecommission: false,
            DateOfDecommission: today
        };

        $http.get( "api/Decommission/" + itemId).
            success(function (data) {
                console.log("data.DateOfDecommission=" + data.DateOfDecommission);
                console.log("data.StateOfDecommission=" + data.StateOfDecommission);
                $scope.formDecommission.StateOfDecommission = data.StateOfDecommission;
                if (data.StateOfDecommission == true) {
                    $scope.formDecommission.DateOfDecommission = new Date(data.DateOfDecommission);
                } else {
                    $scope.formDecommission.DateOfDecommission = today;

                }
                

            }).error(function() {
                toastr.error("Ошибка при получении данных","");
            });


        $scope.open = function() {

            //$timeout(function() {
                $scope.opened = true;
            //});
        };


        $scope.cancel = function() {
            $uibModalInstance.dismiss("cancel");
        };

        $scope.ok = function(formDecommission) {

            $http.put( "api/Decommission/" + itemId, formDecommission).success(function(data, status) {
                toastr.success("Данные изменены!","");
            }).error(function(error) {
                toastr.error("Ошибка при изменении данных!","");
            });
            $uibModalInstance.close();
        }
    }
];