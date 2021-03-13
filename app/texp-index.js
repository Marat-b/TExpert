appTexp.directive("infoEquipment", function() {
    return {
        restrict: "E",
        template: '<div class="panel panel-info"><div class="panel-heading">{{Name}}</div><div class="panel-body">  <div><span>Инвентарный номер:</span><span>&nbsp;{{InventoryNumber}}</span></div>  <div><span>Серийный номер:</span><span>&nbsp;{{SerialNumber}}</span></div>  <div><span>Цена (руб):</span><span>&nbsp;{{MaxPrice}}</span></div><div><span>Дата ввода:</span><span>&nbsp;{{StartupDate | date: "dd.MM.yyyy"}}</span></div></div></div>'
    };
});

appTexp.controller("TExpContr", ['$scope', '$http', '$timeout','$uibModal', 'toastr', function ($scope, $http, $timeout, $uibModal, toastr) {
    $scope.itemsPerPage = 15;
    $scope.allItemEquipments = "";

    $scope.bigCurrentPage = 1;
    $scope.totalItemsInList = 0;
    var query = "";
    var predicate = 0;
    $scope.searchStuff = refreshData;

       

    function refreshData() {
        predicate = 0;
        query = "";
        if ($scope.checkName) {
            query += predicate == 0 ? "contains(toupper(Name),'" + $scope.modelName.toUpperCase() + "')" : " and contains(toupper(Name),'" + $scope.modelName.toUpperCase() + "')";
            predicate++;
        }
        
        if ($scope.checkSer) {
            query += predicate == 0 ? "contains(toupper(SerialNumber),'" + $scope.modelSer.toUpperCase() + "')" : " and contains(toupper(SerialNumber),'" + $scope.modelSer.toUpperCase() + "')";
            predicate++;
        }

        if ($scope.checkInv) {
            query += predicate == 0 ? "contains(toupper(InventoryNumber),'" + $scope.modelInv.toUpperCase() + "')" : " and contains(toupper(InventoryNumber),'" + $scope.modelInv.toUpperCase() + "')";
            predicate++;
        }

        if ($scope.checkMol) {
            query += predicate == 0 ? "contains(toupper(MOL),'" + $scope.modelMol.toUpperCase() + "')" : " and contains(toupper(MOL),'" + $scope.modelMol.toUpperCase() + "')";
            predicate++;
        }

        if (query.length != 0) {
            console.log("query=" + query);
            $http.get("odata/Equipments?$filter=" + encodeURI(query)).success(function(data) {
                $scope.allItemEquipments = data.value;
                if (data.value.length > 0) {
                    toastr.info("", "Найдено " + data.value.length.toString() + " записей!");
                } else {
                    toastr.warning("","Записи не найдены!");
                }
                $scope.bigTotalItems = $scope.allItemEquipments.length;
                console.log("$scope.bigTotalItems=" + $scope.bigTotalItems);
                var pages = Math.floor($scope.allItemEquipments.length / $scope.itemsPerPage);

                $scope.maxSize = pages < 11 ? pages : 10;
                clearSorting();

                doItems();

            }).error(function(error) {
                console.log(error);
                toastr.error("Не удалось считать данные", "Ошибка!");
            });
        } else {
            toastr.warning("Не выбраны параметры для поиска!", "");
        }
        
    }

    /////////////////////

    $scope.$watch("bigCurrentPage", function () {
        doItems();
    });

    function doItems() {
        var begin = (($scope.bigCurrentPage - 1) * $scope.itemsPerPage),
            end = begin + $scope.itemsPerPage;
        $scope.itemEquipments = $scope.allItemEquipments.slice(begin, end);
//        clearSorting();
    }

    //////////////// addToList \\\\\\\\\\\
    /*
    $scope.addToList = function (EquipmentId) {
        $http.put(siteRoot + "api/sessions/ListOfItems/" + EquipmentId).
        success(function (data) {
            $scope.totalItemsInList = data;
        }).error(function (error) {
            toastr.error("Ошибка при добавлении","");
        });
    } */

    ///////////////////// Typeahead \\\\\\\\\\\\\\\\\\\\\\
    var count = 0;
    $scope.searchFinder = "";
    $scope.typeaheadStuff = function (searchSelector, searchFinder) {

        return $http.get( "api/SearchEquipment/typeahead/" + searchSelector + "/" + encodeURIComponent(searchFinder) + "/10").
            then(function (response) {

                return response.data.map(function (item) {

                    return item.FindedStuff;
                });
            });

    }

    ////////////////////// EquipmentModal \\\\\\\\\\\\\\\\\\\\\\\\\\
    $scope.newItem = function () {
        var modalInstance = $uibModal.open({
            templateUrl:  "Pages/EquipmentModal.html",
            controller: TExpModalContr,
            resolve: {
                itemId: function () {
                    return undefined;
                },
                isCreate: function () { return true; }
            }
        });
        modalInstance.result.then(function() {
            refreshData($scope.searchSelector, $scope.searchFinder);
        }, function(){});
    };

    ////////////////////// EquipmentModal \\\\\\\\\\\\\\\\\\\\\\\\\\
    $scope.edit = function (EquipmentId) {
        var modalInstance = $uibModal.open({

            templateUrl:  "Pages/EquipmentModal.html",
            controller: TExpModalContr,
            resolve: {
                itemId: function () {
                    return EquipmentId;
                },
                isCreate: function () { return false; }
            }
        });
        modalInstance.result.then(function () {
            refreshData($scope.searchSelector, $scope.searchFinder);
        }, function () { });
    }

    ///////////////////// Wizard \\\\\\\\\\\\\\\\\\\\\
    /*
    $scope.startWizard = function (EquipmentId) {
        var modalWizard = $uibModal.open({
            templateUrl: siteRoot + "Pages/WizardModal.html",
            controller: WizardModalContr,
            size: "lg",
            resolve: {
                itemId: function () {
                    return EquipmentId;
                }

            }
        });
        modalWizard.result.then(function() {
            refreshData();
        }, function() {});
    } */

    /////////// Decomission \\\\\\\\\\\\\\\\

    $scope.decommission = function (EquipmentId) {
        var modalDecommission = $uibModal.open({
            templateUrl:  "Pages/DecommissionModal.html",
            controller: DecommissionModalContr,

            resolve: {
                itemId: function () {
                    return EquipmentId;
                }

            }
        });
        modalDecommission.result.then(function() {
            refreshData();
            //$scope.searchStuff(searchSelector, searchFinder); // = refreshData;
        }, function() {});
    }

    /////////// doList \\\\\\\\\\\\\\\\\\\\\\\\\\\\

    $scope.doList = function (totalItemsInList) {
        var modalList = $uibModal.open(
            {
                templateUrl:  "Pages/ListModal.html",
                controller: ListModalContr,
                size: "lg",
                resolve: {
                    itemsInList: function () {
                        return totalItemsInList;
                    }
                }
            });

        modalList.result.then(function (countItem) {
            $scope.totalItemsInList = countItem;
        }, function () { });
    }

    ///////////// List of expertises \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

    $scope.listExpertises=function(equipmentId,isReader) {
        var modlList = $uibModal.open(
        {
            templateUrl:  "Pages/ListExpertisesModal.html",
            controller: ExpertiseListModalCtrl,
            size: "lg",
            resolve: {
                equipmentId: function () { return equipmentId; },
                isReader: function () { return isReader; }
            }
        });
        modlList.result.then(function() {
            //$timeout(function () { return refreshData(); }, 5000);
            refreshData();
        }, function() {});
    }

    ////////////////// sorting by name of array member \\\\\\\\\\\\\\\\\\\\\\\\\\\\

    $scope.sorting = function (name, sorted) {
        console.log("sorting by " + name);
        console.log("sorted=" + sorted);
        $scope.allItemTransactions = sortByProperty($scope.allItemEquipments, name);
        doItems();
        clearSorting();
        //$scope.isSorted1 = 1;
        $scope[sorted] = 1;
    }

    ///////////////// clear all sorting flags \\\\\\\\\\\\\\\\\\\\\\\\

    var clearSorting = function () {
        $scope.isSorted1 = 0;
        $scope.isSorted2 = 0;
        $scope.isSorted3 = 0;
        $scope.isSorted4 = 0;
        $scope.isSorted5 = 0;
        $scope.isSorted6 = 0;
    }



}]);