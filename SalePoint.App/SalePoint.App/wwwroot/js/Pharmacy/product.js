$(document).ready(() => {
    $('#tableProducts').DataTable({
        'language': language,
        'destroy': true,
        scrollX: true,
        scrollCollapse: true,
        paging: true,
        fixedColumns: {
            left: 4,
            right: 1
        },
        lengthMenu: [
            [5, 15, 40, -1],
            [5, 15, 40, 'Todos']
        ],
        columns: [
            { data: 'barCode' },
            { data: 'name' },
            { data: 'description' },
            { data: 'expirationDate' },
            { data: 'measurementUnit' },
            { data: 'stock' },
            { data: 'minimumStock' },
            { data: 'purchasePrice', render: $.fn.dataTable.render.number(',', '.', 2, '$') },
            { data: 'salesPrice1', render: $.fn.dataTable.render.number(',', '.', 2, '$') },
            { data: 'percentageProfit1', render: $.fn.dataTable.render.number(',', '.', 2, '', '%') },
            { data: 'revenue1', render: $.fn.dataTable.render.number(',', '.', 2, '$') },
            { data: 'salesPrice2', render: $.fn.dataTable.render.number(',', '.', 2, '$') },
            { data: 'percentageProfit2', render: $.fn.dataTable.render.number(',', '.', 2, '', '%') },
            { data: 'revenue2', render: $.fn.dataTable.render.number(',', '.', 2, '$') },
            { data: 'wholesale' },
            { data: 'departmentName' },
            { data: '' }
        ]
    });

    $('#expirationDate').attr('min', getDateYYYYMMDD());
    $('#loadingPage').fadeIn(1);
    buildTableProducts();
});

$('body').delegate('.btnDeleteProduct', 'click', (e) => {
    let idProduct = parseInt(e.currentTarget.getAttribute('idProduct'));

    Swal.fire({
        title: '¿Estas seguro de eliminar?',
        text: "¡Los cambios no podran revertirse!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#0d6efd',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Aceptar',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {

            $('#loadingPage').fadeIn(1);

            deleteProduct(idProduct).then((res) => {
                if (res != null && res > 0) {
                    buildTableProducts();
                    dinamycTimerAlert({ title: '¡Eliminado correctamente!', text: 'El producto ha sido eliminado', type: 'success' });
                }
                else {
                    $('#loadingPage').fadeOut(1);
                    dinamicAlert({ title: '¡Ocurrio un error!', text: 'Por favor recargue la pagína e intente nuevamente', type: 'error' });
                }
            });
        }
    });
});

$('body').delegate('.btnUpdateProduct', 'click', (e) => {

    $('#loadingPage').fadeIn(1);
    e.currentTarget.disabled = true;
    let productId = e.currentTarget.getAttribute('idProduct');

    getProductById(productId).then((product) => {
        if (product != null) {
            buildModalProductEdit(product);
        }
        else {
            $('#loadingPage').fadeOut(1);
            dinamicAlert({ title: '¡Ocurrio un error!', text: 'Por favor recargue la pagína e intente nuevamente', type: 'error' });
        }
    });

});

$('body').delegate('#btnSaveProduct', 'click', (e) => {

    let applyWholesale = document.getElementById('applyWholesale').checked;

    let product = {
        "id": $('#productId').val() > 0 ? parseInt($('#productId').val()) : 0,
        "name": $('#nameProduct').val(),
        "barCode": $('#barCode').val(),
        "expirationDate": $('#expirationDate').val() == '' ? null : $('#expirationDate').val().split("/").reverse().join("-"),
        "description": $('#description').val(),
        "stock": Math.round($('#stock').val() * 100) / 100,
        "minimumStock": Math.round($('#minimumStock').val() * 100) / 100,
        "purchasePrice": Math.round($('#purchasePrice').val() * 100) / 100,
        "thumbnail": null,
        "unitMeasureId": parseInt($('#measurementUnit').val()),
        "isActive": true,
        "creationDate": null,
        "modificationDate": null,
        "deletionDate": null,
        "userId": 0,
        "department": {
            "id": parseInt($('#department').val()),
            "name": null,
            "isActive": true,
            "creationDate": null,
            "modificationDate": null,
            "deletionDate": null
        },
        "productDepartment": {
            "id": 0,
            "productId": 0,
            "departmentId": 0
        },
        "measurementUnit": {
            "id": parseInt($('#measurementUnit').val()),
            "name": null,
            "icon": null,
            "creationDate": null,
            "modificationDate": null,
            "deletionDate": null,
            "isActive": true
        },
        "priceProducts": [
            {
                "id": 0,
                "productId": $('#productId').val() > 0 ? parseInt($('#productId').val()) : 0,
                "salesPrice": $('#salesPrice1').val() > 0 ? Math.round($('#salesPrice1').val() * 100) / 100 : 0,
                "percentageProfit": $('#percentageProfit1').val() > 0 ? Math.round($('#percentageProfit1').val() * 100) / 100 : 0,
                "revenue": $('#revenue1').val() > 0 ? Math.round($('#revenue1').val() * 100) / 100 : 0,
                "wholesale": 1
            }
        ]
    };

    if (applyWholesale) {
        product.priceProducts.push(
            {
                "id": 0,
                "productId": $('#productId').val() > 0 ? parseInt($('#productId').val()) : 0,
                "salesPrice": $('#salesPrice2').val() > 0 ? Math.round($('#salesPrice2').val() * 100) / 100 : 0,
                "percentageProfit": $('#percentageProfit2').val() > 0 ? Math.round($('#percentageProfit2').val() * 100) / 100 : 0,
                "revenue": $('#revenue2').val() > 0 ? Math.round($('#revenue2').val() * 100) / 100 : 0,
                "wholesale": $('#wholesale2').val() > 0 ? Math.round($('#wholesale2').val() * 100) / 100 : 0
            });
    }

    if (validateFormSaveProduct()) {
        $('#loadingPage').fadeIn(1);

        if (product.id == 0) {

            createProduct(product).then((productId) => {

                if (productId != null && productId > 0) {
                    buildTableProducts();
                    $('#btnCloseProductModal').click();
                    dinamycTimerAlert({ title: '¡Creado correctamente!', text: 'El producto fue creado', type: 'success' });
                }
                else {
                    $('#loadingPage').fadeOut(1);
                    dinamicAlert({ title: '¡Ocurrio un error!', text: 'Por favor recargue la pagína e intente nuevamente', type: 'error' });
                    $('#productModal').modal('show');
                }
            });
        }
        else {

            updateProduct(product).then((productId) => {

                if (productId != null && productId > 0) {
                    buildTableProducts();
                    $('#btnCloseProductModal').click();
                    dinamycTimerAlert({ title: '¡Actualizado correctamente!', text: 'El producto fue actualizado', type: 'success' });
                }
                else {
                    $('#loadingPage').fadeOut(1);
                    dinamicAlert({ title: '¡Ocurrio un error!', text: 'Por favor recargue la pagína e intente nuevamente', type: 'error' });
                    $('#productModal').modal('show');
                }
            });
        }
    }
});

$('body').delegate('#btnCloseProductModal', 'click', (e) => {
    $('#productModal').modal('hide');
    $('.btnUpdateProduct').attr('disabled', false);
});

$('body').delegate('#minimumStock', 'focusout', (e) => {
    let stock = $('#stock').val() == '' ? 0 : parseInt($('#stock').val());
    let minimumStock = e.currentTarget.value == '' ? 0 : parseInt(e.currentTarget.value);

    if (stock < minimumStock) {
        Swal.fire({
            position: 'top-end',
            icon: 'info',
            title: '¡Atención!',
            text: 'La existencia minima no puede ser mayor a las existencias',
            showConfirmButton: false,
            timer: 3000
        });
        e.currentTarget.focus();
    }
});

$('body').delegate('#btnCreateProduct', 'click', (e) => {
    //$('#barCode').attr('disabled', false);
    $('#titleProductModal').text('Crear producto');
    clearFormSaveProduct();
    buildSelectDepartments(0);
    buildSelectMeasurementUnits(0);
    $('#productModal').modal('show');
});

$('body').delegate('.btnAddStockProduct', 'click', (e) => {
    let idProduct = e.currentTarget.getAttribute('idProduct');
    $('#loadingPage').fadeIn(1);

    getProductById(idProduct).then((res) => {
        if (res != null) {
            $('#nameStockProduct').text(res.name);
            $('#btnAddStock').attr('data-id', res.id);
            $('#actuallyStockProduct').val(res.stock);
            $('#newStockProduct').val('');
            $('#AddStockProductModal').modal('show');
            $('#loadingPage').fadeOut(1);
        }
        else {
            $('#loadingPage').fadeOut(1);
            dinamicAlert({ title: '¡Ocurrio un error!', text: 'Por favor recargue la pagína e intente nuevamente', type: 'error' });
        }
    });
});

$('body').delegate('#newStockProduct', 'keypress', (e) => {
    if (e.key === "Enter")
        $('#btnAddStock').click();
});

$('body').delegate('#btnAddStock', 'click', (e) => {
    let idProduct = e.currentTarget.getAttribute('data-id');
    let stock = $('#newStockProduct').val();

    if (stock != '' && stock > 0) {

        $('#loadingPage').fadeIn(1);

        UpdateStockProduct(idProduct, stock).then((res) => {

            if (res) {
                dinamycTimerAlert({ title: '¡Existencias actualizadas!', text: 'Las existencias fuerón agregadas con éxito.', type: 'success' });
                buildTableProducts();
                $('#AddStockProductModal').modal('hide');
            }
            else {
                $('#loadingPage').fadeOut(1);
                dinamicAlert({ title: '¡Ocurrio un error!', text: 'Por favor recargue la pagína e intente nuevamente', type: 'error' });
            }

        });
    }
    else {
        dinamycTimerAlert({ title: '¡Existencias!', text: 'Es necesario que las existencias que se van a añadir sean mayores a 0.', type: 'error' });
    }
});

$('body').delegate('#applyWholesale', 'change', (e) => {
    let isActive = e.currentTarget.checked;
    let sectionWholesale = document.getElementById('sectionWholesale');

    if (isActive)
        sectionWholesale.classList.remove('disabled-section');
    else {
        sectionWholesale.classList.add('disabled-section');
    }
});

$('body').delegate('#salesPrice1', 'keyup', (e) => {
    let salesPrice1 = e.currentTarget;
    let purchasePrice = $('#purchasePrice').val() != '' ? Math.round($('#purchasePrice').val() * 100) / 100 : '';

    if (salesPrice1.value != '' && salesPrice1.value > 0) {

        let salePrice = Math.round(salesPrice1.value * 100) / 100;

        if (purchasePrice != '' && purchasePrice < salePrice) {

            //$('#percentageProfit1').val(Math.round((salePrice - purchasePrice) / salePrice * 100).toFixed(2)); //porcentaje con 2 decimales
            $('#percentageProfit1').val(Math.round((salePrice - purchasePrice) / salePrice * 100)); //porcentaje redondeado 0 decimales
            $('#revenue1').val(Math.round((salePrice - purchasePrice) * 100) / 100);
        }
    }
});

$('body').delegate('#percentageProfit1', 'keyup', (e) => {
    if (e.currentTarget.value > 99)
        e.currentTarget.value = 99;

    let percentageProfit1 = e.currentTarget.value != '' && e.currentTarget.value > 0 ? e.currentTarget.value : '';
    let purchasePrice = $('#purchasePrice').val() != '' && $('#purchasePrice').val() > 0 ? Math.round($('#purchasePrice').val() * 100) / 100 : '';

    if (percentageProfit1 != '') {

        if (purchasePrice != '') {

            $('#salesPrice1').val(Math.round((purchasePrice / (1 - (percentageProfit1 / 100))) * 100) / 100);
            $('#revenue1').val(Math.round(($('#salesPrice1').val() - purchasePrice) * 100) / 100);
        }
        else {
            e.currentTarget.value = '';
            dinamicAlert({ title: '¡Precio de compra!', text: 'El precio de compra debe ser mayor a 0 para calcular el precio de venta.', type: 'warning' });
        }
    }
    else {
        dinamicAlert({ title: '¡Porcentaje de utilidad!', text: 'El porcentaje de utilidad debe ser mayor a 0.', type: 'warning' });
    }
});

$('body').delegate('#salesPrice2', 'keyup', (e) => {
    let salesPrice2 = e.currentTarget;
    let purchasePrice = $('#purchasePrice').val() != '' ? Math.round($('#purchasePrice').val() * 100) / 100 : '';

    if (salesPrice2.value != '' && salesPrice2.value > 0) {

        let salePrice = Math.round(salesPrice2.value * 100) / 100;

        if (purchasePrice != '' && purchasePrice < salePrice) {

            //$('#percentageProfit2').val(Math.round((salePrice - purchasePrice) / salePrice * 100).toFixed(2)); //porcentaje con 2 decimales
            $('#percentageProfit2').val(Math.round((salePrice - purchasePrice) / salePrice * 100)); //porcentaje redondeado 0 decimales
            $('#revenue2').val(Math.round((salePrice - purchasePrice) * 100) / 100);
        }
    }
});

$('body').delegate('#percentageProfit2', 'keyup', (e) => {
    if (e.currentTarget.value > 99)
        e.currentTarget.value = 99;

    let percentageProfit2 = e.currentTarget.value != '' && e.currentTarget.value > 0 ? e.currentTarget.value : '';
    let purchasePrice = $('#purchasePrice').val() != '' && $('#purchasePrice').val() > 0 ? Math.round($('#purchasePrice').val() * 100) / 100 : '';


    if (percentageProfit2.value != '') {
        if (purchasePrice != '') {
            //$('#salesPrice2').val(Math.round(purchasePrice / (1 - (percentageProfit2.value / 100))).toFixed(2)); //porcentaje con 2 decimales
            $('#salesPrice2').val(Math.round(purchasePrice / (1 - (percentageProfit2 / 100)))); //porcentaje redondeado 0 decimales
            $('#revenue2').val(Math.round($('#salesPrice2').val() - purchasePrice));
        }
        else {
            dinamicAlert({ title: '¡Atención!', text: 'El precio de venta debe ser mayor al precio de compra.', type: 'warning' });
        }
    }
});

const buildTableProducts = () => {

    getAllProducts().then((products) => {

        if (products != null && products.length > 0) {

            let tableProducts = $('#tableProducts').DataTable();
            let data = new Array();

            products.map((item) => {

                let btnUpdateProduct = document.createElement('button');
                btnUpdateProduct.classList.add('btn', 'btn-secondary', 'mr-1', 'btnUpdateProduct');

                let iconUpdate = document.createElement('i');
                iconUpdate.classList.add('fa-solid', 'fa-pen');

                btnUpdateProduct.append(iconUpdate);
                btnUpdateProduct.setAttribute('idProduct', item.productId);

                let btnDeleteProduct = document.createElement('button');
                btnDeleteProduct.classList.add('btn', 'btn-danger', 'btnDeleteProduct');

                let iconDelete = document.createElement('i');
                iconDelete.classList.add('fa-solid', 'fa-trash');

                btnDeleteProduct.append(iconDelete);
                btnDeleteProduct.setAttribute('idProduct', item.productId);


                let btnAddStockProduct = document.createElement('button');
                btnAddStockProduct.classList.add('btn', 'btn-info', 'btnAddStockProduct');

                let iconAdd = document.createElement('i');
                iconAdd.classList.add('fa-solid', 'fa-plus', 'text-white');

                btnAddStockProduct.append(iconAdd);
                btnAddStockProduct.setAttribute('idProduct', item.productId);

                data.push({
                    'barCode': item.barCode,
                    'name': item.nameProduct,
                    'description': item.description,
                    'expirationDate': item.expirationDate != null ? new Date(item.expirationDate).toLocaleDateString() : '',
                    'measurementUnit': item.icon,
                    'stock': item.stock,
                    'minimumStock': item.minimumStock,
                    'purchasePrice': item.purchasePrice,
                    'salesPrice1': item.salesPrice1,
                    'percentageProfit1': item.percentageProfit1,
                    'revenue1': item.revenue1,
                    'salesPrice2': item.salesPrice2 > 0 ? item.salesPrice2 : '',
                    'percentageProfit2': item.percentageProfit2 > 0 ? item.percentageProfit2 : '',
                    'revenue2': item.revenue2 > 0 ? item.revenue2 : '',
                    'wholesale': item.wholesale2 > 0 ? `${item.wholesale2} <samllz${item.icon}</small>` : '',
                    'departmentName': '',
                    '': `${btnAddStockProduct.outerHTML} ${btnUpdateProduct.outerHTML} ${btnDeleteProduct.outerHTML}`
                });
            });

            tableProducts.clear().draw();
            tableProducts.rows.add(data); // Add new data
            tableProducts.columns.adjust().draw(); // Redraw the DataTable
        }
        else {
            $('#tableProducts').DataTable().clear().draw();
        }

        $('#loadingPage').fadeOut(1);
    });
}

const buildModalProductEdit = (product) => {
    $('#titleProductModal').text('Modificar producto');
    $('#nameProduct').val(product.name);
    $('#barCode').val(product.barCode);
    $('#expirationDate').val(product.expirationDate != null ? getDateDDMMYYYYHHMMByDateTime(product.expirationDate) : '');
    $('#description').val(product.description);
    $('#stock').val(product.stock);
    $('#minimumStock').val(product.minimumStock);
    $('#purchasePrice').val(product.purchasePrice);

    //Costos menudeo
    $('#salesPrice1').val(product.priceProducts[0].salesPrice);
    $('#percentageProfit1').val(product.priceProducts[0].percentageProfit);
    $('#revenue1').val(product.priceProducts[0].revenue);
    $('#wholesale1').val(1);

    //Check aplica mayoreo 
    if (product.priceProducts.length > 1) {
        $('#applyWholesale').prop('checked', true);
        $('#sectionWholesale').removeClass('disabled-section');

        //Costos mayoreo
        $('#salesPrice2').val(product.priceProducts[1].salesPrice);
        $('#percentageProfit2').val(product.priceProducts[1].percentageProfit);
        $('#revenue2').val(product.priceProducts[1].revenue);
        $('#wholesale2').val(product.priceProducts[1].wholesale);
    }
    else {
        $('#applyWholesale').prop('checked', false);
        $('#sectionWholesale').addClass('disabled-section');

        //Costos mayoreo
        $('#salesPrice2').val('');
        $('#percentageProfit2').val('');
        $('#revenue2').val('');
        $('#wholesale2').val('');
    }

    $('#creationDate').val(product.creationDate);
    $('#modificationDate').val(product.modificationDate);
    $('#deletionDate').val(product.deletionDate);
    $('#userId').val(product.userId);
    $('#isActive').val(product.isActive);
    $('#productId').val(product.id);
    buildSelectDepartments(product.department.id);
    buildSelectMeasurementUnits(product.measurementUnit.id);
    $('#loadingPage').fadeOut(1);

}

const buildSelectDepartments = (departmentId) => {

    getAllDepartments().then((departments) => {

        if (departments != null && departments.length > 0) {
            let department = document.getElementById('department');
            department.innerHTML = '';

            if (departmentId == 0) {
                let option = document.createElement('option');
                option.value = 0;
                option.innerText = 'Seleccionar un departamento';
                department.appendChild(option);
            }

            departments.map((item) => {
                let option = document.createElement('option');
                option.value = item.id;
                option.innerText = item.name;

                if (departmentId > 0 && item.id == departmentId)
                    option.selected = true;

                department.appendChild(option);
            });

            $('#productModal').modal('show');
        }
    });
}

const buildSelectMeasurementUnits = (measurementUnitId) => {

    getMeasurementUnits().then((measurementUnits) => {

        if (measurementUnits != null && measurementUnits.length > 0) {

            let measurementUnit = document.getElementById('measurementUnit');
            measurementUnit.innerHTML = '';

            if (measurementUnitId == 0) {
                let option = document.createElement('option');
                option.value = 0;
                option.innerText = 'Seleccionar una unidad de medida';
                measurementUnit.appendChild(option);
            }

            measurementUnits.map((item) => {
                let option = document.createElement('option');
                option.value = item.id;
                option.innerText = item.name;

                if (measurementUnitId > 0 && item.id == measurementUnitId)
                    option.selected = true;

                measurementUnit.appendChild(option);
            });

            $('#productModal').modal('show');
        }
    });
}

const validateFormSaveProduct = () => {
    let nameProduct = $('#nameProduct').val();
    let barCode = $('#barCode').val();
    let description = $('#description').val();
    let expirationDate = $('#expirationDate').val();
    let stock = $('#stock').val();
    let minimumStock = $('#minimumStock').val();
    let purchasePrice = $('#purchasePrice').val();
    let department = $('#department').val();
    let measurementUnit = $('#measurementUnit').val();
    let salesPrice1 = $('#salesPrice1').val();
    let percentageProfit1 = $('#percentageProfit1').val();
    let revenue1 = $('#revenue1').val();
    let wholesale1 = $('#wholesale1').val();
    let salesPrice2 = $('#salesPrice2').val();
    let percentageProfit2 = $('#percentageProfit2').val();
    let revenue2 = $('#revenue2').val();
    let wholesale2 = $('#wholesale2').val();
    let applyWholesale = document.getElementById('applyWholesale').checked;
    let response = true;

    if (nameProduct == '') {
        dinamycTimerAlert({ title: '¡Nombre del producto!', text: 'El nombre del producto es requerido', type: 'error' });
        return !response;
    }

    //if (barCode == '') {
    //    dinamycTimerAlert({ title: '¡Codigo de barras!', text: 'El codigo de barras es requerido', type: 'error' });
    //    response = false;
    //}

    if (description == '') {
        dinamycTimerAlert({ title: '¡Descripción!', text: 'La descripción es requerida', type: 'error' });
        return !response;
    }

    if (expirationDate != '') {
        if (expirationDate < getDateYYYYMMDD()) {
            dinamycTimerAlert({ title: '¡Fecha de caducidad!', text: 'La fecha de caducidad no puede ser menor al dia actual.', type: 'error' });
            return !response;
        }
    }

    if (stock == '' || stock < 0.1) {
        dinamycTimerAlert({ title: '¡Cantidad!', text: 'La cantidad es requerida', type: 'error' });
        return !response;
    }

    if (minimumStock == '' || minimumStock < 1) {
        dinamycTimerAlert({ title: '¡Existencia minima!', text: 'La existencia minima es requerida', type: 'error' });
        return !response;
    }

    if (purchasePrice == '' || purchasePrice < 1) {
        dinamycTimerAlert({ title: '¡Precio de compra!', text: 'El precio de compra es requerido', type: 'error' });
        return !response;
    }

    if (department == '' || department < 1) {
        dinamycTimerAlert({ title: '¡Departamento!', text: 'El departamento es requerido', type: 'error' });
        return !response;
    }

    if (measurementUnit == '' || measurementUnit < 1) {
        dinamycTimerAlert({ title: '¡Unidad de medida!', text: 'La unidad de medida es requerida.', type: 'error' });
        return !response;
    }

    if (salesPrice1 == '' || salesPrice1 < 1) {
        dinamycTimerAlert({ title: '¡Precio de venta!', text: 'El precio de venta es requerido.', type: 'error' });
        return !response;
    }

    if (percentageProfit1 == '' || percentageProfit1 < 1) {
        dinamycTimerAlert({ title: '¡Porcentaje de utilidad!', text: 'El porcentaje de utilidad es requerido.', type: 'error' });
        return !response;
    }

    if (revenue1 == '' || revenue1 < 1) {
        dinamycTimerAlert({ title: '¡Ganancia!', text: 'La ganancia es requerida.', type: 'error' });
        return !response;
    }

    if (applyWholesale) {
        if (salesPrice2 == '' || salesPrice2 < 1) {
            dinamycTimerAlert({ title: '¡Precio de venta!', text: 'El precio de venta es requerido.', type: 'error' });
            return !response;
        }

        if (percentageProfit2 == '' || percentageProfit2 < 1) {
            dinamycTimerAlert({ title: '¡Porcentaje de utilidad!', text: 'El porcentaje de utilidad es requerido.', type: 'error' });
            return !response;
        }

        if (revenue2 == '' || revenue2 < 1) {
            dinamycTimerAlert({ title: '¡Ganancia!', text: 'La ganancia es requerida.', type: 'error' });
            return !response;
        }

        if (wholesale2 == '' || wholesale2 < 2) {
            dinamycTimerAlert({ title: '¡Cantidad mayoreo!', text: 'La cantidad de productos al mayoreo es requerida y debe ser mayor a 1.', type: 'error' });
            return !response;
        }
    }

    return response;
}

const clearFormSaveProduct = () => {
    $('#nameProduct').val('');
    $('#barCode').val('');
    $('#expirationDate').val('');
    $('#description').val('');
    $('#stock').val('');
    $('#minimumStock').val('');
    $('#purchasePrice').val('');

    //Costos menudeo
    $('#salesPrice1').val('');
    $('#percentageProfit1').val('');
    $('#revenue1').val('');
    $('#wholesale1').val('1');
    //Costos mayoreo
    $('#salesPrice2').val('');
    $('#percentageProfit2').val('');
    $('#revenue2').val('');
    $('#wholesale2').val('');

    $('#creationDate').val('');
    $('#modificationDate').val('');
    $('#deletionDate').val('');
    $('#isActive').val('');
    $('#productId').val(0);
    $('#department').val(0);
}

const dinamicAlert = (settings) => {
    Swal.fire({
        title: settings.title,
        text: settings.text,
        icon: settings.type,
        showCancelButton: false,
        confirmButtonColor: '#0d6efd',
        confirmButtonText: 'Aceptar'
    })
}

const dinamycTimerAlert = (settings) => {
    Swal.fire({
        position: 'top-end',
        icon: settings.type,
        title: settings.title,
        text: settings.text,
        showConfirmButton: false,
        timer: 3500
    });
}

const getDateYYYYMMDD = () => {
    const today = new Date().toLocaleDateString().split('/');
    const yyyy = today[2];
    let mm = today[1] < 10 ? `0${today[1]}` : today[1];
    let dd = today[0] < 10 ? `0${today[0]}` : today[0];

    return `${yyyy}-${mm}-${dd}`;
}

const getDateDDMMYYYYHHMMByDateTime = (dateTime) => {
    dateTime = new Date(dateTime);

    //let UTCdate = new Date(dateTime + 'Z');

    //if (UTCdate.getFullYear() == 0)
    //    return '';

    if (dateTime.getFullYear().toString().length != 4)
        return '';

    let localDate = dateTime.toLocaleString().replace(',', '').split(' '); //UTCdate.toLocaleString().replace(',', '').split(' ');
    let date = localDate[0].split('/');
    let hours = localDate[1].split(':');

    let day = parseInt(date[0], 10);
    let month = parseInt(date[1], 10); + 1;
    let year = parseInt(date[2], 10);

    let dformat = [day < 10 ? '0' + day : day, month < 10 ? '0' + month : month, year].reverse().join('-');

    return dformat;
}

/* * *  Call API  * */

const getAllProducts = async () => {
    let data = new Array();
    const url = new URL(`${window.location.href}/GetAllProducts`);

    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    data = await response.json();

    return data;
}

const getProductById = async (productId) => {
    let data = '';
    const url = new URL(`${window.location.href}/GetProductById`);
    url.searchParams.set('productId', productId);

    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    data = await response.json();

    return data;
}

const createProduct = async (product) => {
    let data = '';
    const url = new URL(`${window.location.href}/CreateProduct`);

    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(product)
    });

    data = await response.json();

    return data;
}

const updateProduct = async (product) => {
    let id = '';
    const url = new URL(`${window.location.href}/UpdateProduct`);

    const response = await fetch(url, {
        method: 'PUT',
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        },
        body: JSON.stringify(product)
    });

    id = await response.json();

    return id;
}

const UpdateStockProduct = async (productId, stock) => {
    let id = '';
    const url = new URL(`${window.location.href}/UpdateStockProduct`);
    url.searchParams.set('productId', productId);


    const response = await fetch(url, {
        method: 'PUT',
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        },
        body: JSON.stringify(stock)
    });

    id = await response.json();

    return id;
}

const deleteProduct = async (productId) => {
    let id = '';
    const url = new URL(`${window.location.href}/DeleteProduct`);
    url.searchParams.set('productId', productId);

    const response = await fetch(url, {
        method: 'DELETE',
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        }
    });

    id = await response.json();
    return id;
}

const getAllDepartments = async () => {
    let data = new Array();
    const url = new URL(`${window.location.href}/GetAllDepartments`);

    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    data = await response.json();
    return data;
}

const getMeasurementUnits = async () => {
    let data = new Array();
    const url = new URL(`${window.location.origin}/Catalog/GetMeasurementUnit`);

    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    data = await response.json();
    return data;
}