materialAdmin
    
    //-----------------------------------------------------
    // TOOLTIP AND POPOVER
    //-----------------------------------------------------

    .directive('toggle', function(){
        return {
            restrict: 'A',
            link: function(scope, element, attrs) {
                var x = attrs.toggle;
                
                //Tooltip
                if(x === 'tooltip') {
                    element.tooltip();
                } 
                
                //Popover
                if(x === 'popover') {
                    element.popover();
                } 
            }
        };
    })

    
    
    //-----------------------------------------------------
    // COLLAPSE
    //-----------------------------------------------------
    .directive('collapse', function(){
        return {
            restrict: 'C',
            link: function(scope, element, attrs) {
                element.on('show.bs.collapse', function (e) {
                    $(this).closest('.panel').find('.panel-heading').addClass('active');
                });

                element.on('hide.bs.collapse', function (e) {
                    $(this).closest('.panel').find('.panel-heading').removeClass('active');
                });

                //Add active class for pre opened items
                $('.collapse.in').each(function(){
                    $(this).closest('.panel').find('.panel-heading').addClass('active');
                });
            }
        };
    })



    //-----------------------------------------------------
    // ANIMATED DROPDOWN MENU
    //-----------------------------------------------------

    .directive('dropdown', function(){
    
        return {
            restrict: 'C',
            link: function(scope, element, attrs) {
                element.on('shown.bs.dropdown', function (e) {
                    if($(this).data('animation')) {
                        $animArray = [];
                        $animation = attrs.animation;
                        $animArray = $animation.split(',');
                        $animationIn = 'animated '+$animArray[0];
                        $animationOut = 'animated '+ $animArray[1];
                        $animationDuration = '';

                        if(!$animArray[2]) {
                            $animationDuration = 500; //if duration is not defined, default is set to 500ms
                        }

                        else {
                            $animationDuration = $animArray[2];
                        }

                        $(this).find('.dropdown-menu').removeClass($animationOut);
                        $(this).find('.dropdown-menu').addClass($animationIn);
                    }
                });

                element.on('hide.bs.dropdown', function (e) {
                    if($(this).data('animation')) {
                        e.preventDefault();
                        $this = $(this);
                        $dropdownMenu = $this.find('.dropdown-menu');

                        $dropdownMenu.addClass($animationOut);
                        setTimeout(function(){
                            $this.removeClass('open');
                        }, $animationDuration);
                        
                    }
                });
            }
        };
    })


    
    // =========================================================================
    // WEATHER WIDGET
    // =========================================================================

    .directive('weatherWidget', function(){
        return {
            restrict: 'A',
            link: function(scope, element) {
                $.simpleWeather({
                    location: 'Austin, TX',
                    woeid: '',
                    unit: 'f',
                    success: function(weather) {
                        html = '<div class="weather-status">'+weather.temp+'&deg;'+weather.units.temp+'</div>';
                        html += '<ul class="weather-info"><li>'+weather.city+', '+weather.region+'</li>';
                        html += '<li class="currently">'+weather.currently+'</li></ul>';
                        html += '<div class="weather-icon wi-'+weather.code+'"></div>';
                        html += '<div class="dash-widget-footer"><div class="weather-list tomorrow">';
                        html += '<span class="weather-list-icon wi-'+weather.forecast[2].code+'"></span><span>'+weather.forecast[1].high+'/'+weather.forecast[1].low+'</span><span>'+weather.forecast[1].text+'</span>';
                        html += '</div>';
                        html += '<div class="weather-list after-tomorrow">';
                        html += '<span class="weather-list-icon wi-'+weather.forecast[2].code+'"></span><span>'+weather.forecast[2].high+'/'+weather.forecast[2].low+'</span><span>'+weather.forecast[2].text+'</span>';
                        html += '</div></div>';
                        $("#weather-widget").html(html);
                    },
                    error: function(error) {
                        $("#weather-widget").html('<p>'+error+'</p>');
                    }
                });
            }
        };
})

