angular.module('medecaApp')
.controller('InsumosMarcasController', ['$scope', 'insumosMarcasFactory', function ($scope, modelosFactory) {
    $scope.modelos = {};
    $scope.nuevoModel = {};
    $scope.deleteInsumoMarca = {};
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
        modelosFactory.eliminar($scope.deleteInsumoMarca.Id).then(function () {
            var i = $scope.modelos.indexOf($scope.deleteInsumoMarca);
            $scope.modelos.splice(i, 1);
            $scope.deleteInsumoMarca = {};
        });
    };

    $scope.editar = function (model) {
        model.editar = true;
        angular.extend($scope.nuevoModel, model);
    };

    $scope.eliminarInsumoMarca = function (model) {
        $scope.deleteInsumoMarca = model;
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
