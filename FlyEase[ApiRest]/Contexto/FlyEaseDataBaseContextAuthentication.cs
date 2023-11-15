using FlyEase_ApiRest_.Models;
using FlyEase_ApiRest_.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace FlyEase_ApiRest_.Contexto;

public partial class FlyEaseDataBaseContextAuthentication : DbContext
{
    public FlyEaseDataBaseContextAuthentication()
    {
    }

    public FlyEaseDataBaseContextAuthentication(DbContextOptions<FlyEaseDataBaseContextAuthentication> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administradores { get; set; }

    public virtual DbSet<Aereolinea> Aereolineas { get; set; }

    public virtual DbSet<Aereopuerto> Aereopuertos { get; set; }

    public virtual DbSet<ApiClient> ApiClients { get; set; }

    public virtual DbSet<Asiento> Asientos { get; set; }

    public virtual DbSet<Avion> Aviones { get; set; }

    public virtual DbSet<Boleto> Boletos { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Ciudad> Ciudades { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Coordenada> Coordenadas { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Pais> Paises { get; set; }

    public virtual DbSet<Refreshtoken> Refreshtokens { get; set; }

    public virtual DbSet<Refreshtokenview> Refreshtokenviews { get; set; }

    public virtual DbSet<Region> Regiones { get; set; }

    public virtual DbSet<TiposDeAplicacion> TiposDeAplicaciones { get; set; }

    public virtual DbSet<Vuelo> Vuelos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.Idadministrador).HasName("pk_administradores_idadministrador");

            entity.ToTable("administradores");

            entity.Property(e => e.Idadministrador).HasColumnName("idadministrador");
            entity.Property(e => e.Apellidos)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("apellidos");
            entity.Property(e => e.Celular)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("celular");
            entity.Property(e => e.Clave)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("clave");
            entity.Property(e => e.Correo)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("correo");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Nombres)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("nombres");
            entity.Property(e => e.Numerodocumento)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("numerodocumento");
            entity.Property(e => e.Tipodocumento)
                .IsRequired()
                .HasMaxLength(15)
                .HasColumnName("tipodocumento");
            entity.Property(e => e.Usuario)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("usuario");
        });

        modelBuilder.Entity<Aereolinea>(entity =>
        {
            entity.HasKey(e => e.Idaereolinea).HasName("pk_aereolineas_idaereolinea");

            entity.ToTable("aereolineas");

            entity.HasIndex(e => e.Codigoiata, "uk_aereolineas_codigoiata").IsUnique();

            entity.HasIndex(e => e.Codigoicao, "uk_aereolineas_codigoicao").IsUnique();

            entity.HasIndex(e => e.Nombre, "uk_aereolineas_nombre").IsUnique();

            entity.Property(e => e.Idaereolinea).HasColumnName("idaereolinea");
            entity.Property(e => e.Codigoiata)
                .IsRequired()
                .HasMaxLength(2)
                .HasColumnName("codigoiata");
            entity.Property(e => e.Codigoicao)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("codigoicao");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Aereopuerto>(entity =>
        {
            entity.HasKey(e => e.Idaereopuerto).HasName("pk_aereopuertos_idaereopuerto");

            entity.ToTable("aereopuertos");

            entity.HasIndex(e => e.Idcoordenada, "coordenadas_UK").IsUnique();

            entity.HasIndex(e => e.Idcoordenada, "uk_aereopuertos_idcoordenada").IsUnique();

            entity.HasIndex(e => e.Nombre, "uk_aerepuertos_nombre").IsUnique();

            entity.Property(e => e.Idaereopuerto).HasColumnName("idaereopuerto");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Idciudad).HasColumnName("idciudad");
            entity.Property(e => e.Idcoordenada).HasColumnName("idcoordenada");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombre");

            entity.HasOne(d => d.Ciudad).WithMany(p => p.ListaAereopuertos)
                .HasForeignKey(d => d.Idciudad)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_aereopuertos_idciudad");

            entity.HasOne(d => d.Coordenadas).WithOne(p => p.Aereopuerto)
                .HasForeignKey<Aereopuerto>(d => d.Idcoordenada)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_aereopuertos_idcoordenada");
        });

        modelBuilder.Entity<ApiClient>(entity =>
        {
            entity.HasKey(e => e.Clientid).HasName("API Clients_pkey");

            entity.ToTable("API Clients");

            entity.HasIndex(e => e.Nombre, "API Clients_nombre_key").IsUnique();

            entity.Property(e => e.Clientid)
                .HasMaxLength(50)
                .HasColumnName("clientid");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(now() - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.TipoAplicacionId).HasColumnName("tipo_aplicacion_id");
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .HasColumnName("token");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasColumnName("url");

            entity.HasOne(d => d.TipoAplicacion).WithMany(p => p.ApiClients)
                .HasForeignKey(d => d.TipoAplicacionId)
                .HasConstraintName("API Clients_tipo_aplicacion_id_fkey");
        });

        modelBuilder.Entity<Asiento>(entity =>
        {
            entity.HasKey(e => e.Idasiento).HasName("pk_asientos_idasiento");

            entity.ToTable("asientos");

            entity.HasIndex(e => new { e.Posicion, e.Idavion }, "uk_asientos_posicion_idavion").IsUnique();

            entity.Property(e => e.Idasiento).HasColumnName("idasiento");
            entity.Property(e => e.Disponibilidad).HasColumnName("disponibilidad");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Idavion)
                .HasMaxLength(10)
                .HasColumnName("idavion");
            entity.Property(e => e.Idcategoria).HasColumnName("idcategoria");
            entity.Property(e => e.Posicion).HasColumnName("posicion");

            entity.HasOne(d => d.Avion).WithMany(p => p.Asientos)
                .HasForeignKey(d => d.Idavion)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_asientos_idavion");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Asientos)
                .HasForeignKey(d => d.Idcategoria)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_asientos_idcategoria");
        });

        modelBuilder.Entity<Avion>(entity =>
        {
            entity.HasKey(e => e.Idavion).HasName("pk_aviones_idavion");

            entity.ToTable("aviones");

            entity.Property(e => e.Idavion)
                .HasMaxLength(10)
                .HasColumnName("idavion");
            entity.Property(e => e.Cantidadcarga).HasColumnName("cantidadcarga");
            entity.Property(e => e.Cantidadpasajeros).HasColumnName("cantidadpasajeros");
            entity.Property(e => e.Fabricante)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("fabricante");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Idaereolinea).HasColumnName("idaereolinea");
            entity.Property(e => e.Modelo)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("modelo");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Velocidadpromedio).HasColumnName("velocidadpromedio");

            entity.HasOne(d => d.Aereolinea).WithMany(p => p.Aviones)
                .HasForeignKey(d => d.Idaereolinea)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_aviones_idaereolinea");
        });

        modelBuilder.Entity<Boleto>(entity =>
        {
            entity.HasKey(e => e.Idboleto).HasName("pk_boletos_idboleto");

            entity.ToTable("boletos");

            entity.Property(e => e.Idboleto).HasColumnName("idboleto");
            entity.Property(e => e.Descuento).HasColumnName("descuento");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Idasiento).HasColumnName("idasiento");
            entity.Property(e => e.Idvuelo).HasColumnName("idvuelo");
            entity.Property(e => e.Numerodocumento)
                .HasMaxLength(10)
                .HasColumnName("numerodocumento");
            entity.Property(e => e.Precio).HasColumnName("precio");
            entity.Property(e => e.Preciototal).HasColumnName("preciototal");

            entity.HasOne(d => d.Asiento).WithMany(p => p.Boletos)
                .HasForeignKey(d => d.Idasiento)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_boletos_idasiento");

            entity.HasOne(d => d.Vuelo).WithMany(p => p.Boletos)
                .HasForeignKey(d => d.Idvuelo)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_boletos_idvuelo");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Boletos)
                .HasForeignKey(d => d.Numerodocumento)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_boletos_numerodocumento");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Idcategoria).HasName("pk_categorias_idcategoria");

            entity.ToTable("categorias");

            entity.HasIndex(e => e.Nombre, "uk_categorias_nombre").IsUnique();

            entity.Property(e => e.Idcategoria).HasColumnName("idcategoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estadocategoria).HasColumnName("estadocategoria");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnName("nombre");
            entity.Property(e => e.Tarifa).HasColumnName("tarifa");
            entity.Property(e => e.Comercial)
                .IsRequired()
                .HasColumnName("comercial");
        });

        modelBuilder.Entity<Ciudad>(entity =>
        {
            entity.HasKey(e => e.Idciudad).HasName("pk_ciudades_idciudad");

            entity.ToTable("ciudades");

            entity.HasIndex(e => new { e.Nombre, e.Idregion }, "uk_ciudades_nombre_idregion").IsUnique();

            entity.Property(e => e.Idciudad).HasColumnName("idciudad");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Idregion).HasColumnName("idregion");
            entity.Property(e => e.Imagen).HasColumnName("imagen");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnName("nombre");

            entity.HasOne(d => d.Region).WithMany(p => p.Ciudades)
                .HasForeignKey(d => d.Idregion)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_ciudades_idregion");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Numerodocumento).HasName("pk_clientes_numerodocumento");

            entity.ToTable("clientes");

            entity.Property(e => e.Numerodocumento)
                .HasMaxLength(10)
                .HasColumnName("numerodocumento");
            entity.Property(e => e.Apellidos)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("apellidos");
            entity.Property(e => e.Celular)
                .HasMaxLength(10)
                .HasColumnName("celular");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .HasColumnName("correo");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Nombres)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("nombres");
            entity.Property(e => e.Tipodocumento)
                .IsRequired()
                .HasMaxLength(15)
                .HasColumnName("tipodocumento");
        });

        modelBuilder.Entity<Coordenada>(entity =>
        {
            entity.HasKey(e => e.Idcoordenada).HasName("pk_coordenadas_idcoordenada");

            entity.ToTable("coordenadas");

            entity.HasIndex(e => new { e.Latitud, e.Longitud }, "uk_coordenadas_latitud_longitud").IsUnique();

            entity.Property(e => e.Idcoordenada).HasColumnName("idcoordenada");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Latitud).HasColumnName("latitud");
            entity.Property(e => e.Longitud).HasColumnName("longitud");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.Idestado).HasName("pk_estados_idestado");

            entity.ToTable("estados");

            entity.Property(e => e.Idestado).HasColumnName("idestado");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .HasColumnName("descripcion");
            entity.Property(e => e.Detencion).HasColumnName("detencion");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.HasKey(e => e.Idpais).HasName("pk_paises_idpais");

            entity.ToTable("paises");

            entity.HasIndex(e => e.Nombre, "uk_paises_nombre").IsUnique();

            entity.Property(e => e.Idpais).HasColumnName("idpais");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Refreshtoken>(entity =>
        {
            entity.HasKey(e => e.Idtoken).HasName("refreshtokens_pkey");

            entity.ToTable("refreshtokens");

            entity.Property(e => e.Idtoken).HasColumnName("idtoken");
            entity.Property(e => e.Fechacreacion)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("fechacreacion");
            entity.Property(e => e.Fechaexpiracion)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("fechaexpiracion");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.IdUser).HasColumnName("idadmin");
            entity.Property(e => e.RefreshtokenAtributte)
                .HasMaxLength(500)
                .HasColumnName("refreshtoken");
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .HasColumnName("token");

            entity.HasOne(d => d.Admin).WithMany(p => p.Refreshtokens)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("refreshtokens_idadmin_fkey");
        });

        modelBuilder.Entity<Refreshtokenview>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("refreshtokenview");

            entity.Property(e => e.Esactivo).HasColumnName("esactivo");
            entity.Property(e => e.Fechacreacion)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("fechacreacion");
            entity.Property(e => e.Fechaexpiracion)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("fechaexpiracion");
            entity.Property(e => e.Fecharegistro)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Idadmin).HasColumnName("idadmin");
            entity.Property(e => e.Idtoken).HasColumnName("idtoken");
            entity.Property(e => e.Refreshtoken)
                .HasMaxLength(500)
                .HasColumnName("refreshtoken");
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .HasColumnName("token");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Idregion).HasName("pk_regiones_idregion");

            entity.ToTable("regiones");

            entity.HasIndex(e => new { e.Nombre, e.Idpais }, "uk_regiones_nombre_idpais").IsUnique();

            entity.Property(e => e.Idregion).HasColumnName("idregion");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Idpais).HasColumnName("idpais");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnName("nombre");

            entity.HasOne(d => d.Pais).WithMany(p => p.Regiones)
                .HasForeignKey(d => d.Idpais)
                .HasConstraintName("fk_regiones_idpais");
        });

        modelBuilder.Entity<TiposDeAplicacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Tipos de aplicaciones_pkey");

            entity.ToTable("Tipos de aplicaciones");

            entity.HasIndex(e => e.Nombre, "Tipos de aplicaciones_nombre_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(350)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Vuelo>(entity =>
        {
            entity.HasKey(e => e.Idvuelo).HasName("pk_vuelos_idvuelo");

            entity.ToTable("vuelos");

            entity.Property(e => e.Idvuelo).HasColumnName("idvuelo");
            entity.Property(e => e.Cupo)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("cupo");
            entity.Property(e => e.Descuento).HasColumnName("descuento");
            entity.Property(e => e.Distanciarecorrida).HasColumnName("distanciarecorrida");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Fechayhoradesalida)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP - '05:00:00'::interval)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fechayhoradesalida");
            entity.Property(e => e.Fechayhorallegada)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fechayhorallegada");
            entity.Property(e => e.Idavion)
                .HasMaxLength(10)
                .HasColumnName("idavion");
            entity.Property(e => e.Iddespegue).HasColumnName("iddespegue");
            entity.Property(e => e.Iddestino).HasColumnName("iddestino");
            entity.Property(e => e.Idestado)
                .HasDefaultValueSql("8")
                .HasColumnName("idestado");
            entity.Property(e => e.Preciovuelo).HasColumnName("preciovuelo");
            entity.Property(e => e.Tarifatemporada).HasColumnName("tarifatemporada");

            entity.HasOne(d => d.Avion).WithMany(p => p.Vuelos)
                .HasForeignKey(d => d.Idavion)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_vuelos_idavion");

            entity.HasOne(d => d.Aereopuerto_Despegue).WithMany(p => p.Despegues)
                .HasForeignKey(d => d.Iddespegue)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_vuelos_iddespegue");

            entity.HasOne(d => d.Aereopuerto_Destino).WithMany(p => p.Destinos)
                .HasForeignKey(d => d.Iddestino)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_vuelos_iddestino");

            entity.HasOne(d => d.Estado).WithMany(p => p.Vuelos)
                .HasForeignKey(d => d.Idestado)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_vuelos_idestado");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}