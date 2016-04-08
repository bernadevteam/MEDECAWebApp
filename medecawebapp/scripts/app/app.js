'use strict';

angular.module('medecaApp', ['ngOrderObjectBy', 'checklist-model', 'ngMaterial', 'angular.morris-chart', 'mwl.calendar', 'ui.bootstrap', 'angularUtils.directives.dirPagination'])
.run(function ($rootScope) {
    /*
        Receive emitted message and broadcast it.
        Event names must be distinct or browser will blow up!
    */
    $rootScope.$on('cambiocita', function (event, args) {
        $rootScope.$broadcast('manejarcambiocita', args);
    });

    $rootScope.$on('editingvehicle', function (event, args) {
        $rootScope.$broadcast('manageeditingvehicle', args);
    }); 
    $rootScope.$on('cambiovehiculo', function (event, args) {
        $rootScope.$broadcast('managecambiovehiculo', args);
    });
    $rootScope.$on('creatingvehicle', function (event, args) {
        $rootScope.$broadcast('managecreatingvehicle', args);
    });
    $rootScope.$on('creatingclient', function (event, args) {
        $rootScope.$broadcast('managecreatingclient', args);
    });
    $rootScope.$on('editingclient', function (event, args) {
        $rootScope.$broadcast('manageeditingclient', args);
    });
});;

function switchModal(el, modalTosSwitch) {
    var parentModal = $(el).closest(".modal");
    parentModal.modal('hide');
    parentModal.on('hidden.bs.modal', function (e) {
        $(modalTosSwitch).modal({
            keyboard: false,
            backdrop: 'static'
        });
        parentModal.unbind('hidden.bs.modal');

    });

}
