angular.module('medecaApp').controller('ProveedoresController', ['$scope', 'actividadesFactory', 'proveedoresFactory', function ($scope, actividadesFactory, modelosFactory) {
    $scope.nuevoModel = { Activo: true, Actividades: [] };
    $scope.nuevaArea = {};
    $scope.areasTrabajo = [];

    if (!$scope.proveedores) {
        $scope.proveedores = [];
        modelosFactory.get().then(function (response) {
            $scope.proveedores = response.data;
        });
    }

    actividadesFactory.get().then(function (response) {
        $scope.areasTrabajo = response.data;
    });

    $scope.agregar = function () {
        modelosFactory.agregar($scope.nuevoModel).then(function (response) {
            $scope.proveedores.push(response.data);
            reset();
        });
    };

    $scope.agregarArea = function () {
        $scope.areasTrabajo.push($scope.nuevaArea);
        $scope.nuevaArea = {};
    };
    var areaSel = function (area) {
        for (var a in $scope.nuevoModel.Actividades) {
            if (area.IdActividad == $scope.nuevoModel.Actividades[a].IdActividad) {
                return true;
            }
        }

        return false;
    };

    $scope.areaSeleccionada = areaSel;

    $scope.editar = function (model) {
        $scope.editingModel = model;
        angular.merge($scope.nuevoModel, model);
        $scope.nuevoModel.editar = true;
    };

    $scope.cambioArea = function (area) {
        var nuevoModel = $scope.nuevoModel;

        if (areaSel(area)) {
            i = $scope.nuevoModel.Actividades.indexOf(area);
            $scope.nuevoModel.Actividades.splice(i, 1);
        } else {
            $scope.nuevoModel.Actividades.push(area);
        }
    };

    function reset() {
        $scope.agregarNuevaArea = false;
        $scope.proveedoresForm.$setPristine();
        $scope.nuevoModel = { Activo: true, Actividades: [] };
        $scope.uniquemsg = '';
        $scope.nuevaArea = {};
    }

    $scope.guardar = function (model) {
        modelosFactory.editar(model).then(function () {
            model.editar = false;
            angular.extend($scope.editingModel, $scope.nuevoModel);
            reset();
        });
    };

    $scope.eliminar = function () {
        modelosFactory.eliminar($scope.editingModel.IdProveedor).then(function () {
            var i = $scope.proveedores.indexOf($scope.editingModel);
            $scope.proveedores.splice(i, 1);
        });
    };

    $scope.restablecerNuevo = function () {
        reset();
    };
}])
;
