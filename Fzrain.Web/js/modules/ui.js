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

    
    //Chosen 

    .directive('tabNav', ['nicescrollService', function(nicescrollService){
        return {
            restrict: 'C',
            link: function(scope, element) {
        
                if (!$('html').hasClass('ismobile')) {                    
                    nicescrollService.niceScroll(element, 'rgba(0,0,0,0.3)', '1px');
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
                    nicescrollService.niceScroll(element, 'rgba(0,0,0,0.3)', '1px');
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
                Waves.attach(element);
                Waves.init();
            }
        }
    })
    
    //Wave buttons for .btn-wave class
    .directive('btnWave', function(){
        return {
            restrict: 'C',
            link: function(scope, element) {
                Waves.attach(element);
                Waves.init();
            }
        }
    })