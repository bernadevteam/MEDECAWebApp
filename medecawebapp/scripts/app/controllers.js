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
    .controller('ServiciosController', ['$scope', 'serviciosFactory', function ($scope, modelosFactory) {
        $scope.modelos = {};
        $scope.nuevoModel = {};
        $scope.deleteServicio = {};

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
            modelosFactory.eliminar($scope.deleteServicio.Id).then(function () {
                var i = $scope.modelos.indexOf($scope.deleteServicio);
                $scope.modelos.splice(i, 1);
                $scope.deleteServicio = {};
            });
        };

        $scope.editar = function (model) {
            model.editar = true;
            angular.extend($scope.nuevoModel, model);
        };

        $scope.eliminarservicio = function (model) {
            $scope.deleteServicio = model;
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
                $scope.modelos.splice(i, 1);
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
    .controller('CitasController', ['$scope', '$mdDialog', '$filter', 'citasFactory', function ($scope, $mdDialog, $filter, modelosFactory) {
        $scope.modelos = {};
        $scope.clientes = [];
        $scope.fechaPopUp = false;
        $scope.format = 'dd/MM/yyyy';
        $scope.searchText = null;
        $scope.nuevoModel = {};
        $scope.calendarView = 'month';
        $scope.calendarDate = new Date();
        $scope.events = [

        ];
        modelosFactory.get().then(function (response) {
            var dat = response.data;

            for (var i in dat) {
                var event = dat[i];
                parseDate(event);
                $scope.events.push(event);
            }
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
            nMod.title = nMod.Cliente + ", " + nMod.Vehiculo,
                nMod.type = 'info';
            nMod.startsAt = ndDate;
        }

    }])
    .controller('ClientesController', ['$scope', 'clientesFactory', function ($scope, modelosFactory) {
        $scope.modelos = {};
        $scope.nuevoModel = {};
        $scope.editingModel = {};
        $scope.tiposIdentificacion = [{ Tipo: 'RNC' }, { Tipo: 'CED' }];
        $scope.isOrden = $scope.$parent.isOrden;
        $scope.ordenClients = null;
        $scope.deleteCliente = {};

        if (!$scope.isOrden) {
            modelosFactory.get().then(function (response) {
                $scope.modelos = response.data;
            });
        }
        $scope.agregar = function () {
            modelosFactory.agregar($scope.nuevoModel).then(function (response) {
                response.data.CanDelete = true;
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
                angular.forEach(vehicles, function (vh) {
                    vh.Cliente = _.omit($scope.nuevoModel, ['editar', 'Vehiculos']);
                    angular.forEach(vh.OrdenesTrabajos, function (ordenTrabajo) {
                        ordenTrabajo.Vehiculo = _.omit(vh, ['OrdenesTrabajos']);
                    });
                });
                $scope.nuevoModel.Vehiculos = vehicles;
                angular.extend($scope.editingModel, $scope.nuevoModel);
                reset();
            });
        };

        $scope.cancelarEdicion = function (model) {
            model.editar = false;
        };

        $scope.eliminar = function () {
            modelosFactory.eliminar($scope.editingModel).then(function () {
                var i = $scope.modelos.indexOf($scope.editingModel);
                $scope.modelos.splice(i, 1);
            });
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
            $scope.modelos = [];
            $scope.nuevaMarca = {};
            $scope.clientes = [];
            $scope.insumos = [];
            $scope.models = [];
            $scope.marcas = [];
            $scope.deleteVehiculo = {};
            $scope.deleteCliente = {};
            $scope.SelectedMarca = {};
            $scope.buscarInsumo = null;
            $scope.selInsumo = null;
            $scope.selectedClient = null;
            $scope.isOrden = $scope.$parent.isOrden;
            $scope.editingModel = {};
            $scope.combustibles = [{ IdCombustible: 1, Nombre: 'GLP' }, { IdCombustible: 2, Nombre: 'Gasolina' }, { IdCombustible: 3, Nombre: 'Gas Natural' }, { IdCombustible: 4, Nombre: 'Diesel' }];
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
            $scope.setMarca = function (marca) {
                $scope.nuevaMarca = marca;
            };
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
                        $scope.selectedClient.Vehiculos.push(vehiculo);
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

            $scope.eliminarVehiculo = function (cl, model) {
                $scope.deleteCliente = cl;
                $scope.deleteVehiculo = model;
            };

            $scope.eliminar = function () {
                modelosFactory.eliminar($scope.deleteVehiculo).then(function () {
                    var i = $scope.deleteCliente.Vehiculos.indexOf($scope.deleteVehiculo);
                    $scope.deleteCliente.Vehiculos.splice(i, 1);
                });
            };

            $scope.restablecerNuevo = function () {
                reset();
            };

            function reset() {
                $scope.vehiculoForm.$setPristine();
                $scope.buscarInsumo = null;
                $scope.nuevoModel = { Insumos: [] };
                $scope.SelectedMarca = {};
                $scope.manageVehiculoDesabilitar = null;
            };

            $scope.guardar = function () {
                modelosFactory.editar($scope.nuevoModel).then(function (response) {
                    $scope.nuevoModel.editar = false;
                    var nVeh = response.data;
                    angular.forEach(nVeh.OrdenesTrabajos, function (ordenTrabajo) {
                        ordenTrabajo.Vehiculo = _.omit(nVeh, ['OrdenesTrabajos', 'editar']);
                    });
                    angular.extend($scope.editingModel, nVeh);
                    reset();
                });
            };

            $scope.$on('manageeditingvehicle', function (event, args) {
                $scope.selectedClient = args.Client;
                $scope.editar(args.Vehicle);
                $scope.manageVehiculoDesabilitar = args.Deshabilitar;
            });

            $scope.$on('managecreatingvehicle', function (event, args) {
                $scope.selectedClient = args.Client;
                $scope.nuevoModel.IdCliente = args.Client.IDCliente;
            });

        }])
    .controller('ModelosController', ['$scope', '$filter', 'modelosFactory', 'vehiculoMarcasFactory', function ($scope, $filter, modelosFactory, vehiculoMarcasFactory) {
        $scope.nuevoModel = {};
        $scope.nuevoModelo = {};
        $scope.nuevosModelos = [];
        $scope.editingModelo = {};

        if (!$scope.marcas) {
            $scope.marcas = {};
            vehiculoMarcasFactory.getMarcas().then(function (response) {
                $scope.marcas = response.data;
            });
        } else {
            $scope.$watch('nuevaMarca', function () {
                $scope.editingModelo = $scope.nuevaMarca;
                angular.extend($scope.nuevoModel, $scope.editingModelo);
            });
        }

        $scope.agregarModelo = function () {
            if (!$scope.nuevoModel.Modelos) {
                $scope.nuevoModel.Modelos = [];
            }
            $scope.nuevoModelo.CanDelete = true;
            $scope.nuevosModelos.push($scope.nuevoModelo);
            $scope.nuevoModelo = {};
        };

        $scope.agregar = function (el) {
            $scope.nuevoModel.Modelos = $scope.nuevoModel.Modelos ? $scope.nuevoModel.Modelos.concat($scope.nuevosModelos) : $scope.nuevosModelos;

            vehiculoMarcasFactory.agregar($scope.nuevoModel).then(function (response) {
                var res = response.data;
                for (var i = 0; i < res.Modelos.length; i++) {
                    res.Modelos[i].CanDelete = true;
                }
                $scope.marcas.push(res);
                reset();
                if ($scope.modalCaller) {
                    switchModal(el, $scope.modalCaller);
                }
            });
        };

        $scope.editar = function (marca) {
            $scope.editingModelo = marca;
            angular.extend($scope.nuevoModel, $scope.editingModelo);
        };

        $scope.eliminarModelo = function (marca, modelo) {
            modelosFactory.eliminar(modelo).then(function () {
                var i = marca.Modelos.indexOf(modelo);
                marca.Modelos.splice(i, 1);
            });

        };

        $scope.eliminarTempModelo = function (modelo) {
            var i = $scope.nuevosModelos.indexOf(modelo);
            $scope.nuevosModelos.splice(i, 1);
        };


        $scope.eliminarMarca = function (marca) {
            vehiculoMarcasFactory.eliminar(marca.IdMarca).then(function () {
                var i = $scope.marcas.indexOf($scope.editingModelo);
                $scope.marcas.splice(i, 1);
            });
        };

        $scope.restablecerNuevo = function (el) {
            reset();
            if ($scope.modalCaller) {
                switchModal(el, $scope.modalCaller);
            }
        };

        function reset() {
            $scope.modeloForm.$setPristine();
            if (!$scope.nuevaMarca) {
                $scope.nuevoModel = {};
            }
            $scope.nuevoModelo = {};
            $scope.nuevosModelos = [];
        };

        $scope.guardar = function (el) {
            $scope.nuevoModel.Modelos = $scope.nuevoModel.Modelos ? $scope.nuevoModel.Modelos.concat($scope.nuevosModelos) : $scope.nuevosModelos;
            vehiculoMarcasFactory.agregar($scope.nuevoModel).then(function (response) {
                var marca = response.data;
                for (var i = 0; i < marca.Modelos.length; i++) {
                    marca.Modelos[i].CanDelete = true;
                }
                angular.extend($scope.editingModelo, marca);
                if ($scope.modalCaller) {
                    switchModal(el, $scope.modalCaller);
                }
                reset();
            });
        };

    }])
    .controller('AutorizadosController', ['$scope', 'usuariosFactory', function ($scope, usuariosFactory) {
        $scope.usuarios = [];
        $scope.usuario = {};
        $scope.deleteUsuario = null;

        usuariosFactory.get().then(function (response) {
            $scope.usuarios = response.data;
        });

        $scope.restablecer = function () {
            $scope.usuario = {};
            $scope.deleteUsuario = null;
        };

        $scope.agregar = function () {
            usuariosFactory.agregar($scope.usuario).then(function (response) {
                $scope.usuarios.push(response.data);
                $scope.usuario = {};
            });
        };
        $scope.eliminarCancel = function (usuario) {
            usuario.delete = false;
            $scope.deleteUsuario = null;
        };
        $scope.eliminarUsuario = function (usuario) {
            usuario.delete = true;
            $scope.deleteUsuario = usuario;
        };
        $scope.eliminar = function () {
            usuariosFactory.eliminar($scope.deleteUsuario.Id).then(function () {
                var i = $scope.usuarios.indexOf($scope.deleteUsuario);
                $scope.usuarios.splice(i, 1);
                $scope.deleteUsuario = null;
            });
        };
    }])
    ;
