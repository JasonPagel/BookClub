(function (angular) {

    angular
        .module('DemoApp')
        .component('authorView', authorView());

    function authorView() {
        return {
            templateUrl: 'views/authordetail.html',
            bindings: { author: '<' },
            controller: function authorController() {
                var vm = this;
            },
            controllerAs: 'ctrl'
        };
    }

})(angular);