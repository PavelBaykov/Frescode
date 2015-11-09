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


function format ( str,tr ) {
    
    // `d` is the original data object for the row
   
        var picturesArray = str.split(',');

        text="";
        if (str.length > 0) 
        {
            for (i = 0; i < picturesArray.length; i++) { 
                text += '<div class="col-lg-2 col-sm-3 col-xs-4">'+
    '                           <div class="thumbnail">'+
    '                               <div class="thumb">'+
    '                                       <span>'+
    '                                           <a href="'+picturesArray[i]+'" data-popup="lightbox" rel="gallery'+tr[0]+'" class="btn border-white text-white btn-flat btn-icon btn-rounded" style="padding:0px"><img src="'+picturesArray[i]+'" alt="" ></a>'+
    '                                       </span>'+
    '                               </div>'+
    '                           </div>'+
    '                       </div>';
            };
        }
        else {
            text+='No images';
        }


        return text;
       
    }

function initDataTables() {


    // Table setup
    // ------------------------------

    // Setting datatable defaults
    $.extend( $.fn.dataTable.defaults, {
        autoWidth: false,
        "bSort" : false,
        columnDefs: [
                {
            //orderable: false,
            targets: [ 0 ],
            className: "number",
            name: "number"
        },
        {
            //orderable: false,
            targets: [ 1 ]
        },
        {
            //orderable: false,
            //width: '50px',
            targets: [ 2 ],
            className: "number"
        },
        ],
        dom: '',
        language: {
            search: '<span>Filter:</span> _INPUT_',
            lengthMenu: '<span>Show:</span> _MENU_',
            paginate: { 'first': 'First', 'last': 'Last', 'next': '&rarr;', 'previous': '&larr;' }
        },
        drawCallback: function () {
            //$(this).find('tbody tr').slice(-3).find('.dropdown, .btn-group').addClass('dropup');
            //$(this).find('tbody thead').remove();
        },
        preDrawCallback: function() {
            //$(this).find('tbody tr').slice(-3).find('.dropdown, .btn-group').removeClass('dropup');
        },
        initComplete: function() {
            $(this).find('thead').remove();
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

    $('.datatable-basic tbody').on('click', 'td:not(.checkboxcolumn):not(.detailedItem)', function () {

        var tr = $(this).closest('tr');
        var row = mainTable.row( tr );
        var attr = tr.data("images");
        debugger;
        if (typeof attr !== typeof undefined && attr !== false)
        {
            if ( row.child.isShown() ) {
                // This row is already open - close it
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                // Open this row
                row.child(format(attr,mainTable.row(tr).data()),'detailedItem' ).show();
                tr.addClass('shown');             
            }        
        }
    });


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
    
}
