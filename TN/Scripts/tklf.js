//Mesajlar
function Mesaj() {
    var mesaj = $("#MesajIcerik").val();

    if (typeof foo !== 'undefined' || mesaj) {

        $.ajax({
            type: "POST",
            url: "/Mesaj/MsjYaz",
            data: {
                MesajIcerik: $('#MesajIcerik').val()
            },
            success: function () {
                window.location.reload();
            }
        });

        return true;
    }

    $("#errMsj").css("display", "block");
    return false;
}


$(".SilMesaj").click(function () {
    var ID = $(this).attr('name');
    var delTR = $(this).closest("div");
    $.confirm({
        title: 'Silme İşlemi !',
        content: 'Silmek istediğinizden emin misiniz ?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Sil',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        url: '/Mesaj/MesajSil/' + ID,
                        type: 'POST',
                        dataType: 'json',
                        success: function (result) {
                            if (result == "1") {
                                delTR.fadeOut(500, function () {
                                    delTR.remove();
                                    location.reload()
                                });
                            }
                            else {
                                alert("İşlem sırasında hata oluştu");
                            }
                        }
                    });
                }
            },
            Vazgeç: function () {
                $.alert('İşlemden vazgeçildi..');
            }
        }
    });
});

$(".Delete").click(function () {
    var ID = $(this).attr('name');
    $.confirm({
        title: 'Silme İşlemi !',
        content: 'Silmek istediğinizden emin misiniz ?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Sil',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        url: '/Mesaj/MsjSil/',
                        type: 'POST',
                        data: {
                            Gonderen: $('#Gonderen').val(),
                            UrunId: $('#UrunId').val()
                        },
                        success: function () {
                            window.location.href = "/GelenMesajlarım";
                        }
                    });
                }
            },
            Vazgeç: function () {
                $.alert('İşlemden vazgeçildi..');
            }
        }
    });
});
//---------------------

//Home butonlar
$(document).ready(function () {
    $('#Btn2').click(function () {
        alert('Üzgünüz Üye Olmanız Gerekmekte..')
    });
});
$(document).ready(function () {
    $('#BtnUye').click(function () {
        alert('Üzgünüz Üye Olmanız Gerekmekte..')
    });
});
$(document).ready(function () {
    $('#urun1').click(function () {
        alert('Üzgünüz Üye Olmanız Gerekmekte..')
    });
});
$(document).ready(function () {
    $('#urun2').click(function () {
        alert('Üzgünüz Üye Olmanız Gerekmekte..')
    });
});
$(document).ready(function () {
    $('#urun3').click(function () {
        alert('Üzgünüz Üye Olmanız Gerekmekte..')
    });
});
$(document).ready(function () {
    $('#urun4').click(function () {
        alert('Üzgünüz Üye Olmanız Gerekmekte..')
    });
});
//---------------

//Kullanici
$(".SilAktf").click(function () {
    var ID = $(this).attr('name');
    var delTR = $(this).closest("div");
    $.confirm({
        title: 'Silme İşlemi !',
        content: 'Silmek istediğinizden emin misiniz ?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Sil',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        url: '/Post/UrunSil/' + ID,
                        type: 'POST',
                        dataType: 'json',
                        success: function (result) {
                            if (result == "1") {
                                delTR.fadeOut(500, function () {
                                    delTR.remove();
                                    location.reload()
                                });
                            }
                            else {
                                alert("İşlem sırasında hata oluştu");
                            }
                        }
                    });
                }
            },
            Vazgeç: function () {
                $.alert('İşlemden vazgeçildi..');
            }
        }
    });
});
$(".Kaldir").click(function () {
    var ID = $(this).attr('name');
    $.confirm({
        title: 'Yayınlanma !',
        content: 'Yayından kaldırmak istediğinizden emin misiniz ?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Kaldır',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        url: '/Post/YayindanKaldir/' + ID,
                        type: 'POST',
                        success: function (result) {

                            if (result == "1") {
                                window.location.reload();
                            }
                            else {
                                alert("Lütfen daha sonra tekrar deneyiniz...");
                            }

                        }
                    });
                }
            },
            Vazgeç: function () {
                $.alert('İşlemden vazgeçildi..');
            }
        }
    });
});
function Teklifver() {
    $.ajax({
        type: "POST",
        url: "/Teklif/TeklifVer",
        data: {
            UrunId: $('#UrunId').val(),
            Tutar: $('#Tutar2').val()
        },
        success: function () {
            window.location.reload()
        }
    });
}
$(document).ready(function () {
    $('#Button1').click(function () {
        location.reload();
    });
});
function Mesaj3() {
    var mesaj = $("#MesajIcerik").val();

    if (typeof foo !== 'undefined' || mesaj) {

        $.ajax({
            type: "POST",
            url: "/Mesaj/MesajYaz",
            data: {
                MesajIcerik: $('#MesajIcerik').val()
            },
            success: function () {
                location.reload()
            }
        });
        return true;
    }

    $("#errMsj3").css("display", "block");
    return false;
}
$(document).ready(function () {
    $('#Button2').click(function () {
        location.reload();
    });
});

function TeklifKatGore() {
    $.ajax({
        type: "POST",
        url: "/Teklif/TeklifVer",
        data: {
            UrunId: $('#UrunId').val(),
            Tutar: $('#Tutar').val()
        },
        success: function () {
            window.location.reload()
        }
    });
}

$(".SilMesajlarim").click(function () {
    var ID = $(this).attr('name');
    var delTR = $(this).closest("div");
    $.confirm({
        title: 'Silme İşlemi !',
        content: 'Silmek istediğinizden emin misiniz ?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Sil',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        url: '/Mesaj/MsjSil/',
                        type: 'POST',
                        data: {
                            Gonderen: $('#Gonderen').val(),
                            UrunId: $('#UrunId').val()
                        },
                        success: function (result) {
                            if (result == "1") {
                                delTR.fadeOut(500, function () {
                                    delTR.remove();
                                    location.reload()
                                });
                            }
                            else {
                                alert("İşlem sırasında hata oluştu");
                            }
                        }
                    });
                }
            },
            Vazgeç: function () {
                $.alert('İşlemden vazgeçildi..');
            }
        }
    });
});

$(".SilPasif").click(function () {
    var ID = $(this).attr('name');
    var delTR = $(this).closest("div");
    $.confirm({
        title: 'Silme İşlemi !',
        content: 'Silmek istediğinizden emin misiniz ?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Sil',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        url: '/Post/UrunSil/' + ID,
                        type: 'POST',
                        dataType: 'json',
                        success: function (result) {
                            if (result == "1") {
                                delTR.fadeOut(500, function () {
                                    delTR.remove();
                                    location.reload()
                                });
                            }
                            else {
                                alert("İşlem sırasında hata oluştu");
                            }
                        }
                    });
                }
            },
            Vazgeç: function () {
                $.alert('İşlemden vazgeçildi..');
            }
        }
    });
});

$(".SilTekliflerim").click(function () {
    var ID = $(this).attr('name');
    var delTR = $(this).closest("div");
    $.confirm({
        title: 'Silme İşlemi !',
        content: 'Silmek istediğinizden emin misiniz ?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Sil',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        url: '/Teklif/TkfSil/',
                        type: 'POST',
                        data: {
                            TeklifVeren: $('#TeklifVeren').val(),
                            UrunId: $('#UrunId').val()
                        },
                        success: function (result) {
                            if (result == "1") {
                                delTR.fadeOut(500, function () {
                                    delTR.remove();
                                    location.reload()
                                });

                            }
                            else {
                                alert("İşlem sırasında hata oluştu");
                            }
                        }
                    });
                }
            },
            Vazgeç: function () {
                $.alert('İşlemden vazgeçildi..');
            }
        }
    });
});

function TeklifUrunIndex() {
    $.ajax({
        type: "POST",
        url: "/Teklif/TeklifVer",
        data: {
            UrunId: $('#UrunId2').val(),
            Tutar: $('#Tutar').val()
        },
        success: function () {
            window.location.reload()
        }
    });
}
$(document).ready(function () {
    $('#BtnIndx').click(function () {
        location.reload();
    });
});

$(".SilUrunlerim").click(function () {
    var ID = $(this).attr('name');
    var delTR = $(this).closest("div");
    $.confirm({
        title: 'Silme İşlemi !',
        content: 'Silmek istediğinizden emin misiniz ?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Sil',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        url: '/Post/UrunSil/' + ID,
                        type: 'POST',
                        success: function (result) {
                            if (result == "1") {
                                delTR.fadeOut(500, function () {
                                    delTR.remove();
                                    location.reload()
                                });

                            }
                            else {
                                alert("İşlem sırasında hata oluştu");
                            }
                        }
                    });
                }
            },
            Vazgeç: function () {
                $.alert('İşlemden vazgeçildi..');
            }
        }
    });
});

//---------

//Teklif
function formKontrol() {
    tutar = document.getElementById("Tutar");

    if (!tutar.checkValidity()) {
        $("#errMsj").css("display", "block");
        return false;
    } else {
        return true;
    }
}

$(".DeleteTekliflerim").click(function () {
    var ID = $(this).attr('name');
    var delTR = $(this).closest("tr");
    $.confirm({
        title: 'Silme İşlemi !',
        content: 'Silmek istediğinizden emin misiniz ?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Sil',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        url: '/Teklif/TkfSil/',
                        type: 'POST',
                        data: {
                            TeklifVeren: $('#TeklifVeren2').val(),
                            UrunId: $('#UrunId').val()
                        },
                        success: function (result) {
                            if (result == 1) {
                                    window.location.href = "/GelenTekliflerim";
                            }
                            else {
                                alert("İşlem sırasında hata oluştu");
                            }

                    }
                    });
}
            },
    Vazgeç: function () {
        $.alert('İşlemden vazgeçildi..');
    }
        }
    });
});

function MesajTeklif() {
    var mesaj = $("#MesajIcerik").val();
    if (typeof foo !== 'undefined' || mesaj) {
        $.ajax({
            type: "POST",
            url: "/Teklif/MesajYaz",
            data: {
                Kime: $('#Kime2').val(),
                UrunId: $('#UrunId').val(),
                MesajIcerik: $('#MesajIcerik').val()
            },
            success: function () {
                alert('Mesaj yollandı..'),
                    window.location.reload()
            }
        });
        return true;
    }
    $("#errMsj2").css("display", "block");
    return false;
}

function TeklifCvp() {
    var teklif = $("#Tutar").val();
    if (typeof foo != 'undefined' || teklif) {
        $.ajax({
            type: "POST",
            url: "/Teklif/TklVer",
            data: {
                UrunId: $('#UrunId').val(),
                Tutar: $('#Tutar').val()
            },
            success: function () {
                window.location.reload();
            }
        });
        return true;
    }
    $("#errMsj").css("display", "block");
    return false;
}

$(".SilTeklif").click(function () {
    var ID = $(this).attr('name');
    var delTR = $(this).closest("div");
    $.confirm({
        title: 'Silme İşlemi !',
        content: 'Silmek istediğinizden emin misiniz ?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Sil',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        url: '/Teklif/TeklifSil/' + ID,
                        type: 'POST',
                        dataType: 'json',
                        success: function (result) {
                            if (result == "1") {
                                delTR.fadeOut(500, function () {
                                    delTR.remove();
                                    window.location.reload()
                                });

                            }
                            else {
                                alert("İşlem sırasında hata oluştu");
                            }
                        }
                    });
                }
            },
            Vazgeç: function () {
                $.alert('İşlemden vazgeçildi..');
            }
        }
    });
});

//Urun----
function Tek() {
    var teklif = $("#Tutar").val();
    if (typeof foo != 'undefined' || teklif) {

        $.ajax({
            type: "POST",
            url: "/Teklif/TeklifVer",
            data: {
                UrunId: $('#UrunId').val(),
                Tutar: $('#Tutar').val()
            },
            success: function () {
                window.location.reload()
            }
        });
        return true;
    }
    $("#errMsj").css("display", "block");
    return false;
}



//iletisimmm

$(function () {
    $.ajaxSetup({
        type: "post",
        url: "/User/IlIlce",
        dataType: "json"
    });
    $.extend({
        ilGetir: function () {
            $.ajax({
                data: { "tip": "ilGetir" },
                success: function (sonuc) {
                    if (sonuc.ok) {

                        $.each(sonuc.text, function (index, item) {
                            var optionhtml = '<option value="' + item.Value + '">' + item.Text + '</option>';
                            $("#il").append(optionhtml);

                        });
                        $("#il").get().selectedIndex

                    } else {
                        $.each(sonuc.text, function (index, item) {
                            var optionhtml = '<option value="' + item.Value + '">' + item.Text + '</option>';
                            $("#il").append(optionhtml);

                        });

                    }
                }
            });
        },
        ilceGetir: function (ilId) {

            $.ajax({

                data: { "ilId": ilId, "tip": "ilceGetir" },
                success: function (sonuc) {

                    $("#ilce option").remove();
                    if (sonuc.ok) {

                        $("#ilce").prop("disabled", false);
                        $.each(sonuc.text, function (index, item) {
                            var optionhtml = '<option value="' + item.Value + '">' + item.Text + '</option>';
                            $("#ilce").append(optionhtml);
                        });

                    } else {
                        $.each(sonuc.text, function (index, item) {
                            var optionhtml = '<option value="' + item.Value + '">' + item.Text + '</option>';
                            $("#ilce").append(optionhtml);

                        });
                    }
                }
            });
        }
    });

    $.ilGetir();

    $("#il").on("change", function () {

        var ilId = $(this).val();

        $.ilceGetir(ilId);
    });
});

//----------
//aramasonuc----

function MesajArama() {
    $.ajax({
        type: "POST",
        url: "/Mesaj/MesajYaz",
        data: {
            MesajIcerik: $('#MesajIcerik').val()
        },
        success: function () {
            alert('Mesaj yollandı..'),
                window.location.reload()
        }
    });
}

function TeklifIlanAra() {
    $.ajax({
        type: "POST",
        url: "/Teklif/TeklifVer",
        data: {
            UrunId: $('#UrunId').val(),
            Tutar: $('#Tutar').val()
        },
        success: function () {
            window.location.reload()
        }
    });
}

$(document).ready(function () {
    $('#btnara').click(function () {
        location.reload();
    });
});