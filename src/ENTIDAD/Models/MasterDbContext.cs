using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Models;

public partial class MasterDbContext : DbContext
{
    public MasterDbContext()
    {
    }

    public MasterDbContext(DbContextOptions<MasterDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditorium> Auditoria { get; set; }

    public virtual DbSet<BitacoraMuestra> BitacoraMuestras { get; set; }

    public virtual DbSet<Documento> Documentos { get; set; }

    public virtual DbSet<EstadoMuestra> EstadoMuestras { get; set; }

    public virtual DbSet<HistorialTrazabilidad> HistorialTrazabilidads { get; set; }

    public virtual DbSet<Muestra> Muestras { get; set; }

    public virtual DbSet<Notificacion> Notificacions { get; set; }

    public virtual DbSet<ParametroNorma> ParametroNormas { get; set; }

    public virtual DbSet<Prueba> Pruebas { get; set; }

    public virtual DbSet<ResultadoPrueba> ResultadoPruebas { get; set; }

    public virtual DbSet<RolUsuario> RolUsuarios { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    public virtual DbSet<TipoMuestra> TipoMuestras { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=master;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__Auditori__9644A3CED1A46BA2");

            entity.HasIndex(e => e.IdUsuario, "IX_Auditoria_id_usuario");

            entity.Property(e => e.IdAuditoria).HasColumnName("id_auditoria");
            entity.Property(e => e.Accion)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("accion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaAccion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_accion");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("id_usuario");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_aud_usuario");
        });

        modelBuilder.Entity<BitacoraMuestra>(entity =>
        {
            entity.HasKey(e => e.IdBitacora).HasName("PK__Bitacora__7E4268B0BAD2A82E");

            entity.ToTable("Bitacora_Muestra");

            entity.HasIndex(e => e.IdAnalista, "IX_Bitacora_Muestra_id_analista");

            entity.HasIndex(e => e.IdMuestra, "IX_Bitacora_Muestra_id_muestra");

            entity.Property(e => e.IdBitacora).HasColumnName("id_bitacora");
            entity.Property(e => e.FechaAsignacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_asignacion");
            entity.Property(e => e.IdAnalista)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("id_analista");
            entity.Property(e => e.IdMuestra)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("id_muestra");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("observaciones");

            entity.HasOne(d => d.IdAnalistaNavigation).WithMany(p => p.BitacoraMuestras)
                .HasForeignKey(d => d.IdAnalista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bit_analista");

            entity.HasOne(d => d.IdMuestraNavigation).WithMany(p => p.BitacoraMuestras)
                .HasForeignKey(d => d.IdMuestra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bit_muestra");
        });

        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasKey(e => e.IdDocumento).HasName("PK__Document__5D2EE7E5FCF72C46");

            entity.ToTable("Documento");

            entity.HasIndex(e => e.IdMuestra, "IX_Documento_id_muestra");

            entity.Property(e => e.IdDocumento).HasColumnName("id_documento");
            entity.Property(e => e.DocPdf).HasColumnName("DOC_PDF");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdMuestra)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("id_muestra");
            entity.Property(e => e.IdTipoDoc).HasColumnName("id_tipo_doc");
            entity.Property(e => e.RutaArchivo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ruta_archivo");
            entity.Property(e => e.Version)
                .HasDefaultValue(1)
                .HasColumnName("version");

            entity.HasOne(d => d.IdMuestraNavigation).WithMany(p => p.Documentos)
                .HasForeignKey(d => d.IdMuestra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doc_muestra");

            entity.HasOne(d => d.IdTipoDocNavigation).WithMany(p => p.Documentos)
                .HasForeignKey(d => d.IdTipoDoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doc_tipodoc");
        });

        modelBuilder.Entity<EstadoMuestra>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado_M__86989FB231F3242F");

            entity.ToTable("Estado_Muestra");

            entity.HasIndex(e => e.Nombre, "UQ__Estado_M__72AFBCC62E07FBCF").IsUnique();

            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<HistorialTrazabilidad>(entity =>
        {
            entity.HasKey(e => e.IdHistorial).HasName("PK__Historia__76E6C5025A7F6AE1");

            entity.ToTable("Historial_Trazabilidad");

            entity.HasIndex(e => e.IdMuestra, "IX_Historial_Trazabilidad_id_muestra");

            entity.HasIndex(e => e.IdUsuario, "IX_Historial_Trazabilidad_id_usuario");

            entity.Property(e => e.IdHistorial).HasColumnName("id_historial");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaCambio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_cambio");
            entity.Property(e => e.IdMuestra)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("id_muestra");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("id_usuario");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("observaciones");

            entity.HasOne(d => d.EstadoNavigation).WithMany(p => p.HistorialTrazabilidads)
                .HasForeignKey(d => d.Estado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ht_estado");

            entity.HasOne(d => d.IdMuestraNavigation).WithMany(p => p.HistorialTrazabilidads)
                .HasForeignKey(d => d.IdMuestra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ht_muestra");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HistorialTrazabilidads)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ht_usuario");
        });

        modelBuilder.Entity<Muestra>(entity =>
        {
            entity.HasKey(e => e.MstCodigo).HasName("PK__Muestra__800EDF4D8255C508");

            entity.ToTable("Muestra");

            entity.HasIndex(e => new { e.FechaRecepcion, e.FechaSalidaEstimada }, "idx_muestra_fechas");

            entity.Property(e => e.MstCodigo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("MST_CODIGO");
            entity.Property(e => e.CondicionesAlmacenamiento)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Condiciones_almacenamiento");
            entity.Property(e => e.CondicionesTransporte)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Condiciones_transporte");
            entity.Property(e => e.EstadoActual).HasColumnName("estado_actual");
            entity.Property(e => e.FechaRecepcion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_recepcion");
            entity.Property(e => e.FechaSalidaEstimada)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Salida_Estimada");
            entity.Property(e => e.IdAnalista)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("id_Analista");
            entity.Property(e => e.IdUsuarioSolicitante)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("id_usuario_solicitante");
            entity.Property(e => e.Nombre)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Origen)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("origen");
            entity.Property(e => e.TpmstId).HasColumnName("TPMST_ID");

            entity.HasOne(d => d.EstadoActualNavigation).WithMany(p => p.Muestras)
                .HasForeignKey(d => d.EstadoActual)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_muestra_estado");

            entity.HasOne(d => d.IdAnalistaNavigation).WithMany(p => p.MuestraIdAnalistaNavigations)
                .HasForeignKey(d => d.IdAnalista)
                .HasConstraintName("fk_muestra_analista");

            entity.HasOne(d => d.IdUsuarioSolicitanteNavigation).WithMany(p => p.MuestraIdUsuarioSolicitanteNavigations)
                .HasForeignKey(d => d.IdUsuarioSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_muestra_solic");

            entity.HasOne(d => d.Tpmst).WithMany(p => p.Muestras)
                .HasForeignKey(d => d.TpmstId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_muestra_tipo");
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.HasKey(e => e.IdNotificacion).HasName("PK__Notifica__8270F9A50C4940E9");

            entity.ToTable("Notificacion");

            entity.HasIndex(e => e.IdMuestra, "IX_Notificacion_id_muestra");

            entity.Property(e => e.IdNotificacion).HasColumnName("id_notificacion");
            entity.Property(e => e.Destinatario)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("destinatario");
            entity.Property(e => e.Detalle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("detalle");
            entity.Property(e => e.Enviado).HasColumnName("enviado");
            entity.Property(e => e.FechaEnvio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_envio");
            entity.Property(e => e.IdMuestra)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("id_muestra");
            entity.Property(e => e.TipoAlerta)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("tipo_alerta");

            entity.HasOne(d => d.IdMuestraNavigation).WithMany(p => p.Notificacions)
                .HasForeignKey(d => d.IdMuestra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_not_muestra");
        });

        modelBuilder.Entity<ParametroNorma>(entity =>
        {
            entity.HasKey(e => e.IdParametro).HasName("PK__Parametr__3D24E3256A0410FE");

            entity.ToTable("Parametro_Norma");

            entity.HasIndex(e => new { e.IdPrueba, e.NombreParametro }, "uk_parametro").IsUnique();

            entity.Property(e => e.IdParametro).HasColumnName("id_parametro");
            entity.Property(e => e.IdPrueba).HasColumnName("id_prueba");
            entity.Property(e => e.NombreParametro)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("nombre_parametro");
            entity.Property(e => e.Unidad)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("unidad");
            entity.Property(e => e.ValorMax)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("valor_max");
            entity.Property(e => e.ValorMin)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("valor_min");

            entity.HasOne(d => d.IdPruebaNavigation).WithMany(p => p.ParametroNormas)
                .HasForeignKey(d => d.IdPrueba)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_parnorma_prueba");
        });

        modelBuilder.Entity<Prueba>(entity =>
        {
            entity.HasKey(e => e.IdPrueba).HasName("PK__Prueba__328A4573149CBD98");

            entity.ToTable("Prueba");

            entity.HasIndex(e => new { e.NombrePrueba, e.TipoMuestraAsociada }, "uk_prueba").IsUnique();

            entity.Property(e => e.IdPrueba).HasColumnName("id_prueba");
            entity.Property(e => e.NombrePrueba)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("nombre_prueba");
            entity.Property(e => e.NormaReferencia)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("norma_referencia");
            entity.Property(e => e.TipoMuestraAsociada).HasColumnName("tipo_muestra_asociada");

            entity.HasOne(d => d.TipoMuestraAsociadaNavigation).WithMany(p => p.Pruebas)
                .HasForeignKey(d => d.TipoMuestraAsociada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_prueba_tipo");
        });

        modelBuilder.Entity<ResultadoPrueba>(entity =>
        {
            entity.HasKey(e => e.IdResultado).HasName("PK__Resultad__33A42B30FAD681EE");

            entity.ToTable("Resultado_Prueba");

            entity.HasIndex(e => e.IdMuestra, "IX_Resultado_Prueba_id_muestra");

            entity.HasIndex(e => e.IdPrueba, "IX_Resultado_Prueba_id_prueba");

            entity.HasIndex(e => new { e.IdMuestra, e.FechaRegistro }, "idx_resultado_muestra_fecha");

            entity.Property(e => e.IdResultado).HasColumnName("id_resultado");
            entity.Property(e => e.CumpleNorma).HasColumnName("cumple_norma");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.IdMuestra)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("id_muestra");
            entity.Property(e => e.IdPrueba).HasColumnName("id_prueba");
            entity.Property(e => e.Unidad)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("unidad");
            entity.Property(e => e.ValidadoPor)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("validado_por");
            entity.Property(e => e.ValorObtenido)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("valor_obtenido");

            entity.HasOne(d => d.IdMuestraNavigation).WithMany(p => p.ResultadoPruebas)
                .HasForeignKey(d => d.IdMuestra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_res_muestra");

            entity.HasOne(d => d.IdPruebaNavigation).WithMany(p => p.ResultadoPruebas)
                .HasForeignKey(d => d.IdPrueba)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_res_prueba");

            entity.HasOne(d => d.ValidadoPorNavigation).WithMany(p => p.ResultadoPruebas)
                .HasForeignKey(d => d.ValidadoPor)
                .HasConstraintName("fk_res_validador");
        });

        modelBuilder.Entity<RolUsuario>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol_Usua__6ABCB5E0C2370F5E");

            entity.ToTable("Rol_Usuario");

            entity.HasIndex(e => e.NombreRol, "UQ__Rol_Usua__673CB435C9C52528").IsUnique();

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDoc).HasName("PK__Tipo_Doc__B0A524EA001DA409");

            entity.ToTable("Tipo_Documento");

            entity.HasIndex(e => e.Nombre, "UQ__Tipo_Doc__72AFBCC67DC151A6").IsUnique();

            entity.Property(e => e.IdTipoDoc).HasColumnName("id_tipo_doc");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<TipoMuestra>(entity =>
        {
            entity.HasKey(e => e.TpmstId).HasName("PK__Tipo_Mue__3DB040A8B6354D0C");

            entity.ToTable("Tipo_Muestra");

            entity.HasIndex(e => e.Nombre, "UQ__Tipo_Mue__72AFBCC6EAC23A93").IsUnique();

            entity.Property(e => e.TpmstId).HasColumnName("TPMST_ID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsCedula).HasName("PK__Usuario__615FCA4672B18000");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Username, "UQ__Usuario__536C85E4F8B48158").IsUnique();

            entity.Property(e => e.UsCedula)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("US_Cedula");
            entity.Property(e => e.Apellido)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Contacto)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("CONTACTO");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("razon_social");
            entity.Property(e => e.Telefono)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
