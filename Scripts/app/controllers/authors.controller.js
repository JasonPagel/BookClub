(function (angular) {

    angular
        .module('DemoApp')
        .controller('AuthorsController', authorsController);

    authorsController.$inject = ['Analytics', 'axcesshttp'];

    function authorsController(Analytics, axcesshttp) {

        Analytics.trackPage('/DemoApp/Authors', 'DemoApp');

        var vm = this;

        loadAuthors();

        vm.searchAuthors = function () {
            if (vm.search !== undefined) {
                loadAuthors();
            }
        };

        vm.clearSearch = function () {
            vm.search = undefined;
            loadAuthors();
        };

        function loadAuthors() {
            var url = 'api/authors';

            if (vm.search !== undefined & vm.search !== '') {
                url = url + '?search=' + vm.search;
            } else {
                vm.search = undefined;
            }
        
            var config = { url: url, method: 'GET'};

            axcesshttp.get(url, config).then(function (response) {
                vm.authors = response.data;
            }, function (response) {
                notificationService.error('Failed to load authors.');
            });
        }
        
    }

})(angular);