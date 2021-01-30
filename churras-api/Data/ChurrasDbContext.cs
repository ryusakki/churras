using ChurrasAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChurrasAPI.Data
{
    public class ChurrasDbContext : DbContext
    {
        public ChurrasDbContext(DbContextOptions<ChurrasDbContext> options) : base(options) { }
        public DbSet<ChurrascoAgendado> ChurrascosAgendados { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
           optionsBuilder.UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //dotnet ef migrations add Init
            //dotnet ef database update
            builder.Entity<Usuario>().HasKey(p => p.Email);
            builder.Entity<ChurrascoAgendado>(ca =>
            {
                ca.HasKey(c => new
                {
                    c.Dia,
                    c.Mes,
                    c.Ano
                });

                ca.HasMany(c => c.Participacoes)
                .WithOne(p => p.ChurrascoAgendado)
                .HasForeignKey(p => new
                {
                    p.ChurrascoAgendadoDia,
                    p.ChurrascoAgendadoMes,
                    p.ChurrascoAgendadoAno
                }).IsRequired();
            });

            builder.Entity<Participacao>(p =>
            {
                p.ToTable("Participacoes");
                p.Property(pa => pa.UsuarioEmail).IsRequired();
                p.HasKey(pa => new
                {
                    pa.UsuarioEmail,
                    pa.ChurrascoAgendadoDia,
                    pa.ChurrascoAgendadoMes,
                    pa.ChurrascoAgendadoAno
                });
                p.HasOne(pa => pa.Usuario);
            });
            base.OnModelCreating(builder);
        }
    }
}
