(function (angular) {

  angular.module('DemoApp')
      .service('userService', userService);

      userService.$inject = ['$rootScope'];

  function userService ($rootScope) {

      var service = {

        user: {
            name: '',
            loggedIn: false,
            id: ''
        },

        SaveState: function () {
            localStorage.userService = angular.toJson(service.user);
            console.log('savestate');
        },

        RestoreState: function () {
            console.log('restoring my state');
            console.log(localStorage);
            if (localStorage.userService !== undefined)
              service.user = angular.fromJson(localStorage.userService);
        }
      };

      $rootScope.$on("savestate", service.SaveState);
      $rootScope.$on("restorestate", service.RestoreState);

      return service;
  }

})(angular);
