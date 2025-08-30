using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdentityFromEstadoDocumento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove identity from Estado_Documento.id_estado_documento
            migrationBuilder.Sql(@"
                -- Step 1: Create a temporary table with the same structure but without identity
                CREATE TABLE [Estado_Documento_Temp] (
                    [id_estado_documento] int NOT NULL,
                    [nombre] varchar(30) NOT NULL,
                    CONSTRAINT [PK_Estado_Documento_Temp] PRIMARY KEY ([id_estado_documento])
                );

                -- Step 2: Copy data from original table to temp table
                INSERT INTO [Estado_Documento_Temp] ([id_estado_documento], [nombre])
                SELECT [id_estado_documento], [nombre] FROM [Estado_Documento];

                -- Step 3: Drop foreign key constraints that reference Estado_Documento
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'fk_doc_estadodoc')
                    ALTER TABLE [Documento] DROP CONSTRAINT [fk_doc_estadodoc];

                -- Step 4: Drop the original table
                DROP TABLE [Estado_Documento];

                -- Step 5: Rename the temp table to the original name
                EXEC sp_rename 'Estado_Documento_Temp', 'Estado_Documento';

                -- Step 6: Recreate the primary key constraint with the original name
                ALTER TABLE [Estado_Documento] DROP CONSTRAINT [PK_Estado_Documento_Temp];
                ALTER TABLE [Estado_Documento] ADD CONSTRAINT [PK__Estado_D__5D9B2B2D1CBBE2F3] PRIMARY KEY ([id_estado_documento]);

                -- Step 7: Recreate the unique constraint
                ALTER TABLE [Estado_Documento] ADD CONSTRAINT [UQ__Estado_D__72AFBCC6E3D8D2B1] UNIQUE ([nombre]);

                -- Step 8: Recreate foreign key constraints
                IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Documento')
                    ALTER TABLE [Documento] ADD CONSTRAINT [fk_doc_estadodoc] FOREIGN KEY ([id_estado_documento]) REFERENCES [Estado_Documento] ([id_estado_documento]);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Restore identity to Estado_Documento.id_estado_documento
            migrationBuilder.Sql(@"
                -- Step 1: Create a temporary table with identity
                CREATE TABLE [Estado_Documento_Temp] (
                    [id_estado_documento] int IDENTITY(1,1) NOT NULL,
                    [nombre] varchar(30) NOT NULL,
                    CONSTRAINT [PK_Estado_Documento_Temp] PRIMARY KEY ([id_estado_documento])
                );

                -- Step 2: Copy data from original table to temp table (identity will be auto-assigned)
                SET IDENTITY_INSERT [Estado_Documento_Temp] ON;
                INSERT INTO [Estado_Documento_Temp] ([id_estado_documento], [nombre])
                SELECT [id_estado_documento], [nombre] FROM [Estado_Documento];
                SET IDENTITY_INSERT [Estado_Documento_Temp] OFF;

                -- Step 3: Drop foreign key constraints that reference Estado_Documento
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'fk_doc_estadodoc')
                    ALTER TABLE [Documento] DROP CONSTRAINT [fk_doc_estadodoc];

                -- Step 4: Drop the original table
                DROP TABLE [Estado_Documento];

                -- Step 5: Rename the temp table to the original name
                EXEC sp_rename 'Estado_Documento_Temp', 'Estado_Documento';

                -- Step 6: Recreate the primary key constraint with the original name
                ALTER TABLE [Estado_Documento] DROP CONSTRAINT [PK_Estado_Documento_Temp];
                ALTER TABLE [Estado_Documento] ADD CONSTRAINT [PK__Estado_D__5D9B2B2D1CBBE2F3] PRIMARY KEY ([id_estado_documento]);

                -- Step 7: Recreate the unique constraint
                ALTER TABLE [Estado_Documento] ADD CONSTRAINT [UQ__Estado_D__72AFBCC6E3D8D2B1] UNIQUE ([nombre]);

                -- Step 8: Recreate foreign key constraints
                IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Documento')
                    ALTER TABLE [Documento] ADD CONSTRAINT [fk_doc_estadodoc] FOREIGN KEY ([id_estado_documento]) REFERENCES [Estado_Documento] ([id_estado_documento]);
            ");
        }
    }
}
