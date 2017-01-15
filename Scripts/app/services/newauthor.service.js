(function (angular) {

    angular
        .module('DemoApp')
        .service('newauthorService', newauthorService);

    newauthorService.$inject = ['$modal'];

    function newauthorService ($modal) {

        function addAuthor(author) {
            return author;
        }

        return function(author) {
            console.dir(author);
            var instance = $modal.open({
                templateUrl: 'views/newauthor.html',
                controller: 'NewAuthorController',
                controllerAs: 'ctrl',
                resolve: {
                    author: function() {
                        return author;
                    }
                }
            });

            return instance.result.then(addAuthor);
        };
    }

})(angular);