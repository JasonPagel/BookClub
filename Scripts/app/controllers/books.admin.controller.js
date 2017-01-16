(function (angular) {
    angular.module('DemoApp')
        .controller('BooksAdminController', booksAdminController);

    booksAdminController.$inject = ['Analytics', 'notificationService', 'axcesshttp', 'messageBoxService', '$rootScope', '$scope', '$interval', 'newauthorService'];

    function booksAdminController(Analytics, notificationService, axcesshttp, messageBoxService, $rootScope, $scope, $interval, newauthorService) {

        Analytics.trackPage('/DemoApp/Admin/Books', 'Book Club');

        var vm = this;

        vm.user = $rootScope.user;

        loadBooks();

        vm.gridOptions = {
            enableSorting: true,
            enableRowHeaderSelection: false,
            multiSelect: false,
            columnDefs: [
                {name: 'Name', field: 'name'},
                {name: 'Author', field: 'authorname'},
                {name: 'Description', field: 'description'},
                {name: 'Pages', field: 'pages'},
                {name: 'Published', field: 'published'}
            ]};

        vm.addBook = function () {
            console.log('add book');
        };

        vm.searchBook = function () {
            if (vm.search !== undefined) {
                loadBooks();
            }
        };

        vm.clearSearch = function () {
            vm.search = undefined;
            loadBooks();
        };

        vm.deleteBook = function () {
            console.log('delete book');
        };

        vm.updateBook = function () {
            console.log('update book');
        };

        function copyBook(book) {
            var newBook = {
                id: book.id,
                name: book.name,
                description: book.description,
                authorname: book.authorname,
                authorid: book.authorid,
                image: book.image,
                pages: book.pages,
                published: book.published
            };

            return newBook;
        }

        function loadBooks() {
            var url = 'api/books';
            if (vm.search !== undefined & vm.search !== '') {
                url = url + '?search=' + vm.search;
            } else {
                vm.search = undefined;
            }
        
          var config = { url: url, method: 'GET'};

            axcesshttp.get(url, config).then(function (response) {
                vm.selectedRow = undefined;
                vm.gridOptions.data = response.data;
                $interval(function () {vm.gridApi.selection.selectRow(vm.gridOptions.data[0]);}, 0, 1);
            }, function (response) {
                notificationService.error('Failed to load books.');
            });
        }

        vm.gridOptions.onRegisterApi = function (gridApi) {
            vm.gridApi = gridApi;

            gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                if (row.isSelected) {
                    vm.selectedRow = row;
                } else {
                    vm.selectedRow = undefined;
                }
            });
        };

    }
})(angular);
