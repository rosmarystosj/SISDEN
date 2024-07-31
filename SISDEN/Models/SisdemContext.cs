using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SISDEN.Models;

public partial class SisdemContext : DbContext
{
    public SisdemContext()
    {
    }

    public SisdemContext(DbContextOptions<SisdemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Articulo> Articulos { get; set; }

    public virtual DbSet<Capitulo> Capitulos { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Denuncium> Denuncia { get; set; }

    public virtual DbSet<Entidadautorizadum> Entidadautorizada { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Evidencium> Evidencia { get; set; }

    public virtual DbSet<Leyviolacion> Leyviolacions { get; set; }

    public virtual DbSet<Motivocierre> Motivocierres { get; set; }

    public virtual DbSet<Opcionpreguntum> Opcionpregunta { get; set; }

    public virtual DbSet<Preguntum> Pregunta { get; set; }

    public virtual DbSet<Puntosart> Puntosarts { get; set; }

    public virtual DbSet<Respuestum> Respuesta { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Seccion> Seccions { get; set; }

    public virtual DbSet<Sesion> Sesions { get; set; }

    public virtual DbSet<Tipoevidencium> Tipoevidencia { get; set; }

    public virtual DbSet<Tipopreguntum> Tipopregunta { get; set; }

    public virtual DbSet<Ubicacion> Ubicacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VistaArticulo> VistaArticulos { get; set; }

    public virtual DbSet<VistaComentario> VistaComentarios { get; set; }

    public virtual DbSet<VistaDenuncia> VistaDenuncias { get; set; }

    public virtual DbSet<VistaEvidencia> VistaEvidencias { get; set; }

    public virtual DbSet<VistaOpcionesPregunta> VistaOpcionesPreguntas { get; set; }

    public virtual DbSet<VistaPregunta> VistaPreguntas { get; set; }

    public virtual DbSet<VistaRespuesta> VistaRespuestas { get; set; }

    public virtual DbSet<VistaUsuario> VistaUsuarios { get; set; }

    public virtual DbSet<VistaViolacione> VistaViolaciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ROSMARYSTOSJ\\SQLEXPRESS;Database=SISDEM;User=RosmaryStos;Password=12345678;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Articulo>(entity =>
        {
            entity.HasKey(e => e.Idarticulo).HasName("PK__ARTICULO__51E79CEF00457179");

            entity.ToTable("ARTICULO");

            entity.Property(e => e.Idarticulo).HasColumnName("IDARTICULO");
            entity.Property(e => e.Artdescripcion)
                .IsUnicode(false)
                .HasColumnName("ARTDESCRIPCION");
            entity.Property(e => e.Artnombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ARTNOMBRE");
            entity.Property(e => e.CapIdcapitulo).HasColumnName("CAP_IDCAPITULO");
        });

        modelBuilder.Entity<Capitulo>(entity =>
        {
            entity.HasKey(e => e.Idcapitulo).HasName("PK__CAPITULO__E247373F8EB78E03");

            entity.ToTable("CAPITULO");

            entity.Property(e => e.Idcapitulo).HasColumnName("IDCAPITULO");
            entity.Property(e => e.Capnombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CAPNOMBRE");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.Idcategoria).HasName("PK__CATEGORI__ADC0E719FFC25D5D");

            entity.ToTable("CATEGORIA");

            entity.Property(e => e.Idcategoria).HasColumnName("IDCATEGORIA");
            entity.Property(e => e.Catgdescripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CATGDESCRIPCION");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.Idcomentario).HasName("PK__COMENTAR__B189A03BB4EBEF47");

            entity.ToTable("COMENTARIOS");

            entity.Property(e => e.Idcomentario).HasColumnName("IDCOMENTARIO");
            entity.Property(e => e.ComIddenuncia).HasColumnName("COM_IDDENUNCIA");
            entity.Property(e => e.ComIdrol).HasColumnName("COM_IDROL");
            entity.Property(e => e.ComIdusuario).HasColumnName("COM_IDUSUARIO");
            entity.Property(e => e.Comdescripcion)
                .HasColumnType("text")
                .HasColumnName("COMDESCRIPCION");
        });

        modelBuilder.Entity<Denuncium>(entity =>
        {
            entity.HasKey(e => e.Iddenuncia).HasName("PK__DENUNCIA__6560087946C99EB1");

            entity.ToTable("DENUNCIA");

            entity.Property(e => e.Iddenuncia).HasColumnName("IDDENUNCIA");
            entity.Property(e => e.DenIdestado).HasColumnName("DEN_IDESTADO");
            entity.Property(e => e.DenIdmotivocierre).HasColumnName("DEN_IDMOTIVOCIERRE");
            entity.Property(e => e.DenIdusuario).HasColumnName("DEN_IDUSUARIO");
            entity.Property(e => e.Denanimal)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DENANIMAL");
            entity.Property(e => e.Dencategoria)
                .IsUnicode(false)
                .HasColumnName("DENCATEGORIA");
            entity.Property(e => e.Dendescripcion)
                .IsUnicode(false)
                .HasColumnName("DENDESCRIPCION");
            entity.Property(e => e.Denevidenciaadjunta).HasColumnName("DENEVIDENCIAADJUNTA");
            entity.Property(e => e.Denfechacierre)
                .HasColumnType("datetime")
                .HasColumnName("DENFECHACIERRE");
            entity.Property(e => e.Denfechacreacion)
                .HasColumnType("datetime")
                .HasColumnName("DENFECHACREACION");
            entity.Property(e => e.Dennumprision).HasColumnName("DENNUMPRISION");
            entity.Property(e => e.Dennumsalarios).HasColumnName("DENNUMSALARIOS");
            entity.Property(e => e.Dennumserv).HasColumnName("DENNUMSERV");
            entity.Property(e => e.Denobservaciones)
                .HasColumnType("text")
                .HasColumnName("DENOBSERVACIONES");
            entity.Property(e => e.Denprision).HasColumnName("DENPRISION");
            entity.Property(e => e.Densalarios).HasColumnName("DENSALARIOS");
            entity.Property(e => e.Denservicio).HasColumnName("DENSERVICIO");
            entity.Property(e => e.Densesion)
                .IsUnicode(false)
                .HasColumnName("DENSESION");
            entity.Property(e => e.Dentitulo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DENTITULO");
            entity.Property(e => e.Denubicacion)
                .IsUnicode(false)
                .HasColumnName("DENUBICACION");
        });

        modelBuilder.Entity<Entidadautorizadum>(entity =>
        {
            entity.HasKey(e => e.Identidadaut).HasName("PK__ENTIDADA__8DDD21D008665DD7");

            entity.ToTable("ENTIDADAUTORIZADA");

            entity.Property(e => e.Identidadaut).HasColumnName("IDENTIDADAUT");
            entity.Property(e => e.EntCorreo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ENT_CORREO");
            entity.Property(e => e.EntautorizadaEstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ENTAUTORIZADA_ESTATUS");
            entity.Property(e => e.Entautorizadadescp)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ENTAUTORIZADADESCP");
            entity.Property(e => e.EntidadSector)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ENTIDAD_SECTOR");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.Idestado).HasName("PK__ESTADO__A93E12E2874D6F13");

            entity.ToTable("ESTADO");

            entity.Property(e => e.Idestado).HasColumnName("IDESTADO");
            entity.Property(e => e.Estdescripcion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ESTDESCRIPCION");
        });

        modelBuilder.Entity<Evidencium>(entity =>
        {
            entity.HasKey(e => e.Idevidencia).HasName("PK__EVIDENCI__281166CE6EF4DFA7");

            entity.ToTable("EVIDENCIA");

            entity.Property(e => e.Idevidencia).HasColumnName("IDEVIDENCIA");
            entity.Property(e => e.EvIddenuncia).HasColumnName("EV_IDDENUNCIA");
            entity.Property(e => e.EvIdtipoevid).HasColumnName("EV_IDTIPOEVID");
            entity.Property(e => e.Evurl)
                .IsUnicode(false)
                .HasColumnName("EVURL");
        });

        modelBuilder.Entity<Leyviolacion>(entity =>
        {
            entity.HasKey(e => e.Idviolacion).HasName("PK__LEYVIOLA__FF7F417AF18F831C");

            entity.ToTable("LEYVIOLACION");

            entity.Property(e => e.Idviolacion).HasColumnName("IDVIOLACION");
            entity.Property(e => e.ViolIdarticulo).HasColumnName("VIOL_IDARTICULO");
            entity.Property(e => e.ViolIddenuncia).HasColumnName("VIOL_IDDENUNCIA");
            entity.Property(e => e.ViolIdpuntoart).HasColumnName("VIOL_IDPUNTOART");
        });

        modelBuilder.Entity<Motivocierre>(entity =>
        {
            entity.HasKey(e => e.Idmotivo).HasName("PK__MOTIVOCI__BE3091F4F4EC55C2");

            entity.ToTable("MOTIVOCIERRE");

            entity.Property(e => e.Idmotivo).HasColumnName("IDMOTIVO");
            entity.Property(e => e.Motivodescripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("MOTIVODESCRIPCION");
        });

        modelBuilder.Entity<Opcionpreguntum>(entity =>
        {
            entity.HasKey(e => e.Idopcionpreg).HasName("PK__OPCIONPR__F9FF149BA0E3AE08");

            entity.ToTable("OPCIONPREGUNTA");

            entity.Property(e => e.Idopcionpreg).HasColumnName("IDOPCIONPREG");
            entity.Property(e => e.OpcionpregIdarticulo).HasColumnName("OPCIONPREG_IDARTICULO");
            entity.Property(e => e.OpcionpregIdpregunta).HasColumnName("OPCIONPREG_IDPREGUNTA");
            entity.Property(e => e.OpcionpregIdpuntoart).HasColumnName("OPCIONPREG_IDPUNTOART");
            entity.Property(e => e.Opcionpregdescripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("OPCIONPREGDESCRIPCION");
        });

        modelBuilder.Entity<Preguntum>(entity =>
        {
            entity.HasKey(e => e.Idpregunta).HasName("PK__PREGUNTA__177765F7963217DD");

            entity.ToTable("PREGUNTA");

            entity.Property(e => e.Idpregunta).HasColumnName("IDPREGUNTA");
            entity.Property(e => e.PreIdcategoria).HasColumnName("PRE_IDCATEGORIA");
            entity.Property(e => e.PregTipo).HasColumnName("PREG_TIPO");
            entity.Property(e => e.Pregdescripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PREGDESCRIPCION");
        });

        modelBuilder.Entity<Puntosart>(entity =>
        {
            entity.HasKey(e => e.Idpuntoart).HasName("PK__PUNTOSAR__412B9EA475C35667");

            entity.ToTable("PUNTOSART");

            entity.Property(e => e.Idpuntoart).HasColumnName("IDPUNTOART");
            entity.Property(e => e.Idarticulo).HasColumnName("IDARTICULO");
            entity.Property(e => e.Puntoartdescripcion)
                .IsUnicode(false)
                .HasColumnName("PUNTOARTDESCRIPCION");
            entity.Property(e => e.Puntoartnumero)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PUNTOARTNUMERO");
        });

        modelBuilder.Entity<Respuestum>(entity =>
        {
            entity.HasKey(e => e.Idrespuesta).HasName("PK__RESPUEST__0B6869FD2473316C");

            entity.ToTable("RESPUESTA");

            entity.Property(e => e.Idrespuesta).HasColumnName("IDRESPUESTA");
            entity.Property(e => e.RespIdopcion).HasColumnName("RESP_IDOPCION");
            entity.Property(e => e.RespIdpregunta).HasColumnName("RESP_IDPREGUNTA");
            entity.Property(e => e.RespIdusuario).HasColumnName("RESP_IDUSUARIO");
            entity.Property(e => e.Respdescripcion)
                .HasColumnType("text")
                .HasColumnName("RESPDESCRIPCION");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Idrol).HasName("PK__ROLES__A686519E3F548FC9");

            entity.ToTable("ROLES");

            entity.Property(e => e.Idrol).HasColumnName("IDROL");
            entity.Property(e => e.Rolnombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROLNOMBRE");
        });

        modelBuilder.Entity<Seccion>(entity =>
        {
            entity.HasKey(e => e.Idseccion).HasName("PK__SECCION__4956D4230A2A9E52");

            entity.ToTable("SECCION");

            entity.Property(e => e.Idseccion).HasColumnName("IDSECCION");
            entity.Property(e => e.CapIdcapitulo).HasColumnName("CAP_IDCAPITULO");
            entity.Property(e => e.Secnombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("SECNOMBRE");
        });

        modelBuilder.Entity<Sesion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SESION__3214EC2752D6A698");

            entity.ToTable("SESION");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Createat)
                .HasColumnType("datetime")
                .HasColumnName("CREATEAT");
            entity.Property(e => e.Expiresat)
                .HasColumnType("datetime")
                .HasColumnName("EXPIRESAT");
            entity.Property(e => e.Sesionid)
                .IsUnicode(false)
                .HasColumnName("SESIONID");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERID");
        });

        modelBuilder.Entity<Tipoevidencium>(entity =>
        {
            entity.HasKey(e => e.Idtipoevid).HasName("PK__TIPOEVID__789FB5A13AA461DC");

            entity.ToTable("TIPOEVIDENCIA");

            entity.Property(e => e.Idtipoevid).HasColumnName("IDTIPOEVID");
            entity.Property(e => e.Tipodescripcion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TIPODESCRIPCION");
        });

        modelBuilder.Entity<Tipopreguntum>(entity =>
        {
            entity.HasKey(e => e.Idtipo).HasName("PK__TIPOPREG__E57FEC10EF926F7A");

            entity.ToTable("TIPOPREGUNTA");

            entity.Property(e => e.Idtipo).HasColumnName("IDTIPO");
            entity.Property(e => e.Tipodescripcion)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("TIPODESCRIPCION");
        });

        modelBuilder.Entity<Ubicacion>(entity =>
        {
            entity.HasKey(e => e.Idubicacion).HasName("PK__UBICACIO__781B28C996FB4984");

            entity.ToTable("UBICACION");

            entity.Property(e => e.Idubicacion).HasColumnName("IDUBICACION");
            entity.Property(e => e.Ubdescripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UBDESCRIPCION");
            entity.Property(e => e.UbiIdentidadaut).HasColumnName("UBI_IDENTIDADAUT");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Idusuario).HasName("PK__USUARIO__98242AA94AA6BF45");

            entity.ToTable("USUARIO");

            entity.Property(e => e.Idusuario).HasColumnName("IDUSUARIO");
            entity.Property(e => e.Usuapellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USUAPELLIDO");
            entity.Property(e => e.Usucontraseña)
                .IsUnicode(false)
                .HasColumnName("USUCONTRASEÑA");
            entity.Property(e => e.Usuemail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USUEMAIL");
            entity.Property(e => e.Usuentidad).HasColumnName("USUENTIDAD");
            entity.Property(e => e.Usuidentificacion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USUIDENTIFICACION");
            entity.Property(e => e.Usunombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USUNOMBRE");
            entity.Property(e => e.Usurol).HasColumnName("USUROL");
            entity.Property(e => e.Usustatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USUSTATUS");
            entity.Property(e => e.Usutelefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USUTELEFONO");
            entity.Property(e => e.Usutelefono2)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USUTELEFONO2");
            entity.Property(e => e.Usuverificacion)
                .IsUnicode(false)
                .HasColumnName("USUVERIFICACION");
            entity.Property(e => e.VerificationExpiry).HasColumnType("datetime");
        });

        modelBuilder.Entity<VistaArticulo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaArticulos");

            entity.Property(e => e.Artdescripcion)
                .IsUnicode(false)
                .HasColumnName("ARTDESCRIPCION");
            entity.Property(e => e.Artnombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ARTNOMBRE");
            entity.Property(e => e.CapituloNombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CAPITULO_NOMBRE");
            entity.Property(e => e.Idarticulo).HasColumnName("IDARTICULO");
            entity.Property(e => e.Puntoartdescripcion)
                .IsUnicode(false)
                .HasColumnName("PUNTOARTDESCRIPCION");
            entity.Property(e => e.Puntoartnumero)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PUNTOARTNUMERO");
            entity.Property(e => e.SeccionNombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("SECCION_NOMBRE");
        });

        modelBuilder.Entity<VistaComentario>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaComentarios");

            entity.Property(e => e.Comdescripcion)
                .HasColumnType("text")
                .HasColumnName("COMDESCRIPCION");
            entity.Property(e => e.DenunciaTitulo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Idcomentario).HasColumnName("IDCOMENTARIO");
            entity.Property(e => e.Iddenuncia).HasColumnName("IDDENUNCIA");
            entity.Property(e => e.Rolnombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROLNOMBRE");
            entity.Property(e => e.UsuarioApellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioNombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VistaDenuncia>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaDenuncias");

            entity.Property(e => e.Denanimal)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DENANIMAL");
            entity.Property(e => e.Dencategoria)
                .IsUnicode(false)
                .HasColumnName("DENCATEGORIA");
            entity.Property(e => e.Dendescripcion)
                .IsUnicode(false)
                .HasColumnName("DENDESCRIPCION");
            entity.Property(e => e.Denevidenciaadjunta).HasColumnName("DENEVIDENCIAADJUNTA");
            entity.Property(e => e.Denfechacierre)
                .HasColumnType("datetime")
                .HasColumnName("DENFECHACIERRE");
            entity.Property(e => e.Denfechacreacion)
                .HasColumnType("datetime")
                .HasColumnName("DENFECHACREACION");
            entity.Property(e => e.Dennumprision).HasColumnName("DENNUMPRISION");
            entity.Property(e => e.Dennumsalarios).HasColumnName("DENNUMSALARIOS");
            entity.Property(e => e.Dennumserv).HasColumnName("DENNUMSERV");
            entity.Property(e => e.Denobservaciones)
                .HasColumnType("text")
                .HasColumnName("DENOBSERVACIONES");
            entity.Property(e => e.Denprision).HasColumnName("DENPRISION");
            entity.Property(e => e.Densalarios).HasColumnName("DENSALARIOS");
            entity.Property(e => e.Denservicio).HasColumnName("DENSERVICIO");
            entity.Property(e => e.Densesion)
                .IsUnicode(false)
                .HasColumnName("DENSESION");
            entity.Property(e => e.Dentitulo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DENTITULO");
            entity.Property(e => e.Denubicacion)
                .IsUnicode(false)
                .HasColumnName("DENUBICACION");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Iddenuncia).HasColumnName("IDDENUNCIA");
            entity.Property(e => e.MotivoCierre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioEmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioNombreCompleto)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioTelefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VistaEvidencia>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaEvidencias");

            entity.Property(e => e.DenunciaTitulo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Evurl)
                .IsUnicode(false)
                .HasColumnName("EVURL");
            entity.Property(e => e.Iddenuncia).HasColumnName("IDDENUNCIA");
            entity.Property(e => e.Idevidencia).HasColumnName("IDEVIDENCIA");
            entity.Property(e => e.TipoEvidencia)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VistaOpcionesPregunta>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaOpcionesPreguntas");

            entity.Property(e => e.Artdescripcion)
                .IsUnicode(false)
                .HasColumnName("ARTDESCRIPCION");
            entity.Property(e => e.Artnombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ARTNOMBRE");
            entity.Property(e => e.Idopcionpreg).HasColumnName("IDOPCIONPREG");
            entity.Property(e => e.NumeroArticulo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Opcionpregdescripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("OPCIONPREGDESCRIPCION");
            entity.Property(e => e.PreguntaDescripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PuntoArticuloDescripcion).IsUnicode(false);
        });

        modelBuilder.Entity<VistaPregunta>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaPreguntas");

            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Idpregunta).HasColumnName("IDPREGUNTA");
            entity.Property(e => e.Pregdescripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PREGDESCRIPCION");
            entity.Property(e => e.PreguntaTipo)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VistaRespuesta>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaRespuestas");

            entity.Property(e => e.Idrespuesta).HasColumnName("IDRESPUESTA");
            entity.Property(e => e.OpcionDescripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PreguntaDescripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Respdescripcion)
                .HasColumnType("text")
                .HasColumnName("RESPDESCRIPCION");
            entity.Property(e => e.UsuarioApellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioNombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VistaUsuario>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaUsuarios");

            entity.Property(e => e.EntCorreo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ENT_CORREO");
            entity.Property(e => e.Entautorizadadescp)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ENTAUTORIZADADESCP");
            entity.Property(e => e.Idusuario).HasColumnName("IDUSUARIO");
            entity.Property(e => e.Usuapellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USUAPELLIDO");
            entity.Property(e => e.Usucontraseña)
                .IsUnicode(false)
                .HasColumnName("USUCONTRASEÑA");
            entity.Property(e => e.Usuemail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USUEMAIL");
            entity.Property(e => e.Usuidentificacion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USUIDENTIFICACION");
            entity.Property(e => e.Usunombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USUNOMBRE");
            entity.Property(e => e.Usurol).HasColumnName("USUROL");
            entity.Property(e => e.Usustatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USUSTATUS");
            entity.Property(e => e.Usutelefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USUTELEFONO");
            entity.Property(e => e.Usutelefono2)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USUTELEFONO2");
        });

        modelBuilder.Entity<VistaViolacione>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaViolaciones");

            entity.Property(e => e.Artdescripcion)
                .IsUnicode(false)
                .HasColumnName("ARTDESCRIPCION");
            entity.Property(e => e.Artnombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ARTNOMBRE");
            entity.Property(e => e.Dentitulo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DENTITULO");
            entity.Property(e => e.Iddenuncia).HasColumnName("IDDENUNCIA");
            entity.Property(e => e.Idviolacion).HasColumnName("IDVIOLACION");
            entity.Property(e => e.Puntoartdescripcion)
                .IsUnicode(false)
                .HasColumnName("PUNTOARTDESCRIPCION");
            entity.Property(e => e.Puntoartnumero)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PUNTOARTNUMERO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
