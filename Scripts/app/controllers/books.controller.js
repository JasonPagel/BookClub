(function(angular) {

    angular.module('DemoApp')
        .controller('BooksController', booksController);

        booksController.$inject = ['Analytics', 'axcesshttp', 'notificationService'];

        function booksController(Analytics, axcesshttp, notificationService) {

            Analytics.trackPage('/DemoApp/Books', 'DemoApp');

            var vm = this;
            loadBooks();

            vm.searchBooks = function () {
                if (vm.search !== undefined) {
                    loadBooks();
                }
            };

            vm.clearSearch = function () {
                vm.search = undefined;
                loadBooks();
            };

            function loadBooks()
            {
                var url = "api/books";

                if (vm.search !== undefined & vm.search !== '') {
                    url = url + '?search=' + vm.search;
                } else {
                    vm.search = undefined;
                }

                axcesshttp.get(url).then(function (response) {
                    var data = response.data;
                    for (var i = 0, l = data.length; i<l; i++ ) {
                        data[i].slideid = i;
                    }
                    vm.books = response.data;
                }, function (reponse) {
                    notificationService.error('An error ocurred getting books.');
                });
            }
        }

})(angular);