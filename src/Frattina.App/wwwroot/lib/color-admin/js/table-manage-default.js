/*
Template Name: Color Admin - Responsive Admin Dashboard Template build with Twitter Bootstrap 4
Version: 4.4.0
Author: Sean Ngu
Website: http://www.seantheme.com/color-admin/admin/
*/

var handleDataTableDefault = function() {
	"use strict";
    
	if ($('#data-table-default').length !== 0) {
		$('#data-table-default').DataTable({
            "language": {
                "sEmptyTable": "Nenhum registro encontrado",
                "sInfo": "Mostrando de _START_ a _END_ de _TOTAL_ registros",
                "sInfoEmpty": "Mostrando 0 a 0 de 0 registros",
                "sInfoFiltered": "(Filtrados de _MAX_ registros)",
                "sInfoPostFix": "",
                "sInfoThousands": ".",
                "sLengthMenu": "_MENU_ resultados por pagina",
                "sLoadingRecords": "Carregando...",
                "sProcessing": "Processando...",
                "sZeroRecords": "Nenhum registro encontrado",
                "sSearch": "Pesquisar",
                "oPaginate": {
                    "sNext": "Proximo",
                    "sPrevious": "Anterior",
                    "sFirst": "Primeiro",
                    "sLast": "Ultimo"
                },
                "oAria": {
                    "sSortAscending": ": Ordenar colunas de forma ascendente",
                    "sSortDescending": ": Ordenar colunas de forma descendente"
                },
                "select": {
                    "rows": {
                        "_": "Selecionado %d linhas",
                        "0": "Nenhuma linha selecionada",
                        "1": "Selecionado 1 linha"
                    }
                }
            },
            "lengthMenu": [[20, 40, 60, 80, -1], [20, 40, 60, 80, "All"]],
			responsive: true
		});
	}
};

var handleDataTableResponsive = function () {
    "use strict";

    if ($('table[data-table-responsive]').length !== 0) {
        $('table[data-table-responsive]').DataTable({
            "paging": false,
            "ordering": false,
            "info": false,
            "searching": false,
            responsive: true
        });
    }
};

var TableManageDefault = function () {
	"use strict";
	return {
		init: function () {
            handleDataTableDefault();
            handleDataTableResponsive();
		}
	};
}();

$(document).ready(function() {
	TableManageDefault.init();
});