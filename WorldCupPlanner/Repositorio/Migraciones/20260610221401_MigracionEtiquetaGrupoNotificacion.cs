using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migraciones
{
    /// <inheritdoc />
    public partial class MigracionEtiquetaGrupoNotificacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EtiquetaGrupo",
                table: "Notificaciones",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EtiquetaGrupo",
                table: "Notificaciones");
        }
    }
}
