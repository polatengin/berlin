String.prototype.replaceAll = function(search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};

/* global StatusBar */
/* global cordova */
/* global angular */
angular.module('arcelik', ['ionic'])
  .run(function ($ionicPlatform) {
    $ionicPlatform.ready(function () {
      if (window.cordova && window.cordova.plugins.Keyboard) {

        cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);

        cordova.plugins.Keyboard.disableScroll(true);
      }
      if (window.StatusBar) {
        StatusBar.styleDefault();
      }
    });
  })
  .controller('index', function ($scope, $http, $location, $ionicScrollDelegate) {

        moment.locale('tr');

        $http({
            method: 'POST',
            url: 'http://arcelikcayirovayemekmenusu.azurewebsites.net/Home/Api'
        }).then(function (response) {
          $scope.moment = moment;
          $scope.list = response.data;

          $location.hash('menu-' + moment(new Date()).format('DD-MM-YYYY'));

          $ionicScrollDelegate.anchorScroll();
        });

        $scope.getMenuImageUrl = function(menu) {
          var imageName = menu
                            .toLowerCase()
                            .replaceAll('ı', 'i')
                            .replaceAll('ç', 'c')
                            .replaceAll('ş', 's')
                            .replaceAll('ü', 'u')
                            .replaceAll('ö', 'o')
                            .replaceAll('ğ', 'g')
                            .replaceAll(' ', '');

          return 'http://arcelikcayirovayemekmenusu.azurewebsites.net/' + imageName + '.png';
        };

  });
