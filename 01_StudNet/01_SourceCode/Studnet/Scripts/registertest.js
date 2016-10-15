function testname(x) {

    var f = document.forms['reg'];
    var WzorImienia = /^[A-Z ]+[a-z ]+$/
    if (!WzorImienia.test(f.user_name.value) || f.user_name.value == '') {
        
            x.style.background = "red";
            return false;
        

    }
    x.style.background = "white";
    return false;
}

function test_surname(x) {

    var f = document.forms['reg'];
    var WzorImienia = /^[A-Z ]+[a-z ]+$/
    if (!WzorImienia.test(f.user_surname.value) || f.user_surname.value == '') {

        x.style.background = "red";
        return false;


    }
    x.style.background = "white";
    return false;
}



function validEmail(x) {
    var f = document.forms['reg'];
    var WzorMaila = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!WzorMaila.test(f.user_mail.value) || f.user_mail.value == '')
    {
        x.style.background = "red";
        return false;
    }
    x.style.background = "white";
    return false;
}


function testuser_address_city(x) {

    var f = document.forms['reg'];
    var WzorImienia = /^[A-Z ]+[a-z ]+$/
    if (!WzorImienia.test(f.user_address_city.value) || f.user_address_city.value == '') {

        x.style.background = "red";
        return false;


    }
    x.style.background = "white";
    return false;
}

function testuser_address_street(x) {

    var f = document.forms['reg'];
    var WzorImienia = /^[A-Z ]+[a-z ]+$/
    if (!WzorImienia.test(f.user_address_street.value) || f.user_address_street.value == '') {

        x.style.background = "red";
        return false;


    }
    x.style.background = "white";
    return false;
}

function testuser_address_home_number(x) {

    var f = document.forms['reg'];
    var WzorImienia = /^[0-9]+$/
    if (!WzorImienia.test(f.user_address_home_number.value) || f.user_address_home_number.value == '') {

        x.style.background = "red";
        return false;


    }
    x.style.background = "white";
    return false;
}

function testuser_index(x) {

    var f = document.forms['reg'];
    var WzorImienia = /^[0-9]+$/
    if (!WzorImienia.test(f.user_index.value) || f.user_index.value == '') {

        x.style.background = "red";
        return false;


    }
    x.style.background = "white";
    return false;
}

function testuser_study_year(x) {

    var f = document.forms['reg'];
    var WzorImienia = /^[0-9]+$/
    if (!WzorImienia.test(f.user_study_year.value) || f.user_study_year.value == '') {

        x.style.background = "red";
        return false;


    }
    x.style.background = "white";
    return false;
}


function testuser_semester(x) {

    var f = document.forms['reg'];
    var WzorImienia = /^[0-9]+$/
    if (!WzorImienia.test(f.user_semester.value) || f.user_semester.value == '') {

        x.style.background = "red";
        return false;


    }
    x.style.background = "white";
    return false;
}

