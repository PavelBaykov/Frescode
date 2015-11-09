/* ------------------------------------------------------------------------------
*
*  # Basic datatables
*
*  Specific JS code additions for datatable_basic.html page
*
*  Version: 1.0
*  Latest update: Aug 1, 2015
*
* ---------------------------------------------------------------------------- */


function format ( r ) {
    
    // `d` is the original data object for the row
    return '<table  border="0" style="margin-left:50px;  ">'+
        '<tr>'+
            '<td style="padding:10px; white-space: normal">Description:</td>'+
            '<td style="padding:10px; white-space: normal">'+r.data("description")+'</td>'+
        '</tr>'+
        '<tr>'+
            '<td style="padding:10px; white-space: normal">DefectSpots count:</td>'+
            '<td style="padding:10px; white-space: normal">'+r.data("quantity")+'</td>'+
        '</tr>'+
    '</table>'+
    '<div <!--class="text-center"-->'+
    '<button type="button" class="btn btn-primary" style="margin-left:50px; margin-top:10px;" href ="ViewItem.html>'+
    '<i class="icon-cog3 position-left"></i>' + 'View item details</button>'+
    '</div>';
    }

function initDataTables() {


    // Table setup
    // ------------------------------

    // Setting datatable defaults
    $.extend( $.fn.dataTable.defaults, {
        autoWidth: false,
        columnDefs: [
        { 
            //width: '150px',
            targets: [ 1 ],
        },
        { 
            //width: '200px',
            targets: [ 2 ],
        },

        { 
            orderable: false,
            width: '100px',
            targets: [ 3 ],
            sClass: "cell-center-align"
        }],
        dom: '<"datatable-header"fl><"datatable-scroll"t><"datatable-footer"ip>',
        language: {
            search: '<span>Filter:</span> _INPUT_',
            lengthMenu: '<span>Show:</span> _MENU_',
            paginate: { 'first': 'First', 'last': 'Last', 'next': '&rarr;', 'previous': '&larr;' }
        },
        drawCallback: function () {
            $(this).find('tbody tr').slice(-3).find('.dropdown, .btn-group').addClass('dropup');
        },
        preDrawCallback: function() {
            $(this).find('tbody tr').slice(-3).find('.dropdown, .btn-group').removeClass('dropup');
        }
    });


    // Basic datatable
    var mainTable = $('.datatable-basic').DataTable(
    {
        "iDisplayLength": 25,
        lengthMenu: [[25, 50, -1], [25, 50, "All"]],
        "scrollX": false,
        "aaSorting": []
    }
    );

    $('.datatable-basic tbody').on('click', 'td:not(.checkboxcolumn)', function () {
        var tr = $(this).closest('tr');
        var row = mainTable.row( tr );

        if ( row.child.isShown() ) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            row.child(format(tr),'detailedItem' ).show();
            tr.addClass('shown');
            $(this).closest('tr').next(tr).find('.btn').on('click',function(){
                window.location.href='ViewItem.html';
            });

        }
    } );


    // Alternative pagination
    $('.datatable-pagination').DataTable({
        pagingType: "simple",
        language: {
            paginate: {'next': 'Next &rarr;', 'previous': '&larr; Prev'}
        }
    });


    // Datatable with saving state
    $('.datatable-save-state').DataTable({
        stateSave: true
    });


    // Scrollable datatable
    $('.datatable-scroll-y').DataTable({
        autoWidth: true,
        scrollY: 300
    });



    // External table additions
    // ------------------------------

    // Add placeholder to the datatable filter option
    $('.dataTables_filter input[type=search]').attr('placeholder','Type to filter...');


    // Enable Select2 select for the length option
    $('.dataTables_length select').select2({
        minimumResultsForSearch: "-1"
    });
    
};
