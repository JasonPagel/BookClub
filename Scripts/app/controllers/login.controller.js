(function (angular) {

    angular.module('DemoApp')
        .controller('LoginController', loginController);

        loginController.$inject = ['Analytics', '$rootScope', '$scope', 'axcesshttp', 'notificationService'];

        function loginController(Analytics, $rootScope, $scope, axcesshttp, notificationService) {

            Analytics.trackPage('/DemoApp/Login', 'DemoApp');

            var vm = this;
            $rootScope.loggingIn = true;
            vm.cancel = $scope.$dismiss;

            vm.login = function () {
                var config = {url: 'api/login', data: vm.user};

                axcesshttp.post('api/login', vm.user, config).then(function (response) {
                    $rootScope.loggingIn = undefined;
                    $scope.$close(response.data);
                }, function (response) {
                    notificationService.error('login failed');
                    $rootScope.loggingIn = undefined;
                    $scope.$dismiss();
                });
            };

        }


})(angular);
