materialAdmin

    // =========================================================================
    // NICE SCROLL
    // =========================================================================

    //Html

    .directive('html', ['nicescrollService', function(nicescrollService){
        return {
            restrict: 'E',
            link: function(scope, element) {

                if (!element.hasClass('ismobile')) {
                    if (!$('.login-content')[0]) {
                        nicescrollService.niceScroll(element, 'rgba(0,0,0,0.3)', '5px');
                    }
                }
            }
        }
    }])


    //Table

    .directive('tableResponsive', ['nicescrollService', function(nicescrollService){
        return {
            restrict: 'C',
            link: function(scope, element) {

                if (!$('html').hasClass('ismobile')) {
                    nicescrollService.niceScroll(element, 'rgba(0,0,0,0.3)', '5px');
                }
            }
        }
    }])


    //Chosen

    .directive('chosenResults', ['nicescrollService', function(nicescrollService){
        return {
            restrict: 'C',
            link: function(scope, element) {

                if (!$('html').hasClass('ismobile')) {
                    nicescrollService.niceScroll(element, 'rgba(0,0,0,0.3)', '5px');
                }
            }
        }
    }])


    //Tabs

    .directive('tabNav', ['nicescrollService', function(nicescrollService){
        return {
            restrict: 'C',
            link: function(scope, element) {

                if (!$('html').hasClass('ismobile')) {
                    nicescrollService.niceScroll(element, 'rgba(0,0,0,0.3)', '2px');
                }
            }
        }
    }])


    //For custom class

    .directive('cOverflow', ['nicescrollService', function(nicescrollService){
        return {
            restrict: 'C',
            link: function(scope, element) {

                if (!$('html').hasClass('ismobile')) {
                    nicescrollService.niceScroll(element, 'rgba(0,0,0,0.5)', '5px');
                }
            }
        }
    }])


    // =========================================================================
    // WAVES
    // =========================================================================

    // For .btn classes
    .directive('btn', function(){
        return {
            restrict: 'C',
            link: function(scope, element) {
                if(element.hasClass('btn-icon') || element.hasClass('btn-float')) {
                    Waves.attach(element, ['waves-circle']);
                }

                else if(element.hasClass('btn-light')) {
                    Waves.attach(element, ['waves-light']);
                }

                else {
                    Waves.attach(element);
                }

                Waves.init();
            }
        }
    })
