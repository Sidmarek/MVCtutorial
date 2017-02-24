    // globální konstanty

    // globální proměnné
    var fontFamily = "Tahoma",
        d = new Date();
    
    // layout
    var cOff = 0.5,
        posX = cOff,
        posY = cOff,        
        fieldBrake = 5,
        fieldTopAdjust = 50,         
        fieldBottomAdjust = 50,        
        fieldLeftAdjust = 40,
        fieldRightAdjust = 20,
        fieldValWidth = 300,
        fieldBreakVals = 15,
        poleBegtimeOdsazY = 30,
        titleBarHeight = 20,
        iBtnHeight = 20,
        canvasOffsetLeft,
        canvasOffsetTop,
        //nacteno pri inicializaci
        iWidth,
        iHeight,
        chartWidth,
        chartHeight;
        
    // pole konstant - kroky mřížky,délka osy
    var stepGridY = [1,2,5,10,20,50,100,200,500,1000,2000,5000,10000],
        stepGridTime =[5,10,20,30,60,60,120,240,1440,2880],
        timeAxisLength = [0.5,1,2,4,8,12,24,48,168,336],
        viewLength = [720,1080,1440], 

        monthsEN = ["January","February","March","April","May","June","July","August","September","October","November","December"],
        monthsCZ = ["Leden","Únor","Březen","Duben","Květen","Červen","Červenec","Srpen","Září","Říjen","Listopad","Prosinec"],
        monthsDE = ["Januar","Februar","März","April","Mai","Juni","Juli","August","September","Oktober","November","Dezember"],
        monthsPL = ["Styczeń","Luty","Marzec","Kwiecień","Maj","Czerwiec","Lipiec","Sierpień","Wrzesień","Październik","Listopad","Grudzień"],
        monthsRU = ["январь","февраль","март","апрель","май","июнь","июль","август","сентябрь","октябрь","ноябрь","декабрь"],

        weekday = new Array();

        daysEN0 = ["Sun","Mon","Tue","Wed","Thu","Fri","Sat"],
        daysCZ0 = ["Ne","Po","Út","St","Čt","Pá","So"],
        daysDE0 = ["So","Mo","Di","Mi","Do","Fr","Sa"],
        daysPL0 = ["Nie","Po","Wt","Śro","Czw","Pią","So"],
        daysRU0 = ["вс","пн","вт","ср","чт","пт","cб"],        

        daysEN1 = ["Mon","Tue","Wed","Thu","Fri","Sat","Sun"],
        daysCZ1 = ["Po","Út","St","Čt","Pá","So","Ne"],
        daysDE1 = ["Mo","Di","Mi","Do","Fr","Sa","So"],
        daysPL1 = ["Po","Wt","Śro","Czw","Pią","So","Nie"],
        daysRU1 = ["пн","вт","ср","чт","пт","cб","вс"];

    // limity
    var MAX_VIEWS = 30,
        MAX_FIELDS = 9,
        MAX_SIGNALS = 16,
        MAX_MULTITEXTS = 19,
        MAX_HORIZ_LINE = 5,
        MAX_CLICKMAPS = 35;    

    // menu
    var factory,        
        utcBias = 0,
        lang = 0,        
        timeAxisIdx = 7,
        beginTime,
        markerTime;

    // fonty
    var fontTitle = 'bold 12px MS Sans Serif',
        fontLegend = 'normal 11px MS Sans Serif',
        fontVals = 'bold 12px MS Sans Serif',
        fontAxis = 'normal 10px MS Sans Serif';

    // barvy    
    var colorWarning = '#FF0000',   
        colorView = '#F0F0F0',       
        colorBackFields = '#000000',
        colorGrid = '#404040',
        colorTitle = '#000000',
        colorBackTime = '#000000',
        colorTime = '#FFFFFF',
        colorFrames = '#000000';
        BLACK = '#000000';
        WHITE = '#FFFFFF';

    var chartMaps;

        // config
        multitexts = {0: {"name": "", "values": []},
                      1: {"name": "", "values": []},
                      2: {"name": "", "values": []},
                      3: {"name": "", "values": []},
                      4: {"name": "", "values": []},
                      5: {"name": "", "values": []},
                      6: {"name": "", "values": []},
                      7: {"name": "", "values": []},
                      8: {"name": "", "values": []},
                      9: {"name": "", "values": []},
                      10: {"name": "", "values": []},
                      11: {"name": "", "values": []},
                      12: {"name": "", "values": []},
                      13: {"name": "", "values": []},
                      14: {"name": "", "values": []},
                      15: {"name": "", "values": []},
                      16: {"name": "", "values": []},
                      17: {"name": "", "values": []},
                      18: {"name": "", "values": []},
                      19: {"name": "", "values": []},
                      20: {"name": "", "values": []},
                      21: {"name": "", "values": []},
                      22: {"name": "", "values": []},
                      23: {"name": "", "values": []},    
                      24: {"name": "", "values": []},
                      25: {"name": "", "values": []},                                            
                      26: {"name": "", "values": []},
                      27: {"name": "", "values": []},                                            
                      28: {"name": "", "values": []},
                      29: {"name": "", "values": []}
                     };
                     
          //nadefinování values
          for (var m=0;m<30;m++) {
            for (var v=0;v<60;v++) {
              multitexts[m]["values"].push({"idx" : -99, "text": ""});
            };
          };

    var titles = new Array();

    var currView;
    var currLang;

    var config;
    var data;

    // operace myši a klávesnice
    var canX,
        canY,
        zoomCanX,
        lastCanX,
        mouseIsDown = 0,
        zoom = false,
        reset = false;

    // event listener
    document.addEventListener("DOMContentLoaded",init,false);

    // inicializace
    function init() {    
      
      // přizpůsobení ploše
      iWidth = windowWidth() - $('.sidenav').width() - 15;                              
      iHeight = windowHeight() - $('.navbar').height() - $('#top_menu').height() - $('.message').height() - $('footer').height() - 10;

      chartWidth = iWidth-fieldLeftAdjust-fieldBreakVals-fieldValWidth-fieldRightAdjust;
      chartHeight = iHeight-fieldTopAdjust-fieldBottomAdjust;
      
      $('#top_menu').css('width',iWidth);
      $('#top_menu').css('position','absolute');  
      $('#top_menu').css('top',$('.navbar').height()+$('.message').height());
      $('#top_menu').css('left',$('.sidenav').width()+8);
      
      $('#calendar').css('left',$('.sidenav').width()+367);
      
      $('#graph_content').css('left',$('.sidenav').width()+7);
      $('#graph_content').css('top',$('.navbar').height()+$('.message').height()+$('#top_menu').height()+6);
      
      $('#active_canvas').attr('width',iWidth+1);
      $('#active_canvas').attr('height',iHeight+1);      
      
      $('#front_canvas').attr('width',iWidth+1);
      $('#front_canvas').attr('height',iHeight+1);
      
      $('#signal_canvas').attr('width',iWidth+1);
      $('#signal_canvas').attr('height',iHeight+1);
      
      $('#back_canvas').attr('width',iWidth+1);
      $('#back_canvas').attr('height',iHeight+1);     

      $('hr').css('position','absolute');
      $('hr').css('top',$('.navbar').height()+$('.message').height()+$('#top_menu').height()+iHeight+14);
      
      $('footer').css('position','absolute');
      $('footer').css('top',$('.navbar').height()+$('.message').height()+$('#top_menu').height()+iHeight+15);      
      
      // index nula vyhrazen pro oblast grafu
      // 1 - 35 pro viditelnost signálu
      chartDef = {0: {"id": ["chart"], "coords": [posX+fieldLeftAdjust,posY+fieldTopAdjust,posX+fieldLeftAdjust+chartWidth,posY+fieldTopAdjust+chartHeight], "offset": 0, "times": [], "values": []},
                   1: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   2: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},                    
                   3: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   4: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   5: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   6: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   7: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   8: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   9: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   10: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   11: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   12: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   13: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   14: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   15: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   16: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   17: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   18: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   19: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   20: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   21: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   22: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   23: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   24: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   25: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   26: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   27: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   28: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   29: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   30: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   31: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   32: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   33: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   34: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                   35: {"id": [""], "coords": [], "visibility": ["true"], "offset": 0, "times": [], "values": []},
                  };

      // aktivní canvas - pro operace myši - kreslení kurzoru
      activeCanvasElement = $("#active_canvas");
      activeCanvas = activeCanvasElement[0].getContext('2d');
      $('#activeCanvas').attr('width',iWidth);
      $('#activeCanvas').attr('height',iHeight);            
      activeCanvasElement[0].addEventListener("mousedown",mouseDown,false);
      activeCanvasElement[0].addEventListener("mousemove",mouseMove,false);
      activeCanvasElement[0].addEventListener("mouseup",mouseUp,false);      
      window.addEventListener("keydown",keyDown,false )
      canvasOffsetLeft = activeCanvasElement.offset().left;
      canvasOffsetTop = activeCanvasElement.offset().top;

      // popředí view
      frontCanvasElement = $("#front_canvas");
      frontCanvas = frontCanvasElement[0].getContext('2d');
      $('#frontCanvas').attr('width',iWidth);
      $('#frontCanvas').attr('height',iHeight);

      // signály - canvas má rozměr a umístění dle oblasti grafu
      signalCanvasElement = $("#signal_canvas");
      signalCanvas = signalCanvasElement[0].getContext('2d');
      $('#signalCanvas').attr('width',chartWidth);
      $('#signalCanvas').attr('height',chartHeight);      
      $('#signalCanvas').attr('top',posY+fieldTopAdjust);
      $('#signalCanvas').attr('left',posX+fieldLeftAdjust);  

      // pozadí grafu - mřížka
      backCanvasElement = $("#back_canvas");
      backCanvas = backCanvasElement[0].getContext('2d');
      $('#backCanvas').attr('width',chartWidth);
      $('#backCanvas').attr('height',chartHeight);
      $('#backCanvas').css('cursor','pointer');           

      var timeSec = getCurrentTime();
      var mod = timeSec%(stepGridTime[timeAxisIdx]*60);

      beginTime = timeSec - mod - (timeAxisLength[timeAxisIdx]*3600 - (stepGridTime[timeAxisIdx]*60)); // + stepGridTime[timeAxisIdx]*60;
            
      view = 0;            
      getConfig();      
    };
    
    function windowWidth() {
      return $(window).width();
    };

    function windowHeight() {
      //return window.innerHeight;
      return $(window).height()-50;
    };   

    // operace klávesnice a myši

    function getClickStep () {    
      switch (timeAxisIdx) { 
      case 0:
        return 5;
        break;
      case 1:
        return 5;
        break;
      case 2:
        return 10;
        break;
      case 3:
        return 20;
        break;
      case 4:
        return 30;
        break;
      case 5:
        return 60;
        break;
      case 6:
        return 120;
        break;
      case 7:
        return 240;
        break;
      case 8:
        return 600;
        break;
      case 9:
        return 1800;
        break;
      };
    };
    
    function keyDown(event) {
    var clickStep = 5;

      // posun vlevo
      if (event.keyCode == 37) {
        if ((markerTime - clickStep) >= beginTime) {
          clickStep = getClickStep();
          markerTime = markerTime - (markerTime%clickStep) - clickStep;
          drawCursor(markerTime);
        }
        else {
          backShift();
        };
      };
      
      // posun vpravo
      if (event.keyCode == 39) {
        if ((markerTime + clickStep) <= (beginTime + timeAxisLength[timeAxisIdx]*3600)) {
          clickStep = getClickStep();
          markerTime = markerTime - (markerTime%clickStep) + clickStep;
          drawCursor(markerTime);
        }
        else {
          fwdShift();
        };
      };
    };

    function mouseDown() {
      mouseIsDown = 1;      

      canX = (event.x-canvasOffsetLeft)+0.5;
      canY = (event.y-canvasOffsetTop)+0.5;

      // oblast grafu
      if ((canX > chartDef[0]["coords"][0]) && (canX < chartDef[0]["coords"][2]) && (canY > chartDef[0]["coords"][1]) && (canY < chartDef[0]["coords"][3])) {
        if (zoom == false) {
          drawCursor(canX);
        }
        else { // mód zoom
          zoomCanX = canX;          
        };
      };

      for (var m=1;m<=MAX_CLICKMAPS;m++) {

        if ((canX  > chartDef[m]["coords"][0]) && (canX < chartDef[m]["coords"][2]) && (canY > chartDef[m]["coords"][1]) && (canY < chartDef[m]["coords"][3])) {
          if (chartDef[m]["visibility"] == "true") {
            chartDef[m]["visibility"] = "false";
          }
          else {
            chartDef[m]["visibility"] = "true";
          };
            
          view = $('#group').val();
          lang = getLang();
          redrawChart(view,beginTime,markerTime,timeAxisIdx);
          redrawTimeAxis(view,lang);                              
          redrawValues(view,markerTime,lang);
          redrawLegend(view,lang);
        };
      };
    };

    function mouseUp() {
    var zoomLength = 0,
        zoomStartTime,
        zoomEndTime;

      canX = (event.x-canvasOffsetLeft)+0.5;
      canY = (event.y-canvasOffsetTop)+0.5;

      mouseIsDown = 0;

      // oblast grafu - souřadnice X
      if ((canX > chartDef[0]["coords"][0]) && (canX < chartDef[0]["coords"][2])) {

        // operace ZOOM
        if (zoom == true) {
          drawCursor(canX);

          // výběr v pixelech
          zoomLength = Math.abs(canX - zoomCanX);

          // jakému času odpovídá 1px
          koefFieldWidth = timeAxisLength[timeAxisIdx]*3600/(chartWidth);          
          
          zoomStartTime = beginTime + Math.round((zoomCanX-fieldLeftAdjust)*koefFieldWidth);          
          zoomEndTime = beginTime + Math.round((canX-fieldLeftAdjust)*koefFieldWidth);
          
          for (var i=0;i<=timeAxisLength.length;i++) {
          
            if ((zoomLength*koefFieldWidth/3600) < timeAxisLength[i]) {              
              timeAxisIdx = i;
              if (((zoomEndTime - zoomStartTime)/3600) > (timeAxisLength[i])) {
                timeAxisIdx = timeAxisIdx++;
              };
              
              // přepočet počátku grafu dle kroku nové mřížky           
              beginTime = zoomStartTime - (zoomStartTime%(stepGridTime[timeAxisIdx]*60));
              getData(view,beginTime,timeAxisLength[timeAxisIdx]*3600);
              // marker umístíme na konec zoom oblasti
              markerTime = beginTime + timeAxisLength[timeAxisIdx]*3600;

              view = $('#group').val();
              lang = getLang();
              redrawChart(view,beginTime,markerTime,timeAxisIdx);
              redrawTimeAxis(view,lang);
              redrawValues(view,markerTime,lang);
              redrawBeginTime(beginTime,lang);
              redrawMarkerTime(markerTime,lang);
              break;          
            };
          };
          zoom = false;
          zoomSignal("ZOOM");
        };
      };      
    };
    
    function mouseMove() {
      canX = (event.x-canvasOffsetLeft)+0.5;
      canY = (event.y-canvasOffsetTop)+0.5;
            
      // oblast grafu - souřadnice X
      if ((canX > chartDef[0]["coords"][0]) && (canX < chartDef[0]["coords"][2])) {
      
        if ((mouseIsDown == 1) && (zoom == false)) {
          koefFieldWidth = timeAxisLength[timeAxisIdx]*3600/(chartWidth);
          markerTime = beginTime + Math.round((canX-fieldLeftAdjust)*koefFieldWidth);
          drawCursor(canX);
          redrawMarkerTime(markerTime,lang);
          lastCanX = canX;
        };
        
        if ((mouseIsDown == 1) && (zoom == true)) {
          drawCursor(zoomCanX);
          drawCursor(canX);
        };        
      };
      
      for (var i=1;i<=MAX_CLICKMAPS;i++) {        
        if ((canX  > chartDef[i]["coords"][0]) && (canX < chartDef[i]["coords"][2]) && (canY > chartDef[i]["coords"][1]) && (canY < chartDef[i]["coords"][3])) {          
          document.getElementById("graph").style.cursor = "pointer";
        }
        else {
          document.getElementById("graph").style.cursor = "auto";
        };
      };  
    };

    // vykreslení kurzoru 
    function drawCursor(x) {    
    var markerX,
        timeX;
    
      view = $('#group').val();
      lang = getLang();

      //jakému času odpovídá 1px
      koefFieldWidth = timeAxisLength[timeAxisIdx]*3600/(chartWidth);
         
      // marker time
      if (x > 100000) {
        
        markerTime = x;
        timeX = (markerTime - beginTime);        
        if (timeX < (timeAxisLength[timeAxisIdx]*3600)) {

        eraseActiveCanvas(0);
        
        redrawMarkerTime(markerTime,lang);

        markerX = Math.round(timeX/koefFieldWidth);

        redrawValues(view,markerTime,lang);
            
        activeCanvas.strokeStyle = WHITE;

        activeCanvas.beginPath();
          activeCanvas.moveTo(posX+fieldLeftAdjust+markerX,posY+fieldTopAdjust);
          activeCanvas.lineTo(posX+fieldLeftAdjust+markerX,posY+fieldTopAdjust+chartHeight);
          activeCanvas.stroke();
        activeCanvas.closePath();        
        
        }; 
      }
      
      // pozice x
      else {
          if ((mouseIsDown == 1) && (zoom == true)) {                        
            eraseActiveCanvas(x);
          };

          if ((mouseIsDown == 1) && (zoom == false)) {
            eraseActiveCanvas(0);

            // jakému času odpovídá 1px            
            markerTime = beginTime + Math.round((canX-fieldLeftAdjust)*koefFieldWidth);            
            redrawMarkerTime(markerTime,lang);
            redrawValues(view,markerTime,lang);             
          };
          
          activeCanvas.strokeStyle = WHITE;
          
          activeCanvas.beginPath();
            activeCanvas.moveTo(x,posY+fieldTopAdjust);
            activeCanvas.lineTo(x,posY+fieldTopAdjust+chartHeight);
            activeCanvas.stroke();
          activeCanvas.closePath();
      };
    };
    
    function eraseActiveCanvas(s) {        
      x = (s == 0) ? posX : s;
      width = (s == 0) ? iWidth : canX-zoomCanX;
      activeCanvas.clearRect(x,posY,width,iHeight);
    };

    // globalni funkce
    function addZero(i) {    
      i = i < 10 ? "0" + String(i) : i;      
      return i;
    };
       
    function getFormatedDateTime(inPktime,lang) {
    var pktime = new Pktime();
    var dateTimeString = "";

      switch (lang) {
      case "EN":
        weekday = daysEN0;
        break;
      case "CZ":
        weekday = daysCZ0;
        break;
      case "DE":
        weekday = daysDE0;
        break;
      case "PL":
        weekday = daysPL0;
        break;
      case "RU":
        weekday = daysRU0;
        break;    
      };
    
      pktime.setValue(inPktime);      
      var dateTime = pktime.getDate();
      
      var day = weekday[dateTime.getDay()];
      var dd = addZero(dateTime.getDate());
      var MM = addZero(1 + dateTime.getMonth());
      var yyyy = dateTime.getFullYear();
      var hh = addZero(dateTime.getHours());      
      var mm = addZero(dateTime.getMinutes());
      var ss = addZero(dateTime.getSeconds());      
    
      dateTimeString = weekday[dateTime.getDay()].slice(0,3) + "," + dd + "." + MM + "." + yyyy + "  " + hh + ":" + mm + ":" + ss;
      return dateTimeString;
    };
    
    function getFormatedTime(inPktime) {
    var pktime = new Pktime(),                       
        timeString = "";        
    
      pktime.setValue(inPktime);      
      var dateTime = pktime.getDate();
            
      var hh = addZero(dateTime.getHours()-1);      
      var mm = addZero(dateTime.getMinutes());
      var ss = addZero(dateTime.getSeconds());      

      timeString = hh + ":" + mm + ":" + ss;
      return timeString;
    };
    
    function getCurrentTime() {    
    var pktime = new Pktime(),
        d = new Date();      
      pktime.setValue(d);      
      return pktime.getPktime();
    };
    
    // server - klient funkce
    
    // načtení konfigurace ze serveru             
    function getConfig() {
    var url_1 = 'graph/getConfig';    
      $.ajax({ url: url_1,
    		async: true,        
    		dataType: "json",
        type: "post",
    		success: function(data,textStatus) {
           config = data;           
           setupLayout();
        }
      });
    };
    
    // získání dat ze serveru ( při reloadu,změně view)
    function getData(view,beginTime,timeAxisLength) {
    var dataRequest,
        tmpVal;

    dataRequest = "";
    
    dataRequest += '{"beginTime": ' + beginTime + ', "timeAxisLength": ' + timeAxisLength + ', "tags":[';
      for (var f=0;f<config.View[view].field.length;f++) {
        for (var s=0;s<config.View[view].field[f].signal.length;s++) {
          signal = config.View[view].field[f].signal[s];          
          
          dataRequest += '{"table": "' + signal.table + '", "column": "' + signal.column + '"';

          tmpVal = timeAxisLength/(2*chartWidth)/10;
          optTimeStep = Math.floor(tmpVal)*10;
          
          switch (signal.table) {
            case "sec5" :
              if (optTimeStep < 5) {
                dataRequest += ', "period": 5}, '; 
              }
              else {
                dataRequest += ', "period": ' + optTimeStep + '}, ';
              };
            break;
            case "sec10" :
              if (optTimeStep < 10) {
                dataRequest += ', "period": 10}, '; 
              }
              else {
                dataRequest += ', "period": ' + optTimeStep + '}, ';
              };
            break;
            case "norm" :
              if (optTimeStep < 20) {
                dataRequest += ', "period": 20}, '; 
              }
              else {
                dataRequest += ', "period": ' + optTimeStep + '}, ';
              };
            break;
            case "slow" :
              if (optTimeStep < 60) {
                dataRequest += ', "period": 60}, '; 
              }
              else {
                dataRequest += ', "period": ' + optTimeStep + '}, ';
              };
            break;
            case "xslow" :
              if (optTimeStep < 300) {
                dataRequest += ', "period": 300}, '; 
              }
              else {
                dataRequest += ', "period": ' + optTimeStep + '}, ';
              };
            break;            
            default :
              dataRequest += ', "period": ' + optTimeStep + '}, ';            
            break;
          };                    
        };        
      };
    dataRequest = dataRequest.substr(0,dataRequest.length-2);
    dataRequest += ']';
    dataRequest += '}';
    var xmlHttp = new XMLHttpRequest();
    
    var url = '/graph/getData';
      dataType: "json";
      xmlHttp.open("POST", url, true);        
      xmlHttp.send(dataRequest);

      xmlHttp.onreadystatechange = function() {
        if (xmlHttp.readyState == xmlHttp.DONE) {
          data = JSON.parse(xmlHttp.responseText.replace(/NaN/g,'null'));
          setData();
          redrawChart(view,beginTime,markerTime,timeAxisIdx);
          redrawValues(view,markerTime,lang);          
        };
      };
    };

    // základní view
    function setupLayout() {
    var lang;
      lang = config.LangEnb[0];      
      //nastavení tlačítka lang na defaultní jazyk      
      setLang(lang);                                
      
      //nastavení roletky a aktuálního titulku
      setTitles(0,lang);

      setMultitexts(lang);
      
      getData(0,beginTime,timeAxisLength[timeAxisIdx]*3600);
      
      redrawScreen();      
      redrawTitle(view,lang);      
      redrawChart(view,beginTime,markerTime,timeAxisIdx);      
      redrawTimeAxis(view,lang);
      redrawBeginTime(beginTime,lang);
      redrawLegend(view,lang);
      drawCursor(beginTime);
    };

    function setData() {
    console.log('setData');
      //přes všechny tagy
      for (var m=0;m<data.tags.length;m++) {
        chartDef[m+1]["id"] = (data.tags[m].table + '.' + data.tags[m].column);                        
        //if (data.tags[m]["vals"] =! "null") {
          for (var v=0;v<data.tags[m]["vals"].length;v++) {
            chartDef[m+1]["times"][v] = beginTime + Math.round(v*data.tags[m].period);            
          };
          chartDef[m+1]["values"] = data.tags[m]["vals"];
        //}
        //else {
        //  console.log('missing data');
        //};
      };
    }; 
    
    // nastavení menu dle konfigurace   

    // nastavení multitextů s aktuálním jazykem   
    function setMultitexts(lang) {
      //vyprázdnění předchozích multitextů
      for (var m=0;m<30;m++) {
        multitexts[m]["name"] = "";
        for (var v=0;v<60;v++) {
          multitexts[m]["values"][v].idx = -99;                       
          multitexts[m]["values"][v].text = "";            
        };    
      };
      
      // nastavení multitextů pro příslušný jazyk
      for (var m=0;m<config.TextlistDef.length;m++) {
        if (m < 30) {
          multitexts[m]["name"] = config.TextlistDef[m].textlist;
          for (var v=0;v<config.TextlistDef[m].values.length;v++) {
            multitexts[m]["values"][v].idx = config.TextlistDef[m]["values"][v].idx;
            multitexts[m]["values"][v].text = config.TextlistDef[m]["values"][v]["text_" + lang];
          };
        };
      };
    };

    // nastavení titulků
    function setTitles(view, lang) {
      if (config.LangEnb.indexOf(lang) != -1) {
        $('#group option').remove();
        for (v=0;v<config.View.length;v++) {
          titles[v] = '';
          titles[v] = config.View[v]["name_" + lang];
          
          //nastavení roletky group
          if (v == view) {
            $('#group').append('<option value="' + v + '" selected="selected">' + titles[v] + '</option>');
          }
          else {
            $('#group').append('<option value="' + v + '" >' + titles[v] + '</option>');
          };          
        };
      }      
    };

    // funkce pro kreslení na canvas
    function redrawScreen() {
      // vyčištění a překreslení plochy mimo backCanvas      
      
      // levá část
      frontCanvas.clearRect(posX,posY,fieldLeftAdjust,iHeight);
      frontCanvas.fillStyle = colorView;
      frontCanvas.fillRect(posX,posY,fieldLeftAdjust,iHeight);
      
      // horní část
      frontCanvas.clearRect(posX,posY,iWidth,fieldTopAdjust-0.5);
      frontCanvas.fillStyle = colorView;
      frontCanvas.fillRect(posX,posY,iWidth,fieldTopAdjust+0.5);

      // pravá část
      frontCanvas.clearRect(posX+iWidth-fieldRightAdjust,posY,fieldRightAdjust-0.5,iHeight);
      frontCanvas.fillStyle = colorView;
      frontCanvas.fillRect(posX+iWidth-fieldRightAdjust,posY,fieldRightAdjust+0.5,iHeight);
      
      // spodní část
      frontCanvas.clearRect(posX+1,posY+fieldTopAdjust+chartHeight,iWidth-2,fieldBottomAdjust-0.5);
      frontCanvas.fillStyle = colorView;
      frontCanvas.fillRect(posX+1,posY+fieldTopAdjust+chartHeight,iWidth-2,fieldBottomAdjust+0.5);

      // vnější orámování
      frontCanvas.strokeStyle = colorFrames;
      frontCanvas.strokeRect(posX,posY,iWidth,iHeight);
      
      // linka pod titulkem
      frontCanvas.strokeStyle = colorFrames;
      frontCanvas.lineWidth = 1;
      
      frontCanvas.beginPath();
        frontCanvas.moveTo(posX,posY+titleBarHeight);
        frontCanvas.lineTo(posX+iWidth,posY+titleBarHeight);
        frontCanvas.stroke();
      frontCanvas.closePath();
            
    };    
                           
    function getViewInfo(view) {
    var viewInfo = [0,0];
    
      // fields
      for (var f=1;f<=config.View[view].field.length;f++) {

        if (config.View[view]["field"][f-1].relSize > 0) {
          viewInfo[0] += config.View[view]["field"][f-1].relSize;
          viewInfo[1]++;
        };
      };
      return viewInfo;
    };
        
    function redrawValues(view,markerTime,lang) {
    var viewInfo = new Array(),
        fieldSumRatio = 0,
        fieldsCnt = 0,
        koefFieldHeight = 0,
        FieldPosY = 0,
        fieldSigHeight = 0,     
        value,
        time,
        chartHeightLoc;
        
      // fields
      viewInfo = getViewInfo(view);      
      fieldSumRatio = viewInfo[0]; 
      fieldsCnt = viewInfo[1];
      chartHeightLoc = chartHeight - chartHeight%fieldSumRatio;
      
      if (fieldSumRatio != 0) {   //ochrana proti děleni nulou
        koefFieldHeight = (chartHeightLoc-fieldBrake*(fieldsCnt-1))/fieldSumRatio;                
      };      
            
      // field
      for (var field=1;field<=config.View[view]["field"].length;field++) {        
        FieldPosY = 0;
        
        for (var j=1;j<field;j++) {
          FieldPosY += (config.View[view]["field"][j-1].relSize*koefFieldHeight)+fieldBrake;
        };
        
        fieldSigHeight = config.View[view]["field"][field-1].relSize*koefFieldHeight;

        var signals = new Array(),            
            mask = "",
            signalStrVal = "",
            signalIdCfg = "",
            signalIdData = "",
            dataLastX = 0,
            dataLastY = 0,
            xPos,
            a,
            regresInterval = 30;
                            
        signals = config.View[view]["field"][field-1]["signal"];

        // signals
        for (var x=0;x<signals.length;x++) {

          var idx = signals.length-x;

          signalIdCfg = signals[x].table + "." + signals[x].column;          
          
          // data
          for (var d=1;d<30;d++) {

            if ((signalIdCfg) == (chartDef[d].id)) {
              
              // times
              for (var t=0;t<chartDef[d]["times"].length;t++) {

                if ((markerTime >= chartDef[d]["times"][t]) && (markerTime < chartDef[d]["times"][t+1])) {
                 
                  // najeli jsme kurzorem na reálná data
                  if (markerTime == chartDef[d]["times"][t]) {
                    value = chartDef[d]["values"][t];

                  }
                  // najeli jsme kursorem mezi reálná data - proložíme regresí pro typ number, pro multitext zobrazíme hodnotu předešlých dat
                  else {
                    if (((markerTime - chartDef[d]["times"][t]) <= regresInterval) || ((chartDef[d]["times"][t+1] - markerTime) <= regresInterval)) {

                      switch (signals[x].type) {
                      case "analog" :
                        a = (chartDef[d]["values"][t+1]-chartDef[d]["values"][t])/(chartDef[d]["times"][t+1]-chartDef[d]["times"][t]);
                      break;
                      case "multitext" :
                        a = 0;
                      break;
                      case "binary" :
                        //
                      break;
                      };                      
                      
                      value = a*(markerTime-chartDef[d]["times"][t]) + chartDef[d]["values"][t];
                    }
                    else {
                      //frontCanvas.clearRect(posX+iWidth-fieldRightAdjust-fieldValWidth+180,posY+fieldTopAdjust+FieldPosY+fieldSigHeight-(idx)*14,95,14);
                      frontCanvas.fillStyle = colorBackFields;
                      frontCanvas.fillRect(posX+iWidth-fieldRightAdjust-fieldValWidth+180-0.5,posY+fieldTopAdjust+FieldPosY+fieldSigHeight-(idx)*14,95.5,14);
                      
                      frontCanvas.fillStyle = signals[x].color;
                      frontCanvas.font = fontLegend;
                      frontCanvas.fillText("???",posX+iWidth-fieldRightAdjust-fieldValWidth+230,posY+fieldTopAdjust+FieldPosY+fieldSigHeight-(idx-1)*14-4);
                      break;
                    };
                  };

                  switch (signals[x].type) {
                    // typ číslo
                    case "analog" :
                      for (var m=1;m<=MAX_CLICKMAPS;m++) {                        
                        if (chartDef[m]["id"] == signalIdCfg) {
                          if (chartDef[m]["visibility"] == "true") {
                            value = value - chartDef[m].offset;                            
                          }
                          else {
                            value = undefined;
                          };
                        };
                      };
  
                      if (Math.abs(chartDef[0].offset - markerTime) <= 5) {
                        value = 0;
                      }; 
                      
                      // získání numerické hodnoty (typ string) s maskou
                      if (value != undefined) {
                        signalStrVal = value.toFixed(signals[x].decimal);
                      }
                      else {
                        signalStrVal = "- - -";
                      };                     
  
                      frontCanvas.font = fontVals;
                      xPos = posX+iWidth-fieldRightAdjust-fieldValWidth+230;
                    break;
                                        
                    // typ multitext
                    case "multitext" :                      
                      signalTextList = signals[x].textlist;
                                            
                      for (var s=1;s<=MAX_CLICKMAPS;s++) {
                        if (chartDef[s]["id"] == signalIdCfg) {                                                    
                          if (chartDef[s]["visibility"] == "true") {
                            //vyhledání multitextu                                                    
                            for (var m=0;m<30;m++) {
                              if ((multitexts[m].name == signals[x].textlist) && (multitexts[m].name != "")) {
                                //vyhledání textu dle hodnoty
                                for (var v=0;v<multitexts[m].values.length;v++) {                                                                  
                                  if (multitexts[m]["values"][v].idx == value) {                                    
                                    signalStrVal = multitexts[m]["values"][v].text;
                                    break;                                    
                                  }
                                  else {                                    
                                    signalStrVal = "undefined";
                                  };
                                };
                                break;
                              };
                            };
                            break;
                          }
                          else {signalStrVal = "- - -";
                          };
                        };
                      };
                      
                      frontCanvas.font = fontLegend;
                      xPos = posX+iWidth-fieldRightAdjust-fieldValWidth+180;  
                    break;  
                    };
                    
                  //frontCanvas.clearRect(posX+iWidth-fieldRightAdjust-fieldValWidth+180,posY+fieldTopAdjust+FieldPosY+fieldSigHeight-(idx)*14,95,14);
                  frontCanvas.fillStyle = colorBackFields;
                  frontCanvas.fillRect(posX+iWidth-fieldRightAdjust-fieldValWidth+180-0.5,posY+fieldTopAdjust+FieldPosY+fieldSigHeight-(idx)*14.3,95.5,14);
                  
                  frontCanvas.fillStyle = signals[x].color;
                  frontCanvas.fillText(signalStrVal,xPos,posY+fieldTopAdjust+FieldPosY+fieldSigHeight-(idx-1)*14-4);
                  break;
                };
              };
            };
          };
        };
      }; 
    };
    
    function redrawChart(view, beginTime,markerTime,timeAxisIdx) {
    var viewInfo = new Array(),
        fieldsCnt = 0,
        fieldSumRatio = 0,
        koefFieldHeight = 0,
        FieldPosY = 0,
        stepHorzGrid = 0,
        stepVertGrid = 0,
        realStep = 0,
        fieldSigHeight = 0,
        chartHeightLoc = 0;
      
      // fields
      viewInfo = getViewInfo(view);      
      fieldSumRatio = viewInfo[0]; 
      fieldsCnt = viewInfo[1];
      
      chartHeightLoc = chartHeight - chartHeight%fieldSumRatio;
      
      // vyčištění y-osy grafu
      frontCanvas.fillStyle = colorView;
      frontCanvas.fillRect(posX+1,posY+fieldBottomAdjust-10,39,chartHeight);
            
      // vyčištění plochy grafu      
      signalCanvas.clearRect(posX+1,posY+fieldBottomAdjust,chartWidth+fieldLeftAdjust,chartHeightLoc);
      backCanvas.clearRect(posX+1,posY+fieldBottomAdjust,chartWidth+fieldLeftAdjust,chartHeightLoc);

      if (fieldSumRatio != 0) { //ochrana proti děleni nulou
        koefFieldHeight = (chartHeightLoc-fieldBrake*(fieldsCnt-1))/fieldSumRatio;                        
      };

      for (var field=1;field<=config.View[view]["field"].length;field++) {        
        FieldPosY = 0;
        for (var j=1;j<field;j++) {
         FieldPosY += (config.View[view]["field"][j-1].relSize*koefFieldHeight)+fieldBrake;
        };
        
        fieldSigHeight = config.View[view]["field"][field-1].relSize*koefFieldHeight;                
        
        // fields
        backCanvas.fillStyle = colorBackFields;
        backCanvas.fillRect(posX+fieldLeftAdjust,posY+fieldTopAdjust+FieldPosY,chartWidth,fieldSigHeight);

        // y-osa
        fieldRange = config.View[view]["field"][field-1].maxY;                
        realStep = (fieldRange/7);

        for (var r=12;r>=0;r--) {
          if (stepGridY[r] > realStep) {
            stepVertGrid = stepGridY[r];
          };
        };

        if (stepVertGrid != 0) {
        lineCnt = Math.floor(fieldRange/stepVertGrid);
        };

        for (var k=0;k<=lineCnt;k++) {
          numLength = ((k*stepVertGrid).toString()).length*6+5;

          if (fieldRange != 0) { //ochrana před dělením nulou            

            if ((fieldRange != k*stepVertGrid)) { // nevykreslovat horní limit - mřížku,bod ani číslo

              // značky - body
              frontCanvas.strokeStyle = BLACK;
              //frontCanvas.setLineDash([0,0]);
              //frontCanvas.setLineDash([1]);
              frontCanvas.lineWidth = 1;

              frontCanvas.beginPath();
                frontCanvas.moveTo(posX+fieldLeftAdjust-3,posY+fieldTopAdjust+FieldPosY+fieldSigHeight-k*stepVertGrid*(fieldSigHeight/fieldRange));
                frontCanvas.lineTo(posX+fieldLeftAdjust,posY+fieldTopAdjust+FieldPosY+fieldSigHeight-k*stepVertGrid*(fieldSigHeight/fieldRange));
                frontCanvas.stroke();
              frontCanvas.closePath();

              if (k != 0) {
                // mřížka
                backCanvas.strokeStyle = colorGrid;                               
                //backCanvas.setLineDash ([15,5]);                
                backCanvas.lineWidth = 1;

                backCanvas.beginPath();
                  backCanvas.moveTo(posX+fieldLeftAdjust+0.5,posY+fieldTopAdjust+FieldPosY+fieldSigHeight-k*stepVertGrid*(fieldSigHeight/fieldRange));
                  backCanvas.lineTo(posX+fieldLeftAdjust+chartWidth+0.5,posY+fieldTopAdjust+FieldPosY+fieldSigHeight-k*stepVertGrid*(fieldSigHeight/fieldRange));
                  backCanvas.stroke();
                backCanvas.closePath();
              };

              // značky - čísla
              frontCanvas.fillStyle = BLACK;
              frontCanvas.font = fontAxis;
              frontCanvas.fillText(k*stepVertGrid,posX+fieldLeftAdjust-numLength,posY+fieldTopAdjust+FieldPosY+fieldSigHeight-k*stepVertGrid*(fieldSigHeight/fieldRange)-2);              
            };
          };
        };        

        var img = document.getElementById("signal_visible"),       
            signals = new Array(),
            signal = new Array(),
            mask = "",       
            signalStrVal = "",
            signalIdCfg = "",  
            signalIdData = "",
            dataLastX = 0,
            dataLastY = 0;        

        signals = config.View[view]["field"][field-1]["signal"];

        // rozbor parametrů jednotlivých signálů
        for (var x=0;x<signals.length;x++) {

          var idx = signals.length-x;                          

          signalIdCfg = signals[x].table + "." + signals[x].column;                    

          signalCanvas.fillStyle = signals[x].color;          

          for (var i=1;i<=MAX_CLICKMAPS;i++) {
            if (chartDef[i]["id"] == signalIdCfg) {
              if  (chartDef[i]["visibility"] == "true") {

                // vykreslení průběhů
                signalCanvas.strokeStyle = signals[x].color;
                //signalCanvas.setLineDash ([0,0]);
                //signalCanvas.setLineDash ([1]);
                signalCanvas.lineWidth = 1;

                for (var v=0;v<chartDef[i]["values"].length;v++) {
                  if (timeAxisLength[timeAxisIdx] != 0) {    //ochrana před dělením nulou
                    dataX = fieldLeftAdjust + ((chartDef[i]["times"][v] - beginTime)/3600)*(chartWidth/timeAxisLength[timeAxisIdx]);
                  };

                  dataY = posY+fieldTopAdjust+FieldPosY+fieldSigHeight-chartDef[i]["values"][v]*(fieldSigHeight/fieldRange);
  
                  if (v == 0) {
                    dataLastX = dataX;
                    dataLastY = dataY;
                  };
  
                  signalCanvas.beginPath();
                  
                    if ((dataX > chartDef[0]["coords"][0]) && (dataX < chartDef[0]["coords"][2])) {
                      if (chartDef[i]["values"][v]*(fieldSigHeight/fieldRange) > fieldSigHeight) {
                        signalCanvas.fillStyle = signals[x].color;
                        signalCanvas.fill();
                      };
  
                      switch (signals[x].type) {
                        case "analog" :
                          signalCanvas.moveTo(dataLastX,dataLastY);
                          signalCanvas.lineTo(dataX,dataY);
                          signalCanvas.stroke();
                        break;
                        case "multitext" :
                          signalCanvas.moveTo(dataLastX,dataLastY);
                          signalCanvas.lineTo(dataX,dataLastY);
                          signalCanvas.moveTo(dataX,dataLastY);
                          signalCanvas.lineTo(dataX,dataY);
                          signalCanvas.moveTo(dataX,dataY);                        
                          signalCanvas.stroke();
                      };
                    };
  
                  signalCanvas.closePath();
  
                  dataLastX = dataX;
                  dataLastY = dataY;
                };
              };
            }; 
          };
        };
      };
          
      // horizontální mřížka
      stepHorzGrid = stepGridTime[timeAxisIdx]/60;
      lineCnt = Math.floor(timeAxisLength[timeAxisIdx]/stepHorzGrid);
      
      for (var k=0;k<=lineCnt;k++) {
        if (timeAxisLength[timeAxisIdx] != 0) { //ochrana před dělením nulou

          // mřížka
          if ((k != 0) && (timeAxisLength[timeAxisIdx] != k*stepHorzGrid)) {  // nevykreslovat mřížku pro dolní a horní limit 
            backCanvas.strokeStyle = colorGrid;
            //backCanvas.setLineDash ([15,5]);
            backCanvas.lineWidth = 1;
            
            backCanvas.beginPath();
              backCanvas.moveTo(posX+fieldLeftAdjust+k*stepHorzGrid*(chartWidth/timeAxisLength[timeAxisIdx]),posY+fieldTopAdjust);
              backCanvas.lineTo(posX+fieldLeftAdjust+k*stepHorzGrid*(chartWidth/timeAxisLength[timeAxisIdx]),posY+iHeight-fieldBottomAdjust);
              backCanvas.stroke();
            backCanvas.closePath();
          };
        };
      };
    };
    
    function redrawTimeAxis(view,lang) {
    var date,
        time,
        viewInfo = new Array(),
        fieldsCnt = 0,
        fieldSumRatio = 0,
        chartHeightLoc;        
        
      //fields
      viewInfo = getViewInfo(view);
      fieldSumRatio = viewInfo[0];
      fieldsCnt = viewInfo[1];
        
      chartHeightLoc = chartHeight - chartHeight%fieldSumRatio;  
      
      // vyčištění plochy časové osy
      frontCanvas.clearRect(posX+1,posY+fieldTopAdjust+chartHeightLoc,fieldLeftAdjust+chartWidth+fieldBreakVals,38.5);
      frontCanvas.fillStyle = colorView;
      frontCanvas.fillRect(posX+1,posY+fieldTopAdjust+chartHeightLoc,fieldLeftAdjust+chartWidth+fieldBreakVals,38.5);
  
      // časová osa
      stepHorzGrid = stepGridTime[timeAxisIdx]/60;
      lineCnt = Math.floor(timeAxisLength[timeAxisIdx]/stepHorzGrid);
      
      for (var k=0;k<=lineCnt;k++) {
        if (timeAxisLength[timeAxisIdx] != 0) { //ochrana před dělením nulou
                    
          // značky - body
          frontCanvas.strokeStyle = BLACK;
          //frontCanvas.setLineDash([0,0]);          
          //frontCanvas.setLineDash([1]);
          frontCanvas.lineWidth = 1;
      
          frontCanvas.beginPath();
            frontCanvas.moveTo(posX+fieldLeftAdjust+k*stepHorzGrid*(chartWidth/timeAxisLength[timeAxisIdx]),posY+iHeight-fieldBottomAdjust);
            frontCanvas.lineTo(posX+fieldLeftAdjust+k*stepHorzGrid*(chartWidth/timeAxisLength[timeAxisIdx]),posY+iHeight-fieldBottomAdjust+3);
            frontCanvas.stroke();
          frontCanvas.closePath();
  
          // značky - čísla
          frontCanvas.fillStyle = BLACK;
          frontCanvas.font = fontAxis;
                                                                 
          datetime = (getFormatedDateTime(beginTime+k*stepHorzGrid*3600+utcBias,lang));

          date = datetime.slice(datetime.length-20,datetime.length-14);
          time = datetime.slice(datetime.length-8,datetime.length-3);              
          
          if ((stepGridTime[timeAxisIdx]) >= 1440) {
            frontCanvas.fillText(time,posX+fieldLeftAdjust+k*stepHorzGrid*(chartWidth/timeAxisLength[timeAxisIdx])-time.length*5/2,posY+iHeight-fieldBottomAdjust+13);
            frontCanvas.fillText(date,posX+fieldLeftAdjust+k*stepHorzGrid*(chartWidth/timeAxisLength[timeAxisIdx])-date.length*5/2,posY+iHeight-fieldBottomAdjust+23);
          }
          else {
            frontCanvas.fillText(time,posX+fieldLeftAdjust+k*stepHorzGrid*(chartWidth/timeAxisLength[timeAxisIdx])-time.length*5/2,posY+iHeight-fieldBottomAdjust+13);
          };
        };
      };
    };
    
    function redrawBeginTime(beginTime,lang) {
      frontCanvas.clearRect(posX+fieldLeftAdjust,posY+iHeight-fieldBottomAdjust+poleBegtimeOdsazY,fieldValWidth,16);

      frontCanvas.strokeStyle = colorFrames;      
      frontCanvas.strokeRect(posX+fieldLeftAdjust,posY+iHeight-fieldBottomAdjust+poleBegtimeOdsazY,fieldValWidth,16);

      frontCanvas.fillStyle = BLACK;               
      frontCanvas.font = fontTitle;
      frontCanvas.fillText(getFormatedDateTime(beginTime+utcBias,lang),posX+fieldLeftAdjust+5,posY+iHeight-fieldBottomAdjust+poleBegtimeOdsazY+12);
    };

    function redrawTitle(view) {
      frontCanvas.clearRect(posX+fieldLeftAdjust,posY+fieldTopAdjust-24,350.5,23.5);

      frontCanvas.fillStyle = colorView;
      frontCanvas.fillRect(posX+fieldLeftAdjust,posY+fieldTopAdjust-24,350.5,23.5);

      frontCanvas.fillStyle = colorTitle;
      frontCanvas.font = fontTitle;      
      frontCanvas.fillText(titles[view],posX+fieldLeftAdjust,posY+fieldTopAdjust-10);
    };

    function redrawLegend(view,lang) {
    var viewInfo = new Array(),
        fieldsCnt = 0,
        fieldSumRatio = 0,
        koefFieldHeight = 0,
        FieldPosY = 0,
        fieldSigHeight = 0,
        sigGlobalIdx = 0,
        chartHeightLoc,        

      //fields      
      viewInfo = getViewInfo(view);
      fieldSumRatio = viewInfo[0];
      fieldsCnt = viewInfo[1];
        
      chartHeightLoc = chartHeight - chartHeight%fieldSumRatio;
      
      // vyčištění plochy legendy
      frontCanvas.clearRect(posX+fieldLeftAdjust+chartWidth,posY+fieldTopAdjust,fieldValWidth+fieldBreakVals-1,chartHeight);
      
      // legenda
      frontCanvas.fillStyle = colorView;
      frontCanvas.fillRect(posX+fieldLeftAdjust+chartWidth,posY+fieldTopAdjust,fieldValWidth+fieldBreakVals-1,chartHeightLoc);
      
      if (fieldSumRatio != 0) { //ochrana proti děleni nulou
        koefFieldHeight = (chartHeightLoc-fieldBrake*(fieldsCnt-1))/fieldSumRatio;
      };
      
      sigGlobalIdx = 0;

      for (var field=1;field<=config.View[view]["field"].length;field++) {        
        FieldPosY = 0;

        for (var j=1;j<field;j++) {
         FieldPosY += (config.View[view]["field"][j-1].relSize*koefFieldHeight)+fieldBrake;         
        };

        fieldSigHeight = config.View[view]["field"][field-1].relSize*koefFieldHeight;        
        
        // legenda
        frontCanvas.fillStyle = colorBackFields;
        frontCanvas.fillRect(posX+iWidth-fieldRightAdjust-fieldValWidth,posY+fieldTopAdjust+FieldPosY,fieldValWidth,fieldSigHeight);                  
        
        var img = document.getElementById("signal_visible"),
            signals = new Array(),
            signal = new Array(),
            mask = "",       
            signalStrVal = "",
            signalIdCfg = "";    
            signalIdData = "";
            
        signals = config.View[view]["field"][field-1]["signal"];        
        
        // rozbor parametrů jednotlivých signálů
        for (var x=0;x<signals.length;x++) {

          var idx = signals.length-x;
                    
          sigGlobalIdx += 1;
          
          signalIdCfg = signals[x].table + "." + signals[x].column;                    
          
          frontCanvas.fillStyle = signals[x].color;          
          frontCanvas.font = fontLegend;
          
          for (n=0;n<config.NameDef.length;n++){
            if (/*(config.NameDef[n].table == signals[x].table) &&*/ (config.NameDef[n].column == signals[x].column)) {                            
              signalLegend = config.NameDef[n]["fullName_" + lang];
              signalUnits = config.NameDef[n]["unit_" + lang]; 
            };
          };

          sX = posX+iWidth-fieldRightAdjust-fieldValWidth-13; 
          sY = posY+fieldTopAdjust+FieldPosY+fieldSigHeight-(idx)*14;
          frontCanvas.fillText(signalLegend,posX+iWidth-fieldRightAdjust-fieldValWidth+5,posY+fieldTopAdjust+FieldPosY+fieldSigHeight-(idx-1)*14-4.5);
          
          // omezeni indexu
          if (sigGlobalIdx <= MAX_CLICKMAPS) {                    
            chartDef[sigGlobalIdx]["id"] = signalIdCfg;
            chartDef[sigGlobalIdx]["coords"][0] = sX;
            chartDef[sigGlobalIdx]["coords"][1] = sY;
            chartDef[sigGlobalIdx]["coords"][2] = sX+11;
            chartDef[sigGlobalIdx]["coords"][3] = sY+11;                      
            if (chartDef[sigGlobalIdx]["visibility"] == "true") {
              frontCanvas.drawImage(img,0,0,11,11,sX,sY,11,11);
            }
            else {
              frontCanvas.drawImage(img,11,0,11,11,sX,sY,11,11);
            };
          };

          //jednotky
          if (signals[x].type == "analog") {  // typ číslo
            
            frontCanvas.fillStyle = signals[x].color;  
            frontCanvas.font = fontLegend;            
            frontCanvas.fillText(signalUnits,posX+iWidth-fieldRightAdjust-fieldValWidth+275,posY+fieldTopAdjust+FieldPosY+fieldSigHeight-(idx-1)*14-4.5);
          };
        };
        
      };
    };
    
    function redrawMarkerTime(markerTime,lang) {
    var sign;
    
      // vyčištění plochy marker time      
      frontCanvas.clearRect(posX+iWidth-fieldRightAdjust-fieldValWidth,posY+iHeight-fieldBottomAdjust+poleBegtimeOdsazY,fieldValWidth,16);
          
      frontCanvas.fillStyle = colorBackTime;
      frontCanvas.fillRect(posX+iWidth-fieldRightAdjust-fieldValWidth,posY+iHeight-fieldBottomAdjust+poleBegtimeOdsazY,fieldValWidth,16);
      
      frontCanvas.fillStyle = colorTime;               
      frontCanvas.font = fontTitle;
      
      if (reset == true) {        
        sign = (markerTime >= chartDef[0].offset) ? " + " : " - ";
        
        frontCanvas.fillText(sign + getFormatedTime(Math.abs(markerTime-chartDef[0].offset)),posX+iWidth-fieldRightAdjust-fieldValWidth+5,posY+iHeight-fieldBottomAdjust+poleBegtimeOdsazY+12);
      }
      else {
        frontCanvas.fillText(getFormatedDateTime(markerTime+utcBias,lang),posX+iWidth-fieldRightAdjust-fieldValWidth+5,posY+iHeight-fieldBottomAdjust+poleBegtimeOdsazY+12);
      };
            
    };
       
    function separate(string,separator,idx) {        
    var items = new Array();
        item = "";
        
      if (idx == -1) {   
        items = string.split(separator);              
        return items;
      }
      else {
      
        items = string.split(separator);
        item = items[idx];                   
        return item;
      };              
    };         
    
   // tlačítka menu 
  
   function backShift() {     
     view = $('#group').val();
     lang = getLang();          
     beginTime -= (timeAxisLength[timeAxisIdx]*3600)/2;
     getData(view,beginTime,timeAxisLength[timeAxisIdx]*3600);
     redrawChart(view,beginTime,markerTime,timeAxisIdx);
     redrawBeginTime(beginTime,lang);
     redrawTimeAxis(view,lang);
     drawCursor(markerTime);
   }; 
   
   function fwdShift() {
     view = $('#group').val();
     lang = getLang();     
     beginTime += (timeAxisLength[timeAxisIdx]*3600)/2;
     getData(view,beginTime,timeAxisLength[timeAxisIdx]*3600);
     redrawChart(view,beginTime,markerTime,timeAxisIdx);
     redrawBeginTime(beginTime,lang);
     redrawTimeAxis(view,lang);
     drawCursor(markerTime);
   };
   
   function narrow() {
     view = $('#group').val();
     lang = getLang();    
     timeAxisIdx = (timeAxisIdx >= 9) ? 9 : timeAxisIdx+1;
     getData(view,beginTime,timeAxisLength[timeAxisIdx]*3600);
     redrawChart(view,beginTime,markerTime,timeAxisIdx);
     redrawBeginTime(beginTime,lang);
     redrawTimeAxis(view,lang);
     drawCursor(markerTime);
   };
   
   function extend() {
     view = $('#group').val();
     lang = getLang();
     timeAxisIdx = (timeAxisIdx <= 0) ? 0 : timeAxisIdx-1;
     getData(view,beginTime,timeAxisLength[timeAxisIdx]*3600);
     redrawChart(view,beginTime,markerTime,timeAxisIdx);
     redrawBeginTime(beginTime,lang);
     redrawTimeAxis(view,lang);
     drawCursor(markerTime);
   };
   
   function backDay() {
     view = $('#group').val();
     lang = getLang();
     beginTime -= 86400;
     getData(view,beginTime,timeAxisLength[timeAxisIdx]*3600);
     redrawChart(view,beginTime,markerTime,timeAxisIdx);
     redrawBeginTime(beginTime,lang);
     redrawTimeAxis(view,lang);
     drawCursor(markerTime);
   };
   
   function fwdDay() {
     view = $('#group').val();
     lang = getLang();
     beginTime += 86400;
     getData(view,beginTime,timeAxisLength[timeAxisIdx]*3600);
     redrawChart(view,beginTime,markerTime,timeAxisIdx);
     redrawBeginTime(beginTime,lang);
     redrawTimeAxis(view,lang);
     drawCursor(markerTime);
   };
   
   function zoomSignal(val) {
     switch (val) {
     case "zoom":
       zoom = true;       
       $('#zoom').attr('value',"ZOOM");        
     break;
     case "ZOOM":       
       zoom = false;
       eraseActiveCanvas(0);
       $('#zoom').attr('value',"zoom");        
     break;               
     };   
   };      

   function resetValue(val) {
     switch (val) {
       case ">0<":
         for (var m=0;m<MAX_CLICKMAPS;m++) {           
           for (var t=0;t<chartDef[m]["values"].length;t++) {
           
             if (Math.abs(chartDef[m]["times"][t] - markerTime) <= 5) {               
               for (var m=1;m<=MAX_CLICKMAPS;m++) {                      
                 chartDef[m].offset = chartDef[m]["values"][t];
                 reset = true;
                 chartDef[0].offset = markerTime;
                 view = $('#group').val();
                 lang = getLang();
                 redrawMarkerTime(markerTime,lang);
                 redrawValues(view,markerTime,lang);
                 $('#reset').attr('value',">>0<<");               
               };
             };
           };                    
         };       
       break;
       case ">>0<<":       
         reset = false;       
         for (var m=0;m<MAX_CLICKMAPS;m++) {               
           chartDef[m].offset = 0;         
         };
         $('#reset').attr('value',">0<");       
         view = $('#group').val();
         lang = getLang();
         redrawMarkerTime(markerTime,lang)
         redrawValues(view,markerTime,lang);        
       break;               
     };     
   };   

   function changeGroup(view) {
     for (m=1;m<=MAX_CLICKMAPS;m++) {       
      chartDef[m]["visibility"] = "true";         
     };
     
     getData(view,beginTime,timeAxisLength[timeAxisIdx]*3600);
     
     lang = getLang();
     redrawScreen();
     redrawTitle(view,lang);
     redrawChart(view,beginTime,markerTime,timeAxisIdx);
     redrawValues(view,markerTime,lang);
     redrawLegend(view,lang);
     redrawTimeAxis(view,lang);
     redrawBeginTime(beginTime,lang);
     drawCursor(markerTime);
   };
   
   function reload() {
     getData(view,beginTime,timeAxisLength[timeAxisIdx]*3600);
     
     view = $('#group').val();
     lang = getLang();
     redrawScreen();
     redrawTitle(view,lang);
     redrawChart(view,beginTime,markerTime,timeAxisIdx);
     redrawLegend(view,lang);
     redrawTimeAxis(view,lang);
     redrawValues(view,markerTime,lang);     
     redrawBeginTime(beginTime,lang);
     redrawMarkerTime(markerTime,lang);
   };

   function changeLang() {
   var lang = '',
       idx = 0;

     view = $('#group').val();
     lang = getLang();
     idx = config.LangEnb.indexOf(lang);

     if (idx < config.LangEnb.length-1) {
       $('#lang').attr('value',"lang:" + config.LangEnb[idx+1]);
       setTitles(view,config.LangEnb[idx+1]);
       setMultitexts(config.LangEnb[idx+1]);
       redrawTitle(view,config.LangEnb[idx+1]);       
       redrawLegend(view,config.LangEnb[idx+1]);
       redrawValues(view,markerTime,config.LangEnb[idx+1]);
       redrawMarkerTime(markerTime,config.LangEnb[idx+1]);
       redrawBeginTime(beginTime,config.LangEnb[idx+1]);       
     }
     else {
       $('#lang').attr('value',"lang:" + config.LangEnb[0]);
       setTitles(view,config.LangEnb[0]);
       setMultitexts(config.LangEnb[0]);
       redrawTitle(view,config.LangEnb[0]);
       redrawLegend(view,config.LangEnb[0]);
       redrawValues(view,markerTime,config.LangEnb[0]);
       redrawMarkerTime(markerTime,config.LangEnb[0]);
       redrawBeginTime(beginTime,config.LangEnb[0]);
     };
   };

   //nastavení tlačítka jazyk vstupem je string "CZ", "EN", apod.
   function setLang(lang) {   
     if (config.LangEnb.indexOf(lang) != -1) {
       $('#lang').attr('value',"lang:" + lang)
     }
     else {
       $('#lang').attr('value',"lang:" + config.LangEnb)
     };
   };

   //získání aktuálně nastaveného jazyka
   function getLang() {
   var lang;
   
     lang = $('#lang').val();
     lang = lang.substring(lang.length-2,lang.length);
     if (config.LangEnb.indexOf(lang) != -1) {
       return lang;
     }
     else return config.LangEnb;
   };

   function changeZone(zone) {
     switch (zone) {
     case "zone:CET":
       utcBias += 3*3600;
       $('#zone').attr('value',"zone:MOSCOW");
     break;                                                     
     case "zone:MOSCOW":
       utcBias -= 4*3600;
       $('#zone').attr('value',"zone:UTC");
     break;
     case "zone:UTC":
       utcBias += 3600;
       $('#zone').attr('value',"zone:CET");
     break;
     };

     view = $('#group').val();
     lang = getLang();
     redrawScreen();
     redrawTitle(view,lang);          
     redrawChart(view,beginTime,markerTime,timeAxisIdx);
     redrawBeginTime(beginTime,lang);
     redrawTimeAxis(view,lang);
     drawCursor(markerTime);
   };
   
   function goDateTime(dd,MM,yyyy) {
     $('#calendar').css('visibility','hidden');     
     beginTime = parseInt((Date.UTC(yyyy,MM-1,dd,-1,0,0,0) - Date.UTC(2000,0,1,0,0,0,0))/1000,10);
     
     view = $('#group').val();
     lang = $('#lang').val();
     timeAxisIdx = 6;
     redrawChart(view,beginTime,markerTime,timeAxisIdx);
     redrawTimeAxis(view,lang);
     redrawBeginTime(beginTime,lang);
     drawCursor(markerTime);     
   };

   function maxDays(mm,yyyy){
   var mDay;
   	 if((mm == 3) || (mm == 5) || (mm == 8) || (mm == 10)) {
   	 mDay = 30;
   	 }
     else {
       mDay = 31
    	 if(mm == 1) {
     	   if (yyyy/4 - parseInt(yyyy/4) != 0){
     		   mDay = 28
     		 }
    	   else {
     		   mDay = 29
    		 };
       };
     };
   return mDay;
   };
    
   function changeBg(id) {
   var selDD,
       selMM,
       selYYYY;
            	
     if (eval(id).className != "c3") {
       selDD = document.getElementById(id).innerHTML;
       selMM = $('#month').prop('selectedIndex')+1;
       selYYYY = $('#year').val();
       eval(id).style.backgroundColor = "yellow";
       goDateTime(selDD,selMM,selYYYY);
     };
   };
    
   function calendar() {
   var now = new Date,
       dd = now.getDate(),
       mm = now.getMonth(),
       dow = now.getDay(),
       yyyy = now.getFullYear(),  
       arrY = new Array(),       
       text = "";
        
     for (ii=0;ii<=4;ii++) {
       arrY[ii] = yyyy - 4 + ii;
     };

     if ($('#calendar').css('visibility') == "visible") {
       $('#calendar').css('visibility','hidden');
       return;
     }; 
     
     $('#calendar').css('visibility','hidden');
          
     var lang = getLang();       	

     switch (lang) {
     case "EN":
        arrM = monthsEN;      
        arrD = daysEN1;
        break;
      case "CZ":
        arrM = monthsCZ;
        arrD = daysCZ1;
        break;
      case "DE":
        arrM = monthsDE;
        arrD = daysDE1;
        break;
      case "PL":
        arrM = monthsPL;
        arrD = daysPL1;
        break;
      case "RU":
        arrM = monthsRU;
        arrD = daysRU1;
        break;    
      };

     text += "<form name=calForm>"
     text += "<table style=\"border-style: groove; border-width: 1px;\">"
   
     // měsíc
     text += "<tr><td>"
     text += "<table width=100% style=\"border-style: none; border-width: 1px;\"><tr>"
     text += "<td align=left>"
     text += "<select id=\'month\' name=selMonth onChange='changeCal()'>"
     for (ii=0;ii<=11;ii++) {
       if (ii==mm) {
         text += "<option value= " + ii + " Selected>" + arrM[ii] + "</option>"
    	   }
    	   else {
    	     text += "<option value= " + ii + ">" + arrM[ii] + "</option>"
    	   };
       };
     text += "</select>"
     text += "</td>"
    
     // rok
     text += "<td align=right>"
     text += "<select id=\'year\' name=selYear onChange='changeCal()'>"
    
     for (ii=4;ii>=0;ii--) {
       if (ii==4) {
         text += "<option value= " + arrY[ii] + " Selected>" + arrY[ii] + "</option>"
      	 }
      	 else {
      	   text += "<option value= " + arrY[ii] + ">" + arrY[ii] + "</option>"
      	 };
       };
    
     text += "</select>"
     text += "</td>"
     text += "</tr></table>"
     text += "</td></tr>"
    
     // názvy dnů
     text += "<tr><td>"
     text += "<table style=\"border-style: groove; border-width: 1px;\">"
     text += "<tr>"
     for (ii=0;ii<=6;ii++) {
       text += "<td align=center;><span style=\"color: black; width: 30px; height: 16px; font: bold 13px Arial;\">" + arrD[ii] + "</span></td>"		
     };	
     text += "</tr>"
    
     // dny
     aa = 0;
     for (kk=0;kk<=5;kk++) {
       text += "<tr>"
       for (ii=0;ii<=6;ii++) {      		  
         text += "<td align=center><span id=sp" + aa + " onClick='changeBg(this.id)'>1</span></td>"
    	   aa += 1;
       };
       text += "</tr>"
     };
     text += "</table>"
     text += "</td></tr>"
     text += "</table>"
     text += "</form>"     
     $('#calendar').html(text);
     $('#calendar').css('visibility','visible');     
     changeCal();
   };
    
   function changeCal() {
   var now = new Date,
       dd = now.getDate(),
       mm = now.getMonth(),
       dow = now.getDay(),
       yyyy = now.getFullYear(),
       currM = parseInt(document.calForm.selMonth.value),
       prevM;
    
     if (currM!=0) {
    	 prevM = currM - 1;
     }
     else {
       prevM = 11;
    	};
    	
   var currY = parseInt(document.calForm.selYear.value),
       mmyyyy = new Date();
        
     mmyyyy.setFullYear(currY);
     mmyyyy.setMonth(currM);
     mmyyyy.setDate(1);
   var day1 = mmyyyy.getDay();
     if (day1 == 0) {
       day1 = 7;
     };
   var arrN = new Array(41);
   var aa;
   
     //předešlý měsíc
     for (ii=0;ii<day1;ii++) {
       arrN[ii] = maxDays((prevM),currY) - day1 + 1 + ii;
     };
     
     //aktuální měsíc
     aa = 1;
     for (ii=day1;ii<=day1+maxDays(currM,currY)-1;ii++){
    	 arrN[ii] = aa;
    	 aa += 1;
     };
     
     //následující měsíc
     aa = 1;
     for (ii=day1+maxDays(currM,currY);ii<=43;ii++) {
       arrN[ii] = aa;
    	 aa += 1;
     };
    	
     for (ii=0;ii<=41;ii++) {
       eval("sp"+ii).style.backgroundColor = "#FFFFFF";
     };
     
   var dCount = 0
     for (ii=0;ii<=41;ii++) {	  
       if (((ii<7)&&(arrN[ii+1]>20))||((ii>27)&&(arrN[ii+1]<20))) {		  
    	   eval("sp"+ii).innerHTML = arrN[ii+1];
    	   eval("sp"+(ii)).className="c3";
    		 eval("sp"+(ii)).style.cursor="default";
    		 eval("sp"+(ii)).style.color="#b0b0b0";
    		 eval("sp"+(ii)).style.width="30px";
    		 eval("sp"+(ii)).style.height="16px";
    		 eval("sp"+(ii)).style.font="normal 12px Arial";
    		 eval("sp"+(ii)).style.backgroundColor="#F0F0F0";
    	 }
    	 else {		  
    	   eval("sp"+ii).innerHTML = arrN[ii+1];
    	   
    		 if (dCount==6) {
    		   eval("sp"+(ii)).className="c2";
    			 eval("sp"+(ii)).style.cursor="pointer";
    		 	 eval("sp"+(ii)).style.color="red";
    		 	 eval("sp"+(ii)).style.width="30px";
    		 	 eval("sp"+(ii)).style.height="16px";
    		 	 eval("sp"+(ii)).style.font="bold 13px Arial";
    		 	 eval("sp"+(ii)).style.backgroundColor="#F0F0F0";
    		 }
    		 else {   
           eval("sp"+(ii)).className="c1"; 		 	 
    		 	 eval("sp"+(ii)).style.cursor="pointer";
    		 	 eval("sp"+(ii)).style.color="black";
    		 	 eval("sp"+(ii)).style.width="30px";
    		 	 eval("sp"+(ii)).style.height="16px";
    		 	 eval("sp"+(ii)).style.font="bold 13px Arial";
    		 	 eval("sp"+(ii)).style.backgroundColor="#F0F0F0";
    		 };	 

    		 // dnes
    		 if ((arrN[ii]==dd)&&(mm==currM)&&(yyyy==currY)) {
    		   eval("sp"+(ii-1)).style.backgroundColor="#90EE90";
    		 };
    	 };
    	 dCount += 1
    	 if (dCount>6) {
    	   dCount=0
    	 };
     };
   };