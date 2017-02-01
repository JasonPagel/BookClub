(function (angular) {

    angular.module('DemoApp')
        .controller('LoginController', loginController);

        loginController.$inject = ['Analytics', '$rootScope', '$scope', 'axcesshttp', 'notificationService', '$cookies'];

        function loginController(Analytics, $rootScope, $scope, axcesshttp, notificationService, $cookies) {

            Analytics.trackPage('/DemoApp/Login', 'DemoApp');

            var vm = this;
            $rootScope.loggingIn = true;
            vm.cancel = $scope.$dismiss;

            vm.login = function () {
                var config = {url: 'api/login', data: vm.user};

                var cookie = $cookies.get('IsLoggedIn');
                if (cookie !== undefined) {
                  console.log('cookie: ' + cookie);
                  notificationService.info('cookie found');
                }

                axcesshttp.post('api/login', vm.user, config).then(function (response) {
                    $rootScope.loggingIn = undefined;
                    $cookies.put('IsLoggedIn', true);
                    $scope.$close(response.data);
                }, function (response) {
                    notificationService.error('login failed');
                    $rootScope.loggingIn = undefined;
                    $scope.$dismiss();
                });
            };

        }


})(angular);
