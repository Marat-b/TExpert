

appTexp.controller("Gontroller",['$scope', '$http', 'toastr', function ($scope, $http, toastr) {
        var isCreate = true;
        $http.get( "api/Malfunction").
            success(function(data) {
                $scope.typeOfMalfunction = data;
            }).
            error(function() { toastr.error("Не удалось считать данные", "Ошибка!"); });

        $scope.createMalfunction = function() {
            isCreate = true;
            $scope.titleAction = "Создать тип неисправности";
            $("#createMalfunction").modal("show");
        };

        $scope.btnSave = function(typeId, formMalfunction) {
            $("#createMalfunction").modal("hide");
            if (isCreate) {
                $http.post( "api/Malfunction", formMalfunction).
                    success(function(data) {
                        $scope.typeOfMalfunction = data;
                        toastr.success("Данные сохранены!", "");
                    }).
                    error(function() {
                        toastr.error("Не удалось записать данные!", "Ошибка");
                    });
            } else {
                $http.put( "api/Malfunction/" + typeId, formMalfunction).
                    success(function(data) {
                        $scope.typeOfMalfunction = data;
                        toastr.success("Данные сохранены!", "");
                    }).
                    error(function() {
                        toastr.error("Не удалось записать данные!", "Ошибка");
                    });

            }
        };

        /////////////////////////

        $scope.edit = function(typeId) {
            isCreate = false;
            $scope.titleAction = "Редактировать тип неисправности";
            $("#createMalfunction").modal("show");
            $http.get( "api/Malfunction/" + typeId).
                success(function(data, status) {
                        $scope.formMalfunction = data;
                        //toastr.success("Данные сохранены!", "");
                    }
                ).
                error(function() {
                        toastr.error("Не удалось записать данные!", "Ошибка");
                    }
                );
        };

        ///////////////////////

        $scope.delete = function(typeId, name) {
            if (confirm("Вы действительно хотите удалить запись - " + name + " ?")) {
                $http.delete( "api/Malfunction/" + typeId).
                    success(function(data, status) {
                            //$scope.formMalfunction = {};
                            $scope.typeOfMalfunction = data;
                            toastr.success("Данные удалены!", "");
                        }
                    ).
                    error(function() {
                            toastr.error("Не удалось удалить данные!", "Ошибка");
                        }
                    );
            }
        };
    }]);