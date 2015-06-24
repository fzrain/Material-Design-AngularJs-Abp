materialAdmin 

    // =========================================================================
    // INPUT FEILDS MODIFICATION
    // =========================================================================

    //Add blue animated border and remove with condition when focus and blur

    .directive('fgLine', function(){
        return {
            restrict: 'C',
            link: function(scope, element) {
                if($('.fg-line')[0]) {
                    $('body').on('focus', '.form-control', function(){
                        $(this).closest('.fg-line').addClass('fg-toggled');
                    })

                    $('body').on('blur', '.form-control', function(){
                        var p = $(this).closest('.form-group');
                        var i = p.find('.form-control').val();

                        if (p.hasClass('fg-float')) {
                            if (i.length == 0) {
                                $(this).closest('.fg-line').removeClass('fg-toggled');
                            }
                        }
                        else {
                            $(this).closest('.fg-line').removeClass('fg-toggled');
                        }
                    });
                }
    
            }
        }
        
    })

    

    // =========================================================================
    // AUTO SIZE TEXTAREA
    // =========================================================================
    
    .directive('autoSize', function(){
        return {
            restrict: 'A',
            link: function(scope, element){
                if (element[0]) {
                   autosize(element);
                }
            }
        }
    })
    

    // =========================================================================
    // BOOTSTRAP SELECT
    // =========================================================================

    .directive('selectPicker', function(){
        return {
            restrict: 'A',
            link: function(scope, element, attrs) {
                //if (element[0]) {
                    element.selectpicker();
                //}
            }
        }
    })
    


    // =========================================================================
    // CHOSEN
    // =========================================================================
    
    .directive('tagSelect', function(){
        return {
            restrict: 'A',
            link: function(scope, element) {
                if (element[0]) {
                    element.chosen({
                        width: '100%',
                        allow_single_deselect: true
                    });
                }
            }
        }
    })

    

    // =========================================================================
    // INPUT MASK
    // =========================================================================

    .directive('inputMask', function(){
        return {
            restrict: 'A',
            scope: {
              inputMask: '='
            },
            link: function(scope, element){
                element.mask(scope.inputMask.mask);
            }
        }
    })


    
    // =========================================================================
    // NO UI SLIDER 
    // =========================================================================

    //Basic

    .directive('inputSlider', function(){
        return {
            restrict: 'C',
            link: function(scope, element, attrs) {
                var isStart = attrs.isStart;
                
                element.noUiSlider({
                    start: isStart,
                    range: {
                        'min': 0,
                        'max': 100,
                    }
                });
            }
        }
    })

    //Range

    .directive('inputSliderRange', function(){
        return {
            restrict: 'C',
            link: function(scope, element, attrs) {                
                element.noUiSlider({
                    start: [30, 60],
                    range: {
                        'min': 0,
                        'max': 100
                    },
                    connect: true
                });
            }
        }
    })
    
    //Values

    .directive('inputSliderValues', function(){
        return {
            restrict: 'C',
            link: function(scope, element, attrs) {
                element.noUiSlider({
                    start: [ 45, 80 ],
                    connect: true,
                    direction: 'rtl',
                    behaviour: 'tap-drag',
                    range: {
                        'min': 0,
                        'max': 100
                    }
                });

                $('.input-slider-values').Link('lower').to($('#value-lower'));
                $('.input-slider-values').Link('upper').to($('#value-upper'), 'html');
                
            }
        }
    })


    
    // =========================================================================
    // DATE TIME PICKER
    // =========================================================================

    .directive('dtPicker', function(){
        return {
            require : '?ngModel',
            restrict: 'A',
            scope: {
                viewMode: '@',
                format: '@'
            },
            link: function(scope, element, attrs, ngModel){
                element.datetimepicker({
                    viewMode: scope.viewMode,
                    format: scope.format
                })
                .on('dp.change', function (e) {
                    // datepick doesn't update the value of the ng-model when the date is changed
                    // when date changed event is triggered 
                    // retreive the value of the new date
                    // set the value to the ng-model 
                    ngModel.$setViewValue($(element).val());
                });   
            }
        }
    })




    
    // =========================================================================
    // COLOR PICKER
    // =========================================================================

    .directive('colorPicker', function(){
        return {
            restrict: 'A',
            link: function(scope, element, attrs) {
                $(element).each(function(){
                    var colorOutput = $(this).closest('.cp-container').find('.cp-value');
                    $(this).farbtastic(colorOutput);
                });
                
            }
        }
    })


    
    // =========================================================================
    // SUMMERNOTE HTML EDITOR
    // =========================================================================

    //Basic

    .directive('htmlEditor', function(){
        return {
            restrict: 'A',
            link: function(scope, element, attrs) {
                element.summernote({
                    height: 150
                });
            }
            
        }
    })

    //Edit and Save

    .directive('hecButton', function(){
        return {
            restrict: 'A',
            link: function(scope, element, attrs) {
                element.on('click', function(){
                    $('.hec-click').summernote({
                        focus: true
                    })
                    
                    $('.hec-save').show();
                })
                
                $('.hec-save').on('click', function(){
                    $('.hec-click').destroy();
                    $('.hec-save').hide();
                })
            }
        }
    })

    //Air Mode

    .directive('hecAirmod', function(){
        return {
            restrict: 'A',
            link: function(scope, element, attrs){
                element.summernote({
                    airMode: true
                })
            }
        }
    })



    // =========================================================================
    // PLACEHOLDER FOR IE 9 (on .form-control class)
    // =========================================================================

    .directive('formControl', function(){
        return {
            restrict: 'C',
            link: function(scope, element, attrs) {
                if(angular.element('html').hasClass('ie9')) {
                    $('input, textarea').placeholder({
                        customClass: 'ie9-placeholder'
                    });
                }
            }
            
        }
    })
