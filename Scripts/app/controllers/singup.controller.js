(function (angular) {

    angular
        .module('DemoApp')
        .controller('SignUpController', signupController);

    signupController.$inject = ['$scope', 'Analytics', 'axcesshttp'];

    function signupController($scope, Analytics, axcesshttp) {

        var vm = this;
        
        Analytics.trackPage('/DemoApp/Signup', 'DemoApp');

        vm.cancel = $scope.$dismiss;

        vm.signUp = function () {

            console.dir(vm.user);

            var config = {url: 'api/signup', data: vm.user};

            axcesshttp.post('api/signup', vm.user, config).then(function (response) {
                    $scope.$close();
                }, function (response) {
                    notificationService.error('Singup failed');
                    $scope.$dismiss();
                });
        };

    }

})(angular);