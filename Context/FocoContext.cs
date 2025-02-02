using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjetoNutri.Models;

namespace ProjetoNutri.Context
{
    public class FocoContext : DbContext
    {
        public FocoContext(DbContextOptions<FocoContext> options) : base(options) 
        {
            
        }

        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
    }
}