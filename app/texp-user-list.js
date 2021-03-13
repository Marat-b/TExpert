var ModalDeleteUserCtrl = (function () {
    function ModalDeleteUserCtrl() {
    }
    return ModalDeleteUserCtrl;
}());
var userCtrl = (function () {
    function userCtrl($scope, $http, $uibModal, toastr) {
        var self = this;
        self.$scope = $scope;
        self.$http = $http;
        self.toastr = toastr;
        self.$uibModal = $uibModal;
        self.refreshData = self.Activate;
        self.$scope.editUser = function (userId) {
            var options = {
                templateUrl: 'Pages/EditUserModal.html',
                controller: ModalEditUserCtrl,
                resolve: {
                    userId: function () { return userId; }
                }
            };
            self.$uibModal.open(options).result.then(function () { self.refreshData(); }, function () { });
        };
        self.$scope.resetUserPassword = function (userId, userName, fio) {
            var options = {
                templateUrl: 'Pages/ResetPasswordModal.html',
                controller: ResetPasswordModalCtrl,
                resolve: {
                    userId: function () { return userId; },
                    userName: function () { return userName; },
                    fio: function () { return fio; }
                }
            };
            self.$uibModal.open(options).result.then(function () { self.refreshData(); });
        };
        self.$scope.deleteUser = function (userId) {
            var options = {
                templateUrl: 'Pages/DeleteUser.html',
                controller: ModalDeleteUserCtrl,
                resolve: {
                    userId: function () { return userId; }
                }
            };
            self.$uibModal.open(options).result.then(function () { self.refreshData(); });
        };
        self.Activate();
    }
    userCtrl.prototype.Activate = function () {
        var self = this;
        self.$http.get('api/UserList').success(function (data) {
            self.$scope.userList = data;
            console.log('UserList get -> data is fetched');
        }).error(function () {
            self.toastr.error('Ошибка получения списка пользователей!');
        });
    };
    return userCtrl;
}());
angular.module('TExpApp').controller('userCtrl', userCtrl);
userCtrl.$inject = ['$scope', '$http', '$uibModal', 'toastr'];
//# sourceMappingURL=texp-user-list.js.map