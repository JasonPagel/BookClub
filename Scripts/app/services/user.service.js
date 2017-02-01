(function (angular) {

  angular.module('DemoApp')
      .service('userService', userService);

      userService.$inject = ['$rootScope'];

  function userService ($rootScope) {

      var service = {

        user: {
            name: ''
        },

        SaveState: function () {
            sessionStorage.userService = angular.toJson(service.user);
        },

        RestoreState: function () {
            service.user = angular.fromJson(sessionStorage.userService);
        }
      };

      $rootScope.$on("savestate", service.SaveState);
      $rootScope.$on("restorestate", service.RestoreState);

      return service;
  }

})(angular);
