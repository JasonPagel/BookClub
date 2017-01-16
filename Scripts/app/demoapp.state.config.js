(function (angular) {

    angular
        .module("DemoApp")
        .config(runRouteSetup);

        runRouteSetup.$inject = ['$stateProvider', '$urlRouterProvider'];

        function runRouteSetup($stateProvider, $urlRouterProvider) {


            var aboutState = {
                name: 'about',
                url: '/about',
                templateUrl: 'views/about.html',
                data: {
                    requireLogin: false
                }
            };

            var profileState = {
                name: 'profile',
                url: '/profile',
                templateUrl: 'views/profile.html',
                data: {
                    requireLogin: true
                }
            };

            var booksState = {
                name: 'books',
                url: '/books',
                templateUrl: 'views/books.html',
                controller: 'BooksController',
                controllerAs: 'ctrl',
                data: {
                    requireLogin: true
                }
            };

            var authorState = {
                name:'authors',
                url: '/authors',
                templateUrl: 'views/authors.html',
                controller: "AuthorsController",
                controllerAs: 'ctrl',
                data: {
                    requireLogin: true
                }
            };

            var adminState = {
                name: 'admin',
                abstract: true,
                url: '/admin',
                templateUrl: 'views/admin.html',
                data: {
                    requireLogin: true
                }
            };

            var authorAdminState = {
                name: 'admin.authors',
                url: '/authors',
                parent: 'admin',
                templateUrl: 'views/authoradmin.html',
                controller: 'AuthorsAdminController',
                controllerAs: 'ctrl',
                data: {
                    requireLogin: true
                }
            };

            var bookAdminState = {
                name: 'admin.books',
                url: '/books',
                parent: 'admin',
                templateUrl: 'views/books.admin.html',
                controller: 'BooksAdminController',
                controllerAs: 'ctrl',
                data: {
                    requireLogin: true
                }
            };
            
            var userAdminState = {
                name: 'admin.users',
                url: '/users',
                parent: 'admin',
                templateUrl: 'views/users.admin.html',
                data: {
                    requireLogin: true
                }
            };

            var welcomeState = {
                name: 'welcome',
                url: '/welcome',
                templateUrl: 'views/welcome.html',
                data: {
                    requireLogin: false
                }
            };

            $stateProvider.state(aboutState);
            $stateProvider.state(authorState);

            $stateProvider.state(adminState);
            $stateProvider.state(authorAdminState);
            $stateProvider.state(bookAdminState);
            $stateProvider.state(userAdminState);

            $stateProvider.state(booksState);
            $stateProvider.state(welcomeState);
            $stateProvider.state(profileState);
            
            $urlRouterProvider.otherwise('/welcome');

        }
})(angular);