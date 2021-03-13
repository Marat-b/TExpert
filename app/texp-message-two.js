  

appTexp.controller("Gontroller", ['$scope', '$http', '$filter','toastr',function ($scope, $http, $filter,toastr) {

        $scope.message = {
            ListId: [],
            Item1: [],
            Item2: [],
            Item3: []
                
        };

           

        /////////////////// OnLoad Page \\\\\\\\\\\\\\\\\\

        $http.get( "api/MessageTwo").
            success(function (data) {
                $scope.message = angular.copy(data);
            }).
            error(function () {
                toastr.error("Ошибка при загрузке данных!","");
            });

        /////////////////// btnMessage \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        $scope.btnMessage = function () {

            if ($scope.message.ListId == undefined || $scope.message.ListId == 0) {
                $scope.message.ListId = 0;
                $http.post( "api/MessageTwo", $scope.message).
                success(function () {
                    toastr.success("Данные успешно сохранены!","");
                }).
                error(function () {
                    toastr.error("Ошибка при сохранении данных!");
                });
            }
            else {
                $http.put( "api/MessageTwo/" + $scope.message.ListId, $scope.message).
                success(function () {
                    toastr.success("Данные успешно сохранены!","");
                }).
                error(function () {
                    toastr.error("Ошибка при сохранении данных!","");
                });

            }
        }
    }]);