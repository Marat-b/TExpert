
var WizardModalContr = [
    '$scope', '$http', '$filter', '$interval', '$timeout', '$uibModalInstance', 'toastr', 'itemId', 'expertiseId',
    function($scope, $http, $filter, $interval, $timeout, $uibModalInstance, toastr, itemId, expertiseId) {
        $scope.showStep2 = false;
        var equipmentId = itemId;

        var Equipment;
        var selectedTypeOfEquipment;
        var typeOfEquipmentSelected;
        var foundTypeOfEquipment;
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        $scope.showStepOne = true;
        $scope.showStepTwo = false;
        $scope.showStepThree = false;
        $scope.showStepFour = false;

        $scope.checkedItem = {
            equipmentFault: []
        };

        $scope.expertise = {
        };
        var nowDate = new Date();
        //$scope.btnStepTwoNext = true;

        $scope.Price = 0;


        /////// parseDate \\\\\\\\\\\\\\\\
        function parseDate(value) {
            var tmp = value.substring(0, 10).split("-");
            return new Date(tmp[0], tmp[1], tmp[2]);
        }

        // There we count only the years
        function datediff(first, second) {
            return (second - first); // / (1000 * 60 * 60 * 24 * 365);
        }


        //////////////// currentPeriod \\\\\\\\\\\\
        function currentPeriod(amountYear, foundItem) {
            ////console.log("amountYear=" + amountYear);
            if (amountYear < foundItem.Warranty) {
                //$scope.btnStepTwoNext = true;
                $("#LifeTimeAlert").removeClass().addClass("alert alert-info");
            }
            if (foundItem.Warranty <= amountYear && amountYear < +foundItem.Warranty + +foundItem.Effective) {
                //$scope.btnStepTwoNext = true;
                $("#LifeTimeAlert").removeClass().addClass("alert alert-success");

            }
            if (+foundItem.Warranty + +foundItem.Effective <= amountYear &&
                amountYear < +foundItem.Warranty + +foundItem.Effective + +foundItem.Limit) {
                //$scope.btnStepTwoNext = false;
                $('div.setup-panel div a[href="#step-3"]').attr("disabled", "disabled");
                $("#LifeTimeAlert").removeClass().addClass("alert alert-warning");

            }
            if (+foundItem.Warranty + +foundItem.Effective + +foundItem.Limit <= amountYear) {
                //$scope.btnStepTwoNext = false;
                $('div.setup-panel div a[href="#step-3"]').attr("disabled", "disabled");
                $("#LifeTimeAlert").removeClass().addClass("alert alert-danger");

            }

        };

        ////////////// StartUp \\\\\\\\\\\\\\\\\\\\\\\
        // Получение типа оборудования
        $http.get("api/WizardStepOne").success(function(data) {
            $scope.typeOfEquipment = data;
        }).error(function() { toastr.error("Ошибка приполучении данных!", ""); });

        // получение экземпляра оборудования по ИД
        $http.get("api/WizardStepOne/" + equipmentId).success(function(data) {
            if (data.selectTypeOfEquipment != null) {

                selectedTypeOfEquipment = data.selectTypeOfEquipment;
            }
            Equipment = data.equipment;
            $scope.Name = data.equipment.Name;
            $scope.StartupDate = data.equipment.StartupDate;
            $scope.MaxPrice = data.equipment.Price;
            $scope.InventoryNumber = data.equipment.InventoryNumber;
            $scope.SerialNumber = data.equipment.SerialNumber;

            console.log(parseDate(Equipment.StartupDate).getFullYear());
            console.log(nowDate.getFullYear());
            //$scope.Lifetime = datediff(parseDate(Equipment.StartupDate).getFullYear(), parseDate($filter("date")(nowDate, "yyyy-MM-dd")).getFullYear());
            // There we count only the years
            $scope.Lifetime = datediff(parseDate(Equipment.StartupDate).getFullYear(), nowDate.getFullYear());

            if (data.selectTypeOfEquipment != null) {

                $scope.typeOfEquipmentSelected = selectedTypeOfEquipment.TypeId;


                foundTypeOfEquipment = $filter("filter")($scope.typeOfEquipment,
                    function(t) { return t.TypeId === $scope.typeOfEquipmentSelected; })[0];
                $scope.Warranty = foundTypeOfEquipment.Warranty;
                $scope.Effective = foundTypeOfEquipment.Effective;
                $scope.Limit = foundTypeOfEquipment.Limit;
                currentPeriod($scope.Lifetime, foundTypeOfEquipment);
            }


        }).error(function() { toastr.error("Ошибка при получении данных!", ""); });

        $scope.selectTypeOfEquipment = function() {

            foundTypeOfEquipment = $filter("filter")($scope.typeOfEquipment,
                function(t) { return t.TypeId === $scope.typeOfEquipmentSelected; })[0];


            $scope.Warranty = foundTypeOfEquipment.Warranty;
            $scope.Effective = foundTypeOfEquipment.Effective;
            $scope.Limit = foundTypeOfEquipment.Limit;
            currentPeriod($scope.Lifetime, foundTypeOfEquipment);
        }

        ///////// navListItems.click \\\\\\\\\\\

        $scope.hrefClick = function(ev) {
            ev.preventDefault();
            ev.stopPropagation();

            var $item = angular.element(ev.currentTarget);

            angular.element(document.querySelectorAll(".btn-circle")).removeClass('btn-primary')
                .addClass('btn-default');
            if (ev.target.id == "hrefStep1") {
                $scope.showStepOne = true;
                $scope.showStepTwo = false;
                $scope.showStepThree = false;
                $scope.showStepFour = false;
            }
            if (ev.target.id == "hrefStep2") {
                $scope.showStepOne = false;
                $scope.showStepTwo = true;
                $scope.showStepThree = false;
                $scope.showStepFour = false;
            }
            if (ev.target.id == "hrefStep3") {
                $scope.showStepOne = false;
                $scope.showStepTwo = false;
                $scope.showStepThree = true;
                $scope.showStepFour = false;
            }
            if (ev.target.id == "hrefStep4") {
                $scope.showStepOne = false;
                $scope.showStepTwo = false;
                $scope.showStepThree = false;
                $scope.showStepFour = true;
            }

            $item.addClass('btn-primary');

        };

        ///////  StepOne \\\\\\\\\\\\\\\\

        $scope.stepOne = function() {

            typeOfEquipmentSelected = $scope.typeOfEquipmentSelected;
            $http.put("api/WizardStepOne/" + equipmentId + "/" + $scope.typeOfEquipmentSelected).error(function() {
                toastr.error("Ошибка при попытке ввода типа оборудования !", "");
            });

            $http.get("api/WizardStepTwo/" + $scope.typeOfEquipmentSelected + "/" + equipmentId).success(
                function(data) {
                    $scope.equipmentMalfunction = data.Malfunctions;

                    $scope.checkedItem.equipmentFault = angular.copy(data.selectedMalfunctions);

                }).error(function() {
                toastr.error("Ошибка при получении данных!", "");
            });

            currentPeriod($scope.Lifetime, foundTypeOfEquipment);

            angular.element(document.querySelector("#hrefStep1")).removeClass("btn-primary").addClass("btn-default");
            angular.element(document.querySelector("#hrefStep2")).removeAttr("disabled").removeClass("btn-default")
                .addClass("btn-primary");

            $scope.showStepOne = false;
            $scope.showStepTwo = true;

        };


        ///////  StepTwo \\\\\\\\\\\\\\\\\\

        $scope.stepTwo = function () {

            $http.put("api/WizardStepTwo/" + equipmentId, $scope.checkedItem.equipmentFault).success(function () {

            }
            ).error(function () {
                toastr.error("Ошибка при получении данных!", "");
            }
            ).then(function () {


                angular.element(document.querySelector("#hrefStep2")).removeClass("btn-primary").addClass("btn-default");
                angular.element(document.querySelector("#hrefStep4")).removeAttr("disabled").removeClass("btn-default")
                    .addClass("btn-primary");

                $scope.showStepTwo = false;
                $scope.showStepFour = true;
                //$timeout(function() {
                $http.get("api/WizardStepFour/" + equipmentId + "/" + expertiseId).success(function (data) {

                    if (data.ExpertiseId != null) {

                        $scope.expertise = angular.copy(data);
                        $scope.expertise.DateExp = new Date(data.DateExp);
                        //$scope.expertise.DateExp = moment(data.DateExp);
                        //$scope.expertise.NumberExp = 1 + +data.NumberExp;
                        console.log("data.DateExp=" + data.DateExp);

                    } else {
                        $scope.expertise.ExpertiseId = 0;
                        $http.get("api/Message").success(function (data) {
                            $scope.expertise.NumberExp = 1 + +data.NumberExp;;
                        }).error(function () {
                            toastr.error("Ошибка при загрузке данных по номуру акта!", "");
                        });
                    }
                });
            });
            //}, 3000);
            // } 
            /*
            $scope.showStepTwo = false;
            $scope.showStepThree = true;*/
        };

        /////////////////// StepThree \\\\\\\\\\\\\\\\\\\\\\\\\\\\
        /*
        $scope.stepThree = function() {
            
            $http.put(siteRoot + "api/WizardStepThree/" + equipmentId + "/" + $scope.Price).
                success(function() {

                }).error(function() {

                });

            $http.get(siteRoot + "api/WizardStepFour/" + equipmentId).
                success(function(data) {

                    if (data.EquipmentId != null) {

                        $scope.expertise = angular.copy(data);
                        $scope.expertise.DateExp = new Date(data.DateExp);
                        console.log("data.DateExp=" + data.DateExp);
                        console.log("get  data.IsServiceableEquipment=" + data.IsServiceableEquipment);
                        console.log("get  $scope.expertise.IsServiceableEquipment=" + $scope.expertise.IsServiceableEquipment);

                    } else {
                        $scope.expertise.EquipmentId = 0;
                    }
                });

            angular.element(document.querySelector("#hrefStep3")).removeClass("btn-primary").addClass("btn-default");
            angular.element(document.querySelector("#hrefStep4")).removeAttr("disabled").removeClass("btn-default").addClass("btn-primary");

            $scope.showStepThree = false;
            $scope.showStepFour = true;
        };*/

        //////////////// StepFour

        $scope.stepFour = function() {

            $http.put("api/WizardStepFour/" + equipmentId, $scope.expertise).success(function() {
                console.log("$scope.expertise.DateExp=" + $scope.expertise.DateExp);
                console.log("$scope.expertise.NumberExp=" + $scope.expertise.NumberExp);
                console.log("put  $scope.expertise.IsServiceableEquipment=" + $scope.expertise.IsServiceableEquipment);
                toastr.success("Данные успешно сохранены!", "");
            }).error(function() {
                toastr.error("Ошибка при сохранении данных!", "");
            }).then(function() {
                $uibModalInstance.close();
            });
            //$uibModalInstance.close();
        }


        //////////////////// Spinner routines - Down
        /*
        $scope.ActionDown = null;
        $scope.ActionUp = null;

        $scope.spinnerDown = function(mouseAction) {
            $scope.isSpinnerDown = false;

            if (mouseAction == "down") {
                $scope.ActionDown = $interval(function() {

                        if ($scope.Price == undefined || $scope.Price > 0) {
                            $scope.Price = parseInt($scope.Price) - 1;

                        } else {
                            $scope.isSpinnerDown = true;
                            $interval.cancel($scope.ActionDown);

                        }

                    }, 50
                );
            }

            if (mouseAction == "up") {

                $interval.cancel($scope.ActionDown);
                if ($scope.Price < $scope.MaxPrice) {
                    $scope.isSpinnerUp = false;
                }

            }

        };*/
        //////////////////// Spinner routines - Up
        /*
        $scope.spinnerUp = function(mouseAction) {
            $scope.isSpinnerUp = false;

            if (mouseAction == "down") {
                $scope.ActionUp = $interval(function() {
                    if ($scope.Price == undefined || $scope.Price == '') {
                        $scope.Price = 0;
                    }
                    if ($scope.Price <= $scope.MaxPrice) {
                        $scope.Price = parseInt($scope.Price) + 1;
                    } else {
                        $scope.isSpinnerUp = true;
                        $interval.cancel($scope.ActionUp);
                    }
                }, 50);
            }
            if (mouseAction == "up") {

                $interval.cancel($scope.ActionUp);
                if ($scope.Price > 0) {
                    $scope.isSpinnerDown = false;
                }

            }
        };*/
        ///////////////////////////////////////////////
        $scope.cancel = function() {
            $uibModalInstance.dismiss("cancel");
        };

        /////////////////openDateExp \\\\\\\\\\\\\\\\\\\\\\\
        $scope.openDateExp = function() {
            $scope.openedDateExp = true;
        }
    }
];