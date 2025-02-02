using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjetoNutri.Models;


namespace ProjetoNutriC_.Context
{
    public class ClienteContext : DbContext
    {
        public ClienteContext(DbContextOptions<ClienteContext> options) : base(options) 
        {
            
        }

        public DbSet<Paciente> Pacientes { get; set; }
    }
}