(function (angular) {
    angular.module('DemoApp')
        .controller('AuthorsAdminController', authorsAdminController);

    authorsAdminController.$inject = ['Analytics', 'notificationService', 'axcesshttp', 'messageBoxService', 'darkfeatures', '$rootScope', '$scope', '$interval', 'newauthorService'];

    function authorsAdminController(Analytics, notificationService, axcesshttp, messageBoxService, darkfeatures, $rootScope, $scope, $interval, newauthorService) {

        Analytics.trackPage('/DemoApp/Admin/Authors', 'Book Club');

        var vm = this;

        vm.percentage = 0.10;

        vm.user = $rootScope.user;


        loadAuthors();
        validateDeleteFunction();

        vm.gridOptions = {
            enableSorting: true,
            enableRowHeaderSelection: false,
            multiSelect: false,
            columnDefs: [
                {name: 'Name', field: 'name'},
                {name: 'Book Count', field: 'bookcount'},
                {name: 'Description', field: 'description'}
            ]};

        vm.addAuthor = function () {
            newauthorService()
                .then(function(author) {
                    var config = {url: 'api/authors', data: author};

                    axcesshttp.post('api/authors', author, config).then(function (response) {
                        var newAuthor = response.data;
                        if (vm.search !== undefined) {
                            vm.search = newAuthor.name;
                        }
                        loadAuthors();
                        notificationService.success('Added ' + newAuthor.name);
                    }, function (response) {
                        notificationService.error('Error ocurred saving new author');
                    });
                })
                .catch(function() {
                });
        };

        vm.searchAuthor = function () {
            if (vm.search !== undefined) {
                loadAuthors();
            }
        };

        vm.clearSearch = function () {
            vm.search = undefined;
            loadAuthors();
        };

        vm.deleteAuthor = function () {
            if (vm.selectedRow !== undefined) {
                var author = vm.selectedRow.entity;
                var config = {url: 'api/authors/' + author.id};
                axcesshttp.delete('api/authors/' + author.id, config).then(function (resoonse) {
                    notificationService.success('Author deleted successfully');
                    loadAuthors();
                }, function (response) {
                    notificationService.error('Failed to delete author');
                });
            }
        };

        vm.updateAuthor = function () {
            if (vm.selectedRow !== undefined) {
                var author = vm.selectedRow.entity;
                newauthorService(copyAuthor(author)).then(function(updatedAuthor) {
                    var config = {url: 'api/authors/'};
                    axcesshttp.put('api/authors', updatedAuthor, config).then(function (response) { // success
                        loadAuthors();
                        notificationService.success('Author updated successfully');
                    }, function (response) { // failure
                        notificationService.error('Unable to update author');
                    });
                })
                .catch(function() {

                });
            }
        };

        function copyAuthor(author) {
            var newAuthor = {
                id: author.id,
                name: author.name,
                description: author.description,
                bookcount: author.bookcount,
                image: author.image
            };

            return newAuthor;
        }

        vm.showMessage = function () {
            messageBoxService.openOk('views/login.html', 'My message box');
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
                vm.selectedRow = undefined;
                vm.gridOptions.data = response.data;
                $interval(function () {vm.gridApi.selection.selectRow(vm.gridOptions.data[0]);}, 0, 1);
            }, function (response) {
                notificationService.error('Failed to load authors.');
            });
        }

        function validateDeleteFunction() {
            darkfeatures.getFirmFlag('delete', true).then(function (value) {
                vm.featureFlag = value;
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
