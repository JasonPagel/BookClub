(function (angular) {

    angular
        .module('DemoApp')
        .controller('NewAuthorController', newauthorController);

    newauthorController.$inject = ['Analytics', '$scope', 'axcesshttp', 'notificationService', 'author'];

    function newauthorController(Analytics, $scope, axcesshttp, notificationService, author) {

        Analytics.trackPage('/DemoApp/NewAuthor', 'DemoApp');

        var vm = this;
        vm.author = author;
        vm.cancel = $scope.$dismiss;

        vm.addAuthor = function () {
        
                $scope.$close(vm.author);


        };
    }

})(angular);