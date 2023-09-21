Imports System.Data.SqlClient
Imports System.Data
Imports System.Xml
Public Enum Paso
    almacen = 0
    Fabricacion
    Coloracion
    Endurecido
    Antireflejo
    Toplight
    Tratamiento
    Calidad
    Montaje
    Externa
    RectificarColor
End Enum

Public Enum Proceso As Integer
    Entrada
    Salida
End Enum

Public Class clsDatos

    Private con2 As String
    Private mcon As SqlConnection
    Public Function PedidoDevuelto(ByVal pedido As Integer) As Boolean
        Dim cad As String = "select count(*) from t_devoluciones_stock where id_pedido=" & pedido & " and salida=0"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        PedidoDevuelto = CBool(cmd.ExecuteScalar)
        mcon.Close()
    End Function
    Public Sub GrabaSalidaDevolucionStock(ByVal pedido As Integer)
        Dim cad As String = "UPDATE t_devoluciones_stock set salida=1 where id_pedido=" & pedido
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetLenteStockDevuelta(ByVal pedido As Integer) As DataTable
        Dim cad As String = "Select * from t_devoluciones_stock where id_pedido=" & pedido & " and salida=0"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Sub GrabaDevolucionStock(ByVal pedido As Integer, ByVal idProveedor As Integer, ByVal lente As Integer, ByVal lote As String)
        Dim cad As String = "INSERT INTO t_devoluciones_stock (fecha,id_pedido,salida,id_proveedor,id_lente,lote) VALUES (" & FechaAcadena(Now.Date) & "," & pedido & ",0," & idProveedor & "," & lente & "," & strsql(lote) & ")"
        'ahora añadimos una lente al almacen de stock
        cad = cad & vbNewLine & "UPDATE t_lentes_stock set stock=stock+1 where id_producto=" & lente
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetSalidaStockBypedido(ByVal idpedido As Integer) As DataTable
        Dim cad As String = "select top 1 * from t_salida_stock where id_pedido=" & idpedido & " order by orden DESC"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetLMAT(ByVal idmodelo As Integer) As Integer
        Dim cad As String = "select convert(nvarchar(2),material)+replace(indice_modelo,'.','') from t_modelos where id_lente=" & idmodelo
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        GetLMAT = cmd.ExecuteScalar
        mcon.Close()
    End Function
    Public Function GetTipoPegatinaByequipo() As Boolean
        Dim cad As String = "select Top 1 pegatina_udi from t_equipos where equipo=" & strsql(Equipo) & " order by pegatina_udi desc"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        GetTipoPegatinaByequipo = cmd.ExecuteScalar
        mcon.Close()
End Function
    Public Function InfPedidosHoras(ByVal fecini As Integer, ByVal fecfin As Integer) As DataTable
        Dim cad As String = "select fecha,hora, sum(Telefono) as telefono,sum(web) as web from ((select dbo.fechaexcel(fecha) as fecha,substring(dbo.hora(hora),1,2) as Hora,count(*) as telefono,0 as web  from t_pedidos where id_usuario>0 and  fecha>=" & fecini & " and fecha<=" & fecfin & " GROUP BY fecha, Substring(dbo.hora(hora),1,2)))" & _
        " UNION (select dbo.fechaexcel(fecha),substring(dbo.hora(hora),1,2) as Hora,0 as telefono,count(*) as web  from t_pedidos where id_usuario<0 and  fecha>=" & fecini & " and fecha<=" & fecfin & ") AS TABLA1 GROUP BY fecha, Substring(dbo.hora(hora),1,2))) GROUP BY fecha,hora"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    Public Function GetclientesSinPorte() As DataTable
        Dim cad As String = "select t_clientes.id_cliente,codigo,nombre_comercial,fabricacion from t_clientes INNER JOIN t_clientes_sin_porte ON t_clientes.id_cliente=t_clientes_sin_porte.id_cliente ORDER BY codigo"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetEspesores() As DataTable
        Dim cad As String = "select * from t_espesores"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Sub GrabaBaseEspesores(ByVal id As Integer, ByVal Nominal As String, ByVal Real As Decimal, ByVal espesor As Decimal, ByVal Desde As Decimal, ByVal Hasta As Decimal)
        Dim cad As String
        
        cad = "DELETE FROM t_base_espesor where id_espesor=" & id & " and base=" & NumSql(Nominal) & vbNewLine & _
    "INSERT INTO t_base_espesor (id_espesor,base,curva,espesor,desde,hasta) VALUES (" & _
 id & "," & NumSql(Nominal) & "," & NumSql(Real) & "," & NumSql(espesor) & "," & NumSql(Desde) & "," & NumSql(Hasta) & ")"
        
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Sub EliminaBaseEspesor(ByVal id As Integer, ByVal base As Decimal)
        Dim cad As String = "DELETE FROM t_base_espesor where id_espesor=" & id & " and Base=" & NumSql(base)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetBasesEspesoresById(ByVal id As Integer) As DataTable
        Dim Cad As String = "Select * from t_base_espesor where id_espesor=" & id & " order by base"
        Dim mda As New SqlDataAdapter(Cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GrabaEspesores(ByVal id As Integer, ByVal nombre As String, ByVal indice As Decimal, ByVal monofocal As String, ByVal progresivo As String) As Integer
        Dim cad As String
        If id = 0 Then
            id = getMaxId("ID_ESPESOR", "T_ESPESORES") + 1
            cad = "INSERT INTO t_espesores (id_espesor,nombre,indice,monofocal,progresivo) VALUES (" & id & "," & strsql(nombre) & "," & NumSql(indice) & "," & strsql(monofocal) & "," & strsql(progresivo) & ")"
        Else
            cad = "UPDATE t_espesores SET nombre=" & strsql(nombre) & ",indice=" & NumSql(indice) & ",monofocal=" & strsql(monofocal) & ",progresivo=" & strsql(progresivo) & " where id_espesor=" & id
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        Return id
    End Function
    Public Function GrabaEspesoresModelo(ByVal Espesor As Integer, ByVal modelo As Integer) As Integer
        Dim cad As String = " declare @cuenta integer " & vbNewLine & "select @cuenta=count(*) from t_modelos_espesor" & " WHERE id_modelo=" & modelo & vbNewLine & _
        "if @cuenta=0" & vbNewLine & "INSERT INTo t_modelos_espesor (id_espesor,id_modelo) VALUES (" & Espesor & "," & modelo & ")" & vbNewLine & _
        "else " & vbNewLine & " UPDATE t_modelos_espesor set id_espesor=" & Espesor & " WHERE id_modelo=" & modelo
        
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        Return Espesor
    End Function
    Public Sub QuitaEspesoresModelo(ByVal modelo As Integer)
        Dim cad As String = " DELETE from t_modelos_espesor  WHERE id_modelo=" & modelo

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Sub GrabaCodigoGrupo(ByVal Grupo As Integer, ByVal idCli As Integer, ByVal codigo As String)
        Dim cad As String
        If codigo = "" Then
            cad = "DELETE FROM t_codigos_grupo_OPTICO where id_cliente=" & idCli & " and id_grupo=" & Grupo
        Else
            cad = "DECLARE @cuenta integer" & vbNewLine & "Select @cuenta=count(*) from t_codigos_grupo_optico where id_cliente=" & idCli & vbNewLine & _
            "IF @cuenta=0 " & vbNewLine & _
            " INSERT INTO t_codigos_grupo_optico (id_grupo,id_cliente,codigo) VALUES (" & Grupo & "," & idCli & "," & strsql(codigo) & ")" & vbNewLine & _
            "if @cuenta=1" & vbNewLine & "UPDATE t_codigos_grupo_optico set codigo=" & strsql(codigo) & " where id_cliente=" & idCli & " and id_grupo=" & Grupo
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetLTYP(ByVal idmodelo As Integer) As Integer
        Dim cad As String = "select id_web from m_tipologia where id_tipo=(select tipologia from  t_modelos where id_lente=" & idmodelo & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        GetLTYP = cmd.ExecuteScalar
        mcon.Close()
    End Function
    Public Function GetContactoById(ByVal id As Integer) As clsCliente
        Dim cli As New clsCliente
        Dim cad As String = "select * from comercial.dbo.t_contactos where id_contacto=" & id
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        If tb.Rows.Count > 0 Then
            cli.id = tb.Rows(0)("id_contacto")
            cli.Nombre_Comercial = tb.Rows(0)("nombre")
            cli.poblacion = tb.Rows(0)("localidad")
            cli.Provincia = tb.Rows(0)("id_provincia")
            cli.direccion = tb.Rows(0)("direccion")
            cli.telefono = tb.Rows(0)("telefono")
            cli.Email = tb.Rows(0)("email")
            cli.Codigo_Postal = tb.Rows("0")("cod_postal")
            cli.Persona_Contacto = tb.Rows(0)("contacto")
            cli.GrupoOptico = tb.Rows(0)("grupo_optico")
            cli.Notas = tb.Rows(0)("notas")
        End If
        Return cli
    End Function
    Public Function GetNombreContactoByid(ByVal id As Integer) As String

        id = Math.Abs(id)
        Dim cad As String = "Select nombre from comercial.dbo.t_contactos where id_contacto=" & id
        Dim contacto As String
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        contacto = cmd.ExecuteScalar
        mcon.Close()
        Return contacto
    End Function
    Public Function InfPedidosColorBycliente(ByVal fecini As Integer, ByVal fecfin As Integer, Optional ByVal idcli As Integer = 0) As DataTable
        Dim cad As String = "select codigo,nombre_comercial AS Cliente,provincia,count(*) as color from t_clientes INNER JOIN m_provincias ON m_provincias.id_provincia=t_clientes.id_provincia INNER JOIN t_pedidos ON t_pedidos.id_cliente=t_clientes.id_cliente where anulado=0 and id_coloracion<>0 " & IIf(idcli = 0, "", " and t_pedidos.id_cliente=" & idcli) & " and fecha>=" & fecini & " and fecha<=" & fecfin & " group by codigo,nombre_comercial,provincia"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return (tb)
    End Function
    Public Function InfPedidosColorBycolores(ByVal fecini As Integer, ByVal fecfin As Integer, Optional ByVal agrupado As Boolean = False) As DataTable
        Dim cad As String
        cad = " Select coloracion,sum(Fabrica) as Fabrica,sum(stock) as Stock, sum(fabrica+stock) as Pedidos  FROM ((select gama+ ' ' + Color as coloracion,count(*) as Fabrica, 0 as Stock from t_pedidos INNER JOIN t_coloraciones ON t_pedidos.id_coloracion=t_coloraciones.id_coloracion INNER JOIN t_ordenes_trabajo ON t_ordenes_trabajo.id_pedido=t_pedidos.id_pedido where   modo='F' and fs_coloracion>=" & fecini & "  and fs_coloracion<=" & fecfin & "  Group by Gama + ' ' + color,t_coloraciones.id_coloracion) UNION " & _
        "(select gama+ ' ' + Color as coloracion,0 as Fabrica, count(*) as Stock from t_pedidos INNER JOIN t_coloraciones ON t_pedidos.id_coloracion=t_coloraciones.id_coloracion INNER JOIN t_ordenes_trabajo ON t_ordenes_trabajo.id_pedido=t_pedidos.id_pedido where   modo<>'F' and fs_coloracion>=" & fecini & "  and fs_coloracion<=" & fecfin & "  Group by Gama + ' ' + color,t_coloraciones.id_coloracion)) as tabla1 GROUP BY  Coloracion"

        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return (tb)
    End Function
    Public Function GetMateriaSecaByFechas(ByVal fecini As Integer, ByVal fecfin As Integer) As DataTable
        Dim cad As String = "select fecha,laca,valor,correccion,(select nombre + ' ' + apellidos from t_usuarios where id_usuario=t_materia_seca.id_usuario) as usuario from t_materia_seca where fecha>=" & fecini & " and fecha<=" & fecfin
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    Public Sub GrabaMantenimientoCubetas(ByVal cubeta As Integer, ByVal dias As Integer, ByVal lote As String)
        Dim ProximaFecha As Date = DateAdd(DateInterval.Day, dias, Now.Date)
        Dim cad As String = "INSERT INTO t_mto_cubetas (id_cubeta,entrada,id_usuario,cambio,lote) " & _
        " VALUES (" & cubeta & ",dbo.fechatotimestamp(Getdate())," & mUsuario.id & ",dbo.fechatotimestamp(" & strsql(ProximaFecha) & ")," & strsql(lote) & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub

    Public Sub GrabaMateriaSeca(ByVal laca As String, ByVal valor As Decimal, ByVal correccion As String)
        Dim cad As String = "Insert into t_materia_seca (fecha,laca,valor,id_usuario,correccion) VALUES (" & FechaAcadena(Now.Date) & "," & strsql(laca) & "," & NumSql(valor) & "," & mUsuario.id & "," & strsql(correccion) & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function MantenimientosCubetasColor() As DataTable
        'aqui devolvemos todas las cubetas con las fechas a las que se les debia hacer el mantenimiento
        Dim cad As String = "Select id_cubeta,cubeta,colorante,isnull((Select top 1 dbo.timestamptofecha(entrada) as fecha from t_mto_cubetas where id_cubeta=t_cubetas_coloracion.id_cubeta order by fecha desc )," & strsql(Now.Date) & ") as fecha,modo_empleo,dias,ISNULL((select top 1 dbo.TimeStamptofecha(cambio) from t_mto_cubetas where id_cubeta=t_cubetas_coloracion.id_cubeta order by entrada desc,cambio desc)," & strsql(Now.Date) & ") as proximo from t_cubetas_coloracion " & _
        " INNER JOIN t_colorantes ON t_cubetas_coloracion.id_colorante=t_colorantes.id_colorante WHERE BAJA=0 ORDER BY proximo"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetCubetaByid(ByVal id As Integer) As String
        Dim cad As String = "Select cubeta from t_cubetas_coloracion where id_cubeta=" & id
        Dim cubeta As String
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cubeta = cmd.ExecuteScalar
        mcon.Close()
        Return cubeta
    End Function

    Public Function ExisteMateriaSeca() As Boolean
        ' devuelve si se ha hecho el test de materia seca o no
        Dim cad As String = " select count(*) from t_materia_seca where fecha=" & FechaAcadena(Now.Date)
        Dim cmd As New SqlCommand(cad, mcon)
        Dim cuenta As Integer
        mcon.Open()
        cuenta = cmd.ExecuteScalar
        mcon.Close()
        If cuenta < 2 Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function GetPasosColor(ByVal idColor As Integer) As ArrayList
        'matriz con los pasos de un color
        Dim lista As New ArrayList
        Dim cad As String = "select * from t_pasos_coloracion where id_color=" & idColor & " order by paso"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows
            Dim p As New clsPasoColor
            p.Idcolor = idColor
            p.Paso = rw("paso")
            p.Cubeta15 = rw("id_cubeta_15")
            p.Cubeta16 = rw("id_cubeta_16")
            p.Cubeta167 = rw("id_cubeta_167")
            Dim tiempos As New ArrayList

            p.tiempos = GetListaTiempoColorBypaso(p.Idcolor, p.Paso)
            lista.Add(p)
        Next
        Return lista
    End Function
    Public Function GetPasosColorByPedido(ByVal idpedido As Integer, ByVal orden As Integer) As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select paso,cubeta,colorante + ' LOT. ' + lote as colorante from t_pedidos_coloracion INNER JOIN t_cubetas_coloracion ON t_pedidos_coloracion.id_cubeta=t_cubetas_coloracion.id_cubeta INNER JOIN t_colorantes ON t_pedidos_coloracion.id_colorante=t_colorantes.id_colorante where id_orden=" & orden & " and id_pedido=" & idpedido
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function
    Public Sub GrabaLoteColorBypedido(ByVal p As clsPedido)
        Dim m As clsModelo = getClsModeloById(p.id_modelo)

        Dim cad As String = "select paso,id_cubeta_" & Replace(Replace(m.IndiceModelo, "0", ""), ",", "") & " as cubeta from t_pasos_coloracion where id_color=" & p.id_coloracion
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Dim tiempo As Integer
        Dim cmd As New SqlCommand("", mcon)
        mcon.Open()
        For Each r As DataRow In tb.Rows
            cmd.CommandText = "select isnull(tiempo_" & Replace(Replace(m.IndiceModelo, "0", ""), ",", "") & ",0) from t_tiempos_coloracion where paso=" & r("paso") & " and id_color=" & p.id_coloracion & " and intensidad=" & p.intensidad
            tiempo = cmd.ExecuteScalar
            If tiempo <> 0 Then
                cmd.CommandText = " INSERT INTO t_pedidos_coloracion select top 1 " & p.id & ",(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & p.id & ")," & r("paso") & "," & r("cubeta") & ",(select id_colorante from t_cubetas_coloracion where id_cubeta=" & r("cubeta") & "),lote from t_mto_cubetas where id_cubeta=" & r("cubeta") & " order by  entrada DESC"
                cmd.ExecuteNonQuery()
            End If
        Next
        mcon.Close()
    End Sub
    Public Sub GrabapasosColor(ByVal Pasos As ArrayList)
        Dim cad As String = ""
        Dim cmd As New SqlCommand("", mcon)
        mcon.Open()
        For Each p As clsPasoColor In Pasos
            cad = "DELETE FROM t_pasos_coloracion where id_color=" & p.Idcolor & " and paso=" & p.Paso & vbNewLine
            cad = cad & " DELETE FROM t_tiempos_coloracion where id_color=" & p.Idcolor & " and paso=" & p.Paso & vbNewLine
            cad = cad & " INSERT INTO t_pasos_coloracion (id_color,paso,id_cubeta_15,id_cubeta_16,id_cubeta_167) VALUES (" & p.Idcolor & "," & p.Paso & "," & p.Cubeta15 & "," & p.Cubeta16 & "," & p.Cubeta167 & ")" & vbNewLine
            For Each t As clsTiempoColor In p.tiempos
                cad = cad & "INSERT INTO t_tiempos_coloracion (id_color,paso,intensidad,tiempo_15,tiempo_16,tiempo_167) VALUES (" & p.Idcolor & "," & p.Paso & "," & t.Intensidad & "," & t.Tiempo15 & "," & t.Tiempo16 & "," & t.Tiempo167 & ")" & vbNewLine
            Next
            cmd.CommandText = cad
            cmd.ExecuteNonQuery()

        Next
        mcon.Close()
    End Sub
    Public Function GetListaTiempoColorBypaso(ByVal idColor As Integer, ByVal paso As Integer) As ArrayList
        Dim cad As String = "select intensidad,isnull((select tiempo_15   from t_tiempos_coloracion WHERE t_intensidades.intensidad=intensidad and id_color=" & idColor & " and paso=" & paso & " ),0) as tiempo15 ,isnull((select tiempo_16 from  t_tiempos_coloracion WHERE t_intensidades.intensidad=intensidad and id_color=" & idColor & " and paso=" & paso & " ),0) as tiempo16,isnull((select tiempo_167 from  t_tiempos_coloracion WHERE t_intensidades.intensidad=intensidad and id_color=" & idColor & " and paso=" & paso & " ),0) as tiempo167  from t_intensidades where t_intensidades.id_color=" & idColor & "  ORDER BY INTENSIDAD"

        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim lista As New ArrayList
        Dim tb As New DataTable
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows
            Dim t As New clsTiempoColor
            t.Intensidad = rw("intensidad")
            t.Tiempo15 = rw("tiempo15")
            t.Tiempo16 = rw("tiempo16")
            t.Tiempo167 = rw("tiempo167")
            lista.Add(t)
        Next
        Return lista
    End Function

    Public Function GettiemposColorBypaso(ByVal idColor As Integer, ByVal paso As Integer) As DataTable
        Dim cad As String = "select * from t_tiempos_coloracion where id_color=" & idColor & " and paso=" & paso & " ORDER BY INTENSIDAD"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetMenu(ByVal id As Integer) As clsMenu
        Dim cad As String = "select * from t_menus where id_menu=" & id
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Dim m As New clsMenu
        m.Id = tb.Rows(0)("id_menu")
        m.Menu = tb.Rows(0)("menu")
        m.Funcion = IIf(IsDBNull(tb.Rows(0)("funcion")), "", tb.Rows(0)("funcion"))
        Return m
    End Function

    Public Function GetmenusByidUsuario(ByVal id As Integer) As DataTable
        Dim cad As String = "select * from t_menus where  id_menu in (select id_menu from t_menus_usuario where id_usuario=" & id & ") order by orden"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function


    'TODO: Reemplazar llamadas para estandarizar
    Public Function GetmenusByUsuario() As DataTable
        Dim cad As String = "select * from t_menus where  id_menu in (select id_menu from t_menus_usuario where id_usuario=" & mUsuario.id & ") order by orden"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    'TODO: Reemplazar llamadas para estandarizar
    Public Function GetmenusByUsuario(ByVal idpadre As Integer) As DataTable
        Dim cad As String = "select * from t_menus where id_padre=" & idpadre & " and id_menu in (select id_menu from t_menus_usuario where id_usuario=" & mUsuario.id & ") order by orden"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    Public Function GetMenus(ByVal padre As Integer) As DataTable
        Dim cad As String = "select * from t_menus where id_padre=" & padre & " order by orden"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    Public Function GetImpresoras(ByVal equipo As String) As DataTable
        Dim campo As String = "(Select campo from t_impresoras where equipo=" & strsql(equipo) & " and id_documento=t_tipos_impresora.id_tipo) as campo"
        Dim impresora As String = Replace(campo, "campo", "impresora")
        Dim orientacion As String = Replace(campo, "campo", "orientacion")
        Dim papel As String = Replace(campo, "campo", "papel")
        Dim ancho As String = Replace(campo, "campo", "ancho")
        Dim alto As String = Replace(campo, "campo", "alto")
        Dim cad As String = "select id_tipo,tipo_impresora," & impresora & "," & papel & "," & orientacion & "," & ancho & "," & alto & " from t_tipos_impresora order by id_tipo"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub GrabaImpresoras(ByVal equipo As String, ByVal idDocumento As Integer, ByVal impresora As String, ByVal orientacion As Boolean, ByVal papel As String, ByVal alto As Integer, ByVal ancho As Integer)
        Dim Cad As String = "INSERT INTO t_impresoras (equipo,id_documento,impresora,orientacion,papel,alto,ancho) " & _
        " VALUES (" & strsql(equipo) & "," & idDocumento & "," & strsql(impresora) & "," & IIf(orientacion = False, 0, 1) & "," & _
        strsql(papel) & "," & NumSql(alto) & "," & NumSql(ancho) & ")"
        Dim cmd As New SqlCommand
        cmd.Connection = mcon
        cmd.CommandText = "DELETE FROM t_impresoras where equipo=" & strsql(equipo) & " and id_documento=" & idDocumento
        mcon.Open()
        cmd.ExecuteNonQuery()
        cmd.CommandText = Cad
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub

    Public Function Getespesor(ByVal gafa As Integer) As DataTable
        Dim cad As String = "select * from t_espesores_montura where id_montura=" & gafa
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetIdEspesor(ByVal modelo As Integer) As Integer
        Dim cad As String = "select id_espesor from t_modelos_espesor where id_modelo=" & modelo
        Dim mda As New SqlCommand(cad, mcon)
        mcon.Open()
        GetIdEspesor = mda.ExecuteScalar
        mcon.Close()
    End Function

    Public Function GetBaseEspesor(ByVal id As Integer, ByVal graduacion As Decimal) As DataTable
        Dim cad As String = "select * from t_base_espesor where id_espesor=" & id & " and desde<=" & NumSql(graduacion) & " and hasta>=" & NumSql(graduacion)
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb

    End Function

    Public Function GetBaseEspecialEspesor(ByVal id As Integer, ByVal Base As Decimal) As DataTable
        Dim cad As String = "select TOP 1 * from t_base_espesor where id_espesor=" & id & " and CURVA<=" & NumSql(Base) & " order by CURVA desc"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb

    End Function
    Public Sub GrabaFestivo(ByVal Fecha As Integer, ByVal Nombre As String)
        Dim Cad As String = "INSERT INTO t_festivos (fecha,festivo) VALUES (" & Fecha & "," & strsql(Nombre) & ")"
        Dim cmd As New SqlCommand(Cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub EliminaFestivo(ByVal Fecha As Integer)
        Dim Cad As String = "DELETE FROM t_festivos where fecha=" & Fecha
        Dim cmd As New SqlCommand(Cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetFestivosEntreFechas(ByVal Inicio As Integer, ByVal Fin As Integer) As DataTable
        Dim Cad As String = "select * from t_festivos where fecha>=" & Inicio & " and fecha<=" & Fin & " ORDER BY FECHA"
        Dim mda As New SqlDataAdapter(Cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetDiametroPlacaByEspesor(ByVal base As Decimal, ByVal diametro As Integer, ByVal MODELO As Integer) As Decimal
        Dim Cad As String = "select top 1 isnull(diametro,0) from t_semiterminados where " & Int(base) & "=SUBSTRING(CONVERT (VARCHAR(30),BASE),1,CHARINDEX('.',CONVERT (VARCHAR(30),BASE),0)-1)  AND ID_MODELO=" & MODELO & " and diametro>=" & diametro & " order by diametro"
        Dim cmd As New SqlCommand(Cad, mcon)
        mcon.Open()
        GetDiametroPlacaByEspesor = cmd.ExecuteScalar
        mcon.Close()
    End Function
    Public Function GetIndiceIdEspesor(ByVal id As Integer) As Single
        Dim cad As String = "select indice from t_espesores where id_espesor=" & id
        Dim mda As New SqlCommand(cad, mcon)
        mcon.Open()
        GetIndiceIdEspesor = mda.ExecuteScalar
        mcon.Close()
    End Function
    Public Sub ReenviarBiselado(ByVal montaje As Integer)
        Dim cad As String = "UPDATE t_biselados set enviado=0 where montaje=" & montaje
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetArchivoBiselado(ByVal montaje As Integer) As String
        Dim cad As String = "select archivo from  t_biselados where montaje=" & montaje
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim archivo As String = cmd.ExecuteScalar
        mcon.Close()
        Return archivo
    End Function

    Public Function GetrappelsparaAlbaran(ByVal desde As Integer, ByVal hasta As Integer, Optional ByVal Modo As Integer = 0, Optional ByVal tipo As Integer = 0) As DataTable

        Dim ClausulaModo As String = ""

        If Modo <> 0 Then
            ClausulaModo = " and id_modo=" & Modo
            If tipo <> 0 Then
                ClausulaModo = ClausulaModo & " and id_modelo in (select id_lente from t_modelos where tipologia=" & tipo & ")"
            End If
        End If
        Dim cad As String = " select t_clientes.id_cliente,codigo,nombre_comercial,sum(t_lineas_albaran.total) as facturacion,isnull((select top 1 rappel from t_rappels where modo=" & Modo & " and vision=" & tipo & " and importe<=sum(t_lineas_albaran.total) and id_rappel in (select id_rappel from t_rappels_cliente where id_cliente=t_clientes.id_cliente)  order by importe desc),'') as rappel," & _
        "isnull((select top 1 descuento from t_rappels where modo=" & Modo & " and vision=" & tipo & " and importe<=sum(t_lineas_albaran.total) and id_rappel in (select id_rappel from t_rappels_cliente where id_cliente=t_clientes.id_cliente) order by importe desc),0) as descuento from t_clientes INNER JOIN t_albaranes ON t_clientes.id_cliente=t_albaranes.id_cliente INNER JOIn t_lineas_albaran ON " & _
        " t_lineas_albaran.id_albaran=t_albaranes.id_albaran where t_clientes.rappel=1 and (id_tipo_producto=1 or id_tipo_producto=2) and fecha>=" & desde & " and fecha<=" & hasta & ClausulaModo & " group by t_clientes.id_cliente,codigo,nombre_comercial having sum(t_lineas_albaran.total)>=(select min(importe) from t_rappels where modo=" & Modo & " and vision=" & tipo & ")"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub CopiaPreciosEspecialesCli(ByVal Clienteorigen As Integer, ByVal CLientedestino As Integer)
        Dim cad As String = " INSERT INTO t_precios_cliente select " & CLientedestino & ",id_grupo,id_modo,id_tratamiento,diametro,cilindro,precio,id_color,desde,hasta from t_precios_cliente where id_cliente=" & Clienteorigen
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub

    Public Function GetrappelsparaAlbaranAsociados(ByVal desde As Integer, ByVal hasta As Integer, Optional ByVal Modo As Integer = 0, Optional ByVal Tipo As Integer = 0) As DataTable

        Dim ClausulaModo As String = ""

        If Modo <> 0 Then
            ClausulaModo = " and id_modo=" & Modo
            If Tipo <> 0 Then
                ClausulaModo = ClausulaModo & " and id_modelo in (select id_lente from t_modelos where tipologia=" & Tipo & ")"
            End If
        End If
        Dim AlbaranesAsociados As String = "(select sum(total) from t_lineas_albaran where (id_tipo_producto=1 or id_tipo_producto=2) and id_albaran in (select id_albaran from t_albaranes where fecha>=" & desde & " and fecha<=" & hasta & " and id_cliente in (select id_cliente_asociado from t_clientes_asociados where id_cliente=t_clientes.id_cliente)) " & ClausulaModo & ")"

        Dim cad As String = " (select t_clientes.id_cliente,codigo,nombre_comercial,sum(t_lineas_albaran.total) as facturacion,isnull((select top 1 rappel from t_rappels where modo=" & Modo & " and vision=" & Tipo & " and id_rappel in (select id_rappel from t_rappels_cliente where id_cliente=t_clientes.id_cliente) and importe<=sum(t_lineas_albaran.total) order by importe desc),'') as rappel," & _
"isnull((select top 1 descuento from t_rappels where modo=" & Modo & " and vision=" & Tipo & " and importe<=sum(t_lineas_albaran.total) and id_rappel in (select id_rappel from t_rappels_cliente where id_cliente=t_clientes.id_cliente)  order by importe desc),0) as descuento from t_clientes INNER JOIN t_albaranes ON t_clientes.id_cliente=t_albaranes.id_cliente INNER JOIn t_lineas_albaran ON " & _
" t_lineas_albaran.id_albaran=t_albaranes.id_albaran where t_clientes.rappel=1 and (id_tipo_producto=1 or id_tipo_producto=2) and t_clientes.id_cliente not in (select id_cliente_asociado from t_clientes_asociados) and fecha>=" & desde & " and fecha<=" & hasta & ClausulaModo & " group by t_clientes.id_cliente,codigo,nombre_comercial having sum(t_lineas_albaran.total)>=(select min(importe) from t_rappels where modo=" & Modo & " and vision=" & Tipo & "))"
        Dim Cad2 As String = "(select t_clientes.id_cliente,codigo,nombre_comercial," & AlbaranesAsociados & " as facturacion,isnull((select top 1 rappel from t_rappels where modo=" & Modo & " and vision=" & Tipo & " and importe<=" & AlbaranesAsociados & " and id_rappel in (select id_rappel from t_rappels_cliente where id_cliente=t_clientes.id_cliente)  order by importe desc),'') as rappel," & _
"isnull((select top 1 descuento from t_rappels where modo=" & Modo & " and vision=" & Tipo & "  and importe<=" & AlbaranesAsociados & " and id_rappel in (select id_rappel from t_rappels_cliente where id_cliente=t_clientes.id_cliente)  order by importe desc),0)  as descuento from t_clientes where rappel=1 and id_cliente in (select id_cliente from t_clientes_asociados) group by id_cliente,codigo,nombre_comercial having " & AlbaranesAsociados & ">=(select min(importe) from t_rappels where modo=" & Modo & " and vision=" & Tipo & "))"

        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        Dim tb2 As New DataTable
        mda.SelectCommand.CommandText = Cad2
        mda.Fill(tb2)
        For Each rw As DataRow In tb2.Rows
            Dim r As DataRow = tb.NewRow()
            For i As Integer = 0 To tb.Columns.Count - 1
                r.Item(i) = rw.Item(i)
            Next
            tb.Rows.Add(r)
        Next
        mcon.Close()
        Return tb
    End Function

    Public Function GetDiseñadores() As DataTable
        Dim cad As String = "select * from t_diseñadores order by id_diseñador"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    Public Function GetDiseñadorByid(ByVal id As Integer) As String
        Dim cad As String = "select diseñador from t_diseñadores where id_diseñador=" & id
        Dim mda As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim str As String = mda.ExecuteScalar
        mcon.Close()
        Return str
    End Function

    Public Function GrabaRappel(ByVal id As Integer, ByVal rappel As String, ByVal tipolente As Integer, ByVal importe As Decimal, ByVal descuento As Decimal, ByVal vision As Integer) As Integer
        Dim cad As String
        If id = 0 Then ' se trata de un nuevo rappel por lo que vamos a añadirlo
            id = getMaxId("id_rappel", "t_rappels") + 1
            cad = "INSERT INTO t_rappels (id_rappel,rappel,modo,importe,descuento,vision) VALUES (" & id & "," & strsql(rappel) & "," & tipolente & "," & NumSql(importe) & "," & NumSql(descuento) & "," & vision & ")"
        Else
            cad = "UPDATE t_rappels SET Rappel=" & strsql(rappel) & ",descuento=" & NumSql(descuento) & ",vision=" & vision & " where id_rappel=" & id
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        Return id
    End Function

    Public Sub EliminaRappel(ByVal id As Integer)
        Dim cad As String = "DELETE FROM t_rappels where id_rappel=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function GetRappels() As DataTable
        Dim cad As String = "select t_rappels.*, case modo when 0 then 'Todas' when 1 then 'Stock' when 2 then 'Transformacion' when 3 then 'Fabricación' END as tipo, case vision when 0 then 'Todas' when '1' then 'Monofocal' when 2 then 'Bifocal'  when 3 then 'Progresivo' END as tipo_vision from t_rappels order by id_rappel"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub GrabaRappelsCliente(ByVal idcli As Integer, ByVal rappels As ArrayList)
        Dim cad As String = "Delete from t_rappels_cliente where id_cliente=" & idcli
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        For Each i As Integer In rappels
            cmd.CommandText = "Insert into t_rappels_cliente (id_rappel,id_cliente) VALUES (" & i & "," & idcli & ")"
            cmd.ExecuteNonQuery()
        Next
        mcon.Close()
    End Sub

    Public Function GetRappelsByCliente(ByVal id As Integer) As DataTable
        Dim cad As String = "select t_rappels.*, case modo when 0 then 'Todas' when 1 then 'Stock' when 2 then 'Transformacion' when 3 then 'Fabricación' END as tipo, case vision when 0 then 'Todas' when '1' then 'Monofocal' when 2 then 'Bifocal'  when 3 then 'Progresivo' END as tipo_vision,isnull((select id_rappel from t_rappels_cliente where id_cliente=" & id & " and id_rappel=t_rappels.id_rappel),0) as cliente from t_rappels order by id_rappel"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function ReferenciaBiselado(ByVal idped As Integer) As String
        Dim cad As String = "select referencia from t_biselados where id_pedido_d=" & idped & " or id_pedido_i=" & idped
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim ref As String = cmd.ExecuteScalar()
        mcon.Close()
        Return ref
    End Function

    Public Function EsDegradado(ByVal idcolor As Integer) As Boolean
        Dim cad As String = "select id_gama from t_coloraciones where id_coloracion=" & idcolor
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim gama As Integer = cmd.ExecuteScalar
        mcon.Close()
        If gama = 3 Or gama = 4 Or gama = 6 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub SalidaAlmacenDesecho(ByVal des As clsDesecho)
        Dim cad As String = "Update t_almacen_desechos set cantidad=cantidad-1 where id_desecho=" & des.Id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Sub ActualizaAlmacenStock(ByVal stock As Integer, ByVal minimo As Integer, ByVal critico As Integer, ByVal idlente As Integer)
        Dim cad As String = "UPDATE t_lentes_stock set stock=" & stock & ",stock_minimo=" & minimo & ",stock_critico=" & critico & " where id_producto=" & idlente
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetCodigoUDIbyPedido(ByVal ped As clsPedido) As String
        Dim cad As String = "select codigo_GCP+cod_udi+dig_control from t_valores_globales, t_udi where id_modelo=" & ped.id_modelo
        Dim o As New clsOrdenesTrabajo
        o = GetOrdenesTrabajo(ped.id)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()

        ' MsgBox(Char.ConvertFromUtf32(232))
        GetCodigoUDIbyPedido = "(01)0" & cmd.ExecuteScalar & "(11)" & o.Item(o.Count - 1).FsCalidad & "(21)" & ped.id
        mcon.Close()
        'GetCodigoUDIbyPedido &= GetLoteSemiterminadoByPedido(ped)

    End Function
    Public Sub New()
          con2 = "data source=212.227.147.192;initial catalog=LOA;user id=usrweb;password=usrweb2023;packet size=4096"
          mcon = New SqlConnection(con2)
    End Sub

    Public Function GetPedidosSinLoteAntireflejoByFecha() As DataTable
        Dim cad As String = "SELECT t_pedidos.*,id_orden,(select nombre from t_modelos where id_lente=id_modelo) as modelo FROM T_PEDIDOS INNER JOIN t_ordenes_trabajo ON t_ordenes_trabajo.id_pedido=t_pedidos.id_pedido WHERE FE_ANTIREFLEJO=" & FechaAcadena(Now.Date) & "  and id_incidencia=0 AND t_pedidos.ID_PEDIDO NOT IN (SELECT ID_PEDIDO FROM T_LOTE_ANTIREFLEJO where id_pedido=t_pedidos.id_pedido and id_orden=t_ordenes_trabajo.id_orden)"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    Public Function GetPedidosSinLoteEndurecidoByFecha() As DataTable
        Dim cad As String = "SELECT t_pedidos.*,id_orden,(select nombre from t_modelos where id_lente=id_modelo) as modelo FROM T_PEDIDOS INNER JOIN t_ordenes_trabajo ON t_ordenes_trabajo.id_pedido=t_pedidos.id_pedido WHERE FE_ENDURECIMIENTO=" & FechaAcadena(Now.Date) & "  and id_incidencia=0 AND t_pedidos.ID_PEDIDO NOT IN (SELECT ID_PEDIDO FROM T_LOTE_endurecido where id_pedido=t_pedidos.id_pedido and id_orden=t_ordenes_trabajo.id_orden)"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    Public Sub EliminaLineaMuestrarioColor(ByVal muestrario As Integer, ByVal modelo As Integer, ByVal modo As String, ByVal tratamiento As Integer, ByVal color As Integer, ByVal intensidad As Integer)
        Dim cad As String = "delete from t_lineas_muestrario_color where id_muestrario=" & muestrario & " and id_modelo=" & modelo & " and modo=" & strsql(modo) & " and id_tratamiento=" & tratamiento & _
        " and id_color=" & color & " and intensidad=" & intensidad
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
    End Sub

    Public Sub EliminaMontaje(ByVal m As clsMontaje)
        Dim cad As String = "delete from t_pedidos_montajes where id_pedido_montaje=" & m.Id & vbNewLine & _
        "delete from t_lineas_pedido_montaje where id_pedido_montaje=" & m.Id & vbNewLine & _
        "DELETE FROM t_biselados where montaje=" & m.Id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'ahora vamos a ver si eliminamos el montaje de las lentes
        If m.Derecho <> 0 Or m.Izquierdo <> 0 Then
            If MsgBox("¿Desea quitarle el montaje a las lentes?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                cmd.CommandText = "Update t_pedidos set montaje=0 where id_pedido in (" & m.Derecho & "," & m.Izquierdo & ")"
                cmd.ExecuteNonQuery()
            End If
        End If
        MsgBox("Se ha eliminado el montaje correctamente, recuerde avisar al taller de montaje y ver en que situacion estan las lentes para avisar al departamento donde se encuentren")
        mcon.Close()

    End Sub

    Public Sub MarcaLentesSinMontaje(ByVal derecho As Integer, ByVal izquierdo As Integer)
        Dim cad As String = "Update t_pedidos set montaje=0 where id_pedido in (" & derecho & "," & izquierdo & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Sub GrabaCostes(ByVal costo As clsCosteslente)
        With costo
            Dim cad As String = "update t_costes_lente set fabrica_min=" & NumSql(.FabricaMineral) & ",fabrica_org=" & NumSql(.FabricaOrganico) & _
            ",surend=" & NumSql(.Surend) & ",surlight=" & NumSql(.SurLight) & ",Endurecido_HI=" & NumSql(.EndurecidoHI) & _
            ",toplight=" & NumSql(.Toplight) & ", general = " & NumSql(.General) & ", color = " & NumSql(.Color) & ",sobre=" & NumSql(.Sobre) & ",pegatina_ARTop=" & NumSql(.PegatinaARTop)
            Dim cmd As New SqlCommand(cad, mcon)
            mcon.Open()
            cmd.ExecuteNonQuery()
            mcon.Close()
        End With
    End Sub

    Public Function UltimoSemiterminadoByPedido(ByVal idpedido As Integer, ByVal orden As Integer) As Integer
        Dim cad As String = "select top 1 isnull( id_lente,0) from t_salidas_semiterminados where id_pedido=" & idpedido & " and id_orden<" & orden & " order by id_orden desc"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim ultimo As Integer = cmd.ExecuteScalar
        mcon.Close()
        Return ultimo
    End Function

    Public Function GetNombreModeloByID(ByVal id As Integer) As String
        Dim cad As String = "select nombre from t_modelos where id_lente=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim nombre As String = cmd.ExecuteScalar
        mcon.Close()
        Return nombre
    End Function

    Public Sub AlertaLoteSemiterminadoEnterado(ByVal idretirada As Integer)
        Dim cad As String = "INSERT INTO t_Lineas_retirada_lote_semiterminado (id_retirada,id_usuario) VALUES (" & idretirada & "," & mUsuario.id & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function AlertaLoteSemiterminado() As Boolean
        Dim cad As String = "select count(*) from t_retirada_lote_semiterminado where id_retirada not in (select id_retirada from t_lineas_retirada_lote_semiterminado where id_usuario=" & mUsuario.id & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim existen As Boolean = cmd.ExecuteScalar
        mcon.Close()
        Return existen
    End Function

    Public Function GetlotesRetiradosByUsuario() As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select *, (select cantidad-salida from t_lotes_semiterminado where lote=t_retirada_lote_semiterminado.lote and id_semiterminado=t_retirada_lote_semiterminado.id_semiterminado) as almacen from  t_retirada_lote_semiterminado where id_retirada not in (select id_retirada from t_lineas_retirada_lote_semiterminado where id_usuario=" & mUsuario.id & ")"
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)

        Return tb
    End Function

    Public Function GetPedidosRetiradosProcesados(ByVal idRetirada As Integer) As DataTable
        ' Select id_pedido,nombre_comercial from t_pedidos where id_albaran<>0 and id_pedido in (select top
        'vemos que pedido ha salido
        Dim cad As String = "select distinct t_pedidos.id_pedido,nombre_comercial,id_albaran,referencia,id_lente,lote from t_pedidos INNER JOIN t_clientes On t_pedidos.id_cliente=t_clientes.id_cliente INNER JOIN t_salidas_semiterminados ON t_pedidos.id_pedido=t_salidas_semiterminados.id_pedido where id_lente =(select id_lente from t_retirada_lote_semiterminado where id_retirada=1) and lote=(select lote from t_retirada_lote_semiterminado where id_retirada=" & idRetirada & ") and id_albaran<>0 order by t_pedidos.id_pedido"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows
            Dim tb1 As New DataTable
            tb1 = GetUltimoLoteByPedido(rw("id_pedido"))
            If tb.Rows.Count = 1 And (rw("lote") <> tb1.Rows(0)("lote") Or rw("id_lente") = tb1.Rows(0)("id_lente")) Then
                tb.Rows.Remove(rw)
            End If
        Next
        Return tb
    End Function

    Public Function GetPedidosRetiradosEnProceso(ByVal idRetirada As Integer) As DataTable
        ' Select id_pedido,nombre_comercial from t_pedidos where id_albaran<>0 and id_pedido in (select top
        'vemos que pedido ha salido
        Dim cad As String = "select distinct t_pedidos.id_pedido,nombre_comercial,id_albaran,referencia,id_lente,lote from t_pedidos INNER JOIN t_clientes On t_pedidos.id_cliente=t_clientes.id_cliente INNER JOIN t_salidas_semiterminados ON t_pedidos.id_pedido=t_salidas_semiterminados.id_pedido where id_lente =(select id_lente from t_retirada_lote_semiterminado where id_retirada=1) and lote=(select lote from t_retirada_lote_semiterminado where id_retirada=" & idRetirada & ") and id_albaran=0 order by t_pedidos.id_pedido"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows
            Dim tb1 As New DataTable
            tb1 = GetUltimoLoteByPedido(rw("id_pedido"))
            If tb.Rows.Count = 1 And (rw("lote") <> tb1.Rows(0)("lote") Or rw("id_lente") = tb1.Rows(0)("id_lente")) Then
                tb.Rows.Remove(rw)
            End If
        Next
        Return tb
    End Function

    Public Function GetUltimoPasoBypedido(ByVal idPedido As Integer) As String
        Dim cad As String = "select top 1 * from t_ordenes_trabajo where id_pedido=" & idPedido & " order by id_orden desc"
        Dim tb As New DataTable
        Dim paso As String = "Inicial"
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        If tb.Rows.Count = 1 Then

            If tb.Rows(0)("FS_ALMACEN") <> 0 Then
                paso = "Salida Almacen"
            End If
            If tb.Rows(0)("Fe_fabrica") <> 0 Then
                paso = "Entrada Fabrica"
            End If
            If tb.Rows(0)("Fs_fabrica") <> 0 Then
                paso = "Salida Fabrica"
            End If
            If tb.Rows(0)("Fe_coloracion") <> 0 Then
                paso = "Entrada Color"
            End If
            If tb.Rows(0)("Fs_coloracion") <> 0 Then
                paso = "Salida Color"
            End If
            If tb.Rows(0)("Fe_endurecimiento") <> 0 Then
                paso = "Entrada Endurecido"
            End If
            If tb.Rows(0)("Fs_endurecimiento") <> 0 Then
                paso = "Salida Endurecido"
            End If
            If tb.Rows(0)("Fe_antireflejo") <> 0 Then
                paso = "Entrada antireflejo"
            End If
            If tb.Rows(0)("Fs_antireflejo") <> 0 Then
                paso = "Salida antireflejo"
            End If
            If tb.Rows(0)("Fs_calidad") <> 0 Then
                paso = "Salida calidad"
            End If

            If tb.Rows(0)("Fe_montaje") <> 0 Then
                paso = "Entrada Montaje"
            End If
            If tb.Rows(0)("Fs_antireflejo") <> 0 Then
                paso = "Salida Montaje"
            End If
        End If
        Return paso
    End Function

    Public Function GetUltimoLoteByPedido(ByVal idPedido As String) As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select top 1 id_lente,lote from t_salidas_semiterminados where id_pedido=" & idPedido & " order by id_orden desc"
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function

    Public Function LoteRetirado(ByVal Lente As Integer, ByVal lote As String) As Boolean
        Dim cad As String = "select count(*) from t_lotes_semiterminado where id_semiterminado=" & Lente & " and lote like " & strsql(lote) & " AND RETIRADO=1"

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim Retirado As Boolean = cmd.ExecuteScalar
        mcon.Close()
        Return Retirado
    End Function

    Public Function LoteRetiradobYPEDIDO(ByVal pedido As Integer) As Boolean
        Dim cad As String = "select count(*) from t_lotes_semiterminado where id_semiterminado=(select top 1 id_lente from t_salidas_semiterminados where id_pedido=" & pedido & " order by id_orden) and lote like (select top 1 lote from t_salidas_semiterminados where id_pedido=" & pedido & " order by id_orden) and retirado=1"

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim Retirado As Boolean = cmd.ExecuteScalar
        mcon.Close()
        Return Retirado
    End Function


    Public Function GetLotes(Optional ByVal Semiterminado As Integer = 0, Optional ByVal Lote As String = "", Optional ByVal SinRetirados As Boolean = False) As DataTable
        Dim Condiciones As String = ""
        If Semiterminado <> 0 Then
            Condiciones = " where id_semiterminado=" & Semiterminado
        End If
        If Lote <> "" Then
            If Condiciones.Length <> 0 Then
                Condiciones = Condiciones & " and lote like " & strsql(Lote)
            Else
                Condiciones = " where lote like " & strsql(Lote)
            End If

        End If
        If SinRetirados = True Then
            If Condiciones.Length <> 0 Then
                Condiciones = Condiciones & " and retirado=0"
            Else
                Condiciones = " where retirado=0"
            End If
        End If
        Dim cad As String = "select *,(SELECT INDICE_MODELO FROM T_MODELOS WHERE ID_LENTE=t_semiterminados.ID_MODELO) AS INDICE from t_lotes_semiterminado INNER JOIN t_semiterminados ON t_lotes_semiterminado.id_semiterminado=t_semiterminados.id_lente " & Condiciones
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb

    End Function

    Public Function GetCostes() As DataTable
        Dim cad As String = "select * from t_costes_lente"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        If mcon.State = ConnectionState.Closed Then
            mcon.Open()
        End If
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetCostesDetalladoByPedido(ByVal id As Integer) As DataTable
        Dim cad As String = "select * from t_costos_pedido where id_pedido=" & id
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetFronto(ByVal idpedido As Integer) As DataTable
        Dim cad As String = "Select * from t_pedidos_fronto where id_pedido=" & idpedido
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetIntensidades(ByVal idColor As Integer) As DataTable
        Dim cad As String = "select  intensidad from t_intensidades  where id_color=" & idColor & " order by intensidad"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function getcodCuentaReByIdBase(ByVal idBase As Integer, ByVal recargo As Boolean) As String
        Dim cad As String = "select cuenta_re as codContable from t_tipos_iva where id_iva =(select id_iva from t_tipos_base where id_tipo_base=" & idBase & ")"
        Dim CodContable As String
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        CodContable = cmd.ExecuteScalar
        mcon.Close()
        Return CodContable
    End Function

    Public Function getcodCuentaIvaByIdBase(ByVal idBase As Integer, ByVal coniva As Boolean, ByVal recargo As Boolean) As String

        Dim Siniva As String = ""
        If coniva = True And recargo = True Then
            Siniva = "re"
        End If
        Dim cad As String = "select cuenta_iva" & Siniva & " as codContable from t_tipos_iva where id_iva =(select id_iva from t_tipos_base where id_tipo_base=" & idBase & ")"
        Dim CodContable As String
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        CodContable = cmd.ExecuteScalar
        mcon.Close()
        Return CodContable
    End Function

    Public Function GetFacturaContabilidad(ByVal Fecini As String, ByVal Fecfin As String) As DataTable
        Dim cad As String = "select id_factura as id,fecha,serie,id_cliente,num_factura,(select sum(convert(decimal(8,2),base*(1+iva/100))+convert(decimal(8,2),base*re/100)) from t_bases_factura where id_factura=t_facturas.id_factura) as importe from t_facturas where fecha>=" & Fecini & " and fecha<=" & Fecfin & " order by fecha,num_factura "
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetClienteBycodContaplus(ByVal codigo As String) As Integer
        Dim cad As String = "select isnull(id_cliente,0) from t_clientes where codigocp=" & strsql(codigo)

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim id As Integer = cmd.ExecuteScalar
        mcon.Close()
        Return id
    End Function

    Public Function GetFactura(ByVal idalbaran As Integer) As String
        Dim cad As String = "select left(fecha,4)+'/'+serie + '-' + convert(varchar,num_factura) from t_facturas where id_factura in (select id_factura from t_lineas_factura where id_albaran=" & idalbaran & ")"

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim resultado As String = cmd.ExecuteScalar
        mcon.Close()
        Return resultado
    End Function

    Public Function CargaPedidosWeb() As clsPedidos
        Dim Peds As New clsPedidos
        Dim Ped As New clsPedido
        Dim tb As New DataTable
        Dim cad As String = "select * from t_pedidos_web"
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Dim i As Integer
        For i = 0 To tb.Rows.Count - 1
            Ped = GetPedidobyId(tb.Rows(i)("id_pedido"))
            Peds.add(Ped)
        Next
        Return Peds

    End Function
    Public Function GetPedidosByMuestrarioColor(ByVal IdMontaje As Integer) As clsPedidos
        Dim cad As String = "select id_modelo,modo,id_tratamiento,id_color,intensidad,diametro,id_cliente from t_muestrarios_color INNER JOIN t_lineas_muestrario_color ON t_muestrarios_color.id_muestrario=t_lineas_muestrario_color.id_muestrario where id_montaje=" & IdMontaje
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Dim peds As New clsPedidos
        Dim idpedido As Integer = 0
        For Each rw As DataRow In tb.Rows
            idpedido += 1
            Dim p As New clsPedido
            p.id = idpedido
            p.id_usuario = mUsuario.id
            p.SinCargo = True
            p.LlevaMontaje = True
            p.id_modelo = rw("id_modelo")
            p.id_cliente = rw("id_cliente")
            p.id_coloracion = rw("id_color")
            p.coloracion = GetNombreColorById(p.id_coloracion)

            p.id_tratamiento = rw("id_tratamiento")
            p.tratamiento = getTratamientoById(p.id_tratamiento)
            p.modelo = GetNombreModeloByID(p.id_modelo)
            p.modo = rw("modo")
            p.intensidad = rw("intensidad")
            p.diametro = rw("diametro")
            p.Observaciones = "Muestrario Color Nº:"
            p.Compensador = False
            If idpedido Mod 2 = 0 Then
                p.pareja = idpedido - 1
                peds.Item(peds.Count - 1).pareja = idpedido
                p.ojo = "I"
            Else
                p.ojo = "D"
            End If
            Dim rn As New clsReglasNegocio
            rn.FechaCompromiso(p)
            peds.add(p)
        Next
        Return peds
    End Function
    Public Function CambiosCubetasColor(ByVal Inicio As String, ByVal Fin As String) As DataTable
        Dim cad As String = "Declare @inicio as integer" & vbNewLine & _
                        "declare @fin as integer " & vbNewLine & _
                        "set @inicio=dbo.fechatotimestamp('" & Inicio & "') " & vbNewLine & _
                        "set @fin=dbo.fechatotimestamp('" & Fin & " 23:59:59')" & vbNewLine & _
                        "select cubeta,colorante,count(*) as cambios from t_mto_cubetas INNER JOIN t_cubetas_coloracion ON t_cubetas_coloracion.id_cubeta=t_mto_cubetas.id_cubeta INNER JOIN t_colorantes ON t_colorantes.id_colorante=t_cubetas_coloracion.id_colorante where entrada>=@inicio and entrada<=@fin group by cubeta,colorante"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Sub ImprimePedidoWeb(ByVal p As clsPedido)
        Dim cad As String = "DELETE FROM t_pedidos_web where id_pedido=" & p.id & " or id_pedido=" & p.pareja
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Select isnull(id_usuario,0) from t_impresion_web where id_pedido=" & p.id
        Dim impreso As Boolean = cmd.ExecuteScalar
        If impreso = True Then
            MsgBox("El pedido " & p.id & " ya se ha mandado a impresion por otro usuario")
            Exit Sub
        End If
        cmd.CommandText = "INSERT INTO t_impresion_web (id_pedido,id_usuario) VALUES (" & p.id & "," & mUsuario.id & ")"
        cmd.ExecuteNonQuery()
        If p.pareja <> 0 Then
            cmd.CommandText = "INSERT INTO t_impresion_web (id_pedido,id_usuario) VALUES (" & p.pareja & "," & mUsuario.id & ")"
            cmd.ExecuteNonQuery()
        End If
        mcon.Close()
        Dim imp As New clsImpresion
        imp.ImprimePedido(p, False)
        'ahora vamos a insertar el usuario que lo haya mandado imprimir

    End Sub

    Public Function GetPreciosProveedores() As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select  Proveedor,t_precios_proveedores.*,t_modelos.nombre as modelo from (t_precios_proveedores INNER JOIN t_modelos On t_modelos.id_lente=t_precios_proveedores.id_modelo)" & _
        " INNER JOIN Proveedores.dbo.t_proveedores ON Proveedores.dbo.t_proveedores.id_proveedor=t_precios_proveedores.id_proveedor order by Proveedores.dbo.t_proveedores.id_proveedor,nombre"
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub DeletePreciosProveedores(ByVal idProveedor As Integer, ByVal idModelo As Integer, ByVal Base As Decimal, ByVal diametro As Integer, ByVal precio As Single)

        Dim cad As String = "delete t_precios_proveedores where id_proveedor=" & idProveedor & " and id_modelo=" & idModelo & " and base=" & NumSql(Base) & " and diametro=" & diametro
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub

    Public Function GetPrecioSemiterminado(ByVal codbarra As String) As Decimal
        Dim cad As String = "select * from t_cod_barras_semiterminado where cod_barra=" & strsql(codbarra)
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        Dim precio As Decimal = 0
        mda.Fill(tb)
        If tb.Rows.Count > 0 Then
            'vamos a ver cual es la lente y cual es el proveedor
            Dim Lente As Integer = 0
            Dim Proveedor As Integer = 0
            Lente = tb.Rows(0)("id_lente")
            Proveedor = tb.Rows(0)("id_proveedor")
            'ahora vamos a buscar el precio
            'vamos a ver si existe un precio para ese modelo, diametro y base return 0
            cad = "select isnull(precio,0) from t_precios_proveedores where id_proveedor=" & Proveedor & " and id_modelo=(select id_modelo from t_semiterminados where id_lente=" & Lente & ") and diametro=(select diametro from t_semiterminados where id_lente=" & Lente & ") and base=(select base from t_semiterminados where id_lente=" & Lente & ")"

            Dim cmd As New SqlCommand(cad, mcon)
            mcon.Open()
            precio = cmd.ExecuteScalar
            If precio = 0 Then
                'buscamos para ese modelo y base
                cad = "select isnull(precio,0) from t_precios_proveedores where id_proveedor=" & Proveedor & " and id_modelo=(select id_modelo from t_semiterminados where id_lente=" & Lente & ") and diametro=-1 and base=(select base from t_semiterminados where id_lente=" & Lente & ")"
                cmd.CommandText = cad
                precio = cmd.ExecuteScalar()
            End If
            If precio = 0 Then
                'buscamos para ese modelo y diametro
                cad = "select isnull(precio,0) from t_precios_proveedores where id_proveedor=" & Proveedor & " and id_modelo=(select id_modelo from t_semiterminados where id_lente=" & Lente & ") and base=-1 and diametro=(select diametro from t_semiterminados where id_lente=" & Lente & ")"
                cmd.CommandText = cad
                precio = cmd.ExecuteScalar()
            End If
            If precio = 0 Then
                'buscamos para ese modelo cualquier base y cualquier diametro
                cad = "select isnull(precio,0) from t_precios_proveedores where id_proveedor=" & Proveedor & " and id_modelo=(select id_modelo from t_semiterminados where id_lente=" & Lente & ") and base=-1 and diametro=-1"
                cmd.CommandText = cad
                precio = cmd.ExecuteScalar()
            End If
            mcon.Close()
        End If


        Return precio
    End Function
    Public Function GetPrecioSemiterminado(ByVal Lente As Integer, ByVal proveedor As Integer) As Decimal

        'ahora vamos a buscar el precio
        'vamos a ver si existe un precio para ese modelo, diametro y base return 0
        Dim Cad As String = "select isnull(precio,0) from t_precios_proveedores where id_proveedor=" & proveedor & " and id_modelo=(select id_modelo from t_semiterminados where id_lente=" & Lente & ") and diametro=(select diametro from t_semiterminados where id_lente=" & Lente & ") and base=(select base from t_semiterminados where id_lente=" & Lente & ")"
        Dim precio As Decimal = 0
        Dim cmd As New SqlCommand(Cad, mcon)
        mcon.Open()
        precio = cmd.ExecuteScalar
        If precio = 0 Then
            'buscamos para ese modelo y base
            Cad = "select isnull(precio,0) from t_precios_proveedores where id_proveedor=" & proveedor & " and id_modelo=(select id_modelo from t_semiterminados where id_lente=" & Lente & ") and diametro=-1 and base=(select base from t_semiterminados where id_lente=" & Lente & ")"
            cmd.CommandText = Cad
            precio = cmd.ExecuteScalar()
        End If
        If precio = 0 Then
            'buscamos para ese modelo y diametro
            Cad = "select isnull(precio,0) from t_precios_proveedores where id_proveedor=" & proveedor & " and id_modelo=(select id_modelo from t_semiterminados where id_lente=" & Lente & ") and base=-1 and diametro=(select diametro from t_semiterminados where id_lente=" & Lente & ")"
            cmd.CommandText = Cad
            precio = cmd.ExecuteScalar()
        End If
        If precio = 0 Then
            'buscamos para ese modelo cualquier base y cualquier diametro
            Cad = "select isnull(precio,0) from t_precios_proveedores where id_proveedor=" & proveedor & " and id_modelo=(select id_modelo from t_semiterminados where id_lente=" & Lente & ") and base=-1 and diametro=-1"
            cmd.CommandText = Cad
            precio = cmd.ExecuteScalar()
        End If
        mcon.Close()
        Return precio
    End Function
    Public Function GetFechaCalculo(ByVal pedido As Integer, ByVal orden As Integer) As DataTable
        Dim cad As String = "Select dbo.timestamptofecha(entrada) as fecha from t_pedidos_calculados where id_pedido=" & pedido & " and orden=" & orden & " order by entrada"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function
    Public Sub GrabaCostePedido(ByVal idpedido As Integer, ByVal concepto As String, ByVal precio As Decimal)
        Dim cad As String = "INSERT INTO t_costos_pedido (id_pedido,paso,coste) VALUES (" & idpedido & "," & strsql(concepto) & "," & NumSql(precio) & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function GetCostePedido(ByVal id As Integer) As Decimal
        Dim coste As Decimal = 0
        Dim cad As String = "select isnull(sum(coste),0) from t_costos_pedido where id_pedido=" & id

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        coste = cmd.ExecuteScalar
        mcon.Close()
        Return coste
    End Function

    Public Sub GrabaPreciosProveedores(ByVal idProveedor As Integer, ByVal idModelo As Integer, ByVal base As Decimal, ByVal diametro As Integer, ByVal precio As Single)

        Dim cad As String = "DELETE FROM T_precios_proveedores where id_proveedor=" & idProveedor & " and id_modelo=" & idModelo & " and base=" & NumSql(base) & " and diametro=" & diametro & vbNewLine & "INSERT INTO t_precios_proveedores VALUES (" & idProveedor & "," & idModelo & "," & diametro & "," & Replace(precio, ",", ".") & "," & NumSql(base) & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub

    Public Sub GrabaPreciosStockProveedores(ByVal precios As clsPreciosProveedorStock)

        Dim cad As String = "DELETE FROM T_precios_stock_proveedor"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        For Each pre As clsPrecioProvStock In precios
            cmd.CommandText = "INSERT INTO t_precios_stock_proveedor (id_modelo,id_tratamiento,id_graduacion,diametro,precio) VALUES (" & pre.idModelo & "," & pre.idTratamiento & "," & pre.IdGraduacion & "," & pre.Diametro & "," & NumSql(pre.Precio) & ")"
            cmd.ExecuteNonQuery()
        Next
        mcon.Close()

    End Sub

    Public Sub SetStockMinimoCriticoSemiterminados(ByVal modelos As String, ByVal idProv As Integer, ByVal DiasMinimo As Integer, ByVal DiasCritico As Integer, ByVal Minimo As Integer)

        Dim dias As Integer = 0, Proveedor As String = "", fecha As String = ""
        fecha = FechaAcadena(DateAdd(DateInterval.Year, -1, Now.Date))
        Dim cad As String = "select count(distinct(fecha)) from t_salidas_semiterminados where fecha>=" & fecha
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        dias = cmd.ExecuteScalar
        If idProv = 0 Then
            Proveedor = ""
        Else
            Proveedor = " and id_lente in ( select id_lente from t_cod_barras_semiterminado where id_proveedor=" & idProv & ")"

        End If
        cad = "select id_lente,(select count(*) from t_salidas_semiterminados where fecha>=" & fecha & " and id_lente=t_semiterminados.id_lente) as Gastos from t_semiterminados where minimo<>0 and id_modelo in " & _
        modelos & Proveedor
        'ahora cargamos las lentes en un datatable
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Dim i As Integer
        Dim StockMinimo As Single = 0, StockCritico As Single = 0
        If tb.Rows.Count > 0 Then
            For i = 0 To tb.Rows.Count - 1
                StockMinimo = DiasMinimo * IIf(IsDBNull(tb.Rows(i)("gastos")), 0, tb.Rows(i)("gastos")) / dias
                StockCritico = DiasCritico * IIf(IsDBNull(tb.Rows(i)("gastos")), 0, tb.Rows(i)("gastos")) / dias
                If StockMinimo Mod 1 <> 0 Then
                    StockMinimo = Int(StockMinimo) + 1


                End If
                If StockMinimo < Minimo Then
                    StockMinimo = Minimo
                End If
                If StockCritico Mod 1 <> 0 Then
                    StockCritico = Int(StockCritico) + 1
                End If
                cad = "Update t_semiterminados set minimo=" & StockMinimo & ",critico=" & StockCritico & " where id_lente=" & tb.Rows(i)("id_lente")
                cmd.CommandText = cad
                cmd.ExecuteNonQuery()
            Next
        End If
        mcon.Close()
        MsgBox("Actualizados Minimo y Critico")

    End Sub

    Public Function GetindiceByModelo(ByVal idmodelo As Integer) As Single
        Dim indice As Single = 0
        Dim cad As String = "select indice from t_modelos where id_lente=" & idmodelo
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        indice = cmd.ExecuteScalar
        mcon.Close()
        Return indice
    End Function

    Public Sub ModificaBaseNominal(ByVal idmodelo As Integer, ByVal diametro As Integer, ByVal base As Decimal, ByVal nuevabase As Decimal)
        Dim cad As String = "Update t_bases set base=" & NumSql(nuevabase) & " where id_modelo=" & idmodelo & " and diametro=" & diametro & " and base=" & NumSql(base) & vbNewLine & _
        "Update t_semiterminados set base=" & NumSql(nuevabase) & " where id_modelo=" & idmodelo & " and diametro=" & diametro & " and base=" & NumSql(base)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function GetModelosByTipologia(ByVal tipo As Integer, Optional ByVal Baja As Boolean = False, Optional ByVal Modelobase As Boolean = False) As DataTable
        Dim Filtro As String = " where tipologia=" & tipo
        If Baja = False Then
            Filtro = Filtro & " and baja=0"
        End If
        If Modelobase = True Then
            Filtro = Filtro & " and id_modelo_iot=0"
        End If
        Dim cad As String = "select id_lente,nombre from t_modelos " & Filtro & " order by nombre"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub BloqueaPedidos()
        'aqui vamos a bloquear la tabla de pedidos para que exista un orden de grabacion
        Dim cad As String = "INSERT INTO t_bloqueos select count(*)+1,-1," & mUsuario.id & ",0 from t_bloqueos where pedidos<>0"
        Dim cmd As New SqlCommand(cad, mcon)
        Dim cnnabierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            cnnabierta = True
        Else
            mcon.Open()
        End If

        cmd.ExecuteNonQuery()
        If cnnabierta = False Then
            mcon.Close()
        End If
    End Sub

    Public Sub DesloqueaPedidos()
        'aqui vamos a bloquear la tabla de pedidos para que esxista un orden de grabacion
        Dim cad As String = "delete from t_bloqueos  where pedidos<>0 and usuario=" & mUsuario.id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function UsuarioBloqueoPedido() As Integer
        Dim cad As String = "select usuario from t_bloqueos where pedidos<>0 order by orden"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            Return mUsuario.id
        Else
            Return tb.Rows(0)("usuario")
        End If

    End Function

    Public Sub BloqueaAlbaranes()
        'aqui vamos a bloquear la tabla de pedidos para que exista un orden de grabacion
        Dim cad As String = "INSERT INTO t_bloqueos select count(*)+1,0," & mUsuario.id & ",-1 from t_bloqueos where pedidos<>0"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Sub DesloqueaAlbaranes()
        'aqui vamos a bloquear la tabla de pedidos para que esxista un orden de grabacion
        Dim cad As String = "delete from t_bloqueos  where Albaranes<>0 and usuario=" & mUsuario.id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function UsuarioBloqueoAlbaranes() As Integer
        Dim cad As String = "select usuario from t_bloqueos where albaranes<>0 order by orden"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            Return mUsuario.id
        Else
            Return tb.Rows(0)("usuario")
        End If

    End Function
    Public Function ChequeaPasoDado(ByVal idpedido As Integer, ByVal mpro As Proceso, ByVal mipaso As Paso) As Boolean
        'en esta funcion chequeamos que no existan dos incidencias seguidas de endurecido
        Dim campo As String
        If mpro = Proceso.Entrada Then
            campo = "Fe_"
        Else
            campo = "fs_"
        End If
        Select Case mipaso
            Case Paso.almacen
                campo = campo & "almacen"
            Case Paso.Fabricacion
                campo = campo & "fabrica"
            Case Paso.Coloracion
                campo = campo & "coloracion"
            Case Paso.Endurecido
                campo = campo & "endurecimiento"
            Case Paso.Calidad
                campo = campo & "calidad"
            Case Paso.Montaje
                campo = campo & "montaje"
            Case Paso.Antireflejo
                campo = campo & "antireflejo"
        End Select
        Dim cad As String = "select top 1 " & campo & " from t_ordenes_trabajo where id_pedido=" & idpedido & "  order by id_orden desc"

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim existe As Boolean = cmd.ExecuteScalar
        mcon.Close()
        Return existe
    End Function

    Public Function ChequeaDobleEndurecido(ByVal idpedido As Integer) As Integer
        'en esta funcion chequeamos que no existan dos incidencias seguidas de endurecido
        Dim cad As String = "select count(*) from t_ordenes_trabajo where paso=3 and id_pedido=" & idpedido & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & idpedido & ")"
        Dim existe As Integer = 0
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        existe = cmd.ExecuteScalar
        mcon.Close()
        If existe = 0 Then
            'devolvemos 0
            Return 0
        Else ' lo volvemos a pasar por ahi hasta 2 veces
            cad = "select count(*) from t_ordenes_trabajo where paso=3 and id_pedido=" & idpedido & " and id_orden=(select max(id_orden)-1 from t_ordenes_trabajo where id_pedido=" & idpedido & ")"
            mcon.Open()
            cmd.CommandText = cad
            existe = existe + cmd.ExecuteScalar
            mcon.Close()
            If existe = 1 Then
                Return existe
            End If
            cad = "select count(*) from t_ordenes_trabajo where paso=3 and id_pedido=" & idpedido & " and id_orden=(select max(id_orden)-2 from t_ordenes_trabajo where id_pedido=" & idpedido & ")"
            mcon.Open()
            cmd.CommandText = cad
            existe = existe + cmd.ExecuteScalar
            mcon.Close()
            Return existe
        End If
    End Function


    Public Function EsSemiterminado(ByVal p As clsPedido) As Boolean
        'chequeamos que en la ultima orden de trabajo haya sacado del almacen un semiterminado
        Dim cad As String = "select count (*) from t_salidas_semiterminados where id_pedido=" & p.id & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & p.id & ")"

        Dim Cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim Semiterminado As Boolean = CBool(Cmd.ExecuteScalar)
        mcon.Close()
        Return Semiterminado
    End Function

    Public Function GetMaxDiametro(ByVal Modelo As Integer, ByVal diametro As Single) As Boolean
        Dim cad As String = "select count(*) from t_semiterminados where (id_modelo=" & Modelo & " or id_modelo=(select id_modelo_iot from t_modelos where id_lente=" & Modelo & ")) And diametro >= " & diametro
        Dim cmd As New SqlCommand(cad, mcon)
        Dim a As Boolean = False
        mcon.Open()
        a = cmd.ExecuteScalar
        mcon.Close()
        Return a
    End Function

    Public Function GetProveedoresbySemiterminado(ByVal idlente As Integer) As String
        Dim cad As String = "select proveedor from Proveedores.dbo.t_proveedores where id_proveedor in (select id_proveedor from t_cod_barras_semiterminado where id_lente= " & idlente & ")"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count > 0 Then
            Dim i As Integer
            cad = ""
            For i = 0 To tb.Rows.Count - 1
                cad = cad & tb.Rows(i)("proveedor") & ","
            Next
            Return cad.Substring(0, cad.Length - 1)
        End If
        Return ""
    End Function

    Public Sub CambioClave(ByVal clave As String)
        Dim cad As String = "Update t_usuarios set clave='" & clave & "',cambio_clave=0 where id_usuario=" & mUsuario.id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function GetBases(ByVal idModelo As Integer, ByVal diametro As Integer) As DataTable
        Dim cad As String = "select * from t_bases where id_modelo=" & idModelo & " and diametro=" & diametro & " order by base"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub GrabaBase(ByVal idModelo As Integer, ByVal diametro As Integer, ByVal base As Single, ByVal desde As Single, ByVal hasta As Single, ByVal desviacion As Single, ByVal negativa As Single, ByVal desdeProg As Single, ByVal hastaprog As Single, ByVal desviacionProg As Single)

        Dim cad As String = "Update t_bases set base_negativa=" & Replace(negativa, ",", ".") & ",desde=" & Replace(desde, ",", ".") & ",hasta=" & Replace(hasta, ",", ".") & ",desviacion=" & Replace(desviacion, ",", ".") & _
        ",desde_prog=" & NumSql(desdeProg) & ",hasta_prog=" & NumSql(hastaprog) & ",desviacion_prog=" & NumSql(desviacionProg) & " where id_modelo=" & idModelo & " and diametro=" & diametro & " and base=" & Replace(base, ",", ".")
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetMensajeAlbaran() As String
        Dim Cad As String = "Select mensaje_albaran from t_valores_globales"
        Dim mensaje As String = ""
        Dim cmd As New SqlCommand(Cad, mcon)
        mcon.Open()
        mensaje = cmd.ExecuteScalar
        mcon.Close()
        Return mensaje

    End Function
    Public Sub GrabaMensajeAlbaran(ByVal mensaje As String)
        Dim cad As String = "UPDATE t_valores_globales SET mensaje_albaran=" & strsql(mensaje)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub GrabaSemiterminados(ByVal idmodelo As Integer, ByVal diametro As Integer, ByVal ojo As Array, ByVal base As Array, ByVal adicion As Array)

        Dim Ultimo As Integer = getMaxId("id_lente", "t_semiterminados") + 1
        Dim j As Single, k As Single, ojito As String
        For Each k In adicion
            For Each j In base
                For Each ojito In ojo
                    'primero buscamos que no exista el semiterminado en cuestion
                    Dim Existe As Boolean = False
                    Dim Mira As String = "select count(*) from t_semiterminados where id_modelo=" & idmodelo & " and diametro=" & diametro & " and ojo='" & ojito & "' and base=" & Replace(j, ",", ".") & " and adicion=" & Replace(k, ",", ".")
                    Dim cm As New SqlCommand(Mira, mcon)
                    mcon.Open()
                    Existe = cm.ExecuteScalar
                    mcon.Close()
                    If Existe = False Then
                        Dim cad As String = "INSERT INTo t_semiterminados VALUES (" & Ultimo & "," & idmodelo & "," & Replace(j, ",", ".") & "," & diametro & "," & Replace(k, ",", ".") & ",'" & ojito & "',1,1,1)"
                        Dim cmd As New SqlCommand(cad, mcon)
                        Ultimo = Ultimo + 1
                        mcon.Open()
                        cmd.ExecuteNonQuery()
                        mcon.Close()
                        cmd = Nothing
                    End If
                Next
            Next
        Next
        For Each j In base
            Dim Existe As Boolean = False
            Dim Mira As String = "select count(*) from t_bases where id_modelo=" & idmodelo & " and diametro=" & diametro & " and base=" & Replace(j, ",", ".")
            Dim cm As New SqlCommand(Mira, mcon)
            mcon.Open()
            Existe = cm.ExecuteScalar
            mcon.Close()
            If existe = False Then
                Dim cad As String = "INSERT INTo t_bases VALUES (" & idmodelo & "," & diametro & "," & Replace(j, ",", ".") & ",0,0,0," & Replace(j, ",", ".") & "," & Replace(j, ",", ".") & ",0,0,0)"
                Dim cmd As New SqlCommand(cad, mcon)
                mcon.Open()
                cmd.ExecuteNonQuery()
                mcon.Close()
                cmd = Nothing
            End If
        Next

    End Sub

    Public Function GrabaSemiterminado(ByVal S As clsSemiterminado) As Integer
        Dim cad As String = ""
        If S.IdLente = 0 Then ' estamos añadiendo
            S.IdLente = getMaxId("id_lente", "t_semiterminados") + 1
            cad = "insert into t_semiterminados (id_lente,id_modelo,base,diametro,adicion,ojo,minimo,critico,stock,desde,hasta,variacion) VALUES (" & S.IdLente & "," & S.idModelo & _
            "," & Replace(S.Base, ",", ".") & S.Diametro & "," & Replace(S.Adicion, ",", ".") & ",'" & S.Ojo & "'," & S.Minimo & "," & S.Critico & "," & S.Stock & "," & Replace(S.RangoMinimo, ",", ".") & "," & Replace(S.RangoMaximo, ",", ".") & "," & Replace(S.Desviacion, ",", ".") & ")"
        Else 'estamos updateando
            cad = "UPDATE t_semiterminados set stock=" & S.Stock & ",minimo=" & S.Minimo & ",critico=" & S.Critico & " where id_lente=" & S.IdLente
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        Return S.IdLente
    End Function

    Private Function GetSemiterminadosByModelo(ByVal idModelo As Integer) As ClsSemiterminados
        Dim cad As String = "select t_semiterminados.*,(select nombre from t_modelos where id_modelo=t_semiterminados.id_modelo) as modelo from t-semiterminados where id_modelo=" & idModelo
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Dim Semis As New ClsSemiterminados
        Dim mrow As DataRow
        For Each mrow In tb.Rows
            Dim s As New clsSemiterminado
            s.IdLente = mrow("id_lente")
            s.idModelo = idModelo
            s.Modelo = mrow("modelo")
            s.Minimo = mrow("minimo")
            s.Stock = mrow("stock")
            s.Base = mrow("base")
            s.Adicion = mrow("adicion")
            s.Critico = mrow("critico")
            s.RangoMinimo = mrow("desde")
            s.RangoMaximo = mrow("hasta")
            s.Desviacion = mrow("variacion")
            s.Ojo = mrow("ojo")
            Semis.add(s)
            s = Nothing
        Next
        Return Semis
    End Function

    Public Function EsFabricacion(ByVal idpedido As Long) As Boolean
        Dim cad As String = "select case when modo='F' then -1 else 0 end from t_pedidos where id_pedido=" & idpedido
        Dim cmd As New SqlCommand(cad, mcon)
        Dim Resultado As Boolean = False
        mcon.Open()
        Resultado = CBool(cmd.ExecuteScalar)
        mcon.Close()
        Return Resultado
    End Function

    Public Function ExistePedido(ByVal idpedido As Long) As Boolean
        Dim cad As String = "select count(*) from t_pedidos where id_pedido=" & idpedido
        Dim cmd As New SqlCommand(cad, mcon)
        Dim Resultado As Boolean = False
        mcon.Open()
        Resultado = CBool(cmd.ExecuteScalar)
        mcon.Close()
        Return Resultado
    End Function
    Public Function ExisteGrupoFacturaUnica() As Boolean
        Dim cad As String = "select count(*) from t_grupos_opticos where factura=1"
        Dim cmd As New SqlCommand(cad, mcon)
        Dim Resultado As Boolean = False
        mcon.Open()
        Resultado = CBool(cmd.ExecuteScalar)
        mcon.Close()
        Return Resultado
    End Function

    Public Function Color(ByVal idpedido As Long) As Integer
        Dim cad As String = "select id_coloracion from t_pedidos where id_pedido=" & idpedido
        Dim cmd As New SqlCommand(cad, mcon)
        Dim Resultado As Integer = 0
        mcon.Open()
        Resultado = (cmd.ExecuteScalar)
        mcon.Close()
        Return Resultado
    End Function

    Public Function Tratamiento(ByVal Idpedido As Integer) As Integer
        Dim cad As String = "select id_tratamiento from t_pedidos where id_pedido=" & Idpedido
        Dim cmd As New SqlCommand(cad, mcon)
        Dim Resultado As Integer = 0
        mcon.Open()
        Resultado = (cmd.ExecuteScalar)
        mcon.Close()
        Return Resultado
    End Function

    Public Function Montaje(ByVal Idpedido As Integer) As Boolean
        Dim cad As String = "select montaje from t_pedidos where id_pedido=" & Idpedido
        Dim cmd As New SqlCommand(cad, mcon)
        Dim Resultado As Boolean = False
        mcon.Open()
        Resultado = (cmd.ExecuteScalar)
        mcon.Close()
        Return Resultado
    End Function

    Public Sub GrabaMotivoAbono(ByVal idAlbAbono As Integer, ByVal motivo As Integer, ByVal idalbaran As Integer)
        Dim cad As String = "INSERT INTO t_detalle_abono (id_albaran_abono,id_motivo,id_albaran)  VALUES(" & idAlbAbono & "," & motivo & "," & idalbaran & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        cmd = Nothing
    End Sub

    Public Function GetMotivosAbono() As DataTable
        Dim cad As String = "select ID_MOTIVO,tipo_MOTIVO + '/' + motivo AS CAUSA from t_MOTIVOS_ABONO WHERE BAJA=0 ORDER BY tipo_MOTIVO,motivo"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim TB As New DataTable
        mcon.Open()
        mda.Fill(TB)
        mcon.Close()
        Return TB
    End Function
    Public Function GetBloquePedidos(ByVal p As clsPedido) As DataTable
        Dim cad As String = "select * from t_pedidos where anulado=0 and fecha=" & FechaAcadena(p.Fechapedido) & " and hora=" & p.horaPedido & " and id_cliente=" & p.id_cliente '*****LO AÑADIREMOS MAS TARDE & " and id_usuario=" & p.id_usuario
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim TB As New DataTable
        mcon.Open()
        mda.Fill(TB)
        mcon.Close()
        Return TB
    End Function
    Public Function GetPedidosEntreFechas(ByVal fecini As Long, ByVal fecfin As Long, Optional ByVal Hora As Integer = 0) As DataTable
        Dim cad As String = "select left(right(fecha,4),2) +'/' + right(fecha,2) +'/' + left(fecha,4) as [Fecha Pedido],(select count(*) from t_pedidos where modo='S' and t_pedidos.fecha=ped.fecha and anulado=0) as stock,(select count(*) from t_pedidos where modo='T' and t_pedidos.fecha=ped.fecha and anulado=0) as Transformacion, (select count(*) from t_pedidos where modo='F' and t_pedidos.fecha=ped.fecha and anulado=0) as Fabricacion  from t_pedidos as ped where fecha>=" & fecini & " and fecha<=" & fecfin & " group by fecha order by fecha"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim TB As New DataTable
        mcon.Open()
        mda.Fill(TB)
        mcon.Close()
        Return TB
    End Function
    Public Function GetTransportista() As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select * from t_transportistas where baja=0 "
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetTransportistaEtiquetas() As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select etiqueta from t_transportistas where baja=0 group by etiqueta order by etiqueta"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetTransportista(ByVal nombre As String) As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select * from t_transportistas where baja=0 and transportista like " & strsql(nombre)
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetPedidosWebAutomaticos() As DataTable
        Dim cad As String = "select referencia,id_pedido,fecha,cilindro,esfera,eje,(select nombre from t_modelos where id_lente=t_pedidos.id_modelo) as modelo," & _
        "(select nombre from t_tratamientos where id_tratamiento=t_pedidos.id_tratamiento) as tratamiento," & _
        "(select color from t_coloraciones where id_coloracion=t_pedidos.id_coloracion) as color, intensidad, eje,adicion,(select nombre_comercial from t_clientes where id_cliente=t_pedidos.id_cliente) as cliente, 'X' as imprimir from t_pedidos where  anulado=0 and id_pedido in (select id_pedido from t_pedidos_web) order by id_pedido"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetProcesosEntreFechas(ByVal fecini As Long, ByVal fecfin As Long, ByVal agrupado As Boolean, ByVal Tratamiento As Boolean) As DataTable

        Dim Campos As String = "modo,"
        Dim Grupo As String = " group by modo"

        If agrupado = True Then
            Campos = Campos & "(select nombre from t_modelos where id_lente=t_pedidos.id_modelo) as modelo,"
            Grupo = Grupo & ",id_modelo"
        End If
        If Tratamiento = True Then
            Campos = Campos & "(select nombre from t_tratamientos where id_tratamiento=t_pedidos.id_tratamiento) as tratamiento,"
            Grupo = Grupo & ",id_tratamiento"
        End If
        Dim cad As String = "(select 1 as orden, 'Fabricacion' as Proceso," & Campos & "count(*) as Cantidad,count(distinct(t_pedidos.id_pedido)) as Pedidos from t_ordenes_trabajo INNER JOIN t_pedidos ON t_pedidos.id_pedido=t_ordenes_trabajo.id_pedido where fs_fabrica>=" & fecini & " and fs_fabrica<=" & fecfin & Grupo & ") UNION " & _
        "(select 2 as orden,'Coloracion' as Proceso," & Campos & "count(*) as Cantidad,count(distinct(t_pedidos.id_pedido)) as Pedidos from t_ordenes_trabajo INNER JOIN t_pedidos ON t_pedidos.id_pedido=t_ordenes_trabajo.id_pedido where fs_coloracion>=" & fecini & " and fs_coloracion<=" & fecfin & Grupo & ") UNION " & _
        "(select 3 as orden,'Endurecido' as Proceso," & Campos & "count(*) as Cantidad,count(distinct(t_pedidos.id_pedido)) as Pedidos from t_ordenes_trabajo INNER JOIN t_pedidos ON t_pedidos.id_pedido=t_ordenes_trabajo.id_pedido where fs_endurecimiento>=" & fecini & " and fs_endurecimiento<=" & fecfin & Grupo & ") UNION " & _
        "(select 4 as orden,'Antireflejo' as Proceso," & Campos & "count(*) as Cantidad,count(distinct(t_pedidos.id_pedido)) as Pedidos from t_ordenes_trabajo INNER JOIN t_pedidos ON t_pedidos.id_pedido=t_ordenes_trabajo.id_pedido where fs_antireflejo>=" & fecini & " and fs_antireflejo<=" & fecfin & Grupo & ") UNION " & _
        "(select 5 as orden,'Calidad' as Proceso," & Campos & "count(*) as Cantidad,count(distinct(t_pedidos.id_pedido)) as Pedidos from t_ordenes_trabajo INNER JOIN t_pedidos ON t_pedidos.id_pedido=t_ordenes_trabajo.id_pedido where fs_calidad>=" & fecini & " and fs_calidad<=" & fecfin & Grupo & ") UNION " & _
         "(select 6 as orden,'Montaje' as Proceso," & Campos & "count(*) as Cantidad,count(distinct(t_pedidos.id_pedido)) as Pedidos from t_ordenes_trabajo INNER JOIN t_pedidos ON t_pedidos.id_pedido=t_ordenes_trabajo.id_pedido where fs_montaje>=" & fecini & " and fs_montaje<=" & fecfin & Grupo & ")UNION " & _
         "(select 7 as orden,'Fab.Externa' as Proceso," & Campos & "count(*) as Cantidad,count(distinct(t_pedidos.id_pedido)) as Pedidos from t_ordenes_trabajo INNER JOIN t_pedidos ON t_pedidos.id_pedido=t_ordenes_trabajo.id_pedido where fs_externa>=" & fecini & " and fs_externa<=" & fecfin & Grupo & ")  "


        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function PedidosProveedorEntreFechas(ByVal fecini As Long, ByVal fecfin As Long) As DataTable
        ' Dim cad As String = "select t_modelos.nombre as modelo,t_tratamientos.nombre as tratamiento, sum(servido) as [lentes servidas] from (((t_pedidos_prov INNER JOIN t_lineas_pedidos_prov ON t_pedidos_prov.id_pedido=t_lineas_pedidos_prov.id_pedido) INNER JOIN t_lentes_stock ON t_lentes_stock.id_producto=t_lineas_pedidos_prov.id_producto) INNER JOIN t_modelos on t_lentes_stock.id_modelo=t_modelos.id_lente)" & _
        ' " INNER JOIN t_tratamientos on t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento Where fecha>=" & fecini & " and fecha<=" & fecfin & " group by t_modelos.nombre,t_tratamientos.nombre order by t_modelos.nombre,t_tratamientos.nombre"
        Dim cad As String = "select t_modelos.nombre as modelo,t_tratamientos.nombre as tratamiento, sum(cantidad) as [lentes compradas] from ((t_movimientos_almacen INNER JOIN t_lentes_stock ON t_lentes_stock.id_producto=t_movimientos_almacen.id_producto) INNER JOIN t_modelos on t_lentes_stock.id_modelo=t_modelos.id_lente)" & _
        " INNER JOIN t_tratamientos on t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento Where fecha>=" & fecini & " and fecha<=" & fecfin & " and entrada=1 group by t_modelos.nombre,t_tratamientos.nombre order by t_modelos.nombre,t_tratamientos.nombre"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetIncidenciasEntreFechas(ByVal fecini As Long, ByVal fecfin As Long, ByVal agrupado As Boolean, ByVal Tratamiento As Boolean, ByVal usuario As Boolean) As DataTable
        'vamos a modificar la consulta para que las incidencias sean para los pedidos entre una fecha determinada
        Dim Campos As String = "incidencia,"
        Dim Grupo As String = " group by incidencia"
        If agrupado = True Then
            Campos = Campos & "(select nombre from t_modelos where id_lente=t_pedidos.id_modelo) as modelo,"
            Grupo = Grupo & ",id_modelo"
        End If

        If Tratamiento = True Then
            Campos = Campos & "(select nombre from t_tratamientos where id_tratamiento=t_pedidos.id_tratamiento) as tratamiento,"
            Grupo = Grupo & ",id_tratamiento"
        End If
        If usuario = True Then
            Campos = Campos & "(select nombre + ' ' + apellidos from t_usuarios where id_usuario=incidencias1.id_usuario) as Usuario,"
            Grupo = Grupo & ",incidencias1.id_usuario"
        End If
        Dim cad As String = "CREATE VIEW incidencias as (select incidencia,t_incidencias_pedidos.id_usuario,t_incidencias_pedidos.id_pedido,t_incidencias_pedidos.id_orden+1 as orden from t_incidencias_pedidos " & _
        "  INNER JOIN t_incidencias ON t_incidencias.id_incidencia=t_incidencias_pedidos.id_incidencia where t_incidencias_pedidos.id_pedido in (select id_pedido from t_pedidos where fecha>=" & fecini & " and fecha<=" & fecfin & _
        " and anulado=0)  and t_incidencias_pedidos.id_incidencia<>1)"
        Dim cmd As New SqlCommand(cad, mcon)

        Dim tb As New DataTable
        BorraConsulta("incidencias")
        BorraConsulta("incidencias1")
        mcon.Open()

        cmd.ExecuteNonQuery()
        'ahora vamos a crear otra consulta con las incidencias y las ordenes de trabajo
        cmd.CommandText = "CREATE VIEW Incidencias1 as (select incidencias.* from incidencias INNER JOIN t_ordenes_trabajo ON incidencias.id_pedido=t_ordenes_trabajo.id_pedido where incidencias.orden=t_ordenes_trabajo.id_orden and paso=0)"
        cmd.ExecuteNonQuery()

        cad = "select " & Campos & "count(*) as total from incidencias1 INNER JOIN t_pedidos ON incidencias1.id_pedido=t_pedidos.id_pedido " & Grupo

        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mda.SelectCommand.CommandTimeout = 150
        mda.Fill(tb)
        cmd.CommandText = "DROP VIEW incidencias"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "DROP VIEW incidencias1"
        cmd.ExecuteNonQuery()
        mcon.Close()
        Return tb
    End Function

    Public Function GetCodContaplusByidBase(ByVal idbase As Integer, ByVal coniva As Boolean) As String
        Dim cad As String = "", Siniva As String = ""
        If coniva = False Then
            Siniva = "_siniva"
        End If
        cad = "select cod_contable" & Siniva & " as codContable from t_tipos_base where id_tipo_base=" & idbase
        Dim CodContable As String
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        CodContable = cmd.ExecuteScalar
        mcon.Close()
        Return CodContable
    End Function

    Public Function GetLentesporPedidos(ByVal fecini As Long, ByVal fecfin As Long, Optional ByVal agrupado As Boolean = False, Optional ByVal tratamiento As Boolean = False) As DataTable
        'Dim cad As String = "CREATE VIEW LentesGastadas as (select id_pedido,modo,(select count(*) from t_ordenes_trabajo where id_pedido=t_pedidos.id_pedido and paso=0) as LentesUsadas from t_pedidos where fecha>=" & fecini & " and fecha<=" & fecfin & ")"
        'mcon.Open()
        Dim cad As String = ""
        If agrupado = False Then
            cad = "select modo,COUNT(distinct (T_PEDIDOS.ID_PEDIDO)) AS Pedidos,count(t_salidas_semiterminados.*) as lentes,convert(numeric (8,2),100*(count(id_orden)-" & _
            " COUNT(distinct T_PEDIDOS.ID_PEDIDO))/convert(decimal(8,2),COUNT( t_salidas_semiterminados.*))) as [% Errores] from t_pedidos inner join T_SALIDAS_SEMITERMINADOS ON T_PEDIDOS.ID_PEDIDO=T_SALIDAS_SMITERMINADOS.ID_PEDIDO" & _
            " where t_pedidos.fecha>=" & fecini & " and anulado=0 and t_pedidos.fecha<=" & fecfin & " group by modo"
        Else
            If tratamiento = False Then
                cad = "select modo,modelo, count(*) as pedidos,sum(lentes) as lentes, 100*(sum(lentes)-count(*))/count(*) as rechazo  from  " & _
                " (select t_pedidos.id_pedido,modo,(select nombre from t_modelos where id_lente=t_pedidos.id_modelo) as modelo, count(*) as lentes " & _
                " from t_pedidos INNER JOIN t_salidas_semiterminados ON t_salidas_semiterminados.id_pedido=t_pedidos.id_pedido   " & _
                " where t_pedidos.fecha>=" & fecini & " and anulado=0 and t_pedidos.fecha<=" & fecfin & "  group by modo,id_modelo,t_pedidos.id_pedido) as vista group by modo,modelo"
            Else
                cad = "select modo,modelo,tratamiento, count(*) as pedidos,sum(lentes) as lentes, 100*(sum(lentes)-count(*))/count(*) as rechazo  from  " & _
                " (select t_pedidos.id_pedido,modo,(select nombre from t_modelos where id_lente=t_pedidos.id_modelo) as modelo, (select nombre from t_tratamientos where id_tratamiento=t_pedidos.id_tratamiento) as Tratamiento, count(*) as lentes " & _
                " from t_pedidos INNER JOIN t_salidas_semiterminados ON t_salidas_semiterminados.id_pedido=t_pedidos.id_pedido   " & _
                " where t_pedidos.fecha>=" & fecini & " and anulado=0 and t_pedidos.fecha<=" & fecfin & "  group by modo,id_modelo,t_pedidos.id_pedido,id_tratamiento) as vista group by modo,modelo,tratamiento"
            End If
        End If
        'Dim cmd As New SqlCommand(cad, mcon)
        ''cmd.ExecuteNonQuery()
        ''mcon.Close()
        'mcon.Open()
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.SelectCommand.CommandTimeout = 420
        Dim tb As New DataTable
        mda.Fill(tb)
        'mcon.Close()
        Return tb
    End Function

    Public Function GetCodUDI() As DataTable
        ' Dim Cad As String = "Select CODIGO_GCP,isnull(id_modelo,-id_lente) as id_modelo,dig_control,nombre as modelo,cod_udi,Reg_sanitario from T_VALORES_GLOBALES,t_modelos LEFT OUTER JOIN t_udi ON t_modelos.id_lente=t_udi.id_modelo where  id_cliente=0 and id_lente not in (select id_modelo from t_udi where cod_UDI='00000') order by cod_udi,orden"
        Dim Cad As String = "(Select CODIGO_GCP,isnull(id_modelo,-id_lente) as id_modelo,dig_control,nombre as modelo,cod_udi,Reg_sanitario from T_VALORES_GLOBALES,t_modelos INNER JOIN t_udi ON t_modelos.id_lente=t_udi.id_modelo where  id_cliente=0 and id_lente  not in (select id_modelo from t_udi where cod_UDI='00000')  ) UNION (Select CODIGO_GCP,-id_lente as id_modelo,NULL as dig_control,nombre as modelo,'11111' as cod_udi,'' as Reg_sanitario from T_VALORES_GLOBALES,t_modelos where  id_cliente=0 and baja=0 and id_lente not in (select id_modelo from t_udi )) order by cod_udi"

        Dim mda As New SqlDataAdapter(Cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetCodUDIToExcel() As DataTable
        Dim Cad As String = "Select nombre as modelo,CODIGO_GCP+ cod_udi as UDI ,dig_control,Reg_sanitario from T_VALORES_GLOBALES,t_modelos LEFT OUTER JOIN t_udi ON t_modelos.id_lente=t_udi.id_modelo where baja=0 and id_cliente=0 and id_lente not in (select id_modelo from t_udi where cod_UDI='00000') order by cod_udi,orden"
        Dim mda As New SqlDataAdapter(Cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Sub GrabaCodUDI(ByVal idmodelo As Integer, ByVal codigo As String, ByVal Control As Integer, ByVal RegSanitario As String)
        Dim cad As String
        If idmodelo < 0 Then
            cad = "INSERT INTO t_udi select " & -idmodelo & "," & strsql(Control) & "," & strsql(RegSanitario) & "," & strsql(codigo)
        Else
            cad = "UPDATE t_udi set reg_sanitario=" & strsql(RegSanitario) & " where id_modelo=" & idmodelo
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetFacturacionporPedidos(ByVal fecini As Long, ByVal fecfin As Long, Optional ByVal agrupado As Boolean = False, Optional ByVal tratamiento As Boolean = False, Optional ByVal cliente As Boolean = False) As DataTable
        'Dim cad As String = "CREATE VIEW LentesGastadas as (select id_pedido,modo,(select count(*) from t_ordenes_trabajo where id_pedido=t_pedidos.id_pedido and paso=0) as LentesUsadas from t_pedidos where fecha>=" & fecini & " and fecha<=" & fecfin & ")"
        'mcon.Open()
        '****************************ARREGLAR ESTO***************************************
        Dim Cli As String = ""
        'Dim Consulta As String = "CREATE VIEW consumo1 as (select id_cliente,t_lineas_albaran.*,(select isnull(sum(coste),0) from t_costos_pedido where id_pedido=t_lineas_albaran.id_pedido) as coste,case  when precio>= 0 then 1 else 0 END as Cantidad from t_lineas_albaran INNER JOIN t_albaranes ON t_albaranes.id_albaran=t_lineas_albaran.id_albaran " & _
        '" where fecha>= " & fecini & " and fecha<=" & fecfin & Cli & " and t_lineas_albaran.montaje=0  and id_tipo_producto=1 and id_modelo<>0)"
        Dim COnsulta As String = "CREATE VIEW consumo1 as (select t_pedidos.*, (select isnull(sum(coste),0) from t_costos_pedido where id_pedido=t_pedidos.id_pedido) as coste,(select sum(total) from t_lineas_albaran where montaje=0 and id_pedido=t_pedidos.id_pedido) as total, 1 as cantidad from t_pedidos where id_pedido in (select id_pedido from t_lineas_albaran where montaje=0 and id_albaran in (select id_albaran from t_albaranes where fecha>=" & fecini & " and fecha<=" & fecfin & ")))"
        Dim Cmd As New SqlCommand(COnsulta, mcon)
        Dim group As String = "", PrecioCosto As String = ""
        Dim Lentes As String = ""
        Dim Vista As String = ""

        PrecioCosto = ",Sum(consumo1.coste) as [Precio Costo] "
        If agrupado = True And tratamiento = True Then
            'PrecioCosto = ",dbo.costegeneral()+dbo.costefabricacion(id_modelo,id_modo)+ dbo.costetratamiento(id_tratamiento,id_modo)+dbo.clip_iot(id_modelo,id_modo)+case id_modo when 3 then (select avg(precio) from t_precios_proveedores where id_modelo=(select case id_modelo_iot when 0 then id_lente else id_modelo_iot END from t_modelos where id_lente=consumo1.id_modelo)) ELSE 0 END as [Precio Costo] "
        End If
        If cliente = True Then
            Cli = "id_cliente,(select codigo from t_clientes where id_cliente=consumo1.id_cliente) as codigo,(select nombre_comercial from t_clientes where id_cliente=consumo1.id_cliente  ) as cliente,"
            group = ",id_cliente"
        End If
        Dim cad As String

        If agrupado = False Then

            cad = "select " & Cli & "modo,Sum(cantidad) AS Pedidos,sum(consumo1.total)  as facturacion,convert (decimal(8,2),case sum(cantidad) when 0 then sum(cantidad) ELSE sum(consumo1.total)/sum(cantidad) END ) as PVP" & PrecioCosto & " from consumo1  group by modo" & group
        Else
            If tratamiento = False Then
                cad = "Select  modo,(select nombre from t_modelos where id_lente=consumo1.id_modelo) as modelo," & Cli & "sum(cantidad) as Lentes,sum(total) as Facturacion,convert (decimal(8,2), case Sum(cantidad) when 0 then sum(total) else sum(total)/sum(cantidad) END) as PVP" & PrecioCosto & " from consumo1 GROUP BY modo,id_modelo" & group
            Else
                cad = "Select modo,(select nombre from t_modelos where id_lente=consumo1.id_modelo) as modelo,(select nombre from t_tratamientos where id_tratamiento=consumo1.id_tratamiento) as Tratamiento," & Cli & "sum(cantidad) as Lentes,sum(total) as facturacion,convert (decimal(8,2), case Sum(cantidad) when 0 then sum(total) else sum(total)/sum(cantidad) END) as PVP" & PrecioCosto & " from consumo1 GROUP BY modo,id_modelo,id_tratamiento" & group

            End If
        End If

        'cmd.ExecuteNonQuery()
        'mcon.Close()
        'antes de nada hay que borrar la consulta cuando exista
        BorraConsulta("consumo1")
        mcon.Open()
        Cmd.ExecuteNonQuery()
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Cmd.CommandText = "DROP VIEW Consumo1"
        Cmd.ExecuteNonQuery()
        mcon.Close()
        Return tb
    End Function

    Private Sub BorraConsulta(ByVal nombre As String)
        Dim cad As String = "select count(*) from sysobjects where  name like " & strsql(nombre)
        Dim existe As Boolean
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        existe = cmd.ExecuteScalar
        If existe = True Then
            cmd.CommandText = "DROP VIEW " & nombre
            cmd.ExecuteNonQuery()
        End If
        mcon.Close()
    End Sub
    Public Function GetPrecioColoracion(ByVal idgama As Integer, ByVal modelo As clsModelo) As Decimal
        Dim campo As String = "precio"
        If modelo.IndiceModelo < 1.6 Then
            campo = campo & "_LI as precio"
        Else
            campo = campo & "_HI as precio"
        End If
        Dim cmd As New SqlCommand("select " & campo & " from t_gamas_coloracion where id_gama=" & idgama, mcon)
        mcon.Open()
        Dim precio As Decimal = 0
        precio = cmd.ExecuteScalar
        mcon.Close()
        Return precio
    End Function
    Public Sub BajaCLiente(ByVal id As Integer)
        Dim cmd As New SqlCommand("Update t_clientes set baja=1 where id_cliente=" & id, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        cmd.CommandText = "UPDATE t_clientes_web set baja=1 where id_cliente=" & id
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub AltaCLiente(ByVal id As Integer)
        Dim cmd As New SqlCommand("Update t_clientes set baja=0 where id_cliente=" & id, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        cmd.CommandText = "UPDATE t_clientes_web set baja=0 where id_cliente=" & id
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function clientesSinCOnsumo(ByVal fecini As String, ByVal fecfin As String) As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select codigo,nombre_comercial,razon_social,direccion,poblacion,provincia,telefono,email from t_clientes INNER JOIN m_provincias on t_clientes.id_provincia=m_provincias.id_provincia where baja=0 and id_cliente not in (select case when id_cliente=0 then cod_envio else id_cliente end from t_albaranes  where fecha>=" & fecini & " and fecha<=" & fecfin & ") order by provincia,nombre_comercial"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function ConsumoPorClientes(ByVal fecini As String, ByVal fecfin As String, ByVal condicion As String) As DataTable
        Dim tb As New DataTable
        Dim cad As String = " select nombre_comercial,sum(total) from t_clientes left outer join t_albaranes on  t_clientes.id_cliente=t_albaranes.id_cliente   where fecha>=" & fecini & " and fecha<=" & fecfin & " group by nombre_comercial having " & condicion
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function ValoresGlobales() As DataTable
        Dim tb As New DataTable
        Dim cad As String = " select stock_miniMo,stock_critico from t_valores_globales"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Sub GrabavaloresGlobales(ByVal minimo As Integer, ByVal critico As Integer)
        Dim cad As String = "UPDATE t_valores_globales set stock_minimo=" & minimo & ",stock_critico=" & critico
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function ConsumoTipoLente(ByVal idcliente As Integer) As DataTable
        Dim fecha As Date = "01/" & Today.Month & "/" & Today.Year
        Dim fecha1 As Date = DateAdd(DateInterval.Month, -1, fecha)
        Dim fecha2 As Date = DateAdd(DateInterval.Month, -1, fecha1)
        Dim fecha3 As Date = DateAdd(DateInterval.Month, -1, fecha2)
        Dim cad As String = "declare @fecini integer declare @fecfin integer declare @fecini1 integer declare @fecfin1 integer declare @fecini2 integer declare @fecfin2 integer declare @fecini3 integer declare @fecfin3 integer " & _
        "set @fecini=" & FechaAcadena(fecha) & " " & _
        "set @fecfin=" & FechaAcadena(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha))) & " " & _
         "set @fecini1=" & FechaAcadena(fecha1) & " " & _
        "set @fecfin1=" & FechaAcadena(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha1))) & " " & _
         "set @fecini2=" & FechaAcadena(fecha2) & " " & _
        "set @fecfin2=" & FechaAcadena(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha2))) & " " & _
         "set @fecini3=" & FechaAcadena(fecha3) & " " & _
        "set @fecfin3=" & FechaAcadena(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha3))) & " " & _
        "(select 1 as orden,'Stock/Transformacion' as lente,(select count(*) as total from t_pedidos where modo not like 'F' and anulado=0 and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & ") as total, (select count(*)  from t_pedidos where modo not like 'F' and anulado=0 and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & ") as total1,(select count(*) from t_pedidos where modo not like 'F' and anulado=0 and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & ") as total2,(select count(*)  from t_pedidos where modo not like 'F' and anulado=0 and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & ") as total3) UNION " & _
        "(select 2,'Monofocal convencional' as lente,(select count(*) as total from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot=0)  and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & "),(select count(*) as total1 from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot=0)  and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & "),(select count(*) as total2 from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot=0)  and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & "),(select count(*) as total3 from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot=0)  and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & ")) UNION " & _
        "(select 3,'Monofocal Freeform' as lente,(select count(*) as total from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot<>0)  and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & "),(select count(*) as total1 from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot<>0)  and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & "),(select count(*) as total2 from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot<>0)  and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & "),(select count(*) as total3 from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot<>0)  and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & ")) UNION " & _
        "(select 4,'Bifocal convencional' as lente,(select count(*) as total from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot=0)  and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & "),(select count(*) as total1 from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot=0)  and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & "),(select count(*) as total2 from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot=0)  and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & "),(select count(*) as total3 from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot=0)  and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & ")) UNION " & _
        "(select 4,'Bifocal FF.' as lente,(select count(*) as total from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot<>0)  and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & "),(select count(*) as total1 from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot<>0)  and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & "),(select count(*) as total2 from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot<>0)  and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & "),(select count(*) as total3 from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot<>0)  and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & ")) UNION " & _
        "(select 5,'Prog. No personal.' as lente,(select count(*) as total from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=0)  and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & "),(select count(*) as total1 from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=0)  and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & "),(select count(*) as total2 from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=0)  and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & "),(select count(*) as total3 from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=0)  and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & ")) UNION " & _
        "(select 6,'Prog.Personalizado.' as lente,(select count(*) as total from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=1)  and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & "),(select count(*) as total1 from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=1)  and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & "),(select count(*) as total2 from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=1)  and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & "),(select count(*) as total3 from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=1)  and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & ")) order by orden "
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function
    Public Function FacturacionTipoLente(ByVal idcliente As Integer) As DataTable
        Dim fecha As Date = "01/" & Today.Month & "/" & Today.Year
        Dim fecha1 As Date = DateAdd(DateInterval.Month, -1, fecha)
        Dim fecha2 As Date = DateAdd(DateInterval.Month, -1, fecha1)
        Dim fecha3 As Date = DateAdd(DateInterval.Month, -1, fecha2)
        Dim cad As String = "declare @fecini integer declare @fecfin integer declare @fecini1 integer declare @fecfin1 integer declare @fecini2 integer declare @fecfin2 integer declare @fecini3 integer declare @fecfin3 integer " & _
        "set @fecini=" & FechaAcadena(fecha) & " " & _
        "set @fecfin=" & FechaAcadena(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha))) & " " & _
         "set @fecini1=" & FechaAcadena(fecha1) & " " & _
        "set @fecfin1=" & FechaAcadena(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha1))) & " " & _
         "set @fecini2=" & FechaAcadena(fecha2) & " " & _
        "set @fecfin2=" & FechaAcadena(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha2))) & " " & _
         "set @fecini3=" & FechaAcadena(fecha3) & " " & _
        "set @fecfin3=" & FechaAcadena(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha3))) & " " & _
        "(select 1 as orden,'Stock/Transformacion' as lente,(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido  from t_pedidos where modo not like 'F' and anulado=0 and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & ")) as total, (select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido  from t_pedidos where modo not like 'F' and anulado=0 and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & ")) as total1,(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where modo not like 'F' and anulado=0 and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & ")) as total2,(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido  from t_pedidos where modo not like 'F' and anulado=0 and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & ")) as total3) UNION " & _
        "(select 2,'Monofocal convencional' as lente,(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot=0)  and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot=0)  and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot=0)  and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot=0)  and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & "))) UNION " & _
        "(select 3,'Monofocal Freeform' as lente,(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot<>0)  and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido  from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot<>0)  and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot<>0)  and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot<>0)  and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & "))) UNION " & _
        "(select 4,'Bifocal convencional' as lente,(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot=0)  and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot=0)  and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot=0)  and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot=0)  and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & "))) UNION " & _
        "(select 4,'Bifocal FF.' as lente,(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot<>0)  and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot<>0)  and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot<>0)  and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido  from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2 and id_modelo_iot<>0)  and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & "))) UNION " & _
        "(select 5,'Prog. No personal.' as lente,(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=0)  and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=0)  and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=0)  and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=0)  and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & "))) UNION " & _
        "(select 6,'Prog.Personalizado.' as lente,(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=1)  and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=1)  and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=1)  and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & ")),(select isnull(sum(total),0) from t_lineas_albaran where montaje=0 and id_pedido in (select id_pedido from t_pedidos where  anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=1)  and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & "))) order by orden "
        Dim tb As New DataTable

        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.SelectCommand.CommandTimeout = 180
        mda.Fill(tb)
        Return tb
    End Function
    Public Function ConsumoGrupoLente(ByVal idcliente As Integer) As DataTable
        Dim fecha As Date = "01/" & Today.Month & "/" & Today.Year
        Dim fecha1 As Date = DateAdd(DateInterval.Month, -1, fecha)
        Dim fecha2 As Date = DateAdd(DateInterval.Month, -1, fecha1)
        Dim fecha3 As Date = DateAdd(DateInterval.Month, -1, fecha2)
        Dim cad As String = "declare @fecini integer declare @fecfin integer declare @fecini1 integer declare @fecfin1 integer declare @fecini2 integer declare @fecfin2 integer declare @fecini3 integer declare @fecfin3 integer " & _
        "set @fecini=" & FechaAcadena(fecha) & " " & _
        "set @fecfin=" & FechaAcadena(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha))) & " " & _
         "set @fecini1=" & FechaAcadena(fecha1) & " " & _
        "set @fecfin1=" & FechaAcadena(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha1))) & " " & _
         "set @fecini2=" & FechaAcadena(fecha2) & " " & _
        "set @fecfin2=" & FechaAcadena(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha2))) & " " & _
         "set @fecini3=" & FechaAcadena(fecha3) & " " & _
        "set @fecfin3=" & FechaAcadena(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha3))) & " " & _
        "select id_grupo, grupo,(select count(*) as total from t_pedidos where modo  like 'F' and anulado=0 and fecha>=@fecini and fecha<=@fecfin and id_cliente=" & idcliente & " and id_modelo in (select id_lente from t_modelos where id_grupo=t_grupos_modelos.id_grupo) ) as total, (select count(*)  from t_pedidos where modo  like 'F' and anulado=0 and fecha>=@fecini1 and fecha<=@fecfin1 and id_cliente=" & idcliente & " and id_modelo in (select id_lente from t_modelos where id_grupo=t_grupos_modelos.id_grupo) ) as total1,(select count(*) from t_pedidos where modo like  'F' and anulado=0 and fecha>=@fecini2 and fecha<=@fecfin2 and id_cliente=" & idcliente & " and id_modelo in (select id_lente from t_modelos where id_grupo=t_grupos_modelos.id_grupo) ) as total2,(select count(*)  from t_pedidos where modo  like 'F' and anulado=0 and fecha>=@fecini3 and fecha<=@fecfin3 and id_cliente=" & idcliente & " and id_modelo in (select id_lente from t_modelos where id_grupo=t_grupos_modelos.id_grupo)) as total3 from t_grupos_modelos order by grupo"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.SelectCommand.CommandTimeout = 180
        mda.Fill(tb)
        Return tb
    End Function
    Public Function Compromisos(ByRef fecha As String) As DataTable
        'descartamos los que son menores del 10/01/2010
        Dim cad As String = "select t_pedidos.fecha,t_pedidos.hora,urgente,t_clientes.nombre_comercial,case modo when 'S' then 'stock' when 'T' then 'Transformacion' ELSE 'Fabricacion' END as Tipo_pedido ,t_pedidos.id_pedido,t_pedidos.compromiso from t_pedidos INNER JOIN t_clientes" & _
        " ON t_pedidos.id_cliente=t_clientes.id_cliente where anulado=0 and compromiso<>0  and fecha_salida=0 and fecha>=20100101 and id_albaran=0  " & fecha
        Dim tb As New DataTable
        If mUsuario.Comercial = True Then
            cad = cad & " and id_comercial=" & mUsuario.id
        End If
        cad = cad & " order by urgente desc,compromiso"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb

    End Function
    Public Function Extraviados(ByRef fecha As String) As DataTable

        Dim cad As String = "select t_pedidos.fecha,t_pedidos.hora,t_clientes.nombre_comercial,case modo when 'S' then 'stock' when 'T' then 'Transformacion' ELSE 'Fabricacion' END as Tipo_pedido ,t_pedidos.id_pedido,t_pedidos.compromiso,urgente from (t_pedidos INNER JOIN t_clientes" & _
        " ON t_pedidos.id_cliente=t_clientes.id_cliente) INNER JOIN t_ordenes_trabajo ON t_ordenes_trabajo.id_pedido=t_pedidos.id_pedido where id_orden=1 and compromiso<>0 and anulado=0 and fecha_salida=0 and Fe_externa=0 and fs_almacen=0 and fe_fabrica=0 and  t_pedidos.id_pedido in (select id_pedido from t_ordenes_trabajo group by id_pedido having count(*)=1) and t_pedidos.fecha>=20071201 " & fecha
        If mUsuario.Comercial = True Then
            cad = cad & " and id_comercial=" & mUsuario.id
        End If
        cad = cad & " order by urgente desc,t_pedidos.fecha,t_pedidos.id_pedido"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))

        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb

    End Function

    Public Function HayCompromisosPendientes() As Boolean
        Dim Fechacompromiso As String
        If Now.DayOfWeek = DayOfWeek.Friday Then 'hay que pasarlo al lunes
            Fechacompromiso = FechaAcadena(DateAdd(DateInterval.Day, 3, Now.Date))
        ElseIf Now.DayOfWeek = DayOfWeek.Saturday Then
            Fechacompromiso = FechaAcadena(DateAdd(DateInterval.Day, 2, Now.Date))
        Else
            Fechacompromiso = FechaAcadena(DateAdd(DateInterval.Day, 1, Now.Date))
        End If
        Dim cad As String = "select count(*) from t_pedidos where anulado=0 and compromiso<>0 and ((modo='S' and compromiso<=" & FechaAcadena(Now.Date) & ")or (modo<>'S' and compromiso<=" & Fechacompromiso & ")) and fecha_salida=0"
        Dim compromisos As Integer = 0
        If mUsuario.Comercial = True Then
            cad = cad & " and id_comercial=" & mUsuario.id
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        compromisos = cmd.ExecuteScalar
        mcon.Close()
        If compromisos <> 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Sub SalidaMontaje(ByVal idMontaje As Integer)
        Dim cad As String = "Update t_pedidos_montajes set fecha_salida=" & FechaAcadena(Now.Date) & " where id_pedido_montaje=" & idMontaje
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub GrabaMontaje(ByRef Mont As clsMontaje)
        Dim cad As String = ""
        If Mont.Id = 0 Then
            Mont.Id = getMaxId("id_pedido_montaje", "t_pedidos_montajes") + 1
            cad = "INSERT INTO t_pedidos_montajes (id_pedido_montaje,fecha,id_cliente,ojo_izq,ojo_dcho,id_tipo_gafa,id_montura,notas,fecha_salida)" & _
            " VALUES (" & Mont.Id & "," & Mont.fecha & "," & Mont.Cli.id & "," & Mont.Izquierdo & "," & Mont.Derecho & "," & Mont.Idgafa & _
            "," & Mont.IdMontura & ",'" & Mont.Notas & "'," & Mont.FechaSalida & ")"
        Else
            cad = "UPDATE t_pedidos_montajes set ojo_izq=" & Mont.Izquierdo & ",ojo_dcho=" & Mont.Derecho & ", notas=" & strsql(Mont.Notas) & " where id_pedido_montaje=" & Mont.Id
        End If
        Dim mlin As clsLineaMontaje
        Dim cmd As New SqlCommand(cad, mcon)
        Dim CnnAbierta As Boolean = False
        If mcon.State = ConnectionState.Open Then CnnAbierta = True
        If CnnAbierta = False Then
            mcon.Open()
        End If

        cmd.ExecuteNonQuery()
        'borramos las lineas si existen y las volvemos a crear
        cmd.CommandText = "DELETE FROM t_lineas_pedido_montaje where id_pedido_montaje=" & Mont.Id
        cmd.ExecuteNonQuery()
        For Each mlin In Mont
            cmd.CommandText = "INSERT INTO t_lineas_pedido_montaje (id_pedido_montaje,id_montaje,precio,nota,dto) VALUES (" & Mont.Id & "," & _
            mlin.idMontaje & "," & Replace(mlin.precio, ",", ".") & ",''," & Replace(mlin.Dto, ",", ".") & ")"
            cmd.ExecuteNonQuery()
        Next
        'ahora updateamos los pedidos derecho e izquierdo al montaje
        If Mont.Derecho <> 0 Then
            cmd.CommandText = "Update t_pedidos set montaje=1 where id_pedido=" & Mont.Derecho
            cmd.ExecuteNonQuery()
        End If
        If Mont.Izquierdo <> 0 Then
            cmd.CommandText = "Update t_pedidos set montaje=1 where id_pedido=" & Mont.Izquierdo
            cmd.ExecuteNonQuery()
        End If
        If CnnAbierta = False Then
            mcon.Close()
        End If
    End Sub
    Public Function GetLentesbyProceso(ByVal fecini As Long, ByVal fecfin As Long) As DataTable
        Dim cad As String = "insert into tmp_lentes_procesos  select id_pedido,id_orden,'' as incidencia from t_ordenes_trabajo where paso=0  " & _
        " and id_pedido in (select id_pedido from t_pedidos where fecha>=" & fecini & " and fecha<=" & fecfin & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        mcon.Open()
        'ahora vamos a ver la incidencia
        cad = "UPDATE tmp_lentes_procesos set incidencia=(select incidencia from t_incidencias where id_incidencia=(select id_incidencia from t_ordenes_trabajo where id_orden=tmp_lentes_procesos.id_orden-1 and id_pedido=tmp_lentes_procesos.id_pedido))"
        cmd.CommandText = cad
        cmd.ExecuteNonQuery()
        mcon.Close()
        'AHORA agrupamos por incidencia
        Dim tb As New DataTable
        mcon.Open()
        cad = "select incidencia,count(*) as Lentes from tmp_lentes_procesos"
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        'ahora borramos la vista
        cad = "Delete from tmp_lentes_proceso"
        cmd.CommandText = cad
        cmd.ExecuteNonQuery()
        mcon.Close()
        Return tb
    End Function

    Public Function EsLenteOrganica(ByVal idlente As Integer) As Boolean
        Dim cad As String = "select material from t_modelos where id_lente=" & idlente
        Dim cmd As New SqlCommand(cad, mcon)
        Dim modelo As Integer = 0
        mcon.Open()
        modelo = cmd.ExecuteScalar
        mcon.Close()
        cmd = Nothing
        If modelo = 1 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function EsMineral(ByVal idlente As Integer) As Boolean
        Dim cad As String = "select material from t_modelos where id_lente=" & idlente
        Dim cmd As New SqlCommand(cad, mcon)
        Dim modelo As Integer = 0
        mcon.Open()
        modelo = cmd.ExecuteScalar
        mcon.Close()
        cmd = Nothing
        If modelo = 2 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetGafabyid(ByVal idgafa As Integer) As String
        Dim cad As String = "select gafa from t_tipos_gafa where id_gafa=" & idgafa
        Dim gafa As String = ""
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        gafa = cmd.ExecuteScalar
        mcon.Close()
        cmd = Nothing
        Return gafa
    End Function
    Public Function GetEquipoByUsuario() As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select * from t_equipos where equipo=" & strsql(Equipo) & " and id_usuario=" & mUsuario.id
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetBonosClientesPendientes() As DataTable
        Dim cad As String = "(select fecini as fecha,id_bono,nombre_comercial,ParteA,ParteB,(select sum(base+convert(decimal(8,2),base*iva/100)+convert(decimal(8,2),base*re/100)) from t_bases_albaran where id_albaran in (select id_albaran from t_lineas_bono where envio=1 and id_bono=t_bonos.id_bono)) as consumo from t_bonos INNER JOIN t_clientes ON t_bonos.id_cliente=t_clientes.id_cliente where ParteA<>0 and parteA>(select sum(base+convert(decimal(8,2),base*iva/100)+convert(decimal(8,2),base*re/100)) from t_bases_albaran where id_albaran in (select id_albaran from t_lineas_bono where envio=1 and id_bono=t_bonos.id_bono))) " & _
        " UNION (select fecini as fecha,id_bono,nombre_comercial,ParteA,ParteB,(select sum(base) from t_bases_albaran where id_albaran in (select id_albaran from t_lineas_bono where envio=0 and id_bono=t_bonos.id_bono)) as consumo from t_bonos INNER JOIN t_clientes ON t_bonos.id_cliente=t_clientes.id_cliente where ParteB<>0 and parteB>(select sum(base) from t_bases_albaran where id_albaran in (select id_albaran from t_lineas_bono where envio=0 and id_bono=t_bonos.id_bono))) "
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetBonoClienteByPedido(ByVal p As clsPedido) As clsBonoCliente
        'buscamos el grupo de ese modelo
        Dim idgrupo As Integer = GetGrupoByModelo(p.id_modelo)
        Dim comercial As String = "" 'los comerciales solo pueden ver los bonos de sus clientes
        If mUsuario.Comercial = True Then
            comercial = " and id_cliente in (select id_cliente from t_clientes where id_comercial=" & mUsuario.id & ")"
        End If
        'vamos a ver si un pedido cumple las condiciones de bono, si existe cogemos el bono con la factura mas antigua
        Dim cad As String = "select top 1 t_bonos_cliente.id_bono_cliente from t_bonos_cliente INNER JOIN t_lineas_bono_cliente ON t_bonos_cliente.id_bono_cliente=t_lineas_bono_cliente.id_bono_cliente " & _
        " where id_cliente=" & p.id_cliente & " and consumo<lentes and caducidad>=" & FechaAcadena(p.Fechapedido) & " and id_factura<>0 " & comercial & _
        " and id_bono in (select id_bono from t_bono_modelos where id_grupo=" & idgrupo & ") order by id_factura"
        Dim cmd As New SqlCommand(cad, mcon)
        Dim cnnAbierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            cnnAbierta = True
        Else
            mcon.Open()
        End If
        Dim idBono As Integer = cmd.ExecuteScalar
        If cnnAbierta = False Then
            mcon.Close()
        End If
        Dim b As New clsBonoCliente
        If idBono <> 0 Then

            b = GetBonoCliente(idBono)

        End If
        Return b
    End Function
    Public Function GetBonosClientes(ByVal Fecha As Integer, Optional ByVal Idcliente As Integer = 0, Optional ByVal Caducados As Boolean = False) As DataTable
        Dim Where As String = " where t_lineas_bono_cliente.lentes>consumo"
        If Idcliente <> 0 Then
            Where = " and t_bonos_cliente.id_cliente=" & Idcliente
        End If
        If mUsuario.Comercial = True Then
            Where = Where & " and t_bonos_cliente.id_cliente in (select id_cliente from t_clientes where id_comercial=" & mUsuario.id & ")"
        End If
        Dim cad As String = "select t_bonos_cliente.id_bono_cliente,nombre_comercial,t_plantilla_bono.nombre,t_lineas_bono_cliente.id_bono,t_lineas_bono_cliente.lentes,t_lineas_bono_cliente.precio,t_lineas_bono_cliente.caducidad,consumo " & _
        " from t_clientes INNER JOIN t_bonos_cliente ON t_bonos_cliente.id_cliente=t_clientes.id_cliente INNER JOIN t_lineas_bono_cliente ON t_bonos_cliente.id_bono_cliente=t_lineas_bono_cliente.id_bono_cliente" & _
        " INNER JOIN t_plantilla_bono ON t_plantilla_bono.id_bono=t_lineas_bono_cliente.id_bono" & Where

        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetMontajebyid(ByVal idmontaje As Long) As String
        Dim cad As String = "select montaje from t_montajes where id_montaje=" & idmontaje
        Dim gafa As String = ""
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        gafa = cmd.ExecuteScalar
        mcon.Close()
        cmd = Nothing
        Return gafa
    End Function

    Public Function devuelveCoincidencias(ByVal cad As String) As DataTable
        Dim scad As String = ""
        scad = "select t_lentes_stock.*,t_modelos.nombre, 1 as cantidad from " & _
        "t_lentes_stock inner join t_modelos on t_lentes_stock.id_modelo = t_modelos.id_lente where " & _
        "id_producto in (" & cad & ") order by cilindro,esfera"
        Dim mda As New SqlDataAdapter(New SqlCommand(scad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetIdTratamiento(ByVal nombre As String) As Integer
        Dim cad As String = "select id_tratamiento from t_tratamientos where nombre='" & nombre & "'"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim id As Integer = cmd.ExecuteScalar
        mcon.Close()
        cmd = Nothing
        Return id

    End Function

    Public Function CargaPedidosPendientesProv() As DataTable
        'aqui cargaremos solo los peedidos de proveedor donde existan algo pendiente
        Dim cad As String = "select *,(select nombre from t_modelos where id_lente=t_pedidos_prov.id_modelo) as modelo from t_pedidos_prov where  eliminado=0 and id_pedido in (select id_pedido from t_lineas_pedidos_prov where cantidad-servido>0) order by id_pedido"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        mda.Fill(tb)
        mcon.Close()
        mda.Dispose()
        Return tb

    End Function
    Public Function GetUltimoCodBarraBySemiterminado(ByVal idlente As Integer, Optional ByVal LTL As Boolean = True) As String
        Dim FiltroProveedor As String = ""
        If LTL = True Then
            FiltroProveedor = " and id_proveedor=4 "
        End If
        Dim cad As String = "Select top 1 cod_barras from t_salidas_semiterminados where id_lente=" & idlente & FiltroProveedor & " order by fecha desc"
        Dim codigo As String = ""
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        codigo = cmd.ExecuteScalar
        mcon.Close()
        Return codigo
    End Function
    Public Function GetSalidaVirtualByPedidoYOrden(ByVal pedido As Integer, ByVal orden As Integer) As DataTable
        Dim cad As String = "Select (SELECT ISNULL(INDICE_MODELO,0) FROM T_MODELOS WHERE ID_LENTE=(SELECT ID_MODELO FROM T_SEMITERMINADOS WHERE ID_LENTE=T_SALIDAS_VIRTUAL.ID_LENTE)) AS INDICE,id_usuario,Dbo.TimeStampToFecha(salida) as Fecha,isnull(id_lente,0) as id_lente,isnull(proveedor,'') as proveedor from t_salidas_virtual where id_pedido=" & pedido & " and id_orden=" & orden
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetLoteSemiterminadoByPedido(ByVal p As clsPedido) As String
        Dim cad As String = "Select top 1 isnull(lote,'') from t_salidas_semiterminados where id_pedido=" & p.id & " order by id_orden desc"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        GetLoteSemiterminadoByPedido = cmd.ExecuteScalar
        mcon.Close()
    End Function
    Public Function GetLoteStockBypedido(ByVal p As clsPedido) As String
        Dim cad As String = "Select top 1 isnull(lote,'') from t_salida_stock where id_pedido=" & p.id & " order by orden desc"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        GetLoteStockBypedido = cmd.ExecuteScalar
        mcon.Close()
    End Function
    Public Function GetloteByPedido(ByVal P As clsPedido) As String

        If P.modo = "F" Then
            GetloteByPedido = GetLoteSemiterminadoByPedido(P)
        Else
            GetloteByPedido = GetLoteStockBypedido(P)
            If GetloteByPedido = "" Then
                GetloteByPedido = GetLoteSemiterminadoByPedido(P)
            End If
        End If
    End Function
    Public Function GetLenteStockBypedido(ByVal p As clsPedido) As clsLenteStock

        Dim cad As String = "Select isnull(id_lente,0) from t_salida_stock where id_pedido=" & p.id & " and orden in (select max(id_orden) from t_ordenes_trabajo where id_pedido=" & p.id & ")"
        Dim lente As Integer
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        lente = cmd.ExecuteScalar
        mcon.Close()
        Return GetLenteStockByID(lente)
    End Function
    Public Function GetSalidaLenteStockByPedido(ByVal id As Integer, ByVal orden As Integer) As String

        Dim cad As String = "SELECT isnull(proveedor + ' LOT. ' + LOTE,'') from t_salida_stock  INNER JOIN Proveedores.dbo.t_proveedores ON Proveedores.dbo.t_proveedores.id_proveedor=t_salida_stock.id_proveedor where id_pedido=" & id & " and orden=" & orden
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        GetSalidaLenteStockByPedido = cmd.ExecuteScalar
        mcon.Close()
    End Function
    Public Function GetSalidaSemiterminadoByPedido(ByVal id As Integer, ByVal orden As Integer) As String
        Dim Semi As String = ""
        Dim cad As String = "select (SELECT ISNULL(INDICE_MODELO,0) FROM T_MODELOS WHERE ID_LENTE=T_SEMITERMINADOS.ID_MODELO) AS INDICE,DIAMETRO,base,proveedor,LOTE from t_semiterminados INNER JOIN t_salidas_semiterminados ON t_semiterminados.id_lente=t_salidas_semiterminados.id_lente INNER JOIN Proveedores.dbo.t_proveedores ON Proveedores.dbo.t_proveedores.id_proveedor=t_salidas_semiterminados.id_proveedor where id_pedido=" & id & " and id_orden=" & orden
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        If tb.Rows.Count > 0 Then
            Semi = tb.Rows(0)("INDICE") & " " & tb.Rows(0)("DIAMETRO") & " B. " & tb.Rows(0)("base") & " " & tb.Rows(0)("proveedor") & IIf(tb.Rows(0)("LOTE") = "", "", " LOT. " & tb.Rows(0)("lote"))
        End If
        Return Semi
    End Function

    Public Function CargaPedidosPendientesSemiterminados() As DataTable
        'aqui cargaremos solo los peedidos de proveedor donde existan algo pendiente
        Dim cad As String = "select *,(select Proveedor from Proveedores.dbo.t_proveedores where id_proveedor=Proveedores.dbo.t_pedidos_semiterminados.id_proveedor) as proveedor,(select nombre from t_modelos where id_lente=Proveedores.dbo.t_pedidos_semiterminados.id_modelo) as modelo from Proveedores.dbo.t_pedidos_semiterminados where  eliminado=0 and id_pedido in (select id_pedido from  Proveedores.dbo.t_lineas_pedido_semiterminados where cantidad-servido>0) order by id_pedido"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        mda.Fill(tb)
        mcon.Close()
        mda.Dispose()
        Return tb

    End Function

    Public Sub EliminaPedidoProveedor(ByVal idPedido As Integer)
        Dim cad As String = "update t_pedidos_prov set eliminado=-1 where id_pedido=" & idPedido
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'cmd.CommandText = "DELETE from t_lineas_pedidos_prov where id_pedido=" & idPedido
        'cmd.ExecuteNonQuery()
        cmd.Dispose()
        mcon.Close()
    End Sub

    Public Sub EliminaPedidoProveedorSemiterminado(ByVal idPedido As Integer)
        Dim cad As String = "update Proveedores.dbo.t_pedidos_semiterminados set eliminado=-1 where id_pedido=" & idPedido
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'cmd.CommandText = "DELETE from t_lineas_pedidos_prov where id_pedido=" & idPedido
        'cmd.ExecuteNonQuery()
        cmd.Dispose()
        mcon.Close()
    End Sub

    Public Function UltimafechaFacturaBono(ByVal año As Integer) As Long
        Dim cad As String = "select case when max(fecha) is null then 20070101 else max(fecha) end from t_factura_bono where fecha>=" & año * 10000 & " and fecha<=" & (año * 10000) + 1231
        Dim cmd As New SqlCommand(cad, mcon)
        Dim b As Long
        mcon.Open()
        b = cmd.ExecuteScalar()
        mcon.Close()
        Return b
    End Function
    Public Function CargaBonosCliente(ByVal idcli As Integer) As DataTable
        Dim cad As String = "select * from t_bonos where id_cliente=" & idcli
        Dim dta As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        dta.Fill(tb)
        mcon.Close()
        dta = Nothing
        Return tb
    End Function
    'Public Function CargaFacturaBono(ByVal idbono As Integer) As DataTable
    '    Dim Cad As String = "select id_albaran,total from t_lineas_factura_bono"
    'End Function

    Public Function ListadoPortes(ByVal fecini As Long, ByVal fecfin As Long, Optional ByVal Agencia As String = "") As DataTable
        'Dim TipoAgencia As String
        If Agencia <> "" Then
            Agencia = " and agencia like '" & Agencia & "'"
        End If
        Dim cad As String = "select convert(varchar(20),(cod_bolsa))+ ' .' as [cod Barras],agencia,t_clientes.codigo,t_clientes.nombre_comercial,direccion,poblacion,importe as [ContraReemb.],(select provincia from m_provincias where id_provincia=t_clientes.id_Provincia) as [Provincia Cliente]" & _
        ",left(right(fecha,4),2) +'/' + right(fecha,2) +'/' + left(fecha,4) as [Fecha Porte], cod_nuestro as [cod Interno] from t_clientes INNER JOIN t_portes ON " & _
        " t_portes.id_cliente=t_clientes.id_cliente Where fecha>=" & fecini & " and fecha<=" & fecfin & Agencia & " order by cod_nuestro,fecha"
        Dim dta As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        dta.Fill(tb)
        mcon.Close()
        dta = Nothing
        Return tb
    End Function
    Public Function ListadoPortesChrono(ByVal fecini As Long, ByVal fecfin As Long, Optional ByVal Agencia As String = "") As DataTable
        'Dim TipoAgencia As String
        If Agencia <> "" Then
            Agencia = " and agencia like '" & Agencia & "'"
        End If
        Dim cad As String = "select convert(nvarchar(20),(cod_bolsa)) as [albaran],t_clientes.nombre_comercial as destinatario,direccion as [direccion destinatario],cp as [c.p destinatario],poblacion as [poblacion destinatario],case contrareembolso when 0 then 0 else case cobrar_portes when 0 then 0 else (select porte*1.18 from t_valores_globales) end + (select sum(total) from t_contrareembolsos where id_cliente=t_clientes.id_cliente and id_albaran in (select id_albaran from t_lineas_porte where id_porte=t_portes.id_porte)) end as [reembolso],'' as [entrega sabado],'' as observaciones,'' as producto " & _
        " from t_clientes INNER JOIN t_portes ON " & _
        " t_portes.id_cliente=t_clientes.id_cliente Where fecha>=" & fecini & " and fecha<=" & fecfin & Agencia & " order by cod_nuestro,fecha"
        Dim dta As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        dta.Fill(tb)
        mcon.Close()
        dta = Nothing
        Return tb
    End Function
    Public Function PorteChrono(ByVal idporte As Long) As DataTable

        Dim cad As String = "select id_porte as [referencia],t_clientes.nombre_comercial as destinatario,direccion as [direccion destinatario],contacto,cp as [c.p destinatario],telefono,poblacion,'P' as M_TIPOPRT,77 as M_TIPO_AL,1 as bulto, 1 as peso,importe as [reembolso],'' as [cod_pais],'' as observaciones,'' as observaciones2 " & _
        " from t_clientes INNER JOIN t_portes ON t_portes.id_cliente=t_clientes.id_cliente where id_porte=" & idporte
        Dim dta As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        dta.Fill(tb)
        mcon.Close()
        dta = Nothing
        Return tb
    End Function
    Public Function PorteSending(ByVal idporte As Long) As DataTable

        Dim cad As String = "select id_porte as [referencia],t_clientes.nombre_comercial as destinatario,direccion as [direccion destinatario],contacto,cp as [c.p destinatario],telefono,poblacion,'P' as M_TIPOPRT,77 as M_TIPO_AL,1 as bulto, 1 as peso,importe as [reembolso],'' as [cod_pais],'' as observaciones,'' as observaciones2 " & _
        " from t_clientes INNER JOIN t_portes ON t_portes.id_cliente=t_clientes.id_cliente where id_porte=" & idporte
        Dim dta As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        dta.Fill(tb)
        mcon.Close()
        dta = Nothing
        Return tb
    End Function
    Public Function PorteCourier(ByVal idporte As Long) As DataTable

        Dim cad As String = "select id_porte as [referencia],t_clientes.nombre_comercial as destinatario,direccion as [direccion destinatario],contacto,'C'+cp as [c.p destinatario],telefono,poblacion,'P' as M_TIPOPRT,77 as M_TIPO_AL,1 as bulto, 1 as peso,importe as [reembolso],'' as [cod_pais],'' as observaciones,'' as observaciones2 " & _
        " from t_clientes INNER JOIN t_portes ON t_portes.id_cliente=t_clientes.id_cliente where id_porte=" & idporte
        Dim dta As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        dta.Fill(tb)
        mcon.Close()
        dta = Nothing
        Return tb
    End Function
    Public Function PorteDiarioSending(ByVal Fecha As Long) As DataTable

        Dim cad As String = "select id_porte as [referencia],t_clientes.nombre_comercial as destinatario,direccion as [direccion destinatario],contacto,cp as [c.p destinatario],telefono,poblacion,'P' as M_TIPOPRT,77 as M_TIPO_AL,1 as bulto, 1 as peso,case contrareembolso when 0 then 0 else (select sum(convert (decimal(8,2),base + base*iva/100+base*re/100)) from t_bases_albaran where id_albaran in " & _
        "(select id_albaran from t_lineas_porte where id_porte=t_portes.id_porte)) end as [reembolso],'' as [cod_pais],'' as observaciones,'' as observaciones2 " & _
        " from t_clientes INNER JOIN t_portes ON t_portes.id_cliente=t_clientes.id_cliente where agencia like 'sending' and fecha=" & Fecha
        Dim dta As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        dta.Fill(tb)
        mcon.Close()
        dta = Nothing
        Return tb
    End Function
    Public Function GetToleranciaEsfera(ByVal esfera As Decimal) As Decimal
        Dim cad As String = "select top 1 isnull(tolerancia,0) from t_tolerancia_potencia where " & NumSql(esfera) & "<= potencia_maxima order by potencia_maxima"
        Dim cmd As New SqlCommand(cad, mcon)
        Dim Tolerancia As Decimal
        mcon.Open()
        Tolerancia = cmd.ExecuteScalar
        mcon.Close()
        If IsNothing(Tolerancia) Then
            Return 0
        Else
            Return Tolerancia
        End If
    End Function
    Public Function GetToleranciaCyl(ByVal esfera As Decimal, ByVal cilindro As Decimal) As Decimal
        Dim cad As String = "select top 1 isnull(tolerancia,0) from t_tolerancia_cilindro where " & NumSql(esfera) & "<= potencia_maxima and cilindro_maximo>=" & NumSql(cilindro) & " order by potencia_maxima,cilindro_maximo"
        Dim cmd As New SqlCommand(cad, mcon)
        Dim Tolerancia As Decimal
        mcon.Open()
        Tolerancia = cmd.ExecuteScalar
        mcon.Close()
        If IsNothing(Tolerancia) Then
            Return 0
        Else
            Return Tolerancia
        End If
    End Function
    Public Function GetToleranciaEje(ByVal cilindro As Decimal) As Decimal
        Dim cad As String = "select top 1 isnull(tolerancia,0) from t_tolerancia_eje where cilindro_maximo>=" & NumSql(cilindro) & " order by cilindro_maximo"
        Dim cmd As New SqlCommand(cad, mcon)
        Dim Tolerancia As Decimal
        mcon.Open()
        Tolerancia = cmd.ExecuteScalar
        mcon.Close()
        If IsNothing(Tolerancia) Then
            Return 0
        Else
            Return Tolerancia
        End If
    End Function
    Public Function GetToleranciaAdicion(ByVal Adicion As Decimal) As Decimal
        Dim cad As String = "select top 1 isnull(tolerancia,0) from t_tolerancia_adicion where adicion_maxima>=" & NumSql(Adicion) & " order by adicion_maxima"
        Dim cmd As New SqlCommand(cad, mcon)
        Dim Tolerancia As Decimal
        mcon.Open()
        Tolerancia = cmd.ExecuteScalar
        mcon.Close()
        If IsNothing(Tolerancia) Then
            Return 0
        Else
            Return Tolerancia
        End If
    End Function

    Public Function ListadoPortesSending(ByVal fecini As Long, ByVal fecfin As Long, Optional ByVal Agencia As String = "") As DataTable
        ' Dim TipoAgencia As String
        If Agencia <> "" Then
            Agencia = " and agencia like '%sending%'"
        End If
        Dim cad As String = "select convert(nvarchar(20),(cod_bolsa)) as [albaran],t_clientes.nombre_comercial as destinatario,direccion as [direccion destinatario],cp as [c.p destinatario],poblacion as [poblacion destinatario],case contrareembolso when 0 then 0 else  (select sum(total) from t_contrareembolsos where id_cliente=t_clientes.id_cliente and id_albaran in (select id_albaran from t_lineas_porte where id_porte=t_portes.id_porte)) end as [reembolso]" & _
        " from t_clientes INNER JOIN t_portes ON " & _
        " t_portes.id_cliente=t_clientes.id_cliente Where fecha>=" & fecini & " and fecha<=" & fecfin & Agencia & " order by cod_nuestro,fecha"
        Dim dta As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        dta.Fill(tb)
        mcon.Close()
        dta = Nothing
        Return tb
    End Function
    Public Function ListadoTtoExterno(ByVal Fecha As Integer, Optional ByVal Salida As Boolean = True) As DataTable
        Dim Condicion As String
        If Salida = True Then
            Condicion = " where fec_salida=" & Fecha
        Else
            Condicion = " where fec_entrada=" & Fecha
        End If
        Dim Cad As String = "select t_pedidos.id_pedido as pedido,(select nombre from t_modelos where id_lente=t_pedidos.id_modelo) as lente," & _
        "(select nombre from t_tratamientos where id_tratamiento=t_pedidos.id_tratamiento) as tratamiento, (select nombre_comercial from t_clientes where id_cliente=t_pedidos.id_cliente) as cliente,right(t_tratamiento_externo.fec_salida,2) + '/'+" & _
           "left(right(t_tratamiento_externo.fec_salida,4),2) +'/'+ left(t_tratamiento_externo.fec_salida,4) as Salida,left(convert(char(4),t_tratamiento_externo.hora_salida),2) + ':' + right(t_tratamiento_externo.hora_salida,2) as [hora Salida]," & _
           " case t_tratamiento_externo.fec_entrada when 0 then '' else right(t_tratamiento_externo.fec_entrada,2) + '/' + left(right(t_tratamiento_externo.fec_entrada,4),2) + '/'+left(t_tratamiento_externo.fec_entrada,4) end  as Entrada," & _
           " case t_tratamiento_externo.hora_entrada when 0 then '' else left(convert(char(4),t_tratamiento_externo.hora_entrada),2) + ':' + right( t_tratamiento_externo.hora_entrada,2) end as [Hora Entrada] from t_tratamiento_externo INNER JOIN t_pedidos " & _
           " ON t_tratamiento_externo.id_pedido=t_pedidos.id_pedido" & Condicion
        Dim mda As New SqlDataAdapter(New SqlCommand(Cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        mda.Fill(tb)
        mcon.Close()
        mda = Nothing
        Return tb

    End Function
    Public Function FechaSalidaTtoExterno(ByVal idPedido As Integer, ByVal Fecha As Long) As Boolean
        'primero comprobamos que no este ya metido
        Dim cad As String = "select count(*) from t_tratamiento_externo where id_pedido=" & idPedido & " and fec_salida=" & Fecha
        Dim cmd1 As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim i As Integer = cmd1.ExecuteScalar
        mcon.Close()
        cmd1 = Nothing
        If i > 0 Then Return False
        cad = "INSERT INTO t_tratamiento_externo (id_pedido,fec_salida,hora_salida,fec_entrada,hora_entrada)" & _
        " VALUES (" & idPedido & "," & Fecha & ",0,0,0)"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        Return True
    End Function
    Public Sub HoraSalidaTtoExterno(ByVal idPedido As Integer, ByVal fecha As Long, ByVal Hora As Long)

        Dim cad As String = "Update t_tratamiento_externo set fec_salida=" & fecha & ", hora_salida = " & _
        Hora & " where id_pedido=" & idPedido
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Function GetUltimoCodBarraPorte() As Integer
        Dim cad As String = "select case when max(cod_nuestro) is null then 1 else max(cod_nuestro) +1 end from t_portes"

        Dim cmd As New SqlCommand(cad, mcon)
        Dim Num As Integer
        Dim control As Integer
        mcon.Open()
        Num = cmd.ExecuteScalar
        mcon.Close()
        Dim CodBar As New clsCodigoBarras
        control = CodBar.DigitoControl(Format(Num, "000000000000"))
        Return Num & control
    End Function
    Public Function ExisteAlbaranMontaje(ByVal idpedido As Integer) As Boolean
        Dim cad As String = "select count(*) from t_albaranes where montaje<>0 and id_pedido=" & idpedido
        Dim cmd As New SqlCommand(cad, mcon)
        Dim existe As Integer = 0
        mcon.Open()
        existe = cmd.ExecuteScalar
        mcon.Close()
        If existe <> 0 Then
            Return True
        Else
            Return False
        End If


    End Function
    Public Sub FechaEntradaTtoExterno(ByVal idPedido As Integer, ByVal Fecha As Long, ByVal hora As Integer)

        Dim cad As String = "Update t_tratamiento_externo set Fec_entrada=" & _
        Fecha & ",hora_entrada=" & Format(hora, "0000") & " where id_pedido=" & idPedido
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()

        'ahora actualizamos la fecha de salida de tratamiento
        cmd.CommandText = "UPDATE t_pedidos set f_salida_tratamiento=" & Fecha & ",h_salida_tratamiento='" & Format(hora, "0000") & "' where id_pedido=" & idPedido
        cmd.ExecuteNonQuery()
        cmd = Nothing

        mcon.Close()

    End Sub

    Public Function PrecioPorte() As Single
        'en esta funcion daremos el precio de porte cuando exista en la base de datos, de momento lo ponemos a huevo 
        'y que sea lo que dios quiere
        Dim cad As String = " select porte from t_valores_globales"
        Dim cmd As New SqlCommand(cad, mcon)
        Dim precio As Decimal
        mcon.Open()
        precio = cmd.ExecuteScalar
        mcon.Close()
        cmd = Nothing
        Return precio
        'Return 2.65
    End Function
    'Private con2 As String = "data source=localhost;initial catalog=optica;integrated security=SSPI;persist security info=False;user id=sa;workstation id=PORTATIL;packet size=4096"
    'Private mcon As New SqlConnection(con2)
    Public Function BuscaAlbaranesParaPortes(ByVal idcli As Integer, ByVal fecha As Integer, Optional ByVal Contrareembolso As Boolean = False, Optional ByVal re As Boolean = False) As DataTable
        'buscamos los albaranes de ese cliente, cuya salida a distribucion sea con fecha de hoy, y que no hayan salido en ningun porte
        Dim Total As String
        If Contrareembolso = False Then
            Total = "total"
        Else
            Total = "(select sum(base+convert(decimal (8,2),base*iva/100)+convert(decimal (8,2),base*re/100)) from t_bases_albaran where id_albaran=t_albaranes.id_albaran) as total"

        End If
        Dim cad As String = "select t_albaranes.id_albaran," & Total & " from t_albaranes  where  id_cliente=" & idcli & " and fecha<=" & fecha & " and fecha>=20101001 and t_albaranes.id_albaran not in (select id_albaran from t_lineas_porte)  ORDER BY t_albaranes.id_albaran"
        Dim mdat As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mdat.SelectCommand.CommandTimeout = 180
        mdat.Fill(tb)
        mcon.Close()
        mdat = Nothing
        '  Dim idalb As Long
        'ahora deberiamos quitar los albaranes que estan repetidos
        '  Dim MRow As Integer
        'For MRow = tb.Rows.Count - 1 To 0 Step -1
        '    If idalb = tb.Rows(MRow)("id_albaran") Then
        '        tb.Rows.RemoveAt(MRow)
        '    Else
        '        idalb = tb.Rows(MRow)("id_albaran")
        '    End If

        'Next
        Return tb
    End Function
    Public Function GetDescripcionByidAlbaran(ByVal id As Integer) As String
        Dim cad As String = "select top 1 (descripcion) from t_lineas_albaran where id_albaran=" & id
        Dim des As String
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        des = cmd.ExecuteScalar
        mcon.Close()
        Return des
    End Function
    Public Function GetlentesByidAlbaran(ByVal id As Integer) As Integer
        Dim cad As String = "select count(*) from t_lineas_albaran where id_tipo_producto=1 and id_modelo<> 0 and id_albaran=" & id & " and id_albaran  in (select id_albaran from t_albaranes where id_alb_abono=0)"
        Dim lentes As Integer
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        lentes = cmd.ExecuteScalar
        mcon.Close()
        Return lentes
    End Function
    Public Function NuevoAlbaranPorte(ByVal numero As Integer) As DataTable
        Dim cad As String = "select t_albaranes.id_albaran,id_cliente, t_albaranes.total from t_albaranes  where t_albaranes.id_albaran=" & numero & _
        " and t_albaranes.id_albaran not in (select id_albaran from t_lineas_porte)"
        Dim mdat As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mdat.Fill(tb)
        mcon.Close()
        mdat = Nothing
        Return tb
    End Function
    Public Function GetmaxidPorte() As Long
        Dim Porte As Long
        Dim cad As String = "select case when  max(id_porte)is null then 1 else max(id_porte)+1 end from t_portes"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Porte = cmd.ExecuteScalar
        mcon.Close()
        Return Porte
    End Function
    Public Function GrabaPortes(ByVal idcli As Integer, ByVal fecha As Integer, ByVal hora As Integer, ByVal porte As Boolean, ByVal codbolsa As Long, ByVal codNuestro As String, ByVal importe As Single, ByVal agencia As String) As Long
        'grabamos el porte y devolvemos su idPorte
        Dim idporte As Long = GetmaxidPorte()
        'ahora grabamos los datos en la tabla
        Dim cad As String = "INSERT INTO t_portes (id_porte,id_cliente,fecha,hora,cod_bolsa,porte,precio,cod_nuestro,cod_control,agencia,id_usuario) VALUES (" & _
        idporte & "," & idcli & "," & fecha & "," & hora & "," & codbolsa & "," & IIf(porte = False, 0, 1) & "," & Replace(importe, ",", ".") & _
        ",0,0,'" & agencia & "'," & mUsuario.id & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()

        mcon.Close()
        cmd = Nothing
        Return idporte
    End Function
    Public Sub GrabaDebeHaber(ByVal idCLi As Integer, ByVal idPorte As Integer)
        Dim Importe As Decimal
        Dim cad As String = "Select sum(convert(decimal(8,2),base+ base*iva/100+ base*re/100)) from t_bases_albaran where id_albaran in (select id_albaran from t_lineas_porte where id_porte=" & idPorte & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Importe = cmd.ExecuteScalar
        'cmd.CommandText = "INSERT INTO t_debe_haber (id_cliente,fecha,concepto,debe,haber) VALUES (" & idCLi & "," & FechaAcadena(Now.Date) & ",'Porte " & idPorte & "'," & NumSql(Format(Importe, "0.00")) & ",0)"
        'cmd.ExecuteNonQuery()
        'mcon.Close()
    End Sub
    Public Function GetDeudas(Optional ByVal Pendiente As Boolean = True) As DataTable
        ' Dim cad As String = "select t_deudas_cliente.*,nombre_comercial from t_deudas_cliente INNER JOIN t_clientes ON t_deudas_cliente.id_cliente=t_clientes.id_cliente"
        ' If Pendiente = True Then
        '  cad = cad & " where pendiente<>0"
        ' End If
        'cad = cad & " order by id_deuda"
        'Dim tb As New DataTable
        ' Dim mda As New SqlDataAdapter(cad, mcon)
        ' mda.Fill(tb)
        ' Return tb
    End Function
    Public Sub GrabaDeuda(ByVal d As clsDeuda)
        ' Dim cad As String
        'Dim Diferencia As Decimal = 0

        'If d.IdDeuda = 0 Then
        'd.IdDeuda = getMaxId("id_deuda", "t_deudas_cliente") + 1
        ' cad = "INSERT INTO t_deudas_cliente (id_deuda,id_cliente,fecha,deuda,pendiente,cobro) VALUES (" & d.IdDeuda & "," & d.IdCliente & "," & d.Fecha & "," & NumSql(d.Deuda) & "," & NumSql(d.Pendiente) & "," & NumSql(d.Cobro) & ")"
        ' Else
        ' Dim cmd As New SqlCommand("select deuda from t_deudas_cliente where id_deuda=" & d.IdDeuda, mcon)
        ''''''' 'vemos la diferencia entre la deuda antigua y la actualizada
        ' mcon.Open()
        ' Dim DeudaAntigua As Decimal = cmd.ExecuteScalar
        ' mcon.Close()
        ' Diferencia = d.Deuda - DeudaAntigua
        ' d.Pendiente = d.Pendiente + Diferencia
        ' cad = "UPDATE t_deudas_cliente set deuda=" & NumSql(d.Deuda) & ",pendiente=" & NumSql(d.Pendiente) & ",cobro=" & NumSql(d.Cobro) & " where id_deuda=" & d.IdDeuda
        ' End If
        ' Dim cmdDeuda As New SqlCommand(cad, mcon)
        ' mcon.Open()
        ' cmdDeuda.ExecuteNonQuery()
        ' mcon.Close()
    End Sub
    Public Sub GrabaLIneasPorte(ByVal idporte As Integer, ByVal idalbaran As Integer, ByVal coste As Decimal)

        Dim cad As String = " INSERT INTO t_lineas_porte (id_porte,id_albaran) VALUES (" & idporte & "," & idalbaran & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'ahora vemos si tiene contrareembolso y actualizamos el contrareembolso del porte
        cmd.CommandText = "UPDATE t_portes set importe=importe+isnull((select total from t_contrareembolsos where id_albaran=" & idalbaran & "),0) where id_porte=" & idporte
        cmd.ExecuteNonQuery()
        'ahora grabamos el gasto del transporte en los pedidos
        If coste <> 0 Then
            Dim mda As New SqlDataAdapter("select distinct(id_pedido) as pedido from t_lineas_albaran where id_tipo_producto=1 and id_albaran=" & idalbaran, mcon)
            Dim tb As New DataTable
            mda.Fill(tb)
            'vamos a meterle el coste en cada pedido
            For Each rw As DataRow In tb.Rows
                cmd.CommandText = "INSERT INTO t_costos_pedido (id_pedido,paso,coste) VALUES (" & rw("pedido") & ",'Mensajeria'," & NumSql(coste) & ")"
                cmd.ExecuteNonQuery()
            Next
        End If
        mcon.Close()
        cmd = Nothing

    End Sub
    Public Function GetCantidadLentesStock(ByVal idModelo As Integer, ByVal diametro As Integer, ByVal cilindro As Decimal, ByVal esfera As Decimal, ByVal tratamiento As Integer) As Integer
        Dim cil As String = Replace(cilindro, ",", ".")
        Dim esf As String = Replace(esfera, ",", ".")

        Dim cad As String
        cad = "select stock from t_lentes_stock where id_modelo=" & idModelo & " and diametro=" & diametro & " and cilindro=" & cil & " and esfera=" & esf & " and tratamiento=" & tratamiento
        Dim cmd As New SqlCommand(cad, mcon)
        Dim Total As Integer
        mcon.Open()
        Total = cmd.ExecuteScalar
        mcon.Close()
        Return Total

    End Function
    Public Function CuentaFechasSalida() As Integer

        Dim FecFin As Date = Now
        Dim FecIni As Date = DateAdd(DateInterval.Year, -1, Now)
        Dim cad As String = "select count(distinct(fecha)) as dias from t_movimientos_almacen where entrada=0 and fecha>=" & FechaAcadena(FecIni) & " and fecha<=" & FechaAcadena(FecFin)
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("dias")
    End Function
    Public Function CuentaFechasSalidaSemiterminado() As Integer

        Dim FecFin As Date = Now
        Dim FecIni As Date = DateAdd(DateInterval.Year, -1, Now)
        Dim cad As String = "select count(distinct(fecha)) as dias from t_salidas_semiterminados where entrada=0 and fecha>=" & FechaAcadena(FecIni) & " and fecha<=" & FechaAcadena(FecFin)
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("dias")
    End Function
    Public Sub ReseteaPassword(ByVal idcli As Integer)
        Dim cad As String = "UPdate t_clientes_web set clave='123',cambio_clave=1 where id_cliente=" & idcli
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        MsgBox("Se ha cambiado la contraseña del cliente a 123." & vbNewLine & "La primera vez que el cliente se valide de nuevo le solicitará el cambio de contraseña")
    End Sub
    Public Sub SetStockCriticoyMinimo(Optional ByVal Minimo As Integer = 20, Optional ByVal Critico As Integer = 15, Optional ByVal Lentes As Integer = 2)
        'la formula utilizada sera numero de lentes de salida en el periodo de una año atras/numero de dias de salida* (Minimo o Critico)
        Dim cad As String = "select id_producto,(select case when sum(cantidad) is null  then 0 else sum(cantidad) end from t_movimientos_almacen where entrada=0 " & _
        " AND FECHA>=" & FechaAcadena(DateAdd(DateInterval.Year, -1, Now)) & " and fecha<=" & FechaAcadena(Now) & _
        " and id_producto=t_lentes_stock.id_producto) as lentes from t_lentes_stock "

        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        'ahora vamos a crear las variables StockM y StockC
        Dim stockM As Decimal
        Dim StockC As Decimal
        Dim Dias As Integer
        Dias = CuentaFechasSalida()
        Dim i As Integer
        Dim Cmd As New SqlCommand
        Cmd.Connection = mcon
        For i = 0 To tb.Rows.Count - 1

            stockM = tb.Rows(i)("lentes") * Minimo / Dias
            StockC = tb.Rows(i)("lentes") * Critico / Dias
            If stockM < Lentes Then
                stockM = Lentes
            End If
            If StockC > Lentes Then
                StockC = Lentes
            End If
            'ahora vemos si es entero o no, si no lo es lo igualamos al siguiente entero
            If stockM Mod 1 <> 0 Then
                
                stockM = Int(stockM) + 1

            End If
            If StockC Mod 1 <> 0 Then
               
                StockC = Int(StockC + 1)

            End If
            ' si alguno de los dos es cero, lo hacemos 1
            'If StockC = 0 Then StockC = 1
            ' If stockM = 0 Then stockM = 1
            'ahora updateamos el stock

            mcon.Open()
            Cmd.CommandText = " UPDATE t_lentes_stock set stock_minimo=" & stockM & ", stock_critico=" & StockC & " where id_producto=" & tb.Rows(i)("id_producto")
            Cmd.ExecuteNonQuery()
            mcon.Close()

        Next
        Cmd = Nothing
    End Sub
    Public Sub SetStockCriticoyMinimoSemiterminado(Optional ByVal Minimo As Integer = 20, Optional ByVal Critico As Integer = 15, Optional ByVal Lentes As Integer = 2)
        'la formula utilizada sera numero de lentes de salida en el periodo de una año atras/numero de dias de salida* (Minimo o Critico)
        Dim cad As String = "select id_lente,count(*) as lentes   from t_salidas_semiterminados " & _
        " where FECHA>=" & FechaAcadena(DateAdd(DateInterval.Year, -1, Now)) & " and fecha<=" & FechaAcadena(Now) & _
        " GROUP BY id_lente"

        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        'ahora vamos a crear las variables StockM y StockC
        Dim stockM As Decimal
        Dim StockC As Decimal
        Dim Dias As Integer
        Dias = CuentaFechasSalidaSemiterminado()
        Dim i As Integer
        Dim Cmd As New SqlCommand
        Cmd.Connection = mcon
        For i = 0 To tb.Rows.Count - 1

            stockM = tb.Rows(i)("lentes") * Minimo / Dias
            StockC = tb.Rows(i)("lentes") * Critico / Dias
            If stockM < Lentes Then
                stockM = Lentes
            End If
            If StockC > Lentes Then
                StockC = Lentes
            End If
            'ahora vemos si es entero o no, si no lo es lo igualamos al siguiente entero
            If stockM Mod 1 <> 0 Then

                stockM = Int(stockM) + 1

            End If
            If StockC Mod 1 <> 0 Then

                StockC = Int(StockC + 1)

            End If
            ' si alguno de los dos es cero, lo hacemos 1
            'If StockC = 0 Then StockC = 1
            ' If stockM = 0 Then stockM = 1
            'ahora updateamos el stock

            mcon.Open()
            Cmd.CommandText = " UPDATE t_semiterminados set stock_minimo=" & stockM & ", stock_critico=" & StockC & " where id_lente=" & tb.Rows(i)("id_lente")
            Cmd.ExecuteNonQuery()
            mcon.Close()

        Next
        Cmd = Nothing
    End Sub

    Public Function validacion(ByVal log As String, ByVal clave As String) As clsUsuario
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand("select id_usuario from t_usuarios where login='" & log & "' and clave = '" & clave & "'"))
        Dim us As New clsUsuario
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            us = Nothing
        Else
            us = getUsuarioById(tb.Rows(0)("id_usuario"))
        End If

        Return us
    End Function

    Public Function GetUsuarios() As DataTable
        Dim cad As String = "select * from t_usuarios order by apellidos"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function getProvincias() As DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand("select * from m_provincias order by provincia"))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function getcomerciales() As DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand("select id_usuario as id_comercial,(nombre + ' '+ apellidos) as comercial from t_usuarios where comercial=1 order by apellidos"))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function getLocalidades(ByVal idProvincia As Long) As DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand("select * from m_localidades where id_provincia=" & idProvincia))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetAbonoPortesByCliente(ByVal Cliente As Integer, ByVal Fabrica As Decimal, ByVal inicio As Integer, ByVal fin As Integer) As Decimal
        'Tenemos que ver si la facturacion en ese periodo de fechas ha superado la cantidad asignada
        Dim Portes As Decimal
        Dim cad As String = "select isnull(isnull(sum(t_lineas_albaran.total),0)-" & NumSql(Fabrica) & ",-1) from t_lineas_albaran INNER JOIN t_pedidos ON t_lineas_albaran.id_pedido=t_pedidos.id_pedido INNER JOIN t_albaranes ON t_albaranes.id_albaran= t_lineas_albaran.id_albaran where t_albaranes.id_cliente=" & Cliente & " and t_albaranes.fecha>=" & inicio & " and t_albaranes.fecha<=" & fin & " and modo='F' and T_lineas_albaran.montaje=0"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Portes = cmd.ExecuteScalar
        If Portes >= 0 Then
            cmd.CommandText = "Select isnull(sum(base),0) from t_bases_albaran INNER JOIN t_albaranes ON t_albaranes.id_albaran=t_bases_albaran.id_albaran where id_cliente=" & Cliente & " and fecha>=" & inicio & " and fecha<=" & fin & " and id_tipo_base=0"
            Portes = cmd.ExecuteScalar
        End If
        mcon.Close()
        Return Portes
    End Function
    Public Function GetSinColorByModelo(ByVal idmodelo As Integer) As DataTable
        Dim cad As String = "select id_color from t_modelos_sin_color where id_modelo=" & idmodelo
        Dim tb As New DataTable
        Dim cmd As New SqlDataAdapter(cad, mcon)
        cmd.Fill(tb)
        Return tb
    End Function
    Public Function ModeloSinColor(ByVal idmodelo As Integer, ByVal idcolor As Integer) As Boolean
        Dim cad As String = "select count(*) from t_modelos_sin_color where (id_modelo=" & idmodelo & " or id_modelo in (select id_modelo_iot from t_modelos where id_lente=" & idmodelo & ")) and id_color=" & idcolor
        Dim existe As Boolean
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        existe = cmd.ExecuteScalar
        mcon.Close()
        Return existe
    End Function
    Public Sub GrabaModeloSinColor(ByVal idModelo As Integer, ByVal Colores As Array)
        Dim cad As String = "DELETE FROM t_modelos_sin_color where id_modelo=" & idModelo
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        For Each color As Integer In Colores
            If color <> 0 Then
                cmd.CommandText = "INSERT INTO t_modelos_sin_color (id_modelo,id_color) VALUES (" & idModelo & "," & color & ")"
                cmd.ExecuteNonQuery()
            End If
        Next
        mcon.Close()
    End Sub
    Public Function getUsuarioById(ByVal idUsuario As Long) As clsUsuario
        Dim tb As New DataTable
        Dim cad As String = "select * from t_usuarios where id_usuario=" & idUsuario
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim us As New clsUsuario
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        If tb.Rows.Count = 0 Then
            us.apellidos = "Pedido web Automatico"
        Else
            us.apellidos = tb.Rows(0).Item("apellidos")
5:          us.clave = tb.Rows(0).Item("clave")
            us.id = tb.Rows(0).Item("id_usuario")
            us.login = tb.Rows(0).Item("login")
            us.nombre = tb.Rows(0).Item("nombre")
            us.Comercial = CBool(tb.Rows(0)("comercial"))
            us.CambiarClave = tb.Rows(0).Item("cambio_clave")
            us.Baja = tb.Rows(0).Item("Baja")
            'us.perfil = tb.Rows(0).Item("perfil")
        End If
        tb.Clear()
        mda.SelectCommand.CommandText = "Select * from t_permisos_usuarios where id_usuario=" & us.id
        mda.Fill(tb)
        'ahora vamos a ir rellenando los accesos

        For Each rw As DataRow In tb.Rows
            With (us.Acceso)
                Select Case (rw("acceso"))
                    Case "Pedidos"
                        .Pedidos.Crear = rw("crear")
                        .Pedidos.Consultar = rw("consultar")
                        .Pedidos.Anular = rw("anular")
                        .Pedidos.Eliminar = rw("eliminar")
                        .Pedidos.Modificar = rw("modificar")
                    Case "Albaranes"
                        .Albaranes.Crear = CBool(rw("crear"))
                        .Albaranes.Consultar = CBool(rw("consultar"))
                        .Albaranes.Anular = CBool(rw("anular"))
                        .Albaranes.Eliminar = CBool(rw("eliminar"))
                        .Albaranes.Modificar = CBool(rw("modificar"))
                    Case "Facturas"
                        .Facturas.Crear = rw("crear")
                        .Facturas.Consultar = rw("consultar")
                        .Facturas.Anular = rw("anular")
                        .Facturas.Eliminar = rw("eliminar")
                        .Facturas.Modificar = rw("modificar")
                    Case "Usuarios"
                        .Usuarios.Crear = rw("crear")
                        .Usuarios.Consultar = rw("consultar")
                        .Usuarios.Anular = rw("anular")
                        .Usuarios.Eliminar = rw("eliminar")
                        .Usuarios.Modificar = rw("modificar")
                    Case "Clientes"
                        .Clientes.Crear = rw("crear")
                        .Clientes.Consultar = rw("consultar")
                        .Clientes.Anular = rw("anular")
                        .Clientes.Eliminar = rw("eliminar")
                        .Clientes.Modificar = rw("modificar")
                        '    Case "Modelos"
                        '        '.Articulos.Crear = rw("crear")
                        '        '.Articulos.Consultar = rw("consultar")
                        '        '.Articulos.Anular = rw("anular")
                        '        '.Articulos.Eliminar = rw("eliminar")
                        '        '.Articulos.Modificar = rw("modificar")
                    Case "Abono"
                        .Abono.Crear = rw("crear")
                        .Abono.Consultar = rw("consultar")
                        .Abono.Anular = rw("anular")
                        .Abono.Eliminar = rw("eliminar")
                        .Abono.Modificar = rw("modificar")
                    Case "Tarifas"
                        .Tarifas.Crear = rw("crear")
                        .Tarifas.Consultar = rw("consultar")
                        .Tarifas.Anular = rw("anular")
                        .Tarifas.Eliminar = rw("eliminar")
                        .Tarifas.Modificar = rw("modificar")
                    Case "Portes"
                        .Portes.Crear = rw("crear")
                        .Portes.Consultar = rw("consultar")
                        .Portes.Anular = rw("anular")
                        .Portes.Eliminar = rw("eliminar")
                        .Portes.Modificar = rw("modificar")
                        '.Tarifas.Modificar = rw("modificar")
                    Case "Modelos"
                        .Modelos.Crear = rw("crear")
                        .Modelos.Consultar = rw("consultar")
                        .Modelos.Anular = rw("anular")
                        .Modelos.Eliminar = rw("eliminar")
                        .Modelos.Modificar = rw("modificar")
                    Case "Rentabilidad"
                        .Rentabilidad.Crear = rw("crear")
                        .Rentabilidad.Consultar = rw("consultar")
                        .Rentabilidad.Anular = rw("anular")
                        .Rentabilidad.Eliminar = rw("eliminar")
                        .Rentabilidad.Modificar = rw("modificar")
                    Case "Bonos"
                        .Bono.Crear = rw("crear")
                        .Bono.Consultar = rw("consultar")
                        .Bono.Anular = rw("anular")
                        .Bono.Eliminar = rw("eliminar")
                        .Bono.Modificar = rw("modificar")
                    Case Else
                        .Bono.Consultar = False
                End Select
            End With
        Next
        mcon.Close()
        Return us
    End Function
    Public Function GetPVCByModelo(ByVal idModelo As Integer) As Boolean
        Dim pvc As Boolean = False
        Dim cnnAbierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            cnnAbierta = True
        Else
            mcon.Open()
        End If
        Dim cad As String = "select PVC from t_garantias where id_garantia=(select id_garantia from t_modelos where id_lente=" & idModelo & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        pvc = CBool(cmd.ExecuteScalar)
        If cnnAbierta = False Then
            mcon.Close()
        End If
        Return pvc
    End Function
    Public Function GetGarantiaBymodelo(ByVal modelo As Integer) As Integer
        Dim garantia As Integer = 0
        Dim conAbierta As Boolean = False
        Dim cad As String = "select id_garantia from t_modelos where id_lente=" & modelo
        Dim cmd As New SqlCommand(cad, mcon)
        If mcon.State = ConnectionState.Open Then
            conAbierta = True
        Else
            mcon.Open()
        End If
        garantia = cmd.ExecuteScalar
        If conAbierta = False Then
            mcon.Close()
        End If
        Return garantia
    End Function
    Public Function GetNumLentesByAlbaran(ByVal id As Integer) As Integer
        Dim Pedidos As Integer = 0
        Dim conAbierta As Boolean = False
        Dim cad As String = "select count(distinct(id_pedido)) from t_lineas_albaran where id_albaran=" & id & " and id_tipo_producto=1"
        Dim cmd As New SqlCommand(cad, mcon)
        If mcon.State = ConnectionState.Open Then
            conAbierta = True
        Else
            mcon.Open()
        End If
        Pedidos = cmd.ExecuteScalar
        If conAbierta = False Then
            mcon.Close()
        End If
        Return Pedidos
    End Function
    Public Function GetDiasByTratamiento(ByVal tratamiento As Integer) As Integer
        Dim dias As Integer = 0
        Dim conAbierta As Boolean = False
        Dim cad As String = "select dias from t_tratamientos where id_tratamiento=" & tratamiento
        Dim cmd As New SqlCommand(cad, mcon)
        If mcon.State = ConnectionState.Open Then
            conAbierta = True
        Else
            mcon.Open()
        End If
        dias = cmd.ExecuteScalar
        If conAbierta = False Then
            mcon.Close()
        End If
        Return dias
    End Function
    Public Function GetFormaPagoByid(ByVal id As Integer) As clsFormaPago
        Dim tb As New DataTable
        Dim cad As String = "select * from t_formas_pago where id_forma_pago=" & id
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Dim Pago As New clsFormaPago
        Pago.IdForma = tb.Rows(0)("id_forma_pago")
        Pago.FormaPago = tb.Rows(0)("forma_pago")
        Pago.Dias = tb.Rows(0)("dias")
        Return Pago
    End Function

    Public Function getClientebyId(ByVal idCliente As Long) As clsCliente
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand("select t_clientes.*,(select isnull(sum(puntos),0) from t_puntos_pedido where puntos<0 and id_pedido in (select id_pedido from t_pedidos where id_albaran=0 and anulado=0 and id_cliente=t_clientes.id_cliente)) as puntos_pendientes, provincia,(select provincia from m_provincias where id_provincia=id_provincia_fiscal) as provinciafiscal from t_clientes left join m_provincias on t_clientes.id_provincia=m_provincias.id_provincia  where id_cliente = " & idCliente))
        Dim cli As New clsCliente
        Dim ConexionAbierta As Boolean = False
        mda.SelectCommand.Connection = mcon
        If mcon.State = ConnectionState.Open Then
            ConexionAbierta = True
        Else
            mcon.Open()
        End If


        mda.Fill(tb)
        If tb.Rows.Count = 0 Then
            cli = Nothing
            mcon.Close()
            Return cli
        Else
            cli.MaxDescubierto = "" & tb.Rows(0).Item("maximo_descubierto")
            cli.CIF = "" & tb.Rows(0).Item("cif")
            cli.Codigo = tb.Rows(0).Item("codigo")
            cli.direccion = "" & tb.Rows(0).Item("direccion")
            cli.id = tb.Rows(0).Item("id_cliente")
            cli.poblacion = "" & tb.Rows(0).Item("poblacion")
            cli.Provincia = "" & tb.Rows(0).Item("provincia")
            cli.Nombre_Comercial = "" & tb.Rows(0).Item("nombre_comercial")
            cli.telefono = "" & tb.Rows(0).Item("telefono")
            'cli.antigua_poblacion = tb.Rows(0).Item("poblacion")
            cli.Razon_social = IIf(IsDBNull(tb.Rows(0)("razon_social")), "", tb.Rows(0)("razon_social"))
            cli.cobro_portes = CBool(tb.Rows(0)("cobrar_portes"))
            cli.tipo_porte = tb.Rows(0)("id_portes")
            cli.codigoCp = "" & tb.Rows(0)("codigoCp")
            cli.Id_Provincia = tb.Rows(0)("id_provincia")
            cli.Cuenta_corriente = "" & tb.Rows(0)("cc")
            cli.Codigo_Postal = "" & tb.Rows(0)("cp")
            cli.Persona_Contacto = "" & tb.Rows(0)("contacto")
            cli.MostrarPrecio = CBool(tb.Rows(0)("mostrar_precios"))
            cli.recargo = CBool(tb.Rows(0)("recargo"))
            cli.contrareembolso = CBool(tb.Rows(0)("contrareembolso"))
            cli.hoja_blanca = tb.Rows(0)("hoja_blanca")
            cli.FormaPago = GetFormaPagoByid(tb.Rows(0)("id_forma_pago"))
            cli.Deuda = CBool(tb.Rows(0)("deuda"))
            cli.SinIva = CBool(tb.Rows(0)("siniva"))
            cli.CodWeb = tb.Rows(0)("cod_web")
            cli.Email = IIf(IsDBNull(tb.Rows(0)("email")), 0, tb.Rows(0)("email"))
            cli.SinSuplementos = CBool(tb.Rows(0)("sin_suplementos"))
            cli.DtoMontaje = IIf(IsDBNull(tb.Rows(0)("dto_montaje")), 0, tb.Rows(0)("dto_montaje"))
            cli.SinDtoWeb = CBool(tb.Rows(0)("sin_dto_web"))
            cli.AlbaranPorPedido = CBool(tb.Rows(0)("albaranXpedido"))
            cli.Vip = CBool(tb.Rows(0)("vip"))
            cli.CLIENT = IIf(IsDBNull(tb.Rows(0)("CLIENT")), "", tb.Rows(0)("CLIENT"))
            cli.baja = CBool(IIf(IsDBNull(tb.Rows(0)("baja")), False, tb.Rows(0)("baja")))
            cli.DiasEnvio = Split(tb.Rows(0)("dias_envio"), ",")
            cli.Seguimiento = CBool(IIf(IsDBNull(tb.Rows(0)("seguimiento")), False, tb.Rows(0)("seguimiento")))
            cli.E_Factura = CBool(IIf(IsDBNull(tb.Rows(0)("e_factura")), False, tb.Rows(0)("e_factura")))
            cli.UsarReferencia = CBool(IIf(IsDBNull(tb.Rows(0)("usar_referencia")), False, tb.Rows(0)("usar_referencia")))
            cli.DirFiscal = "" & tb.Rows(0)("direccion_fiscal")
            cli.CPFiscal = "" & tb.Rows(0)("cp_fiscal")
            cli.PoblacionFiscal = "" & tb.Rows(0)("poblacion_fiscal")
            cli.IdProvinciaFiscal = tb.Rows(0)("id_provincia_fiscal")
            cli.ProvinciaFiscal = "" & tb.Rows(0).Item("provinciaFiscal")
            cli.IdComercial = tb.Rows(0).Item("id_comercial")
            cli.Facturacion = tb.Rows(0).Item("facturacion")
            cli.PorteMensual = tb.Rows(0).Item("porte_mensual")
            cli.Transporte = tb.Rows(0).Item("transportista")
            cli.ConfirmacionPedidos = tb.Rows(0).Item("confirma_pedidos")
            cli.ConfirmacionPortes = tb.Rows(0).Item("confirma_portes")
            cli.DeudaPendiente = GetDeudaByidCLiente(cli.id)
            cli.PendienteDatos = CBool(tb.Rows(0)("pendiente_datos"))
            cli.AplicarRappel = CBool(tb.Rows(0)("rappel"))
            cli.ProntoPago = tb.Rows(0)("dto_pp")
            cli.Puntos = tb.Rows(0)("puntos") + tb.Rows(0)("puntos_pendientes")
            cli.FacturaEnPapel = CBool(tb.Rows(0)("factura_papel"))
            cli.GrupoOptico = tb.Rows(0)("id_grupo")
            cli.Biselado = CBool(tb.Rows(0)("biselado"))
            cli.RegimenGerenal = CBool(tb.Rows(0)("regimen_general"))
            cli.Publicidad = CBool(tb.Rows(0)("publicidad"))
        End If
        'cargo el codigo y nombre del acuerdo
        Dim Fecha As Integer = FechaAcadena(Now.Date)

        mda.SelectCommand.CommandText = "select t_acuerdos.nombre,t_acuerdos.id_acuerdo from t_acuerdos inner join " & _
            "t_acuerdos_clientes on t_acuerdos.id_acuerdo=t_acuerdos_clientes.id_acuerdo where " & _
            "t_acuerdos_clientes.id_cliente = " & idCliente & " and t_acuerdos_clientes.desde<=" & Fecha & " and t_acuerdos_clientes.hasta>=" & Fecha
        tb.Clear()
        mda.Fill(tb)
        If tb.Rows.Count = 0 Then
            cli.id_Acuerdo = 0
            cli.Acuerdo = ""
        Else
            cli.id_Acuerdo = tb.Rows(0)("id_acuerdo")
            cli.Acuerdo = tb.Rows(0)("nombre")
        End If
        'ahora vamos a ver si tiene login en internet
        Dim cad As String = " Select login from t_clientes_web where id_cliente=" & cli.id
        Dim cmd As New SqlCommand(cad, mcon)
        cli.LoginInternet = IIf(IsDBNull(cmd.ExecuteScalar), "", cmd.ExecuteScalar)
        'cad = "Select isnull(sum(debe-haber),0) from t_debe_haber where id_cliente=" & cli.id
        'cmd.CommandText = cad
        cli.Descubierto = 0
        If ConexionAbierta = False Then ' no entra con la conecionabierta
            mcon.Close()
        End If
        Return cli
    End Function
    Public Function GetAcuerdoByGrupo(ByVal idGrupo As Integer, Optional ByVal Desde As Integer = 0, Optional ByVal Hasta As Integer = 0) As Integer
        If Desde = 0 Then Desde = FechaAcadena(Now.Date)
        If Hasta = 0 Then Hasta = FechaAcadena(Now.Date)
        Dim cad As String = "select isnull(id_acuerdo,0) from t_acuerdos where id_grupo=" & idGrupo & " And ((Desde <= " & Desde & " and " & Desde & " <= Hasta) or (Desde <= " & Hasta & " and " & Hasta & " <= Hasta))"
        Dim cmd As New SqlCommand(cad, mcon)
        Dim abierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            abierta = True
        Else
            mcon.Open()
        End If
        Dim grupo As Integer
        grupo = cmd.ExecuteScalar
        If abierta = False Then
            mcon.Close()
        End If
        Return grupo
    End Function
    Public Sub CargaComboGruposOpticos(ByVal cmb As ComboBox)
        Dim cad As String = "(select 0 as id_grupo,'?' as grupo) UNION (select id_grupo,grupo_optico as grupo from t_grupos_opticos ) order by id_grupo"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        cmb.ValueMember = "id_grupo"
        cmb.DisplayMember = "grupo"
        cmb.DataSource = tb
    End Sub
    Public Function GetDeudaByidCLiente(ByVal id As Integer) As clsDeuda
        Dim tb As New DataTable
        Dim cad As String = "select * from t_deudas_cliente where pendiente<>0 and id_cliente=" & id & " order by id_deuda"
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Dim d As New clsDeuda
        If tb.Rows.Count > 0 Then
            d.IdDeuda = tb.Rows(0)("id_deuda")
            d.Fecha = tb.Rows(0)("fecha")
            d.IdCliente = id
            d.Deuda = tb.Rows(0)("deuda")
            d.Pendiente = tb.Rows(0)("pendiente")
            d.Cobro = tb.Rows(0)("cobro")
        End If
        Return d
    End Function
    Public Sub PagoDeuda(ByVal d As clsDeuda, ByVal albaran As clsAlbaran)
        Dim cad As String = "UPDATE t_deudas_cliente set pendiente=pendiente-" & NumSql(albaran.Total) & " where id_deuda=" & d.IdDeuda
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'ahora insertamos la linea de deuda
        cmd.CommandText = " INSERT INTO t_lineas_deuda (id_deuda,id_albaran) VALUES (" & d.IdDeuda & "," & NumSql(albaran.Total) & ")"
        cmd.ExecuteNonQuery()
        mcon.Close()


    End Sub
    Public Function ExistePorteMensual(ByVal idcliente As Integer) As Boolean
        Dim cad As String = "Select count(*) from t_portes_mensual where id_cliente=" & idcliente & " and año=" & Now.Year & " and mes=" & Now.Month
        Dim existe As Boolean
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        existe = CBool(cmd.ExecuteScalar)
        mcon.Close()
        Return existe

    End Function
    Public Sub Grabaportemensual(ByVal idcli As Integer)
        Dim cad As String = "INSERT INTO t_portes_mensual (id_cliente,año,mes) VALUES (" & idcli & "," & Now.Date.Year & "," & Now.Date.Month & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function ExisteLogin(ByVal idcli As Integer, ByVal login As String) As Boolean
        If login = "" Then Return False
        Dim existe As Boolean
        Dim cad As String = "select count(*) from t_clientes_web where id_cliente<>" & idcli & " and login like '" & login & "'"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        existe = cmd.ExecuteScalar
        mcon.Close()
        Return existe
    End Function
    Public Function GetFormaPago(ByVal id As Integer) As String
        Dim cad As String = "select forma_pago from t_formas_pago where t_formas_pago.id_forma_pago=" & id
        Dim forma As String
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        forma = cmd.ExecuteScalar
        cmd = Nothing
        mcon.Close()
        Return forma
    End Function
    Public Function GetFormasPago() As DataTable
        Dim cad As String = "select * from t_formas_pago"
        Dim dta As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        dta.Fill(tb)
        mcon.Close()
        dta = Nothing
        Return tb
    End Function
    Public Function getCodigoNuevoCliente(ByVal id_provincia As Long) As String
        'el codigo se establece en base a la provincia
        Dim mda As New SqlDataAdapter(New SqlCommand("select max(codigo) as cod from t_clientes where left(codigo,2)='" & id_provincia & "'"))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If (tb.Rows(0).IsNull("cod")) Then
            Return CStr(id_provincia) & "0001"
        Else
            Return CLng(tb.Rows(0).Item("cod")) + 1
        End If
    End Function


    Public Function cargaLente(ByVal codigo As String) As clsProducto
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand("select * from t_productos where codigo='" & codigo & "'"))
        Dim us As New clsProducto
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        If tb.Rows.Count = 0 Then
            us = Nothing
        Else
            us.codigo = codigo
            us.Id = tb.Rows(0).Item("id_producto")
            us.precio = tb.Rows(0).Item("precio")
            us.Nombre = tb.Rows(0).Item("nombrelente")
        End If
        mcon.Close()
        Return us
    End Function


   
    Public Function getClientes(ByVal cadSql As String) As DataTable
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cadSql))
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GrabaSalidaAlmacenSemiterminado(ByVal idPedido As Integer) As Boolean
        'Hacemos esta funcion para asegurar que cuando sale de almacen grabe entrada en fabrica por que esta fallando
        Dim fecha As Integer = FechaAcadena(Now.Date)
        Dim hora As String = Format(Now.Hour, "00") & Format(Now.Minute, "00")
        Dim cad As String = " UPDATE t_ordenes_trabajo set fs_almacen=" & fecha & ",hs_almacen=" & hora & _
            ",usr_s_almacen=" & mUsuario.id & ",fe_fabrica=" & fecha & ",he_fabrica=" & hora & _
            ",usr_e_fabrica=" & mUsuario.id & " where id_pedido=" & idPedido & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & idPedido & ")"

        Dim cmd As New SqlCommand(cad, mcon)
        Dim cnnabierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            cnnabierta = True
        End If
        If cnnabierta = False Then mcon.Open()
        Try
            cmd.ExecuteNonQuery()
            If cnnabierta = False Then mcon.Close()
            Return True

        Catch ex As Exception
            If cnnabierta = False And mcon.State = ConnectionState.Open Then
                mcon.Close()
            End If
            Return False
        End Try

    End Function
    Public Function GrabaProceso(ByVal idPedido As Integer, ByVal mPaso As Paso, ByVal mProceso As Proceso, ByVal fecha As Long, ByVal hora As Integer) As Boolean
        Dim Grabado As Boolean = False
        Dim CampoFecha As String, CampoHora As String, usuario As String = "", auxUsuario As String = ""
        If mProceso = Proceso.Entrada Then
            CampoFecha = "Fe_"
            CampoHora = "He_"
            auxUsuario = "usr_e_"
        Else
            CampoFecha = "Fs_"
            CampoHora = "Hs_"
            auxUsuario = "usr_s_"
        End If
        'ahora vemos que paso estamos dando
        Dim auxiliar As String = ""
        Select Case mPaso
            Case Paso.almacen
                auxiliar = "almacen"
                usuario = "almacen"
            Case Paso.Coloracion
                'vamos a ver si se trata de una coloracion o un retoque de color
                auxiliar = "coloracion"
                usuario = "color"

            Case Paso.Endurecido
                auxiliar = "Endurecimiento"
                usuario = "Endurecimiento"
            Case Paso.Antireflejo
                auxiliar = "antireflejo"
                usuario = "antireflejo"
            Case Paso.Fabricacion
                auxiliar = "fabrica"
                usuario = "fabrica"
            Case Paso.Toplight
                auxiliar = "toplight"
                usuario = "toplight"
            Case Paso.Tratamiento
                auxiliar = "tratamiento"
            Case Paso.Calidad
                auxiliar = "calidad"
                usuario = "calidad"
            Case Paso.Externa
                auxiliar = "externa"
                usuario = "externa"
            Case Paso.Montaje
                auxiliar = "montaje"
                usuario = "montaje"
        End Select
        CampoFecha = CampoFecha & auxiliar
        CampoHora = CampoHora & auxiliar
        usuario = auxUsuario & usuario
        Dim cad As String
        If mPaso <> Paso.Tratamiento Then
            cad = " UPDATE t_ordenes_trabajo set " & CampoFecha & "=" & fecha & "," & CampoHora & "=" & hora & _
            "," & usuario & "=" & mUsuario.id & " where id_pedido=" & idPedido & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & idPedido & ")"
        Else 'entonces no se graba el usuario
            cad = " UPDATE t_ordenes_trabajo set " & CampoFecha & "=" & fecha & "," & CampoHora & "=" & hora & _
             " where id_pedido=" & idPedido & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & idPedido & ")"
        End If
        Try
            Dim cnnAbierta As Boolean
            Dim cmd As New SqlCommand(cad, mcon)
            If mcon.State = ConnectionState.Closed Then
                mcon.Open()
            Else
                cnnAbierta = True
            End If
            cmd.ExecuteNonQuery()
            mcon.Close()
            Return True
        Catch
            Mensaje("Error grabando " & auxiliar & " en el pedido " & idPedido)
            If mcon.State <> ConnectionState.Closed Then
                mcon.Close()
            End If
            Return False
        End Try
    End Function
    Public Function RetoqueColor(ByVal idpedido As Integer) As Boolean
        Dim o As New clsOrdenesTrabajo
        o = GetOrdenesTrabajo(idpedido)
        If o.Item(o.Count - 1).FeColor <> 0 And o.Item(o.Count - 1).FsColor <> 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function ProcesoGrabado(ByVal idpedido As Integer, ByVal mpaso As Paso, ByVal mproceso As Proceso) As Boolean
        Dim CampoFecha As String, CampoHora As String
        If mproceso = Proceso.Entrada Then
            CampoFecha = "Fe_"
            CampoHora = "He_"
        Else
            CampoFecha = "Fs_"
            CampoHora = "Hs_"
        End If
        'ahora vemos que paso estamos dando
        Dim auxiliar As String = ""
        Select Case mpaso
            Case Paso.almacen
                auxiliar = "almacen"
            Case Paso.Coloracion
                'If RetoqueColor(idpedido) = False Then
                auxiliar = "coloracion"
                '
            Case Paso.Endurecido
                auxiliar = "Endurecimiento"
            Case Paso.Antireflejo
                auxiliar = "antireflejo"
            Case Paso.Fabricacion
                auxiliar = "fabrica"
            Case Paso.Toplight
                auxiliar = "toplight"
            Case Paso.Tratamiento
                auxiliar = "tratamiento"
            Case Paso.Calidad
                auxiliar = "calidad"
            Case Paso.Montaje
                auxiliar = "montaje"
            Case Paso.Externa
                auxiliar = "externa"
        End Select
        If mpaso = Paso.Calidad And mproceso = Proceso.Entrada Then
            Return True
        End If
        CampoFecha = CampoFecha & auxiliar
        CampoHora = CampoHora & auxiliar
        Dim cad As String = "select " & CampoFecha & " from t_ordenes_trabajo where id_pedido=" & idpedido & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & idpedido & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        Dim grabado As Boolean
        mcon.Open()
        grabado = CBool(cmd.ExecuteScalar)
        mcon.Close()
        Return grabado

    End Function

    Public Function getClientes2(ByVal nombre As String, ByVal codigo As String) As clsClientes
        'devuelve una colecciON t_clientes si coinciden fracmento de nombre o codigo
        Dim tb As New DataTable
        Dim mclis As New clsClientes
        Dim cad As String = "Select * from t_clientes where nombre_comercial where id_cliente<>0"
        If nombre <> "" Then
            cad = cad & " and nombre_comercial like '$" & nombre & "$'"
        End If
        If codigo <> "" Then
            cad = cad & " and codigo like '$" & codigo & "'"
        End If
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Dim i As Integer
        If tb.Rows.Count = 0 Then
            mclis = Nothing
            Return mclis
        End If
        For i = 0 To tb.Rows.Count - 1
            Dim mcli As New clsCliente
            mcli = getClientebyId(tb.Rows(i)("id_cliente"))
            mclis.add(mcli)
            mcli = Nothing
        Next
        Return mclis
    End Function

    Public Function getMaxId(ByVal campo As String, ByVal tabla As String, Optional ByVal clausulawhere As String = "") As String
        'devuelve el maximo identificador
        Dim mda As New SqlDataAdapter(New SqlCommand("select max(" & campo & ") as maximo from " & tabla & " " & clausulawhere))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        Dim cnnabierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            cnnabierta = True
        Else
            mcon.Open()
        End If
        mda.Fill(tb)

        mda = Nothing
        If cnnabierta = False Then
            mcon.Close()
        End If
        If tb.Rows(0).IsNull("maximo") Then
            Return 0
        Else
            Return tb.Rows(0).Item("maximo")
        End If
    End Function

    Public Function cargaRangoFabricacionLente(ByVal tipoLente As Integer, ByVal diametro As Integer) As DataTable
        'carga el rango de fabricacion de la lente
        Dim mda As New SqlDataAdapter(New SqlCommand("select * from t_rangos where " & _
         "id_modelo = " & tipoLente & " and diametro=" & diametro & "  order by cilindro,esfera"))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function CargaLentes(ByVal tipoLente As Integer, ByVal diametro As Integer, ByVal tratamiento As Integer) As DataTable
        'carga el rango de fabricacion de la lente


        Dim mda As New SqlDataAdapter(New SqlCommand("select * from t_lentes_stock where " & _
        "id_modelo = '" & tipoLente & "' and baja=0 and diametro=" & diametro & " and tratamiento=" & tratamiento & " order by cilindro,esfera"))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function CargaRangoDesecho(ByVal tipoLente As Integer, ByVal diametro As Integer, ByVal tratamiento As Integer) As DataTable
        'carga el rango de fabricacion de la lente


        Dim mda As New SqlDataAdapter(New SqlCommand("select * from t_almacen_desechos where " & _
        "id_modelo = '" & tipoLente & "' and diametro=" & diametro & " and id_tratamiento=" & tratamiento & " order by cilindro,esfera"))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub ElimimaLente(ByVal nombre As String, ByVal diametro As Integer, ByVal tratamiento As Integer)
        'elimina una lente de stock

        Dim cad As String = ("delete from t_productos " & _
        "where diametro=" & diametro & " and tratamiento = " & tratamiento & " and nombrelente ='" & nombre & "'")
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub ElimimaSemiterminado(ByVal idlente As Integer)
        'elimina una lente de stock
        'Primero hemos de ver si ha habido movimientos en el almacen o no
        Dim Cuenta As Integer
        Dim sql As String = "Select count(*) from t_salidas_semiterminados where id_lente=" & idlente
        Dim cm As New SqlCommand(sql, mcon)
        mcon.Open()
        Cuenta = cm.ExecuteScalar
        mcon.Close()
        If Cuenta > 0 Then
            MsgBox("No se puede eliminar la base en cuestion, puesto que ya han salido lentes")
            Exit Sub
        End If
        Dim cad As String = ("delete from t_semiterminados " & _
        "where id_lente=" & idlente)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub creaLente(ByVal nombre As String, ByVal diametro As Integer, ByVal tratamiento As Integer, ByVal cilindro As Decimal, ByVal esfera As Decimal)
        'elimina una lente de stock
        Dim id As Long
        Dim cil As String
        Dim esf As String

        cil = Replace((cilindro), ",", ".")
        esf = Replace((esfera), ",", ".")

        id = getMaxId("id_producto", "t_productos") + 1

        Dim cad As String = ("insert into t_productos" & _
        "(id_producto,nombrelente,diametro,tratamiento,cilindro,esfera) values(" & id & ",'" & nombre & "'," & _
        diametro & "," & tratamiento & "," & cil & "," & esf & ")")
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        cmd = Nothing
    End Sub

    Public Sub creaRango(ByVal nombre As String, ByVal diametro As Integer, ByVal tratamiento As Integer, ByVal cilindro As Decimal, ByVal esfera As Decimal)
        'elimina una lente de stock
        Dim id As Long
        Dim cil As String
        Dim esf As String

        cil = Replace((cilindro), ",", ".")
        esf = Replace((esfera), ",", ".")

        id = getMaxId("id_producto", "t_productos") + 1
        Dim mda As New SqlDataAdapter
        mda.InsertCommand = New SqlCommand("insert into t_rangos" & _
        "(lente,diametro,cilindro,esfera,normal) values('" & nombre & "'," & diametro & "," & _
        cil & "," & esf & "," & 1 & ")")
        mda.InsertCommand.Connection = mcon
        mcon.Open()
        mda.InsertCommand.ExecuteNonQuery()
        mcon.Close()
        mda = Nothing
    End Sub

    Public Sub creaRangoFueraNorma(ByVal nombre As String, ByVal diametro As Integer, ByVal tratamiento As Integer, ByVal cilindro As Decimal, ByVal esfera As Decimal)
        'elimina una lente de stock
        Dim id As Long
        Dim cil As String
        Dim esf As String

        cil = Replace((cilindro), ",", ".")
        esf = Replace((esfera), ",", ".")

        id = getMaxId("id_producto", "t_productos") + 1
        Dim mda As New SqlDataAdapter
        mda.InsertCommand = New SqlCommand("insert into t_rangos" & _
        "(lente,diametro,cilindro,esfera,normal) values('" & nombre & "'," & diametro & "," & _
        cil & "," & esf & "," & 0 & ")")
        mda.InsertCommand.Connection = mcon
        mcon.Open()
        mda.InsertCommand.ExecuteNonQuery()
        mcon.Close()
        mda = Nothing
    End Sub


    Public Function getMaxCilindroLente(ByVal nombre As String, ByVal diametro As Integer, ByVal tratamiento As Integer, Optional ByVal rango As Boolean = False) As Decimal
        'obtiene el maximo diametro de una lente concreta
        'devuelve el maximo identificador
        Dim cad As String
        cad = "select max(cilindro) as maximo from t_productos " & _
        "where nombrelente='" & nombre & "' and diametro=" & diametro & " and tratamiento=" & tratamiento
        If rango = True Then
            cad = "select max(cilindro) as maximo from t_rangos " & _
           "where lente='" & nombre & "' and diametro=" & diametro
        End If

        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        mda = Nothing
        If tb.Rows(0).IsNull("maximo") Then
            Return 0
        Else
            Return tb.Rows(0).Item("maximo")
        End If
    End Function
    Public Function getMaxEsferaLente(ByVal nombre As String, ByVal diametro As Integer, ByVal tratamiento As Integer, Optional ByVal rango As Boolean = False) As Decimal
        'obtiene el maximo diametro de una lente concreta
        'devuelve el maximo identificador
        Dim cad As String
        cad = "select max(esfera) as maximo from t_productos " & _
        "where nombrelente='" & nombre & "' and diametro=" & diametro & " and tratamiento=" & tratamiento
        If rango = True Then
            cad = "select max(esfera) as maximo from t_rangos " & _
        "where lente='" & nombre & "' and diametro=" & diametro
        End If

        Dim mda As New SqlDataAdapter(New SqlCommand(cad))

        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        mda = Nothing
        If tb.Rows(0).IsNull("maximo") Then
            Return 0
        Else
            Return tb.Rows(0).Item("maximo")
        End If
    End Function
    Public Function getMinEsferaLente(ByVal nombre As String, ByVal diametro As Integer, ByVal tratamiento As Integer, Optional ByVal rango As Boolean = False) As Decimal
        'obtiene el maximo diametro de una lente concreta
        'devuelve el maximo identificador
        Dim cad As String
        cad = "select min(esfera) as minimo from t_productos " & _
        "where nombrelente='" & nombre & "' and diametro=" & diametro & " and tratamiento=" & tratamiento
        If rango = True Then
            cad = "select min(esfera) as minimo from t_rangos " & _
        "where lente='" & nombre & "' and diametro=" & diametro
        End If
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        mda = Nothing
        If tb.Rows(0).IsNull("minimo") Then
            Return 0
        Else
            Return tb.Rows(0).Item("minimo")
        End If
    End Function
    Public Function esProductoStock(ByVal cilindro As Decimal, ByVal esfera As Decimal, ByVal diametro As Integer, ByVal lente As String, ByVal tratamiento As Integer) As Integer
        'devuelve el id del producto o 0 si no es de stock
        Dim cad As String
        cad = "select id_producto from t_productos " & _
        "where nombrelente='" & lente & "' and diametro=" & diametro & " and tratamiento=" & tratamiento & _
        " and cilindro=" & Replace(cilindro, ",", ".") & " and esfera=" & Replace(esfera, ",", ".")
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        mda = Nothing
        If tb.Rows.Count = 0 Then

            Return 0
        Else
            Return tb.Rows(0).Item("id_producto")
        End If
    End Function
    Public Function Existencias(ByVal idProducto As Long) As Integer
        'devuelve el stock del producto
        Dim cad As String
        cad = "select stock from t_productos where id_producto=" & idProducto
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        mda = Nothing
        If tb.Rows.Count = 0 Then
            Return 0
        Else
            Return tb.Rows(0).Item("stock")
        End If
    End Function
    Public Function getNombreLentes(Optional ByVal bajas As Boolean = False, Optional ByVal SoloStock As Boolean = False) As DataTable
        Dim cad As String = ""
        Dim baja As String = ""
        If bajas = False Then
            baja = " where baja=0 "
        End If
        If SoloStock = True Then
            If bajas = False Then
                baja = " where id_lente in (select id_modelo from t_lentes_stock)"
            Else
                baja = baja & " and id_lente in (select id_modelo from t_lentes_stock)"
            End If
        End If
        cad = "select id_lente,nombre from t_modelos " & baja & " order by orden"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function getNombreLentesMonofocales(Optional ByVal bajas As Boolean = False, Optional ByVal SoloStock As Boolean = False) As DataTable
        Dim cad As String
        Dim baja As String = ""
        If bajas = False Then
            baja = " and baja=0 "
        End If
        If SoloStock = True Then
            If bajas = False Then
                baja = " and id_lente in (select id_modelo from t_lentes_stock)"
            Else
                baja = baja & " and id_lente in (select id_modelo from t_lentes_stock)"
            End If
        End If
        cad = "select id_lente,nombre from t_modelos where tipologia=1 " & baja & " order by orden"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function getDiametrosFabricacionDeLente(ByVal lente As String) As DataTable
        Dim cad As String
        cad = "select distinct diametro from t_rangos where lente ='" & lente & "' order by diametro"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function getlentesSerieDeLente(ByVal lente As String) As DataTable
        Dim cad As String
        cad = "select distinct diametro,tratamiento from t_productos where nombrelente='" & lente & "' order by tratamiento, diametro"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function getLentesbyTipo(ByVal tipologia As Integer) As DataTable
        Dim cad As String
        cad = "select nombre,id_lente from t_lentes where tipologia=" & tipologia
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub GrabaordenModelo(ByVal idModelo As Integer, ByVal orden As Integer)
        Dim cad As String = "Update t_modelos set orden=" & orden & " where id_lente=" & idModelo
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function GetModelosBasicos(Optional ByVal Orden As String = "") As DataTable
        If Orden = "" Then
            Orden = " order by orden,nombre"
        Else
            Orden = " order by " & Orden
        End If
        Dim cad As String = "select nombre,id_lente,codigo from t_modelos where id_modelo_iot=0 " & Orden
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetModelosBasicosByBase(Optional ByVal Orden As String = "") As DataTable
        If Orden = "" Then
            Orden = " order by nombre"
        Else
            Orden = " order by " & Orden
        End If
        Dim cad As String = "select nombre,id_lente,codigo from t_modelos where id_lente in (select id_modelo from t_semiterminados)" & Orden
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetModelosBasicosEspesores(Optional ByVal Orden As String = "") As DataTable
        If Orden = "" Then
            Orden = " order by nombre"
        Else
            Orden = " order by " & Orden
        End If
        Dim cad As String = "select nombre,id_lente,isnull((select id_espesor from t_modelos_espesor where id_modelo=id_lente),0) as espesor from t_modelos where id_lente in (select id_modelo from t_semiterminados)" & Orden
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetModelos(Optional ByVal baja As Boolean = False, Optional ByVal idCLiente As Integer = 0, Optional ByVal PropiosDeCliente As Boolean = False, Optional ByVal idTipo As Integer = 0) As DataTable
        Dim cad As String
        Dim filtro As String = ""
        If PropiosDeCliente = False Then
            If idCLiente <> 0 Then
                filtro = " where  (id_cliente=0 or id_cliente in (select id_tipo_modelo from t_modelos_cliente where id_cliente=" & idCLiente & ")) "
            ElseIf idTipo <> 0 Then
                filtro = " where id_cliente=" & idTipo
            End If
        Else
            If idCLiente <> 0 Then
                filtro = " where id_cliente in (select id_tipo_modelo from t_modelos_cliente where id_cliente=" & idCLiente & ")"
            ElseIf idTipo <> 0 Then
                filtro = " where id_cliente=" & idTipo
            End If
        End If
        If idCLiente = 0 And idTipo = 0 Then
            filtro = " where id_cliente=0"
        End If

        If baja = False Then
            filtro = filtro & " and  baja = 0"
        End If
        cad = "select nombre,id_lente,codigo from t_modelos " & filtro & " order by id_cliente ,orden,id_lente"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function


    Public Function EsModeloIOT(ByVal id_modelo As Long) As Boolean
        Dim cad As String
        cad = "select isnull(id_modelo_iot,0) from t_modelos where  id_lente=" & id_modelo
        Dim mda As New SqlCommand(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        Dim Modelo As Boolean = mda.ExecuteScalar
        mcon.Close()
        Return Modelo
    End Function
    Public Function InfFacturacionGrupoOptico(ByVal GrupoOptico As Integer, ByVal Fecini As Integer, ByVal Fecfin As Integer, ByVal Detallado As Boolean, Optional ByVal TipoLente As Boolean = False) As DataTable
        Dim cad As String = "declare @fecini integer" & vbNewLine & _
        "declare @fecfin integer" & vbNewLine & _
        "set @fecini=" & Fecini & vbNewLine & _
        "set @fecfin=" & Fecfin & vbNewLine
        If Detallado = False Then
            If TipoLente = False Then
                cad = cad & "select tipo_base as Concepto,sum(base) as Facturación,comision,sum(base)*comision/100 as [Comisión Grupo] from t_bases_factura INNER JOIN t_tipos_base ON t_bases_factura.id_tipo_base= t_tipos_base.id_tipo_base INNER JOIN t_facturas on t_facturas.id_factura= t_bases_factura.id_factura" & _
            " INNER JOIN t_clientes ON t_facturas.id_cliente=t_clientes.id_cliente INNER JOIN t_grupos_opticos ON t_grupos_opticos.id_grupo=t_clientes.id_grupo  where fecha>=@fecini and fecha<=@fecfin and t_clientes.id_grupo=" & GrupoOptico & " and t_bases_factura.id_tipo_base=1 GROUP BY tipo_base,comision"
            Else
                cad = cad & "select case t_pedidos.modo when 'S' then 'Stock' when 'T'then 'Transformacion' when 'F' then 'Fabrica' END as Tipo ,sum(t_lineas_factura.total) as Facturación,comision,sum(t_lineas_factura.total)*comision/100 as [Comisión Grupo] from t_lineas_factura INNER JOIN t_lineas_albaran ON t_lineas_albaran.id_albaran=t_lineasfactura.id_albaran INNER JOIN t_pedidos ON t_lineas_albaran.id_pedido= t_pedidos.id_pedido INNER JOIN t_facturas on t_facturas.id_factura= t_lineas_factura.id_factura" & _
          " INNER JOIN t_clientes ON t_facturas.id_cliente=t_clientes.id_cliente INNER JOIN t_grupos_opticos ON t_grupos_opticos.id_grupo=t_clientes.id_grupo  where fecha>=@fecini and fecha<=@fecfin and t_clientes.id_grupo=" & GrupoOptico & " and t_lineas_factura.id_tipo_producto=1 GROUP BY t_pedidos.modo,comision"

            End If
        Else
            If TipoLente = False Then
                cad = cad & "select codigo,nombre_comercial as cliente,num_factura as Factura,tipo_base as Concepto,sum(base) as Facturación,comision,sum(base)* comision/100 as [Comisión Grupo] from t_bases_factura INNER JOIN t_tipos_base ON t_bases_factura.id_tipo_base= t_tipos_base.id_tipo_base INNER JOIN t_facturas on t_facturas.id_factura= t_bases_factura.id_factura" & _
                      " INNER JOIN t_clientes ON t_facturas.id_cliente=t_clientes.id_cliente INNER JOIN t_grupos_opticos ON t_grupos_opticos.id_grupo=t_clientes.id_grupo where fecha>=@fecini and fecha<=@fecfin and t_clientes.id_grupo=" & GrupoOptico & " and t_bases_factura.id_tipo_base=1 GROUP BY comision,tipo_base,codigo,nombre_comercial,num_factura order by codigo,num_factura"
            Else
                cad = cad & "select nombre_comercial as cliente,case t_pedidos.modo when 'S' then 'Stock' when 'T'then 'Transformacion' when 'F' then 'Fabrica' END as Tipo ,sum(t_lineas_factura.total) as Facturación,comision,sum(t_lineas_factura.total)*comision/100 as [Comisión Grupo] from t_lineas_factura INNER JOIN t_lineas_albaran ON t_lineas_albaran.id_albaran=t_lineasfactura.id_albaran INNER JOIN t_pedidos ON t_lineas_albaran.id_pedido= t_pedidos.id_pedido INNER JOIN t_facturas on t_facturas.id_factura= t_lineas_factura.id_factura" & _
       " INNER JOIN t_clientes ON t_facturas.id_cliente=t_clientes.id_cliente INNER JOIN t_grupos_opticos ON t_grupos_opticos.id_grupo=t_clientes.id_grupo  where fecha>=@fecini and fecha<=@fecfin and t_clientes.id_grupo=" & GrupoOptico & " and t_lineas_factura.id_tipo_producto=1 GROUP BY nombre_comercial,t_pedidos.modo,comision ORDER BY nombre_comercial"

            End If
        End If
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function InfCLientesRangoFacturacion(ByVal Fecini As Integer, ByVal Fecfin As Integer) As DataTable
        Dim cad As String = "declare @fecini integer" & vbNewLine & _
        "declare @fecfin integer" & vbNewLine & _
        "set @fecini=" & Fecini & vbNewLine & _
        "set @fecfin=" & Fecfin & vbNewLine & _
        "(select 0 as orden,'Clientes<=100' as rango,count(*) as total from (select id_cliente from t_albaranes  where fecha>=@fecini and fecha<=@fecfin group by id_cliente having sum(total)<=100) as facturacion) UNION " & vbNewLine & _
        "(select 1 as orden,'Clientes entre 101 y 200' as rango,count(*) as total from (select id_cliente from t_albaranes  where fecha>=@fecini and fecha<=@fecfin group by id_cliente having sum(total)<=101 and sum(total)<=200) as facturacion) UNION " & vbNewLine & _
        "(select 2,'Clientes entre 201 y 500',count(*) as total from (select id_cliente from t_albaranes  where fecha>=@fecini and fecha<=@fecfin group by id_cliente having 201<=sum(total) and sum(total)<=500) as facturacion) UNION " & vbNewLine & _
        "(select 3,'Clientes entre 501 y 1000',count(*) as total from (select id_cliente from t_albaranes  where fecha>=@fecini and fecha<=@fecfin group by id_cliente having 501<=sum(total) and sum(total)<=1000) as facturacion) UNION " & vbNewLine & _
        "(select 4,'Clientes entre 1001 y 1500',count(*) as total from (select id_cliente from t_albaranes  where fecha>=@fecini and fecha<=@fecfin group by id_cliente having 1001<=sum(total) and sum(total)<=1500) as facturacion) UNION " & vbNewLine & _
        "(select 5,'Clientes entre 1501 y 2000',count(*) as total from (select id_cliente from t_albaranes  where fecha>=@fecini and fecha<=@fecfin group by id_cliente having 1501<=sum(total) and sum(total)<=2000) as facturacion) UNION " & vbNewLine & _
        "(select 6,'Clientes entre 2001 y 3000',count(*) as total from (select id_cliente from t_albaranes  where fecha>=@fecini and fecha<=@fecfin group by id_cliente having  sum(total)>2000  and sum(total)<=3000)  as facturacion) UNION  " & vbNewLine & _
        "(select 7,'Clientes Mayor que 3000',count(*) as total from (select id_cliente from t_albaranes  where fecha>=@fecini and fecha<=@fecfin group by id_cliente having  sum(total)>3001)  as facturacion) order by orden"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    Public Function InfRentabilidadTipoLente(ByVal fecini As Integer, ByVal fecfin As Integer) As DataTable
        Dim cad As String = "declare @fecini integer" & vbNewLine & _
        "declare @fecfin integer" & vbNewLine & _
        "set @fecini=" & fecini & vbNewLine & _
        "set @fecfin=" & fecfin & vbNewLine & _
        "(select 0 as orden,'STOCK' as lentes,((select sum(total) from t_lineas_albaran where id_pedido in (select id_pedido from t_pedidos where modo like 'S' and anulado=0 and fecha>=@fecini and fecha<=@fecfin))-(select sum(coste) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where modo like 'S' and anulado=0 and fecha>=@fecini and fecha<=@fecfin))) *100/(select sum(coste) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where modo like 'S' and id_albaran<>0 and anulado=0 and fecha>=@fecini and fecha<=@fecfin)) as rentabilidad ) UNION " & vbNewLine & _
        "(select 1 as orden,'Pedidos' as lentes,((select sum(total) from t_lineas_albaran where id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and fecha>=@fecini and fecha<=@fecfin))-(select sum(coste) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and fecha>=@fecini and fecha<=@fecfin))) *100/(select sum(coste) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where modo like 'F' and id_albaran<>0 and anulado=0 and fecha>=@fecini and fecha<=@fecfin)) as rentabilidad ) UNION " & vbNewLine & _
        "(select 2,'Monofocal convencional' as lente,((select sum(total) from t_lineas_albaran where id_pedido in  (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot=0)  and fecha>=@fecini and fecha<=@fecfin))-(select sum(coste) from t_costos_pedido where id_pedido in  (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot=0)  and fecha>=@fecini and fecha<=@fecfin )))*100/(select sum(coste) from t_costos_pedido where id_pedido in  (select id_pedido from t_pedidos where modo like 'F' and id_albaran<>0 and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot=0)  and fecha>=@fecini and fecha<=@fecfin )) as total) UNION " & vbNewLine & _
        "(select 3,'Monofocal Freeform' as lente, ((select sum(total) from t_lineas_albaran where id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot<>0)  and fecha>=@fecini and fecha<=@fecfin ))-(select sum(coste) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot<>0)  and fecha>=@fecini and fecha<=@fecfin )))*100/(select sum(coste) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where modo like 'F' and id_albaran<>0 and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=1 and id_modelo_iot<>0)  and fecha>=@fecini and fecha<=@fecfin )) as total) UNION " & vbNewLine & _
        "(select 4,'Bifocales' as lente,((select sum(total) from t_lineas_albaran where id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2)  and fecha>=@fecini and fecha<=@fecfin ))-(select sum(coste) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=2)  and fecha>=@fecini and fecha<=@fecfin )))*100/(select sum(coste) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where modo like 'F' and anulado=0 and id_albaran<>0 and id_modelo in (select id_lente from t_modelos where tipologia=2)  and fecha>=@fecini and fecha<=@fecfin )) as total ) UNION " & vbNewLine & _
        "(select 5,'Prog. No personal.' as lente, ((select sum(total) from t_lineas_albaran where id_pedido in (select id_pedido from t_pedidos where modo='F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=0)  and fecha>=@fecini and fecha<=@fecfin))-(select sum(coste) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where modo='F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=0)  and fecha>=@fecini and fecha<=@fecfin))) *100/(select sum(coste) from t_costos_pedido  where id_pedido in (select id_pedido from t_pedidos where modo='F' and id_albaran<>0 and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=0)  and fecha>=@fecini and fecha<=@fecfin)) as total ) UNION " & vbNewLine & _
        "(select 6,'Prog.Personalizado.' as lente,((select sum(total) from t_lineas_albaran where id_pedido in (select id_pedido from t_pedidos where modo='F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=1)  and fecha>=@fecini and fecha<=@fecfin)) -(select sum(coste) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where modo='F' and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=1)  and fecha>=@fecini and fecha<=@fecfin)))*100/(select sum(coste) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where modo='F' and id_albaran<>0 and anulado=0 and id_modelo in (select id_lente from t_modelos where tipologia=3 and personalizado=1)  and fecha>=@fecini and fecha<=@fecfin)) as total) order by orden"
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.SelectCommand.CommandTimeout = 420
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function InfRentabilidadModelo(ByVal fecini As Integer, ByVal fecfin As Integer, ByVal tratamiento As Boolean) As DataTable

        '  BorraConsulta("RentModelo" & mUsuario.id)
        Dim cad As String
        If tratamiento = True Then
            cad = "select lente,tratamiento,count(*) as pedidos,sum(facturacion) as facturacion,sum(coste) as coste from (select t_modelos.nombre as lente,t_tratamientos.nombre as tratamiento,(select isnull(sum(total),0) from t_lineas_albaran where id_pedido=t_pedidos.id_pedido) as Facturacion,(select isnull(sum(coste),0) from t_costos_pedido where id_pedido=t_pedidos.id_pedido) as coste from  t_modelos INNER JOIN t_pedidos ON t_modelos.id_lente=t_pedidos.id_modelo INNER JOIN t_tratamientos ON t_pedidos.id_tratamiento=t_tratamientos.id_tratamiento   where  t_pedidos.id_albaran<>0 and anulado=0 and fecha>=" & fecini & " and fecha<=" & fecfin & ") as tabla  group by lente,tratamiento order by lente,tratamiento"
        Else
            cad = "select lente,count(*) as pedidos,sum(facturacion) as facturacion,sum(coste) as coste from (select t_modelos.nombre as lente,(select isnull(sum(total),0) from t_lineas_albaran where id_pedido=t_pedidos.id_pedido) as Facturacion,(select isnull(sum(coste),0) from t_costos_pedido where id_pedido=t_pedidos.id_pedido) as coste from  t_modelos INNER JOIN t_pedidos ON t_modelos.id_lente=t_pedidos.id_modelo where  t_pedidos.id_albaran<>0 and anulado=0 and fecha>=" & fecini & " and fecha<=" & fecfin & ") as tabla  group by lente order by lente"

        End If
        ' Dim cmd As New SqlCommand(cad, mcon)
        'BorraConsulta("RentLente" & mUsuario.id)

        'mcon.Open()
        'cmd.ExecuteNonQuery() 'creamos la vista
        'mcon.Close()
        ' Dim cad1 As String = "select *, case pedidos when 0 then 0 else facturacion/pedidos END as [PVP Lente], case pedidos when 0 then 0 else coste/pedidos END as [PCoste Lente], case coste when 0 then 0 else convert(decimal(8,2),(facturacion-coste)*100/coste) END as rentabilidad from Rentmodelo" & mUsuario.id & " where pedidos>0 order by lente"

        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.SelectCommand.CommandTimeout = 420
        '  mda.SelectCommand.CommandTimeout = 180
        Dim tb As New DataTable
        mda.Fill(tb)
        'ahora borramos la vista
        BorraConsulta("RentModelo" & mUsuario.id)
        Return tb

    End Function

    Public Function EsModeloStock(ByVal id_modelo As Long) As Boolean
        Dim cad As String
        cad = "select count(*) from t_lentes_stock where baja = 0 and id_modelo=" & id_modelo
        Dim mda As New SqlCommand(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        Dim Modelo As Boolean = mda.ExecuteScalar
        mcon.Close()
        Return Modelo
    End Function

    Public Function EsModeloPersonalizado(ByVal id_modelo As Long) As Boolean
        Dim cad As String
        cad = "select personalizado from t_modelos where baja = 0 and id_lente=" & id_modelo
        Dim mda As New SqlCommand(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        Dim Modelo As Boolean = mda.ExecuteScalar
        mcon.Close()
        Return Modelo
    End Function

    Public Function GetPreciosProveedorStock() As clsPreciosProveedorStock
        Dim cad As String = "select * from t_precios_stock_proveedor order by id_modelo"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        Dim Precios As New clsPreciosProveedorStock
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows
            Dim pre As New clsPrecioProvStock
            pre.idModelo = rw("id_modelo")
            pre.Diametro = rw("diametro")
            pre.IdGraduacion = rw("id_graduacion")
            pre.idTratamiento = rw("id_tratamiento")
            pre.Precio = rw("precio")
            Precios.add(pre)
        Next
        Return Precios
    End Function

    Public Function getModeloById(ByVal id_modelo As Long) As DataTable
        Dim cad As String
        cad = "select * from t_modelos where id_lente=" & id_modelo
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function getIndicesModelos() As DataTable
        Dim cad As String
        cad = "select distinct(indice_modelo) as indice from t_modelos where baja = 0 order by indice_modelo"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetModelosNoagrupados() As DataTable
        'vamos a devolver el codigo, el idmodelo y el nombre
        Dim cad As String = "select id_lente ,codigo,nombre from t_modelos where id_grupo=0 and baja=0 order by orden"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        mda.Dispose()
        Return tb
    End Function

    Public Sub GrabaGrupoModelos(ByVal grupo As clsGrupo, ByVal modelos As ArrayList)
        Dim cad As String
        Dim mid As Integer

        If grupo.Id = 0 Then ' se trata de un nuevo grupo, vamos a  insertarlo
            mid = getMaxId("id_grupo", "t_grupos_modelos") + 1
            cad = "INSERT INTO t_grupos_modelos (id_grupo,grupo) VALUES (" & mid & "," & strsql(grupo.Nombre) & ")"
        Else
            mid = grupo.Id
            cad = "Update t_grupos_modelos set Grupo=" & strsql(grupo.Nombre) & " where id_grupo=" & mid
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'ahora updateamos el modelo para la lente en cuestion, primero ponemos a cero los que estaban en ese grupo
        cad = "Update t_modelos set id_grupo=0 where id_grupo=" & mid
        cmd.CommandText = cad
        cmd.ExecuteNonQuery()
        If modelos.Count > 0 Then
            For i As Integer = 0 To modelos.Count - 1
                cad = "Update t_modelos set id_grupo=" & mid & " where id_lente=" & modelos(i)
                cmd.CommandText = cad
                cmd.ExecuteNonQuery()
            Next
        End If
        mcon.Close()
    End Sub

    Public Function GetModelosByGrupo(ByVal id As Integer) As DataTable
        'vamos a devolver el codigo, el idmodelo y el nombre
        Dim cad As String = "select id_lente as id_modelo,codigo,nombre from t_modelos where id_grupo=" & id
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        mda.Dispose()
        Return tb
    End Function

    Public Function getRXModeloById(ByVal id_modelo As Long) As String
        Dim cad As String
        cad = "select rx_nombre from t_modelos where  id_lente=" & id_modelo
        Dim cmd As New SqlCommand(cad, mcon)
        Dim modelo As String
        mcon.Open()
        modelo = cmd.ExecuteScalar
        mcon.Close()
        Return modelo
    End Function

    Public Function getClsModeloById(ByVal id_modelo As Long) As clsModelo
        Dim cad As String
        Dim m As New clsModelo

        cad = "select * from t_modelos where id_lente=" & id_modelo
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)

        m.id_modelo = tb.Rows(0)("id_Lente")
        m.codigo = tb.Rows(0)("codigo")
        m.Nombre = tb.Rows(0)("nombre")
        m.material = tb.Rows(0)("material")
        m.tipologia = tb.Rows(0)("tipologia")
        m.RxNombre = tb.Rows(0)("rx_nombre")
        m.Indice = tb.Rows(0)("indice")
        m.PrefijoIOT = tb.Rows(0)("prefijo_iot")
        m.SufijoIOT = tb.Rows(0)("sufijo_iot")
        m.Baja = CBool(tb.Rows(0)("baja"))
        m.MontajeRegresion = IIf(IsDBNull(tb.Rows(0)("montaje")), 0, tb.Rows(0)("montaje"))
        m.MaterialLente = IIf(IsDBNull(tb.Rows(0)("material_lente")), "", tb.Rows(0)("material_lente"))
        m.ModeloAlternativo = tb.Rows(0)("id_modelo_iot")
        m.IndiceModelo = tb.Rows(0)("indice_modelo")
        m.AdicionMaxima = tb.Rows(0)("adicion_maxima")
        m.Personalizado = tb.Rows(0)("personalizado")
        m.id_grupo = tb.Rows(0)("ID_GRUPO")
        m.idCliente = tb.Rows(0)("id_cliente")
        m.RangoHasta = tb.Rows(0)("rango_hasta")
        m.RangoDesde = tb.Rows(0)("rango_desde")
        m.Diseñador = tb.Rows(0)("id_diseñador")
        m.Pique = GetPiqueByid(tb.Rows(0)("id_pique"))
        m.Puntos = tb.Rows(0)("puntos")
        m.Garantia = tb.Rows(0)("id_garantia")
        'ahora cargamos los tratamientos
        tb.Clear()
        mda.SelectCommand.CommandText = "select id_tratamiento from t_tratamientos_modelo where id_modelo=" & m.id_modelo
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows
            m.Tratamientos.Add(rw("id_tratamiento"))
        Next
        'ahora vamos a cargar las lenticulares
        tb.Clear()
        mda.SelectCommand.CommandText = "select * from t_lenticulares where id_modelo=" & id_modelo
        mda.Fill(tb)
        mcon.Close()

        For Each rw As DataRow In tb.Rows
            m.Lenticular.Add(rw("lenticular"))
        Next
        tb.Clear()
        mda.SelectCommand.CommandText = "select * from t_pasillos where id_modelo=" & id_modelo
        mda.Fill(tb)
        mcon.Close()

        For Each rw As DataRow In tb.Rows
            m.Pasillos.Add(rw("pasillo"))
        Next
        Return m
    End Function

    Public Sub GrabaModelo(ByVal m As clsModelo)
        Dim cad As String
        If m.id_modelo = 0 Then
            m.id_modelo = getMaxId("id_lente", "t_modelos") + 1
            cad = "Insert into t_modelos (id_lente,codigo,nombre,material,tipologia,baja,orden,indice,rx_nombre,prefijo_iot,sufijo_iot,id_modelo_iot,montaje,material_lente,adicion_maxima,indice_modelo,personalizado,id_grupo,id_cliente,rango_desde,rango_hasta,id_diseñador,id_pique,puntos,id_garantia) VALUES " & _
            " (" & m.id_modelo & "," & strsql(m.codigo) & "," & strsql(m.Nombre) & "," & m.material & "," & m.tipologia & " ," & IIf(m.Baja = False, 0, 1) & "," & _
            getMaxId("orden", "t_modelos") + 1 & "," & NumSql(m.Indice) & "," & strsql(m.RxNombre) & "," & strsql(m.PrefijoIOT) & "," & strsql(m.SufijoIOT) & "," & m.ModeloAlternativo & "," & NumSql(m.MontajeRegresion) & "," & strsql(m.MaterialLente) & _
            "," & NumSql(m.AdicionMaxima) & "," & NumSql(m.IndiceModelo) & "," & IIf(m.Personalizado = False, 0, 1) & "," & m.id_grupo & "," & m.idCliente & "," & NumSql(m.RangoDesde) & "," & NumSql(m.RangoHasta) & "," & m.Diseñador & "," & m.Pique.id & "," & m.Puntos & "," & m.Garantia & ")"
        Else
            cad = "update t_modelos set codigo=" & strsql(m.codigo) & ",nombre=" & strsql(m.Nombre) & ",material=" & m.material & ",tipologia=" & m.tipologia & ",baja=" & IIf(m.Baja = False, 0, 1) & _
            ",indice=" & NumSql(m.Indice) & ",rx_nombre=" & strsql(m.RxNombre) & ",prefijo_iot=" & strsql(m.PrefijoIOT) & ",sufijo_iot=" & strsql(m.SufijoIOT) & ",id_modelo_iot=" & m.ModeloAlternativo & _
            ",montaje=" & strsql(m.MontajeRegresion) & ",material_lente=" & strsql(m.MaterialLente) & ",adicion_maxima=" & NumSql(m.AdicionMaxima) & ",indice_modelo=" & NumSql(m.IndiceModelo) & _
            ",personalizado=" & IIf(m.Personalizado = False, 0, 1) & ",id_cliente=" & m.idCliente & ",rango_desde=" & NumSql(m.RangoDesde) & ",rango_hasta=" & NumSql(m.RangoHasta) & ",id_diseñador=" & m.Diseñador & ",id_pique=" & m.Pique.id & ",puntos=" & m.Puntos & ",id_garantia=" & m.Garantia & " where id_lente=" & m.id_modelo
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'ahora borramos las lenticulares e insertamos las nuevas
        cmd.CommandText = "DELETE FROM t_Lenticulares where id_modelo=" & m.id_modelo
        cmd.ExecuteNonQuery()
        If m.Lenticular.Count > 0 Then
            For i As Integer = 0 To m.Lenticular.Count - 1
                cmd.CommandText = "INSERT INTO t_lenticulares VALUES (" & m.id_modelo & "," & m.Lenticular(i) & ")"
                cmd.ExecuteNonQuery()
            Next
        End If
        cmd.CommandText = "DELETE FROM t_pasillos where id_modelo=" & m.id_modelo
        cmd.ExecuteNonQuery()
        If m.Pasillos.Count > 0 Then
            For i As Integer = 0 To m.Pasillos.Count - 1
                cmd.CommandText = "INSERT INTO t_pasillos VALUES (" & m.id_modelo & "," & m.Pasillos(i) & ")"
                cmd.ExecuteNonQuery()
            Next
        End If
        cmd.CommandText = "DELETE FROM t_Tratamientos_modelo where id_modelo=" & m.id_modelo
        cmd.ExecuteNonQuery()
        If m.Tratamientos.Count > 0 Then
            For i As Integer = 0 To m.Tratamientos.Count - 1
                cmd.CommandText = "INSERT INTO t_tratamientos_modelo (id_modelo,id_tratamiento) VALUES (" & m.id_modelo & "," & m.Tratamientos(i) & ")"
                cmd.ExecuteNonQuery()
            Next
        End If
        mcon.Close()
    End Sub

    Public Function GrabaPedidoProveedor(ByVal fecha As Long, ByVal Proveedor As String, ByVal modelo As Integer) As Integer
        Dim id As Integer
        id = getMaxId("id_pedido", "t_pedidos_prov") + 1
        Dim cad As String = " INSERT INTO t_pedidos_prov (id_pedido,fecha,proveedor,id_modelo) VALUES (" & id & "," & fecha & ",'" & Proveedor & "'," & modelo & ")"
        Dim comando As New SqlCommand(cad, mcon)
        mcon.Open()
        comando.ExecuteNonQuery()
        mcon.Close()
        comando = Nothing
        Return id
    End Function

    Public Function GrabaPedidoProveedorSemiterminado(ByVal fecha As Long, ByVal Proveedor As Integer, ByVal modelo As Integer) As Integer
        Dim id As Integer
        id = getMaxId("id_pedido", "Proveedores.dbo.t_pedidos_semiterminados") + 1
        Dim cad As String = " INSERT INTO Proveedores.dbo.t_pedidos_semiterminados (id_pedido,fecha,id_proveedor,id_modelo,eliminado) VALUES (" & id & "," & fecha & "," & Proveedor & "," & modelo & ",0)"
        Dim comando As New SqlCommand(cad, mcon)
        mcon.Open()
        comando.ExecuteNonQuery()
        mcon.Close()
        comando = Nothing
        Return id
    End Function
    Public Sub GrabaLineaPedidoProveedorSemiterminados(ByVal idPedido As Integer, ByVal idArticulo As Integer, ByVal cantidad As Integer, ByVal servido As Integer)
        Dim cad As String = "INSERT INTO  Proveedores.dbo.t_lineas_pedido_semiterminados (id_pedido,id_lente,cantidad,servido) VALUES (" & idPedido & "," & idArticulo & "," & cantidad & "," & servido & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        cmd = Nothing
    End Sub
    Public Sub GrabaLineaPedidoProveedor(ByVal idPedido As Integer, ByVal idArticulo As Integer, ByVal cantidad As Integer, ByVal servido As Integer)
        Dim cad As String = "INSERT INTO t_lineas_pedidos_prov (id_pedido,id_producto,cantidad,servido) VALUES (" & idPedido & "," & idArticulo & "," & cantidad & "," & servido & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        cmd = Nothing
    End Sub
    Public Function GetModeloByCodigo(ByVal codigo As String) As DataTable
        Dim cad As String
        cad = "select * from t_modelos where baja = 0 and codigo='" & codigo & "'"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Sub CambiaHoraPedido(ByVal idPedido As Integer, ByVal hora As String)
        If hora.Length = 3 Then hora = "0" & hora
        Dim cmd As New SqlCommand("Update t_pedidos set hora='" & hora & "' where id_pedido=" & idPedido, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Function GetParejaPedido(ByVal id As Integer) As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select id_pedido,fecha,hora,nombre_comercial,nombre as modelo from t_pedidos INNER JOIN t_clientes ON t_pedidos.id_cliente=t_clientes.id_cliente INNER JOIN t_modelos on t_pedidos.id_modelo=t_modelos.id_lente Where id_pedido=" & id & " or pareja=" & id
        Dim mda As New SqlDataAdapter(cad, mcon)

        mda.Fill(tb)
        Return tb
    End Function
    Public Function getLenteStockByGraduacion(ByVal idModelo As Integer, ByVal diametro As Integer, ByVal cilindro As Decimal, ByVal esfera As Decimal, ByVal tratamiento As Integer) As clsLenteStock
        Dim cil As String = Replace(cilindro, ",", ".")
        Dim esf As String = Replace(esfera, ",", ".")

        Dim cad As String
        cad = "select t_lentes_stock.*,t_modelos.nombre,t_tratamientos.nombre as ntratamiento " & _
        "from (t_lentes_stock inner join t_modelos on t_lentes_stock.id_modelo=t_modelos.id_lente)inner join t_tratamientos on t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento " & _
        "where id_modelo=" & idModelo & " and diametro=" & diametro & " and cilindro=" & cil & " and esfera=" & esf & " and tratamiento=" & tratamiento & " and t_lentes_stock.baja=0"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then Return Nothing
        Dim len As New clsLenteStock
        len.cilindro = tb.Rows(0)("cilindro")
        len.esfera = tb.Rows(0)("esfera")
        len.diametro = tb.Rows(0)("diametro")
        len.id = tb.Rows(0)("id_producto")
        len.id_modelo = tb.Rows(0)("id_modelo")
        len.id_tratamiento = tb.Rows(0)("tratamiento")
        len.modelo = tb.Rows(0)("nombre")
        len.tratamiento = tb.Rows(0)("ntratamiento")
        len.PrecioCompra = GetPrecioCompraStock(len)
        Return len

    End Function

    Public Function EstaEnRangoFabricacion(ByVal idModelo As Integer, ByVal diametro As Integer, ByVal cilindro As Decimal, ByVal esfera As Decimal) As Boolean
        Dim cad As String
        Dim cil As String
        Dim esf As String
        cil = Replace(cilindro, ",", ".")
        esf = Replace(esfera, ",", ".")

        cad = "select * from t_rangos where id_modelo=" & idModelo & " and diametro=" & diametro & " and cilindro=" & cil & " and esfera=" & esf
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count > 0 Then Return True
        Return False
    End Function

    Public Function ListarTratamientos() As clsTratamientos
        Dim cad As String
        cad = "select * from t_tratamientos"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Dim i As Integer
        Dim ts As New clsTratamientos
        For i = 0 To tb.Rows.Count - 1
            ts.add(tb.Rows(i)("id_tratamiento"), tb.Rows(i)("nombre"))
        Next
        Return ts
        ' Dim dv As New DataView

    End Function
    Public Function ListarTratamientosbyModelo(ByVal idmodelo As Integer) As DataTable
        Dim cad As String
        cad = "select * from t_tratamientos  where id_tratamiento in (select id_tratamiento from t_tratamientos_modelo where id_modelo=" & idmodelo & ")"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function listarColoraciones() As DataTable
        Dim cad As String
        cad = "select  *,(gama + ' ' + isnull(color,'')) as desig from t_coloraciones order by id_coloracion"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function ListarTratamientosTabla() As DataTable
        Dim cad As String
        cad = "select * from t_tratamientos"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub GrabaEquipo(ByVal pedidosweb As Boolean, ByVal compromisos As Boolean, ByVal extraviados As Boolean, ByVal Mantenimientos As Boolean)
        Dim cad As String = "delete from t_equipos where equipo like " & strsql(Equipo) & " and id_usuario=" & mUsuario.id
        Dim Conversion As Decimal = GetFactorConversionforma()
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()

        cmd.ExecuteNonQuery()
        'ahora grabamos el estado del equipo
        cmd.CommandText = "INSERT INTO t_equipos (equipo,pedidos_web,compromisos,extraviados,id_usuario,maquinas,id_departamento,conversion_forma) VALUES (" & strsql(Equipo) & "," & IIf(pedidosweb = True, 1, 0) & _
        "," & IIf(compromisos = True, 1, 0) & "," & IIf(extraviados = True, 1, 0) & "," & mUsuario.id & "," & IIf(Mantenimientos = True, 1, 0) & "," & GetDepartamentoByequipo() & "," & NumSql(Conversion) & ")"
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub GrabaTipoPegatinaByEquipo(ByVal UDI As Boolean)
        Dim cad As String = "UPDATE t_equipos set pegatina_udi=" & IIf(UDI = True, 1, 0) & " where equipo=" & strsql(Equipo)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function EspedidoReposicion(ByVal idPedido As Integer) As Integer
        'si el pedido es de reposicion devolvemos el id del padre
        Dim cad As String = "select isnull(id_padre,0) from t_reposicion_pedido where id_pedido=" & idPedido
        Dim mda As New SqlCommand(cad, mcon)
        Dim cnnabierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            cnnabierta = True
        Else
            mcon.Open()
        End If

        Dim Padre As Integer = mda.ExecuteScalar
        If cnnabierta = False Then
            mcon.Close()
        End If
        Return Padre
    End Function
    Public Sub GrabaPedidos(ByRef mPeds As clsPedidos, Optional ByVal stock As Boolean = False)
        'graba los pedidos
        Dim cil As String
        Dim esf As String
        Dim addi As String
        Dim id As Long
        Dim cad As String
        Dim fecha As Long
        Dim hora As String
        Dim mda As New SqlDataAdapter
        Dim p As clsPedido
        Dim i As Integer
        Dim idstock As Integer
        ' Dim puntos As Integer = 0
        fecha = FechaAcadena(Today)
        hora = Format(Hour(Now), "00") & Format(Minute(Now), "00")
        For i = 0 To mPeds.Count - 1

            p = mPeds.Item(i)

            cil = Replace(p.cilindro, ",", ".")
            esf = Replace(p.esfera, ",", ".")
            addi = Replace(p.adicion, ",", ".")
            p.Fechapedido = Today()
            'p.horaPedido = Format(Hour(Now), "00") & Format(Minute(Now), "00")

            id = getMaxId("id_pedido", "T_pedidos") + 1
            p.horaPedido = hora
            'If id = 0 Then
            '    id = 10000000001
            'Else
            '    id = id + 1
            'End If
            'cambio el numero de las parejas en casao de parejas
            If p.ojo = "D" And p.pareja <> 0 Then
                mPeds.Item(i + 1).pareja = id
                p.pareja = id + 1
            ElseIf p.ojo = "I" And p.pareja <> 0 Then
                mPeds.Item(i - 1).pareja = id
                p.pareja = id - 1
            End If
            If mcon.State = ConnectionState.Closed Then
                mcon.Open()
            End If
            p.id = id
            cad = "insert into t_pedidos" & _
            " (id_pedido,fecha,hora,id_usuario,id_cliente,id_modelo,modo," & _
            "diametro,ojo,cilindro,esfera,eje,adicion,descentramiento_mm,descentramiento_grados,prisma_valor," & _
            "prisma_eje,espesor_ranurar,espesor_taladrar,base_curva,base_plana,esmerilado,precalibrado," & _
            "id_tratamiento,id_coloracion,intensidad,pareja,id_reservado,referencia,observaciones,Pedido_Web,montaje," & _
            "compromiso,fecha_coloracion,hora_coloracion,a_filo,eliptico,lenticular,sin_cargo,tallado,urgente,compensador,pasillo,inset,id_promocion)" & _
            " values (" & p.id & "," & fecha & ",'" & _
            hora & "'," & p.id_usuario & "," & p.id_cliente & "," & p.id_modelo & ",'" & _
            p.modo & "'," & p.diametro & ",'" & p.ojo & "'," & cil & "," & esf & "," & _
            p.eje & "," & addi & "," & NumSql(p.descentramiento_mm) & "," & _
            p.descentramiento_grados & "," & NumSql(p.prisma_valor) & "," & p.prisma_eje & "," & _
            p.espesor_ranurar & "," & p.espesor_taladrar & "," & NumSql(p.base_curva) & "," & p.base_plana & "," & _
            p.esmerilado & "," & p.precalibrado & "," & p.id_tratamiento & "," & _
            p.id_coloracion & "," & p.intensidad & "," & p.pareja & "," & p.Id_reserva & "," & strsql(p.Referencia) & "," & strsql(p.Observaciones) & "," & Int(p.PedidoWeb) & _
            "," & IIf(p.Montaje = True, 1, 0) & "," & p.FechaCompromiso & "," & p.FechaSalidaColoracion & ",'" & p.HoraSalidaColoracion & "'," & p.A_Filo & "," & IIf(p.Eliptico = False, 0, 1) & _
            "," & p.Lenticular & "," & IIf(p.SinCargo = False, 0, 1) & "," & strsql(p.Tallado) & "," & IIf(p.Urgente = True, 1, 0) & _
            "," & IIf(p.Compensador = True, 1, 0) & "," & p.Pasillo & "," & NumSql(p.Inset) & "," & p.Idpromocion & ")"
            mda.InsertCommand = New SqlCommand(cad)
            mda.InsertCommand.Connection = mcon
            mda.InsertCommand.ExecuteNonQuery()
            'si es una reposicion de lente lo grabamos en su tabla correspondiente y grabamos los calculos del pedido padre para su impresion
            If p.Padre <> 0 Then
                mda.InsertCommand.CommandText = "INSERT INTO t_reposicion_pedido (id_padre,id_pedido) VALUES (" & p.Padre & "," & p.id & ")"
                mda.InsertCommand.ExecuteNonQuery()
                mda.InsertCommand.CommandText = "INSERT INTO t_calculos_pedido select " & p.id & ",parametro,valor from t_calculos_pedido where id_pedido=" & p.Padre
                mda.InsertCommand.ExecuteNonQuery()
                mda.InsertCommand.CommandText = "INSERT INTO t_formas_pedido select " & p.id & ",forma from t_formas_pedido where id_pedido=" & p.Padre
                mda.InsertCommand.ExecuteNonQuery()
            End If
            ' si hay montaje 

            If stock = True Then
                If i = 0 Then
                    ' como se trata de un pedido de stock vamos a grabar el id_pedido_stock
                    idstock = getMaxId("id_pedido_stock", "t_pedidos_stock") + 1
                    If mcon.State = ConnectionState.Closed Then
                        mcon.Open()
                    End If
                    cad = "INSERT INTO t_pedidos_stock (id_pedido_stock,id_cliente,fecha,servido) VALUES (" & idstock & "," & p.id_cliente & "," & FechaAcadena(Now.Date) & ",0)"
                    mda.InsertCommand.CommandText = cad
                    mda.InsertCommand.Connection = mcon
                    mda.InsertCommand.ExecuteNonQuery()
                End If
                cad = "INSERT INTO t_lineas_pedido_stock (id_pedido_stock,id_pedido) VALUES (" & idstock & "," & id & ")"
                mda.InsertCommand.CommandText = cad
                mda.InsertCommand.Connection = mcon
                mda.InsertCommand.ExecuteNonQuery()
            End If
            'vamos a ver si el modelo en cuestion tiene puntos, solo para los pedidos sin cargo


            'If p.SinCargo = False Then
            '    Dim PuntosByModelo As Integer = 0
            '    PuntosByModelo = GetPuntosByModelo(p.id_modelo)
            '    If PuntosByModelo > 0 Then 'grabamos los puntos de dicho pedido
            '        mda.InsertCommand.CommandText = "INSERT INTO t_puntos_pedido (id_pedido,puntos) VALUES (" & p.id & "," & PuntosByModelo & ")"
            '        mda.InsertCommand.ExecuteNonQuery()
            '        puntos += PuntosByModelo
            '    End If
            'End If
            'si hay lente reservada updateo la lente para marcar una reserva
            cad = "update t_lentes_stock set reservas=reservas + 1 where id_producto=" & p.Id_reserva
            mda.UpdateCommand = New SqlCommand(cad)
            mda.UpdateCommand.Connection = mcon
            mda.UpdateCommand.ExecuteNonQuery()
            'ahora grabamos una nueva orden de trabajo
            cad = "insert into t_ordenes_trabajo (id_orden,id_pedido,fecha,fs_almacen,hs_almacen,paso,fs_fabrica,hs_fabrica,fs_coloracion,hs_coloracion,fs_tratamiento,hs_tratamiento,fe_fabrica" & _
            ",he_fabrica,fe_coloracion,he_coloracion,fe_endurecimiento,he_endurecimiento,fs_endurecimiento,hs_endurecimiento," & _
            "fe_antireflejo,he_antireflejo,fs_antireflejo,hs_antireflejo,fe_toplight,he_toplight,fs_toplight,hs_toplight,id_incidencia,hora)" & _
            " VALUES (  1," & id & "," & FechaAcadena(Now.Date) & ",0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," & Now.Hour & Format(Now.Minute, "00") & ")"
            mda.InsertCommand = New SqlCommand(cad)
            mda.InsertCommand.Connection = mcon
            mda.InsertCommand.ExecuteNonQuery()
            'ahora vemos si tiene datos IOT
            If p.PrecalHorizontal <> "" Or p.Precalvertical <> "" Or p.hMontaje <> "" Or p.ContornoMontaje <> "" Or p.puente <> "" Or p.AnguloFacial <> "" Or p.AnguloPantoscopico <> "" Or p.DistanciaVertice <> "" Or p.DistanciaCerca <> "" Then
                mda.InsertCommand.CommandText = "INSERT INTO t_IOT_pedidos (id_pedido,puente,h_montaje,contorno_montaje,angulo_pantoscopico,angulo_facial,distancia_vertice,distancia_cerca,precal_horizontal,precal_vertical) VALUES (" & p.id & _
                "," & strsql(p.puente) & "," & strsql(p.hMontaje) & "," & strsql(p.ContornoMontaje) & "," & strsql(p.AnguloPantoscopico) & "," & strsql(p.AnguloFacial) & "," & strsql(p.DistanciaVertice) & "," & strsql(p.DistanciaCerca) & "," & strsql(p.PrecalHorizontal) & "," & strsql(p.Precalvertical) & ")"
                mda.InsertCommand.ExecuteNonQuery()
            End If
            'ahora vemos si lleva precalibrado
            If Not p.Precal.IdMontura = 0 Then
                With p.Precal
                    mda.InsertCommand.CommandText = "INSERT INTO t_precalibrados (id_pedido,id_montura,hbox_d,hbox_i,vbox_d,vbox_i,dbl,ocht_d,ocht_i,ipd_d,ipd_i,minedg_d,minedg_i,forma_precal) VALUES (" & _
                    p.id & "," & .IdMontura & "," & NumSql(.HorizontalD) & "," & NumSql(.HorizontalI) & "," & NumSql(.VerticalD) & "," & NumSql(.VerticalI) & "," & NumSql(.Puente) & "," & _
                    NumSql(.AlturaPupilarD) & "," & NumSql(.AlturaPupilarI) & "," & NumSql(.NasoPupilarD) & "," & NumSql(.NasoPupilarI) & "," & NumSql(.EspesorBordeD) & "," & NumSql(.EspesorBordeI) & "," & .idFormaPrecal & ")"
                End With
                mda.InsertCommand.ExecuteNonQuery()
            End If
            If p.Puntos <> 0 Then

                mda.InsertCommand.CommandText = "INSERT INTO t_puntos_pedido (id_pedido,puntos) VALUES (" & p.id & "," & p.Puntos & ")"
                mda.InsertCommand.ExecuteNonQuery()
            End If
            If p.PedidoMontaje.Derecho <> 0 Or p.PedidoMontaje.Izquierdo <> 0 Then
                If p.ojo = "D" Then
                    p.PedidoMontaje.Derecho = id
                    If p.pareja <> 0 Then
                        p.PedidoMontaje.Izquierdo = id + 1

                    End If
                Else
                    p.PedidoMontaje.Izquierdo = id
                End If
                GrabaMontaje(p.PedidoMontaje)

            End If

        Next
        'ahora si los puntos son mayor que cero los sumamos al cliente

        If mcon.State = ConnectionState.Open Then
            mcon.Close()
        End If
        mda = Nothing

    End Sub
    Public Function GetPuntosByPedido(ByVal idpedido As Integer) As Integer
        'devuelve los puntos de un modelo para intercambiar por regalos
        Dim puntos As Integer = 0
        Dim ConeccionCerrada As Boolean = False
        Dim cad As String = "select isnull(sum(puntos),0) from t_puntos_pedido where id_pedido=" & idpedido
        Dim cmd As New SqlCommand(cad, mcon)
        If mcon.State = ConnectionState.Closed Then
            ConeccionCerrada = True
            mcon.Open()
        End If
        'mcon.Open()
        puntos = cmd.ExecuteScalar
        If ConeccionCerrada = True Then
            mcon.Close()
        End If
        Return puntos
    End Function
    Public Sub GrabaSistemaPuntos(ByVal inicio As Date, ByVal fin As Date, ByVal canje As Date)
        Dim cad As String = "UPDATE t_puntos set inicio=" & FechaAcadena(inicio) & ",fin=" & FechaAcadena(fin) & ",canje=" & FechaAcadena(canje)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetModelosPuntos() As DataTable
        Dim cad As String = "select id_lente as id_modelo,nombre,puntos from t_modelos where puntos<>0 order by nombre"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetModelosregalo() As DataTable
        Dim cad As String = "select id_modelo,nombre,t_puntos_regalo.puntos from t_puntos_regalo INNER JOIN t_modelos On t_modelos.id_lente=t_puntos_regalo.id_modelo  order by nombre"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Sub GrabaPuntosModelo(ByVal Modelo As Integer, ByVal puntos As Integer)
        Dim cad As String = "UPDATE t_modelos set puntos=" & puntos & " where id_lente=" & Modelo
        Dim cmd As New SqlCommand(cad, mcon)

        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Sub GrabaPuntosRegalo(ByVal Modelo As Integer, ByVal puntos As Integer)
        Dim cad As String = " DECLARE @existe integer" & vbNewLine & _
        "select @existe=count(*) from t_puntos_regalo where id_modelo=" & Modelo & vbNewLine & _
        "if @existe=0 " & vbNewLine & _
        "INSERT INTO t_puntos_regalo (id_modelo,puntos) VALUES (" & Modelo & "," & puntos & ")" & vbNewLine & _
        "if @existe=1 " & vbNewLine & _
        "UPDATE t_puntos_regalo set puntos=" & puntos & " where id_modelo=" & Modelo & vbNewLine & _
        "DELETE FROM t_puntos_regalo where puntos=0"

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Function GetSistemaPuntos() As DataTable
        Dim cad As String = "select * from t_puntos"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetPuntosRegaloByModelo(ByVal idmodelo As Integer) As Integer
        'devuelve los puntos de un modelo para intercambiar por regalos
        Dim puntos As Integer = 0
        Dim ConeccionCerrada As Boolean = False
        Dim cad As String = "select isnull(puntos,0) from t_puntos_regalo where id_modelo=" & idmodelo
        Dim cmd As New SqlCommand(cad, mcon)
        If mcon.State = ConnectionState.Closed Then
            ConeccionCerrada = True
            mcon.Open()
        End If
        'mcon.Open()
        puntos = cmd.ExecuteScalar
        If ConeccionCerrada = True Then
            mcon.Close()
        End If
        Return puntos
    End Function
    Public Function GetPuntosByModelo(ByVal idmodelo As Integer) As Integer
        'devuelve los puntos de un modelo para intercambiar por regalos
        Dim puntos As Integer = 0
        Dim ConeccionCerrada As Boolean = False
        Dim cad As String = "select puntos from t_modelos where id_lente=" & idmodelo
        Dim cmd As New SqlCommand(cad, mcon)
        If mcon.State = ConnectionState.Closed Then
            ConeccionCerrada = True
            mcon.Open()
        End If
        'mcon.Open()
        puntos = cmd.ExecuteScalar
        If ConeccionCerrada = True Then
            mcon.Close()
        End If
        Return puntos
    End Function
    Public Function InformeMontajes(ByVal fecini As Integer, ByVal fecfin As Integer) As DataTable
        Dim cad As String = "select dbo.fechaexcel(t_pedidos_montajes.fecha) as [Fecha Pedido],id_pedido_montaje,dbo.fechaexcel(t_albaranes.fecha) as [fecha_salida],datediff(d,dbo.fecha(t_pedidos_montajes.fecha),dbo.fecha(t_albaranes.fecha)) as dias from t_pedidos_montajes INNER JOIN t_lineas_albaran ON t_lineas_albaran.id_pedido=t_pedidos_montajes.id_pedido_montaje INNER JOIN t_albaranes ON t_lineas_albaran.id_albaran=t_albaranes.id_albaran where t_lineas_albaran.montaje=1 and t_albaranes.fecha>=" & fecini & " and t_albaranes.fecha<=" & fecfin & " group by t_pedidos_montajes.fecha,id_pedido_montaje, t_albaranes.fecha"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub MarcaPedidoStockServido(ByVal id As Integer)
        Dim cad As String = "update t_pedidos_stock set servido=1 where id_pedido_stock=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function GetPedidosStockByIdCLiente(ByVal id As Integer) As DataTable
        Dim cad As String = "select id_pedido_stock,dbo.fecha(fecha) as fecha,(select count(*) From t_lineas_pedido_stock where id_pedido_stock=t_pedidos_stock.id_pedido_stock) as Lentes,servido from t_pedidos_stock where id_cliente=" & id & " order by fecha"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetOrdenesTrabajo(ByVal idped As Long) As clsOrdenesTrabajo
        Dim cad As String = "select *,(select hora from t_pedidos where id_pedido=t_ordenes_trabajo.id_pedido) as horapedido from t_ordenes_trabajo where id_pedido=" & idped & " order by id_orden"
        Dim tb As New DataTable
        Dim mOrdenes As New clsOrdenesTrabajo
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Dim mrow As DataRow
        For Each mrow In tb.Rows
            Dim mOrden As New clsOrdenTrabajo
            mOrden.IdPedido = mrow("id_pedido")
            mOrden.Orden = mrow("id_orden")
            mOrden.fecha = mrow("fecha")
            If mOrden.Orden = 1 Then
                mOrden.Hora = mrow("horapedido")
            Else
                mOrden.Hora = IIf(IsDBNull(mrow("hora")), 1200, mrow("hora"))
            End If

            mOrden.FeFabrica = mrow("fe_fabrica")
            mOrden.HeFabrica = mrow("he_fabrica")
            mOrden.FsFabrica = mrow("fs_fabrica")
            mOrden.HsFabrica = mrow("hs_fabrica")
            mOrden.FeColor = mrow("fe_coloracion")
            mOrden.HeColor = mrow("he_coloracion")
            mOrden.FsColor = mrow("fs_coloracion")
            mOrden.HsColor = mrow("hs_coloracion")
            mOrden.FeEndurecido = mrow("fe_endurecimiento")
            mOrden.HeEndurecido = mrow("he_endurecimiento")
            mOrden.FsEndurecido = mrow("fs_endurecimiento")
            mOrden.HsEndurecido = mrow("hs_endurecimiento")
            mOrden.FeAntireflejo = mrow("fe_antireflejo")
            mOrden.HeAntireflejo = mrow("he_antireflejo")
            mOrden.FsAntireflejo = mrow("fs_antireflejo")
            mOrden.HsAntireflejo = mrow("hs_antireflejo")
            mOrden.FeToplight = mrow("fe_toplight")
            mOrden.HeToplight = mrow("he_toplight")
            mOrden.FsToplight = mrow("fs_toplight")
            mOrden.HsToplight = mrow("hs_toplight")
            mOrden.FeMontaje = mrow("fe_montaje")
            mOrden.HeMontaje = mrow("he_montaje")
            mOrden.FsMontaje = mrow("fs_montaje")
            mOrden.HsMontaje = mrow("hs_montaje")
            'mOrden.FeTratamiento = mrow("fe_tratamiento")
            'mOrden.HeTratamiento = mrow("he_tratamiento")
            mOrden.FsTratamiento = mrow("fs_tratamiento")
            mOrden.HsTratamiento = mrow("hs_tratamiento")
            mOrden.Incidencia = mrow("id_incidencia")
            mOrden.paso = mrow("paso")
            mOrden.FsAlmacen = 0 + mrow("fs_almacen")
            mOrden.HsAlmacen = 0 + mrow("hs_almacen")
            mOrden.FsCalidad = 0 + mrow("fs_calidad")
            mOrden.HsCalidad = 0 + mrow("hs_calidad")
            mOrden.FeExterna = mrow("fe_externa")
            mOrden.HeExterna = mrow("He_externa")
            mOrden.FsExterna = mrow("fs_externa")
            mOrden.HsExterna = mrow("Hs_externa")
            mOrden.FeRetoque = mrow("fe_retoque")
            mOrden.HoraEntradaRetoque = mrow("he_retoque")
            mOrden.FsRetoque = mrow("fs_retoque")
            mOrden.HoraSalidaRetoque = mrow("hs_retoque")
            mOrden.UsuarioSalidaExterna = mrow("usr_s_externa")
            mOrden.UsuarioEntradaExterna = mrow("usr_e_externa")
            mOrden.UsuarioSalidaAlmacen = 0 + mrow("usr_s_almacen")
            mOrden.UsuarioEntradaFabrica = 0 + mrow("usr_e_fabrica")
            mOrden.UsuarioSalidaFabrica = 0 + mrow("usr_s_fabrica")
            mOrden.UsuarioEntradaColor = 0 + IIf(IsDBNull(mrow("usr_e_color")), 0, mrow("usr_e_color"))
            mOrden.UsuarioSalidaColor = 0 + mrow("usr_s_color")
            mOrden.usuarioEntradaEndurecido = 0 + mrow("usr_e_endurecimiento")
            mOrden.UsuarioSalidaEndurecido = 0 + mrow("usr_s_endurecimiento")
            mOrden.UsuarioEntradaAntireflejo = 0 + mrow("usr_e_antireflejo")
            mOrden.UsuarioSalidaAntireflejo = 0 + mrow("usr_s_antireflejo")
            mOrden.UsuarioCalidad = 0 + mrow("Usr_s_calidad")
            mOrden.UsuariEntradaMontaje = mrow("usr_e_montaje")
            mOrden.UsuarioSalidaMontaje = mrow("usr_s_montaje")
            mOrden.UsuarioEntradaRetoque = mrow("usr_e_retoque")
            mOrden.UsuarioSalidaRetoque = mrow("usr_s_retoque")
            mOrdenes.add(mOrden)
            mOrden = Nothing

        Next
        Return mOrdenes
    End Function
    Public Function GetUltimaOrdenByPedido(ByVal id As Integer) As DataTable
        Dim cad As String = "select top 1 *,(select hora from t_pedidos where id_pedido=" & id & ") as horapedido from t_ordenes_trabajo where id_pedido=" & id & " order by id_orden desc"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function

    Public Sub GrabaMonitor(ByVal id As Integer, ByVal lineas As Integer, ByVal segundos As Integer, ByVal Correccion As Integer)
        Dim cad As String = ""

        If id = 0 Then
            'insertaremos un nuevo monitor en su dia
        Else
            cad = "Update t_monitores_informacion set segundos=" & segundos & ",lineas=" & lineas & ",correccion=" & Correccion & " where id_monitor=" & id

        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetMonitores() As DataTable
        Dim cad As String = "select * from t_monitores_informacion order by id_monitor"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function
    'Public Function GetPedidoById(ByVal idPedido As Long) As clsPedido
    '    '********************************************************************************
    '    '***************   pedido a medias
    '    '*****  el pedido no se carga completo , solo se carga  la parte para ver pareja
    '    '************************* realizar de nuevo la busqueda e introducir todos los 
    '    'parametros
    '    Dim cad As String
    '    Dim mP As New clsPedido()
    '    cad = "select * from t_pedidos where id_pedido=" & idPedido
    '    Dim mda As New SqlDataAdapter(New SqlCommand(cad))
    '    Dim tb As New DataTable()
    '    mda.SelectCommand.Connection = mcon
    '    mcon.Open()
    '    mda.Fill(tb)
    '    mcon.Close()
    '    mP.adicion = tb.Rows(0)("adicion")
    '    mP.cilindro = tb.Rows(0)("cilindro")
    '    mP.eje = tb.Rows(0)("eje")
    '    mP.descentramiento = tb.Rows(0)("descentramiento")
    '    mP.esfera = tb.Rows(0)("esfera")
    '    mP.prisma = tb.Rows(0)("prisma")
    '    mP.ojo = tb.Rows(0)("ojo")
    '    mp.id = idPedido

    '    Return mP

    ' End Function
    Public Sub marcarReserva(ByVal idlenteStock As Long, ByVal cantidad As Integer)
        ' Dim cad As String
        mcon.Open()

        'cad = "update t_lentes_stock set reserva = " & cantidad
        'mda.InsertCommand = New SqlCommand(cad)
        'mda.InsertCommand.Connection = mcon
        'mda.InsertCommand.ExecuteNonQuery()
        'Next
        mcon.Close()
    End Sub

    Public Function CargaRangos(ByVal tipolente As Long, ByVal diametro As Integer, ByVal tratamiento As Integer) As DataTable
        'carga el rango de fabricacion de la lente
        Dim mda As New SqlDataAdapter(New SqlCommand("select * from t_rangos where " & _
        "id_modelo = " & tipolente & " and diametro=" & diametro & "  order by cilindro,esfera"))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function getMaxCilindroLente(ByVal modelo As Long, ByVal diametro As Integer, ByVal tratamiento As Integer, Optional ByVal rango As Boolean = False) As Single
        'obtiene el maximo diametro de una lente concreta
        'devuelve el maximo identificador
        Dim cad As String
        cad = "select max(cilindro) as maximo from t_lentes_stock " & _
        "where id_modelo=" & modelo & " and diametro=" & diametro & " and tratamiento=" & tratamiento
        If rango = True Then
            cad = "select max(cilindro) as maximo from t_rangos " & _
           "where id_modelo=" & modelo & " and diametro=" & diametro
        End If

        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        mda = Nothing
        If tb.Rows(0).IsNull("maximo") Then
            Return 0
        Else
            Return tb.Rows(0).Item("maximo")
        End If
    End Function

    Public Function getMaxCilindroDesecho(ByVal modelo As Long, ByVal diametro As Integer, ByVal tratamiento As Integer) As Single
        'obtiene el maximo diametro de una lente concreta
        'devuelve el maximo identificador
        Dim cad As String
        cad = "select max(cilindro) as maximo from t_almacen_desechos " & _
        "where id_modelo=" & modelo & " and diametro=" & diametro & " and id_tratamiento=" & tratamiento


        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        mda = Nothing
        If tb.Rows(0).IsNull("maximo") Then
            Return 0
        Else
            Return tb.Rows(0).Item("maximo")
        End If
    End Function

    Public Function getMaxEsferaLente(ByVal modelo As Long, ByVal diametro As Integer, ByVal tratamiento As Integer, Optional ByVal rango As Boolean = False) As Single
        'obtiene el maximo diametro de una lente concreta
        'devuelve el maximo identificador
        Dim cad As String
        cad = "select max(esfera) as maximo from t_lentes_stock " & _
        "where id_modelo=" & modelo & " and diametro=" & diametro & " and tratamiento=" & tratamiento
        If rango = True Then
            cad = "select max(esfera) as maximo from t_rangos " & _
        "where id_modelo=" & modelo & " and diametro=" & diametro
        End If

        Dim mda As New SqlDataAdapter(New SqlCommand(cad))

        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        mda = Nothing
        If tb.Rows(0).IsNull("maximo") Then
            Return 0
        Else
            Return tb.Rows(0).Item("maximo")
        End If
    End Function

    Public Function getMinEsferaLente(ByVal modelo As Long, ByVal diametro As Integer, ByVal tratamiento As Integer, Optional ByVal rango As Boolean = False) As Single
        'obtiene el maximo diametro de una lente concreta
        'devuelve el maximo identificador
        Dim cad As String
        cad = "select min(esfera) as minimo from t_lentes_stock " & _
        "where id_modelo=" & modelo & " and diametro=" & diametro & " and tratamiento=" & tratamiento
        If rango = True Then
            cad = "select min(esfera) as minimo from t_rangos " & _
        "where id_modelo=" & modelo & " and diametro=" & diametro
        End If
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        mda = Nothing
        If tb.Rows(0).IsNull("minimo") Then
            Return 0
        Else
            Return tb.Rows(0).Item("minimo")
        End If
    End Function

    Public Function getMaxEsferaDesecho(ByVal modelo As Long, ByVal diametro As Integer, ByVal tratamiento As Integer, Optional ByVal rango As Boolean = False) As Single
        'obtiene el maximo diametro de una lente concreta
        'devuelve el maximo identificador
        Dim cad As String
        cad = "select max(esfera) as maximo from t_almacen_desechos " & _
        "where id_modelo=" & modelo & " and diametro=" & diametro & " and id_tratamiento=" & tratamiento

        Dim mda As New SqlDataAdapter(New SqlCommand(cad))

        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        mda = Nothing
        If tb.Rows(0).IsNull("maximo") Then
            Return 0
        Else
            Return tb.Rows(0).Item("maximo")
        End If
    End Function
    Public Function getMinEsferaDesecho(ByVal modelo As Long, ByVal diametro As Integer, ByVal tratamiento As Integer, Optional ByVal rango As Boolean = False) As Single
        'obtiene el maximo diametro de una lente concreta
        'devuelve el maximo identificador
        Dim cad As String
        cad = "select min(esfera) as minimo from t_almacen_desechos " & _
        "where id_modelo=" & modelo & " and diametro=" & diametro & " and id_tratamiento=" & tratamiento

        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        mda = Nothing
        If tb.Rows(0).IsNull("minimo") Then
            Return 0
        Else
            Return tb.Rows(0).Item("minimo")
        End If
    End Function
    Public Function CargaEsferasPositivasRango(ByVal tipoLente As Long, ByVal diametro As Integer) As DataTable
        'carga el rango de fabricacion de la lente
        Dim mda As New SqlDataAdapter(New SqlCommand("select * from t_rangos where " & _
        "id_modelo = " & tipoLente & " and diametro=" & diametro & " and esfera>=0 order by cilindro,esfera"))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function CargaEsferasNegativasRango(ByVal tipoLente As Long, ByVal diametro As Integer) As DataTable
        'carga el rango de fabricacion de la lente
        Dim mda As New SqlDataAdapter(New SqlCommand("select * from t_rangos where " & _
        "id_modelo = " & tipoLente & " and diametro=" & diametro & "  and esfera<0 order by cilindro,esfera"))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function getPedidoStockByID(ByVal id As Integer) As clsPedidos
        Dim cad As String = "select t_lineas_pedido_stock.id_pedido from t_lineas_pedido_stock INNER JOIN T_pedidos ON t_lineas_pedido_stock.id_pedido=t_pedidos.id_pedido where id_pedido_stock=" & id & " and fecha_salida=0 order by t_lineas_pedido_stock.id_pedido"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.SelectCommand.CommandTimeout = 240
        mda.Fill(tb)
        mcon.Close()
        Dim Peds As New clsPedidos
        For Each rw As DataRow In tb.Rows
            Dim p As New clsPedido
            p = GetPedidobyId(rw("id_pedido"))
            Peds.add(p)
        Next
        Return Peds
    End Function
    Public Function GetNombreColorById(ByVal id As Integer) As String
        Dim cad As String = "select gama + ' ' + t_coloraciones.color as coloracion from t_coloraciones where id_coloracion=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim nombre As String = cmd.ExecuteScalar
        mcon.Close()
        Return nombre
    End Function

    Public Function GetPedidobyId(ByVal id_pedido As Long) As clsPedido
        Dim p As New clsPedido
        Dim cad As String
        cad = "select (select isnull(sum(puntos),0) from t_puntos_pedido where id_pedido=t_pedidos.id_pedido) as puntos,t_pedidos.*,t_clientes.nombre_comercial,t_modelos.nombre as modelo,(t_coloraciones.gama + ' ' + t_coloraciones.color) as coloracion," & _
        "t_tratamientos.nombre as nTratamiento from (((t_pedidos inner join t_clientes on t_pedidos.id_cliente= t_clientes.id_cliente) " & _
        "inner join t_modelos on t_pedidos.id_modelo = t_modelos.id_lente) " & _
        "left join t_coloraciones on t_pedidos.id_coloracion=t_coloraciones.id_coloracion) inner join t_tratamientos on " & _
        "t_pedidos.id_tratamiento=t_tratamientos.id_tratamiento where id_pedido=" & id_pedido

        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)

        If tb.Rows.Count = 0 Then
            'p = Nothing
            mcon.Close()
            Return p
        End If
        p.Montaje = False
        p.adicion = tb.Rows(0)("adicion")
        p.cilindro = tb.Rows(0)("cilindro")
        p.cliente = tb.Rows(0)("nombre_comercial")
        p.coloracion = IIf(IsDBNull(tb.Rows(0).Item("coloracion")), "Sin Coloración", tb.Rows(0)("coloracion"))
        p.descentramiento_mm = tb.Rows(0)("descentramiento_mm")
        p.descentramiento_grados = tb.Rows(0)("descentramiento_grados")
        p.prisma_valor = tb.Rows(0)("prisma_valor")
        p.prisma_eje = tb.Rows(0)("prisma_eje")
        p.espesor_ranurar = tb.Rows(0)("espesor_ranurar")
        p.espesor_taladrar = tb.Rows(0)("espesor_taladrar")
        p.base_curva = tb.Rows(0)("base_curva")
        p.base_plana = tb.Rows(0)("base_plana")
        p.esmerilado = tb.Rows(0)("esmerilado")
        p.precalibrado = tb.Rows(0)("precalibrado")
        p.diametro = tb.Rows(0)("diametro")
        p.Inset = tb.Rows(0)("inset")
        p.Eliptico = CBool(tb.Rows(0)("eliptico"))
        p.Urgente = CBool(IIf(IsDBNull(tb.Rows(0)("urgente")), 0, tb.Rows(0)("urgente")))
        p.eje = tb.Rows(0)("eje")
        p.esfera = tb.Rows(0)("esfera")
        p.Fechapedido = cadenaAfecha(tb.Rows(0)("fecha"))
        p.horaPedido = tb.Rows(0)("hora")
        If p.horaPedido.Length = 3 Then p.horaPedido = "0" & p.horaPedido
        p.id_cliente = tb.Rows(0)("id_cliente")
        p.id_coloracion = tb.Rows(0)("id_coloracion")
        p.id_modelo = tb.Rows(0)("id_modelo")
        p.id = tb.Rows(0)("id_pedido")
        p.Id_reserva = tb.Rows(0)("id_reservado")
        p.id_tratamiento = tb.Rows(0)("id_tratamiento")
        p.intensidad = tb.Rows(0)("intensidad")
        p.modelo = tb.Rows(0)("modelo")
        p.modo = tb.Rows(0)("modo")
        p.ojo = tb.Rows(0)("ojo")
        p.pareja = tb.Rows(0)("pareja")
        p.Referencia = tb.Rows(0)("referencia")
        p.FechaSalida = cadenaAfecha(tb.Rows(0)("fecha_salida"))
        p.HoraSalida = "" & tb.Rows(0)("hora_salida")
        p.FechaSalidaColoracion = "" & tb.Rows(0)("fecha_coloracion")
        p.HoraSalidaColoracion = "" & tb.Rows(0)("hora_coloracion")
        p.Lenticular = tb.Rows(0)("Lenticular")
        p.Compensador = CBool(tb.Rows(0)("compensador"))
        'p.SalidaAlmacen = cadenaAfecha(tb.Rows(0)("fsa"))
        'p.salidaAlmacenHora = tb.Rows(0)("hsa")
        p.tratamiento = tb.Rows(0)("ntratamiento")
        p.Anulado = tb.Rows(0)("anulado")
        p.CausaAnulacion = "" & tb.Rows(0)("causa")
        p.id_usuario = tb.Rows(0)("id_usuario")
        p.FechaSalidaFabrica = "" & cadenaAfecha(tb.Rows(0)("f_salida_fabrica"))
        p.HoraSalidaFabrica = "" & tb.Rows(0)("h_salida_fabrica")
        p.FechaSalidaTratamiento = "" & cadenaAfecha(tb.Rows(0)("f_salida_tratamiento"))
        p.HoraSalidaTratamiento = "" & tb.Rows(0)("h_salida_tratamiento")
        If Not IsDBNull(tb.Rows(0)("observaciones")) Then p.Observaciones = tb.Rows(0)("observaciones")
        p.SinCargo = IIf(IsDBNull(tb.Rows(0)("Sin_cargo")), 0, tb.Rows(0)("sin_cargo"))
        p.Tallado = IIf(IsDBNull(tb.Rows(0)("tallado")), "", tb.Rows(0)("tallado"))
        p.PedidoWeb = tb.Rows(0)("Pedido_Web")
        p.A_Filo = tb.Rows(0)("a_filo")
        p.Albaran = tb.Rows(0)("id_albaran")
        p.Montaje = False ' no se trata de un pedido de montaje
        p.LlevaMontaje = CBool(tb.Rows(0)("montaje"))
        'ahora vemos si existe fecha de compromiso o no
        p.FechaCompromiso = (tb.Rows(0)("compromiso"))
        p.Pasillo = tb.Rows(0)("pasillo")
        p.Idpromocion = tb.Rows(0)("id_promocion")
        p.Puntos = tb.Rows(0)("puntos")
        'ahora vamos a ver si es un pedido IOT
        tb.Clear()
        mda.SelectCommand.CommandText = "select * from t_IOT_pedidos where id_pedido=" & p.id
        mda.Fill(tb)
        If tb.Rows.Count > 0 Then
            p.AnguloPantoscopico = tb.Rows(0)("angulo_pantoscopico")
            p.AnguloFacial = tb.Rows(0)("angulo_facial")
            p.DistanciaCerca = tb.Rows(0)("distancia_cerca")
            p.DistanciaVertice = tb.Rows(0)("distancia_vertice")
            p.hMontaje = tb.Rows(0)("h_montaje")

        End If
        mda.SelectCommand.CommandText = "SELECT * from t_precalibrados where id_pedido=" & p.id
        tb.Clear()
        mda.Fill(tb)
        If tb.Rows.Count > 0 Then
            p.Precal.IdMontura = tb.Rows(0)("id_montura")
            p.Precal.HorizontalD = tb.Rows(0)("hbox_d")
            p.Precal.HorizontalI = tb.Rows(0)("hbox_i")
            p.Precal.VerticalD = tb.Rows(0)("vbox_d")
            p.Precal.VerticalI = tb.Rows(0)("vbox_i")
            p.Precal.Puente = tb.Rows(0)("dbl")
            p.Precal.AlturaPupilarD = tb.Rows(0)("ocht_d")
            p.Precal.AlturaPupilarI = tb.Rows(0)("ocht_i")
            p.Precal.NasoPupilarD = tb.Rows(0)("ipd_d")
            p.Precal.NasoPupilarI = tb.Rows(0)("ipd_i")
            p.Precal.EspesorBordeD = tb.Rows(0)("minedg_d")
            p.Precal.EspesorBordeI = tb.Rows(0)("minedg_i")
            p.Precal.idFormaPrecal = tb.Rows(0)("forma_precal")
        End If
        p.Padre = EspedidoReposicion(p.id)
        'p.FechaSalidaAlmacen = "" & cadenaAfecha(tb.Rows(0)("f_salida_almacen"))
        'p.HoraSalidaAlmacen = "" & tb.Rows(0)("h_salida_almacen")
        'ahora vamos a buscar si ha salido en porte
        p.Fronto.getFronto(id_pedido)

        mcon.Close()
        p = PortePedido(p)
        Return p
    End Function
    Public Function GetTipoMonturaByid(ByVal id As Integer) As String
        Dim cad As String = "select montura from t_espesores_montura where id_montura=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        GetTipoMonturaByid = cmd.ExecuteScalar
        mcon.Close()
    End Function

    Public Function PortePedido(ByVal p As clsPedido) As clsPedido
        Dim cad As String
        'If p.fechaPedido >= "01/01/2010" Then
        cad = "select * from t_portes where id_porte in ( select id_porte from t_lineas_porte where id_albaran =" & p.Albaran & ")"

        Dim mda2 As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb2 As New DataTable
        mcon.Open()
        mda2.Fill(tb2)
        mcon.Close()
        If tb2.Rows.Count > 0 Then
            p.FechaSalidaPorte = tb2.Rows(0)("fecha")
            p.HoraSalidaPorte = Format(tb2.Rows(0)("hora"), "0000")
            p.Agencia = tb2.Rows(0)("agencia")
        End If
        tb2.Dispose()
        mda2.Dispose()
        Return p
    End Function
    Public Function GetProveedorByCodBarra(ByVal CodBarra As String) As Integer

        Dim cmd As New SqlCommand("select id_proveedor from t_codigos_barra where cod_barra=" & strsql(CodBarra), mcon)
        mcon.Open()
        GetProveedorByCodBarra = cmd.ExecuteScalar
        mcon.Close()

    End Function
    Public Function GetProveedorById(ByVal idProv As Integer) As String
        Dim prov As String
        Dim cmd As New SqlCommand("select proveedor from Proveedores.dbo.t_proveedores where id_proveedor=" & idProv, mcon)

        mcon.Open()
        prov = cmd.ExecuteScalar
        mcon.Close()
        Return prov
    End Function
    Public Function GetSemiterminadobyID(ByVal id As Long) As clsSemiterminado
        Dim cad As String = "select *, (select nombre from t_modelos where id_lente=id_modelo) as Modelo" & _
        " from t_semiterminados where id_lente=" & id
        Dim cmd As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        Dim Semi As New clsSemiterminado
        mcon.Open()
        cmd.Fill(tb)
        mcon.Close()
        If tb.Rows.Count > 0 Then
            Semi.IdLente = tb.Rows(0)("id_lente")
            Semi.idModelo = tb.Rows(0)("id_modelo")
            Semi.Modelo = tb.Rows(0)("modelo")
            Semi.Base = tb.Rows(0)("base")
            Semi.Ojo = tb.Rows(0)("ojo")
            Semi.Stock = tb.Rows(0)("stock")
            Semi.Diametro = tb.Rows(0)("diametro")
            Semi.Adicion = tb.Rows(0)("adicion")
        End If
        Return Semi
    End Function
    Public Function GrabaSalidaSemiterminado(ByVal idpedido As Long, ByVal lente As Integer, ByVal idProv As Integer, ByVal codigoBarras As String, ByVal lote As String) As Boolean
        Dim fecha As Integer = FechaAcadena(Now.Date)
        Dim hora As String = Format(Now.Hour, "00") & Format(Now.Minute, "00")
        Dim cad As String = " UPDATE t_ordenes_trabajo set fs_almacen=" & fecha & ",hs_almacen=" & hora & _
            ",usr_s_almacen=" & mUsuario.id & ",fe_fabrica=" & fecha & ",he_fabrica=" & hora & _
            ",usr_e_fabrica=" & mUsuario.id & " where id_pedido=" & idpedido & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & idpedido & ")" & vbNewLine
        cad = cad & "INSERT INTO t_salidas_semiterminados (id_pedido,id_lente,id_proveedor,id_orden,fecha,cod_barras,lote) " & _
        " select " & idpedido & "," & lente & "," & idProv & ", isnull(max(id_orden),0)," & FechaAcadena(Now.Date) & "," & strsql(codigoBarras) & "," & strsql(lote) & " from t_ordenes_trabajo where id_pedido=" & idpedido & vbNewLine
        cad = cad & " UPDATE t_semiterminados set stock=stock-1 where id_lente=" & lente & vbNewLine & " UPDATE t_lotes_semiterminado set salida=salida+1 where id_semiterminado=" & lente & " and lote like " & strsql(lote)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Try
            cmd.ExecuteNonQuery()
            'ahora updateamos el stock a -1 y las salidas de semiterminados del lote a menos uno
            mcon.Close()
            Return True
        Catch ex As Exception
            mcon.Close()
            Return False
        End Try

    End Function
    Public Function GrabaSalidaStock(ByVal pedido As clsPedido, ByVal lente As clsLenteStock, ByVal idProv As Integer, ByVal lote As String) As Boolean
        Dim fecha As Integer = FechaAcadena(Now.Date)
        Dim hora As String = Format(Now.Hour, "00") & Format(Now.Minute, "00")
        'Dim cad As String = " UPDATE t_ordenes_trabajo set fs_almacen=" & fecha & ",hs_almacen=" & hora & _
        '    ",usr_s_almacen=" & mUsuario.id & ",fe_fabrica=" & fecha & ",he_fabrica=" & hora & _
        '    ",usr_e_fabrica=" & mUsuario.id & " where id_pedido=" & pedido.id & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & idpedido & ")" & vbNewLine
        'cad = cad & "INSERT INTO t_salidas_semiterminados (id_pedido,id_lente,id_proveedor,id_orden,fecha,cod_barras,lote) " & _
        '" select " & idpedido & "," & lente & "," & idProv & ", isnull(max(id_orden),0)," & FechaAcadena(Now.Date) & "," & strsql(codigoBarras) & "," & strsql(lote) & " from t_ordenes_trabajo where id_pedido=" & idpedido & vbNewLine
        '' cad = cad & " UPDATE t_semiterminados set stock=stock-1 where id_lente=" & lente & vbNewLine & " UPDATE t_lotes_semiterminado set salida=salida+1 where id_semiterminado=" & lente & " and lote like " & strsql(lote)
        Dim cad As String = "BEGIN TRY " & vbNewLine & "BEGIN TRAN SalidaLenteStock" & vbNewLine
        Dim cmd As New SqlCommand(cad, mcon)
        'Dim tran As SqlTransaction

       
        mcon.Open()
        'tran = mcon.BeginTransaction(IsolationLevel.ReadCommitted)
        'cmd.Transaction = tran
        Try
            'hay que grabar el coste del pedido
            cmd.CommandText &= "INSERT INTO t_costos_pedido (id_pedido,paso,coste) VALUES (" & pedido.id & ",'STOCK'," & NumSql(lente.PrecioCompra) & ")" & vbNewLine
            'Updateamos el almacen de Stock
            cmd.CommandText &= "UPDATE t_lentes_stock set stock=stock-1 where id_producto=" & lente.id & vbNewLine
            'updateamos la orden de trabajo
            cmd.CommandText &= "UPDATE t_ordenes_trabajo set fs_almacen=" & fecha & ",hs_almacen=" & hora & _
        ",usr_s_almacen=" & mUsuario.id & " where id_pedido=" & pedido.id & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & pedido.id & ")" & vbNewLine

            'tenemos que comprobar si la lente lleva el mismo tratamiento que el pedido, que no lleva color ni va a montaje
            If pedido.id_coloracion = 0 And pedido.id_tratamiento = lente.id_tratamiento And pedido.LlevaMontaje = False Then
                'damos salida calidad
                cmd.CommandText &= "UPDATE t_ordenes_trabajo set fs_calidad=" & fecha & ",hs_calidad=" & hora & _
                       ",usr_s_calidad=" & mUsuario.id & " where id_pedido=" & pedido.id & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & pedido.id & ")" & vbNewLine
                'vemos si leva pareja y esta ya ha salido de calidad
                If pedido.pareja <> 0 Then

                    'tenemos que darle salida distribucion y el coste del sobre a la lente y a su pareja
                    If pedido.LlevaMontaje = False Then
                        cmd.CommandText &= " DECLARE @Calidad as integer" & vbNewLine & "Select @calidad=fs_calidad from t_ordenes_trabajo where id_pedido=" & pedido.pareja & " and id_orden= (select max(id_orden) from t_ordenes_trabajo where id_pedido=" & pedido.pareja & ")"
                        cmd.CommandText &= "if @calidad<>0" & vbNewLine & "BEGIN" & vbNewLine
                        cmd.CommandText &= "Update t_pedidos set fecha_salida=" & fecha & ",hora_salida=" & hora & " where id_pedido in (" & pedido.id & "," & pedido.pareja & ")" & vbNewLine
                        cmd.CommandText &= "INSERT INTO t_costos_pedido select " & pedido.id & ",'SOBRE',sobre from t_costes_lente" & vbNewLine
                        cmd.CommandText &= "INSERT INTO t_costos_pedido select " & pedido.pareja & ",'SOBRE',sobre from t_costes_lente" & vbNewLine & "END" & vbNewLine
                    End If
                Else
                    'le damos distribucion y le metemos el gasto del sobre si no lleva montaje
                    If pedido.LlevaMontaje = False Then
                        cmd.CommandText &= "Update t_pedidos set fecha_salida=" & fecha & ",hora_salida=" & hora & " where id_pedido=" & pedido.id & vbNewLine
                        cmd.CommandText &= "INSERT INTO t_costos_pedido select " & pedido.id & ",'SOBRE',sobre from t_costes_lente" & vbNewLine
                    End If
                End If
            End If
            'grabamos la salida del almacen de stock
            cmd.CommandText &= " INSERT INTO t_salida_stock select " & pedido.id & ",(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & pedido.id & ")," & lente.id & "," & idProv & "," & strsql(lote) & "," & ConvertDateToTimeStamp(Now.Date) & vbNewLine

            cmd.CommandText &= "COMMIT TRAN SalidaLenteStock" & vbNewLine & "END TRY" & vbNewLine & "BEGIN CATCH" & vbNewLine & "ROLLBACK TRAN SalidaLenteStock" & vbNewLine & "END CATCH"
            cmd.ExecuteNonQuery()
            'ahora updateamos el stock a -1 y las salidas de semiterminados del lote a menos uno
            mcon.Close()
            Return True
        Catch ex As Exception
            Err.Clear()
            mcon.Close()
            Return False
        End Try

    End Function

    Public Function GetGruposOpticos() As DataTable
        Dim cad As String = "select * from t_grupos_opticos order by grupo_optico"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    Public Function GetGruposOpticosFacturar() As DataTable
        Dim cad As String = "select *,isnull((select nombre_comercial from t_clientes where id_cliente=t_grupos_opticos.id_cliente),'') as cliente from t_grupos_opticos  where factura=1 order by grupo_optico"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    Public Function GetGrupoOpticoByid(ByVal id As Integer) As DataTable
        Dim cad As String = "select * from t_grupos_opticos where id_grupo=" & id
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function

    Public Function GetNombreGrupoOpticoByid(ByVal id As Integer) As String
        Dim cad As String = "select isnull(grupo_optico,'') from t_grupos_opticos where id_grupo=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        GetNombreGrupoOpticoByid = cmd.ExecuteScalar
        mcon.Close()
    End Function

    Public Function GetClientesGrupo(ByVal Grupo As Integer) As DataTable
        Dim cad As String = "select id_cliente,nombre_comercial,codigo,isnull((select codigo from t_codigos_grupo_optico where id_cliente=t_clientes.id_cliente and id_grupo=" & Grupo & "),'') as Cod_Grupo from t_clientes where id_grupo=" & Grupo & " order by codigo"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    Public Function ChequeaLoteSemiterminado(ByVal Lente As Integer, ByVal lote As String) As Boolean
        Dim cad As String = "select count(*) from t_lotes_semiterminado where id_semiterminado=" & Lente & " and lote like " & strsql(lote)
        Dim Existe As Boolean
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Existe = cmd.ExecuteScalar
        mcon.Close()
        Return Existe
    End Function

    Public Sub GrabaGrupoOptico(ByVal id As Integer, ByVal grupo As String, ByVal cliente As Integer, ByVal comision As Decimal, ByVal FacturaUnica As Boolean)
        Dim cad As String
        If id = 0 Then
            cad = " INSERT INTO t_grupos_opticos select max(id_grupo)+1," & strsql(grupo) & "," & NumSql(comision) & "," & cliente & "," & IIf(FacturaUnica = True, 1, 0) & " from t_grupos_opticos"
        Else
            cad = "UPDATE t_grupos_opticos set grupo_optico=" & strsql(grupo) & ",id_cliente=" & cliente & ",comision=" & NumSql(comision) & ",factura=" & IIf(FacturaUnica = True, 1, 0) & " where id_grupo=" & id

        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub MeteSemiterminadoAlmacenByPedido(ByVal id As Integer)
        Dim cad As String = "DECLARE @idLente integer " & vbNewLine & _
        "DECLARE @idProv integer" & vbNewLine & _
        "DECLARE @lote varchar(50)" & vbNewLine & _
        "DECLARE @orden int " & vbNewLine & _
        "DECLARE @precio decimal(8,3) " & vbNewLine & _
        "select Top 1 @idlente=id_lente,@idProv=id_proveedor,@lote=lote,@orden=id_orden,@precio=dbo.PrecioSemiterminadoProveedor(id_proveedor,id_lente) from t_salidas_semiterminados where id_pedido=" & id & " order by id_orden desc" & vbNewLine & _
        "select @idLente,@idProv,@lote,@orden,@precio" & vbNewLine & _
        "--UPDATEAMOS EL ALMACEN, el lote, la orden de trabajo, el coste del semiterminado y borramos la salida de almacen " & vbNewLine & _
        "UPDATE t_semiterminados set stock=stock+1 where id_lente=@idLente" & vbNewLine & _
        "UPDATE t_ordenes_trabajo set fs_almacen=0,hs_almacen=0,fe_fabrica=0,hs_fabrica=0 where id_pedido=" & id & " and id_incidencia=0" & vbNewLine & _
        "INSERT INTO t_costos_pedido select " & id & ",'Semiterminado a Almacen',-@precio" & vbNewLine & _
        "UPDATE t_lotes_semiterminado set salida=salida-1 where id_semiterminado=@idLente and lote like @lote" & vbNewLine & _
        " DELETE t_salidas_semiterminados where id_pedido=" & id & " and id_orden=(select max(id_orden) from t_salidas_semiterminados where id_pedido=" & id & ")"


        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function ExisteLoteSemiterminado(ByVal Lente As Integer) As Boolean
        Dim cad As String = "select count(*) from t_lotes_semiterminado where id_semiterminado=" & Lente
        Dim Existe As Boolean
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Existe = cmd.ExecuteScalar
        mcon.Close()
        Return Existe
    End Function
    Public Sub SemiterminadoAlmacen(ByVal Pedido As clsPedido)
        'cargamos la ultima salida de semiterminado
        Dim cad As Integer = "select top 1 * from t_salidas_semiterminado where id_pedido=" & Pedido.id & " order by orden desc"
        Dim Lote As Integer
        Dim Orden As Integer
        Dim Lente As Integer
        Dim Precio As Integer
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim d As New clsDatos
        Dim tb As New DataTable
        mda.Fill(tb)
        If tb.Rows.Count = 1 Then
            Lote = tb.Rows(0)("lote")
            Orden = tb.Rows(0)("id_orden")
            Lente = tb.Rows(0)("id_semiterminado")
            Precio = d.GetPrecioSemiterminado(tb.Rows(0)("id_semiterminado"), tb.Rows(0)("id_proveedor"))
        End If

    End Sub
    Public Sub ModificaStockDesecho(ByVal id As Integer, ByVal cantidad As Integer)
        Dim cad As String = "UPDATE t_almacen_desechos set cantidad=" & cantidad & " where id_desecho=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub GrabaSalidaDesecho(ByVal des As clsDesecho, ByVal ped As clsPedido)
        Dim cad As String = "INSERT INTO t_salidas_desechos (id_pedido,id_desecho,id_orden,fecha) " & _
        " select " & ped.id & "," & des.Id & ", max(id_orden)," & FechaAcadena(Now.Date) & "  from t_ordenes_trabajo where id_pedido=" & ped.id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'ahora updateamos el stock a -1
        cmd.CommandText = "UPDATE t_almacen_desechos set cantidad=cantidad-1 where id_desecho=" & des.Id
        cmd.ExecuteNonQuery()
        Dim p As Paso
        cmd.CommandText = "UPDATE t_ordenes_trabajo set fs_almacen=" & FechaAcadena(Now.Date) & ",hs_almacen=" & Format(Now.Hour, "00") & Format(Now.Minute, "00") & ",usr_s_almacen=" & mUsuario.id & " where id_pedido=" & ped.id & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & ped.id & ")"
        cmd.ExecuteNonQuery()
        'ahora tenemos que ver donde va a empezar la orden de trabajo()
        If ped.id_tratamiento = des.idTratamiento And ped.id_coloracion = 0 Then ' va a salida de calidad
            cad = "UPDATE t_ordenes_trabajo set fs_calidad=" & FechaAcadena(Now.Date) & ",hs_calidad=" & Format(Now.Hour, "00") & Format(Now.Minute, "00") & ",usr_s_calidad=" & mUsuario.id & " where id_pedido=" & ped.id & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & ped.id & ")"
        ElseIf ped.id_tratamiento > des.idTratamiento And des.idTratamiento = 0 Then ' va a entrada de endurecido
            p = Paso.Endurecido
            cad = "UPDATE t_ordenes_trabajo set paso=" & p & " where id_pedido=" & ped.id & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & ped.id & ")"
        ElseIf ped.id_tratamiento > des.idTratamiento And des.idTratamiento = 1 Then ' va a entrada de tratamiento
            p = Paso.Antireflejo
            cad = "UPDATE t_ordenes_trabajo set paso=" & p & " where id_pedido=" & ped.id & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & ped.id & ")"
        End If

        cmd.CommandText = cad
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Function GetSemiterminadobycodBarras(ByVal id As String) As DataTable
        Dim cad As String = "select *, (select Proveedor from Proveedores.dbo.t_proveedores where id_proveedor=t_cod_barras_semiterminado.id_proveedor) as Proveedor" & _
        " from t_cod_barras_semiterminado where cod_barra='" & id & "'"
        Dim cmd As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        Dim Semi As New clsSemiterminado
        mcon.Open()
        cmd.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetProveedorSemiterminadoByLote(ByVal Lente As Integer, ByVal Lote As String) As DataTable
        Dim Cad As String = "select * from Proveedores.dbo.t_proveedores where id_proveedor=(select id_proveedor from t_lotes_semiterminado where id_semiterminado=" & Lente & " and lote like " & strsql(Lote) & ")"
        Dim mda As New SqlDataAdapter(Cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetTipoMontajeByIdMontaje(ByVal id As Long) As String
        Dim cad As String = "select tipo_montaje from t_tipos_montaje INNER JOIN t_montajes ON t_tipos_montaje.id_tipo_montaje=t_montajes.id_tipo_montaje where id_montaje=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        Dim montaje As String
        mcon.Open()
        montaje = cmd.ExecuteScalar
        mcon.Close()
        Return montaje
    End Function
    Public Function GetPedidoMontajebyId(ByVal id_pedido As Long) As clsMontaje
        Dim p As New clsMontaje
        Dim cad As String
        cad = "select t_pedidos_montajes.*,case t_lineas_pedido_montaje.id_montaje when 0 then (select 'Montura ' + modelo + '-' + color from t_monturas where id_montura=t_pedidos_montajes.id_montura)" & _
        " else (select montaje from t_montajes where id_montaje=t_lineas_pedido_montaje.id_montaje) END as montaje,t_lineas_pedido_montaje.* FROM " & _
        "t_pedidos_montajes " & _
        "LEFT OUTER join  t_lineas_pedido_montaje  on t_pedidos_montajes.id_pedido_montaje = t_lineas_pedido_montaje.id_pedido_montaje " & _
        " where t_pedidos_montajes.id_pedido_montaje=" & id_pedido

        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            p = Nothing
            Return p
        End If

        p.Cli = getClientebyId(tb.Rows(0)("id_cliente"))
        p.fecha = tb.Rows(0)("fecha")
        p.Izquierdo = tb.Rows(0)("ojo_izq")
        p.Derecho = tb.Rows(0)("ojo_dcho")
        p.Id = tb.Rows(0)("id_pedido_montaje")
        p.IdMontura = tb.Rows(0)("id_montura")
        p.Montura = GetNombreMonturabyid(p.IdMontura)
        p.Notas = IIf(IsDBNull(tb.Rows(0)("notas")), "", tb.Rows(0)("notas"))
        p.FechaSalida = IIf(IsDBNull(tb.Rows(0)("fecha_salida")), 0, tb.Rows(0)("fecha_salida"))
        p.Albaran = tb.Rows("0")("id_albaran")
        Dim i As Integer

        ' ahora grabamos cada linea del albaran
        If tb.Rows.Count > 0 Then
            For i = 0 To tb.Rows.Count - 1
                If Not IsDBNull(tb.Rows(i)("id_montaje")) Then
                    Dim mlin As New clsLineaMontaje
                    mlin.idMontaje = tb.Rows(i)("id_montaje")
                    mlin.Montaje = tb.Rows(i)("montaje")
                    mlin.precio = tb.Rows(i)("precio")
                    mlin.Dto = tb.Rows(i)("dto")
                    p.add(mlin)
                    mlin = Nothing
                End If
            Next
        End If
        Return p
    End Function
    Public Function GetPedidoMontajebyLente(ByVal idPedidoLente As Long) As clsMontaje
        Dim p As New clsMontaje
        Dim cad As String
        cad = "select t_pedidos_montajes.*,case t_lineas_pedido_montaje.id_montaje when 0 then (select 'Montura ' + modelo + '-' + color from t_monturas where id_montura=t_pedidos_montajes.id_montura)" & _
        " else (select montaje from t_montajes where id_montaje=t_lineas_pedido_montaje.id_montaje) END as montaje,t_lineas_pedido_montaje.* FROM " & _
        "t_lineas_pedido_montaje  " & _
        "inner join t_pedidos_montajes on t_pedidos_montajes.id_pedido_montaje = t_lineas_pedido_montaje.id_pedido_montaje " & _
        " where ojo_izq=" & idPedidoLente & " or ojo_dcho=" & idPedidoLente

        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            p.Id = 0
            Return p
        End If

        p.Cli = getClientebyId(tb.Rows(0)("id_cliente"))
        p.fecha = tb.Rows(0)("fecha")
        p.Izquierdo = tb.Rows(0)("ojo_izq")
        p.Derecho = tb.Rows(0)("ojo_dcho")
        p.Id = tb.Rows(0)("id_pedido_montaje")
        p.IdMontura = tb.Rows(0)("id_montura")
        p.Montura = GetNombreMonturabyid(p.IdMontura)
        p.Notas = IIf(IsDBNull(tb.Rows(0)("notas")), "", tb.Rows(0)("notas"))
        Dim i As Integer

        ' ahora grabamos cada linea del albaran
        For i = 0 To tb.Rows.Count - 1
            Dim mlin As New clsLineaMontaje
            mlin.idMontaje = tb.Rows(i)("id_montaje")
            mlin.Montaje = tb.Rows(i)("montaje")
            mlin.precio = tb.Rows(i)("precio")
            mlin.Dto = tb.Rows(i)("dto")
            p.add(mlin)
            mlin = Nothing
        Next

        Return p
    End Function
    Public Function GetLotesEndurecidoPendientes() As DataTable
        Dim cad As String = "select lote,cesta,dbo.TimeStampToFecha(entrada) as fecha from t_pedidos_cesta where salida=0 order by entrada"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    
    Public Function GetMonturas() As DataTable
        Dim cad As String
        ' Dim Montura As String
        cad = "(select 0 as id_montura,'ninguna' as  montura) UNION (select id_montura,(modelo + '-' + color) as montura from t_monturas) "
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetMonturaByid(ByVal id As Integer) As String
        Dim cad As String
        Dim Montura As String
        cad = "select ('MONTURA L.O.A.:'+  modelo + '-' + color) as montura from t_monturas  where id_montura=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        Montura = cmd.ExecuteScalar
        mcon.Close()
        Return Montura
    End Function
    Public Function GetNombreMonturabyid(ByVal idmontura As Integer) As String
        If idmontura = 0 Then
            Return ""
        Else
            Dim cad As String
            Dim Montura As String
            cad = "select modelo + '-' + color from t_monturas where id_montura=" & idmontura
            Dim cmd As New SqlCommand(cad, mcon)
            mcon.Open()
            Montura = cmd.ExecuteScalar
            mcon.Close()
            cmd.Dispose()
            Return Montura
        End If
    End Function

    Public Function getlentesSerieDeLente(ByVal modelo As Long) As DataTable
        Dim cad As String
        cad = "select distinct diametro,tratamiento from t_lentes_stock where id_modelo=" & modelo & "  and baja=0 order by tratamiento, diametro"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function getlentesSerieDesechos(ByVal modelo As Long) As DataTable
        Dim cad As String
        cad = "select distinct diametro,id_tratamiento as tratamiento from t_almacen_desechos where id_modelo=" & modelo & "   order by id_tratamiento, diametro"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function CargaEsferasPositivas(ByVal tipoLente As Long, ByVal diametro As Integer, ByVal tratamiento As Integer) As DataTable
        'carga el rango de fabricacion de la lente
        Dim mda As New SqlDataAdapter(New SqlCommand("select * from t_lentes_stock where " & _
        "id_modelo = " & tipoLente & " and diametro=" & diametro & " and tratamiento=" & tratamiento & " and esfera>=0 and baja=0 order by cilindro,esfera"))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function CargaEsferasNegativas(ByVal tipoLente As Long, ByVal diametro As Integer, ByVal tratamiento As Integer) As DataTable
        'carga el rango de fabricacion de la lente
        Dim mda As New SqlDataAdapter(New SqlCommand("select * from t_lentes_stock where " & _
        "id_modelo = " & tipoLente & " and diametro=" & diametro & " and tratamiento=" & tratamiento & " and esfera<0 and baja=0 order by cilindro,esfera"))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function CargaEsferasPositivasDesechos(ByVal tipoLente As Long, ByVal diametro As Integer, ByVal tratamiento As Integer) As DataTable
        'carga el rango de fabricacion de la lente
        Dim mda As New SqlDataAdapter(New SqlCommand("select * from t_almacen_desechos where " & _
        "id_modelo = " & tipoLente & " and diametro=" & diametro & " and id_tratamiento=" & tratamiento & " and esfera>=0  order by cilindro,esfera"))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetClientesBiselado() As DataTable
        Dim cad As String = "select id_cliente,codigo,nombre_comercial from t_clientes where biselado=1 order by codigo"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function CargaEsferasNegativasDesechos(ByVal tipoLente As Long, ByVal diametro As Integer, ByVal tratamiento As Integer) As DataTable
        'carga el rango de fabricacion de la lente
        Dim mda As New SqlDataAdapter(New SqlCommand("select * from t_almacen_desechos where " & _
        "id_modelo = " & tipoLente & " and diametro=" & diametro & " and id_tratamiento=" & tratamiento & " and esfera<0  order by cilindro,esfera"))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetDepartamentoByequipo() As Integer
        Dim cad As String = "select isnull(id_departamento,0) from t_equipos where equipo=" & strsql(Equipo)
        Dim cmd As New SqlCommand(cad, mcon)
        Dim cnnabierta As Boolean = False
        If mcon.State = ConnectionState.Open Then cnnabierta = True
        If cnnabierta = False Then mcon.Open()
        Dim departamento As Integer = cmd.ExecuteScalar
        If cnnabierta = False Then mcon.Close()
        Return departamento
    End Function
    Public Sub GrabaDepartamentoByequipo(ByVal id As Integer)
        Dim cad As String = "UPDATE t_equipos set id_departamento=" & id & " where equipo=" & strsql(Equipo)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Sub GrabaClienteBiselado(ByVal id As Integer, Optional ByVal añadir As Boolean = True)
        Dim cad As String = "UPdate t_clientes set biselado=" & IIf(añadir = True, 1, 0) & "  where id_cliente=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Function getProductoByGraduacion(ByVal idModelo As Long, ByVal idTratamiento As Integer, ByVal diametro As Integer, ByVal esfera As Decimal, ByVal cilindro As Decimal) As clsProducto
        'devuelve las caracteristicas del producto
        Dim mProd As New clsProducto
        'carga el rango de fabricacion de la lente
        Dim sCadSql As String = ""
        Dim cil As String = ""
        Dim esf As String = ""
        cil = Replace(cilindro, ",", ".")
        esf = Replace(esfera, ",", ".")
        sCadSql = "select t_lentes_stock.*,t_modelos.nombre from t_lentes_stock inner join t_modelos on " & _
        "t_lentes_stock.id_modelo = t_modelos.id_lente where tratamiento=" & idTratamiento & " and " & _
        "id_modelo=" & idModelo & " and diametro=" & diametro & " and t_LENTES_STOCK.baja=0 and cilindro=" & cil & " and esfera =" & esf
        Dim mda As New SqlDataAdapter(New SqlCommand(sCadSql))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then Return mProd
        mProd.Id = tb.Rows(0).Item("id_producto")
        mProd.cilindro = tb.Rows(0).Item("cilindro")
        mProd.codigo = ""
        mProd.diametro = tb.Rows(0).Item("diametro")
        mProd.esfera = tb.Rows(0).Item("esfera")
        mProd.modelo = tb.Rows(0).Item("id_modelo")
        mProd.Nombre = tb.Rows(0).Item("nombre")
        mProd.stock = tb.Rows(0).Item("stock")
        mProd.stockMinimo = tb.Rows(0).Item("stock_minimo")
        mProd.tratamiento = tb.Rows(0).Item("tratamiento")
        mProd.StockCritico = tb.Rows(0).Item("stock_critico")

        Return mProd
    End Function

    Public Function GetLenteStockByID(ByVal id As Long) As clsLenteStock
        Dim mProd As New clsLenteStock
        'carga el rango de fabricacion de la lente
        Dim sCadSql As String = "select t_lentes_stock.*,t_modelos.nombre as modelo,t_tratamientos.nombre as trat from (t_lentes_stock inner join t_modelos on " & _
        "t_lentes_stock.id_modelo = t_modelos.id_lente)inner join t_tratamientos on t_lentes_stock.tratamiento = " & _
        "t_tratamientos.id_tratamiento where t_lentes_stock.id_producto = " & id

        Dim mda As New SqlDataAdapter(New SqlCommand(sCadSql))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then Return mProd

        mProd.id = tb.Rows(0).Item("id_producto")
        mProd.modelo = tb.Rows(0).Item("modelo")
        mProd.tratamiento = tb.Rows(0)("trat")
        mProd.cilindro = tb.Rows(0).Item("cilindro")
        mProd.diametro = tb.Rows(0).Item("diametro")
        mProd.esfera = tb.Rows(0).Item("esfera")
        mProd.stock = tb.Rows(0).Item("stock")
        mProd.stock_min = tb.Rows(0)("stock_minimo")
        mProd.reservas = tb.Rows(0)("reservas")

        mProd.id_modelo = tb.Rows(0).Item("id_modelo")
        mProd.id_tratamiento = tb.Rows(0)("tratamiento")
        'ahora tenemos que calcular el precio para esa lente
        mProd.PrecioCompra = GetPrecioCompraStock(mProd)
        Return mProd
    End Function
    Public Function GetPrecioCompraStock(ByVal lente As clsLenteStock) As Decimal
        'aqui devolvemos el precio de compra de la lente
        Dim idGraduacion As Integer = 2
        If lente.cilindro = 0 Then idGraduacion = 1
        Dim cad As String = "select isnull(precio,0) as pcompra from t_precios_stock_proveedor where id_modelo=" & lente.id_modelo & " and (diametro=-1 or diametro=" & lente.diametro & ") and (id_graduacion=-1 or id_graduacion=" & idGraduacion & ") and id_tratamiento=" & lente.id_tratamiento
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        If tb.Rows.Count = 0 Then
            Return 0
        Else
            Return tb.Rows(0)("pcompra")
        End If
    End Function
    Public Function GetLentesStockByCodBarra(ByVal CodBarra As String) As DataTable
        Dim cad As String = " Select id_producto,t_modelos.nombre as modelo,t_tratamientos.nombre as tratamiento,diametro,cilindro,esfera from t_lentes_stock INNER JOIN t_modelos ON id_modelo=id_lente INNER JOIN t_tratamientos ON tratamiento=id_tratamiento where id_producto in (select id_producto from t_codigos_barra where cod_barra=" & strsql(CodBarra) & ")"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetLenteStockByCodBarra(ByVal codBarra As String, Optional ByVal Lente As Integer = 0) As clsLenteStock
        Dim mProd As New clsLenteStock
        'carga el rango de fabricacion de la lente
        Dim sCadSql As String = "select t_lentes_stock.*,t_modelos.nombre as modelo,t_tratamientos.nombre as trat from (t_lentes_stock inner join t_modelos on " & _
        "t_lentes_stock.id_modelo = t_modelos.id_lente)inner join t_tratamientos on t_lentes_stock.tratamiento = " & _
        "t_tratamientos.id_tratamiento where t_lentes_stock.id_producto in (select id_producto from t_codigos_barra where cod_barra like " & strsql(codBarra) & ")"
        If Lente <> 0 Then
            sCadSql &= " and t_lentes_stock.id_producto=" & Lente
        End If
        Dim mda As New SqlDataAdapter(New SqlCommand(sCadSql))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then Return mProd

        mProd.id = tb.Rows(0).Item("id_producto")
        mProd.modelo = tb.Rows(0).Item("modelo")
        mProd.tratamiento = tb.Rows(0)("trat")
        mProd.cilindro = tb.Rows(0).Item("cilindro")
        mProd.diametro = tb.Rows(0).Item("diametro")
        mProd.esfera = tb.Rows(0).Item("esfera")
        mProd.stock = tb.Rows(0).Item("stock")
        mProd.stock_min = tb.Rows(0)("stock_minimo")
        mProd.reservas = tb.Rows(0)("reservas")

        mProd.id_modelo = tb.Rows(0).Item("id_modelo")
        mProd.id_tratamiento = tb.Rows(0)("tratamiento")
        mProd.PrecioCompra = GetPrecioCompraStock(mProd)
        Return mProd
    End Function
    Public Function GetCodigosLentesStock(Optional ByVal Lente As Integer = 0) As DataTable
        'Dim mProd As New clsLenteStock
        'carga el rango de fabricacion de la lente
        Dim sCadSql As String
        Dim where As String = ""
        If Lente <> 0 Then
            where = " where t_codigos_barra.id_producto=" & Lente
        End If
        sCadSql = "select t_lentes_stock.*,t_modelos.nombre as modelo,t_tratamientos.nombre as trat,cod_barra,id_proveedor, (select proveedor from t_proveedores where t_proveedores.id_proveedor=t_codigos_barra.id_proveedor) as proveedor from ((t_lentes_stock inner join t_modelos on " & _
        "t_lentes_stock.id_modelo = t_modelos.id_lente)inner join t_tratamientos on t_lentes_stock.tratamiento = " & _
        "t_tratamientos.id_tratamiento) INNER JOIN t_codigos_barra ON t_lentes_stock.id_producto=t_codigos_barra.id_producto " & where & " ORDER BY t_modelos.nombre,diametro,t_tratamientos.nombre"

        Dim mda As New SqlDataAdapter(New SqlCommand(sCadSql))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb

    End Function
    Public Sub SalidaProducto(ByVal id As Long, ByVal cantidad As Integer)
        'Dim mProd As New clsProducto
        'carga el rango de fabricacion de la lente
        Dim sCadSql As String = "update t_lentes_stock set stock = stock - " & cantidad & _
        " where id_producto= " & id
        Dim cmd As New SqlCommand(sCadSql, mcon)

        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        MovimientoAlmacen(id, cantidad)
    End Sub
    Public Sub EntradaProducto(ByVal id As Long, ByVal cantidad As Integer)
        ' Dim mProd As New clsProducto
        'carga el rango de fabricacion de la lente
        Dim sCadSql As String = "update t_lentes_stock set stock = stock + " & cantidad & _
        " where id_producto= " & id
        Dim cmd As New SqlCommand(sCadSql, mcon)
        'mda.UpdateCommand = New SqlCommand(sCadSql)
        'mda.UpdateCommand.Connection = mcon

        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        MovimientoAlmacen(id, cantidad, True)
    End Sub
    Public Sub EntradaSemiterminado(ByVal id As Long, ByVal cantidad As Integer)
        Dim mProd As New clsProducto
        'carga el rango de fabricacion de la lente
        Dim sCadSql As String = "update t_semiterminados set stock = stock + " & cantidad & _
        " where id_lente= " & id
        Dim cmd As New SqlCommand(sCadSql, mcon)
        'mda.UpdateCommand = New SqlCommand(sCadSql)
        'mda.UpdateCommand.Connection = mcon

        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        'MovimientoAlmacen(id, cantidad, True)
    End Sub
    Public Sub GrabaCodigoBarra(ByVal idArt As Integer, ByVal codigo As String, ByVal prov As String)
        Dim cad As String = "INSERT INTO t_codigos_barra (id_producto,cod_barra,id_proveedor) VALUES (" & idArt & ",'" & codigo & "'," & prov & ")"

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        cmd = Nothing
    End Sub
    Public Sub MovimientoAlmacen(ByVal id As Long, ByVal cantidad As Integer, Optional ByVal entrada As Boolean = False, Optional ByVal observaciones As String = "")

        Dim sCadSql As String
        sCadSql = "insert into t_movimientos_almacen (fecha,id_producto,cantidad,entrada,observaciones) values " & _
        "(" & CLng(FechaAcadena(Today)) & "," & id & "," & cantidad & "," & IIf(entrada = False, 0, 1) & ",'" & observaciones & "')"
        Dim cmd As New SqlCommand(sCadSql, mcon)

        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function Seguimientos(ByVal fecini As Integer, ByVal fecfin As Integer) As DataTable
        Dim cad As String = "select t_pedidos.*, t_modelos.nombre as modelo,t_tratamientos.nombre as tratamiento, t_clientes.nombre_comercial from ((" & _
            "(t_pedidos inner join t_modelos on t_pedidos.id_modelo = t_modelos.id_lente)INNER JOIN t_tratamientos On t_pedidos.id_tratamiento=t_tratamientos.id_tratamiento) inner join " & _
            "t_clientes on t_pedidos.id_cliente= t_clientes.id_cliente) where fecha>=" & fecini & " and fecha<=" & fecfin & " and t_pedidos.id_cliente in (select id_cliente from t_clientes where seguimiento<>0)"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)

        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function SeguimientosToExcel(ByVal fecini As Integer, ByVal fecfin As Integer) As DataTable
        Dim cad As String = "select dbo.fechaexcel(fecha) as fecha,t_pedidos.id_pedido, t_modelos.nombre as modelo,t_tratamientos.nombre as tratamiento, t_clientes.nombre_comercial as cliente,Eje,diametro,cilindro,esfera,adicion from ((" & _
            "(t_pedidos inner join t_modelos on t_pedidos.id_modelo = t_modelos.id_lente)INNER JOIN t_tratamientos On t_pedidos.id_tratamiento=t_tratamientos.id_tratamiento) inner join " & _
            "t_clientes on t_pedidos.id_cliente=t_clientes.id_cliente) where fecha>=" & fecini & " and fecha<=" & fecfin & " and t_pedidos.id_cliente in (select id_cliente from t_clientes where seguimiento<>0)"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)

        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetTrackingPedido(ByVal pedido As Integer, ByVal orden As Integer, Optional ByVal Inicio As Long = 0, Optional ByVal fin As Long = 0) As DataTable
        Dim filtro As String = ""
        Dim Fecha As Date
        Dim Hora As String = ""
        If Inicio <> 0 Then
            Hora = CStr(Inicio).Substring(Len(CStr(Inicio)) - 4, 2) & ":" & CStr(Inicio).Substring(Len(CStr(Inicio)) - 2, 2)
            Inicio = CStr(Inicio).Substring(0, Len(CStr(Inicio)) - 4)
            Fecha = cadenaAfecha(Inicio) & " " & Hora
            filtro = " and entrada>=" & ConvertDateToTimeStamp(Fecha)
        End If
        If fin <> 0 Then
            Hora = CStr(fin).Substring(Len(CStr(fin)) - 4, 2) & ":" & CStr(fin).Substring(Len(CStr(fin)) - 2, 2)
            fin = CStr(fin).Substring(0, Len(CStr(fin)) - 4)
            Fecha = cadenaAfecha(fin) & " " & Hora
            'Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            filtro = filtro & " and entrada<" & ConvertDateToTimeStamp(Fecha)
        End If
        Dim cad As String = "select maquina,dbo.TimeStampToFecha(entrada) as fecha from t_tracking_maquina INNER JOIN t_maquinas ON t_maquinas.id_maquina=t_tracking_maquina.id_maquina where id_pedido=" & pedido & " and id_orden=" & orden & filtro & " order by entrada"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb

    End Function
    Public Function Getmaquinas() As ArrayList
        Dim maquinas As New ArrayList
        Dim cad As String = "select * from t_maquinas where baja=0 order by id_maquina "
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows
            Dim m As New ClsMaquina
            m.id = rw("id_maquina")
            m.Nombre = rw("maquina")
            m.Ip = rw("ip")
            maquinas.Add(m)
        Next
        Return maquinas
    End Function
    Public Function GetMtoByMaquina(ByVal id As Integer) As ArrayList
        Dim mto As New ArrayList
        Dim cad As String = "select * from t_mto_maquina where id_maquina=" & id
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows
            Dim m As New clsMtoMaquina
            m.Id = rw("id_mantenimiento")
            m.Mantenimiento = rw("mantenimiento")
            m.Lentes = rw("lentes")
            m.Dias = rw("dias")
            m.Aviso = rw("aviso")
            mto.Add(m)
        Next
        Return mto
    End Function
    Public Function GetVisitasByCliente(ByVal idcli As Integer, ByVal Inicio As Integer, ByVal Fin As Integer) As DataTable
        Dim Cad As String = "select * from Comercial.Dbo.t_visitas where id_contacto=" & idcli & " and fecha>=" & Inicio & " and fecha<=" & Fin & " order by fecha desc"
        Dim mda As New SqlDataAdapter(Cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Sub GrabaMaquina(ByRef maq As ClsMaquina)
        Dim cad As String = ""
        If maq.id = 0 Then
            maq.id = getMaxId("id_maquina", "t_maquinas") + 1
            cad = "INSERT INTO t_maquinas (id_maquina,maquina,ip,puertoin,puertoout) VALUES (" & maq.id & "," & strsql(maq.Nombre) & "," & strsql(maq.Ip) & ",0,0)"
        Else
            cad = "UPDATE t_maquinas set maquina=" & strsql(maq.Nombre) & ",ip=" & strsql(maq.Ip) & " where id_maquina=" & maq.id
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'ahora grabamos los mantenimientos
        For Each m As clsMtoMaquina In maq.Mantenimientos
            If m.Id = 0 Then
                m.Id = getMaxId("id_mantenimiento", "t_mto_maquina", " where id_maquina=" & maq.id) + 1
                cad = "INSERT INTO t_mto_maquina (id_maquina,id_mantenimiento,mantenimiento,lentes,dias,aviso) VALUES (" & maq.id & "," & m.Id & "," & strsql(m.Mantenimiento) & "," & m.Lentes & "," & m.Dias & "," & m.Aviso & ")"
            Else
                cad = "UPDATE t_mto_maquina set mantenimiento=" & strsql(m.Mantenimiento) & ",lentes=" & m.Lentes & ",dias=" & m.Dias & ",aviso=" & m.Aviso & " where id_maquina=" & maq.id & " and id_mantenimiento=" & m.Id
            End If
            cmd.CommandText = cad
            cmd.ExecuteNonQuery()
        Next
        mcon.Close()
    End Sub
    Public Sub GrabaMtoMaquina(ByVal idMaquina As Integer, ByVal idMto As Integer, ByVal valor As String)
        Dim cad As String = "INSERT INTO t_fechas_mto_maquina (id_maquina,id_mantenimiento,id_usuario,valor,entrada) VALUES " & _
        " (" & idMaquina & "," & idMto & "," & mUsuario.id & "," & strsql(valor) & ",dbo.FechatoTimeStamp(" & strsql(Now) & "))"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetProximosMtomaquinas() As DataTable
        Dim FiltroDepartamento As String = " where baja=0"
        Dim Departamento As Integer = GetDepartamentoByequipo()
        If Departamento > 0 Then
            If Departamento = 5 Or Departamento = 8 Then
                FiltroDepartamento = FiltroDepartamento & " and id_departamento in (5,8)"
            Else
                FiltroDepartamento = FiltroDepartamento & " and id_departamento=" & Departamento
            End If

        End If
        'aqui vamos a devolver el mantenimiento de una maquina que esta pendiente de hacerse
        Dim cad As String = "select maquina,t_mto_maquina.*,isnull((select dbo.TimeStampTofecha(max(entrada)) as fecha from t_fechas_mto_maquina where id_maquina=t_mto_maquina.id_maquina and id_mantenimiento=t_mto_maquina.id_mantenimiento),getdate()) as fecha " & _
        ",(select count(*) from t_tracking_maquina where id_maquina=t_mto_maquina.id_maquina and id_mantenimiento=t_mto_maquina.id_mantenimiento and entrada>=isnull((select  top 1 (entrada) from  t_fechas_mto_maquina where id_maquina=t_mto_maquina.id_maquina and id_mantenimiento=t_mto_maquina.id_mantenimiento order by entrada desc),0)) as procesos" & _
        " from t_mto_maquina INNER JOIN t_maquinas ON t_mto_maquina.id_maquina=t_maquinas.id_maquina" & FiltroDepartamento
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function InfTrackingMaquinasFecha(ByVal inicio As Integer, ByVal fin As Integer, Optional ByVal detalle As Boolean = False) As DataTable
        Dim cad As String = ""
        If detalle = False Then
            cad = "select maquina,dbo.fechaexcel(fecha) as fecha,count(*) as lentes from t_maquinas INNER JOIN t_tracking_maquina ON t_tracking_maquina.id_maquina=t_maquinas.id_maquina where fecha>=" & inicio & " and fecha<=" & fin & " group by maquina,fecha order by fecha,maquina"

        Else
            cad = "select maquina,dbo.fechaexcel(fecha) as fecha,substring(dbo.hora(hora),1,2) as hora ,count(*) as lentes from t_maquinas INNER JOIN t_tracking_maquina ON t_tracking_maquina.id_maquina=t_maquinas.id_maquina where fecha>=" & inicio & " and fecha<=" & fin & " group by maquina,substring(dbo.hora(hora),1,2),fecha order by fecha,maquina,convert(int,(substring(dbo.hora(hora),1,2))"

        End If
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb

    End Function
    Public Function RegistroSanitarioEntreFechas(ByVal inicio As Integer, ByVal fin As Integer, Optional ByVal Excel As Boolean = False) As DataTable
        Dim incidencias As String = ""
        If Excel = False Then
            incidencias = ",(select count(*) from t_incidencias_pedidos where id_pedido=t_pedidos.id_pedido) as incidencias"
        End If
        Dim cad As String = "select dbo." & IIf(Excel = True, "fechaexcel", "fecha") & "(fecha_salida) + ' ' + dbo.hora(hora_salida) as Salida" & incidencias & ",t_pedidos.id_pedido as numero,(select nombre from t_modelos where  id_lente=id_modelo) + ' Cyl ' + convert(varchar(6),cilindro) +  ' Sph ' + convert(varchar(6),esfera) as modelo,(select nombre from t_tratamientos where id_tratamiento=t_pedidos.id_tratamiento) as tratamiento,(select color from t_coloraciones where id_coloracion=t_pedidos.id_coloracion) as color,nombre+ ' ' + apellidos as tecnico,isnull((select case fecha when  null then '' else convert(varchar(20),dbo." & IIf(Excel = False, "TimeStampTofecha(t_registro_sanitario.fecha)", "TimeStampTofechaExcel(t_registro_sanitario.fecha)") & ") END from t_registro_sanitario where id_pedido=t_pedidos.id_pedido),'') as [Fecha Liberacion],isnull((select nombre + ' '  + apellidos from t_usuarios where id_usuario=(select responsable from t_registro_sanitario where id_pedido=t_pedidos.id_pedido)),'') as Responsable from t_pedidos inner join t_ordenes_trabajo On t_ordenes_trabajo.id_pedido=t_pedidos.id_pedido INNER JOIN t_usuarios ON t_usuarios.id_usuario=t_ordenes_trabajo.usr_s_calidad where id_incidencia=0 and (modo='F'or modo='T') and fecha_salida>=" & inicio & " and fecha_salida<=" & fin & " order by salida,t_pedidos.id_pedido"

        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.SelectCommand.CommandTimeout = 240
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb

    End Function
    Public Sub ExcluirCLientePromociones(ByVal idcli As Integer, ByVal Promos As ArrayList)
        Dim cad As String = "DELETE FROM t_clientes_sin_promocion where id_cliente=" & idcli
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        For Each i As Integer In Promos
            cmd.CommandText = "INSERT INTO t_clientes_sin_promocion (id_cliente,id_promocion) VALUES (" & idcli & "," & i & ")"
            cmd.ExecuteNonQuery()
        Next
        mcon.Close()
    End Sub
    Public Function ClienteSinPromocion(ByVal idCli As Integer, ByVal idPromo As Integer) As Boolean
        Dim cad As String = "select count(*) from t_clientes_sin_promocion where id_cliente=" & idCli & " and id_promocion=" & idPromo
        Dim Excluido As Boolean = False
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Excluido = cmd.ExecuteScalar
        mcon.Close()
        Return Excluido
    End Function
    Public Function InfTrackingMaquinasFecha(ByVal inicio As Date, ByVal fin As Date, Optional ByVal detalle As Boolean = False) As DataTable
        Dim cad As String = ""

        Dim FecEntrada As String = "convert(nvarchar(2),datepart(m,dbo.TimeStampTofecha(entrada))) + '/' + convert(nvarchar(2),datepart(d,dbo.TimeStampTofecha(entrada)))+ '/' + convert(nvarchar(4),datepart(yyyy,dbo.TimeStampTofecha(entrada)))"
        If detalle = False Then
            cad = "select maquina," & FecEntrada & " as Fecha,count(*) as lentes from t_maquinas INNER JOIN t_tracking_maquina ON t_tracking_maquina.id_maquina=t_maquinas.id_maquina where entrada>=" & ConvertDateToTimeStamp(inicio) & " and entrada<" & ConvertDateToTimeStamp(DateAdd(DateInterval.Day, 1, fin)) & " group by maquina," & FecEntrada & " order by " & FecEntrada & ",maquina"

        Else
            Dim HoraEntrada As String = "datepart(hour,dbo.TimeStampTofecha(entrada))"

            cad = "select maquina," & FecEntrada & " as fecha," & HoraEntrada & " as hora ,count(*) as lentes from t_maquinas INNER JOIN t_tracking_maquina ON t_tracking_maquina.id_maquina=t_maquinas.id_maquina where entrada>=" & ConvertDateToTimeStamp(inicio) & " and entrada<" & ConvertDateToTimeStamp(DateAdd(DateInterval.Day, 1, fin)) & vbNewLine & " group by maquina," & FecEntrada & "," & HoraEntrada & " order by " & FecEntrada & ",maquina," & HoraEntrada

        End If
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb

    End Function
    Public Function InfTrackingTratamientoFecha(ByVal inicio As Date, ByVal fin As Date) As DataTable
        Dim cad As String = ""



        cad = "(select 'ENDURECIDO' as maquina,dbo.fechaexcel(Fe_endurecimiento) as fecha,substring(dbo.Hora(he_endurecimiento),1,2) as hora,count(*) as lentes from t_ordenes_trabajo where Fe_endurecimiento>=" & FechaAcadena(inicio) & " and Fe_endurecimiento<=" & FechaAcadena(fin) & " group by fe_endurecimiento,substring(dbo.Hora(he_endurecimiento),1,2) UNION " & _
        "(select 'ANTIREFLEJO' as maquina,dbo.fechaexcel(Fe_antireflejo) as fecha,substring(dbo.Hora(he_antireflejo),1,2) as hora,count(*) as lentes from t_ordenes_trabajo where Fe_antireflejo>=" & FechaAcadena(inicio) & " and Fe_antireflejo<=" & FechaAcadena(fin) & " group by Fe_antireflejo,substring(dbo.Hora(he_antireflejo),1,2))) order by " & _
          "maquina,Fecha,hora"

        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb

    End Function
    Public Function BuscaPedidos(ByVal nombre As String, ByVal numPed As Long, ByVal fecIni As String, ByVal fecFin As String, ByVal referencia As String, ByVal Modelo As String, ByVal diametro As String, ByVal Cyl As String, ByVal sph As String, ByVal Add As String, _
    ByVal soloPendiente As Boolean, ByVal soloServidos As Boolean, ByVal anulados As Boolean, ByVal orden As String, Optional ByVal descendente As Boolean = False) As DataTable
        '**********************************************************************************************************
        'cambio la definicion de la funcion ya que se tarda mucho al cargar el pedido completo cuando lo que es necesario solo es
        ' un numero pequeño de datos , no es necesario cargar todo el pedido , mientras que los datos se cargan muy rapiso 
        'desde la base de datos, el crear la coleccion de pedidos tarda mucho
        '****************************************************************************************************************

        'Public Function BuscaPedidos(ByVal nombre As String, ByVal numPed As Long, ByVal fecIni As String, ByVal fecFin As String, ByVal referencia As Long, _
        'ByVal soloPendiente As Boolean, ByVal soloServidos As Boolean, ByVal anulados As Boolean) As clsPedidos

        'carga el rango de fabricacion de la lente
        Dim cad As String = "select t_pedidos.*,(select count(*) from t_ordenes_trabajo where id_pedido=t_pedidos.id_pedido) as ordenes, t_modelos.nombre as modelo,t_clientes.nombre_comercial from ((" & _
            "t_pedidos inner join t_modelos on t_pedidos.id_modelo = t_modelos.id_lente)inner join " & _
            "t_clientes on t_pedidos.id_cliente=t_clientes.id_cliente) where t_pedidos.id_cliente>=0"
        If soloPendiente = True Then
            cad = cad & " and (fecha_salida=0 and id_albaran=0)"
        End If
        If soloServidos = True Then
            cad = cad & " and fecha_salida<>0"
        End If
        If nombre <> "" Then
            cad = cad & " and (nombre_comercial like '%" & nombre & "%' or razon_social  like '%" & nombre & "%' or t_clientes.codigo like " & strsql(nombre) & ")"
        End If
        If fecIni <> "" And fecFin <> "" Then
            cad = cad & " and fecha >= " & FechaAcadena(fecIni) & " and fecha<=" & FechaAcadena(fecFin)
        End If
        If diametro <> "" Then
            cad = cad & " and diametro=" & NumSql(diametro)

        End If
        If Modelo <> "" Then
            cad = cad & " and id_modelo in (select id_lente from t_modelos where nombre like '%" & Modelo & "%')"
        End If
        If Cyl <> "" Then
            cad = cad & " and Cilindro=" & NumSql(Cyl)

        End If
        If Add <> "" Then
            cad = cad & " and Adicion=" & NumSql(Add)

        End If
        If sph <> "" Then
            cad = cad & " and esfera=" & NumSql(sph)

        End If
        If fecIni <> "" And fecFin = "" Then
            cad = cad & " and fecha >= " & FechaAcadena(fecIni)
        End If
        If fecIni = "" And fecFin <> "" Then
            cad = cad & " and fecha<=" & FechaAcadena(fecFin)
        End If
        If numPed <> 0 Then
            cad = cad & " and id_pedido=" & numPed
        End If
        If mUsuario.Comercial = True Then
            cad = cad & " and id_comercial=" & mUsuario.id
        End If
        If referencia <> "" Then
            cad = cad & " and referencia like " & strsql("%" & referencia & "%")
        End If
        If anulados = False Then
            cad = cad & " and anulado=0"
        End If
        Dim Desc As String = ""
        If descendente = True Or soloServidos = True Then
            Desc = " desc"
        End If
        Select Case orden
            Case "pedido"
                cad = cad & " order by fecha" & Desc & ",id_pedido"
            Case "cliente"
                cad = cad & " order by nombre_comercial" & Desc & ",id_pedido"
            Case "anulado"
                cad = cad & " order by anulado" & Desc & ",nombre_comercial,id_pedido"
        End Select



        ''en el caso particular que elcliente sea  la cadena 'nulo' se cargan solo los pedidos anulados
        'If UCase(nombre) = "NULO" Then
        '    cad = "select t_pedidos.*,t_clientes.nombre_comercial from t_pedidos " & _
        '    "inner JOIN t_clientes on t_pedidos.id_cliente=t_clientes.id_cliente where anulado=1"
        'End If

        Dim mda2 As New SqlDataAdapter
        Dim tb2 As New DataTable
        mda2.SelectCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda2.Fill(tb2)
        mcon.Close()
        'Dim i As Integer
        Dim cPs As New clsPedidos

        If tb2.Rows.Count = 0 Then
            Return Nothing
        Else
            Return tb2
            'For i = 0 To tb2.Rows.Count - 1
            '    Dim cp As New clsPedido()
            '    '********************cargo los datos mínimos del pedido para que no se me atasque la busqueda
            '    ' cp = GetPedidobyId(tb.Rows(i).Item("id_pedido"))
            '    'son necesarios id_pedido, modelo, cilindro , esfera, tratamiento, nulo o no nulo
            '    '**********************************************************************************************

            '    ''cPs.add(cp)
            '    cp = Nothing
            'Next
        End If
        ' Return cPs
    End Function
    Public Function clientesSinPortes() As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select codigo, razon_social,nombre_comercial,direccion,poblacion from t_clientes where cobrar_portes=0 and baja=0"
        If mUsuario.Comercial = True Then
            cad = cad & " and id_comercial=" & mUsuario.id
        End If
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function CodBarraRepetido(ByVal Codigo As String) As Boolean
        Dim cad As String = "Select count(distinct(id_producto)) from t_codigos_barra where cod_barra=" & strsql(Codigo)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim cuenta As Integer = cmd.ExecuteScalar
        mcon.Close()
        If cuenta <= 1 Then
            Return False
        Else
            Return True
        End If

    End Function
    Public Sub BorraCodBarraByLente(ByVal idLente As Integer, ByVal codBarra As String)
        Dim cad As String = "DELETE FROM t_codigos_barra where id_producto=" & idLente & " and cod_barra=" & strsql(codBarra)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function Borrabase(ByVal idModelo As Integer, ByVal diametro As Integer, ByVal base As Single) As Boolean
        'Primero hay que ver que no haya salido ningun semiterminado de esa base
        Dim cuenta As Integer = 0
        Dim sql As String = "select count(*) from t_salidas_semiterminados where id_lente in (select id_lente from t_semiterminados where id_modelo=" & idModelo & " and diametro=" & diametro & " and base=" & Replace(base, ",", ".") & ")"
        Dim cm As New SqlCommand(sql, mcon)
        mcon.Open()
        cuenta = cm.ExecuteScalar
        mcon.Close()
        If cuenta > 0 Then
            MsgBox("No se puede borrar dicha base, puesto que ya hay salidas de almacen de lentes con esa base")
            Return False
        End If
        Dim cad As String = "delete from t_bases where id_modelo=" & idModelo & " and diametro=" & diametro & " and base=" & Replace(base, ",", ".") & vbNewLine & _
        "delete from t_semiterminados where id_modelo=" & idModelo & " and diametro=" & diametro & " and base=" & Replace(base, ",", ".")
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        Return True
    End Function
    Public Function getModeloByTipologiaMaterial(ByVal material As Integer, ByVal tipo As Integer) As DataTable
        Dim cad As String = "select * from t_modelos where baja = 0 and material=" & material & " and tipologia=" & tipo
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function TieneStock(ByVal idmodelo As Long) As Boolean
        'determina si en ese modelo existen lentes de stock
        Dim cad As String = "select * from t_lentes_stock where id_modelo=" & idmodelo
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count > 0 Then Return True
        Return False
    End Function
    Public Function TieneTransformacion(ByVal idmodelo As Long) As Boolean
        'determina si en ese modelo existen lentes de stock
        Dim cad As String = "select * from t_lentes_stock where tratamiento = 0 and id_modelo=" & idmodelo
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count > 0 Then Return True
        Return False
    End Function
    Public Function GetMateriales() As DataTable
        Dim cad As String = "select * from m_materiales"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GrabaMaterialesComercial(ByVal id As Integer, ByVal Material As String, Optional ByVal baja As Boolean = False) As Integer
        Dim cad As String = ""
        If id = 0 Then
            id = getMaxId("ID_MATERIAL", "COMERCIAL.DBO.T_MATERIALES") + 1
            cad = "insert into comercial.dbo.t_materiales select " & id & "," & strsql(Material) & "," & IIf(baja = False, "0", "1")
        Else
            cad = "Update comercial.dbo.t_materiales set material=" & strsql(Material) & ", baja=" & IIf(baja = False, "0", "1") & " where id_material=" & id

        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        Return id
    End Function

    Public Function GetMaterialescomerciales() As DataTable
        Dim cad As String = "select * from comercial.dbo.t_materiales where baja=0"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetTipologia() As DataTable
        Dim cad As String = "select * from m_tipologia"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetProgresivoIdEspesor(ByVal id As Integer) As String
        Dim cad As String = "select progresivo from t_espesores where id_espesor=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        GetProgresivoIdEspesor = cmd.ExecuteScalar
        mcon.Close()

    End Function
    Public Function GetAdicionMaximaBymodelo(ByVal id As Integer) As Decimal
        Dim cad As String = "select adicion_maxima from t_modelos where id_lente=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        GetAdicionMaximaBymodelo = cmd.ExecuteScalar
        mcon.Close()
    End Function

    Public Function EsMonoFocal(ByVal idModelo As Integer) As Boolean
        Dim cad As String = "select * from t_modelos where id_lente = " & idModelo
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then Return False
        If tb.Rows(0).Item("tipologia") = 1 Then Return True
        Return False
    End Function
    Public Function EsAltoIndice(ByVal idModelo As Integer) As Boolean
        Dim cad As String = "select * from t_modelos where id_lente = " & idModelo
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then Return False
        If tb.Rows(0).Item("indice_modelo") > 1.5 Then Return True
        Return False
    End Function
    Public Function getFamilias() As DataTable
        Dim cad As String = "select * from t_familias order by id_familia"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return (tb)
    End Function
    Public Sub CreaAgrupamiento(ByVal nombre As String, ByVal secuenciaFamilias As String)
        Dim idFamilias() As String
        idFamilias = Split(secuenciaFamilias, ",")
        Dim i As Integer = 0
        Dim id As Long = getMaxId("id_agrupamiento", "t_agrupamientos") + 1
        Dim cad As String = "insert into t_agrupamientos (id_agrupamiento,nombre) values(" & id & ",'" & nombre & "')"
        Dim mda1 As New SqlDataAdapter
        mda1.InsertCommand = New SqlCommand(cad)
        mda1.InsertCommand.Connection = mcon
        mcon.Open()
        mda1.InsertCommand.ExecuteNonQuery()

        For i = 0 To idFamilias.Length - 1
            cad = "insert into t_agrupamientos_detalle (id_agrupamiento,id_familia) values (" & id & "," & idFamilias(i) & ")"
            Dim mda As New SqlDataAdapter
            mda.InsertCommand = New SqlCommand(cad)
            mda.InsertCommand.Connection = mcon
            mcon.Open()
            mda.InsertCommand.ExecuteNonQuery()
            mcon.Close()
            mda = Nothing
        Next

    End Sub
    Public Sub ModificaPrecioBase(ByVal precio As String, ByVal id_modelo As Integer, ByVal modo As String, ByVal id_tratamiento As Integer, Optional ByVal grad As String = "*")
        Dim p As String = Replace(precio, ",", ".")
        Dim cad As String = "update t_productos set precio_base=" & p & " where id_producto<>0 and id_modelo = " & id_modelo & " and " & _
        "modo='" & modo & "' and id_tratamiento=" & id_tratamiento & ""
        If grad <> "*" Then
            If grad = "torico" Then
                cad = cad & " and cilindro>0"
            Else
                cad = cad & " and cilindro=0"
            End If
        End If
        Dim mda As New SqlDataAdapter
        mda.UpdateCommand = New SqlCommand(cad)
        mda.UpdateCommand.Connection = mcon
        mcon.Open()
        mda.UpdateCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function getPrecioBase(ByVal id_modelo As Integer, ByVal modo As String, _
        ByVal id_tratamiento As Integer, ByVal color As Integer, _
        ByVal grad As Integer) As String

        Dim cad As String = "select precio_base from  t_precios_grupo where id_modelo = " & id_modelo & " and " & _
        "modo=" & modo & " and tratamiento=" & id_tratamiento & " and color=" & color & " and forma=" & grad

        Dim mda As New SqlDataAdapter
        Dim tb As New DataTable
        mda.SelectCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0).Item("precio_base")
    End Function
    Public Function EsPotenciaDeRango(ByVal id_modelo As Integer, ByVal modo As String, ByVal cilindro As String, ByVal esfera As String) As Boolean
        Dim cad As String = ""
        Dim c As String = Replace(cilindro, ",", ".")
        Dim e As String = Replace(esfera, ",", ".")
        cad = "select * from  t_productos where id_producto<>0 and id_modelo = " & id_modelo & " and " & _
        "modo='" & modo & "' and cilindro=" & c & " and esfera=" & e

        Dim mda As New SqlDataAdapter
        Dim tb As New DataTable
        mda.SelectCommand = New SqlCommand(cad)
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count > 0 Then Return True
        Return False
    End Function
    Public Sub GrabaPreciosCLiente(ByVal acuerdo As clsAcuerdo, ByVal idcliente As Integer)

        Dim la As clsAcuerdoLinea
        mcon.Open()
        Dim cad As String = "DELETE FROM t_precios_cliente WHERE id_cliente=" & idcliente
        Dim cmd As New SqlCommand(cad, mcon)
        cmd.ExecuteNonQuery()
        'ahora vamos grabando las lineas
        For Each la In acuerdo
            Dim can As String = Replace(la.descuento, ",", ".")
            cad = "insert into t_precios_cliente(id_cliente," & _
            "id_grupo,id_modo,id_tratamiento,diametro,id_graduacion,descuento,id_color) values (" & idcliente & _
            "," & la.idGrupo & "," & la.id_modo & "," & la.id_tratamiento & "," & la.diametro & "," & _
            la.id_graduacion & "," & can & "," & la.id_color & ")"

            Dim mda As New SqlCommand(cad, mcon)
            mda.ExecuteNonQuery()
            mda = Nothing
        Next
    End Sub
    Public Sub GrabaPreciosESPECIALESBYCLiente(ByVal idcliente As Integer, ByVal Precios As ArrayList)



        mcon.Open()
        Dim cad As String = "DELETE FROM t_precios_cliente WHERE id_cliente=" & idcliente
        Dim cmd As New SqlCommand(cad, mcon)
        cmd.ExecuteNonQuery()
        'ahora vamos grabando las lineas
        For Each la As PrecioCliente In Precios

            cad = "insert into t_precios_cliente(id_cliente," & _
            "id_grupo,id_modo,id_tratamiento,diametro,cilindro,precio,id_color,desde,hasta) values (" & idcliente & _
            "," & la.idGrupo & "," & la.idModo & "," & la.idTratamiento & "," & la.diametro & "," & _
            NumSql(la.cilindro) & "," & NumSql(la.Precio) & "," & la.idGama & "," & la.Inicio & "," & la.Fin & ")"

            Dim mda As New SqlCommand(cad, mcon)
            mda.ExecuteNonQuery()
            mda = Nothing
        Next
    End Sub
    Public Function GrabaAcuerdo(ByRef acuerdo As clsAcuerdo) As Long
        'graba un acuerdo nuevo o modifica uno antiguo, el idacuerdo indica si es nuevo o no
        Dim cad As String = ""
        Dim la As clsAcuerdoLinea
        Dim id As Long = getMaxId("id_acuerdo", "t_acuerdos") + 1
        mcon.Open()
        If acuerdo.id = 0 Then
            'inserto los datos generales
            cad = "insert into t_acuerdos (id_acuerdo,nombre,descripcion,consumo_minimo,numProgresivos," & _
            "dto_stock,dto_fabricacion,dto_progresivos,desde,hasta,id_grupo) values (" & id & "," & _
            strsql(acuerdo.nombre) & ",'" & acuerdo.descripcion & "'," & Replace(acuerdo.ConsumoMensual, ",", ".") & "," & _
            acuerdo.NumMinimoProgresivos & "," & Replace(acuerdo.dto_stock, ",", ".") & "," & Replace(acuerdo.dto_fabricacion, ",", ".") & "," & _
            Replace(acuerdo.dto_Progresivos, ",", ".") & "," & acuerdo.Desde & "," & acuerdo.Hasta & "," & acuerdo.GrupoOptico & ")"
            Dim mda As New SqlCommand(cad, mcon)
            mda.ExecuteNonQuery()
            acuerdo.id = id
            mda = Nothing
        Else
            'updatea los datos generales y elimina las lineas
            Dim m As SqlCommand
            id = acuerdo.id
            cad = "update t_acuerdos set nombre = '" & acuerdo.nombre & "',descripcion='" & acuerdo.descripcion & _
            "',consumo_minimo=" & Replace(acuerdo.ConsumoMensual, ",", ".") & ",numProgresivos=" & acuerdo.NumMinimoProgresivos & _
            ",dto_stock=" & Replace(acuerdo.dto_stock, ",", ".") & ",dto_fabricacion=" & Replace(acuerdo.dto_fabricacion, ",", ".") & _
            ",dto_progresivos=" & Replace(acuerdo.dto_Progresivos, ",", ".") & ",desde=" & acuerdo.Desde & ",hasta=" & acuerdo.Hasta & ",id_grupo=" & acuerdo.GrupoOptico & " where id_acuerdo = " & id
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
            m = Nothing
            cad = "delete from t_acuerdo_cantidades where id_acuerdo=" & id
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
            cad = "delete from t_lineas_acuerdo where id_acuerdo=" & id
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
            cad = "delete from t_acuerdo_PVP_fabricacion where id_acuerdo=" & id
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
            cad = "delete from t_acuerdo_PVP_stock where id_acuerdo=" & id
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
            cad = "delete from t_acuerdo_tratamientos where id_acuerdo=" & id
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
            cad = "delete from t_acuerdo_gamas where id_acuerdo=" & id
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
        End If
        'inserto las lineas de detalle
        For Each i As Cantidades In acuerdo.Cantidades
            Dim m As SqlCommand
            cad = "INSERT INTO t_acuerdo_cantidades (id_acuerdo,cantidad,fabricacion) VALUES (" & acuerdo.id & "," & i.id & "," & NumSql(i.Cantidad) & ")"
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
        Next
        For Each pre As Precio In acuerdo.Fabrica
            Dim m As SqlCommand
            cad = "INSERT INTO t_acuerdo_PVP_fabricacion (id_acuerdo,id_grupo,cantidad,precio) VALUES (" & id & "," & pre.id & "," & pre.Cantidad & "," & NumSql(pre.precio) & ")"
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
        Next

        For Each pre As clsPVPLenteStock In acuerdo.ModelosStock
            Dim m As SqlCommand
            cad = "INSERT INTO t_acuerdo_PVP_stock (id_acuerdo,id_grupo,id_tratamiento,diametro,cilindro,precio) VALUES (" & id & "," & pre.grupo & "," & pre.Tratamiento & "," & pre.Diametro & "," & pre.Cilindro & "," & NumSql(pre.precio) & ")"
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
        Next
        For Each pre As Precio In acuerdo.Tratamientos
            Dim m As SqlCommand
            cad = "INSERT INTO t_acuerdo_tratamientos (id_acuerdo,id_tratamiento,precio) VALUES (" & id & "," & pre.id & "," & NumSql(pre.precio) & ")"
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
        Next
        For Each pre As PrecioGamaColor In acuerdo.Colores
            Dim m As SqlCommand
            cad = "INSERT INTO t_acuerdo_gamas (id_acuerdo,id_gama,precio_li,precio_hi) VALUES (" & id & "," & pre.id & "," & NumSql(pre.PrecioLI) & "," & NumSql(pre.PrecioHI) & ")"
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
        Next
        For Each la In acuerdo
            Dim can As String = Replace(la.descuento, ",", ".")
            cad = "insert into t_lineas_acuerdo(id_acuerdo," & _
            "id_grupo,id_modo,id_tratamiento,diametro,id_graduacion,descuento,id_color,precio) values (" & id & _
            "," & la.idGrupo & "," & la.id_modo & "," & la.id_tratamiento & "," & la.diametro & "," & _
            NumSql(la.id_graduacion) & "," & can & "," & la.id_color & "," & NumSql(la.Precio) & ")"
            Dim mda2 As New SqlCommand(cad, mcon)
            mda2.ExecuteNonQuery()
            mda2 = Nothing
        Next
        mcon.Close()
    End Function
    Public Sub GrabaAcuerdosClientes(ByVal cli As ArrayList, ByVal ac As clsAcuerdo)
        Dim cad As String = "delete from t_acuerdos_clientes where id_acuerdo=" & ac.id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        For Each i As Integer In cli
            cmd.CommandText = "INSERT INTO t_acuerdos_clientes (id_acuerdo,id_cliente,desde,hasta) VALUES (" & ac.id & "," & i & "," & ac.Desde & "," & ac.Hasta & ")"
            cmd.ExecuteNonQuery()
        Next
        mcon.Close()
    End Sub
    Public Sub FirmaResponsableSanitario(ByVal pedido As Integer)
        Dim P As clsPedido = GetPedidobyId(pedido)

        Dim m As New Random

        ' Dim Minutos As Integer =
        Dim Fecha As String = P.FechaSalida & " 19:" & m.Next(1, 59)
        Fecha = ConvertDateToTimeStamp(Fecha)
        'Exit Sub


        Dim cad As String = "declare @CUENTA INTEGER" & vbNewLine & _
        "SELECT @CUENTA=count(*) from t_registro_sanitario where id_pedido=" & pedido & vbNewLine & _
        "if @cuenta=0" & vbNewLine & "BEGIN " & vbNewLine & "INSERT INTO t_registro_sanitario (fecha,responsable,id_pedido)" & _
        " VALUES (" & Fecha & "," & mUsuario.id & "," & pedido & ")" & vbNewLine
        If P.pareja <> 0 Then
            cad = cad & "INSERT INTO t_registro_sanitario (fecha,responsable,id_pedido)" & _
        " VALUES (" & Fecha & "," & mUsuario.id & "," & P.pareja & ")"
        End If
        cad = cad & vbNewLine & "END"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub GrabaOferta(ByVal oferta As clsOfertas)
        Dim cad As String = ""
        Dim la As clsLineaOferta
        Dim id As Long = getMaxId("id_oferta", "t_ofertas") + 1
        mcon.Open()
        If oferta.id = 0 Then
            'inserto los datos generales
            cad = "insert into t_ofertas (id_oferta,nombre,fec_ini,fec_fin) values (" & id & ",'" & _
            oferta.Nombre & "'," & oferta.Inicio & "," & oferta.Fin & ")"
            Dim mda As New SqlCommand(cad, mcon)
            mda.ExecuteNonQuery()
            mda = Nothing
        Else
            'updatea los datos generales y elimina las lineas
            Dim m As SqlCommand
            id = oferta.id
            cad = "update t_ofertas set nombre = '" & oferta.Nombre & "',fec_ini=" & oferta.Inicio & _
            ",fec_fin=" & oferta.Fin & " where id_oferta = " & id
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
            m = Nothing

            cad = "delete from t_lineas_oferta where id_oferta=" & id
            m = New SqlCommand(cad, mcon)
            m.ExecuteNonQuery()
        End If
        'inserto las lineas de detalle
        For Each la In oferta
            Dim can As String = Replace(la.cantidad, ",", ".")
            cad = "insert into t_lineas_oferta(id_oferta," & _
            "id_grupo,id_modo,id_tratamiento,diametro,id_graduacion,precio,id_color) values (" & id & _
            "," & la.id_modelo & "," & la.id_modo & "," & la.id_tratamiento & "," & IIf(la.diametro = "*", -1, la.diametro) & "," & _
            la.id_graduacion & "," & can & "," & la.id_color & ")"
            Dim mda2 As New SqlCommand(cad, mcon)
            mda2.ExecuteNonQuery()
            mda2 = Nothing
        Next
        mcon.Close()

    End Sub
    Public Function getMaterialById(ByVal id As Integer) As String
        Dim cad As String = "select * from m_materiales where id_material = " & id
        Dim mda As New SqlDataAdapter
        Dim tb As New DataTable
        mda.SelectCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("material")

    End Function


    Public Function getTipologiaById(ByVal id As Integer) As String
        Dim cad As String = "select * from m_tipologia where id_tipo = " & id
        Dim mda As New SqlDataAdapter
        Dim tb As New DataTable
        mda.SelectCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("tipo")
    End Function
    Public Function getTratamientoById(ByVal id As Integer) As String
        Dim cad As String = "select * from t_tratamientos where id_tratamiento = " & id
        Dim mda As New SqlDataAdapter
        Dim tb As New DataTable
        mda.SelectCommand = New SqlCommand(cad, mcon)
        If mcon.State = ConnectionState.Open Then mcon.Close()
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("nombre")
    End Function
    Public Function getNombreModeloById(ByVal id As Long) As String
        Dim cad As String = "select * from t_modelos where id_lente = " & id
        Dim mda As New SqlDataAdapter
        Dim tb As New DataTable
        mda.SelectCommand = New SqlCommand(cad, mcon)
        If mcon.State = ConnectionState.Open Then mcon.Close()
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("nombre")
    End Function
    Public Sub GrabapreciosSuplementos(ByVal tb As DataTable)
        Dim cmd As New SqlCommand
        cmd.Connection = mcon
        Try
            mcon.Open()
            For Each rw As DataRow In tb.Rows
                cmd.CommandText = "UPDATE t_suplementos set precio_base=" & NumSql(rw("precio")) & ",dto_maximo=" & NumSql(rw("dto")) & "  where id_suplemento=" & rw("id_suplemento")
                cmd.ExecuteNonQuery()
            Next
            mcon.Close()
            Exit Sub
        Catch ex As Exception
            MsgBox("error en la grabacion")
            mcon.Close()
        End Try


    End Sub
    Public Function GetPrecioTratamientoById(ByVal id As Integer) As Decimal
        Dim precio As Decimal = 0
        Dim cad As String = "select precio from t_tratamientos where id_tratamiento=" & id
        Dim CMD As New SqlCommand(cad, mcon)
        Dim cnAbierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            cnAbierta = True
        Else
            mcon.Open()
        End If
        precio = CMD.ExecuteScalar
        If cnAbierta = False Then
            mcon.Close()
        End If
        Return precio
    End Function
    Public Sub GrabapreciossuplementosByidcliente(ByVal idcli As Integer, ByVal tb As DataTable)
        Dim cad As String = "DELETE FROM t_clientes_suplementos where id_Cliente=" & idcli
        Dim cmd As New SqlCommand(cad, mcon)

        Try
            mcon.Open()
            cmd.ExecuteNonQuery()
            For Each rw As DataRow In tb.Rows
                cmd.CommandText = "INSERT INTO t_clientes_suplementos (id_cliente,id_suplemento,precio,descuento) VALUES (" & idcli & "," & rw("id_suplemento") & "," & NumSql(rw("precio")) & ",1)"
                cmd.ExecuteNonQuery()
            Next
            mcon.Close()
            Exit Sub
        Catch ex As Exception
            MsgBox("error en la grabacion")
            mcon.Close()
        End Try
    End Sub
    
    Public Sub GrabafactorConversionForma(ByVal factor As Decimal)
        Dim cad As String = "UPDATE t_equipos set conversion_forma=" & NumSql(factor) & " where equipo like " & strsql(Equipo)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Function GetFormaByPedido(ByVal idPedido As Integer) As String
        Dim cad As String = "select isnull(forma,'0') from t_formas_pedido where id_pedido=" & idPedido
        Dim cmd As New SqlCommand(cad, mcon)
        Dim forma As String = ""
        mcon.Open()
        forma = cmd.ExecuteScalar
        If IsNothing(forma) Then
            forma = ""
        End If
        mcon.Close()
        Return forma
    End Function
    Public Function GetFactorConversionforma() As Decimal
        Dim cad As String = "select isnull(conversion_forma,1) from t_equipos where equipo like " & strsql(Equipo)
        Dim Factor As Decimal
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Factor = cmd.ExecuteScalar
        mcon.Close()
        Return Factor
    End Function
    Public Function getdtoMontajeByCLiente(ByVal idcli As Integer, ByVal idmontaje As Integer, ByVal pedido As Long) As Decimal
        Dim cad As String = ""
        Dim p As New clsPedido
        If pedido = 0 Then
            cad = "select isnull(descuento,0) from t_clientes_montaje where id_cliente=" & idcli & " and id_montaje=" & idmontaje
        Else
            p = GetPedidobyId(pedido)
            If p.modo = "F" Then
                cad = "select isnull(dto_fabrica,0) from t_clientes_montaje where id_cliente=" & idcli & " and id_montaje=" & idmontaje
            Else
                cad = "select isnull(dto_stock,0) from t_clientes_montaje where id_cliente=" & idcli & " and id_montaje=" & idmontaje
            End If
        End If

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim dto As Decimal
        dto = cmd.ExecuteScalar
        mcon.Close()
        Return dto
    End Function
    Public Sub GrabaSalidaVirtual(ByVal Pedido As Integer, ByVal idlente As Integer, ByVal proveedor As String)
        Dim cad As String = "INSERT INTO t_Salidas_virtual  select " & Pedido & "," & mUsuario.id & ",dbo.FechaToTimeStamp(Getdate()),  max(id_orden)," & idlente & "," & strsql(proveedor) & " from t_ordenes_trabajo where id_pedido=" & Pedido
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Function getdtoMontajeByCLiente(ByVal idcli As Integer, ByVal idmontaje As Integer, ByVal STOCK As Boolean, ByVal Fabrica As Boolean) As Decimal
        Dim cad As String = ""
        Dim p As New clsPedido
        If Fabrica = True And STOCK = False Then
            cad = "select isnull(dto_fabrica,0) from t_clientes_montaje where id_cliente=" & idcli & " and id_montaje=" & idmontaje
        ElseIf Fabrica = False And STOCK = True Then
            cad = "select isnull(dto_stock,0) from t_clientes_montaje where id_cliente=" & idcli & " and id_montaje=" & idmontaje

        ElseIf Fabrica = True And STOCK = True Then
            cad = "select isnull(dto_stock+dto_fabrica,0)/2  from t_clientes_montaje where id_cliente=" & idcli & " and id_montaje=" & idmontaje

        End If

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim dto As Decimal = 0
        dto = cmd.ExecuteScalar
        mcon.Close()
        Return dto
    End Function

    Public Function getdtoSuplementosByCLiente(ByVal idcli As Integer, ByVal idmontaje As Integer) As Decimal
        Dim cad As String = "select isnull(precio,0) from t_clientes_suplementos where id_cliente=" & idcli & " and id_suplemento=" & idmontaje
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim dto As Decimal = 0
        dto = cmd.ExecuteScalar
        mcon.Close()
        Return dto
    End Function

    Public Sub GrabapreciosMontajes(ByVal tb As DataTable)
        Dim cmd As New SqlCommand
        cmd.Connection = mcon
        Try
            mcon.Open()
            For Each rw As DataRow In tb.Rows
                cmd.CommandText = "UPDATE t_montajes set precio=" & NumSql(rw("precio")) & ",precio_stock=" & NumSql(rw("dto_stock")) & ",precio_fabrica=" & NumSql(rw("dto_fabrica")) & " where id_montaje=" & rw("id_montaje")
                cmd.ExecuteNonQuery()
            Next
            mcon.Close()
            Exit Sub
        Catch ex As Exception
            MsgBox("error en la grabacion")
            mcon.Close()
        End Try


    End Sub
    Public Sub GrabapreciosMontajesByidcliente(ByVal idcli As Integer, ByVal tb As DataTable)
        Dim cad As String = "DELETE FROM t_clientes_montaje where id_Cliente=" & idcli
        Dim cmd As New SqlCommand(cad, mcon)

        Try
            mcon.Open()
            cmd.ExecuteNonQuery()
            For Each rw As DataRow In tb.Rows
                cmd.CommandText = "INSERT INTO t_clientes_montaje (id_cliente,id_montaje,descuento,dto_fabrica,dto_stock) VALUES (" & idcli & "," & rw("id_montaje") & "," & NumSql(rw("precio")) & "," & NumSql(rw("Dto_fabrica")) & "," & NumSql(rw("dto_stock")) & ")"
                cmd.ExecuteNonQuery()
            Next
            mcon.Close()
            Exit Sub
        Catch ex As Exception
            MsgBox("error en la grabacion")
            mcon.Close()
        End Try
    End Sub
    Public Function GetAcuerdoById(ByVal id As Long, ByVal ok As String) As DataTable

        'ahora cargo el dcetalle
        Dim tb2 As New DataTable

        'Dim mda2 As New SqlDataAdapter("select * from t_lineas_acuerdo where id_acuerdo=" & id, mcon)
        Dim cad As String = "select *,(select precio from t_precios_grupo where id_grupo=t_lineas_acuerdo.id_grupo and id_modo=t_lineas_acuerdo.id_modo and id_tratamiento=t_lineas_acuerdo.id_tratamiento and id_color=0) as preciotarifa from t_lineas_acuerdo  INNER JOIN t_grupos_modelos ON t_grupos_modelos.id_grupo=t_lineas_acuerdo.id_grupo where id_acuerdo=" & id & " order by grupo,id_tratamiento"
        Dim mda2 As New SqlDataAdapter(cad, mcon)
        mda2.Fill(tb2)
        Return tb2
    End Function

    Public Function GetAcuerdoById(ByVal id As Long, Optional ByVal Grupo As Integer = 0) As clsAcuerdo
        Dim cad As String = "select * from t_acuerdos where id_acuerdo=" & id
        Dim FiltroGrupo As String = ""
        If Grupo <> 0 Then
            FiltroGrupo = " and id_grupo=" & Grupo
        End If
        Dim ac As New clsAcuerdo
        If id = 0 Then Return ac
        Dim mda As New SqlDataAdapter
        Dim tb As New DataTable
        mda.SelectCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.Fill(tb)

        ac.id = id
        ac.nombre = tb.Rows(0)("nombre")
        ac.descripcion = tb.Rows(0)("descripcion")
        ac.ConsumoMensual = tb.Rows(0)("consumo_minimo")
        ac.NumMinimoProgresivos = tb.Rows(0)("numProgresivos")
        ac.dto_stock = tb.Rows(0)("dto_stock")
        ac.dto_fabricacion = tb.Rows(0)("dto_fabricacion")
        ac.dto_Progresivos = tb.Rows(0)("dto_progresivos")
        ac.Desde = tb.Rows(0)("desde")
        ac.Hasta = tb.Rows(0)("hasta")
        ac.GrupoOptico = tb.Rows(0)("id_grupo")
        'ahora cargo el dcetalle
        Dim tb2 As New DataTable
        'Dim mda2 As New SqlDataAdapter("select * from t_lineas_acuerdo where id_acuerdo=" & id, mcon)

        Dim mda2 As New SqlDataAdapter("select *,(select precio from t_precios_grupo where id_grupo=t_lineas_acuerdo.id_grupo and id_modo=t_lineas_acuerdo.id_modo and id_tratamiento=t_lineas_acuerdo.id_tratamiento and diametro=t_lineas_acuerdo.diametro and id_color=0) as preciotarifa from t_lineas_acuerdo  INNER JOIN t_grupos_modelos ON t_grupos_modelos.id_grupo=t_lineas_acuerdo.id_grupo where id_acuerdo=" & id & IIf(FiltroGrupo.Length = 0, "", " and t_lineas_acuerdo.id_grupo=" & Grupo) & " order by grupo,id_tratamiento, diametro desc,id_graduacion desc", mcon)
        mda2.Fill(tb2)
        mcon.Close()
        Dim i As Integer
        For i = 0 To tb2.Rows.Count - 1
            Dim la As New clsAcuerdoLinea
            la.descuento = tb2.Rows(i)("descuento")
            la.idGrupo = tb2.Rows(i)("id_grupo")
            la.id_tratamiento = tb2.Rows(i)("id_tratamiento")
            la.id_modo = tb2.Rows(i)("id_modo")
            la.id_graduacion = tb2.Rows(i)("id_graduacion")
            la.id_color = tb2.Rows(i)("id_color")
            la.Grupo = tb2.Rows(i)("grupo")
            la.tratamiento = "*"
            la.modo = "*"
            la.Graduacion = "*"
            la.Precio = tb2.Rows(i)("precio")
            la.diametro = tb2.Rows(i)("diametro")
            la.TarifaNormal = IIf(IsDBNull(tb2.Rows(i)("preciotarifa")), 0, tb2.Rows(i)("preciotarifa"))


            'If la.idGrupo <> -1 Then la.Grupo = GetGrupoByid(la.Grupo)
            If la.id_tratamiento <> -1 Then la.tratamiento = getTratamientoById(la.id_tratamiento)
            If la.id_modo <> -1 Then la.modo = getModoById(la.id_modo)
            la.color = GetColorById(la.id_color)
            If tb2.Rows(i)("id_graduacion") <> -1 Then
                If tb2.Rows(i)("id_graduacion") = 1 Then
                    la.Graduacion = "Menisco"
                Else
                    la.Graduacion = "Tórico"
                End If
            End If

            'la.modo = tb.Rows(i)("modo")

            ac.add(la)
            la = Nothing
        Next
        tb.Clear()
        mda.SelectCommand.CommandText = "select * from t_acuerdo_cantidades where id_acuerdo=" & id & " order by cantidad"
        mda.Fill(tb)
        For Each r As DataRow In tb.Rows
            Dim c As New Cantidades
            c.id = r("cantidad")
            c.Cantidad = r("fabricacion")
            ac.Cantidades.Add(c)
        Next
        tb.Clear()
        tb.Clear()
        If Grupo <> 0 Then
            FiltroGrupo = " and t_acuerdo_PVP_fabricacion.id_grupo=" & Grupo
        End If
        mda.SelectCommand.CommandText = "select t_acuerdo_PVP_fabricacion.* from t_acuerdo_PVP_fabricacion INNER JOIN t_grupos_modelos ON t_acuerdo_PVP_fabricacion.id_grupo=t_grupos_modelos.id_grupo where id_acuerdo=" & id & FiltroGrupo & " order by grupo"
        mda.Fill(tb)
        For Each r As DataRow In tb.Rows
            Dim pre As New Precio
            pre.id = r("id_grupo")
            pre.cantidad = r("cantidad")
            pre.precio = r("precio")
            ac.Fabrica.Add(pre)
        Next
        tb.Clear()
        If Grupo <> 0 Then
            FiltroGrupo = " and " & Grupo & "=t_acuerdo_PVP_stock.id_grupo"
        End If
        tb.Clear()
        mda.SelectCommand.CommandText = "select * from t_acuerdo_PVP_stock where id_acuerdo=" & id & FiltroGrupo & " order by id_grupo,id_tratamiento, diametro desc"
        mda.Fill(tb)
        For Each r As DataRow In tb.Rows
            Dim pre As New clsPVPLenteStock
            pre.grupo = r("id_grupo")
            pre.Tratamiento = r("id_tratamiento")
            pre.Diametro = r("diametro")
            pre.Cilindro = r("cilindro")
            pre.precio = r("precio")
            ac.ModelosStock.Add(pre)
        Next


        'vamos a ver los precios de tratamiento y de gamas que tiene
        tb.Clear()
        mda.SelectCommand.CommandText = "select * from t_acuerdo_tratamientos where id_acuerdo=" & id & " order by id_tratamiento"
        mda.Fill(tb)
        For Each r As DataRow In tb.Rows
            Dim pre As New Precio
            pre.id = r("id_tratamiento")

            pre.precio = r("precio")
            ac.Tratamientos.Add(pre)
        Next
        tb.Clear()
        mda.SelectCommand.CommandText = "select * from t_acuerdo_gamas where id_acuerdo=" & id & " order by id_gama"
        mda.Fill(tb)
        For Each r As DataRow In tb.Rows
            Dim pre As New PrecioGamaColor
            pre.id = r("id_gama")
            pre.PreciolI = r("precio_li")
            pre.PrecioHI = r("precio_hi")
            ac.Colores.Add(pre)
        Next
        Return ac
    End Function
    Public Function GetGrupoByid(ByVal id As Integer) As String
        Dim grupo As String = ""
        Dim cad As String = "select grupo from t_grupos_modelos where id_grupo=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        grupo = cmd.ExecuteScalar
        mcon.Close()
        Return grupo
    End Function
    Public Function GetGrupoByModelo(ByVal id As Integer) As Integer
        Dim grupo As String = ""
        Dim cad As String = "select id_grupo from t_grupos_modelos where id_grupo=(select id_grupo from t_modelos where id_lente=" & id & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        grupo = cmd.ExecuteScalar
        mcon.Close()
        Return grupo
    End Function
    Public Function EjecutaSentencia(ByVal sentencia As String) As DataTable
        Dim mda As New SqlDataAdapter(sentencia, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    Public Sub GrabaVisitaComercial(ByVal V As clsVisitaComercial)
        Dim cad As String = ""
        V.Visita = getMaxId("id_visita", "comercial.dbo.t_visitas") + 1
        cad = "INSERT INTO comercial.dbo.t_visitas (id_visita,id_comercial,fecha,hora,duracion,notas, id_contacto)" & _
        " VALUES (" & V.Visita & "," & V.Comercial & "," & V.Fecha & "," & V.Hora & "," & V.Duracion & "," & strsql(V.Notas) & "," & V.Contacto & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        For Each i As Integer In V.Materiales
            cmd.CommandText = "INSERT INTO comercial.dbo.t_materiales_visita (id_visita,id_material) VALUES (" & V.Visita & "," & i & ")"
            cmd.ExecuteNonQuery()
        Next
        mcon.Close()
    End Sub
    Public Function GetPreciosEspecialesCli(ByVal idcli As Integer) As DataTable
        Dim tb As New DataTable

        Dim cad As String = "select grupo,case  convert(varchar,diametro) when '-1' then '*' else  convert(varchar,diametro) END as Diametro, t_tratamientos.nombre as tratamiento,t_precios_cliente.descuento,(select precio  from t_precios_grupo where id_grupo=t_precios_cliente.id_grupo and id_modo=t_precios_cliente.id_modo and id_tratamiento=t_precios_cliente.id_tratamiento and id_color=0) as preciotarifa,(select convert(decimal(8,2),precio*(1-t_precios_cliente.descuento/100))  from t_precios_grupo where id_grupo=t_precios_cliente.id_grupo and id_modo=t_precios_cliente.id_modo and id_tratamiento=t_precios_cliente.id_tratamiento and id_color=0) as [Precio Acuerdo] from t_precios_cliente INNER JOIN t_grupos_modelos ON t_precios_cliente.id_grupo=t_grupos_modelos.id_grupo INNER JOIN t_tratamientos ON t_tratamientos.id_tratamiento=t_precios_cliente.id_tratamiento where id_cliente=" & idcli & " order by grupo,t_tratamientos.id_tratamiento,diametro"
        Dim mda2 As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda2.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetPreciosByCliente(ByVal idcli As Integer) As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select t_precios_cliente.id_grupo,desde,hasta,id_modo,t_tratamientos.id_tratamiento,case id_modo when 1 then 'S' when 2 then 'T' when 3 then 'F' END as modo,grupo, Diametro,cilindro, t_tratamientos.nombre as tratamiento,id_color,(select denominacion from t_gamas_coloracion where id_gama=id_color) as gama, t_precios_cliente.precio  from t_precios_cliente INNER JOIN t_grupos_modelos ON t_precios_cliente.id_grupo=t_grupos_modelos.id_grupo INNER JOIN t_tratamientos ON t_tratamientos.id_tratamiento=t_precios_cliente.id_tratamiento WHERE id_cliente=" & idcli & " and hasta>=" & FechaAcadena(Now.Date) & " order by grupo,t_tratamientos.id_tratamiento,diametro,cilindro,desde"
        Dim mda2 As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda2.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetPreciosEspeciales() As DataTable
        Dim tb As New DataTable
        Dim Filtro As String = ""
        If mUsuario.Comercial = True Then
            Filtro = " where id_comercial=" & mUsuario.id
        End If
        'Dim cad As String = "select codigo,nombre_comercial as cliente,grupo,case  convert(varchar,id_modo) when '-1' then '*' when 1 then  'S' when 2 then 'T' when 3 then 'F' END as Modo,case  convert(varchar,diametro) when '-1' then '*' else  convert(varchar,diametro) END as Diametro, t_tratamientos.nombre as tratamiento,denominacion as [Gama Color],t_precios_cliente.precio,(select precio  from t_precios_grupo where id_grupo=t_precios_cliente.id_grupo and id_modo=t_precios_cliente.id_modo and id_tratamiento=t_precios_cliente.id_tratamiento and id_color=0) as preciotarifa,(select convert(decimal(8,2),precio*(1-t_precios_cliente.descuento/100))  from t_precios_grupo where id_grupo=t_precios_cliente.id_grupo and id_modo=t_precios_cliente.id_modo and id_tratamiento=t_precios_cliente.id_tratamiento and id_color=0) as [Precio Acuerdo] from t_clientes INNER JOIN t_precios_cliente ON t_clientes.id_cliente=t_precios_cliente.id_cliente INNER JOIN t_grupos_modelos ON t_precios_cliente.id_grupo=t_grupos_modelos.id_grupo INNER JOIN t_tratamientos ON t_tratamientos.id_tratamiento=t_precios_cliente.id_tratamiento INNER JOIN t_gamas_coloracion ON t_precios_clientes.id_color=t_gamas_coloracion.id_gama  order by codigo,nombre_comercial,grupo,t_tratamientos.id_tratamiento,diametro"
        Dim cad As String = "select codigo,nombre_comercial as cliente,grupo,case  convert(varchar,id_modo) when '-1' then '*' when 1 then  'S' when 2 then 'T' when 3 then 'F' END as Modo,case  convert(varchar,diametro) when '-1' then '*' else  convert(varchar,diametro) END as Diametro, t_tratamientos.nombre as tratamiento,denominacion as [Gama Color],t_precios_cliente.precio   from t_clientes INNER JOIN t_precios_cliente ON t_clientes.id_cliente=t_precios_cliente.id_cliente INNER JOIN t_grupos_modelos ON t_precios_cliente.id_grupo=t_grupos_modelos.id_grupo INNER JOIN t_tratamientos ON t_tratamientos.id_tratamiento=t_precios_cliente.id_tratamiento INNER JOIN t_gamas_coloracion ON t_precios_cliente.id_color=t_gamas_coloracion.id_gama " & Filtro & "  order by codigo,nombre_comercial,grupo,t_tratamientos.id_tratamiento,diametro"
        Dim mda2 As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda2.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetPlantillasBonos() As DataTable
        Dim tb As New DataTable

        'Dim cad As String = "select codigo,nombre_comercial as cliente,grupo,case  convert(varchar,id_modo) when '-1' then '*' when 1 then  'S' when 2 then 'T' when 3 then 'F' END as Modo,case  convert(varchar,diametro) when '-1' then '*' else  convert(varchar,diametro) END as Diametro, t_tratamientos.nombre as tratamiento,denominacion as [Gama Color],t_precios_cliente.precio,(select precio  from t_precios_grupo where id_grupo=t_precios_cliente.id_grupo and id_modo=t_precios_cliente.id_modo and id_tratamiento=t_precios_cliente.id_tratamiento and id_color=0) as preciotarifa,(select convert(decimal(8,2),precio*(1-t_precios_cliente.descuento/100))  from t_precios_grupo where id_grupo=t_precios_cliente.id_grupo and id_modo=t_precios_cliente.id_modo and id_tratamiento=t_precios_cliente.id_tratamiento and id_color=0) as [Precio Acuerdo] from t_clientes INNER JOIN t_precios_cliente ON t_clientes.id_cliente=t_precios_cliente.id_cliente INNER JOIN t_grupos_modelos ON t_precios_cliente.id_grupo=t_grupos_modelos.id_grupo INNER JOIN t_tratamientos ON t_tratamientos.id_tratamiento=t_precios_cliente.id_tratamiento INNER JOIN t_gamas_coloracion ON t_precios_clientes.id_color=t_gamas_coloracion.id_gama  order by codigo,nombre_comercial,grupo,t_tratamientos.id_tratamiento,diametro"
        Dim cad As String = "select * from t_plantilla_bono order by nombre"
        Dim mda2 As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda2.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function BorraPlantillaBono(ByVal id As Integer) As Boolean
        Dim cmd As New SqlCommand("select count(*) from t_lineas_bono_cliente where id_bono=" & id, mcon)
        Dim cnnAbierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            cnnAbierta = True
        Else
            mcon.Open()
        End If
        Dim Existe As Integer = cmd.ExecuteScalar
        If Existe <> 0 Then
            MsgBox("No se puede eliminar el bono ya que se encuentra incluidos en bonos de clientes")
            If cnnAbierta = False Then
                mcon.Close()
            End If
            Return True
        Else
            'borramos el bono
            cmd.CommandText = "DELETE FROM t_plantilla_bono where id_bono=" & id & " DELETE FROM t_bono_tratamientos WHERE id_bono=" & id & " DELETE FROM t_bonos_modelos WHERE Iid_bono=" & id
            cmd.ExecuteNonQuery()
            If cnnAbierta = False Then
                mcon.Close()
            End If
            Return False
        End If
    End Function
    Public Function GetPlantillaBono(ByVal idBono As Integer) As clsPlantillaBono
        Dim Bono As New clsPlantillaBono
        Dim cad As String = "select * from t_plantilla_bono where id_bono=" & idBono
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        If tb.Rows.Count = 1 Then 'hemos encontrado la plantilla, vamos a cargar los datos
            Bono.Lentes = tb.Rows(0)("lentes")
            Bono.Id = idBono
            Bono.Nombre = tb.Rows(0)("nombre")
            Bono.Descripcion = tb.Rows(0)("descripcion")
            Bono.Precio = tb.Rows(0)("precio")
            Bono.Caducidad = tb.Rows(0)("caducidad")
            'buscamos los tratamientos incluidos
            tb.Clear()
            cad = "select * from t_bono_tratamientos where id_bono=" & idBono
            mda.SelectCommand.CommandText = cad
            mda.Fill(tb)
            For Each rw As DataRow In tb.Rows
                Bono.Tratamientos.Add(rw("id_tratamiento"))

            Next
            'lo mismo pra los grupos de modelos
            tb.Clear()
            cad = "select * from t_bono_modelos where id_bono=" & idBono
            mda.SelectCommand.CommandText = cad
            mda.Fill(tb)
            For Each rw As DataRow In tb.Rows
                Bono.Modelos.Add(rw("id_grupo"))
            Next
        End If
        Return Bono
    End Function
    Public Sub GrabaPlantillaBono(ByRef b As clsPlantillaBono)
        Dim cad As String
        If b.Id = 0 Then 'bono Nuevo, Insertamos
            b.Id = getMaxId("id_bono", "t_plantilla_bono") + 1
            cad = "Insert into t_plantilla_bono (id_bono,nombre,descripcion,caducidad,lentes,precio) VALUES (" & b.Id & "," & strsql(b.Nombre) & "," & strsql(b.Descripcion) & "," & b.Caducidad & "," & b.Lentes & "," & NumSql(b.Precio) & ")"
        Else
            cad = "UPDATE t_plantilla_bono set nombre=" & strsql(b.Nombre) & ",descripcion=" & strsql(b.Descripcion) & ",caducidad=" & b.Caducidad & ",lentes=" & b.Lentes & ",precio=" & NumSql(b.Precio) & " where id_bono=" & b.Id
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        cmd.CommandText = "DELETE FROM t_bono_tratamientos where id_bono=" & b.Id & " DELETE FROM t_bono_modelos where id_bono=" & b.Id
        cmd.ExecuteNonQuery()
        For Each i As Integer In b.Tratamientos
            cmd.CommandText = "INSERT INTO t_bono_tratamientos (id_bono,id_tratamiento) VALUES (" & b.Id & "," & i & ")"
            cmd.ExecuteNonQuery()
        Next
        For Each i As Integer In b.Modelos
            cmd.CommandText = "INSERT INTO t_bono_modelos (id_bono,id_grupo) VALUES (" & b.Id & "," & i & ")"
            cmd.ExecuteNonQuery()
        Next
        mcon.Close()
    End Sub
    Public Function GetBonoCliente(ByVal idBono As Integer) As clsBonoCliente
        Dim B As New clsBonoCliente
        Dim tb As New DataTable
        Dim cad As String = "Select * from t_bonos_cliente where id_bono_cliente=" & idBono
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        If tb.Rows.Count = 1 Then
            B.id = tb.Rows(0)("id_bono_cliente")
            B.Cliente = tb.Rows(0)("id_cliente")
            B.Factura = tb.Rows(0)("id_factura")
            B.Caducidad = tb.Rows(0)("caduca")
        End If
        tb.Clear()
        mda.SelectCommand.CommandText = "Select * from t_lineas_bono_cliente where id_bono_cliente=" & B.id
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows
            'añadimos las lineas del bono 
            Dim l As New clsLineaBonoCliente
            l.idBono = rw("id_bono")

            l.Lentes = rw("lentes")
            l.Precio = rw("precio")
            l.Caducidad = rw("caducidad")
            l.Consumo = rw("consumo")
            B.AddLinea(l)
        Next
        Return B
    End Function

    Public Sub ActualizaConsumoBonoCliente(ByVal idBonoCliente As Integer, ByVal idBono As Integer)
        Dim cad As String = "Update t_lineas_bono_cliente set consumo=consumo+1 where id_bono_cliente=" & idBonoCliente & " and id_bono=" & idBono
        Dim cnnAbierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            cnnAbierta = True
        Else
            mcon.Open()
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        cmd.ExecuteNonQuery()
        If cnnAbierta = False Then
            mcon.Close()
        End If
    End Sub

    Public Sub GrabaBonoCLiente(ByVal b As clsBonoCliente)
        Dim cad As String = ""

        'si esta facturado mandamos un mensaje de error y salimos
        If b.Factura <> 0 Then
            MsgBox("El bono ya se encuentra facturado por lo que no se podra modificar")

        End If
        Dim cnnAbierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            cnnAbierta = True
        Else
            mcon.Open()
        End If
        Dim cmd As New SqlCommand(cad, mcon)

        If b.id = 0 Then 'se trata de un bono nuevo vamoa a insertar primero el bono
            b.id = getMaxId("id_bono_cliente", "t_bonos_cliente") + 1
            cad = "INSERT INTO T_bonos_cliente (id_bono_cliente,id_cliente,caduca,id_factura) VALUES (" & b.id & "," & b.Cliente & "," & b.Caducidad & "," & b.Factura & ")"
            cmd.CommandText = cad
            cmd.ExecuteNonQuery()
        End If
        'ahora borramos las lineas del bono y las insertamos
        cmd.CommandText = "DELETE FROM t_lineas_bono_cliente where id_bono_cliente=" & b.id
        cmd.ExecuteNonQuery()
        For Each l As clsLineaBonoCliente In b.Lineas
            cmd.CommandText = "insert into t_lineas_bono_cliente (id_bono_cliente,id_bono,lentes,caducidad,precio,consumo) VALUES (" & b.id & "," & l.idBono & "," & l.Lentes & "," & l.Caducidad & "," & NumSql(l.Precio) & "," & l.Consumo & ")"
            cmd.ExecuteNonQuery()
        Next
        If cnnAbierta = False Then
            mcon.Close()
        End If
    End Sub
    Public Function GetCalculoFF(ByVal parametro As String, ByVal pedido As Integer) As String
        Dim cad As String = "Select isnull(valor,'') as valor from t_calculos_pedido where parametro=" & strsql(parametro) & " and id_pedido=" & pedido
        Dim mda As New SqlCommand(cad, mcon)
        mcon.Open()
        GetCalculoFF = mda.ExecuteScalar
        If GetCalculoFF = Nothing Then GetCalculoFF = "    "
        mcon.Close()
    End Function
    Public Function EstadisticasConsumoByCliente(ByVal idcli As Integer) As DataTable
        Dim mesactual As Integer = Month(Now.Date)
        Dim fechaactual As Date
        Dim fecMes1 As Date
        Dim Fecmes2 As Date
        Dim Fecmes3 As Date
        fechaactual = "01/" & Format(Now.Month, "00") & "/" & Now.Year
        fecMes1 = DateAdd(DateInterval.Month, -1, fechaactual)
        Fecmes2 = DateAdd(DateInterval.Month, -2, fechaactual)
        Fecmes3 = DateAdd(DateInterval.Month, -2, fechaactual)
        Dim cad As String = "select id_grupo,grupo,modo,count(*)  as Pedidos" & _
        "(select count(*) from t_pedidos where id_pedido in (select id_pedido from t_lineas_albaran where id_albaran in (select id_albaran from t_albaranes where id_alb_abono<>0))) as Abonos" & _
        " from t_grupos_modelo INNER JOIN t_modelos ON t_modelos.id_grupo=t_grupos_modelo.id_grupo INNER JOIN t_pedidos ON t_pedidos.id_modelo=t_modelos.id_lente" & _
        "  where anulado=0 and fecha>=" & FechaAcadena(fechaactual) & " and fecha<=" & FechaAcadena(fechaactual) + 30 & " GROUP BY id_grupo,grupo,modo"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetPreciosEspecialesCliente(ByVal idcli As Integer) As clsAcuerdo
        Dim tb2 As New DataTable
        Dim ac As New clsAcuerdo
        ac.nombre = "Precios Especiales Cliente"
        Dim cad As String = "select grupo,t_precios_cliente.*,(select precio  from t_precios_grupo where id_grupo=t_precios_cliente.id_grupo and id_modo=t_precios_cliente.id_modo and id_tratamiento=t_precios_cliente.id_tratamiento and id_color=0) as preciotarifa from t_precios_cliente INNER JOIN t_grupos_modelos ON t_precios_cliente.id_grupo=t_grupos_modelos.id_grupo where id_cliente=" & idcli & " order by grupo,id_tratamiento"
        Dim mda2 As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda2.Fill(tb2)
        mcon.Close()

        For i As Integer = 0 To tb2.Rows.Count - 1
            Dim la As New clsAcuerdoLinea
            la.descuento = tb2.Rows(i)("descuento")
            la.idGrupo = tb2.Rows(i)("id_grupo")
            la.id_tratamiento = tb2.Rows(i)("id_tratamiento")
            la.id_modo = tb2.Rows(i)("id_modo")
            la.id_graduacion = tb2.Rows(i)("id_graduacion")
            la.id_color = tb2.Rows(i)("id_color")
            la.Grupo = tb2.Rows(i)("grupo")
            la.tratamiento = "*"
            la.modo = "*"
            la.Graduacion = "*"
            la.diametro = IIf(CInt(tb2.Rows(i)("diametro")) <> -1, tb2.Rows(i)("diametro"), "*")
            la.TarifaNormal = IIf(IsDBNull(tb2.Rows(i)("preciotarifa")), 0, tb2.Rows(i)("preciotarifa"))

            If la.idGrupo <> -1 Then la.Grupo = GetGrupoByid(la.idGrupo)
            If la.id_tratamiento <> -1 Then la.tratamiento = getTratamientoById(la.id_tratamiento)
            If la.id_modo <> -1 Then la.modo = getModoById(la.id_modo)
            la.color = GetColorById(la.id_color)
            If tb2.Rows(i)("id_graduacion") <> -1 Then
                If tb2.Rows(i)("id_graduacion") = 1 Then
                    la.Graduacion = "Menisco"
                Else
                    la.Graduacion = "Tórico"
                End If
            End If

            'la.modo = tb.Rows(i)("modo")

            ac.add(la)
            la = Nothing
        Next
        Return ac
    End Function
    Public Function PedidoFueraRango(ByVal mp As clsPedido) As Boolean
        Dim m As New clsModelo
        m = getClsModeloById(mp.id_modelo)

        Dim potencia As Decimal = mp.cilindro + mp.esfera
        If mp.esfera < 0 Then
            potencia = mp.esfera
        End If
        If m.RangoDesde <= potencia And m.RangoHasta >= potencia Then
            Return False
        Else
            Return True
        End If


    End Function
    Public Function GetAcuerdoAexcelById(ByVal id As Long, Optional ByVal proxima As Boolean = False) As DataTable


        'ahora cargo el dcetalle
        Dim tb2 As New DataTable
        'Dim mda2 As New SqlDataAdapter("select * from t_lineas_acuerdo where id_acuerdo=" & id, mcon)
        Dim camposextras As String = ""
        If proxima = True Then
            camposextras = ", convert(decimal(8,2),(1-descuento/100)*(select precio from t_precios_grupo where id_grupo=t_lineas_acuerdo.id_grupo and id_modo=t_lineas_acuerdo.id_modo and id_tratamiento=t_lineas_acuerdo.id_tratamiento and (diametro=t_lineas_acuerdo.diametro or diametro=-1) and id_color=0)) as ProximoPrecioAcuerdo"
        End If
        Dim cad As String = "select grupo, case id_modo when 1 then 'S' when 2 then 'T' when 3 then 'F' end as modo, case diametro when -1 then '*' else convert(varchar,diametro) END as diametro,(select nombre from t_tratamientos where id_tratamiento=t_lineas_acuerdo.id_tratamiento) as tratamiento,(select precio from t_precios_grupo where id_grupo=t_lineas_acuerdo.id_grupo and id_modo=t_lineas_acuerdo.id_modo and id_tratamiento=t_lineas_acuerdo.id_tratamiento and (diametro=t_lineas_acuerdo.diametro or diametro=-1) and id_color=0) as preciotarifa,descuento, convert(decimal(8,2),(1-descuento/100)*(select precio from t_precios_grupo where id_grupo=t_lineas_acuerdo.id_grupo and id_modo=t_lineas_acuerdo.id_modo and id_tratamiento=t_lineas_acuerdo.id_tratamiento and (diametro=t_lineas_acuerdo.diametro or diametro=-1) and id_color=0)) as precioAcuerdo" & camposextras & " from t_lineas_acuerdo  INNER JOIN t_grupos_modelos ON t_grupos_modelos.id_grupo=t_lineas_acuerdo.id_grupo where id_acuerdo=" & id & " order by grupo,id_tratamiento"

        Dim mda2 As New SqlDataAdapter(cad, mcon)
        mda2.Fill(tb2)
        mcon.Close()

        Return tb2
    End Function
    Public Function getGafa() As DataTable
        Dim cad As String = "select * from t_tipos_gafa where id_gafa>0"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function getTiposMontaje(Optional ByVal idgafa As Integer = 0) As DataTable
        Dim cad As String = "select * from t_tipos_montaje where id_tipo_montaje in (select id_tipo_montaje from t_montajes where id_gafa=" & idgafa & " or id_gafa=0)"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function getMontaje(ByVal tipoMontaje As Integer, ByVal gafa As Integer) As DataTable
        Dim cad As String = "select * from t_montajes where id_tipo_montaje=" & tipoMontaje & " and (id_gafa=" & gafa & " or id_gafa=0)"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function getMontajes() As DataTable


        Dim cad As String = "select t_montajes.*,(select tipo_montaje from t_tipos_montaje where id_tipo_montaje=t_montajes.id_tipo_montaje) as tipo from t_montajes order by id_tipo_montaje"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function getMontajesByidCliente(ByVal idcli As Integer) As DataTable


        Dim cad As String = "select t_montajes.*,isnull((select descuento from t_clientes_montaje where id_cliente=" & idcli & " and id_montaje=t_montajes.id_montaje),0) as descuento,isnull((select dto_stock from t_clientes_montaje where id_cliente=" & idcli & " and id_montaje=t_montajes.id_montaje),0) as dto_stock" & _
        ",isnull((select dto_fabrica from t_clientes_montaje where id_cliente=" & idcli & " and id_montaje=t_montajes.id_montaje),0) as dto_fabrica, (select tipo_montaje from t_tipos_montaje where id_tipo_montaje=t_montajes.id_tipo_montaje) as tipo from t_montajes order by id_tipo_montaje"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetSuplementosByCliente(ByVal idcliente As Integer) As DataTable
        Dim cad As String = "select t_suplementos.id_suplemento,nombre,precio_base,isnull((select precio from t_clientes_suplementos where t_suplementos.id_suplemento=t_clientes_suplementos.id_suplemento and id_cliente=" & idcliente & "),0) as precio,dto_maximo" & _
        ", isnull((select descuento from t_clientes_suplementos where t_suplementos.id_suplemento=t_clientes_suplementos.id_suplemento and id_cliente=" & idcliente & "),0) as descuento from  t_suplementos"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        Return tb
    End Function
    Public Function iNFSuplementosByCliente(ByVal idcliente As Integer) As DataTable
        Dim cad As String = "select nombre,precio_base,PRECIO_BASE* (1-PRECIO/100) AS PrecioCliente" & _
        "  from  t_suplementos INNER JOIN t_clientes_suplementos ON t_suplementos.id_suplemento=t_clientes_suplementos.id_suplemento where id_cliente=" & idcliente
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetSuplementos() As DataTable
        Dim cad As String = "select * from t_suplementos ORDER BY id_suplemento"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetPrecioMontaje(ByVal idmontaje As Integer, Optional ByVal Stock As Boolean = False, Optional ByVal Fabri As Boolean = False) As Decimal
        Dim TipoPrecio As String = "Precio"
        If Stock = True And Fabri = True Then
            TipoPrecio = "(precio_stock+precio_fabrica)/2 as precio"
        ElseIf Stock = True Then
            TipoPrecio = "precio_stock as precio"
        ElseIf Fabri = True Then
            TipoPrecio = "precio_fabrica as precio"
        End If
        Dim cad As String = "select " & TipoPrecio & " from t_montajes where id_montaje=" & idmontaje
        Dim precio As Decimal = 0
        Dim cmd As New SqlCommand(cad, mcon)

        mcon.Open()
        precio = cmd.ExecuteScalar()
        mcon.Close()
        Return precio

    End Function
    Public Function getLentesMontaje(ByVal idcli As Integer, ByVal ojo As String) As DataTable

        Dim CampoOjo As String = IIf(ojo = "D", "ojo_dcho", "ojo_izq")
        Dim cad As String = "select * from t_pedidos where id_cliente=" & idcli & " and montaje<>0  and ojo='" & ojo & "' and  id_pedido not in (select " & CampoOjo & " from t_pedidos_montajes where id_cliente=" & idcli & ")"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetOfertabyID(ByVal id As Integer) As clsOfertas
        Dim cad As String = "select * from t_ofertas where id_oferta=" & id

        Dim ac As New clsOfertas

        Dim mda As New SqlDataAdapter
        Dim tb As New DataTable
        mda.SelectCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.Fill(tb)

        ac.id = id
        ac.Nombre = tb.Rows(0)("nombre")

        ac.Inicio = tb.Rows(0)("fec_ini")
        ac.Fin = tb.Rows(0)("fec_fin")
        'ahora cargo el dcetalle
        Dim tb2 As New DataTable
        'Dim mda2 As New SqlDataAdapter("select * from t_lineas_acuerdo where id_acuerdo=" & id, mcon)

        Dim mda2 As New SqlDataAdapter("select * from t_lineas_oferta inner join t_grupos_modelos on t_lineas_oferta.id_grupo = t_grupos_modelos.id_grupo where id_oferta=" & id & " order by grupo,id_tratamiento", mcon)
        mda2.Fill(tb2)
        mcon.Close()
        Dim i As Integer
        For i = 0 To tb2.Rows.Count - 1
            Dim la As New clsLineaOferta
            la.cantidad = tb2.Rows(i)("precio")
            la.id_modelo = tb2.Rows(i)("id_grupo")
            la.id_tratamiento = tb2.Rows(i)("id_tratamiento")
            la.id_modo = tb2.Rows(i)("id_modo")
            la.id_graduacion = tb2.Rows(i)("id_graduacion")
            la.id_color = tb2.Rows(i)("id_color")
            la.modelo = "*"
            la.tratamiento = "*"
            la.modo = "*"
            la.Graduacion = "*"
            la.diametro = IIf(CInt(tb2.Rows(i)("diametro")) <> -1, tb2.Rows(i)("diametro"), "*")


            If la.id_modelo <> -1 Then la.modelo = GetGrupoByid(la.id_modelo)
            If la.id_tratamiento <> -1 Then la.tratamiento = getTratamientoById(la.id_tratamiento)
            If la.id_modo <> -1 Then la.modo = getModoById(la.id_modo)
            la.color = GetColorById(la.id_color)
            If tb2.Rows(i)("id_graduacion") <> -1 Then
                If tb2.Rows(i)("id_graduacion") = 1 Then
                    la.Graduacion = "Menisco"
                Else
                    la.Graduacion = "Tórico"
                End If
            End If

            'la.modo = tb.Rows(i)("modo")

            ac.add(la)
            la = Nothing
        Next
        Return ac
    End Function
    Public Function getModoById(ByVal idModo As Integer) As String
        Dim tb As New DataTable
        Dim cad As String = "Select * from t_modos where id_modo=" & idModo
        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        If mcon.State = ConnectionState.Open Then mcon.Close()
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("descripcion")
    End Function
    Public Function getAcuerdos() As clsAcuerdos
        Dim cad As String = "select * from t_acuerdos where baja = 0"
        Dim Acs As New clsAcuerdos

        If mUsuario.Comercial = True Then
            cad = cad & " and id_acuerdo in (select id_acuerdo from t_acuerdos_clientes where id_cliente in (select id_cliente from t_clientes where id_comercial=" & mUsuario.id & "))"
        End If
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter
        mda.SelectCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Dim i As Integer
        For i = 0 To tb.Rows.Count - 1
            Dim a As New clsAcuerdo
            a.id = tb.Rows(i)("id_acuerdo")
            a.nombre = tb.Rows(i)("nombre")
            a.descripcion = tb.Rows(i)("descripcion")
            Acs.add(a)
            a = Nothing
        Next
        Return Acs
    End Function
    Public Sub AgregarClienteAcuerdo(ByVal idCliente As Long, ByVal idAcuerdo As Long, ByVal fecha As String, ByVal cantidad As Decimal)
        'asociamos cliente
        Dim can As String = Replace(cantidad, ",", ".")
        Dim cad As String = "insert into t_acuerdos_clientes values(" & idAcuerdo & "," & idCliente & "," & FechaAcadena(fecha) & ")"
        Dim mda As New SqlCommand(cad, mcon)
        mcon.Open()
        mda.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub EliminaClienteSinOferta(ByVal idcliente As Long, ByVal idOferta As Long)
        Dim cad As String = "DELETE FROM t_clientes_sin_oferta where id_cliente=" & idcliente & " and id_oferta=" & idOferta
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub GrabaClienteSinPorte(ByVal idCli As Integer, ByVal Cantidad As Decimal)
        Dim cad As String = "declare @cuenta integer" & vbNewLine & "select @cuenta=count(*) from t_clientes_sin_porte where id_cliente=" & idCli & vbNewLine & _
        "if @cuenta=0 " & vbNewLine & "INSERT INTO t_clientes_sin_porte (id_cliente,fabricacion) VALUES (" & idCli & "," & NumSql(Cantidad) & ")" & vbNewLine & _
        "if @cuenta=1 " & vbNewLine & "UPDATE t_clientes_sin_porte SET fabricacion=" & NumSql(Cantidad) & " where id_cliente=" & idCli
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Sub EliminaClienteSinPorte(ByVal idcliente As Long)
        Dim cad As String = "DELETE FROM t_clientes_sin_porte where id_cliente=" & idcliente
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub AgregarClienteSinOferta(ByVal idCliente As Long, ByVal idAcuerdo As Long)
        'asociamos cliente
        Dim cad As String = "insert into t_clientes_sin_oferta (id_oferta,id_cliente) values(" & idAcuerdo & "," & idCliente & ")"
        Dim mda As New SqlCommand(cad, mcon)
        mcon.Open()
        mda.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub AgregarTarifa(ByVal lista As clsProductosTarifa)

        Dim cad As String = ""
        Dim m As New SqlCommand
        Dim tran As SqlTransaction
        Dim p As New clsProductoTarifa
        Dim cant As String = ""

        m.Connection = mcon
        Dim id As Integer


        ' id = getMaxId("id_producto", "t_precios_grupo")
        'iniciamos una trnasaccion para no elimnar registros y que no se grabrn los otros
        mcon.Open()
        tran = mcon.BeginTransaction(IsolationLevel.ReadCommitted)
        m.Transaction = tran
        Try
            m.CommandText = ("delete from t_precios_grupo")
            m.ExecuteNonQuery()

            For Each p In lista
                'como no puedo llamar a getmaxid desde unatransaccion
                'lo llamo antes de iniciar la transaccion y luego voy incrementandolo

                id = id + 1
                cant = Replace(p.precio_base, ",", ".")
                cad = "insert into t_precios_grupo values(" & p.id_modelo & "," & p.id_modo & "," & p.id_tratamiento & _
                "," & p.id_graduacion & "," & p.id_color & "," & IIf(p.diametro = "*", -1, p.diametro) & "," & cant & ")"
                m.CommandText = cad
                m.ExecuteNonQuery()
            Next

            tran.Commit()
        Catch
            tran.Rollback()
        End Try
        mcon.Close()

    End Sub
    Public Sub AgregarTarifaTemporal(ByVal lista As clsProductosTarifa)

        Dim cad As String = ""
        Dim m As New SqlCommand
        Dim tran As SqlTransaction
        Dim p As New clsProductoTarifa
        Dim cant As String

        m.Connection = mcon
        Dim id As Integer


        ' id = getMaxId("id_producto", "tmp_precios_grupo")
        'iniciamos una trnasaccion para no elimnar registros y que no se grabrn los otros
        mcon.Open()
        tran = mcon.BeginTransaction(IsolationLevel.ReadCommitted)
        m.Transaction = tran
        Try
            m.CommandText = ("delete from tmp_precios_grupo")
            m.ExecuteNonQuery()

            For Each p In lista
                'como no puedo llamar a getmaxid desde unatransaccion
                'lo llamo antes de iniciar la transaccion y luego voy incrementandolo

                id = id + 1
                cant = Replace(p.precio_base, ",", ".")
                cad = "insert into tmp_precios_grupo values(" & p.id_modelo & "," & p.id_modo & "," & p.id_tratamiento & _
                "," & p.id_graduacion & "," & p.id_color & "," & IIf(p.diametro = "*", -1, p.diametro) & "," & cant & ")"
                m.CommandText = cad
                m.ExecuteNonQuery()
            Next

            tran.Commit()
        Catch
            tran.Rollback()
        End Try
        mcon.Close()

    End Sub
    Public Function CargaOfertasPendientes() As DataTable
        Dim cad As String = "select * from t_ofertas where fec_fin>=" & FechaAcadena(Now.Date)
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetTarifa(ByVal excel As Boolean) As DataTable
        Dim ps As New clsProductosTarifa
        Dim tb As New DataTable
        Dim cad As String = "select t_precios_grupo.*,t_gamas_coloracion.denominacion as color,t_tratamientos.nombre as tratamiento," & _
        "t_modos.descripcion as modo,grupo as  modelo,t_graduaciones.graduacion from ((((t_precios_grupo inner join t_gamas_coloracion on " & _
        "t_gamas_coloracion.id_gama=t_precios_grupo.id_color)inner join t_tratamientos on t_tratamientos.id_tratamiento=" & _
        "t_precios_grupo.id_tratamiento) inner join t_modos on t_modos.id_modo=t_precios_grupo.id_modo) inner join " & _
        "t_grupos_modelos on t_grupos_modelos.id_grupo=t_precios_grupo.id_grupo) left join t_graduaciones on t_graduaciones.id_graduacion=" & _
        "t_precios_grupo.id_graduacion order by grupo,t_precios_grupo.id_modo,t_precios_grupo.id_tratamiento,t_precios_grupo.id_color,t_precios_grupo.id_grupo"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetTarifaTemporal(ByVal excel As Boolean) As DataTable
        Dim ps As New clsProductosTarifa
        Dim tb As New DataTable
        Dim cad As String = "select tmp_precios_grupo.*,t_gamas_coloracion.denominacion as color,t_tratamientos.nombre as tratamiento," & _
        "t_modos.descripcion as modo,grupo as  modelo,t_graduaciones.graduacion from ((((tmp_precios_grupo inner join t_gamas_coloracion on " & _
        "t_gamas_coloracion.id_gama=tmp_precios_grupo.id_color)inner join t_tratamientos on t_tratamientos.id_tratamiento=" & _
        "tmp_precios_grupo.id_tratamiento) inner join t_modos on t_modos.id_modo=tmp_precios_grupo.id_modo) inner join " & _
        "t_grupos_modelos on t_grupos_modelos.id_grupo=tmp_precios_grupo.id_grupo) left join t_graduaciones on t_graduaciones.id_graduacion=" & _
        "tmp_precios_grupo.id_graduacion order by grupo,tmp_precios_grupo.id_modo,tmp_precios_grupo.id_tratamiento,tmp_precios_grupo.id_color"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetTarifa() As clsProductosTarifa
        Dim ps As New clsProductosTarifa
        Dim p As clsProductoTarifa
        Dim tb As New DataTable
        Dim cad As String = "select dbo.codigosurcolor(t_precios_grupo.id_grupo,t_precios_grupo.id_modo,t_precios_grupo.id_tratamiento,t_precios_grupo.id_color) as codigoSurcolor,t_precios_grupo.*,t_gamas_coloracion.denominacion as color,t_tratamientos.nombre as tratamiento," & _
        "t_modos.descripcion as modo,grupo as  modelo,t_graduaciones.graduacion from ((((t_precios_grupo inner join t_gamas_coloracion on " & _
        "t_gamas_coloracion.id_gama=t_precios_grupo.id_color)inner join t_tratamientos on t_tratamientos.id_tratamiento=" & _
        "t_precios_grupo.id_tratamiento) inner join t_modos on t_modos.id_modo=t_precios_grupo.id_modo) inner join " & _
        "t_grupos_modelos on t_grupos_modelos.id_grupo=t_precios_grupo.id_grupo) left join t_graduaciones on t_graduaciones.id_graduacion=" & _
        "t_precios_grupo.id_graduacion order by grupo,t_precios_grupo.id_modo,t_precios_grupo.id_tratamiento,t_precios_grupo.diametro,t_precios_grupo.id_color"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()

        For i As Integer = 0 To tb.Rows.Count - 1
            p = New clsProductoTarifa
            p.Codigo = tb.Rows(i)("codigosurcolor")
            p.id_color = tb.Rows(i)("id_color")
            p.descripcion = tb.Rows(i)("MODELO")
            p.diametro = IIf(tb.Rows(i)("diametro") = -1, "*", tb.Rows(i)("diametro"))
            p.id_graduacion = tb.Rows(i)("id_graduacion")
            p.id_modelo = tb.Rows(i)("id_grupo")
            p.id_modo = tb.Rows(i)("id_modo")
            p.precio_base = tb.Rows(i)("precio")
            p.id_tipo = tb.Rows(i)("id_modo")
            p.id_tratamiento = tb.Rows(i)("id_tratamiento")
            p.color = tb.Rows(i)("color")
            p.graduacion = IIf(IsDBNull(tb.Rows(i)("graduacion")), "*", tb.Rows(i)("graduacion"))
            p.modelo = tb.Rows(i)("modelo")
            p.modo = tb.Rows(i)("modo")
            p.tratamiento = tb.Rows(i)("tratamiento")

            ps.add(p)
            p = Nothing
        Next

        Return ps
    End Function
    Public Sub GrabaPromocion(ByRef p As clsPromocion)
        Dim fallo As String = ""
        Dim cad As String = ""
        Try


            With p
                If .id = 0 Then 'promocion nueva, a grabar
                    .id = getMaxId("id_promocion", "t_promociones") + 1
                    cad = "INSERT INTO t_promociones (id_promocion,nombre,lentes,parejas,inicio,fin,combinar,misma_graduacion,tratamientos,gamas_color,id_grupo) VALUES (" & _
                    .id & "," & strsql(.Nombre) & "," & NumSql(.Lentes) & "," & NumSql(.Parejas) & "," & .Inicio & "," & .Fin & "," & IIf(.Combinar = False, 0, 1) & _
                    "," & IIf(.MismaGraduacion = False, 0, 1) & "," & strsql(.Tratamientos) & "," & strsql(.Colores) & "," & p.GrupoOptico & ")"
                Else
                    cad = "UPdate t_promociones set nombre=" & strsql(.Nombre) & ",lentes=" & NumSql(.Lentes) & ",parejas=" & NumSql(.Parejas) & ",inicio=" & .Inicio & _
                    ",fin=" & .Fin & ",combinar=" & IIf(.Combinar = False, 0, 1) & ",misma_graduacion=" & IIf(.MismaGraduacion = False, 0, 1) & ",tratamientos=" & strsql(.Tratamientos) & ",gamas_color=" & strsql(.Colores) & ",id_grupo=" & p.GrupoOptico & " where id_promocion=" & p.id
                End If
            End With
            Dim cmd As New SqlCommand(cad, mcon)
            mcon.Open()
            fallo = " Grabando datos generales de promocion"
            cmd.ExecuteNonQuery()
            'borramos tanto los tratamientos como los modelos y colores y los volvemos a insertar
            cmd.CommandText = "DELETE FROM t_promo_tratamientos where id_promocion=" & p.id & vbNewLine & " DELETE from t_promo_PVP_Fabricacion where id_promocion=" & p.id & vbNewLine & _
            "DELETE FROM t_promo_gamas_color where id_promocion=" & p.id & _
            "DELETE FROM t_promo_PVP_Stock where id_promocion=" & p.id & _
            "DELETE FROM t_clientes_sin_promocion where id_promocion=" & p.id
            fallo = " Eliminando valores antiguos"
            cmd.ExecuteNonQuery()
            'insertamos
            For Each pre As Precio In p.Modelos
                fallo = " Grabando modelos"
                cmd.CommandText = "INSERT INTO t_promo_PVP_fabricacion (id_promocion,id_grupo,precio) VALUES (" & p.id & "," & pre.id & "," & NumSql(pre.precio) & ")"
                cmd.ExecuteNonQuery()
            Next
            For Each pre As Precio In p.PVPTratamiento
                fallo = " Grabando precios detratamientos"
                cmd.CommandText = "INSERT INTO t_promo_tratamientos (id_promocion,id_tratamiento,precio) VALUES (" & p.id & "," & pre.id & "," & NumSql(pre.precio) & ")"
                cmd.ExecuteNonQuery()
            Next
            For Each pre As PrecioGamaColor In p.PVPColores
                fallo = "grabando precios de colores"
                cmd.CommandText = "INSERT INTO t_promo_gamas_color (id_promocion,id_gama,precio_HI,precio_LI) VALUES (" & p.id & "," & pre.id & "," & NumSql(pre.PrecioHI) & "," & NumSql(pre.PrecioLI) & ")"
                cmd.ExecuteNonQuery()
            Next
            For Each stock As clsPVPLenteStock In p.ModelosStock
                fallo = "grabando precios stock"
                cmd.CommandText = "INSERT INTO t_promo_PVP_stock (id_promocion,id_grupo,id_tratamiento,diametro,cilindro,precio) VALUES (" & p.id & "," & stock.grupo & "," & stock.Tratamiento & "," & stock.Diametro & "," & NumSql(stock.Cilindro) & "," & NumSql(stock.Precio) & ")"
                cmd.ExecuteNonQuery()
            Next

            For Each i As Integer In p.ClientesSinPromo
                fallo = "grabando Clientes sin promo"
                cmd.CommandText = "INSERT INTO t_clientes_sin_promocion (id_promocion,id_cliente) VALUES (" & p.id & "," & i & ")"
                cmd.ExecuteNonQuery()
            Next
            mcon.Close()
            MsgBox("Grabacion de datos correcta")
        Catch ex As Exception
            MsgBox("Error " & fallo)
            If mcon.State = ConnectionState.Open Then
                mcon.Close()
            End If
        End Try
        'MsgBox(p.id)
    End Sub
    Public Function GetPromociones(Optional ByVal caducadas As Boolean = False) As DataTable
        Dim Filtro As String = ""
        If caducadas = False Then
            Dim hoy As String = FechaAcadena(Now.Date)
            Filtro = " where fin>=" & hoy
        End If
        Dim cad As String = "select * from t_promociones " & Filtro & " order by fin"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetPVPStock() As DataTable
        Dim cad As String = "select t_pvp_lentes_stock.*,t_tratamientos.nombre as tratamiento,t_pvp_lentes_stock.cilindro,(select grupo from t_grupos_modelos where id_grupo=t_pvp_lentes_stock.id_grupo) as  modelo from t_pvp_lentes_stock inner join t_tratamientos on t_tratamientos.id_tratamiento=t_pvp_lentes_stock.id_tratamiento " & _
       "order by modelo,t_pvp_lentes_stock.id_tratamiento,t_pvp_lentes_stock.diametro,t_pvp_lentes_stock.cilindro"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetPVPByTratamiento(ByVal Tratamiento As Integer) As Decimal
        Dim Cad As String = "select isnull(precio,0) from t_tratamientos where id_Tratamiento=" & Tratamiento
        Dim precio As Decimal = 0
        Dim abierta As Boolean = False 'guardamos si la conexion viene abierta o no
        If mcon.State = ConnectionState.Open Then
            abierta = True
        Else
            mcon.Open()
        End If
        Dim cmd As New SqlCommand(Cad, mcon)
        precio = cmd.ExecuteScalar
        If abierta = False Then
            mcon.Close()
        End If
        Return precio
    End Function
    Public Function GetPVPFabricaByGrupo(ByVal Grupo As Integer) As Decimal
        Dim Cad As String = "select isnull(precio,0) from t_PVP_lentes_fabricacion where id_grupo=" & Grupo
        Dim precio As Decimal = 0
        Dim abierta As Boolean = False 'guardamos si la conexion viene abierta o no
        If mcon.State = ConnectionState.Open Then
            abierta = True
        Else
            mcon.Open()
        End If
        Dim cmd As New SqlCommand(Cad, mcon)
        precio = cmd.ExecuteScalar
        If abierta = False Then
            mcon.Close()
        End If
        Return precio
    End Function
    Public Function GetPVPStockByGrupo(ByVal Grupo As Integer, ByVal Tratamiento As Integer) As Decimal
        Dim Cad As String = "select isnull(precio,0) from t_PVP_lentes_stock where id_grupo=" & Grupo & " and id_tratamiento=" & Tratamiento
        Dim precio As Decimal = 0
        Dim abierta As Boolean = False 'guardamos si la conexion viene abierta o no
        If mcon.State = ConnectionState.Open Then
            abierta = True
        Else
            mcon.Open()
        End If
        Dim cmd As New SqlCommand(Cad, mcon)
        precio = cmd.ExecuteScalar
        If abierta = False Then
            mcon.Close()
        End If
        Return precio
    End Function
    Public Function GetPVPStockExcel() As DataTable
        Dim cad As String = "select (select grupo from t_grupos_modelos where id_grupo=t_pvp_lentes_stock.id_grupo) as  modelo,t_tratamientos.nombre as tratamiento,diametro,cilindro,t_pvp_lentes_stock.precio from t_pvp_lentes_stock inner join t_tratamientos on t_tratamientos.id_tratamiento=t_pvp_lentes_stock.id_tratamiento " & _
       "order by modelo,t_pvp_lentes_stock.id_tratamiento,t_pvp_lentes_stock.diametro,cilindro"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetPVPFabricacionExcel() As DataTable
        Dim cad As String = "select (select grupo from t_grupos_modelos where id_grupo=t_pvp_lentes_fabricacion.id_grupo) as grupo,t_tratamientos.nombre as tratamiento,(t_pvp_lentes_fabricacion.precio+t_tratamientos.precio) as PVP from t_tratamientos,t_pvp_lentes_fabricacion where id_tratamiento in (0,1,3) and t_pvp_lentes_fabricacion.precio<>0 order by grupo,id_tratamiento "
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Sub GrabaPrecioTratamiento(ByVal id As Integer, ByVal precio As Decimal)
        Dim cad As String = "UPDATE t_tratamientos set precio=" & NumSql(precio) & " where id_tratamiento=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetTarifatemporal() As clsProductosTarifa
        Dim ps As New clsProductosTarifa
        Dim p As clsProductoTarifa
        Dim tb As New DataTable
        Dim cad As String = "select dbo.codigosurcolor(tmp_precios_grupo.id_grupo,tmp_precios_grupo.id_modo,tmp_precios_grupo.id_tratamiento,tmp_precios_grupo.id_color) as codigoSurcolor,tmp_precios_grupo.*,t_gamas_coloracion.denominacion as color,t_tratamientos.nombre as tratamiento," & _
       "t_modos.descripcion as modo,grupo as  modelo,t_graduaciones.graduacion from ((((tmp_precios_grupo inner join t_gamas_coloracion on " & _
       "t_gamas_coloracion.id_gama=tmp_precios_grupo.id_color)inner join t_tratamientos on t_tratamientos.id_tratamiento=" & _
       "tmp_precios_grupo.id_tratamiento) inner join t_modos on t_modos.id_modo=tmp_precios_grupo.id_modo) inner join " & _
       "t_grupos_modelos on t_grupos_modelos.id_grupo=tmp_precios_grupo.id_grupo) left join t_graduaciones on t_graduaciones.id_graduacion=" & _
       "tmp_precios_grupo.id_graduacion order by grupo,tmp_precios_grupo.id_modo,tmp_precios_grupo.id_tratamiento,tmp_precios_grupo.diametro,tmp_precios_grupo.id_color"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()

        For i As Integer = 0 To tb.Rows.Count - 1
            p = New clsProductoTarifa
            p.Codigo = tb.Rows(i)("codigosurcolor")
            p.id_color = tb.Rows(i)("id_color")
            p.descripcion = tb.Rows(i)("MODELO")
            p.diametro = IIf(tb.Rows(i)("diametro") = -1, "*", tb.Rows(i)("diametro"))
            p.id_graduacion = tb.Rows(i)("id_graduacion")
            p.id_modelo = tb.Rows(i)("id_grupo")
            p.id_modo = tb.Rows(i)("id_modo")
            p.precio_base = tb.Rows(i)("precio")
            p.id_tipo = tb.Rows(i)("id_modo")
            p.id_tratamiento = tb.Rows(i)("id_tratamiento")
            p.color = tb.Rows(i)("color")
            p.graduacion = IIf(IsDBNull(tb.Rows(i)("graduacion")), "*", tb.Rows(i)("graduacion"))
            p.modelo = tb.Rows(i)("modelo")
            p.modo = tb.Rows(i)("modo")
            p.tratamiento = tb.Rows(i)("tratamiento")

            ps.add(p)
            p = Nothing
        Next

        Return ps
    End Function

    Public Function GetOferta() As clsProductosTarifa
        Dim ps As New clsProductosTarifa
        Dim p As clsProductoTarifa
        Dim tb As New DataTable
        Dim cad As String = "select t_precios_grupo.*,t_gamas_coloracion.denominacion as color,t_tratamientos.nombre as tratamiento," & _
        "t_modos.descripcion as modo,t_modelos.nombre as  modelo,t_graduaciones.graduacion,t_modelos.orden from ((((t_precios_grupo inner join t_gamas_coloracion on " & _
        "t_gamas_coloracion.id_gama=t_precios_grupo.id_color)inner join t_tratamientos on t_tratamientos.id_tratamiento=" & _
        "t_precios_grupo.id_tratamiento) inner join t_modos on t_modos.id_modo=t_precios_grupo.id_modo) inner join " & _
        "t_modelos on t_modelos.id_lente=t_precios_grupo.id_modelo) left join t_graduaciones on t_graduaciones.id_graduacion=" & _
        "t_precios_grupo.id_graduacion order by t_modelos.orden,t_precios_grupo.id_modo,t_precios_grupo.id_tratamiento,t_precios_grupo.id_color"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()

        For i As Integer = 0 To tb.Rows.Count - 1
            p = New clsProductoTarifa
            p.id_color = tb.Rows(i)("id_color")
            p.descripcion = tb.Rows(i)("descripcion")
            p.diametro = IIf(tb.Rows(i)("diametro") = -1, "*", tb.Rows(i)("diametro"))
            p.id_graduacion = tb.Rows(i)("id_graduacion")
            p.id_modelo = tb.Rows(i)("id_modelo")
            p.id_modo = tb.Rows(i)("id_modo")
            p.precio_base = tb.Rows(i)("precio_base")
            p.id_tipo = tb.Rows(i)("id_tipo")
            p.id_tratamiento = tb.Rows(i)("id_tratamiento")
            p.color = tb.Rows(i)("color")
            p.graduacion = IIf(IsDBNull(tb.Rows(i)("graduacion")), "*", tb.Rows(i)("graduacion"))
            p.modelo = tb.Rows(i)("modelo")
            p.modo = tb.Rows(i)("modo")
            p.tratamiento = tb.Rows(i)("tratamiento")

            ps.add(p)
            p = Nothing
        Next

        Return ps
    End Function
    Public Function GetFormaPrecal(ByVal id As Integer) As DataTable
        Dim cad As String = "select forma_r,forma_l from t_formas_precal where id_forma=" & id
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb

    End Function

    Public Function GetTarifaFabricacion() As DataTable
        Dim cad As String = "select *,isnull((select precio from t_pvp_lentes_fabricacion where id_grupo=t_grupos_modelos.id_grupo),0) as PVP from t_grupos_modelos order by grupo"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function

    Public Function GetModos() As DataTable
        Dim cad As String = ""
        cad = "select * from t_modos"
        Dim mda As New SqlDataAdapter
        Dim tb As New DataTable
        mda.SelectCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function getColoresByGama(ByVal Gama As Integer) As DataTable
        Dim cad As String = ""
        cad = "select * from t_coloraciones where id_gama_precio=" & Gama
        Dim mda As New SqlDataAdapter
        Dim tb As New DataTable
        mda.SelectCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function getColoracionTb() As DataTable
        Dim cad As String = ""
        cad = "select * from t_gamas_coloracion order by id_gama"
        Dim mda As New SqlDataAdapter
        Dim tb As New DataTable
        mda.SelectCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetColorAlbaran(ByVal idcolor As Integer) As String
        Dim cad As String = "select * from t_coloraciones where id_coloracion=" & idcolor
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        If mcon.State = ConnectionState.Open Then mcon.Close()
        mcon.Open()
        Dim tb As New DataTable
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("gama") & " " & IIf(IsDBNull(tb.Rows(0)("color")), "", tb.Rows(0)("color"))
    End Function

    Public Function GetColorById(ByVal idColor As Integer) As String
        Dim cad As String = "select * from t_gamas_coloracion where id_gama=" & idColor
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        If mcon.State = ConnectionState.Open Then mcon.Close()
        mcon.Open()
        Dim tb As New DataTable
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("Denominacion")

    End Function

    Public Function CantidadesMesByCliente(ByVal idAcuerdo As Integer, ByVal idCli As Integer, ByVal Fecha As Date) As Integer
        Dim desde As Integer = Fecha.Year & Format(Fecha.Month, "00") & "01"
        Dim Hasta As Integer = Fecha.Year & Format(Fecha.Month, "00") & "31"
        '**********************************
        'aqui vamos a hacer una clausula para ver la suma de albaranes de los clientes asociados
        Dim Cliente As String = " (id_cliente= " & idCli & " or id_cliente in (select id_cliente_asociado from t_clientes_asociados where id_cliente=" & idCli & " or id_cliente_asociado=" & idCli & "))"
        '********************
        Dim cad As String = "select top 1 isnull(cantidad,0) from t_acuerdo_cantidades where id_acuerdo=" & idAcuerdo & _
        " and fabricacion<=(select isnull(sum(total),0) from t_lineas_albaran where id_modo=3 and id_albaran in " & _
        "(select id_albaran from t_albaranes where " & Cliente & " and fecha>=" & desde & " and fecha<=" & Hasta & ")) ORDER BY cantidad desc"
        Dim cantidad As Integer = 0
        Dim cmd As New SqlCommand(cad, mcon)
        Dim conAbierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            conAbierta = True
        Else
            mcon.Open()
        End If
        cantidad = cmd.ExecuteScalar
        If conAbierta = False Then
            mcon.Close()
        End If
        Return cantidad
    End Function
    Public Function ComparaCantidadesBycliente(ByVal idacuerdo As Integer, ByVal idCliente As Integer, ByRef Cantidad As Integer, ByVal fecha As Date) As Integer
        'aqui comparamos si la cantidad es igual o menor o si es mayor, si es mayor debemos updatear la cantidad, si no existe debemos insertarla
        Dim cad As String = "select * from t_cantidades_cliente where id_acuerdo=" & idacuerdo & " and id_cliente=" & idCliente & " and mes=" & fecha.Month & " and año=" & fecha.Year
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        Dim conAbierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            conAbierta = True
        End If
        mda.Fill(tb)
        If tb.Rows.Count = 0 Then ' no existe, grabamos la cantidad que hay que aplicar
            If conAbierta = False Then
                mcon.Open()
            End If
            cad = "INSERT INTO t_cantidades_cliente" & _
            " Select " & idacuerdo & ",id_Cliente,1 ," & Cantidad & "," & fecha.Month & "," & fecha.Year & " from t_clientes where id_cliente=" & idCliente & _
            " OR id_cliente in ( select id_cliente_asociado from t_clientes_asociados where id_cliente=" & idCliente & " or id_cliente_asociado=" & idCliente & ")"
            mda.InsertCommand = New SqlCommand(cad, mcon)
            mda.InsertCommand.ExecuteNonQuery()
            If conAbierta = False Then
                mcon.Close()
            End If

        Else 'existe, si la cantidad que vamos a aplicar es mayor la updateamos
            If Cantidad > tb.Rows(0)("cantidad") Then
                If conAbierta = False Then
                    mcon.Open()
                End If
                cad = "Update t_cantidades_cliente set actualizar=1,cantidad=" & Cantidad & " where id_acuerdo=" & idacuerdo & " and (id_cliente=" & idCliente & _
                " or id_cliente in ( select id_cliente_asociado from t_clientes_asociados where id_cliente=" & idCliente & " or id_cliente_asociado=" & idCliente & "))" & _
             " and mes=" & fecha.Month & " and año=" & fecha.Year
                mda.UpdateCommand = New SqlCommand(cad, mcon)
                mda.UpdateCommand.ExecuteNonQuery()
                If conAbierta = False Then
                    mcon.Close()
                End If
            ElseIf Cantidad < tb.Rows(0)("cantidad") Then
                Cantidad = tb.Rows(0)("cantidad") ' si es menor cambiamos la cantidad por la que tenemos grabada, ya que en su dia llego a esa cantidad
            End If
        End If
        Return Cantidad
    End Function

    Public Sub ActualizaPedidoEnAlbaran(ByVal lineas As ArrayList, ByVal diferencia As Decimal)

        Dim cad As String = ""

        Dim idAlbaran As Integer = 0

        For Each l As clsAlbaranLinea In lineas
            idAlbaran = getIdAlbaranByIdPedido(l.idpedido)
            cad = "UPDATE t_lineas_albaran set precio=" & NumSql(l.precio) & ",total=" & NumSql(l.total) & " where id_pedido=" & l.idpedido & " and descripcion=" & strsql(l.descripcion) & vbNewLine
            'cmd.ExecuteNonQuery()
        Next
        cad = cad & "Update t_bases_albaran set base=base+" & NumSql(diferencia) & " where id_tipo_base=1 and id_albaran=" & idAlbaran & vbNewLine
        cad = cad & " Update t_albaranes set total=total+" & NumSql(diferencia) & " where id_albaran=" & idAlbaran
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        'ahora update
    End Sub
    Public Function GetPedidosAcuerdoCantidades(ByVal cli As Integer, ByVal mes As Integer, ByVal año As Integer) As DataTable
        Dim cad As String = "select id_pedido,count(distinct(id_albaran)) as cuenta from t_lineas_albaran where id_albaran in(select id_albaran from t_albaranes where id_cliente=" & cli & " and fecha>=" & año & Format(mes, "00") & "01 and fecha<=" & año & Format(mes, "00") & "31) and id_modo=3 group by id_pedido"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetCantidadesPendientesByacuerdo(ByVal id As Integer) As DataTable
        Return DameTb("select t_clientes.id_cliente,codigo,nombre_comercial,cantidad,mes,año from t_clientes INNER JOIN t_cantidades_cliente ON t_clientes.id_cliente=t_cantidades_cliente.id_cliente")
    End Function
    Private Function DameTb(ByVal cad As String) As DataTable

        'ESTA FUNCION DEVUELVE DATATABLE CON LA CADENA QUE RECIBE
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function

    Public Function GetDiametrosFabricacionByModelo(ByVal idModelo As Integer) As DataTable
        Dim cad As String = "select distinct(diametro) from t_rangos where id_modelo=" & idModelo
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter
        mda.SelectCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return (tb)
    End Function


    Public Sub AñadeLente(ByVal modelo As Integer, ByVal modo As Integer, ByVal tratamiento As Integer, _
        ByVal color As Integer, ByVal graduacion As Integer, ByVal diametro As Integer, ByVal precio As String, _
        ByVal descripcion As String, ByVal iva As String, ByVal tipo_producto As String)


        'menisco = 0 el torico = 1

        Dim cad As String

        Dim id As Long = getMaxId("id_producto", "t_precios_grupo") + 1
        cad = "insert into t_precios_grupo (id_producto,id_modelo," & _
        "id_modo,id_tratamiento,id_graduacion,id_color,diametro,precio_base) values ( " & id & "," & _
        modelo & "," & modo & "," & tratamiento & "," & _
        graduacion & "," & color & "," & diametro & "," & Replace(precio, ",", ".") & ")"

        Dim q As New SqlCommand(cad, mcon)
        mcon.Open()
        q.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetGamaColorByID(ByVal idColor As Integer) As Integer
        Dim cad As String = "select  *,(gama + ' ' + isnull(color,'')) as desig from t_coloraciones where id_coloracion=" & idColor
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("id_gama_precio")
    End Function
    Public Function GetGamaNombreByID(ByVal idgama As Integer) As String
        Dim cad As String = " select denominacion from t_gamas_coloracion where id_gama=" & idgama
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("denominacion")
    End Function
    Public Function EsPotenciaFueraDeRango(ByVal cilindro As String, ByVal esfera As String, ByVal id_modelo As Integer, ByVal diametro As Integer) As Boolean
        'si la lente es de stock no hace falta seguir pero si no es de stock busfo en fabricacion
        Dim cad As String = ""
        cad = "select * from t_rangos where id_modelo=" & id_modelo & " and diametro=" & diametro & _
        " and esfera = " & Replace(esfera, ",", ".") & " and cilindro=" & Replace(cilindro, ",", ".")
        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            Return True
        End If
        Return False

    End Function
    Public Function getInventarioReposicionByModelos(ByVal idModelo As Integer) As DataTable
        Dim cad As String = ""
        cad = "select t_lentes_stock.*,t_modelos.nombre,t_tratamientos.nombre from (t_lentes_stock inner join" & _
        " t_modelos on t_lentes_stock.id_modelo=t_modelos.id_modelo) inner join t_tratamientos on " & _
        "t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento where t_lentes_stock.id_modelo=" & idModelo & _
        " and stock< stock_minimo"
        Dim tb As New DataTable
        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb

    End Function

    Public Function GetLentesStock() As DataTable
        Dim cad As String = "select * from t_modelos where tipologia = 1"
        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetModelosStock() As DataTable
        Dim cad As String = "select * from t_modelos where baja=0 and id_lente in (select id_modelo from t_lentes_stock) order by orden"
        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function ListarInventario(ByVal idmodelo As Integer, ByVal diametro As Integer, ByVal idtratamiento As Integer) As DataTable
        Dim tratamiento As String = ""
        Dim DiametroLente As String = ""
        If diametro = 0 Then
            DiametroLente = ""
            tratamiento = " "
        ElseIf diametro = -1 Then
            DiametroLente = ""
            tratamiento = " and t_lentes_stock.tratamiento=" & idtratamiento
        Else
            DiametroLente = " and t_lentes_stock.diametro = " & diametro
            tratamiento = " and t_lentes_stock.tratamiento=" & idtratamiento
        End If
        Dim cad As String = "SELECT t_lentes_stock.id_producto, t_Modelos.nombre as Modelo,t_lentes_stock.diametro, " & _
        "t_tratamientos.nombre as nTratamiento, t_lentes_stock.cilindro, t_lentes_stock.esfera, " & _
        "t_lentes_stock.stock  FROM (t_lentes_stock INNER JOIN t_Modelos t_Modelos ON " & _
        "t_lentes_stock.id_modelo=t_Modelos.id_Lente) INNER JOIN t_tratamientos ON " & _
        "t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento where t_lentes_stock.id_modelo=" & idmodelo & _
        DiametroLente & tratamiento & " ORDER BY t_lentes_stock.id_modelo, " & _
        "t_lentes_stock.diametro, t_lentes_stock.tratamiento, t_lentes_stock.CILINDRO,t_lentes_stock.ESFERA"

        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetInventarioSemiterminado(Optional ByVal Valorado As Boolean = True) As DataTable
        Dim cad As String = "select t_modelos.nombre as modelo,base,diametro,adicion,ojo,stock from t_modelos INNER JOIN t_semiterminados ON t_modelos.id_lente=t_semiterminados.id_modelo where stock<>0 order by t_semiterminados.id_modelo"
        If Valorado = True Then
            cad = "select t_modelos.nombre as modelo,base,diametro,adicion,ojo,stock,Stock * dbo.precioplaca(t_semiterminados.id_lente) as valor from t_modelos INNER JOIN t_semiterminados ON t_modelos.id_lente=t_semiterminados.id_modelo where stock<>0 order by t_semiterminados.id_modelo"
        End If
        Dim da As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        If Valorado = True Then
            da.SelectCommand.CommandTimeout = 300
        End If
        '   mcon.co
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetHoraTransporte(ByVal agencia As String) As String
        Dim cad As String = "select hora from t_transportistas where transportista=" & strsql(agencia)
        Dim hora As String = ""
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        hora = hora & cmd.ExecuteScalar
        mcon.Close()
        Return hora
    End Function
    Public Function GetInventarioAgrupado() As DataTable
        'Dim cad As String = "select  t_modelos.nombre,t_tratamientos.nombre as Trat,sum(stock) from " & _
        '"(t_lentes_stock inner join t_modelos on t_lentes_stock.id_modelo = t_modelos.id_lente)" & _
        '"inner join t_tratamientos on t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento " & _
        '"group by t_modelos.nombre,t_tratamientos.nombre order by t_modelos.nombre,t_tratamientos.nombre,cilindro,esfera"
        Dim cad As String = "select  t_modelos.nombre,t_tratamientos.nombre as Trat,diametro,sum(stock) as cantidad,sum(stock_minimo) as [Minimo Teorico],sum(stock_minimo)* dbo.preciocomprastock(t_lentes_stock.tratamiento,t_lentes_stock.id_modelo,diametro) As [Valor Teorico],sum(stock) * dbo.preciocomprastock(t_lentes_stock.tratamiento,t_lentes_stock.id_modelo,diametro) as Valoracion from " & _
               "(t_lentes_stock inner join t_modelos on t_lentes_stock.id_modelo = t_modelos.id_lente)" & _
               "inner join t_tratamientos on t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento " & _
               "group by t_modelos.nombre,t_tratamientos.nombre,t_lentes_stock.tratamiento,t_lentes_stock.id_modelo,diametro order by t_modelos.nombre,t_tratamientos.nombre"
        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Sub AsociaFormaApedido(ByVal origen As Integer, ByVal Destino As Integer)
        Dim cad As String = "DELETE FROM t_formas_pedido where id_pedido=" & Destino & vbNewLine & _
        "INSERT INTo t_formas_pedido select " & Destino & ",forma from t_formas_pedido where id_pedido=" & origen
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetSalidasLentesStock(ByVal Fecini As String, ByVal FecFin As String) As DataTable
        'Dim cad As String = "select  t_modelos.nombre,t_tratamientos.nombre as Trat,sum(stock) from " & _
        '"(t_lentes_stock inner join t_modelos on t_lentes_stock.id_modelo = t_modelos.id_lente)" & _
        '"inner join t_tratamientos on t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento " & _
        '"group by t_modelos.nombre,t_tratamientos.nombre order by t_modelos.nombre,t_tratamientos.nombre,cilindro,esfera"
        Dim cad As String = "select  t_modelos.nombre,t_tratamientos.nombre as Trat,diametro,sum(cantidad) as cantidad,sum(cantidad) * dbo.preciocomprastock(t_lentes_stock.tratamiento,t_lentes_stock.id_modelo,diametro) as Valoracion from " & _
               "(t_lentes_stock inner join t_modelos on t_lentes_stock.id_modelo = t_modelos.id_lente)" & _
               "inner join t_tratamientos on t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento INNER JOIN t_movimientos_almacen ON t_lentes_stock.id_producto=t_movimientos_almacen.id_producto where fecha>=" & FechaAcadena(Fecini) & " and fecha<=" & FechaAcadena(FecFin) & " and entrada=0" & _
               "group by t_modelos.nombre,t_tratamientos.nombre,t_lentes_stock.tratamiento,t_lentes_stock.id_modelo,diametro order by t_modelos.nombre,t_tratamientos.nombre"
        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function ListarNuevoPedidoReposicion(ByVal idmodelo As Integer, ByVal diametro As Integer, ByVal idtratamiento As Integer, ByVal Desde As Integer, ByVal Hasta As Integer) As DataTable
        Dim DiametroLente As String = ""
        Dim tratamiento As String = ""
        If diametro = 0 Then
            DiametroLente = ""
            tratamiento = ""
        ElseIf diametro = -1 Then 'se trata de detallado pero sin poner diamtero
            DiametroLente = ""
            tratamiento = " and t_lentes_stock.tratamiento=" & idtratamiento
        Else
            DiametroLente = " and t_lentes_stock.diametro = " & diametro
            tratamiento = " and t_lentes_stock.tratamiento=" & idtratamiento
        End If
        Dim PedidoEliminado As String = " and id_pedido in (select id_pedido from t_pedidos_prov where eliminado=0)"
        Dim cad As String
        If Desde <> 0 Then
            cad = "SELECT t_lentes_stock.id_producto, t_Modelos.nombre as Modelo,t_lentes_stock.diametro, " & _
       "t_tratamientos.nombre as Tratamiento, t_lentes_stock.cilindro, t_lentes_stock.esfera, " & _
       "stock,stock_minimo,stock_critico,isnull(case when (select  sum(cantidad-servido)   from t_lineas_pedidos_prov where id_producto" & _
       "=t_lentes_stock.id_producto " & PedidoEliminado & ")is null then 0 else (select sum(cantidad-servido) from t_lineas_pedidos_prov" & _
       " where id_producto=t_lentes_stock.id_producto " & PedidoEliminado & ") end ,0) as Pendiente ,(select count(*) from t_salida_stock where id_lente=t_lentes_stock.id_producto and salida>=" & ConvertDateToTimeStamp(cadenaAfecha(Desde)) & " and salida<=" & ConvertDateToTimeStamp(cadenaAfecha(Hasta)) & ") as pedir,0 as servido FROM (t_lentes_stock INNER JOIN t_Modelos t_Modelos ON " & _
       "t_lentes_stock.id_modelo=t_Modelos.id_Lente) INNER JOIN t_tratamientos ON " & _
       "t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento where t_lentes_stock.baja=0 and t_lentes_stock.id_modelo=" & idmodelo & _
       DiametroLente & tratamiento & _
       "  ORDER BY t_lentes_stock.id_modelo, " & _
       "t_lentes_stock.diametro, t_lentes_stock.tratamiento, t_lentes_stock.cilindro, t_lentes_stock.esfera "
        Else
            cad = "SELECT t_lentes_stock.id_producto, t_Modelos.nombre as Modelo,t_lentes_stock.diametro, " & _
        "t_tratamientos.nombre as Tratamiento, t_lentes_stock.cilindro, t_lentes_stock.esfera, " & _
        "stock,stock_minimo,stock_critico,isnull(case when (select  sum(cantidad-servido)   from t_lineas_pedidos_prov where id_producto" & _
        "=t_lentes_stock.id_producto " & PedidoEliminado & ")is null then 0 else (select sum(cantidad-servido) from t_lineas_pedidos_prov" & _
        " where id_producto=t_lentes_stock.id_producto " & PedidoEliminado & ") end,0) as Pendiente ,case when stock_minimo-stock<0 " & _
        " then 0 ELSE stock_minimo-stock end as pedir,0 as servido FROM (t_lentes_stock INNER JOIN t_Modelos t_Modelos ON " & _
        "t_lentes_stock.id_modelo=t_Modelos.id_Lente) INNER JOIN t_tratamientos ON " & _
        "t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento where t_lentes_stock.baja=0 and t_lentes_stock.id_modelo=" & idmodelo & _
        DiametroLente & tratamiento & _
        "  ORDER BY t_lentes_stock.id_modelo, " & _
        "t_lentes_stock.diametro, t_lentes_stock.tratamiento, t_lentes_stock.cilindro, t_lentes_stock.esfera"

        End If

        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        da.SelectCommand.CommandTimeout = 240

        Dim tb As New DataTable
        'mcon.Open()
        da.Fill(tb)
        'mcon.Close()
        Return tb
    End Function
    Public Function ListarPedidoReposicion(ByVal idmodelo As Integer, ByVal diametro As Integer, ByVal idtratamiento As Integer, ByVal Desde As Integer, ByVal Hasta As Integer) As DataTable
        Dim DiametroLente As String = ""
        Dim tratamiento As String = ""
        If diametro = 0 Then
            DiametroLente = ""
            tratamiento = ""
        ElseIf diametro = -1 Then 'se trata de detallado pero sin poner diamtero
            DiametroLente = ""
            tratamiento = " and t_lentes_stock.tratamiento=" & idtratamiento
        Else
            DiametroLente = " and t_lentes_stock.diametro = " & diametro
            tratamiento = " and t_lentes_stock.tratamiento=" & idtratamiento
        End If
        Dim PedidoEliminado As String = " and id_pedido in (select id_pedido from t_pedidos_prov where eliminado=0)"
        Dim cad As String
        If Desde <> 0 Then
            cad = "SELECT t_lentes_stock.id_producto, t_Modelos.nombre as Modelo,t_lentes_stock.diametro, " & _
       "t_tratamientos.nombre as Tratamiento, t_lentes_stock.cilindro, t_lentes_stock.esfera, " & _
       "stock,stock_minimo,stock_critico,isnull(case when (select  sum(cantidad-servido)   from t_lineas_pedidos_prov where id_producto" & _
       "=t_lentes_stock.id_producto " & PedidoEliminado & ")is null then 0 else (select sum(cantidad-servido) from t_lineas_pedidos_prov" & _
       " where id_producto=t_lentes_stock.id_producto " & PedidoEliminado & ") end ,0) as Pendiente ,(select count(*) from t_movimientos_almacen where entrada=0 and id_producto=t_lentes_stock.id_producto and fecha>=" & Desde & " and fecha<=" & Hasta & ") as pedir,0 as servido FROM (t_lentes_stock INNER JOIN t_Modelos t_Modelos ON " & _
       "t_lentes_stock.id_modelo=t_Modelos.id_Lente) INNER JOIN t_tratamientos ON " & _
       "t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento where t_lentes_stock.baja=0 and t_lentes_stock.id_modelo=" & idmodelo & _
       DiametroLente & tratamiento & _
       "  ORDER BY t_lentes_stock.id_modelo, " & _
       "t_lentes_stock.diametro, t_lentes_stock.tratamiento, t_lentes_stock.cilindro, t_lentes_stock.esfera "
        Else
            cad = "SELECT t_lentes_stock.id_producto, t_Modelos.nombre as Modelo,t_lentes_stock.diametro, " & _
        "t_tratamientos.nombre as Tratamiento, t_lentes_stock.cilindro, t_lentes_stock.esfera, " & _
        "stock,stock_minimo,stock_critico,isnull(case when (select  sum(cantidad-servido)   from t_lineas_pedidos_prov where id_producto" & _
        "=t_lentes_stock.id_producto " & PedidoEliminado & ")is null then 0 else (select sum(cantidad-servido) from t_lineas_pedidos_prov" & _
        " where id_producto=t_lentes_stock.id_producto " & PedidoEliminado & ") end,0) as Pendiente ,case when stock_minimo-stock<0 " & _
        " then 0 ELSE stock_minimo-stock end as pedir,0 as servido FROM (t_lentes_stock INNER JOIN t_Modelos t_Modelos ON " & _
        "t_lentes_stock.id_modelo=t_Modelos.id_Lente) INNER JOIN t_tratamientos ON " & _
        "t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento where t_lentes_stock.baja=0 and t_lentes_stock.id_modelo=" & idmodelo & _
        DiametroLente & tratamiento & _
        "  ORDER BY t_lentes_stock.id_modelo, " & _
        "t_lentes_stock.diametro, t_lentes_stock.tratamiento, t_lentes_stock.cilindro, t_lentes_stock.esfera"

        End If

        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        da.SelectCommand.CommandTimeout = 240

        Dim tb As New DataTable
        'mcon.Open()
        da.Fill(tb)
        'mcon.Close()
        Return tb
    End Function
    Public Function GetHojaBlancaByFactura(ByVal idfactura As Long) As DataTable
        Dim cad As String = "select id_albaran as Albaran, descripcion, precio, dto as descuento, total from t_hoja_blanca where id_factura=" & idfactura
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function ListarPedidoReposicionSemiterminado(ByVal IdProveedor As Integer, ByVal Modelos As String) As DataTable
        Dim lentes As String = ""
        Dim Proveedor As String = ""

        lentes = " id_modelo in " & Modelos
        If IdProveedor = 1 Then
            'en el caso de Signet cargamos signet y LTL
            Proveedor = " and t_semiterminados.id_lente in (select id_lente from t_cod_barras_semiterminado where id_proveedor=1 or id_proveedor=4) "
        ElseIf IdProveedor > 1 Then
            Proveedor = " and t_semiterminados.id_lente in (select id_lente from t_cod_barras_semiterminado where id_proveedor=" & IdProveedor & ") "
        End If
        Dim PedidoEliminado As String = " and id_pedido in (select id_pedido from Proveedores.dbo.t_pedidos_semiterminados where eliminado=0 and id_proveedor=" & IdProveedor & ")"
        Dim cad As String = "SELECT t_semiterminados.id_lente , t_Modelos.nombre as Modelo,diametro, " & _
        "base,ojo,adicion, stock,minimo,critico,isnull(case when (select  isnull(sum(cantidad-servido),0)   from  Proveedores.dbo.t_lineas_pedido_semiterminados where id_lente" & _
        "=t_semiterminados.id_lente " & PedidoEliminado & ")is null then 0 else (select isnull(sum(cantidad-servido),0) from  Proveedores.dbo.t_lineas_pedido_semiterminados" & _
        " where id_lente=t_semiterminados.id_lente and CANTIDAD-SERVIDO>0 " & PedidoEliminado & ") end ,0) as Pendiente ,case when minimo-(stock+case " & _
        " when(select isnull(sum(cantidad-servido),0) from  Proveedores.dbo.t_lineas_pedido_semiterminados where id_lente=t_semiterminados.id_lente " & PedidoEliminado & ")" & _
        " <0 then 0 else (select isnull(sum(cantidad-servido),0) from  Proveedores.dbo.t_lineas_pedido_semiterminados where id_lente=t_semiterminados.id_lente" & PedidoEliminado & ")" & _
        " end  ) >0 then minimo-(stock+case when(select isnull(sum(cantidad-servido),0) from  Proveedores.dbo.t_lineas_pedido_semiterminados where " & _
        " id_lente=t_semiterminados.id_lente" & PedidoEliminado & ") <0 then 0 else (select isnull(sum(cantidad-servido),0) from  Proveedores.dbo.t_lineas_pedido_semiterminados" & _
        " where id_lente=t_semiterminados.id_lente" & PedidoEliminado & ")end ) else 0 end as pedir,0 as servido FROM t_semiterminados INNER JOIN  t_Modelos ON " & _
        "t_semiterminados.id_modelo=t_Modelos.id_Lente  where " & lentes & _
        Proveedor & _
        "  ORDER BY  " & _
        "t_modelos.orden,t_semiterminados.diametro, t_semiterminados.base, t_semiterminados.adicion,ojo"

        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function PedidoSemiterminadoEntreFechas(ByVal IdProveedor As Integer, ByVal Modelos As String, ByVal Inicio As Integer, ByVal Fin As Integer) As DataTable
        Dim lentes As String = ""
        Dim Proveedor As String = ""

        lentes = " id_modelo in " & Modelos
        If IdProveedor = 1 Then
            'en el caso de Signet cargamos signet y LTL
            Proveedor = " and t_semiterminados.id_lente in (select id_lente from t_cod_barras_semiterminado where id_proveedor=1 or id_proveedor=4) "
        ElseIf IdProveedor > 1 Then
            Proveedor = " and t_semiterminados.id_lente in (select id_lente from t_cod_barras_semiterminado where id_proveedor=" & IdProveedor & ") "
        End If
        Dim PedidoEliminado As String = " and id_pedido in (select id_pedido from Proveedores.dbo.t_pedidos_semiterminados where eliminado=0 and id_proveedor=" & IdProveedor & ")"

        Dim cad As String = "SELECT t_semiterminados.id_lente , t_Modelos.nombre as Modelo,diametro, " & _
        "base,ojo,adicion, stock,minimo,critico,isnull(case when (select  isnull(sum(cantidad-servido),0)   from  Proveedores.dbo.t_lineas_pedido_semiterminados where id_lente" & _
        "=t_semiterminados.id_lente " & PedidoEliminado & ")is null then 0 else (select isnull(sum(cantidad-servido),0) from  Proveedores.dbo.t_lineas_pedido_semiterminados" & _
        " where id_lente=t_semiterminados.id_lente and CANTIDAD-SERVIDO>0 " & PedidoEliminado & ") end ,0) as Pendiente,(select count(*) from t_salidas_semiterminados where id_lente=t_semiterminados.id_lente and fecha>=" & Inicio & " and fecha<=" & Fin & _
        " and id_proveedor=" & IdProveedor & ") as Cantidad,0 as servido from t_semiterminados INNER JOIN t_modelos ON t_modelos.id_lente=t_semiterminados.id_modelo" & _
        " where " & lentes & Proveedor & "  ORDER BY  " & _
        "t_modelos.orden,t_semiterminados.diametro, t_semiterminados.base, t_semiterminados.adicion,ojo"

        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function


    Public Function ListarPedidoProv(ByVal idpedido As Integer) As DataTable

        Dim cad As String = "SELECT t_lentes_stock.id_producto, t_Modelos.nombre as Modelo,t_lentes_stock.diametro, " & _
        "t_tratamientos.nombre as Tratamiento, t_lentes_stock.cilindro, t_lentes_stock.esfera, " & _
        "sum(cantidad-servido) as Total  FROM ((t_lentes_stock INNER JOIN t_Modelos t_Modelos ON " & _
        "t_lentes_stock.id_modelo=t_Modelos.id_Lente) INNER JOIN t_tratamientos ON " & _
        "t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento) INNER JOIN t_lineas_pedidos_prov ON t_lineas_pedidos_prov.id_producto=t_lentes_stock.id_producto where" & _
        " id_pedido=" & idpedido & "group by t_lentes_stock.id_producto,t_modelos.nombre,t_tratamientos.nombre,cilindro,diametro,esfera" & _
        " ORDER BY  " & _
        "t_lentes_stock.diametro, t_tratamientos.nombre, t_lentes_stock.cilindro, t_lentes_stock.esfera"

        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetclienteBono(ByVal idbono As Integer) As clsCliente
        Dim cad As String = "select id_cliente from t_bonos where id_bono=" & idbono
        Dim cli As New clsCliente
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim idcli As Integer = 0
        idcli = cmd.ExecuteScalar
        cli = getClientebyId(idcli)
        mcon.Close()
        Return cli
    End Function
    Public Function BasesNegativasSemiterminados(ByVal idmodelo As Integer, ByVal base As Single, ByVal Diametro As Integer) As Single
        Dim cad As String = "select base_negativa from t_bases where id_modelo=" & idmodelo & " and base=" & Replace(base, ",", ".") & " and diametro=" & Diametro
        Dim cmd As New SqlCommand(cad, mcon)
        Dim baseNegativa As Single = 0
        mcon.Open()
        baseNegativa = cmd.ExecuteScalar
        mcon.Close()
        Return baseNegativa
    End Function
    Public Function GetTiposModelos() As DataTable
        Dim cad As String = "Select * from t_tipos_modelo order by id_tipo_modelo asc"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetClientesByTipoModelo(ByVal idtipo As Integer) As DataTable
        Dim cad As String = "select t_clientes.id_cliente,codigo,nombre_comercial from t_clientes INNER JOIN t_modelos_cliente ON t_modelos_cliente.id_cliente =t_clientes.id_cliente where id_tipo_modelo=" & idtipo
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Sub GrabaClienteByTipoModelo(ByVal idtipo As Integer, ByVal idCliente As Integer)
        Dim cad As String = "INSERT INTO t_modelos_cliente (id_tipo_modelo,id_cliente) VALUES (" & idtipo & "," & idCliente & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GrabaTipoModelo(ByVal idtipo As Integer, ByVal tipo As String) As Integer
        Dim cad As String = ""
        If idtipo = -1 Then
            idtipo = getMaxId("id_tipo_modelo", "t_tipos_modelo") + 1
            cad = "insert into t_tipos_modelo (id_tipo_modelo,nombre) VALUES (" & idtipo & "," & strsql(tipo) & ")"
        Else
            cad = "UPdate t_tipos_modelo SET NOMBRE=" & strsql(tipo) & " WHERE ID_TIPO_MODELO=" & idtipo

        End If
        Dim Cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Cmd.ExecuteNonQuery()
        'ahora eliminamos los clientes que estan 
        Cmd.CommandText = "DELETE FROM t_modelos_cliente where id_tipo_modelo=" & idtipo
        Cmd.ExecuteNonQuery()
        mcon.Close()
        Return idtipo
    End Function
    Public Function GetTiposModelosByCliente(ByVal idcliente As Integer) As DataTable
        Dim cad As String = "select * from t_tipos_modelo where id_tipo_modelo in (select id_tipo_modelo from t_modelos_cliente where id_cliente=" & idcliente & ")"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetLenteIOTbyPedido(ByVal ped As clsPedido) As DataTable
        Dim tb As New DataTable, TbUnion As New DataTable
        Dim Valor As Single = 0
        Dim cad As String = 0
        'sumamos el cilindro mas la esfera, si el valor es positivo le mandamos ese valor, si es negativo, le mandamos el de la esfera
        If ped.cilindro + ped.esfera >= 0 Then
            Valor = ped.cilindro + ped.esfera + ped.adicion
        Else
            If EsMonoFocal(ped.id_modelo) Then
                Valor = ped.esfera + ped.adicion
            Else
                Valor = ped.cilindro + ped.esfera

            End If
        End If
        Dim m As New clsModelo
        m = getClsModeloById(ped.id_modelo)
        If m.ModeloAlternativo = 0 Then 'no hay ninguna lente IOT
            Return tb
        Else
            If EsMonoFocal(ped.id_modelo) Or Valor < 0 Then
                cad = "select id_modelo,base,diametro from t_bases where id_modelo=" & m.ModeloAlternativo & " and diametro>=" & ped.diametro & " and " & Replace(Valor, ",", ".") & ">=desde  and " & Replace(Valor, ",", ".") & "<=hasta order by base,diametro"
            Else
                cad = "select id_modelo,base,diametro from t_bases where id_modelo=" & m.ModeloAlternativo & " and diametro>=" & ped.diametro & " and " & NumSql(Valor) & " >=desde_prog and " & NumSql(Valor) & "<=hasta_prog order by base,diametro"
            End If
        End If
        Dim bases As String = ""
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        bases = " and ("
        If tb.Rows.Count > 0 Then

            Dim i As Integer

            For i = 0 To tb.Rows.Count - 1
                bases = bases & " ( id_modelo=" & tb.Rows(i)("id_modelo") & " and  base=" & Replace(tb.Rows(i)("base"), ",", ".") & " and diametro=" & tb.Rows(i)("diametro") & ") or"
            Next
            'ahora le quitamos la ultima coma y le añadimos el cierre del parentesisç
            bases = bases.Substring(0, bases.Length - 2) & ")"
        End If
        tb.Clear()
        If bases.Length > 6 Then
            'ahora vamos a ver los semiterminados que hay que cumplan esa base, dependera de la tipologia (monofocales o bifocales)
            Dim Sentencia As String, sentenciaEspecial As String = ""
            Dim BaseEspecial As Single
            If ped.base_plana <> 0 Then
                BaseEspecial = ped.base_plana
            End If
            If ped.base_curva <> 0 Then
                BaseEspecial = ped.base_curva
            End If
            If EsMonoFocal(ped.id_modelo) Then
                'vemos si el pedido tiene una base especial, si lo tiene hay que ponerla primero
                If BaseEspecial <> 0 Then
                    sentenciaEspecial = "select id_lente,id_modelo,diametro,base,ojo,adicion,stock,minimo,critico,(select nombre from t_modelos where id_lente=t_semiterminados.id_modelo) as Modelo,(select codigo from t_modelos where id_lente=t_semiterminados.id_modelo) as Codigo from t_semiterminados where id_modelo=" & ped.id_modelo & " and diametro>=" & ped.diametro & " and base>=" & Int(BaseEspecial) & " and base<" & Int(BaseEspecial) + 1 & " order by base,diametro  "
                End If
                Sentencia = "select  id_lente,id_modelo,diametro,base,ojo,adicion,stock,minimo,critico,(select nombre from t_modelos where id_lente=t_semiterminados.id_modelo) as Modelo,(select codigo from t_modelos where id_lente=t_semiterminados.id_modelo) as Codigo from t_semiterminados where id_modelo>0 " & bases & " order by base,diametro "
            Else
                If BaseEspecial <> 0 Then
                    sentenciaEspecial = "select  id_lente,id_modelo,diametro,base,ojo,adicion,stock,minimo,critico,(select nombre from t_modelos where id_lente=t_semiterminados.id_modelo) as Modelo,(select codigo from t_modelos where id_lente=t_semiterminados.id_modelo) as Codigo from t_semiterminados where id_modelo=" & ped.id_modelo & " and diametro>=" & ped.diametro & " and base>=" & Int(BaseEspecial) & " and base<=" & Int(BaseEspecial) + 1 & " and ojo='" & ped.ojo & "' and adicion=" & Replace(ped.adicion, ",", ".") & "  order by base,diametro"
                End If
                'aqui se pasa del ojo y de la adicion
                Sentencia = "select  id_lente,id_modelo,diametro,base,ojo,adicion,stock,minimo,critico,(select nombre from t_modelos where id_lente=t_semiterminados.id_modelo) as Modelo,(select codigo from t_modelos where id_lente=t_semiterminados.id_modelo) as Codigo from t_semiterminados where id_modelo>0 " & bases & _
                 " order by base,diametro"
            End If

            Dim md As New SqlDataAdapter(Sentencia, mcon)

            mcon.Open()
            If sentenciaEspecial.Length <> 0 Then
                Dim mdEspecial As New SqlDataAdapter(sentenciaEspecial, mcon)

                mdEspecial.Fill(TbUnion)
                mcon.Close()
                Return TbUnion
            End If
            md.Fill(tb)

            mcon.Close()



        End If
        Return tb
    End Function

    Public Function GetlentesSemiterminadosbyPedido(ByVal ped As clsPedido) As DataTable
        Dim tb As New DataTable, TbUnion As New DataTable
        Dim Valor As Single = 0
        'sumamos el cilindro mas la esfera, si el valor es positivo le mandamos ese valor, si es negativo, le mandamos el de la esfera
        If ped.cilindro + ped.esfera >= 0 Then
            Valor = ped.cilindro + ped.esfera
        Else
            Valor = ped.esfera
        End If
        If EsProgresivo(ped.id_modelo) Then
            Valor = Valor + ped.adicion
        End If
        'ahora vamos a buscar las bases para ese modelo que cumplen las condiciones

        Dim cad As String = ""
        If ped.id_modelo = 28 Then 'CASO ESPECIAL DEL SUPERVIEW, QUE PUEDE FABRICARSE CON MAGICVIEW
            cad = "select id_modelo,base,diametro from t_bases where (id_modelo=28 or id_modelo=29) and diametro>=" & ped.diametro & " and " & Replace(Valor, ",", ".") & ">=desde  and " & Replace(Valor, ",", ".") & "<=hasta"
        ElseIf ped.id_modelo = 8 And ped.esfera <= 2 Then ' si es 1,56 Asferico con la esfera negativa
            cad = "select id_modelo,base,diametro from t_bases where (id_modelo=8 or (id_modelo=9 and (base=3 or base=4 or base=5))) and diametro>=" & ped.diametro & " and " & Replace(Valor, ",", ".") & ">=desde  and " & Replace(Valor, ",", ".") & "<=hasta order by base,diametro"
        Else
            cad = "select id_modelo,base,diametro from t_bases where id_modelo=" & ped.id_modelo & " and diametro>=" & ped.diametro & " and " & Replace(Valor, ",", ".") & ">=desde  and " & Replace(Valor, ",", ".") & "<=hasta order by base,diametro"
        End If
        Dim bases As String = ""
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        bases = " and ("
        If tb.Rows.Count > 0 Then

            Dim i As Integer = 0

            For i = 0 To tb.Rows.Count - 1
                bases = bases & " ( id_modelo=" & tb.Rows(i)("id_modelo") & " and  base=" & Replace(tb.Rows(i)("base"), ",", ".") & " and diametro=" & tb.Rows(i)("diametro") & ") or"
            Next
            'ahora le quitamos la ultima coma y le añadimos el cierre del parentesisç
            bases = bases.Substring(0, bases.Length - 2) & ")"
        End If
        tb.Clear()
        If bases.Length > 6 Then
            'ahora vamos a ver los semiterminados que hay que cumplan esa base, dependera de la tipologia (monofocales o bifocales)
            Dim Sentencia As String = "", sentenciaEspecial As String = ""
            Dim BaseEspecial As Single = 0
            If ped.base_plana <> 0 Then
                BaseEspecial = ped.base_plana
            End If
            If ped.base_curva <> 0 Then
                BaseEspecial = ped.base_curva
            End If
            If EsMonoFocal(ped.id_modelo) Then
                'vemos si el pedido tiene una base especial, si lo tiene hay que ponerla primero
                If BaseEspecial <> 0 Then
                    sentenciaEspecial = "select id_lente,id_modelo,diametro,base,ojo,adicion,stock,minimo,critico,(select nombre from t_modelos where id_lente=t_semiterminados.id_modelo) as Modelo,(select codigo from t_modelos where id_lente=t_semiterminados.id_modelo) as Codigo from t_semiterminados where id_modelo=" & ped.id_modelo & " and diametro>=" & ped.diametro & " and base>=" & Int(BaseEspecial) & " and base<" & Int(BaseEspecial) + 1 & " order by base, diametro  "
                End If
                Sentencia = "select  id_lente,id_modelo,diametro,base,ojo,adicion,stock,minimo,critico,(select nombre from t_modelos where id_lente=t_semiterminados.id_modelo) as Modelo,(select codigo from t_modelos where id_lente=t_semiterminados.id_modelo) as Codigo from t_semiterminados where id_modelo>0 " & bases & " order by diametro,base "
            Else
                If BaseEspecial <> 0 Then
                    sentenciaEspecial = "select  id_lente,id_modelo,diametro,base,ojo,adicion,stock,minimo,critico,(select nombre from t_modelos where id_lente=t_semiterminados.id_modelo) as Modelo,(select codigo from t_modelos where id_lente=t_semiterminados.id_modelo) as Codigo from t_semiterminados where id_modelo=" & ped.id_modelo & " and diametro>=" & ped.diametro & " and base>=" & Int(BaseEspecial) & " and base<=" & Int(BaseEspecial) + 1 & " and ojo='" & ped.ojo & "' and adicion=" & Replace(ped.adicion, ",", ".") & "  order by base,diametro"
                End If
                Sentencia = "select  id_lente,id_modelo,diametro,base,ojo,adicion,stock,minimo,critico,(select nombre from t_modelos where id_lente=t_semiterminados.id_modelo) as Modelo,(select codigo from t_modelos where id_lente=t_semiterminados.id_modelo) as Codigo from t_semiterminados where id_modelo>0 " & bases & " and adicion=" & _
                Replace(ped.adicion, ",", ".") & " and ojo='" & ped.ojo & "'  order by base,diametro"
            End If

            Dim md As New SqlDataAdapter(Sentencia, mcon)

            mcon.Open()
            If sentenciaEspecial.Length <> 0 Then
                Dim mdEspecial As New SqlDataAdapter(sentenciaEspecial, mcon)

                mdEspecial.Fill(TbUnion)
                mcon.Close()
                Return TbUnion
            End If
            md.Fill(tb)
            'If TbUnion.Rows.Count = 0 Then
            '    md.Fill(TbUnion)
            'Else
            '    md.Fill(tb)
            '    Dim fila As DataRow
            '    For Each fila In tb.Rows
            '        Dim FilaAñadir As DataRow
            '        FilaAñadir = TbUnion.NewRow

            '        FilaAñadir("id_lente") = fila("id_lente")
            '        FilaAñadir("id_modelo") = fila("id_modelo")
            '        FilaAñadir("diametro") = fila("diametro")
            '        FilaAñadir("modelo") = fila("modelo")
            '        FilaAñadir("codigo") = fila("codigo")
            '        FilaAñadir("base") = fila("base")
            '        FilaAñadir("adicion") = fila("adicion")
            '        FilaAñadir("ojo") = fila("ojo")
            '        FilaAñadir("adicion") = fila("adicion")
            '        FilaAñadir("stock") = fila("stock")
            '        FilaAñadir("minimo") = fila("minimo")
            '        FilaAñadir("critico") = fila("critico")


            '        TbUnion.Rows.Add(FilaAñadir)
            '        FilaAñadir = Nothing
            '    Next
            'End If



            mcon.Close()



        End If
        Return tb
    End Function
    Public Function GetlentesSemiterminadosAlternativasbyPedido(ByVal ped As clsPedido) As DataTable
        Dim tb As New DataTable
        Dim Valor As Single = 0
        'sumamos el cilindro mas la esfera, si el valor es positivo le mandamos ese valor, si es negativo, le mandamos el de la esfera
        If ped.cilindro + ped.esfera >= 0 Then
            Valor = ped.cilindro + ped.esfera
        Else
            Valor = ped.esfera
        End If
        'ahora vamos a buscar las bases para ese modelo que cumplen las condiciones

        Dim cad As String = ""
        If ped.id_modelo = 28 Then 'CASO ESPECIAL DEL SUPERVIEW, QUE PUEDE FABRICARSE CON MAGICVIEW
            cad = "select id_modelo,base,diametro from t_bases where (id_modelo=28 or id_modelo=29) and diametro>=" & ped.diametro & " and not (" & Replace(Valor, ",", ".") & ">=desde  and " & Replace(Valor, ",", ".") & "<=hasta) and (" & Replace(Valor, ",", ".") & ">=desde-desviacion  and " & Replace(Valor, ",", ".") & "<=hasta+ desviacion)"
        ElseIf ped.id_modelo = 8 And ped.esfera <= 2 Then ' si es 1,56 Asferico con la esfera negativa
            cad = "select id_modelo,base,diametro from t_bases where (id_modelo=8 or (id_modelo=9 and (base=3 or base=4 or base=5))) and diametro>=" & ped.diametro & " and not(" & Replace(Valor, ",", ".") & ">=desde  and " & Replace(Valor, ",", ".") & "<=hasta) and (" & Replace(Valor, ",", ".") & ">=desde-desviacion  and " & Replace(Valor, ",", ".") & "<=hasta+ desviacion)"
        Else
            cad = "select id_modelo,base,diametro from t_bases where id_modelo=" & ped.id_modelo & " and diametro>=" & ped.diametro & "and not (" & Replace(Valor, ",", ".") & ">=desde  and " & Replace(Valor, ",", ".") & "<=hasta) and (" & Replace(Valor, ",", ".") & ">=desde-desviacion  and " & Replace(Valor, ",", ".") & "<=hasta+ desviacion)"
        End If
        Dim bases As String
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        bases = " and ("
        If tb.Rows.Count > 0 Then

            Dim i As Integer

            For i = 0 To tb.Rows.Count - 1
                bases = bases & " ( id_modelo=" & tb.Rows(i)("id_modelo") & " and  base=" & Replace(tb.Rows(i)("base"), ",", ".") & " and diametro=" & tb.Rows(i)("diametro") & ") or"
            Next
            'ahora le quitamos la ultima coma y le añadimos el cierre del parentesisç
            bases = bases.Substring(0, bases.Length - 2) & ")"
        End If
        tb.Clear()
        If bases.Length > 6 Then
            'ahora vamos a ver los semiterminados que hay que cumplan esa base, dependera de la tipologia (monofocales o bifocales)
            Dim Sentencia As String
            If EsMonoFocal(ped.id_modelo) Then
                Sentencia = "select *,(select nombre from t_modelos where id_lente=t_semiterminados.id_modelo) as Modelo,(select codigo from t_modelos where id_lente=t_semiterminados.id_modelo) as Codigo from t_semiterminados where id_modelo>0 " & bases
            Else
                Sentencia = "select *,(select nombre from t_modelos where id_lente=t_semiterminados.id_modelo) as Modelo,(select codigo from t_modelos where id_lente=t_semiterminados.id_modelo) as Codigo from t_semiterminados where id_modelo>0 " & bases & " and adicion=" & _
                Replace(ped.adicion, ",", ".") & " and ojo='" & ped.ojo & "'"
            End If
            Sentencia = Sentencia & " order by diametro,base"
            Dim md As New SqlDataAdapter(Sentencia, mcon)
            mcon.Open()
            md.Fill(tb)
            mcon.Close()

        End If
        Return tb
    End Function
    Public Function BasesDistintas(ByVal ped As clsPedido, ByVal Ped2 As clsPedido) As String
        Dim Coinciden As Boolean = False
        Dim Valor As Single = 0, valor1 As Single = 0

        'sumamos el cilindro mas la esfera, si el valor es positivo le mandamos ese valor, si es negativo, le mandamos el de la esfera
        If ped.cilindro + ped.esfera >= 0 Then
            Valor = ped.cilindro + ped.esfera
        Else
            Valor = ped.esfera
        End If
        If Ped2.cilindro + Ped2.esfera >= 0 Then
            valor1 = Ped2.cilindro + Ped2.esfera
        Else
            valor1 = Ped2.esfera
        End If
        'ahora vamos a buscar las bases para ese modelo que cumplen las condiciones
        Dim cad As String = "select count(*) from t_bases where (id_modelo=" & ped.id_modelo & " and diametro>=" & ped.diametro & " and " & Replace(Valor, ",", ".") & ">=desde  and " & Replace(Valor, ",", ".") & "<=hasta) " & _
        " and (id_modelo=" & Ped2.id_modelo & " and diametro>=" & Ped2.diametro & " and " & Replace(valor1, ",", ".") & ">=desde  and " & Replace(valor1, ",", ".") & "<=hasta)"
       ' Dim bases As String
        Dim mda As New SqlCommand(cad, mcon)
        mcon.Open()
        Coinciden = mda.ExecuteScalar

        mcon.Close()
        If Coinciden = False Then
            Return "(Bases distintas)"
        Else
            Return ""
        End If
    End Function
    Public Function ListarPedidoProvSemiterminados(ByVal idpedido As Integer) As DataTable

        Dim cad As String = "SELECT t_semiterminados.id_lente,isnull((select  nombre  from t_modelos_proveedor where id_modelo=t_semiterminados.id_modelo and id_proveedor=Proveedores.dbo.t_pedidos_semiterminados.id_proveedor),t_modelos.nombre) as [Ref. proveedor],diametro,base,ojo,adicion,sum(cantidad-servido) as cantidad " & _
        " ,dbo.precioProveedor (id_proveedor,t_semiterminados.id_modelo,diametro) as Precio,sum(cantidad-servido)*dbo.precioProveedor (id_proveedor,t_semiterminados.id_modelo,diametro) as total FROM ((t_semiterminados INNER JOIN t_Modelos  ON " & _
        "t_semiterminados.id_modelo=t_Modelos.id_lente) INNER JOIN  Proveedores.dbo.t_lineas_pedido_semiterminados ON " & _
        "Proveedores.dbo.t_lineas_pedido_semiterminados.id_lente=t_semiterminados.id_lente) INNER JOIN Proveedores.dbo.t_pedidos_semiterminados ON  Proveedores.dbo.t_lineas_pedido_semiterminados.id_pedido=Proveedores.dbo.t_pedidos_semiterminados.id_pedido where" & _
        " Proveedores.dbo.t_pedidos_semiterminados.id_pedido=" & idpedido & " group by t_modelos.nombre,t_semiterminados.id_lente,t_semiterminados.id_modelo,id_proveedor,diametro,base,ojo,adicion order by t_semiterminados.id_modelo,diametro,base,adicion,ojo"


        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub EntradaPedidoProvSemiterminado(ByVal idPedido As Integer, ByVal idproducto As Integer, ByVal cantidad As Integer)
        Dim cad As String = "UPDATE  Proveedores.dbo.t_lineas_pedido_semiterminados set servido=servido+" & cantidad & " where id_pedido=" & idPedido & " and id_lente=" & idproducto
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        EntradaSemiterminado(idproducto, cantidad)

    End Sub

    Public Sub EntradaPedidoProv(ByVal idPedido As Integer, ByVal idproducto As Integer, ByVal cantidad As Integer)
        Dim cad As String = "UPDATE t_lineas_pedidos_prov set servido=servido+" & cantidad & " where id_pedido=" & idPedido & " and id_producto=" & idproducto
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        EntradaProducto(idproducto, cantidad)

    End Sub

    Public Function CargaPedidoProv(ByVal idPedido As Integer) As DataTable
        Dim cad As String = "SELECT t_lentes_stock.id_producto, t_Modelos.nombre as Modelo,t_lentes_stock.diametro, " & _
      "t_tratamientos.nombre as Tratamiento, t_lentes_stock.cilindro, t_lentes_stock.esfera, " & _
      "'','','','',sum(cantidad-servido) as Total,0   FROM ((t_lentes_stock INNER JOIN  t_Modelos ON " & _
      "t_lentes_stock.id_modelo=t_Modelos.id_Lente) INNER JOIN t_tratamientos ON " & _
      "t_lentes_stock.tratamiento=t_tratamientos.id_tratamiento) INNER JOIN t_lineas_pedidos_prov ON t_lineas_pedidos_prov.id_producto=t_lentes_stock.id_producto where" & _
      " id_pedido=" & idPedido & " and cantidad-servido>0 group by t_lentes_stock.id_producto,t_modelos.nombre,t_tratamientos.nombre,cilindro,diametro,esfera" & _
      " ORDER BY  " & _
      "t_lentes_stock.diametro, t_tratamientos.nombre, t_lentes_stock.cilindro, t_lentes_stock.esfera"

        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function CargaPedidoSemiterminado(ByVal idPedido As Integer) As DataTable
        Dim cad As String = "SELECT t_semiterminados.id_lente, t_Modelos.nombre as Modelo,t_semiterminados.diametro, " & _
      "base, ojo,adicion, " & _
      "'','','','',cantidad,sum(cantidad-servido) as Total,0   FROM (t_semiterminados INNER JOIN t_Modelos  ON " & _
      "t_semiterminados.id_modelo=t_Modelos.id_Lente) INNER JOIN  Proveedores.dbo.t_lineas_pedido_semiterminados ON  Proveedores.dbo.t_lineas_pedido_semiterminados.id_lente=t_semiterminados.id_lente where" & _
      " id_pedido=" & idPedido & " and cantidad-servido>0 group by t_semiterminados.id_lente,nombre,diametro,base,ojo,adicion,cantidad" & _
      " ORDER BY  " & _
      "nombre,diametro, base,adicion, ojo"

        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function getPrecioBaseLente(ByVal modelo As Integer, ByVal modo As Integer, ByVal tratamiento As Integer, ByVal graduacion As Integer, _
        ByVal color As Integer) As clsProductoTarifa
        Dim cad As String = "Select * from t_precios_grupo where modelo=" & modelo & " and modo=" & modo & _
        " and tratamiento=" & tratamiento & " and forma=" & graduacion & " and color= " & color

        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        Dim mProd As New clsProductoTarifa

        mcon.Open()
        da.Fill(tb)
        mProd.modelo = modelo
        mProd.modo = modo
        mProd.tratamiento = tratamiento
        mProd.color = color
        mProd.graduacion = graduacion
        mProd.precio_base = tb.Rows(0)("precio_base")
        mProd.descripcion = tb.Rows(0)("descripcion")
        mProd.iva = tb.Rows(0)("descripcion")
        mProd.tipo = "L"
        mProd.id = tb.Rows(0)("id")
        Return mProd

    End Function
    Public Sub Distribucion(ByVal pedido As Integer)
        Dim cad As String = "UPDATE t_pedidos set fecha_salida=" & FechaAcadena(Now.Date) & ",hora_salida=" & Format(Now.Hour, "00") & Format(Now.Minute, "00") & " where id_pedido=" & pedido
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function SalidaDistribucion(ByVal idPedido As Long) As Boolean
        Dim cad As String = "select fecha_salida from t_pedidos where id_pedido=" & idPedido
        Dim da As New SqlCommand(cad, mcon)
        Dim Salida As Boolean
        mcon.Open()
        Salida = da.ExecuteScalar
        mcon.Close()
        Return Salida

    End Function

    Public Sub SalidaFabrica(ByVal fecha As String, ByVal hora As String, ByVal idPedido As Long)
        Dim da As New SqlDataAdapter
        mcon.Open()
        da.UpdateCommand = New SqlCommand("update t_pedidos set f_salida_fabrica=" & fecha & ",h_salida_fabrica='" & hora & "' where id_pedido=" & idPedido, mcon)
        da.UpdateCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Sub SalidaTratamiento(ByVal fecha As String, ByVal hora As String, ByVal idPedido As Long)
        Dim da As New SqlDataAdapter
        mcon.Open()
        da.UpdateCommand = New SqlCommand("update t_pedidos set f_salida_tratamiento=" & fecha & ",h_salida_tratamiento='" & hora & "' where id_pedido=" & idPedido, mcon)
        da.UpdateCommand.ExecuteNonQuery()
        da.UpdateCommand.CommandText = "Update t_tratamiento_externo set Fec_entrada=" & _
        fecha & ",hora_entrada=" & Format(hora, "0000") & " where id_pedido=" & idPedido
        da.UpdateCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Sub SalidaColoracion(ByVal fecha As String, ByVal hora As String, ByVal idPedido As Long)
        Dim da As New SqlDataAdapter
        mcon.Open()
        da.UpdateCommand = New SqlCommand("update t_pedidos set fecha_coloracion=" & fecha & ",hora_coloracion='" & hora & "' where id_pedido=" & idPedido, mcon)
        da.UpdateCommand.ExecuteNonQuery()
        da.UpdateCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function ExisteAlbaran(ByVal idPedido As Long) As Boolean
        Dim existe As Boolean = False
        Dim cad As String = "select count(*) from t_albaranes where id_pedido=" & idPedido
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        existe = CBool(cmd.ExecuteScalar)
        mcon.Close()
        Return existe


    End Function
    Public Function ExisteCodBarraStock(ByVal codbarra As String) As Boolean
        Dim existe As Boolean = False
        Dim cad As String = "select count(*) from t_codigos_barra where cod_barra=" & strsql(codbarra)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        existe = CBool(cmd.ExecuteScalar)
        mcon.Close()
        Return existe


    End Function
    Public Function ExisteAlbaranAlquilerTracer(ByVal idcli As Long, ByVal fecha As Date) As Boolean
        Dim existe As Boolean = False
        Dim desde As Integer = Now.Year & Format(Now.Month, "00") & "01"
        Dim fecfin As Date = DateAdd(DateInterval.Month, 1, CDate("01/" & Now.Month & "/" & Now.Year))
        Dim hasta As Integer = FechaAcadena(DateAdd(DateInterval.Day, -1, fecfin))
        Dim cad As String = "select count(*) from t_albaranes INNER JOIN t_lineas_albaran ON t_lineas_albaran.id_albaran=t_albaranes.id_albaran where id_tipo_producto=5 and id_cliente=" & idcli & " and fecha>=" & desde & " and fecha<=" & hasta
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        existe = CBool(cmd.ExecuteScalar)
        mcon.Close()
        Return existe


    End Function

    Public Sub SalidaAlmacen(ByVal fecha As String, ByVal hora As String, ByVal idpedido As Long)
        Dim da As New SqlDataAdapter
        mcon.Open()
        da.UpdateCommand = New SqlCommand("update t_pedidos set f_salida_almacen=" & fecha & ",h_salida_almacen='" & hora & "' where id_pedido=" & idpedido, mcon)
        da.UpdateCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function ComprobarReferenciaRepetida(ByVal ref As String, ByVal idCliente As Long) As Boolean
        Dim cad As String = "select * from t_pedidos where anulado = 0 and referencia like " & strsql(ref) & " and id_cliente=" & idCliente
        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        da.Fill(tb)
        mcon.Close()
        If tb.Rows.Count > 0 Then Return True
        Return False
    End Function

    Public Function GetPedidosPendienteTtoExterno() As DataTable
        'aqui tenemos que buscar aquellos pedidos que cumplan la condicion de que HoraSalida sea 0
        Dim cad As String = " select * from t_tratamiento_externo where hora_salida=0"
        Dim Dta As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        Dta.Fill(tb)
        mcon.Close()
        Dta = Nothing
        Return tb

    End Function

    Public Function GetPedidosPendienteVueltaTtoExterno() As DataTable
        'aqui tenemos que buscar aquellos pedidos que cumplan la condicion de que hora_salida sea 0
        Dim cad As String = " select * from t_tratamiento_externo where fec_entrada=0 and hora_salida>0"
        Dim Dta As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        Dta.Fill(tb)
        mcon.Close()
        Dta = Nothing
        Return tb

    End Function
    Public Sub GrabaTrazadorCliente(ByVal idcli As Integer, ByVal idtrazador As Integer, ByVal numSerie As String, ByVal pventa As Decimal, ByVal alquiler As Decimal, ByVal fecha As Integer, Optional ByVal Nuevo As Boolean = True)
        Dim cad As String = ""
        If Nuevo = True Then
            cad = "insert into t_trazadores_cliente (id_cliente,id_trazador,num_serie,pventa,alquiler,fecha_fin) VALUES (" & idcli & "," & idtrazador & "," & strsql(numSerie) & "," & NumSql(pventa) & "," & NumSql(alquiler) & "," & fecha & ")"
        Else
            cad = "Update t_trazadores_cliente set num_serie=" & strsql(numSerie) & ",pventa=" & NumSql(pventa) & ",alquiler=" & NumSql(alquiler) & ",fecha_fin=" & fecha & " where id_cliente=" & idcli
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function getAnotacionesPedidoById(ByVal idPedido As Long) As clsAnotaciones
        Dim cad As String = "select t_anotaciones_pedidos.*,(t_usuarios.nombre + ' ' + t_usuarios.apellidos) as nombre from t_anotaciones_pedidos inner join t_usuarios on " & _
        "t_anotaciones_pedidos.id_usuario=t_usuarios.id_usuario where id_pedido=" & idPedido & " order by fecha desc ,hora desc"
        Dim da As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        Dim mAns As New clsAnotaciones

        da.Fill(tb)

        Dim i As Integer
        For i = 0 To tb.Rows.Count - 1
            Dim mA As New clsAnotacion
            mA.idAnotacion = tb.Rows(i)("id_anotacion")
            mA.IdUsuario = tb.Rows(i)("id_usuario")
            mA.fecha = tb.Rows(i)("fecha")
            mA.comentario = tb.Rows(i)("comentario")
            mA.IdPedido = idPedido
            mA.Usuario = tb.Rows(i)("nombre")
            mA.hora = tb.Rows(i)("hora")
            mAns.add(mA)
            mA = Nothing

        Next
        'ahora vamos a añadir las incidencias
        cad = "select t_incidencias_pedidos.*,(select incidencia from t_incidencias where id_incidencia=t_incidencias_pedidos.id_incidencia) as incidencia ,(t_usuarios.nombre + ' ' + t_usuarios.apellidos) as nombre from t_incidencias_pedidos inner join t_usuarios on " & _
       "t_incidencias_pedidos.id_usuario=t_usuarios.id_usuario where id_pedido=" & idPedido & " order by fecha,hora"
        Dim da1 As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb1 As New DataTable
        da1.Fill(tb1)
        For i = 0 To tb1.Rows.Count - 1
            Dim mA As New clsAnotacion
            mA.idAnotacion = tb1.Rows(i)("id_orden")
            mA.IdUsuario = tb1.Rows(i)("id_usuario")
            mA.fecha = tb1.Rows(i)("fecha")
            Dim Aviso As String = ""
            If tb1.Rows(i)("aviso_cliente") <> 0 Then
                Aviso = "AVISADO CLIENTE:"
            ElseIf IsDBNull(tb1.Rows(i)("omitida")) Then

                Aviso = "AVISO OMITIDO:"
            ElseIf tb1.Rows(i)("omitida") <> 0 Then

                Aviso = "AVISO OMITIDO:"
            End If
            mA.comentario = Aviso & "ORDEN DE TRABAJO " & tb1.Rows(i)("id_orden") & " INCIDENCIA EN " & UCase(tb1.Rows(i)("incidencia"))
            mA.IdPedido = idPedido
            mA.Usuario = tb1.Rows(i)("nombre")
            mA.hora = tb1.Rows(i)("hora")
            mAns.add(mA)
            mA = Nothing

        Next
        If mAns.Count = 0 Then
            mAns = Nothing
        End If
        Return mAns
    End Function
    Public Function GetTrazadoresClientes() As DataTable
        Dim cad As String = "select t_clientes.id_cliente,codigo,nombre_comercial,t_trazadores.id_trazador,trazador,num_serie,pventa,alquiler,fecha_fin from t_trazadores_cliente INNER JOIN t_trazadores ON t_trazadores_cliente.id_trazador=t_trazadores.id_trazador INNER JOIN t_clientes ON t_clientes.id_cliente=t_trazadores_cliente.id_cliente ORDER BY codigo"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetModelosCliente(ByVal idCliente As Integer) As DataTable
        Dim cad As String = "select modelo,nombre,id_modelo from t_modelos INNER JOIN t_modelos_cliente ON t_modelos.id_lente=t_modelos_cliente.id_modelo where id_cliente=" & idCliente
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb

    End Function
    Public Function GetTratamientosCliente(ByVal idCliente As Integer) As DataTable
        Dim cad As String = "select tratamiento,nombre,t_tratamientos.id_tratamiento from t_tratamientos INNER JOIN t_tratamientos_cliente ON t_tratamientos.id_tratamiento=t_tratamientos_cliente.id_tratamiento where id_cliente=" & idCliente
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb

    End Function
    Public Function GetTratamientos() As DataTable
        Dim cad As String = "select nombre,id_tratamiento,precio from t_tratamientos order by id_tratamiento"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        ' mcon.Open()
        mda.Fill(tb)
        'mcon.Close()
        Return tb

    End Function
    Public Function GetTratamientosPromocion(ByVal idpromo As Integer) As DataTable
        Dim cad As String = "select nombre,id_tratamiento,isnull((select 'X' from t_promociones where id_promocion=" & idpromo & " and tratamientos like '%('+ convert(varchar(1),t_tratamientos.id_tratamiento)+')%'),'') as incluido from t_tratamientos order by id_tratamiento"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb

    End Function
    Public Function GetPreciosTratamientosPromocion(ByVal idPromo As Integer) As DataTable
        Dim cad As String = "select nombre,id_tratamiento,precio,isnull((select precio from t_promo_tratamientos where id_promocion=" & idPromo & " and id_tratamiento=t_tratamientos.id_tratamiento),precio) as PVPTratamiento from t_tratamientos order by id_tratamiento"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb

    End Function
    Public Function GetGamasColor() As DataTable
        Dim cad As String = "select * from t_gamas_coloracion order by id_gama"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        '  mcon.Open()
        mda.Fill(tb)
        ' mcon.Close()
        Return tb

    End Function
    Public Function GetGamasColorPromocion(ByVal idpromo As Integer) As DataTable
        Dim cad As String = "select *,isnull((select 'X' from t_promociones where id_promocion=" & idpromo & " and gamas_color like '%('+ convert(varchar(1),t_gamas_coloracion.id_gama)+')%'),'') as incluido from t_gamas_coloracion order by id_gama"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb

    End Function
    Public Function GetPromocion(ByVal id As Integer) As clsPromocion
        Dim p As New clsPromocion
        Dim cad As String = ""
        cad = "select * from t_promociones where id_promocion=" & id
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        If tb.Rows.Count > 0 Then
            p.id = tb.Rows(0)("id_promocion")
            p.Nombre = tb.Rows(0)("nombre")
            p.Lentes = tb.Rows(0)("lentes")
            p.Parejas = tb.Rows(0)("parejas")
            p.Tratamientos = tb.Rows(0)("tratamientos")
            p.Colores = tb.Rows(0)("gamas_color")
            p.Combinar = CBool(tb.Rows(0)("combinar"))
            p.MismaGraduacion = CBool(tb.Rows(0)("misma_graduacion"))
            p.Inicio = tb.Rows(0)("inicio")
            p.Fin = tb.Rows(0)("fin")
            p.GrupoOptico = tb.Rows("0")("id_grupo")
        End If
        tb.Clear()
        mda.SelectCommand.CommandText = "select * from t_promo_PVP_fabricacion where id_promocion=" & id
        mda.Fill(tb)
        For Each r As DataRow In tb.Rows
            Dim pre As New Precio
            pre.id = r("id_grupo")
            pre.precio = r("precio")
            p.Modelos.Add(pre)
        Next
        tb.Clear()
        mda.SelectCommand.CommandText = "select * from t_promo_tratamientos where id_promocion=" & id
        mda.Fill(tb)

        For Each r As DataRow In tb.Rows
            Dim pre As New Precio
            pre.id = r("id_tratamiento")
            pre.precio = r("precio")
            p.PVPTratamiento.Add(pre)
        Next
        tb.Clear()
        mda.SelectCommand.CommandText = "select * from t_promo_gamas_color where id_promocion=" & id
        mda.Fill(tb)
        For Each r As DataRow In tb.Rows
            Dim pre As New PrecioGamaColor
            pre.id = r("id_gama")
            pre.PrecioHI = r("precio_hi")
            pre.PreciolI = r("precio_li")
            p.PVPColores.Add(pre)
        Next
        tb.Clear()
        mda.SelectCommand.CommandText = "select * from t_promo_PVP_stock where id_promocion=" & id
        mda.Fill(tb)
        For Each r As DataRow In tb.Rows
            Dim pre As New clsPVPLenteStock
            pre.grupo = r("id_grupo")
            pre.Tratamiento = r("id_tratamiento")
            pre.Diametro = r("diametro")
            pre.Cilindro = r("cilindro")
            pre.precio = r("precio")
            p.ModelosStock.Add(pre)
        Next
        tb.Clear()
        mda.SelectCommand.CommandText = "select id_cliente from t_clientes_sin_promocion where id_promocion=" & id
        mda.Fill(tb)
        For Each r As DataRow In tb.Rows
            p.ClientesSinPromo.Add(r("id_cliente"))
        Next
        Return p
    End Function
    Public Function GetPreciosGamasColorPromocion(ByVal idpromo As Integer) As DataTable
        Dim cad As String = "select *,isnull((select precio_LI from t_promo_gamas_color where id_promocion=" & idpromo & " and id_gama=t_gamas_coloracion.id_gama),precio_LI) as PromoLI,isnull((select precio_HI from t_promo_gamas_color where id_promocion=" & idpromo & " and id_gama=t_gamas_coloracion.id_gama),precio_HI) as PromoHI from t_gamas_coloracion order by id_gama"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb

    End Function
    Public Sub GrabaPrecioFabricacion(ByVal idGrupo As Integer, ByVal precio As Decimal)
        Dim cad As String = "Delete from t_pvp_lentes_fabricacion where id_grupo=" & idGrupo
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        cmd.CommandText = "INSERT INTO t_pvp_lentes_fabricacion (id_grupo,precio) VALUES (" & idGrupo & "," & NumSql(precio) & ")"
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub GrabaPrecioStock(ByVal grupo As ArrayList)
        Dim cad As String = "BEGIN TRAN PvpStock" & vbNewLine & "Delete from t_pvp_lentes_stock "
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        Try
            For Each p As clsPVPLenteStock In grupo
                cmd.CommandText = "INSERT INTO t_pvp_lentes_stock (id_grupo,id_tratamiento,diametro,cilindro,precio) VALUES (" & _
                p.grupo & "," & p.Tratamiento & "," & p.Diametro & "," & NumSql(p.Cilindro) & "," & NumSql(p.Precio) & ")"
                cmd.ExecuteNonQuery()
            Next
            cmd.CommandText = "COMMIT TRAN PvpStock"
            cmd.ExecuteNonQuery()
            mcon.Close()
            'Exit Sub
        Catch ex As Exception
            cmd.CommandText = "ROLLBACK TRAN PvpStock"
            cmd.ExecuteNonQuery()
            mcon.Close()
        End Try

    End Sub
    Public Function getTratamientoCliente(ByVal idcliente As Integer, ByVal idTratamiento As Integer) As String
        Dim Tratamiento As String = ""
        Dim cad As String = "Select tratamiento from t_tratamientos_cliente where id_cliente=" & idcliente & " and id_tratamiento=" & idTratamiento
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        If tb.Rows.Count > 0 Then
            Tratamiento = tb.Rows(0)("tratamiento")
        End If
        'If Tratamiento.Length = 0 Then
        '    tb.Clear()
        '    mda.SelectCommand.CommandText = "select nombre from t_tratamientos where id_tratamiento=" & idTratamiento
        '    mda.Fill(tb)
        '    Tratamiento = tb.Rows(0)("nombre")
        'End If
        mcon.Close()
        Return Tratamiento
    End Function
    Public Function getModeloCliente(ByVal idcliente As Integer, ByVal idModelo As Integer) As String
        Dim modelo As String = ""
        Dim cad As String = "Select modelo from t_modelos_cliente where id_cliente=" & idcliente & " and id_modelo=" & idModelo
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        If tb.Rows.Count > 0 Then
            modelo = tb.Rows(0)("modelo")
        End If
        'If modelo.Length = 0 Then
        '    tb.Clear()
        '    mda.SelectCommand.CommandText = "select nombre from t_modelos where id_lente=" & idModelo
        '    mda.Fill(tb)
        '    modelo = tb.Rows(0)("nombre")
        'End If
        mcon.Close()
        Return modelo
    End Function

    Public Sub EliminaTratamientoCliente(ByVal idcliente As Integer, ByVal idTratamiento As Integer)
        Dim cad As String = "DELETE FROM t_tratamientos_cliente where id_cliente=" & idcliente & " and id_tratamiento=" & idTratamiento
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub GrabaTratamientoCliente(ByVal idcliente As Integer, ByVal idTratamiento As Integer, ByVal nombre As String)
        Dim cad As String = "INSERT INTO t_tratamientos_cliente (id_cliente,id_tratamiento,tratamiento) VALUES (" & idcliente & "," & idTratamiento & "," & strsql(nombre) & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        EliminaTratamientoCliente(idcliente, idTratamiento)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub EliminaModeloCliente(ByVal idcliente As Integer, ByVal idmodelo As Integer)
        Dim cad As String = "DELETE FROM t_modelos_cliente where id_cliente=" & idcliente & " and id_modelo=" & idmodelo
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub GrabaModeloCliente(ByVal idcliente As Integer, ByVal idmodelo As Integer, ByVal nombre As String)
        Dim cad As String = "INSERT INTO t_modelos_cliente (id_cliente,id_modelo,modelo) VALUES (" & idcliente & "," & idmodelo & "," & strsql(nombre) & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        EliminaModeloCliente(idcliente, idmodelo)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function GetAvisosPendientes() As DataTable
        Dim cad As String = "select id_orden,t_pedidos.id_pedido,t_pedidos.fecha as Fecha_pedido,compromiso,id_incidencia,(select incidencia from t_incidencias where id_incidencia=t_incidencias_pedidos.id_incidencia) as incidencia " & _
        ",isnull((select nombre + ' ' + apellidos from t_usuarios where id_usuario=t_pedidos.id_usuario),'Pedido Web Automatico') as usuario,t_incidencias_pedidos.fecha as fecha_incidencia,nombre_comercial from (t_incidencias_pedidos INNER JOIN " & _
        " t_Pedidos On t_incidencias_pedidos.id_pedido=t_pedidos.id_pedido) INNER JOIN t_clientes ON t_clientes.id_cliente=t_pedidos.id_cliente where aviso_cliente=0 and omitida=0 and anulado=0 and id_albaran=0 and  t_pedidos.fecha_salida=0 and t_pedidos.anulado=0"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub AvisaCliente(ByVal idpedido As Integer, ByVal idOrden As Integer, ByVal idincidencia As Integer)
        Dim cad As String = "update t_incidencias_pedidos set aviso_cliente=-1 where id_pedido=" & idpedido & " and id_orden=" & idOrden & " and id_incidencia=" & idincidencia
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub

    Public Sub OmitirAviso(ByVal idpedido As Integer, ByVal idOrden As Integer, ByVal idincidencia As Integer)
        Dim cad As String = "update t_incidencias_pedidos set omitida=-1 where id_pedido=" & idpedido & " and id_orden=" & idOrden & " and id_incidencia=" & idincidencia
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub

    Public Sub GrabaAnotacion(ByVal idPedido As Long, ByVal idUsuario As Long, ByVal fecha As String, ByVal comentario As String, ByVal hora As String)
        Dim cad As String = ""
        Dim id As Long = getMaxId("id_anotacion", "t_anotaciones_pedidos") + 1
        cad = "insert into t_anotaciones_pedidos (id_pedido,id_anotacion,id_usuario,fecha,comentario,hora) values " & _
        "(" & idPedido & "," & id & "," & idUsuario & "," & fecha & ",'" & comentario & "'," & hora & ")"
        Dim q As New SqlCommand(cad, mcon)
        mcon.Open()
        q.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Sub DesanulaPedido(ByVal p As clsPedido)
        Dim cad As String = "UPDATE t_Pedidos set anulado=0 where id_pedido=" & p.id & vbNewLine & _
        "UPDATE t_pedidos set anulado=0 where id_pedido=" & p.pareja & vbNewLine & _
        "DELETE FROM t_pedidos_anulado where id_pedido=" & p.id & vbNewLine & _
        "DELETE FROM t_pedidos_anulado where id_pedido=" & p.pareja
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub AnularPedido(ByVal idPedido As Long, ByVal causa As String)
        Dim da As New SqlDataAdapter
        mcon.Open()



        da.UpdateCommand = New SqlCommand("update t_pedidos set anulado=1,causa='" & causa & "' where id_pedido=" & idPedido, mcon)
        da.UpdateCommand.ExecuteNonQuery()
        'si es un pedido web lo borramos de la impresion
        da.DeleteCommand = New SqlCommand("Delete from t_pedidos_web where id_pedido=" & idPedido, mcon)
        da.DeleteCommand.ExecuteNonQuery()
        '****************Vamos a grabar el usuario que anula el pedido
        da.InsertCommand = New SqlCommand("Insert into t_pedidos_anulado (id_pedido,id_usuario) VALUES (" & idPedido & "," & mUsuario.id & ")", mcon)
        da.InsertCommand.ExecuteNonQuery()
        'ahora vemos si llevaba puntos, si los llevaba anulamos la linea y se los restamos al cliente
        Dim puntos As Integer = GetPuntosByPedido(idPedido)

        If puntos > 0 Then

            Dim CLiente As Integer
            da.SelectCommand = New SqlCommand
            da.SelectCommand.Connection = mcon
            da.SelectCommand.CommandText = "Select id_cliente from t_pedidos where id_pedido=" & idPedido
            CLiente = da.SelectCommand.ExecuteScalar
            da.DeleteCommand.CommandText = "DELETE FROM t_puntos_pedido where id_pedido=" & idPedido
            da.DeleteCommand.ExecuteNonQuery()
            da.UpdateCommand.CommandText = "UPDATE t_clientes set puntos=puntos-" & puntos & " where id_cliente=" & CLiente
            da.UpdateCommand.ExecuteNonQuery()
        End If
        mcon.Close()
    End Sub


    'Public Function ListadoPedidosClientes(ByVal idCli As Long, ByVal fecini As String, ByVal fecfin As String) As DataTable
    '    Dim da As New SqlDataAdapter
    '    mcon.Open()
    '    Dim cad As String = "select t_pedidos.id_pedido,t_pedidos.fecha,t_pedidos.hora,isnull((select nombre + ' ' + apellidos from t_usuarios where id_usuario=t_pedidos.id_usuario),"Pedido Web" as usuario," & _
    '    "(select nombre from t_modelos where id_lente=t_pedidos.id_modelo) as modelo, modo,diametro,ojo,eje,cilindro,esfera,adicion,(select " & _
    '    " nombre from t_tratamientos where id_tratamiento=t_pedidos.id_tratamiento) as tratamiento, case when id_coloracion=0 then '' else 'SI' end as color,pareja,referencia,fecha_salida,hora_salida,esmerilado,precalibrado," & _
    '    "fs_fabrica as [Fecha Fabrica],hs_fabrica as [Hora Fabrica],fs_coloracion as [Fecha Color],hs_coloracion as [Hora Color], fs_endurecimiento as [Fecha Endurecido], hs_endurecimiento as [hora_endurecido]," & _
    '    "fs_tratamiento as [Fecha tto],hs_tratamiento as [hora tto], Observaciones,sin_cargo,montaje,compromiso,(select incidencia from t_incidencias where id_incidencia in (select id_incidencia from t_incidencias_pedidos" & _
    '    " where id_pedido=t_pedidos.id_pedido and id_orden =(select max(id_orden) from t_incidencias_pedidos where id_pedido=t_pedidos.id_pedido))) as incidencia from t_pedidos INNER JOIN t_ordenes_trabajo on t_pedidos.id_pedido=t_ordenes_trabajo.id_pedido where id_cliente = " & idCli & " and t_pedidos.fecha>=" & fecini & _
    '    " and t_pedidos.fecha<=" & fecfin & "  and id_orden in (select max(id_orden) from t_ordenes_trabajo where id_pedido=t_pedidos.id_pedido) order by t_pedidos.id_pedido"
    '    da.SelectCommand = New SqlCommand(cad, mcon)
    '    Dim tb As New DataTable
    '    da.Fill(tb)
    '    mcon.Close()
    '    Return tb

    'End Function
    Public Function ListadoPedidosClientes(ByVal idCli As Long, ByVal fecini As String, ByVal fecfin As String) As DataTable
        Dim da As New SqlDataAdapter
        mcon.Open()
        Dim cad As String = "select t_pedidos.id_pedido as [Nº Pedido],dbo.fechaexcel(t_pedidos.fecha) as Fecha,dbo.Hora(t_pedidos.hora) as Hora,isnull((select nombre + ' ' + apellidos from t_usuarios where id_usuario=t_pedidos.id_usuario),'Pedido Web') as usuario," & _
        "(select nombre from t_modelos where id_lente=t_pedidos.id_modelo) as modelo, modo,diametro,ojo,eje,cilindro,esfera,adicion,(select " & _
        " nombre from t_tratamientos where id_tratamiento=t_pedidos.id_tratamiento) as tratamiento, case when id_coloracion=0 then '' else 'SI' end as color,pareja,referencia,fecha_salida,hora_salida,esmerilado,precalibrado," & _
        "fs_fabrica as [Fecha Fabrica],hs_fabrica as [Hora Fabrica],fs_coloracion as [Fecha Color],hs_coloracion as [Hora Color], fs_endurecimiento as [Fecha Endurecido], hs_endurecimiento as [hora_endurecido]," & _
        "fs_tratamiento as [Fecha tto],hs_tratamiento as [hora tto], Observaciones,sin_cargo,montaje,compromiso,(select incidencia from t_incidencias where id_incidencia in (select id_incidencia from t_incidencias_pedidos" & _
        " where id_pedido=t_pedidos.id_pedido and id_orden =(select max(id_orden) from t_incidencias_pedidos where id_pedido=t_pedidos.id_pedido))) as incidencia from t_pedidos INNER JOIN t_ordenes_trabajo on t_pedidos.id_pedido=t_ordenes_trabajo.id_pedido where id_cliente = " & idCli & " and t_pedidos.fecha>=" & fecini & _
        " and t_pedidos.fecha<=" & fecfin & " and anulado=0 and id_orden in (select max(id_orden) from t_ordenes_trabajo where id_pedido=t_pedidos.id_pedido) order by t_pedidos.id_pedido"
        da.SelectCommand = New SqlCommand(cad, mcon)
        Dim tb As New DataTable
        da.Fill(tb)
        mcon.Close()
        Return tb

    End Function
    Public Function ExisteAcuerdoEntreFechasByCLiente(ByVal cli As Integer, ByVal Inicio As Integer, ByVal fin As Integer) As Integer
        Dim Cad As String = "select isnull(id_acuerdo,0) from t_acuerdos_clientes where id_cliente=" & cli & " and ((desde<=" & Inicio & " and hasta>=" & Inicio & ") or (desde<=" & fin & " and hasta>=" & fin & "))"
        Dim cmd As New SqlCommand(Cad, mcon)
        Dim abierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            abierta = True
        Else
            mcon.Open()
        End If
        Dim acuerdo As Integer = cmd.ExecuteScalar
        If abierta = False Then
            mcon.Close()
        End If
        Return acuerdo
    End Function
    Public Function GetIDAcuerdoByCliente(ByVal idCLiente As Long, ByVal fecha As Integer) As Long
        Dim da As New SqlDataAdapter
        mcon.Open()
        Dim cad As String = "select id_acuerdo from t_acuerdos_clientes where id_cliente=" & idCLiente & " and desde<=" & fecha & " and hasta>=" & fecha
        da.SelectCommand = New SqlCommand(cad, mcon)
        Dim tb As New DataTable
        da.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then Return 0
        Return tb.Rows(0)("ID_ACUERDO")
    End Function

    Public Function DevuelveTbSemiterminado(ByVal idMOdelo As Long, ByVal diametro As String) As DataTable
        Dim da As New SqlDataAdapter
        mcon.Open()
        '************** ATENCION **************
        'el orden en la select es imprescindible ya que la carga de las imagnenes del almacen se hacen en base a 
        'la posicion de las lentes en la consulta.
        '***********************************************************
        Dim cad As String = "select * from t_semiterminados where id_modelo = " & idMOdelo & " and diametro =" & diametro & _
        " order by adicion,base,ojo"
        da.SelectCommand = New SqlCommand(cad, mcon)
        Dim tb As New DataTable
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function CargaSemiterminado(ByVal idModelo As Integer, ByVal diametro As Integer, ByVal ojo As String, ByVal base As Single, ByVal adicion As Single) As clsSemiterminado

        Dim Cad As String = " select * from t_semiterminados where id_modelo=" & idModelo & " and diametro=" & diametro & " and (ojo='" & ojo & "' or ojo='') and base=" & Replace(base, ",", ".") & " and adicion=" & Replace(adicion, ",", ".")
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(Cad, mcon)
        Dim semi As New clsSemiterminado
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count > 0 Then
            semi.IdLente = tb.Rows(0)("id_lente")
            semi.idModelo = idModelo
            semi.Adicion = adicion
            semi.Base = base
            semi.Diametro = diametro
            semi.Ojo = tb.Rows(0)("ojo")
            semi.Stock = tb.Rows(0)("stock")
            semi.Critico = tb.Rows(0)("critico")
            semi.Minimo = tb.Rows(0)("minimo")
        End If
        tb.Dispose()
        Return semi
    End Function

    Public Function getListaAdiciones(ByVal id_modelo As Integer, ByVal diametro As Integer) As String
        Dim da As New SqlDataAdapter
        mcon.Open()
        Dim cad As String = "select distinct adicion from t_semiterminados where id_modelo = " & id_modelo & " and diametro =" & diametro
        da.SelectCommand = New SqlCommand(cad, mcon)
        Dim tb As New DataTable
        da.Fill(tb)
        mcon.Close()
        Dim i As Integer
        cad = ""
        For i = 0 To tb.Rows.Count - 1
            cad = cad & tb.Rows(i)("adicion") & ";"
        Next
        cad = cad.Substring(0, cad.Length - 1)
        Return cad
    End Function
    Public Function getlistaBases(ByVal id_modelo As Integer, ByVal diametro As Integer) As String
        Dim da As New SqlDataAdapter
        mcon.Open()
        Dim cad As String = "select distinct base from t_semiterminados where id_modelo = " & id_modelo & " and diametro =" & diametro
        da.SelectCommand = New SqlCommand(cad, mcon)
        Dim tb As New DataTable
        da.Fill(tb)
        mcon.Close()
        Dim i As Integer
        cad = ""
        For i = 0 To tb.Rows.Count - 1
            cad = cad & tb.Rows(i)("base") & ";"
        Next
        cad = cad.Substring(0, cad.Length - 1)
        Return cad
    End Function

    Public Function getStockSemiterminado(ByVal modelo As Integer, ByVal diametro As Integer, ByVal base As String, ByVal adicion As String, ByVal ojo As Integer) As DataTable
        Dim da As New SqlDataAdapter
        mcon.Open()
        Dim cad As String = "select * from t_semiterminados where " & _
        "id_modelo = " & modelo & " And diametro = " & diametro & _
        " and base=" & base & " and adicion =" & adicion & " and ojo =" & ojo
        da.SelectCommand = New SqlCommand(cad, mcon)
        Dim tb As New DataTable
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function CargaCodBarrasSemiterminados(ByVal idLente As Integer) As clsCodBarrasProveedores
        Dim cad As String = "select t_cod_barras_semiterminado.*,Proveedor from t_cod_barras_semiterminado INNER JOIN Proveedores.dbo.t_proveedores ON t_cod_barras_semiterminado.id_proveedor=Proveedores.dbo.t_proveedores.id_proveedor where id_lente=" & idLente
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Dim codBarras As New clsCodBarrasProveedores
        Dim i As Integer
        For i = 0 To tb.Rows.Count - 1
            Dim CodBarra As New clsCodBarraSemiterminado
            CodBarra.IdProveedor = tb.Rows(i)("id_proveedor")
            CodBarra.Proveedor = tb.Rows(i)("proveedor")
            CodBarra.Radio = tb.Rows(i)("radio_curvatura")
            CodBarra.Codbarra = tb.Rows(i)("cod_barra")
            codBarras.add(CodBarra)
            CodBarra = Nothing
        Next
        Return codBarras

    End Function

    Public Function CargaProveedores() As DataTable
        Dim cad As String = " select * from Proveedores.dbo.t_proveedores where baja=0 order by proveedor"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub GrabaCodBarrasSemiterminados(ByVal idLente As String, ByVal codigos As clsCodBarrasProveedores)
        Dim cad As String = "DELETE FROM t_cod_barras_semiterminado where id_lente=" & idLente
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        Dim codBarras As clsCodBarraSemiterminado
        For Each codBarras In codigos
            cad = " INSERT INTO t_cod_barras_semiterminado VALUES (" & idLente & "," & codBarras.IdProveedor & ",'" & codBarras.Codbarra & "',0,0,0)"
            cmd.CommandText = cad
            cmd.ExecuteNonQuery()
        Next
        mcon.Close()
    End Sub
    Public Function getDiametrosSemiterminados(ByVal modelo As Integer) As DataTable
        Dim da As New SqlDataAdapter
        mcon.Open()
        Dim cad As String = "Select distinct diametro from t_semiterminados where id_modelo = " & modelo
        da.SelectCommand = New SqlCommand(cad, mcon)
        Dim tb As New DataTable
        da.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetAcuerdoByIdCliente(ByVal id_cliente As Integer, ByVal fecha As Integer, Optional ByVal Grupo As Integer = 0) As clsAcuerdo

        Dim tb As New DataTable
        Dim mA As New clsAcuerdo
        Dim i As Integer = 0
        Dim mda As New SqlDataAdapter("select id_acuerdo from " & _
        " t_acuerdos_clientes where id_cliente = " & id_cliente & " and desde<=" & fecha & " and hasta>=" & fecha, mcon)
        'mcon.Open()
        mda.Fill(tb)
        'mcon.Close()
        If tb.Rows.Count > 0 Then
            mA = GetAcuerdoById(tb.Rows(0)("id_acuerdo"), Grupo)
        End If
        Return mA

    End Function
    Public Function TieneAcuerdobyCliente(ByVal idCliente As Long) As Long
        '**********************************
        'determina si un cliente tiene un acuerdo especifico o no
        '***************************************

        Dim tb As New DataTable
        Dim mA As New clsAcuerdo
        Dim mda As New SqlDataAdapter("select * from t_acuerdos_clientes where id_cliente = " & idCliente, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count > 0 Then
            Return tb.Rows(0)("id_acuerdo")
        End If
        Return -1
    End Function
    Public Sub ModificaPedido(ByVal p As clsPedido, ByVal modificacion As String)
        Dim o As New clsOrdenesTrabajo
        o = GetOrdenesTrabajo(p.id)
        Dim cad As String = "Update t_pedidos set id_cliente=" & p.id_cliente & ",modo=" & strsql(p.modo) & ",id_modelo=" & p.id_modelo & ",diametro=" & NumSql(p.diametro) & ",id_tratamiento=" & p.id_tratamiento & ",id_coloracion=" & p.id_coloracion & ",intensidad=" & NumSql(p.intensidad) & ",pasillo=" & p.Pasillo & _
        ",eliptico=" & IIf(p.Eliptico = False, 0, 1) & ",inset=" & NumSql(p.Inset) & ",cilindro=" & NumSql(p.cilindro) & ",esfera=" & NumSql(p.esfera) & ",eje=" & NumSql(p.eje) & ",adicion=" & NumSql(p.adicion) & ",precalibrado=" & p.precalibrado & " where id_pedido=" & p.id & vbNewLine & _
        "DELETE FROM t_puntos_pedido where id_pedido=" & p.id
        If p.Puntos <> 0 Then
            cad = cad & vbNewLine & "INSERT INTO t_puntos_pedido (id_pedido,puntos) VALUES (" & p.id & "," & p.Puntos & ")"
        End If

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        GrabaAnotacion(p.id, mUsuario.id, FechaAcadena(Now.Date), modificacion, Now.Hour & Format(Now.Minute, "00"))
        'ahora lanzamos un mensaje en la situacion en que esta el pedido
        If o.Item(o.Count - 1).FeAntireflejo <> 0 And o.Item(o.Count - 1).FsAntireflejo <> 0 Then
            MsgBox("El pedido " & IIf(p.ojo = "D", "Derecho", "Izquierdo") & " se encuentra en salida de Antireflejo, Informe a Calidad")
        ElseIf o.Item(o.Count - 1).FeAntireflejo <> 0 And o.Item(o.Count - 1).FsAntireflejo = 0 Then
            MsgBox("El pedido " & IIf(p.ojo = "D", "Derecho", "Izquierdo") & " se encuentra en  Antireflejo, Informe a Tratamientos")
        ElseIf o.Item(o.Count - 1).FeEndurecido <> 0 And o.Item(o.Count - 1).FsEndurecido <> 0 Then
            MsgBox("El pedido " & IIf(p.ojo = "D", "Derecho", "Izquierdo") & " se encuentra en  Salida de Endurecido, Informe a Calidad")
        ElseIf o.Item(o.Count - 1).FeEndurecido <> 0 And o.Item(o.Count - 1).FsEndurecido = 0 Then
            MsgBox("El pedido " & IIf(p.ojo = "D", "Derecho", "Izquierdo") & " se encuentra en  Endurecido, Informe a Tratamientos")
        ElseIf o.Item(o.Count - 1).FeFabrica <> 0 And o.Item(o.Count - 1).FsFabrica <> 0 Then
            MsgBox("El pedido " & IIf(p.ojo = "D", "Derecho", "Izquierdo") & " se encuentra en  Salida de fabrica, Informe a Fabrica")
        ElseIf o.Item(o.Count - 1).FeFabrica <> 0 And o.Item(o.Count - 1).FsFabrica = 0 Then
            MsgBox("El pedido " & IIf(p.ojo = "D", "Derecho", "Izquierdo") & " se encuentra en  Entrada de fabrica, Informe a Fabrica")
        ElseIf o.Item(o.Count - 1).FsAlmacen = 0 Then
            MsgBox("El pedido " & IIf(p.ojo = "D", "Derecho", "Izquierdo") & " se encuentra en Ciclo Inicial, Informe a Fabrica")
        End If
    End Sub
    Public Function CompruebaPrecioNetoEnAcuerdo(ByVal idModelo As Integer, ByVal idModo As Integer, ByVal IdTratamiento As Integer, _
    ByVal idColor As Integer, ByVal diametro As Integer, ByVal idGraduacion As Integer, ByVal idAcuerdo As Integer) As Decimal
        '*****************************************************************************************************
        'busca si la tabal de precios netos de un acuerdo existe el pedido 
        'caso de que aparezca devuelve el precio encontrado 
        'si no lo encuentra devuelve -1
        '*****************************************************************************************************
        Dim cad As String = ""
        Dim i As Integer = 0
        Dim tb As New DataTable
        cad = "Select * from t_lineas_acuerdo where id_grupo =(select id_grupo from t_modelos where id_lente=" & idModelo & ") and id_modo=" & idModo & _
        " and id_tratamiento = " & IdTratamiento & " and (diametro=" & diametro & " or diametro=-1 or diametro=0)  and (id_color = " & idColor & " or id_color=0) and id_acuerdo=" & idAcuerdo & _
        " and (id_graduacion=" & idGraduacion & " or id_graduacion=0 or id_graduacion=-1) ORDER BY id_graduacion DESC "
        ' no se busca ni diametro ni graduacion ya que estos son variables , con lo que despues chequeo si hay los compruebo

        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            Return 0
        Else
            For i = 0 To tb.Rows.Count - 1
                If tb.Rows(i)("diametro") = -1 Or tb.Rows(i)("diametro") = 0 Or tb.Rows(i)("diametro") = diametro Then
                    'hay una fila con diametro general
                    If tb.Rows(i)("id_graduacion") = -1 Or tb.Rows(i)("id_graduacion") = 0 Or tb.Rows(i)("id_graduacion") = idGraduacion Then
                        'tanto el diametro como la graduacion son generales
                        Return tb.Rows(i)("precio")
                    Else
                        Return 0
                    End If
                Else
                    Return 0
                End If
            Next
        End If

    End Function
    Public Function GetIncidenciaByid(ByVal id As Integer) As String

        Dim cad As String = "select incidencia from t_incidencias where id_incidencia=" & id
        Dim Incidencia As String
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Incidencia = cmd.ExecuteScalar
        mcon.Close()
        Return Incidencia

    End Function
    Public Function GetIncidencias() As DataTable
        Dim cad As String = "select * from t_incidencias"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function InfVisitasComercial(ByVal desde As Integer, ByVal hasta As Integer, ByVal comercial As Integer) As DataTable
        ''''lo cambio por la consulta mia para mostrar la población
        '  Dim cad As String = "select (select nombre + ' ' + apellidos from t_usuarios where id_usuario=id_comercial) as comercial,dbo.FechaExcel(fecha) as Fecha,Dbo.Hora(hora) as Hora, Duracion,case when id_contacto<0 then 'CONTACTO' else (select codigo from t_clientes where id_cliente=id_contacto) END as Codigo, " & _
        ' "case when id_contacto<0 THEN (Select nombre from Comercial.DBO.t_contactos where Comercial.dbo.t_contactos.id_contacto=-Comercial.DBO.t_visitas.id_contacto) else (select nombre_comercial from t_clientes where id_cliente=id_contacto) END as nombre, " & _
        '  "case when id_contacto<0 THEN (Select provincia from m_provincias where id_provincia=(select id_provincia from Comercial.DBO.t_contactos where Comercial.dbo.t_contactos.id_contacto=-Comercial.DBO.t_visitas.id_contacto)) else (select provincia from m_provincias where id_provincia=(select id_provincia from t_clientes where id_cliente=id_contacto)) END as provincia" & _
        ' ",notas, convert (nvarchar(200),id_Visita) as [Material entregado] from Comercial.dbo.t_visitas where fecha>=" & desde & " and fecha<=" & hasta & IIf(comercial = 0, "", " and id_comercial=" & comercial) & " order by fecha,hora"

        'consulta mia con población
        Dim cad As String = "select (select nombre + ' ' + apellidos from t_usuarios where id_usuario=id_comercial) as comercial,dbo.FechaExcel(fecha) as Fecha,Dbo.Hora(hora) as Hora, Duracion,case when id_contacto<0 then 'CONTACTO' else (select codigo from t_clientes where id_cliente=id_contacto) END as Codigo, " & _
        "case when id_contacto<0 THEN (Select nombre from Comercial.DBO.t_contactos where Comercial.dbo.t_contactos.id_contacto=-Comercial.DBO.t_visitas.id_contacto) else (select nombre_comercial from t_clientes where id_cliente=id_contacto)  END as nombre, " & _
         "case when id_contacto<0 THEN (Select Localidad from Comercial.DBO.t_contactos where Comercial.dbo.t_contactos.id_contacto=-Comercial.DBO.t_visitas.id_contacto) else (select poblacion from t_clientes where id_cliente=id_contacto)  END as poblacion, " & _
         "case when id_contacto<0 THEN (Select provincia from m_provincias where id_provincia=(select id_provincia from Comercial.DBO.t_contactos where Comercial.dbo.t_contactos.id_contacto=-Comercial.DBO.t_visitas.id_contacto)) else (select provincia from m_provincias where id_provincia=(select id_provincia from t_clientes where id_cliente=id_contacto)) END as provincia" & _
        ",notas, convert (nvarchar(200),id_Visita) as [Material entregado] from Comercial.dbo.t_visitas where fecha>=" & desde & " and fecha<=" & hasta & IIf(comercial = 0, "", " and id_comercial=" & comercial) & " order by fecha,hora"

        'fin consulta con población

        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        'ahora vamos a rellenar los materiales entregados en la visita
        For Each rw As DataRow In tb.Rows
            Dim Material As String = ""
            Dim tbmat As New DataTable
            rw("notas") = Replace(rw("notas"), vbNewLine, ". ")
            mda.SelectCommand.CommandText = " Select material from Comercial.dbo.t_materiales where id_material IN (select id_material from Comercial.dbo.t_materiales_visita where id_visita=" & rw("material entregado") & ")"
            mda.Fill(tbmat)
            For Each r As DataRow In tbmat.Rows
                Material = Material & r("material") & " "
            Next
            rw("material entregado") = Material
        Next
        mcon.Close()
        Return tb
    End Function
    Public Function Getpasos() As DataTable
        Dim cad As String = "select * from t_pasos order by id_paso"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb

    End Function
    Public Function CompruebaPrecioNetoEnOferta(ByVal idModelo As Integer, ByVal idModo As Integer, ByVal IdTratamiento As Integer, _
       ByVal idColor As Integer, ByVal diametro As Integer, ByVal idGraduacion As Integer, ByVal idAcuerdo As Integer) As Decimal
        '*****************************************************************************************************
        'busca si la tabal de precios netos de un acuerdo existe el pedido 
        'caso de que aparezca devuelve el precio encontrado 
        'si no lo encuentra devuelve -1
        '*****************************************************************************************************
        Dim cad As String = ""
        Dim i As Integer = 0
        Dim tb As New DataTable
        cad = "Select * from t_lineas_oferta where id_grupo=(select id_grupo from t_modelos where id_lente = " & idModelo & ") and (id_modo=" & idModo & " or id_modo=-1) " & _
        " and (id_tratamiento = " & IdTratamiento & " or id_tratamiento=-1) and (diametro=" & diametro & " or diametro=-1) and (id_color = " & idColor & " or id_color=-1) and (id_graduacion=" & idGraduacion & " or id_graduacion=-1) and id_oferta=" & idAcuerdo & " order by id_graduacion DESC"
        ' no se busca ni diametro ni graduacion ya que estos son variables , con lo que despues chequeo si hay los compruebo

        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            Return -1
        Else
            For i = 0 To tb.Rows.Count - 1
                If tb.Rows(i)("diametro") = -1 Or tb.Rows(i)("diametro") = diametro Then
                    'hay una fila con diametro general
                    If tb.Rows(i)("id_graduacion") = -1 Or tb.Rows(i)("id_graduacion") = idGraduacion Then
                        'tanto el diametro como la graduacion son generales
                        Return tb.Rows(i)("precio")
                        'Else
                        '    Return -1
                    End If
                    'Else
                    '    Return -1
                End If
            Next
        End If
        Return -1
    End Function
    'Public Function GetPrecioStockProveedor(ByVal Lente As clsLenteStock, ByVal idProveedor As Integer) As Decimal
    '    Dim cad As String = "select isnull(precio,0) from t_precios_stock_proveedor where id_proveedor=" & idProveedor & " and id_tratamiento=" & Lente.id_tratamiento & " and diametro=" & Lente.diametro & " and id_grupo in (select id_grupo from t_modelos where id_lente=" & Lente.id_modelo & ")"
    '    Dim Precio As Decimal
    '    Dim cmd As New SqlCommand(cad, mcon)
    '    mcon.Open()
    '    Precio = cmd.ExecuteScalar
    '    mcon.Close()
    '    Return Precio
    'End Function
    Public Function getGamaColorByCodigoColoracion(ByVal id_coloracion As Integer) As Integer
        'devuelve la gama de colores de precios dependiendo del color pedido
        Dim cad As String = "select id_gama_precio from t_coloraciones where id_coloracion=" & id_coloracion
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("id_gama_precio")

    End Function
    Public Function GetAcuerdoSinNetosByCliente(ByVal idcliente As Integer, ByVal fecha As Integer) As clsAcuerdo
        '******************************************** d
        'devuelve las condiciones del acuerdo sin la lista de netos
        '**************************************************
        Dim cad As String = "select  t_acuerdos.*, t_acuerdos_clientes.* from t_acuerdos inner join " & _
        "t_acuerdos_clientes on t_acuerdos.id_acuerdo= t_acuerdos_clientes.id_acuerdo " & _
        " where t_acuerdos_clientes.id_cliente = " & idcliente & " and t_acuerdos_clientes.desde<=" & fecha & " and t_acuerdos_clientes.hasta>=" & fecha
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        Dim tb As New DataTable
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            Return Nothing
        End If
        'cargo la clase acuerdo
        Dim Acuerdo As New clsAcuerdo
        Acuerdo.ConsumoMensual = tb.Rows(0)("consumo_minimo")
        Acuerdo.descripcion = tb.Rows(0)("descripcion")
        Acuerdo.dto_fabricacion = tb.Rows(0)("dto_fabricacion")
        Acuerdo.dto_Progresivos = tb.Rows(0)("dto_progresivos")
        Acuerdo.dto_stock = tb.Rows(0)("dto_stock")
        Acuerdo.id = tb.Rows(0)("id_acuerdo")
        Acuerdo.nombre = tb.Rows(0)("nombre")
        Acuerdo.NumMinimoProgresivos = tb.Rows(0)("numprogresivos")
        Return Acuerdo
    End Function
    Public Function GetOfertasbyFecha(ByVal Fecha As Long, ByVal idcliente As Integer) As clsOfertasSurcolor
        '******************************************** d
        'devuelve las condiciones del acuerdo sin la lista de netos
        '**************************************************
        Dim cad As String = "select * from t_ofertas where fec_ini<=  " & Fecha & " and fec_fin>=" & Fecha & " and id_oferta not in (select id_oferta from t_clientes_sin_oferta where id_cliente=" & idcliente & ")"
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        Dim tb As New DataTable
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            Return Nothing
        End If
        Dim Mofertas As New clsOfertasSurcolor
        'cargo la clase acuerdo

        Dim mrow As DataRow
        For Each mrow In tb.Rows
            Dim Acuerdo As New clsOfertas
            Acuerdo.Nombre = mrow("nombre")
            Acuerdo.Inicio = mrow("fec_ini")
            Acuerdo.Fin = mrow("fec_fin")
            'Acuerdo.dto_Progresivos = tb.Rows(0)("dto_progresivos")
            'Acuerdo.dto_stock = tb.Rows(0)("dto_stock")
            Acuerdo.id = mrow("id_oferta")
            Mofertas.add(Acuerdo)
            Acuerdo = Nothing
        Next

        Return Mofertas
    End Function
    Public Function GetModelosByidGrupo(ByVal id As Integer) As DataTable
        Dim cad As String = "select * from t_modelos where id_grupo<>0 and id_grupo=" & id
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        ' mcon.Open()
        mda.Fill(tb)
        'mcon.Close()
        Return tb
    End Function
    Public Function GetGrupoModeloByidGrupo(ByVal id As Integer) As DataTable
        Dim cad As String = "select * from t_grupos_modelos where id_grupo=" & id
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        ' mcon.Open()
        mda.Fill(tb)
        'mcon.Close()
        Return tb
    End Function
    Public Function GetOrdenesTrabajoByPedido(ByVal idpedido As Integer) As DataTable
        Dim cad As String = "select * from t_ordenes_trabajo where id_pedido=" & idpedido & " order by id_orden"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetAlbaranesdeAbono(ByVal fecini As Long, ByVal fecfin As Long, ByVal agrupado As Boolean, ByVal idcliente As Long) As DataTable
        Dim cad As String = "", cli As String = ""
        If idcliente <> 0 Then
            cli = " and t_albaranes.id_cliente=" & idcliente
        End If
        If agrupado = False Then
            cad = "(select cast(left(right(fecha,4),2)+'/'+ right(fecha,2)+'/'+left(fecha,4) as nvarchar) as [Fecha Albaran],t_albaranes.id_albaran as [num Albaran],nombre_comercial as cliente,id_alb_abono as [albaran abonado], Tipo_motivo + '-'+ motivo as Causa,t_lineas_albaran.total,descripcion,CASE id_tipo_producto WHEN 1 THEN (select sum(coste) from t_costos_pedido where id_pedido=t_lineas_albaran.id_pedido) ELSE 0 END as Costes    from (((t_albaranes INNER JOIN t_clientes ON t_clientes.id_cliente=t_albaranes.id_cliente) INNER JOIN t_detalle_abono ON t_detalle_abono.id_albaran=t_albaranes.id_albaran) INNER JOIN t_motivos_abono on T_motivos_abono.id_motivo=t_detalle_abono.id_motivo) INNER JOIN t_lineas_albaran on t_albaranes.id_albaran=t_lineas_albaran.id_albaran where fecha>=" & fecini & " and fecha<=" & fecfin & cli & " ) " & _
            " UNION ALL (select cast(left(right(fecha,4),2)+'/'+ right(fecha,2)+'/'+left(fecha,4) as nvarchar) as [Fecha Albaran],t_albaranes.id_albaran as [num Albaran],nombre_comercial as cliente,id_alb_abono as [albaran abonado], Tipo_motivo + '-'+ motivo as Causa,t_lineas_albaran.total,'Albaran de Montaje',0    from (((t_albaranes INNER JOIN t_clientes ON t_clientes.id_cliente=t_albaranes.id_cliente) INNER JOIN t_detalle_abono ON t_detalle_abono.id_albaran_abono=t_albaranes.id_alb_abono) INNER JOIN t_motivos_abono on T_motivos_abono.id_motivo=t_detalle_abono.id_motivo) INNER JOIN t_lineas_albaran on t_albaranes.id_albaran=t_lineas_albaran.id_albaran where fecha>=" & fecini & " and fecha<=" & fecfin & cli & " and id_tipo_producto=4 group by fecha,t_albaranes.id_albaran,nombre_comercial,id_alb_abono,tipo_motivo,motivo,t_lineas_albaran.total,t_albaranes.id_albaran)"
        Else
            cad = "select  Tipo_motivo + '-'+ motivo as Causa,count(*) as albaranes,sum(total) as Importe from (t_albaranes  INNER JOIN t_detalle_abono ON t_detalle_abono.id_albaran_abono=t_albaranes.id_alb_abono) INNER JOIN t_motivos_abono on T_motivos_abono.id_motivo=t_detalle_abono.id_motivo where fecha>=" & fecini & " and fecha<=" & fecfin & cli & " group by tipo_motivo,motivo"
        End If
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mda.SelectCommand.CommandTimeout = 180
        mcon.Open()

        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function TipoGafaMontaje(ByVal idped As Long) As String
        Dim gafa As String = ""
        Dim cad As String = "select gafa from t_tipos_gafa where id_gafa in (select id_tipo_gafa from t_pedidos_montajes where ojo_izq= " & idped & " or ojo_dcho=" & idped & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        gafa = cmd.ExecuteScalar
        mcon.Close()
        Return gafa
    End Function
    Public Function GetDiasPedidos(ByVal idcliente As Integer, ByVal fecini As Long, ByVal fecfin As Long, Optional ByVal modo As String = "%", Optional ByVal agrupado As Boolean = True) As DataTable
        Dim cli As String = ""

        Dim tipo As String = " and modo like '" & modo & "'"
        If idcliente <> 0 Then
            cli = " and t_pedidos.id_cliente=" & idcliente

        End If
        Dim cad As String = "Create view diasPedidos as (select t_pedidos.id_pedido,case when id_coloracion=0 then '' else ' color' end as color,modo,(select nombre from t_tratamientos where id_tratamiento=t_pedidos.id_tratamiento) as tratamiento,cast(left(right(t_portes.fecha,4),2)+'/'+ right(t_portes.fecha,2)+'/'+left(t_portes.fecha,4) as nvarchar)" & _
            " as Salida,cast(left(right(t_pedidos.fecha,4),2)+'/'+ right(t_pedidos.fecha,2)+'/'+left(t_pedidos.fecha,4) as nvarchar)as entrada,convert(decimal(8,2),datediff(d,cast(right(t_pedidos.fecha,2)+'/'+left(right(t_pedidos.fecha,4),2)+'/'+left(t_pedidos.fecha,4)" & _
            " as smalldatetime),cast(right(t_portes.fecha,2)+'/'+left(right(t_portes.fecha,4),2)+'/'+left(t_portes.fecha,4) as smalldatetime))) as dias from ((t_pedidos INNER JOIN t_albaranes ON t_albaranes.id_pedido=t_pedidos.id_pedido) INNER JOIN t_lineas_porte ON t_albaranes.id_albaran=t_lineas_porte.id_albaran) INNER JOIN t_portes On t_lineas_porte.id_porte=t_portes.id_porte where fecha_salida<>0 " & _
            "  and t_pedidos.fecha>=" & fecini & " and t_pedidos.fecha<=" & fecfin & cli & tipo & ")"
        Dim sqlcmd As New SqlCommand(cad, mcon)
        BorraConsulta("diaspedidos")
        mcon.Open()
        sqlcmd.CommandTimeout = 120
        sqlcmd.ExecuteNonQuery()
        'ya tenemos la consulta, ahora hay que ver si actuamos sobre un cliente o sobre todos
        Dim consulta As String
        If agrupado = False Then
            consulta = "select * from diaspedidos order by modo,tratamiento,id_pedido"
        Else
            consulta = "select modo,color,tratamiento,count(*) as lentes,convert(decimal(4,2),avg(dias)) as Promedio from diaspedidos group by modo,color,tratamiento order by modo,tratamiento,color"
        End If
        mcon.Close()
        Dim mda As New SqlDataAdapter(New SqlCommand(consulta, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.SelectCommand.CommandTimeout = 180
        mda.Fill(tb)
        mcon.Close()
        'ahora borramos la consulta
        mcon.Open()
        Dim borra As String = "DROP VIEW diaspedidos"
        Dim Borracmd As New SqlCommand(borra, mcon)
        Borracmd.ExecuteNonQuery()
        mcon.Close()
        Return tb


    End Function
    Public Function getPrecioTarifa(ByVal id As Integer, ByVal idmodo As Integer, ByVal idtratamiento As Integer, _
    ByVal idcolor As Integer, ByVal diametro As Integer, ByVal idgraduacion As Integer, Optional ByVal Modelo As Boolean = True) As Single
        Dim cad As String = ""
        Dim i As Integer = 0
        Dim tb As New DataTable
        If Modelo = True Then
            cad = "Select * from t_precios_grupo where id_grupo=(select id_grupo from t_modelos where id_lente = " & id & ") and id_modo=" & idmodo & _
            " and id_tratamiento = " & idtratamiento & " and (diametro=" & diametro & " or diametro=-1) and id_color = " & idcolor & " and (id_graduacion=" & idgraduacion & " or id_graduacion=-1) ORDER BY id_graduacion DESC"
            ' no se busca ni diametro ni graduacion ya que estos son variables , con lo que despues chequeo si hay los compruebo
        Else
            cad = " Select * from t_precios_grupo where id_grupo=" & id & " and id_modo=" & idmodo & _
            " and id_tratamiento = " & idtratamiento & " and (diametro=" & diametro & " or diametro=-1) and id_color = " & idcolor & " and (id_graduacion=" & idgraduacion & " or id_graduacion=-1) ORDER BY id_graduacion DESC"
        End If
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            Return False
        Else
            Dim precio As Single = 0
            For i = 0 To tb.Rows.Count - 1
                If tb.Rows(i)("diametro") = -1 Or tb.Rows(i)("diametro") = diametro Then
                    'hay una fila con diametro general
                    If tb.Rows(i)("id_graduacion") = -1 Or tb.Rows(i)("id_graduacion") = idgraduacion Then
                        'tanto el diametro como la graduacion son generales
                        precio = tb.Rows(i)("precio")
                    Else
                        ' precio = tb.Rows(i)("precio_base")
                    End If
                Else
                End If
            Next
            If precio <> 0 Then
                Return precio
            End If
            Return -1
        End If

    End Function
    Public Function getPrecioTarifaBypedido(ByVal p As clsPedido) As clsAuxPrecio
        Dim cad As String = ""
        Dim i As Integer = 0
        Dim tb As New DataTable
        Dim IdTratamiento As Integer = p.id_tratamiento
        If IdTratamiento = 5 Then IdTratamiento = 2
        If IdTratamiento = 6 Then IdTratamiento = 3
        Dim IdModo As Integer = 3 'por defecto fabricacion
        If p.modo = "S" Then IdModo = 1
        If p.modo = "T" Then IdModo = 2
        Dim conAbierta As Boolean = False
        Dim Pre As New clsAuxPrecio
        'If Modelo = True Then
        Select Case FechaAcadena(p.Fechapedido) ' si el pedido es anterior al 1/01/2016 seguimos el metodo antiguo
            Case Is < 20160101
                cad = "Select isnull(precio,0) from t_precios_grupo where id_grupo=(select id_grupo from t_modelos where id_lente = " & p.id_modelo & ") and id_modo=" & IdModo & _
        " and id_tratamiento = " & IdTratamiento & " and (diametro=" & p.diametro & " or diametro=-1) and id_color = 0 and (id_graduacion=" & p.diametro & " or id_graduacion=-1) ORDER BY id_graduacion DESC"
            Case Else 'sistema nuevo de precios
                Select Case p.modo
                    Case Is = "F" 'buscamos el precio en las lentes de fabricacion
                        cad = "select isnull(precio+(select precio from t_tratamientos where id_tratamiento=" & p.id_tratamiento & ") ,0) from t_PVP_lentes_fabricacion where id_grupo in (select id_grupo from t_modelos where id_lente=" & p.id_modelo & ")"
                    Case Is = "S" 'lo buscamos en el precio de stock para ese tratamiento
                        cad = "select top 1 isnull(precio,0) from t_PVP_lentes_stock where id_tratamiento=" & IdTratamiento & " and (diametro=0 or diametro=" & p.diametro & ") and id_grupo in (select id_grupo from t_modelos where id_lente=" & p.id_modelo & ") order by diametro desc"
                    Case Is = "T" 'buscamos el precio de la lente de stock en blanco + el precio del tratamiento
                        cad = "select top 1 isnull(precio+(select precio from t_tratamientos where id_tratamiento=" & p.id_tratamiento & "),0) from t_PVP_lentes_stock where id_tratamiento=0 and (diametro=0 or diametro=" & p.diametro & ") and id_grupo in (select id_grupo from t_modelos where id_lente=" & p.id_modelo & ") order by diametro desc"

                End Select

        End Select
        ' no se busca ni diametro ni graduacion ya que estos son variables , con lo que despues chequeo si hay los compruebo
        If mcon.State = ConnectionState.Open Then
            conAbierta = True
        Else
            mcon.Open()
        End If
        Dim mda As New SqlCommand(cad, mcon)
        Pre.Precio = mda.ExecuteScalar
        If conAbierta = False Then
            mcon.Close()
        End If
        Return Pre

    End Function


    Public Function getPrecioModeloByCliente(ByVal idCliente As Integer, ByVal idmodelo As Integer, ByVal idmodo As Integer, ByVal idtratamiento As Integer, _
    ByVal idcolor As Integer, ByVal diametro As Integer, ByVal cilindro As Decimal, ByVal fecha As Integer) As Single
        Dim cad As String = ""
        Dim i As Integer = 0
        Dim tb As New DataTable
        cad = "Select isnull(precio,0),* from t_precios_cliente where id_grupo=(select id_grupo from t_modelos where id_lente= " & idmodelo & ") and id_modo=" & idmodo & _
        " and id_tratamiento = " & idtratamiento & "  and (diametro<=" & diametro & " or diametro=0) and id_color = " & idcolor & _
        " and cilindro<=" & NumSql(cilindro) & " and id_cliente=" & idCliente & " and desde<=" & fecha & _
        " and hasta>=" & fecha & " ORDER BY desde DESC, diametro desc"
        ' no se busca ni diametro ni graduacion ya que estos son variables , con lo que despues chequeo si hay los compruebo

        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            Return 0
        Else
            Dim precio As Single = 0

            ' If tb.Rows(i)("diametro") = 0 Or tb.Rows(i)("diametro") = diametro Then
            'hay una fila con diametro general
            'If tb.Rows(i)("id_graduacion") = -1 Or tb.Rows(i)("id_graduacion") = idgraduacion Then
            'tanto el diametro como la graduacion son generales
            precio = tb.Rows(0)("precio")


            '    End If

            If precio <> 0 Then
                Return precio
            End If
            Return 0
        End If

    End Function
    Public Function getPrecioEspecialByPedido(ByVal p As clsPedido, ByVal fecha As Integer, Optional ByVal Color As Boolean = True) As Decimal
        Dim cad As String = ""
        Dim tb As New DataTable
        Dim d As New clsDatos
        Dim IdModo As Integer = 3 ' por defecto lo ponemos de fabricacion
        If p.modo = "S" Then
            IdModo = 1
        ElseIf p.modo = "T" Then
            IdModo = 2
        End If
        Dim ConAbierta As Boolean = False
        Dim idColor As String = d.GetGamaColorByID(p.id_coloracion)
        Dim Precio As Decimal = 0

        If Color = False Then 'chequeamos la lente en blanco
            idColor = 0
        End If
        Dim tratamiento As Integer = p.id_tratamiento
        '*****hasta el dia 1 de enero del 2016, despues no debe pasar
        If FechaAcadena(p.Fechapedido) < 20160101 Then
            If tratamiento = 4 Then tratamiento = 2 'el arCC mismo precio que ar
            If tratamiento = 5 Then tratamiento = 3

        End If
        cad = "Select isnull(precio,0) from t_precios_cliente where id_grupo=(select id_grupo from t_modelos where id_lente= " & p.id_modelo & ") and id_modo=" & IdModo & _
        " and id_tratamiento = " & tratamiento & "  and diametro<=" & p.diametro & " and id_color = " & idColor & _
        " and cilindro<=" & NumSql(p.cilindro) & " and id_cliente=" & p.id_cliente & " and desde<=" & fecha & _
        " and hasta>=" & fecha & " ORDER BY desde DESC, diametro desc"
        ' no se busca ni diametro ni graduacion ya que estos son variables , con lo que despues chequeo si hay los compruebo

        Dim mda As New SqlCommand(cad, mcon)
        If mcon.State = ConnectionState.Open Then
            ConAbierta = True
        Else
            mcon.Open()
        End If
        Precio = mda.ExecuteScalar
        If ConAbierta = False Then
            mcon.Close()
        End If
        Return Precio

    End Function
    Public Function GetPromocionesByPedido(ByVal p As clsPedido) As ArrayList
        'aqui cargamos todas las promociones posibles de un pedido que contengan dicho modelo
        Dim Fecha As Integer = FechaAcadena(p.Fechapedido)
        Dim grupo As Integer = getClientebyId(p.id_cliente).GrupoOptico
        Dim cad As String = "select * from t_promociones  where inicio<=" & Fecha & " and fin>=" & Fecha & " AND (ID_GRUPO=0 or id_grupo=" & grupo & ") and " & p.id_cliente & " not in (select id_cliente from t_clientes_sin_promocion where id_promocion=t_promociones.id_promocion)"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim lista As New ArrayList
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows
            Dim pro As New clsPromocion
            pro = GetPromocion(rw("id_promocion"))
            lista.Add(pro)
        Next
        Return lista
    End Function
    Public Function GetPrecioMontura(ByVal idMontura As Integer) As Decimal
        Dim precio As Decimal = 0
        Dim cad As String = "select precio from t_monturas where id_montura=" & idMontura
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        precio = cmd.ExecuteScalar
        mcon.Close()
        cmd.Dispose()
        Return precio
    End Function
    Public Function GetModoPedido(ByVal idpedido As Integer) As String
        Dim modo As String = ""
        Dim cad As String = "select modo from t_pedidos where id_pedido=" & idpedido
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        modo = cmd.ExecuteScalar
        mcon.Close()
        cmd.Dispose()
        Return modo
    End Function
    Public Function EsProgresivoPedido(ByVal idPedido As Integer) As Boolean
        Dim cad As String = "select tipologia from t_modelos where id_lente in (select id_modelo from t_pedidos where id_pedido=" & idPedido & ")"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows(0)("tipologia") = 3 Then
            Return True
        End If
        Return False
    End Function
    Public Function Actualizador() As String
        Dim Cad As String = "Select actualizaciones from t_valores_globales"
        Dim cmd As New SqlCommand(Cad, mcon)
        mcon.Open()
        Actualizador = cmd.ExecuteScalar

        Actualizador = Actualizador & "\Loalens"
        mcon.Close()

    End Function
    Public Function ActualizadorAntiguo() As String
        Dim Cad As String = "select actualizacion_loa from t_valores_globales"
        Dim cmd As New SqlCommand(Cad, mcon)
        mcon.Open()
        ActualizadorAntiguo = cmd.ExecuteScalar
        mcon.Close()
    End Function
    Public Function EsProgresivo(ByVal idmodelo As Integer) As Boolean
        Dim cad As String = "select tipologia from t_modelos where id_lente=" & idmodelo
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows(0)("tipologia") = 3 Then
            Return True
        End If
        Return False
    End Function
    Public Function EsBifocal(ByVal idmodelo As Integer) As Boolean
        Dim cad As String = "select tipologia from t_modelos where id_lente=" & idmodelo
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows(0)("tipologia") = 2 Or tb.Rows(0)("Tipologia") = 4 Then
            Return True
        End If
        Return False
    End Function
    Public Function Esregresivo(ByVal idmodelo As Integer) As Boolean
        Dim cad As String = "select tipologia from t_modelos where id_lente=" & idmodelo
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows(0)("tipologia") = 5 Then
            Return True
        End If
        Return False
    End Function
    Public Function GetPrecioSuplemento(ByVal id_suplemento As Integer) As Decimal
        Dim cad As String = "select * from t_suplementos where id_suplemento= " & id_suplemento
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("precio_base")
    End Function
    Public Function getClientesSinOferta(ByVal idAcuerdo As Integer) As DataTable
        'devuelve lost_clientes que tienen un acuerdo específico , como solo es para mostrar la lista
        'sólo cargo los datos de id, codigo y nombre comercial
        Dim cad As String = ""
        cad = "select id_cliente,codigo,nombre_comercial from t_clientes where id_cliente in (select id_cliente from t_clientes_sin_oferta where id_oferta=" & idAcuerdo & ")"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function getClientesByAcuerdo(ByVal idAcuerdo As Integer) As DataTable
        'devuelve lost_clientes que tienen un acuerdo específico , como solo es para mostrar la lista
        'sólo cargo los datos de id, codigo y nombre comercial
        Dim cad As String = ""
        cad = "select T_clientes.id_cliente,codigo,nombre_comercial from t_acuerdos_clientes inner join " & _
        "t_clientes on t_acuerdos_clientes.id_cliente =t_clientes.id_cliente where t_acuerdos_clientes.id_acuerdo = " & idAcuerdo
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()


        Return tb
    End Function
    Public Sub AnulaSalidaFabrica(ByVal idPedido As Long)
        Dim cad As String = "update t_pedidos set f_salida_fabrica=0,h_salida_fabrica ='0', " & _
        "f_salida_tratamiento=0, h_salida_tratamiento='0',fecha_salida=0,hora_salida='0' where id_pedido = " & idPedido
        Dim da As New SqlDataAdapter
        mcon.Open()
        da.UpdateCommand = New SqlCommand(cad, mcon)
        da.UpdateCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub AnulaSalidaTratamiento(ByVal idPedido As Long)
        Dim cad As String = "update t_pedidos set f_salida_tratamiento=0, h_salida_tratamiento='0',fecha_salida=0,hora_salida='0' where id_pedido = " & idPedido
        Dim da As New SqlDataAdapter
        mcon.Open()
        da.UpdateCommand = New SqlCommand(cad, mcon)
        da.UpdateCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub AnulaSalidadistribucion(ByVal idPedido As Long)
        Dim cad As String = "update t_pedidos set fecha_salida=0,hora_salida='0' where id_pedido = " & idPedido
        Dim da As New SqlDataAdapter
        mcon.Open()
        da.UpdateCommand = New SqlCommand(cad, mcon)
        da.UpdateCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub EliminaAcuerdo(ByVal idAcuerdo As Long)
        Dim mda As New SqlDataAdapter
        mda.DeleteCommand = New SqlCommand("delete from t_acuerdos where id_acuerdo=" & idAcuerdo)
        mda.DeleteCommand.Connection = mcon
        mcon.Open()
        mda.DeleteCommand.ExecuteNonQuery()
        mda.DeleteCommand.CommandText = "delete from t_lineas_acuerdo where id_acuerdo=" & idAcuerdo
        mda.DeleteCommand.ExecuteNonQuery()
        mda.DeleteCommand.CommandText = "delete from t_acuerdos_clientes where id_acuerdo=" & idAcuerdo
        mda.DeleteCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub GrabaColorMonitor(ByVal Proceso As String, ByVal Paso As Integer, ByVal color As Integer)
        Dim cad As String = "DELETE FROM t_colores_monitor where proceso=" & strsql(Proceso) & " and paso=" & Paso & vbNewLine & _
        "INSERT INTO t_colores_monitor (proceso,paso,color) VALUES (" & strsql(Proceso) & "," & Paso & "," & color & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub GrabaLoteSemiterminado(ByVal lote As clsLoteSemiterminado)
        Dim cad As String = "DECLARE @cuenta integer " & vbNewLine & _
        "DECLARE @idLote as integer" & vbNewLine & _
        "SET @idLote=" & lote.IdLote & vbNewLine & _
        "Select @cuenta=count(*) from t_lotes_semiterminado where id_semiterminado=" & lote.Lente & " and lote like " & strsql(lote.Lote) & vbNewLine & _
        "IF @cuenta=1 " & vbNewLine & _
        "   BEGIN " & vbNewLine & _
        "       Update t_lotes_semiterminado set cantidad=cantidad+" & lote.Cantidad & " where id_semiterminado=" & lote.Lente & " and lote like " & strsql(lote.Lote) & vbNewLine & _
        "  Select @idlote=id_lote from  t_lotes_semiterminado where id_semiterminado=" & lote.Lente & " and lote like " & strsql(lote.Lote) & vbNewLine & _
"END " & vbNewLine & _
        "IF @cuenta=0 " & vbNewLine & _
        "   BEGIN " & vbNewLine & _
        "       INSERT INTO t_lotes_semiterminado SELECT max(id_lote)+1," & strsql(lote.Lote) & ",0," & lote.Lente & "," & lote.IdProveedor & "," & lote.Cantidad & "," & lote.Pedido & ",0,0 from t_lotes_semiterminado" & vbNewLine & _
        "       select @idlote= max(id_lote) from t_lotes_semiterminado" & vbNewLine & _
        "   END " & vbNewLine & _
        "--AHORA INSERTAMOS LA ETIQUETA" & vbNewLine & _
        " INSERT INTO t_etiquetas_lote_semiterminado select @idLote," & lote.Lente & "," & strsql(lote.Lote) & "," & lote.Cantidad

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetLoteSemiterminadoByid(ByVal id As Integer) As DataTable
        Dim cad As String = "select * from t_lotes_semiterminado where id_lote=" & id
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Sub GrabaSalidaLoteAntireflejo(ByVal lote As Long)
        Dim cad As String = "select * from t_lote_antireflejo where lote=" & lote

        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        If tb.Rows.Count > 0 Then
            mcon.Open()
            For Each rw As DataRow In tb.Rows
                Dim cmd As New SqlCommand("UPDATE t_ordenes_trabajo set fs_antireflejo=" & FechaAcadena(Now.Date) & ",hs_antireflejo=" & Now.Hour & Format(Now.Minute, "00") & ",usr_s_antireflejo=" & mUsuario.id & ",fs_tratamiento=fs_antireflejo,hs_tratamiento=hs_antireflejo where id_pedido=" & rw("id_pedido") & " and id_orden=" & rw("id_orden"), mcon)
                cmd.ExecuteNonQuery()
            Next
            mcon.Close()
        End If

        'mcon.Open()
        'For Each i As Integer In pedidos
        '    cmd.CommandText = "INSERT INTO t_lote_antireflejo select TOP 1 " & lote & "," & i & ", id_orden from t_ordenes_trabajo where id_pedido=" & i & " order by id_orden desc"
        '    cmd.ExecuteNonQuery()
        'Next

        mcon.Close()
    End Sub
    Public Sub GrabaFrontofocometro(ByVal pedido As Integer, ByVal cyl As Decimal, ByVal sph As Decimal, ByVal ax As Integer, ByVal add As Decimal, ByVal prisma As Decimal, ByVal Ejeprisma As Decimal, ByVal visual As String, ByVal Revision As Integer)
        Dim cad As String = "DELETE FROM t_salida_calidad where id_pedido=" & pedido & vbNewLine & _
        "INSERT INTO t_salida_calidad (id_pedido,sph,cyl,eje,adicion,prisma,prisma_eje,visual,revision) VALUES (" & pedido & "," & NumSql(sph) & "," & NumSql(cyl) & "," & ax & "," & NumSql(add) & "," & NumSql(prisma) & "," & NumSql(Ejeprisma) & "," & strsql(visual) & "," & Revision & ")"
        If InStr(UCase(visual), "OK") = 0 Then
            Dim id As Integer = getMaxId("id_anotacion", "t_anotaciones_pedidos") + 1
            cad = cad & vbNewLine & "insert into t_anotaciones_pedidos (id_pedido,id_anotacion,id_usuario,fecha,comentario,hora) values " & _
        "(" & pedido & "," & id & "," & mUsuario.id & "," & FechaAcadena(Now.Date) & "," & strsql("CALIDAD: " & visual) & "," & Now.Minute & Format(Now.Second, "00") & ")"
        End If

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub



    Public Function SalidaCalidad(ByVal p As clsPedido) As Boolean
        'cuando un pedido que no lleva montaje hace salida de calidad y no lleva pareja debe hacer salida de distribucion en caso de que la pareja tambien haya salido de calidad
        Try
            Dim cad As String = "DECLARE @montaje bit " & vbNewLine & _
            "select @montaje=0 from t_pedidos where id_pedido=" & p.id & vbNewLine & _
            "UPDATE t_ordenes_trabajo set fs_calidad=" & FechaAcadena(Now.Date) & ",hs_calidad=" & Now.Hour & Format(Now.Minute, "00") & ",usr_s_calidad=" & mUsuario.id & " where id_pedido=" & p.id & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & p.id & ")" & vbNewLine & _
            "-- Esto solo pasara cuando lleva montaje" & vbNewLine & _
            "if @montaje=0 " & vbNewLine & _
            "BEGIN" & vbNewLine & _
            "--Si no lleva pareja hacemos la salida de distribucion" & vbNewLine & _
            "if " & p.pareja & "=0" & vbNewLine & _
            "   UPDATE t_pedidos set fecha_salida=" & FechaAcadena(Now.Date) & ",hora_salida=" & Format(Now.Hour, "00") & Format(Now.Minute, "00") & " where id_pedido=" & p.id & vbNewLine & _
            "--SI LLEVA PAREJA BUSCAMOS SI LA PAREJA TIENE SALIDA DE CALIDAD" & vbNewLine & _
            "if " & p.pareja & "<>0" & vbNewLine & _
            "   BEGIN " & vbNewLine & _
            "       DECLARE @Salida integer" & vbNewLine & _
            "       Select TOP 1 @Salida=fs_calidad from t_ordenes_trabajo where id_pedido=" & p.pareja & " order by id_orden desc" & vbNewLine & _
            "       if @salida<>0" & vbNewLine & _
            "           UPDATE t_pedidos set fecha_salida=" & FechaAcadena(Now.Date) & ",hora_salida=" & Format(Now.Hour, "00") & Format(Now.Minute, "00") & " where id_pedido in (" & p.id & "," & p.pareja & ")" & vbNewLine & _
            "   END " & vbNewLine & _
            "END"
            Dim cmd As New SqlCommand(cad, mcon)
            mcon.Open()
            cmd.ExecuteNonQuery()
            mcon.Close()
            Return True
        Catch
            If mcon.State = ConnectionState.Open Then
                mcon.Close()
            End If
        End Try
        Return False

    End Function

    Public Sub GrabaLoteAntireflejo(ByVal lote As Long, ByVal pedidos As ArrayList)
        Dim cad As String = ""

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        For Each i As Integer In pedidos
            cmd.CommandText = "INSERT INTO t_lote_antireflejo select TOP 1 " & lote & "," & i & ", id_orden,0 from t_ordenes_trabajo where id_pedido=" & i & " order by id_orden desc"
            cmd.ExecuteNonQuery()
        Next

        mcon.Close()
    End Sub
    Public Sub GrabaLoteEndurecido(ByVal lote As Long, ByVal pedidos As ArrayList)
        Dim cad As String = ""

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        For Each i As Integer In pedidos
            cmd.CommandText = "INSERT INTO t_lote_endurecido select TOP 1 " & lote & "," & i & ", id_orden,0 from t_ordenes_trabajo where id_pedido=" & i & " order by id_orden desc"
            cmd.ExecuteNonQuery()
        Next

        mcon.Close()
    End Sub
    Public Sub GrabaTrackingTratamiento(ByVal maquina As Long, ByVal pedidos As ArrayList)
        Dim cad As String = ""

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        For Each i As Integer In pedidos
            cmd.CommandText = "INSERT INTO t_tracking_maquina select " & maquina & "," & i & ", max(id_orden)," & ConvertDateToTimeStamp(Now) & " from t_ordenes_trabajo where id_pedido=" & i
            cmd.ExecuteNonQuery()
        Next

        mcon.Close()
    End Sub
    Public Function GetLoteAntireflejoByPedido(ByVal id As Integer) As Long
        Dim cad As String = "select top 1 isnull(lote,0)  from t_lote_antireflejo where id_pedido=" & id & " order by id_orden desc"
        Dim lote As Long = 0
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        lote = cmd.ExecuteScalar
        mcon.Close()
        Return lote
    End Function
    Public Function ChequeaMenuByusuario(ByVal Menu As String) As Boolean
        Dim cad As String = "Select count(*) from t_menus_usuario where id_usuario=" & mUsuario.id & " and id_menu in (select id_menu from t_menus where menu like " & strsql(Menu) & ")"
        Dim existe As Boolean
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        existe = cmd.ExecuteScalar
        mcon.Close()
        Return existe
    End Function
    Public Function GetLoteEndurecidoByPedido(ByVal id As Integer) As Long
        Dim cad As String = "select top 1 isnull(lote,0)  from t_lote_endurecido where id_pedido=" & id & " order by id_orden desc"
        Dim lote As Long = 0
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        lote = cmd.ExecuteScalar
        mcon.Close()
        Return lote
    End Function
    Public Function GetEtiquetasLoteSemiterminado() As ArrayList
        Dim lista As New ArrayList
        Dim cad As String = "select *,(select nombre from t_modelos where id_lente=id_modelo) as modelo,(select indice_modelo from t_modelos where id_lente=id_modelo) as indice from t_semiterminados INNER JOIN t_etiquetas_lote_semiterminado ON id_semiterminado=id_lente"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows
            Dim l As New clsLoteSemiterminado
            l.Lente = rw("id_semiterminado")
            l.Indice = rw("indice")
            l.Lote = rw("lote")
            l.Semiterminado = rw("id_semiterminado")
            l.Cantidad = rw("cantidad")
            l.Base = rw("base")
            l.IdLote = rw("id_lote")
            l.Diametro = rw("diametro")
            lista.Add(l)
        Next
        Return lista
    End Function
    Public Sub BorraEtiquetasLoteSemiterminados()
        Dim cmd As New SqlCommand("DELETE FROM t_etiquetas_lote_semiterminado", mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetColorMonitor(ByVal Proceso As String, ByVal Paso As Integer) As Color
        Dim cad As String = "select isnull(color,0) FROM t_colores_monitor where proceso like " & strsql(Proceso) & " and paso=" & Paso

        Dim Color As Color
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Color = Color.FromArgb(cmd.ExecuteScalar)
        mcon.Close()
        Return Color
    End Function
    Public Function GrabaAlbaran(ByVal a As clsAlbaran, Optional ByVal deuda As Boolean = False) As Long

        'graba el albaran y devuelve el id albaran
        BloqueaAlbaranes()
        If mUsuario.id <> UsuarioBloqueoAlbaranes() Then
            Dim frm As New frmSemaforo
            frm.ShowDialog()

        End If
        DesloqueaAlbaranes()
        Dim id As Long = getMaxId("id_albaran", "t_albaranes") + 1
        Dim mda As New SqlDataAdapter
        Dim cad As String
        Dim TotalContrareembolso As Decimal = 0
        mcon.Open()
        If a.fecha = 0 Then
            a.fecha = FechaAcadena(Today)
        End If
        If a.Id_albaran = 0 Then
            a.Id_albaran = id
            cad = "insert into t_albaranes values(" & id & "," & a.Cliente.id & "," & _
            a.fecha & "," & id & "," & IIf(a.facturado = True, 1, 0) & "," & strsql(a.referencia) & "," & _
            CByte(a.abono) & "," & a.id_alb_abono & "," & a.id_pedido & "," & Replace(a.Total, ",", ".") & "," & a.CodEnvio & ",0," & IIf(a.Montaje = False, 0, 1) & "," & mUsuario.id & " )"

            mda.InsertCommand = New SqlCommand(cad, mcon)
            mda.InsertCommand.ExecuteNonQuery()
            'ahora inserto las lineas de albaran
            Dim lin As New clsAlbaranLinea
            Dim Miva As Decimal
            Dim mRe As Decimal
            Dim Iva As Decimal
            Dim re As Decimal = 0
            Dim SubTotal As Decimal = 0
            For Each lin In a
                Dim b As New clsBaseImponible
                If lin.id_tipo_Producto < 4 Then

                    If lin.id_tipo_Producto = 0 Then 'se trata de un porte

                        b = GetIvaByTipo(1)
                        b.TipoBase = 0
                        b.Re = 0
                    ElseIf lin.id_tipo_Producto < 5 Then
                        b = GetIvaByTipo(2)
                        b.TipoBase = 1
                        'b.BaseI = lin.precio * (1 - lin.dto / 100)
                    End If


                ElseIf (lin.id_tipo_Producto = 4 And lin.c1 = 0) Then
                    b = GetIvaByTipo(2)
                    b.TipoBase = 2

                ElseIf (lin.id_tipo_Producto = 4 And lin.c1 <> 0) Then
                    b = GetIvaByTipo(1)
                    b.TipoBase = 4

                ElseIf lin.id_tipo_Producto = 5 Then
                    b = GetIvaByTipo(1)
                    b.TipoBase = 5
                End If
                b.BaseI = Format(lin.precio * (1 - lin.dto / 100), "0.00")
                If a.Cliente.SinIva = True Then
                    b.Iva = 0
                End If
                If a.Cliente.recargo = False Or lin.Re = 0 Then
                    b.Re = 0
                End If
                Miva = b.Iva
                mRe = b.Re
                'ahora vamos añadiendo la base si no la encontramos o la sumamos si la encontramos
                Dim Encontrada As Boolean = False
                For Each base As clsBaseImponible In a.Bases
                    If b.TipoBase = base.TipoBase And b.Re = base.Re Then
                        Encontrada = True
                        base.BaseI += b.BaseI
                        Exit For
                    End If
                Next
                If Encontrada = False Then
                    a.Bases.add(b)
                End If
                ' If a.Cliente.contrareembolso = True Then

                'aqui calculamos el total de contrareembolso que vamos a grabar 
                SubTotal = Format(lin.precio * (1 - lin.dto / 100), "0.00")
                re = SubTotal * (mRe / 100)
                Iva = SubTotal * (Miva / 100)

                TotalContrareembolso = TotalContrareembolso + SubTotal + Iva + re
                'End If

                cad = "insert into t_lineas_albaran values(" & _
                id & "," & lin.id_tipo_Producto & "," & lin.id_modelo & "," & lin.id_tratamiento & _
                "," & lin.id_coloracion & "," & lin.id_modo & "," & lin.c1 & ",'" & lin.c2 & "'," & _
                strsql(lin.descripcion) & "," & Replace(lin.precio, ",", ".") & "," & Replace(lin.dto, ",", ".") & "," & _
                Replace(lin.total, ",", ".") & "," & lin.idpedido & "," & IIf(lin.Montaje = True, 1, 0) & _
                "," & NumSql(Miva) & "," & NumSql(mRe) & ")"
                mda.InsertCommand.CommandText = cad
                mda.InsertCommand.ExecuteNonQuery()

            Next
        Else
            'solo se permite la variacion de las lineas de pedido
        End If
        If a.Cliente.contrareembolso = True Then
            If Not (a.Total < 0 And a.Cliente.DeudaPendiente.Pendiente > 0) Then ' si se trata de un abono de un cliente con deuda no se crea el contrareembolso
                cad = "Insert into t_contrareembolsos (id_albaran,id_cliente,total,pagado) VALUES (" & a.Id_albaran & "," & a.Cliente.id & "," & NumSql(TotalContrareembolso) & ",0)"
                mda.InsertCommand.CommandText = cad
                mda.InsertCommand.ExecuteNonQuery()
            End If
        End If
        'ahora grabamos en la tabla del debe/haber el total del albaran

        Dim Pendiente As Decimal = 0
        Pendiente = TotalContrareembolso
        Pendiente = Format(Pendiente, "0.00")
        'cad = "Insert into t_debe_haber (id_cliente,fecha,concepto,debe,haber) VALUES (" & a.Cliente.id & "," & a.fecha & ",'albaran " & a.Id_albaran & "'," & NumSql(Pendiente) & ",0)"
        'mda.InsertCommand.CommandText = cad
        'mda.InsertCommand.ExecuteNonQuery()

        'ahora tenemos que grabar las bases del albaran
        For Each base As clsBaseImponible In a.Bases
            cad = "INSERT INTO t_bases_albaran (id_albaran,base,iva,re,id_tipo_base) VALUES (" & a.Id_albaran & "," & NumSql(base.BaseI) & "," & NumSql(base.Iva) & "," & NumSql(base.Re) & "," & NumSql(base.TipoBase) & ")"
            mda.InsertCommand.CommandText = cad
            mda.InsertCommand.ExecuteNonQuery()
        Next
        'vamos a grabar el coste del sobre en cada pedido

        '   mcon.Close()
        If a.id_alb_abono = 0 Then 'si se trata de un albaran de abono no se le añade el gasto, y hay que abonar los puntos
            Dim tb As New DataTable
            Dim md As New SqlDataAdapter("select distinct(id_pedido) as pedido from t_lineas_albaran where id_albaran=" & a.Id_albaran & " and montaje=0", mcon)
            md.Fill(tb)
            For Each rw As DataRow In tb.Rows
                Dim tbCoste As New DataTable
                tbCoste = GetCostes()
                GrabaCostePedido(rw("pedido"), "Sobre", tbCoste.Rows(0)("sobre"))

            Next
        End If
        If a.Puntos <> 0 Then
            cad = "UPDATE t_clientes set puntos=puntos+" & a.Puntos & " where id_cliente=" & a.Cliente.id
            Dim cmd As New SqlCommand(cad, mcon)
            Dim abierta As Boolean = False
            If mcon.State = ConnectionState.Open Then
                abierta = True
            Else
                mcon.Open()
            End If
            cmd.ExecuteNonQuery()
            If a.id_alb_abono <> 0 Then
                cmd.CommandText = "INSERT INTO t_puntos_pedido select id_pedido,-sum(puntos) from t_puntos_pedido where id_pedido in (select id_pedido from t_lineas_albaran where id_tipo_producto=1 and id_albaran=" & a.Id_albaran & ") group by id_pedido"
                cmd.ExecuteNonQuery()
            End If
            If abierta = True Then
                mcon.Close()
            End If
        End If
        If mcon.State = ConnectionState.Open Then
            mcon.Close()
        End If

        Return a.Id_albaran
    End Function

    Public Function GetloginByid(ByVal id As Integer) As String
        Dim cad As String = "Select login from t_clientes_web where id_cliente=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        GetloginByid = cmd.ExecuteScalar
        mcon.Close()
    End Function

    Public Sub AbreSesion()
        Dim cad As String = "INSERT INTO t_sesiones (equipo,id_usuario) VALUES (" & strsql(Equipo) & "," & mUsuario.id & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function SalidaAlmacenStock(ByVal p As clsPedido, ByVal l As clsLenteStock) As Boolean
        'aqui vamos a meter todo lo que hace falta cuando sacas una lente del almacen de stock
        'primero vemos el precio de coste de esa lente de stock y se lo asignamos al pedido
        Dim cad As String = "INSERT INTO t_costos_pedido (id_pedido,paso,coste) VALUES (" & p.id & ",'STOCK'," & NumSql(l.PrecioCompra) & ")"

        'ahora debemos hacer la salida de almacen de la lente y del pedido
        Dim calidad As Boolean = True
        If p.id_coloracion <> 0 Or p.id_tratamiento <> l.id_tratamiento Or p.esmerilado = 1 Then
            calidad = False
        End If
        If calidad = False Then ' solo sale de almacen
            cad = cad & vbNewLine & " UPDATE t_ordenes_trabajo set fs_almacen=" & FechaAcadena(Now.Date) & ",hs_almacen=" & Format(Now.Hour, "00") & Format(Now.Minute, "00") & _
            ",usr_s_almacen =" & mUsuario.id & " where id_pedido=" & p.id & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & p.id & ")"

        Else 'sale de almacen y de calidad
            cad = cad & vbNewLine & " UPDATE t_ordenes_trabajo set fs_almacen=" & FechaAcadena(Now.Date) & ",hs_almacen=" & Format(Now.Hour, "00") & Format(Now.Minute, "00") & _
                        ",usr_s_almacen =" & mUsuario.id & ",fs_calidad=" & FechaAcadena(Now.Date) & ",hs_calidad=" & Format(Now.Hour, "00") & Format(Now.Minute, "00") & ",usr_s_calidad=" & mUsuario.id & " where id_pedido=" & p.id & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & p.id & ")"

        End If
        'tenemos que ver si lleva montaje y si ha hecho la salida de calidad su pareja(en caso de que no tenga montaje)
        If Not (p.LlevaMontaje = True Or p.Montaje = True) Then
            'vamos a ver si va sin pareja o va con pareja y esta tiene salida de calidad, en ese caso hacemos la distribucion
            Dim distribucion As Boolean = True
            If p.pareja <> 0 Then
                Dim o As clsOrdenesTrabajo = GetOrdenesTrabajo(p.pareja)
                If o.Item(o.Count - 1).FsCalidad = 0 Then
                    distribucion = False
                End If
            End If
            If distribucion = True Then
                cad = cad & vbNewLine & ""
            End If
        End If


        Dim cmd As New SqlCommand(cad, mcon)

    End Function

    Public Sub CierraSesion()
        Dim cad As String = "DELETE FROM t_sesiones where equipo=" & strsql(Equipo) & " and id_usuario=" & mUsuario.id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GrabaAlbaranBono(ByVal a As clsAlbaran) As clsAlbaran

        'graba el albaran del bono, inserta las lineas en la factura y devuelve el albaran para imprimirlo
        BloqueaAlbaranes()
        If mUsuario.id <> UsuarioBloqueoAlbaranes() Then
            Dim frm As New frmSemaforo
            frm.ShowDialog()

        End If
        DesloqueaAlbaranes()
        Dim id As Long = getMaxId("id_albaran", "t_albaranes") + 1
        Dim mda As New SqlDataAdapter
        Dim cad As String

        mcon.Open()
        If a.fecha = 0 Then
            a.fecha = FechaAcadena(Today)
        End If
        If a.Id_albaran = 0 Then
            a.Id_albaran = id
            cad = "insert into t_albaranes values(" & id & "," & a.Cliente.id & "," & _
            a.fecha & "," & id & "," & IIf(a.facturado = True, 1, 0) & ",'" & a.referencia & "'," & _
            CByte(a.abono) & "," & a.id_alb_abono & "," & a.id_pedido & "," & Replace(a.Total, ",", ".") & "," & a.CodEnvio & ",0," & IIf(a.Montaje = False, 0, 1) & "," & mUsuario.id & " )"

            mda.InsertCommand = New SqlCommand(cad, mcon)
            mda.InsertCommand.ExecuteNonQuery()
            'ahora inserto las lineas de albaran
            Dim lin As New clsAlbaranLinea
            Dim Miva As Decimal
            Dim mRe As Decimal
            Dim Iva As Decimal
            Dim re As Decimal = 0
            Dim SubTotal As Decimal = 0
            For Each lin In a
                Dim b As New clsBaseImponible
                b = GetIvaByTipo(2) ' al tratarse de una lente el tipo de base es 1
                b.TipoBase = 1
                b.BaseI = lin.precio * (1 - lin.dto / 100)
                If a.Cliente.SinIva = True Then
                    b.Iva = 0
                End If
                If a.Cliente.recargo = False Then
                    b.Re = 0
                End If
                Miva = b.Iva
                mRe = b.Re
                'ahora vamos añadiendo la base si no la encontramos o la sumamos si la encontramos
                Dim Encontrada As Boolean = False
                For Each base As clsBaseImponible In a.Bases
                    If b.TipoBase = base.TipoBase Then
                        Encontrada = True
                        base.BaseI += b.BaseI
                        Exit For
                    End If
                Next
                If Encontrada = False Then
                    a.Bases.add(b)
                End If
                ' If a.Cliente.contrareembolso = True Then

                'aqui calculamos el total de contrareembolso que vamos a grabar 
                SubTotal = lin.precio * (1 - lin.dto / 100)
                re = SubTotal * (mRe / 100)
                Iva = SubTotal * (Miva / 100)
                'End If

                cad = "insert into t_lineas_albaran values(" & _
                id & "," & lin.id_tipo_Producto & "," & lin.id_modelo & "," & lin.id_tratamiento & _
                "," & lin.id_coloracion & "," & lin.id_modo & "," & lin.c1 & ",'" & lin.c2 & "','" & _
                lin.descripcion & "'," & Replace(lin.precio, ",", ".") & "," & Replace(lin.dto, ",", ".") & "," & _
                Replace(lin.total, ",", ".") & "," & lin.idpedido & "," & IIf(lin.Montaje = True, 1, 0) & _
                "," & NumSql(Miva) & "," & NumSql(mRe) & ")"
                mda.InsertCommand.CommandText = cad
                mda.InsertCommand.ExecuteNonQuery()
                'ahora insertamos la linea en la factura, no actualizamos las bases puesto que  la base se le cobro al emitir la factura
                Dim l As New clsFacturaLinea
                l.id_albaran = a.Id_albaran
                l.descripcion = lin.descripcion
                l.id_coloracion = lin.coloracion
                l.id_modelo = lin.id_modelo
                l.id_modo = lin.id_modo
                l.id_tipo_Producto = lin.id_tipo_Producto
                l.id_tratamiento = lin.id_tratamiento
                l.precio = lin.precio

                l.dto = 100 'aqui le ponemos el 100% de descuento en la factura para que no salga total
                l.total = 0
                l.iva = lin.Iva
                l.Re = lin.Re
                'updateamos el pedido con el idalbaran

                cad = "Update t_pedidos set id_albaran=" & a.Id_albaran & " where id_pedido=" & lin.idpedido
                mda.UpdateCommand = New SqlCommand(cad, mcon)
                mda.UpdateCommand.ExecuteNonQuery()
                mda.InsertCommand.CommandText = "INSERT INTO t_lineas_factura select " & lin.Factura & "," & l.id_tipo_Producto & "," & _
                l.id_modelo & "," & l.id_tratamiento & "," & l.id_coloracion & "," & l.id_modo & "," & l.c1 & "," & strsql(l.c2) & "," & _
                strsql(l.descripcion) & "," & NumSql(l.precio) & "," & NumSql(l.dto) & "," & NumSql(l.total) & "," & NumSql(l.iva) & "," & _
                NumSql(l.Re) & "," & l.id_albaran & ",max(linea)+1 from t_lineas_factura where id_factura=" & lin.Factura
                mda.InsertCommand.ExecuteNonQuery()
            Next

        End If



        'ahora tenemos que grabar las bases del albaran
        For Each base As clsBaseImponible In a.Bases
            cad = "INSERT INTO t_bases_albaran (id_albaran,base,iva,re,id_tipo_base) VALUES (" & a.Id_albaran & "," & NumSql(base.BaseI) & "," & NumSql(base.Iva) & "," & NumSql(base.Re) & "," & NumSql(base.TipoBase) & ")"
            mda.InsertCommand.CommandText = cad
            mda.InsertCommand.ExecuteNonQuery()
        Next
        'vamos a grabar el coste del sobre en cada pedido

        '   mcon.Close()

        If mcon.State = ConnectionState.Open Then
            mcon.Close()
        End If

        Return a
    End Function
    Public Function GetMuestrarioSColor() As DataTable
        Dim cad As String = "select *,(select montaje from t_montajes where id_montaje=t_muestrarios_color.id_montaje) as montaje,(select nombre_comercial from t_clientes where id_cliente=t_muestrarios_color.id_cliente) as cliente from t_muestrarios_color "
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetMuestrarioColor(ByVal Muestrario As Integer) As DataTable
        Dim cad As String = "select *,(select nombre from t_modelos where id_lente=id_modelo) as modelo,(select nombre from t_tratamientos where id_tratamiento=t_lineas_muestrario_color.id_tratamiento) as tratamiento,(select (gama + ' ' + isnull(color,'')) from  t_coloraciones where id_coloracion=t_lineas_muestrario_color.id_color) as color from t_lineas_muestrario_color  where id_muestrario=" & Muestrario
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Sub GrabaLineaMuestrarioColor(ByVal idMuestrario As Integer, ByVal Modo As String, ByVal idModelo As Integer, ByVal idTratamiento As Integer, ByVal idColor As Integer, ByVal Intensidad As Integer)

        Dim cad As String = "INSERT INTO t_lineas_muestrario_color (id_muestrario,id_modelo,modo,id_tratamiento,id_color,intensidad) VALUES (" & idMuestrario & "," & idModelo & "," & strsql(Modo) & "," & idTratamiento & "," & idColor & "," & Intensidad & ")"

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Function GrabaMuestrarioColor(ByVal idMuestrario As Integer, ByVal muestrario As String, ByVal idMontaje As Integer, ByVal idCliente As Integer, ByVal diametro As Integer) As Integer
        Dim cad As String = ""
        If idMuestrario = 0 Then
            idMuestrario = getMaxId("id_muestrario", "t_muestrarios_color") + 1
            cad = "INSERT INTO t_muestrarios_color (id_muestrario,nombre,id_montaje,id_cliente,diametro) VALUES (" & idMuestrario & "," & strsql(muestrario) & "," & idMontaje & "," & idCliente & "," & diametro & ")"
        Else
            cad = "UPDATE t_muestrarios_color set nombre=" & strsql(muestrario) & ",id_montaje=" & idMontaje & ",id_cliente=" & idCliente & ",diametro=" & diametro & " where id_muestrario=" & idMuestrario
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        Return idMuestrario
    End Function
    Public Function GetIvaByTipo(ByVal id As Integer) As clsBaseImponible
        Dim cad As String = "select * from t_tipos_iva where id_iva=" & id
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        Dim abierta As Boolean = False
        If mcon.State <> ConnectionState.Closed Then
            abierta = True 'si esta abierta cuando llega debe dejarse abierta al salir
        Else
            mcon.Open()
        End If

        mda.Fill(tb)
        If abierta = False Then
            mcon.Close()
        End If
        Dim b As New clsBaseImponible
        b.Iva = tb.Rows(0)("iva")
        b.Re = tb.Rows(0)("re")
        Return b
    End Function

    Public Function GetPedidosPendienteByIdCliente(ByVal idcliente As Integer, Optional ByVal IncluirMontajes As Boolean = True) As DataTable
        Dim cad As String = "(select id_pedido,fecha,id_pedido as numero, 'NO' as montaje from t_pedidos where id_Cliente=" & idcliente & " and fecha>=20100101 and fecha_salida<>0 and anulado=0 and id_albaran=0 " & IIf(IncluirMontajes = False, " and montaje=0", "") & ")"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Sub MarcaPedidoAlbaraneado(ByVal idPed As Long, ByVal idAlb As Long)
        Dim cad As String = "update t_pedidos set id_albaran=" & idAlb & " where id_pedido=" & idPed
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub MarcaPedidoMontajeAlbaraneado(ByVal idPed As Long, ByVal idAlb As Long)
        Dim cad As String = "update t_pedidos_montajes set id_albaran=" & idAlb & " where id_pedido_montaje=" & idPed
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function PedidoAlbaraneado(ByVal idPedido As Long) As Long
        'comprueba si el pedido esta o no albaraneado , para que no pueda hacer repeticion 
        'si esta albaraneado devuelve el nuemro de albaran para poder repasarloara
        Dim cad As String = "select isnull(id_albaran,0) as albaran from t_lineas_albaran where id_pedido = " & idPedido
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count > 0 Then
            Return tb.Rows(0)("albaran")
        Else
            Return 0
        End If


    End Function
    Public Sub GrabaContactoComercial(ByRef cli As clsCliente)
        Dim cad As String = ""
        If cli.id = 0 Then
            cli.id = getMaxId("id_contacto", "comercial.dbo.t_contactos") + 1
            cad = "INSERT INTO comercial.dbo.t_contactos (id_contacto,nombre,direccion,localidad,id_provincia,telefono,cod_postal,email,id_comercial,contacto,grupo_optico,notas) VALUES " & _
            "(" & cli.id & "," & strsql(cli.Nombre_Comercial) & "," & strsql(cli.direccion) & "," & strsql(cli.poblacion) & "," & cli.Provincia & "," & strsql(cli.telefono) & "," & strsql(cli.Codigo_Postal) & "," & strsql(cli.Email) & _
            "," & cli.IdComercial & "," & strsql(cli.Persona_Contacto) & "," & cli.GrupoOptico & "," & strsql(cli.Notas) & ")"
        Else
            cad = "UPDATE comercial.dbo.t_contactos SET nombre=" & strsql(cli.Nombre_Comercial) & ",direccion=" & strsql(cli.direccion) & ",localidad=" & strsql(cli.poblacion) & ",id_provincia=" & cli.Provincia & ",telefono=" & strsql(cli.telefono) & _
            ",cod_postal=" & strsql(cli.Codigo_Postal) & ",email=" & strsql(cli.Email) & ",contacto=" & strsql(cli.Persona_Contacto) & ",grupo_optico=" & cli.GrupoOptico & ",notas=" & strsql(cli.Notas) & " where id_contacto=" & cli.id
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function GrabaCliente(ByRef id As Long, ByVal codigo As String, ByVal cif As String, ByVal nombre_comercial As String, _
    ByVal direccion As String, ByVal DirFiscal As String, ByVal poblacion As String, ByVal PoblacionFiscal As String, ByVal telefono As String, ByVal Grupo As Integer, ByVal idProvincia As Integer, _
    ByVal IdProvinciaFiscal As String, ByVal idComercial As Integer, ByVal codigo_contaplus As String, ByVal razon_social As String, ByVal cp As String, ByVal CpFiscal As String, ByVal contacto As String, ByVal cuenta_corriente As String, _
    ByVal email As String, ByVal mostrar_precios As Boolean, ByVal cobrar_portes As Boolean, ByVal recargo As Boolean, ByVal contrarembolso As Boolean, ByVal hoja_blanca As Integer, ByVal formapago As Integer, _
    ByVal diasPago As Integer, ByVal Deuda As Boolean, Optional ByVal SinIva As Boolean = False, Optional ByVal codWeb As String = "", Optional ByVal SinSuplemento As Boolean = False, _
    Optional ByVal DtoMontaje As Single = 0, Optional ByVal Login As String = "", Optional ByVal SinDtoWeb As Boolean = False, Optional ByVal AlbaranPorPedido As Boolean = False, Optional ByVal CLIENT As String = "", _
 Optional ByVal Diasenvio As String = "0", Optional ByVal Vip As Boolean = False, Optional ByVal seguimiento As Boolean = False, Optional ByVal MaxDescubierto As Decimal = 0, Optional ByVal e_factura As Boolean = False, _
 Optional ByVal UsoReferencia As Boolean = False, Optional ByVal Facturacion As String = "Quincenal", Optional ByVal PorteMensual As Decimal = 0, Optional ByVal transporte As String = "", _
 Optional ByVal ConfirmaPedidos As Boolean = False, Optional ByVal ConfirmaPorte As Boolean = False, Optional ByVal PendienteDatos As Boolean = False, Optional ByVal Rappel As Boolean = False, Optional ByVal ProntoPago As Decimal = 0, Optional ByVal Enpapel As Boolean = False, Optional ByVal RegGeneral As Boolean = True, Optional ByVal Publicidad As Boolean = False) As Integer

        Dim mda As New SqlDataAdapter
        Dim cad As String = ""

        If id = 0 Then ' si no tiene id es que es nuevo
            id = getMaxId("id_cliente", "t_clientes") + 1

            cad = "insert into t_clientes (id_cliente,codigo,cif,nombre_comercial,direccion,direccion_fiscal, poblacion,poblacion_fiscal,telefono," & _
            "id_provincia,id_provincia_fiscal,codigocp,razon_social,cp,cp_fiscal,contacto,cc,email,mostrar_precios,cobrar_portes,recargo,contrareembolso," & _
            "hoja_blanca,id_forma_pago,dias_pago,deuda,siniva,cod_web,sin_suplementos,dto_montaje, sin_dto_web,albaranXpedido,CLIENT,dias_envio,vip,seguimiento," & _
            "maximo_descubierto,e_factura,usar_referencia,id_comercial,facturacion,porte_mensual,transportista,confirma_pedidos,confirma_portes,pendiente_datos,rappel,Dto_pp,factura_papel,id_grupo,regimen_general,publicidad)" & _
            " values (" & id & ",'" & codigo & "','" & cif & "','" & nombre_comercial & _
            "','" & direccion & "','" & DirFiscal & "','" & poblacion & "','" & PoblacionFiscal & "','" & telefono & "'," & idProvincia & "," & IdProvinciaFiscal & ",'" & codigo_contaplus & "','" & _
            razon_social & "','" & cp & "','" & CpFiscal & "','" & contacto & "','" & cuenta_corriente & "','" & email & "'," & CInt(mostrar_precios) & _
            "," & CInt(cobrar_portes) & "," & CInt(recargo) & "," & CInt(contrarembolso) & "," & hoja_blanca & "," & formapago & "," & diasPago & _
            "," & IIf(Deuda = False, 0, 1) & "," & IIf(SinIva = False, 0, 1) & ",'" & codWeb & "'," & IIf(SinSuplemento = False, 0, 1) & "," & Replace(DtoMontaje, ",", ".") & "," & IIf(SinDtoWeb = False, 0, 1) & "," & IIf(AlbaranPorPedido = False, 0, 1) & _
            "," & strsql(CLIENT) & "," & strsql(Diasenvio) & "," & IIf(Vip = False, 0, 1) & "," & IIf(seguimiento = False, 0, 1) & "," & NumSql(MaxDescubierto) & "," & IIf(e_factura = False, 0, 1) & "," & IIf(UsoReferencia = False, 0, 1) & "," & idComercial & _
            "," & strsql(Facturacion) & "," & NumSql(PorteMensual) & "," & strsql(transporte) & "," & IIf(ConfirmaPedidos = True, 1, 0) & _
            "," & IIf(ConfirmaPorte = True, "1", "0") & "," & IIf(PendienteDatos = False, 0, 1) & "," & IIf(Rappel = False, 0, 1) & "," & NumSql(ProntoPago) & "," & IIf(Enpapel = False, 0, 1) & "," & Grupo & "," & IIf(RegGeneral = True, 1, 0) & _
            "," & IIf(Publicidad = False, 0, 1) & ")"
            mcon.Open()
            mda.InsertCommand = New SqlCommand(cad, mcon)
            mda.InsertCommand.ExecuteNonQuery()
            If Grupo <> 0 Then
                mda.InsertCommand.CommandText = "INSERT INTO t_acuerdos_clientes select id_acuerdo," & id & ",desde,hasta from t_acuerdos where id_acuerdo in (select id_acuerdo from t_acuerdos where id_grupo=" & Grupo & ")"
                mda.InsertCommand.ExecuteNonQuery()
            End If
        Else
            cad = "update t_clientes set codigo='" & codigo & "',cif='" & cif & "',nombre_comercial='" & _
            nombre_comercial & "',telefono='" & _
            telefono & "',id_provincia=" & idProvincia & ",poblacion=" & strsql(poblacion) & ",codigocp='" & codigo_contaplus & _
            "',razon_social='" & razon_social & "',cp='" & cp & "',contacto='" & contacto & "',cc='" & _
            cuenta_corriente & "',direccion=" & strsql(direccion) & ",email='" & email & "',mostrar_precios=" & CInt(mostrar_precios) & ",cobrar_portes=" & _
            CInt(cobrar_portes) & ",recargo=" & CInt(recargo) & ",contrareembolso=" & CInt(contrarembolso) & ",hoja_blanca=" & hoja_blanca & _
            ",id_forma_pago=" & formapago & ",dias_pago=" & diasPago & ",deuda=" & IIf(Deuda = False, 0, 1) & _
            ",sinIva=" & IIf(SinIva = False, 0, 1) & ",cod_web='" & codWeb & "',sin_suplementos=" & IIf(SinSuplemento = False, 0, 1) & _
            ",dto_montaje=" & Replace(DtoMontaje, ",", ".") & ",sin_dto_web=" & IIf(SinDtoWeb = False, 0, 1) & ",albaranXpedido=" & IIf(AlbaranPorPedido = False, 0, 1) & _
            ",CLIENT=" & strsql(CLIENT) & ",dias_envio=" & strsql(Diasenvio) & ",vip=" & IIf(Vip = False, 0, 1) & ",seguimiento=" & IIf(seguimiento = False, 0, 1) & ",maximo_descubierto=" & NumSql(MaxDescubierto) & _
            ",e_factura=" & IIf(e_factura = False, 0, 1) & ",usar_referencia=" & IIf(UsoReferencia = False, 0, 1) & _
            ",direccion_fiscal=" & strsql(DirFiscal) & ",poblacion_fiscal=" & strsql(PoblacionFiscal) & ",cp_fiscal=" & strsql(CpFiscal) & ",id_provincia_fiscal=" & IdProvinciaFiscal & _
            ",id_comercial=" & idComercial & ",facturacion=" & strsql(Facturacion) & ",porte_mensual=" & NumSql(PorteMensual) & ",transportista=" & strsql(transporte) & _
            ",confirma_pedidos=" & IIf(ConfirmaPedidos = True, 1, 0) & ",confirma_Portes=" & IIf(ConfirmaPorte = True, 1, 0) & _
            ",pendiente_datos=" & IIf(PendienteDatos = False, 0, 1) & ",rappel=" & IIf(Rappel = False, 0, 1) & ",dto_pp=" & NumSql(ProntoPago) & ",factura_papel=" & IIf(Enpapel = False, 0, 1) & ",id_grupo=" & Grupo & ",regimen_general=" & IIf(RegGeneral = True, "1", "0") & _
            ",publicidad=" & IIf(Publicidad = False, 0, 1) & " where id_cliente = " & id
            mcon.Open()
            mda.UpdateCommand = New SqlCommand(cad, mcon)
            mda.UpdateCommand.ExecuteNonQuery()
        End If

        mcon.Close()

        If Login <> "" Then
            cad = "select count(*) from t_clientes_web where id_cliente=" & id
            Dim existe As Boolean
            Dim cmd As New SqlCommand(cad, mcon)
            mcon.Open()
            existe = cmd.ExecuteScalar
            If existe = False Then
                cad = "INSERT INTO t_clientes_web VALUES (" & id & ",'" & Login & "','123',1,0)"
                MsgBox("Ha dado de alta un nuevo codigo web. Recuerde informar al cliente de lo siguiente:" & vbNewLine & "Puede realizar pedidos online las 24 horas del dia desde www.lentesloa.com." & vbNewLine & _
                "Su codigo de usuario es " & Login & " y su contraseña para entrar por primera vez es 123, posteriormente debe cambiar la contraseña.")
            Else
                cad = "UPDATE t_clientes_web set login='" & Login & "' where id_cliente=" & id
                ' MsgBox("Recuerde avisar al cliente de que ha cambiado su login al nuevo login: " & Login & ". Su contraseña permanece igual.")

            End If
            cmd.CommandText = cad
            cmd.ExecuteNonQuery()
            mcon.Close()
        End If
        Return id
    End Function
    Public Function GrabaCliente(ByVal Cli As clsCliente) As Integer
        Dim mda As New SqlDataAdapter
        Dim cad As String = ""
        With Cli
            Dim DiasEnvio As String = ""
            For i As Integer = 0 To UBound(.DiasEnvio)
                DiasEnvio = DiasEnvio & "," & .DiasEnvio(i)
            Next
            DiasEnvio = DiasEnvio.Substring(0, DiasEnvio.Length - 1)
            If Cli.id = 0 Then ' si no tiene id es que es nuevo
                Cli.id = getMaxId("id_cliente", "t_clientes") + 1

                cad = "insert into t_clientes (id_cliente,codigo,cif,nombre_comercial,direccion,direccion_fiscal, poblacion,poblacion_fiscal,telefono," & _
                "id_provincia,id_provincia_fiscal,codigocp,razon_social,cp,cp_fiscal,contacto,cc,email,mostrar_precios,cobrar_portes,recargo,contrareembolso," & _
                "hoja_blanca,id_forma_pago,dias_pago,deuda,siniva,cod_web,sin_suplementos,dto_montaje, sin_dto_web,albaranXpedido,CLIENT,dias_envio,vip,seguimiento," & _
                "maximo_descubierto,e_factura,usar_referencia,id_comercial,facturacion,porte_mensual,transportista,confirma_pedidos,confirma_portes,pendiente_datos,rappel,Dto_pp,factura_papel,id_grupo,regimen_general,publicidad)" & _
                " values (" & .id & ",'" & .Codigo & "','" & .CIF & "','" & .Nombre_Comercial & _
                "','" & .direccion & "','" & .DirFiscal & "','" & .poblacion & "','" & .PoblacionFiscal & "','" & .telefono & "'," & .Id_Provincia & "," & .IdProvinciaFiscal & ",'" & .codigoCp & "','" & _
                .Razon_social & "','" & .Codigo_Postal & "','" & .CPFiscal & "','" & .Persona_Contacto & "','" & .Cuenta_corriente & "','" & .Email & "'," & CInt(.MostrarPrecio) & _
                "," & CInt(.cobro_portes) & "," & CInt(.recargo) & "," & CInt(.contrareembolso) & "," & .hoja_blanca & "," & .FormaPago.IdForma & "," & .FormaPago.Dias & _
                "," & IIf(Cli.Deuda = False, 0, 1) & "," & IIf(.SinIva = False, 0, 1) & ",'" & .CodWeb & "'," & IIf(.SinSuplementos = False, 0, 1) & "," & Replace(.DtoMontaje, ",", ".") & "," & IIf(.SinDtoWeb = False, 0, 1) & "," & IIf(.AlbaranPorPedido = False, 0, 1) & _
                "," & strsql(.CLIENT) & "," & strsql(DiasEnvio) & "," & IIf(.Vip = False, 0, 1) & "," & IIf(.Seguimiento = False, 0, 1) & "," & NumSql(.MaxDescubierto) & "," & IIf(.E_Factura = False, 0, 1) & "," & IIf(.UsarReferencia = False, 0, 1) & "," & .IdComercial & _
                "," & strsql(.Facturacion) & "," & NumSql(.PorteMensual) & "," & strsql(.Transporte) & "," & IIf(.ConfirmacionPedidos = True, 1, 0) & _
                "," & IIf(.ConfirmacionPortes = True, "1", "0") & "," & IIf(.PendienteDatos = False, 0, 1) & "," & IIf(.AplicarRappel = False, 0, 1) & "," & NumSql(.ProntoPago) & "," & IIf(.FacturaEnPapel = False, 0, 1) & "," & .GrupoOptico & "," & IIf(.RegimenGerenal = True, 1, 0) & _
                "," & IIf(.Publicidad = True, 1, 0) & ")"
                mcon.Open()
                mda.InsertCommand = New SqlCommand(cad, mcon)
                mda.InsertCommand.ExecuteNonQuery()
                If .GrupoOptico <> 0 Then
                    mda.InsertCommand.CommandText = "INSERT INTO t_acuerdos_clientes select id_acuerdo," & .id & ",desde,hasta from t_acuerdos where id_acuerdo in (select id_acuerdo from t_acuerdos where id_grupo=" & .GrupoOptico & ")"
                    mda.InsertCommand.ExecuteNonQuery()
                End If

            Else
                cad = "update t_clientes set codigo='" & .Codigo & "',cif='" & .CIF & "',nombre_comercial='" & _
        .Nombre_Comercial & "',telefono='" & _
        .telefono & "',id_provincia=" & .Id_Provincia & ",poblacion=" & strsql(.poblacion) & ",codigocp='" & .codigoCp & _
        "',razon_social='" & .Razon_social & "',cp='" & .Codigo_Postal & "',contacto='" & .Persona_Contacto & "',cc='" & _
        .Cuenta_corriente & "',direccion=" & strsql(.direccion) & ",email='" & .Email & "',mostrar_precios=" & CInt(.MostrarPrecio) & ",cobrar_portes=" & _
        CInt(.cobro_portes) & ",recargo=" & CInt(.recargo) & ",contrareembolso=" & CInt(.contrareembolso) & ",hoja_blanca=" & .hoja_blanca & _
        ",id_forma_pago=" & .FormaPago.IdForma & ",dias_pago=" & .FormaPago.Dias & ",deuda=" & IIf(.Deuda = False, 0, 1) & _
        ",sinIva=" & IIf(.SinIva = False, 0, 1) & ",cod_web='" & .CodWeb & "',sin_suplementos=" & IIf(.SinSuplementos = False, 0, 1) & _
        ",dto_montaje=" & Replace(.DtoMontaje, ",", ".") & ",sin_dto_web=" & IIf(.SinDtoWeb = False, 0, 1) & ",albaranXpedido=" & IIf(.AlbaranPorPedido = False, 0, 1) & _
        ",CLIENT=" & strsql(.CLIENT) & ",dias_envio=" & strsql(DiasEnvio) & ",vip=" & IIf(.Vip = False, 0, 1) & ",seguimiento=" & IIf(.Seguimiento = False, 0, 1) & ",maximo_descubierto=" & NumSql(.MaxDescubierto) & _
        ",e_factura=" & IIf(.E_Factura = False, 0, 1) & ",usar_referencia=" & IIf(.UsarReferencia = False, 0, 1) & _
        ",direccion_fiscal=" & strsql(.DirFiscal) & ",poblacion_fiscal=" & strsql(.PoblacionFiscal) & ",cp_fiscal=" & strsql(.CPFiscal) & ",id_provincia_fiscal=" & .IdProvinciaFiscal & _
        ",id_comercial=" & .IdComercial & ",facturacion=" & strsql(.Facturacion) & ",porte_mensual=" & NumSql(.PorteMensual) & ",transportista=" & strsql(.Transporte) & _
        ",confirma_pedidos=" & IIf(.ConfirmacionPedidos = True, 1, 0) & ",confirma_Portes=" & IIf(.ConfirmacionPortes = True, 1, 0) & _
        ",pendiente_datos=" & IIf(.PendienteDatos = False, 0, 1) & ",rappel=" & IIf(.AplicarRappel = False, 0, 1) & ",dto_pp=" & NumSql(.ProntoPago) & ",factura_papel=" & IIf(.FacturaEnPapel = False, 0, 1) & _
        ",id_grupo=" & .GrupoOptico & ",regimen_general=" & IIf(.RegimenGerenal = True, "1", "0") & ",publicidad=" & IIf(.Publicidad = True, 1, 0) & " where id_cliente = " & .id
                mcon.Open()
                mda.UpdateCommand = New SqlCommand(cad, mcon)
                mda.UpdateCommand.ExecuteNonQuery()
            End If

            mcon.Close()

            If .LoginInternet <> "" Then
                cad = "select count(*) from t_clientes_web where id_cliente=" & .id
                Dim existe As Boolean
                Dim cmd As New SqlCommand(cad, mcon)
                mcon.Open()
                existe = cmd.ExecuteScalar
                If existe = False Then
                    cad = "INSERT INTO t_clientes_web VALUES (" & .id & ",'" & .LoginInternet & "','123',1,0)"
                    MsgBox("Ha dado de alta un nuevo codigo web. Recuerde informar al cliente de lo siguiente:" & vbNewLine & "Puede realizar pedidos online las 24 horas del dia desde www.lentesloa.com." & vbNewLine & _
                    "Su codigo de usuario es " & .LoginInternet & " y su contraseña para entrar por primera vez es 123, posteriormente debe cambiar la contraseña.")
                Else
                    cad = "UPDATE t_clientes_web set login='" & .LoginInternet & "' where id_cliente=" & .id
                    ' MsgBox("Recuerde avisar al cliente de que ha cambiado su login al nuevo login: " & Login & ". Su contraseña permanece igual.")

                End If
                cmd.CommandText = cad
                cmd.ExecuteNonQuery()
                mcon.Close()
            End If
        End With
        Return Cli.id
    End Function
    Public Function GetClientesPublicidad(ByVal Filtro As String) As DataTable
        If Filtro.Length <> 0 Then
            Filtro = " and " & Filtro
        End If
        Dim cad As String = "select t_clientes.id_cliente,email,clave from t_clientes INNER JOIN t_clientes_web ON t_clientes.id_cliente=t_clientes_web.id_cliente where publicidad=1  and email like '%@%' and t_clientes.baja=0" & Filtro
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function DevuelveCodigoClienteByProvincia(ByVal id_provincia As Integer) As String
        Dim cad As String = "select max(codigo) as ultimo from t_clientes where codigo like '" & Format(id_provincia, "00") & "%'"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        Dim mCod As Long
        If IsDBNull(tb.Rows(0)("ultimo")) Then Return Format(id_provincia, "00") & "000001"
        mCod = tb.Rows(0)("ultimo") + 1
        cad = Format(mCod, "00000000")
        mcon.Close()
        Return cad

    End Function
    Public Function CompruebaCodClienteRepetido(ByVal cod As String) As Boolean
        Dim cad As String = "select * from t_clientes where codigo = '" & cod & "'"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function GetPuertoSerie() As Integer
        Dim cad As String = "Select isnull(puerto_serie,0) from t_puertos_serie where equipo like " & strsql(Equipo)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim puerto As Integer = cmd.ExecuteScalar
        If puerto = 0 Then puerto = 1
        mcon.Close()
        Return puerto
    End Function
    Public Sub GrabaPuertoSerie(ByVal puerto As Integer)
        Dim cad As String = "declare @cuenta as integer" & vbNewLine & "select @cuenta=count(*) from t_puertos_serie where equipo like " & strsql(Equipo) & vbNewLine & _
        "if @cuenta=0   " & vbNewLine & "begin " & vbNewLine & "INSERT into t_puertos_serie select " & strsql(Equipo) & "," & puerto & vbNewLine & "END " & vbNewLine & "if @cuenta<>0  " & vbNewLine & "BEGIN " & vbNewLine & "UPDATE t_puertos_serie set puerto_serie=" & puerto & " where equipo like " & strsql(Equipo) & " END"

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Function GetImpresoraBytipo(ByVal tipo As TipoImpresora) As clsImpresoraDocumento
        Dim Imp As New clsImpresoraDocumento
        Dim cad As String = "select * from t_impresoras where equipo=" & strsql(Equipo) & " and id_documento=" & tipo
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            Imp.Documento = tipo
            Imp.Impresora = ImpresoraPredeterminada.PrinterName
            Imp.Alto = ImpresoraPredeterminada.DefaultPageSettings.PaperSize.Height
            Imp.Ancho = ImpresoraPredeterminada.DefaultPageSettings.PaperSize.Width
            Imp.Orientacion = ImpresoraPredeterminada.DefaultPageSettings.Landscape
            Imp.NombrePapel = ImpresoraPredeterminada.DefaultPageSettings.PaperSize.PaperName
        Else
            Imp.Documento = tipo
            Imp.Impresora = tb.Rows(0)("impresora")
            Imp.Alto = tb.Rows(0)("alto")
            Imp.Ancho = tb.Rows(0)("ancho")
            Imp.Orientacion = CBool(tb.Rows(0)("orientacion"))
            Imp.NombrePapel = tb.Rows(0)("papel")
        End If
        Return Imp
    End Function

    Public Sub MarcarSinCargo(ByVal idPedido As Long)
        ' invierte el valor que tenga el campo si es 0 lo pone a uno y si es uno a 0
        Dim cad As String = "select sin_cargo from t_pedidos where id_pedido = " & idPedido
        Dim mda As New SqlDataAdapter
        Dim tb As New DataTable
        mda.SelectCommand = (New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        Dim valor As Boolean
        valor = tb.Rows(0)("sin_cargo")
        ' invierto el valor
        valor = Not valor
        cad = "Update t_pedidos set sin_cargo=" & CInt(valor) & " where id_pedido= " & idPedido
        mda.UpdateCommand = New SqlCommand(cad, mcon)
        mda.UpdateCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Sub RectificaUltimaSalida(ByVal idPedido As Long)
        'esta funcion sirve para hacer que un picking de distribución se 
        'elimino el pickig de distribucion y  elimino el albaran 
    End Sub
    Public Function GetPVPGarantiaBypedido(ByVal p As clsPedido) As Decimal
        Dim cad As String = "select coste from t_garantias where id_garantia=(select id_garantia from t_modelos where id_lente=" & p.id_modelo & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        Dim pvp As Decimal
        Dim CnnAbierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            CnnAbierta = True
        Else
            mcon.Open()
        End If
        pvp = cmd.ExecuteScalar
        If CnnAbierta = False Then
            mcon.Close()

        End If
        Return (pvp)
    End Function
    Public Function PasoCalidadADistribucion(ByVal p As clsPedido, Optional ByVal Imprimir As Boolean = True) As Boolean
        Dim imp As New clsImpresion
        'Dim d As New clsDatos

        '    nos aseguramos que se grabe calidad puesto que en algun sitio ha habido un fallo
        If SalidaCalidad(p) = False Then
            If Mensaje("Error dando el paso de calidad. ¿Quiere intentarlo de nuevo?", True) = True Then
                If SalidaCalidad(p) = False Then
                    Mensaje("No se ha podido grabar Calidad")
                    Return False
                End If
            End If
        End If

        Dim PVPGarantia As Decimal = GetPVPGarantiaBypedido(p)
        If p.pareja = 0 Then
            
            'If Imprimir = True Then
            '    imp.ImprimePegatinaSobre(p)
            'End If
            'If SalidaDistribucion(p.id) = False Then Return False

            If PVPGarantia <> 0 Then
                GrabaCostePedido(p.id, "Garantia", PVPGarantia)
            End If

            If Imprimir = True Then
                imp.ImprimePegatinaSobre(p)
            End If
            'If p.LlevaMontaje = True Then
            '    QuitaDistribucion(p)
            'End If
        Else

            If ProcesoGrabado(p.pareja, Paso.Calidad, Proceso.Salida) = True Then
                Dim Mpar As New clsPedido
                If p.LlevaMontaje = True And Biselado(p.id) = False Then
                    If SalidaDistribucion(p.id) = False Then Return False
                    If SalidaDistribucion(p.pareja) = False Then Return False
                End If
                If PVPGarantia <> 0 And p.FechaSalida <> "" Then
                    GrabaCostePedido(p.id, "Garantia", PVPGarantia / 2)
                    GrabaCostePedido(p.pareja, "Garantia", PVPGarantia / 2)
                End If





                If Imprimir = True Then
                    ' p = GetPedidobyId(p.id)
                    'If p.FechaSalida <> "" Then
                    imp.ImprimePegatinaSobre(p)
                    Mpar = GetPedidobyId(p.pareja)
                    imp.ImprimePegatinaSobre(Mpar)
                End If
                If EsProgresivoPedido(p.id) Then

                    If SalidaDistribucion(p.id) = False Then Return False
                    If SalidaDistribucion(p.pareja) = False Then Return False
                    'aqui sacamos la pegatina
                    'si el ojo es izquierdo y hay pareja no lo imprimimos, en caso contrario si lo hacemos
                    'imprimimos la pegatina de garantia de adaptacion
                    imp.ImprimeGarantiaAdaptacion(p)
                End If
            End If
        End If
        imp = Nothing
        Return True
    End Function
    Public Function buscaAlbaran(ByVal numero As Long, ByVal cliente As String, ByVal fecini As Long, ByVal fecfin As Long, Optional ByVal Numpedido As Long = 0, Optional ByVal SinFacturar As Boolean = False) As DataTable
        Dim cad As String
        cad = "select t_albaranes.*,nombre_comercial from t_albaranes inner join " & _
        "t_clientes on t_albaranes.id_cliente=t_clientes.id_cliente where id_albaran <>0 "
        If numero <> 0 Then
            cad = cad & " and id_albaran = " & numero
        End If
        If cliente <> "" Then
            cad = cad & " and (nombre_comercial like '%" & cliente & "%' or codigo like " & strsql(cliente) & ")"
        End If
        If SinFacturar = True Then
            cad = cad & " and id_albaran not in (select id_albaran from t_lineas_factura) "
        End If
        If Numpedido <> 0 Then
            cad = cad & " and (id_pedido=" & Numpedido & " or id_albaran in (select id_albaran from t_lineas_albaran where id_pedido=" & Numpedido & "))"
        End If
        If fecini <> 0 Then
            cad = cad & " and fecha>=" & fecini
        End If
        If fecfin <> 0 Then
            cad = cad & " and fecha<=" & fecfin
        End If
        If mUsuario.Comercial = True Then
            cad = cad & " and id_comercial=" & mUsuario.id
        End If
        cad = cad & " order by id_albaran desc"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.SelectCommand.CommandTimeout = 180
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function AlbaranesCioneByFacturas(ByVal fecha As Integer) As ArrayList
        Dim matriz As New ArrayList
        Dim cad As String = "Select id_albaran  from t_facturas INNER JOIN t_lineas_factura ON t_facturas.id_factura=t_lineas_factura.id_factura where fecha=" & fecha & " and id_cliente=4532 group by id_albaran order by id_albaran"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows
            Dim a As New clsAlbaran
            a = GetAlbaranById(rw("id_albaran"))
            matriz.Add(a)
        Next
        Return matriz
    End Function
    Public Sub QuitaDistribucion(ByVal p As clsPedido)
        Dim Cad As String = "UPDATE t_pedidos set fecha_salida=0,hora_salida=0 where id_pedido=" & p.id
        Dim cmd As New SqlCommand(Cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub CambioPuntosPedido(ByVal idPedido As Integer, ByVal puntos As Integer)
        Dim cad As String = "Update t_puntos_pedido set Puntos=" & puntos & " where id_pedido=" & idPedido
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Function AlbaranesByidFactura(ByVal id As Integer) As ArrayList
        Dim matriz As New ArrayList
        Dim cad As String = "Select id_albaran  from  t_lineas_factura  where id_factura=" & id & " AND ID_ALBARAN>570695 group by id_albaran order by id_albaran"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows


            Dim a As New clsAlbaran
            a = GetAlbaranById(rw("id_albaran"))
            matriz.Add(a)
        Next
        Return matriz
    End Function
    Public Function FacturaCioneExcel(ByVal Fecha As Integer) As DataTable
        Dim Cad As String = "select 2323 as [Código numérico del proveedor en Cione],serie + '-' + convert(varchar,num_factura) as [Factura o de Abono (factura rectificativa)],dbo.fechaexcel(fecha) as [Fecha de Factura o de Abono],id_albaran AS [Número de Albarán de Cargo o de Abono],(select dbo.FechaExcel(fecha) from t_albaranes where id_albaran=t_lineas_factura.id_albaran) as [Fecha de Albarán de Cargo o de Abono],(select codigo from t_codigos_grupo_optico where id_cliente in (select id_cliente from t_albaranes where id_albaran=t_lineas_factura.id_albaran)) as [Código Socio en CIOSA],(select codigo from t_clientes where id_cliente in (select id_cliente from t_albaranes where id_albaran=t_lineas_factura.id_albaran)) as [Código del Cliente en el Proveedor], " & _
        "(select nombre_comercial from t_clientes where id_cliente in (select id_cliente from t_albaranes where id_albaran=t_lineas_factura.id_albaran)) as [Nombre Socio],DESCRIPCION AS [Descripción de Producto], case when id_tipo_producto=1 then (select codigo from t_modelos where id_lente=id_modelo) when id_tipo_producto=0 THEN 'PORTE' ELSE 'SIN CODIGO' END as [Código Interno del Producto en el Proveedor],'' as [Código Producto ISO / EDI],'' as Marca,case when id_tipo_producto=1 then (select nombre from t_modelos where id_lente=id_modelo) ELSE '' END AS Modelo,(select forma_pago from t_formas_pago where id_forma_pago=(select id_forma_pago from t_clientes where id_cliente in (select id_cliente from t_albaranes where id_albaran=t_lineas_factura.id_albaran))) as Vencimientos,case  when t_lineas_factura.total>=0 then 1 else -1 END as [Cantidad a Facturar],abs(T_LINEAS_FACTURA.total) as [Precio Unitario Bruto (sin IVA)],abs(T_LINEAS_FACTURA.total) as [Precio Unitario Neto (sin IVA)],'EUR' as [Moneda / Divisa],iva as [Tipo de IVA],0.00 as [Descuento 1 (sobre el bruto)],0.00 as [Descuento 2 (sobre el bruto)],0.00 as [Descuento 3 (sobre el bruto)],'' as [Código Promoción],(select comision from t_grupos_opticos where id_grupo=4) as [Intermediación Aplicable a la Central],'00' as [Línea susceptible de Descuento adicional],case when precio<0 then (select convert(varchar,id_alb_abono) from t_albaranes where id_albaran=t_lineas_factura.id_albaran) ELSE '' END as [Nº Albarán de cargo a abonar] from t_facturas INNER JOIN t_lineas_factura ON t_facturas.id_factura=t_lineas_factura.id_factura where fecha=" & Fecha & " and id_cliente=4532 order by t_facturas.id_factura,id_albaran"
        Dim mda As New SqlDataAdapter(Cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return (tb)
    End Function
    Public Function GetserverEspesores() As String
        Dim cad As String = "select espesores from t_valores_globales"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        GetserverEspesores = cmd.ExecuteScalar
        mcon.Close()
    End Function
    Public Function buscaPedidosMontaje(ByVal cliente As String, ByVal fecini As Long, ByVal fecfin As Long, Optional ByVal Numpedido As Long = 0, Optional ByVal Lente As Long = 0, Optional ByVal Notas As String = "") As DataTable
        Dim cad As String
        cad = "select *,nombre_comercial from t_pedidos_montajes inner join " & _
        "t_clientes on t_pedidos_montajes.id_cliente=t_clientes.id_cliente where id_pedido_montaje <>0 "

        If cliente <> "" Then
            cad = cad & " and (nombre_comercial like '%" & cliente & "%' or codigo=" & strsql(cliente) & ")"
        End If
        If Numpedido <> 0 Then
            cad = cad & " and id_pedido_montaje=" & Numpedido
        End If
        If fecini <> 0 Then
            cad = cad & " and fecha>=" & fecini
        End If
        If fecfin <> 0 Then
            cad = cad & " and fecha<=" & fecfin
        End If
        If Lente <> 0 Then
            cad = cad & " and (ojo_dcho=" & Lente & " or ojo_izq=" & Lente & ")"
        End If
        If Notas.Length > 0 Then
            cad = cad & "  and notas like " & strsql("%" & Notas & "%")
        End If
        cad = cad & " order by id_pedido_montaje"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function buscaPedidosMontajeCorto(ByVal cliente As String, ByVal fecini As Long, ByVal fecfin As Long, Optional ByVal Numpedido As Long = 0) As DataTable
        Dim cad As String
        cad = "select id_pedido_montaje as Pedido, dbo.fechaexcel(fecha) as fecha,nombre_comercial as cliente,notas from t_pedidos_montajes inner join " & _
        "t_clientes on t_pedidos_montajes.id_cliente=t_clientes.id_cliente where id_pedido_montaje <>0 "

        If cliente <> "" Then
            cad = cad & " and nombre_comercial like '%" & cliente & "%'"
        End If
        If Numpedido <> 0 Then
            cad = cad & " and id_pedido_montaje=" & Numpedido
        End If
        If fecini <> 0 Then
            cad = cad & " and fecha>=" & fecini
        End If
        If fecfin <> 0 Then
            cad = cad & " and fecha<=" & fecfin
        End If
        cad = cad & " order by id_pedido_montaje"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function buscaAlbaranesDetallados(ByVal numero As Long, ByVal cliente As String, ByVal fecini As Long, ByVal fecfin As Long, Optional ByVal numpedido As Long = 0, Optional ByVal SinFacturar As Boolean = False) As DataTable
        Dim cad As String
        cad = "select ( right(left(fecha,6),2) + '/'+ right(fecha,2) + '/' + left(fecha,4)) as Fecha,t_albaranes.id_albaran as Albaran,t_lineas_albaran.id_pedido as pedido,case id_alb_abono when 0 then '' else convert(nvarchar(10),id_alb_abono) end as [albaran Abono],t_clientes.codigo,razon_social,case id_modo when 1 then 'S' when 2 then 'T' when 3 then 'F' else '' end as Modo,(select nombre from t_tratamientos where id_tratamiento=t_lineas_albaran.id_tratamiento) as tratamiento, descripcion,precio,dto,t_lineas_albaran.total from (t_albaranes inner join " & _
        "t_clientes on t_albaranes.id_cliente=t_clientes.id_cliente) INNER JOIN t_lineas_albaran ON t_albaranes.id_albaran=t_lineas_albaran.id_albaran where t_albaranes.id_albaran <>0 "
        If numero <> 0 Then
            cad = cad & " and id_albaran = " & numero
        End If
        If cliente <> "" Then
            cad = cad & " and (nombre_comercial like '%" & cliente & "%' or codigo like " & strsql(cliente) & ")"
        End If
        If SinFacturar = True Then
            cad = cad & " and t_albaranes.id_albaran not in (select id_albaran from t_lineas_factura) "
        End If
        If numpedido <> 0 Then
            cad = cad & " And id_pedido = " & numpedido
        End If
        If fecini <> 0 Then
            cad = cad & " and fecha>=" & fecini
        End If
        If fecfin <> 0 Then
            cad = cad & " and fecha<=" & fecfin
        End If
        If mUsuario.Comercial = True Then
            cad = cad & " and id_comercial=" & mUsuario.id
        End If
        
        cad = cad & " order by t_albaranes.id_albaran"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function


    Public Function PromoFabricacion(ByVal idCliente As Integer, ByVal precio As Decimal, ByVal modelo As Integer, ByVal idtratamiento As Integer) As Decimal
        Dim pvp As Decimal = 0
        Dim cad As String = "select precio_stock,fecha from t_promo_fabricacion where id_cliente=" & idCliente & " and id_modelo=" & modelo & " and id_tratamiento=" & idtratamiento & " and fabricacion>stock order by fecha desc"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)

        If tb.Rows.Count > 0 Then 'vamos a actualizar el stock sumandole uno
            If VGVirtual = False Then
                Dim cmd As New SqlCommand("update t_promo_fabricacion set stock=stock+1 where id_cliente=" & idCliente & " and id_modelo=" & modelo & " and id_tratamiento=" & idtratamiento & " and fecha=" & tb.Rows(0)("fecha"), mcon)
                cmd.ExecuteNonQuery()
            End If
            pvp = tb.Rows(0)("precio_stock")

        End If
        If pvp <> 0 Then
            precio = pvp
        End If
        mcon.Close()
        Return precio
    End Function
    Public Sub GrabaNuevaOrdenTrabajo(ByVal idPed As Long, Optional ByVal PASO As Integer = 0)
        Dim cad As String = "insert into t_ordenes_trabajo (id_orden,id_pedido,fecha,hora,fs_fabrica,hs_fabrica,fs_coloracion,hs_coloracion,fs_tratamiento,hs_tratamiento,fe_fabrica" & _
            ",he_fabrica,fe_coloracion,he_coloracion,fe_endurecimiento,he_endurecimiento,fs_endurecimiento,hs_endurecimiento," & _
            "fe_antireflejo,he_antireflejo,fs_antireflejo,hs_antireflejo,fe_toplight,he_toplight,fs_toplight,hs_toplight,id_incidencia,paso)" & _
            "  ( select max(id_orden)+ 1," & idPed & "," & FechaAcadena(Now.Date) & "," & Now.Hour & Format(Now.Minute, "00") & ",0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," & PASO & " from t_ordenes_trabajo where id_pedido=" & idPed & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub GrabaIncidencia(ByVal idped As Long, ByVal Incidencia As Integer, ByVal paso As Integer)
        Dim cad As String = "update t_ordenes_trabajo set ID_incidencia=" & Incidencia & ",usr_incidencia=" & mUsuario.id & " where id_pedido=" & idped & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & idped & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'ahora grabamos en la tabla de incidencias
        cad = "insert into t_incidencias_pedidos (fecha,hora,id_pedido,id_orden,id_incidencia,id_usuario,aviso_cliente,paso) (select " & FechaAcadena(Now.Date.Date) & "," & Now.Hour & Format(Now.Minute, "00") & "," & idped & ",max(id_orden)," & Incidencia & "," & mUsuario.id & ",0," & paso & " from t_ordenes_trabajo where id_pedido=" & idped & " GROUP BY ID_PEDIDO)"
        cmd.CommandText = cad
        cmd.ExecuteNonQuery()
        'le quitamos la distribucion si ya esta listo para salir
        cmd.CommandText = "Update t_pedidos set fecha_salida=0,hora_salida=0 where id_pedido=" & idped
        cmd.ExecuteNonQuery()
        'le quitamos la distribucion a la pareja
        cmd.CommandText = "Update t_pedidos set fecha_salida=0,hora_salida=0 where pareja=" & idped
        cmd.ExecuteNonQuery()

        mcon.Close()
    End Sub
    Public Sub GrabaIncidenciaLoteAntireflejo(ByVal Lote As Long, ByVal Incidencia As Integer, ByVal paso As Integer)
        Dim cad As String = "select id_pedido from t_lote_antireflejo where lote=" & Lote
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        If tb.Rows.Count > 0 Then
            mcon.Open()
            For Each rw As DataRow In tb.Rows
                Dim idped As Integer = rw("id_pedido")
                cad = "update t_ordenes_trabajo set ID_incidencia=" & Incidencia & ",usr_incidencia=" & mUsuario.id & " where id_pedido=" & idped & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & idped & ")"
                Dim cmd As New SqlCommand(cad, mcon)
                cmd.ExecuteNonQuery()
                'ahora grabamos en la tabla de incidencias
                cad = "insert into t_incidencias_pedidos (fecha,hora,id_pedido,id_orden,id_incidencia,id_usuario,aviso_cliente,paso) (select " & FechaAcadena(Now.Date.Date) & "," & Now.Hour & Format(Now.Minute, "00") & "," & idped & ",max(id_orden)," & Incidencia & "," & mUsuario.id & ",0," & paso & " from t_ordenes_trabajo where id_pedido=" & idped & " GROUP BY ID_PEDIDO)"
                cmd.CommandText = cad
                cmd.ExecuteNonQuery()
                'le quitamos la distribucion si ya esta listo para salir
                cmd.CommandText = "Update t_pedidos set fecha_salida=0,hora_salida=0 where id_pedido=" & idped
                cmd.ExecuteNonQuery()
                'le quitamos la distribucion a la pareja
                cmd.CommandText = "Update t_pedidos set fecha_salida=0,hora_salida=0 where pareja=" & idped
                cmd.ExecuteNonQuery()
                'grabamos la nueva orden de trabajo
                cmd.CommandText = "insert into t_ordenes_trabajo (id_orden,id_pedido,fecha,hora,fs_fabrica,hs_fabrica,fs_coloracion,hs_coloracion,fs_tratamiento,hs_tratamiento,fe_fabrica" & _
                ",he_fabrica,fe_coloracion,he_coloracion,fe_endurecimiento,he_endurecimiento,fs_endurecimiento,hs_endurecimiento," & _
                "fe_antireflejo,he_antireflejo,fs_antireflejo,hs_antireflejo,fe_toplight,he_toplight,fs_toplight,hs_toplight,id_incidencia,paso)" & _
                "  ( select max(id_orden)+ 1," & idped & "," & FechaAcadena(Now.Date) & "," & Now.Hour & Format(Now.Minute, "00") & ",0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," & paso & " from t_ordenes_trabajo where id_pedido=" & idped & ")"
                cmd.ExecuteNonQuery()
            Next
            mcon.Close()
        End If

    End Sub
    Public Sub GrabaIncidenciaLoteEndurecido(ByVal Lote As Long, ByVal Incidencia As Integer, ByVal paso As Integer)
        Dim cad As String = "select id_pedido from t_lote_endurecido where lote=" & Lote
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        If tb.Rows.Count > 0 Then
            mcon.Open()
            For Each rw As DataRow In tb.Rows
                Dim idped As Integer = rw("id_pedido")
                cad = "update t_ordenes_trabajo set ID_incidencia=" & Incidencia & ",usr_incidencia=" & mUsuario.id & " where id_pedido=" & idped & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & idped & ")"
                Dim cmd As New SqlCommand(cad, mcon)
                cmd.ExecuteNonQuery()
                'ahora grabamos en la tabla de incidencias
                cad = "insert into t_incidencias_pedidos (fecha,hora,id_pedido,id_orden,id_incidencia,id_usuario,aviso_cliente,paso) (select " & FechaAcadena(Now.Date.Date) & "," & Now.Hour & Format(Now.Minute, "00") & "," & idped & ",max(id_orden)," & Incidencia & "," & mUsuario.id & ",0," & paso & " from t_ordenes_trabajo where id_pedido=" & idped & " GROUP BY ID_PEDIDO)"
                cmd.CommandText = cad
                cmd.ExecuteNonQuery()
                'le quitamos la distribucion si ya esta listo para salir
                cmd.CommandText = "Update t_pedidos set fecha_salida=0,hora_salida=0 where id_pedido=" & idped
                cmd.ExecuteNonQuery()
                'le quitamos la distribucion a la pareja
                cmd.CommandText = "Update t_pedidos set fecha_salida=0,hora_salida=0 where pareja=" & idped
                cmd.ExecuteNonQuery()
                'grabamos la nueva orden de trabajo
                cmd.CommandText = "insert into t_ordenes_trabajo (id_orden,id_pedido,fecha,hora,fs_fabrica,hs_fabrica,fs_coloracion,hs_coloracion,fs_tratamiento,hs_tratamiento,fe_fabrica" & _
                ",he_fabrica,fe_coloracion,he_coloracion,fe_endurecimiento,he_endurecimiento,fs_endurecimiento,hs_endurecimiento," & _
                "fe_antireflejo,he_antireflejo,fs_antireflejo,hs_antireflejo,fe_toplight,he_toplight,fs_toplight,hs_toplight,id_incidencia,paso)" & _
                "  ( select max(id_orden)+ 1," & idped & "," & FechaAcadena(Now.Date) & "," & Now.Hour & Format(Now.Minute, "00") & ",0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," & paso & " from t_ordenes_trabajo where id_pedido=" & idped & ")"
                cmd.ExecuteNonQuery()
            Next
            mcon.Close()
        End If

    End Sub
    Public Function HayEntrada(ByVal mpaso As Paso, ByVal idPedido As Long) As Boolean
        Dim campo As String = ""
        Select Case mpaso
            Case Paso.almacen
                Return True
            Case Paso.Coloracion
                campo = "FE_coloracion"
            Case Paso.Endurecido
                campo = "FE_endurecimiento"
            Case Paso.Fabricacion
                campo = "FE_fabrica"
            Case Paso.Antireflejo
                campo = "FE_antireflejo"
            Case Paso.Toplight
                campo = "FE_toplight"
            Case Paso.Calidad
                Return True
            Case Paso.Montaje
                campo = "FE_montaje"
            Case Paso.Externa
                campo = "FE_externa"
        End Select
        Dim cad As String = "select " & campo & " from t_ordenes_trabajo where id_pedido=" & idPedido & " and id_orden=(select max(id_orden) from t_ordenes_trabajo where id_pedido=" & idPedido & ")"
        Dim resultado As Boolean
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        resultado = CBool(cmd.ExecuteScalar)
        mcon.Close()
        Return resultado


    End Function

    Public Function GetAlbaranById(ByVal id_albaran As Integer) As clsAlbaran
        Dim cad As String = "select * from t_albaranes where id_albaran =" & id_albaran
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        ' mcon.Open()
        mda.Fill(tb)
        If tb.Rows.Count = 0 Then Return Nothing
        Dim a As New clsAlbaran
        a.abono = tb.Rows(0)("abono")
        a.Cliente = getClientebyId(tb.Rows(0)("id_cliente"))
        a.facturado = tb.Rows(0)("facturado")
        a.fecha = tb.Rows(0)("fecha")
        a.id_alb_abono = tb.Rows(0)("id_alb_abono")
        a.Id_albaran = id_albaran
        a.numAlbaran = tb.Rows(0)("num_albaran")
        a.id_pedido = tb.Rows(0)("id_pedido")
        a.referencia = tb.Rows(0)("referencia")
        a.Total = tb.Rows(0)("total")
        a.CodEnvio = IIf(IsDBNull(tb.Rows(0)("cod_envio")), 0, tb.Rows(0)("cod_envio"))
        a.Montaje = IIf(tb.Rows(0)("montaje") = 0, False, True)
        a.Bono = tb.Rows(0)("id_bono")
        tb.Clear()
        cad = "select * from t_lineas_albaran where id_albaran = " & id_albaran & " order by id_pedido,id_tipo_producto"
        Dim mda2 As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb2 As New DataTable
        mda2.Fill(tb2)
        Dim i As Integer
        Dim ml As clsAlbaranLinea
        For i = 0 To tb2.Rows.Count - 1
            ml = New clsAlbaranLinea
            ml.c1 = tb2.Rows(i)("c1")
            ml.c2 = tb2.Rows(i)("c2")
            ml.id_coloracion = tb2.Rows(i)("id_coloracion")
            ml.descripcion = tb2.Rows(i)("descripcion")
            ml.id_modelo = tb2.Rows(i)("id_modelo")
            ml.id_modo = tb2.Rows(i)("id_modo")
            ml.id_tipo_Producto = tb2.Rows(i)("id_tipo_producto")
            ml.id_tratamiento = tb2.Rows(i)("id_tratamiento")
            ml.Montaje = CBool(tb2.Rows(i)("montaje"))
            If ml.id_tratamiento <> 0 Then ml.tratamiento = getTratamientoById(ml.id_tratamiento)
            If ml.id_coloracion <> 0 Then ml.coloracion = GetColorAlbaran(ml.id_coloracion)
            If ml.id_modelo <> 0 Then ml.modelo = GetNombreModeloByID(ml.id_modelo)
            If ml.id_modo <> 0 Then ml.modo = getModoById(ml.id_modo)
            ml.idpedido = tb2.Rows(i)("id_pedido")
            ml.modelo = CBool(tb2.Rows(i)("montaje"))
            ml.dto = tb2.Rows(i)("dto")
            ml.precio = tb2.Rows(i)("precio")
            ml.total = tb2.Rows(i)("total")
            ml.Iva = tb2.Rows(i)("Iva")
            ml.Re = tb2.Rows(i)("re")
            a.add(ml)
        Next
        'ahora cargamos las bases
        cad = "select * from t_bases_albaran where id_albaran=" & id_albaran
        Dim Mda4 As New SqlDataAdapter(New SqlCommand(cad, mcon))

        tb2.Clear()
        Mda4.Fill(tb2)
        For Each rw As DataRow In tb2.Rows
            Dim b As New clsBaseImponible
            b.TipoBase = rw("id_tipo_base")
            b.BaseI = rw("base")
            b.Iva = rw("IVA")
            b.Re = rw("RE")

            a.Bases.add(b)
            b = Nothing
        Next
        'ahora vemos el porte del albaran
        cad = "select * from t_portes where id_porte in (select id_porte from t_lineas_porte where id_albaran=" & a.Id_albaran & ")"
        Dim mda3 As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb3 As New DataTable
        mda3.Fill(tb3)
        If tb3.Rows.Count > 0 Then
            a.FecPorte = tb3.Rows(0)("fecha")
            a.Agencia = tb3.Rows(0)("agencia")
        End If
        tb3 = Nothing
        mda3.Dispose()
        '  mcon.Close()
        Return a


    End Function
    Public Function EstaAbonado(ByVal idpedido As Integer, ByVal Haymontaje As Boolean) As Boolean
        Dim cad As String
        cad = "select count(*) from t_albaranes where id_alb_abono<>0 and id_albaran in (select id_albaran from t_lineas_albaran where id_pedido<>0 and id_pedido=" & idpedido & " and montaje=" & IIf(Haymontaje = False, 0, 1) & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim Existe As Boolean = CBool(cmd.ExecuteScalar)

        mcon.Close()
        Return Existe
    End Function
    Public Function GetcantidadAbonada(ByVal idpedido As Integer, ByVal descripcion As String, ByVal Conmontaje As Boolean) As Decimal
        Dim cad As String = "select abs(isnull(sum(total),0)) from t_lineas_albaran where id_pedido=" & idpedido & " and descripcion=" & strsql(descripcion) & " and total<0 and montaje=" & IIf(Conmontaje = False, 0, 1)
        Dim Abono As Decimal
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Abono = cmd.ExecuteScalar
        mcon.Close()
        Return Abono
    End Function
    Public Sub ActualizaTratamientoenBONOS()
        'ESTO ESTA HECHO PARA ARREGLAR UN FALLO
        Dim TB As New DataTable
        Dim cad As String = "select id_alb_abono,id_albaran from t_albaranes where montaje=0 and id_alb_abono<>0"

        Dim MDA As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        MDA.Fill(TB)
        Dim cmd As New SqlCommand(cad, mcon)
        If TB.Rows.Count > 0 Then
            Dim i As Integer
            For i = 0 To TB.Rows.Count - 1
                cad = "update t_lineas_albaran set id_tratamiento=(select id_tratamiento from t_lineas_albaran where id_albaran=" & TB.Rows(i)("id_alb_abono") & " and id_tipo_producto=1) where id_albaran=" & TB.Rows(i)("id_albaran")
                cmd.CommandText = cad
                cmd.ExecuteNonQuery()

            Next i
            cmd = Nothing
        End If
        MsgBox("Actualizado " & TB.Rows.Count)
        mcon.Close()
    End Sub
    Public Function getIdAlbaranByIdPedido(ByVal idPedido As Long) As Long
        Dim cad As String = "select * from t_albaranes where id_pedido = " & idPedido
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count > 0 Then
            Return tb.Rows(0)("id_albaran")

        End If
        mcon.Open()
        mda.SelectCommand.CommandText = "select id_albaran from t_pedidos  where id_pedido = " & idPedido
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count > 0 Then
            Return tb.Rows(0)("id_albaran")

        End If
    End Function
    Public Function FiltroContrareembolsos(ByVal fecini As Integer, ByVal fecfin As Integer, Optional ByVal agencia As String = "", Optional ByVal pagados As Boolean = False, Optional ByVal OcultarPendientes As Boolean = False) As DataTable
        Dim cad As String = "select id_porte,fecha,importe,pagado,agencia,nombre_comercial from t_portes INNER JOIN t_clientes ON t_clientes.id_cliente=t_portes.id_cliente where importe<>0 and fecha>=" & fecini & " and fecha<=" & fecfin
        If agencia <> "" Then
            cad = cad & " and agencia in (select transportista from t_transportistas where etiqueta like " & strsql(agencia) & ")"
        End If
        If pagados = False Then
            cad = cad & " and pagado<>1"
        End If
        If OcultarPendientes = True Then
            cad = cad & " and pagado<>0"
        End If
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function

    Public Function GetContrareembolsosByCLiente(ByVal idcli As Integer, Optional ByVal pagados As Boolean = False, Optional ByVal OcultarPagados As Boolean = False) As DataTable
        Dim cad As String
        Dim filtro As String = ""
        If pagados = False Then
            filtro = " and pagado<>1"
        End If
        If OcultarPagados = True Then
            filtro = filtro & " and pagado<>0"
        End If
        cad = "select *,dbo.fecha(t_portes.fecha) as fechaPorte from t_portes  where importe<>0 and id_cliente=" & idcli & filtro & "  order by id_porte"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetContrareembolsosByIdPorte(ByVal idPorte As Integer) As Decimal
        Dim cad As String

        cad = "select isnull(sum(total),0) as Importe from t_contrareembolsos INNER JOIN t_lineas_porte ON t_contrareembolsos.id_albaran=t_lineas_porte.id_albaran INNER JOIN t_portes ON t_portes.id_porte=t_lineas_porte.id_porte where t_contrareembolsos.id_albaran in (select id_albaran from t_lineas_porte where id_porte=" & idPorte & ")"

        Dim cmd As New SqlCommand(cad, mcon)
        Dim total As Decimal
        mcon.Open()
        total = cmd.ExecuteScalar
        mcon.Close()
        Return total
    End Function
    Public Sub GrabaPagoPorte(ByVal idporte As Long, Optional ByVal pagado As Boolean = True)
        Dim cad As String = "UPDATE t_portes set pagado=" & IIf(pagado = True, 1, 0) & " where id_porte=" & idporte
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'ahora grabamos todos los albaranes de ese porte como pagado
        cmd.CommandText = "UPDATE t_contrareembolsos set pagado=" & IIf(pagado = True, 1, 0) & " where id_albaran in (select id_albaran from t_lineas_porte where id_porte=" & idporte & ")"
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetcomisionComercialByIdcliente(ByVal id As Integer) As Decimal
        Dim cad As String = "Select comision from t_comerciales where id_comercial=(select id_comercial from t_clientes where id_cliente=" & id & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        Dim comision As Decimal
        mcon.Open()
        comision = cmd.ExecuteScalar
        mcon.Close()
        Return comision
    End Function
    Public Function GetcomisionComercialByPedido(ByVal p As clsPedido) As Decimal
        Dim campo As String
        If FechaAcadena(p.Fechapedido) >= 20160101 And p.modo <> "F" Then
            campo = "comision_stock"
        Else
            campo = "comision"
        End If
        Dim cad As String = "Select " & campo & " from t_comerciales where id_comercial=(select id_comercial from t_clientes where id_cliente=" & p.id_cliente & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        Dim comision As Decimal
        mcon.Open()
        comision = cmd.ExecuteScalar
        mcon.Close()
        Return comision
    End Function
    Public Function GetClientesPendientesRecapitular(Optional ByVal AGencia As String = "") As DataTable
        Dim Filtro As String = ""
        If AGencia <> "" Then
            Filtro = " and transportista in (select transportista from t_transportistas where etiqueta like " & strsql(AGencia) & ") "
        End If
        Dim cad As String = "Select id_cliente, provincia, codigo,nombre_comercial, (Select count(*) from t_pedidos where fecha_salida<>0 and id_albaran=0  and montaje=0 and anulado=0 and fecha>=20100101 and id_cliente=t_clientes.id_cliente and montaje=0 ) as lentes, 0 as Montajes from t_clientes INNER JOIN m_provincias ON t_clientes.id_provincia=m_provincias.id_provincia  where (id_cliente in (select id_cliente from t_pedidos where fecha>=20100101 and fecha_salida<>0 and id_albaran=0) or id_cliente in (select id_cliente from t_pedidos_montajes where fecha>=20100101 and fecha_salida<>0 and id_albaran=0)) and (dias_envio='0' or dias_envio like '%" & CInt(Now.DayOfWeek) & "%')" & Filtro & " order by provincia,t_clientes.codigo"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function getIdPedidoByIdAlbaran(ByVal idAlbaran As Long) As Long
        Dim cad As String = "select * from t_albaranes where id_albaran = " & idAlbaran
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("id_pedido")
    End Function
    Public Function Biselado(ByVal idped As Integer) As Boolean
        Dim cad As String = "select count(*) from t_biselados where (id_pedido_d=" & idped & " or id_pedido_i=" & idped & ") and (precal<>0 or bisel<>0 or montura<>0)"
        Dim bisel As Boolean
        Dim Abierta As Boolean = False
        Dim cmd As New SqlCommand(cad, mcon)
        If mcon.State = ConnectionState.Closed Then
            mcon.Open()
        Else
            Abierta = True
        End If

        bisel = cmd.ExecuteScalar
        If Abierta = False Then
            mcon.Close()
        End If
        Return bisel

    End Function
    Public Function BiseladoMontaje(ByVal idMont As Integer) As Boolean
        Dim cad As String = "select count(*) from t_biselados where Montaje=" & idMont
        Dim bisel As Boolean
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        bisel = cmd.ExecuteScalar
        mcon.Close()
        Return bisel

    End Function

    Public Sub BajaLenteStock(ByVal id As Integer)
        Dim cad As String = "update t_lentes_stock set baja=1 where id_producto=" & id

        Dim Cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub Bajausuario(ByVal id As Integer)
        Dim cad As String = "update t_usuarios set baja=1 where id_usuario=" & id

        Dim Cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub Altausuario(ByVal id As Integer)
        Dim cad As String = "update t_usuarios set baja=0 where id_usuario=" & id

        Dim Cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Sub GrabaUsuario(ByVal us As clsUsuario)
        Dim cad As String
        If us.id = 0 Then
            'se trata de un usuario nuevo
            us.id = getMaxId("id_usuario", "t_usuarios") + 1
            cad = " INSERT INTO t_usuarios (id_usuario,nombre,apellidos,login,clave,cambio_clave,baja,comercial) VALUES " & _
            " (" & us.id & "," & strsql(us.nombre) & "," & strsql(us.apellidos) & "," & strsql(us.login) & "," & strsql(us.clave) & "," & IIf(us.CambiarClave = True, 1, 0) & "," & IIf(us.Baja = True, 1, 0) & _
            "," & IIf(us.Comercial = True, 1, 0) & ")"
        Else
            cad = "UPDATE t_usuarios set clave=" & strsql(us.clave) & ",cambio_clave=" & IIf(us.CambiarClave = False, 0, 1) & ",baja=" & IIf(us.Baja = False, 0, 1) & ",comercial=" & IIf(us.Comercial = True, 1, 0) & " where id_usuario=" & us.id

        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'ahora borramos los permisos de ese usuario
        cmd.CommandText = "DELETE FROM t_menus_usuario where id_usuario=" & us.id
        cmd.ExecuteNonQuery()
        If us.Menus.Count > 0 Then
            For j As Integer = 0 To us.Menus.Count - 1
                cmd.CommandText = "INSERT INTO t_menus_usuario (id_menu,id_usuario) VALUES (" & us.Menus(j) & "," & us.id & ")"
                cmd.ExecuteNonQuery()
            Next
        End If
        cmd.CommandText = " DELETE FROM t_permisos_usuarios where id_usuario=" & us.id
        cmd.ExecuteNonQuery()
        'escribimos los permisos
        With us.Acceso.Pedidos
            cmd.CommandText = " INSERT INTO t_permisos_usuarios (id_usuario,Acceso,Consultar,Crear,Modificar,eliminar,anular) VALUES " & _
            " (" & us.id & ",'Pedidos'," & IIf(.Consultar = False, 0, 1) & "," & IIf(.Crear = False, 0, 1) & "," & IIf(.Modificar = False, 0, 1) & _
            "," & IIf(.Eliminar = False, 0, 1) & "," & IIf(.Anular = False, 0, 1) & ")"
            cmd.ExecuteNonQuery()
        End With
        With us.Acceso.Albaranes
            cmd.CommandText = " INSERT INTO t_permisos_usuarios (id_usuario,Acceso,Consultar,Crear,Modificar,eliminar,anular) VALUES " & _
            " (" & us.id & ",'Albaranes'," & IIf(.Consultar = False, 0, 1) & "," & IIf(.Crear = False, 0, 1) & "," & IIf(.Modificar = False, 0, 1) & _
            "," & IIf(.Eliminar = False, 0, 1) & "," & IIf(.Anular = False, 0, 1) & ")"
            cmd.ExecuteNonQuery()
        End With
        With us.Acceso.Facturas
            cmd.CommandText = " INSERT INTO t_permisos_usuarios (id_usuario,Acceso,Consultar,Crear,Modificar,eliminar,anular) VALUES " & _
            " (" & us.id & ",'Facturas'," & IIf(.Consultar = False, 0, 1) & "," & IIf(.Crear = False, 0, 1) & "," & IIf(.Modificar = False, 0, 1) & _
            "," & IIf(.Eliminar = False, 0, 1) & "," & IIf(.Anular = False, 0, 1) & ")"
            cmd.ExecuteNonQuery()
        End With
        With us.Acceso.Usuarios
            cmd.CommandText = " INSERT INTO t_permisos_usuarios (id_usuario,Acceso,Consultar,Crear,Modificar,eliminar,anular) VALUES " & _
            " (" & us.id & ",'Usuarios'," & IIf(.Consultar = False, 0, 1) & "," & IIf(.Crear = False, 0, 1) & "," & IIf(.Modificar = False, 0, 1) & _
            "," & IIf(.Eliminar = False, 0, 1) & "," & IIf(.Anular = False, 0, 1) & ")"
            cmd.ExecuteNonQuery()
        End With
        With us.Acceso.Bono
            cmd.CommandText = " INSERT INTO t_permisos_usuarios (id_usuario,Acceso,Consultar,Crear,Modificar,eliminar,anular) VALUES " & _
            " (" & us.id & ",'Bonos'," & IIf(.Consultar = False, 0, 1) & "," & IIf(.Crear = False, 0, 1) & "," & IIf(.Modificar = False, 0, 1) & _
            "," & IIf(.Eliminar = False, 0, 1) & "," & IIf(.Anular = False, 0, 1) & ")"
            cmd.ExecuteNonQuery()
        End With
        'With us.Acceso.Acuerdos
        '    cmd.CommandText = " INSERT INTO t_permisos_usuarios (id_usuario,Acceso,Consultar,Crear,Modificar,eliminar,anular) VALUES " & _
        '    " (" & us.id & ",'acuerdos'," & IIf(.Consultar = False, 0, 1) & "," & IIf(.Crear = False, 0, 1) & "," & IIf(.Modificar = False, 0, 1) & _
        '    "," & IIf(.Eliminar = False, 0, 1) & "," & IIf(.Anular = False, 0, 1) & ")"
        '    cmd.ExecuteNonQuery()
        'End With
        With us.Acceso.Clientes
            cmd.CommandText = " INSERT INTO t_permisos_usuarios (id_usuario,Acceso,Consultar,Crear,Modificar,eliminar,anular) VALUES " & _
            " (" & us.id & ",'Clientes'," & IIf(.Consultar = False, 0, 1) & "," & IIf(.Crear = False, 0, 1) & "," & IIf(.Modificar = False, 0, 1) & _
            "," & IIf(.Eliminar = False, 0, 1) & "," & IIf(.Anular = False, 0, 1) & ")"
            cmd.ExecuteNonQuery()
        End With
        With us.Acceso.Tarifas
            cmd.CommandText = " INSERT INTO t_permisos_usuarios (id_usuario,Acceso,Consultar,Crear,Modificar,eliminar,anular) VALUES " & _
            " (" & us.id & ",'Tarifas'," & IIf(.Consultar = False, 0, 1) & "," & IIf(.Crear = False, 0, 1) & "," & IIf(.Modificar = False, 0, 1) & _
            "," & IIf(.Eliminar = False, 0, 1) & "," & IIf(.Anular = False, 0, 1) & ")"
            cmd.ExecuteNonQuery()
        End With
        With us.Acceso.Abono
            cmd.CommandText = " INSERT INTO t_permisos_usuarios (id_usuario,Acceso,Consultar,Crear,Modificar,eliminar,anular) VALUES " & _
            " (" & us.id & ",'Abono'," & IIf(.Consultar = False, 0, 1) & "," & IIf(.Crear = False, 0, 1) & "," & IIf(.Modificar = False, 0, 1) & _
            "," & IIf(.Eliminar = False, 0, 1) & "," & IIf(.Anular = False, 0, 1) & ")"
            cmd.ExecuteNonQuery()
        End With
        With us.Acceso.Portes
            cmd.CommandText = " INSERT INTO t_permisos_usuarios (id_usuario,Acceso,Consultar,Crear,Modificar,eliminar,anular) VALUES " & _
            " (" & us.id & ",'Portes'," & IIf(.Consultar = False, 0, 1) & "," & IIf(.Crear = False, 0, 1) & "," & IIf(.Modificar = False, 0, 1) & _
            "," & IIf(.Eliminar = False, 0, 1) & "," & IIf(.Anular = False, 0, 1) & ")"
            cmd.ExecuteNonQuery()
        End With
        With us.Acceso.Modelos
            cmd.CommandText = " INSERT INTO t_permisos_usuarios (id_usuario,Acceso,Consultar,Crear,Modificar,eliminar,anular) VALUES " & _
            " (" & us.id & ",'Modelos'," & IIf(.Consultar = False, 0, 1) & "," & IIf(.Crear = False, 0, 1) & "," & IIf(.Modificar = False, 0, 1) & _
            "," & IIf(.Eliminar = False, 0, 1) & "," & IIf(.Anular = False, 0, 1) & ")"
            cmd.ExecuteNonQuery()
        End With

        With (us.Acceso.Rentabilidad)
            cmd.CommandText = " INSERT INTO t_permisos_usuarios (id_usuario,Acceso,Consultar,Crear,Modificar,eliminar,anular) VALUES " & _
            " (" & us.id & ",'Rentabilidad'," & IIf(.Consultar = False, 0, 1) & "," & IIf(.Crear = False, 0, 1) & "," & IIf(.Modificar = False, 0, 1) & _
            "," & IIf(.Eliminar = False, 0, 1) & "," & IIf(.Anular = False, 0, 1) & ")"
            cmd.ExecuteNonQuery()
        End With
        mcon.Close()
    End Sub
    Public Function GetNotasByAlbaran(ByVal id As Integer) As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select t_notas_albaran.id_usuario,fecha,nombre+ ' ' + apellidos as usuario,nota from t_notas_albaran INNER JOIN t_usuarios ON t_usuarios.id_usuario=t_notas_albaran.id_usuario WHERE id_albaran=" & id & " order by fecha"
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb
    End Function
    Public Sub EiminaAlbaranById(ByVal idAlbaran As Long, Optional ByVal Abono As Boolean = False)
        Dim cad As String = "delete from t_albaranes where id_albaran = " & idAlbaran
        Dim cad2 As String = "delete from t_lineas_albaran where id_albaran = " & idAlbaran
        Dim AnulaPedido As String = "Update t_pedidos set fecha_salida=0,hora_salida=0,id_albaran=0 where id_pedido in (select id_pedido from t_lineas_albaran where montaje=0 and id_albaran=" & idAlbaran & ")"
        Dim AnulaPedidoMontaje As String = "Update t_pedidos_montajes set fecha_salida=0,id_albaran=0 where id_pedido_montaje in (select isnull(id_pedido,0) from t_lineas_albaran where montaje=1 and id_albaran=" & idAlbaran & ")"
        Dim mda As New SqlDataAdapter
        Dim cmd As New SqlCommand(AnulaPedido, mcon)
        mda.DeleteCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        'ahora le quitamos el coste de la comision del pedido 
        Dim Puntos As Integer = 0
        If Abono = False Then
            cmd.ExecuteNonQuery()
            cmd.CommandText = "DELETE FROM t_costos_pedido where id_pedido in (select id_pedido from t_lineas_albaran where montaje=0 and id_albaran=" & idAlbaran & ") and paso like '%comision%'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = AnulaPedidoMontaje
            cmd.ExecuteNonQuery()
            cmd.CommandText = "select isnull(sum(puntos),0) from t_puntos_pedido where id_pedido in (select id_pedido from t_lineas_albaran where id_albaran=" & idAlbaran & "  and montaje=0)"
            Puntos = cmd.ExecuteScalar
        Else
            cmd.CommandText = "select isnull(sum(puntos),0) from t_puntos_pedido where id_pedido in (select id_pedido from t_lineas_albaran where id_albaran=" & idAlbaran & " and montaje=0) and puntos<0"
            Puntos = cmd.ExecuteScalar
        End If
        If Puntos <> 0 Then
            cmd.CommandText = "UPDAte t_clientes set puntos=puntos-" & Puntos & " where id_cliente in (select id_cliente from t_albaranes where id_albaran=" & idAlbaran & ")"
            cmd.ExecuteNonQuery()
        End If
        cmd.CommandText = "DELETE FROM t_contrareembolsos where id_albaran=" & idAlbaran
        cmd.ExecuteNonQuery()
        cmd.CommandText = "DELETE FROM t_bases_albaran where id_albaran=" & idAlbaran
        cmd.ExecuteNonQuery()


        mda.DeleteCommand.ExecuteNonQuery()
        mda.DeleteCommand.CommandText = cad2
        mda.DeleteCommand.ExecuteNonQuery()

        '   mda.DeleteCommand.CommandText = "delete from t_debe_haber where concepto like 'albaran " & idAlbaran & "'"
        '  mda.DeleteCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub


    Public Function listadoVentasTotaleClientes(ByVal fecini As String, ByVal fecfin As String, Optional ByVal agrupado As Boolean = False) As DataTable

        Dim cad As String = "select sum(t_bases_albaran.base)as venta,(select isnull(sum(coste),0) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where  id_albaran in (select id_albaran from t_albaranes where id_alb_abono=0 and id_cliente=t_clientes.id_cliente and fecha>=" & _
        fecini & " and fecha<=" & fecfin & " and montaje=0))) as costes,codigo,nombre_comercial,(select count(*) from t_portes where id_cliente=t_clientes.id_cliente and  fecha>= " & fecini & " and fecha<=" & fecfin & ") as Portes " & _
        " from t_albaranes inner join t_bases_albaran On t_bases_albaran.id_albaran=t_albaranes.id_albaran inner join t_clientes on t_albaranes.id_cliente =t_clientes.id_cliente  where fecha>= " & fecini & " and fecha<=" & fecfin & _
        " and t_albaranes.id_albaran not in (select t_albaranes.id_albaran from t_lineas_albaran INNER JOIN t_albaranes ON t_lineas_albaran.id_albaran=t_albaranes.id_albaran where fecha>=" & fecini & " and fecha<=" & fecfin & " and descripcion like '%deuda%') and id_tipo_base<>0  group by t_clientes.id_cliente,codigo,nombre_comercial order by sum(base)"

        '        Dim cad As String = "select sum(t_bases_albaran.base)as venta,(select isnull(sum(coste),0) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where  id_albaran in (select id_albaran from t_albaranes where id_alb_abono=0 and id_cliente=t_clientes.id_cliente and fecha>=" & _
        '        fecini & " and fecha<=" & fecfin & "))) as costes,codigo,nombre_comercial" & _
        '        " from t_albaranes inner join t_bases_albaran On t_bases_albaran.id_albaran=t_albaranes.id_albaran inner join t_clientes on t_albaranes.id_cliente =t_clientes.id_cliente  where fecha>= " & fecini & " and fecha<=" & fecfin & _
        '"and t_albaranes.id_albaran not in ((select id_albaran from t_lineas_albaran INNER JOIN t_albaranes where fecha>=" & fecini & " and fecha<=" & fecfin & " and descripcion like '%deuda%')" & _
        '" group by codigo,nombre_comercial order by sum(base)"

        If agrupado = True Then
            cad = "select DATENAME(month,dbo.fecha(t_albaranes.fecha)) + '-' + convert(varchar(4),year(dbo.fecha(t_albaranes.fecha))) as Mes, sum(t_bases_albaran.base)as venta,codigo,nombre_comercial from t_albaranes inner join t_bases_albaran On t_bases_albaran.id_albaran=t_albaranes.id_albaran inner join t_clientes on t_albaranes.id_cliente =t_clientes.id_cliente  where fecha>= " & fecini & " and fecha<=" & fecfin & _
            " and t_albaranes.id_albaran not in (select id_albaran from t_lineas_albaran where descripcion like '%deuda%') and id_tipo_base<>0  group by t_clientes.id_cliente,codigo,nombre_comercial,DATENAME(month,dbo.fecha(t_albaranes.fecha))+'-'+convert(varchar(4),year(dbo.fecha(t_albaranes.fecha))),year(dbo.fecha(t_albaranes.fecha)),month(dbo.fecha(t_albaranes.fecha)) order by codigo,year(dbo.fecha(t_albaranes.fecha)),month(dbo.fecha(t_albaranes.fecha))"

        End If
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))

        Dim tb As New DataTable

        mcon.Open()
        mda.SelectCommand.CommandTimeout = 450
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function ExisteFormaPrecal(ByVal derecho As Integer, ByVal izquierdo As Integer) As Boolean
        Dim cad As String = "select count(*) from t_formas_precal where forma_R in (" & derecho & "," & -izquierdo & ") or forma_L in (" & -derecho & "," & izquierdo & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        ExisteFormaPrecal = CBool(cmd.ExecuteScalar)
        mcon.Close()
    End Function
    Public Sub GrabaFormaPrecal(ByVal nombre As String, ByVal derecho As Integer, ByVal izquierdo As Integer)
        Dim Cad As String = "INSERT INTO t_formas_precal select isnull(max(id_forma),0)+1," & strsql(nombre) & "," & derecho & "," & izquierdo & ",0 from t_formas_precal"
        Dim cmd As New SqlCommand(Cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function listadoVentasbycomercial(ByVal idcomercial As Integer, ByVal fecini As String, ByVal fecfin As String, Optional ByVal agrupado As Boolean = False) As DataTable
        Dim cad As String = "select (select isnull(sum(base),0) from t_bases_albaran where id_tipo_base<>0 and id_albaran not in (select id_albaran from t_lineas_albaran where descripcion like '%deuda%') and id_albaran in (select id_albaran from t_albaranes where  id_cliente =t_clientes.id_cliente  and fecha>= " & fecini & _
        " and fecha<=" & fecfin & ")) as venta,codigo,nombre_comercial,(select forma_pago from t_formas_pago where t_formas_pago.id_forma_pago=t_clientes.id_forma_pago) as [Forma de Pago]  " & _
        "from t_clientes  where id_comercial=" & idcomercial & " group by t_clientes.id_cliente,codigo,nombre_comercial,id_forma_pago "
        cad = "select  isnull(sum(base),0)   as venta,codigo,nombre_comercial,(select forma_pago from t_formas_pago where t_formas_pago.id_forma_pago=t_clientes.id_forma_pago) as [Forma de Pago]  from t_clientes INNER JOIN t_albaranes ON t_albaranes.id_cliente=t_clientes.id_cliente INNER JOIN t_bases_albaran ON " & _
        " t_bases_albaran.id_albaran=t_albaranes.id_albaran  where    fecha>=" & fecini & " and fecha<=" & fecfin & " and  id_comercial=" & idcomercial & " and  id_tipo_base<>0 and t_bases_albaran.id_albaran not in (select id_albaran from t_lineas_albaran where descripcion like '%deuda%') " & _
        " group by t_clientes.id_cliente,codigo,nombre_comercial,id_forma_pago"
        If agrupado = True Then
            cad = "select DATENAME(month,dbo.fecha(t_albaranes.fecha)) + '-' + convert(varchar(4),year(dbo.fecha(t_albaranes.fecha))) as Mes, sum(t_bases_albaran.base)as venta,codigo,nombre_comercial from t_albaranes inner join t_bases_albaran On t_bases_albaran.id_albaran=t_albaranes.id_albaran inner join t_clientes on t_albaranes.id_cliente =t_clientes.id_cliente  where id_comercial=" & idcomercial & " and fecha>= " & fecini & " and fecha<=" & fecfin & _
          " and t_albaranes.id_albaran not in (select id_albaran from t_lineas_albaran where descripcion like '%deuda%') and id_tipo_base<>0  group by t_clientes.id_cliente,codigo,nombre_comercial,DATENAME(month,dbo.fecha(t_albaranes.fecha))+'-'+convert(varchar(4),year(dbo.fecha(t_albaranes.fecha))),year(dbo.fecha(t_albaranes.fecha)),month(dbo.fecha(t_albaranes.fecha)) order by codigo,year(dbo.fecha(t_albaranes.fecha)),month(dbo.fecha(t_albaranes.fecha))"

        End If
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.SelectCommand.CommandTimeout = 180
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetAlbaranesSinFacturarByidCLienteAsociado(ByVal idcli As Integer, ByVal fecini As Integer, ByVal fecfin As Integer) As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select id_albaran from t_albaranes where  id_bono=0 and facturado=0 and fecha>=" & fecini & " and fecha<=" & fecfin & " and id_cliente in (select id_cliente_asociado from t_clientes_asociados where id_cliente=" & idcli & ")"
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    'Public Function AlbaranesSinFacturarAsociados(ByVal fecini As Integer, ByVal fecfin As Integer, ByVal periodo As String) As DataTable
    '    Dim cad As String = "select t_clientes.id_cliente,codigo,nombre_comercial, (select count(*) from t_albaranes where facturado=0 and id_bono=0 and fecha>=" & fecini & " and fecha<=" & fecfin & " and id_cliente in (select id_cliente_asociado from t_clientes_asociados where id_cliente=t_clientes.id_cliente)) as num_albaranes" & _
    '    " from t_clientes INNER JOIN t_clientes_asociados ON t_clientes.id_cliente=t_clientes_asociados.id_cliente where facturacion like " & strsql(periodo) & " group by t_clientes.id_cliente,codigo,nombre_comercial"
    '    Dim mda As New SqlDataAdapter(cad, mcon)
    '    Dim tb As New DataTable
    '    mcon.Open()
    '    mda.Fill(tb)
    '    mcon.Close()
    '    Return tb
    'End Function
    Public Sub FacturaLineaBono(ByVal idfact As Integer, ByVal idbono As Integer)
        Dim cad As String = ("Update t_lineas_bono set id_factura=" & idfact & " where id_factura=0 and envio<>0 and id_bono=" & idbono)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub ActualizaCaducidadBonoCliente(ByVal idBono As Integer, ByRef l As clsLineaBonoCliente)
        Dim cad As String = "UPDATE t_lineas_bono_cliente set caducidad=" & l.Caducidad & " where id_bono=" & l.idBono & " and id_bono_cliente=" & idBono
        Dim cmd As New SqlCommand(cad, mcon)
        Dim cnnabierta As Boolean = False
        If mcon.State = ConnectionState.Open Then
            cnnabierta = True
        Else
            mcon.Open()
        End If
        cmd.ExecuteNonQuery()
        'ahora actualizamos la caducidad del bono si esta es mas grande que la que tenia
        cmd.CommandText = "UPDATE t_bonos_cliente set caduca=" & l.Caducidad & " where id_bono_cliente=" & idBono & " and caduca<" & l.Caducidad
        cmd.ExecuteNonQuery()
        If cnnabierta = False Then
            mcon.Close()
        End If
    End Sub
    Public Sub FacturaBonoCliente(ByVal b As clsBonoCliente)
        Dim cad As String = ("Update t_bonos_cliente set id_factura=" & b.Factura & " where id_bono_cliente=" & b.id)
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function GetIdBonoByFactura(ByVal idFac As Integer) As Integer
        Dim cad As String = "select id_bono from t_lineas_bono where id_factura=" & idFac & " group by id_bono"
        Dim bono As Integer = 0
        Dim mda As New SqlDataAdapter(cad, mcon)

        Dim tb As New DataTable
        mda.Fill(tb)
        If tb.Rows.Count > 0 Then
            bono = tb.Rows(0)("id_bono")
        End If

        mcon.Close()
        Return bono
    End Function
    'Public Function AlbaranesSinFacturarBono(ByVal fecini As Integer, ByVal fecfin As Integer, ByVal periodo As String) As DataTable
    '    Dim cad As String = "select t_clientes.id_cliente,codigo,nombre_comercial,id_bono, (select count(*) from t_lineas_bono where id_factura=0 and envio=1 and id_bono=t_bonos.id_bono) as num_albaranes" & _
    '    " from t_clientes INNER JOIN t_bonos ON t_bonos.id_cliente=t_clientes.id_cliente where partea<>0 and facturacion like " & strsql(periodo) & " group by t_clientes.id_cliente,codigo,nombre_comercial,id_bono"
    '    Dim mda As New SqlDataAdapter(cad, mcon)
    '    Dim tb As New DataTable
    '    mcon.Open()
    '    mda.Fill(tb)
    '    mcon.Close()
    '    Return tb
    'End Function
    Public Function listadoVentasTipoLentes(ByVal fecini As String, ByVal fecfin As String, Optional ByVal Cliente As Long = 0) As DataTable
        Dim Cli As String = ""
        If Cliente <> 0 Then
            Cli = " and (id_cliente=" & Cliente & " or cod_envio=" & Cliente & " or id_bono in (select id_bono from t_bonos where id_cliente=" & Cliente & "))"
            'Cli = " and id_cliente=" & Cliente
        End If
        Dim Consulta As String = "CREATE VIEW consumo as (select * from t_lineas_albaran where  id_albaran in (select id_albaran from t_albaranes where fecha>= " & fecini & " and fecha<=" & fecfin & Cli & " and montaje=0 ) and id_tipo_producto=1 and id_modelo<>0)"
        Dim cad As String = "select t_modelos.nombre,(select material from m_materiales where id_material=t_modelos.material)as material," & _
        "(select tipo from m_tipologia where id_tipo=t_modelos.tipologia) as tipo,t_tratamientos.nombre as tratamiento" & _
        ",(select count(*) from consumo where precio>=0 and id_modelo=t_modelos.id_lente and id_tratamiento=t_tratamientos.id_tratamiento) as Unidades,(select count(*) from consumo where id_modo=1 and id_modelo=t_modelos.id_lente and id_tratamiento=t_tratamientos.id_tratamiento and precio>=0) as Stock, " & _
        "(select count(*) from consumo where id_modo=2 and id_modelo=t_modelos.id_lente and id_tratamiento=t_tratamientos.id_tratamiento and precio>=0) as Transformacion, " & _
         "(select count(*) from consumo where id_modo=3 and id_modelo=t_modelos.id_lente and id_tratamiento=t_tratamientos.id_tratamiento and precio>=0) as Fabricacion, " & _
         "(select count(*) from consumo where  id_modelo=t_modelos.id_lente and id_tratamiento=t_tratamientos.id_tratamiento and precio<0) as [Total Abonos]," & _
         "(select count(*) from consumo where id_modo=1 and  id_modelo=t_modelos.id_lente and id_tratamiento=t_tratamientos.id_tratamiento and precio<0) as [Abonos Stock]," & _
         "(select count(*) from consumo where id_modo=2 and  id_modelo=t_modelos.id_lente and id_tratamiento=t_tratamientos.id_tratamiento and precio<0) as [Abonos Transform.]," & _
        "(select count(*) from consumo where id_modo=3 and  id_modelo=t_modelos.id_lente and id_tratamiento=t_tratamientos.id_tratamiento and precio<0) " & _
        " as [Abonos Fabrica]" & _
        " from (consumo  INNER JOIN t_modelos on t_modelos.id_lente=consumo.id_modelo)  INNER JOIN t_tratamientos ON t_tratamientos.id_tratamiento=consumo.id_tratamiento  GROUP BY t_modelos.nombre,t_tratamientos.id_tratamiento,material,tipologia,t_modelos.id_LENTE,t_tratamientos.nombre"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim cmd As New SqlCommand(Consulta, mcon)
        Dim tb As New DataTable
        Try
            BorraConsulta("consumo")


            mcon.Open()
            cmd.ExecuteNonQuery()
            mda.SelectCommand.CommandTimeout = 240
            mda.Fill(tb)
            cmd.CommandText = "DROP VIEW CONSUMO"
            cmd.ExecuteNonQuery()
            mcon.Close()

        Catch ex As Exception
            cmd.CommandText = "DROP VIEW CONSUMO"
            cmd.ExecuteNonQuery()
            mcon.Close()
        End Try
        Return tb
    End Function
    Public Sub modificaAlbaran(ByVal a As clsAlbaran)
        Dim cad As String = ""

        Dim comisionComercial As Decimal
        comisionComercial = GetcomisionComercialByIdcliente(a.Cliente.id)
        ' de las lineas se borran y se hacen de nuevo igual que las bases
        cad = "update t_albaranes set total = " & Replace(a.Total, ",", ".") & ",fecha=" & a.fecha & " where id_albaran = " & a.Id_albaran
        Dim mda As New SqlDataAdapter
        mda.UpdateCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.UpdateCommand.ExecuteNonQuery()
        mda.DeleteCommand = New SqlCommand("delete from t_lineas_albaran where id_albaran = " & a.Id_albaran, mcon)
        mda.DeleteCommand.ExecuteNonQuery()
        mda.DeleteCommand.CommandText = "DELETE FROM t_bases_albaran where id_albaran=" & a.Id_albaran
        mda.DeleteCommand.ExecuteNonQuery()
        mda.DeleteCommand.CommandText = "DELETE FROM t_contrareembolsos where id_albaran=" & a.Id_albaran
        mda.DeleteCommand.ExecuteNonQuery()
        mda.InsertCommand = New SqlCommand
        mda.InsertCommand.Connection = mcon
        Dim lin As New clsAlbaranLinea
        Dim TotalContrareembolso As Decimal = 0
        Dim miva As Decimal
        Dim mre As Decimal
        a.Bases.Clear()
        For Each lin In a
            Dim b As New clsBaseImponible
            If lin.id_tipo_Producto <> 4 Then

                If lin.id_tipo_Producto = 0 Then 'se trata de un porte
                    b = GetIvaByTipo(1)
                    b.TipoBase = 0
                    b.Re = 0
                Else
                    b = GetIvaByTipo(2)
                    b.TipoBase = 1
                    b.BaseI = lin.precio * (1 - lin.dto / 100)
                End If


            ElseIf (lin.id_tipo_Producto = 4 And lin.c1 = 0) Then

                b = GetIvaByTipo(2)
                b.TipoBase = 2
            ElseIf (lin.id_tipo_Producto = 4 And lin.c1 <> 0) Then

                b = GetIvaByTipo(1)
                b.TipoBase = 4

            End If
            b.BaseI = lin.precio * (1 - lin.dto / 100)
            If a.Cliente.SinIva = True Then
                b.Iva = 0
            End If
            If a.Cliente.recargo = False Then
                b.Re = 0
            End If
            miva = b.Iva
            mre = b.Re
            'ahora vamos añadiendo la base si no la encontramos o la sumamos si la encontramos
            Dim Encontrada As Boolean = False
            For Each base As clsBaseImponible In a.Bases
                If b.TipoBase = base.TipoBase Then
                    Encontrada = True
                    base.BaseI += b.BaseI
                    Exit For
                End If
            Next
            If Encontrada = False Then
                a.Bases.add(b)
            End If

            'If a.Cliente.contrareembolso = True Then


            Dim SubTotal As Decimal = 0
            SubTotal = lin.precio * (1 - lin.dto / 100)
            'aqui calculamos el total de contrareembolso que vamos a grabar

            miva = SubTotal * lin.Iva / 100
            mre = SubTotal * lin.Re / 100
            TotalContrareembolso = TotalContrareembolso + SubTotal + miva + mre
            'End If
            cad = "insert into t_lineas_albaran values(" & _
            a.Id_albaran & "," & lin.id_tipo_Producto & "," & lin.id_modelo & "," & lin.id_tratamiento & _
            "," & lin.id_coloracion & "," & lin.id_modo & "," & lin.c1 & ",'" & lin.c2 & "','" & _
            lin.descripcion & "'," & Replace(lin.precio, ",", ".") & "," & Replace(lin.dto, ",", ".") & "," & _
            Replace(lin.total, ",", ".") & "," & lin.idpedido & "," & IIf(lin.Montaje = True, 1, 0) & "," & NumSql(lin.Iva) & _
            "," & NumSql(lin.Re) & ")"
            mda.InsertCommand.CommandText = cad
            mda.InsertCommand.ExecuteNonQuery()
        Next
        'ahora grabamos el contrareembolso en caso de que el cliente lo tenga
        If a.Cliente.contrareembolso = True Then
            If Not (a.Total < 0 And a.Cliente.DeudaPendiente.Pendiente > 0) Then ' si es un abono y ademas tiene deuda pendiente no se hace contrareembolso
                cad = "INSERT INTO t_contrareembolsos (id_albaran,id_cliente,total,pagado) VALUES (" & a.Id_albaran & "," & a.Cliente.id & "," & NumSql(Format(TotalContrareembolso, "0.00")) & ",0)"
                mda.InsertCommand.CommandText = cad
                mda.InsertCommand.ExecuteNonQuery()
            End If
        End If
        'ahora tenemos que grabar las bases del albaran
        Dim Pendiente As Decimal
        Pendiente = TotalContrareembolso
        Pendiente = Format(Pendiente, "0.00")
        ' cad = "Update t_debe_haber set debe=" & NumSql(Pendiente) & " where concepto like 'albaran " & a.Id_albaran & "'"
        'mda.InsertCommand.CommandText = cad
        'mda.InsertCommand.ExecuteNonQuery()

        'ahora tenemos que grabar las bases del albaran
        For Each base As clsBaseImponible In a.Bases
            cad = "INSERT INTO t_bases_albaran (id_albaran,base,iva,re,id_tipo_base) VALUES (" & a.Id_albaran & "," & NumSql(base.BaseI) & "," & NumSql(base.Iva) & "," & NumSql(base.Re) & "," & NumSql(base.TipoBase) & ")"
            mda.InsertCommand.CommandText = cad
            mda.InsertCommand.ExecuteNonQuery()
        Next
        'vamos a updatear la comisioncomercial
        cad = "Update t_costos_pedido set coste=(select sum(total)*" & NumSql(comisionComercial) & "/ 100 from t_lineas_albaran where id_albaran=" & a.Id_albaran & " and id_pedido=t_costos_pedido.id_pedido ) where id_pedido in (select id_pedido from t_lineas_albaran where id_albaran=" & a.Id_albaran & " and montaje=0 ) and paso like 'comision%'"
        mda.UpdateCommand.CommandText = cad
        mda.UpdateCommand.ExecuteNonQuery()
        'ahora vamos a ver que usuario modifica el albaran
        cad = "Insert into t_albaran_modificado (id_albaran,id_usuario) VALUES (" & a.Id_albaran & "," & mUsuario.id & ")"
        mda.InsertCommand.CommandText = cad
        mda.InsertCommand.ExecuteNonQuery()
        mcon.Close()

    End Sub
    Public Function AlbContrareembolsoConPorte(ByVal id As Integer) As Boolean
        Dim cad As String = "select count(*) from t_lineas_porte INNER JOIN t_portes ON t_portes.id_porte=t_lineas_porte.id_porte where  t_portes.importe>0 and t_lineas_porte.id_albaran=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        Dim Existe As Boolean
        mcon.Open()
        Existe = cmd.ExecuteScalar
        mcon.Close()
        Return Existe
    End Function
    Public Sub EliminaAcuerdoCliente(ByVal id_cliente As Integer, ByVal acuerdo As Integer)
        'esta funcion elimina la relacion entre un cliente y un acuerdo
        Dim cad As String = "delete from t_acuerdos_clientes where id_cliente = " & id_cliente & " and id_acuerdo=" & acuerdo
        Dim mda As New SqlDataAdapter
        mda.DeleteCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.DeleteCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub EliminaCantidadAcuerdoByCli(ByVal id_cliente As Integer, ByVal acuerdo As Integer, ByVal mes As Integer, ByVal año As Integer)
        'esta funcion elimina la relacion entre un cliente y un acuerdo
        Dim cad As String = "delete from t_cantidades_cliente where id_cliente = " & id_cliente & " and id_acuerdo=" & acuerdo & " and mes=" & mes & " and año=" & año
        Dim mda As New SqlDataAdapter
        mda.DeleteCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.DeleteCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Sub ActualizaEtiquetasLote(ByVal idlote As Integer, ByVal diferencia As Integer)
        Dim cad As String = "Update t_lotes_semiterminado set cantidad=cantidad+" & diferencia & " where id_lote=" & idlote & vbNewLine & _
        "Update t_etiquetas_lote_semiterminado set cantidad=cantidad+" & diferencia & " where id_lote=" & idlote & vbNewLine & _
        "DELETE from t_etiquetas_lote_semiterminado where cantidad=0"

        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteScalar()
        mcon.Close()
    End Sub

    Public Function ListadoVentasTotalesProvincias(ByVal fecini As Long, ByVal fecfin As Long) As DataTable
        'aqui los albaranes normales de cada uno
        Dim cad As String = "select provincia,sum(t_albaranes.total) as totalAlbaranes from " & _
        "(t_albaranes inner join t_clientes on t_clientes.id_cliente = t_albaranes.id_cliente)" & _
        " inner join m_provincias on t_clientes.id_provincia = m_provincias.id_provincia where t_albaranes.id_cliente<>0 and " & _
        "fecha>=" & fecini & " and fecha <=" & fecfin & " group by provincia"
        'aqui los albarananes del bono




        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mda.SelectCommand.CommandTimeout = 180
        Dim tb As New DataTable

        mda.Fill(tb)
        'borramos la vista

        Return tb
    End Function

    Public Function GetEmpresa() As clsEmpresa
        Dim Empresa As New clsEmpresa
        Dim cad As String = "select * from t_empresas"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable

        '  mcon.Open()
        mda.Fill(tb)
        ' mcon.Close()
        Empresa.Nombre = tb.Rows(0)("empresa")
        Empresa.Direccion = tb.Rows(0)("direccion")
        Empresa.Registro = tb.Rows(0)("reg_mercantil")
        Empresa.Cif = tb.Rows(0)("cif")
        ' Empresa.Telefonos = tb.Rows(0)("telefonos")


        Return Empresa
    End Function
    Public Function GetGruposModelos() As DataTable
        Dim cad As String = "Select * from t_grupos_modelos order by grupo"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        mda.Dispose()
        Return tb
    End Function
    Public Function GetGruposModelosStock() As DataTable
        Dim cad As String = "Select * from t_grupos_modelos where id_grupo in (select id_grupo from t_modelos where id_lente in (select id_modelo from t_lentes_stock)) order by grupo"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        mda.Dispose()
        Return tb
    End Function
    Public Function GetCilindros(ByVal idmodelo As Integer, ByVal idtratamiento As Integer, Optional ByVal traspuesta As Boolean = False) As DataTable
        Dim cad As String
        'cargamos primero el cilindro en negativo
        cad = "(select distinct (-cilindro) as cilindro from t_lentes_stock where id_modelo =" & idmodelo & " and baja=0 and (tratamiento=0 or tratamiento=" & idtratamiento & ")  and cilindro<>0 group by cilindro )" & _
        " UNION (select distinct cilindro from t_lentes_stock where id_modelo =" & idmodelo & " and baja=0 and (tratamiento=0 or tratamiento=" & idtratamiento & ")  group by cilindro ) order by cilindro"

        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable

        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb



    End Function
    Public Function GetEsferas(ByVal idmodelo As Integer, ByVal cilindro As Single, ByVal idtratamiento As Integer, Optional ByVal Traspuesta As Boolean = False) As DataTable
        Dim cad As String
        Dim esfera As String
        If cilindro >= 0 Then
            esfera = "esfera"
        Else
            cilindro = Math.Abs(cilindro)
            esfera = "(esfera+cilindro) as esfera"


        End If
        cad = "select distinct " & esfera & " from t_lentes_stock where id_modelo =" & idmodelo & " and (tratamiento=0 or tratamiento=" & idtratamiento & ") and baja=0 and cilindro=" & Replace(cilindro, ",", ".") & "  order by esfera"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetDiametrosStockbyModelo(ByVal idmodelo As Integer, ByVal idTratamiento As Integer, ByVal cilindro As Single, ByVal esfera As Single) As DataTable
        ' si el cilindro es negativo hay que trasponer
        If cilindro < 0 Then
            cilindro = Math.Abs(cilindro)
            esfera = esfera - cilindro
        End If
        Dim cad As String = " select distinct diametro,case Tratamiento when " & idTratamiento & " then 0 else 1 end as trat,convert(nvarchar(2),diametro)+ case tratamiento when " & idTratamiento & " then '' else '*' end + ' ('+ convert(nvarchar(7),stock) + ')' as diam  from t_lentes_stock where baja=0 and id_modelo=" & idmodelo & " and (tratamiento=" & idTratamiento & " or tratamiento=0) and cilindro=" & Replace(cilindro, ",", ".") & " and esfera=" & Replace(esfera, ",", ".") & " order by trat,diametro"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetDiametros(ByVal idModelo As Integer) As DataTable
        Dim cad As String
        cad = "select distinct diametro from t_lentes_stock where id_modelo =" & idModelo & " and baja=0  group by diametro order by diametro"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad))
        Dim tb As New DataTable
        mda.SelectCommand.Connection = mcon
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function ListadoVentasProvincias(ByVal fecini As Long, ByVal fecfin As Long) As DataTable
        Dim cad As String = "SELECT t_clientes.id_provincia,provincia from m_provincias INNER JOIN t_clientes ON m_provincias.id_provincia=t_clientes.id_provincia where id_cliente in" & _
        "(select id_cliente from t_albaranes where " & _
        " fecha >= " & fecini & " And fecha <= " & fecfin & " and id_cliente=t_clientes.id_cliente )" & _
       " group by t_clientes.id_provincia, PROVINCIA  order by Provincia"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetPiques() As DataTable
        Dim tb As New DataTable
        Dim cad As String = "select * from t_piques order by id_pique"
        Dim cmd As New SqlDataAdapter(cad, mcon)
        cmd.Fill(tb)
        Return tb
    End Function

    Public Function GetPiqueByid(ByVal id As Integer) As clspique
        Dim cad As String = "select * from t_piques where id_pique=" & id
        Dim p As New clspique
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        If tb.Rows.Count = 1 Then
            p.id = tb.Rows(0)("id_Pique")
            p.Nombre = tb.Rows(0)("nombre")
            p.Lenticular = tb.Rows(0)("Lenticular")
            p.Precio = tb.Rows(0)("precio")
        End If
        tb = Nothing
        Return p
    End Function

    Public Function GetBloqueos() As DataTable
        Dim cad As String = "select orden,case  when usuario<0 then (select nombre_comercial from t_clientes where -id_cliente=usuario) else (select nombre+ ' ' + apellidos  from t_usuarios where id_usuario=usuario) END as Usuario,pedidos,albaranes from t_bloqueos order by orden"

        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)

        Return tb
    End Function

    Public Sub LiberaBloqueos(ByVal orden As Integer, ByVal tabla As String)
        Dim cad As String = "DELETE FROM t_bloqueos where orden=" & orden & " and " & tabla & "=1"
        Dim cnnabierta As Boolean = False
        Dim cmd As New SqlCommand(cad, mcon)
        If mcon.State = ConnectionState.Open Then
            cnnabierta = True
        Else
            mcon.Open()
        End If
        cmd.ExecuteNonQuery()
        If cnnabierta = False Then
            mcon.Close()
        End If
    End Sub
    Public Sub GrabaNotaAlbaran(ByVal idAlbaran As Integer, ByVal usuario As Integer, ByVal fecha As Integer, ByVal nota As String)
        Dim cad As String = "INSERT INTO t_notas_albaran (id_albaran,id_usuario,fecha,nota) VALUES (" & idAlbaran & "," & usuario & "," & fecha & "," & strsql(nota) & ")"
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()

    End Sub

    Public Sub GrabaPique(ByVal p As clspique, Optional ByVal Eliminar As Boolean = False)
        Dim cad As String
        If Eliminar = True Then
            cad = "DELETE FROM t_piques WHERE id_pique=" & p.id

        ElseIf p.id = 0 Then 'se trata de un nuevo tipo de pique, insertamos
            p.id = getMaxId("id_pique", "t_piques") + 1
            cad = "INSERT INTO t_piques (id_pique,nombre,precio,lenticular) VALUES (" & p.id & "," & strsql(p.Nombre) & "," & NumSql(p.Precio) & "," & NumSql(p.Lenticular) & ")"
        Else
            cad = "UPDATE T_piques set nombre=" & strsql(p.Nombre) & ",precio=" & NumSql(p.Precio) & ",lenticular=" & NumSql(p.Lenticular) & " where id_pique=" & p.id

        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function ListadoVentasClientesProvincias(ByVal Provincia As Integer, ByVal fecini As Long, ByVal fecfin As Long) As DataTable
        Dim cadnormal As String = "SELECT codigo,t_clientes.nombre_comercial,(SELECT sum(t_albaranes.total) as venta from " & _
        " t_albaranes WHERE fecha>=" & fecini & " and fecha <=" & fecfin & " AND ID_CLIENTE=t_clientes.id_cliente" & _
        ")   as Total,(select isnull(sum(coste),0) from t_costos_pedido where id_pedido in (select id_pedido from t_pedidos where  id_albaran in (select id_albaran from t_albaranes where id_alb_abono=0 and id_cliente=t_clientes.id_cliente   and fecha>=" & _
        fecini & " and fecha<=" & fecfin & "))) as costes from t_clientes " & _
        " where id_cliente<>0 and " & _
         "  id_provincia=" & Provincia & _
        " group by codigo,id_cliente,nombre_comercial "

        'BorraConsulta("ventasclientesprovincia")
        'mcon.Open()
        'Dim cmd As New SqlCommand(cad, mcon)
        'cmd.ExecuteNonQuery()
        ' cad = "select nombre_comercial, sum(total) as Facturacion,sum(costes) as coste from VentasClientesProvincia group by nombre_comercial order by nombre_comercial"
        Dim mda As New SqlDataAdapter(New SqlCommand(cadnormal, mcon))
        mda.SelectCommand.CommandTimeout = 180
        Dim tb As New DataTable
        ' MsgBox(Provincia)
        mda.Fill(tb)
        'ahora borramos la vista
        '      cmd.CommandText = "DROP VIEW VentasClientesProvincia"
        '     cmd.ExecuteNonQuery()
        '    mcon.Close()
        Return tb
    End Function
    Public Function GetClientesAsociados(ByVal idCli As Integer) As DataTable
        'si ya hemos cargado
        Dim cad As String = "select id_cliente,codigo,nombre_comercial from t_clientes where id_cliente in (select id_cliente_asociado from t_clientes_asociados where id_cliente=" & idCli & ")"

        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)

        mcon.Close()

        Return tb

    End Function
    Public Sub GrabaclientesAsociados(ByVal idCli As Integer, ByVal tb As DataTable)
        Dim cad As String = "DELETE FROM t_clientes_asociados where id_cliente=" & idCli
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        Dim rw As DataRow
        For Each rw In tb.Rows
            cmd.CommandText = "INSERT INTO t_clientes_asociados (id_cliente,id_cliente_asociado) VALUES (" & idCli & "," & rw("id_asociado") & ")"
            cmd.ExecuteNonQuery()
        Next
        mcon.Close()
    End Sub
    Public Function ListadoVentasClientesRangoFecha(ByVal fecini1 As Long, ByVal fecfin1 As Long, ByVal fecini2 As Long, ByVal fecfin2 As Long, Optional ByVal idComercial As Integer = 0) As DataTable
        Dim rango1 As String = "[Desde " & cadenaAfecha(fecini1) & " a " & cadenaAfecha(fecfin1) & "]"
        Dim rango2 As String = "[Desde " & cadenaAfecha(fecini2) & " a " & cadenaAfecha(fecfin2) & "]"
        Dim comercial As String = ""
        If idComercial <> 0 Then
            comercial = " and id_comercial=" & idComercial
        End If
        Dim cadnormal1 As String = "select codigo,t_clientes.nombre_comercial,poblacion,cp,email,(SELECT sum(t_albaranes.total)  from " & _
        " t_albaranes WHERE fecha>=" & fecini1 & " and fecha <=" & fecfin1 & " AND (id_cliente=t_clientes.id_cliente" & _
        " )) as total1 ,0 as total2 from t_clientes " & _
        " where  t_clientes.id_cliente<>0" & comercial & " group by t_clientes.id_cliente,nombre_comercial,codigo,poblacion,cp,email "

        'creamos la vista
        Dim cadnormal2 As String = "select codigo,t_clientes.nombre_comercial,poblacion,cp,email,0 as total1,(SELECT sum(t_albaranes.total)   from " & _
        " t_albaranes WHERE fecha>=" & fecini2 & " and fecha <=" & fecfin2 & " AND (id_cliente=t_clientes.id_cliente" & _
        " ))   as total2 from t_clientes " & _
        " where t_clientes.id_cliente<>0 " & comercial & "group by id_cliente,nombre_comercial,codigo,poblacion,cp,email "

        Dim cad As String = "CREATE VIEW VentasClientesRangos as ((" & cadnormal1 & ") UNION (" & cadnormal2 & "))"
        BorraConsulta("ventasclientesrangos")
        mcon.Open()
        Dim cmd As New SqlCommand(cad, mcon)
        cmd.CommandTimeout = 240
        cmd.ExecuteNonQuery()
        cad = "select nombre_comercial,codigo,poblacion,cp,email, sum(total1) as " & rango1 & ",sum(total2) as " & rango2 & ",sum(total1)-sum(total2) as diferencia from VentasClientesRangos group by nombre_comercial,codigo,poblacion,cp,email order by nombre_comercial"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mda.SelectCommand.CommandTimeout = 180
        mda.Fill(tb)
        'ahora borramos la vista
        cmd.CommandText = "DROP VIEW VentasClientesrangos"
        cmd.ExecuteNonQuery()
        mcon.Close()
        'ahora vamos a eliminar aquellas lineas donde los totales en ambos rangos sea 0
        Dim i As Integer
        For i = tb.Rows.Count - 1 To 0 Step -1
            If IIf(IsDBNull(tb.Rows(i)(5)), 0, tb.Rows(i)(5)) = 0 And IIf(IsDBNull(tb.Rows(i)(6)), 0, tb.Rows(i)(6)) = 0 Then
                tb.Rows.RemoveAt(i)
            End If
        Next
        Return tb
    End Function
    Public Function GetRemesa(ByVal fecha As Integer, ByVal fin As Integer) As DataTable
        Dim cad As String = "select id_factura from t_facturas where  vencimiento>=" & fecha & " and vencimiento<=" & fin & " and id_forma_pago in (select id_forma_pago from t_formas_pago where remesa<>0)"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetTotalAlbaranesNofacturados(ByVal fecini As Long, ByVal fecfin As Long, ByVal idcliente As Long) As Decimal
        Dim cad As String
        cad = "select sum(base) as importe from t_bases_albaran where id_albaran in (select id_albaran from t_albaranes where (id_cliente=" & idcliente & " or id_cliente in (select id_cliente_asociado from t_clientes_asociados where id_cliente=" & idcliente & ")) and facturado=0 and fecha>=" & fecini & " and fecha<=" & fecfin & ")"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        mda.Fill(tb)
        mcon.Close()
        Return tb.Rows(0)("importe")
    End Function
    Public Sub PasarAlbaranaB(ByVal idalb As Long, ByVal cliente As Integer)
        'aqui cogemos un albaran y lo pasamos a cliente al contado, grabando su idcliente en el campo cod_envio
        Dim cad As String = "UPDATE t_albaranes SET facturado=1  where id_albaran=" & idalb
        Dim mda As New SqlDataAdapter
        mda.UpdateCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.UpdateCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function CompruebaAlbaranAbonado(ByVal idAlbaran As Long) As Long
        'compruebo si un lbaran ya se ha abonado previamente para que no se pueda volver a abonar
        ' compruebo si existe el albaran de abono con id_alb_abono ya abonado
        Dim cad As String = "Select id_albaran from t_albaranes where id_alb_abono= " & idAlbaran
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        mda.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            Return 0
        Else
            Return tb.Rows(0)("id_albaran")
        End If

    End Function
    Public Function getLineasAlbaranNoFacturadosTB(ByVal fecini As Long, ByVal fecfin As Long, ByVal id_cliente As Long) As DataTable
        Dim cad As String = "select * from t_lineas_albaran inner join t_albaranes on t_lineas_albaran.id_albaran=" & _
        "t_albaranes.id_albaran where fecha>=" & fecini & " and fecha <=" & fecfin & " and (id_cliente =" & id_cliente & _
        " OR ID_CLIENTE IN (select id_asociado  from t_clientes_asociados where id_cliente=" & id_cliente & ")) and facturado = 0 order by t_lineas_albaran.id_albaran"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function getLineasAlbaranByPedido(ByVal Ped As clsPedido) As ArrayList
        'devolvemos todas las lineas de un albaran que tengan ese pedido
        Dim cad As String = "select * from t_lineas_albaran where id_pedido=" & Ped.id
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim lista As New ArrayList
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Dim ml As clsAlbaranLinea
        For Each rw As DataRow In tb.Rows
            ml = New clsAlbaranLinea
            ml.c1 = rw("c1")
            ml.c2 = rw("c2")
            ml.id_coloracion = rw("id_coloracion")
            ml.descripcion = rw("descripcion")
            ml.id_modelo = rw("id_modelo")
            ml.id_modo = rw("id_modo")
            ml.id_tipo_Producto = rw("id_tipo_producto")
            ml.id_tratamiento = rw("id_tratamiento")
            ml.Montaje = CBool(rw("montaje"))
            If ml.id_tratamiento <> 0 Then ml.tratamiento = getTratamientoById(ml.id_tratamiento)
            If ml.id_coloracion <> 0 Then ml.coloracion = GetColorAlbaran(ml.id_coloracion)
            If ml.id_modelo <> 0 Then ml.modelo = GetNombreModeloByID(ml.id_modelo)
            If ml.id_modo <> 0 Then ml.modo = getModoById(ml.id_modo)
            ml.idpedido = rw("id_pedido")
            ml.modelo = CBool(rw("montaje"))
            ml.dto = rw("dto")
            ml.precio = rw("precio")
            ml.total = rw("total")
            ml.Iva = rw("Iva")
            ml.Re = rw("re")
            lista.Add(ml)
        Next

        Return lista
    End Function
    Public Function getAlbaranesNoFacturadosTB(ByVal fecini As Long, ByVal fecfin As Long, ByVal id_cliente As Long) As DataTable
        Dim cad As String = "select * from t_albaranes where fecha>=" & fecini & " and fecha <=" & fecfin & " and (id_cliente =" & id_cliente & _
        " OR ID_CLIENTE IN (select id_cliente_asociado  from t_clientes_asociados where id_cliente=" & id_cliente & "))  and id_bono=0 and facturado = 0 order by id_albaran"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function InfIncidencias(ByVal fecini As Integer, ByVal fecfin As Integer) As DataTable
        Dim cad As String = "select incidencia,count(*) as total from t_ordenes_trabajo INNER JOIN t_incidencias On t_ordenes_trabajo.id_incidencia=t_incidencias.id_incidencia where fecha>=" & fecini & " and fecha<=" & fecfin & "  group by incidencia"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Return tb

    End Function
    Public Function GetMinutosDescanso(ByVal Departamento As String) As Integer
        If InStr(Departamento, "Espera") <> 0 Then
            Departamento = "LOA"
        End If
        Dim cad As String = "select entrada,salida from t_horarios_departamento where departamento like " & strsql("%" & Departamento)
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mda.Fill(tb)
        Dim minutos As Integer = 0
        If tb.Rows.Count <> 0 Then
            Dim horas As Integer = 24 - tb.Rows(0)("salida") + tb.Rows(0)("entrada")
            minutos = horas * 60
        End If
        Return minutos
    End Function
    Public Function DameDatatable(ByVal Tabla As String) As DataTable
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(Tabla, mcon)
        mda.SelectCommand.CommandTimeout = 180
        mda.Fill(tb)
        Return tb
    End Function

    Public Function GetDesechosByidModelo(ByVal idModelo As Integer) As DataTable
        Dim cad As String = "Select *,(select nombre from t_tratamientos where id_tratamiento=t_almacen_desechos.id_tratamiento) as tratamiento from t_almacen_desechos where id_modelo=" & idModelo & " and cantidad>0 order by id_tratamiento,diametro,cilindro,esfera"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        'Dim des As New clsDesecho

        Return tb
    End Function
    Public Function GetDesecho(ByVal id As Integer) As clsDesecho
        Dim cad As String = "Select * from t_almacen_desechos where id_desecho=" & id
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Dim des As New clsDesecho
        If tb.Rows.Count > 0 Then
            des.Id = tb.Rows(0)("id_desecho")
            des.idModelo = tb.Rows(0)("id_modelo")
            des.Adicion = tb.Rows(0)("adicion")
            des.Cantidad = tb.Rows(0)("cantidad")
            des.cilindro = tb.Rows(0)("cilindro")
            des.diametro = tb.Rows(0)("diamtetro")
            des.Eje = tb.Rows(0)("eje")
            des.esfera = tb.Rows(0)("esfera")
            des.idTratamiento = tb.Rows(0)("id_tratamiento")
            des.Ojo = tb.Rows(0)("ojo")
        End If
        Return des

    End Function
    Public Function GetDesecho(ByVal idModelo As Integer, ByVal idTratamiento As Integer, ByVal diametro As Integer, ByVal cilindro As Single, ByVal esfera As Single, ByVal eje As Integer, ByVal adicion As Integer, Optional ByVal ojo As String = "") As clsDesecho
        Dim cad As String
        Dim Potencia As String
        If cilindro + esfera >= 0 Then
            Potencia = "="
        Else
            Potencia = ">="
        End If
        'If EsMonoFocal(idModelo) Then
        cad = "(Select * from t_almacen_desechos where cantidad>0 and id_modelo=" & idModelo & " and id_tratamiento=" & idTratamiento & " and diametro" & Potencia & NumSql(diametro) & " and cilindro=" & NumSql(cilindro) & " and esfera=" & NumSql(esfera) & _
        " ) UNION (Select * from t_almacen_desechos where cantidad>0 and id_modelo=" & idModelo & " and (id_tratamiento=0 or id_tratamiento=" & IIf(idTratamiento > 1, 1, 0) & ") and diametro" & Potencia & NumSql(diametro) & " and cilindro=" & NumSql(cilindro) & " and esfera=" & NumSql(esfera) & _
        ")order by id_tratamiento,diametro,cantidad desc"
        'End If
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Dim des As New clsDesecho
        If tb.Rows.Count > 0 Then
            des.Id = tb.Rows(0)("id_desecho")
            des.idModelo = tb.Rows(0)("id_modelo")
            des.Adicion = tb.Rows(0)("adicion")
            des.Cantidad = tb.Rows(0)("cantidad")
            des.cilindro = tb.Rows(0)("cilindro")
            des.diametro = tb.Rows(0)("diametro")
            des.Eje = tb.Rows(0)("eje")
            des.esfera = tb.Rows(0)("esfera")
            des.Ojo = tb.Rows(0)("ojo")
            des.idTratamiento = tb.Rows(0)("id_tratamiento")
        End If
        Return des

    End Function
    Public Function GetClientesByProvincia(ByVal id As Integer) As DataTable
        Dim cad As String = "select " & IIf(id = 0, "(select provincia from m_provincias where id_provincia=t_clientes.id_provincia),", "") & " * from t_clientes where id_provincia" & IIf(id = 0, ">", "=") & id
        If mUsuario.Comercial <> 0 Then
            cad = cad & " and id_comercial=" & mUsuario.id
        End If
        cad = cad & " order by id_provincia"
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetIdDesecho(ByVal modelo As Integer, ByVal diametro As Integer, ByVal idtratamiento As Integer, ByVal cilindro As Single, ByVal esfera As Single, ByVal eje As Integer, ByVal adicion As Single) As Integer
        Dim cad As String = "select id_desecho from t_almacen_desechos where id_modelo=" & modelo & " and diametro=" & diametro & " and id_tratamiento=" & idtratamiento & " and cilindro=" & NumSql(cilindro) & " and esfera=" & NumSql(esfera) & " and eje=" & NumSql(eje) & " and adicion=" & NumSql(adicion)
        Dim id As Integer = 0
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        id = cmd.ExecuteScalar
        mcon.Close()
        Return id
    End Function
    Public Sub GrabaDesecho(ByVal des As clsDesecho)
        Dim cad As String
        With des
            If .Id = 0 Then ' se trata de uno nuevo

                .Id = getMaxId("id_desecho", "t_almacen_desechos") + 1

                cad = "insert into t_almacen_desechos (id_desecho,id_modelo,id_tratamiento,diametro,cilindro,esfera,eje,adicion,cantidad,ojo) VALUES (" & _
                .Id & "," & .idModelo & "," & .idTratamiento & "," & .diametro & "," & NumSql(.cilindro) & "," & NumSql(.esfera) & "," & .Eje & _
                "," & NumSql(.Adicion) & "," & .Cantidad & "," & strsql(.Ojo) & ")"
            Else ' se trata de uno que ya existe, solo updateamos la cantidad
                cad = "Update t_almacen_desechos set cantidad=cantidad+ " & .Cantidad & " where id_desecho=" & .Id

            End If
        End With
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function getLineasfacturas(ByVal idFactura As Long) As DataTable
        Dim cad As String = "select *,(select min(fecha) from t_albaranes where id_albaran=t_lineas_factura.id_albaran) as fecalbaran from t_lineas_factura inner join t_facturas on t_lineas_factura.id_factura=" & _
        "t_facturas.id_factura where t_facturas.id_factura = " & idFactura & " order by id_albaran"
        Dim tb As New DataTable
        Dim cmd As New SqlCommand(cad, mcon)
        cmd.CommandTimeout = 340
        Dim mda As New SqlDataAdapter(cmd)
        'mcon.ConnectionTimeout()
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function getLineasfacturasBonos(ByVal idFactura As Long) As DataTable
        Dim cad As String = "select t_lineas_factura_bono.* from t_lineas_factura_bono inner join t_factura_bono on t_lineas_factura_bono.id_factura_bono=" & _
        "t_factura_bono.id_factura_bono where t_factura_bono.id_factura_bono = " & idFactura & " order by id_albaran"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetClientesRecibos(ByVal idfactura As Long) As DataTable
        'seleccionamos el numero de la factura, la fecha y lost_clientes de B
        Dim cad As String = "select t_facturas.fecha,num_factura,cod_envio as cliente from (t_facturas INNER JOIN t_lineas_factura " & _
        " ON t_facturas.id_factura=t_lineas_factura.id_factura) INNER JOIN t_albaranes ON t_lineas_factura.id_albaran=t_albaranes.id_albaran" & _
        " Where t_albaranes.id_cliente=0 and t_facturas.id_factura=" & idfactura & " GROUP BY t_facturas.fecha,num_factura,cod_envio"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetLineasRecibo(ByVal idRecibo As Long) As DataTable
        'el idrecibo en este caso sera igual al idfactura del cliente contado
        Dim cad As String = "select fecha,t_lineas_albaran.* from t_lineas_albaran INNER JOIN t_albaranes ON t_lineas_albaran.id_albaran=t_albaranes.id_albaran where id_bono=" & _
        idRecibo & " order by t_lineas_albaran.id_albaran"
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Sub MarcarAlbaranFacturado(ByVal idalbaran As Integer)
        Dim cad As String = "update t_albaranes set facturado = 1 where id_albaran = " & idalbaran
        Dim mda As New SqlDataAdapter
        mda.UpdateCommand = New SqlCommand(cad, mcon)
        mcon.Open()
        mda.UpdateCommand.ExecuteNonQuery()
        mcon.Close()
    End Sub
    Public Function getSumTotalFacturar(ByVal id_cliente As Integer, ByVal fecini As Integer, ByVal fecfin As Integer) As Decimal
        Dim cad As String = "select sum(total) as total from t_albaranes where fecha>=" & fecini & " and fecha<=" & fecfin & " and id_cliente = " & id_cliente
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If IsDBNull(tb.Rows(0)("total")) Then
            Return 0
        Else
            Return tb.Rows(0)("total")
        End If
    End Function
    Public Function GetPorteByAlbaran(ByVal idalbaran As Long) As clsPorte
        Dim cad As String = "select t_portes.* from t_portes INNER JOIN t_lineas_porte ON t_portes.id_porte=t_lineas_porte.id_porte where id_albaran =" & idalbaran
        Dim mPorte As New clsPorte
        Dim mdat As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        mdat.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            MsgBox("No existe salida de porte para ese Albaran")
            Return mPorte
        Else
            mPorte.idPorte = tb.Rows(0)("id_porte")
            mPorte.Cliente = getClientebyId(tb.Rows(0)("id_cliente"))
            mPorte.Fecha = cadenaAfecha(tb.Rows(0)("fecha"))
            mPorte.CodBarra = tb.Rows(0)("cod_bolsa")
            mPorte.Agencia = tb.Rows(0)("agencia")
            Return mPorte
        End If
    End Function
    Public Function GetPortespendienteByMensajeria(ByVal mensajeria As String) As DataTable
        Dim cad As String = "select t_clientes.id_cliente,codigo,nombre_comercial from t_clientes INNER JOIN t_albaranes ON t_clientes.id_cliente=t_albaranes.id_cliente where transportista like " & strsql(mensajeria) & " and id_alb_abono=0 and id_albaran not in (select id_albaran from t_lineas_porte) group by t_clientes.id_cliente,codigo,nombre_comercial order by nombre_comercial"
        Dim mPorte As New clsPorte
        Dim mdat As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mdat.SelectCommand.CommandTimeout = 180
        ' mcon.Open()
        Dim tb As New DataTable
        mdat.Fill(tb)
        'mcon.Close()
        Return tb
    End Function
    Public Function GetTaladrosBypedido(ByVal pedido As clsPedido) As ArrayList
        'Dim Taladros As New ArrayList
        Dim cad As String = "Select taladro from t_taladros_pedido where id_pedido=" & pedido.id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Dim cadena() As String = Split(cmd.ExecuteScalar, "F;")
        mcon.Close()
        Dim matriz As New ArrayList
        If UBound(cadena) > 0 Then
            For i As Integer = 0 To UBound(cadena)
                Dim Taladro() As String = Split(cadena(i), ";")
                If UBound(Taladro) > 1 Then
                    Dim t As New Clstaladro
                    t.Inicio = New Point(100 * (Replace(Taladro(1), ".", ",")), 100 * (Replace(Taladro(2), ".", ",")))
                    t.Grosor = Replace(Taladro(3), ".", ",") * 100
                    If Taladro(4) <> "" Then
                        t.Fin = New Point(100 * (Replace(Taladro(4), ".", ",")), 100 * Replace(Taladro(5), ".", ","))
                    End If
                    If pedido.ojo = "I" Then
                        t.Inicio = New Point(-t.Inicio.X, t.Inicio.Y)
                        t.Fin = New Point(-t.Fin.X, t.Fin.Y)
                    End If
                    matriz.Add(t)
                End If
            Next
        End If
        GetTaladrosBypedido = matriz
    End Function
    Public Function GetPorteByCodBolsa(ByVal codbolsa As Long) As clsPorte
        Dim cad As String = "select * from t_portes  where cod_bolsa =" & codbolsa
        Dim mPorte As New clsPorte
        Dim mdat As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        mdat.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            MsgBox("No existe salida de porte para ese Albaran")
            Return mPorte
        Else
            mPorte.idPorte = tb.Rows(0)("id_porte")
            mPorte.Cliente = getClientebyId(tb.Rows(0)("id_cliente"))
            mPorte.Fecha = cadenaAfecha(tb.Rows(0)("fecha"))
            mPorte.CodBarra = tb.Rows(0)("cod_bolsa")
            mPorte.Agencia = tb.Rows(0)("agencia")
            Return mPorte
        End If
    End Function

    Public Function GetPorteByPedido(ByVal idped As Long) As clsPorte
        Dim cad As String = "select t_portes.* from t_portes INNER JOIN t_lineas_porte ON t_portes.id_porte=t_lineas_porte.id_porte where id_albaran in (select id_albaran from t_lineas_albaran where id_pedido=" & idped & ")"
        Dim mPorte As New clsPorte
        Dim mdat As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        mdat.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            MsgBox("No existe salida de porte para ese Pedido")
            Return mPorte
        Else
            mPorte.idPorte = tb.Rows(0)("id_porte")
            mPorte.Cliente = getClientebyId(tb.Rows(0)("id_cliente"))
            mPorte.Fecha = cadenaAfecha(tb.Rows(0)("fecha"))
            mPorte.CodBarra = tb.Rows(0)("cod_bolsa")
            mPorte.Agencia = tb.Rows(0)("agencia")
            Return mPorte
        End If
    End Function

    Public Function GetPorteBycliente(ByVal idcli As Long, ByVal fecha As Long, ByVal agencia As String) As clsPorte
        Dim cad As String = "select t_portes.* from t_portes INNER JOIN t_lineas_porte ON t_portes.id_porte=t_lineas_porte.id_porte where id_cliente=" & idcli & _
        " and fecha=" & fecha & " and agencia='" & agencia & "'"
        Dim mPorte As New clsPorte
        Dim mdat As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        mdat.Fill(tb)
        mcon.Close()
        If tb.Rows.Count = 0 Then
            MsgBox("No existe salida de porte para ese cliente con esa fecha y agencia")
            Return mPorte
        Else
            mPorte.idPorte = tb.Rows(0)("id_porte")
            mPorte.Cliente = getClientebyId(tb.Rows(0)("id_cliente"))
            mPorte.Fecha = cadenaAfecha(tb.Rows(0)("fecha"))
            mPorte.CodBarra = tb.Rows(0)("cod_bolsa")
            mPorte.Agencia = tb.Rows(0)("agencia")
            Return mPorte
        End If
    End Function

    Public Function GetlineasPorteById(ByVal idporte As String) As DataTable
        Dim cad As String = "select id_albaran from t_lineas_porte where id_porte=" & idporte
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        mda = Nothing
        Return tb
    End Function

    Public Function getPortesByIdCliente(ByVal idCliente As Long, ByVal fecini As Long, ByVal fecfin As Long) As Integer
        'Dim cad As String = "select count(distinct(fecha)) as portes " & _
        '"from t_albaranes where fecha>=" & fecini & " and fecha<=" & fecfin & " and id_alb_abono=0 and facturado=0 and (id_cliente=" & idCliente & _
        '" or cod_envio=" & idCliente & ")  "
        Dim cad As String = "select count(*) as portes " & _
        "from t_portes where fecha>=" & fecini & " and fecha<=" & fecfin & " and id_cliente=" & idCliente & " and porte<>0"

        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        If IsDBNull(tb.Rows(0)("portes")) Then
            Return 0
        Else
            Return tb.Rows(0)("portes")
        End If
        ' mcon.Close()

    End Function



    Public Function CargaAlbaranesFacturadosBono(ByVal idbono As Integer) As DataTable
        Dim cad As String = "select t_lineas_bono.id_albaran,id_factura, (select sum(base) from t_bases_albaran where id_albaran=t_lineas_bono.id_albaran) as totalbase,(select sum(base+ convert(decimal(8,2),base*iva/100)+convert(decimal(8,2),base*re/100)) from t_bases_albaran where id_albaran=t_lineas_bono.id_albaran)  as total from t_lineas_bono where id_bono= " & idbono & " and envio=1"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    'Public Function GrabaFacturaBono(ByVal idBono As Integer, ByVal idcli As Integer, ByVal fecha As Long, ByVal base As Decimal, ByVal iva As Single, ByVal re As Single) As Integer
    '    Dim id As Integer = getMaxId("id_factura_bono", "t_factura_bono") + 1
    '    Dim num As Integer = getMaxId("num_factura", "t_factura_bono", " where fecha>=" & Left(fecha, 4) & "0101 and fecha<=" & Left(fecha, 4) & "1231") + 1
    '    Dim cad As String = "INSERT INTO t_factura_bono (id_factura_bono,fecha,id_cliente,serie,num_factura,base,iva,re,id_bono) VALUES (" & id & "," & fecha & _
    '    "," & idcli & ",'B'," & num & "," & Replace(base, ",", ".") & "," & Replace(iva, ",", ".") & "," & Replace(re, ",", ".") & "," & idBono & ")"

    '    'aqui grabamos la factura y devolvemos el id de la factura
    '    Dim cmd As New SqlCommand(cad, mcon)
    '    mcon.Open()
    '    cmd.ExecuteNonQuery()
    '    mcon.Close()
    '    Return id
    'End Function
    Public Sub GrabaLineasFacturaBonos(ByVal idBono As Integer, ByVal idAlbaran As Integer)
        'grabamos la linea de factura y updateamos el albaran a facturado con su id_factura
        Dim tb As New DataTable

        mcon.Open()
        Dim cad As String = "INSERT INTO t_lineas_Bono (id_bono,id_albaran,envio,id_factura) VALUES (" & idBono & "," & idAlbaran & ",1,0)"
        Dim cmd As New SqlCommand(cad, mcon)
        cmd.ExecuteNonQuery()




        cad = "UPDATE t_albaranes set id_bono=" & idBono & " where id_albaran=" & idAlbaran
        Dim cmd1 As New SqlCommand(cad, mcon)
        'ahora updateamos ese albaran a facturado

        cmd1.ExecuteNonQuery()
        cmd1 = Nothing
        mcon.Close()
    End Sub
    Public Function CargaAlbaranesNoFacturadosBono(ByVal idbono As Integer) As DataTable
        Dim cad As String = "select t_lineas_bono.id_albaran,sum(base) as totalbase,sum(base+convert(decimal(8,2),base*iva/100)+convert(decimal(8,2),base*re/100)) as total from t_bases_albaran INNER JOIN t_lineas_bono ON t_bases_albaran.id_albaran=t_lineas_bono.id_albaran where id_bono=" & idbono & _
        " and envio=0 group by t_lineas_bono.id_albaran,id_factura"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Sub GrabaAlbaranesNoFacturadosBono(ByVal idalbaran As Integer, ByVal idBono As Integer)
        Dim cad As String = "UPDATE t_albaranes set id_bono=" & idBono & " where id_albaran=" & idalbaran
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        cmd.CommandText = "INSERT INTO t_lineas_bono (id_bono,id_albaran,id_factura,envio) VALUES (" & idBono & "," & idalbaran & ",0,0)"
        cmd.ExecuteNonQuery()
        mcon.Close()
        cmd = Nothing
    End Sub
    Public Sub RevierteBono(ByVal idbono As Integer)
        'borramos las lineas de facturas
        Dim cad As String = "delete from t_lineas_bono where id_factura=0 and id_bono =" & idbono
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'por ultimo los albaranes no facturados los ponemos a cero
        cmd.CommandText = "UPDATE t_albaranes set id_bono=0 where id_bono=" & idbono & " and facturado=0"
        cmd.ExecuteNonQuery()


        mcon.Close()
        cmd = Nothing
    End Sub

    Public Function GrabaBono(ByVal idcliente As Integer, ByVal idbono As Integer, ByVal factura As Decimal, ByVal albaran As Decimal) As Integer
        Dim cad As String
        If idbono = 0 Then
            'grabamos el bono
            idbono = getMaxId("id_bono", "t_bonos") + 1
            cad = "INSERT INTO t_bonos (id_bono,id_cliente,parteA,parteB,fecini) VALUES (" & idbono & "," & _
            idcliente & "," & Replace(factura, ",", ".") & "," & Replace(albaran, ",", ".") & "," & FechaAcadena(Now.Date) & ")"
        Else
            cad = "UPDATE t_bonos set parteA=" & Replace(factura, ",", ".") & ",parteB=" & Replace(albaran, ",", ".") & " where id_bono=" & idbono
        End If
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
        cmd = Nothing
        Return idbono
    End Function

    Public Function GetAlbaranesPendienteBonos(ByVal idcliente As Integer) As DataTable
        Dim cad As String = "select id_albaran,(select sum(base) from t_bases_albaran where id_albaran=t_albaranes.id_albaran)as totalbase" & _
        ",(select sum(convert(decimal (8,2),base+base*iva/100+base*re/100)) as total from t_bases_albaran where id_albaran=t_albaranes.id_albaran) as total,0 as id_factura from t_albaranes where (id_cliente=" & idcliente & " or id_cliente in (select id_cliente_asociado from t_clientes_asociados where id_cliente=" & idcliente & ")) and facturado=0 and id_bono=0 order by id_albaran "
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function

    Public Function GetAlbaranesSinFacturarByGrupo(ByVal grupo As Integer, ByVal Fecini As Integer, ByVal Fecfin As Integer) As DataTable
        Dim cad As String = "Select id_albaran,dbo.fecha(fecha) as fecha,nombre_comercial,isnull((select isnull(codigo,'') from t_codigos_grupo_optico where id_grupo=" & grupo & " and id_cliente=t_clientes.id_cliente),'') As cod_grupo FROM t_albaranes INNER JOIN t_clientes ON t_clientes.id_cliente=t_albaranes.id_cliente where id_grupo=" & grupo & " and fecha>=" & Fecini & " and fecha<=" & Fecfin & " and facturado=0 ORDER BY fecha,id_albaran"
        Dim cmd As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        cmd.Fill(tb)
        Return tb
    End Function
    Public Function InfSalidaColor(ByVal FecIni As Long, ByVal Fecfin As Long) As DataTable
        Dim Cad As String = "select Fecha,sum(entradas) as Entrada, sum(Salidas) as Salida from ((Select dbo.fechaexcel(fs_coloracion) as Fecha,0 as Entradas, count(*) as Salidas from t_ordenes_trabajo where fs_coloracion>=" & FecIni & " and fs_coloracion<=" & Fecfin & " Group by fs_coloracion)" & _
        "UNION (Select dbo.fechaexcel(fe_coloracion) as Fecha, count(*) as Entradas,0 as salidas from t_ordenes_trabajo where fe_coloracion>=" & FecIni & " and fe_coloracion<=" & Fecfin & " Group by fe_coloracion)) as tabla GROUP BY fecha ORDER BY Fecha"
        Dim mda As New SqlDataAdapter(Cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function getAlbaranesSinfacturar(ByVal fecini As Long, ByVal fecfin As Long, ByVal periodicidad As String) As DataTable
        'TENEMOS QUE MOFIFICAR LA SELECT PARA QUE NO VENGAN LOS CLIENTES QUE PERTENECEN A UN GRUPO QUE TIENE FACTURA UNICA
        Dim cad As String = "(select t_clientes.id_cliente,nombre_comercial, (select count(*) from t_lineas_bono where id_factura=0 and envio=1 and id_bono=t_bonos.id_bono) as num_albaranes,'B'+ convert(nvarchar(4),id_bono) as tipo" & _
        " from t_clientes INNER JOIN t_bonos ON t_bonos.id_cliente=t_clientes.id_cliente where partea<>0 and facturacion like " & strsql(periodicidad) & " and t_clientes.id_cliente not in (select id_cliente from t_clientes where id_grupo in (select id_grupo from t_grupos_opticos where factura<>0)) group by t_clientes.id_cliente,codigo,nombre_comercial,id_bono) UNION " & _
        "(select t_clientes.id_cliente,nombre_comercial, (select count(*) from t_albaranes where facturado=0 and id_bono=0 and fecha>=" & fecini & " and fecha<=" & fecfin & " and id_cliente in (select id_cliente_asociado from t_clientes_asociados where factura_separada=0 and id_cliente=t_clientes.id_cliente)) as num_albaranes,'A' as tipo" & _
        " from t_clientes INNER JOIN t_clientes_asociados ON t_clientes.id_cliente=t_clientes_asociados.id_cliente where no_facturar=0 and facturacion like " & strsql(periodicidad) & " and t_clientes.id_cliente not in (select id_cliente from t_clientes where id_grupo in (select id_grupo from t_grupos_opticos where factura<>0)) group by t_clientes.id_cliente,codigo,nombre_comercial) UNION " & _
        "(select t_clientes.id_cliente,nombre_comercial,count(id_albaran)as num_albaranes,'N' as tipo from t_albaranes inner join " & _
        "t_clientes ON t_clientes.id_cliente= t_albaranes.id_cliente  " & _
        "where no_facturar=0 and (facturado = 0 And fecha >=" & fecini & " And fecha <=" & fecfin & _
        " ) and facturacion like " & strsql(periodicidad) & " and t_albaranes.id_bono=0 and  t_clientes.id_cliente not in (select id_cliente_asociado from t_clientes_asociados where factura_separada=0) and t_clientes.id_cliente not in (select id_cliente from t_clientes where id_grupo in (select id_grupo from t_grupos_opticos where factura<>0)) group by t_clientes.id_cliente,nombre_comercial ) order by NOMBRE_COMERCIAL"

        ' primero cargamos los bonos, luego los asociados, luego los clientes normales

        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mda.SelectCommand.CommandTimeout = 180
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GrabaFactura(ByVal fac As clsfactura) As clsfactura
        Dim id As Long
        Dim numfac As Long
        Dim año As Integer = Left(fac.fecha, 4)

        id = getMaxId("id_factura", "t_facturas") + 1
        numfac = getMaxId("num_factura", "t_facturas", " where fecha>=" & año & "0101" & " and fecha<=" & año & "1231") + 1
        fac.Id_factura = id
        fac.num_factura = numfac
        If fac.IdFormaPago <> 9 Then
            fac.IdFormaPago = fac.Cliente.FormaPago.IdForma
        End If
        Dim Vencimiento As Integer
        Dim fecha As Date = cadenaAfecha(fac.fecha)
        If Vencimiento = 0 Then
            Vencimiento = FechaAcadena(DateAdd(DateInterval.Day, fac.Cliente.FormaPago.Dias, fecha))
        End If
        Dim cad As String = "insert into t_facturas (id_factura,id_cliente,fecha,num_factura,serie,total,id_usuario,fecini,fecfin,id_forma_pago,vencimiento) " & _
                "values (" & id & "," & fac.Cliente.id & "," & fac.fecha & "," & numfac & ",'A'," & Replace(fac.Total, ",", ".") & "," & fac.usuario.id & _
                "," & fac.FecIni & "," & fac.FecFin & "," & fac.IdFormaPago & "," & Vencimiento & ")"
        mcon.Open()
        Dim mda As New SqlDataAdapter
        mda.InsertCommand = New SqlCommand(cad, mcon)
        mda.InsertCommand.ExecuteNonQuery()
        'ahora inserto las lineas de albaran
        Dim lin As New clsFacturaLinea
        Dim i As Integer
        Dim c2 As String
        For Each lin In fac
            i = i + 1
            cad = "insert into t_lineas_factura values(" & _
            id & "," & lin.id_tipo_Producto & "," & lin.id_modelo & "," & lin.id_tratamiento & _
            "," & lin.id_coloracion & "," & lin.id_modo & "," & lin.c1 & ",'" & lin.c2 & "'," & _
            strsql(lin.descripcion) & "," & Replace(lin.precio, ",", ".") & "," & Replace(lin.dto, ",", ".") & "," & _
            Replace(lin.total, ",", ".") & "," & Replace(lin.iva, ",", ".") & "," & Replace(lin.Re, ",", ".") & _
            "," & lin.id_albaran & "," & i & ")"
            mda.InsertCommand.CommandText = cad
            mda.InsertCommand.ExecuteNonQuery()
            c2 = "update t_albaranes set facturado = 1 where id_albaran = " & lin.id_albaran
            mda.UpdateCommand = New SqlCommand(c2, mcon)
            mda.UpdateCommand.ExecuteNonQuery()
        Next
        'ahora vamos a grabar las bases de la factura
        For Each b As clsBaseImponible In fac.Bases
            cad = "INSERT INTO t_bases_factura (id_factura,id_tipo_base,base,iva,re) VALUES (" & id & "," & b.TipoBase & _
            "," & NumSql(b.BaseI) & "," & NumSql(b.Iva) & "," & NumSql(b.Re) & ")"
            mda.InsertCommand.CommandText = cad
            mda.InsertCommand.ExecuteNonQuery()
        Next
        'ahora grabamos la hoja blanca correspondiente a dicha factura
        For Each lin In fac.HojaBlanca
            i = i + 1
            cad = "insert into t_hoja_blanca values(" & _
            id & "," & lin.id_tipo_Producto & "," & lin.id_modelo & "," & lin.id_tratamiento & _
            "," & lin.id_coloracion & "," & lin.id_modo & "," & lin.c1 & ",'" & lin.c2 & "'," & _
            strsql(lin.descripcion) & "," & Replace(lin.precio, ",", ".") & "," & Replace(lin.dto, ",", ".") & "," & _
            Replace(lin.total, ",", ".") & "," & Replace(lin.iva, ",", ".") & "," & Replace(lin.Re, ",", ".") & _
            "," & lin.id_albaran & "," & i & ")"
            mda.InsertCommand.CommandText = cad
            mda.InsertCommand.ExecuteNonQuery()
            c2 = "update t_albaranes set facturado = 1 where id_albaran = " & lin.id_albaran
            mda.UpdateCommand = New SqlCommand(c2, mcon)
            mda.UpdateCommand.ExecuteNonQuery()
        Next
        mcon.Close()
        Return fac
    End Function

    Public Sub GrabaCambioGrupoModelos(ByVal lista As ArrayList, ByVal idgrupo As Integer, ByVal orden As Integer)
        'priero vamos a ordenar todos los que sean mayor que el que mandamos
        Dim cad As String = "Update t_modelos set orden=orden+" & lista.Count & " where id_cliente=" & idgrupo & " and orden>" & orden
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        'ahora vamos a ordenar el resto
        For Each i As Integer In lista
            orden += 1
            cmd.CommandText = "UPDATE t_modelos set id_cliente=" & idgrupo & ",orden=" & orden & " where id_lente=" & i
            cmd.ExecuteNonQuery()
        Next
        mcon.Close()
    End Sub
    Public Sub CambiaClienteAlbaran(ByVal idAlbaran As Integer, ByVal idcliente As Integer)
        Dim cad As String = "Update t_albaranes set id_cliente=" & idcliente & " where id_albaran=" & idAlbaran
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        cmd.ExecuteNonQuery()
        mcon.Close()
    End Sub

    Public Function GetAlbaranesSinFacturarByCliente(ByVal fecini As Long, ByVal fecfin As Long, ByVal idcliente As Long) As DataTable
        Dim cad As String = "select * from t_albaranes where facturado = 0 and fecha>=" & fecini & " and fecha<=" & fecfin & _
        " and id_bono=0 and id_cliente = " & idcliente
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        mcon.Open()
        Dim tb As New DataTable
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetBasesFacturasById(ByVal idFactura As Long) As DataTable
        Dim cad As String = "select sum(base) as baseI,iva,re," & _
        "sum(base)*iva/100 as tIva," & _
        "sum(base)*re/100 as tRe," & _
        "sum(base)*(1+iva/100) as totalSinRe," & _
        "sum(base)*(1+iva/100+re/100) as totalConRe " & _
        "from t_bases_factura  where id_factura=" & idFactura & " group by iva,re  "
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.SelectCommand.CommandTimeout = 180
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    'Public Function GetBasesConceptosFacturasById(ByVal idFactura As Long)
    '    Dim cad As String = "(select 'lentes' as Concepto,convert(decimal(8,2),sum(total)) as base,iva,re," & _
    '    "convert(decimal(8,2),sum(total)*iva/100) as tIva," & _
    '    "convert(decimal(8,2),sum(total)*re/100) as tRe," & _
    '    "convert(decimal(8,2),sum(total)*(1+iva/100)) as totalSinRe," & _
    '    "convert(decimal(8,2),sum(total)*(1+iva/100))+convert(decimal(8,2),sum(total)*re/100) as totalConRe " & _
    '    "from t_lineas_factura  where id_factura=" & idFactura & " and id_tipo_producto<>0 and  id_tipo_producto<>4 group by iva,re having sum(total)<>0 ) "

    '    cad = cad & " UNION ALL (select 'Monturas' as Concepto,convert(decimal(8,2),sum(total)) as base,iva,re," & _
    '    "convert(decimal(8,2),sum(total)*iva/100) as tIva," & _
    '    "convert(decimal(8,2),sum(total)*re/100) as tRe," & _
    '    "convert(decimal(8,2),sum(total)*(1+iva/100)) as totalSinRe," & _
    '    "convert(decimal(8,2),sum(total)*(1+iva/100))+convert(decimal(8,2),sum(total)*re/100) as totalConRe " & _
    '    "from t_lineas_factura  where id_factura=" & idFactura & " and  id_tipo_producto=4 and descripcion like '%montura%' group by iva,re having sum(total)<>0)  "
    '    cad = cad & " UNION ALL (select 'Montajes' as Concepto,convert(decimal(8,2),sum(total)) as base,iva,re," & _
    '            "convert(decimal(8,2),sum(total)*iva/100) as tIva," & _
    '            "convert(decimal(8,2),sum(total)*re/100) as tRe," & _
    '            "convert(decimal(8,2),sum(total)*(1+iva/100)) as totalSinRe," & _
    '            "convert(decimal(8,2),sum(total)*(1+iva/100))+convert(decimal(8,2),sum(total)*re/100) as totalConRe " & _
    '            "from t_lineas_factura  where id_factura=" & idFactura & " and  id_tipo_producto=4 and descripcion not like '%montura%'  group by iva,re having sum(total)<>0)  "
    '    cad = cad & " UNION ALL (select 'Portes' as Concepto,convert(decimal(8,2),sum(total)) as base,iva,re," & _
    '           "convert(decimal(8,2),sum(total)*iva/100) as tIva," & _
    '           "convert(decimal(8,2),sum(total)*re/100) as tRe," & _
    '           "convert(decimal(8,2),sum(total)*(1+iva/100)) as totalSinRe," & _
    '           "convert(decimal(8,2),sum(total)*(1+iva/100))+convert(decimal(8,2),sum(total)*re/100) as totalConRe " & _
    '           "from t_lineas_factura  where id_factura=" & idFactura & " and  id_tipo_producto=0   group by iva,re having sum(total)<>0)  "
    '    Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
    '    mda.SelectCommand.CommandTimeout = 180
    '    Dim tb As New DataTable


    '    mcon.Open()
    '    mda.Fill(tb)

    '    'Dim tb2 As DataTable 
    '    ' ahora lo mismo para los montajes



    '    'idem para monturas


    '    'idem para los portes


    '    mcon.Close()
    '    Return tb
    'End Function
    Public Function GetBasesConceptosFacturasById(ByVal idFactura As Long) As DataTable
        Dim cad As String = "select t_bases_factura.id_tipo_base,tipo_base as concepto,base,iva,re from t_bases_factura INNER JOIN t_tipos_base ON t_bases_factura.id_tipo_base=t_tipos_base.id_tipo_base where id_factura=" & idFactura
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter(cad, mcon)

        'mcon.Open()
        mda.Fill(tb)

        'Dim tb2 As DataTable 
        ' ahora lo mismo para los montajes



        'idem para monturas


        'idem para los portes


        ' mcon.Close()
        Return tb
    End Function
    Public Function GetBasesRecibosById(ByVal idfactura As Long, ByVal cliente As Long) As DataTable
        Dim cad As String = "select sum(t_lineas_factura.total) as base,iva,re," & _
        "sum(t_lineas_factura.total)*iva/100 as tIva," & _
        "sum(t_lineas_factura.total)*re/100 as tRe," & _
        "sum(t_lineas_factura.total)*(1+iva/100) as totalSinRe," & _
        "sum(t_lineas_factura.total)*(1+iva/100+re/100) as totalConRe " & _
        "from t_lineas_factura  INNER JOIN t_albaranes ON t_albaranes.id_albaran=t_lineas_factura.id_albaran " & _
        " where id_factura=" & idfactura & " and cod_envio=" & cliente & " group by iva,re  "
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetFacturaById(ByVal nfactura As Long) As clsfactura
        Dim mF As New clsfactura
        Dim cad As String = "Select * from t_facturas where id_factura=" & nfactura
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        ' mcon.Open()
        mda.SelectCommand.CommandTimeout = 180
        mda.Fill(tb)
        mF.Cliente = getClientebyId(tb.Rows(0)("id_cliente"))
        mF.FecFin = tb.Rows(0)("fecfin")
        mF.fecha = tb.Rows(0)("fecha")
        mF.FecIni = tb.Rows(0)("fecini")
        mF.Id_factura = nfactura
        mF.num_factura = tb.Rows(0)("num_factura")
        mF.serie = tb.Rows(0)("serie")
        mF.Total = tb.Rows(0)("total")
        mF.usuario = getUsuarioById(tb.Rows(0)("id_usuario"))
        mF.IdFormaPago = tb.Rows(0)("id_forma_pago")
        mF.FormaPago = GetFormaPago(mF.IdFormaPago)
        mF.Vencimiento = tb.Rows(0)("vencimiento")
        'ahora cargo las lineas de factura
        Dim tb2 As New DataTable
        cad = "select * from t_lineas_factura where id_factura = " & nfactura & " order by id_albaran"
        mda.SelectCommand.CommandText = cad
        mda.Fill(tb2)
        Dim lin As clsFacturaLinea
        Dim i As Integer
        For i = 0 To tb2.Rows.Count - 1
            lin = New clsFacturaLinea
            lin.c1 = tb2.Rows(i)("c1")
            lin.c2 = tb2.Rows(i)("c2")
            lin.descripcion = tb2.Rows(i)("descripcion")
            lin.dto = tb2.Rows(i)("dto")
            lin.id_albaran = tb2.Rows(i)("id_albaran")
            lin.id_coloracion = tb2.Rows(i)("id_coloracion")
            lin.id_modelo = tb2.Rows(i)("id_modelo")
            lin.id_modo = tb2.Rows(i)("id_modo")
            lin.id_tipo_Producto = tb2.Rows(i)("id_tipo_producto")
            lin.id_tratamiento = tb2.Rows(i)("id_tratamiento")
            lin.iva = tb2.Rows(i)("iva")
            lin.precio = tb2.Rows(i)("precio")
            lin.Re = tb2.Rows(i)("re")
            lin.total = tb2.Rows(i)("total")
            'lin.tratamiento=
            'lin.modelo=
            'lin.modo=
            'lin.coloracion=
            mF.add(lin)

        Next
        tb2.Clear()
        'ahora cargamos la hoja blanca si lo tiene
        cad = "select * from t_hoja_blanca where id_factura = " & nfactura & " order by linea"
        mda.SelectCommand.CommandText = cad
        mda.Fill(tb2)

        For i = 0 To tb2.Rows.Count - 1
            lin = New clsFacturaLinea
            lin.c1 = tb2.Rows(i)("c1")
            lin.c2 = tb2.Rows(i)("c2")
            lin.descripcion = tb2.Rows(i)("descripcion")
            lin.dto = tb2.Rows(i)("dto")
            lin.id_albaran = tb2.Rows(i)("id_albaran")
            lin.id_coloracion = tb2.Rows(i)("id_coloracion")
            lin.id_modelo = tb2.Rows(i)("id_modelo")
            lin.id_modo = tb2.Rows(i)("id_modo")
            lin.id_tipo_Producto = tb2.Rows(i)("id_tipo_producto")
            lin.id_tratamiento = tb2.Rows(i)("id_tratamiento")
            lin.iva = tb2.Rows(i)("iva")
            lin.precio = tb2.Rows(i)("precio")
            lin.Re = tb2.Rows(i)("re")
            lin.total = tb2.Rows(i)("total")
            'lin.tratamiento=
            'lin.modelo=
            'lin.modo=
            'lin.coloracion=
            mF.HojaBlanca.Add(lin)

        Next
        'ahora cargamos las bases
        tb2.Clear()
        mda.SelectCommand.CommandText = "select * from t_bases_factura where id_factura=" & mF.Id_factura
        mda.Fill(tb2)
        For Each rw As DataRow In tb2.Rows
            Dim b As New clsBaseImponible
            b.BaseI = rw("base")
            b.Iva = rw("iva")
            b.Re = rw("re")
            b.TipoBase = rw("id_tipo_base")
            mF.Bases.add(b)
        Next
        ' mcon.Close()
        Return mF


    End Function
    Public Function COmpruebaMontaje(ByVal id As Integer) As Boolean
        Dim Albaraneado As Boolean
        Dim cad As String = "select count(*) from t_lineas_albaran where montaje<>0 and id_pedido=" & id
        Dim cmd As New SqlCommand(cad, mcon)
        mcon.Open()
        Albaraneado = cmd.ExecuteScalar
        mcon.Close()
        Return Albaraneado
    End Function
    Public Function GetLineasbono(ByVal idbono As Integer) As DataTable
        Dim cad As String = "Select * from t_lineas_bono where id_bono=" & idbono
        Dim mda As New SqlDataAdapter(cad, mcon)
        Dim tb As New DataTable
        mda.Fill(tb)
        Return tb
    End Function
    Public Function GetFacturasBonoByid(ByVal id As Integer) As DataTable

        Dim cad As String = "Select distinct(id_factura) as factura from t_lineas_bono where id_factura<>0 and id_bono=" & id
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb
    End Function
    Public Function GetBonobyId(ByVal idbono As Integer) As clsBono
        Dim b As New clsBono
        b.Bono = idbono
        Dim tb As New DataTable
        Dim mda As New SqlDataAdapter("select * from t_bonos where id_bono=" & idbono, mcon)
        mda.Fill(tb)
        If tb.Rows.Count > 0 Then
            b.Fecha = tb.Rows(0)("fecini")
            b.Cliente = getClientebyId(tb.Rows(0)("id_cliente"))
            b.ConEnvio = tb.Rows(0)("parteA")
            b.SinEnvio = tb.Rows(0)("parteB")
        End If
        'ahora le añadimos las lineas
        tb.Clear()
        mda.SelectCommand.CommandText = "Select * from t_lineas_bono where id_bono=" & idbono & " order by id_albaran"
        mda.Fill(tb)
        For Each rw As DataRow In tb.Rows
            Dim l As New clsLineasBono
            l.Albaran = rw("id_albaran")
            l.Factura = rw("id_factura")
            l.ConEnvio = CBool(rw("envio"))
            b.add(l)
        Next
        'ahora vemos que facturas van en el bono
        Return b
    End Function
    Public Function FechaUltimaFactura() As Long
        Dim cad As String = "select max(fecha) as ultima from t_facturas"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Dim Fecha As Integer
        Fecha = IIf(IsDBNull(tb.Rows(0)("ultima")), 0, tb.Rows(0)("ultima"))
        Return (Fecha)
    End Function
    Public Function GetIdFacturaByNum(ByVal numFacIni As Long, ByVal numfacfin As Long, ByVal año As Long) As DataTable

        Dim inicio As Long
        Dim fin As Long
        inicio = año * 10000
        fin = inicio + 1231
        Dim cad As String = "select id_factura from t_facturas where num_factura>=" & numFacIni & " and num_factura<=" & numfacfin & _
        " and fecha>=" & inicio & " and fecha<=" & fin & " order by num_factura"
        Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        mcon.Open()
        mda.Fill(tb)
        mcon.Close()
        Return tb



    End Function
    Public Function GetIdFacturaBonoByNum(ByVal numFacIni As Long, ByVal numfacfin As Long, ByVal año As Long) As DataTable

        Dim inicio As Long
        Dim fin As Long
        inicio = año * 10000
        fin = inicio + 1231
        'Dim cad As String = "select id_factura_bono from t_factura_bono where num_factura>=" & numFacIni & " and num_factura<=" & numfacfin & _
        '" and fecha>=" & inicio & " and fecha<=" & fin
        'Dim mda As New SqlDataAdapter(New SqlCommand(cad, mcon))
        Dim tb As New DataTable
        'mcon.Open()
        'mda.Fill(tb)
        mcon.Close()
        Return tb



    End Function

End Class