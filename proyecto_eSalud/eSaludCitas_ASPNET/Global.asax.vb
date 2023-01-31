Imports System.ServiceModel.Activation
Imports System.Web.Routing

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' ServiceRoute
        ' Habilita la creación de rutas de servicio sobre HTTP para servicios WCF con compatibilidad con direcciones base sin extensión.
        RouteTable.Routes.Add(New ServiceRoute("citas", New WebServiceHostFactory, GetType(CitasClass)))
    End Sub


    ' Para las solicitudes de dominios cruzados, establecer el tipo de contenido en algo distinto a application / x - www - form - urlencoded, multipart / form - data o text / plain 
    ' activará el navegador para enviar una solicitud de OPTIONS de verificación previa al servidor.
    ' Entonces, el servidor permite la solicitud de origen cruzado pero no permite Access - Control - Allow - Headers. Si hay una solicitud previa de OPTIONS establecemos las cabeceras para POST
    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*")
        If HttpContext.Current.Request.HttpMethod = "OPTIONS" Then
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS")
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept")
            HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", " 1728000")
            HttpContext.Current.Response.End()
        End If
    End Sub

End Class