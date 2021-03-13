////////////////// Controller TExpModalContr \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


var TExpModalContr = [
    '$scope', '$http', '$uibModalInstance', 'toastr', 'itemId', 'isCreate',
    function ($scope, $http, $uibModalInstance,toastr, itemId, isCreate) {

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        if (isCreate) {
            //$scope.formEquipment.Price = 0.0;
            $scope.ok = function(formEquipment) {
                $http.post( "api/equipment", formEquipment).success(function() {
                    toastr.success("Данные сохранены!","");
                }).error(function(error) {
                    toastr.error("Ошибка при сохранении данных!");
                }).then(function() {
                    $uibModalInstance.close();
                });
                //$uibModalInstance.close();
            }
        } else {


            $http.get( "api/Equipment/" + itemId).success(function(data) {
                $scope.formEquipment = data;
                $scope.formEquipment.StartupDate = new Date(data.StartupDate);
                console.log("data.StartupDate=" + data.StartupDate.toString());
            }).error(function() { toastr.error("Ошибка при загрузке данных!",""); });

            $scope.ok = function(formEquipment) {

                $http.put( "api/Equipment/" + itemId, formEquipment).success(function() {
                    toastr.success("Данные изменены!","");
                }).error(function() {
                    toastr.error("Ошибка при изменении данных!","");
                }).then(function() {
                    $uibModalInstance.close();
                });
                //$uibModalInstance.close();
            }


        }

        $scope.cancel = function() {
            $uibModalInstance.dismiss("cancel");
        };
        $scope.openDT = function($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.opened = true;
        };
    }
]; 