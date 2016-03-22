'use strict';

angular.module('medecaApp')
    .controller('NavBarController', ['$scope', 'citasFactory', function ($scope, citasFact) {
        $scope.citasdehoy = {};

        citasFact.getCitasdeHoy().then(function (response) {
            $scope.citasdehoy = response.data;
        });

        $scope.$on('manejarcambiocita', function (event, args) {
            citasFact.getCitasdeHoy().then(function (response) {
                $scope.citasdehoy = response.data;
            });
        });

    }])
    .controller('DashboardController', ['$scope', 'dashboardFactory', 'citasFactory', function ($scope, modelosFactory, citasFact) {
        $scope.modelos = {};
        $scope.citas = {};
        $scope.serviciosContinuos = {};
        $scope.serviciosContinuosProx = {};
        $scope.pivotservs = {};
        $scope.selPanel = {};
        $scope.currentPage = 1;
        $scope.nuevoModel = {};
        $scope.mostrarPendientes = false;

        modelosFactory.get().then(function (response) {
            $scope.modelos = response.data;
        });

        modelosFactory.getServsCont().then(function (response) {
            $scope.serviciosContinuos = response.data;
        });

        modelosFactory.getProxServsCont().then(function (response) {
            $scope.serviciosContinuosProx = response.data;
        });

        citasFact.getCitasdeHoy().then(function (response) {
            $scope.citas = response.data;
        });

        $scope.agregar = function () {
            modelosFactory.agregar($scope.nuevoModel).then(function (response) {
                $scope.modelos.push(response.data);
                $scope.nuevoModel = {};
            });
        };

        $scope.editar = function (model) {
            model.editar = true;
        };

        $scope.mostrarServPendientes = function (i, panel) {
            $scope.selPanel = panel;
            switch (i) {
                case 0:
                    $scope.pivotservs = $scope.serviciosContinuosProx;
                    break;
                case 1:
                    $scope.pivotservs = $scope.serviciosContinuos;
                    break;
                case 2:
                    $scope.pivotservs = $scope.citas;
                    break;
            }
        };

        $scope.cancelarEdicion = function (model) {
            model.editar = false;
        };

        $scope.guardar = function (model) {

            modelosFactory.editar(model).then(function () {
                model.editar = false;
            });
        };
    }])
	.controller('VehiculoMarcasController', ['$scope', 'vehiculoMarcasFactory', function ($scope, modelosFactory) {
	    $scope.marcas = {};
	    $scope.nuevoModel = {};
	    $scope.editingModel = {};

	    modelosFactory.getMarcas().then(function (response) {
	        $scope.marcas = response.data;
	    });

	    $scope.agregar = function () {
	        modelosFactory.agregar($scope.nuevoModel).then(function (response) {
	            $scope.marcas.push(response.data);
	            $scope.nuevoModel = {};
	        });
	    };

	    $scope.editar = function (model) {
	        $scope.editingModel.editar = false;
	        $scope.editingModel = model;
	        $scope.editingModel.editar = true;
	        angular.extend($scope.nuevoModel, model);
	    };

	    $scope.cancelarEdicion = function (model) {
	        model.editar = false;
	        $scope.nuevoModel = {};
	    };

	    $scope.guardar = function (model) {
	        modelosFactory.editar($scope.nuevoModel).then(function () {
	            angular.extend(model, $scope.nuevoModel);
	            $scope.editingModel.editar = false;
	            $scope.editingModel = {};
	            $scope.nuevoModel = {};
	        });
	    };
	}])
    .controller('ReportesController', ['$scope', 'reportesFactory', function ($scope, modelosFactory) {
        $scope.historicos = {};
        $scope.currentPage = 1;
        $scope.pageSize = "15";
        $scope.verHist = "0";

            modelosFactory.getHist().then(function (response) {
                $scope.historicos = response.data;
            });
            modelosFactory.getHistMesesCont().then(function (response) {
                
                var js = JSON.stringify(response.data);
                $scope.historicosMesConteo = JSON.parse(js);
            });
            modelosFactory.getHistCont().then(function (response) {
                
                var js = JSON.stringify(response.data);

                $scope.historicosConteo = JSON.parse(js);
            });
           
        }])
    .controller('InsumosController', ['$scope', 'insumosFactory', function ($scope, modelosFactory) {
        $scope.modelos = {};
        $scope.nuevoModel = {};
        $scope.deleteInsumo = {};

        modelosFactory.get().then(function (response) {
            $scope.modelos = response.data;
        });

        $scope.agregar = function () {
            modelosFactory.agregar($scope.nuevoModel).then(function (response) {
                $scope.modelos.push(response.data);
                $scope.nuevoModel = {};
            });
        };
        $scope.eliminarInsumo = function (model) {
            $scope.deleteInsumo = model;
        };

        $scope.eliminar = function () {
            modelosFactory.eliminar($scope.deleteInsumo).then(function () {
                var i = $scope.modelos.indexOf($scope.deleteInsumo);
                $scope.modelos.splice(i,1);
            });
        };

        $scope.editar = function (model) {
            model.editar = true;
            angular.extend($scope.nuevoModel, model);
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
    }])
    .controller('CitasController', ['$scope', '$mdDialog', '$filter', 'citasFactory', 'clientesFactory', function ($scope, $mdDialog, $filter, modelosFactory, clientesFact) {
        $scope.modelos = {};
        $scope.clientes = [];
        $scope.searchText = null;
        $scope.nuevoModel = {};
        $scope.calendarView = 'month';
        $scope.calendarDate = new Date();
        $scope.events = [
  
        ];
        modelosFactory.getClientes().then(function (response) {
            $scope.clientes = response.data;
            modelosFactory.get().then(function (response) {
                var dat = response.data;

                for (var i in dat) {
                    var event = dat[i];
                    event.Cliente = $filter('getByKey')($scope.clientes, 'IDCliente', event.IdCliente);
                    event.Vehiculo = $filter('getByKey')(event.Cliente.Vehiculos, 'IdVehiculo', event.IdVehiculo);
                    parseDate(event);
                    $scope.events.push(event);
                }
            });

        });

       $scope.agregar = function () {
            $scope.nuevoModel.IdVehiculo = $scope.nuevoModel.Vehiculo.IdVehiculo;
            
            modelosFactory.agregar($scope.nuevoModel).then(function (response) {
                $scope.nuevoModel.IdCita = response.data.IdCita;
                parseDate($scope.nuevoModel);
                $scope.events.push($scope.nuevoModel);
                $scope.$emit('cambiocita', { cita: $scope.nuevoModel });
                $scope.nuevoModel = {};
            });
        };

        $scope.editar = function (model) {
            $scope.editingModel = model;
            angular.extend($scope.nuevoModel, model);
            $scope.nuevoModel.editar = true;
        };
        $scope.eliminar = function (model) {
            modelosFactory.eliminar(model.IdCita).then(function (response) {
                var i = $scope.events.indexOf(model);
                $scope.events.splice(i, 1);
            });
        };

        $scope.guardar = function () {
            $scope.nuevoModel.IdVehiculo = $scope.nuevoModel.Vehiculo.IdVehiculo;

            modelosFactory.editar($scope.nuevoModel).then(function () {
                $scope.nuevoModel.editar = false;
                parseDate($scope.nuevoModel);
                angular.extend($scope.editingModel, $scope.nuevoModel);
                $scope.$emit('cambiocita', { cita: $scope.nuevoModel });
                $scope.nuevoModel = {};
            });
        };

        $scope.querySearch = function (query) {
            var results = query ? $scope.clientes.filter(createFilterFor(query)) : [];
            return results;
        }

        function createFilterFor(query) {
            var lowercaseQuery = angular.lowercase(query);

            return function filterFn(cliente) {
                return (cliente.Nombre.toLowerCase().indexOf(lowercaseQuery) != -1);
            };
        }
      
        $scope.restablecerNuevo = function () {
            restablecer();
        };

        function restablecer() {
            $scope.nuevoModel = {};
        }

        $scope.cancelarEdicion = function (model) {
            model.editar = false;
        };

        function parseDate(nMod) {
            if (typeof nMod.Fecha == 'string') {
                nMod.Fecha = new Date(nMod.Fecha.split("T")[0].replace(/[^a-zA-Z0-9]/g, '/'));
            }
            var ndDate = new Date(nMod.Fecha.toDateString() + " " + nMod.Hora);
            nMod.title = nMod.Cliente.Nombre + ", " + nMod.Vehiculo.Modelo.VehiculoMarca.Nombre + " " + nMod.Vehiculo.Modelo.Nombre + " Año " + nMod.Vehiculo.Anio,
            nMod.type = 'info';
            nMod.startsAt = ndDate;
        }

    }])
     .controller('ProveedoresController', ['$scope', 'proveedoresFactory', function ($scope, modelosFactory) {
         $scope.modelos = {};
         $scope.nuevoModel = {};

         modelosFactory.get().then(function (response) {
             $scope.modelos = response.data;
         });

         $scope.agregar = function () {
             modelosFactory.agregar($scope.nuevoModel).then(function (response) {
                 $scope.modelos.push(response.data);
                 reset();
             });
         };

         $scope.editar = function (model) {
             $scope.editingModel = model;
             angular.extend($scope.nuevoModel, model);
             $scope.nuevoModel.editar = true;
         };

         function reset() {
             $scope.modelForm.$setPristine();
             $scope.nuevoModel = {};
             $scope.uniquemsg = '';
         }

         $scope.guardar = function (model) {
             modelosFactory.editar(model).then(function () {
                 model.editar = false;
                 angular.extend($scope.editingModel, $scope.nuevoModel);
                 reset();
             });
         };

         $scope.restablecerNuevo = function () {
             reset();
         };
     }])
    .controller('ClientesController', ['$scope', 'clientesFactory', function ($scope, modelosFactory) {
        $scope.modelos = {};
        $scope.nuevoModel = {};
        $scope.editingModel = {};
        $scope.tiposIdentificacion = [{ Tipo: 'RNC' }, { Tipo: 'CED' }];
        $scope.isOrden = $scope.$parent.isOrden;
        $scope.ordenClients = null;

        if (!$scope.isOrden) {
            modelosFactory.get().then(function (response) {
                $scope.modelos = response.data;
            });
        }
        $scope.agregar = function () {
            modelosFactory.agregar($scope.nuevoModel).then(function (response) {
                if ($scope.isOrden) {
                    $scope.ordenClients.push(response.data);
                } else {
                    $scope.modelos.push(response.data);
                }
                reset();
            });
        };

        $scope.editar = function (model) {
            $scope.editingModel = model;
            angular.extend($scope.nuevoModel, model);
            $scope.nuevoModel.editar = true;
        };
        function reset() {
            $scope.clientForm.$setPristine();
            $scope.nuevoModel = {};
            $scope.uniquemsg = '';
        }

        $scope.guardar = function (model) {
            var vehicles = model.Vehiculos;
            model.Vehiculos = [];

            modelosFactory.editar(model).then(function () {
                $scope.nuevoModel.editar = false;
                $scope.nuevoModel.Vehiculos = vehicles;
                angular.extend($scope.editingModel, $scope.nuevoModel);
                reset();
            });
        };

        $scope.cancelarEdicion = function (model) {
            model.editar = false;
        };

        $scope.restablecerNuevo = function () {
            reset();
        };
        $scope.$on('manageeditingclient', function (event, args) {            
            $scope.editar(args.Client);
        });

        $scope.$on('managecreatingclient', function (event, args) {            
            $scope.ordenClients = args.Clients;
        });
    }])
    .controller('VehiculosController', ['$scope', '$filter', 'vehiculosFactory', 'clientesFactory', 'modelosFactory', 'vehiculoMarcasFactory', 'insumosFactory',
        function ($scope, $filter, modelosFactory, clientesFact, modFact, vehMarcVehFact, insumosFact) {
            $scope.modelos = {};
            $scope.clientes = {};
            $scope.insumos = {};
            $scope.models = {};
            $scope.marcas = {};
            $scope.buscarInsumo = null;
            $scope.selInsumo = null;
            $scope.SelCliente = null;
            $scope.isOrden = $scope.$parent.isOrden;

            $scope.editingModel = {};
            $scope.combustibles = [{ IdCombustible: 1, Nombre: 'Gas Natural' }, { IdCombustible: 2, Nombre: 'GLP' }, { IdCombustible: 3, Nombre: 'Gasolina' }];
            $scope.nuevoModel = { Insumos: [] };
            if (!$scope.isOrden) {
                clientesFact.getVehiculos().then(function (response) {
                    $scope.clientes = response.data;
                });

                modelosFactory.get().then(function (response) {
                    $scope.modelos = response.data;
                });
            }
       
            insumosFact.get().then(function (response) {
                $scope.insumos = response.data;
            });

            $scope.querySearch = function (query) {
                var results = query ? $scope.insumos.filter(createFilterFor(query)) : [];
                return results;
            }
            $scope.transformInsumo = function (chip) {
                if (angular.isObject(chip)) {
                    return chip;
                }
                return { Nombre: chip, isNew: true, Activo: true };
            }
            function createFilterFor(query) {
                var lowercaseQuery = angular.lowercase(query);

                return function filterFn(insumo) {
                    return (insumo.Nombre.toLowerCase().indexOf(lowercaseQuery) != -1);
                };
            }

            vehMarcVehFact.get().then(function (response) {
                $scope.marcas = response.data;
                modFact.get().then(function (response) {
                    $scope.models = response.data;
                    $scope.marcas.forEach(function (mc) {
                        mc.Modelos = $filter('getAllByKey')($scope.models, 'IdMarca', mc.IdMarca);
                    });
                });
            });

            modFact.get().then(function (response) {
                $scope.models = response.data;
            });
           

            $scope.agregar = function () {
                modelosFactory.agregar($scope.nuevoModel).then(function (response) {
                    var vehiculo = response.data;
                    if ($scope.isOrden) {
                        $scope.SelCliente.Vehiculos.push(vehiculo);
                    } else {
                        $filter('getByKey')($scope.clientes, 'IDCliente', vehiculo.IdCliente).Vehiculos.push(vehiculo);
                    }
                    reset();
                });
            };

            $scope.editar = function (model) {
                $scope.editingModel = model;
                $scope.SelectedMarca = $filter('getByKey')($scope.marcas, 'IdMarca', model.Modelo.IdMarca);
                angular.extend($scope.nuevoModel, $scope.editingModel);
                $scope.nuevoModel.editar = true;
            };

            $scope.restablecerNuevo = function () {
                reset();
            };

            function reset() {
                $scope.vehiculoForm.$setPristine();
                $scope.buscarInsumo = null;
                $scope.nuevoModel = {Insumos:[]};
                $scope.SelectedMarca = {};
            };

            $scope.guardar = function () {
                modelosFactory.editar($scope.nuevoModel).then(function (response) {
                    $scope.nuevoModel.editar = false;
                    angular.extend($scope.editingModel, response.data);
                    reset();
                });
            };

            $scope.$on('manageeditingvehicle', function (event, args) {
                $scope.SelCliente = args.Client;
                $scope.editar(args.Vehicle);
            });

            $scope.$on('managecreatingvehicle', function (event, args) {
                $scope.SelCliente = args.Client;
                $scope.nuevoModel.IdCliente = args.Client.IDCliente;
            });

        }])
    .controller('ModelosController', ['$scope', '$filter', 'modelosFactory', 'vehiculoMarcasFactory', function ($scope, $filter, modelosFactory, vehiculoMarcasFactory) {
        $scope.modelos = {};
        $scope.nuevoModel = {};
        $scope.editingModel = {};

        $scope.marcas = {};

        vehiculoMarcasFactory.getMarcas().then(function (response) {
            $scope.marcas = response.data;
        });

        modelosFactory.get().then(function (response) {
            $scope.modelos = response.data;
            $scope.modelos.forEach(function (v) {
                v.VehiculoMarca = $filter('getByKey')($scope.marcas, 'IdMarca', v.IdMarca);
            });
        });

        $scope.agregar = function () {
            $scope.nuevoModel.IdMarca = $scope.nuevoModel.VehiculoMarca.IdMarca;
            modelosFactory.agregar($scope.nuevoModel).then(function (response) {
                var res = response.data;
                res.VehiculoMarca = $scope.nuevoModel.VehiculoMarca;

                $scope.modelos.push(res);
                $scope.nuevoModel = {};
            });
        };

        $scope.editar = function (model) {
            $scope.editingModel.editar = false;
            $scope.editingModel = model;
            $scope.editingModel.editar = true;
            angular.extend($scope.nuevoModel, model);
        };
        $scope.cancelarEdicion = function (modelo) {
            modelo.editar = false;
        };
        $scope.guardar = function () {

            modelosFactory.editar($scope.nuevoModel).then(function () {
                angular.extend($scope.editingModel, $scope.nuevoModel);
                $scope.editingModel.editar = false;
                $scope.nuevoModel = {};
            });
        };
    }])
    .controller('OrdenesTrabajosController', ['$scope', '$filter', '$mdDialog', 'serviciosFactory', 'ordenesTrabajosFactory', 'clientesFactory', 'proveedoresFactory',
        function ($scope, $filter, $mdDialog, serviciosFact, modelosFactory, clientesFact, provFact) {
        $scope.modelos = {};
        $scope.servicios = {};
        $scope.proveedores = {};
        $scope.nuevoModel = {};
        $scope.selectedClient = {};
        $scope.selectedVehiculo = {};
        $scope.isOrden = true;
        $scope.SelectedVehiculo = null;
        $scope.buscarInsumo = null;
        $scope.buscarProveedor = null;
        $scope.selInsumosProvs = [];
        $scope.selInsumo = null;
        $scope.selProveedor = null;
        $scope.test = 'test';

        clientesFact.getWithVeh().then(function (response) {
            $scope.modelos = response.data;
            serviciosFact.get().then(function (response) {
                $scope.servicios = response.data;

                provFact.getActivos().then(function (response) {
                    $scope.proveedores = response.data;
                });
            });
        });

        $scope.totalPreciosInsumos = function () {
            var total = 0;
            for (var ins in $scope.nuevoModel.InsumosProveedores) {
                total += ins.Precio;
            }
            return total;
        };

        $scope.agregar = function () {
            asignarInsumosProvs();
            modelosFactory.agregar($scope.nuevoModel).then(function (response) {
                $scope.SelectedVehiculo.OrdenesTrabajos.push(response.data);
                reset();
            });
        };

        $scope.editar = function (cliente, vehiculo, model, ev) {
            $scope.editingModel = model;
            angular.extend($scope.nuevoModel, model);
            if (model != null) {
                $scope.nuevoModel.Fecha = new Date(model.Fecha);
                $scope.nuevoModel.editar = true;
                for (var i in model.InsumosProveedores) {
                    var ins = model.InsumosProveedores[i].Insumo;
                    ins.IdProveedor = model.InsumosProveedores[i].IdProveedor;
                    ins.Precio = model.InsumosProveedores[i].Precio;
                    $scope.selInsumosProvs.push(ins);
                }
            } else {
                $scope.nuevoModel = {};
            }
            $scope.nuevoModel.IdVehiculo = vehiculo.IdVehiculo;
            $scope.SelectedVehiculo = vehiculo;
            $scope.SelectedCliente = cliente;
            $scope.buscarInsumo = null;

        };

        $scope.restablecerNuevo = function (model) {
            reset();
        };

        function reset() {
            $scope.nuevoModel = {};
            $scope.buscarInsumo = '';
            $scope.selInsumosProvs = [];
            $('#otTabs a:first').tab('show');
            $scope.buscarProveedor = null;
            $scope.selInsumo = null;
            $scope.selProveedor = null;

            $scope.modelForm.$setPristine();
        }

        function asignarInsumosProvs() {
            $scope.nuevoModel.InsumosProveedores = [];

            for (var i in $scope.selInsumosProvs) {
                var selInsProv = $scope.selInsumosProvs[i];
                var newItem = { IdInsumo: selInsProv.IdInsumo, IdProveedor: selInsProv.IdProveedor, Precio: selInsProv.Precio, Insumo: selInsProv };

                $scope.nuevoModel.InsumosProveedores.push(newItem);
            }
        }
        $scope.guardar = function (model) {
            asignarInsumosProvs();
            modelosFactory.editar($scope.nuevoModel).then(function () {
                $scope.nuevoModel.editar = false;
                angular.extend($scope.editingModel, $scope.nuevoModel);
                reset();
            });
        };

        $scope.selectedItemChange = function (item) {
            
        }

        $scope.querySearch = function (query, items) {
            var results = query ? items.filter(createFilterFor(query)) : [];
            return results;
        }

        function createFilterFor(query) {
            var lowercaseQuery = angular.lowercase(query);

            return function filterFn(item) {
                return (item.Nombre.toLowerCase().indexOf(lowercaseQuery) != -1);
            };
        }

        $scope.addVeh = function (cl) {
            $scope.$emit('creatingvehicle', { Client: cl });
        }

        $scope.editVeh = function (cl, vh) {
            $scope.$emit('editingvehicle', { Client: cl, Vehicle: vh });
        }
        $scope.addClient = function () {
            $scope.$emit('creatingclient', { Clients: $scope.modelos });
        }
        $scope.editClient = function (cl) {
            $scope.$emit('editingclient', { Client: cl});
        }
    }]);

