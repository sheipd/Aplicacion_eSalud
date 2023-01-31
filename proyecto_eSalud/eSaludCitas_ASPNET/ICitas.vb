<ServiceContract()>
Public Interface ICitas

    <OperationContract()>
    <WebInvoke(UriTemplate:="paciente/key", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, Method:="POST", BodyStyle:=WebMessageBodyStyle.Wrapped)>
    Function ObtenerKey(ByVal login As String, ByVal password As String) As String

    <OperationContract()>
    <WebInvoke(UriTemplate:="especialidades", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, Method:="GET", BodyStyle:=WebMessageBodyStyle.Wrapped)>
    Function ObtenerEspecialidades() As List(Of Especialidades)

    <OperationContract()>
    <WebInvoke(UriTemplate:="medicosEspecialidad/{id}", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, Method:="GET", BodyStyle:=WebMessageBodyStyle.Wrapped)>
    Function MedicosEspecialidad(ByVal id As String) As List(Of ListaMedicos)

    <OperationContract()>
    <WebInvoke(UriTemplate:="horasLibres/{dia}/{colegiado}", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, Method:="GET", BodyStyle:=WebMessageBodyStyle.Wrapped)>
    Function HorasLibres(ByVal dia As String, ByVal colegiado As String) As List(Of ListaHoras)

    <OperationContract()>
    <WebInvoke(UriTemplate:="altaCita", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, Method:="POST", BodyStyle:=WebMessageBodyStyle.Wrapped)>
    Function AltaCita(ByVal keyP As String, ByVal idC As Int32) As String

    <OperationContract()>
    <WebInvoke(UriTemplate:="listarCitas", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, Method:="POST", BodyStyle:=WebMessageBodyStyle.Wrapped)>
    Function ListarCitas(ByVal keyP As String) As List(Of Listacitas)

    <OperationContract()>
    <WebInvoke(UriTemplate:="borrarCita", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, Method:="POST", BodyStyle:=WebMessageBodyStyle.Wrapped)>
    Function BorrarCitas(ByVal keyP As String, ByVal idCita As Int32) As String
End Interface

<DataContract()>
Public Class Especialidades

    <DataMember()>
    Public Property Id() As Int32

    <DataMember()>
    Public Property Denominacion() As String
End Class

<DataContract()>
Public Class ListaMedicos
    <DataMember()>
    Public Property NIF() As String

    <DataMember()>
    Public Property Nombre() As String

    <DataMember()>
    Public Property Colegiado() As String

    <DataMember()>
    Public Property Telefono() As String

    <DataMember()>
    Public Property Email() As String
End Class

<DataContract()>
Public Class ListaHoras
    <DataMember()>
    Public Property Dia() As String

    <DataMember()>
    Public Property Hora() As String

    <DataMember()>
    Public Property Disponible() As Boolean
End Class

<DataContract()>
Public Class Listacitas
    <DataMember()>
    Public Property Id() As Int32

    <DataMember()>
    Public Property Especialidad() As String

    <DataMember()>
    Public Property Nombre() As String

    <DataMember()>
    Public Property Dia() As String

    <DataMember()>
    Public Property Hora() As String
End Class