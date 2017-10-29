'use strict';
var medecaURL = $("#baseURL").val();

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
             templateUrl: medecaURL + 'partials/manageVehiculo.html?V1.1.1'
         }
     })
       .directive('manageClient', function () {
           return {
               restrict: 'E',
               scope: false,
               templateUrl: medecaURL + 'partials/manageClient.html?V1.0.3'
           }
       })
          .directive('viewClient', function () {
              return {
                  restrict: 'E',
                  scope: false,
                  templateUrl: medecaURL + 'partials/viewClient.html?V1.2.1'
              }
          })
     .directive('viewVehicle', function () {
         return {
             restrict: 'E',
             scope: false,
             templateUrl: medecaURL + 'partials/viewVehicle.html?V1.2.1'
         }
     })
       .directive('viewOrder', function () {
           return {
               restrict: 'E',
               scope: false,
               templateUrl: medecaURL + 'partials/viewOrder.html?V1.2.1'
           }
       })
         .directive('manageModel', function () {
             return {
                 restrict: 'E',
                 scope: false,
                 templateUrl: medecaURL + 'partials/manageModel.html?V1.0.3'
             }
         })
      .directive('manageProvider', function () {
          return {
              restrict: 'E',
              scope: false,
              templateUrl: medecaURL + 'partials/manageProviders.html?V1.2.1'
          }
      })
     .directive('confirmDelete', function () {
         return {
             restrict: 'EA',
             scope: {
                 objectname: '@elementname',
                 objectelement: '=objectelement',
                 'eliminar':'&'
             },
             controller: function ($scope) {
                 $scope.deleteEl = function () {
                     $scope.delete()();
                 };
             },
             templateUrl: medecaURL + 'partials/confirmDelete.html?V1.0.4'
         }
     })
          .directive('manageServices', function () {
              return {
                  restrict: 'E',
                  scope: false,
                  templateUrl: medecaURL + 'partials/manageServices.html?V1.0.1'
              }
          })
    .directive('manageOrder', function () {
        return {
            restrict: 'E',
            scope: false,
            templateUrl: medecaURL + 'partials/manageOrder.html?V1.2.1'
        }
    })
    .directive('manageUsers', function () {
        return {
            restrict: 'E',
            scope: false,
            templateUrl: medecaURL + 'partials/manageUsers.html?V1.2.1'
        }
    })
    .directive('viewAlertasRevisiones', function () {
        return {
            restrict: 'E',
            scope: false,
            templateUrl: medecaURL + 'partials/viewAlertasRevisiones.html?V1.2.1'
        }
    })
          .directive('diagnosticoTpl', function () {
              return {
                  restrict: 'E',
                  scope: true,
                  link: function (scope, element, attrs) {
                      scope.Estado = attrs.idestado;
                      scope.EsDetalle = attrs.esdetalle;
                  },
                  templateUrl: medecaURL + 'partials/tplDiagnostico.html?V1.2.1'
              }
          })
      .directive('insumosCotizadosTpl', function () {
          return {
              restrict: 'E',
              templateUrl: medecaURL + 'partials/tpl/InsumosCotizados.html?V1.1.1'
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