'use strict';
 
angular.module('medecaApp')
  .directive('ngUnique', ['$http', function ($http) {
      return {
          require: 'ngModel',
          link: function (scope, elem, attrs, ctrl) {
              elem.on('blur', function (evt) {
                  ctrl.$setValidity('unique', true);
                  scope.uniquemsg = '';
                  var thisVal = elem.val();

                  if (ctrl.$$parentForm[elem.attr('name')].$dirty && ctrl.$$parentForm[elem.attr('name')].$valid)
                      scope.$apply(function () {
                          $http({
                              method: 'GET',
                              url: attrs.ngUnique + '?field=' + thisVal + '&id=' + (attrs.ngModid || 0)
                          }).success(function (data, status, headers, config) {
                              ctrl.$setValidity('unique', data.Existe == 0);
                              scope.uniquemsg = data.Nombre;
                          });
                      });
              })
          }
      }
  }
  ])
     .directive('manageVehiculo', function () {
         return {
             restrict: 'E',
             scope: false,
             templateUrl: '/partials/manageVehiculo.html?V1.0.3'
         }
     })
       .directive('manageClient', function () {
           return {
               restrict: 'E',
               scope: false,
               templateUrl: '/partials/manageClient.html?V1.0.3'
           }
       })
          .directive('viewClient', function () {
              return {
                  restrict: 'E',
                  scope: false,
                  templateUrl: '/partials/viewClient.html?V1.0.3'
              }
          })
     .directive('viewVehicle', function () {
         return {
             restrict: 'E',
             scope: false,
             templateUrl: '/partials/viewVehicle.html?V1.0.3'
         }
     })
       .directive('viewOrder', function () {
           return {
               restrict: 'E',
               scope: false,
               templateUrl: '/partials/viewOrder.html?V1.0.3'
           }
       })
         .directive('manageModel', function () {
             return {
                 restrict: 'E',
                 scope: false,
                 templateUrl: '/partials/manageModel.html?V1.0.3'
             }
         })
      .directive('manageProvider', function () {
          return {
              restrict: 'E',
              scope: false,
              templateUrl: '/partials/manageProviders.html?V1.0.3'
          }
      })
          .directive('manageServices', function () {
              return {
                  restrict: 'E',
                  scope: false,
                  templateUrl: '/partials/manageServices.html?V1.0.3'
              }
          })
    .directive('loading', ['$http', function ($http) {
        return {
            restrict: 'A',
            link: function (scope, elm, attrs) {
                scope.isLoading = function () {
                    return $http.pendingRequests.length > 0;
                };

                scope.$watch(scope.isLoading, function (v) {
                    if (v) {
                        elm.show();
                    } else {
                        elm.hide();
                    }
                });
            }
        };

    }]);