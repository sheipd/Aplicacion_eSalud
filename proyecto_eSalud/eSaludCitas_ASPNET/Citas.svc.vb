Public Class CitasClass
    Implements ICitas

    Public Sub New()
    End Sub

    Public Function ObtenerKey(ByVal login As String, ByVal password As String) As String Implements ICitas.ObtenerKey
        Dim k As String = ""

        Try
            Using ctx As New eSaludCitas.eSaludEntities
                k = ctx.pacientes.Where(Function(u) u.login = login And u.password = password).Select(Function(s) s.C_key).FirstOrDefault
            End Using
        Catch ex As Exception
        End Try
        Return k
    End Function

    'obterner una lista de todas las especialidades que hay
    Public Function ObtenerEspecialidades() As List(Of Especialidades) Implements ICitas.ObtenerEspecialidades
        Dim result As New List(Of Especialidades)

        Try
            Using ctx As New eSaludCitas.eSaludEntities
                result = ctx.especialidades.Select(Function(u) New Especialidades With {.Id = u.id, .Denominacion = u.denominacion}).ToList
            End Using
        Catch ex As Exception
            result.Clear()
        End Try
        Return result
    End Function

    'dado el id de la especialidad, listar todos los medicos de dicha especialidad
    Public Function MedicosEspecialidad(ByVal id As String) As List(Of ListaMedicos) Implements ICitas.MedicosEspecialidad
        Dim result As New List(Of ListaMedicos)
        Dim idEsp = Int32.Parse(id)

        Try
            Using ctx As New eSaludCitas.eSaludEntities
                result = ctx.medicos.Where(Function(r) r.rama = idEsp).Select(Function(k) New ListaMedicos With {.NIF = k.nif, .Nombre = k.nombre, .Colegiado = k.colegiado, .Telefono = k.telefono, .Email = k.email}).ToList
            End Using
        Catch ex As Exception
            result.Clear()
        End Try
        Return result
    End Function

    'dado un dia y el colegiado de un medico, listar todas las horas libres de dicho medico
    Public Function HorasLibres(ByVal dia As String, ByVal colegiado As String) As List(Of ListaHoras) Implements ICitas.HorasLibres
        Dim result As New List(Of ListaHoras)
        Dim nif As String

        Try
            Using ctx As New eSaludCitas.eSaludEntities
                nif = ctx.medicos.Where(Function(u) u.colegiado = colegiado).Select(Function(s) s.nif).First
                result = ctx.cuadrantes.Where(Function(u) u.medico = nif And u.disponible = True And u.dia = dia).Select(Function(s) New ListaHoras With {.Dia = s.dia, .Hora = s.hora, .Disponible = s.disponible}).ToList
            End Using
        Catch ex As Exception
            result.Clear()
        End Try
        Return result
    End Function

    'dada la key del paciente y el id del cuadrante, reservar una cita para dicho paciente en ese cuadrante
    Function altaCita(ByVal keyP As String, ByVal idC As Int32) As String Implements ICitas.AltaCita
        Dim result As String
        Dim nif As String

        Try
            Using ctx As New eSaludCitas.eSaludEntities
                Dim dispon As New cuadrante()
                dispon = ctx.cuadrantes.Where(Function(d) d.id = idC).First
                dispon.disponible = False
                ctx.SaveChanges()

                Try
                    Using ctx2 As New eSaludCitas.eSaludEntities
                        nif = ctx2.pacientes.Where(Function(u) u.C_key = keyP).Select(Function(s) s.nif).First
                        Dim cita As New cita() With {.paciente = nif, .cuadrante = idC}

                        ctx2.Entry(cita).State = EntityState.Added
                        ctx2.SaveChanges()

                        result = "Cita reservada"
                    End Using
                Catch ex As Exception
                    result = "No se ha podido reservar la cita"
                End Try
            End Using
        Catch ex As Exception
            result = ex.Message
        End Try

        Return result
    End Function

    'dada la key del paciente, listar todas sus citas (id, nombre del medico, especialidad, dia y hora)
    Public Function listarCitas(ByVal keyP As String) As List(Of Listacitas) Implements ICitas.ListarCitas
        Dim result As New List(Of Listacitas)

        Try
            Using ctx As New eSaludCitas.eSaludEntities
                result = ctx.citas.Where(Function(u) u.CitaSuPaciente.C_key = keyP).Select(Function(s) New Listacitas With {.Id = s.id, .Especialidad = s.CitaSuCuadrante.cuadranteMedico.especialidadDelMedico.denominacion, .Nombre = s.CitaSuCuadrante.cuadranteMedico.nombre, .Dia = s.CitaSuCuadrante.dia, .Hora = s.CitaSuCuadrante.hora}).ToList
            End Using
        Catch ex As Exception
            result.Clear()
        End Try
        Return result
    End Function

    'borrar una cita dados la Key del paciente y el id de la cita
    Public Function borrarCitas(ByVal keyP As String, ByVal idCita As Int32) As String Implements ICitas.BorrarCitas
        Dim result As String
        Dim nif As String

        Try
            Using ctx As New eSaludCitas.eSaludEntities
                Dim dispon As New cuadrante()
                Dim idCuadrante As Int32

                idCuadrante = ctx.citas.Where(Function(u) u.id = idCita).Select(Function(s) s.CitaSuCuadrante.id).First
                dispon = ctx.cuadrantes.Where(Function(u) u.id = idCuadrante).First
                dispon.disponible = True
                ctx.SaveChanges()

                Try
                    Using ctx2 As New eSaludCitas.eSaludEntities
                        nif = ctx2.pacientes.Where(Function(u) u.C_key = keyP).Select(Function(s) s.nif).First
                        Dim cita As New cita()
                        cita = ctx2.citas.Where(Function(u) u.id = idCita And u.paciente = nif).First

                        ctx2.Entry(cita).State = EntityState.Deleted
                        ctx2.SaveChanges()

                        result = "Cita eliminada"
                    End Using
                Catch ex As Exception
                    result = "No se ha podido eliminar la cita"
                End Try
            End Using
        Catch ex As Exception
            result = ex.Message
        End Try
        Return result
    End Function
End Class
