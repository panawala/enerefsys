using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using EnerefsysDAL.Model;

namespace DataContext
{
    public class EnerefsysContext : DbContext
    {
        public EnerefsysContext()
            : base()
        {
        }

         public EnerefsysContext(string connName)
            : base()
        { }

        public DbSet<DayLoad> DayLoads { get; set; }
        public DbSet<Electronic> Electronics { get; set; }
        public DbSet<EngineFitResult> EngineFitResults { get; set; }
        public DbSet<FitResult> FitResults { get; set; }
        public DbSet<OptimizationResult> OptimizationResults { get; set; }
        public DbSet<PumpFitResult> PumpFitResults { get; set; }
        public DbSet<PumpInfo> PumpInfoes { get; set; }
        public DbSet<StandardLoad> StandardLoads { get; set; }
        public DbSet<RunResult> RunResults { get; set; }
    }
}
