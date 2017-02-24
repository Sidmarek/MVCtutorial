// Class pkTime
function Pktime() {
  // Pktime value - conversion to Date
  var pkTime = 0;
  // Date value - conversion to Pktime
  var utcTime = new Date(Date.UTC(2000,00,01,0,0,0,0));

  function setPktime(pktime) {
    var pktimeS = pktime.toString();

    var patt=/^\d+$/;
    var result=patt.test(pktimeS);

    if (!result) {
      throw new PktimeException("Illegal pkTime input value ('" + pktime + "').");
    } else {
      pkTime = pktime;
      resetY2K(pkTime);
    }
  }

  function setDate(date) {
    if (date instanceof Date) {
      if (date.getUTCFullYear() < 2000) {
        throw new PktimeException("Pktime is counted since 01 Jan 2000; 00:00:00. Your date is lower: " + date.toUTCString());
      }
      pkTime = parseInt((date.getTime() - Date.UTC(2000,0,1,0,0,0,0))/1000, 10);      
      utcTime = date;
    } else {
      throw new PktimeException("Illegal input type: " + typeof(date) + " instead of Date");
    }
  }
 
  this.setValue = function(value) {
    if (value instanceof Date) {      
      // Hodnota je typu Date - setDate()
      setDate(value);
    } else if (value instanceof Number || typeof value === "number") {
      // hodnota je typu Number - setPktime()
      setPktime(value);
    } else if (typeof value === "string") {
      // Hodnota je typu String - muze jit o hodnotu prevoditelnou na Number =-pktime
      var pattNum=/^\d+$/;
      // Hodnota je typu String - kontrola jestli lze konvertovat do Date
      var pattDate=/^(\d{4})\D+(\d{1,2})\D+(\d{1,2})\D*(\d{0,2})\D*(\d{0,2})\D*(\d{0,2})$/;
      if (pattNum.test(value)) {
	// Number hodnota - pktime
	setPktime(value);
      } else if (pattDate.test(value)) {
        // Date hodnota - datum
	var r=value.match(pattDate);
        var date = new Date(Date.UTC(r[1],r[2]-1,r[3],r[4],r[5],r[6]));
        setDate(date);
      } else {
	throw new PktimeException("Unsupported input string format. Legal format: unsigned integer or 'YYYY-M-D H:m:s'");
      }
    } else {
      throw new PktimeException("Unsupported input format.");
    }
  }

  function resetY2K(seconds) {
    utcTime.setUTCFullYear(2000);
    utcTime.setUTCMonth(0);
    utcTime.setUTCDate(1);
    utcTime.setUTCHours(0);
    utcTime.setUTCMinutes(0);
    utcTime.setUTCSeconds(seconds ? seconds : 0);
    utcTime.setUTCMilliseconds(0);
  }

  this.getPktime = function() {
    return pkTime;
  }
  this.getDate = function() {
    return utcTime;
  }
}

function PktimeException(errorMessage) {
  var message = errorMessage ? errorMessage : "Error";
  this.getMessage = function() {
    return message;
  }
}
