namespace AuctionApp {

    angular.module('AuctionApp', ['ui.router', 'ngRoute', 'ngResource', 'ui.bootstrap', 'ngFileUpload']).config((
        $stateProvider: ng.ui.IStateProvider,
        $urlRouterProvider: ng.ui.IUrlRouterProvider,
        $locationProvider: ng.ILocationProvider
    ) => {
        // Define routes
        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/ngApp/views/home.html',
                controller: AuctionApp.Controllers.AuctionsController,
                controllerAs: 'controller'
            })
            .state('create', {
                url: '/addItem',
                templateUrl: '/ngApp/views/createAuctionItem.html',
                controller: AuctionApp.Controllers.AuctionsController,
                controllerAs: 'controller'
            })
            .state('update', {
                url: '/editItem/:id',
                //params: { id: -1 },
                templateUrl: '/ngApp/views/editAuctionItem.html',
                controller: AuctionApp.Controllers.AuctionsController,
                controllerAs: 'controller'
            })
            .state('delete', {
                url: '/deleteItem/:id',
                //params: { id: -1 },
                templateUrl: '/ngApp/views/deleteAuctionItem.html',
                controller: AuctionApp.Controllers.AuctionsController,
                controllerAs: 'controller'
            })
            .state('login', {
                url: '/login',
                templateUrl: '/ngApp/views/login.html',
                controller: AuctionApp.Controllers.LoginController,
                controllerAs: 'controller'
            })
            .state('register', {
                url: '/register',
                templateUrl: '/ngApp/views/register.html',
                controller: AuctionApp.Controllers.RegisterController,
                controllerAs: 'controller'
            })
            //.state('externalRegister', {
            //    url: '/externalRegister',
            //    templateUrl: '/ngApp/views/externalRegister.html',
            //    controller: AuctionApp.Controllers.ExternalRegisterController,
            //    controllerAs: 'controller'
            //}) 
            //.state('about', {
            //    url: '/about',
            //    templateUrl: '/ngApp/views/about.html',
            //    controller: AuctionApp.Controllers.AboutController,
            //    controllerAs: 'controller'
            //})
            .state('notFound', {
                url: '/notFound',
                templateUrl: '/ngApp/views/notFound.html'
            });

        // Handle request for non-existent route
        $urlRouterProvider.otherwise('/notFound');

        // Enable HTML5 navigation
        $locationProvider.html5Mode(true);

    });

    
    angular.module('AuctionApp').factory('authInterceptor', (
        $q: ng.IQService,
        $window: ng.IWindowService,
        $location: ng.ILocationService
    ) =>
        ({
            request: function (config) {
                config.headers = config.headers || {};
                config.headers['X-Requested-With'] = 'XMLHttpRequest';
                return config;
            },
            responseError: function (rejection) {
                if (rejection.status === 401 || rejection.status === 403) {
                    $location.path('/login');
                }
                return $q.reject(rejection);
            }
        })
    );

    angular.module('AuctionApp').config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptor');
    });

    

}
