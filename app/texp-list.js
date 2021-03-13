var ListModalContr = ['$scope', '$http', '$uibModalInstance','toastr', 'itemsInList',
    function($scope, $http, $uibModalInstance,toastr, itemsInList) {

        var countItem;
        $http.get( "api/sessions/ListOfItems/0").
            success(function(data) {
                $scope.itemLists = data;
            }).error(function() {
                toastr.error("Ошибка при получении данных","");
            });

        $scope.deleteFromList = function(itemId) {
            $scope.showBtn = true;
            $http.delete( "api/sessions/ListOfItems/" + itemId).
                success(function(data) {
                    $scope.itemLists = data;
                }).error(function() { toastr.error("Ошибка при удалении",""); });


            $http.get( "api/sessions/CountOfItems/0").
                success(function(data) {
                    countItem = data;
                }).error(function() {
                    countItem = "0";
                });

        }


        $scope.ok = function() {
            $uibModalInstance.close(countItem);
        }

        $scope.cancel = function() {
            $uibModalInstance.dismiss("cancel");
        };
    }
];