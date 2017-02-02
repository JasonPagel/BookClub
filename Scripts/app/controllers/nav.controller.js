(function (angular) {

    angular
        .module('DemoApp')
        .controller('navController', navController);

    navController.$inject = ['$rootScope', 'darkfeatures', '$scope', '$state', 'axcesshttp', 'loginService', 'notificationService', 'signupService', 'userService'];

    function navController($rootScope, darkfeatures, $scope, $state, axcesshttp, loginService, notificationService, signupService, userService) {

        var vm = this;

        checkLogin();

        $scope.$watch(function() {
            return $rootScope.user;
        } , function() {
            checkLogin();
        }, true);

        function checkLogin() {
          console.log('checklogin');
          console.log(userService);

            if (userService.user === undefined || userService.user.loggedIn !== true ) {
                vm.bookFlag = undefined;
                vm.username = undefined;
                vm.loggedIn = false;
                return;
            }

            vm.loggedIn = userService.user.loggedIn;
            vm.username = userService.user.name;

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
                userService.user.loggedIn = false;
                $rootScope.$broadcast('savestate');
                $rootScope.user = undefined;
                $state.go('welcome');
            }, function (response) {
                userService.user.loggedIn = false;
                $rootScope.$broadcast('savestate');
                $rootScope.user = undefined;
                notificationService.error('Logout failed');
                $state.go('welcome');
            });

        };

    }

})(angular);
