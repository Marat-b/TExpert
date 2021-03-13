
appTexp.controller("Gontroller" ,['$scope', '$http', 'toastr', function ($scope, $http,toastr) {
    var isCreate = true;
    $scope.checkedItem = {
        equipmentFault: []
    };
    $http.get( "api/t_TypeOfEquipment").
    success(function (data, status, headers, config) {
        $scope.typeOfEquipment = data;
    }).
    error(function () {
        toastr.error("Ошибка загрузки данных!","");
    });

    $scope.createEquipment = function () {
        isCreate = true;
        $scope.titleAction = "Создать тип оборудования";
        $("#createEquipment").modal("show");
    };

    $scope.btnSave = function (typeId, formEquipment) {
        $("#createEquipment").modal("hide");
        if (isCreate) {
            $http.post( "api/t_TypeOfEquipment", formEquipment).
            success(function (data) {
                $scope.typeOfEquipment = data;
            }).
            error(function () {
                    toastr.error("Ошибка при попытке записи!", "");
                });
        }
        else
        {
            $http.put( "api/t_TypeOfEquipment/" + typeId, formEquipment).
            success(function (data) {
                $scope.typeOfEquipment = data;
            }).
            error(function (error) {
                toastr.error("Ошибка при редактировании!","");
            });

        }
    };

    /////////////////////////

    $scope.edit = function (typeId) {
        isCreate = false;
        $scope.titleAction = "Редактировать тип оборудования";
        $("#createEquipment").modal("show");
        $http.get("api/t_TypeOfEquipment/" + typeId).
        success(function (data) {
            $scope.formEquipment = data;
        }
        ).
        error(function () {
            toastr.error("Ошибка при редактировании!", "");
        }
        );
    };

    ///////////////////////

    $scope.delete = function (typeId,name) {
        if (confirm("Вы действительно хотите удалить запись - " + name +" ?"))
        {
            $http.delete( "api/t_TypeOfEquipment/" + typeId).
        success(function (data) {
            //$scope.formEquipment = {};
            $scope.typeOfEquipment = data;
            toastr.warning("Запись удалена!","Предупреждение")
        }
        ).
        error(function () {
            toastr.error("Ошибка при удалении!", "");
        }
        );
        }
    };

    ////////////////
    $scope.selectMalfunctions = function (typeId, name) {
        $scope.titleMalfunction = name;
        $("#selectMalfunction").modal("show");
        $http.get( "api/TypeOfEquipmentMalfunction/" + typeId).
        success(function (data) {
            $scope.formMalfunction.TypeId = data.TypeId;
            $scope.equipmentMalfunction = data.Malfunctions;
            $scope.checkedItem.equipmentFault=angular.copy(data.selectedMalfunctions);
        }).
        error(function (error) {
            toastr.error("Ошибка при выборе данных!", "");
        });
    };
    ///////////////
    $scope.btnSaveFault = function (typeId) {
        $http.put( "api/TypeOfEquipmentMalfunction/" + typeId, $scope.checkedItem.equipmentFault).
        success(function () {
            toastr.success("Данные сохранены!", "");
        }
        ).
        error(function () {
            toastr.error("Ошибка при получении данных!", "");
        }
        );
        $("#selectMalfunction").modal("hide");
    };

    ///////////////
    /*$scope.testFault = function () {
        $scope.checkedItem.equipmentFault.push(1);
    }*/
    ////////////////
}]);