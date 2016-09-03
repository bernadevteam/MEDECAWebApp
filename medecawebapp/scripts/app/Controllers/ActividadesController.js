angular.module('medecaApp')
.controller('ActividadesController', ['$scope', 'actividadesFactory', function ($scope, modelosFactory) {
    $scope.modelos = {};
    $scope.nuevoModel = {};
    $scope.deleteActividad = {};

    modelosFactory.get().then(function (response) {
        $scope.modelos = response.data;
    });

    $scope.agregar = function () {
        modelosFactory.agregar($scope.nuevoModel).then(function (response) {
            $scope.modelos.push(response.data);
            $scope.nuevoModel = {};
        });
    };
    $scope.eliminar = function () {
        modelosFactory.eliminar($scope.deleteActividad.Id).then(function () {
            var i = $scope.modelos.indexOf($scope.deleteActividad);
            $scope.modelos.splice(i, 1);
            $scope.deleteActividad = {};
        });
    };

    $scope.editar = function (model) {
        model.editar = true;
        angular.extend($scope.nuevoModel, model);
    };

    $scope.eliminaractividad = function (model) {
        $scope.deleteActividad = model;
    };
    $scope.cancelarEdicion = function (model) {
        model.editar = false;
        $scope.nuevoModel = {};
    };

    $scope.guardar = function (model) {
        modelosFactory.editar($scope.nuevoModel).then(function () {
            angular.extend(model, $scope.nuevoModel);
            model.editar = false;
            $scope.nuevoModel = {};
        });
    };
}]);
