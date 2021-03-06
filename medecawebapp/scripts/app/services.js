﻿'use strict';
var medecaURL = $("#baseURL").val();

angular.module('medecaApp')
	.factory('vehiculoMarcasFactory', ['$http', function ($http, baseURL) {
		var vehMarcFact = {};
		var vehMarcas = [{ id: 1, nombre: 'Toyota' }, { id: 1, nombre: 'Honda' }];

		vehMarcFact.getMarcas = function () {
			return $http.get(medecaURL + 'api/VehiculoMarcas');
		};

		vehMarcFact.get = function () {
			return $http.get(medecaURL + 'api/VehiculoMarcas');
		};

		vehMarcFact.agregar = function (marca) {
			return $http.post(medecaURL + 'api/VehiculoMarcas/', marca);
		};

		vehMarcFact.editar = function (marca) {
			return $http.put(medecaURL + 'api/VehiculoMarcas/', marca);
		};

		vehMarcFact.eliminar = function (model) {
			return $http.delete(medecaURL + 'api/VehiculoMarcas/' + '?id=' + model);
		};

		return vehMarcFact;
	}])
	.factory('citasFactory', ['$http', function ($http, baseURL) {
		var modelFact = {};
		var url = medecaURL + 'api/Citas';
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
			return $http.delete(url + '?id=' + model);
		};

		return modelFact;
	}])
	.factory('modelosFactory', ['$http', function ($http, baseURL) {
		var modelFact = {};
		var url = medecaURL + 'api/Modelos';
		modelFact.get = function () {
			return $http.get(url);
		};

		modelFact.agregar = function (model) {
			return $http.post(url, model);
		};

		modelFact.editar = function (model) {
			return $http.put(url, model);
		};

		modelFact.eliminar = function (model) {
			return $http.delete(url + '?id=' + model.IdModelo);
		};

		return modelFact;
	}])
		.factory('proveedoresFactory', ['$http', function ($http, baseURL) {
			var modelFact = {};
			var url = medecaURL + 'api/Proveedores';
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

			modelFact.eliminar = function (id) {
				return $http.delete(url + '?id=' + id);
			};

			return modelFact;
		}])
	.factory('reportesFactory', ['$http', function ($http, baseURL) {
		var modelFact = {};
		var url = medecaURL + 'api/Reportes';
		modelFact.getHist = function () {
			return $http.get(url + '/Historico');
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
		var url = medecaURL + 'api/Insumos';
		modelFact.get = function () {
			return $http.get(url);
		};

		modelFact.agregar = function (model) {
			return $http.post(url, model);
		};

		modelFact.editar = function (model) {
			return $http.put(url, model);
		};

		modelFact.eliminar = function (model) {
			return $http.delete(url + '?id=' + model.IdInsumo);
		};

		return modelFact;
	}])
	.factory('vehiculosFactory', ['$http', function ($http, baseURL) {
		var modelFact = {};
		var url = medecaURL + 'api/Vehiculos';
		modelFact.get = function () {
			return $http.get(url + '/GetVehiculos');
		};

		modelFact.agregar = function (model) {
			return $http.post(url, model);
		};

		modelFact.editar = function (model) {
			return $http.put(url, model);
		};

		modelFact.eliminar = function (model) {
			return $http.delete(url + '?id=' + model.IdVehiculo);
		};

		return modelFact;
	}])
	.factory('clientesFactory', ['$http', function ($http, baseURL) {
		var modelFact = {};
		var url = medecaURL + 'api/Clientes';
		modelFact.get = function () {
			return $http.get(url + '/Clientes');
		};

		modelFact.getWithVeh = function () {
			return $http.get(url + '/ClientesOrdenes');
        };

        modelFact.getAlertasRevisiones = function () {
            return $http.get(url + '/ProximasRevisiones');
        };

		modelFact.getVehiculos = function () {
			return $http.get(url + '/ClientesVehiculos');
        };

        modelFact.verificarEstadoSesion = function () {
            return $http.get(url + '/MantenerSesion');
        };

		modelFact.agregar = function (model) {
			return $http.post(url, model);
		};

		modelFact.editar = function (model) {
			return $http.put(url, model);
		};

		modelFact.eliminar = function (model) {
			return $http.delete(url + '?id=' + model.IDCliente);
		};

		return modelFact;
	}])
	.factory('serviciosFactory', ['$http', function ($http, baseURL) {
		var modelFact = {};
		var url = medecaURL + 'api/Servicios';
		modelFact.get = function () {
			return $http.get(url);
		};

		modelFact.agregar = function (model) {
			return $http.post(url, model);
		};

		modelFact.editar = function (model) {
			return $http.put(url, model);
		};

		modelFact.eliminar = function (id) {
			return $http.delete(url + '?id=' + id);
		};

		return modelFact;
	}])
	.factory('actividadesFactory', ['$http', function ($http, baseURL) {
		var modelFact = {};
		var url = medecaURL + 'api/Actividades';
		modelFact.get = function () {
			return $http.get(url);
		};

		modelFact.agregar = function (model) {
			return $http.post(url, model);
		};

		modelFact.editar = function (model) {
			return $http.put(url, model);
		};

		modelFact.eliminar = function (id) {
			return $http.delete(url + '?id=' + id);
		};

		return modelFact;
	}])
	.factory('insumosMarcasFactory', ['$http', function ($http, baseURL) {
		var modelFact = {};
		var url = medecaURL + 'api/InsumosMarcas';
		modelFact.get = function () {
			return $http.get(url);
		};

		modelFact.agregar = function (model) {
			return $http.post(url, model);
		};

		modelFact.editar = function (model) {
			return $http.put(url, model);
		};

		modelFact.eliminar = function (id) {
			return $http.delete(url + '?id=' + id);
		};

		return modelFact;
    }])
    .factory('usuariosFactory', ['$http', function ($http, baseURL) {
        var modelFact = {};
        var url = medecaURL + 'api/Autorizados';
        modelFact.get = function () {
            return $http.get(url);
        };

        modelFact.agregar = function (model) {
            return $http.post(url, model);
        };

        modelFact.editar = function (model) {
            return $http.put(url, model);
        };

        modelFact.eliminar = function (id) {
            return $http.delete(url + '?id=' + id);
        };

        return modelFact;
    }])
    .factory('alertasFactory', ['$http', function ($http, baseURL) {
        var modelFact = {};
        var url = medecaURL + 'api/AlertasProximosChequeos';
        modelFact.get = function () {
            return $http.get(url);
        };

        modelFact.agregar = function (model) {
            return $http.post(url, model);
        };

        modelFact.editar = function (alerta, idEstado) {
            return $http.put(url + '?id=' + alerta.IdAlerta+'&idEstado='+idEstado);
        };

        modelFact.eliminar = function (id) {
            return $http.delete(url + '?id=' + id);
        };

        return modelFact;
    }])
	.factory('dashboardFactory', ['$http', function ($http, baseURL) {
		var modelFact = {};
		var url = medecaURL + 'api/Dashboard';
		modelFact.get = function () {
			return $http.get(url + '/GetDashboard');
		};

		modelFact.getServsCont = function () {
			return $http.get(url + '/ServiciosContinuos');
		};

		modelFact.getProxServsCont = function () {
			return $http.get(url + '/ProximosCambiosAceite');
		};

		return modelFact;
	}])
	.factory('ordenesTrabajosFactory', ['$http', function ($http, baseURL) {
		var modelFact = {};
		var url = medecaURL + 'api/OrdenesTrabajos';
		modelFact.get = function () {
			return $http.get(url);
		};
		modelFact.obtenerDiagnosticosEstados = function () {
			return $http.get(url + '/ObtenerDiagnosticoEstados');
        };        
		modelFact.agregar = function (model) {
			return $http.post(url, model);
		};

		modelFact.editar = function (model) {
		    return $http.put(url + '/OrdenesTrabajo', model);
		};

		modelFact.aprobarDiagnostico = function (model) {
		    return $http.put(url + '/AprobarDiagnostico', model);
        };

        modelFact.obtenerOrdenesActivas = function () {
            return $http.get(url + '/OrdenesActivas');
        };

        modelFact.obtenerOrden = function (orden) {
            return $http.get(url + '/ClienteOrden?idOrden=' + orden);
        };

        modelFact.obtenerOrdenActiva = function (orden) {
            return $http.get(url + '/OrdenActiva?id=' + orden);
        };

		return modelFact;
	}]);