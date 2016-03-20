'use strict';

angular.module('medecaApp')
	.factory('vehiculoMarcasFactory', ['$http', function ($http, baseURL) {
	    var vehMarcFact = {};
	    var vehMarcas = [{ id: 1, nombre: 'Toyota' }, { id: 1, nombre: 'Honda' }];

	    vehMarcFact.getMarcas = function () {
	        return $http.get('/api/VehiculoMarcas');
	    };

	    vehMarcFact.get = function () {
	        return $http.get('/api/VehiculoMarcas');
	    };

	    vehMarcFact.agregar = function (marca) {
	        return $http.post('/api/VehiculoMarcas/', marca);
	    };

	    vehMarcFact.editar = function (marca) {
	        return $http.put('/api/VehiculoMarcas/', marca);
	    };
	    return vehMarcFact;
	}])
    .factory('citasFactory', ['$http', function ($http, baseURL) {
        var modelFact = {};
        var url = '/api/Citas';
        modelFact.get = function () {
            return $http.get(url + '/Citas');
        };

        modelFact.getCitasdeHoy = function () {
            return $http.get(url + '/CitasdeHoy');
        };

        modelFact.getClientes = function () {
            return $http.get(url + '/Clientes');
        };
        modelFact.agregar = function (model) {
            return $http.post(url, model);
        };

        modelFact.editar = function (model) {
            return $http.put(url, model);
        };

        modelFact.eliminar = function (model) {
            return $http.delete(url+'?id='+model);
        };

        return modelFact;
    }])
    .factory('modelosFactory', ['$http', function ($http, baseURL) {
    var modelFact = {};
    var url = '/api/Modelos';
    modelFact.get = function () {
        return $http.get(url);
    };

    modelFact.agregar = function (model) {
        return $http.post(url, model);
    };

    modelFact.editar = function (model) {
        return $http.put(url, model);
    };

    return modelFact;
    }])
        .factory('proveedoresFactory', ['$http', function ($http, baseURL) {
            var modelFact = {};
            var url = '/api/Proveedores';
            modelFact.get = function () {
                return $http.get(url + '/GetProveedores');
            };
            modelFact.getActivos = function () {
                return $http.get(url + '/GetProveedoresActivos');
            };
            modelFact.agregar = function (model) {
                return $http.post(url, model);
            };

            modelFact.editar = function (model) {
                return $http.put(url, model);
            };

            return modelFact;
        }])
    .factory('reportesFactory', ['$http', function ($http, baseURL) {
        var modelFact = {};
        var url = '/api/Reportes';
        modelFact.getHist = function () {
            return $http.get(url+'/Historico');
        };

        modelFact.getHistCont = function () {
            return $http.get(url + '/HistoricoConteo');
        };

        modelFact.getHistMesesCont = function () {
            return $http.get(url + '/HistoricoMesesConteo');
        };

        return modelFact;
    }])
    .factory('insumosFactory', ['$http', function ($http, baseURL) {
    var modelFact = {};
    var url = '/api/Insumos';
    modelFact.get = function () {
        return $http.get(url);
    };

    modelFact.agregar = function (model) {
        return $http.post(url, model);
    };

    modelFact.editar = function (model) {
        return $http.put(url, model);
    };

    return modelFact;
    }])
    .factory('vehiculosFactory', ['$http', function ($http, baseURL) {
        var modelFact = {};
        var url = '/api/Vehiculos';
        modelFact.get = function () {
            return $http.get(url + '/GetVehiculos');
        };

        modelFact.agregar = function (model) {
            return $http.post(url, model);
        };

        modelFact.editar = function (model) {
            return $http.put(url, model);
        };

        return modelFact;
    }])
    .factory('clientesFactory', ['$http', function ($http, baseURL)  {
        var modelFact = {};
        var url = '/api/Clientes';
        modelFact.get = function () {
            return $http.get(url+'/Clientes');
        };

        modelFact.getWithVeh = function () {
            return $http.get(url + '/ClientesOrdenes');
        };

        modelFact.getVehiculos = function () {
            return $http.get(url + '/ClientesVehiculos');
        };

        modelFact.agregar = function (model) {
            return $http.post(url, model);
        };

        modelFact.editar = function (model) {
            return $http.put(url, model);
        };

        return modelFact;
    }])
    .factory('serviciosFactory', ['$http', function ($http, baseURL) {
        var modelFact = {};
        var url = '/api/Servicios';
        modelFact.get = function () {
            return $http.get(url);
        };

        modelFact.agregar = function (model) {
            return $http.post(url, model);
        };

        modelFact.editar = function (model) {
            return $http.put(url, model);
        };

        return modelFact;
    }])
    .factory('dashboardFactory', ['$http', function ($http, baseURL) {
        var modelFact = {};
        var url = '/api/Dashboard';
        modelFact.get = function () {
            return $http.get(url + '/GetDashboard');
        };

        modelFact.getServsCont = function () {
            return $http.get(url+'/ServiciosContinuos');
        };

        modelFact.getProxServsCont = function () {
            return $http.get(url + '/ProximosCambiosAceite');
        };
       
        return modelFact;
    }])
    .factory('ordenesTrabajosFactory', ['$http', function ($http, baseURL) {
        var modelFact = {};
        var url = '/api/OrdenesTrabajos';
        modelFact.get = function () {
            return $http.get(url);
        };

        modelFact.agregar = function (model) {
            return $http.post(url, model);
        };

        modelFact.editar = function (model) {
            return $http.put(url, model);
        };

        return modelFact;
    }]);