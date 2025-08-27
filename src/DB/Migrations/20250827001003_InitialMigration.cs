using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estado_Muestra",
                columns: table => new
                {
                    id_estado = table.Column<byte>(type: "tinyint", nullable: false),
                    nombre = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Estado_M__86989FB231F3242F", x => x.id_estado);
                });

            migrationBuilder.CreateTable(
                name: "Rol_Usuario",
                columns: table => new
                {
                    id_rol = table.Column<byte>(type: "tinyint", nullable: false),
                    nombre_rol = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rol_Usua__6ABCB5E0C2370F5E", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Documento",
                columns: table => new
                {
                    id_tipo_doc = table.Column<byte>(type: "tinyint", nullable: false),
                    nombre = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tipo_Doc__B0A524EA001DA409", x => x.id_tipo_doc);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Muestra",
                columns: table => new
                {
                    TPMST_ID = table.Column<byte>(type: "tinyint", nullable: false),
                    nombre = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tipo_Mue__3DB040A8B6354D0C", x => x.TPMST_ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    US_Cedula = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    nombre = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    apellido = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    razon_social = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: true),
                    Direccion = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Correo = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false),
                    Telefono = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    CONTACTO = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: true),
                    Username = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    contrasena = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    id_rol = table.Column<byte>(type: "tinyint", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuario__615FCA4672B18000", x => x.US_Cedula);
                    table.ForeignKey(
                        name: "fk_usuario_rol",
                        column: x => x.id_rol,
                        principalTable: "Rol_Usuario",
                        principalColumn: "id_rol");
                });

            migrationBuilder.CreateTable(
                name: "Prueba",
                columns: table => new
                {
                    id_prueba = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_prueba = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false),
                    tipo_muestra_asociada = table.Column<byte>(type: "tinyint", nullable: false),
                    norma_referencia = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Prueba__328A4573149CBD98", x => x.id_prueba);
                    table.ForeignKey(
                        name: "fk_prueba_tipo",
                        column: x => x.tipo_muestra_asociada,
                        principalTable: "Tipo_Muestra",
                        principalColumn: "TPMST_ID");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(15)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "US_Cedula",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "varchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "US_Cedula",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(15)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "US_Cedula",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(15)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "US_Cedula",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auditoria",
                columns: table => new
                {
                    id_auditoria = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    accion = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false),
                    fecha_accion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    descripcion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Auditori__9644A3CED1A46BA2", x => x.id_auditoria);
                    table.ForeignKey(
                        name: "fk_aud_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "US_Cedula");
                });

            migrationBuilder.CreateTable(
                name: "Muestra",
                columns: table => new
                {
                    MST_CODIGO = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    TPMST_ID = table.Column<byte>(type: "tinyint", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: true),
                    Fecha_recepcion = table.Column<DateTime>(type: "datetime", nullable: false),
                    origen = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Fecha_Salida_Estimada = table.Column<DateTime>(type: "datetime", nullable: true),
                    Condiciones_almacenamiento = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Condiciones_transporte = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    id_usuario_solicitante = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    id_Analista = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    estado_actual = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Muestra__800EDF4D8255C508", x => x.MST_CODIGO);
                    table.ForeignKey(
                        name: "fk_muestra_analista",
                        column: x => x.id_Analista,
                        principalTable: "Usuario",
                        principalColumn: "US_Cedula");
                    table.ForeignKey(
                        name: "fk_muestra_estado",
                        column: x => x.estado_actual,
                        principalTable: "Estado_Muestra",
                        principalColumn: "id_estado");
                    table.ForeignKey(
                        name: "fk_muestra_solic",
                        column: x => x.id_usuario_solicitante,
                        principalTable: "Usuario",
                        principalColumn: "US_Cedula");
                    table.ForeignKey(
                        name: "fk_muestra_tipo",
                        column: x => x.TPMST_ID,
                        principalTable: "Tipo_Muestra",
                        principalColumn: "TPMST_ID");
                });

            migrationBuilder.CreateTable(
                name: "Parametro_Norma",
                columns: table => new
                {
                    id_parametro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_prueba = table.Column<int>(type: "int", nullable: false),
                    nombre_parametro = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false),
                    valor_min = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    valor_max = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    unidad = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Parametr__3D24E3256A0410FE", x => x.id_parametro);
                    table.ForeignKey(
                        name: "fk_parnorma_prueba",
                        column: x => x.id_prueba,
                        principalTable: "Prueba",
                        principalColumn: "id_prueba");
                });

            migrationBuilder.CreateTable(
                name: "Bitacora_Muestra",
                columns: table => new
                {
                    id_bitacora = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_muestra = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    id_analista = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    fecha_asignacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    observaciones = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bitacora__7E4268B0BAD2A82E", x => x.id_bitacora);
                    table.ForeignKey(
                        name: "fk_bit_analista",
                        column: x => x.id_analista,
                        principalTable: "Usuario",
                        principalColumn: "US_Cedula");
                    table.ForeignKey(
                        name: "fk_bit_muestra",
                        column: x => x.id_muestra,
                        principalTable: "Muestra",
                        principalColumn: "MST_CODIGO");
                });

            migrationBuilder.CreateTable(
                name: "Documento",
                columns: table => new
                {
                    id_documento = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_muestra = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    id_tipo_doc = table.Column<byte>(type: "tinyint", nullable: false),
                    version = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ruta_archivo = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DOC_PDF = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Document__5D2EE7E5FCF72C46", x => x.id_documento);
                    table.ForeignKey(
                        name: "fk_doc_muestra",
                        column: x => x.id_muestra,
                        principalTable: "Muestra",
                        principalColumn: "MST_CODIGO");
                    table.ForeignKey(
                        name: "fk_doc_tipodoc",
                        column: x => x.id_tipo_doc,
                        principalTable: "Tipo_Documento",
                        principalColumn: "id_tipo_doc");
                });

            migrationBuilder.CreateTable(
                name: "Historial_Trazabilidad",
                columns: table => new
                {
                    id_historial = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_muestra = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    id_usuario = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    estado = table.Column<byte>(type: "tinyint", nullable: false),
                    fecha_cambio = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    observaciones = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Historia__76E6C5025A7F6AE1", x => x.id_historial);
                    table.ForeignKey(
                        name: "fk_ht_estado",
                        column: x => x.estado,
                        principalTable: "Estado_Muestra",
                        principalColumn: "id_estado");
                    table.ForeignKey(
                        name: "fk_ht_muestra",
                        column: x => x.id_muestra,
                        principalTable: "Muestra",
                        principalColumn: "MST_CODIGO");
                    table.ForeignKey(
                        name: "fk_ht_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "US_Cedula");
                });

            migrationBuilder.CreateTable(
                name: "Notificacion",
                columns: table => new
                {
                    id_notificacion = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_muestra = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    tipo_alerta = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    destinatario = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false),
                    enviado = table.Column<bool>(type: "bit", nullable: false),
                    fecha_envio = table.Column<DateTime>(type: "datetime", nullable: true),
                    detalle = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__8270F9A50C4940E9", x => x.id_notificacion);
                    table.ForeignKey(
                        name: "fk_not_muestra",
                        column: x => x.id_muestra,
                        principalTable: "Muestra",
                        principalColumn: "MST_CODIGO");
                });

            migrationBuilder.CreateTable(
                name: "Resultado_Prueba",
                columns: table => new
                {
                    id_resultado = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_prueba = table.Column<int>(type: "int", nullable: false),
                    id_muestra = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    valor_obtenido = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    unidad = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    cumple_norma = table.Column<bool>(type: "bit", nullable: true),
                    fecha_registro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    validado_por = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Resultad__33A42B30FAD681EE", x => x.id_resultado);
                    table.ForeignKey(
                        name: "fk_res_muestra",
                        column: x => x.id_muestra,
                        principalTable: "Muestra",
                        principalColumn: "MST_CODIGO");
                    table.ForeignKey(
                        name: "fk_res_prueba",
                        column: x => x.id_prueba,
                        principalTable: "Prueba",
                        principalColumn: "id_prueba");
                    table.ForeignKey(
                        name: "fk_res_validador",
                        column: x => x.validado_por,
                        principalTable: "Usuario",
                        principalColumn: "US_Cedula");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_id_usuario",
                table: "Auditoria",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Bitacora_Muestra_id_analista",
                table: "Bitacora_Muestra",
                column: "id_analista");

            migrationBuilder.CreateIndex(
                name: "IX_Bitacora_Muestra_id_muestra",
                table: "Bitacora_Muestra",
                column: "id_muestra");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_id_muestra",
                table: "Documento",
                column: "id_muestra");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_id_tipo_doc",
                table: "Documento",
                column: "id_tipo_doc");

            migrationBuilder.CreateIndex(
                name: "UQ__Estado_M__72AFBCC62E07FBCF",
                table: "Estado_Muestra",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Historial_Trazabilidad_estado",
                table: "Historial_Trazabilidad",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "IX_Historial_Trazabilidad_id_muestra",
                table: "Historial_Trazabilidad",
                column: "id_muestra");

            migrationBuilder.CreateIndex(
                name: "IX_Historial_Trazabilidad_id_usuario",
                table: "Historial_Trazabilidad",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "idx_muestra_fechas",
                table: "Muestra",
                columns: new[] { "Fecha_recepcion", "Fecha_Salida_Estimada" });

            migrationBuilder.CreateIndex(
                name: "IX_Muestra_estado_actual",
                table: "Muestra",
                column: "estado_actual");

            migrationBuilder.CreateIndex(
                name: "IX_Muestra_id_Analista",
                table: "Muestra",
                column: "id_Analista");

            migrationBuilder.CreateIndex(
                name: "IX_Muestra_id_usuario_solicitante",
                table: "Muestra",
                column: "id_usuario_solicitante");

            migrationBuilder.CreateIndex(
                name: "IX_Muestra_TPMST_ID",
                table: "Muestra",
                column: "TPMST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_id_muestra",
                table: "Notificacion",
                column: "id_muestra");

            migrationBuilder.CreateIndex(
                name: "uk_parametro",
                table: "Parametro_Norma",
                columns: new[] { "id_prueba", "nombre_parametro" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prueba_tipo_muestra_asociada",
                table: "Prueba",
                column: "tipo_muestra_asociada");

            migrationBuilder.CreateIndex(
                name: "uk_prueba",
                table: "Prueba",
                columns: new[] { "nombre_prueba", "tipo_muestra_asociada" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_resultado_muestra_fecha",
                table: "Resultado_Prueba",
                columns: new[] { "id_muestra", "fecha_registro" });

            migrationBuilder.CreateIndex(
                name: "IX_Resultado_Prueba_id_muestra",
                table: "Resultado_Prueba",
                column: "id_muestra");

            migrationBuilder.CreateIndex(
                name: "IX_Resultado_Prueba_id_prueba",
                table: "Resultado_Prueba",
                column: "id_prueba");

            migrationBuilder.CreateIndex(
                name: "IX_Resultado_Prueba_validado_por",
                table: "Resultado_Prueba",
                column: "validado_por");

            migrationBuilder.CreateIndex(
                name: "UQ__Rol_Usua__673CB435C9C52528",
                table: "Rol_Usuario",
                column: "nombre_rol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Tipo_Doc__72AFBCC67DC151A6",
                table: "Tipo_Documento",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Tipo_Mue__72AFBCC6EAC23A93",
                table: "Tipo_Muestra",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Usuario",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_id_rol",
                table: "Usuario",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuario__536C85E4F8B48158",
                table: "Usuario",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Usuario",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Auditoria");

            migrationBuilder.DropTable(
                name: "Bitacora_Muestra");

            migrationBuilder.DropTable(
                name: "Documento");

            migrationBuilder.DropTable(
                name: "Historial_Trazabilidad");

            migrationBuilder.DropTable(
                name: "Notificacion");

            migrationBuilder.DropTable(
                name: "Parametro_Norma");

            migrationBuilder.DropTable(
                name: "Resultado_Prueba");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Tipo_Documento");

            migrationBuilder.DropTable(
                name: "Muestra");

            migrationBuilder.DropTable(
                name: "Prueba");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Estado_Muestra");

            migrationBuilder.DropTable(
                name: "Tipo_Muestra");

            migrationBuilder.DropTable(
                name: "Rol_Usuario");
        }
    }
}
