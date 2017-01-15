(function (angular) {

    angular
        .module('DemoApp')
        .component('bookView', bookView());

    function bookView() {
        return {
            templateUrl: 'views/bookdetail.html',
            bindings: { book: '<' },
            controller: function bookController() {
                var vm = this;

                vm.bookVote = function () {
                    console.log('voted for: ' + vm.book.name);
                };
            },
            controllerAs: 'ctrl'
        };
    }

})(angular);