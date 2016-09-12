angular.module('medecaApp')
.controller('OrdenesTrabajosController', ['$scope', '$filter', '$mdDialog', 'serviciosFactory', 'ordenesTrabajosFactory', 'clientesFactory',
    'proveedoresFactory', 'insumosMarcasFactory',
    function ($scope, $filter, $mdDialog, serviciosFact, modelosFactory, clientesFact, provFact, insumosMarcasFactory) {
        //OT Variables
        $scope.modelos = [];
        $scope.format = 'dd/MM/yyyy';
        $scope.fechaRecibidoPopUp = $scope.fechaPrometidoPopUp = $scope.fechaEntregadoPopUp = false;
        $scope.proveedores = [];
        $scope.nuevoProveedor = {};
        $scope.SelectedCliente = {};
        $scope.SelectedVehiculo = {};
        $scope.ocultarGuardar = false;
        $scope.isOrden = true;
        $scope.diagnosticoPivot = {};
        $scope.diagnosticoEdit = {};
        $scope.buscarInsumo = null;
        $scope.buscarProveedor = null;
        $scope.diagEstados = [];
        $scope.marcasInsumos = [];
        $scope.selInsumosProvs = [];
        $scope.selInsumo = null;
        $scope.selProveedor = null;
        $scope.nuevaMarcaInsumo = {};

        $scope.Math = window.Math;

        function restablecerOrdenTrabajo() {
            $scope.nuevoModel = { Diagnosticos: [], InsumosCotizados: [], Entregado: false };
        }

        restablecerOrdenTrabajo();

        function updateClients() {
            angular.forEach($scope.modelos, function (cliente) {
                angular.forEach(cliente.Vehiculos, function (vehiculo) {
                    vehiculo.Cliente = _.omit(cliente, ['Vehiculos']);
                    angular.forEach(vehiculo.OrdenesTrabajos, function (ordenTrabajo) {
                        ordenTrabajo.Vehiculo = _.omit(vehiculo, ['OrdenesTrabajos']);
                    });
                });
            });
        }

        clientesFact.getWithVeh().then(function (response) {
            $scope.modelos = response.data;
            updateClients();
            serviciosFact.get().then(function (response) {
                $scope.servicios = response.data;

                provFact.getActivos().then(function (response) {
                    $scope.proveedores = response.data;
                    modelosFactory.obtenerDiagnosticosEstados().then(function (response) {
                        $scope.diagEstados = response.data;
                        resetDiagPivot();
                        insumosMarcasFactory.get().then(function (response) {
                            $scope.marcasInsumos = response.data;
                        });
                    });
                });
            });
        });
        $scope.parseOrden = function (noorden) {
            return noorden ? "No. " + "00000".slice(0, -noorden.length) + noorden : "";
        };
        $scope.totalPreciosInsumos = function () {
            var total = 0;
            for (var ins in $scope.nuevoModel.InsumosProveedores) {
                total += ins.Precio;
            }
            return total;
        };
        $scope.editar = function (cliente, vehiculo, model, ev) {
            $scope.editingModel = model;
            angular.merge($scope.nuevoModel, model);
            $scope.selInsumosProvs = [];
            if (model != null) {
                $scope.nuevoModel.Fecha = new Date(model.Fecha);
                $scope.nuevoModel.FechaEntrega = model.FechaEntrega ? new Date(model.FechaEntrega) : null;
                $scope.nuevoModel.FechaPrometida = model.FechaPrometida ? new Date(model.FechaPrometida) : null;
                $scope.nuevoModel.editar = true;
                for (var i in model.InsumosProveedores) {
                    var ins = model.InsumosProveedores[i].Insumo;
                    ins.IdProveedor = model.InsumosProveedores[i].IdProveedor;
                    ins.Precio = model.InsumosProveedores[i].Precio;
                    ins.Cantidad = model.InsumosProveedores[i].Cantidad;
                    ins.NombreProveedor = model.InsumosProveedores[i].Proveedore ? model.InsumosProveedores[i].Proveedore.Nombre : null;
                    $scope.selInsumosProvs.push(ins);
                }
            } else {
                restablecerOrdenTrabajo();
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
            restablecerOrdenTrabajo();
            $scope.buscarInsumo = '';
            $scope.selInsumosProvs = [];
            $('#otTabs a:first').tab('show');
            $scope.buscarProveedor = null;
            $scope.selInsumo = null;
            $scope.selProveedor = null;
            resetDiagPivot();
            restablecerInsumoCotizado();
            $scope.modelForm.$setPristine();
        }
        //INSUMOS 
        function restablecerInsumoCotizado() {
            $scope.insumoCotizadoPivot = null;
            $scope.insumoCotizado = { Aprobado: false, Cantidad: 1, EsLocal: true };
            $scope.ocultarGuardar = false;
            $scope.cotizarInsumo = false;
        }

        //Funcion para evaluar si la orden tiene cotizaciones pendientes.
        $scope.tieneCotizacionesPendientes = function (cotizacion) {
            return !cotizacion.Precio || isNaN(cotizacion.Precio) || cotizacion.Precio == 0 || !cotizacion.IdProveedor;
        };

        $scope.guardarInsumoCotizado = function () {
            var insumoCotizado = $scope.insumoCotizado;
            insumoCotizado.NombreInsumo = $filter('getByKey')($scope.SelectedVehiculo.Insumos, 'IdInsumo', insumoCotizado.IdInsumo).Nombre;
            insumoCotizado.NombreProveedor = insumoCotizado.IdProveedor ? $filter('getByKey')($scope.proveedores, 'IdProveedor', insumoCotizado.IdProveedor).Nombre : null;
            insumoCotizado.NombreMarca = insumoCotizado.IdMarcaInsumo ? $filter('getByKey')($scope.marcasInsumos, 'IdMarcaInsumo', insumoCotizado.IdMarcaInsumo).Nombre : null;

            if ($scope.insumoCotizadoPivot) {
                angular.extend($scope.insumoCotizadoPivot, insumoCotizado);
            } else {
                insumoCotizado.IdOrden = $scope.nuevoModel.Id;
                $scope.nuevoModel.InsumosCotizados.push(insumoCotizado);
            }
            restablecerInsumoCotizado();
        };


        $scope.modificarInsumoCotizado = function (insumo) {
            $scope.ocultarGuardar = true;
            $scope.cotizarInsumo = true;

            $scope.insumoCotizadoPivot = insumo;
            angular.extend($scope.insumoCotizado, insumo);
        };

        $scope.eliminarInsumoCotizado = function (insumo) {
            var i = $scope.nuevoModel.InsumosCotizados.indexOf(insumo);
            $scope.nuevoModel.InsumosCotizados.splice(i, 1);
        };


        $scope.cancelarEdicionInsumoCotizado = restablecerInsumoCotizado;

        restablecerInsumoCotizado();

        $scope.agregarMarcaInsumo = function () {
            insumosMarcasFactory.agregar($scope.nuevaMarcaInsumo).then(function (response) {
                $scope.marcasInsumos.push(response.data);
                $scope.nuevaMarcaInsumo = {};
            });
        };

        function asignarInsumosProvs() {
            $scope.nuevoModel.InsumosProveedores = [];

            for (var i in $scope.selInsumosProvs) {
                var selInsProv = $scope.selInsumosProvs[i];
                var newItem = { IdInsumo: selInsProv.IdInsumo, IdProveedor: selInsProv.IdProveedor, Precio: selInsProv.Precio, Cantidad: selInsProv.Cantidad, Insumo: selInsProv };

                $scope.nuevoModel.InsumosProveedores.push(newItem);
            }
        }
        $scope.agregar = function () {
            asignarInsumosProvs();
            modelosFactory.agregar($scope.nuevoModel).then(function (response) {
                response.data.Vehiculo = _.omit($scope.SelectedVehiculo, ['OrdenesTrabajos']);
                response.data.Cliente = _.omit($scope.SelectedCliente, ['Vehiculos']);
                $scope.SelectedVehiculo.OrdenesTrabajos.push(response.data);
                reset();
            });
        };


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
            var results = query && items ? items.filter(createFilterFor(query)) : [];
            return results;
        }
        //Diagnosticos
        function resetDiagPivot() {
            var defEstado = angular.extend($scope.diagEstados[0]);
            $scope.diagnosticoPivot = { Estado: defEstado, IdEstado: defEstado.IdDiagnosticoEstado };
        };
        //Funcion para evaluar si un diagnostico tiene reparaciones pendientes.
        $scope.tienePendientes = function (diagnostico) {
            return diagnostico.IdEstado == 1;
        };

        $scope.agregarDiagnostico = function () {
            $scope.nuevoModel.Diagnosticos.push(angular.extend($scope.diagnosticoPivot));
            resetDiagPivot();
        };

        $scope.modificarDiagnostico = function (diagnostico) {
            angular.extend($scope.diagnosticoPivot, diagnostico);
            $scope.diagnosticoEdit = diagnostico;
            $scope.diagnosticoPivot.Editando = true;
        };

        $scope.guardarDiagnostico = function () {
            angular.extend($scope.diagnosticoEdit, $scope.diagnosticoPivot);
            resetDiagPivot();
        };

        $scope.removerDiagnostico = function (diagnostico) {
            var i = $scope.nuevoModel.Diagnosticos.indexOf(diagnostico);
            $scope.nuevoModel.Diagnosticos.splice(i, 1);
        };

        $scope.cancelarEdicionDiagnostico = resetDiagPivot;

        $scope.aprobarDiagnostico = function (diag) {
            modelosFactory.aprobarDiagnostico(diag).then(function () {
                diag.IdEstado = 2;
                angular.extend($scope.editingModel, $scope.nuevoModel);
            });
        };

        function createFilterFor(query) {
            var lowercaseQuery = angular.lowercase(query);

            return function filterFn(item) {
                return (item.Nombre.toLowerCase().indexOf(lowercaseQuery) != -1);
            };
        }

        $scope.addVeh = function (cl) {
            $scope.$emit('creatingvehicle', { Client: cl });
        }

        $scope.editVeh = function (cl, vh, des) {
            $scope.$emit('editingvehicle', { Client: cl, Vehicle: vh, Deshabilitar: des });
        }
        $scope.addClient = function () {
            $scope.$emit('creatingclient', { Clients: $scope.modelos });
        }
        $scope.editClient = function (cl) {
            $scope.$emit('editingclient', { Client: cl });
        }


    }]);