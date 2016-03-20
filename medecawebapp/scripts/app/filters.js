'use strict';

var app = angular.module('medecaApp');

app.filter('getByKey', function () {
    return function (input, keyName, value) {
        var i = 0, len = input.length;
        for (; i < len; i++) {
            if (+input[i][keyName] == +value || !value) {
                return input[i];
            }
        }
        return null;
    }
}).filter('propsFilter', function() {
  return function(items, props) {
    var out = [];

    if (angular.isArray(items)) {
      var keys = Object.keys(props);
        
      items.forEach(function(item) {
        var itemMatches = false;

        for (var i = 0; i < keys.length; i++) {
          var prop = keys[i];
          var text = props[prop].toLowerCase();
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
    .filter("dataRangeFilter", function() {
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
.filter('sumByKey', function() {
         return function(data, key) {
             if (typeof(data) === 'undefined' || typeof(key) === 'undefined') {
                 return 0;
             }

             var sum = 0;
             for (var i = data.length - 1; i >= 0; i--) {
                 sum += parseFloat(data[i][key]);
             }

             return sum;
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
        var items = {};
        if (clienteoplaca != null && clienteoplaca.length > 2) {
            angular.forEach(model, function (cliente) {
                if (cliente.Nombre.toLowerCase().indexOf(clienteoplaca) != -1) {
                    cliente.PorPlaca = false;
                    items.Cliente = cliente;
                    return;
                }
                angular.forEach(cliente.Vehiculos, function (vehiculo) {
                    if (vehiculo.Placa.toLowerCase().indexOf(clienteoplaca) != -1) {
                        cliente.PorPlaca = true;
                        items.Cliente = cliente;
                        return;
                    }
                });

            });
        }
        return items;
    };
})
.filter('existeClienteOPlaca', function () {
    return function (model, clienteoplaca) {
        var items = [];
        if (clienteoplaca != null && clienteoplaca.length > 2) {
            angular.forEach(model, function (cliente) {
                if (cliente.Nombre.toLowerCase().indexOf(clienteoplaca) != -1) {
                    items = [{exists:true}];
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
});




