var pathServidor = 'http://ws.iesoretania.es';
var pathPacienteKey = '/paciente/key'; //POST (login, password)
var pathEspecialidades = '/especialidades'; //GET
var pathEspecialistas = '/especialistas'; //GET 
var pathCuadrante = '/cuadrante'; //GET 
var pathCitaAlta = '/cita/alta'; //POST (keyPaciente, idCuadrante)
var pathCitaCancelar = '/cita/cancelar'; //POST (keyPaciente, idCuadrante)
var pathPacienteCitas = '/paciente/citas'; //POST (keyPaciente)

var botonInicio;
var misCitas;
var pedirCita; 
var cerrarSesion;

var fsLogin;
var fsMisCitas;
var fsPedirCita;
var slEspecialidades;
var slEspecialistas;
var inputDia;
var slHoras;
var btPedirCita;
var lbAdios;
var ddUsuario;

var idPaciente;
var colegiado;

document.addEventListener('DOMContentLoaded', function(){
    botonInicio = document.getElementById("btnEnviar");
    misCitas = document.getElementById("misCitas");
    pedirCita = document.getElementById("pedirCita");
    cerrarSesion = document.getElementById("cerrarSesion");

    fsLogin = document.getElementById("fsLogin");
    fsMisCitas = document.getElementById("fsMisCitas");
    fsPedirCita = document.getElementById("fsPedirCita");
    slEspecialidades = document.getElementById("slEspecialidades");
    slEspecialistas = document.getElementById("slEspecialistas");
    inputDia = document.getElementById("dia");
    slHoras = document.getElementById("slHora");
    btPedirCita = document.getElementById("btPedirCita");
    lbAdios = document.getElementById("lbAdios");
    ddUsuario = document.getElementById("usuario");

    login = document.getElementById("login");
    password = document.getElementById("password");

    botonInicio.addEventListener('click', function(){
        iniciarSesion(login.value, password.value);
    });

    misCitas.addEventListener('click', function(){
        if(sessionStorage.getItem(login.value) != null){
            fsMisCitas.innerHTML = "<legend>Mis citas</legend>";

            listarCitas();

            if(!fsLogin.classList.contains("ocultar")) fsLogin.classList.add("ocultar");
            if(fsMisCitas.classList.contains("ocultar")) fsMisCitas.classList.remove("ocultar");
            if(!fsPedirCita.classList.contains("ocultar")) fsPedirCita.classList.add("ocultar");
        } else {
            alert("Debes iniciar sesión antes.");
        }
    })

    pedirCita.addEventListener('click', function(){
        if(sessionStorage.getItem(login.value) != null){
            slEspecialidades.innerHTML = "";

            listarEspecialidades();

            if(!fsLogin.classList.contains("ocultar")) fsLogin.classList.add("ocultar");
            if(!fsMisCitas.classList.contains("ocultar")) fsMisCitas.classList.add("ocultar");
            if(fsPedirCita.classList.contains("ocultar")) fsPedirCita.classList.remove("ocultar");
        } else {
            alert("Debes iniciar sesión antes.");
        }
    })

    cerrarSesion.addEventListener('click', function(){
        if(sessionStorage.getItem(login.value) != null){
            sessionStorage.removeItem(login.value);
            fieldsets = document.getElementsByTagName("fieldset");

            if(fsLogin.classList.contains("ocultar")) fsLogin.classList.remove("ocultar");
            if(!document.getElementById("lbBienvenido").classList.contains("ocultar")){
                elementosFsLogin = fsLogin.children;
            
                for(let i = 0; i < elementosFsLogin.length; i++){
                    elementosFsLogin[i].classList.toggle("ocultar");
                }
            } 
            if(!fsMisCitas.classList.contains("ocultar")) fsMisCitas.classList.add("ocultar");
            if(!fsPedirCita.classList.contains("ocultar")) fsPedirCita.classList.add("ocultar");
            if(!ddUsuario.classList.contains("ocultar")) ddUsuario.classList.add("ocultar");
            ddUsuario.textContent = "Usuario: ";

            fsMisCitas.innerHTML = "<legend>Mis citas</legend>";
            slEspecialidades.innerHTML = "";
            slEspecialistas.innerHTML = "";
            slHoras.innerHTML = "";
        } else {
            alert("Debes iniciar sesión antes.");
        }
    })
})

function iniciarSesion(login, password){
    let request = new XMLHttpRequest();
    let response;
    
    request.open('POST', pathServidor + pathPacienteKey); 
    request.setRequestHeader('Content-Type', 'application/json'); 
    request.send(JSON.stringify({ login: login, password: password }));

    request.addEventListener('load', function () {
        response = request.responseText;
        
        if (request.status == 200 && request.readyState == 4) {
            sessionStorage.setItem(login, response);
            
            idPaciente = response;
            elementosFsLogin = fsLogin.children;
            
            for(let i = 0; i < elementosFsLogin.length - 1; i++){
                elementosFsLogin[i].classList.toggle("ocultar");
            }

            if(!lbAdios.classList.contains("ocultar")) lbAdios.classList.add("ocultar");
            if(ddUsuario.classList.contains("ocultar")){
                ddUsuario.classList.remove("ocultar");
                ddUsuario.textContent += login;
            }
        } else {
            alert("No se ha podido conectar con el servidor.");
        }
    });
}

function listarCitas(){
    let request = new XMLHttpRequest();
    let response;
    
    request.open('POST', pathServidor + pathPacienteCitas); 
    request.setRequestHeader('Content-Type', 'application/json'); 
    request.send(JSON.stringify({ key: idPaciente}));

    request.addEventListener('load', function () {
        response = JSON.parse(request.responseText);

        if (request.status == 200 && request.readyState == 4) {
            let i = 1;
            response.forEach(element => {
                label = document.createElement("label");
                text = document.createTextNode(i + ".- Dia: " + element.dia + " | Hora: " + element.hora + " | Doctor: " + element.doctor + " | Especialidad: " + element.especialidad);
                i++;
                label.appendChild(text);

                boton = document.createElement("button");
                btTexto = document.createTextNode("Cancelar cita");

                boton.appendChild(btTexto);

                fsMisCitas.appendChild(label);
                fsMisCitas.appendChild(boton);
                
                boton.addEventListener('click', function(){
                    cancelarCita(element.id);
                })
            });
        } else {
            alert("Este paciente no tiene citas.");
        }
    });
}

function listarEspecialidades(){
    let request = new XMLHttpRequest();
    let response;

    request.open('GET', pathServidor + pathEspecialidades);
    request.send();

    request.addEventListener('load', function () {
        if (this.status == 200 && this.readyState == 4) {
            response = JSON.parse(this.responseText);
            
            for(let i = 0; i < response.length; i++){
                opcion = new Option(response[i].denominacion, response[i].id);
                slEspecialidades.appendChild(opcion);
            }

            slEspecialidades.addEventListener('change', function () {
                idEspecialidad = this.value;

                listarEspecialistas(idEspecialidad);
            });
        }
    });
}

function listarEspecialistas(idEspecialidad){
    let request = new XMLHttpRequest();
    let response;

    request.open('GET', pathServidor + pathEspecialistas + `/${idEspecialidad}`);
    request.setRequestHeader('Content-Type', 'application/json');
    request.send();

    request.addEventListener('load', function () {
        response = JSON.parse(this.responseText);
        slEspecialistas.innerHTML = "";
        
        if (this.status == 200 && this.readyState == 4) {

            for(let i = 0; i < response.length; i++){
                opcion = new Option(response[i].nombre, response[i].colegiado);
                slEspecialistas.appendChild(opcion);
            }

            slEspecialistas.addEventListener('change', function () {
                colegiado = this.value;
                
                inputDia.addEventListener('change', function() {
                    ponerCuadrante(colegiado);
                })
            });
        } else {
            alert("Esta especialidad no contiene especialistas.");
        }
    });
}

function ponerCuadrante(colegiado){
    let request = new XMLHttpRequest();
    let response;

    dia = inputDia.value;

    request.open('GET', pathServidor + pathCuadrante + `?dia=${dia}&colegiado=${colegiado}`);
    request.setRequestHeader('Content-Type', 'application/json');
    request.send();

    request.addEventListener('load', function () {
        response = JSON.parse(this.responseText);
    
        slHoras.innerHTML = "";
        if (this.status == 200 && this.readyState == 4) {

            for(let i = 0; i < response.length; i++){
                opcion = new Option(response[i].hora, response[i].id);
                slHoras.appendChild(opcion);
            }

            slHoras.addEventListener('change', function () {
                idCuadrante = this.value;

                btPedirCita.addEventListener('click', function(){
                    darAlta(idCuadrante);
                });
            });
        } else {
            alert(response.message);
        }
    });
}

function darAlta(idCuadrante){
    let request = new XMLHttpRequest();
    let response;
    
    request.open('POST', pathServidor + pathCitaAlta); 
    request.setRequestHeader('Content-Type', 'application/json'); 
    request.send(JSON.stringify({ key: idPaciente, id: idCuadrante }));

    request.addEventListener('load', function () {
        response = request.responseText;
        
        if (request.status == 200 && request.readyState == 4) {
            alert("Cita reservada correctamente.");
            
            listarEspecialidades();
        } else {
            alert("No se ha podido conectar con el servidor.");
        }
    });
}

function cancelarCita(idCita){
    let request = new XMLHttpRequest();
    let response;
    
    request.open('POST', pathServidor + pathCitaCancelar); 
    request.setRequestHeader('Content-Type', 'application/json'); 
    request.send(JSON.stringify({ key: idPaciente, id: idCita }));

    request.addEventListener('load', function () {
        if (request.status == 200 && request.readyState == 4) {
            alert("Cita cancelada correctamente.");
            fsMisCitas.innerHTML = "<legend>Mis citas</legend>";
            listarCitas();
        } else {
            alert("No se ha podido conectar con el servidor.");
        }
    });
}