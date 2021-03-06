﻿'use strict';

var app = angular.module('medecaApp');

app.filter('getByKey', function () {
    return function (input, keyName, value) {
        var i = 0, len = input.length;
        for (; i < len; i++) {
            if (+input[i][keyName] === +value || !value) {
                return input[i];
            }
        }
        return null;
    }
}).filter('propsFilter', function () {
    return function (items, props) {
        var out = [];

        if (angular.isArray(items)) {
            var keys = Object.keys(props);

            items.forEach(function (item) {
                var itemMatches = false;

                for (var i = 0; i < keys.length; i++) {
                    var prop = keys[i];
                    var text = props[prop].toString().toLowerCase();
                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                        itemMatches = true;
                        break;
                    }
                }

                if (itemMatches) {
                    out.push(item);
                }
            });
        } else {
            // Let the output be the input untouched
            out = items;
        }

        return out;
    };
})
    .filter("dataRangeFilter", function () {
        return function (items, from, to) {

            var arrayToReturn = [];
            if (items != undefined) {
                for (var i = 0; i < items.length; i++) {
                    var dt = new Date(items[i].Fecha);
                    //                if (dt.getTime() >= (from || dt).getTime() && dt.getTime() <= (to || dt).getTime()) {
                    if (dt >= (from || dt) && dt <= (to || dt)) {

                        arrayToReturn.push(items[i]);
                    }
                }
            }
            return arrayToReturn;
        };
    })
        .filter('distanceUnit', function () {
            return function (data) {
                if (typeof (data) === 'undefined') {
                    return '';
                }

                return data === 'Km' ? 'Kilometraje' : 'Millaje';
            };
        })
    .filter('vehicleInfo', function () {
        return function (vehicle) {
            return vehicle.Modelo.VehiculoMarca.Nombre + ' ' + vehicle.Modelo.Nombre + ' ' + vehicle.Anio;
        };
    })
    .filter('sumByKey', function () {
        return function (data, key) {
            if (typeof (data) === 'undefined' || typeof (key) === 'undefined') {
                return 0;
            }

            var sum = 0;
            for (var i = data.length - 1; i >= 0; i--) {
                sum += parseFloat(data[i][key]);
            }

            return sum;
        };
    })
    .filter('totalCal', function () {
        return function (data, props) {
            if (typeof (data) === 'undefined' || typeof (props) === 'undefined') {
                return 0;
            }

            var sum = 0;

            for (var i = data.length - 1; i >= 0; i--) {
                sum += parseFloat(data[i][props[0]]) * parseFloat(data[i][props[1]]);
            }

            return sum.toFixed(2);
        };
    })

.filter('orderObjectBy', function () {
    return function (items, field, reverse) {
        var filtered = [];
        angular.forEach(items, function (item) {
            filtered.push(item);
        });
        filtered.sort(function (a, b) {
            return (a[field] > b[field] ? 1 : -1);
        });
        if (reverse) filtered.reverse();
        return filtered;
    };
})
    .filter('filtrarDiagnosticos', function () {
        return function (items, estado) {
            var filtered = [];
            angular.forEach(items, function (item) {
                if (item.IdEstado === estado) {
                    filtered.push(item);
                }
            });
            return filtered;
        };
    })
.filter('getAllByKey', function () {
    return function (input, keyName, value) {
        if (keyName != null && value) {
            var i = 0, len = input.length, res = [];
            for (; i < len; i++) {
                if (String(input[i][keyName]) == value) {
                    res.push(input[i]);
                }
            }
            return res;
        } else {
            return input;
        }
    }
})
.filter('porclienteoplaca', function () {
    return function (model, clienteoplaca) {
        var items = [];
        if (clienteoplaca != null && clienteoplaca.length > 2) {
            angular.forEach(model, function (cliente) {
                var anadir = false;

                if (cliente.Nombre.toLowerCase().indexOf(clienteoplaca) != -1) {
                    cliente.PorPlaca = false;
                    anadir = true;
                }

                if (!anadir) {
                    angular.forEach(cliente.Vehiculos, function (vehiculo) {
                        if (vehiculo.Placa && vehiculo.Placa.toLowerCase().indexOf(clienteoplaca) != -1) {
                            cliente.PorPlaca = true;
                            items.Cliente = cliente;
                            anadir = true;
                        }
                    });
                }

                if (anadir) {
                    items.push(cliente);
                }

            });
        }
        return items;
    };
})
.filter('ordenesactivas', function () {
    return function (model) {
        var items = [];
        angular.forEach(model, function (cliente) {
            angular.forEach(cliente.Vehiculos, function (vehiculo) {
                angular.forEach(vehiculo.OrdenesTrabajos, function (ordenTrabajo) {
                    if (!ordenTrabajo.Entregado) {
                        ordenTrabajo.DaysOverdue = Math.floor((new Date(ordenTrabajo.FechaPrometida) - new Date()) / (1000 * 60 * 60 * 24));
                        items.push(ordenTrabajo);
                    }
                });
            });
        });
        return items;
    };
})
.filter('existeClienteOPlaca', function () {
    return function (model, clienteoplaca) {
        var items = [];
        if (clienteoplaca !== null && clienteoplaca.length > 2) {
            angular.forEach(model, function (cliente) {
                if (cliente.Nombre.toLowerCase().indexOf(clienteoplaca) != -1) {
                    items = [{ exists: true }];
                    return;
                }
                angular.forEach(cliente.Vehiculos, function (vehiculo) {
                    if (vehiculo.Placa.toLowerCase().indexOf(clienteoplaca) != -1) {
                        items = [{ exists: true }];
                        return;
                    }
                });

            });
        }
        return items;
    };
})
.filter('propiedadOarray', function () {
    return function (model, propiedad, arreglo, elemProp, filtro) {
        var items = [];
        if (filtro && filtro.length > 2) {
            angular.forEach(model, function (entidad) {
                if (entidad[propiedad].toLowerCase().indexOf(filtro) !== -1) {
                    items.push(entidad);
                } else {
                    angular.forEach(entidad[arreglo], function (elem) {
                        if (elem[elemProp].toLowerCase().indexOf(filtro) !== -1) {
                            items.push(entidad);
                        }
                    });
                }
            });
        }

        return items;
    }
});




