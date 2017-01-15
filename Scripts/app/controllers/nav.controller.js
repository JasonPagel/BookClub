(function (angular) {

    angular
        .module('DemoApp')
        .controller('navController', navController);

    navController.$inject = ['$rootScope', 'darkfeatures', '$scope', '$state', 'axcesshttp', 'loginService', 'notificationService', 'signupService'];

    function navController($rootScope, darkfeatures, $scope, $state, axcesshttp, loginService, notificationService, signupService) {

        var vm = this;

        checkLogin();
        
        $scope.$watch(function() {
            return $rootScope.user;
        } , function() {
            checkLogin();
        }, true);

        function checkLogin() {
            if ($rootScope.user === undefined) {
                vm.bookFlag = undefined;
                vm.loggedIn = false;
                vm.username = undefined;
                return;
            }

            vm.loggedIn = true;
            vm.username = $rootScope.user.name;

            darkfeatures.getFirmFlag("book", false).then(function (flag) {
                vm.bookFlag = flag;
            });
        }

        vm.signUp = function () {
            signupService()
                .then(function() {

                })
                .catch(function() {
                    $state.go('welcome');
                });
        };

        vm.logIn = function () {
            loginService()
                .then(function() {
                })
                .catch(function() {
                    $state.go('welcome');
                });
        };

        vm.logOut = function () {
            var user = $rootScope.user;

            axcesshttp.delete('api/login/' + user.id).then(function (response) {
                $rootScope.user = undefined;
                $state.go('welcome');
            }, function (response) {
                $rootScope.user = undefined;
                notificationService.error('Logout failed');
                $state.go('welcome');
            });

        };

    }

})(angular);